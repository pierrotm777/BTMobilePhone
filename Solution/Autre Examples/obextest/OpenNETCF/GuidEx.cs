//==========================================================================================
//
//		OpenNETCF.GuidEx
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
//==========================================================================================
using System;

namespace OpenNETCF
{
	#region OpenNETCF.GuidEx ValueType
	/// <summary>
	/// Represents a globally unique identifier (GUID).
	/// </summary>
	/// <remarks>A GUID is a 128-bit integer (16 bytes) that can be used across all computers and networks wherever a unique identifier is required. Such an identifier has a very low probability of being duplicated.</remarks>
	/// <seealso cref="System.Guid"/>
	public struct GuidEx
	{

		#region Private Fields

		private uint		_a;
		private ushort	_b;
		private ushort	_c;
		private byte	_d;
		private byte	_e;
		private byte	_f;
		private byte	_g;
		private byte	_h;
		private byte	_i;
		private byte	_j;
		private byte	_k;
		
		private static GuidState _guidState = new GuidState(true); // use pseudo RNG

		#endregion

		#region Helper Functions
		private static void CheckNull(object o)
		{
			if(o == null)
			{
				throw new ArgumentNullException("Value cannot be null.");
			}
		}

		private static void CheckLength(byte[] o, int l)
		{
			if(o.Length != l)
			{
				throw new ArgumentException(String.Format("Array should be exactly {0} bytes long.", l));
			}
		}

		private static void CheckArray(byte[] o, int l)
		{
			CheckNull(o);
			CheckLength(o, l);
		}
		#endregion

		/// <summary>
		///		Initializes a new instance of the <see cref="T:OpenNETCF.GuidEx"/> class.
		/// </summary>
		public static readonly GuidEx Empty = new GuidEx(0,0,0,0,0,0,0,0,0,0,0);

		/// <summary>
		/// 	Initializes a new instance of the <see cref="T:OpenNETCF.GuidEx"/> class using the specified array of bytes.
		/// </summary>
		/// <param name="b">A 16 element byte array containing values with which to initialize the GUID.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="b"/> is <see langword="null"/> .
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="b "/> is not 16 bytes long.
		/// </exception>
		public GuidEx(byte[] b)
		{
			CheckArray(b, 16);
			_a = BitConverter.ToUInt32(b, 0);
			_b = BitConverter.ToUInt16(b, 4);
			_c = BitConverter.ToUInt16(b, 6);
			_d = b[8];
			_e = b[9];
			_f = b[10];
			_g = b[11];
			_h = b[12];
			_i = b[13];
			_j = b[14];
			_k = b[15];
		}

