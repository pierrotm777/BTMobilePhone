namespace ForCf
{
    partial class Form1
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
            System.Windows.Forms.MenuItem menuItemQuit;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemStart = new System.Windows.Forms.MenuItem();
            this.menuItemStop = new System.Windows.Forms.MenuItem();
            this.menuItemBeamReceive = new System.Windows.Forms.MenuItem();
            this.menuItemBeamReceiveStatus = new System.Windows.Forms.MenuItem();
            this.menuItemBeamReceiveStart = new System.Windows.Forms.MenuItem();
            this.menuItemBeamReceiveStop = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemClearLog = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            menuItemQuit = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // menuItemQuit
            // 
            menuItemQuit.Text = "Quit";
            menuItemQuit.Click += new System.EventHandler(this.menuItemQuit_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "&Go serve!";
            this.menuItem1.Click += new System.EventHandler(this.menuItemStart_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.MenuItems.Add(this.menuItemStart);
            this.menuItem2.MenuItems.Add(this.menuItemStop);
            this.menuItem2.MenuItems.Add(this.menuItemBeamReceive);
            this.menuItem2.MenuItems.Add(menuItemQuit);
            this.menuItem2.MenuItems.Add(this.menuItem4);
            this.menuItem2.MenuItems.Add(this.menuItemClearLog);
            this.menuItem2.Text = "Menu";
            // 
            // menuItemStart
            // 
            this.menuItemStart.Text = "Start";
            this.menuItemStart.Click += new System.EventHandler(this.menuItemStart_Click);
            // 
            // menuItemStop
            // 
            this.menuItemStop.Text = "Stop";
            this.menuItemStop.Click += new System.EventHandler(this.menuItemStop_Click);
            // 
            // menuItemBeamReceive
            // 
            this.menuItemBeamReceive.MenuItems.Add(this.menuItemBeamReceiveStatus);
            this.menuItemBeamReceive.MenuItems.Add(this.menuItemBeamReceiveStart);
            this.menuItemBeamReceive.MenuItems.Add(this.menuItemBeamReceiveStop);
            this.menuItemBeamReceive.Text = "Beam Receive";
            this.menuItemBeamReceive.Popup += new System.EventHandler(this.menuItemBeamReceiveMenu_PopupSubMenu);
            // 
            // menuItemBeamReceiveStatus
            // 
            this.menuItemBeamReceiveStatus.Text = "Status";
            this.menuItemBeamReceiveStatus.Popup += new System.EventHandler(this.menuItemBeamReceiveMenu_PopupItem);
            this.menuItemBeamReceiveStatus.Click += new System.EventHandler(this.menuItemMenuBeamReceive_Click);
            // 
            // menuItemBeamReceiveStart
            // 
            this.menuItemBeamReceiveStart.Text = "Start";
            this.menuItemBeamReceiveStart.Click += new System.EventHandler(this.menuItemBeamReceiveStart_Click);
            // 
            // menuItemBeamReceiveStop
            // 
            this.menuItemBeamReceiveStop.Text = "Stop";
            this.menuItemBeamReceiveStop.Click += new System.EventHandler(this.menuItemBeamReceiveStop_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Text = "-";
            // 
            // menuItemClearLog
            // 
            this.menuItemClearLog.Text = "Clear log";
            this.menuItemClearLog.Click += new System.EventHandler(this.menuItemClearLog_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 20);
            this.label1.Text = "status";
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 20);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(240, 248);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "textBox1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "ObexServer ForCf";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItemStart;
        private System.Windows.Forms.MenuItem menuItemStop;
        private System.Windows.Forms.MenuItem menuItemBeamReceive;
        private System.Windows.Forms.MenuItem menuItemBeamReceiveStart;
        private System.Windows.Forms.MenuItem menuItemBeamReceiveStop;
        private System.Windows.Forms.MenuItem menuItemBeamReceiveStatus;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItemClearLog;
    }
}

