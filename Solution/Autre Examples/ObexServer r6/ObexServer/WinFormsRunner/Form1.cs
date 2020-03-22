using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ForCf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        delegate void SetTextAndMenuEnabledDlgt(Control c1, String text, ToolStripMenuItem menuItem, bool enabled);
        void SetTextAndMenuEnabled(Control c, String text, ToolStripMenuItem menuItem, bool enabled)
        {
            c.Text = text;
            menuItem.Enabled = enabled;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DoQuit(sender, e);
        }

    }
}