/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2008 IVT Corporation
*
* All rights reserved.
* 
---------------------------------------------------------------------------*/

/////////////////////////////////////////////////////////////////////////////
// Module Name:
//    
// Abstract:
//     
// Usage:
//     
// 
// Author:    
//     chenjinfeng
// Revision History:
//     2008-10-15		Created
// 
/////////////////////////////////////////////////////////////////////////////
#include "sdk_tst.h"

void Test_Btsdk_SetLocalName(void)
{
	BTUINT8 name[BTSDK_DEVNAME_LEN];
	BTUINT16 len;

	OnSelDlg(g_hWnd, _T("Set Local Name"), _T("Please enter local name in the edit"), (char *)name, sizeof(name));
	
	PRINTMSG(1, "Local Name = %s\r\n", name);
	len  = strlen((char *)name) + 1;
	BTSDK_CHECK_RETURN_CODE(Btsdk_SetLocalName(name, len));
}

void Test_Btsdk_SetDiscoveryMode(void)
{
	BTUINT16 mode;

	PRINTMSG(1, "Set Discover Mode:\n");
	PRINTMSG(1, "1. GENERAL_DISCOVERABLE\n");
	PRINTMSG(1, "2. LIMITED_DISCOVERABLE\n");
	PRINTMSG(1, "4. CONNECTABLE\n");
	PRINTMSG(1, "8. PAIRABLE\n");
	PRINTMSG(1, "15. GENERAL_DISCOVERABLE|CONNECTABLE|PAIRABLE\n");
	PRINTMSG(1, "Choose = "); 
	
	mode = OnSelDlg(g_hWnd, _T("Set Discovery Mode"), _T("Please enter number according to the prompt"), NULL, 0);
	switch(mode)
	{
	case 1:
		PRINTMSG(1, "Set Discovery Mode(%s)\r\n", "GENERAL_DISCOVERABLE");
		break;
	case 2:
		PRINTMSG(1, "Set Discovery Mode(%s)\r\n", "LIMITED_DISCOVERABLE");
		break;
	case 4:
		PRINTMSG(1, "Set Discovery Mode(%s)\r\n", "CONNECTABLE");
		break;
	case 8:
		PRINTMSG(1, "Set Discovery Mode(%s)\r\n", "PAIRABLE");
		break;
	case 15:
		PRINTMSG(1, "Set Discovery Mode(%s)\r\n", "GENERAL_DISCOVERABLE|CONNECTABLE|PAIRABLE");
		break;
	}
	
	BTSDK_CHECK_RETURN_CODE(Btsdk_SetDiscoveryMode(mode));
}

void Test_Btsdk_SetLocalClassDevice(void)
{
	BTUINT16 mode;
	BTUINT32 device_class;

	PRINTMSG(1, "Set device class:\n");
	PRINTMSG(1, "1. BTSDK_DEVCLS_COMPUTER\n");
	PRINTMSG(1, "2. BTSDK_DEVCLS_PHONE\n");
	PRINTMSG(1, "3. BTSDK_DEVCLS_MISC\n");
	PRINTMSG(1, "Choose = "); 
	
	//scanf("%ld", &device_class);
	mode = OnSelDlg(g_hWnd, _T("Set device classe"), _T("Please enter number according to the prompt"), NULL, 0);
	switch(mode)
	{
	case 1:
		PRINTMSG(1, "Set device class(%s)\r\n", "BTSDK_DEVCLS_COMPUTER");
		device_class = 0x000100;
		break;
	case 2:
		PRINTMSG(1, "Set device class(%s)\r\n", "BTSDK_DEVCLS_PHONE");
		device_class = 0x000200;
		break;
	case 3:
		PRINTMSG(1, "Set device class(%s)\r\n", "BTSDK_DEVCLS_MISC");
		device_class = 0x000000;
		break;
	}

	PRINTMSG(1, "Device Class = %0x", device_class);
	
	BTSDK_CHECK_RETURN_CODE(Btsdk_SetLocalClassDevice(device_class));
}

void Test_Btsdk_GetLocalLMPInfo(void)
{
	int j;
	BtSdkLocalLMPInfoStru lmp_info;

	PRINTMSG(1, "\n\n");
	BTSDK_CHECK_RETURN_CODE(Btsdk_GetLocalLMPInfo(&lmp_info));

	PRINTMSG(1, "LMP feature=");
	for(j = 0; j < 7; j++)
		PRINTMSG(1, "%02X:", lmp_info.lmp_feature[j]);

	PRINTMSG(1, "%02X\n", lmp_info.lmp_feature[j]);
	PRINTMSG(1, "The name of the manufacturer = %04X\n", lmp_info.manuf_name);
	PRINTMSG(1, "The sub version of the LMP firmware = %04X\n", lmp_info.lmp_subversion);
	PRINTMSG(1, "The main version of the LMP firmware = %02X\n", lmp_info.lmp_version);
	PRINTMSG(1, "HCI version = %02X\n", lmp_info.hci_version);
	PRINTMSG(1, "HCI revision = %04X\n", lmp_info.hci_revision);
	PRINTMSG(1, "Country code = %02X\n", lmp_info.country_code);
	PRINTMSG(1, "\n\n");
}

