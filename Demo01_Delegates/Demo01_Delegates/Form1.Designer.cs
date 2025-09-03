namespace Demo01_Delegates
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
      _btnSpawn = new Button();
      _lbStatus = new ListBox();
      SuspendLayout();
      // 
      // _btnSpawn
      // 
      _btnSpawn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      _btnSpawn.Location = new Point(12, 12);
      _btnSpawn.Name = "_btnSpawn";
      _btnSpawn.Size = new Size(560, 35);
      _btnSpawn.TabIndex = 0;
      _btnSpawn.Text = "button1";
      _btnSpawn.UseVisualStyleBackColor = true;
      // 
      // _lbStatus
      // 
      _lbStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      _lbStatus.FormattingEnabled = true;
      _lbStatus.Location = new Point(12, 53);
      _lbStatus.Name = "_lbStatus";
      _lbStatus.Size = new Size(557, 394);
      _lbStatus.TabIndex = 1;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(12F, 30F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(581, 459);
      Controls.Add(_lbStatus);
      Controls.Add(_btnSpawn);
      Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      Margin = new Padding(5, 6, 5, 6);
      Name = "Form1";
      Text = "Form1";
      ResumeLayout(false);
    }

    #endregion

    private Button _btnSpawn;
        private ListBox _lbStatus;
    }
}
