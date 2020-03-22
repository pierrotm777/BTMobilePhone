//==========================================================================================
//
//		OpenNETCF.Multimedia.Audio.UserChunk
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
	internal class ListChunk: MMChunk
	{
		public ListChunk(FourCC fourCC, Stream st): base(fourCC, st)
		{
			m_ckType = ChunkType.List;
		}

		public override MMChunk CreateSubChunk(FourCC fourCC, Stream st)
		{
			MMChunk ck = base.CreateSubChunk(fourCC, st);
			int size = 0;
			foreach( MMChunk cck in m_chunkList )
			{
				if ( cck != ck )
					size += cck.GetSize();
			}
			ck.SetStart( m_posStart + 4 + size);
			return ck;
		}

		public override int GetSize()
		{
			int size = base.GetSize ();
			foreach( MMChunk ck in m_chunkList )
			{
				size += ck.GetSize();
			}
			return size;
		}

		virtual public void LoadFromStream(Stream st)
		{
			while ( true )
			{
				MMChunk ck = MMChunk.FromStream(st);
				if ( ck == null )
					break;
				m_chunkList.Add(ck);
			}
		}

		public Stream BeginWrite()
		{
			m_posCurrent = m_str.Position;
			m_str.Position = m_posStart + 4;
			return m_str;
		}

		public void EndWrite()
		{
			long lPos = m_str.Position;
			m_str.Position = m_posStart;
			m_str.Write(BitConverter.GetBytes((int)m_fourCC), 0, 4);
			m_str.Position = lPos;
		}

	}
}