void Test_VendorSpecificCommand(void)
{
	BTINT32 iRet = 0;
	BTUINT8 cmd[] = {0xC2, 0x00, 0x00, 0x12, 0x00, 
					 0x00, 0x00, 0x03, 0x70, 0x00, 0x00, 0x91, 0x01, 0x0A, 0x00, 0x00, 0x00,
					 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
					 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
	BtSdkVendorCmdStru in_cmd = {0};
	PBtSdkEventParamStru out_ev;

	in_cmd.param_len = sizeof(cmd);
	memcpy(in_cmd.param, cmd, in_cmd.param_len);

	out_ev = (PBtSdkEventParamStru)malloc(256);
	memset(out_ev, 0, 256);
	out_ev->param_len = 255;

	iRet = Btsdk_VendorCommand(0, &in_cmd, out_ev);
	if (iRet == BTSDK_OK)
	{
		PRINTMSG(1, "ok -- send a vendor specific HCI command to the local device \n");
		PRINTMSG(1, "and receives the corresponding event   \n");		
	}
	else
	{
		PRINTMSG(1, "fail(%d)-- send a vendor specific HCI command to the local device \n");
		PRINTMSG(1, "and receives the corresponding event   \n", iRet);	
	}

	free(out_ev);
	return;
}

void Test_Print_Local_Info(void)
{
	int j;
	BTUINT8 name[BTSDK_DEVNAME_LEN + 1];
	BTUINT8 bd_addr[BTSDK_BDADDR_LEN];
	BTUINT32 device_class;
	BTUINT16 mode;
	BTUINT16 len = BTSDK_DEVNAME_LEN;

	PRINTMSG(1, "\n\n");
	BTSDK_CHECK_RETURN_CODE(Btsdk_GetLocalName(name, &len));
	PRINTMSG(1, "Local Name = \"%s\"\n", name);
	BTSDK_CHECK_RETURN_CODE(Btsdk_GetLocalBDAddr(bd_addr));
	
	PRINTMSG(1, "BD Addr: ");
	for(j = 5; j > 0; j--)
		PRINTMSG(1, "%02X:", bd_addr[j]);
	PRINTMSG(1, "%02X\n", bd_addr[0]);
	BTSDK_CHECK_RETURN_CODE(Btsdk_GetLocalClassDevice(&device_class));
	PRINTMSG(1, "Device Class: %08lX\n", device_class);
	BTSDK_CHECK_RETURN_CODE(Btsdk_GetLocalPinCode(name, &len));
	name[len] = '\0';
	PRINTMSG(1, "Pin Code Len: %04X\n", len);
	PRINTMSG(1, "Pin Code: \"%s\"\n", name);
	BTSDK_CHECK_RETURN_CODE(Btsdk_GetDiscoveryMode(&mode));
	PRINTMSG(1, "Discovery Mode: ");
	if (mode & BTSDK_GENERAL_DISCOVERABLE)
		PRINTMSG(1, "GENERAL_DISCOVERABLE |");
	if (mode & BTSDK_LIMITED_DISCOVERABLE)
		PRINTMSG(1, " LIMITED_DISCOVERABLE |");
	if (mode & BTSDK_CONNECTABLE)
		PRINTMSG(1, " CONNECTABLE |");
	if (mode & BTSDK_PAIRABLE)
		PRINTMSG(1, " PAIRABLE |");
	PRINTMSG(1, "\n");
}

void LocDevShowMenu()
{
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, "*   Local Device Manager Testing Menu   *\n");
	PRINTMSG(1, "* <1> Set Local Name                    *\n");
	PRINTMSG(1, "* <2> Set Discovery Mode                *\n");
	PRINTMSG(1, "* <3> Set Local Device Class            *\n");
	PRINTMSG(1, "* <5> Get Local LMP Info                *\n");
//	PRINTMSG(1, "* <6> Send Vendor Specific Command      *\n");
	PRINTMSG(1, "* <7> Print Local Information           *\n");
	PRINTMSG(1, "* <m> Show this menu                    *\n");
	PRINTMSG(1, "* <q> Quit                              *\n");
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, ">");
}


void LocDevExecCmd(BTUINT8 choice)
{
	switch (choice) 
	{
		case '1':
			Test_Btsdk_SetLocalName();
			break;
		case '2':
			Test_Btsdk_SetDiscoveryMode();
			break;
		case '3':
			Test_Btsdk_SetLocalClassDevice();
			break;
		case '5':
			Test_Btsdk_GetLocalLMPInfo();
			break;
		case '6':
//			Test_VendorSpecificCommand();
			break;
		case '7':
			Test_Print_Local_Info();
			break;
		case 'm':
			LocDevShowMenu();
			break;
		case 'q':
			InterlockedDecrement(&g_NumberLevel); 
			break;
		default:
			PRINTMSG(1, "Invalid command.\n");
			break;
	}
}


void TestLocDevMgr(void)
{
	BTUINT8 ch = 0;

	LocDevShowMenu();

	while (ch != 'q')
	{
		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TestLocDevMgr: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd,g_NumberLevel);

		LocDevExecCmd(ch);
	}
}

