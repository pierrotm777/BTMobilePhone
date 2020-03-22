/*=====================================================================
  File:      SecuritySettings.cs

  Summary:   Security settings for GSMComm Demo application.

---------------------------------------------------------------------

This source code is intended only as a supplement to the GSMComm
development package or on-line documentation and may not be distributed
separately.
=====================================================================*/

using System;
using System.Collections.Generic;
using System.Text;

namespace GSMCommDemo
{
	public class SecuritySettings
	{
		private bool authUserNamePassword;
		private string userName;
		private string password;
		private string domain;

		public SecuritySettings()
		{
			authUserNamePassword = false;
			userName = string.Empty;
			password = string.Empty;
			domain = string.Empty;
		}

		public bool AuthUserNamePassword
		{
			get { return authUserNamePassword; }
			set { authUserNamePassword = value; }
		}

		public string UserName
		{
			get { return userName; }
			set { userName = value; }
		}

		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		public string Domain
		{
			get { return domain; }
			set { domain = value; }
		}
	}
}
