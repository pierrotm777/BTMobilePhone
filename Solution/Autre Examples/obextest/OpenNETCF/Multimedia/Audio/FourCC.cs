//==========================================================================================
//
//		OpenNETCF.Multimedia.Audio.FourCC
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

namespace OpenNETCF.Multimedia.Audio
{
	internal struct FourCC
	{
		public char m_c1;
		public char m_c2;
		public char m_c3;
		public char m_c4;
		public int GetFourCC()
		{
			return (int)( (Convert.ToInt32(m_c4) << 24) | (Convert.ToInt32(m_c3) << 16) | (Convert.ToInt32(m_c2) << 8) | Convert.ToInt32(m_c1) );
		}

		public FourCC(int fcc)
		{
			m_c4 = Convert.ToChar((fcc >> 24) & 0xff); 
			m_c3 = Convert.ToChar((fcc >> 16) & 0xff); 
			m_c2 = Convert.ToChar((fcc >> 8) & 0xff); 
			m_c1 = Convert.ToChar((fcc) & 0xff); 
		}


		public FourCC(char c1, char c2, char c3, char c4)
		{
			m_c1 = c1; m_c2 = c2; m_c3 = c3; m_c4 = c4;
		}

		static public FourCC Riff = new FourCC('R', 'I', 'F', 'F');
		static public FourCC Wave = new FourCC('W', 'A', 'V', 'E');
		static public FourCC Fmt = new FourCC('f', 'm', 't', ' ');
		static public FourCC Data = new FourCC('d', 'a', 't', 'a');

		public override bool Equals(object obj)
		{
			if ( obj is FourCC )
			{
				FourCC fcc = (FourCC)obj;
				//return fcc.m_c1 = c1 && fcc.m_c2 = c2 && fcc.m_c3 = c3 && fcc.m_c4 = c4;
				return fcc.GetFourCC() == GetFourCC();
			}
			else
				return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return GetFourCC();
		}
		public static implicit operator int(FourCC fcc)
		{
			return fcc.GetFourCC();
		}
		public static implicit operator FourCC(int val)
		{
			return new FourCC(val);
		}

	}
}
