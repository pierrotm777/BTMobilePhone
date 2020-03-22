/*=====================================================================
  File:      frmSecurity.cs

  Summary:   Provides security settings dialog for GSMComm Demo
             application.

---------------------------------------------------------------------

This source code is intended only as a supplement to the GSMComm
development package or on-line documentation and may not be distributed
separately.
=====================================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GSMCommDemo
{
	public partial class frmSecurity : Form
	{
		public frmSecurity()
		{
			InitializeComponent();
		}

		private void chkAuthUserNamePassword_CheckedChanged(object sender, EventArgs e)
		{
			bool enabled = chkAuthUserNamePassword.Checked;
			txtUsername.Enabled = enabled;
			txtPassword.Enabled = enabled;
			txtDomain.Enabled = enabled;
		}
	}
}