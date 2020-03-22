//==========================================================================================
//
//		OpenNETCF.Windows.Forms.RSAPKCS1KeyExchangeFormatter
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
	public class RSAPKCS1KeyExchangeFormatter: AsymmetricKeyExchangeFormatter
	{
		private RSA rsa;
		private RandomNumberGenerator random;
	
		public RSAPKCS1KeyExchangeFormatter ()
		{
		}
	
		public RSAPKCS1KeyExchangeFormatter (AsymmetricAlgorithm key)
		{
			SetKey (key);
		}
	
		public RandomNumberGenerator Rng 
		{
			get { return random; }
			set { random = value; }
		}
	
		public override string Parameters 
		{
			get { return "<enc:KeyEncryptionMethod enc:Algorithm=\"http://www.microsoft.com/xml/security/algorithm/PKCS1-v1.5-KeyEx\" xmlns:enc=\"http://www.microsoft.com/xml/security/encryption/v1.0\" />"; }
		}
	
		public override byte[] CreateKeyExchange (byte[] rgbData)
		{
			if (rsa == null)
				throw new CryptographicException ();
			if (random == null)
				random = new RNGCryptoServiceProvider();
				//random = RandomNumberGenerator.Create ();
			return rsa.EncryptValue(rgbData);
		}
	
		public override byte[] CreateKeyExchange (byte[] rgbData, Type symAlgType)
		{
			return CreateKeyExchange (rgbData);
		}
	
		public override void SetKey (AsymmetricAlgorithm key)
		{
			if (key != null) {
				if (key is RSA) {
					rsa = (RSA)key;
				}
				else
					throw new InvalidCastException ();
			}
		}
	}
}
