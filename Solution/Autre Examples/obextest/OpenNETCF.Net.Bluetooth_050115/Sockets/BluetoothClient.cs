//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.BluetoothClient
//		Copyright (C) 2003-2005, OpenNETCF.org
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
using System.Net;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;
using OpenNETCF.Net.Bluetooth;
using OpenNETCF.Net.Bluetooth.Internal;
using OpenNETCF.Win32;

namespace OpenNETCF.Net.Sockets
{
	/// <summary>
	///  Provides client connections for Bluetooth network services.
	/// </summary>
	/// <remarks>This class currently only supports devices which use the Microsoft Bluetooth stack such as the Orange SPV E200, devices which use the WidComm stack will not work.</remarks>
	public class BluetoothClient : IDisposable
	{
		private Socket mSocket;
		
		//private ArrayList m_devices;
		//length of time for query
		private short m_query = 16;

		#region Constructor
		/// <summary>
		/// Creates a new instance of BluetoothClient.
		/// </summary>
		public BluetoothClient() : this(new Socket((AddressFamily)BluetoothAddressFamily.Bluetooth, SocketType.Stream, (ProtocolType)BluetoothProtocolType.RFComm))
		{
		}

		internal BluetoothClient(Socket s)
		{
			mSocket = s;
		}
		#endregion

		#region Protected
		/// <summary>
		/// Gets or sets the underlying <see cref="Socket"/>.
		/// </summary>
		protected Socket Client
		{
			get
			{
				return mSocket;
			}
		}

		#endregion

		#region Radio Mode
		/// <summary>
		/// Gets or Sets the current mode of operation of the Bluetooth radio.
		/// </summary>
		/// <remarks>This setting will be persisted when the device is reset.
		/// An Icon will be displayed in the tray on the Home screen and the device will emit a blue LED when Bluetooth is enabled.</remarks>
		public static RadioMode RadioMode
		{
			get
			{
				RadioMode mode = 0;
				//get the mode
				int result = NativeMethods.BthGetMode(ref mode);

				//if successful return retrieved value
				if(result != 0)
				{
					throw new ExternalException("Error getting Bluetooth mode: " + result.ToString("X")); 

				}
				
				//return setting
				return mode;
			}
			set
			{
				//set the status
				int result = NativeMethods.BthSetMode(value);

				//check for error
				if(result != 0)
				{
					throw new ExternalException("Error setting Bluetooth mode: " + result.ToString("X"));
				}
			}
		}
		#endregion

		#region Query Length
		/// <summary>
		/// Amount of time allowed to perform the query.
		/// </summary>
		/// <remarks>This value is measured in units of 1.28 seconds (time to query = length * 1.28 seconds).
		/// The default value is 16.</remarks>
		public short QueryLength
		{
			get
			{
				return m_query;
			}
			set
			{
				if(value > 0)
				{
					m_query = value;
				}
				else
				{
					throw new  ArgumentOutOfRangeException("QueryLength must be a positive integer");
				}
			}
		}
		#endregion

