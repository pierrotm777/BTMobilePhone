using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Brecham.Obex;
using Brecham.Obex.Objects;

namespace TestOBEXParser
{
    public partial class Form1 : Form
    {
        public delegate void tsAnswerDelegate(TextBox lbl, string text);
        public delegate void tsBrowserDelegate(ListView list, ListViewItem item);
        public delegate void tsBrowserClearDelegate(ListView list);

        private BluetoothRadio mRadio = null;
        private BluetoothClient mBC = null;
        private Dictionary<string, BluetoothDeviceInfo> mdicDevices = new Dictionary<string, BluetoothDeviceInfo>();
        private bool mbConnected = false;
        private bool mbConnecting = false;
        private ObexClientSession mSession = null;
        private ObexFileItem mDownloadFile = null;

        public Form1()
        {
            InitializeComponent();
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = false;
            btnSearchDevices.Enabled = false;
            btnLoad.Enabled = false;
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
            txtAnswer.Text += "\r\n**********************************************\r\n";
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
                    txtAnswer.Text = "Connecting...";
                    AsyncCallback cb = new AsyncCallback(ConnectCallback);
                    mBC.BeginConnect(devInfo.DeviceAddress, BluetoothService.ObexFileTransfer, cb, null);
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
                tsBrowserClear(lstBrowser);
                if (mSession != null) mSession.Disconnect();
                mBC.Close();
                mBC = new BluetoothClient();
                mbConnected = false;
                mbConnecting = false;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnLoad.Enabled = false;
                btnUp.Enabled = false;
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
        private void tsBrowser(ListView list, ListViewItem item)
        {
            if (list.InvokeRequired)
            {
                tsBrowserDelegate dlg = new tsBrowserDelegate(tsBrowser);
                list.Invoke(dlg, new object[] { list, item });
            }
            else
                list.Items.Add(item);
        }
        private void tsBrowserClear(ListView list)
        {
            if (list.InvokeRequired)
            {
                tsBrowserClearDelegate dlg = new tsBrowserClearDelegate(tsBrowserClear);
                list.Invoke(dlg, new object[] { list });
            }
            else
                list.Items.Clear();
        }
        private void ConnectingDone()
        {
            mbConnected = true;
            tsAnswer(txtAnswer, "Connected!");
            // Set up connection requrements
            mSession = new ObexClientSession(mBC.GetStream(), UInt16.MaxValue);
            mSession.Connect(ObexConstant.Target.FolderBrowsing);
            BrowseRemoteDevice();
        }
        private void BrowseRemoteDevice()
        {
            // responce stream holen
            ObexGetStream reader = mSession.Get(null, ObexConstant.Type.FolderListing);
            // an den folder parser übergeben
            ObexFolderListingParser parser = new ObexFolderListingParser(reader);
            parser.IgnoreUnknownAttributeNames = true;
            //ObexFolderListing listener = parser.GetAllItems();
            //listener.Files;
            //listener.Files;
            
            ObexFolderListingItem item = null;

            // durch den remote folder iterieren
            while ((item = parser.GetNextItem()) != null)
            {
                // keine Vaterknoten lesen
                if (item is ObexParentFolderItem)
                    continue;

                btnUp.Enabled = true;
                ObexFileOrFolderItem innerItem = item as ObexFileOrFolderItem;
                

                ListViewItem lst = new ListViewItem(new string[] {
                        innerItem.Name,
                        innerItem.Size.ToString() + "|" ,
                        innerItem.Modified.ToShortTimeString() + "|" ,
                        innerItem.Accessed.ToShortTimeString() + "|" ,
                        innerItem.Created.ToShortTimeString() + "|" ,
                        innerItem.Type});
                lst.Tag = innerItem;
                lst.Name = innerItem.Name;
                tsBrowser(lstBrowser, lst);
            }
        }

        private void BrowseFolder(string sPath)
        {
            try
            {
                //Set path on the device
                mSession.SetPath(sPath);
            }
            catch (Exception ex)
            {
                tsAnswer(txtAnswer, "Browse folder failed: " + ex.Message);
                return;
            }
            tsBrowserClear(lstBrowser);
            BrowseRemoteDevice();
        }
        private void lstBrowser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListViewItem item = lstBrowser.HitTest(e.Location).Item;
                if (item != null)
                {
                    ObexFileOrFolderItem innerItem = item.Tag as ObexFileOrFolderItem;
                    bool isFolder = innerItem is ObexFolderItem;
                    if (isFolder)
                    {
                        btnUp.Enabled = true;
                        btnLoad.Enabled = false;
                        BrowseFolder(item.Text);
                    }
                    else
                    {
                        btnLoad.Enabled = true;
                        mDownloadFile = innerItem as ObexFileItem;
                    }
                }            
            }
            catch (Exception ex)
            {
                tsAnswer(txtAnswer, "Browse folder failed: " + ex.Message);
                return;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                //Set path to parent folder.
                mSession.SetPathUp();
            }
            catch (Exception ex)
            {
                tsAnswer(txtAnswer, "Up failed: " + ex.Message);
                return;
            }
            btnLoad.Enabled = false;
            tsBrowserClear(lstBrowser);
            BrowseRemoteDevice(); 
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            long lProgress = 0;
            if (mDownloadFile == null) return;

            string filename = mDownloadFile.Name;

            //Stream on our file system. We will need to either read from it or write to it.
            FileStream outStream = new FileStream(
                Environment.SpecialFolder.ApplicationData + filename, FileMode.Create, FileAccess.Write, FileShare.None);

            AbortableStream inRemoteStream = null;
            try
            {
                //Stream on our device. We will need to either read from it or write to it.
                inRemoteStream = (AbortableStream)mSession.Get(filename, null);
            }
            catch (Exception ex)
            {
                tsAnswer(txtAnswer, "Load file failed: " + ex.Message);
                return;
            }
            tsAnswer(txtAnswer, "Load file: " + filename + "...");
            //This is the function that does actual reading/writing.
            long result = LoadFileFromRemote(inRemoteStream, outStream, lProgress, filename);
            lProgress = result;
            tsAnswer(txtAnswer, "Done: " + filename + " Size : " + lProgress.ToString() + " bytes");
            outStream.Close();
            inRemoteStream.Close();
        }
        private long LoadFileFromRemote(Stream source, Stream destination, long progress,
                            string filename)
        {
            //Allocate buffer
            byte[] buffer = new byte[1024 * 4];
            while (true)
            {
                try
                {
                    //Read from source and write to destination.
                    //Break if finished reading. Count read bytes.
                    int length = source.Read(buffer, 0, buffer.Length);
                    if (length == 0) break;
                    destination.Write(buffer, 0, length);
                    progress += length;
                }
                //Return 0 as if operation was cancelled so that processedFiles is set.
                catch (IOException ex)
                {
                    tsAnswer(txtAnswer, "ProgressStreams IO failed: " + ex.Message);
                    return 0;
                }
                catch (ObexResponseException ex)
                {
                    tsAnswer(txtAnswer, "ProgressStreams Responce failed: " + ex.Message);
                    return 0;
                }
            }
            return progress;
        }
    }
}