		/// <summary>
		/// 	 Initializes a new instance of the <see cref="T:OpenNETCF.GuidEx"/> class using the value represented by the specified string.
		/// </summary>
		/// <param name="g">
		/// 	A <see cref="T:OpenNETCF.String"/> that contains a GUID in one of the following formats('d' represents a hexadecimal digit whose case is ignored):
		/// 	32 contiguous digits:
		/// 	dddddddddddddddddddddddddddddddd 
		/// 	 -or-
		/// 	Groups of 8, 4, 4, 4, and 12 digits with hyphens between the groups. The entire GUID can optionally be enclosed in matching braces or parentheses:
		/// 	dddddddd-dddd-dddd-dddd-dddddddddddd
		/// 	-or-
		/// 	{dddddddd-dddd-dddd-dddd-dddddddddddd}
		/// 	-or-
		/// 	(dddddddd-dddd-dddd-dddd-dddddddddddd)
		/// 	 -or-
		/// 	Groups of 8, 4, and 4 digits, and a subset of eight groups of 2 digits, with each group prefixed by "0x" or "0X", and separated by commas. The entire GUID, as well as the subset, is enclosed in matching braces:
		/// 	{0xdddddddd, 0xdddd, 0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}}
		/// 	 All braces, commas, and "0x" prefixes are required. All embedded spaces are ignored. All leading zeroes in a group are ignored. 
		/// 	 The digits shown in a group are the maximum number of meaningful digits that can appear in that group. You can specify from 1 to the number of digits shown for a group. The specified digits are assumed to be the low order digits of the group. If you specify more digits for a group than shown, the high-order digits are ignored. 
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="g "/>is <see langword="null"/>.</exception>
		/// <exception cref="T:System.FormatException">
		/// 	The format of <paramref name="g "/> is invalid.
		/// </exception>
		public GuidEx(string g)
		{
			CheckNull(g);

			GuidParser p = new GuidParser(g);
			GuidEx guid = p.Parse();

			this = guid;
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="T:OpenNETCF.GuidEx"/> class using the specified integers and byte array.
		/// </summary>
		/// <param name=" a">The first 4 bytes of the GUID.</param>
		/// <param name=" b">The next 2 bytes of the GUID.</param>
		/// <param name=" c">The next 2 bytes of the GUID.</param>
		/// <param name=" d">The remaining 8 bytes of the GUID.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="d "/>is <see langword="null"/> .
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="d "/>is not 8 bytes long.
		/// </exception>
		public GuidEx(int a, short b, short c, byte[] d)
		{
			CheckArray(d, 8);
			_a =(uint)a;
			_b =(ushort)b;
			_c =(ushort)c;
			_d = d[0];
			_e = d[1];
			_f = d[2];
			_g = d[3];
			_h = d[4];
			_i = d[5];
			_j = d[6];
			_k = d[7];
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="T:OpenNETCF.GuidEx"/> class using the specified integers and bytes.
		/// </summary>
		/// <param name="a">The first 4 bytes of the GUID.</param>
		/// <param name="b">The next 2 bytes of the GUID.</param>
		/// <param name="c">The next 2 bytes of the GUID.</param>
		/// <param name="d">The next byte of the GUID.</param>
		/// <param name="e">The next byte of the GUID.</param>
		/// <param name="f">The next byte of the GUID.</param>
		/// <param name="g">The next byte of the GUID.</param>
		/// <param name="h">The next byte of the GUID.</param>
		/// <param name="i">The next byte of the GUID.</param>
		/// <param name="j">The next byte of the GUID.</param>
		/// <param name="k">The next byte of the GUID.</param>
		public GuidEx(
			int a,
			short b,
			short c,
			byte d,
			byte e,
			byte f,
			byte g,
			byte h,
			byte i,
			byte j,
			byte k)
			: this((uint)a,(ushort)b,(ushort)c, d, e, f, g, h, i, j, k) {}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="T:OpenNETCF.GuidEx"/> class using the specified unsigned integers and bytes.
		/// </summary>
		/// <param name="a">The first 4 bytes of the GUID.</param>
		/// <param name="b">The next 2 bytes of the GUID.</param>
		/// <param name="c">The next 2 bytes of the GUID.</param>
		/// <param name="d">The next byte of the GUID.</param>
		/// <param name="e">The next byte of the GUID.</param>
		/// <param name="f">The next byte of the GUID.</param>
		/// <param name="g">The next byte of the GUID.</param>
		/// <param name="h">The next byte of the GUID.</param>
		/// <param name="i">The next byte of the GUID.</param>
		/// <param name="j">The next byte of the GUID.</param>
		/// <param name="k">The next byte of the GUID.</param>
		[CLSCompliant(false)]
		public GuidEx(
			uint a,
			ushort b,
			ushort c,
			byte d,
			byte e,
			byte f,
			byte g,
			byte h,
			byte i,
			byte j,
			byte k)
		{
			_a = a;
			_b = b;
			_c = c;
			_d = d;
			_e = e;
			_f = f;
			_g = g;
			_h = h;
			_i = i;
			_j = j;
			_k = k;
		}

		private static int Compare(uint x, uint y)
		{
			if(x<y)
			{
				return -1;
			} 
			else 
			{
				return 1;
			}
		}

		/// <summary>
		/// Compares this instance to a specified object and returns an indication of their relative values.
		/// </summary>
		/// <param name="value">An object to compare, or null.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="value" />.
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Description</description>
		/// </listheader>
		/// <item>
		/// <term> A negative integer</term>
		/// <description>This instance is less than <paramref name="value" />.</description>
		/// </item>
		/// <item>
		/// <term> Zero</term>
		/// <description>This instance is equal to <paramref name="value" />.</description>
		/// </item>
		/// <item>
		/// <term> A positive integer</term>
		/// <description>
		/// This instance is greater than <paramref name="value" />.
		/// -or-
		/// 
		/// <paramref name="value" /> is <see langword="null" />.
		/// </description>
		/// </item>
		/// </list>
		/// </returns>
		/// <exception cref="ArgumentException"><paramref name="value" /> is not a <see langword="GuidEx" />.</exception>
		public int CompareTo(object value)
		{
			if(value == null)
				return 1;

			if(!(value is OpenNETCF.GuidEx)) 
			{
				throw new ArgumentException("Argument of OpenNETCF.GuidEx.CompareTo should be a GuidEx");
			}

			GuidEx v = (GuidEx)value;

			if(_a != v._a) 
			{
				return Compare(_a, v._a);
			}
			else if(_b != v._b) 
			{
				return Compare(_b, v._b);
			}
			else if(_c != v._c) 
			{
				return Compare(_d, v._d);
			}
			else if(_d != v._d) 
			{
				return Compare(_d, v._d);
			}
			else if(_e != v._e) 
			{
				return Compare(_e, v._e);
			}
			else if(_f != v._f) 
			{
				return Compare(_f, v._f);
			}
			else if(_g != v._g) 
			{
				return Compare(_g, v._g);
			}
			else if(_h != v._h) 
			{
				return Compare(_h, v._h);
			}
			else if(_i != v._i) 
			{
				return Compare(_i, v._i);
			}
			else if(_j != v._j) 
			{
				return Compare(_j, v._j);
			}
			else if(_k != v._k) 
			{
				return Compare(_k, v._k);
			}

			return 0;
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="o">The object to compare with this instance. </param>
		/// <returns> 
		/// <see langword="true" /> if <paramref name="o" /> is a <see langword="GuidEx" /> 
		/// that has the same value as this instance; otherwise, <see langword="false" />.
		/// </returns>
		public override bool Equals(object o)
		{
			try 
			{
				return CompareTo(o) == 0;
			}
			catch(ArgumentException ) 
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code for this instance.</returns>
		public override int GetHashCode()
		{
			int res;

			res  = ((int) _a);
			res ^= ((int) _b << 16 | _c);
			res ^= ((int)_d << 24);
			res ^= ((int)_e << 8);
			res ^= ((int)_f);
			res ^= ((int)_g << 24);
			res ^= ((int)_h << 16);
			res ^= ((int)_i << 8);
			res ^= ((int)_j);

			return res;
		}

		private static GuidEx NewTimeGuid()
		{
			ulong timeStamp = _guidState.NewTimeStamp();

			// Bit[31..0] (32 bits) for timeLow
			uint timeLow = (uint)(timeStamp & 0x00000000fffffffful);

			// Bit[47..32] (16 bits) for timeMid
			ushort timeMid = (ushort)((timeStamp & 0x0000ffff00000000ul) >> 32);

			// Bit[59..48] (12 bits) for timeHi
			ushort timeHi = (ushort)((timeStamp & 0x0ffff00000000000ul) >> 48);

			// Bit[7..0] (8 bits) for clockSeqLow
			byte clockSeqLow = (byte)(GuidEx._guidState.ClockSeq & 0x00ffu);

			// Bit[13..8] (6 bits) for clockSeqHi
			byte clockSeqHi = (byte)((GuidEx._guidState.ClockSeq & 0x3f00u) >> 8);

			byte[] mac = GuidEx._guidState.MAC;

			clockSeqHi = (byte)(clockSeqHi | 0x80u); // Bit[7] = 1, Bit[6] = 0 (Variant)
			timeHi = (ushort)(timeHi | 0x1000u); // Bit[15..13] = 1 (GuidEx is time-based)

			return new GuidEx(timeLow, timeMid, timeHi, clockSeqHi, clockSeqLow,mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);
		}

		private static GuidEx NewRandomGuid()
		{
			byte[] b = new byte[16];
			_guidState.NextBytes(b);

			GuidEx res = new GuidEx(b);

			// Mask in Variant 1-0 in Bit[7..6]
			res._d = (byte)((res._d & 0x3fu) | 0x80u);

			// Mask in Version 4 (random based GuidEx) in Bits[15..13]
			res._c = (ushort)((res._c & 0x0fffu) | 0x4000u);

			return res;
		}

		/// <summary>
		/// Initializes a new instance of the <see langword="GuidEx" /> class.
		/// </summary>
		/// <returns>A new <see langword="GuidEx" /> object.</returns>
		/// <remarks>This is a convenient static (<see langword="Shared" /> in Visual Basic) 
		/// method that you can call to get a new <see langword="GuidEx" /></remarks>
		public static GuidEx NewGuid()
		{
			return NewRandomGuid();
		}

		/// <summary>
		/// Returns a 16-element byte array that contains the value of the GUID.
		/// </summary>
		/// <returns>A 16-element byte array.</returns>
		public byte[] ToByteArray()
		{
			byte[] res = new byte[16];
			byte[] tmp;
			int d = 0;
			int s;

			tmp = BitConverter.GetBytes(_a); // timeLow
			for(s=0; s<4; s++) 
			{
				res[d++] = tmp[s];
			}

			tmp = BitConverter.GetBytes(_b); // timeMid
			for(s=0; s<2; s++)
			{
				res[d++] = tmp[s];
			}

			tmp = BitConverter.GetBytes(_c); // timeHiAndVersion
			for(s=0; s<2; s++) 
			{
				res[d++] = tmp[s];
			}

			res[8]	= _d;
			res[9]	= _e;
			res[10] = _f;
			res[11] = _g;
			res[12] = _h;
			res[13] = _i;
			res[14] = _j;
			res[15] = _k;

			return res;
		}

		private string BaseToString(bool h, bool p, bool b)
		{
			string res = "";

			if(p) 
			{
				res +="(";
			}
			else if(b)
			{
				res += "{";
			}

			res += _a.ToString("x8");
			if(h) 
			{
				res += "-";
			}

			res += _b.ToString("x4");
			if(h) 
			{
				res += "-";
			}
			
			res += _c.ToString("x4");
			if(h) 
			{
				res += "-";
			}
			
			res += _d.ToString("x2");
			res += _e.ToString("x2");
			if(h) 
			{
				res += "-";
			}
			res += _f.ToString("x2");
			res += _g.ToString("x2");
			res += _h.ToString("x2");
			res += _i.ToString("x2");
			res += _j.ToString("x2");

			if(p) 
			{
				res += ")";
			}
			else if(b) 
			{
				res += "}";
			}

			return res;
		}

		/// <summary>
		/// Returns a <see langword="String" /> representation of the value of this instance in registry format.
		/// </summary>
		/// <returns><para>A <see langword="String" /> formatted in this pattern:</para>
		/// <para>xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</para>
		/// <para>where the value of the GUID is represented as a series of lower-case hexadecimal digits in groups of 8, 4, 4, 4, and 12 digits and separated by hyphens. An example of a return value is "382c74c3-721d-4f34-80e5-57657b6cbc27".</para>
		/// </returns>
		public override string ToString()
		{
			return BaseToString(true, false, false);
		}

		/// <summary>
		/// Returns a <see langword="String" /> representation of the value of this	<see langword="GuidEx" /> instance, according to the provided format specifier.
		/// </summary>
		/// <param name="format">A single format specifier that indicates how to format the value of this <see cref="T:OpenNETCF.GuidEx" />. The <paramref name="format" /> parameter can be "N", "D", "B", or "P". If <paramref name="format" /> is <see langword="null" /> or the empty string (""), "D" is used.</param>
		/// <returns>A <see cref="T:System.String" /> representation of the value of this <see cref="T:OpenNETCF.GuidEx" />.</returns>
		/// <exception cref="T:System.FormatException">The value of <paramref name="format" /> is not <see langword="null" />, the empty string (""), "N", "D", "B", or "P".</exception>
		public string ToString(string format) 
		{
			string f;
			bool h = true;
			bool p = false;
			bool b = false;

			if(format != null)
			{
				f = format.ToLower();

				if(f == "b") 
				{
					b = true;
				}
				else if(f == "p") 
				{
					p = true;
				} 
				else if(f == "n") 
				{
					h = false;
				}
				else if(f != "d" && f != "")
				{
					throw new FormatException("Argument to GuidEx.ToString(string format) should be \"b\", \"B\", \"d\", \"D\", \"n\", \"N\", \"p\", or \"P\"");
				}
			}
			
			return BaseToString(h, p, b);
		}

		/// <summary>
		/// Returns a <see cref="T:System.String" /> representation of the value of this instance of the <see cref="T:OpenNETCF.GuidEx" /> class, according to the provided format specifier and culture-specific format information.
		/// </summary>
		/// <param name="format">A single format specifier that indicates how to format the value of this <see cref="T:OpenNETCF.GuidEx" />. The <paramref name="format" /> parameter can be "N", "D", "B", or "P". If <paramref name="format" /> is <see langword="null" /> or the empty string (""), "D" is used.</param>
		/// <param name="provider">(Reserved) An <see langword="IFormatProvider" /> reference that supplies culture-specific formatting services.</param>
		/// <returns>A <see cref="T:System.String" /> representation of the value of this <see cref="T:OpenNETCF.GuidEx" />.</returns>
		/// <exception cref="T:System.FormatException">The value of <paramref name="format" /> is not <see langword="null" />, the empty string (""), "N", "D", "B", or "P".</exception>
		public string ToString(string format, IFormatProvider provider)
		{
			return ToString(format);
		}

		/// <summary>
		/// Returns an indication whether the values of two specified <see langword="GuidEx" /> objects are equal.
		/// </summary>
		/// <param name="a">A <see langword="GuidEx" /> object.</param>
		/// <param name="b">A <see langword="GuidEx" /> object.</param>
		/// <returns><see langword="true" /> if <paramref name="a " />and <paramref name="b " />are equal; otherwise, <see langword="false" /> .</returns>
		public static bool operator ==(GuidEx a, GuidEx b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Returns an indication whether the values of two specified <see langword="GuidEx" /> objects are not equal.
		/// </summary>
		/// <param name="a">A <see langword="GuidEx" /> object.</param>
		/// <param name="b">A <see langword="GuidEx" /> object.</param>
		/// <returns><see langword="true" /> if <paramref name="a " />and <paramref name="b " />are not equal; otherwise, <see langword="false" /> .</returns>
		public static bool operator !=(GuidEx a, GuidEx b)
		{
			return !(a.Equals(b));
		}
	}
	#endregion

	#region Internal Classes
	internal class GuidParser
	{

		private string _src;
		private int _length;
		private int _cur;

		public GuidParser(string src)
		{
			_src = src;
			Reset();
		}

		private void Reset()
		{
			_cur = 0;
			_length = _src.Length;
		}

		private bool AtEnd()
		{
			return _cur >= _length;
		}

		private void ThrowFormatException()
		{
			throw new FormatException("Invalid format for GuidEx.GuidEx(string)");
		}

		private ulong ParseHex(int length, bool strictLength)
		{
			ulong res = 0;
			int i;
			bool end = false;

			for(i=0;(!end) && i<length; ++i)
			{
				if(AtEnd())
				{
					if(strictLength || i==0)
					{
						ThrowFormatException();
					}
					else
					{
						end = true;
					}
				}
				else
				{
					char c = Char.ToLower(_src[_cur]);
					if(Char.IsDigit(c))
					{
						res = res * 16 + c - '0';
						_cur++;
					}
					else if(c >= 'a' && c <= 'f')
					{
						res = res * 16 + c - 'a' + 10;
						_cur++;
					}
					else
					{
						if(strictLength || i==0)
						{
							ThrowFormatException();
						}
						else
						{
							end = true;
						}
					}
				}
			}

			return res;
		}

		private bool ParseOptChar(char c)
		{
			if(!AtEnd() && _src[_cur] == c)
			{
				_cur++;
				return true;
			}
			else
			{
				return false;
			}
		}

		private void ParseChar(char c)
		{
			bool b = ParseOptChar(c);
			if(!b)
			{
				ThrowFormatException();
			}
		}

		private GuidEx ParseGuid1()
		{
			bool openBrace;
			int a;
			short b;
			short c;
			byte[] d = new byte[8];
			int i;

			openBrace = ParseOptChar('{');
			a =(int) ParseHex(8, true);
			ParseChar('-');
			b =(short) ParseHex(4, true);
			ParseChar('-');
			c =(short) ParseHex(4, true);
			ParseChar('-');
			for(i=0; i<8; ++i)
			{
				d[i] =(byte) ParseHex(2, true);
				if(i == 1)
				{
					ParseChar('-');
				}
			}

			if(openBrace && !ParseOptChar('}'))
			{
				ThrowFormatException();
			}

			return new GuidEx(a, b, c, d);
		}

		private void ParseHexPrefix()
		{
			ParseChar('0');
			ParseChar('x');
		}

		private GuidEx ParseGuid2()
		{
			int a;
			short b;
			short c;
			byte[] d = new byte [8];
			int i;

			ParseChar('{');
			ParseHexPrefix();
			a =(int) ParseHex(8, false);
			ParseChar(',');
			ParseHexPrefix();
			b =(short) ParseHex(4, false);
			ParseChar(',');
			ParseHexPrefix();
			c =(short) ParseHex(4, false);
			ParseChar(',');
			ParseChar('{');

			for(i=0; i<8; ++i)
			{
				ParseHexPrefix();
				d[i] =(byte) ParseHex(2, false);
				if(i != 7)
				{
					ParseChar(',');
				}

			}
			ParseChar('}');
			ParseChar('}');

			return new GuidEx(a,b,c,d);

		}

		public GuidEx Parse()
		{
			GuidEx g;

			try
			{
				g  = ParseGuid1();
			}
			catch(FormatException)
			{
				Reset();
				g = ParseGuid2();
			}
			if(!AtEnd() )
			{
				ThrowFormatException();
			}
			return g;
		}
	}

	internal class GuidState
	{
		protected Random _pRnd; // Pseudo RNG
		protected bool _usePRnd; // 'true' for pseudi RNG
		protected ushort _clockSeq;
		protected ulong _lastTimestamp;
		protected byte[] _mac;

		public int NextInt(uint x)
		{
			if(_usePRnd)
			{
				return _pRnd.Next((int)x);
			}
			else
			{
				// Should use a better RNG here but none available (yet)
				return _pRnd.Next((int) x);
			}
		}

		public void NextBytes(byte[] b)
		{
			if(_usePRnd)
			{
				_pRnd.NextBytes(b);
			} 
			else 
			{
				// Should use a better RNG here but none available (yet)
				_pRnd.NextBytes(b);
			}
		}

		public GuidState(bool usePRnd)
		{
			_usePRnd = usePRnd;
			if(_usePRnd)
			{
				_pRnd = new Random(unchecked((int) DateTime.Now.Ticks));
			}
			else 
			{
				_pRnd = new Random(unchecked((int) DateTime.Now.Ticks));
			}

			_clockSeq = (ushort)NextInt(0x4000); // 14 bits
			_lastTimestamp = 0ul;
			_mac = new byte[6];
			_mac[0] |= 0x80;
		}

		public ulong NewTimeStamp()
		{
			ulong timeStamp;
			do
			{
				timeStamp = (ulong)(DateTime.UtcNow - new DateTime(1582,10,15,0,0,0)).Ticks;

				if(timeStamp < _lastTimestamp)
				{
					_clockSeq++;
					_clockSeq = (ushort)(_clockSeq & 0x3fff);
					return timeStamp;
				}

				if(timeStamp > _lastTimestamp)
				{
					_lastTimestamp = timeStamp;
					return timeStamp;
				}

			} while(true);
		}

		public ushort ClockSeq
		{
			get { return _clockSeq; }
		}

		public byte[] MAC
		{
			get { return _mac; }
		}
	}
	#endregion
}