		#region Discover Devices
		/// <summary>
		/// Discovers accessible Bluetooth devices and returns their names and addresses.
		/// </summary>
		/// <returns>An array of BluetoothDeviceInfo objects describing the devices discovered.</returns>
		public BluetoothDeviceInfo[] DiscoverDevices(int maxDevices)
		{
			int discoveredDevices = 0;
			ArrayList al = new ArrayList(maxDevices);

			int handle = 0;
			
			byte[] buffer = new byte[1024];
			BitConverter.GetBytes((int)60).CopyTo(buffer, 0);
			BitConverter.GetBytes((int)16).CopyTo(buffer, 20);

			int bufferlen = buffer.Length;

			int lookupresult = 0;

			BTHNS_INQUIRYBLOB bib = new BTHNS_INQUIRYBLOB(m_query, (short)maxDevices);

			
			BLOB b = new BLOB(bib);
			
			GCHandle hBlob = GCHandle.Alloc(b.ToByteArray(), GCHandleType.Pinned);

			
			BitConverter.GetBytes(hBlob.AddrOfPinnedObject().ToInt32() + 4).CopyTo(buffer, 56);


			//start looking for Bluetooth devices
			if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
			{
				lookupresult = NativeMethods.CeLookupServiceBegin(buffer, LookupFlags.Containers, ref handle);
			}
			else
			{
				lookupresult = NativeMethods.XpLookupServiceBegin(buffer, LookupFlags.Containers | LookupFlags.FlushCache, ref handle);
			}

			hBlob.Free();

			while(discoveredDevices < maxDevices && lookupresult != -1)
			{
				if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
				{
					lookupresult = NativeMethods.CeLookupServiceNext(handle, LookupFlags.ReturnAddr | LookupFlags.ReturnName , ref bufferlen, buffer);
				}
				else
				{
					lookupresult = NativeMethods.XpLookupServiceNext(handle, LookupFlags.ReturnAddr | LookupFlags.ReturnName , ref bufferlen, buffer);
				}

				
				if(lookupresult != -1)
				{
					//increment found count
					discoveredDevices++;

				
					//pointer to outputbuffer
					int bufferptr = BitConverter.ToInt32(buffer, 48);
					//remote socket address
					int sockaddrptr = Marshal.ReadInt32((IntPtr)bufferptr, 8);
					//remote socket len
					int sockaddrlen = Marshal.ReadInt32((IntPtr)bufferptr, 12);
					

					SocketAddress btsa = new SocketAddress(AddressFamily.Unspecified, sockaddrlen);
						
					for(int sockbyte = 0; sockbyte < sockaddrlen; sockbyte++)
					{
						btsa[sockbyte] = Marshal.ReadByte((IntPtr)sockaddrptr, sockbyte);
					}

					BluetoothEndPoint bep = new BluetoothEndPoint(null, Guid.Empty);
					bep = (BluetoothEndPoint)bep.Create(btsa);
				
					//new deviceinfo
					BluetoothDeviceInfo newdevice;

					if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
					{
						newdevice = new BluetoothDeviceInfo(bep.Address, Marshal.PtrToStringUni((IntPtr)BitConverter.ToInt32(buffer, 4)));
					}
					else
					{
						newdevice = new BluetoothDeviceInfo(bep.Address, MarshalEx.PtrToStringAuto((IntPtr)BitConverter.ToInt32(buffer, 4)));
					}
					
					//add to discovered list
					al.Add(newdevice);

				}
			}

			//stop looking
			if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
			{
				lookupresult = NativeMethods.CeLookupServiceEnd(handle);
			}
			else
			{
				lookupresult = NativeMethods.XpLookupServiceEnd(handle);
			}
			
			//return results
			return (BluetoothDeviceInfo[])al.ToArray(typeof(BluetoothDeviceInfo));
		}
		#endregion

		#region Bonded Devices
		/// <summary>
		/// Returns details of all devices which have already been discovered and bonded.
		/// </summary>
		/// <remarks>Take care when using these for connections as the device may not be available at the current time.</remarks>
		public BluetoothDeviceInfo[] BondedDevices
		{
			get
			{
				ArrayList a = new ArrayList();

				//open bluetooth device key
				RegistryKey devkey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Bluetooth\\Device");

				//enumerate the keys
				foreach(string devid in devkey.GetSubKeyNames())
				{
					//get friendly name
					RegistryKey thisdevkey = devkey.OpenSubKey(devid);
					string name = thisdevkey.GetValue("name").ToString();
					thisdevkey.Close();

					//add to collection
					BluetoothDeviceInfo thisdevice = new BluetoothDeviceInfo(BluetoothAddress.Parse(devid), name);
					a.Add(thisdevice);
				}

				devkey.Close();

				//return results as fixed size array
				return (BluetoothDeviceInfo[])a.ToArray(typeof(BluetoothDeviceInfo));
			}
		}
		#endregion
	

		#region Connect
		/// <summary>
		/// Connects a client to a specified endpoint.
		/// </summary>
		/// <param name="ep">A <see cref="OpenNETCF.Net.BluetoothEndPoint"/> that represents the remote device.</param>
		public void Connect(BluetoothEndPoint ep)
		{
			Close();
			mSocket.Connect(ep);
		}
		#endregion

		#region Close
		/// <summary>
		/// Closes the socket of the connection.
		/// </summary>
		public void Close()
		{
			if(mSocket.Connected)
			{
				mSocket.Close();
			}
		}
		#endregion

		#region Get Stream
		/// <summary>
		/// Gets the underlying stream of data.
		/// </summary>
		/// <returns></returns>
		public Stream GetStream()
		{
			return new NetworkStream(mSocket);
		}
		#endregion
		

		#region Authenticate
		/// <summary>
		/// Triggers Authentication.
		/// </summary>
		/// <remarks>On connected socket, triggers authentication. On not connected socket, forces authentication on connection.
		/// For incoming connection this means that connection is rejected if authentication cannot be performed.</remarks>
		public void Authenticate()
		{
			mSocket.SetSocketOption((SocketOptionLevel)BluetoothSocketOptionLevel.RFComm, (SocketOptionName)BluetoothSocketOptionName.Authenticate, (int)0);

		}
		#endregion

