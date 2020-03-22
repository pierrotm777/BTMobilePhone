using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using hcwgenericclasses;
using System.IO;
using OpenHardwareMonitor;

namespace Temperature
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(string content)
        {
            InitializeComponent();
            TimerInterval = content;  // PASS TIMER INTERVAL FOR DISPLAY
            
        }
        private AboutBoxTools abt = new AboutBoxTools();
        private DialogTools dt = new DialogTools();
        private string TimerInterval = String.Empty;
        // ALTERNATING LISTVIEW ROW COLORS
        public void SetAlternatingColors(ListView lv, Color c1, Color c2)
       {
          foreach (ListViewItem item in lv.Items)
          { 
              if (item.Index % 2 == 0)
              {
                  item.BackColor = c1;
              }
              else
              {
                  item.BackColor = c2;
              }
          }     
        }
        // FORM CLOSE
        private void btnForm2Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // FORM LOAD
        private void Form2_Load(object sender, EventArgs e)
        {
            if (TimerInterval != String.Empty)
            {
                lblTimerInterval.Text = TimerInterval;
            }
            else
            {
                lblTimerInterval.Text = "";
            }
            OHData odF2 = new OHData();
            odF2.Update();
            foreach (OHData.OHMitem o in odF2.DataList)
            {
                string[] r = new string[2];
                r[0] = o.name;
                r[1] = o.reading;
                listView1.Items.Add(o.type).SubItems.AddRange(r);

            }
            WMIBIOS b = new WMIBIOS();
            b.Update();
            if (b.Name != String.Empty)
            {
                string type = String.Empty;
                string[] r = new string[2];
                type = "BIOS";
                r[0] = b.Name;
                r[1] = "";
                listView1.Items.Add(type).SubItems.AddRange(r);
                type = "Manufacturer";
                r[0] = b.Manufacturer;
                r[1] = "";
                listView1.Items.Add(type).SubItems.AddRange(r);
                type = "Version";
                r[0] = b.Version;
                r[1] = "";
                listView1.Items.Add(type).SubItems.AddRange(r);
                type = "Release Date";
                //r[0] = FormatDate( b.Date);
                r[0] = b.Date;
                r[1] = "";
                listView1.Items.Add(type).SubItems.AddRange(r);
            }

            foreach (ListViewItem LVI in this.listView1.Items)
            {
                LVI.ForeColor = Color.Blue;
                LVI.UseItemStyleForSubItems = false;
                LVI.SubItems[0].ForeColor = Color.Black;
                //LVI.SubItems[0].BackColor = Color.LightSteelBlue;
                LVI.SubItems[1].ForeColor = Color.Navy;
                //LVI.SubItems[1].BackColor = Color.LightSteelBlue;
                LVI.SubItems[2].ForeColor = Color.Blue;
                //LVI.SubItems[2].BackColor = Color.LightSteelBlue;
            }
            this.listView1.Columns[1].Width = -1;
            this.listView1.Columns[2].Width = listView1.Width - (listView1.Columns[0].Width + listView1.Columns[1].Width+25);
        
        }
        // ABOUT BOX
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            abt.UseCustomLicense(Temperature.Properties.Resources.NETCHECKLICENSE);
            Bitmap myBitmap = Temperature.Properties.Resources.THERMOMETER;
            MemoryStream ms = new MemoryStream();
            myBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            abt.Progam = "Temperature";
            abt.Version = "1.0.0.9";
            abt.Copyright = "HC Williams (freeware)";
            abt.Build = "July 2016";
            abt.Compiler = "Visual C# 2015";
            abt.AboutBoxImage = ms;
            abt.DisplayAboutBox();
            if (ms != null)
            {
                ms.Dispose(); 
            }
            


        }
        // COPY LISTVIEW TO CLIPBOARD
        // CREDITS: http://dotnetref.blogspot.com/2007/06/copy-listview-to-clipboard.html
        public void CopyListViewToClipboard(ListView lv)
        {
            int pixelstochars = 5;
            string s = string.Empty;
            int[] pad = new int[3];
            pad[0] = (lv.Columns[0].Width)/pixelstochars;
            pad[1] = (lv.Columns[1].Width)/(pixelstochars-2);
            pad[2] = (lv.Columns[2].Width)/(pixelstochars+2);
            StringBuilder buffer = new StringBuilder();

            for (int i = 0; i < lv.Columns.Count; i++)
            {
                buffer.Append(lv.Columns[i].Text.PadRight(pad[i]));
                buffer.Append("\t");
                
            }
           
            buffer.Append("\n\n");

            for (int i = 0; i < lv.Items.Count; i++)
            {
                for (int j = 0; j < lv.Columns.Count; j++)
                {
                    // s = lv.Items[i].SubItems[j].Text.PadRight(pad[j]);
                    buffer.Append(lv.Items[i].SubItems[j].Text.PadRight(pad[j]));
                    buffer.Append("\t");
                }

                buffer.Append("\n");
            }

            Clipboard.SetText(buffer.ToString());
        }
        // CLIPBOARD PICTURE CLICK
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
            CopyListViewToClipboard(this.listView1);
            dt.NotifyDialog(this, "Information Copied to Cliboard");
        }
        // INFOICON PICTURE CLICK
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            aboutToolStripMenuItem_Click(sender, EventArgs.Empty);
        }

        
    }
}
