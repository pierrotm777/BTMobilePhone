namespace Brecham.Obex.Net.Forms
{
    partial class TcpipAddressDialogCfImpl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
                this.buttonOk = new System.Windows.Forms.Button();
                this.buttonCancel = new System.Windows.Forms.Button();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 20);
            this.label1.Text = "IP &Address or Hostname:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(4, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(233, 21);
            this.textBox1.TabIndex = 1;
            // 
            // buttonOk
            // 
                this.buttonOk.Location = new System.Drawing.Point(87, 55);
                this.buttonOk.Name = "buttonOk";
                this.buttonOk.Size = new System.Drawing.Size(72, 20);
                this.buttonOk.TabIndex = 2;
                this.buttonOk.Text = "&OK";
                this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
                this.buttonCancel.Location = new System.Drawing.Point(165, 55);
                this.buttonCancel.Name = "buttonCancel";
                this.buttonCancel.Size = new System.Drawing.Size(72, 20);
                this.buttonCancel.TabIndex = 3;
                this.buttonCancel.Text = "&Cancel";
                this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "OK";
            this.menuItem1.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Cancel";
            this.menuItem2.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // TcpipAddressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.buttonCancel); 
            this.Controls.Add(this.buttonOk); 
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "TcpipAddressDialogCf";
            this.Text = "TCP/IP Address";
            this.Load += new System.EventHandler(this.TcpipAddressDialog_Load);
            this.ResumeLayout(false);

        }

        //private void _hacked_InitializeComponent()
        //{
        //    this.mainMenu1 = new System.Windows.Forms.MainMenu();
        //    this.label1 = new System.Windows.Forms.Label();
        //    this.textBox1 = new System.Windows.Forms.TextBox();
        //    if (!IsSmartphonePlatform()) {
        //        this.buttonOk = new System.Windows.Forms.Button();
        //        this.buttonCancel = new System.Windows.Forms.Button();
        //    }
        //    this.menuItem1 = new System.Windows.Forms.MenuItem();
        //    this.menuItem2 = new System.Windows.Forms.MenuItem();
        //    this.SuspendLayout();
        //    // 
        //    // mainMenu1
        //    // 
        //    this.mainMenu1.MenuItems.Add(this.menuItem1);
        //    this.mainMenu1.MenuItems.Add(this.menuItem2);
        //    // 
        //    // label1
        //    // 
        //    this.label1.Location = new System.Drawing.Point(4, 4);
        //    this.label1.Name = "label1";
        //    this.label1.Size = new System.Drawing.Size(233, 20);
        //    this.label1.Text = "IP &Address or Hostname:";
        //    // 
        //    // textBox1
        //    // 
        //    this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        //                | System.Windows.Forms.AnchorStyles.Right)));
        //    this.textBox1.Location = new System.Drawing.Point(4, 28);
        //    this.textBox1.Name = "textBox1";
        //    this.textBox1.Size = new System.Drawing.Size(233, 21);
        //    this.textBox1.TabIndex = 1;
        //    // 
        //    // buttonOk
        //    // 
        //    if (buttonOk != null) {
        //        this.buttonOk.Location = new System.Drawing.Point(87, 55);
        //        this.buttonOk.Name = "buttonOk";
        //        this.buttonOk.Size = new System.Drawing.Size(72, 20);
        //        this.buttonOk.TabIndex = 2;
        //        this.buttonOk.Text = "&OK";
        //        this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
        //    }
        //    // 
        //    // buttonCancel
        //    // 
        //    if (buttonCancel != null) {
        //        this.buttonCancel.Location = new System.Drawing.Point(165, 55);
        //        this.buttonCancel.Name = "buttonCancel";
        //        this.buttonCancel.Size = new System.Drawing.Size(72, 20);
        //        this.buttonCancel.TabIndex = 3;
        //        this.buttonCancel.Text = "&Cancel";
        //        this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        //    }
        //    // 
        //    // menuItem1
        //    // 
        //    this.menuItem1.Text = "OK";
        //    this.menuItem1.Click += new System.EventHandler(this.buttonOk_Click);
        //    // 
        //    // menuItem2
        //    // 
        //    this.menuItem2.Text = "Cancel";
        //    this.menuItem2.Click += new System.EventHandler(this.buttonCancel_Click);
        //    // 
        //    // TcpipAddressDialog
        //    // 
        //    this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        //    this.AutoScroll = true;
        //    this.ClientSize = new System.Drawing.Size(240, 268);
        //    if (this.buttonCancel != null) { this.Controls.Add(this.buttonCancel); }
        //    if (this.buttonOk != null) { this.Controls.Add(this.buttonOk); }
        //    this.Controls.Add(this.textBox1);
        //    this.Controls.Add(this.label1);
        //    this.Menu = this.mainMenu1;
        //    this.Name = "TcpipAddressDialog";
        //    this.Text = "TCP/IP Address";
        //    this.Load += new System.EventHandler(this.TcpipAddressDialog_Load);
        //    this.ResumeLayout(false);

        //}
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
    }
}