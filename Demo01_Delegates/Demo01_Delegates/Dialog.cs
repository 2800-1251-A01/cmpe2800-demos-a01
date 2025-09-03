// ica00 Delegate Demo
// Herb V
// date: 2024-06-20
// Demo01_Delegates - a delegate example project, with documentation examples.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo01_Delegates
{
  public partial class Dialog : Form
  {
    // delegate type, not a delegate yet, specifies signature
    public delegate void _delVoidString(string msg);

    // actual delegate, allowing public access.. not great
    //public _delVoidString? MyDelegateX = null; // not used, for example only
    public Action<string>? MyDelegate = null; // Action<> is a built-in generic delegate type

    // Like a auto-property,
    // MyEvent is public with a private "invisible" backing delegate
    // accessed with add/remove handlers with built-in lock()s**
    //public event _delVoidString MyEventX; // again for example only
    public event Action<string>? MyEvent; // Action<> is a built-in generic delegate type

    // demo workings
    static Random _rnd = new(); // example of targeted object new()
    System.Windows.Forms.Timer _tim = new();

    public Dialog(string name, Action<string>? setDelegate, Action<string>? setEvent)
    {
      Text = name;
      // assign callback of public delegate
      MyDelegate = setDelegate;
      // delegates are multicast and can invoke multiple objects ( += )
      // combine with existing for multi-callback, YUP do this twice
      MyDelegate += setDelegate;
      //MyDelegate.

      // assign callback of event - this only works as local member
      // any "outside" access on public event is limited to +=, -=
      MyEvent = setEvent;
      // events are multicast as well and can invoke multiple objects ( += )
      // combine with existing for multi-callback, YUP this happens twice
      MyEvent += setEvent;
      //MyEvent.

      _tim.Interval = _rnd.Next(3, 7) * 1000;
      _tim.Tick += _tim_Tick; // Tick is event of type EventArgs
      _tim.Enabled = true;

    }

    // This callback is of delegate type EventHandler => void( object, EventArgs )
    private void _tim_Tick(object? sender, EventArgs e)
    {
      // change color randomly so we can see something happened
      BackColor = RandomColor();

      // Fire both delegate and event
      MyDelegate?.Invoke(Text + ":" + BackColor.ToString()); // Best practice Invoke with Elvis
      MyEvent?.Invoke(Text + ":" + BackColor.ToString());

      // Or old style
      //if (MyEvent != null)
      //  MyEvent(Text); // MyEvent.Invoke(Text); // equivalent

      // Or thread safe old style
      //_delVoidString tmpEvent = MyEvent;
      //if (tmpEvent != null)
      //  tmpEvent(Text); // tmpEvent.Invoke( Text );

      // NOTE : this is delegate Invoke not Form.Invoke() - careful
    }
    // Select a random enumerated color
    private Color RandomColor()
    {
      Array values = Enum.GetValues(typeof(KnownColor));
      KnownColor randomColor = (KnownColor)values.GetValue(_rnd.Next(values.Length))!;
      return Color.FromKnownColor(randomColor);
    }
  }
}
