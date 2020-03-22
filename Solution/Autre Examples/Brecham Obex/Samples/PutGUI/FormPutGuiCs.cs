#define DONT_USE_TIMER_TAG

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//
using System.IO;
//
using Brecham.Obex;
//
using System.Net.Sockets;   //e.g. NetworkStream

namespace PutGuiCs
{
    public partial class FormPutGuiCs : Form
    {
        //--------------------------------------------------------------
        // The user cancelled the operation.
        bool m_cancelled;

        //--------------------------------------------------------------
        public FormPutGuiCs()
        {
            InitializeComponent();
        }

        private void FormPutGuiCs_Load(object sender, EventArgs e)
        {
            this.labelStatus.Text = "";
        }

        //--------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                State state = new State();

                //------------------------------------------------------
                // Get the file
                //------------------------------------------------------
                String putName; // = "dummy.txt";
                try {
                    state.m_fileStream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                    putName = Path.GetFileName(openFileDialog1.FileName);
                } catch (IOException ioex) {
                    MessageBox.Show("Failed to open the file: " + ioex.ToString());
                    return;
                } catch (System.Security.SecurityException) {
                    // The main library supports low trust CAS zones, so make some attempt
                    //  to do so here too...
                    state.m_fileStream = openFileDialog1.OpenFile();
                    putName = "file.dat";
                    // ... However, accessing the network will likely require higher 
                    // rights, so we'll fail below, either with
                    // * System.Security.SecurityException: "That assembly does not
                    // allow partially trusted callers.", when loading the 32feet.NET
                    // assembly, or
                    // * System.Security.SecurityException: "Request for the permission
                    // of type 'System.Net.SocketPermission, System, Version=2.0.0.0,
                    // Culture=neutral, PublicKeyToken=b77a5c561934e089' failed.",
                    // when using TCP/IP.
                }
                state.m_progressStream = new ReadProgressStream(state.m_fileStream);
                state.m_progressStream.SetTotalReadLength(state.m_fileStream.Length);

                //------------------------------------------------------
                // Get the peer
                //------------------------------------------------------
                ProtocolFamily pf = this.protocolComboBox1.SelectedProtocol;
                state.m_conn = new Brecham.Obex.Net.GuiObexSessionConnection(pf, false, this.labelStatus);
                try {
                    if (!state.m_conn.Connect()) {
                        //user cancelled the connect
                        return;
                    }
                } catch (Exception ex) {
                    Type typeOfEx = ex.GetType();
                    if (typeof(ObexResponseException) != typeOfEx
                        && typeof(System.Net.ProtocolViolationException) != typeOfEx
                        && typeof(System.IO.IOException) != typeOfEx
                        && typeof(System.Net.Sockets.SocketException) != typeOfEx) {
                        // Not one of the expected exception types, rethrow!
                        throw;
                    }
                    String descr = ex.Message + "\r\n" + ex.GetType().ToString();
                    this.labelStatus.Text = "Connect failed: " + descr;
                    MessageBox.Show(this, descr, "Connect failed");
                    return;
                }

