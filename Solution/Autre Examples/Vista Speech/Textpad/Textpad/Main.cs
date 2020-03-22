using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Textpad
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        #region Main File Operations

        private void NewMDIChild()
        {
            NewMDIChild("Untitled");
        }

        private void NewMDIChild(string filename)
        {
            TextDocument newMDIChild = new TextDocument();
            newMDIChild.MdiParent = this;
            newMDIChild.Text = filename;
            newMDIChild.WindowState = FormWindowState.Maximized;
            newMDIChild.Show();
        }

        private void OpenFile()
        {
            try
            {
                openFileDialog1.FileName = "";
                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
                string fileName = openFileDialog1.FileName;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string text = sr.ReadToEnd();
                    NewMDIChild(fileName, text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewMDIChild(string filename, string text)
        {
            NewMDIChild(filename);
            LoadTextToActiveDocument(text);
        }

        private void LoadTextToActiveDocument(string text)
        {
            TextDocument doc = (TextDocument)ActiveMdiChild;
            doc.richTextBox1.Text = text;
        }

        private void Exit()
        {
            Dispose();
        }

        #endregion

        #region MDI Layout

        private void cascadeMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tileVerticalMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void tileHorizontalMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        #endregion

        #region Main Menu Handlers

        private void newMenuItem_Click(object sender, EventArgs e)
        {
            NewMDIChild();
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        #endregion

    }
}