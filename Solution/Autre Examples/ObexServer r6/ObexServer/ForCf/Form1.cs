using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ForCf
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            this.textBox1.Text = null;
        }

        delegate void SetTextAndMenuEnabledDlgt(Control c1, String text, MenuItem menuItem, bool enabled);
        void SetTextAndMenuEnabled(Control c, String text, MenuItem menuItem, bool enabled)
        {
            c.Text = text;
            menuItem.Enabled = enabled;
        }

    }//class
}