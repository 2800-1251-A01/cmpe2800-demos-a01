namespace DataGridView.Basic
{
  partial class Form1
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      _btnPopulate = new Button();
      _dgv = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)_dgv).BeginInit();
      SuspendLayout();
      // 
      // _btnPopulate
      // 
      _btnPopulate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      _btnPopulate.Location = new Point(14, 0);
      _btnPopulate.Name = "_btnPopulate";
      _btnPopulate.Size = new Size(774, 23);
      _btnPopulate.TabIndex = 0;
      _btnPopulate.Text = "button1";
      _btnPopulate.UseVisualStyleBackColor = true;
      // 
      // _dgv
      // 
      _dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      _dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      _dgv.Location = new Point(12, 29);
      _dgv.Name = "_dgv";
      _dgv.Size = new Size(776, 409);
      _dgv.TabIndex = 1;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(_dgv);
      Controls.Add(_btnPopulate);
      Name = "Form1";
      Text = "Form1";
      ((System.ComponentModel.ISupportInitialize)_dgv).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private Button _btnPopulate;
    private System.Windows.Forms.DataGridView _dgv;
  }
}
