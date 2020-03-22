using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.Bluetooth
{
	internal class NativeMethods
	{
		private NativeMethods(){}

		//Authenticate

		[DllImport("Btdrt.dll", SetLastError=true)]
		public static extern int BthAuthenticate(byte[] pbt);


		//BthCloseConnection

		[DllImport("Btdrt.dll", SetLastError=true)]
		public static extern int BthCloseConnection(ushort handle);


		//BthCreateACLConnection

		[DllImport("Btdrt.dll", SetLastError=true)]
		public static extern int BthCreateACLConnection(byte[] pbt, ref ushort phandle);


		//BthGetHardwareStatus

		[DllImport("Btdrt.dll", SetLastError=true)]
		public static extern int BthGetHardwareStatus(ref HardwareStatus pistatus);


		//BthReadLocalAddr

		[DllImport("Btdrt.dll", SetLastError=true)]
		public static extern int BthReadLocalAddr(byte[] pba);


		//BthRevokePin

		[DllImport("btdrt.dll", SetLastError=true)]
		public static extern int BthRevokePIN(byte[] pba);


		//BthSetMode

		[DllImport("BthUtil.dll", SetLastError=true)]
		public static extern int BthSetMode(
			RadioMode dwMode );

		[DllImport("BthUtil.dll", SetLastError=true)]
		public static extern int BthGetMode(
			ref RadioMode dwMode );


		//BthSetPin

		[DllImport("btdrt.dll", SetLastError=true)]
		public static extern int BthSetPIN(byte[] pba, int cPinLength, byte[] ppin);


		//GetLastError
		
		[DllImport("ws2.dll", EntryPoint="WSAGetLastError", SetLastError=true)]
		public static extern int CeGetLastError();

		[DllImport("ws2_32.dll", EntryPoint="WSAGetLastError", SetLastError=true)]
		public static extern int XpGetLastError();


		//SetService

		[DllImport("ws2.dll", EntryPoint="WSASetService", SetLastError=true)]
		public static extern int CeSetService(
			byte[] lpqsRegInfo,
			WSAESETSERVICEOP essoperation,
			int dwControlFlags);

		[DllImport("ws2_32.dll", EntryPoint="WSASetService", SetLastError=true)]
		public static extern int XpSetService(
			byte[] lpqsRegInfo,
			WSAESETSERVICEOP essoperation,
			int dwControlFlags);


		//LookupService

		[DllImport("ws2.dll", EntryPoint="WSALookupServiceBegin", SetLastError=true)]
		public static extern int CeLookupServiceBegin(
			byte[] pQuerySet,
			LookupFlags dwFlags,
			ref int lphLookup);

		[DllImport("Ws2_32.dll", EntryPoint="WSALookupServiceBegin", SetLastError=true)]
		public static extern int XpLookupServiceBegin(
			byte[] pQuerySet,
			LookupFlags dwFlags,
			ref int lphLookup);

		[DllImport("ws2.dll", EntryPoint="WSALookupServiceNext", SetLastError=true)]
		public static extern int CeLookupServiceNext(
			int hLookup,
			LookupFlags dwFlags,
			ref int lpdwBufferLength,
			byte[] pResults);

		[DllImport("Ws2_32.dll", EntryPoint="WSALookupServiceNext", SetLastError=true)]
		public static extern int XpLookupServiceNext(
			int hLookup,
			LookupFlags dwFlags,
			ref int lpdwBufferLength,
			byte[] pResults);

		[DllImport("ws2.dll", EntryPoint="WSALookupServiceEnd", SetLastError=true)]
		public static extern int CeLookupServiceEnd(
			int hLookup);

		[DllImport("Ws2_32.dll", EntryPoint="WSALookupServiceEnd", SetLastError=true)]
		public static extern int XpLookupServiceEnd(
			int hLookup);
		

	}

	internal enum WSAESETSERVICEOP : int
	{
		/// <summary>
		/// Register the service. For SAP, this means sending out a periodic broadcast.
		/// This is an NOP for the DNS namespace.
		/// For persistent data stores, this means updating the address information. 
		/// </summary>
		RNRSERVICE_REGISTER = 0,
		/// <summary>
		///  Remove the service from the registry.
		///  For SAP, this means stop sending out the periodic broadcast.
		///  This is an NOP for the DNS namespace.
		///  For persistent data stores this means deleting address information. 
		/// </summary>
		RNRSERVICE_DEREGISTER,
		/// <summary>
		/// Delete the service from dynamic name and persistent spaces.
		/// For services represented by multiple CSADDR_INFO structures (using the SERVICE_MULTIPLE flag), only the specified address will be deleted, and this must match exactly the corresponding CSADDR_INFO structure that was specified when the service was registered 
		/// </summary>
		RNRSERVICE_DELETE, 

	}

	internal enum LookupFlags : uint
	{
		Containers = 0x0002,
		ReturnName = 0x0010,
		ReturnAddr = 0x0100,
		ReturnBlob = 0x0200,
		FlushCache = 0x1000,
	}
}
