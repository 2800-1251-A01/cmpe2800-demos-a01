using System;
using System.Collections.Specialized;
using System.Text;
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
      WriteLine($"_btnStart_Click:Pre-await() Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId} {Environment.TickCount}");

      await Task.Delay(4000); // Main Form continues the event loop

      // continue execution here after Delay task is complete
      WriteLine($"_btnStart_Click:Post-await() Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId} {Environment.TickCount}");

      _btnStart.BackColor = Color.Orange; // So we know we made it here

      // The run task need not be async, it will be ( probably ) a long blocking operation
      //   Check the AnotherDelay() output, clearly these .Run() calls spawn threads for AnotherDelay calls
      // Direct call using Action<> deduction, no args required.
      await Task.Run(BasicDelay);
      WriteLine($"_btnStart_Click:BasicDelay Done {Environment.TickCount}");

      // Non-action function required!, thats OK, just use a lambda, and variable capture
      int delay = 1500;
      await Task.Run(() => AnotherDelay(delay)); // invoke async lambda method
      WriteLine($"_btnStart_Click:AnotherDelay Done {Environment.TickCount}");

      // Using a async function that returns a Task can be awaited, this is what
      //  most XXXAsync functions work like
      string gotIt = await GetWordAsync("Whirrlygig");
      WriteLine($"_btnStart_Click:GetWordAsync returned {gotIt} {Environment.TickCount}");

      // continue execution here after AnotherDelay() has completed
      WriteLine($"_btnStart_Click:Post-await-await() Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId} {Environment.TickCount}");
      _btnStart.BackColor = Color.Pink;
    }

    // NOT async, but will call async function Foo();
    // NOTE : await within Foo() will pause and this handler is paused, execution resumes in the UI message pump
    //        until Foo() await completes, and it continues execution
    private void _btnTry_Click(object? sender, EventArgs e)
    {
      // async function Foo(), it has local awaits, which will pause
      //  AND then returns execution to the caller
      Foo(); 
      // Foo() -> when an await is encountered, execution returns to the CALLER - Right here,
      //  You will see the button color and output complete BEFORE Foo() completes !!
      //  Yet Foo() will complete, after this handler has completed !
      //  LESSON : Don't call async functions that you require completion on

      // NOTE : await Foo(); is illegal because, while it IS async, it returns void, not a Task<> so it can't be awaited.
      _btnTry.BackColor = Color.Violet;
      WriteLine($"btnTry_Click:Foo() complete");
    }
    
    // Simple Action compliant function, no args, not async - could be some long running transform algoritm
    private void BasicDelay()
    {
      Task.Delay(2000).Wait(); // Same as Sleep(2000) a blocking call
      WriteLine($"BasicDelay() Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId} {Environment.TickCount}");
    }
    
    // NOT Action compliant, requires an argument to start some long running process, also NOT async ( see StartButton handler )
    private void AnotherDelay(int delay = 2000)
    {
      Task.Delay(delay).Wait(); // Same as Sleep(2000) a blocking call
      WriteLine($"AnotherDelay() Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId} {Environment.TickCount}");
    }
    
    // Simulated NNNAsync IO method, faked
    Task<string> GetWordAsync(string source)
    {
      // This really does nothing, other than returning the string,
      //   but shows an alternate to an async function with an await, since this
      //   does not await, it is not async - but since it returns a Task, it can be awaited..
      return Task.Run(() => { WriteLine("**"); return source; });

      // THE previous return overrides these following examples

      // while this technically meets the requirements of returning a Task, and it can be
      //   awaited in the calling code, it NEVER STARTED.. and when awaited WILL NEVER COMPLETE !
      return new Task<string>(() => { WriteLine("**"); return source; });

      // Solved : You could make the task, start it, and return the task - at least it is started
      //   Since Run() is a new Task that is started, it is the same thing.
      var retTask = new Task<string>(() => { WriteLine("**"); return source; });
      retTask.Start();
      return retTask;
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
      WriteLine($"{tasks.Count()} Tasks created : Thread : {System.Threading.Thread.CurrentThread.ManagedThreadId}");
      WriteLine($"Task.WhenAll() about to start : Thread :  {System.Threading.Thread.CurrentThread.ManagedThreadId}");
      var results = await Task.WhenAll(tasks);
      foreach (string item in results)
      {
        WriteLine(Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(item));
      }
      WriteLine($"Foo(): done");
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
