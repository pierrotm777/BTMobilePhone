namespace Speechpad
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.newMenuItem = new System.Windows.Forms.MenuItem();
            this.openMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.cascadeMenuItem = new System.Windows.Forms.MenuItem();
            this.tileHorizontalMenuItem = new System.Windows.Forms.MenuItem();
            this.tileVerticalMenuItem = new System.Windows.Forms.MenuItem();
            this.selectVoiceMenuItem = new System.Windows.Forms.MenuItem();
            this.speechMenuItem = new System.Windows.Forms.MenuItem();
            this.readSelectedTextMenuItem = new System.Windows.Forms.MenuItem();
            this.readDocumentMenuItem = new System.Windows.Forms.MenuItem();
            this.speechRecognitionMenuItem = new System.Windows.Forms.MenuItem();
            this.takeDictationMenuItem = new System.Windows.Forms.MenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Text Files | *.txt";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem3,
            this.selectVoiceMenuItem,
            this.speechMenuItem});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.menuItem5,
            this.exitMenuItem});
            this.menuItem1.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItem1.Text = "&File";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Index = 0;
            this.newMenuItem.Text = "&New";
            this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Index = 1;
            this.openMenuItem.Text = "&Open ...";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 2;
            this.menuItem5.MergeOrder = 2;
            this.menuItem5.Text = "-";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 3;
            this.exitMenuItem.MergeOrder = 2;
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MdiList = true;
            this.menuItem2.Text = "&Window";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cascadeMenuItem,
            this.tileHorizontalMenuItem,
            this.tileVerticalMenuItem});
            this.menuItem3.Text = "&Arrange Windows";
            // 
            // cascadeMenuItem
            // 
            this.cascadeMenuItem.Index = 0;
            this.cascadeMenuItem.Text = "Cascade";
            this.cascadeMenuItem.Click += new System.EventHandler(this.cascadeMenuItem_Click);
            // 
            // tileHorizontalMenuItem
            // 
            this.tileHorizontalMenuItem.Index = 1;
            this.tileHorizontalMenuItem.Text = "Tile Horizontal";
            this.tileHorizontalMenuItem.Click += new System.EventHandler(this.tileHorizontalMenuItem_Click);
            // 
            // tileVerticalMenuItem
            // 
            this.tileVerticalMenuItem.Index = 2;
            this.tileVerticalMenuItem.Text = "Tile Vertical";
            this.tileVerticalMenuItem.Click += new System.EventHandler(this.tileVerticalMenuItem_Click);
            // 
            // selectVoiceMenuItem
            // 
            this.selectVoiceMenuItem.Index = 3;
            this.selectVoiceMenuItem.Text = "Select Voice";
            // 
            // speechMenuItem
            // 
            this.speechMenuItem.Index = 4;
            this.speechMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.readSelectedTextMenuItem,
            this.readDocumentMenuItem,
            this.speechRecognitionMenuItem,
            this.takeDictationMenuItem});
            this.speechMenuItem.Text = "Speech";
            // 
            // readSelectedTextMenuItem
            // 
            this.readSelectedTextMenuItem.Index = 0;
            this.readSelectedTextMenuItem.Text = "Read Selected Text";
            this.readSelectedTextMenuItem.Click += new System.EventHandler(this.readSelectedTextMenuItem_Click);
            // 
            // readDocumentMenuItem
            // 
            this.readDocumentMenuItem.Index = 1;
            this.readDocumentMenuItem.Text = "Read Document";
            this.readDocumentMenuItem.Click += new System.EventHandler(this.readDocumentMenuItem_Click);
            // 
            // speechRecognitionMenuItem
            // 
            this.speechRecognitionMenuItem.Index = 2;
            this.speechRecognitionMenuItem.Text = "Speech Recognition";
            this.speechRecognitionMenuItem.Click += new System.EventHandler(this.speechRecognitionMenuItem_Click);
            // 
            // takeDictationMenuItem
            // 
            this.takeDictationMenuItem.Index = 3;
            this.takeDictationMenuItem.Text = "Take Dictation";
            this.takeDictationMenuItem.Click += new System.EventHandler(this.takeDictationMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 574);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(646, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(57, 17);
            this.toolStripStatusLabel2.Text = "You Said:";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(574, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 596);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu1;
            this.Name = "Main";
            this.Text = "Speechpad";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem newMenuItem;
        private System.Windows.Forms.MenuItem openMenuItem;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem cascadeMenuItem;
        private System.Windows.Forms.MenuItem tileHorizontalMenuItem;
        private System.Windows.Forms.MenuItem tileVerticalMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.MenuItem selectVoiceMenuItem;
        private System.Windows.Forms.MenuItem speechMenuItem;
        private System.Windows.Forms.MenuItem readSelectedTextMenuItem;
        private System.Windows.Forms.MenuItem readDocumentMenuItem;
        private System.Windows.Forms.MenuItem speechRecognitionMenuItem;
        private System.Windows.Forms.MenuItem takeDictationMenuItem;
    }
}

