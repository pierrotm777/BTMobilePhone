using System;

namespace OpenNETCF.Net
{
	/// <summary>
	/// 
	/// </summary>
	public class Networking
	{
		private Networking() {}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static AdapterCollection GetAdapters()
		{
			return ( new AdapterCollection() );
		}
	}
}
