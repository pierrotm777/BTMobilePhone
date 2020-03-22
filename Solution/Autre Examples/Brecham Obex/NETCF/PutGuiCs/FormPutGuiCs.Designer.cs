namespace PutGuiCs
{
    partial class FormPutGuiCs
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
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.Label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.protocolComboBox1 = new Brecham.Obex.Net.Forms.ProtocolComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Browse…";
            this.menuItem1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Cancel";
            this.menuItem2.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(4, 73);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(232, 23);
            this.Label1.Text = "To choose the file to send, click Browse…";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(4, 207);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(233, 20);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(163, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 20);
            this.button1.TabIndex = 1;
            this.button1.Text = "B&rowse…";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(4, 145);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(233, 56);
            this.labelStatus.Text = "label2";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(162, 245);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(72, 20);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // protocolComboBox1
            // 
            this.protocolComboBox1.Location = new System.Drawing.Point(70, 8);
            this.protocolComboBox1.Name = "protocolComboBox1";
            this.protocolComboBox1.Size = new System.Drawing.Size(95, 21);
            this.protocolComboBox1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 20);
            this.label2.Text = "&Protocol:";
            // 
            // FormPutGuiCs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.protocolComboBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormPutGuiCs";
            this.Text = "PutGuiCs";
            this.Load += new System.EventHandler(this.FormPutGuiCs_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ProgressBar progressBar1;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Timer timer1;
        internal System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonCancel;
        private Brecham.Obex.Net.Forms.ProtocolComboBox protocolComboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
    }
}

