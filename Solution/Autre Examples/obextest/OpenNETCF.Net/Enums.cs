using System;

namespace OpenNETCF.Net
{
//	/// <summary>
//	/// Enumeration giving relative strength indication for
//	/// RF signal.
//	/// </summary>
//	public enum SignalQuality
//	{
//		/// <summary>
//		/// No signal is being received by the RF adapter or
//		/// the adapter is not an RF adapter at all.
//		/// </summary>
//		NoSignal,
//
//		/// <summary>
//		/// Signal strength is very low.
//		/// </summary>
//		VeryLow,
//
//		/// <summary>
//		/// Signal strength is low.
//		/// </summary>
//		Low,
//
//		/// <summary>
//		/// Signal strength is acceptable.
//		/// </summary>
//		Good,
//
//		/// <summary>
//		/// Signal strength is ample.
//		/// </summary>
//		VeryGood,
//
//		/// <summary>
//		/// Signal strength is at the highest level.
//		/// </summary>
//		Excellent,
//
//		/// <summary>
//		/// The adapter is definitely not an RF adapter.
//		/// </summary>
//		NotAnRFAdapter
//	}

	/// <summary>
	/// Enumeration returned in the NetworkTypeInUse property.
	/// Indicates the general type of radio network in use.
	/// </summary>
	public enum NetworkType
	{
		/// <summary>
		/// Indicates the physical layer of the frequency hopping spread-spectrum radio
		/// </summary>
		FH,
		/// <summary>
		/// Indicates the physical layer of the direct sequencing spread-spectrum radio
		/// </summary>
		DS,
		/// <summary>
		/// Indicates the physical layer for 5-GHz Orthagonal Frequency Division Multiplexing radios
		/// </summary>
		OFDM5,
		/// <summary>
		/// Indicates the physical layer for 24-GHz Orthagonal Frequency Division Multiplexing radios
		/// </summary>
		OFDM24
	}

	/// <summary>
	/// Define the general network infrastructure mode in
	/// which the selected network is presently operating.
	/// </summary>
	public enum InfrastructureMode
	{
		/// <summary>
		/// Specifies the independent basic service set (IBSS) mode. This mode is also known as ad hoc mode
		/// </summary>
		AdHoc,
		/// <summary>
		/// Specifies the infrastructure mode.
		/// </summary>
		Infrastructure,
		/// <summary>
		/// The infrastructure mode is either set to automatic or cannot be determined.
		/// </summary>
		AutoUnknown
	}
}
