//==========================================================================================
//
//		OpenNETCF.IO.FileEx
//		Copyright (c) 2003, OpenNETCF.org
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
using System.IO;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;

namespace OpenNETCF.IO
{
	#region Object Store Information
	/// <summary>
	/// Describes the current status of the Object Store
	/// </summary>
	public struct ObjectStoreInformation
	{
		/// <summary>
		/// Size of the Object Store in Bytes
		/// </summary>
		public int StoreSize;
		/// <summary>
		/// Free space in the Object Store in Bytes
		/// </summary>
		public int FreeSize;

		/// <summary>
		/// This function retrieves the size of the object store and the amount of free space currently in the object store
		/// </summary>
		/// <returns>An <see cref="ObjectStoreInformation"/> struct with the current status of the Object Store.</returns>
		public static ObjectStoreInformation GetStoreInformation()
		{
			try
			{
				ObjectStoreInformation osinfo = new ObjectStoreInformation();

				GetStoreInformationCE(out osinfo);

				return osinfo;
			}
			catch(Exception)
			{
				throw new WinAPIException("Error retrieving ObjectStoreInformation.");
			}
		}

		[DllImport("coredll", EntryPoint="GetStoreInformation", SetLastError=true)]
		private static extern int GetStoreInformationCE(out ObjectStoreInformation lpsi);

	}
	#endregion

	#region File Ex
	/// <summary>
	/// Provides additional file related functionality.
	/// </summary>
	public class FileEx
	{
		private FileEx(){}

		/// <summary>
		/// Maximum length of Filepath string (in characters)
		/// </summary>
		public const int	MaxPath				= 260;
		internal const int	ERROR_NO_MORE_FILES		= 18;

		/// <summary>
		/// Represents an invalid native operating system handle.
		/// </summary>
		public const int	InvalidHandle	= -1;

		#region Base wrappers
		/// <summary>
		/// Creates a StreamWriter that appends UTF-8 encoded text to an existing file.
		/// </summary>
		/// <param name="path">The path to the file to append to.</param>
		/// <returns>A StreamWriter that appends UTF-8 encoded text to an existing file.</returns>
		public static StreamWriter AppendText(string path)
		{
			return File.AppendText(path);
		}

		/// <summary>
		/// Copies an existing file to a new file.  Overwriting a file of the same name is not allowed.
		/// </summary>
		/// <param name="sourceFileName">The file to copy.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
		public static void Copy(string sourceFileName, string destFileName)
		{
			File.Copy(sourceFileName, destFileName);
		}

		/// <summary>
		/// Copies an existing file to a new file. Overwriting a file of the same name is allowed.
		/// </summary>
		/// <param name="sourceFileName">The file to copy.</param>
		/// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
		/// <param name="overwrite"><b>true</b> if the destination file can be overwritten; otherwise, <b>false</b>.</param>
		public static void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			File.Copy(sourceFileName, destFileName, overwrite);
		}

		/// <summary>
		/// Creates or overwrites the specified file.
		/// </summary>
		/// <param name="path">The name of the file.</param>
		/// <returns>A FileStream that provides read/write access to the specified file.</returns>
		public static FileStream Create(string path)
		{
			return File.Create(path);
		}

		/// <summary>
		/// Creates or overwrites the specified file.
		/// </summary>
		/// <param name="path">The name of the file.</param>
		/// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
		/// <returns>A new file with the specified buffer size.</returns>
		public static FileStream Create(string path, int bufferSize)
		{
			return File.Create(path, bufferSize);
		}

		/// <summary>
		/// Creates or opens a file for writing UTF-8 encoded text.
		/// </summary>
		/// <param name="path">The file to be opened for writing.</param>
		/// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
		/// <exception cref="ArgumentException">path is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.</exception>
		public static StreamWriter CreateText(string path)
		{
			return File.CreateText(path);
		}

		/// <summary>
		/// Deletes the specified file. An exception is not thrown if the specified file does not exist.
		/// </summary>
		/// <param name="path">The name of the file to be deleted. </param>
		public static void Delete(string path)
		{
			File.Delete(path);
		}

