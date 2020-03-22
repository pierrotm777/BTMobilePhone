#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

#endregion

namespace Brecham.Obex.Net.Forms
{
    /// <summary>
    /// Summary description for ProtocolComboBox.
    /// </summary>
    public class ProtocolComboBox : System.Windows.Forms.Control
    {
        //----------------------------------------------------------------------
        // Properties
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets the currently selected protocol as an instance of 
        /// <see cref="T:System.Net.Sockets.ProtocolFamily"/>.
        /// </summary>
        /// <remarks>
        /// This will currently be either <c>0x20</c> (<c>&amp;H20</c> in Visual Basic)
        /// for Bluetooth, 
        /// <see cref="F:System.Net.Sockets.ProtocolFamily.Irda"/> for IrDA, or
        /// <see cref="F:System.Net.Sockets.ProtocolFamily.InterNetwork"/> for TCP/IP.
        /// <note>
        /// The UI control is a <c>DropDownList</c> ComboBox so the user may not type
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
            //this.comboBox1.BeginUpdate();
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add(new ProtocolUiItem(TransportConnection.ProtocolFamily_Bluetooth, "Bluetooth"));
            this.comboBox1.Items.Add(new ProtocolUiItem(ProtocolFamily.Irda, "IrDA"));
            this.comboBox1.Items.Add(new ProtocolUiItem(ProtocolFamily.InterNetwork, "TCP/IP"));
            //this.comboBox1.EndUpdate();
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


        //----------------------------------------------------------------------
        //----------------------------------------------------------------------
        
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;


        public ProtocolComboBox()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitComponent call
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            // andyh==============
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            // 
            // comboBox1
            // 
            this.comboBox1.Items.Add("Failed!");
            this.comboBox1.Location = new System.Drawing.Point(0, 0);
            this.comboBox1.Size = new System.Drawing.Size(83, 22);
            // 
            // ProtocolComboBox
            // 
            this.Controls.Add(this.comboBox1);
            //
            this.ProtocolComboBox_Load(null, null);
        }
        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        //--------------------------------------------------------------
        //protected override void OnPaint(PaintEventArgs pe)
        //{
        //    // TODO: Add custom paint code here

        //    // Calling the base class OnPaint
        //    base.OnPaint(pe);
        //}

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.comboBox1.Size = this.Size;
        }

    }
}
