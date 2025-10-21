namespace Async
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
      _btnStart = new Button();
      _btnTry = new Button();
      SuspendLayout();
      // 
      // _btnStart
      // 
      _btnStart.Location = new Point(12, 12);
      _btnStart.Name = "_btnStart";
      _btnStart.Size = new Size(75, 23);
      _btnStart.TabIndex = 0;
      _btnStart.Text = "button1";
      _btnStart.UseVisualStyleBackColor = true;
      // 
      // _btnTry
      // 
      _btnTry.Location = new Point(12, 41);
      _btnTry.Name = "_btnTry";
      _btnTry.Size = new Size(75, 23);
      _btnTry.TabIndex = 1;
      _btnTry.Text = "button1";
      _btnTry.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(567, 319);
      Controls.Add(_btnTry);
      Controls.Add(_btnStart);
      Name = "Form1";
      Text = "Form1";
      ResumeLayout(false);
    }

    #endregion

    private Button _btnStart;
    private Button _btnTry;
  }
}
