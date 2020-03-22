//==========================================================================================
//
//		OpenNETCF.IO.FileSystemWatcher
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
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;


namespace OpenNETCF.IO
{

	/// <summary>
	/// Represents the method that will handle the <see cref="FileSystemWatcher.Changed"/>, <see cref="FileSystemWatcher.Created"/>, or <see cref="FileSystemWatcher.Deleted"/> event of a <see cref="FileSystemWatcher"/> class.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="FileSystemEventArgs"/> that contains the event data.</param>
	/// <seealso cref="FileSystemEventArgs"/>
	/// <seealso cref="RenamedEventArgs"/>
	public delegate void FileSystemEventHandler(object sender, FileSystemEventArgs e);
	
	/// <summary>
	/// Represents the method that will handle the <see cref="FileSystemWatcher.Renamed"/> event of a <see cref="FileSystemWatcher"/> class.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RenamedEventArgs"/> that contains the event data.</param>
	/// <seealso cref="RenamedEventArgs"/>
	/// <seealso cref="FileSystemEventHandler"/>
	/// <seealso cref="FileSystemEventArgs"/>
	public delegate void RenamedEventHandler(object sender, RenamedEventArgs e);


	#region Enums
	/// <summary>
	/// Changes that might occur to a file or directory.
	/// </summary>
	/// <remarks>Each <see cref="WatcherChangeTypes"/> member is associated with an event in <see cref="FileSystemWatcher"/>.
	/// For more information on the events, see <see cref="FileSystemWatcher.Changed"/>, <see cref="FileSystemWatcher.Created"/>, <see cref="FileSystemWatcher.Deleted"/> and <see cref="FileSystemWatcher.Renamed"/>.</remarks>
	public enum WatcherChangeTypes
	{
		/// <summary>
		/// The creation, deletion, change, or renaming of a file or folder. 
		/// </summary>
		All  = 15,
		/// <summary>
		/// The change of a file or folder. The types of changes include: changes to size, attributes, security settings, last write, and last access time.
		/// </summary>
		Changed = 4,
		/// <summary>
		/// The creation of a file or folder.
		/// </summary>
		Created  = 1,
		/// <summary>
		/// The deletion of a file or folder.
		/// </summary>
		Deleted = 2, 
		/// <summary>
		/// The renaming of a file or folder.
		/// </summary>
		Renamed = 8 
	}

	/// <summary>
	/// Specifies changes to watch for in a file or folder.
	/// </summary>
	/// <remarks>You can combine the members of this enumeration to watch for more than one kind of change. For example, you can watch for changes in the size of a file or folder, and for changes in security settings. This raises an event anytime there is a change in size or security settings of a file or folder.</remarks>
	/// <seealso cref="FileSystemWatcher"/>
	/// <seealso cref="FileSystemEventArgs"/>
	/// <seealso cref="FileSystemEventHandler"/>
	/// <seealso cref="RenamedEventArgs"/>
	/// <seealso cref="RenamedEventHandler"/>
	/// <seealso cref="WatcherChangeTypes"/>
	public enum NotifyFilters
	{
		/// <summary>
		/// The attributes of the file or folder.
		/// </summary>
		Attributes = 4,
		/// <summary>
		/// The time the file or folder was created.
		/// </summary>
		CreationTime = 64,
		/// <summary>
		/// The name of the directory.
		/// </summary>
		DirectoryName = 2, 
		/// <summary>
		/// The name of the file.
		/// </summary>
		FileName = 1,
		/// <summary>
		/// The date the file or folder was last opened.
		/// </summary>
		LastAccess = 32, 
		/// <summary>
		/// The date the file or folder last had anything written to it.
		/// </summary>
		LastWrite = 16, 
		/// <summary>
		/// The security settings of the file or folder.
		/// </summary>
		Security = 256, 
		/// <summary>
		/// The size of the file or folder.
		/// </summary>
		Size = 8, 
	}
	#endregion