		/// <summary>
		/// Determines whether the specified file exists.
		/// </summary>
		/// <param name="path">The file to check.</param>
		/// <returns><b>true</b> if the caller has the required permissions and path contains the name of an existing file; otherwise, <b>false</b>. This method also returns <b>false</b> if path is a null reference (Nothing in Visual Basic) or a zero-length string. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <b>false</b> regardless of the existence of path.</returns>
		public static bool Exists(string path)
		{
			return File.Exists(path);
		}

		/// <summary>
		/// Returns the creation date and time of the specified file or directory.
		/// </summary>
		/// <param name="path">The file or directory for which to obtain creation date and time information.</param>
		/// <returns><seealso cref="DateTime"/>A DateTime structure set to the creation date and time for the specified file or directory. This value is expressed in local time.</returns>
		public static DateTime GetCreationTime(string path)
		{
			return File.GetCreationTime(path);
		}

		/// <summary>
		/// Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory.
		/// </summary>
		/// <param name="path">The file or directory for which to obtain creation date and time information.</param>
		/// <returns>A DateTime structure set to the creation date and time for the specified file or directory. This value is expressed in UTC time.</returns>
		public static DateTime GetCreationTimeUtc(string path)
		{
			return File.GetCreationTime(path).ToUniversalTime();
		}
		
		/// <summary>
		/// Returns the date and time the specified file or directory was last accessed.
		/// </summary>
		/// <param name="path">The file or directory for which to obtain access date and time information.</param>
		/// <returns>A DateTime structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
		public static DateTime GetLastAccessTime(string path)
		{
			return File.GetLastAccessTime(path);
		}

		/// <summary>
		/// Returns the date and time the specified file or directory was last accessed.
		/// </summary>
		/// <param name="path">The file or directory for which to obtain access date and time information.</param>
		/// <returns>A DateTime structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
		public static DateTime GetLastAccessTimeUtc(string path)
		{
			return File.GetLastAccessTime(path).ToUniversalTime();
		}

		/// <summary>
		/// Returns the date and time the specified file or directory was last written to.
		/// </summary>
		/// <param name="path">The file or directory for which to obtain access date and time information.</param>
		/// <returns>A DateTime structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
		public static DateTime GetLastWriteTime(string path)
		{
			return File.GetLastWriteTime(path);
		}

		/// <summary>
		/// Returns the date and time the specified file or directory was last written to.
		/// </summary>
		/// <param name="path">The file or directory for which to obtain access date and time information.</param>
		/// <returns>A DateTime structure set to the date and time that the specified file or directory was last written to. This value is expressed in UTC time.</returns>
		public static DateTime GetLastWriteTimeUtc(string path)
		{
			return File.GetLastWriteTime(path).ToUniversalTime();
		}

		/// <summary>
		/// Moves a specified file to a new location, providing the option to specify a new file name.
		/// </summary>
		/// <param name="sourceFileName">The name of the file to move.</param>
		/// <param name="destFileName">The new path for the file.</param>
		public static void Move(string sourceFileName, string destFileName)
		{
			File.Move(sourceFileName, destFileName);
		}


		/// <summary>
		/// Opens a FileStream on the specified path.
		/// </summary>
		/// <param name="path">The file to open.</param>
		/// <param name="mode">A FileMode value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
		/// <returns>A FileStream on the specified path.</returns>
		public static FileStream Open(string path, FileMode mode)	
		{
			return File.Open(path, mode);
		}

		/// <summary>
		/// Opens a FileStream on the specified path.
		/// </summary>
		/// <param name="path">The file to open.</param>
		/// <param name="mode">A FileMode value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
		/// <param name="access">A FileAccess value that specifies the operations that can be performed on the file.</param>
		/// <returns>A FileStream on the specified path, having the specified mode with read, write, or read/write access.</returns>
		public static FileStream Open(string path, FileMode mode, FileAccessEx access)
		{
			return File.Open(path, mode, (System.IO.FileAccess)access);
		}

