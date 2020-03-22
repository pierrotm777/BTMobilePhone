using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using GsmComm.Server;
using System.IO;

namespace RemotingServer
{
	public partial class RemotingServer : ServiceBase
	{
		private SmsServer server;
		private string appDataDirectory;
		private string logFilePath;

		public RemotingServer()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			// This service creates a log file at "%ALLUSERSPROFILE%\RemotingServer\RemotingServer.log".
			// The log file is deleted on every startup, which is OK for development purposes.
			appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "RemotingServer");
			logFilePath = Path.Combine(appDataDirectory, "RemotingServer.log");
			if (!Directory.Exists(appDataDirectory))
				Directory.CreateDirectory(appDataDirectory);
			if (File.Exists(logFilePath))
				File.Delete(logFilePath);

			try
			{
				Log("SMS server is starting...");
				server = new SmsServer();
				server.PortName = "COM1";
				server.BaudRate = 115200;
				server.MessageSendStarting += new MessageSendEventHandler(server_MessageSendStarting);
				server.MessageSendComplete += new MessageSendEventHandler(server_MessageSendComplete);
				server.MessageSendFailed += new MessageSendErrorEventHandler(server_MessageSendFailed);
				server.Start();
				Log("SMS server started.");
			}
			catch(Exception ex)
			{
				Log("SMS server could not be started. Error: " + ex.ToString());
				throw;
			}
		}

		protected override void OnStop()
		{
			try
			{
				Log("SMS server is stopping...");
				server.Stop();
				server.MessageSendStarting -= new MessageSendEventHandler(server_MessageSendStarting);
				server.MessageSendComplete -= new MessageSendEventHandler(server_MessageSendComplete);
				server.MessageSendFailed -= new MessageSendErrorEventHandler(server_MessageSendFailed);
				server = null;
				Log("SMS server stopped.");
			}
			catch (Exception ex)
			{
				Log("SMS server could not be stopped. Error: " + ex.ToString());
				throw;
			}
		}

		void server_MessageSendFailed(object sender, MessageSendErrorEventArgs e)
		{
			Log(string.Format("Failed to send message to {0}. Details: {1}",
				e.Destination, e.Exception.Message), e.UserName);
		}

		void server_MessageSendComplete(object sender, MessageSendEventArgs e)
		{
			Log(string.Format("Message sent to {0}.", e.Destination), e.UserName);
		}

		void server_MessageSendStarting(object sender, MessageSendEventArgs e)
		{
			Log(string.Format("Sending message to {0}.", e.Destination), e.UserName);
		}

		private void Log(string text)
		{
			StreamWriter logWriter = new StreamWriter(this.logFilePath, true);
			try
			{
				logWriter.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString(), text));
			}
			finally
			{
				logWriter.Close();
			}
		}

		private void Log(string text, string userName)
		{
			if (userName.Length == 0)
				Log(text);
			else
				Log(string.Format("{0} User name: {1}", text, userName));
		}

	}
}
