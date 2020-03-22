// BigInteger.cs - Big Integer implementation
//
// Authors:
//	Ben Maurer
//	Chew Keong TAN
//	Sebastien Pouliot (spouliot@motus.com)
//
// Copyright (c) 2003 Ben Maurer
// All rights reserved
//
// Copyright (c) 2002 Chew Keong TAN
// All rights reserved.

// partially pilfered from Mono for deriving J for Dsa
// Mono.Math has a dependency on System.Security.Cryptography!
// from MONO (BigInteger) j = (p - 1) / q;
// calculate the public key y = g^x % p
// y = g.modPow (x, p);

using System;

//namespace Mono.Math
namespace OpenNETCF
{
	[CLSCompliant(false)]
	public class BigInteger
	{
		const string WouldReturnNegVal = "Operation would return a negative value";

		uint length = 1;

		uint [] data;

		const uint DEFAULT_LEN = 20;

		public BigInteger ()
		{
			data = new uint [DEFAULT_LEN];
		}

		public BigInteger (BigInteger bi)
		{
			this.data = (uint [])bi.data.Clone ();
			this.length = bi.length;
		}

		public BigInteger (uint ui)
		{
			data = new uint [] {ui};
		}

		public BigInteger (Sign sign, uint len) 
		{
			this.data = new uint [len];
			this.length = len;
		}

		public BigInteger (BigInteger bi, uint len)
		{

			this.data = new uint [len];

			for (uint i = 0; i < bi.length; i++)
				this.data [i] = bi.data [i];

			this.length = bi.length;
		}

		public BigInteger (byte [] inData)
		{
			length = (uint)inData.Length >> 2;
			int leftOver = inData.Length & 0x3;

			// length not multiples of 4
			if (leftOver != 0) length++;

			data = new uint [length];

			for (int i = inData.Length - 1, j = 0; i >= 3; i -= 4, j++) 
			{
				data [j] = (uint)(
					(inData [i-3] << (3*8)) |
					(inData [i-2] << (2*8)) |
					(inData [i-1] << (1*8)) |
					(inData [i-0] << (0*8))
					);
			}

			switch (leftOver) 
			{
				case 1: data [length-1] = (uint)inData [0]; break;
				case 2: data [length-1] = (uint)((inData [0] << 8) | inData [1]); break;
				case 3: data [length-1] = (uint)((inData [0] << 16) | (inData [1] << 8) | inData [2]); break;
			}

			this.Normalize ();
		}

		public int bitCount ()
		{
			this.Normalize ();

			uint value = data [length - 1];
			uint mask = 0x80000000;
			uint bits = 32;

			while (bits > 0 && (value & mask) == 0) 
			{
				bits--;
				mask >>= 1;
			}
			bits += ((length - 1) << 5);

			return (int)bits;
		}

		public byte [] getBytes ()
		{
			if (this == 0) return new byte [1];

			int numBits = bitCount ();
			int numBytes = numBits >> 3;
			if ((numBits & 0x7) != 0)
				numBytes++;

			byte [] result = new byte [numBytes];

			int numBytesInWord = numBytes & 0x3;
			if (numBytesInWord == 0) numBytesInWord = 4;

			int pos = 0;
			for (int i = (int)length - 1; i >= 0; i--) 
			{
				uint val = data [i];
				for (int j = numBytesInWord - 1; j >= 0; j--) 
				{
					result [pos+j] = (byte)(val & 0xFF);
					val >>= 8;
				}
				pos += numBytesInWord;
				numBytesInWord = 4;
			}
			return result;
		}

		public static BigInteger operator - (BigInteger bi1, BigInteger bi2)
		{
			if (bi2 == 0)
				return new BigInteger (bi1);

			if (bi1 == 0)
				throw new ArithmeticException (WouldReturnNegVal);

			switch (Kernel.Compare (bi1, bi2)) 
			{

				case Sign.Zero:
					return 0;

				case Sign.Positive:
					return Kernel.Subtract (bi1, bi2);

				case Sign.Negative:
					throw new ArithmeticException (WouldReturnNegVal);
				default:
					throw new Exception ();
			}
		}

		public static BigInteger operator / (BigInteger bi1, BigInteger bi2)
		{
			return Kernel.multiByteDivide (bi1, bi2)[0];
		}

		public static BigInteger operator >> (BigInteger bi1, int shiftVal)
		{
			return Kernel.RightShift (bi1, shiftVal);
		}

		public static BigInteger operator << (BigInteger bi1, int shiftVal)
		{
			return Kernel.LeftShift (bi1, shiftVal);
		}

