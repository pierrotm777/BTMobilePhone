//==========================================================================================
//
//		OpenNETCF.Windows.Forms.RSACryptoServiceProvider
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
	public class RSACryptoServiceProvider : RSA
	{
		private Rsa _rsa;
		private KeySpec keySpec;

		public RSACryptoServiceProvider() : this(KeySpec.KEYEXCHANGE, false)
		{
			//exchange user key by default
		}

		public RSACryptoServiceProvider(KeySpec keySpec, bool genKey)
		{
			IntPtr prov = IntPtr.Zero;
			IntPtr rsaKey = IntPtr.Zero;
			try
			{
				this.keySpec = keySpec;
				if(genKey == false) //faster
				{
					prov = Context.AcquireContext("rSaContainer");
					rsaKey = Key.GetUserKey(prov, keySpec);

				}
				else //new key
				{
					prov = Context.AcquireContext("rSaContainerImp");
					Calg calg = Calg.RSA_KEYX;
					if(keySpec == KeySpec.SIGNATURE) 
					{calg = Calg.RSA_SIGN;}
					rsaKey = Key.GenKey(prov, calg, GenKeyParam.EXPORTABLE);
				}
				byte [] baPrivKey = Key.ExportKey(rsaKey, IntPtr.Zero, KeyBlob.PRIVATEKEYBLOB);
				_rsa = new Rsa(baPrivKey);
			}
			finally
			{
				Key.DestroyKey(rsaKey);
				Context.ReleaseContext(prov);
			}
		}

		public override int KeySize
		{
			get{return (int) _rsa.rpk.bitlen;}
			set{throw new Exception("set_KeySize not implemented");}
		}

	
		public override string ToXmlString(bool includePrivateParameters)
		{
			return _rsa.ToXmlString(includePrivateParameters);
		}

		public override void FromXmlString(string xmlString)
		{
			_rsa.FromXmlString(xmlString);
		}
	
		public override void ImportParameters(RSAParameters parameters)
		{
			_rsa.Modulus = parameters.Modulus;
			_rsa.Exponent = parameters.Exponent;
			_rsa.P = parameters.P;
			_rsa.Q = parameters.Q;
			_rsa.DP = parameters.DP;
			_rsa.DQ = parameters.DQ;
			_rsa.InverseQ = parameters.InverseQ;
			_rsa.D = parameters.D;
			bool privateKey = false;
			if(parameters.D!=null && parameters.D.Length > 0)
				privateKey = true;
			_rsa.BuildRawKey(privateKey);
		}
		
		public override RSAParameters ExportParameters(bool includePrivateParameters)
		{
			RSAParameters parameters = new RSAParameters();
			parameters.Modulus = _rsa.Modulus;
			parameters.Exponent = _rsa.Exponent;
			if(includePrivateParameters == true)
			{
				parameters.P = _rsa.P;
				parameters.Q = _rsa.Q;
				parameters.DP = _rsa.DP;
				parameters.DQ = _rsa.DQ;
				parameters.InverseQ = _rsa.InverseQ;
				parameters.D = _rsa.D;
			}
			return parameters;
		}
	
		public override byte[] EncryptValue(byte[] pBuff)
		{
			IntPtr prov = IntPtr.Zero;
			IntPtr rsaKey = IntPtr.Zero;
			byte [] cBuff;
			try
			{
				byte [] tempBuff = (byte []) pBuff.Clone();
				prov = Context.AcquireContext("rSaContainerImp");
				rsaKey = Key.ImportKey(prov, _rsa.rawKey, IntPtr.Zero, GenKeyParam.EXPORTABLE);
				cBuff = Cipher.Encrypt(rsaKey, IntPtr.Zero, tempBuff);
				Array.Reverse(cBuff, 0, cBuff.Length);
			}
			finally
			{
				Key.DestroyKey(rsaKey);
				Context.ReleaseContext(prov);
			}
			return cBuff;
		}
	
		public override byte[] DecryptValue(byte[] cBuff)
		{
			IntPtr prov = IntPtr.Zero;
			IntPtr rsaKey = IntPtr.Zero;
			byte [] pBuff;
			try
			{
				prov = Context.AcquireContext("rSaContainerImp");
				rsaKey = Key.ImportKey(prov, _rsa.rawKey, IntPtr.Zero, GenKeyParam.EXPORTABLE);
				byte [] tempBuff = (byte []) cBuff.Clone();
				Array.Reverse(tempBuff, 0, tempBuff.Length);
				pBuff = Cipher.Decrypt(rsaKey, IntPtr.Zero, tempBuff);
			}
			finally
			{
				Key.DestroyKey(rsaKey);
				Context.ReleaseContext(prov);
			}
			return pBuff;
		}

		private IntPtr GetHashAlgorithm(IntPtr prov, string halg)
		{
			if(halg.ToLower().IndexOf("md5") != -1)
				return Hash.CreateHash(prov, CalgHash.MD5);
			else if(halg.ToLower().IndexOf("sha") != -1)
				return Hash.CreateHash(prov, CalgHash.SHA1);
			else
				throw new Exception("unknown hash algorithm");
		}

		private HashAlgorithm GetHashAlgorithm(object halg, out string hashStr)
		{
			if(halg is MD5)
			{
				hashStr = "MD5";
				return new MD5CryptoServiceProvider();
			}
			else if(halg is SHA1)
			{
				hashStr = "SHA";
				return new SHA1CryptoServiceProvider();
			}
			else
				throw new Exception("unknown hash algorithm");
		}

		public byte[] SignData(byte[] data, object halg)
		{
			string hashStr;
			HashAlgorithm ha = GetHashAlgorithm(halg, out hashStr);
			byte [] baHash = ha.ComputeHash(data);
			return SignHash(baHash, hashStr);
		}

		public bool VerifyData(byte[] data, object halg, byte[] sig)
		{
			string hashStr;
			HashAlgorithm ha = GetHashAlgorithm(halg, out hashStr);
			byte [] baHash = ha.ComputeHash(data);
			return VerifyHash(baHash, hashStr, sig);
		}

		public override byte[] SignHash(byte[] rgbHash, string str)
		{
			IntPtr prov = IntPtr.Zero;
			IntPtr hash = IntPtr.Zero;
			IntPtr rsaKey = IntPtr.Zero;
			byte [] sig;
			try
			{
				byte [] tempHash= (byte[]) rgbHash.Clone();
				prov = Context.AcquireContext("rSaContainerImp");
				hash = GetHashAlgorithm(prov, str);
				//Hash.HashData(hash, tempData);
				Hash.SetHashParam(hash, HashParam.HASHVAL, tempHash);
				rsaKey = Key.ImportKey(prov, _rsa.rawKey, IntPtr.Zero, GenKeyParam.EXPORTABLE);
				sig = Hash.SignHash(hash, keySpec); //KeySpec.EXCHANGE?
				Array.Reverse(sig, 0, sig.Length);
			}
			finally
			{
				Key.DestroyKey(rsaKey);
				Hash.DestroyHash(hash);
				Context.ReleaseContext(prov);
			}
			return sig;
		}

		public override bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
		{
			bool valid = true;
			IntPtr prov = IntPtr.Zero;
			IntPtr hash = IntPtr.Zero;
			IntPtr rsaKey = IntPtr.Zero;
			try
			{
				byte [] tempHash = (byte []) rgbHash.Clone();
				byte [] tempSig = (byte []) rgbSignature.Clone();
				prov = Context.AcquireContext("rSaContainerImp");
				hash = GetHashAlgorithm(prov, str);
				//Hash.HashData(hash, tempData);
				Hash.SetHashParam(hash, HashParam.HASHVAL, tempHash);
				rsaKey = Key.ImportKey(prov, _rsa.rawKey, IntPtr.Zero, GenKeyParam.NONE); //EXPORTABLE
				Array.Reverse(tempSig, 0, tempSig.Length);
				Hash.VerifySignature(hash, tempSig, rsaKey);
			}
			catch(Exception ex)
			{
				string sex = ex.ToString();
				valid = false;
			}
			finally
			{
				Key.DestroyKey(rsaKey);
				Hash.DestroyHash(hash);
				Context.ReleaseContext(prov);
			}
			return valid;
		}
	}
}
