//==========================================================================================
//
//		OpenNETCF.Multimedia.Audio.RiffChunk
//	    OpenNETCF.Multimedia.Audio.WaveChunk
//		OpenNETCF.Multimedia.Audio.FmtChunk
//		OpenNETCF.Multimedia.Audio.DataChunk
//
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
using System.IO;
using OpenNETCF.Win32;

namespace OpenNETCF.Multimedia.Audio
{
	/// <summary>
	/// Type of mm chunk.
	/// </summary>
	internal enum ChunkType
	{
		/// <summary>
		/// Chunk is Riff chunk
		/// </summary>
		List,
		/// <summary>
		/// Chunk is List chunk
		/// </summary>
		Riff
	}

	internal class RiffChunk: _RiffChunk
	{
		public RiffChunk(Stream st): base(new FourCC('R', 'I', 'F', 'F'), st)
		{
		}
	}

	internal class WaveChunk: ListChunk
	{
		public WaveChunk(Stream st): base(new FourCC('W', 'A', 'V', 'E'), st)
		{
		}
	}

	internal class FmtChunk: _RiffChunk
	{
		private WaveFormatEx m_fmt;
		public WaveFormatEx WaveFormat { get { return m_fmt; } set { m_fmt = value; } }
		public FmtChunk(Stream st, WaveFormatEx fmt): base(new FourCC('f', 'm', 't', ' '), st)
		{
			m_fmt = fmt;
		}

		public override int GetSize()
		{
			return base.GetSize () + m_fmt.Size + m_fmt.GetBytes().Length;
		}
		public override void LoadFromStream(Stream st)
		{
			m_posStart = st.Position - 4;
			byte[] data = new byte[4];
			st.Read(data, 0, 4);
			m_nSize = BitConverter.ToInt32(data, 0);
			data = new byte[18];
			st.Read(data, 0, Math.Min(18, m_nSize));
			m_fmt = new WaveFormatEx(data);
			st.Position = m_posStart + m_nSize;
		}

	}

	internal class DataChunk: _RiffChunk
	{
		public DataChunk(Stream st): base(new FourCC('d', 'a', 't', 'a'), st)
		{
		}
		public override int GetSize()
		{
			return base.GetSize () + m_nSize;
		}

		public override void LoadFromStream(Stream st)
		{
			m_posStart = st.Position - 4;
			byte[] data = new byte[4];
			st.Read(data, 0, 4);
			m_nSize = BitConverter.ToInt32(data, 0);
			st.Position += m_nSize;
		}

		override public void EndWrite()
		{
			m_nSize = (int)(m_str.Position - m_posStart) - 8;
			m_str.Position = m_posStart;
			Write(BitConverter.GetBytes((int)m_fourCC));
			m_str.Write(BitConverter.GetBytes(m_nSize), 0, 4);
			m_str.Position = m_posCurrent;
		}

		public int DataSize { get { return m_nSize; } }
	}
}
 

 