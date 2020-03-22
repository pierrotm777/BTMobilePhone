using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Speechpad
{
    public partial class TextDocument : Form
    {
        public TextDocument()
        {
            InitializeComponent();
        }

        #region Document File Operations

        private void SaveAs(string fileName)
        {
            try
            {
                saveFileDialog1.FileName = fileName;
                DialogResult dr = saveFileDialog1.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
                string saveFileName = saveFileDialog1.FileName;
                Save(saveFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveAs()
        {
            string fileName = this.Text;
            SaveAs(fileName);
        }

        internal void Save()
        {
            string fileName = this.Text;
            Save(fileName);
        }

        private void Save(string fileName)
        {
            string text = this.richTextBox1.Text;
            Save(fileName, text);
        }

        private void Save(string fileName, string text)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, false))
                {
                    sw.Write(text);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseDocument()
        {
            Dispose();
        }

        internal void Paste()
        {
            try
            {
                IDataObject data = Clipboard.GetDataObject();
                if (data.GetDataPresent(DataFormats.Text))
                {
                    InsertText(data.GetData(DataFormats.Text).ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal void InsertText(string text)
        {
            RichTextBox theBox = richTextBox1;
            theBox.SelectedText = text;
        }

        internal void Copy()
        {
            try
            {
                RichTextBox theBox = richTextBox1;
                Clipboard.Clear();
                Clipboard.SetDataObject(theBox.SelectedText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal void Cut()
        {
            Copy();
            Delete();
        }

        internal void Delete()
        {
            richTextBox1.SelectedText = string.Empty;
        }

        #endregion

        #region Main Menu Handlers

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            CloseDocument();
        }

        #endregion

        #region Popup Menu Handlers

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }
        #endregion
    }
}