		private void Normalize ()
		{
			// Normalize length
			while (length > 0 && data [length-1] == 0) length--;

			// Check for zero
			if (length == 0)
				length++;
		}

		public static implicit operator BigInteger (uint value)
		{
			return (new BigInteger (value));
		}

		public static implicit operator BigInteger (int value)
		{
			if (value < 0) throw new ArgumentOutOfRangeException ("value");
			return (new BigInteger ((uint)value));
		}

		public override bool Equals (object o)
		{
			if (o == null) return false;
			if (o is int) return (int)o >= 0 && this == (uint)o;

			return Kernel.Compare (this, (BigInteger)o) == 0;
		}

		public override int GetHashCode ()
		{
			uint val = 0;

			for (uint i = 0; i < this.length; i++)
				val ^= this.data [i];

			return (int)val;
		}

		public static bool operator == (BigInteger bi1, uint ui)
		{
			if (bi1.length != 1) bi1.Normalize ();
			return bi1.length == 1 && bi1.data [0] == ui;
		}

		public static bool operator != (BigInteger bi1, uint ui)
		{
			if (bi1.length != 1) bi1.Normalize ();
			return !(bi1.length == 1 && bi1.data [0] == ui);
		}

		public static bool operator == (BigInteger bi1, BigInteger bi2)
		{
			// we need to compare with null
			if ((bi1 as object) == (bi2 as object))
				return true;
			if (null == bi1 || null == bi2)
				return false;
			return Kernel.Compare (bi1, bi2) == 0;
		}

		public static bool operator != (BigInteger bi1, BigInteger bi2)
		{
			// we need to compare with null
			if ((bi1 as object) == (bi2 as object))
				return false;
			if (null == bi1 || null == bi2)
				return true;
			return Kernel.Compare (bi1, bi2) != 0;
		}

		public enum Sign : int 
		{
			Negative = -1,
			Zero = 0,
			Positive = 1
		};

		private sealed class Kernel
		{
			public static BigInteger RightShift (BigInteger bi, int n)
			{
				if (n == 0) return new BigInteger (bi);

				int w = n >> 5;
				int s = n & ((1 << 5) - 1);

				BigInteger ret = new BigInteger (Sign.Positive, bi.length - (uint)w + 1);
				uint l = (uint)ret.data.Length - 1;

				if (s != 0) 
				{

					uint x, carry = 0;

					while (l-- > 0) 
					{
						x = bi.data [l + w];
						ret.data [l] = (x >> n) | carry;
						carry = x << (32 - n);
					}
				} 
				else 
				{
					while (l-- > 0)
						ret.data [l] = bi.data [l + w];

				}
				ret.Normalize ();
				return ret;
			}

			public static BigInteger LeftShift (BigInteger bi, int n)
			{
				if (n == 0) return new BigInteger (bi, bi.length + 1);

				int w = n >> 5;
				n &= ((1 << 5) - 1);

				BigInteger ret = new BigInteger (Sign.Positive, bi.length + 1 + (uint)w);

				uint i = 0, l = bi.length;
				if (n != 0) 
				{
					uint x, carry = 0;
					while (i < l) 
					{
						x = bi.data [i];
						ret.data [i + w] = (x << n) | carry;
						carry = x >> (32 - n);
						i++;
					}
					ret.data [i + w] = carry;
				} 
				else 
				{
					while (i < l) 
					{
						ret.data [i + w] = bi.data [i];
						i++;
					}
				}

				ret.Normalize ();
				return ret;
			}

			public static BigInteger Subtract (BigInteger big, BigInteger small)
			{
				BigInteger result = new BigInteger (Sign.Positive, big.length);

				uint [] r = result.data, b = big.data, s = small.data;
				uint i = 0, c = 0;

				do 
				{

					uint x = s [i];
					if (((x += c) < c) | ((r [i] = b [i] - x) > ~x))
						c = 1;
					else
						c = 0;

				} while (++i < small.length);

				if (i == big.length) goto fixup;

				if (c == 1) 
				{
					do
						r [i] = b [i] - 1;
					while (b [i++] == 0 && i < big.length);

					if (i == big.length) goto fixup;
				}

				do
					r [i] = b [i];
				while (++i < big.length);

				fixup:

					result.Normalize ();
				return result;
			}