		#region Authentication Enabled
		/// <summary>
		/// Gets a value to indicated whether Authentication is enabled.
		/// </summary>
		public bool AuthenticationEnabled
		{
			get
			{
				byte[] authbytes = mSocket.GetSocketOption((SocketOptionLevel)BluetoothSocketOptionLevel.RFComm, (SocketOptionName)BluetoothSocketOptionName.GetAuthenticationEnabled, 4);
				int auth = BitConverter.ToInt32(authbytes, 0);
				return auth==0 ? false : true;
				
			}
		}
		#endregion

		#region Class Of Device
		/// <summary>
		/// Returns the Class of Device for the local device.
		/// </summary>
		public ClassOfDevice ClassOfDevice
		{
			get
			{
				byte[] codbytes = mSocket.GetSocketOption((SocketOptionLevel)BluetoothSocketOptionLevel.RFComm, (SocketOptionName)BluetoothSocketOptionName.GetCod, 4);
				int cod = BitConverter.ToInt32(codbytes, 0);
				return (ClassOfDevice)cod;
			}
		}
		#endregion

		#region Encrypt
		/// <summary>
		/// 
		/// </summary>
		public bool Encrypt
		{
			set
			{
				mSocket.SetSocketOption((SocketOptionLevel)BluetoothSocketOptionLevel.RFComm, (SocketOptionName)BluetoothSocketOptionName.Encrypt, value ? -1 : 0);
			}
		}
		#endregion

		#region Hardware Status
		/// <summary>
		/// Returns the current status of the Bluetooth radio hardware.
		/// </summary>
		/// <value>A member of the <see cref="OpenNETCF.Net.Bluetooth.HardwareStatus"/> enumeration.</value>
		public HardwareStatus HardwareStatus
		{
			get
			{
				HardwareStatus status = 0;
				int result = NativeMethods.BthGetHardwareStatus(ref status);

				if(result!=0)
				{
					throw new ExternalException("Error retrieving Bluetooth hardware status");
				}
				return status;
			}
		}
			
		#endregion

		#region Link Key
		/// <summary>
		/// Returns link key associated with peer Bluetooth device.
		/// </summary>
		public Guid LinkKey
		{
			get
			{
				byte[] link = mSocket.GetSocketOption((SocketOptionLevel)BluetoothSocketOptionLevel.RFComm, (SocketOptionName)BluetoothSocketOptionName.GetLink, 32);

				byte[] bytes = new byte[16];
				Buffer.BlockCopy(link, 16, bytes, 0, 16);
				return new Guid(bytes);
			}
		}
		#endregion

		#region Link Policy
		/// <summary>
		/// Returns the Link Policy of the current connection.
		/// </summary>
		public LinkPolicy LinkPolicy
		{
			get
			{
				byte[] policy = mSocket.GetSocketOption((SocketOptionLevel)BluetoothSocketOptionLevel.RFComm, (SocketOptionName)BluetoothSocketOptionName.GetLinkPolicy, 4);
				return (LinkPolicy)BitConverter.ToInt32(policy, 0);
			}
		}
		#endregion

		#region Local Address
		/// <summary>
		/// Returns the address of the local Bluetooth device.
		/// </summary>
		public BluetoothAddress LocalAddress
		{
			get
			{
				BluetoothAddress ba = new BluetoothAddress();

				int result = NativeMethods.BthReadLocalAddr(ba.ToByteArray());

				if(result != 0)
				{
					throw new ExternalException("Error retrieving local Bluetooth address");
				}

				return ba;
			}
		}
		
		#endregion

		#region Local Version

		public BluetoothVersion LocalVersion
		{
			get
			{
				if(System.Environment.OSVersion.Platform==PlatformID.WinCE)
				{
					byte hv = 0;
					ushort hr = 0;
					byte lv = 0;
					ushort ls = 0;
					ushort man = 0;
					byte fea = 0;

					int hresult = BthReadLocalVersion(ref hv, ref hr, ref lv, ref ls, ref man, ref fea);

					return new BluetoothVersion(hv, hr, lv, ls, man, fea);
				}
				else
				{
					byte[] version = mSocket.GetSocketOption((SocketOptionLevel)BluetoothSocketOptionLevel.RFComm, (SocketOptionName)BluetoothSocketOptionName.GetLocalVersion, 16);
					return new BluetoothVersion(version);
				}
			}
		}
		[DllImport("Btdrt.dll", SetLastError=true)]
		private static extern int BthReadLocalVersion(
			ref byte phci_version,
			ref ushort phci_revision,
			ref byte plmp_version,
			ref ushort plmp_subversion,
			ref ushort pmanufacturer,
			ref byte plmp_features);
		#endregion