                //------------------------------------------------------
                // Send
                //------------------------------------------------------
                try
                {
                    ObexClientSession sess = state.m_conn.ObexClientSession;
                    //
                    this.labelStatus.Text = "Sending...";
                    this.progressBar1.Visible = true;
                    StartProgressBarUpdater(state);
                    //sess.PutFrom(state.m_progressStream, putName, null, state.m_fileStream.Length);
                    state.m_putCaller = new PutFromNtiCaller(sess.PutFrom);
                    AsyncCallback cb = new AsyncCallback(PutCompleted);
                    state.SetStartTime();
                    state.m_putCaller.BeginInvoke(
                        state.m_progressStream, putName, null, state.m_fileStream.Length,
                        cb, state);

                    // Enable the Cancel button
                    m_cancelled = false;
                    buttonCancel.Enabled = true;
                    buttonCancel.Tag = sess; // Give the button access to the session.
                }
                catch
                {
                    // All OBEX errors occur on the delegate.BeginInvoke's thread, and
                    // thus are seen on calling EndInvoke in the PutCompleted method.
                    //
                    // Just ensure the streams are closed etc, and rethrow.
                    state.Dispose();
                    throw;
                }
            }//if
        }

        delegate void PutFromNtiCaller(Stream source, String name, String type, Int64 length);

        //----------------------------------------------------------------------
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ObexClientSession conn = buttonCancel.Tag as ObexClientSession;
            System.Diagnostics.Debug.Assert(conn != null);
            m_cancelled = true;
            if (conn != null) {
                conn.Abort("User cancel.");
            }
        }

        //----------------------------------------------------------------------
        class State : IDisposable
        {
            public Stream m_fileStream;
            public ReadProgressStream m_progressStream;
            public Brecham.Obex.Net.ObexSessionConnection m_conn;
            public PutFromNtiCaller m_putCaller;
            //
            private DateTime m_dtStart;
            private DateTime m_dtEnd;


            public void SetStartTime()
            {
                m_dtStart = DateTime.UtcNow;
            }

            /// <summary>
            /// Set the end time.  If called more than once, only the first is effective.
            /// </summary>
            public void SetEndTime()
            {
                if (m_dtEnd.Ticks == 0)
                {
                    m_dtEnd = DateTime.UtcNow;
                }//if
            }

            public TimeSpan Elapsed
            {
                get
                {
                    if (m_dtEnd.Ticks==0 || m_dtStart.Ticks==0)
                    {
                        throw new InvalidOperationException("Start or end time not set.");
                    }
                    TimeSpan ts = m_dtEnd - m_dtStart;
                    if (ts < TimeSpan.Zero)
                    {
                        throw new InvalidOperationException("End time is before start time.");
                    }
                    return ts;
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (m_conn != null) m_conn.Close();
                if (m_progressStream != null) m_progressStream.Close();
                if (m_fileStream != null) m_fileStream.Close();
            }

            #endregion
        }


        //----------------------------------------------------------------------
#if DONT_USE_TIMER_TAG
        // There's no way of passing data to the Timer's event hendler so we
        // unfortunately need this 'global' reference.  In desktop FXv2 we use its
        // Tag property.
        ReadProgressStream m_m_progressStreamReferenceForTimerBooHoo;
#endif

        private void timer1_Tick(object sender, EventArgs e)
        {
#if DONT_USE_TIMER_TAG
            this.progressBar1.Value = (int)m_m_progressStreamReferenceForTimerBooHoo.ReadPercentage;
#else
            ReadProgressStream progressStream = this.timer1.Tag as ReadProgressStream;
            System.Diagnostics.Debug.Assert(progressStream != null);
            if (progressStream != null) {
                this.progressBar1.Value = (int)progressStream.ReadPercentage;
            }
#endif
        }

        private void StartProgressBarUpdater(State state)
        {
#if DONT_USE_TIMER_TAG
            m_m_progressStreamReferenceForTimerBooHoo = state.m_progressStream;
#else
            this.timer1.Tag = state.m_progressStream;
#endif
            this.timer1.Enabled = true;
        }//fn--button1_Click

        private void EndProgressBarUpdater()
        {
            this.timer1.Enabled = false;
            try {
                // Force a final update.
                this.timer1_Tick(null, null);
            } catch (InvalidOperationException) {
                //"To read the percentage complete the total length must have been set beforehand."
                // Occurs during PUT (initiation) failure.
            }
#if DONT_USE_TIMER_TAG
            // just for tidyness.
            m_m_progressStreamReferenceForTimerBooHoo = null;
#endif
        }

        //----------------------------------------------------------------------
        void PutCompleted(IAsyncResult ar)
        {
            State state = (State)ar.AsyncState;
            state.SetEndTime(); //this only has an effect on the first call

            // We're going to update the UI so ensure that we're running on the UI 
            // thread, if necessary by calling ourselves back with Control.Invoke.
            if (this.InvokeRequired)
            {
                System.Diagnostics.Debug.WriteLine("InvokeRequired.");
                // (Argument '2' is not a params array in FX1.1)
                this.Invoke(new AsyncCallback(this.PutCompleted), new object[] { ar });
                return;
            }

            // Now do the work...
            try {
                buttonCancel.Enabled = false;
                EndProgressBarUpdater();
                try {
                    // Get the result of the Put operation. This doesn't need to 
                    // be on the UI thread, but the rest does...
                    state.m_putCaller.EndInvoke(ar);
                    this.labelStatus.Text = "PutFrom took: " + state.Elapsed.ToString();
                } finally {
                    state.Dispose();
                }
            } catch (Exception ex) {
                // Intending to to catch exceptions from the Put operation...
                // Feel ok to catch all here, as anything catastrophic would 
                // surely have occured on the UI thread too...
                String descr = ex.Message + "\r\n" + ex.GetType().ToString();
                if (m_cancelled) {
                    // User cancelled it, so hide any resultant exception.
                    String text = "Put was cancelled.";
#if DEBUG
                    text += "\r\n[" + descr + "]";
#endif
                    this.labelStatus.Text = text;
                } else {
                    this.labelStatus.Text = "PutFrom failed: " + descr;
                    MessageBox.Show(this, descr, "PutFrom failed");
                }
            }//catch
        }

    }//class

}//namespace

