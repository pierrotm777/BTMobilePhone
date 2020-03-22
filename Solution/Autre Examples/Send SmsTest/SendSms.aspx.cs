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

namespace SmsTest
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class WebForm1 : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button Send;
		protected System.Web.UI.WebControls.TextBox txtMobileNo;
		protected System.Web.UI.WebControls.TextBox txtEmailId;
		protected System.Web.UI.WebControls.TextBox txtCountryCode;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected System.Web.UI.WebControls.RadioButtonList rdoType;
		protected System.Web.UI.WebControls.TextBox txtMessage;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			txtCountryCode.Enabled = false;
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
			this.Send.Click += new System.EventHandler(this.Send_Click);
			this.rdoType.SelectedIndexChanged += new System.EventHandler(this.rdoType_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Send_Click(object sender, System.EventArgs e)
		{
			try
			{
				SmsTest.net.webservicex.www.SendSMS smsIndia= new SmsTest.net.webservicex.www.SendSMS();
				SmsTest.com.webservicex.www.SendSMSWorld smsWorld =  new SmsTest.com.webservicex.www.SendSMSWorld();
				if(rdoType.SelectedValue == "1")
					smsIndia.SendSMSToIndia(txtMobileNo.Text.Trim(), txtEmailId.Text.Trim(), txtMessage.Text);
				else 
					smsWorld.sendSMS(txtEmailId.Text.Trim(), txtCountryCode.Text.Trim(), txtMobileNo.Text.Trim(), txtMessage.Text);
				lblMessage.Visible = true;
				lblMessage.Text="Message Send Succesfully";
			}
			catch(Exception ex)
			{
				lblMessage.Visible = true;
				lblMessage.Text="Error in Sending message"+ex.ToString();
			}
		}

		private void rdoType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(rdoType.SelectedValue =="1")
				txtCountryCode.Enabled = false;
			else
				txtCountryCode.Enabled = false;

		}
	}
}
