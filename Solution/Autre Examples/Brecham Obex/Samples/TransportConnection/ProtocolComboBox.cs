using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;//eg ProtocolFamily

namespace Brecham.Obex.Net.Forms
{
    /// <summary>
    /// Represents a Windows <see cref="T:System.Windows.Forms.ComboBox"/> for
    /// selecting a <see cref="T:System.Net.Sockets.ProtocolFamily"/>.
    /// </summary>
    /// <remarks>
    /// This will currently allow selection from Bluetooth, IrDA, and TCP/IP.
    /// The <see cref="P:Brecham.Obex.Net.Forms.ProtocolComboBox.SelectedProtocol"/> property returning respectively
    /// <c>0x20</c> (<c>&amp;H20</c> in Visual Basic) for Bluetooth (i.e. 
    /// <see cref="F:Brecham.Obex.Net.TransportConnection.ProtocolFamily_Bluetooth">Brecham.Obex.Net.TransportConnection.ProtocolFamily_Bluetooth</see>), 
    /// <see cref="F:System.Net.Sockets.ProtocolFamily.Irda">System.Net.Sockets.ProtocolFamily.Irda</see>, or
    /// <see cref="F:System.Net.Sockets.ProtocolFamily.InterNetwork">System.Net.Sockets.ProtocolFamily.InterNetwork</see>.
    /// </remarks>
    public partial class ProtocolComboBox : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProtocolComboBox"/> class.
        /// </summary>
        public ProtocolComboBox()
        {
            InitializeComponent();
#if NETCF
            // For NETCF when running on desktop CLR.
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ProtocolComboBox_Load(null, null);
#endif
            System.Diagnostics.Debug.Assert(this.comboBox1.DropDownStyle == ComboBoxStyle.DropDownList);
        }

        //----------------------------------------------------------------------
        // Properties
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets the currently selected protocol as an instance of 
        /// <see cref="T:System.Net.Sockets.ProtocolFamily"/>.
        /// </summary>
        /// <remarks>
        /// This will currently be either <c>0x20</c> (<c>&amp;H20</c> in Visual Basic)
        /// for Bluetooth (i.e. 
        /// <see cref="F:Brecham.Obex.Net.TransportConnection.ProtocolFamily_Bluetooth">TransportConnection.ProtocolFamily_Bluetooth</see>), 
        /// <see cref="F:System.Net.Sockets.ProtocolFamily.Irda">ProtocolFamily.Irda</see> for IrDA, or
        /// <see cref="F:System.Net.Sockets.ProtocolFamily.InterNetwork">ProtocolFamily.InterNetwork</see> for TCP/IP.
        /// <note>
        /// The UI control is a <c>DropDownList</c> ComboBox so the user may <i>not</i> type
        /// their own value in the control and thus cause an invalid value to be
        /// returned. However if any unexpected event occurs to prevent this then the
        /// <see cref="F:System.Net.Sockets.ProtocolFamily.Unspecified"/>
        /// value is returned.
        /// </note>
        /// </remarks>
        public ProtocolFamily SelectedProtocol
        {
            get
            {
                // This may be null if the user has managed to defeat the system and
                // typed in their own new item.  This can occur as described below.
                Object selectedObj = comboBox1.SelectedItem;
                ProtocolUiItem selectedItem = selectedObj as ProtocolUiItem;
                if (selectedItem != null) {
                    return selectedItem.ProtocolFamily;
                } else {
                    // By design this should not ever happen, however when a CF
                    // program is run on the desktop UI a DropDownList ComboBox becomes
                    // a DropDown, and thus allows the user two write their own text!
                    //System.Diagnostics.Debug.Fail("Unexpected/null return from ComboBox.");
                    return ProtocolFamily.Unspecified;
                }
            }
        }

        //----------------------------------------------------------------------
        // Methods
        //----------------------------------------------------------------------
        private void ProtocolComboBox_Load(object sender, EventArgs e)
        {
            // Can't use AddRange for compatibility with NETCF.
            this.comboBox1.BeginUpdate();
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add(new ProtocolUiItem(TransportConnection.ProtocolFamily_Bluetooth, "Bluetooth"));
            this.comboBox1.Items.Add(new ProtocolUiItem(ProtocolFamily.Irda, "IrDA"));
            this.comboBox1.Items.Add(new ProtocolUiItem(ProtocolFamily.InterNetwork, "TCP/IP"));
            this.comboBox1.EndUpdate();
            this.comboBox1.SelectedIndex = 0;
        }//class--ProtocolUiItem

        //----------------------------------------------------------------------
        // Classes
        //----------------------------------------------------------------------
        private class ProtocolUiItem
        {
            // Methods
            public ProtocolUiItem(ProtocolFamily pf, string text)
            {
                this.m_pf = pf;
                this.m_text = text;
            }

            public override string ToString()
            {
                if (this.m_text != null) {
                    return this.m_text;
                }
                return this.m_pf.ToString();
            }

            // Properties
            public ProtocolFamily ProtocolFamily { get { return this.m_pf; } }

            // Fields
            private ProtocolFamily m_pf;
            private string m_text;
        }

    }//class ProtocolComboBox
}
