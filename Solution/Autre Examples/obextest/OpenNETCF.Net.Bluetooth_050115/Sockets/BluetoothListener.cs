//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.BluetoothListener
//		Copyright (C) 2004, OpenNETCF.org
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
using System.Net.Sockets;
using System.Runtime.InteropServices;
using OpenNETCF.Net;
using OpenNETCF.Net.Bluetooth;
using OpenNETCF.Net.Bluetooth.Internal;
using OpenNETCF.Net.Bluetooth.Sdp;


namespace OpenNETCF.Net.Sockets
{
	/// <summary>
	/// Listens for connections from Bluetooth network clients.
	/// </summary>
	public class BluetoothListener : IDisposable
	{
		private BluetoothEndPoint mEP;
		private Socket mSocket;
		private int mServiceHandle;

		/// <summary>
		/// Initializes a new instance of the <see cref="OpenNETCF.Net.Sockets.BluetoothListener"/> class.
		/// </summary>
		/// <param name="ep">The device address to monitor for making a connection.</param>
		public BluetoothListener(BluetoothEndPoint ep)
		{
			mEP = ep;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OpenNETCF.Net.Sockets.BluetoothListener"/> class.
		/// </summary>
		/// <param name="service">The Bluetooth service to listen for.</param>
		public BluetoothListener(Guid service)
		{
			mEP = new BluetoothEndPoint(new BluetoothAddress(), service);
		}

		/// <summary>
		/// Gets a new instance of the <see cref="OpenNETCF.Net.Sockets.BluetoothListener"/> class.  
		/// </summary>
		public BluetoothEndPoint LocalEndPoint
		{
			get
			{
				if(mSocket!=null)
				{
					return (BluetoothEndPoint)mSocket.LocalEndPoint;
				}
				return mEP;
			}
		}

		/// <summary>
		/// Starts the socket to listen for incoming connections.
		/// </summary>
		public void Start()
		{
			if(mSocket==null)
			{
				mSocket = new Socket((AddressFamily)BluetoothAddressFamily.Bluetooth, SocketType.Stream, (ProtocolType)BluetoothProtocolType.RFComm);
				mSocket.Bind(mEP);
				mServiceHandle = SetService(mEP.Service, ((BluetoothEndPoint)mSocket.LocalEndPoint).Port); 
				mSocket.Listen(int.MaxValue);
			}
		}

		/// <summary>
		/// Stops the socket from monitoring connections.
		/// </summary>
		public void Stop()
		{
			if(mSocket!=null)
			{
				//remove service registration
				if(mServiceHandle != 0)
				{
					RemoveService(mServiceHandle);
					mServiceHandle = 0;
				}
				mSocket.Close();
				((IDisposable)mSocket).Dispose();
				mSocket = null;
			}
		}

		/// <summary>
		/// Creates a new socket for a connection.
		/// </summary>
		/// <returns></returns>
		public Socket AcceptSocket()
		{
			if(mSocket==null)
			{
				throw new InvalidOperationException("No Socket");
			}
			return mSocket.Accept();
		}

		/// <summary>
		/// Creates a client object for a connection when the specified service or endpoint is detected by the listener component.
		/// </summary>
		/// <returns>A <see cref="OpenNETCF.Net.Sockets.BluetoothClient"/> component.</returns>
		public BluetoothClient AcceptBluetoothClient()
		{
			Socket s = this.AcceptSocket();
			return new BluetoothClient(s);
		}

		/// <summary>
		/// Determines if there is a connection pending.
		/// </summary>
		/// <returns>true if there is a connection pending; otherwise, false.</returns>
		public bool Pending()
		{
			if(mSocket==null)
			{
				throw new InvalidOperationException("No Socket");
			}
			return mSocket.Poll(0, SelectMode.SelectRead);
		}


		private static void RemoveService(int handle)
		{
			SdpRecordTemp rec = new SdpRecordTemp();
			BTHNS_SETBLOB blob = new BTHNS_SETBLOB(rec.ToByteArray());
			blob.Handle = handle;
			
			WSAQuerySet qs = new WSAQuerySet();
			qs.Blob = blob;

			int hresult;

			if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
			{
				hresult = NativeMethods.CeSetService(qs.ToByteArray(), WSAESETSERVICEOP.RNRSERVICE_DELETE, 0);
			}
			else
			{
				hresult = NativeMethods.XpSetService(qs.ToByteArray(), WSAESETSERVICEOP.RNRSERVICE_DELETE, 0);
			}

			if(hresult != 0)
			{
				int lasterror;

				if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
				{
					lasterror = NativeMethods.CeGetLastError();
				}
				else
				{
					lasterror = NativeMethods.XpGetLastError();
				}
			}
		}

		private static int SetService(Guid service, int channel)
		{
			SdpRecordTemp rec = new SdpRecordTemp();
			//SdpStream ss = new SdpStream();

			rec.Service = HostToNetworkOrder(service);
			//ss.ByteSwapUuid128(service);
			//ss.Dispose();
			rec.Channel = Convert.ToByte(channel);
			BTHNS_SETBLOB blob = new BTHNS_SETBLOB(rec.ToByteArray());
			WSAQuerySet qs = new WSAQuerySet();
			qs.Blob = blob;
			

			int hresult;

			if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
			{
				hresult = NativeMethods.CeSetService(qs.ToByteArray(), WSAESETSERVICEOP.RNRSERVICE_REGISTER, 0);
			}
			else
			{
				hresult = NativeMethods.XpSetService(qs.ToByteArray(), WSAESETSERVICEOP.RNRSERVICE_REGISTER, 0);
			}

			if(hresult != 0)
			{
				int lasterror;

				if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
				{
					lasterror = NativeMethods.CeGetLastError();
				}
				else
				{
					lasterror = NativeMethods.XpGetLastError();
				}

				throw new SocketException(lasterror);
			}

			return blob.Handle;
		}

		

		private static Guid HostToNetworkOrder(Guid hostGuid)
		{
			byte[] guidBytes = hostGuid.ToByteArray();
			
			BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BitConverter.ToInt32(guidBytes, 0))).CopyTo(guidBytes, 0);
			BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BitConverter.ToInt16(guidBytes, 4))).CopyTo(guidBytes, 4);
			BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BitConverter.ToInt16(guidBytes, 6))).CopyTo(guidBytes, 6);
			

			return new Guid(guidBytes);
		}

		

		#region IDisposable Members

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			Stop();

			if(disposing)
			{
				mEP = null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		~BluetoothListener()
		{
			Dispose(false);

		}
		#endregion
	}
}
