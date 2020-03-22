using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using GsmComm.Interfaces;

namespace RemotingClient
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
		}

		private void btnRemoteSendSMS_Click(object sender, EventArgs e)
		{
			// Set up a client channel
			IDictionary props = new Hashtable();
			if (chkSecure.Checked)
			{
				props["secure"] = "true";
				if (rbAuthBasic.Checked)
				{
					// Add basic authentication info
					props["username"] = txtUsername.Text;
					props["password"] = txtPassword.Text;
					props["domain"] = txtDomain.Text;
				}
			}
			TcpClientChannel clientChannel = new TcpClientChannel(props, null);
			ChannelServices.RegisterChannel(clientChannel, chkSecure.Checked);
			
			// Get object and send message
			try
			{
				string url = string.Format("tcp://{0}:{1}/SMSSender", txtServer.Text, int.Parse(txtPort.Text));
				ISmsSender smsSender = (ISmsSender)Activator.GetObject(typeof(ISmsSender), url);
				if (!chkUnicode.Checked)
					smsSender.SendMessage(txtMessage.Text, txtNumber.Text); // Standard message
				else
					smsSender.SendMessage(txtMessage.Text, txtNumber.Text, true); // Unicode message
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, string.Format("{0}\r\n\r\nException type: {1}",
					ex.Message, ex.GetType().ToString()), "Error");
			}
			finally
			{
				// Destroy client channel
				ChannelServices.UnregisterChannel(clientChannel);
				clientChannel = null;
			}
		}
	}
}