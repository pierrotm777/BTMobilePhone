//==========================================================================================
//
//		OpenNETCF.IO.StreamInterfaceDriver
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
using System.Runtime.InteropServices;

namespace OpenNETCF.IO
{
	/// <summary>
	/// Use this abstract base class to create wrappers around Stream Interface
	/// Drivers that are not supported by the CF
	/// <para><b>New in v1.1</b></para>
	/// </summary>
	public abstract class StreamInterfaceDriver
	{
		IntPtr m_hPort		= IntPtr.Zero;
		string m_portName	= null;

		/// <summary>
		/// Create an instance of the StreamInterfaceDriver class
		/// </summary>
		/// <param name="portName">Name of port (prefix and index) to open</param>
		protected StreamInterfaceDriver(string portName)
		{
			m_portName = portName;
		}

		/// <summary>
		/// Open the driver
		/// <see cref="FileAccess"/>
		/// <see cref="FileShare"/>
		/// </summary>
		/// <param name="access">File Access (Read, Write or Both)</param>
		/// <param name="share">Share Mode (Read, Write or both)</param>
		[CLSCompliant(false)]
		protected void Open(OpenNETCF.IO.FileAccess access, OpenNETCF.IO.FileShare share)
		{
			m_hPort = FileEx.CreateFile(m_portName, access, share, IO.FileCreateDisposition.OpenExisting, 0);

			if((int)m_hPort == -1)
			{
				throw new Win32.WinAPIException("Cannot open driver", Marshal.GetLastWin32Error());
			}
		}

		/// <summary>
		/// Read data from the driver
		/// </summary>
		/// <param name="bytesToRead">The number of bytes requested.</param>
		/// <returns>A byte array returned by the driver</returns>
		protected byte[] Read(int bytesToRead)
		{
			byte[] buffer = new byte[bytesToRead];
			int read = 0;

			FileEx.ReadFile(m_hPort, buffer, bytesToRead, ref read);

			return buffer;			
		}

		/// <summary>
		/// Write data to the driver
		/// </summary>
		/// <param name="data">Data to write</param>
		/// <returns>Number of bytes actually written</returns>
		protected int Write(byte[] data)
		{
			int written = 0;

			FileEx.WriteFile(m_hPort, data, data.Length, ref written);

			return written;
		}

		/// <summary>
		/// Call a device specific IOControl
		/// </summary>
		/// <param name="controlCode">The IOCTL code</param>
		/// <param name="inData">Data to pass into the IOCTL</param>
		/// <param name="outData">Data returned by the IOCTL</param>
		protected void DeviceIoControl(int controlCode, byte[] inData, byte[] outData)
		{
			int xfer = 0;
			if(FileEx.DeviceIoControlCE(m_hPort, controlCode, inData, inData.Length, outData, outData.Length, ref xfer, IntPtr.Zero) == 0)
			{
				throw new Win32.WinAPIException("IOControl call failed", Marshal.GetLastWin32Error());
			}
		}

		/// <summary>
		/// This function moves the file pointer of an open file
		/// <seealso cref="FileEx.MoveMethod"/>
		/// </summary>
		/// <param name="distance">Bytes to move - a positive number moves forward, a negative moves backward</param>
		/// <param name="seekFrom">Starting position from where distance is measured</param>
		/// <returns>New file position</returns>
		/// <remarks>The current file position can be queried using seekFrom(0, MoveMethod.CurrentPosition)</remarks>
		protected int Seek(int distance, FileEx.MoveMethod seekFrom)
		{
			int dist = FileEx.SetFilePointerCE(m_hPort, distance, 0, seekFrom);
			
			if(dist == 0)
			{
				throw new Win32.WinAPIException("Seek Failed", Marshal.GetLastWin32Error());
			}

			return dist;
		}

		/// <summary>
		/// Close the driver
		/// </summary>
		protected void Close()
		{
			FileEx.CloseHandle(m_hPort);
		}
	}
}