	#region FileSystemEventArgs
	/// <summary>
	/// Provides data for the directory events: <see cref="FileSystemWatcher.Changed"/>, <see cref="FileSystemWatcher.Created"/>, <see cref="FileSystemWatcher.Deleted"/>.
	/// </summary>
	/// <remarks>The <b>FileSystemEventArgs</b> class is passed as a parameter to event handlers for these events:
	/// <para>The <see cref="FileSystemWatcher.Changed"/> event occurs when changes are made to the size, system attributes, last write time, last access time, or security permissions in a file or directory in the specified <see cref="FileSystemWatcher.Path"/> of a <see cref="FileSystemWatcher"/>.</para>
	/// <para>The <see cref="FileSystemWatcher.Created"/> event occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> of a <see cref="FileSystemWatcher"/> is created.</para>
	/// <para>The <see cref="FileSystemWatcher.Deleted"/> event occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> of a <see cref="FileSystemWatcher"/> is deleted. For more information, see <see cref="FileSystemWatcher"/>.</para></remarks>
	public class FileSystemEventArgs
	{
		private string fullPath = "";
		private string name = "";
		internal WatcherChangeTypes changeType = WatcherChangeTypes.All;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:OpenNETCF.IO.FileSystemEventArgs"/> class.
		/// </summary>
		/// <param name="changeType">One of the <see cref="T:OpenNETCF.IO.WatcherChangeTypes"/> values, which represents the kind of change detected in the file system.</param>
		/// <param name="directory">The root directory of the affected file or directory.</param>
		/// <param name="name">The name of the affected file or directory.</param>
		public FileSystemEventArgs(WatcherChangeTypes changeType, string directory, string name) : base() 
		{
			this.changeType = changeType;
			this.name = name;
			if (!(directory.EndsWith("\\")))
				directory = directory + "\\";
			this.fullPath = directory + name;
		}

//		internal FileSystemEventArgs(WatcherChangeTypes chgType, string flPath)
//		{
//			changeType = chgType;
//			fullPath = flPath;
//			name = flPath;
//		}

		/// <summary>
		/// Gets the type of directory event that occurred.
		/// </summary>
		/// <value>One of the <see cref="WatcherChangeTypes"/> values that represents the kind of change detected in the file system.</value>
		/// <seealso cref="FileSystemEventArgs"/>
		/// <seealso cref="WatcherChangeTypes"/>
		public WatcherChangeTypes ChangeType 
		{
			get 
			{
				return changeType;					
			}
		}
		/// <summary>
		/// Gets the fully qualifed path of the affected file or directory.
		/// </summary>
		/// <value>The path of the affected file or directory.</value>
		public string FullPath 
		{
			get 
			{
			   return fullPath;					
			}
		}
		/// <summary>
		/// Gets the name of the affected file or directory.
		/// </summary>
		/// <value>The name of the affected file or directory.</value>
		public string Name 
		{
			get 
			{
				return name;					
			}
		}
	}



	/// <summary>
	/// Provides data for the Renamed event.
	/// </summary>
	public class RenamedEventArgs : FileSystemEventArgs
	{
		private string oldFullPath;
		private string oldName;

		
		public RenamedEventArgs(
			WatcherChangeTypes changeType,
			string directory,
			string name,
			string oldName
			): base(changeType, directory, name)
		{
			this.oldFullPath = directory;
			this.oldName = oldName;
		}

		/// <summary>
		/// Gets the previous fully qualified path of the affected file or directory.
		/// </summary>
		public string OldFullPath 
		{
			get
			{
				return oldFullPath;
			}
		}

		/// <summary>
		/// Gets the old name of the affected file or directory.
		/// </summary>
		public string OldName 
		{
			get
			{
				return oldName;
			}
		}
		
	}


	#endregion

	#region FileSystemWatcher class
	/// <summary>
	/// Listens to the file system change notifications and raises events when a directory, or file in a directory, changes.
	/// </summary>
	public class FileSystemWatcher : System.ComponentModel.Component, IDisposable		
	{
		private string path = "";
		internal string filter = "*.*";
		private bool enableRaisingEvents = false;
		private bool includeSubdirectories = false;
		internal NotifyFilters notifyFilter;
		//hide from NDoc
#if !NDOC
		private WindowSink windowSink;
#endif
		internal string directory = "";
		//private Control synchronizingObject;

