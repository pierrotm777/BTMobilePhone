//==========================================================================================
//
//		OpenNETCF.Windows.Forms.PasswordDeriveBytes
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
	public class PasswordDeriveBytes : DeriveBytes
	{
		private string password;
		private byte [] salt;
		private string hashName;
		int iterations;

		public PasswordDeriveBytes(string password, byte [] salt)
		{
			this.password = password;
			this.salt = salt;
			this.hashName = "SHA1"; //default hash
			this.iterations = 100; //default iterations
		}

		public PasswordDeriveBytes(string password, byte[] salt, string hashName, int iterations)
		{
			this.password = password;
			this.salt = salt;
			this.hashName = hashName;
			this.iterations = iterations;
		}

		//Properties
		public string HashName
		{
			get{return hashName;}
			set{hashName = value;}
		}
		public int IterationCount
		{
			get{return iterations;}
			set{iterations = value;}
		}
		public byte [] Salt
		{
			get{return salt;}
			set{salt = value;}
		}

		/// <summary>
		/// Derives a cryptographic key from the PasswordDeriveBytes object.
		/// </summary>
		/// <remarks>
		/// If the keySize parameter is set to 0, the default key size for the specified algorithm is used.
		/// </remarks>
		/// <param name="algName">The algorithm name for which to derive the key. </param>
		/// <param name="algHashName">The hash algorithm name to use to derive the key. </param>
		/// <param name="keySize">The size of the key to derive. </param>
		/// <param name="IV">The initialization vector (IV) to use to derive the key.</param>
		/// <returns>The derived key.</returns>
		public byte [] CryptDeriveKey(string algName, string algHashName, int keySize, byte [] IV)
		{
			//RC2 / SHA1 works
			//TODO DES / MD5 seems to be salted
			//TODO not using salt, IV, or keySize
			IntPtr prov = Context.AcquireContext();
			
			byte [] baPassword = Format.GetBytes(this.password);
			IntPtr hash = GetHashAlgorithm(prov, algHashName);
			Hash.HashData(hash, baPassword);

			IntPtr key = GetKeyAlgorithm(prov, hash, algName);
			int keyLen = Key.GetKeyLength(key) / 8;
			byte [] baKey = Key.ExportSessionKey(prov, key, keyLen, true);

			Hash.DestroyHash(hash);
			Key.DestroyKey(key);

			Context.ReleaseContext(prov);
			return baKey;
		}

		/// <summary>
		/// Returns pseudo-random key bytes.
		/// </summary>
		/// <param name="cb">The number of pseudo-random key bytes to generate.</param>
		/// <returns>A byte array filled with pseudo-random key bytes.</returns>
		public override byte[] GetBytes(int cb)
		{
			byte[] data = ASCIIEncoder(password);
			if(salt != null)
			{
				byte[] temp = new byte[data.Length + salt.Length];
				Array.Copy(data, 0, temp, 0, data.Length);
				Array.Copy(salt, 0, temp, data.Length, salt.Length);
				data = temp;
			}

			HashAlgorithm ha = GetHashAlgorithm(this.hashName);
			//if(ha is HMACSHA1)
			//	ha = new HMACSHA1(data);
			//if(ha is MACTripleDES)
			//	ha = new MACTripleDES(data);

			if(iterations <= 0) iterations = 1;
			for(int i = 0; i < iterations; i++)
			{
				data = ha.ComputeHash(data);
			}

			byte[] key = new byte[cb];
			Array.Copy(data, 0, key, 0, cb);
			return key;
		}

		/// <summary>
		/// Resets the state of the operation.
		/// </summary>
		public override void Reset()
		{
			this.salt = null;
		}

		private IntPtr GetKeyAlgorithm(IntPtr prov, IntPtr hash, string halg)
		{
			if(halg.ToLower().IndexOf("rc2") != -1)
				return Key.DeriveKey(prov, Calg.RC2, hash, GenKeyParam.EXPORTABLE | GenKeyParam.NO_SALT);
			else if(halg.ToLower().IndexOf("tripledes") != -1)
				return Key.DeriveKey(prov, Calg.TRIP_DES, hash, GenKeyParam.EXPORTABLE | GenKeyParam.NO_SALT);
			else if(halg.ToLower().IndexOf("des") != -1)
				return Key.DeriveKey(prov, Calg.DES, hash, GenKeyParam.EXPORTABLE | GenKeyParam.NO_SALT);
			else
				throw new Exception("unknown hash algorithm");
		}

		private IntPtr GetHashAlgorithm(IntPtr prov, string halg)
		{
			//keyed hashes
			if(halg.ToLower().IndexOf("hmacsha1") != -1)
				return Hash.CreateHash(prov, CalgHash.HMAC);
			if(halg.ToLower().IndexOf("mactripledes") != -1)
				return Hash.CreateHash(prov, CalgHash.MAC);
			//normal hashes
			if(halg.ToLower().IndexOf("md5") != -1)
				return Hash.CreateHash(prov, CalgHash.MD5);
			else if(halg.ToLower().IndexOf("sha") != -1)
				return Hash.CreateHash(prov, CalgHash.SHA1);
			else
				throw new Exception("unknown hash algorithm");
		}

		private HashAlgorithm GetHashAlgorithm(string halg)
		{
			//keyed hashes
			if(halg.ToLower().IndexOf("hmacsha1") != -1)
				return new HMACSHA1();
			if(halg.ToLower().IndexOf("mactripledes") != -1)
				return new MACTripleDES();
				//normal hashes
			else if(halg.ToLower().IndexOf("md5") != -1)
				return new MD5CryptoServiceProvider();
			//else if(halg.ToLower().IndexOf("sha256") != -1)
			//	return new SHA256Managed();
			else if(halg.ToLower().IndexOf("sha1") != -1) //also in hmacsha1
				return new SHA1CryptoServiceProvider();
			else if(halg.ToLower().IndexOf("sha") != -1) //some form of sha last
				return new SHA1CryptoServiceProvider();
			else
				throw new Exception("unknown hash algorithm");
		}

		private static byte[] ASCIIEncoder(string s)
		{
			byte[] ascii = new byte[s.Length];
			for(int i = 0; i < s.Length; i++)
			{
				ascii[i] = (byte)s[i];
			}
			return ascii;
		}
	}
}
