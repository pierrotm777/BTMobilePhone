/*=====================================================================
  File:      frmConnection.cs

  Summary:   Provides connection settings dialog for GSMComm Demo.

---------------------------------------------------------------------

This source code is intended only as a supplement to the GSMComm
development package or on-line documentation and may not be distributed
separately.
=====================================================================*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using GsmComm.GsmCommunication;

namespace GSMCommDemo
{
	/// <summary>
	/// Summary description for frmConnection.
	/// </summary>
	public class frmConnection : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblTimeout;
		private System.Windows.Forms.Label lblBaudRate;
		private System.Windows.Forms.ComboBox cboTimeout;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.ComboBox cboPort;
		private System.Windows.Forms.ComboBox cboBaudRate;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnTest;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string portName;
		private int baudRate;
		private int timeout;

		public frmConnection()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblTimeout = new System.Windows.Forms.Label();
			this.lblBaudRate = new System.Windows.Forms.Label();
			this.cboTimeout = new System.Windows.Forms.ComboBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.cboPort = new System.Windows.Forms.ComboBox();
			this.cboBaudRate = new System.Windows.Forms.ComboBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnTest = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblTimeout
			// 
			this.lblTimeout.Location = new System.Drawing.Point(33, 64);
			this.lblTimeout.Name = "lblTimeout";
			this.lblTimeout.Size = new System.Drawing.Size(100, 23);
			this.lblTimeout.TabIndex = 4;
			this.lblTimeout.Text = "Ti&meout (ms):";
			this.lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblBaudRate
			// 
			this.lblBaudRate.Location = new System.Drawing.Point(33, 40);
			this.lblBaudRate.Name = "lblBaudRate";
			this.lblBaudRate.Size = new System.Drawing.Size(100, 23);
			this.lblBaudRate.TabIndex = 2;
			this.lblBaudRate.Text = "&Baud rate:";
			this.lblBaudRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cboTimeout
			// 
			this.cboTimeout.Location = new System.Drawing.Point(145, 64);
			this.cboTimeout.Name = "cboTimeout";
			this.cboTimeout.Size = new System.Drawing.Size(104, 21);
			this.cboTimeout.TabIndex = 5;
			// 
			// lblPort
			// 
			this.lblPort.Location = new System.Drawing.Point(33, 16);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(88, 23);
			this.lblPort.TabIndex = 0;
			this.lblPort.Text = "&Port name:";
			this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cboPort
			// 
			this.cboPort.Location = new System.Drawing.Point(145, 16);
			this.cboPort.Name = "cboPort";
			this.cboPort.Size = new System.Drawing.Size(104, 21);
			this.cboPort.TabIndex = 1;
			// 
			// cboBaudRate
			// 
			this.cboBaudRate.Location = new System.Drawing.Point(145, 40);
			this.cboBaudRate.Name = "cboBaudRate";
			this.cboBaudRate.Size = new System.Drawing.Size(104, 21);
			this.cboBaudRate.TabIndex = 3;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(104, 112);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(192, 112);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			// 
			// btnTest
			// 
			this.btnTest.Location = new System.Drawing.Point(16, 112);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 6;
			this.btnTest.Text = "&Test";
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// frmConnection
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(282, 152);
			this.Controls.Add(this.btnTest);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblTimeout);
			this.Controls.Add(this.lblBaudRate);
			this.Controls.Add(this.cboTimeout);
			this.Controls.Add(this.lblPort);
			this.Controls.Add(this.cboPort);
			this.Controls.Add(this.cboBaudRate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmConnection";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connection settings";
			this.Load += new System.EventHandler(this.frmConnection_Load);
			this.ResumeLayout(false);

		}
		#endregion

		public void SetData(string portName, int baudRate, int timeout)
		{
			this.portName = portName;
			this.baudRate = baudRate;
			this.timeout = timeout;
		}

		public void GetData(out string portName, out int baudRate, out int timeout)
		{
			portName = this.portName;
			baudRate = this.baudRate;
			timeout = this.timeout;
		}

		private bool EnterNewSettings()
		{
			string newPortName;
			int newBaudRate;
			int newTimeout;

			try
			{
				if (cboPort.Text.Length == 0)
					throw new FormatException();
				newPortName = cboPort.Text;
			}
			catch(Exception)
			{
				MessageBox.Show(this, "Invalid port name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cboPort.Focus();
				return false;
			}

			try
			{
				newBaudRate = int.Parse(cboBaudRate.Text);
			}
			catch(Exception)
			{
				MessageBox.Show(this, "Invalid baud rate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cboBaudRate.Focus();
				return false;
			}

			try
			{
				newTimeout = int.Parse(cboTimeout.Text);
			}
			catch(Exception)
			{
				MessageBox.Show(this, "Invalid timeout value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cboTimeout.Focus();
				return false;
			}

			this.portName = newPortName;
			this.baudRate = newBaudRate;
			this.timeout = newTimeout;

			return true;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if (!EnterNewSettings())
				DialogResult = DialogResult.None;
		}

		private void frmConnection_Load(object sender, System.EventArgs e)
		{
			cboPort.Items.Add("COM1");
			cboPort.Items.Add("COM2");
			cboPort.Items.Add("COM3");
			cboPort.Items.Add("COM4");
			cboPort.Items.Add("COM5");
			cboPort.Items.Add("COM6");
			cboPort.Items.Add("COM7");
			cboPort.Items.Add("COM8");
			cboPort.Items.Add("COM9");
			cboPort.Text = portName.ToString();

			cboBaudRate.Items.Add("9600");
			cboBaudRate.Items.Add("19200");
			cboBaudRate.Items.Add("38400");
			cboBaudRate.Items.Add("57600");
			cboBaudRate.Items.Add("115200");
			cboBaudRate.Text = baudRate.ToString();

			cboTimeout.Items.Add("150");
			cboTimeout.Items.Add("300");
			cboTimeout.Items.Add("600");
			cboTimeout.Items.Add("900");
			cboTimeout.Items.Add("1200");
			cboTimeout.Items.Add("1500");
			cboTimeout.Items.Add("1800");
			cboTimeout.Items.Add("2000");
			cboTimeout.Text = timeout.ToString();
		}

		private void btnTest_Click(object sender, System.EventArgs e)
		{
			if (!EnterNewSettings())
				return;

			Cursor.Current = Cursors.WaitCursor;
			GsmCommMain comm = new GsmCommMain(portName, baudRate, timeout);
			try
			{
				comm.Open();
				while (!comm.IsConnected())
				{
					Cursor.Current = Cursors.Default;
					if (MessageBox.Show(this, "No phone connected.", "Connection setup",
						MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
					{
						comm.Close();
						return;
					}
					Cursor.Current = Cursors.WaitCursor;
				}

				comm.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, "Connection error: " + ex.Message, "Connection setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			MessageBox.Show(this, "Successfully connected to the phone.", "Connection setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