		/// <summary>
		/// Opens a FileStream on the specified path.
		/// </summary>
		/// <param name="path">The file to open.</param>
		/// <param name="mode">A FileMode value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
		/// <param name="access">A FileAccess value that specifies the operations that can be performed on the file.</param>
		/// <param name="share">A FileShare value specifying the type of access other threads have to the file. </param>
		/// <returns>A FileStream on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
		public static FileStream Open(string path, FileMode mode, FileAccessEx access, FileShare share)
		{
			return File.Open(path, mode, (System.IO.FileAccess)access, (System.IO.FileShare)share);
		}

		/// <summary>
		/// Opens an existing file for reading.
		/// </summary>
		/// <param name="path">The file to be opened for reading.</param>
		/// <returns>A read-only FileStream on the specified path.</returns>
		public static FileStream OpenRead(string path)
		{
			return File.OpenRead(path);
		}

		/// <summary>
		/// Opens an existing UTF-8 encoded text file for reading.
		/// </summary>
		/// <param name="path">The file to be opened for reading.</param>
		/// <returns>A StreamReader on the specified path.</returns>
		public static StreamReader OpenText(string path)
		{
			return File.OpenText(path);
		}

		/// <summary>
		/// Opens an existing file for writing.
		/// </summary>
		/// <param name="path">The file to be opened for writing.</param>
		/// <returns>A read/write, unshared FileStream object on the specified path.</returns>
		public static FileStream OpenWrite(string path)
		{
			return File.OpenWrite(path);
		}

		#endregion

