/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2009 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    hid_tst.c
Abstract:
    this module is to test HID profiles relative functionality. 
Revision History:
	2009-11-26   Chu Enlai  Created

---------------------------------------------------------------------------*/

#include "sdk_tst.h"
#include "profiles_tst.h"
#include "Btsdk_Stru.h"

static BTCONNHDL s_currHidConnHdl = BTSDK_INVALID_HANDLE;
static BTDEVHDL s_currRmtHidDevHdl = BTSDK_INVALID_HANDLE;

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select device's handle.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectRmtHidDev()
{   
    s_currRmtHidDevHdl = SelectRemoteDevice(0);
	if (BTSDK_INVALID_HANDLE == s_currRmtHidDevHdl)
	{
		printf("Please make sure that the expected device \
				is in discoverable state and search again.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to connect HID device.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestConnectHidSvc()
{
	ULONG ulSvcHdlCnt = 0;
	BTUINT32 ulRet = BTSDK_FALSE;
	BTSDKHANDLE hEnum = BTSDK_INVALID_HANDLE;
	BTSVCHDL hSvcHdl = BTSDK_INVALID_HANDLE;

	BtSdk_SDAP_PNPINFO PnpInfo = {0};
	BtSdkRemoteServiceAttrStru struRmtSvcAttr = {0};

	Btsdk_BrowseRemoteServices(s_currRmtHidDevHdl, NULL, &ulSvcHdlCnt);	

	hEnum = Btsdk_StartEnumRemoteService(s_currRmtHidDevHdl);
	if (BTSDK_INVALID_HANDLE == hEnum)
	{
		return;
	}

	struRmtSvcAttr.mask = BTSDK_RSAM_EXTATTRIBUTES;

	while (BTSDK_INVALID_HANDLE != (hSvcHdl = Btsdk_EnumRemoteService(hEnum, &struRmtSvcAttr)))
	{
		if (NULL != struRmtSvcAttr.ext_attributes)
		{
			if (BTSDK_CLS_PNP_INFO == struRmtSvcAttr.svc_class)
			{			
				PnpInfo.size = sizeof(BtSdk_SDAP_PNPINFO);
				PnpInfo.product_id = ((BtSdkRmtDISvcExtAttrStru*)struRmtSvcAttr.ext_attributes)->product_id;
				PnpInfo.mask = ((BtSdkRmtDISvcExtAttrStru*)struRmtSvcAttr.ext_attributes)->mask;
				PnpInfo.spec_id = ((BtSdkRmtDISvcExtAttrStru*)struRmtSvcAttr.ext_attributes)->spec_id;
				PnpInfo.svc_hdl = 0;
				PnpInfo.vendor_id = ((BtSdkRmtDISvcExtAttrStru*)struRmtSvcAttr.ext_attributes)->vendor_id;
				PnpInfo.vendor_id_src = ((BtSdkRmtDISvcExtAttrStru*)struRmtSvcAttr.ext_attributes)->vendor_id_source;
				PnpInfo.version_value = ((BtSdkRmtDISvcExtAttrStru*)struRmtSvcAttr.ext_attributes)->version;
				Btsdk_FreeMemory(struRmtSvcAttr.ext_attributes);
				struRmtSvcAttr.ext_attributes = NULL;
				break;
			}
			else
			{
				Btsdk_FreeMemory(struRmtSvcAttr.ext_attributes);
				struRmtSvcAttr.ext_attributes = NULL;
			}
		}
	}
	
	Btsdk_EndEnumRemoteService(hEnum);
	hEnum = BTSDK_INVALID_HANDLE;
	
	ulRet = Btsdk_ConnectEx(s_currRmtHidDevHdl, BTSDK_CLS_HID, (BTUINT32)&PnpInfo, &s_currHidConnHdl);
	
	if (BTSDK_OK != ulRet)
	{
		printf("Please make sure that the expected device is powered on and connectable.\n");
		return;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show HID Services user interface.
Arguments:
    void
Return:
	void 
---------------------------------------------------------------------------*/
void HidShowMenu()
{
	printf("*****************************************\n");
	printf("*           HID Testing Menu            *\n");
	printf("* <1> Connect Remote HID Device         *\n");
	printf("* <2> Unplug Device                     *\n");
	printf("* <r> Return to upper menu              *\n");
	printf("*****************************************\n");
	printf(">");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution for HID sample.
Arguments:
	BTUINT8 choice
Return:
	void 
---------------------------------------------------------------------------*/
void HidExecCmd(BTUINT8 choice)
{
	BTINT8 ch = 0;
	BTUINT8 comNum = 0;
	BTUINT8 bd_addr[6] = {0};

	if (choice == '1')
	{
		TestSelectRmtHidDev();
		TestConnectHidSvc();
		HidShowMenu();
		while (ch != 'r')
		{
			scanf(" %c", &ch);
			getchar();		
			if (ch == '\n')
			{
				printf(">>");
			}
			else if('r' == ch)
			{
				break;
			}
			else
			{
				HidExecCmd(ch);
				printf("\n");
				HidShowMenu();
			}
		}
	}
	else if (choice == '2')
	{
		if (s_currHidConnHdl)
		{
			Btsdk_GetRemoteDeviceAddress(s_currRmtHidDevHdl, bd_addr);
			Btsdk_Hid_ClntUnPluggedDev(bd_addr);
			s_currHidConnHdl = BTSDK_INVALID_HANDLE;
		}
		else
		{
			printf("make sure exist connection now!");
		}
	}
	else
	{
		printf("Invalid command.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the entry function for HID sample.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestHidFunc(void)
{
	BTUINT8 ch = 0;
	BTUINT8 comNum = 0;
	BTUINT8 bd_addr[6] = {0};

	HidShowMenu();	
	while (ch != 'r')
	{
		scanf(" %c", &ch);
		getchar();		
		if (ch == '\n')
		{
			printf(">>");
		}
		else if('r' == ch)
		{
			break;
		}
		else
		{
			HidExecCmd(ch);
			printf("\n");
			HidShowMenu();
		}
	}
	if (BTSDK_INVALID_HANDLE != s_currHidConnHdl)
	{
		Btsdk_GetRemoteDeviceAddress(s_currRmtHidDevHdl, bd_addr);
		Btsdk_Hid_ClntUnPluggedDev(bd_addr);
		s_currHidConnHdl = BTSDK_INVALID_HANDLE;
	}
}




















