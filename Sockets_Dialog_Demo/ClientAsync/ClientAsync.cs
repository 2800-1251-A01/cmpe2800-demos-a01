
using static System.Diagnostics.Trace;

//sockets support
using System.Net;
using System.Net.Sockets;

// Our Dialog Project - Remember to add a ProjectReference -> Project -> DialogDemo
using DialogDemo;

// This Socket Demo will exercise a proper connection, followed by
//  the client ONLY setting up an Rx Thread, not sending any messages
namespace ClientAsync
{
  public partial class ClientAsync : Form
  {
    const string _msgPrefix = "Client:";
    Socket? _client = null;
    public ClientAsync()
    {
      InitializeComponent();
      BackColor = Color.Beige;
      Text = "Client";
      Shown += (s, e) => { Top = 300; Left = Screen.PrimaryScreen!.Bounds.Width - Width; }; // Set to (almost) top of screen.
      Msgs("ClientAsync");
      KeyDown += Form1_KeyDown;
    }

    async private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
      // D for Dialog Test
      if (e.KeyCode == Keys.D)
      {
        // Sample Dialog from library use
        DialogDemo.DialogDemoForm dlg = new();
        if (dlg.ShowDialog() == DialogResult.OK)
          WriteLine("OK");
      }
      // Enter for Socket creation and Connection attempt
      if (e.KeyCode == Keys.Enter)
      {
        // This keydown handler will NOT complete until the Socket has been
        //  connected and the RxThread has been started, and finally the connection
        //  has been closed.
        try
        {
          // re-entrant ? blow away the old one ?
          // Depends on Spec
          // Try if not null.

          // Soft-Disco to let other side know we're done.
          _client?.Shutdown(SocketShutdown.Both);
          _client?.Close();

          // New socket, basic protocol defined
          _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

          // have a socket attempt a connection
          string addr = "localhost";
          int port = 1666;

          // await the connection attempt
          await _client.ConnectAsync(addr, port);
          // if you make it here, connection was successful, otherwise an exception should
          //  have been thrown.
          Msgs("Connection Successful");

          RxTask(_client);
          // This may seem weird - this method will "return" and keep processing
          //   once the RxTask() hits the await.. 
          // You will see the log message below and everything proceeds.
          //   When the RxTask() exits out with the SocketException, that branch
          //   of processing just terminates - the following code and completion
          ///  of this invoke of the key handler has already been completed.
          Msgs("RxTask : Initiated...");
        }
        catch (SocketException exc)
        {
          // reset on error
          _client = null;
          Msgs($"Connect:SocketException:{exc.Message}");
        }
        catch (Exception exc)
        {
          _client = null;
          Msgs($"Connect:Exception:{exc.Message}");
        }
      }
    }
    /// <summary>
    /// UI Helper method, thread-safe to localize UI updates,
    ///  would be good to pass an ENUM in with application state : eConnected, eConnectFailed...etc
    ///  or other additional flags/params to maximize utility.
    /// </summary>
    /// <param name="s">message to display</param>
    private void Msgs(string s)
    {
      if (InvokeRequired)
      {
        WriteLine("InvokeRequired.. you shouldn't see this..");
        Invoke(new Action(() => Msgs(s)));
        WriteLine(s);
        return;
      }
      Text = s;
      WriteLine($"{_msgPrefix}{s}");
    }
    async private void RxTask(Socket? client)
    {
      if (client == null) return; // no socket, no work

      // forever, until error/disco, recv into buffer.
      byte[] bytebuff = new byte[100]; // our actual buffer

      // map the ArraySegment to the buffer, all of it, for use in ReceiveAsync.
      //ArraySegment<byte> buffSegment = new ArraySegment<byte>(bytebuff);

      int iNumBytes = 0;
      while (true)// until, disconnect or error ( kinda the same.. )
      {
        // Target your try/catch to the smallest possible block, on functions that 
        //  can throw exceptions that you can't control.
        try
        {
          iNumBytes = await client.ReceiveAsync(bytebuff, SocketFlags.None);
          //iNumBytes = await client.ReceiveAsync(buffSegment, SocketFlags.None);
        }
        catch (SocketException exc)
        {
          Msgs("RxThread:SocketException : " + exc.Message);
          return;
        }
        catch (Exception exc)
        {
          Msgs("RxThread:GenericException : " + exc.Message);
          return;
        }
        // Got here ? it was successful, BUT it may be 0 bytes indicating
        // A Soft Disconnect from the OTHER side - if we disconnect, we'll get an exception.
        if (iNumBytes == 0) // Soft Disco ? What now ?
        {
          Msgs("Soft Disco Encountered");
          client = null; // ? maybe ? depends on spec
          return; // Why stay ? Socket is disconnected
        }
        // HERE ? Do something with the data...
        Msgs($"RxThread Got {iNumBytes} :  {bytebuff[0]:0b}, {bytebuff[1]:0b}");

        // Rinse and Repeat while() we are not disconnected.
      }
    }
  }
}
