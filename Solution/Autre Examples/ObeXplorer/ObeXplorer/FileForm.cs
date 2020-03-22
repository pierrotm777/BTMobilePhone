using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Brecham.Obex;

namespace ObeXplorer
{
	public partial class FileForm : Form
	{
		#region Fields

		public ExceptionDelegate ExceptionMethod;

		ObexClientSession currentSession;
		string dir;
		bool download;
		bool exceptionoccured = false;
		int filesProcessed = 0;
		List<string> filesToProcess;
		long totalsize;

		#endregion Fields

		#region Constructors

		public FileForm(List<string> files, bool down, ObexClientSession session, long size, string directory)
		{
			InitializeComponent();

			filesToProcess = files;
			download = down;
			currentSession = session;
			totalsize = size;
			dir = directory;
		}

		#endregion Constructors

		#region Public Properties

		public int FilesUploaded
		{
			get
			{
			    return filesProcessed;
			}
		}

		#endregion Public Properties

		#region Private Methods

		private void FileForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (bgwWorker.CancellationPending)
			{
			    e.Cancel = true;
			    return;
			}

			if (bgwWorker.IsBusy)
			{
			    if (MessageBox.Show("Cancel operation?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			    {
			        bgwWorker.CancelAsync();
			        lblFileCount.Text = "Canceling...";

			        while (bgwWorker.IsBusy)
			        {
			            System.Threading.Thread.Sleep(2000);
			        }
			    }
			    else
			        e.Cancel = true;
			}
		}

		private void FileForm_Shown(object sender, EventArgs e)
		{
			if (download)
			{
			    lblFileCount.Text = string.Format("Downloading File 1 of {0}", filesToProcess.Count);
			    lblCurrentFile.Text = string.Format("Downloading {0}", filesToProcess[0]);

			    this.Text = "Downloading...";
			}
			else
			{
			    lblFileCount.Text = string.Format("Uploading File 1 of {0}", filesToProcess.Count);
			    lblCurrentFile.Text = string.Format("Uploading {0}", filesToProcess[0]);

			    this.Text = "Uploading...";
			}

			bgwWorker.RunWorkerAsync();
		}

		
        private long ProcessStreams(Stream source, Stream destination, long progress, string filename)
		{
			byte[] buffer = new byte[1024 * 4];
			while (true)
			{
			    //Report downloaded file size
			    bgwWorker.ReportProgress((int)(((progress * 100) / totalsize)), progress);

			    if (bgwWorker.CancellationPending)
			    {
			        currentSession.Abort();
			        return 0;
			    }

			    try
			    {
			        int length = source.Read(buffer, 0, buffer.Length);
			        if (length == 0) break;
			        destination.Write(buffer, 0, length);
			        progress += length;
			    }
			    //Return 0 as if operation was canceled so that processedFiles is set.    
			    catch (IOException ex)
			    {
			        exceptionoccured = true;
			        ExceptionMethod(ex);
			        return 0;
			    }
			    catch (ObexResponseException ex)
			    {
			        exceptionoccured = true;
			        ExceptionMethod(ex);
			        return 0;
			    }
			}
			return progress;
		}

		
        private void bgwWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			long progress = 0;
			DateTime start = DateTime.Now;

			for (int i = 0; i < filesToProcess.Count; i++)
			{
			    string currentfile = filesToProcess[i];

			    //Report that we started downloading new file
			    bgwWorker.ReportProgress((int)(((progress * 100) / totalsize)), i + 1);

			    string filename = download ? Path.Combine(dir, currentfile) : currentfile;

			    FileStream hoststream = download ? new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)
			                               : new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);

			    AbortableStream remotestream = null;
			    try
			    {
			        remotestream = download ? (AbortableStream)currentSession.Get(currentfile, null)
			                                : (AbortableStream)currentSession.Put(Path.GetFileName(currentfile), null);
			    }
			    catch (IOException ex)
			    {
			        exceptionoccured = true;
			        ExceptionMethod(ex);
			        return;
			    }
			    catch (ObexResponseException ex)
			    {
			        exceptionoccured = true;
			        ExceptionMethod(ex);
			        return;
			    }
			    using (hoststream)
			    {
			        using (remotestream)
			        {
			            long result = download ? ProcessStreams(remotestream, hoststream, progress, currentfile)
			                : ProcessStreams(hoststream, remotestream, progress, currentfile);

                        //Even if we are canceled we need to report how many files we have already uploaded
                        //so that they are added to the listview. Or if it is download we need to delete the 
                        //partially downloaded last file.
                        filesProcessed = i;

			            if (result == 0)
			            {
			                e.Cancel = true;
			                return;
			            }
			            else
			                progress = result;
			        }
			    }
			}
			DateTime end = DateTime.Now;
			e.Result = end - start;
		}

		private void bgwWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			prbProgress.Value = e.ProgressPercentage;

			if (download)
			{
			    if (e.UserState is long)
			    {
			        lblSize.Text = string.Format("Downloaded {0} of {1} bytes", (long)e.UserState, totalsize);
			    }
			    else
			    {
			        lblFileCount.Text = string.Format("Downloading File {0} of {1}", (int)e.UserState, filesToProcess.Count);
			        lblCurrentFile.Text = string.Format("Downloading {0}", filesToProcess[(int)e.UserState - 1]);
			    }
			}
			else
			{
			    if (e.UserState is long)
			    {
			        lblSize.Text = string.Format("Uploaded {0} of {1} bytes", (long)e.UserState, totalsize);
			    }
			    else
			    {
			        lblFileCount.Text = string.Format("Uploading File {0} of {1}", (int)e.UserState, filesToProcess.Count);
			        lblCurrentFile.Text = string.Format("Uploading {0}", filesToProcess[(int)e.UserState - 1]);
			    }
			}
		}

		private void bgwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (exceptionoccured)
			{
			    if (download) File.Delete(Path.Combine(dir, filesToProcess[filesProcessed]));
			    this.Close();
			    return;
			}

			if (e.Cancelled)
			{
			    if (download) File.Delete(Path.Combine(dir, filesToProcess[filesProcessed]));
			    
			    lblFileCount.Text = "Canceled";
			    lblCurrentFile.Visible = false;
			    
			    btnCancel.Text = "Canceled";
			    this.Text = "Canceled";
			}
			else
			{
			    TimeSpan elapsed = (TimeSpan)e.Result;
			    
			    lblFileCount.Text = "Done";
			    lblCurrentFile.Text = string.Format("Download time: {0} hours {1} minutes {2} seconds. Average speed: {3} KB/sec",
			                                                            elapsed.Hours,
			                                                            elapsed.Minutes,
			                                                            elapsed.Seconds,
			                                                            Math.Round(totalsize / elapsed.TotalSeconds/1024,2));
			    prbProgress.Value = 100;
			    btnCancel.Text = "Done";
			    this.Text = "Done";
			}
		}

		
        private void btnCancel_Click(object sender, EventArgs e)
		{
			if (bgwWorker.CancellationPending) return;

			if (bgwWorker.IsBusy)
			{
			    bgwWorker.CancelAsync();
			    this.Text = lblFileCount.Text = "Canceling...";
			}
			else
			    this.Close();
		}

		#endregion Private Methods

		#region Other

		public delegate void ExceptionDelegate(Exception ex);

		#endregion Other
	}
}