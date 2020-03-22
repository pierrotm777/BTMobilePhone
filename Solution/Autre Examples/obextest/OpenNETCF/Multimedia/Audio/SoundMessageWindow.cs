//==========================================================================================
//
//		OpenNETCF.Multimedia.Audio.SoundMessageWindow
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
#if !NDOC
using Microsoft.WindowsCE.Forms;
#endif

namespace OpenNETCF.Multimedia.Audio
{
#if!NDOC
	internal class SoundMessageWindow : MessageWindow
	{
		public event WaveOpenHandler		WaveOpenMessage;
		public event WaveCloseHandler		WaveCloseMessage;
		public event WaveDoneHandler		WaveDoneMessage;

		public const int WM_WOM_OPEN  = 0x03BB;
		public const int WM_WOM_CLOSE = 0x03BC;
		public const int WM_WOM_DONE  = 0x03BD;
		public const int MM_WIM_OPEN  = 0x03BE;
		public const int MM_WIM_CLOSE = 0x03BF;
		public const int MM_WIM_DATA  = 0x03C0;
 
		public SoundMessageWindow()
		{
		}

		protected override void WndProc(ref Message msg)
		{
			switch(msg.Msg)
			{
				case WM_WOM_CLOSE:
				case MM_WIM_CLOSE:
					if(WaveCloseMessage != null)
					{
						WaveCloseMessage(this);
					}
					break;
				case WM_WOM_OPEN:
				case MM_WIM_OPEN:
					if(WaveOpenMessage != null)
					{
						WaveOpenMessage(this);
					}
					break;
				case MM_WIM_DATA:
				case WM_WOM_DONE:
					if(WaveDoneMessage != null)
					{
						WaveDoneMessage(this, msg.WParam, msg.LParam);
					}
					break;
			}
 
			// call the base class WndProc for default message handling
			base.WndProc(ref msg);
		}
	}
#endif
}
