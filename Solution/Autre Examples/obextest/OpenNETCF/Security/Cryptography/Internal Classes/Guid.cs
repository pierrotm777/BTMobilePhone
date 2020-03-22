//==========================================================================================
//
//		OpenNETCF.Windows.Forms.Guid
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
//		!!! A HUGE thank-you goes out to Casey Chesnut for supplying this class library !!!
//      !!! You can contact Casey at http://www.brains-n-brawn.com                      !!!
//
//==========================================================================================
using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Security.Cryptography.NativeMethods
{
	public class Guid
	{
		[DllImport("ole32.dll", SetLastError=true)]
		private extern static uint CoCreateGuid(byte [] rawVal);

		/// <summary>
		/// returns preferred NewGuidXXX()
		/// </summary>
		public static System.Guid NewGuid()
		{
			return NewGuidCrypto();
			//return NewGuidOle(); //MissingMethodException
		}

		/// <summary>
		/// pInvokes ole32.dll
		/// </summary>
		/// <remarks>not on PPC 2002 device</remarks>
		public static System.Guid NewGuidOle()
		{
			System.Guid guid = new System.Guid();
			byte [] rawVal = new byte[16];
			uint retVal = CoCreateGuid(rawVal);
			if(retVal != 0)
				throw new NotSupportedException("CoCreateGuid pInvoke");
			guid = new System.Guid(rawVal);
			return guid;
		}

		//PocketGuid
		//http://smartdevices.microsoftdev.com/Learn/Articles/507.aspx

		// guid variant types
		private enum GuidVariant
		{
			ReservedNCS = 0x00,
			Standard = 0x02,
			ReservedMicrosoft = 0x06,
			ReservedFuture = 0x07
		}

		// guid version types
		private enum GuidVersion
		{
			TimeBased = 0x01,
			Reserved = 0x02,
			NameBased = 0x03,
			Random = 0x04
		}
         
		// constants that are used in the class
		private class Const
		{
			// number of bytes in guid
			public const int ByteArraySize = 16;
         
			// multiplex variant info
			public const int VariantByte = 8;
			public const int VariantByteMask = 0x3f;
			public const int VariantByteShift = 6;

			// multiplex version info
			public const int VersionByte = 7;
			public const int VersionByteMask = 0x0f;
			public const int VersionByteShift = 4;
		}

		/// <summary>
		/// uses CryptoApi
		/// </summary>
		public static System.Guid NewGuidCrypto()
		{
			IntPtr hCryptProv = IntPtr.Zero;
			System.Guid guid = System.Guid.Empty;
        
			try
			{
				// holds random bits for guid
				byte[] bits = new byte[Const.ByteArraySize];

				// get crypto provider handle
				if (!Crypto.CryptAcquireContext(out hCryptProv, null, null, 
					(uint)ProvType.RSA_FULL, (uint)ContextFlag.VERIFYCONTEXT))
				{
					throw new SystemException(
						"Failed to acquire cryptography handle.");
				}
    
				// generate a 128 bit (16 byte) cryptographically random number
				if (!Crypto.CryptGenRandom(hCryptProv, bits.Length, bits))
				{
					throw new SystemException(
						"Failed to generate cryptography random bytes.");
				}
        
				// set the variant
				bits[Const.VariantByte] &= Const.VariantByteMask;
				bits[Const.VariantByte] |= 
					((int)GuidVariant.Standard << Const.VariantByteShift);

				// set the version
				bits[Const.VersionByte] &= Const.VersionByteMask;
				bits[Const.VersionByte] |= 
					((int)GuidVersion.Random << Const.VersionByteShift);
        
				// create the new System.Guid object
				guid = new System.Guid(bits);
			}
			finally
			{
				// release the crypto provider handle
				if (hCryptProv != IntPtr.Zero)
					Crypto.CryptReleaseContext(hCryptProv, 0);
			}
        
			return guid;
		}

		/// <summary>
		/// only uses TickCount and bit shifting
		/// </summary>
		public static System.Guid NewGuidTicks()
		{
			// Create a unique GUID
			long fileTime = DateTime.Now.ToUniversalTime().ToFileTime();
			UInt32 high32 = (UInt32)(fileTime >> 32)+0x146BF4;
			int tick = System.Environment.TickCount;
			byte[] guidBytes = new byte[8];

			// load the byte array with random bits
			Random rand = new Random((int)fileTime);
			rand.NextBytes(guidBytes);

			// use tick info in the middle of the array
			guidBytes[2] = (byte)(tick >> 24);
			guidBytes[3] = (byte)(tick >> 16);
			guidBytes[4] = (byte)(tick >> 8);
			guidBytes[5] = (byte)tick;

			// Construct a Guid with our data
			System.Guid guid = new System.Guid((int)fileTime, (short)high32, (short)((high32 | 0x10000000) >> 16), guidBytes);
			return guid;
		}

	}
}
