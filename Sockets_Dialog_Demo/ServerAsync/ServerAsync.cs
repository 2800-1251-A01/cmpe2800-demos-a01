
using static System.Diagnostics.Trace;

//sockets support
using System.Net;
using System.Net.Sockets;

namespace ServerAsync
{
  public partial class ServerAsync : Form
  {
    const string _msgPrefix = "Server:";

    Socket? _listener;
    Socket? _client;
    public ServerAsync()
    {
      InitializeComponent();
      BackColor = Color.Aqua;
      Text = "Server";
      Shown += (s, e) => { Top = 30; Left = Screen.PrimaryScreen!.Bounds.Width - Width; }; // Set to (almost) top of screen.
      Msgs("ServerAsync");
      KeyDown += Form1_KeyDown;
    }

    async private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
      // E for Create socket as Listener, Bind and Listen
      if (e.KeyCode == Keys.Enter)
      {
        // ?? Do you want to do this if the _client is currently connected ?
        if (_client != null)
        {
          Msgs("Client is connected, cannot listen, disconnect first");
          return;
        }

        try
        {
          // ?? Re-entrant or not ?
          // _listener?.Shutdown(SocketShutdown.Both); // DONT SHUTDOWN, Listeners!
          _listener?.Close();
        }
        catch { }
        try
        {
          // Establish Socket, same as client
          _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
          // Bind will attempt to put a listening socket on the system, 
          //  may or may not have privilege to do this
          //  here we will listen for ANYone, but can be a single or filtered address too
          //  Port must known/shared with client
          _listener.Bind(new IPEndPoint(IPAddress.Any, 1666)); // Any incoming address for port 1666

          // Start formal listening, with a backlog Q of 5, allowing for 5 connections
          //  to be in the processing of connecting, without losing them.
          // 0 means infinite backlog, but may be limited by system
          _listener.Listen(0);
          Msgs("Listening...");
        }
        catch (SocketException exc)
        {
          Msgs($"Listener:SocketException : {exc.Message}");
          return;
        }
        catch (Exception exc)
        {
          Msgs($"Listener:Exception : {exc.Message}");
          return;
        }

        try
        {
          // Allow a full connection to be formed ? Yes, - Accept() the connection..

          // NOT required, but you might have more than client..
          Socket client = await _listener.AcceptAsync();

          // Add to client list, or just use it - here overwrite member _client
          _client = client;
          _client.NoDelay = true; // Turn off Nagel efficiency algorithm..
          Msgs("Accept Complete, _client socket is good.");

        }
        catch (SocketException exc)
        {
          _client = null;
          Msgs($"Accept:SocketException : {exc.Message}");
          return;
        }
        catch (Exception exc)
        {
          _client = null;
          Msgs($"Accept:Exception : {exc.Message}");
          return;
        }
      }
      // E for Create socket as Listener, Bind and Listen
      if (e.KeyCode == Keys.Delete && _client != null)
      {
        // This path does NOT await, Shutdown and Close are synchronous but non-blocking
        _client?.Shutdown(SocketShutdown.Both);
        _client?.Close();
        _client = null;
        Msgs("_client shutdown/close/null");
        // ?? Do you want to start a new Listener? And/or Accept ?
      }
      // E for Create socket as Listener, Bind and Listen
      if (e.KeyCode == Keys.Add && _client != null)
      {
        try
        {
          // Attempt to send something
          Random r = new();
          byte[] buff = new byte[2] { (byte)r.Next(256), (byte)r.Next(256) };
          //int iNumBytesSent = await _client.SendAsync(new ArraySegment<byte>(buff), SocketFlags.None); // Actual
          int iNumBytesSent = await _client.SendAsync(buff, SocketFlags.None); // Implicit conversion to ArraySegment
          Msgs($"Sent {iNumBytesSent} bytes {buff[0]:0b},{buff[1]:0b}");
        }
        catch (SocketException exc)
        {
          _client = null;
          Msgs($"Send:SocketException : {exc.Message}");
          return;
        }
        catch (Exception exc)
        {
          _client = null;
          Msgs("Send:Exception : " + exc.Message);
          return;
        }
        Msgs("Send Complete");
      }

    }
    /// <summary>
    /// UI helper, pretty lame, but extensible
    /// </summary>
    /// <param name="s">message to add to TitleBar</param>
    private void Msgs(string s)
    {
      if (InvokeRequired)
      {
        Invoke(new Action(() => Msgs(s)));
        return;
      }
      Text = s;
      WriteLine($"{_msgPrefix}{s}");
    }
  }
}
