//==========================================================================================
//
//		OpenNETCF.Windows.Forms.CryptographicException
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
using System.Runtime.Serialization;

namespace OpenNETCF.Security.Cryptography 
{
	//[Serializable]
	public class CryptographicException : SystemException 
	{

		public CryptographicException ()
			: base ("Error occured during a cryptographic operation.")
		{
			// default to CORSEC_E_CRYPTO
			// defined as EMAKEHR(0x1430) in CorError.h
			//HResult = unchecked ((int)0x80131430);
		}

		public CryptographicException (int hr)
		{
			//HResult = hr;
		}

		public CryptographicException (string message)
			: base (message)
		{
			//HResult = unchecked ((int)0x80131430);
		}

		public CryptographicException (string message, Exception inner)
			: base (message, inner)
		{
			//HResult = unchecked ((int)0x80131430);
		}

		public CryptographicException (string format, string insert)
			: base (String.Format(format, insert))
		{
			//HResult = unchecked ((int)0x80131430);
		}

		//protected CryptographicException (SerializationInfo info, StreamingContext context)
		//	: base (info, context) {}
	}

}
