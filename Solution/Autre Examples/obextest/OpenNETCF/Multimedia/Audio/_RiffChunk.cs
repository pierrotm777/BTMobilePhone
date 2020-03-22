//==========================================================================================
//
//		OpenNETCF.Multimedia.Audio._RiffChunk
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

namespace OpenNETCF.Multimedia.Audio
{
	internal class _RiffChunk: MMChunk
	{
		public _RiffChunk(FourCC fourCC, Stream st): base(fourCC, st)
		{
			m_ckType = ChunkType.Riff;
		}

		public void Write(byte[] data, int offset, int count)
		{
			m_str.Write(data, offset, count);
		}

		public void Write(byte[] data)
		{
			Write(data, 0, data.Length);
			m_posCurrent = m_str.Position;
		}

		public override int GetSize()
		{
			int size = base.GetSize () + 4; // + ckSize
			foreach( MMChunk ck in m_chunkList )
			{
				size += ck.GetSize();
			}
			return size;
		}

		public virtual void UpdateSize()
		{
			int nSize = GetSize();
			long lPos = m_str.Position;
			m_str.Position = m_posStart + 4;
			m_str.Write(BitConverter.GetBytes(nSize), 0, 4);
			m_str.Position = lPos;
		}
		virtual public void LoadFromStream(Stream st)
		{
			m_posStart = st.Position - 4;
			byte[] data = new byte[4];
			st.Read(data, 0, 4);
			m_nSize = BitConverter.ToInt32(data, 0);
			while ( st.Position - m_posStart != m_nSize + 8)
			{
				MMChunk ck = MMChunk.FromStream(st);
				if ( ck == null )
					break;
				m_chunkList.Add(ck);
			}
		}

		public Stream BeginRead()
		{
			m_posCurrent = m_str.Position;
			m_str.Position = m_posStart + 8;
			return m_str;
		}

		public void EndRead()
		{
			m_str.Position = m_posCurrent;
		}

		public Stream BeginWrite()
		{
			m_posCurrent = m_str.Position;
			m_str.Position = m_posStart + 8;
			return m_str;
		}
		virtual public void EndWrite()
		{
			int nSize = GetSize() - 8;
			m_str.Position = m_posStart;
			Write(BitConverter.GetBytes((int)m_fourCC));
			m_str.Write(BitConverter.GetBytes(nSize), 0, 4);
			m_str.Position = m_posCurrent;
		}
	}
}
