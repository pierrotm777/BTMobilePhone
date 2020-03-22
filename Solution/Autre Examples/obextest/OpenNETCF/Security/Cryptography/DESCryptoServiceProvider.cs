//==========================================================================================
//
//		OpenNETCF.Windows.Forms.DESCryptoServiceProvider
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
using OpenNETCF.Security.Cryptography.NativeMethods;

namespace OpenNETCF.Security.Cryptography
{
	public class DESCryptoServiceProvider : DES
	{
		public DESCryptoServiceProvider()
		{
			IntPtr prov = IntPtr.Zero;
			IntPtr ipKey = IntPtr.Zero;
			try
			{
				prov = Context.AcquireContext("sYmContainer");
				ipKey = OpenNETCF.Security.Cryptography.NativeMethods.Key.GenKey(prov, Calg.DES, GenKeyParam.EXPORTABLE);
				this.Key = OpenNETCF.Security.Cryptography.NativeMethods.Key.ExportSessionKey(prov, ipKey, 8, true);
				this.IV = OpenNETCF.Security.Cryptography.NativeMethods.Key.GetIv(ipKey);
			}
			finally
			{
				OpenNETCF.Security.Cryptography.NativeMethods.Key.DestroyKey(ipKey);
				Context.ReleaseContext(prov);
			}
		}

		//dc.KeySize = 64; //although conventionally expressed as 64 bit number?
		//dc.Mode = CipherMode.CBC; //CBC, ECB, CFB, CTS, OFB
		//dc.BlockSize = 64;
		//salt
		//dc.Padding = PaddingMode.PKCS7; //PKCS7, None, Zeros

		private byte [] iv;
		public override byte[] IV 
		{ 
			get{return iv;}
			set{iv = value;} 
		}

		private byte [] key;
		public override byte[] Key 
		{ 
			get{return (byte[])key.Clone();} 
			set{key = (byte[])value.Clone();} 
		}

		//non-standard
		public override byte[] DecryptValue(byte[] cBuff)
		{
			IntPtr prov = IntPtr.Zero;
			IntPtr ipKey = IntPtr.Zero;
			byte [] pBuff;
			try
			{
				prov = Context.AcquireContext("sYmContainer");
				ipKey = OpenNETCF.Security.Cryptography.NativeMethods.Key.ImportSessionKey(prov, Calg.DES, this.Key, true);
				OpenNETCF.Security.Cryptography.NativeMethods.Key.SetIv(ipKey, this.IV);
				pBuff = Cipher.Decrypt(ipKey, IntPtr.Zero, cBuff);
			}
			finally
			{
				OpenNETCF.Security.Cryptography.NativeMethods.Key.DestroyKey(ipKey);
				Context.ReleaseContext(prov);
			}
			return pBuff;
		}

		//non-standard
		public override byte[] EncryptValue(byte[] pBuff)
		{
			IntPtr prov = IntPtr.Zero;
			IntPtr ipKey = IntPtr.Zero;
			byte [] cBuff;
			try
			{
				prov = Context.AcquireContext("sYmContainer");
				ipKey = OpenNETCF.Security.Cryptography.NativeMethods.Key.ImportSessionKey(prov, Calg.DES, this.Key, true);
				OpenNETCF.Security.Cryptography.NativeMethods.Key.SetIv(ipKey, this.IV);
				cBuff = Cipher.Encrypt(ipKey, IntPtr.Zero, pBuff);
			}
			finally
			{
				OpenNETCF.Security.Cryptography.NativeMethods.Key.DestroyKey(ipKey);
				Context.ReleaseContext(prov);
			}
			return cBuff;
		}
	}
}
