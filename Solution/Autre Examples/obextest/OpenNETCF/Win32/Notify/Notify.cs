//==========================================================================================
//
//		OpenNETCF.Win32.Notify
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
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Win32.Notify
{
	#region Notify
	/// <summary>
	/// Contains Notification related Methods
	/// </summary>
	public class Notify
	{		
		private Notify(){}

		#region Run App At Event
		/// <summary>   
		/// This function starts running an application when a specified event occurs.   
		/// </summary>   
		/// <param name="appName">Name of the application to be started.</param>   
		/// <param name="whichEvent">Event at which the application is to be started.</param>   
		/// <seealso cref="T:OpenNETCF.Win32.Core.EventNotifications"/>   
		public static void RunAppAtEvent(string appName, NotificationEvent whichEvent)   
		{   
			if(!CeRunAppAtEvent(appName, (int)whichEvent))   
			{   
				throw new WinAPIException("Cannot Set Notification Handler");   
			}   
		} 
		#endregion
    
		#region Run App At Time
		/// <summary>   
		/// This function prompts the system to start running a specified application at a specified time.   
		/// </summary>   
		/// <param name="appName">Name of the application to be started.</param>   
		/// <param name="time">DateTime at which to run application.</param>
		/// <remarks>To cancel an existing RunAppATime request pass the application name and DateTime.MinValue</remarks>
		public static void RunAppAtTime(string appName, System.DateTime time)   
		{   
			byte[] timebytes;

			if(time==System.DateTime.MinValue)
			{
				//cancel request pass null
				timebytes = null;
			}
			else
			{
				//get native system time struct
				SystemTime st = SystemTime.FromDateTime(time);
				//get bytes
				timebytes = st.ToByteArray();
			}

			if(!CeRunAppAtTime(appName, timebytes))   
			{   
				throw new WinAPIException("Cannot Set Notification Handler");   
			}  
		}   
		#endregion

		#region Set User Notification
		/// <summary>
		/// Creates a new user notification.
		/// </summary>
		/// <param name="application">String that specifies the name of the application that owns this notification.</param>
		/// <param name="time">The time when the notification should occur.</param>
		/// <param name="notify">Notification object that describes the events that are to occur when the notification time is reached.</param>
		/// <returns>The handle to the notification indicates success.</returns>
		public static int SetUserNotification(string application, System.DateTime time, UserNotification notify)
		{
			return SetUserNotification(0, application, time, notify);
		}
		/// <summary>
		/// Edit an existing user notification.
		/// </summary>
		/// <param name="handle">Handle to the notification to overwrite.</param>
		/// <param name="application">String that specifies the name of the application that owns this notification.</param>
		/// <param name="time">The time when the notification should occur.</param>
		/// <param name="notify">Notification object that describes the events that are to occur when the notification time is reached.</param>
		/// <returns>The handle to the notification indicates success.</returns>
		public static int SetUserNotification(int handle, string application, System.DateTime time, UserNotification notify)
		{
			//call api function
			int outhandle = CeSetUserNotification(handle, application, SystemTime.FromDateTime(time).ToByteArray(), notify.data); 
			
			//if invalid handle throw exception
			if(outhandle==0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(),"Error setting UserNotification");
			}
			return outhandle;
		}
		/// <summary>
		/// This function creates a new user notification.
		/// </summary>
		/// <param name="trigger">A UserNotificationTrigger that defines what event activates a notification.</param>
		/// <param name="notification">A UserNotification that defines how the system should respond when a notification occurs.</param>
		/// <returns>Handle to the notification event if successful.</returns>
		public static int SetUserNotification(UserNotificationTrigger trigger, UserNotification notification)
		{
			int outhandle = CeSetUserNotificationEx(0, trigger.data, notification.data);
			
			//throw on invalid handle
			if(outhandle==0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(),"Error setting UserNotification");
			}
			return outhandle;
		}
		/// <summary>
		/// This function modifies an existing user notification.
		/// </summary>
		/// <param name="handle">Handle of the Notification to be modified</param>
		/// <param name="trigger">A UserNotificationTrigger that defines what event activates a notification.</param>
		/// <param name="notification">A UserNotification that defines how the system should respond when a notification occurs.</param>
		/// <returns>Handle to the notification event if successful.</returns>
		public static int SetUserNotification(int handle, UserNotificationTrigger trigger, UserNotification notification)
		{
			int outhandle = CeSetUserNotificationEx(handle, trigger.data, notification.data);
			
			//throw on invalid handle
			if(outhandle==0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error setting UserNotification");
			}
			return outhandle;
		}
		#endregion

		#region Clear User Notification
		/// <summary>
		/// Deletes a registered user notification that was created by a previous call to the SetUserNotification function.
		/// </summary>
		/// <param name="handle">Handle to the user notification to delete.</param>
		/// <returns>TRUE indicates success. FALSE indicates failure.</returns>
		/// <remarks>ClearNotification does not operate on notifications that have occurred.</remarks>
		public static bool ClearUserNotification(int handle)
		{
			return CeClearUserNotification(handle);
		}
		#endregion

		#region Get User Notification
		/// <summary>
		/// Retrieves notification information associated with a handle.
		/// </summary>
		/// <param name="handle">Handle to the user notification to retrieve.</param>
		/// <returns>The requested UserNotification.</returns>
		public static UserNotificationInfoHeader GetUserNotification(int handle)
		{
			//buffer size
			int size = 0;

			//first query for buffer size required
			CeGetUserNotification(handle, 0, ref size, IntPtr.Zero);

			//create a marshallable buffer
			IntPtr buffer = MarshalEx.AllocHGlobal(size);

			//call native getter
			if(!CeGetUserNotification(handle, (uint)size, ref size, buffer))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error getting UserNotification");
			}

			UserNotificationInfoHeader nih = UserNotificationInfoHeader.FromPtr(buffer);

			//free native memory
			MarshalEx.FreeHGlobal(buffer);

			return nih;
		}
		#endregion

		#region Get User Notification Handles
		/// <summary>
		/// Returns an array of currently stored notifications.
		/// </summary>
		/// <returns>Array of currently stored notifications.</returns>
		public static int[] GetUserNotificationHandles()
		{
			int size = 0;
			//get size required
			if(!CeGetUserNotificationHandles(null, 0, ref size))
			{
				throw new WinAPIException("Error retrieving handles");
			}
			//create array to fill
			int[] handles = new int[size];
			//this time pass the buffer to be filled
			if(!CeGetUserNotificationHandles(handles, size, ref size))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error retrieving handles");
			}

			//return populated handles array
			return handles;
		}
		#endregion

		#region Get User Notification Preferences
		/// <summary>
		/// This function queries the user for notification settings by displaying a dialog box showing options that are valid for the current hardware platform.
		/// </summary>
		/// <param name="hWnd">Handle to the parent window for the notification settings dialog box.</param>
		/// <returns>A UserNotification structure containing the user's notification settings.</returns>
		public static UserNotification GetUserNotificationPreferences(IntPtr hWnd)
		{
			UserNotification template = new UserNotification();

			return GetUserNotificationPreferences(hWnd, template);

		}
		/// <summary>
		/// This function queries the user for notification settings by displaying a dialog box showing options that are valid for the current hardware platform.
		/// </summary>
		/// <param name="hWnd">Handle to the parent window for the notification settings dialog box.</param>
		/// <param name="template">UserNotification structure used to populate the default settings.</param>
		/// <returns>A UserNotification structure containing the user's notification settings.</returns>
		public static UserNotification GetUserNotificationPreferences(IntPtr hWnd, UserNotification template)
		{
			if(!CeGetUserNotificationPreferences(hWnd, template.data))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Could not get user preferences");
			}
			return template;
		}
		#endregion

		#region Handle App Notifications
		/// <summary>
		/// This function marks as "handled" all notifications previously registered by the given application that have occurred.
		/// </summary>
		/// <param name="application">The name of the application whose events are to be marked as "handled".
		/// This must be the name that was passed in to <see cref="M:OpenNETCF.Win32.Notify.Notify.SetUserNotification"/> as the owner of the notification.</param>
		public static void HandleAppNotifications(string application)
		{
			//throw exception on failure
			if(!CeHandleAppNotifications(application))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error clearing Application Notifications");
			}
		}
		#endregion
  

		#region Notify P/Invokes

		[DllImport("coredll", EntryPoint="CeRunAppAtEvent", SetLastError=true)]   
		private static extern bool CeRunAppAtEvent(string pwszAppName, int lWhichEvent);   
    
		[DllImport("coredll", EntryPoint="CeRunAppAtTime", SetLastError=true)]   
		private static extern bool CeRunAppAtTime(string pwszAppName, byte[] lpTime);   

		[DllImport("coredll", EntryPoint="CeSetUserNotificationEx", SetLastError=true)]   
		private static extern int CeSetUserNotificationEx(int hNotification, byte[] lpTrigger, byte[] lpUserNotification);

		[DllImport("coredll", EntryPoint="CeSetUserNotification", SetLastError=true)]   
		private static extern int CeSetUserNotification(int hNotification, string pwszAppName, byte[] lpTime, byte[] lpUserNotification);

		[DllImport("coredll", EntryPoint="CeClearUserNotification", SetLastError=true)]   
		private static extern bool CeClearUserNotification(int hNotification);
		
		[DllImport("coredll", EntryPoint="CeGetUserNotification", SetLastError=true)]   
		private static extern bool CeGetUserNotification (int hNotification, uint cBufferSize, ref int pcBytesNeeded, IntPtr pBuffer );

		[DllImport("coredll", EntryPoint="CeGetUserNotificationHandles", SetLastError=true)]   
		private static extern bool CeGetUserNotificationHandles(int[] rghNotifications, int cHandles, ref int pcHandlesNeeded);

		[DllImport("coredll", EntryPoint="CeGetUserNotificationPreferences", SetLastError=true)]   
		private static extern bool CeGetUserNotificationPreferences(IntPtr hWndParent, byte[] lpNotification);

		[DllImport("coredll", EntryPoint="CeHandleAppNotifications", SetLastError=true)]   
		private static extern bool CeHandleAppNotifications(string appName);

		#endregion 
	}
	#endregion

	#region User Notification
	/// <summary>
	/// This structure contains information used to initialize the user notifications settings dialog box, and receives the user’s notification preferences entered by way of the dialog box.
	/// Also used when setting a user notification. 
	/// </summary>
	public class UserNotification
	{
		internal byte[] data;

		#region Constructor
		/// <summary>
		/// Create a new instance of the UserNotification class
		/// </summary>
		public UserNotification()
		{
			data = new byte[24];

			//set maxsound to maxpath * charsize (512)
			BitConverter.GetBytes((int)512).CopyTo(data, 16);
		}
		#endregion

		#region From Pointer
		/// <summary>
		/// Returns a UserNotification object from a specified memory location.
		/// </summary>
		/// <param name="data">Pointer to UserNotification in unmanaged memory.</param>
		/// <returns>A new UserNotification object.</returns>
		/// <remarks>This method is used internally and should not be required in normal use.</remarks>
		public static UserNotification FromPtr(IntPtr data)
		{
			//create new notification instance
			UserNotification newnotification = new UserNotification();

			//set action flags
			newnotification.Action = (NotificationAction)Marshal.ReadInt32(data, 0);
			//System.Windows.Forms.MessageBox.Show(newnotification.Action.ToString());

			//set title
			newnotification.Title = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(data, 4));

			//set text
			newnotification.Text = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(data, 8));

			//set sound
			newnotification.Sound = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(data, 12));

			return newnotification;
		}
		#endregion

		#region Destructor
		/// <summary>
		/// This member overrides <see cref="M:System.Object.Finalize">Object.Finalize</see>.
		/// </summary>
		~UserNotification()
		{
			//free up native memory
			IntPtr titleptr = (IntPtr)BitConverter.ToInt32(data, 4);
			if(titleptr!=IntPtr.Zero)
			{
				MarshalEx.FreeHGlobal(titleptr);
			}
			IntPtr textptr = (IntPtr)BitConverter.ToInt32(data, 8);
			if(textptr!=IntPtr.Zero)
			{
				MarshalEx.FreeHGlobal(textptr);
			}
			IntPtr soundptr = (IntPtr)BitConverter.ToInt32(data, 12);
			if(soundptr!=IntPtr.Zero)
			{
				MarshalEx.FreeHGlobal(soundptr);
			}
			/*sDialogTitleHandle.Free();
			sDialogTextHandle.Free();
			sSoundHandle.Free();*/
		}
		#endregion

		#region Action
		/// <summary>
		/// Any combination of the <see cref="T:OpenNETCF.Win32.Notify.NotificationAction"/> members.  
		/// </summary>
		/// <value>Flags which specifies the action(s) to be taken when the notification is triggered.</value>
		/// <remarks>Flags not valid on a given hardware platform will be ignored.</remarks>
		public NotificationAction Action
		{
			get
			{
				return (NotificationAction)BitConverter.ToInt32(data, 0);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(data, 0);
			}
		}
		#endregion

		#region Title
		/// <summary>
		/// Required if NotificationAction.Dialog is set, ignored otherwise
		/// </summary>
		public string Title
		{
			get
			{
				IntPtr titleptr = (IntPtr)BitConverter.ToInt32(data, 4);
				if(titleptr!=IntPtr.Zero)
				{
					return Marshal.PtrToStringUni(titleptr);
				}
				else
				{
					return null;
				}
			}
			set
			{
				IntPtr titleptr = (IntPtr)BitConverter.ToInt32(data, 4);
				
				//free up previous value
				if(titleptr!=IntPtr.Zero)
				{
					MarshalEx.FreeHGlobal(titleptr);
				}
				//marshal string to unmanaged memory
				titleptr = MarshalEx.StringToHGlobalUni(value);
				//store ptr in bytearray
				BitConverter.GetBytes((int)titleptr).CopyTo(data, 4);
			}
		}
		#endregion
		
		#region Text
		/// <summary>
		/// Required if NotificationAction.Dialog is set, ignored otherwise.
		/// </summary>
		public string Text
		{
			get
			{
				IntPtr textptr = (IntPtr)BitConverter.ToInt32(data, 8);

				if(textptr!=IntPtr.Zero)
				{
					return Marshal.PtrToStringUni(textptr);
				}
				else
				{
					return null;
				}
			}
			set
			{
				//free up previous value
				IntPtr textptr = (IntPtr)BitConverter.ToInt32(data, 8);

				if(textptr!=IntPtr.Zero)
				{
					MarshalEx.FreeHGlobal(textptr);
				}
				//marshal string to unmanaged memory
				textptr = MarshalEx.StringToHGlobalUni(value);
				//store ptr in bytearray
				BitConverter.GetBytes((int)textptr).CopyTo(data, 8);			
			}
		}
		#endregion

		#region Sound
		/// <summary>
		/// Sound string as supplied to PlaySound.
		/// </summary>
		/// <remarks>SetUserNotification() ignores it if the <see cref="P:OpenNETCF.Win32.Notify.UserNotification.Action">Action property</see> does not contain <see cref="T:OpenNETCF.Win32.Notify.NotifyAction">NotifyAction.Sound</see>.</remarks>
		public string Sound
		{
			get
			{
				IntPtr soundptr = (IntPtr)BitConverter.ToInt32(data, 12);

				if(soundptr!=IntPtr.Zero)
				{
					return Marshal.PtrToStringUni(soundptr);
				}
				else
				{
					return null;
				}
			}
			set
			{
				//free up previous value
				IntPtr soundptr = (IntPtr)BitConverter.ToInt32(data, 12);

				if(soundptr!=IntPtr.Zero)
				{
					MarshalEx.FreeHGlobal(soundptr);
				}
				//marshal string to unmanaged memory
				soundptr = MarshalEx.StringToHGlobalUni(value);
				//store ptr in bytearray
				BitConverter.GetBytes((int)soundptr).CopyTo(data, 12);
			}
		}
		#endregion
	}
	#endregion

	#region User Notification Info Header
	/// <summary>
	/// Contains information about notification events.
	/// </summary>
	public class UserNotificationInfoHeader
	{
		private byte[] data;

		private UserNotificationTrigger trigger;
		private UserNotification notify;

		#region Constructor
		/// <summary>
		/// Create a new instance of UserNotificationInfoHeader
		/// </summary>
		public UserNotificationInfoHeader()
		{
			//16 byte structure
			data = new byte[16];
		}
		#endregion

		#region From Pointer
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pointer"></param>
		/// <returns></returns>
		internal static UserNotificationInfoHeader FromPtr(IntPtr pointer)
		{
			UserNotificationInfoHeader nih = new UserNotificationInfoHeader();

			Marshal.Copy(pointer, nih.data, 0, 16);

			//marshall trigger
			nih.trigger = UserNotificationTrigger.FromPtr((IntPtr)BitConverter.ToInt32(nih.data, 8));

			//marshall notification
			IntPtr notifyptr = (IntPtr)BitConverter.ToInt32(nih.data, 12);
			//if null pointer no notification
			if(notifyptr!=IntPtr.Zero)
			{
				//else convert to managed notificatio object
				nih.notify = UserNotification.FromPtr(notifyptr);
			}

			return nih;
		}
		#endregion

		#region Handle
		/// <summary>
		/// Handle to the notification.
		/// </summary>
		public int Handle
		{
			get
			{
				return BitConverter.ToInt32(data, 0);
			}
		}
		#endregion

		#region Status
		/// <summary>
		/// Indicates current state of the notification.
		/// </summary>
		public NotificationStatus Status
		{
			get
			{
				return (NotificationStatus)BitConverter.ToInt32(data, 4);
			}
		}
		#endregion

		#region User Notification Trigger
		/// <summary>
		/// The UserNotificationTrigger object
		/// </summary>
		public UserNotificationTrigger UserNotificationTrigger
		{
			get
			{
				return trigger;
			}
		}
		#endregion

		#region User Notification
		/// <summary>
		/// The UserNotification object.
		/// </summary>
		public UserNotification UserNotification
		{
			get
			{
				return notify;
			}
		}
		#endregion
	}
	#endregion

	#region User Notification Trigger
	/// <summary>
	/// Defines what event activates a notification.
	/// </summary>
	public class UserNotificationTrigger
	{
		internal byte[] data;

		private char[] sApplication;
		GCHandle sApplicationHandle;

		private char[] sArgs;
		GCHandle sArgsHandle;

		#region Constructor
		/// <summary>
		/// Create a new instance of UserNotificationTrigger
		/// </summary>
		public UserNotificationTrigger()
		{
			data = new byte[52];

			sApplication = new char[OpenNETCF.IO.FileEx.MaxPath];
			sApplicationHandle = GCHandle.Alloc(sApplication, GCHandleType.Pinned);
			BitConverter.GetBytes((int)sApplicationHandle.AddrOfPinnedObject() + 4).CopyTo(data, 12);

			sArgs = new char[OpenNETCF.IO.FileEx.MaxPath];
			sArgsHandle = GCHandle.Alloc(sArgs, GCHandleType.Pinned);
			BitConverter.GetBytes((int)sArgsHandle.AddrOfPinnedObject() + 4).CopyTo(data, 16);

		}
		#endregion

		#region From Pointer
		/// <summary>
		/// Returns a NotificationTrigger object from a native pointer.
		/// </summary>
		/// <param name="pointer">Native memory pointer.</param>
		/// <returns>New managed NotificationTrigger object.</returns>
		public static UserNotificationTrigger FromPtr(IntPtr pointer)
		{
			UserNotificationTrigger trigger = new UserNotificationTrigger();

			//copy the bytes
			Marshal.Copy(pointer, trigger.data, 0, 52);

			//clear the existing string pointers
			BitConverter.GetBytes((int)0).CopyTo(trigger.data, 12);
			BitConverter.GetBytes((int)0).CopyTo(trigger.data, 16);

			//marshal the strings
			trigger.Application = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pointer, 12));
			trigger.Arguments = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pointer, 16));

			return trigger;
		}
		#endregion

		#region Type
		/// <summary>
		/// Specifies the type of notification.
		/// </summary>
		public NotificationType Type
		{
			get
			{
				return (NotificationType)BitConverter.ToInt32(data, 4);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(data, 4);
			}
		}
		#endregion

		#region Event
		/// <summary>
		/// Specifies the type of event should Type = Event.
		/// </summary>
		public NotificationEvent Event
		{
			get
			{
				return (NotificationEvent)BitConverter.ToInt32(data, 8);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(data, 8);
			}
		}
		#endregion

		#region Application
		/// <summary>
		/// Name of the application to execute.
		/// </summary>
		public string Application
		{
			get
			{
				IntPtr appptr = (IntPtr)BitConverter.ToInt32(data, 12);
				if(appptr!=IntPtr.Zero)
				{
					return Marshal.PtrToStringUni(appptr);
				}
				else
				{
					return null;
				}
			}
			set
			{
				IntPtr appptr = (IntPtr)BitConverter.ToInt32(data, 12);
				//if native string already allocated
				if(appptr!=IntPtr.Zero)
				{
					//free previous value
					MarshalEx.FreeHGlobal(appptr);
				}
				appptr = MarshalEx.StringToHGlobalUni(value);
				//put value into array
				BitConverter.GetBytes((int)appptr).CopyTo(data, 12);
			}
		}
		#endregion

		#region Arguments
		/// <summary>
		/// Command line (without the application name). 
		/// </summary>
		public string Arguments
		{
			get
			{
				IntPtr argptr = (IntPtr)BitConverter.ToInt32(data, 16);
				if(argptr!=IntPtr.Zero)
				{
					return Marshal.PtrToStringUni(argptr);
				}
				else
				{
					return null;
				}
			}
			set
			{
				IntPtr argptr = (IntPtr)BitConverter.ToInt32(data, 16);
				//if native string already allocated
				if(argptr!=IntPtr.Zero)
				{
					//free previous value
					MarshalEx.FreeHGlobal(argptr);
				}
				argptr = MarshalEx.StringToHGlobalUni(value);
				//put value into array
				BitConverter.GetBytes((int)argptr).CopyTo(data, 16);
			}
		}
		#endregion

		#region Start Time
		/// <summary>
		/// Specifies the beginning of the notification period.
		/// </summary>
		public DateTime StartTime
		{
			get
			{
				byte[] datebytes = new byte[16];
				Buffer.BlockCopy(data, 20, datebytes, 0, 16);
				SystemTime st = new SystemTime(datebytes);
				
				return st.ToDateTime();
			}
			set
			{
				byte[] datebytes = SystemTime.FromDateTime(value).ToByteArray();
				Buffer.BlockCopy(datebytes, 0, data, 20, 16);
			}
		}
		#endregion

		#region End Time
		/// <summary>
		/// Specifies the end of the notification period. 
		/// </summary>
		public DateTime EndTime
		{
			get
			{
				byte[] datebytes = new byte[16];
				Buffer.BlockCopy(data, 36, datebytes, 0, 16);
				
				SystemTime st = new SystemTime(datebytes);
				
				return st.ToDateTime();
			}
			set
			{
				byte[] datebytes = SystemTime.FromDateTime(value).ToByteArray();
				Buffer.BlockCopy(datebytes, 0, data, 36, 16);
			}
		}
		#endregion
	}
	#endregion

	#region Notification Event
	/// <summary>   
	/// System Event Flags   
	/// </summary>   
	public enum NotificationEvent : int   
	{   
		/// <summary>   
		/// No events—remove all event registrations for this application.   
		/// </summary>   
		None                    = 0x00,   
		/// <summary>   
		/// When the system time is changed.   
		/// </summary>   
		TimeChange              = 0x01,   
		/// <summary>   
		/// When data synchronization finishes.   
		/// </summary>   
		SyncEnd                 = 0x02,   
		/// <summary>   
		/// When a PC Card device is changed.   
		/// </summary>   
		DeviceChange    = 0x07,   
		/// <summary>   
		/// When an RS232 connection is made.   
		/// </summary>   
		RS232Detected   = 0x09,   
		/// <summary>   
		/// When a full device data restore completes.   
		/// </summary>   
		RestoreEnd              = 0x0A,   
		/// <summary>   
		/// When the device wakes up.   
		/// </summary>   
		Wakeup                  = 0x0B,   
		/// <summary>   
		/// When the time zone is changed.   
		/// </summary>   
		TimeZoneChange  = 0x0C,
		/// <summary>
		/// When the machines name changes.
		/// Requires Windows CE.NET 4.2.
		/// </summary>
		MachineNameChange = 0x0D,
	}   
	#endregion

	#region Notification Action
	/// <summary>
	/// Specifies the action to take when a notification event occurs.
	/// </summary>
	[Flags()]
	public enum NotificationAction : int
	{
		/// <summary>
		/// Flashes the LED.
		/// </summary>
		Led = 1,
		/// <summary>
		/// Vibrates the device.
		/// </summary>
		Vibrate = 2,
		/// <summary>
		/// Displays the user notification dialog box.
		/// </summary>
		Dialog = 4,
		/// <summary>
		/// Plays the sound specified.
		/// </summary>
		Sound = 8,
		/// <summary>
		/// Repeats the sound for 10–15 seconds.
		/// </summary>
		Repeat = 16,
		/// <summary>
		/// Dialog box z-order flag.
		/// Set if the notification dialog box should come up behind the password.
		/// </summary>
		Private = 32,
	}
	#endregion

	#region Notification Status
	/// <summary>
	/// The status of the notification.
	/// </summary>
	public enum NotificationStatus : int
	{
		/// <summary>
		/// The notification is not currently active.
		/// </summary>
		Inactive = 0,
		/// <summary>
		/// The notification is currently active.
		/// </summary>
		Signalled = 1,
	}
	#endregion

	#region Notification Type
	/// <summary>
	/// Specifies the type of notification.
	/// </summary>
	public enum NotificationType :int
	{
		/// <summary>
		/// System event notification.
		/// </summary>
		Event = 1,
		/// <summary>
		/// Time-based notification.
		/// </summary>
		Time = 2,
		/// <summary>
		/// Time-based notification that is active for the time period between <see cref="M:OpenNETCF.Win32.Notify.NotificationTrigger.StartTime"/> and <see cref="M:OpenNETCF.Win32.Notify.NotificationTrigger.EndTime"/>.
		/// </summary>
		Period = 3,
		/// <summary>
		/// Equivalent to using the SetUserNotification function.
		/// The standard command line is supplied.
		/// </summary>
		ClassicTime = 4,
	}
	#endregion
	
	#region Notification Command Line
	/// <summary>
	/// Strings passed on the command line when an event occurs that the app has requested via CeRunAppAtEvent.  
	/// </summary>
	/// <remarks>Note that some of these strings will be used as the command line *prefix*, since the rest of the command line will be used as a parameter.</remarks>
	public enum NotificationCommandLine
	{
		/// <summary>
		/// String passed on the command line when an app is run as the result of a call to <see cref="M:OpenNETCF.Win32.Notify.Notify.RunAppAtTime"/>.
		/// </summary>
		AppRunAtTime,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application that "owns" a notification.  It is followed by a space, and the stringized version of the notification handle.
		/// </summary>
		AppRunToHandleNotification,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application when the system time settings are changed.
		/// </summary>
		AppRunAfterTimeChange,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application after synchronisation.
		/// </summary>
		AppRunAfterSync,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application when the device is connected to AC power.
		/// </summary>
		AppRunAtAcPowerOn,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application when the AC power is disconnected.
		/// </summary>
		AppRunAtAcPowerOff,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application when the device connects to a LAN.
		/// </summary>
		AppRunAtNetConnect,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application when the device disconnects from a LAN.
		/// </summary>
		AppRunAtNetDisconnect,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application that "owns" a notification.  It is followed by a space, and the stringized version of the notification handle.
		/// </summary>
		AppRunAtDeviceChange,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application when another device is discovered using IR.
		/// </summary>
		AppRunAtIrDiscovery,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application when a serial port connection is attempted.
		/// </summary>
		AppRunAtRs232Detect,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application after a system restore.
		/// </summary>
		AppRunAfterRestore,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application when the device wakes up from standby.
		/// </summary>
		AppRunAfterWakeup,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application when the device time-zone is changed.
		/// </summary>
		AppRunAfterTzChange,
		/// <summary>
		/// Prefix of the command line when the user requests to run the application after an extended event.
		/// </summary>
		AppRunAfterExtendedEvent,
	}
	#endregion
}
