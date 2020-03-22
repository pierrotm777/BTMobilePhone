using System;
using System.Windows.Forms;

namespace Brecham.Obex.Net.Forms
{
#if FX1_1
    // HACK
    public 
#else
    internal
#endif
        class TcpipAddressDialogImplForm : Form, ITcpipAddressDialog
    {
        //--------------------------------------------------------------
        protected String m_address;
        protected TextBox m_addressTextBox;

        //--------------------------------------------------------------
        protected void TcpipAddressDialog_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(this.m_addressTextBox != null,
                "m_addressTextBox must be set in the derived form class contructor to its textbox.");
            if (m_address != null) {
                this.m_addressTextBox.Text = m_address;
            }
        }

        protected void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        protected void buttonOk_Click(object sender, EventArgs e)
        {
            //TODO Validation of IPAddress/Hostname input?
            // Could Do IpAddress.TryParse, and IpAddress.Lookup.......
            m_address = this.m_addressTextBox.Text;
            this.DialogResult = DialogResult.OK;
        }

        //--------------------------------------------------------------
        public String AddressOrHostName
        {
            get { return m_address; }
            set { m_address = value; }
        }

    }



    /// <summary>
    /// Interface to allow the TcpipAddressDialog on NETCF to be an Adapter pattern,
    /// hiding separate implementations for CE/PocketPC and Smartphone.
    /// </summary>
    internal interface ITcpipAddressDialog
    {
        DialogResult ShowDialog();

        String AddressOrHostName { get; set; }
    }//interface

    /// <summary>
    /// A dialog box to get an IP Address of HostName from the user.  Intended mainly
    /// for TransportConnection's internal use.
    /// </summary>
    public class TcpipAddressDialog : ITcpipAddressDialog
    {
        //--------------------------------------------------------------
        ITcpipAddressDialog m_impl;


        //--------------------------------------------------------------
        public TcpipAddressDialog()
        {
#if FX1_1
            //System.Diagnostics.Debug.Fail("m_impl");
            m_impl = new TcpipAddressDialogCf1();
#else
            if (IsSmartphonePlatform()) {
                m_impl = new TcpipAddressDialogSmartphoneImpl();
            } else {
                m_impl = new TcpipAddressDialogCfImpl();
            }
#endif
        }

#if ! FX1_1
        internal static bool IsSmartphonePlatform()
        {
            Platform p = PlatformDetector.GetPlatform();
            return p == Platform.Smartphone;
        }
#endif


        //--------------------------------------------------------------
        public DialogResult ShowDialog()
        {
            return m_impl.ShowDialog();
        }


        public String AddressOrHostName
        {
            // Proxy onto the implementation object
            get { return m_impl.AddressOrHostName; }
            set { m_impl.AddressOrHostName = value; }
        }

    }//class

}
