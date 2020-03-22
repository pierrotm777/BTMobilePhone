using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Text;
using OpenNETCF.Diagnostics;

namespace OpenNETCF.ToolHelp
{
	/// <summary>
	/// Wrapper around the ToolHelp ProcessEntry information
	/// </summary>
	/// <remarks>
	/// This class requires the toolhelp32.dll
	/// </remarks>
	public class ProcessEntry
	{
		private PROCESSENTRY32 m_pe;

		private ProcessEntry(PROCESSENTRY32 pe)
		{
			m_pe = pe;
		}

		/// <summary>
		/// Get the short name of the current process
		/// </summary>
		/// <returns>The current process name</returns>
		public override string ToString()
		{
			return System.IO.Path.GetFileName(m_pe.ExeFile);
		}

		/// <summary>
		/// Base address for the process
		/// </summary>
		[CLSCompliant(false)]
		public uint BaseAddress
		{
			get { return m_pe.BaseAddress; }
		}

		/// <summary>
		/// Number of execution threads started by the process.
		/// </summary>
		public int ThreadCount
		{
			get { return (int)m_pe.cntThreads; }
		}

		/// <summary>
		/// Identifier of the process. The contents of this member can be used by Win32 API elements. 
		/// </summary>
		public IntPtr Handle
		{
			get	{ return (IntPtr)m_pe.ProcessID; }
		}

		/// <summary>
		/// Null-terminated string that contains the path and file name of the executable file for the process. 
		/// </summary>
		public string ExeFile
		{
			get { return m_pe.ExeFile; }
		}

		/// <summary>
		/// Kill the Process
		/// </summary>
		public void Kill()
		{
			IntPtr hProcess;
		
			hProcess = Process.OpenProcess(PROCESS_TERMINATE, false, this.Handle);

			if(hProcess != (IntPtr) INVALID_HANDLE_VALUE) 
			{
				bool bRet;
				bRet = Process.TerminateProcess(hProcess, 0);
				Process.CloseHandle(hProcess);
			}
		}

		/// <summary>
		/// Rerieves an array of all running processes
		/// </summary>
		/// <returns></returns>
		public static ProcessEntry[] GetProcesses()
		{
			ArrayList procList = new ArrayList();

			IntPtr handle = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

			if ((int)handle > 0)
			{
				try
				{
					PROCESSENTRY32 peCurrent;
					PROCESSENTRY32 pe32 = new PROCESSENTRY32();
					//Get byte array to pass to the API calls
					byte[] peBytes = pe32.ToByteArray();
					//Get the first process
					int retval = Process32First(handle, peBytes);

					while(retval == 1)
					{
						//Convert bytes to the class
						peCurrent = new PROCESSENTRY32(peBytes);
						//New instance
						ProcessEntry proc = new ProcessEntry(peCurrent);
			
						procList.Add(proc);

						retval = Process32Next(handle, peBytes);
					}
				}
				catch(Exception ex)
				{
					throw new Exception("Exception: " + ex.Message);
				}
				
				//Close handle
				CloseToolhelp32Snapshot(handle); 
				
				return (ProcessEntry[])procList.ToArray(typeof(ProcessEntry));

			}
			else
			{
				throw new Exception("Unable to create snapshot");
			}


		}

		#region PROCESSENTRY32 implementation

		//		typedef struct tagPROCESSENTRY32 
		//		{ 
		//			DWORD dwSize; 
		//			DWORD cntUsage; 
		//			DWORD th32ProcessID; 
		//			DWORD th32DefaultHeapID; 
		//			DWORD th32ModuleID; 
		//			DWORD cntThreads; 
		//			DWORD th32ParentProcessID; 
		//			LONG pcPriClassBase; 
		//			DWORD dwFlags; 
		//			TCHAR szExeFile[MAX_PATH]; 
		//			DWORD th32MemoryBase;
		//			DWORD th32AccessKey;
		//		} PROCESSENTRY32;

		private class PROCESSENTRY32
		{
			// constants for structure definition
			private const int SizeOffset = 0;
			private const int UsageOffset = 4;
			private const int ProcessIDOffset=8;
			private const int DefaultHeapIDOffset = 12;
			private const int ModuleIDOffset = 16;
			private const int ThreadsOffset = 20;
			private const int ParentProcessIDOffset = 24;
			private const int PriClassBaseOffset = 28;
			private const int dwFlagsOffset = 32;
			private const int ExeFileOffset = 36;
			private const int MemoryBaseOffset = 556;
			private const int AccessKeyOffset = 560;
			private const int Size = 564;
			private const int MAX_PATH = 260;