		#region FileAttributes Get/Set
		/// <summary>
		/// Gets the FileAttributes of the file on the path.
		/// <seealso cref="FileAttributes"/>
		/// </summary>
		/// <param name="path">The path to the file.</param>
		/// <returns>The FileAttributes of the file on the path, or -1 if the path or file is not found.</returns>
		/// <exception cref="ArgumentException">path is empty, contains only white spaces, or contains invalid characters.</exception>
		/// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
		/// <exception cref="NotSupportedException">path is in an invalid format.</exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
		public static FileAttributes GetAttributes(string path)
		{
			if(path.Length > 260)
			{
				throw new PathTooLongException();
			}

			if(path.Trim().Length == 0)
			{
				throw new ArgumentException();
			}

			uint attr = GetFileAttributes(path);

			if(attr ==  0xFFFFFFFF)
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
					throw new Exception("Unmanaged Error: " + e);
				}
			}
			return (FileAttributes)attr;
		}

		/// <summary>
		/// Sets the specified FileAttributes of the file on the specified path.
		/// </summary>
		/// <param name="path">The path to the file.</param>
		/// <param name="fileAttributes"><seealso cref="FileAttributes"/>The desired FileAttributes, such as Hidden, ReadOnly, Normal, and Archive.</param>
		public static void SetAttributes(string path, FileAttributes fileAttributes)
		{
			if(path.Length > 260)
			{
				throw new PathTooLongException();
			}

			if(path.Trim().Length == 0)
			{
				throw new ArgumentException();
			}

			if(! SetFileAttributes(path, (uint)fileAttributes))
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
					throw new Exception("Unmanaged Error: " + e);
				}
			}
		}
		#endregion

		#region FileTime Sets
		/// <summary>
		/// Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="creationTimeUtc">A DateTime containing the value to set for the creation date and time of path. This value is expressed in UTC time.</param>
		public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			SetCreationTime(path, creationTimeUtc.ToLocalTime());
		}

		/// <summary>
		/// Sets the date and time, in local time, that the file was created.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="creationTime">A DateTime containing the value to set for the creation date and time of path. This value is expressed in local time.</param>
		public static void SetCreationTime(string path, DateTime creationTime)
		{
			FILETIME	ft;
			IntPtr		hFile	= IntPtr.Zero;

			if(path == null)
				throw new ArgumentNullException();
			
			if(path.Length > 260)
				throw new PathTooLongException();

			if(path.Trim().Length == 0)
				throw new ArgumentException();

			hFile = CreateFile(path, FileAccess.Write, FileShare.Write, FileCreateDisposition.OpenExisting, 0);

			if((int)hFile == InvalidHandle)
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
					throw new Exception("Unmanaged Error: " + e);
				}
			}

			ft = new FILETIME(creationTime.ToFileTime());
			
			SetFileTime(hFile, ft, null, null);

			CloseHandle(hFile);
		}

		/// <summary>
		/// Sets the date and time, in coordinated universal time (UTC), that the file was last accessed.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="lastAccessTimeUtc">A DateTime containing the value to set for the last access date and time of path. This value is expressed in UTC time.</param>
		public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			SetLastAccessTime(path, lastAccessTimeUtc.ToLocalTime());
		}

		/// <summary>
		/// Sets the date and time, in local time, that the file was last accessed.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="lastAccessTime">A DateTime containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
		public static void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			FILETIME	ft;
			IntPtr		hFile	= IntPtr.Zero;

			if(path == null)
				throw new ArgumentNullException();
			
			if(path.Length > 260)
				throw new PathTooLongException();

			if(path.Trim().Length == 0)
				throw new ArgumentException();

			hFile = CreateFile(path, FileAccess.Write, FileShare.Write, FileCreateDisposition.OpenExisting, 0);

			if((int)hFile == InvalidHandle)
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
					throw new Exception("Unmanaged Error: " + e);
				}
			}

			ft = new FILETIME(lastAccessTime.ToFileTime());
			
			SetFileTime(hFile, null, ft, null);

			CloseHandle(hFile);
		}

		/// <summary>
		/// Sets the date and time, in coordinated universal time (UTC), that the file was last updated or written to.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="lastWriteTimeUtc">A DateTime containing the value to set for the last write date and time of path. This value is expressed in UTC time.</param>
		public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			SetLastWriteTime(path, lastWriteTimeUtc.ToLocalTime());
		}

		/// <summary>
		/// Sets the date and time, in local time, that the file was last updated or written to.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="lastWriteTime">A DateTime containing the value to set for the last write date and time of path. This value is expressed in local time.</param>
		public static void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			FILETIME	ft;
			IntPtr		hFile	= IntPtr.Zero;

			if(path == null)
				throw new ArgumentNullException();
			
			if(path.Length > 260)
				throw new PathTooLongException();

			if(path.Trim().Length == 0)
				throw new ArgumentException();

			hFile = CreateFile(path, FileAccess.Write, FileShare.Write, FileCreateDisposition.OpenExisting, 0);

			if((int)hFile == InvalidHandle)
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
					throw new Exception("Unmanaged Error: " + e);
				}
			}

			ft = new FILETIME(lastWriteTime.ToFileTime());
			
			SetFileTime(hFile, null, ft, null);

			CloseHandle(hFile);
		}
		#endregion

		#region --------------- File API Calls ---------------

		#region Create File
		/// <summary>
		/// Wrapper around the CreateFile API
		/// </summary>
		/// <param name="fileName">Path to the file or CE port name</param>
		/// <param name="desiredAccess">Specifies the type of access to the object. An application can obtain read access, write access, read-write access, or device query access.</param>
		/// <param name="shareMode">Specifies how the object can be shared.</param>
		/// <param name="creationDisposition">Specifies which action to take on files that exist, and which action to take when files do not exist.</param>
		/// <param name="flagsAndAttributes">Specifies the file attributes and flags for the file.</param>
		/// <returns>Handle to the created file</returns>
		[CLSCompliant(false)]
		public static IntPtr CreateFile(string fileName,
			FileAccess desiredAccess,
			FileShare shareMode,
			FileCreateDisposition creationDisposition,
			int flagsAndAttributes)
		{
			IntPtr hFile = IntPtr.Zero;

			hFile = (IntPtr)CreateFileCE(fileName, (uint)desiredAccess, (uint)shareMode, 0, (uint)creationDisposition, (uint)flagsAndAttributes, 0);

			if((int)hFile == InvalidHandle)
			{
				throw new WinAPIException("Failed to Create File");
			}

			return hFile;
		}
		#endregion

		#region Write File
		/// <summary>
		/// This function writes data to a file.
		/// </summary>
		/// <remarks> WriteFile starts writing data to the file at the position indicated by the file pointer. After the write operation has been completed, the file pointer is adjusted by the number of bytes actually written.</remarks>
		/// <param name="hFile">Handle to the file to be written to. The file handle must have been created with GENERIC_WRITE access to the file.</param>
		/// <param name="lpBuffer">Buffer containing the data to be written to the file.</param>
		/// <param name="nNumberOfBytesToWrite">Number of bytes to write to the file.</param>
		/// <param name="lpNumberOfBytesWritten">Number of bytes written by this function call. WriteFile sets this value to zero before doing any work or error checking.</param>
		public static void WriteFile(IntPtr hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten)
		{
			bool b;

			Core.CheckHandle(hFile);

			b = Convert.ToBoolean(WriteFileCE(hFile, lpBuffer, nNumberOfBytesToWrite, ref lpNumberOfBytesWritten, IntPtr.Zero));

			if(!b)
			{
				throw new WinAPIException("Write Failed");
			}
		}
		#endregion

		#region ReadFile
		/// <summary>
		/// This function reads data from a file, starting at the position indicated by the file pointer. After the read operation has been completed, the file pointer is adjusted by the number of bytes actually read.
		/// </summary>
		/// <param name="hFile">Handle to the file to be read. The file handle must have been created with GENERIC_READ access to the file. This parameter cannot be a socket handle.</param>
		/// <param name="lpBuffer">Buffer that receives the data read from the file.</param>
		/// <param name="nNumberOfBytesToRead">Number of bytes to be read from the file.</param>
		/// <param name="lpNumberOfBytesRead">number of bytes read. ReadFile sets this value to zero before doing any work or error checking.</param>
		public static void ReadFile(IntPtr hFile, byte[] lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead)
		{
			bool b;

			Core.CheckHandle(hFile);

			b = Convert.ToBoolean(ReadFileCE(hFile, lpBuffer, nNumberOfBytesToRead, ref lpNumberOfBytesRead, IntPtr.Zero));

			if(!b)
			{
				throw new WinAPIException("Write Failed");
			}
		}
		#endregion

		#region CloseHandle
		/// <summary>
		/// This function closes an open object handle
		/// </summary>
		/// <param name="hObject">Object Handle, Could be any of the following Objects:- Communications device, Mutex, Database, Process, Event, Socket, File or Thread</param>
		public static void CloseHandle(IntPtr hObject)
		{
			bool b;

			Core.CheckHandle(hObject);

			try
			{
				b = Convert.ToBoolean(CloseHandleCE(hObject));
			}
			catch(Exception ex)
			{
				throw new WinAPIException(ex);
			}

			if(!b)
			{
				throw new WinAPIException("Failed to close handle");
			}

			return;
		}
		#endregion

		#endregion ---------------------------------------------

		#region FILETIME class
		private class FILETIME
		{
			private byte[] m_data;

			public FILETIME(long FileTime)
			{
				m_data = BitConverter.GetBytes(FileTime);
			}

			public FILETIME()
			{
				m_data = new byte[8];
			}


			public uint dwLowDateTime
			{
				get
				{
					return BitConverter.ToUInt32(m_data, 0);
				}
				set
				{
					Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, 0, 4);
				}
			}

			public uint dwHighDateTime
			{
				get
				{
					return BitConverter.ToUInt32(m_data, 4);
				}
				set
				{
					Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, 4, 4);
				}
			}

			public static implicit operator byte[](FILETIME f)
			{
				return f.m_data;
			}
		}
		#endregion

		/// <summary>
		/// Use by Seek for determining move start position
		/// <i>New in SDF version 1.1</i>
		/// <seealso cref="StreamInterfaceDriver.Seek"/>
		/// </summary>
		public enum MoveMethod
		{
			/// <summary>
			/// The start of the file
			/// </summary>
			FileBeginning,
			/// <summary>
			/// The current file pointer position
			/// </summary>
			CurrentPosition,
			/// <summary>
			/// The end of the file
			/// </summary>
			FileEnd
		}


		#region File P/Invokes

		[DllImport("coredll.dll", EntryPoint="CreateFile", SetLastError=true)]
		internal static extern int CreateFileCE(
			string lpFileName,
			uint dwDesiredAccess,
			uint dwShareMode,
			int lpSecurityAttributes,
			uint dwCreationDisposition,
			uint dwFlagsAndAttributes,
			int hTemplateFile);

		[DllImport("coredll.dll", EntryPoint="CloseHandle", SetLastError=true)]
		internal static extern int CloseHandleCE(IntPtr hObject);

		/// <summary>
		/// <i>New in SDF version 1.1</i>
		/// </summary>
		/// <param name="hDevice"></param>
		/// <param name="dwIoControlCode"></param>
		/// <param name="lpInBuffer"></param>
		/// <param name="nInBufferSize"></param>
		/// <param name="lpOutBuffer"></param>
		/// <param name="nOutBufferSize"></param>
		/// <param name="lpBytesReturned"></param>
		/// <param name="lpOverlapped"></param>
		/// <returns></returns>
		[DllImport("coredll.dll", EntryPoint="DeviceIoControl", SetLastError=true)]
		internal static extern int DeviceIoControlCE(
			IntPtr hDevice, 
			int dwIoControlCode, 
			byte[] lpInBuffer, 
			int nInBufferSize, 
			byte[] lpOutBuffer, 
			int nOutBufferSize, 
			ref int lpBytesReturned, 
			IntPtr lpOverlapped);

		/// <summary>
		/// <i>New in SDF version 1.1</i>
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="lDistanceToMove"></param>
		/// <param name="lpDistanceToMoveHigh"></param>
		/// <param name="dwMoveMethod"></param>
		/// <returns></returns>
		[DllImport("coredll.dll", EntryPoint="SetFilePointer", SetLastError=true)]
		internal static extern int SetFilePointerCE(
			IntPtr hFile, 
			int lDistanceToMove, 
			int lpDistanceToMoveHigh, 
			MoveMethod dwMoveMethod); 

		[DllImport("coredll.dll", EntryPoint="WriteFile", SetLastError=true)]
		internal static extern int WriteFileCE(
			IntPtr hFile,
			Byte[] lpBuffer,
			int nNumberOfBytesToWrite,
			ref int lpNumberOfBytesWritten,
			IntPtr lpOverlapped);

		[DllImport("coredll.dll", EntryPoint="ReadFile", SetLastError=true)]
		internal static extern int ReadFileCE(
			IntPtr hFile,
			byte[] lpBuffer,
			int nNumberOfBytesToRead,
			ref int lpNumberOfBytesRead,
			IntPtr lpOverlapped);

		[DllImport("coredll", EntryPoint="GetFileAttributes", SetLastError=true)]
		internal static extern uint GetFileAttributes(string lpFileName);

		[DllImport("coredll", EntryPoint="SetFileAttributes", SetLastError=true)]
		internal static extern bool SetFileAttributes(string lpFileName, uint dwFileAttributes);

		[DllImport("coredll", EntryPoint="SetFileTime", SetLastError=true)]
		internal static extern bool SetFileTime(IntPtr hFile, byte[] lpCreationTime, byte[] lpLastAccessTime, byte[] lpLastWriteTime); 

		#endregion
	}
	#endregion

	#region File Access Enumeration
	/// <summary>
	/// CreateFile file access flags
	/// </summary>
	[Flags(),CLSCompliant(false)]
	public enum FileAccess : uint
	{
		/// <summary>
		/// Read access to the file.  Data can be read from the file.
		/// </summary>
		Read		= 0x80000000,
		/// <summary>
		/// Write access to the file.  Data can be written to the file.
		/// </summary>
		Write		= 0x40000000,
		/// <summary>
		/// Execute permission. The file can be executed.
		/// </summary>
		Execute		= 0x20000000,
		/// <summary>
		/// All permissions.
		/// </summary>
		All			= 0x10000000
	}

	public enum FileAccessEx
	{
		/// <summary>
		/// Read access to the file. Data can be read from the file. Combine with Write for read/write access.
		/// </summary>
		Read		= 1,
		/// <summary>
		/// Write access to the file.  Data can be written to the file.
		/// </summary>
		Write		= 2,
		/// <summary>
		/// Read and write access to the file. Data can be written to and read from the file.
		/// </summary>
		ReadWrite		= 3
	}
	#endregion

	#region File Share
	/// <summary>
	/// CreateFile file share mode
	/// </summary>
	public enum FileShare : int
	{
		/// <summary>
		/// Declines sharing of the current file.
		/// Any request to open the file (by this process or another process) will fail until the file is closed.
		/// </summary>
		None		= 0,
		/// <summary>
		/// Allows subsequent opening of the file for reading.
		/// If this flag is not specified, any request to open the file for reading (by this process or another process) will fail until the file is closed.
		/// However, if this flag is specified additional permissions might still be needed to access the file.
		/// </summary>
		Read		= 1,
		/// <summary>
		/// Allows subsequent opening of the file for writing.
		/// If this flag is not specified, any request to open the file for writing (by this process or another process) will fail until the file is closed.
		/// However, if this flag is specified additional permissions might still be needed to access the file.
		/// </summary>
		Write		= 2,
		/// <summary>
		/// Allows subsequent opening of the file for reading or writing.
		/// If this flag is not specified, any request to open the file for writing or reading (by this process or another process) will fail until the file is closed.
		/// However, if this flag is specified additional permissions might still be needed to access the file.
		/// </summary>
		ReadWrite	= 3,
		/// <summary>
		/// Allows the file to be deleted.
		/// </summary>
		Delete		= 4,
	}
	#endregion

	#region File Create Disposition
	/// <summary>
	/// Specifies which action to take on files that exist, and which action to take when files do not exist.
	/// </summary>
	public enum FileCreateDisposition : int
	{
		/// <summary>
		/// Creates a new file.
		/// The function fails if the specified file already exists.
		/// </summary>
		CreateNew			= 1,
		/// <summary>
		/// Creates a new file.
		/// If the file exists, the function overwrites the file and clears the existing attributes.
		/// </summary>
		CreateAlways		= 2,
		/// <summary>
		/// Opens the file.
		/// The function fails if the file does not exist.
		/// </summary>
		OpenExisting		= 3,
		/// <summary>
		/// Opens the file, if it exists.
		/// If the file does not exist, the function creates the file as if dwCreationDisposition were <b>CreateNew</b>.
		/// </summary>
		OpenAlways			= 4,
		/// <summary>
		/// Opens the file.
		/// Once opened, the file is truncated so that its size is zero bytes. The calling process must open the file with at least Write access.
		/// </summary>
		TruncateExisting	= 5,
		/// <summary>
		/// 
		/// </summary>
		OpenForLoader		= 6
	}
	#endregion

	#region File Flags
	/// <summary>
	/// CreateFile file flags
	/// </summary>
	[CLSCompliant(false)]
	public enum FileFlags : uint
	{
		/// <summary>
		/// Instructs the system to write through any intermediate cache and go directly to disk.
		/// The system can still cache write operations, but cannot lazily flush them.
		/// </summary>
		WriteThrough				= 0x80000000,
		/// <summary>
		/// This flag is not supported; however, multiple read/write operations pending on a device at a time are allowed.
		/// </summary>
		Overlapped				= 0x40000000,
		/// <summary>
		/// Indicates that the file is accessed randomly.
		/// The system can use this as a hint to optimize file caching.
		/// </summary>
		RandomAccess				= 0x10000000,
		/// <summary>
		/// 
		/// </summary>
		SequentialScan			= 0x08000000,
		/// <summary>
		/// 
		/// </summary>
		DeleteOnClose			= 0x04000000
	}
	#endregion

}
