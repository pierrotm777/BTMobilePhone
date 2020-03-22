namespace ForCf
{
    partial class NetworkStartDialog
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
            this.menuItemOk = new System.Windows.Forms.MenuItem();
            this.menuItemCancel = new System.Windows.Forms.MenuItem();
            this.bluetoothFtpCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.protoComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItemOk);
            this.mainMenu1.MenuItems.Add(this.menuItemCancel);
            // 
            // menuItemOk
            // 
            this.menuItemOk.Text = "OK";
            this.menuItemOk.Click += new System.EventHandler(this.menuItemOk_Click);
            // 
            // menuItemCancel
            // 
            this.menuItemCancel.Text = "Cancel";
            this.menuItemCancel.Click += new System.EventHandler(this.menuItemCancel_Click);
            // 
            // bluetoothFtpCheckBox
            // 
            this.bluetoothFtpCheckBox.Location = new System.Drawing.Point(57, 28);
            this.bluetoothFtpCheckBox.Name = "bluetoothFtpCheckBox";
            this.bluetoothFtpCheckBox.Size = new System.Drawing.Size(144, 20);
            this.bluetoothFtpCheckBox.TabIndex = 9;
            this.bluetoothFtpCheckBox.Text = "Bluetooth FTP profile";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.Text = "Select protocol:";
            // 
            // protoComboBox
            // 
            this.protoComboBox.Items.Add("!!uninitialised!!");
            this.protoComboBox.Location = new System.Drawing.Point(109, 3);
            this.protoComboBox.Name = "protoComboBox";
            this.protoComboBox.Size = new System.Drawing.Size(100, 22);
            this.protoComboBox.TabIndex = 8;
            // 
            // NetworkStartDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.bluetoothFtpCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.protoComboBox);
            this.Menu = this.mainMenu1;
            this.Name = "NetworkStartDialog";
            this.Text = "NetworkStartDialog";
            this.Load += new System.EventHandler(this.NetworkStartDialog_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.NetworkStartDialog_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemOk;
        private System.Windows.Forms.MenuItem menuItemCancel;
        private System.Windows.Forms.CheckBox bluetoothFtpCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox protoComboBox;
    }
}