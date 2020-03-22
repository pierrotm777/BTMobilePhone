using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Brecham.Obex.Net.Forms
{
    internal partial class TcpipAddressDialogSmartphoneImpl : TcpipAddressDialogImplForm
    {
        public TcpipAddressDialogSmartphoneImpl()
        {
            InitializeComponent();
            base.m_addressTextBox = this.textBox1;
        }
    }//class
}