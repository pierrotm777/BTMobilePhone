using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Brecham.Obex.Net.Forms
{
    /// <summary>
    /// Prompts the user to supply an TCP/IP Address or Hostname.
    /// </summary>
    /// <example>
    /// <code lang="C#">
    /// String addr = "192.168.1.101";
    /// Brecham.Obex.Net.Forms.TcpipAddressDialog form
    ///   = new Brecham.Obex.Net.Forms.TcpipAddressDialog();
    /// form.AddressOrHostname = addr;
    /// System.Windows.Forms.DialogResult rslt = form.ShowDialog();
    /// if (rslt == System.Windows.Forms.DialogResult.OK) {
    ///   addr = form.AddressOrHostname;
    ///   return addr;
    /// } else {
    ///   return null;
    /// }
    /// </code>
    /// </example>
    public partial class TcpipAddressDialog : Form
    {
        String m_address;

        /// <summary>
        /// Initializes an instance of the 
        /// <see cref="Brecham.Obex.Net.Forms.TcpipAddressDialog"/> class.
        /// </summary>
        public TcpipAddressDialog()
        {
            InitializeComponent();
        }

        private void TcpipAddressDialog_Load(object sender, EventArgs e)
        {
            if (m_address != null) {
                this.textBox1.Text = m_address;
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            //TODO Validation of IPAddress/Hostname input?
            // Could Do IpAddress.TryParse, and IpAddress.Lookup.......
            m_address = this.textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Gets or sets the TCP/IP Address or Hostname from the dialog box.
        /// </summary>
        /// <remarks>
        /// If set before displaying the dialog the value will be pre-selected in the
        /// dialog when displayed, allowing the user to simply hit return to choose
        /// that address.  On the dialog box closing, the value will only have been
        /// updated if the dialog box was closed with OK.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AddressOr")]
        public String AddressOrHostName
        {
            get { return m_address; }
            set { m_address = value; }
        }

    }//class
}