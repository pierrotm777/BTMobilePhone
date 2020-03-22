namespace RemotingClient
{
	partial class frmMain
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
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.chkSecure = new System.Windows.Forms.CheckBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtServer = new System.Windows.Forms.TextBox();
			this.lblServer = new System.Windows.Forms.Label();
			this.chkUnicode = new System.Windows.Forms.CheckBox();
			this.txtNumber = new System.Windows.Forms.TextBox();
			this.btnSend = new System.Windows.Forms.Button();
			this.btnMessage = new System.Windows.Forms.Label();
			this.btnNumber = new System.Windows.Forms.Label();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbAuthBasic = new System.Windows.Forms.RadioButton();
			this.rbAuthDefault = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDomain = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.groupBox4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.chkSecure);
			this.groupBox4.Controls.Add(this.txtPort);
			this.groupBox4.Controls.Add(this.lblPort);
			this.groupBox4.Controls.Add(this.txtServer);
			this.groupBox4.Controls.Add(this.lblServer);
			this.groupBox4.Controls.Add(this.chkUnicode);
			this.groupBox4.Controls.Add(this.txtNumber);
			this.groupBox4.Controls.Add(this.btnSend);
			this.groupBox4.Controls.Add(this.btnMessage);
			this.groupBox4.Controls.Add(this.btnNumber);
			this.groupBox4.Controls.Add(this.txtMessage);
			this.groupBox4.Location = new System.Drawing.Point(12, 12);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(347, 151);
			this.groupBox4.TabIndex = 30;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Send SMS remotely";
			// 
			// chkSecure
			// 
			this.chkSecure.AutoSize = true;
			this.chkSecure.Location = new System.Drawing.Point(144, 45);
			this.chkSecure.Name = "chkSecure";
			this.chkSecure.Size = new System.Drawing.Size(60, 17);
			this.chkSecure.TabIndex = 4;
			this.chkSecure.Text = "Secure";
			this.chkSecure.UseVisualStyleBackColor = true;
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(70, 42);
			this.txtPort.MaxLength = 30;
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(68, 20);
			this.txtPort.TabIndex = 3;
			this.txtPort.Text = "2000";
			// 
			// lblPort
			// 
			this.lblPort.Location = new System.Drawing.Point(6, 42);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(56, 23);
			this.lblPort.TabIndex = 2;
			this.lblPort.Text = "Port:";
			this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtServer
			// 
			this.txtServer.Location = new System.Drawing.Point(70, 16);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(128, 20);
			this.txtServer.TabIndex = 1;
			this.txtServer.Text = "localhost";
			// 
			// lblServer
			// 
			this.lblServer.Location = new System.Drawing.Point(6, 16);
			this.lblServer.Name = "lblServer";
			this.lblServer.Size = new System.Drawing.Size(56, 23);
			this.lblServer.TabIndex = 0;
			this.lblServer.Text = "Server:";
			this.lblServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkUnicode
			// 
			this.chkUnicode.Location = new System.Drawing.Point(9, 120);
			this.chkUnicode.Name = "chkUnicode";
			this.chkUnicode.Size = new System.Drawing.Size(163, 24);
			this.chkUnicode.TabIndex = 10;
			this.chkUnicode.Text = "Send as Unicode (UCS2)";
			// 
			// txtNumber
			// 
			this.txtNumber.Location = new System.Drawing.Point(70, 94);
			this.txtNumber.MaxLength = 30;
			this.txtNumber.Name = "txtNumber";
			this.txtNumber.Size = new System.Drawing.Size(128, 20);
			this.txtNumber.TabIndex = 8;
			this.txtNumber.Text = "+483341234567";
			// 
			// btnSend
			// 
			this.btnSend.Location = new System.Drawing.Point(204, 94);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(128, 32);
			this.btnSend.TabIndex = 9;
			this.btnSend.Text = "Send Message";
			this.btnSend.Click += new System.EventHandler(this.btnRemoteSendSMS_Click);
			// 
			// btnMessage
			// 
			this.btnMessage.Location = new System.Drawing.Point(6, 68);
			this.btnMessage.Name = "btnMessage";
			this.btnMessage.Size = new System.Drawing.Size(56, 24);
			this.btnMessage.TabIndex = 5;
			this.btnMessage.Text = "Message:";
			this.btnMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnNumber
			// 
			this.btnNumber.Location = new System.Drawing.Point(6, 94);
			this.btnNumber.Name = "btnNumber";
			this.btnNumber.Size = new System.Drawing.Size(56, 23);
			this.btnNumber.TabIndex = 7;
			this.btnNumber.Text = "Number:";
			this.btnNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(70, 68);
			this.txtMessage.MaxLength = 160;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(256, 20);
			this.txtMessage.TabIndex = 6;
			this.txtMessage.Text = "This is a remoting test!";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbAuthBasic);
			this.groupBox1.Controls.Add(this.rbAuthDefault);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtDomain);
			this.groupBox1.Controls.Add(this.txtPassword);
			this.groupBox1.Controls.Add(this.txtUsername);
			this.groupBox1.Location = new System.Drawing.Point(12, 170);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(347, 147);
			this.groupBox1.TabIndex = 34;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Authentication (in secure mode)";
			// 
			// rbAuthBasic
			// 
			this.rbAuthBasic.AutoSize = true;
			this.rbAuthBasic.Location = new System.Drawing.Point(6, 42);
			this.rbAuthBasic.Name = "rbAuthBasic";
			this.rbAuthBasic.Size = new System.Drawing.Size(231, 17);
			this.rbAuthBasic.TabIndex = 1;
			this.rbAuthBasic.Text = "Authenticate with user name and password:";
			this.rbAuthBasic.UseVisualStyleBackColor = true;
			// 
			// rbAuthDefault
			// 
			this.rbAuthDefault.AutoSize = true;
			this.rbAuthDefault.Checked = true;
			this.rbAuthDefault.Location = new System.Drawing.Point(6, 19);
			this.rbAuthDefault.Name = "rbAuthDefault";
			this.rbAuthDefault.Size = new System.Drawing.Size(129, 17);
			this.rbAuthDefault.TabIndex = 0;
			this.rbAuthDefault.TabStop = true;
			this.rbAuthDefault.Text = "Default authentication";
			this.rbAuthDefault.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(40, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(46, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Domain:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(40, 94);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Password:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(40, 68);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "User name:";
			// 
			// txtDomain
			// 
			this.txtDomain.Location = new System.Drawing.Point(126, 117);
			this.txtDomain.Name = "txtDomain";
			this.txtDomain.Size = new System.Drawing.Size(188, 20);
			this.txtDomain.TabIndex = 7;
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(126, 91);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(188, 20);
			this.txtPassword.TabIndex = 5;
			// 
			// txtUsername
			// 
			this.txtUsername.Location = new System.Drawing.Point(126, 65);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(188, 20);
			this.txtUsername.TabIndex = 3;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(371, 329);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmMain";
			this.Text = "Send SMS";
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox chkSecure;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.Label lblServer;
		private System.Windows.Forms.CheckBox chkUnicode;
		private System.Windows.Forms.TextBox txtNumber;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Label btnMessage;
		private System.Windows.Forms.Label btnNumber;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtDomain;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton rbAuthDefault;
		private System.Windows.Forms.RadioButton rbAuthBasic;

	}
}

