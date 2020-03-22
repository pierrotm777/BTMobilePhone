using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Runtime.Serialization;
using System.IO;

namespace MobileXorEncDec
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            cmbPort.Items.Clear();
            string[] lPorts = SerialPort.GetPortNames();
            foreach (string str in lPorts)
            {
                cmbPort.Items.Add(str);
            }
            if (cmbPort.Items.Count > 0)
                cmbPort.SelectedIndex = 0;

        }


        public byte[] ReadFileToByteArray(string fileName, int bc, long offset)
        {
            try
            {
                byte[] buff = null;
                FileStream fs = new FileStream(fileName,
                                               FileMode.Open,
                                               FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                br.BaseStream.Position = offset;
                buff = br.ReadBytes(bc);
                fs.Close();
                return buff;
            }
            catch
            {
                return null;
            }
        }
        public bool WriteBytesToFile(byte[] buff, string fileName, int bc, long offset)
        {
            try
            {
                FileStream fs = new FileStream(fileName,
                                               FileMode.Open,
                                               FileAccess.Write);
                BinaryWriter br = new BinaryWriter(fs);
                br.BaseStream.Position = offset;
                br.Write(buff, 0, bc);
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btnEncDec_Click(object sender, EventArgs e)
        {
            if (cmbPort.Text != "" && txtSource.Text != "" && txtDistenation.Text != "")
            {
                string Key = GetMobileSerialNumber(cmbPort.Text);
                if (Key != "")
                {
                    try
                    {
                        Stream stream = new FileStream(txtDistenation.Text, FileMode.Create, FileAccess.Write, FileShare.Read);
                        stream.Close();

                        stream = new FileStream(txtSource.Text, FileMode.Open, FileAccess.Read, FileShare.Read);
                        long len = stream.Length;
                        stream.Close();

                        if (len>=1)
                        {
                            try
                            {
                                byte[] buffer;
                                long Used=0;
                                int Mb=(int) Math.Pow(2,20);
                                while (Used < len)
                                {
                                    int Itr = 0;
                                    if (Used + Mb < len)
                                    {
                                        Itr = Mb;
                                    }
                                    else
                                        Itr = (int)(len-Used);
                                    if (Itr >= 1)
                                    {
                                        buffer = ReadFileToByteArray(txtSource.Text, Itr, Used);
                                        for (int i = 0; i < Itr; i++)
                                        {
                                            buffer[i] = (byte)(buffer[i] ^ Key[i%Key.Length]);
                                        }
                                        WriteBytesToFile(buffer, txtDistenation.Text, Itr, Used);
                                        Used += Itr;
                                    }
                                    else
                                        break;
                                }
                                MessageBox.Show("Operation Completed!");
                            }
                            catch
                            {
                                MessageBox.Show("Error!");
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error!");
                    }
                }
            }
            else
                MessageBox.Show("Please Fill Information");
        }
        private string GetMobileSerialNumber(string PortName)
        {
            string key = "";
            SerialPort serialPort = new SerialPort();
            serialPort.PortName = PortName;
            serialPort.BaudRate = 56700;

            try
            {
                if (!(serialPort.IsOpen))
                    serialPort.Open();
                serialPort.Write("AT\r\n");
                Thread.Sleep(3000);
                key = serialPort.ReadExisting();
                serialPort.Write("AT+CGSN\r\n");
                Thread.Sleep(3000);
                key = serialPort.ReadExisting();


                serialPort.Close();
                string Serial = "";
                for (int i = 0; i < key.Length; i++)
                    if (char.IsDigit(key[i]))
                        Serial += key[i];
                return Serial;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in opening/writing to serial port :: " + ex.Message, "Error!");
                return "";
            } 

        }

        private void btnSelectFixFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Multiselect = false;
            if (opf.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = "";
                if (opf.FileName != "")
                {
                    txtSource.Text = opf.FileName;
                }
            }
        }

        private void SaveFixFilePath_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.OverwritePrompt = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                txtDistenation.Text = "";
                if (sfd.FileName != "")
                {
                    txtDistenation.Text = sfd.FileName;
                }
            }

        }

        private void btnRefreshports_Click(object sender, EventArgs e)
        {
            cmbPort.Items.Clear();
            string[] lPorts = SerialPort.GetPortNames();
            foreach (string str in lPorts)
            {
                cmbPort.Items.Add(str);
            }
            if (cmbPort.Items.Count > 0)
                cmbPort.SelectedIndex = 0;

        }
    }
}