			public static Sign Compare (BigInteger bi1, BigInteger bi2)
			{
				//
				// Step 1. Compare the lengths
				//
				uint l1 = bi1.length, l2 = bi2.length;

				while (l1 > 0 && bi1.data [l1-1] == 0) l1--;
				while (l2 > 0 && bi2.data [l2-1] == 0) l2--;

				if (l1 == 0 && l2 == 0) return Sign.Zero;

				// bi1 len < bi2 len
				if (l1 < l2) return Sign.Negative;
					// bi1 len > bi2 len
				else if (l1 > l2) return Sign.Positive;

				//
				// Step 2. Compare the bits
				//

				uint pos = l1 - 1;

				while (pos != 0 && bi1.data [pos] == bi2.data [pos]) pos--;
				
				if (bi1.data [pos] < bi2.data [pos])
					return Sign.Negative;
				else if (bi1.data [pos] > bi2.data [pos])
					return Sign.Positive;
				else
					return Sign.Zero;
			}

			public static BigInteger [] multiByteDivide (BigInteger bi1, BigInteger bi2)
			{
				if (Kernel.Compare (bi1, bi2) == Sign.Negative)
					return new BigInteger [2] { 0, new BigInteger (bi1) };

				bi1.Normalize (); bi2.Normalize ();

				if (bi2.length == 1)
					return DwordDivMod (bi1, bi2.data [0]);

				uint remainderLen = bi1.length + 1;
				int divisorLen = (int)bi2.length + 1;

				uint mask = 0x80000000;
				uint val = bi2.data [bi2.length - 1];
				int shift = 0;
				int resultPos = (int)bi1.length - (int)bi2.length;

				while (mask != 0 && (val & mask) == 0) 
				{
					shift++; mask >>= 1;
				}

				BigInteger quot = new BigInteger (Sign.Positive, bi1.length - bi2.length + 1);
				BigInteger rem = (bi1 << shift);

				uint [] remainder = rem.data;

				bi2 = bi2 << shift;

				int j = (int)(remainderLen - bi2.length);
				int pos = (int)remainderLen - 1;

				uint firstDivisorByte = bi2.data [bi2.length-1];
				ulong secondDivisorByte = bi2.data [bi2.length-2];

				while (j > 0) 
				{
					ulong dividend = ((ulong)remainder [pos] << 32) + (ulong)remainder [pos-1];

					ulong q_hat = dividend / (ulong)firstDivisorByte;
					ulong r_hat = dividend % (ulong)firstDivisorByte;

					do 
					{

						if (q_hat == 0x100000000 ||
							(q_hat * secondDivisorByte) > ((r_hat << 32) + remainder [pos-2])) 
						{
							q_hat--;
							r_hat += (ulong)firstDivisorByte;

							if (r_hat < 0x100000000)
								continue;
						}
						break;
					} while (true);

					//
					// At this point, q_hat is either exact, or one too large
					// (more likely to be exact) so, we attempt to multiply the
					// divisor by q_hat, if we get a borrow, we just subtract
					// one from q_hat and add the divisor back.
					//

					uint t;
					uint dPos = 0;
					int nPos = pos - divisorLen + 1;
					ulong mc = 0;
					uint uint_q_hat = (uint)q_hat;
					do 
					{
						mc += (ulong)bi2.data [dPos] * (ulong)uint_q_hat;
						t = remainder [nPos];
						remainder [nPos] -= (uint)mc;
						mc >>= 32;
						if (remainder [nPos] > t) mc++;
						dPos++; nPos++;
					} while (dPos < divisorLen);

					nPos = pos - divisorLen + 1;
					dPos = 0;

					// Overestimate
					if (mc != 0) 
					{
						uint_q_hat--;
						ulong sum = 0;

						do 
						{
							sum = ((ulong)remainder [nPos]) + ((ulong)bi2.data [dPos]) + sum;
							remainder [nPos] = (uint)sum;
							sum >>= 32;
							dPos++; nPos++;
						} while (dPos < divisorLen);

					}

					quot.data [resultPos--] = (uint)uint_q_hat;

					pos--;
					j--;
				}

				quot.Normalize ();
				rem.Normalize ();
				BigInteger [] ret = new BigInteger [2] { quot, rem };

				if (shift != 0)
					ret [1] >>= shift;

				return ret;
			}

			public static BigInteger [] DwordDivMod (BigInteger n, uint d)
			{
				BigInteger ret = new BigInteger (Sign.Positive , n.length);

				ulong r = 0;
				uint i = n.length;

				while (i-- > 0) 
				{
					r <<= 32;
					r |= n.data [i];
					ret.data [i] = (uint)(r / d);
					r %= d;
				}
				ret.Normalize ();

				BigInteger rem = (uint)r;

				return new BigInteger [] {ret, rem};
			}
		}
	}
}