			// data members
			public uint dwSize; 
			public uint cntUsage; 
			public uint th32ProcessID; 
			public uint th32DefaultHeapID; 
			public uint th32ModuleID; 
			public uint cntThreads; 
			public uint th32ParentProcessID; 
			public long pcPriClassBase; 
			public uint dwFlags; 
			public string szExeFile;
			public uint th32MemoryBase;
			public uint th32AccessKey;

			//Default constructor
			public PROCESSENTRY32()
			{
			}

			// create a PROCESSENTRY instance based on a byte array		
			public PROCESSENTRY32(byte[] aData)
			{
				dwSize = GetUInt(aData, SizeOffset);
				cntUsage = GetUInt(aData, UsageOffset);
				th32ProcessID = GetUInt(aData, ProcessIDOffset);
				th32DefaultHeapID = GetUInt(aData, DefaultHeapIDOffset);
				th32ModuleID = GetUInt(aData, ModuleIDOffset);
				cntThreads = GetUInt(aData, ThreadsOffset);
				th32ParentProcessID = GetUInt(aData, ParentProcessIDOffset);
				pcPriClassBase = (long) GetUInt(aData, PriClassBaseOffset);
				dwFlags = GetUInt(aData, dwFlagsOffset);
				szExeFile = GetString(aData, ExeFileOffset, MAX_PATH);
				th32MemoryBase = GetUInt(aData, MemoryBaseOffset);
				th32AccessKey = GetUInt(aData, AccessKeyOffset);
			}

			#region Helper conversion functions
			// utility:  get a uint from the byte array
			private static uint GetUInt(byte[] aData, int Offset)
			{
				return BitConverter.ToUInt32(aData, Offset);
			}
		
			// utility:  set a uint int the byte array
			private static void SetUInt(byte[] aData, int Offset, int Value)
			{
				byte[] buint = BitConverter.GetBytes(Value);
				Buffer.BlockCopy(buint, 0, aData, Offset, buint.Length);
			}

			// utility:  get a ushort from the byte array
			private static ushort GetUShort(byte[] aData, int Offset)
			{
				return BitConverter.ToUInt16(aData, Offset);
			}
		
			// utility:  set a ushort int the byte array
			private static void SetUShort(byte[] aData, int Offset, int Value)
			{
				byte[] bushort = BitConverter.GetBytes((short)Value);
				Buffer.BlockCopy(bushort, 0, aData, Offset, bushort.Length);
			}
		
			// utility:  get a unicode string from the byte array
			private static string GetString(byte[] aData, int Offset, int Length)
			{
				String sReturn =  Encoding.Unicode.GetString(aData, Offset, Length);
				return sReturn;
			}
		
			// utility:  set a unicode string in the byte array
			private static void SetString(byte[] aData, int Offset, string Value)
			{
				byte[] arr = Encoding.ASCII.GetBytes(Value);
				Buffer.BlockCopy(arr, 0, aData, Offset, arr.Length);
			}
			#endregion

			// create an initialized data array
			public byte[] ToByteArray()
			{
				byte[] aData;
				aData = new byte[Size];
				//set the Size member
				SetUInt(aData, SizeOffset, Size);
				return aData;
			}

			public string ExeFile
			{
				get { return szExeFile; }
			}

			/// <summary>
			/// Identifier of the process. The contents of this member can be used by Win32 API elements.
			/// </summary>
			public ulong ProcessID
			{
				get
				{
					return th32ProcessID;
				}
			}

			public uint BaseAddress
			{
				get
				{
					return th32MemoryBase;
				}
			}

			public ulong ThreadCount
			{
				get
				{
					return cntThreads;
				}
			}
		}
		#endregion

		#region PInvoke declarations
		private const int PROCESS_TERMINATE = 1;
		private const int INVALID_HANDLE_VALUE = -1;
		private const int TH32CS_SNAPPROCESS = 0x00000002;

		[DllImport("toolhelp.dll")]
		private static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);
		[DllImport("toolhelp.dll")]
		private static extern int CloseToolhelp32Snapshot(IntPtr handle);
		[DllImport("toolhelp.dll")]
		private static extern int Process32First(IntPtr handle, byte[] pe);
		[DllImport("toolhelp.dll")]
		private static extern int Process32Next(IntPtr handle, byte[] pe);

/*
		[DllImport("coredll.dll")]
		private static extern IntPtr OpenProcess(int flags, bool fInherit, IntPtr PID);
		[DllImport("coredll.dll")]
		private static extern bool TerminateProcess(IntPtr hProcess, uint ExitCode);
		[DllImport("coredll.dll")]
		private static extern bool CloseHandle(IntPtr handle);
*/
		#endregion
	}
}
