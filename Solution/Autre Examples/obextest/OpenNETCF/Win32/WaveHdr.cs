//==========================================================================================
//
//		OpenNETCF.Win32.WaveHdr
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
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// This class defines the header used to identify a waveform-audio buffer
	/// </summary>
	public sealed class WaveHdr
	{
		private const int lpDataOffset			= 0;
		private const int dwBufferLengthOffset  = lpDataOffset + 4;
		private const int dwBytesRecordedOffset	= dwBufferLengthOffset + 4;
		private const int dwUserOffset			= dwBytesRecordedOffset + 4;
		private const int dwFlagsOffset			= dwUserOffset + 4;
		private const int dwLoopsOffset			= dwFlagsOffset + 4;
		private const int lpNextOffset			= dwLoopsOffset + 4;
		private const int reservedOffset		= lpNextOffset + 4;
		private const int WAVEHDR_SIZE			= reservedOffset + 4;

		private byte[] flatStruct = new byte[4 * 8];

		public byte[] ToByteArray()
		{
			return flatStruct;
		}
		
		public int StructSize
		{
			get
			{
				return WAVEHDR_SIZE;
			}
		}

		public static implicit operator byte[]( WaveHdr wh )
		{
			return wh.flatStruct;
		}

		public WaveHdr(int lpData, int cbDataLength)
		{
			byte[]	bytes = BitConverter.GetBytes( (int)lpData );
			Buffer.BlockCopy( bytes, 0, flatStruct, lpDataOffset, Marshal.SizeOf(lpData));

			bytes = BitConverter.GetBytes( cbDataLength );
			Buffer.BlockCopy( bytes, 0, flatStruct, dwBufferLengthOffset, Marshal.SizeOf(cbDataLength));
		}

		public WaveHdr()
		{
			Array.Clear(flatStruct, 0, flatStruct.Length);
		}

		public WaveHdr( byte[] bytes ) : this( bytes, 0 )
		{
		}

		public WaveHdr( byte[] bytes, int offset )
		{
			Buffer.BlockCopy( bytes, offset, flatStruct, 0, flatStruct.Length );
		}

		public int lpData
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, lpDataOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, lpDataOffset, Marshal.SizeOf(value));
			}
		}

		public int BufferLength
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, dwBufferLengthOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, dwBufferLengthOffset, Marshal.SizeOf(value));
			}
		}

		public int BytesRecorded
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, dwBytesRecordedOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, dwBytesRecordedOffset, Marshal.SizeOf(value));
			}
		}

		public int dwUser
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, dwUserOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, dwUserOffset, Marshal.SizeOf(value));
			}
		}

		public int Flags
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, dwFlagsOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, dwFlagsOffset, Marshal.SizeOf(value));
			}
		}

		public int Loops
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, dwLoopsOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, dwLoopsOffset, Marshal.SizeOf(value));
			}
		}

		public int lpNext
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, lpNextOffset);
			}
		}

		public int reserved
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, reservedOffset);
			}
		}
	}
}
