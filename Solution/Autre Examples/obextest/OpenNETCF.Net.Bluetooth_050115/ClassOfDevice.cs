//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.ClassOfDevice
//		Copyright (C) 2003-2004, OpenNETCF.org
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
using System.Collections;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.Bluetooth
{
	[Flags()]
	public enum ClassOfDevice : int
	{
		InformationService		= 0x800000,
		TelephonyService		= 0x400000,
		AudioService			= 0x200000,
		ObexService				= 0x100000,
		CaptureService			= 0x080000,
		RenderingService		= 0x040000,
		NetworkService			= 0x020000,
		LimitedDiscService		= 0x002000,

		Computer				= 0x000100,
		DesktopComputer			= 0x000104,
		ServerComputer			= 0x000108,
		LaptopComputer			= 0x00010c,
		HandheldComputer		= 0x000110,
		PdaComputer				= 0x000114,
		WearableComputer		= 0x000118,

		Phone					= 0x000200,
		CellPhonePhone			= 0x000204,
		CordlessPhonePhone		= 0x000208,
		SmartPhone				= 0x00020c,
		WiredPhone				= 0x000210,
		IsdnAccess				= 0x000214,

		AccessPointAvailable	= 0x000300,
		AccessPoint1To17		= 0x000304,
		AccessPoint17To33		= 0x000308,
		AccessPoint33To50		= 0x00030c,
		AccessPoint50To67		= 0x000310,
		AccessPoint67To83		= 0x000314,
		AccessPoint83To99		= 0x000318,
		AccessPointNoService	= 0x00031c,

		AudioVideoUnclassified	= 0x000400,
		AudioVideoHeadset		= 0x000404,
		AudioVideoHandsFree		= 0x000408,
		AudioVideoMicrophone	= 0x000410,
		AudioVideoLoudSpeaker	= 0x000414,
		AudioVideoHeadphones	= 0x000418,
		AudioVideoPortable		= 0x00041c,
		AudioVideoCar			= 0x000420,
		AudioVideoSetTopBox		= 0x000424,
		AudioVideoHiFi			= 0x000428,
		AudioVideoVcr			= 0x00042c,
		AudioVideoVideoCamera	= 0x000430,
		AudioVideoCamcorder		= 0x000434,
		AudioVideoMonitor		= 0x000438,
		AudioVideoDisplayLoudSpeaker	= 0x00043c,
		AudioVideoVideoConferencing		= 0x000440,
		AudioVideoGaming				= 0x000448,

		Peripheral				= 0x000500,
		PeripheralJoystick		= 0x000504,
		PeripheralGamepad		= 0x000508,
		PeripheralRemoteControl			= 0x00050c,
		PeripheralSensingDevice			= 0x000510,
		PeripheralDigitizerTablet		= 0x000514,
		PeripheralCardReader			= 0x000518,

		PeripheralKeyboard		= 0x000540,
		PeripheralPointingDevice		= 0x000580,
		

		Imaging					= 0x000600,
		ImagingDisplay			= 0x000610,
		ImagingCamera			= 0x000620,
		ImagingScanner			= 0x000640,
		ImagingPrinter			= 0x000680,

		UnclassifiedDevice		= 0x001f00,
	}
}

 