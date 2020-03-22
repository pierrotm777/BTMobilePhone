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
using System.Xml.Serialization;
using cPOSdotNet.Core.RemoteControl;

namespace TestRemoteControl
{
    public partial class Form1 : Form
    {
        public delegate void tsAnswerDelegate(TextBox lbl, string text);

        private BluetoothRadio mRadio = null;
        private bool mbConnected = false;
        //private static Guid mServiceName = new Guid("{A175D486-E23D-4887-8AF5-DAA1F6A5B172}");
        private static Guid mServiceName = new Guid("{C5D52389-C64B-4a17-ACC8-20961C73A871}");
        private BluetoothListener mServiceListener = null;
        private Thread mthServiceListener = null;
        private BluetoothClient mBC = null;
        private Thread mthReader = null;
        private StreamReader mReader = null;
        private StreamWriter mWriter = null;
        private XmlSerializer mSer = new XmlSerializer(typeof(cPOSMessage));

        public Form1()
        {
            InitializeComponent();
            btnDisconnect.Enabled = false;
            btnEnableService.Enabled = false;
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
                btnEnableService.Enabled = true;                
            }
        }

        private void btnEnableService_Click(object sender, EventArgs e)
        {
            mbConnected = true;
            tsAnswer(txtAnswer, "Start enable service " + mServiceName.ToString() + "...");
            btnDisconnect.Enabled = true;
            btnEnableService.Enabled = false;
            //  set up a listener for client connections
            mthServiceListener = new Thread(ServiceListener);
            mthServiceListener.Start();
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            try
            {
                if (mReader != null) mReader.Close();
                if (mWriter != null) mWriter.Close();
                if (mServiceListener != null) mServiceListener.Stop();
                mBC = null;
                mbConnected = false;
                btnEnableService.Enabled = true;
                btnDisconnect.Enabled = false;
                if (mthReader != null) mthReader.Abort();
                if (mthServiceListener != null) mthServiceListener.Abort();
                tsAnswer(txtAnswer, "Disconnected!");
            }
            catch (Exception ex)
            {
                tsAnswer(txtAnswer, "Failed on disconnect! " + ex.Message);
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

        private void ServiceListener()
        {
            mServiceListener = new BluetoothListener(mServiceName);

            mServiceListener.Start();
            mRadio.Mode = RadioMode.Discoverable;
            tsAnswer(txtAnswer, "Waiting for a client to connect to service ...");
            try
            {
                // blocking
                mBC = mServiceListener.AcceptBluetoothClient();
                tsAnswer(txtAnswer, "Connection established on service...");
                mbConnected = true;
                mReader = new StreamReader(mBC.GetStream(), Encoding.UTF8);
                mWriter = new StreamWriter(mBC.GetStream(), Encoding.UTF8);
                // Ackknowledge
                SendMessage(ServerMessages.SM_ACKNOWLEDGEOK);
                // set up a reader
                mthReader = new Thread(ServiceReader);
                mthReader.Start();
            }
            catch (Exception ex)
            {
                tsAnswer(txtAnswer, "ServiceListener error: " + ex.Message);
            }
        }
        private void ServiceReader()
        {
            try
            {
                string sMessage = "";

                StringBuilder sbLine = new StringBuilder();
                while (sMessage != ClientMessages.CM_DISCONNECT && !mReader.EndOfStream)
                {
                    int iVal = mReader.Read();
                    switch (iVal)
                    {
                        case 10: //new line
                            sMessage = sbLine.ToString() + "\n";
                            tsAnswer(txtAnswer, "Service: Message received: " + sMessage);
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
                mReader.Close();
            }
            catch (Exception ex)
            {
                tsAnswer(txtAnswer, "ServiceReader error: " + ex.Message);
            }
        }
        private void SendMessage(string msg)
        {
            if (mBC != null && mBC.Connected && 
                mWriter != null && mWriter.BaseStream != null && mWriter.BaseStream.CanWrite)
            {
                try
                {
                    //mSer.Serialize(mWriter.BaseStream, msg);
                    //mWriter.Write('\n');
                    mWriter.Write(msg + '\n');
                    mWriter.Flush();
                }
                catch (InvalidOperationException ex)
                {
                    tsAnswer(txtAnswer, "Send message error: : " + ex.Message);
                }

            }
        }

        private void btnTitel_Click(object sender, EventArgs e)
        {
            string msg = "music|titel|Linkin Park - Hands held high.mp3";
            SendMessage(msg);
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            string msg = "Music|list|D@c:\\~" +
                         "F@Music~T@Linkin Park - Sould we.mp3~" +
                         "T@Bob Marley - No woman no cry.mp3~" +
                         "T@Rammstein - Rammlied.mp3~" +
                         "T@Hottie and the blowfish - Just a song with a long titel.mp3~" +
                         "T@Linkin Park - What I've done.mp3~" +
                         "T@Rammstein - Ich tu Dir weh.mp3";
            SendMessage(msg);
        }

        private void bntQuit_Click(object sender, EventArgs e)
        {
            SendMessage(ServerMessages.SM_DISCONNECT);
        }
    }
}
