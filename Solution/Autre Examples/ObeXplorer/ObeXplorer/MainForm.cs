using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Windows.Forms;

using Brecham.Obex;
using Brecham.Obex.Objects;

using IconHandler;
using DragAndDropFileControlLibrary;

namespace ObeXplorer
{
	public partial class MainForm : Form
	{
		#region Fields

		BluetoothClient client;
		Stack<ListViewItem[]> previousItemsStack = new Stack<ListViewItem[]>();
		ObexClientSession session;

		#endregion Fields

		#region Constructors

		public MainForm()
		{
			InitializeComponent();
            this.Icon = Properties.Resources.BL;
		}

		#endregion Constructors

		#region Private Methods

		private void Connect()
		{
            using (SelectBluetoothDeviceDialog bldialog = new SelectBluetoothDeviceDialog())
            {
                bldialog.ShowAuthenticated = true;
                bldialog.ShowRemembered = true;
                bldialog.ShowUnknown = true;

                if (bldialog.ShowDialog() == DialogResult.OK)
                {
                    SetControlState(false);

                    tsStatusLabel.Text = "Operation started";

                    if (bldialog.SelectedDevice == null)
                    {
                        MessageBox.Show("No device selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    BluetoothDeviceInfo selecteddevice = bldialog.SelectedDevice;
                    BluetoothEndPoint remoteEndPoint = new BluetoothEndPoint(selecteddevice.DeviceAddress,
                                                                       BluetoothService.ObexFileTransfer);

                    client = new BluetoothClient();
                    try
                    {
                        client.Connect(remoteEndPoint);
                        session = new ObexClientSession(client.GetStream(), UInt16.MaxValue);
                        session.Connect(ObexConstant.Target.FolderBrowsing);
                    }
                    catch (SocketException ex)
                    {
                        ExceptionHandler(ex, false);
                        return;
                    }
                    catch (ObjectDisposedException ex)
                    {
                        ExceptionHandler(ex, false);
                        return;
                    }
                    catch (IOException ex)
                    {
                        ExceptionHandler(ex, false);
                        return;
                    }

                    tsStatusLabel.Text = string.Format("Connected to: {0}", selecteddevice.DeviceName);
                    this.Text = string.Format("Bluetooth Device Browser. Current Device: {0}", selecteddevice.DeviceName);

                    tsiConnect.Enabled = tsbConnect.Enabled = false;
                    bgwWorker.RunWorkerAsync();
                }
            }
		}

		private void CreateNewFolder()
		{
            ListViewItem newitem = new ListViewItem("", 1);
			lsvExplorer.Items.Add(newitem);
			lsvExplorer.LabelEdit = true;
			newitem.BeginEdit();
		}

		private void DeleteSelectedItems()
		{
			if (MessageBox.Show("Do you really want to delete selected items?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			{
			    lsvExplorer.BeginUpdate();

			    SetControlState(false);
			    foreach (ListViewItem item in lsvExplorer.SelectedItems)
			    {
			        try
			        {
			            session.Delete(item.Text);
			        }
			        catch (IOException ex)
			        {
			            ExceptionHandler(ex);
			            return;
			        }
			        item.Remove();
			    }

			    lsvExplorer.EndUpdate();
			    SetControlState(true);
			}
		}

		private void DownloadFiles()
		{
			List<string> items = new List<string>(lsvExplorer.SelectedItems.Count);
			long totalsize = 0;

			foreach (ListViewItem item in lsvExplorer.SelectedItems)
			{
			    if ((bool)item.Tag) continue;

			    items.Add(item.Text);

			    string[] sizeparts = item.SubItems[1].Text.Split(' ');
			    int power = sizeparts[1].Contains("G") ? 3 : (sizeparts[1].Contains("M") ? 2 :
			                                                    (sizeparts[1].Contains("K") ? 1 : 0));

			    totalsize += (long)(double.Parse(sizeparts[0]) * Math.Pow(1024, (double)power));
			}

			if (totalsize > 0 && fbdChooseFolder.ShowDialog() == DialogResult.OK)
			{
			    using (FileForm downform = new FileForm(items, true, session, totalsize, fbdChooseFolder.SelectedPath))
			    {
			        downform.ExceptionMethod = ExceptionHandler;
			        downform.ShowDialog();
			    }
			}
		}

		private void ExceptionHandler(Exception ex)
		{
			ExceptionHandler(ex, true);
		}

		private void ExceptionHandler(Exception ex, bool setControlState)
		{
			MessageBox.Show(string.Format("Connection failed. Error message: {0}", ex.Message),
			                "Error",
			                MessageBoxButtons.OK, MessageBoxIcon.Error);

			if (InvokeRequired)
			    this.Invoke(new EventHandler(delegate
			    {
			        lsvExplorer_SelectedIndexChanged(lsvExplorer, EventArgs.Empty);
			        SetControlState(setControlState);
			    }));
			else
			{
			    SetControlState(setControlState);
			    lsvExplorer_SelectedIndexChanged(lsvExplorer, EventArgs.Empty);
			}
		}

		private string FormatDate(DateTime date)
		{
			if (date == DateTime.MinValue)
			    return "-";

			return date.ToString();
		}

		private string FormatSize(long size, bool isFolder)
		{
			if (isFolder)
			    return "-";

			if (size < 1024)
			{
			    return size.ToString() + " B";
			}

			if (size < 1024 * 1024)
			{
			    return Math.Round((double)size / 1024, 2).ToString() + " KB";
			}

			if (size < 1024 * 1024 * 1024)
			{
			    return Math.Round((double)size / 1024 / 1024, 2).ToString() + " MB";
			}
			else
			{
			    return Math.Round((double)size / 1024 / 1024 / 1024, 2).ToString() + " GB";
			}
		}

        private int GetIconIndex(string extension, bool isFolder)
        {
            if (isFolder)
                return 1;

            if (string.IsNullOrEmpty(extension))
                return 0;

            if (imlSmall.Images.ContainsKey(extension))
                return imlSmall.Images.IndexOfKey(extension);

            Icon small = IconHandler.IconHandler.IconFromExtension(extension, IconSize.Small);
            if (small != null)
                imlSmall.Images.Add(extension, small);

            Icon large = IconHandler.IconHandler.IconFromExtension(extension, IconSize.Large);
            if (large != null)
                imlLarge.Images.Add(extension, large);

            if (small != null & large == null)
                imlLarge.Images.Add(extension, small);

            if (small == null & large != null)
                imlSmall.Images.Add(extension, large);

            int result = small == null & large == null ? 0 : imlSmall.Images.IndexOfKey(extension);

            small.Dispose();
            large.Dispose();

            return result;
        }

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (session != null)
			{
			    try
			    {
			        session.Disconnect();
			        session.Dispose();
			    }
			    catch { }
			}

			if (client != null)
			{
			    client.Close();
			    client.Dispose();
			}
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			SetInitialControls();
			if (!BluetoothRadio.IsSupported)
			{
			    MessageBox.Show("Your bluetooth device is not supported by this program.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			    SetControlState(false);
			}
		}

		private void MoveUp()
		{
			//Check if we are at the topmost folder.
			if (previousItemsStack.Count > 0)
			{
			    SetControlState(false);
			    try
			    {
			        session.SetPathUp();
			    }
			    catch (IOException ex)
			    {
			        ExceptionHandler(ex);
			        return;
			    }

			    lsvExplorer.Items.Clear();
			    lsvExplorer.Items.AddRange(previousItemsStack.Pop());
			    SetControlState(true);
			}
		}

		private void ProcessFolder(string folderName)
		{
			try
			{
			    session.SetPath(folderName);
			}
			catch (IOException ex)
			{
			    ExceptionHandler(ex);
			    return;
			}

			ListViewItem[] previousItems = new ListViewItem[lsvExplorer.Items.Count];
			lsvExplorer.Items.CopyTo(previousItems, 0);
			lsvExplorer.Items.Clear();
			previousItemsStack.Push(previousItems);

            SetControlState(false);
            tsStatusLabel.Text = "Operation started";
			bgwWorker.RunWorkerAsync();
		}

		private void RefreshFolder()
        {
            SetControlState(false);
            tsStatusLabel.Text = "Operation started";
			lsvExplorer.Items.Clear();
			bgwWorker.RunWorkerAsync();
		}

		private void SetControlState(bool enable)
		{
			cmsExplorerContextMenu.Enabled=
			tsiRefresh.Enabled =
			tsiNewFolder.Enabled =
			tsiUpload.Enabled =
			tsiDisconnect.Enabled =
			tsbUp.Enabled =
			tsbRefresh.Enabled =
			tsbDownload.Enabled =
			tsbUpload.Enabled =
			tsbNewFolder.Enabled =
			tsbDelete.Enabled = enable;
		}

		private void SetInitialControls()
		{
			SetControlState(false);
			tsiConnect.Enabled = tsbConnect.Enabled = true;
            tscViewCombo.SelectedIndex = 0;

			tsStatusLabel.Text = "Not connected";
			this.Text = "Bluetooth Device Explorer";

			lsvExplorer.Items.Clear();
			lsvExplorer.EndUpdate();
		}

        private void UploadFiles(string[] files)
        {
            long size = 0;
            List<string> filestoupload = new List<string>();

            foreach (string filename in files)
            {
                if (File.Exists(filename))
                {
                    FileInfo info = new FileInfo(filename);
                    filestoupload.Add(filename);
                    size += info.Length;
                }
            }

            using (FileForm upform = new FileForm(new List<string>(filestoupload),
                                                  false, session, size, null))
            {
                upform.ExceptionMethod = ExceptionHandler;
                upform.ShowDialog();


                lsvExplorer.BeginUpdate();
                for (int i = 0; i <= upform.FilesUploaded; i++)
                {
                    ListViewItem temp = new ListViewItem(new string[]{ Path.GetFileName(filestoupload[i]),
                                                                       FormatSize(new FileInfo(filestoupload[i]).Length, false)},
                                                         GetIconIndex(Path.GetExtension(filestoupload[i]), false));
                    temp.Tag = false;
                    lsvExplorer.Items.Add(temp);
                }
                lsvExplorer.EndUpdate();
            }
        }

		
        private void bgwWorker_DoWork(object sender, DoWorkEventArgs e)
		{
            DateTime old = DateTime.Now;
            TimeSpan dr = TimeSpan.FromMilliseconds(200);

            using (ObexGetStream str = session.Get(null, ObexConstant.Type.FolderListing))
            {
                ObexFolderListingParser parser = new ObexFolderListingParser(str);
                parser.IgnoreUnknownAttributeNames = true;

                ObexFolderListingItem item = null;
                List<ListViewItem> items = new List<ListViewItem>();

                while ((item = parser.GetNextItem()) != null)
                {
                    if (item is ObexParentFolderItem)
                        continue;

                    ObexFileOrFolderItem filefolderitem = item as ObexFileOrFolderItem;

                    bool isfolder = filefolderitem is ObexFolderItem;
                    ListViewItem temp = new ListViewItem(new string[] {filefolderitem.Name,
                                                                       FormatSize(filefolderitem.Size, isfolder),
                                                                       FormatDate(filefolderitem.Modified),
                                                                       FormatDate(filefolderitem.Accessed),
                                                                       FormatDate(filefolderitem.Created)},
                                                         GetIconIndex(Path.GetExtension(filefolderitem.Name), isfolder));

                    temp.Tag = isfolder;
                    temp.Name = filefolderitem.Name;
                    items.Add(temp);

                    if (old.Add(dr) < DateTime.Now)
                    {
                        old = DateTime.Now;
                        bgwWorker.ReportProgress(0, temp.Text);
                    }
                }
                e.Result = items.ToArray();
            }
		}

		private void bgwWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
            tsStatusLabel.Text = e.UserState as string;
		}

		private void bgwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (!e.Cancelled)
			{
			    tsStatusLabel.Text = "Operation completed";

                lsvExplorer.Items.AddRange((ListViewItem[])e.Result);
			    lsvExplorer_SelectedIndexChanged(lsvExplorer, EventArgs.Empty);

			    SetControlState(true);
			}
		}


        private void lsvExplorer_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                lsvExplorer.Items.RemoveAt(e.Item);
                return;
            }
            if (lsvExplorer.Items.ContainsKey(e.Label))
            {
                if (MessageBox.Show(string.Format("There is already a folder called {0}", e.Label),
                                    "Error",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Error) == DialogResult.OK)
                {
                    e.CancelEdit = true;
                    lsvExplorer.Items[e.Item].BeginEdit();
                }
                else
                {
                    lsvExplorer.LabelEdit = false;
                    lsvExplorer.BeginInvoke((MethodInvoker)(() =>
                    {
                        lsvExplorer.Items.RemoveAt(e.Item);
                    }));
                }
            }
            else
            {
                e.CancelEdit = false;
                lsvExplorer.LabelEdit = false;
                lsvExplorer.Items[e.Item].Name = e.Label;

                SetControlState(false);
                try
                {
                    session.SetPath(BackupFirst.DoNot, e.Label, IfFolderDoesNotExist.Create);
                    session.SetPathUp();
                }
                catch (IOException ex)
                {
                    ExceptionHandler(ex);
                    return;
                }
                catch (ObexResponseException ex)
                {
                    ExceptionHandler(ex);
                }
                SetControlState(true);
            }
        }