		//internal Queue dlgQueue = new Queue(); 
		
		//private FileSystemEventHandler onChangedHandler;

		/// <summary>
		/// Occurs when a file or directory in the specified <see cref="P:OpenNETCF.IO.FileSystemWatcher.Path"/> is created.
		/// </summary>
		public event FileSystemEventHandler Created;
		/// <summary>
		/// Occurs when a file or directory in the specified <see cref="P:OpenNETCF.IO.FileSystemWatcher.Path"/> is changed.
		/// </summary>
		public event FileSystemEventHandler Changed;
		/// <summary>
		/// Occurs when a file or directory in the specified <see cref="P:OpenNETCF.IO.FileSystemWatcher.Path"/> is deleted.
		/// </summary>
		public event FileSystemEventHandler Deleted;
		/// <summary>
		/// Occurs when a file or directory in the specified <see cref="P:OpenNETCF.IO.FileSystemWatcher.Path"/> is renamed.
		/// </summary>
		public event RenamedEventHandler Renamed;

		/// <summary>
		/// Initializes a new instance of the <b>FileSystemWatcher</b> class.
		/// </summary>
		public FileSystemWatcher()
		{
			notifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName;
#if !NDOC
			windowSink = new WindowSink(this);
#endif

		}

		/// <summary>
		/// Initializes a new instance of the <b>FileSystemWatcher</b> class, given the specified directory to monitor.
		/// </summary>
		/// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
		public FileSystemWatcher(string path): this()
		{
			this.Path = path;
		}
		
		/// <summary>
		/// Initializes a new instance of the <b>FileSystemWatcher</b> class, given the specified directory and type of files to monitor.
		/// </summary>
		/// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
		/// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
		public FileSystemWatcher(string path, string filter): this()
		{
			this.Path = path;
			this.Filter = filter;
			
		}