		#region PairRequest
		/*
		/// <summary>
		/// Attempts a pairing with the specified device and PIN number.
		/// </summary>
		/// <param name="device"></param>
		/// <param name="pin"></param>
		public void PairRequest(BluetoothAddress device, string pin)
		{
			ushort handle = 0;

			SetPin(device, pin);

			int success = NativeMethods.BthCreateACLConnection(device.ToByteArray(), ref handle);

			success = NativeMethods.BthAuthenticate(device.ToByteArray());

			success = NativeMethods.BthCloseConnection(handle);
		}*/
		#endregion

		#region Pin
		/// <summary>
		/// Sets the PIN associated with the currently connected device.
		/// </summary>
		/// <value>PIN which must be composed of 1 to 16 ASCII characters.
		/// Assigning null (Nothing in VB) or an empty String will revoke the PIN.</value>
		public string Pin
		{
			set
			{
				SetPin(null, value);
			}
		}

		/// <summary>
		/// Set or change the PIN to be used with a specific remote device.
		/// </summary>
		/// <param name="device">Address of Bluetooth device.</param>
		/// <param name="pin">PIN string consisting of 1-16 ASCII characters.</param>
		public void SetPin(BluetoothAddress device, string pin)
		{
			if(pin.Length < 1 | pin.Length > 16)
			{
				throw new ArgumentException("Pin must be between 1 and 16 characters long.");
			}
			byte[] pinbytes = System.Text.Encoding.ASCII.GetBytes(pin);
			int len = pin.Length;
			
			int result = NativeMethods.BthSetPIN(device.ToByteArray(), len, pinbytes);

			if(result != 0)
			{
				int error = Marshal.GetLastWin32Error();
				throw new System.ComponentModel.Win32Exception(error, "Error setting PIN");
			}

			/*byte[] link = new byte[32];

			//copy remote device address
			if(device != null)
			{
				Buffer.BlockCopy(device.ToByteArray(), 0, link, 8, 6);
			}

			//copy PIN
			if(pin!=null & pin.Length > 0)
			{
				if(pin.Length > 16)
				{
					throw new ArgumentOutOfRangeException("PIN must be between 1 and 16 characters");
				}
				//copy pin bytes
				byte[] pinbytes = System.Text.Encoding.ASCII.GetBytes(pin);
				Buffer.BlockCopy(pinbytes, 0, link, 16, pin.Length);
				BitConverter.GetBytes(pin.Length).CopyTo(link, 0);
			}	
				
			mSocket.SetSocketOption((SocketOptionLevel)BluetoothSocketOptionLevel.RFComm, (SocketOptionName)BluetoothSocketOptionName.SetPin, link);*/
		}

		/// <summary>
		/// Revoke a previously assigned PIN for connecting to the specified device.
		/// </summary>
		/// <param name="device">Address of Bluetooth device.</param>
		public void RevokePin(BluetoothAddress device)
		{
			int result = NativeMethods.BthRevokePIN(device.ToByteArray());

			if(result != 0)
			{
				int error = Marshal.GetLastWin32Error();
				throw new System.ComponentModel.Win32Exception(error, "Error setting PIN");
			}

			//SetPin(device, null);
		}
		#endregion

		#region Remote Machine Name
		/// <summary>
		/// Gets the name of the remote device.
		/// </summary>
		public string RemoteMachineName
		{
			get
			{
				return GetRemoteMachineName(mSocket);
			}
		}
		

		/// <summary>
		/// Gets the name of a device by a specified socket.
		/// </summary>
		/// <param name="s"> A <see cref="Socket"/>.</param>
		/// <returns>Returns a string value of the computer or device name.</returns>
		public static string GetRemoteMachineName(Socket s)
		{
			byte[] buffer = new byte[504];
			//copy remote device address to buffer
			Buffer.BlockCopy(((BluetoothEndPoint)s.RemoteEndPoint).Address.ToByteArray(), 0, buffer, 0, 6);

			try
			{
				s.SetSocketOption((SocketOptionLevel)BluetoothSocketOptionLevel.RFComm, (SocketOptionName)BluetoothSocketOptionName.ReadRemoteName, buffer);
				string name = System.Text.Encoding.Unicode.GetString(buffer, 8, 496);
				int offset = name.IndexOf('\0');
				if(offset > -1)
				{
					name = name.Substring(0, offset);
				}

				return name;
			}
			catch(SocketException ex)
			{
				return null;
			}

		}
		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{
			if(mSocket != null)
			{
				if(mSocket.Connected)
				{
					mSocket.Close();
				}

				mSocket = null;
			}
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Frees resources used by the <see cref="BluetoothClient"/> class.
		/// </summary>
		~BluetoothClient()
		{
			Dispose(false);
		}

		#endregion
	}

	

	
}
