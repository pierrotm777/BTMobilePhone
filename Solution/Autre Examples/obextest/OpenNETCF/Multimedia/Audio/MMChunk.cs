//==========================================================================================
//
//		OpenNETCF.Multimedia.Audio.MMChunk
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
using System.Collections;

namespace OpenNETCF.Multimedia.Audio
{
	internal class MMChunk
	{
		protected FourCC m_fourCC;
		protected int m_nSize;
		protected bool m_bDirty;
		protected Stream m_str;
		protected ArrayList m_chunkList;
		protected ChunkType m_ckType;
		protected long m_posCurrent, m_posStart;

		public FourCC FourCC { get { return m_fourCC; } }
		public MMChunk this[int index] { get { return m_chunkList[index] as MMChunk; } }
		public MMChunk FindChunk(FourCC fcc)
		{
			foreach(MMChunk ck in m_chunkList)
			{
				if ( ck.FourCC == fcc )
				{
					return ck;
				}
			}
			return null;
		}

		public MMChunk(FourCC fourCC, Stream st)
		{
			m_fourCC = fourCC;
			m_nSize = 0;
			m_bDirty = false;
			m_str = st;
			m_posCurrent = st.Position;
			m_posStart = st.Position;
			m_chunkList = new ArrayList();
			m_ckType = ChunkType.Riff;
		}

		virtual public void Flush()
		{
			if ( !IsDirty() )
				return;
		}

		virtual public bool IsDirty()
		{
			return m_bDirty;
		}

		virtual public MMChunk CreateSubChunk(FourCC fourCC, Stream st)
		{
			if ( fourCC == FourCC.Riff )
			{
				MMChunk ck = new RiffChunk(st);
				m_chunkList.Add(ck);
				return ck;
			}
			else if ( fourCC == FourCC.Wave )
			{
				MMChunk ck = new WaveChunk(st);
				m_chunkList.Add(ck);
				return ck;
			}
			else if ( fourCC == FourCC.Fmt )
			{
				MMChunk ck = new FmtChunk(st, null);
				m_chunkList.Add(ck);
				return ck;
			}
			else if ( fourCC == FourCC.Data )
			{
				MMChunk ck = new DataChunk(st);
				m_chunkList.Add(ck);
				return ck;
			}
			
			return null;
		}

		static public MMChunk FromStream(Stream st)
		{
			if ( !st.CanRead )
				return null;
			if ( st.Position > st.Length - 4 )
				return null;
			byte[] data = new byte[4];
			st.Read(data, 0, 4);
			FourCC fourCC = BitConverter.ToInt32(data, 0);
			if ( fourCC == FourCC.Riff )
			{
				RiffChunk ck = new RiffChunk(st);
				ck.LoadFromStream(st);
				return ck;
			}
			else if ( fourCC == FourCC.Wave )
			{
				WaveChunk ck = new WaveChunk(st);
				ck.LoadFromStream(st);
				return ck;
			}
			else if ( fourCC == FourCC.Fmt )
			{
				FmtChunk ck = new FmtChunk(st, null);
				ck.LoadFromStream(st);
				return ck;
			}
			else if ( fourCC == FourCC.Data )
			{
				DataChunk ck = new DataChunk(st);
				ck.LoadFromStream(st);
				return ck;
			}
			else
				return new MMChunk(fourCC, st);
		}

		public virtual int GetSize()
		{ 
			return 4; //FourCC
		}

		public void SetStart(long pos)
		{
			m_posStart = pos;
		}
	}
}