        private void lsvExplorer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
                MoveUp();

            if (e.KeyCode == Keys.Enter & lsvExplorer.SelectedItems.Count == 1)
                if ((bool)lsvExplorer.SelectedItems[0].Tag)
                    ProcessFolder(lsvExplorer.SelectedItems[0].Text);
                else
                    DownloadFiles();

            if (e.KeyCode == Keys.Delete & lsvExplorer.SelectedItems.Count > 0)
                DeleteSelectedItems();

            if (e.KeyCode == Keys.F5)
                RefreshFolder();

        }

		private void lsvExplorer_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ListViewItem clicked = lsvExplorer.HitTest(e.Location).Item;

			if (clicked != null)
			{
			    if ((bool)clicked.Tag)
			        ProcessFolder(clicked.Text);
			    else
			        DownloadFiles();
			}
		}

        private void lsvExplorer_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsbDownload.Enabled = 
            tsbDelete.Enabled = 
            tsiSave.Enabled = 
            tsiDelete.Enabled = lsvExplorer.SelectedItems.Count > 0;
        }

		
        private void tsbDelete_Click(object sender, EventArgs e)
		{
			DeleteSelectedItems();
		}

		private void tsbDownload_Click(object sender, EventArgs e)
		{
			DownloadFiles();
		}

		private void tsbNewFolder_Click(object sender, EventArgs e)
		{
			CreateNewFolder();
		}

		private void tsbRefresh_Click(object sender, EventArgs e)
		{
			RefreshFolder();
		}

		private void tsbUp_Click(object sender, EventArgs e)
		{
			MoveUp();
		}

		private void tsbUpload_Click(object sender, EventArgs e)
		{
            if (opdOpenDialog.ShowDialog() == DialogResult.OK)
            {
                UploadFiles(opdOpenDialog.FileNames);
            }
		}


        private void tscViewCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lsvExplorer.View = (View)tscViewCombo.SelectedIndex;
        }

        private void tsiConnect_Click(object sender, EventArgs e)
		{
			Connect();
		}

		private void tsiDisconnect_Click(object sender, EventArgs e)
		{
			SetControlState(false);

			try
			{
			    session.Disconnect();
			    session.Dispose();
			}
			catch
			{ }
			session = null;

			client.Close();
			client.Dispose();
			client = null;

			tsStatusLabel.Text = "Not connected";
			this.Text = "Bluetooth Device Browser";

			lsvExplorer.Items.Clear();

			SetInitialControls();
		}

		private void tsiExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

        private void tsiAbout_Click(object sender, EventArgs e)
        {
            using (AboutBox box = new AboutBox())
            {
                box.ShowDialog();
            }
        }        

        private void DragDrop_FileDropped(object sender, FileDroppedEventArgs e)
        {
            UploadFiles(e.Filenames);
        }

        #endregion Private Methods
	}
}