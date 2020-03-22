using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ForCf
{
    public partial class NetworkStartDialog : Form
    {
        public NetworkStartDialog()
        {
            InitializeComponent();
            //
            this.protoComboBox.Items.Clear();
            this.protoComboBox.Items.Add(CmdlineRunner.ListenProtocol.Bluetooth);
            this.protoComboBox.Items.Add(CmdlineRunner.ListenProtocol.Irda);
            this.protoComboBox.Items.Add(CmdlineRunner.ListenProtocol.Ipv4);
            this.protoComboBox.Items.Add(CmdlineRunner.ListenProtocol.Ipv6);
            //TODO this.protoComboBox.Items.Add(CmdlineRunner.ListenProtocol.Ipv6And4Pair);
            this.protoComboBox.Items.Add(CmdlineRunner.ListenProtocol.Ipv6And4Dual);
        }

        //--------
        private CmdlineRunner.ListenProtocol m_listenProtocol;
        private bool m_bluetoothFTP;

        //--------
        public CmdlineRunner.ListenProtocol NetworkProtocol
        {
            get { return m_listenProtocol; }
            set { m_listenProtocol = value; }
        }

        public bool BluetoothFtp
        {
            get { return m_bluetoothFTP; }
            set { m_bluetoothFTP = value; }
        }

        //--------
        private void menuItemOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void menuItemCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void NetworkStartDialog_Load(object sender, EventArgs e)
        {
            this.protoComboBox.SelectedItem = m_listenProtocol;
            this.bluetoothFtpCheckBox.Checked = m_bluetoothFTP;
        }

        private void NetworkStartDialog_Closing(object sender, CancelEventArgs e)
        {
            // Only validate on OK buttons (OK menu and title-bar close [Ok] button).
            if (this.DialogResult != DialogResult.OK) {
                return;
            }
            //
            object item = this.protoComboBox.SelectedItem;
            CmdlineRunner.ListenProtocol listenProtocol;
            if (item != null) {
                listenProtocol = (CmdlineRunner.ListenProtocol)item;
            } else {
                listenProtocol = CmdlineRunner.ListenProtocol.None;
            }
            if (listenProtocol == CmdlineRunner.ListenProtocol.None) {
                MessageBox.Show("Unknown protocol chosen");
                e.Cancel = true;
            } else {
                m_listenProtocol = listenProtocol;
                m_bluetoothFTP = this.bluetoothFtpCheckBox.Checked;
                System.Diagnostics.Debug.Assert(this.DialogResult == DialogResult.OK);
            }
        }

    }//class

}