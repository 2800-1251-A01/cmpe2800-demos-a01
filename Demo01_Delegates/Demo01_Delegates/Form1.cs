// ica00 Delegate Demo
// Herb V
// date: 2024-06-20
// Demo01_Delegates - a delegate example project, with documentation examples.

using System;
using static System.Threading.Thread;

namespace Demo01_Delegates
{
  public partial class Form1 : Form
  {
    #region UI
    public Form1()
    {
      InitializeComponent();
      KeyPreview = true; // allow form to capture key presses before they go to any control

      // Assign new Font to the button, using Target-typed new
      _btnSpawn.Font = new("Comic Sans MS", 16f);

      _btnSpawn.Text = "Spawn Dialog";
      Shown += Form1_Shown; // bind the Shown event to complete startup tasks ( ie. CDrawer positioning )
      _btnSpawn.Click += _btnSpawn_Click; // bind click event to handler
    }

    private void _btnSpawn_Click(object? sender, EventArgs e)
    {
      // new Dialog - provide delegate and event callbacks as args
      Dialog d = new Dialog(DateTime.Now.Second.ToString(), DelegateCallback, EventCallback);

      // Since delegate is public, can be directly modified
      //d.MyDelegate = null; // public access, can destroy - YIKES

      d.MyEvent += EventCallback; // uses built in add/remove +=, -= only
      d.MyEvent -= EventCallback; // remove last callback matching - not all

      // Place it beside the main form
      d.StartPosition = FormStartPosition.Manual;
      d.Location = new Point(Location.X + Width + 10, Location.Y);
      // All wired up Show it
      d.Show();
    }
    // Shown event, finishing up after constructor and Load event
    private void Form1_Shown(object? sender, EventArgs e)
    {
      _lbStatus.Items.Insert(0, "Form1 shown event fired."); // Insert at top
    }
    #endregion
    #region Delegates
    /// <summary>
    /// Dialog callback, update status listbox
    /// </summary>
    /// <param name="msg">Message to add to listbox</param>
    private void DelegateCallback(string msg)
    {
      _lbStatus.Items.Insert(0, $"delegate : {msg}");
    }
    /// <summary>
    /// Dialog callback, update status listbox
    /// </summary>
    /// <param name="msg">Message to add to listbox</param>
    private void EventCallback(string msg)
    {
      _lbStatus.Items.Insert(0, $"event : {msg}");
    }
    #endregion
  }
}
