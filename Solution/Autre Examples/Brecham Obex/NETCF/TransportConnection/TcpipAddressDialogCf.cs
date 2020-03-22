using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Brecham.Obex.Net.Forms
{
    internal partial class TcpipAddressDialogCfImpl : TcpipAddressDialogImplForm
    {
        public TcpipAddressDialogCfImpl()
        {
            InitializeComponent();
            base.m_addressTextBox = this.textBox1;
        }
    }//class
}