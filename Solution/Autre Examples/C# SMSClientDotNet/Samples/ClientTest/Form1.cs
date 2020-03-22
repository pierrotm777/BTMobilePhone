using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using PSWinCom.Gateway.Client;

namespace ClientTest
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button cmdSend;
		private System.Windows.Forms.Button cmdListen;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button cmdStopListen;
		private System.Windows.Forms.TextBox txtReceiver;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtTraceLog;

		private ServerSocket socket = null;
		private SMSClient gw = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			gw = new SMSClient();
			gw.Username = "test"; // your username
			gw.Password = "test"; // your password
			gw.PrimaryGateway = "http://sms3.pswin.com/sms";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.cmdSend = new System.Windows.Forms.Button();
			this.cmdListen = new System.Windows.Forms.Button();
			this.cmdStopListen = new System.Windows.Forms.Button();
			this.txtReceiver = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTraceLog = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdSend
			// 
			this.cmdSend.Location = new System.Drawing.Point(180, 8);
			this.cmdSend.Name = "cmdSend";
			this.cmdSend.Size = new System.Drawing.Size(100, 28);
			this.cmdSend.TabIndex = 0;
			this.cmdSend.Text = "Send()";
			this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
			// 
			// cmdListen
			// 
			this.cmdListen.Location = new System.Drawing.Point(168, 128);
			this.cmdListen.Name = "cmdListen";
			this.cmdListen.Size = new System.Drawing.Size(100, 28);
			this.cmdListen.TabIndex = 1;
			this.cmdListen.Text = "Start Listen";
			this.cmdListen.Click += new System.EventHandler(this.cmdListen_Click);
			// 
			// cmdStopListen
			// 
			this.cmdStopListen.Location = new System.Drawing.Point(272, 128);
			this.cmdStopListen.Name = "cmdStopListen";
			this.cmdStopListen.Size = new System.Drawing.Size(100, 28);
			this.cmdStopListen.TabIndex = 2;
			this.cmdStopListen.Text = "Stop listen";
			this.cmdStopListen.Click += new System.EventHandler(this.cmdStopListen_Click);
			// 
			// txtReceiver
			// 
			this.txtReceiver.Location = new System.Drawing.Point(60, 12);
			this.txtReceiver.Name = "txtReceiver";
			this.txtReceiver.Size = new System.Drawing.Size(92, 20);
			this.txtReceiver.TabIndex = 3;
			this.txtReceiver.Text = "4792091312";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 24);
			this.label1.TabIndex = 4;
			this.label1.Text = "To:";
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(8, 56);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(364, 64);
			this.txtMessage.TabIndex = 5;
			this.txtMessage.Text = "test זרו ֶ״ֵ slutt";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "Message:";
			// 
			// txtTraceLog
			// 
			this.txtTraceLog.AcceptsReturn = true;
			this.txtTraceLog.Location = new System.Drawing.Point(8, 164);
			this.txtTraceLog.Multiline = true;
			this.txtTraceLog.Name = "txtTraceLog";
			this.txtTraceLog.Size = new System.Drawing.Size(364, 92);
			this.txtTraceLog.TabIndex = 7;
			this.txtTraceLog.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 148);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Trace log:";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(384, 266);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtTraceLog);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtMessage);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtReceiver);
			this.Controls.Add(this.cmdStopListen);
			this.Controls.Add(this.cmdListen);
			this.Controls.Add(this.cmdSend);
			this.Name = "Form1";
			this.Text = ".NET Gateway Client";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void cmdSend_Click(object sender, System.EventArgs e)
		{

			txtTraceLog.Text = "Sending message...\n\r";

			try 
			{
				// Create a message object
				PSWinCom.Gateway.Client.Message m = new PSWinCom.Gateway.Client.Message();
				m.ReceiverNumber = txtReceiver.Text;
				m.Text = txtMessage.Text;
				m.RequestReceipt = false; // Set to true for receipt

				// Add message object to Messages collection of client object
				gw.Messages.Clear();
				gw.Messages.Add(1, m);

				// Send all messages in Messages collection. This method is blocking. After completed,
				// each message will have Status updated to reflect the result of send-operation.
				gw.SendMessages();
				// Reference will only be available if activated on account.
				txtTraceLog.Text += "Message done. Status=" + m.Status.ToString() + " Reference=" + m.Reference + "\n";
			} 
			catch(SMSException se)
			{
				txtTraceLog.Text += "Sending failed: " + se.Message + "\n";
			}
		
		}


		/// <summary>
		/// Set up a ServerSocket and start listening on port 1112 for incoming messages and
		/// delivery notifications. Your account on the Gateway must be set up to forward
		/// notifications and messages to your IP address before you can test this.
		/// Contact support@pswin.com to arrange this.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdListen_Click(object sender, System.EventArgs e)
		{
			// If socket object has not been instanciated, then create object and hook up event.
			if(socket == null) 
			{
				socket = new ServerSocket();
				ServerSocket.ConnectionReceived +=new ConnectionReceivedHandler(ServerSocket_ConnectionReceived);
			}
			// Set port to listen to
			socket.Port = 1112;
			// Start listening. This is a non-blocking operation
			socket.StartListening();
			txtTraceLog.Text += "Start listening\n";
		}

		/// <summary>
		/// This event is fired by the ServerSocket object when a connection is established. The two 
		/// Stream objects is then handed over to the SMSClient object to extract and handle the incoming
		/// mess
		/// age or delivery report.
		/// </summary>
		/// <param name="inStream"></param>
		/// <param name="outStream"></param>
		private void ServerSocket_ConnectionReceived(System.IO.Stream inStream, System.IO.Stream outStream)
		{
			// Create a SMSClient object and let it handle the incoming streams
			SMSClient gw = new SMSClient();
			gw.HandleIncomingMessages(inStream, outStream);
			
			// Check collections of Received Messages and Delivery Reports and print 
			// what we have received.
			foreach(int i in gw.ReceivedMessages.Keys)
				txtTraceLog.Text += "Received Message: " + gw.ReceivedMessages[i].SenderNumber + " " + gw.ReceivedMessages[i].Text + "\n";

			foreach(int i in gw.DeliveryReports.Keys)
				txtTraceLog.Text += "Delivery Reports: " + gw.DeliveryReports[i].ReceiverNumber + " " + gw.DeliveryReports[i].State + "\n";
		}

		/// <summary>
		/// Stop listening
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdStopListen_Click(object sender, System.EventArgs e)
		{
			socket.StopListening();		
			txtTraceLog.Text += "Stopped listening\n";
		}
	}
}