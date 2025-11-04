using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogDemo
{
  public partial class DialogDemoForm : Form
  {
    System.Windows.Forms.Timer _timer = new();
    public DialogDemoForm()
    {
      InitializeComponent();
      Text = "Dialog";
      _timer.Interval = 6000;
      _timer.Enabled = true;
      _timer.Tick += (s, e) => { DialogResult = DialogResult.OK; }; // Just a quickie to close the Dialog
    }
  }
}
