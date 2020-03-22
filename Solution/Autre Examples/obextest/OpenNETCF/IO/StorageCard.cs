//==========================================================================================
//
//		OpenNETCF.IO.StorageCard
//		Copyright (C) 2003-2004, OpenNETCF.org
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
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenNETCF.IO
{
	#region Storage Card
	/// <summary>
	/// Provides access to removable storage cards on the device
	/// </summary>
	public class StorageCard
	{
		private StorageCard(){}

		//storage cards are directories with the temporary attribute
		private const System.IO.FileAttributes attrStorageCard = System.IO.FileAttributes.Directory | System.IO.FileAttributes.Temporary;

		#region Get StorageCard Names
		// Based on original sample code by Alex Feinman:-
		// http://www.opennetcf.org/forums/topic.asp?TOPIC_ID=432

		/// <summary>
		/// Returns the Storage Cards currently attached to the device 
		/// </summary>
		/// <returns>Array containing the names of Storage Cards currently attached to the device.</returns>
		public static string[] GetStorageCardNames()
		{
			ArrayList scards = new ArrayList();

			DirectoryInfo rootDir = new DirectoryInfo(@"\");
			
			foreach(DirectoryInfo di in rootDir.GetDirectories() )
			{
				//if directory and temporary
				if ( (di.Attributes & attrStorageCard) == attrStorageCard )
				{
					//add to collection of storage cards
					scards.Add(di.Name);
				}
			}
			return (string[])scards.ToArray(typeof(string));
		}
		#endregion

		#region Get StorageCards
		/// <summary>
		/// Returns an array of <see cref="T:System.IO.DirectoryInfo"/> entries listing all the Storage Cards currently attached
		/// </summary>
		/// <returns>An array of <see cref="T:System.IO.DirectoryInfo"/> entries.</returns>
		public static DirectoryInfo[] GetStorageCards()
		{
			ArrayList scards = new ArrayList();

			DirectoryInfo rootDir = new DirectoryInfo(@"\");
			
			foreach( DirectoryInfo di in rootDir.GetDirectories() )
			{
				if ( (di.Attributes & attrStorageCard) == attrStorageCard )
				{
					scards.Add(di);
				}
			}
			return (DirectoryInfo[])scards.ToArray(Type.GetType("System.IO.DirectoryInfo"));
		}
		#endregion

		#region Get Documents Folder
		/*/// <summary>
		/// Gets the path of the user documents folder on the specified storage card
		/// </summary>
		/// <param name="storageCard">Path to storage card e.g. "\Storage Card"</param>
		/// <returns>Path to user documents folder e.g. "\Storage Card\My Documents"</returns>
		public static string GetDocumentsFolder(string storageCard)
		{
			//create buffer (Max Path)
			char[] buffer = new char[256];

			//get documents folder for supplied storage card
			bool success = SHGetDocumentsFolder(storageCard, ref buffer);

			if(!success)
			{
				Console.WriteLine(System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString());
			}

			//return documents folder as string
			return new string(buffer).TrimEnd('\0');
		}

		[DllImport("ceshell.dll", EntryPoint="#75", SetLastError=true)]
		private static extern bool SHGetDocumentsFolder(string pszVolume, ref char[] pszDocs);
		*/
		#endregion

		#region GetFreeSpace
		/// <summary>
		/// Obtains the following information about the amount of space available on a disk volume: the total amount of space, the total amount of free space, and the amount of free space available to the user associated with the calling thread.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		/// <param name="directoryName"><see cref="System.String"/> that specifies a directory on the specified disk.
		/// This string can be a Universal Naming Convention (UNC) name.</param>
		/// <returns>A <see cref="DiskFreeSpace"/> structure containing details about the space available on the specified storage media.</returns>
		public static DiskFreeSpace GetDiskFreeSpace(string directoryName)
		{
			DiskFreeSpace result = new DiskFreeSpace();

			if(!GetDiskFreeSpaceEx(directoryName, ref result.FreeBytesAvailable, ref result.TotalBytes, ref result.TotalFreeBytes))
			{
				throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Error retrieving free disk space");
			}

			return result;
		}

		/// <summary>
		/// Describes the free space on a storage card.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		public struct DiskFreeSpace
		{
			/// <summary>
			/// The total number of free bytes on the disk that are available to the user associated with the calling thread.
			/// </summary>
			public long FreeBytesAvailable;
			/// <summary>
			/// The total number of bytes on the disk that are available to the user associated with the calling thread.
			/// </summary>
			public long TotalBytes;
			/// <summary>
			/// The total number of free bytes on the disk.
			/// </summary>
			public long TotalFreeBytes;
		}

		[DllImport("coredll")]
		private static extern bool GetDiskFreeSpaceEx(string directoryName, ref long freeBytesAvailable, ref long totalBytes, ref long totalFreeBytes);

		#endregion

		#region AutoRun Path
		/// <summary>
		/// Returns the path Windows CE will use to search for an AutoRun application when a Storage Card is inserted
		/// </summary>
		/// <remarks>Requires a Pocket PC or Windows CE.NET 4.2 with AYGShell extensions.</remarks>
		public static string AutoRunPath
		{
			get
			{
				//create buffer (Max Path)
				char[] buffer = new char[256];

				//get documents folder for supplied storage card
				bool result = SHGetAutoRunPath(buffer);

				//return documents folder as string
				return new String(buffer).TrimEnd('\0');
			}
		}

		[DllImport("aygshell.dll", EntryPoint="SHGetAutoRunPath", SetLastError=true)]
		private static extern bool SHGetAutoRunPath(char[]  pAutoRunPath );

		#endregion
	}
	#endregion
}
