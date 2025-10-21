using System;
using static System.Diagnostics.Trace;
namespace Async
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
      Text = "Async";
      _btnStart.Text = "Start";
      _btnTry.Text = "Try";
      _btnStart.Click += _btnStart_Click;
      _btnTry.Click += _btnTry_Click;
    }

    async private void _btnStart_Click(object? sender, EventArgs e)
    {
      _btnStart.BackColor = Color.Gray;
      WriteLine($"_btnStart_Click:Pre-await() Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");

      await Task.Delay(4000); // Main Form continues the event loop
      // continue execution here after Delay task is complete
      WriteLine($"_btnStart_Click:Post-await() Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");

      _btnStart.BackColor = Color.Orange; // So we know we made it here

      // The run task need not be async, it will be ( probably ) a long blocking operation
      await Task.Run(() => AnotherDelay()); // invoke async lambda method
      await Task.Run(AnotherDelay);
      // continue execution here after AnotherDelay() has completed
      WriteLine($"_btnStart_Click:Post-await-await() Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");

      //finally
      _btnStart.BackColor = Color.Pink;
    }

    private void _btnTry_Click(object? sender, EventArgs e)
    {
      _btnTry.BackColor = Color.Violet;
      Foo();
      WriteLine($"btnTry_Click:UI Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
    }
    private void AnotherDelay()
    {
      Task.Delay(2000).Wait(); // Same as Sleep(2000) a blocking call
    }
    async Task<string> GreetAfter(string who, int when)
    {
      await Task.Delay(when);
      WriteLine($"GreetAfter:Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
      WriteLine($"Hello, {who}");
      Alert(who); // This is a UI update, but let the function handle it.
      return Newtonsoft.Json.JsonConvert.SerializeObject(new Person { First = who, Last = "Doe", Delay = when });
    }
    async void Foo()
    {
      var random = new Random();
      // Make Tasks for each name,
      // Select will return a Task from the GreetAfter call for each,
      // these are then saved in tasks
      var names = new[] { "John", "Jill", "Jane", "Jake" };
      var tasks = names.Select(x => GreetAfter(x, random.Next(1, 5) * 1000));
      WriteLine($"Tasks created : {System.Threading.Thread.CurrentThread.ManagedThreadId}");
      Task.Delay(2000).Wait(); // Wait for 2 seconds
      WriteLine($"Task.WhenAll() starts tasks : {System.Threading.Thread.CurrentThread.ManagedThreadId}");
      var results = await Task.WhenAll(tasks);
      foreach (string item in results)
      {
        WriteLine(Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(item));
      }
    }


    /// <summary>
    /// Alert is a UI update, so it must be done on the UI thread,
    ///  following the MS Best Practice of using Invoke, check if InvokeRequired, and if
    ///  it is, Invoke the method with the same parameters.
    ///  Kinda looks recursive, but it is not.!
    /// </summary>
    /// <param name="message">titlebar message</param>
    private void Alert(string message)
    {
      // MS Best Practice: Use Invoke if required, do not rely on user
      if (InvokeRequired)
      {
        WriteLine($"Alert:InvokeRequired Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        Invoke(new Action<string>(Alert), message);
        // or Invoke(new Action(() => Alert(message)));
        return;
      }
      // Update of UI is safe here  
      Text = $"Alert: {message}";
    }
  }
  public class Person
  {
    public string First { get; set; }
    public string Last { get; set; }
    public int Delay { get; set; }
    override public string ToString()
    {
      return $"{First} {Last} {Delay}";
    }
  }
}
