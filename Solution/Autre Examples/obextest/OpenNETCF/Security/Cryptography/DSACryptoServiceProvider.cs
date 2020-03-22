//==========================================================================================
//
//		OpenNETCF.Windows.Forms.DSACryptoServiceProvider
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
	public class DSACryptoServiceProvider : DSA
	{
		private Dsa _dsa;

		public DSACryptoServiceProvider() : this(false)
		{
			//doesnt gen a new key by default
		}

		public DSACryptoServiceProvider(bool genKey)
		{
			IntPtr prov = IntPtr.Zero;
			IntPtr key = IntPtr.Zero;
			//for bad key state
			//Crypto.CryptAcquireContext(out prov, "dSaContainer", ProvName.MS_ENH_DSS_DH_PROV, (uint)ProvType.DSS_DH, (uint)ContextFlag.DELETEKEYSET);
			//Crypto.CryptAcquireContext(out prov, "dSaContainer", ProvName.MS_ENH_DSS_DH_PROV, (uint)ProvType.DSS_DH, (uint)ContextFlag.NEWKEYSET);
			try
			{
				if(genKey == false) //faster
				{
					prov = Context.AcquireContext("dSaContainer", ProvName.MS_ENH_DSS_DH_PROV, ProvType.DSS_DH, ContextFlag.NONE);
					key = Key.GetUserKey(prov, KeySpec.SIGNATURE);
				}
				else //create new one
				{
					prov = Context.AcquireContext("dSaContainerImp", ProvName.MS_ENH_DSS_DH_PROV, ProvType.DSS_DH, ContextFlag.NONE);
					key = Key.GenKey(prov, Calg.DSS_SIGN, GenKeyParam.EXPORTABLE);

				}
				byte [] baPriKey = Key.ExportKey(key, IntPtr.Zero, KeyBlob.PRIVATEKEYBLOB);
				byte [] baPubKey = Key.ExportKey(key, IntPtr.Zero, KeyBlob.PUBLICKEYBLOB);
				_dsa = new Dsa(baPriKey); //X is returned
				Dsa dsaPub = new Dsa(baPubKey); //Y is only from public call
				_dsa.Y = (byte[])dsaPub.Y.Clone();
			}
			finally
			{
				Key.DestroyKey(key);
				Context.ReleaseContext(prov);
			}
		}

		public override int KeySize
		{
			get{return (int) _dsa.dpk.bitlen;}
			set{throw new Exception("set_KeySize not implemented");}
		}

	
		public override string ToXmlString(bool includePrivateParameters)
		{
			return _dsa.ToXmlString(includePrivateParameters);
		}

		public override void FromXmlString(string xmlString)
		{
			_dsa.FromXmlString(xmlString);
		}

		public override void ImportParameters(DSAParameters parameters)
		{
			_dsa.P = parameters.P;
			_dsa.Q = parameters.Q;
			_dsa.G = parameters.G;
			_dsa.Y = parameters.Y;
			_dsa.J = parameters.J;
			_dsa.Seed = parameters.Seed;
			byte [] baCounter = BitConverter.GetBytes(parameters.Counter);
			_dsa.PgenCounter = baCounter;
			_dsa.X = parameters.X;

			bool privateKey = false;
			if(parameters.X!=null && parameters.X.Length > 0)
				privateKey = true;
			_dsa.BuildRawKey(privateKey);
		}
		
		public override DSAParameters ExportParameters(bool includePrivateParameters)
		{
			DSAParameters parameters = new DSAParameters();
			parameters.P = _dsa.P;
			parameters.Q = _dsa.Q;
			parameters.G = _dsa.G;
			parameters.Y = _dsa.Y;
			parameters.J = _dsa.J;
			parameters.Seed = _dsa.Seed;
			parameters.Counter = (int) _dsa.ds.counter;
			if(includePrivateParameters == true)
			{
				parameters.X = _dsa.X;
			}
			return parameters;
		}

		private IntPtr GetHashAlgorithm(IntPtr prov, string halg)
		{
			if(halg.ToLower().IndexOf("sha") != -1)
				return Hash.CreateHash(prov, CalgHash.SHA1);
			else
				throw new Exception("unknown hash algorithm");
		}

		private HashAlgorithm GetHashAlgorithm(object halg, out string hashStr)
		{
			if(halg is SHA1)
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
				prov = Context.AcquireContext("dSaContainerImp", ProvName.MS_ENH_DSS_DH_PROV, ProvType.DSS_DH, ContextFlag.NONE);
				hash = GetHashAlgorithm(prov, str);
				//Hash.HashData(hash, tempData);
				Hash.SetHashParam(hash, HashParam.HASHVAL, tempHash);
				rsaKey = Key.ImportKey(prov, _dsa.rawKey, IntPtr.Zero, GenKeyParam.EXPORTABLE);
				sig = Hash.SignHash(hash, KeySpec.SIGNATURE);
				Array.Reverse(sig, 0, 20); //r
				Array.Reverse(sig, 20, 20); //s
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
				prov = Context.AcquireContext("dSaContainerImp", ProvName.MS_ENH_DSS_DH_PROV, ProvType.DSS_DH, ContextFlag.NONE);
				hash = GetHashAlgorithm(prov, str);
				//Hash.HashData(hash, tempData);
				Hash.SetHashParam(hash, HashParam.HASHVAL, tempHash);
				rsaKey = Key.ImportKey(prov, _dsa.rawKey, IntPtr.Zero, GenKeyParam.NONE); //EXPORTABLE wont work here
				Array.Reverse(tempSig, 0, 20); //r
				Array.Reverse(tempSig, 20, 20); //s
				Hash.VerifySignature(hash, tempSig, rsaKey);
			}
			catch(Exception)
			{
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
