//==========================================================================================
//
//		OpenNETCF.Windows.Forms.RSAPKCS1SignatureDeformatter
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

namespace OpenNETCF.Security.Cryptography 
{ 	
	public class RSAPKCS1SignatureDeformatter : AsymmetricSignatureDeformatter 
	{
		private RSACryptoServiceProvider rsa;
		private string hashName;
	
		public RSAPKCS1SignatureDeformatter () {}
	
		public RSAPKCS1SignatureDeformatter (AsymmetricAlgorithm key) 
		{
			SetKey (key);
		}
	
		public override void SetHashAlgorithm (string strName) 
		{
			if (strName == null)
				throw new ArgumentNullException ("strName");
			hashName = strName;
		}
	
		public override void SetKey (AsymmetricAlgorithm key) 
		{
			if (key != null)
				rsa = (RSACryptoServiceProvider) key;
		}
	
		public override bool VerifySignature (byte[] rgbHash, byte[] rgbSignature) 
		{
			if (rsa == null)
				throw new CryptographicUnexpectedOperationException ("missing key");
			if (hashName == null)
				throw new CryptographicUnexpectedOperationException ("missing hash algorithm");
			if (rgbHash == null)
				throw new ArgumentNullException ("rgbHash");
			if (rgbSignature == null)
				throw new ArgumentNullException ("rgbSignature");

			return rsa.VerifyHash(rgbHash, hashName, rgbSignature);
		}

		public override bool VerifySignature (HashAlgorithm hash, byte[] rgbSignature) 
		{
			if (hash == null)
				throw new ArgumentNullException ("hash");
			return VerifySignature (hash.Hash, rgbSignature);
		}
	}
}
