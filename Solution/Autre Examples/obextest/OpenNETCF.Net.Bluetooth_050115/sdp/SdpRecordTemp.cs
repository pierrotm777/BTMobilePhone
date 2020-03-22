using System;

namespace OpenNETCF.Net.Bluetooth.Sdp
{
	/// <summary>
	/// Summary description for SdpRecord.
	/// </summary>
	public class SdpRecordTemp
	{
		byte[] m_data;

		public SdpRecordTemp()
		{
			m_data = new byte[] {0x35,0x27,0x09,0x00,0x01,0x35,0x11,0x1c,0x00,0x00,0x00,
								0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
								0x00,0x00,0x09,0x00,0x04,0x35,0x0c,0x35,0x03,0x19,0x01,
								0x00,0x35,0x05,0x19,0x00,0x03,0x08,0x00};
		}

		public Guid Service
		{
			get
			{
				byte[] guidbytes = new byte[16];
				Buffer.BlockCopy(m_data, 8, guidbytes, 0, 16);
				return new Guid(guidbytes);
			}
			set
			{
				Buffer.BlockCopy(value.ToByteArray(), 0, m_data, 8, 16);
			}
		}

		public byte Channel
		{
			get
			{
				return m_data[m_data.Length - 1];
			}
			set
			{
				m_data[m_data.Length - 1] = value;
			}
		}

		public byte[] ToByteArray()
		{
			return m_data;
		}
	}
}
