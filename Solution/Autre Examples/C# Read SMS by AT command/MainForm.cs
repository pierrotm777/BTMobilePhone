using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace ReadSMS_AT_CS20
{
	public partial class MainForm : Form
	{
		private AutoResetEvent receiveNow;
		private SerialPort port;

		public MainForm()
		{
			InitializeComponent();
			port = null;
			receiveNow = new AutoResetEvent(false);
		}

		private void btnRead_Click(object sender, EventArgs e)
		{
			// Get and verify port name
			string portName = cboPort.Text;
			if (portName.Length == 0)
			{
				MessageBox.Show(this, "You must enter a port name.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			lvwMessages.Items.Clear();
			Update();

			// Set up the phone and read the messages
			ShortMessageCollection messages = null;
			try
			{
				this.port = OpenPort(portName);
				Cursor.Current = Cursors.WaitCursor;
				// Check connection
				ExecCommand("AT", 300, "No phone connected at " + portName + "." );
				// Use message format "Text mode"
				ExecCommand("AT+CMGF=1", 300, "Failed to set message format.");
				// Use character set "ISO 8859-1"
				ExecCommand("AT+CSCS=\"8859-1\"", 300, "Failed to set character set.");
				// Select SIM storage
				ExecCommand("AT+CPMS=\"SM\"", 300, "Failed to select message storage.");
				// Read the messages
				string input = ExecCommand("AT+CMGL=\"ALL\"", 5000, "Failed to read the messages.");
				messages = ParseMessages(input);
				Cursor.Current = Cursors.Default;
			}
			catch (Exception ex)
			{
				Cursor.Current = Cursors.Default;
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			finally
			{
				if (port != null)
				{
					ClosePort(this.port);
					this.port = null;
				}
			}

			if (messages != null)
				DisplayMessages(messages);
		}

		private void DisplayMessages(ShortMessageCollection messages)
		{
			lvwMessages.BeginUpdate();
			foreach (ShortMessage msg in messages)
			{
				ListViewItem item = new ListViewItem(new string[] { msg.Sender, msg.Message });
				item.Tag = msg;
				lvwMessages.Items.Add(item);
			}
			lvwMessages.EndUpdate();
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MainForm_Resize(object sender, EventArgs e)
		{
			colMessage.Width = -2;
		}

		private ShortMessageCollection ParseMessages(string input)
		{
			ShortMessageCollection messages = new ShortMessageCollection();
			Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.+)\r\n");
			Match m = r.Match(input);
			while (m.Success)
			{
				ShortMessage msg = new ShortMessage();
				msg.Index = int.Parse(m.Groups[1].Value);
				msg.Status = m.Groups[2].Value;
				msg.Sender = m.Groups[3].Value;
				msg.Alphabet = m.Groups[4].Value;
				msg.Sent = m.Groups[5].Value;
				msg.Message = m.Groups[6].Value;
				messages.Add(msg);

				m = m.NextMatch();
			}

			return messages;
		}

		#region Communication
		private SerialPort OpenPort(string portName)
		{
			SerialPort port = new SerialPort();
			port.PortName = portName;
			port.BaudRate = 19200;
			port.DataBits = 8;
			port.StopBits = StopBits.One;
			port.Parity = Parity.None;
			port.ReadTimeout = 300;
			port.WriteTimeout = 300;
			port.Encoding = Encoding.GetEncoding("iso-8859-1");
			port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
			port.Open();
			port.DtrEnable = true;
			port.RtsEnable = true;
			return port;
		}

		private void ClosePort(SerialPort port)
		{
			port.Close();
			port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
		}

		void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (e.EventType == SerialData.Chars)
				receiveNow.Set();
		}

		private string ReadResponse(int timeout)
		{
			string buffer = string.Empty;
			do
			{
				if (receiveNow.WaitOne(timeout, false))
				{
					string t = port.ReadExisting();
					buffer += t;
				}
				else
				{
					if (buffer.Length > 0)
						throw new ApplicationException("Response received is incomplete.");
					else
						throw new ApplicationException("No data received from phone.");
				}
			}
			while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\nERROR\r\n"));
			return buffer;
		}

		private string ExecCommand(string command, int responseTimeout, string errorMessage)
		{
			try
			{
				port.DiscardOutBuffer();
				port.DiscardInBuffer();
				receiveNow.Reset();
				port.Write(command + "\r");

				string input = ReadResponse(responseTimeout);
				if ((input.Length == 0) || (!input.EndsWith("\r\nOK\r\n")))
					throw new ApplicationException("No success message was received.");
				return input;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(errorMessage, ex);
			}
		}
		#endregion
	}
}