		/// <summary>
		/// Gets or sets a value indicating whether the component is enabled.
		/// </summary>
		public bool EnableRaisingEvents
		{
			set
			{
				if (enableRaisingEvents != value)
				{
					if (enableRaisingEvents == false)
					{	
						if (!enableRaisingEvents)
						{
							Remove();
							AddDir();
						}
					}
					else
					{
						if (enableRaisingEvents)
							Remove();
					}
					enableRaisingEvents = value;
				}
			}
			get
			{
				return enableRaisingEvents;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether subdirectories within the specified path should be monitored.
		/// </summary>
		public bool IncludeSubdirectories 
		{
			set
			{
				includeSubdirectories = value;
			}
			get
			{
				return includeSubdirectories;

			}
		}

		
		/// <summary>
		/// Gets or sets the type of changes to watch for.
		/// </summary>
		public NotifyFilters NotifyFilter 
		{
			set
			{
				notifyFilter = value;
			}
			get
			{
				return notifyFilter;

			}
		}


		/// <summary>
		/// Gets or sets the path of the directory to watch.
		/// </summary>
		public string Path
		{
			set
			{
				object[] obj;

				if (value!=null)
				{
					if (!(Directory.Exists(value))) 
					{
						obj = new Object[1];
						obj[0] = value;
						throw new ArgumentException("Invalid Directory Name", value);
					}
				    this.directory = value;
				}
				
				path = value;
				path+='\0';
			}
			get
			{
				return directory;

			}
		}

		/// <summary>
		/// Gets or sets the filter string, used to determine what files are monitored in a directory.
		/// </summary>
		public string Filter
		{
			set
			{
				if (value == null || value == String.Empty)
							value = "*.*";
				if (String.Compare(this.filter, value, true) != 0)
							this.filter = value;
			}
			get
			{
				return filter;

			}

		}

		//
//		public Control SynchronizingObject
//		{
//			set
//			{
//				synchronizingObject = value;
//			}
//			get
//			{
//				return synchronizingObject;
//
//			}
//		}


		internal void Remove()
		{
#if !NDOC
			SHChangeNotifyDeregister(windowSink.Hwnd);
#endif
		}

		internal void AddDir()
		{	
			SHCHANGENOTIFYENTRY notEntry = new SHCHANGENOTIFYENTRY();

			//Set mask
			notEntry.dwEventMask = (int)SHCNE_ALLEVENTS;
			//notEntry.dwEventMask = (int)SHCNE_ATTRIBUTES | (int)SHCNE_UPDATEDIR | (int)SHCNE_UPDATEITEM;
			//notEntry.dwEventMask = (int)SHCNE_UPDATEDIR | (int)SHCNE_UPDATEITEM;
			
			notEntry.fRecursive = BOOL(includeSubdirectories);

			//Set watch dir
			IntPtr lpWatchDir = StringToHLocal(path);
			notEntry.pszWatchDir = lpWatchDir;
			
			//Call API
#if !NDOC
			int res = SHChangeNotifyRegister(windowSink.Hwnd, ref notEntry);
#endif
							
		}
       
		//Destructor
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="FileSystemWatcher"/> is reclaimed by garbage collection.
		/// </summary>
		~FileSystemWatcher()
		{
			this.Dispose(true);
		}

		#region virtual events

		
		private void OnInternalFileSystemEventArgs(object sender, FileSystemEventArgs e) 
		{
			lock (this)
				//if (!(this.isChanged)) 
				{
					//this.changedResult = new WaitForChangedResult(e.ChangeType, e.Name, false);
					//this.isChanged = 1;
					//Monitor.Pulse(this);
				}
		}


		public virtual void OnChanged(FileSystemEventArgs e)
		{
			lock (this)
			{	
				if (this.Changed!=null)
				{
					this.Changed(this, e);
				}
			}
		}

		public virtual void OnCreated(FileSystemEventArgs e)
		{
			lock (this)
			{
				if (this.Created!=null)
					this.Created(this, e);
			}
		}

		public virtual void OnDeleted(FileSystemEventArgs e)
		{
			lock (this)
			{
				if (this.Deleted!=null)
					this.Deleted(this, e);
			}
		}

		public virtual void OnRenamed(RenamedEventArgs e)
		{
			lock (this)
			{
				if (this.Renamed!=null)
					this.Renamed(this, e);
			}
		}


		#endregion

		#region API Declarations

		private const uint LMEM_FIXED = 0;
		private const uint LMEM_ZEROINIT = 0x0040;
		private const uint LPTR = (LMEM_FIXED | LMEM_ZEROINIT);


		internal static int BOOL(bool value)
		{
			if (value)
				return 1;
			else
				return 0;
		}

		[DllImport("coredll.dll")]
		internal static extern IntPtr LocalAlloc(
			uint uFlags, 
			uint uBytes ); 
		
		[DllImport("coredll.dll")]
		internal static extern IntPtr LocalFree(
			IntPtr hMem ); 


		internal static IntPtr AllocHGlobal(int cb) 
		{
			IntPtr hMem;

			hMem = LocalAlloc(0, (uint)cb);
			if (hMem == IntPtr.Zero)
				throw new OutOfMemoryException();
			return hMem;
		}

		private static IntPtr AllocHLocal( int bytes )
		{
			return( LocalAlloc(LPTR, (uint)bytes) );
		}


		internal static IntPtr StringToHLocal( string s )
		{
			if( s == null ) 
			{
				return( IntPtr.Zero );
			} 
			else 
			{
				int nc = s.Length;
				int len = 2*(1+nc);
				IntPtr hLocal = AllocHLocal( len );
				if( hLocal == IntPtr.Zero ) 
				{
					throw new OutOfMemoryException();
				} 
				else 
				{
					Marshal.Copy( s.ToCharArray(), 0, hLocal, s.Length );
					return( hLocal );
				}
			}
		}


		internal static void FreeHGlobal(IntPtr hglobal) 
		{
			LocalFree(hglobal);
		}
		[DllImport("aygshell.dll")]
		internal static extern int SHChangeNotifyRegister (
			IntPtr  hwnd,
			ref SHCHANGENOTIFYENTRY pshcne );

		[DllImport("aygshell.dll")]
		internal static extern int SHChangeNotifyRegister (
			IntPtr  hwnd,
			IntPtr pshcne );


		[DllImport("aygshell.dll")]
		internal static extern void SHChangeNotifyFree(IntPtr pshcne);

		[DllImport("aygshell.dll")]
		internal static extern int SHChangeNotifyDeregister(
			IntPtr  hwnd);


		internal struct SHCHANGENOTIFYENTRY 
		{
			internal int  dwEventMask;
			internal IntPtr  pszWatchDir;
			internal int  fRecursive;
		} 

		
		internal const long SHCNE_RENAME	        =  0x00000001L;   // GOING AWAY
		internal const long SHCNE_RENAMEITEM     =     0x00000001L;
		internal const long SHCNE_CREATE	        =  0x00000002L;
		internal const long SHCNE_DELETE	        =  0x00000004L;
		internal const long SHCNE_MKDIR	        =      0x00000008L;
		internal const long SHCNE_RMDIR          =     0x00000010L;
		internal const long SHCNE_MEDIAINSERTED  =     0x00000020L;
		internal const long SHCNE_MEDIAREMOVED   =     0x00000040L;
		internal const long SHCNE_DRIVEREMOVED   =     0x00000080L;
		internal const long SHCNE_DRIVEADD       =     0x00000100L;
		internal const long SHCNE_NETSHARE       =     0x00000200L;
		internal const long SHCNE_NETUNSHARE     =     0x00000400L;
		internal const long SHCNE_ATTRIBUTES     =     0x00000800L;
		internal const long SHCNE_UPDATEDIR      =     0x00001000L;
		internal const long SHCNE_UPDATEITEM     =     0x00002000L;
		internal const long SHCNE_SERVERDISCONNECT =   0x00004000L;
		internal const long SHCNE_UPDATEIMAGE      =   0x00008000L;
		internal const long SHCNE_DRIVEADDGUI      =   0x00010000L;
		internal const long SHCNE_RENAMEFOLDER     =   0x00020000L;
		internal const long SHCNE_ALLEVENTS       =    0x7FFFFFFFL;

		internal const int WM_FILECHANGEINFO  = (0x8000 + 0x101);


		internal struct FILECHANGENOTIFY 
		{
			internal int  dwRefCount;
			internal int cbSize;
			internal int wEventId;
			internal uint uFlags;             
			internal int dwItem1;            
			internal int dwItem2;
			internal int dwAttributes;  
			internal int dwLowDateTime; 
			internal int dwHighDateTime;   
			internal uint nFileSize;      
		} 


		#endregion


		#region IDisposable Members

		private bool disposed = false;

		/// <summary>
		/// Releases the resources used by the <see cref="FileSystemWatcher"/>.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="FileSystemWatcher"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if(!this.disposed)
			{
				// Release unmanaged resources. If disposing is false, 
				// only the following code is executed.
				if (enableRaisingEvents)
					this.Remove();

			}
			disposed = true;         
		}

		#endregion

		#region WindowSink
#if !NDOC
		internal class WindowSink : Microsoft.WindowsCE.Forms.MessageWindow
		{

			private FileSystemWatcher watcher;

			public WindowSink(FileSystemWatcher w)
			{
				watcher = w;	
			}
			
			//helper function
			private bool ValidateByFilter(string fileName)
			{
				
				if (watcher.filter != "*.*")
				{
					string filterName = System.IO.Path.GetFileNameWithoutExtension(watcher.filter);
					string filterExt = System.IO.Path.GetExtension(watcher.filter);
						
					if (filterName == "*") //check ext only
					{
						if (System.IO.Path.GetExtension(fileName).ToLower() != filterExt.ToLower())
						{
							return false;
						}
						else 
							return true;
					}
					else //we've got name in the filter
					{
						if (filterExt != ".*") //star in the ext
						{
							if (System.IO.Path.GetFileNameWithoutExtension(fileName).ToLower() != filterName.ToLower())
							{
								return  false;
							}
							else
								return true;
						}
						else //name and ext supplied
						{
							if (System.IO.Path.GetFileNameWithoutExtension(fileName).ToLower() != filterName.ToLower())
							{
								return  false;
							}
							else
								return true;

						}
					}

				}
				return true;

			}


			protected override void WndProc(ref Microsoft.WindowsCE.Forms.Message msg)
			{
							 
				if (msg.Msg == WM_FILECHANGEINFO)
				{
					if (msg.LParam == IntPtr.Zero)
						return;
					
					if ((int)msg.LParam == 0)
						return;				
	

					FILECHANGENOTIFY fchnot = (FILECHANGENOTIFY)Marshal.PtrToStructure(msg.LParam, typeof(FILECHANGENOTIFY));
								
					string fullPath = Marshal.PtrToStringUni((IntPtr)fchnot.dwItem1);
					string oldfullPath = Marshal.PtrToStringUni((IntPtr)fchnot.dwItem2);
					string fileName = System.IO.Path.GetFileName(fullPath);
					string dirName = System.IO.Path.GetDirectoryName(fullPath);
					string oldfileName = "";
			
			
					if (ValidateByFilter(fileName))
					{
						FileSystemEventArgs args;
						RenamedEventArgs renArgs;
								
						switch (fchnot.wEventId)
						{
							case (int)SHCNE_CREATE:
								if (watcher.notifyFilter == NotifyFilters.FileName)
								{
									args = new FileSystemEventArgs(WatcherChangeTypes.Created, dirName, fileName);
									watcher.OnCreated(args);
								}
								break;
							case (int)SHCNE_MKDIR:
								if ((watcher.notifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
								{
									args = new FileSystemEventArgs(WatcherChangeTypes.Created, dirName, fileName);
									watcher.OnCreated(args);
								}
								break;
							case (int)SHCNE_UPDATEDIR:		
								if ((watcher.notifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
								{
									args = new FileSystemEventArgs(WatcherChangeTypes.Changed, dirName, fileName);
									watcher.OnChanged(args);
								}
								break;		
						   case (int)SHCNE_RMDIR:		
								if ((watcher.notifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
								{
									args = new FileSystemEventArgs(WatcherChangeTypes.Deleted, dirName, fileName);
									watcher.OnChanged(args);
								}
								break;
							case (int)SHCNE_DELETE:
								if ((watcher.notifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName)
								{
									args = new FileSystemEventArgs(WatcherChangeTypes.Deleted, dirName, fileName);
									watcher.OnDeleted(args);
								}
								break;
							case (int)SHCNE_UPDATEITEM:
								if ((watcher.notifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName)
								{
									args = new FileSystemEventArgs(WatcherChangeTypes.Changed, dirName, fileName);
									watcher.OnChanged(args);
								}
								break;
							case (int)SHCNE_RENAMEFOLDER:
								if ((watcher.notifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
								{
									 oldfileName = Marshal.PtrToStringUni((IntPtr)fchnot.dwItem2);
									renArgs = new RenamedEventArgs(WatcherChangeTypes.Renamed, dirName, fileName, oldfileName);
									watcher.OnRenamed(renArgs);
								}
								break;
							case (int)SHCNE_RENAME:	
								if ((watcher.notifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName)
								{
									oldfileName = Marshal.PtrToStringUni((IntPtr)fchnot.dwItem2);
									renArgs = new RenamedEventArgs(WatcherChangeTypes.Renamed, dirName, fileName, oldfileName);
									watcher.OnRenamed(renArgs);
								}
								break;
			
							case (int)SHCNE_ATTRIBUTES:
								if ((watcher.notifyFilter & NotifyFilters.Attributes) == NotifyFilters.Attributes)
								{
									args = new FileSystemEventArgs(WatcherChangeTypes.Changed, dirName, fileName);
									watcher.OnChanged(args);
								}
								break;
			
						}
					}
			
					SHChangeNotifyFree(msg.LParam);
				}
			
				msg.Result = (IntPtr)0;
				base.WndProc(ref msg);					
			}

		}
#endif


		#endregion
	}

	#endregion

}
