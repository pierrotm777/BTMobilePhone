#region Using directives

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

#endregion

namespace Brecham.Obex.Net.Forms
{
    /// <summary>
    /// Summary description for TcpipAddressDialogCf1.
    /// </summary>
    public class TcpipAddressDialogCf1 : TcpipAddressDialogImplForm //System.Windows.Forms.Form
    {
        private Label label1;
        private TextBox textBox1;
        private Button buttonOk;
        private Button buttonCancel;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        /// <summary>
        /// Main menu for the form.
        /// </summary>
        private System.Windows.Forms.MainMenu mainMenu1;

        public TcpipAddressDialogCf1()
        {
            InitializeComponent();
            base.m_addressTextBox = this.textBox1;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "OK";
            this.menuItem1.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Cancel";
            this.menuItem2.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Size = new System.Drawing.Size(233, 20);
            this.label1.Text = "IP &Address or Hostname:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(4, 28);
            this.textBox1.Size = new System.Drawing.Size(233, 21);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(87, 55);
            this.buttonOk.Size = new System.Drawing.Size(72, 20);
            this.buttonOk.Text = "&OK";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(165, 55);
            this.buttonCancel.Size = new System.Drawing.Size(72, 20);
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // TcpipAddressDialogCf1
            // 
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Text = "TCP/IP Address";
            this.Load += new System.EventHandler(this.TcpipAddressDialog_Load);

        }

        #endregion

        private void foo(object sender, EventArgs e)
        {

        }
    }
}
