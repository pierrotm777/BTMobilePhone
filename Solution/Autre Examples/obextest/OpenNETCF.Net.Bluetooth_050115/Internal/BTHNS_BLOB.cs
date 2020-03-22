using System;

namespace OpenNETCF.Net.Bluetooth.Internal
{
	/// <summary>
	/// 
	/// </summary>
	internal abstract class BTHNS_BLOB : IDisposable
	{
		protected byte[] m_data;

		/// <summary>
		/// Size of the structure.
		/// </summary>
		internal int Length
		{
			get
			{
				return m_data.Length;
			}
		}

		/// <summary>
		/// Internal bytes
		/// </summary>
		/// <returns></returns>
		internal byte[] ToByteArray()
		{
			return m_data;
		}

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				m_data = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~BTHNS_BLOB()
		{
			Dispose(false);
		}
		#endregion
	}
}
