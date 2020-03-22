//==========================================================================================
//
//		OpenNETCF.Diagnostics.Process
//		Copyright (c) 2003-2004, OpenNETCF.org
//
//		This library is free software; you can redistribute it and/or modify it under 
//		the terms of the OpenNETCF.org Shared Source License.
//
//		This library is distributed in the hope that it will be useful, but 
//		WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
//		FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
//		for more details.
//
//		You should have received a copy of the OpenNETCF.org Shared Source License 
//		along with this library; if not, email licensing@opennetcf.org to request a copy.
//
//		If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
//		email licensing@opennetcf.org.
//
//		For general enquiries, email enquiries@opennetcf.org or visit our website at:
//		http://www.opennetcf.org
//
//==========================================================================================
using System;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Provides access to local and remote processes and enables you to start and stop local system processes.
	/// </summary>
	/// <remarks>A <see cref="Process"/> component provides access to a process that is running on a computer.
	/// A process, in the simplest terms, is a running application.
	/// A thread is the basic unit to which the operating system allocates processor time.
	/// A thread can execute any part of the code of the process, including parts currently being executed by another thread.</remarks>
	public class Process : System.ComponentModel.Component, IDisposable
	{
		//process is still active
		private const int StillActive = 0x00000103;
		
		//handle to process
		private ProcessInfo m_pinfo;

		//structure used when starting - contains filename etc
		private ProcessStartInfo m_pstart;
		
		//flag to market when process has been killed
		private bool m_exited;

		private const int SYS_HANDLE_BASE	 = 64;
		private const int SH_WIN32          = 0;
		private const int SH_CURTHREAD      = 1;
		private const int SH_CURPROC        = 2;
		private const uint SYSHANDLE_OFFSET = 0x004;

		private const uint PUserKDataARM = 0xFFFFC800;
		private const uint PUserKDataX86 = 0x00005800;

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the OpenNETCF.Diagnostics.Process class.
		/// </summary>
		public Process()
		{
			m_pstart = new ProcessStartInfo();
			m_pinfo = new ProcessInfo();
			m_exited = false;
		}
		#endregion

		#region Dispose
		/// <summary>
		/// Releases all resources used by the <see cref="Process"/>.
		/// </summary>
		/// <remarks>Calling <b>Dispose</b> allows the resources used by the <see cref="Process"/> to be reallocated for other purposes.</remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="Process"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			//close process handle
			CloseHandle((IntPtr)m_pinfo.hProcess);

			base.Dispose (disposing);
		}
		#endregion

		#region Get Process By Id
		/// <summary>
		/// Returns a new <see cref="Process"/> component, given the identifier of a process on the device.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		/// <param name="processId">The system-unique identifier of a process resource.</param>
		/// <returns>A <see cref="Process"/> component that is associated with the local process resource identified by the processId parameter.</returns>
		public static Process GetProcessById(int processId)
		{
			//create new process
			Process newprocess = new Process();

			//assign process id
			newprocess.m_pinfo.dwProcessID = processId;

			//open handle
			newprocess.m_pinfo.hProcess = OpenProcess(0, false, (IntPtr)processId);
			
			//return process instance
			return newprocess;
		}
		#endregion

		#region Get Process From Hwnd
		/*public static Process GetProcessFromHwnd(IntPtr Hwnd)
		{
			int pid = GetWindowThreadProcessId(Hwnd, IntPtr.Zero);
			return GetProcessFromId(pid);
		}*/
		#endregion


		#region Start
		/// <summary>
		/// Starts a process resource by specifying the name of a document or application file and associates the resource with a new OpenNETCF.Diagnostics.Process component.
		/// </summary>
		/// <param name="fileName">The name of a document or application file to run in the process.</param>
		/// <param name="arguments">Command line arguments to pass when starting the process.</param>
		public static Process Start(string fileName, string arguments)
		{
			return Process.Start(new ProcessStartInfo(fileName, arguments));
		}
		/// <summary>
		/// Starts a process resource by specifying the name of a document or application file and associates the resource with a new OpenNETCF.Diagnostics.Process component.
		/// </summary>
		/// <param name="fileName">The name of a document or application file to run in the process.</param>
		/// <returns>A new OpenNETCF.Diagnostics.Process component that is associated with the process resource, or null, if no process resource is started (for example, if an existing process is reused).</returns>
		public static Process Start(string fileName)
		{
			return Process.Start(new ProcessStartInfo(fileName));
		}
		/// <summary>
		/// Starts the process resource that is specified by the parameter containing process start information (for example, the file name of the process to start) and associates the resource with a new OpenNETCF.Diagnostics.Process component.
		/// </summary>
		/// <param name="startInfo">The OpenNETCF.Diagnostics.ProcessStartInfo that contains the information that is used to start the process, including the file name and any command line arguments.</param>
		/// <returns>A new OpenNETCF.Diagnostics.Process component that is associated with the process resource, or null if no process resource is started (for example, if an existing process is reused).</returns>
		public static Process Start(ProcessStartInfo startInfo)
		{
			Process newprocess = new Process();
			newprocess.m_pstart = startInfo;
			newprocess.Start();

			return newprocess;
		}
		/// <summary>
		/// Starts (or reuses) the process resource that is specified by the OpenNETCF.Diagnostics.Process.StartInfo property of this OpenNETCF.Diagnostics.Process component and associates it with the component.
		/// </summary>
		/// <returns><b>true</b> if a process resource is started; <b>false</b> if no new process resource is started (for example, if an existing process is reused).</returns>
		public bool Start()
		{
			if(m_pstart.UseShellExecute)
			{
				//create a shellexecute structure
				ShellExecuteInfo sei = new ShellExecuteInfo();
				
				//pin strings
				GCHandle hfile = GCHandle.Alloc((m_pstart.FileName + '\0').ToCharArray(), GCHandleType.Pinned);
				sei.lpFile = (IntPtr)((int)hfile.AddrOfPinnedObject() + 4);

				//windowstyle
				sei.nShow = (int)m_pstart.WindowStyle;

				//pin arguments
				GCHandle hargs = new GCHandle();
				if(m_pstart.Arguments!=null)
				{
					hargs = GCHandle.Alloc((m_pstart.Arguments + '\0').ToCharArray(), GCHandleType.Pinned);
					sei.lpParameters = (IntPtr)((int)hargs.AddrOfPinnedObject() + 4);
				}

				//pin verb
				GCHandle hverb = new GCHandle();
				if(m_pstart.Verb!=null)
				{
					hverb = GCHandle.Alloc(m_pstart.Verb.ToCharArray(), GCHandleType.Pinned);
					sei.lpVerb = (IntPtr)((int)hverb.AddrOfPinnedObject() + 4);
				}

				//call api function
				bool result = ShellExecuteEx(sei);

				//store the process handle
				this.m_pinfo.hProcess = sei.hProcess;
				
				
				//free pinned handles
				if(hfile.IsAllocated)
				{
					hfile.Free();
				}
				if(hargs.IsAllocated)
				{
					hargs.Free();
				}
				if(hverb.IsAllocated)
				{
					hverb.Free();
				}

				//return status
				return result;
			}
			else
			{
				return CreateProcess(m_pstart.FileName, m_pstart.Arguments, IntPtr.Zero, IntPtr.Zero, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, m_pinfo);
			}
		}
		#endregion

		#region Kill
		/// <summary>
		/// Immediately stops the associated process.
		/// </summary>
		/// <exception cref="System.ComponentModel.Win32Exception">The associated process could not be terminated.</exception>
		/// <exception cref="System.InvalidOperationException">The process has already exited.</exception>
		/// <event></event>
		public void Kill()
		{
			if(!m_exited)
			{
				IntPtr hProcess = OpenProcess(0, false, this.Handle);

				//if successfully terminated
				if(TerminateProcess(hProcess, 0))
				{
					CloseHandle(hProcess);

					//raise Exited event
					OnExited();
				}
				else
				{
					throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "The associated process could not be terminated");
				}
			}
			else
			{
				throw new InvalidOperationException("The process has already exited.");
			}
		}
		#endregion

		#region Wait For Exit
		/// <summary>
		/// Instructs the <see cref="Process"/> component to wait indefinitely for the associated process to exit.
		/// </summary>
		/// <remarks>WaitForExit is used to make the current thread wait until the associated process terminates.
		/// This overload of <see cref="Process.WaitForExit"/> instructs the <see cref="Process"/> component to wait an infinite amount of time for the process to exit.
		/// This can cause an application to stop responding.</remarks>
		public void WaitForExit()
		{
			//wait for infinity
			WaitForExit(int.MinValue);
		}

		/// <summary>
		/// Instructs the <see cref="T:OpenNETCF.Diagnostics.Process"/> component to wait the specified number of milliseconds for the associated process to exit.
		/// </summary>
		/// <param name="milliseconds">The amount of time, in milliseconds, to wait for the associated process to exit.
		/// The maximum is the largest possible value of a 32-bit integer, which represents infinity to the operating system.</param>
		/// <returns>true if the associated process has exited; otherwise, false.</returns>
		public bool WaitForExit(int milliseconds)
		{
			//wait time
			uint wait;
			//catch max value and convert to infinite flag
			if(milliseconds==int.MaxValue | milliseconds==int.MinValue)
			{
				wait = uint.MaxValue;
			}
			else if(milliseconds < 0)
			{
				wait = uint.MaxValue;
			}
			else
			{
				wait = Convert.ToUInt32(milliseconds);
			}
			int result = WaitForSingleObject((IntPtr)this.m_pinfo.hProcess, wait);
			
			if(result==0)
			{
				//raise exited event
				OnExited();

				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion


		#region Handle
		/// <summary>
		/// Returns the associated process's native handle.
		/// </summary>
		/// <value>The handle that the operating system assigned to the associated process when the process was started.
		/// The system uses this handle to keep track of process attributes.</value>
		public IntPtr Handle
		{
			get
			{
				//if not valid handle
				if(m_pinfo.hProcess == IntPtr.Zero)
				{
					if(m_pinfo.dwProcessID != 0)
					{
						

						//populate handle
						m_pinfo.hProcess = OpenProcess(0, false, (IntPtr)m_pinfo.dwProcessID);
					}
					else
					{
						//no handle or id available
						return IntPtr.Zero;
					}
				}

				return m_pinfo.hProcess;
			}
		}
		#endregion

		#region Id
		/// <summary>
		/// Gets the unique identifier for the associated process.
		/// </summary>
		/// <remarks>The system-generated unique identifier of the process that is referenced by this <see cref="Process"/> instance.</remarks>
		public int Id
		{
			get
			{
				return m_pinfo.dwProcessID;
			}
		}
		#endregion

		#region Has Exited
		/// <summary>
		/// Gets a value indicating whether the associated process has been terminated.
		/// </summary>
		/// <remarks>true if the operating system process referenced by the <see cref="Process"/> component has terminated; otherwise, false.</remarks>
		public bool HasExited
		{
			get
			{
				return m_exited;
			}
		}
		#endregion

		#region Exit Code
		/// <summary>
		/// Gets the value that the associated process specified when it terminated.
		/// </summary>
		/// <remarks>The code that the associated process specified when it terminated.</remarks>
		public int ExitCode
		{
			get
			{
				int exitcode = 0;
				if(GetExitCodeProcess((IntPtr)m_pinfo.hProcess, ref exitcode))
				{
					if(exitcode==StillActive)
					{
						throw new System.InvalidOperationException(" The process has not exited.");
					}
					else
					{
						//process is definately exited;
						m_exited = true;

						return exitcode;
					}
				}
				else
				{
					throw new System.InvalidOperationException("The process OpenNETCF.Diagnostics.Process.Handle is not valid.");
				}
			}
		}
		#endregion

		#region Start Info
		/// <summary>
		/// Gets or sets the properties to pass to the <see cref="Process.Start"/> method of the <see cref="Process"/>.
		/// </summary>
		/// <value>The <see cref="ProcessStartInfo"/> that represents the data with which to start the process.
		/// These arguments include the name of the executable file or document used to start the process.</value>
		/// <exception cref="T:System.ArgumentException">The value that specifies the <see cref="Process.StartInfo"/> is null.</exception>
		/// <remarks>StartInfo represents the set of parameters to use to start a process.
		/// When <see cref="Process.Start"/> is called, the <see cref="Process.StartInfo"/> is used to specify the process to start.
		/// The only necessary StartInfo member to set is the <see cref="ProcessStartInfo.FileName"/> property.
		/// The <see cref="ProcessStartInfo.FileName"/> property does not need to represent an executable file.
		/// It can be of any file type for which the extension has been associated with an application installed on the system.
		/// For example the FileName can have a .txt extension if you have associated text files with an editor, such as Notepad, or it can have a .pwd if you have associated .pwd files with a word processing tool, such as Microsoft Pocket Word.</remarks>
		public ProcessStartInfo StartInfo
		{
			get
			{
				return m_pstart;
			}
			set
			{
				if(value!=null)
				{
					m_pstart = value;
				}
				else
				{
					throw new ArgumentException("The value that specifies the OpenNETCF.Diagnostics.Process.StartInfo is null.");
				}
			}
		}
		#endregion


		#region Exited
		/// <summary>
		/// Occurs when a process exits.
		/// </summary>
		/// <remarks>Note that in this partial implementation the Exited event will only be raised when a <see cref="Process"/> is explictly Killed.
		/// The <see cref="Process"/> class will not watch created processes unless you use the <see cref="Process.WaitForExit"/> method.</remarks>
		public event System.EventHandler Exited;

		/// <summary>
		/// Raises the <see cref="E:OpenNETCF.Diagnostics.Process.Exited"/> event.
		/// </summary>
		protected void OnExited()
		{
			//set exited flag
			this.m_exited = true;

			if(Exited!=null)
			{
				//raise Exited event
				Exited(this, new EventArgs());
			}
		}
		#endregion

		private static uint GetPUserKData()
		{
			uint			PUserKData	= 0;
			Core.SystemInfo si			= Core.GetSystemInfo();
			
			switch(si.wProcessorArchitecture)
			{
				case Core.ProcessorArchitecture.ARM:
					PUserKData = PUserKDataARM;
					break;
				case Core.ProcessorArchitecture.Intel:
					PUserKData = PUserKDataX86;
					break;
				default:
					throw new NotSupportedException("Unsupported on current processor: " + EnumEx.GetName(typeof(Core.ProcessorArchitecture), si.wProcessorArchitecture));
			}

			return PUserKData;
		}

		/// <summary>
		/// This function returns the process identifier of the calling process.
		/// </summary>
		/// <returns>The return value is the process identifier of the calling process.</returns>
		public static int GetCurrentProcessID()
		{
			IntPtr pBase = new IntPtr((int) (GetPUserKData() + SYSHANDLE_OFFSET));
			return Marshal.ReadInt32(pBase, SH_CURTHREAD * 4);
		}

		/// <summary>
		/// This function returns the thread identifier, which is used as a handle of the calling thread. 
		/// </summary>
		/// <returns>The thread identifier of the calling thread indicates success.</returns>
		public static int GetCurrentThreadID()
		{
			IntPtr pBase = new IntPtr((int) (GetPUserKData() + SYSHANDLE_OFFSET));
			return Marshal.ReadInt32(pBase, SH_CURPROC * 4);
		}

		/// <summary>
		/// Gets a new <see cref="Process"/> component and associates it with the currently active process.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		/// <returns>A new <see cref="Process"/> component associated with the process resource that is running the calling application.</returns>
		/// <remarks>Use this method to create a new Process instance and associate it with the process resource on the local computer.</remarks>
		public static IntPtr GetCurrentProcess()
		{
			return new IntPtr(SH_CURPROC + SYS_HANDLE_BASE);
		}

		/// <summary>
		/// This function returns a pseudohandle for the current thread. 
		/// </summary>
		/// <returns>The return value is a pseudohandle for the current thread.</returns>
		/// <remarks>
		/// <p>A pseudohandle is a special constant that is interpreted as the current thread handle. </p>
		/// <p>The calling thread can use this handle to specify itself whenever a thread handle is required.This handle has the maximum possible access to the thread object.</p>
		/// <p>The function cannot be used by one thread to create a handle that can be used by other threads to refer to the first thread. The handle is always interpreted as referring to the thread that is using it.</p>
		/// <p>The pseudohandle need not be closed when it is no longer needed. Calling the CloseHandle function with this handle has no effect.</p>
		/// </remarks>
		public static IntPtr GetCurrentThread()
		{
			return new IntPtr(SH_CURTHREAD + SYS_HANDLE_BASE);
		}
		#region Process P/Invokes

		[DllImport("coredll", EntryPoint="ShellExecuteEx", SetLastError=true)]
		private extern static bool ShellExecuteEx( ShellExecuteInfo ex );

		[DllImport("coredll", EntryPoint="CreateProcess", SetLastError=true)]
		private extern static bool CreateProcess(string pszImageName, string pszCmdLine, IntPtr psaProcess, IntPtr psaThread, int fInheritHandles, int fdwCreate, IntPtr pvEnvironment, IntPtr pszCurDir, IntPtr psiStartInfo, ProcessInfo pi);

		[DllImport("coredll", EntryPoint="OpenProcess", SetLastError=true)]
		internal extern static IntPtr OpenProcess(uint fdwAccess, bool fInherit, IntPtr IDProcess);

		[DllImport("coredll.dll", EntryPoint="CloseHandle", SetLastError=true)] 
		internal static extern int CloseHandle(IntPtr hObject);

		[DllImport("coredll.dll", EntryPoint="TerminateProcess", SetLastError=true)]
		internal extern static bool TerminateProcess(IntPtr hProcess, int uExitCode);

		[DllImport("coredll.dll", EntryPoint="WaitForSingleObject", SetLastError = true)]
		private static extern int WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds); 

		[DllImport("coredll.dll", EntryPoint="GetExitCodeProcess", SetLastError=true)]
		private extern static bool GetExitCodeProcess(IntPtr hProcess, ref int lpExitCode);

//		[DllImport("coredll.dll", EntryPoint="GetCurrentProcessId", SetLastError=true)]
//		private extern static int GetCurrentProcessId();

		[DllImport("coredll.dll", EntryPoint="GetWindowThreadProcessId", SetLastError=true)]
		private extern static int GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);

		#endregion
		

		#region Process Info
		private sealed class ProcessInfo
		{
			public IntPtr hProcess	= IntPtr.Zero;
			public IntPtr hThread	= IntPtr.Zero;
			public int dwProcessID	= 0;
			public int dwThreadID	= 0;
		}
		#endregion

		#region Shell Execute Info
		private sealed class ShellExecuteInfo
		{

			public UInt32 cbSize		= 60; 
			public UInt32 fMask			= 0x00000040; 
			public IntPtr hwnd			= IntPtr.Zero; 
			public IntPtr lpVerb		= IntPtr.Zero; 
			public IntPtr lpFile		= IntPtr.Zero; 
			public IntPtr lpParameters	= IntPtr.Zero; 
			public IntPtr lpDirectory	= IntPtr.Zero; 
			public int nShow			= 0; 
			public IntPtr hInstApp		= IntPtr.Zero; 
			public IntPtr lpIDList		= IntPtr.Zero; 
			public IntPtr lpClass		= IntPtr.Zero; 
			public IntPtr hkeyClass		= IntPtr.Zero; 
			public UInt32 dwHotKey		= 0; 
			public IntPtr hIcon			= IntPtr.Zero; 
			public IntPtr hProcess		= IntPtr.Zero; 
		}
		#endregion

	}
}
