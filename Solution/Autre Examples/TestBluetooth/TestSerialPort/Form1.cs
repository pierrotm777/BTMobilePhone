using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace TestSerialPort
{
    public partial class Form1 : Form
    {
        public delegate void tsAnswerDelegate(TextBox lbl, string text);

        private BluetoothRadio mRadio = null;
        private BluetoothClient mBC = null;
        private Dictionary<string,  BluetoothDeviceInfo> mdicDevices = new Dictionary<string,BluetoothDeviceInfo>();
        private System.Net.Sockets.NetworkStream mstreamConnection = null;
        private System.IO.StreamWriter mWriter = null;
        private bool mbConnected = false;
        private bool mbConnecting = false;
        private Thread mthReader = null;

        public Form1()
        {
            InitializeComponent();
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = false;
            btnSend.Enabled = false;
            btnSearchDevices.Enabled = false;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Disconnect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mRadio = BluetoothRadio.PrimaryRadio;
            if (mRadio == null)
            {
                txtAnswer.Text = "No Bluetooth device installed!\n";
            }
            else
            {
                txtAnswer.Text = "\n-- local device info --";
                BluetoothAddress Addr = mRadio.LocalAddress;
                BluetoothDeviceInfo DevInfo = new BluetoothDeviceInfo(Addr);
                txtAnswer.Text += "\nName: " + DevInfo.DeviceName;
                txtAnswer.Text += "\nAddress: " + Addr.ToString();
                btnSearchDevices.Enabled = true;
            }
        }

        private void btnSearchDevices_Click(object sender, EventArgs e)
        {
            listDevices.Items.Clear();
            btnConnect.Enabled = false;
            txtAnswer.Text = "Searching authenticated devices...";
            mBC = new BluetoothClient();
            BluetoothDeviceInfo[] lstDevices = mBC.DiscoverDevices(10, true, false, false);
            txtAnswer.Text = "Found " + lstDevices.Length + " devices:";
            foreach (BluetoothDeviceInfo devInfo in lstDevices)
            {
                txtAnswer.Text += "Name: " + devInfo.DeviceName;
                txtAnswer.Text += "\r\n";
                txtAnswer.Text += "Address: " + devInfo.DeviceAddress.ToString();
                txtAnswer.Text += "\r\n";
                txtAnswer.Text += "Device: " + devInfo.ClassOfDevice.Device.ToString();
                txtAnswer.Text += "\r\n";
                txtAnswer.Text += "Connected: " + (devInfo.Connected == true ? "yes" : "no");
                txtAnswer.Text += "\r\n";
                txtAnswer.Text += "Remembered: " + (devInfo.Remembered == true ? "yes" : "no");
                txtAnswer.Text += "\r\n**********************************************\r\n";
                mdicDevices.Add(devInfo.DeviceName, devInfo);
                listDevices.Items.Add(devInfo.DeviceName);
                btnConnect.Enabled = true;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (mWriter != null || mWriter.BaseStream != null || mWriter.BaseStream.CanWrite)
            {
                try
                {
                    mWriter.Write(txtSend.Text + '\r');
                    mWriter.Flush();
                }
                catch (Exception ex)
                {
                    tsAnswer(txtAnswer, "Send error: " + ex.Message);
                }
            }
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            BluetoothDeviceInfo devInfo = null;
            if (mdicDevices.TryGetValue(listDevices.Text, out devInfo))
            {
                try
                {
                    mbConnecting = true;
                    btnDisconnect.Enabled = true;
                    btnConnect.Enabled = false;
                    btnSend.Enabled = true;
                    txtAnswer.Text = "Connecting...";
                    AsyncCallback cb = new AsyncCallback(ConnectCallback);
                    mBC.BeginConnect(devInfo.DeviceAddress, BluetoothService.SerialPort, cb, null); 
                }
                catch (Exception ex)
                {
                    txtAnswer.Text = "Error connecting to device: " + ex.Message;
                }
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            if (mbConnecting || mbConnected)
            {
                if (mthReader != null) mthReader.Abort();
                if (mWriter != null) mWriter.Close();
                mBC.Close();
                mBC = new BluetoothClient();
                mbConnected = false;
                mbConnecting = false;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnSend.Enabled = false;
                tsAnswer(txtAnswer, "Disconnected!");
            }
        }

        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                mBC.EndConnect(result);  
                ConnectingDone();
            }
            catch (Exception ex)
            {
                tsAnswer(txtAnswer, "Connection failed: " + ex.Message);
            }
                
        }
        private void tsAnswer(TextBox lbl, string text)
        {
            if (lbl.InvokeRequired)
            {
                tsAnswerDelegate dlg = new tsAnswerDelegate(tsAnswer);
                lbl.Invoke(dlg, new object[] { lbl, text });
            }
            else
                lbl.Text = text;
        }
        private void ConnectingDone()
        {
            mbConnected = true;
            tsAnswer(txtAnswer, "Connected!");//+ Environment.NewLine);
            mstreamConnection = mBC.GetStream();
            mWriter = new StreamWriter(mstreamConnection, Encoding.UTF8);
            mthReader = new Thread(ReceiveMessage);
            mthReader.Start();
        }

        private void ReceiveMessage()
        {
            if (!mBC.Connected || mstreamConnection == null || !mstreamConnection.CanRead )
            {
                tsAnswer(txtAnswer, "Messagequeue could not established!");
            }
            else
            {
                string str = "";// "received:\n";
                StreamReader rd = new StreamReader(mstreamConnection, Encoding.UTF8);
                StringBuilder sbLine = new StringBuilder();
                try
                {
                    //char[] buffer = new char[1000];
                    do
                    {
                        //int iBytes = rd.Read(buffer, 0, buffer.Length);
                        //str += new String(buffer, 0, iBytes);
                        //tsAnswer(txtAnswer, str);
                        int iVal = rd.Read();
                        switch (iVal)
                        {
                            case 10: //new line
                                str += sbLine.ToString() + Environment.NewLine;//"\n";
                                tsAnswer(txtAnswer, str);
                                sbLine = new StringBuilder();
                                break;
                            case 13: //carriage return - ignore
                                break;
                            case -1:
                                break;
                            default: //build string message
                                sbLine.Append(char.ConvertFromUtf32(iVal));
                                break;
                        }
                    }
                    while (true);
                }
                catch (IOException ex)
                {
                    tsAnswer(txtAnswer, "!! IOException: " + ex.Message);
                }
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtAnswer.Text="";
        }

 
    }
}
