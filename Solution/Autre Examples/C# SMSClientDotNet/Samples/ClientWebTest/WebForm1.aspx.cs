using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using PSWinCom.Gateway.Client;

namespace ClientWebTest
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class WebForm1 : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			//AppDomain.CurrentDomain.AppendPrivatePath(Request.PhysicalApplicationPath);
			if(Request.HttpMethod.Equals("POST"))
			{
				Response.ClearContent();
				SMSClient gw = new SMSClient();
				gw.HandleIncomingMessages(Request.InputStream, Response.OutputStream);

				// Check collections of Received Messages and Delivery Reports
				foreach(int i in gw.ReceivedMessages.Keys)
					Console.WriteLine("Received Message: " + gw.ReceivedMessages[i].SenderNumber + " " + gw.ReceivedMessages[i].ReceiverNumber + " " + gw.ReceivedMessages[i].Text);

				foreach(int i in gw.DeliveryReports.Keys)
					Console.WriteLine("Delivery Reports: " + gw.DeliveryReports[i].ReceiverNumber + " " + gw.DeliveryReports[i].State);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
