using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ForCf
{

    public partial class Form1 : Form
    {
        CmdlineRunner.NetworkServer m_serverInstance;
        bool m_shuttingDown;
        const String NewLine = "\r\n";

        private void menuItemStart_Click(object sender, EventArgs e)
        {
            bool active = m_serverInstance != null && m_serverInstance.IsActive;
            System.Diagnostics.Debug.Assert(active != this.menuItem1.Enabled,
                "'Go Serve!' menu item should be disabled when server running and vice versa.");
            if (active) {
                MessageBox.Show("Server is running.  Stop first to restart.");
                return;
            }
            //
            NetworkStartDialog dlg = new NetworkStartDialog();
            DialogResult rslt = dlg.ShowDialog();
            if (rslt != DialogResult.OK) {
                return;
            }
            CmdlineRunner.ListenProtocol proto = dlg.NetworkProtocol;
            bool bluetoothFtp = dlg.BluetoothFtp;
            //
            if (proto == CmdlineRunner.ListenProtocol.Bluetooth) {
                InTheHand.Net.Bluetooth.BluetoothRadio myRadio
                    = InTheHand.Net.Bluetooth.BluetoothRadio.PrimaryRadio;
                if (myRadio == null ||
                    !(myRadio.Mode == InTheHand.Net.Bluetooth.RadioMode.Connectable
                        || myRadio.Mode == InTheHand.Net.Bluetooth.RadioMode.Discoverable)) {
                    MessageBox_Show(this, "No bluetooth radio, or disabled.", "Bluetooth status");
                    return;
                }
            }
            //
            //--------
            m_serverInstance = new CmdlineRunner.NetworkServer();
            m_serverInstance.m_console = new AndyH.SWF.FormsControlWriter(this.textBox1);
            m_serverInstance.NetworkProtocol = proto;
            m_serverInstance.BluetoothFtpProfile = bluetoothFtp;

            // At the moment ListenAndHandleForever (within Start) blocks on accepting a new 
            // socket connection, on the session completing, and then loops agauin to 
            // accept the next connection, so Start doesn't ever return.  In an
            // implementation better suited to use in a UI application the accept
            // should be done asynchronously and the completion of the OBEX
            // server signalled via a callback.  And an even more complete implementation
            // might want to support multiple simultaneous connections.
            // For now we just run this on a new thread. :-(  
            //
            //m_serverInstance.Start(proto);
            System.Threading.ThreadPool.QueueUserWorkItem(Run);
        }

        private void MessageBox_Show(Form owner, string message, string caption)
        {
            MessageBox.Show(
#if !NETCF
                owner,
#endif
                message, caption);
        }

        private void Run(Object state)
        {
            SetTextAndMenuEnabledDlgt dlgt = SetTextAndMenuEnabled;
            try {
                String proto = m_serverInstance.NetworkProtocol.ToString();
                if (m_serverInstance.NetworkProtocol == CmdlineRunner.ListenProtocol.Bluetooth) {
                    proto += " " + (m_serverInstance.BluetoothFtpProfile ? "FTP" : "OPP");
                }
                this.label1.Invoke(dlgt, this.label1, "Running " + proto, this.menuItem1, false);
                m_serverInstance.Start();
            } finally {
                if (!m_shuttingDown) {
                    this.label1.Invoke(dlgt, this.label1, "Stopped", this.menuItem1, true);
                }
            }
        }

        private void menuItemStop_Click(object sender, EventArgs e)
        {
            if (m_serverInstance != null) {
                this.textBox1.Text += "Stop" + NewLine;
                m_serverInstance.StopHack();
            }
        }

        private void menuItemQuit_Click(object sender, EventArgs e)
        {
            try {
                DoQuit(sender, e);
            } finally {
                this.Close();
            }
        }

        private void DoQuit(object sender, EventArgs e)
        {
            m_shuttingDown = true;
            menuItemStop_Click(sender, e);
        }

        //--------------------------------------------------------------
        private void menuItemClearLog_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = null;
        }

        //--------------------------------------------------------------
#if PocketPC
        CeServiceState QueryServiceStatus(String serviceName, bool reportErrorsOnUi)
        {
            IntPtr hService = NativeMethods.INVALID_HANDLE_VALUE;
            bool success;
            UInt32 dwStatus = 0;
            hService = NativeMethods.CreateFile(serviceName, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, 0,
                                          NativeMethods.NULL, NativeMethods.OPEN_EXISTING, 0, NativeMethods.NULL);
            if (NativeMethods.INVALID_HANDLE_VALUE == hService) {
                ReportServiceStatusError("Open service", reportErrorsOnUi);
                return CeServiceState.Unknown;
            }
            try {
                int sizeOutReturned;
                int sizeOutIn = Marshal.SizeOf(dwStatus);
                success = NativeMethods.DeviceIoControl_UInt32(hService, CeServiceIoctl.Status,
                    IntPtr.Zero, 0, out dwStatus, sizeOutIn, out sizeOutReturned, IntPtr.Zero);
                if (!success) {
                    ReportServiceStatusError("Get status", reportErrorsOnUi);
                    return CeServiceState.Unknown;
                }
                System.Diagnostics.Debug.Assert(sizeOutIn == sizeOutReturned, "out buffer size: in vs out");
                return (CeServiceState)dwStatus;
            } finally {
                bool ret2 = NativeMethods.CloseHandle(hService);
                System.Diagnostics.Debug.Assert(ret2, "CloseHandle");
            }//finally
            /*
                DWORD OBEXQuery() 
                {
                    HANDLE hService;
                    BOOL fRet;
                    DWORD dwStatus;
                    hService = CreateFile(L"OBX0:",GENERIC_READ|GENERIC_WRITE,0,
                                          NULL,OPEN_EXISTING,0,NULL);
                    if (INVALID_HANDLE_VALUE == hService)
                        return FALSE;
                    fRet = DeviceIoControl(hService,IOCTL_SERVICE_STATUS,0,0,&dwStatus,sizeof(DWORD),0, 0);
                    CloseHandle(hService);
                    if(0 == fRet) {
                        return 0xFFFFFFFF;
                    } else {
                        return dwStatus;
                    }
                     
                }
            */
        }

        void SetServiceStatus(String serviceName, CeServiceIoctl state, bool reportErrorsOnUi)
        {
            IntPtr hService;
            bool success;
            hService = NativeMethods.CreateFile(serviceName, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, 0,
                                  NativeMethods.NULL, NativeMethods.OPEN_EXISTING, 0, NativeMethods.NULL);
            if (NativeMethods.INVALID_HANDLE_VALUE == hService) {
                ReportServiceStatusError("Open service", reportErrorsOnUi);
                return;
            }
            try {
                int tmp;
                success = NativeMethods.DeviceIoControl_IntPtr(hService, state,
                    IntPtr.Zero, 0, IntPtr.Zero, 0, out tmp, IntPtr.Zero);
                if (!success) {
                    ReportServiceStatusError("Set status", reportErrorsOnUi);
                    return;
                }
            } finally {
                bool ret2 = NativeMethods.CloseHandle(hService);
                System.Diagnostics.Debug.Assert(ret2, "CloseHandle");
            }
            /*
                HRESULT OBEXIOCTL(DWORD dwIOCTL)
                {
                    HANDLE hService;
                    BOOL fRet;
                    hService = CreateFile(L"OBX0:",GENERIC_READ|GENERIC_WRITE,0,
                                          NULL,OPEN_EXISTING,0,NULL);
                    if (INVALID_HANDLE_VALUE == hService)
                        return FALSE;
                    fRet = DeviceIoControl(hService,dwIOCTL,0,0,0,0,NULL,0);
                    CloseHandle(hService);
                    return (0 == fRet)?E_FAIL:S_OK;
                }
            */
        }

        private void ReportServiceStatusError(string msg, bool reportErrorsOnUi)
        {
            if (!reportErrorsOnUi) {
                throw new Win32Exception();
            } else {
                int gle = Marshal.GetLastWin32Error();
                MessageBox.Show(String.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "Error code: {0} (0x{0:X})", gle),
                    "Failed at " + msg);
            }
        }


        class NativeMethods
        {
            internal static readonly IntPtr NULL = IntPtr.Zero;
            internal static readonly IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

            [DllImport("coredll.dll", SetLastError = true)]
            internal static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes,
                uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

            internal const uint GENERIC_READ = (0x80000000);
            internal const uint GENERIC_WRITE = (0x40000000);
            internal const uint GENERIC_EXECUTE = (0x20000000);
            internal const uint GENERIC_ALL = (0x10000000);

            internal const uint FILE_SHARE_READ = 0x00000001;
            internal const uint FILE_SHARE_WRITE = 0x00000002;
            //
            internal const uint CREATE_NEW = 1;
            internal const uint CREATE_ALWAYS = 2;
            internal const uint OPEN_EXISTING = 3;
            internal const uint OPEN_ALWAYS = 4;
            internal const uint TRUNCATE_EXISTING = 5;
            internal const uint OPEN_FOR_LOADER = 6;


            [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeviceIoControl_IntPtr(
                IntPtr hDevice, CeServiceIoctl dwIoControlCode,
                IntPtr lpInBuffer, int nInBufferSize,
                IntPtr lpOutBuffer, int nOutBufferSize,
                out int lpBytesReturned,
                IntPtr lpOverlapped);

            [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeviceIoControl_UInt32(
                IntPtr hDevice, CeServiceIoctl dwIoControlCode,
                IntPtr lpInBuffer, int nInBufferSize,
                out uint lpOutBuffer, int nOutBufferSize,
                out int lpBytesReturned,
                IntPtr lpOverlapped);

            internal const uint _IOCTL_SERVICE_STATUS = 0x01040020;

            [DllImport("coredll.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseHandle(IntPtr hObject);

        }


        /// <summary>
        /// Service states
        /// </summary>
        enum CeServiceState : uint
        {
            Off = 0,
            On = 1,
            StartingUp = 2,
            ShuttingDown = 3,
            Unloading = 4,
            Uninitialized = 5,
            Unknown = 0xffffffff,
        }

        enum CeServiceIoctl : uint
        {
            Start = 0x01040004,
            Stop = 0x01040008,
            //...
            Status = 0x1040020,
            //...
            NotifyAddrChange = 0x01040040
        }

        private void menuItemBeamReceiveStart_Click(object sender, EventArgs e)
        {
            SetServiceStatus("OBX0:", CeServiceIoctl.Start, true);
        }

        private void menuItemBeamReceiveStop_Click(object sender, EventArgs e)
        {
            SetServiceStatus("OBX0:", CeServiceIoctl.Stop, true);
        }

        bool m_seenMenuItemPopupEvent;

        private void menuItemMenuBeamReceive_Click(object sender, EventArgs e)
        {
            // On some version of the NETCF (machine OS versions?) the Popup event 
            // isn't raised, so clicking the menu item shows a dialog with the status.
            //
            //if (m_seenMenuItemPopupEvent)
            //    return;
            CeServiceState state = QueryServiceStatus("OBX0:", true);
            MessageBox.Show("State: " + state
                // FYI append a dot when popup events work.
                + (m_seenMenuItemPopupEvent ? "." : String.Empty),
                "Beam Receive status");
        }

        private void menuItemBeamReceiveMenu_PopupSubMenu(object sender, EventArgs e)
        {
            m_seenMenuItemPopupEvent = true;
            CeServiceState state = QueryServiceStatus("OBX0:", true);
            this.menuItemBeamReceiveStatus.Text = "State: " + state;
        }

        private void menuItemBeamReceiveMenu_PopupItem(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Fail("Popup event on item!?!");
            m_seenMenuItemPopupEvent = true;
            CeServiceState state = QueryServiceStatus("OBX0:", true);
            this.menuItemBeamReceiveStatus.Text = "State: " + state;
        }
#endif

    }//class
}