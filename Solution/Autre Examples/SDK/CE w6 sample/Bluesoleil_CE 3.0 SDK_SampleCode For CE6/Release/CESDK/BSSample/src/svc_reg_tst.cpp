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

static BTSVCHDL g_loc_svc_hdls[32];
static BTINT32 g_loc_svc_num;

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

---------------------------------------------------------------------------*/
void DisplayLocalServices(void)
{
	int i;
	BTUINT8 DevName[32];
	BtSdkLocalServerAttrStru loc_svc;
	BTINT32 result;
	char quote = ' ';

	g_loc_svc_num = 32;

	PRINTMSG(1, "Btsdk_GetLocalServiceList:svc_hdl = %0x; *svc_count = %0x \r\n",g_loc_svc_hdls ,g_loc_svc_num);

	Btsdk_GetLocalServiceList(g_loc_svc_hdls, (BTUINT32 *)&g_loc_svc_num);
	if (g_loc_svc_num == 0)
		return;

	PRINTMSG(1, "+++++++++++ total %d service registered +++++++++++\n", g_loc_svc_num);

	PRINTMSG(1, "number  service name %21hc service class\n", quote);
	for (i = 0; i < g_loc_svc_num; i++)
	{
		PRINTMSG(1, "  %d%5hc", i + 1, quote);

		loc_svc.mask = BTSDK_LSAM_SERVICENAME|BTSDK_LSAM_EXTATTRIBUTES;
		result = Btsdk_GetServerAttributes(g_loc_svc_hdls[i], &loc_svc);
		if (result != BTSDK_OK)
		{
			PRINTMSG(1, "%-34hs %s\r\n", "Unknown", "Unkonwn");
		}
		else
		{
			MultibyteToMultibyte(CP_UTF8, (char*)loc_svc.svc_name, -1, CP_ACP, (char *)DevName, 32);
			PRINTMSG(1, "%-34hs", DevName);
			PRINTMSG(1, " %04x\r\n", loc_svc.service_class);
			Btsdk_FreeMemory(loc_svc.ext_attributes);
		}
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

---------------------------------------------------------------------------*/
BTDEVHDL SelectLocService(void)
{
	int idx = 0;
	char s[10] = {0};

	if (g_loc_svc_num == 0)
	{
		g_loc_svc_num = 32;
		Btsdk_GetLocalServiceList(g_loc_svc_hdls, (BTUINT32 *)&g_loc_svc_num);
		if (g_loc_svc_num == 0)
			return BTSDK_INVALID_HANDLE;
	}

	DisplayLocalServices();
	PRINTMSG(1, "Select the target service :\n"); 
	PRINTMSG(1, "(Press key 'q' to return to the function list.)\n");
	do
	{
		PRINTMSG(1, "Target device number = ");
		
		OnSelDlg(g_hWnd, _T("Enter the device number"), _T("Please Enter the device number "), s, 10);
		if((s[0] == 'q') || (s[0] == 'Q'))
		{
			PRINTMSG(1, "\nUser abort the operation.\n");
			return BTSDK_INVALID_HANDLE;
		}
		idx = atoi(s);
		if((idx <= 0) || (idx > g_loc_svc_num)){
			PRINTMSG(1, "%d is not a valid datum, please select again.\n", idx);
			continue;
		}
		else
		{
			PRINTMSG(1, " %d  \r\n ", idx);
			break;
		}
	}while(1);

	return (g_loc_svc_hdls[idx - 1]);
}


/*******************************************************************
*																	*
********************************************************************/
void RegisterSPPCom6Service(void)
{
	BTSVCHDL hSvc;
	hSvc = Btsdk_RegisterSPPService(6);
}

/*******************************************************************
*																	*
********************************************************************/
void PassThrInd(BTUINT8 op_id, BTUINT8 state_flag)
{
	PRINTMSG(1, "Receive command %02X, button state %d\n", op_id, state_flag);
}

/*******************************************************************
*																	*
********************************************************************/
void RegisterAVTGService(void)
{
	BTSVCHDL hSvc = Btsdk_RegisterAVRCPTGService();
	Btsdk_AVRCP_RegPassThrCmdCbk(PassThrInd);
}

/*******************************************************************
*																	*
********************************************************************/
void RegisterSPPCom7Service(void)
{
	BTSVCHDL hSvc = Btsdk_RegisterSPPService(7);
}

/*******************************************************************
*																	*
********************************************************************/
void RegisterFTPService(void)
{
	BTSVCHDL hSvc;
	BtSdkLocalServerAttrStru SvrAttr = {0};
	BTSDKHANDLE hEnum = Btsdk_StartEnumLocalServer();
	if (hEnum != BTSDK_INVALID_HANDLE)
	{
		hSvc = Btsdk_EnumLocalServer(hEnum, &SvrAttr);
		while ((hSvc = Btsdk_EnumLocalServer(hEnum, &SvrAttr)) != BTSDK_INVALID_HANDLE)
		{
			if (SvrAttr.service_class == BTSDK_CLS_OBEX_FILE_TRANS)
				break;
		}
		Btsdk_EndEnumLocalServer(hEnum);
	}
	if (hSvc == BTSDK_INVALID_HANDLE)
		hSvc = Btsdk_RegisterFTPService(BTSDK_FTPDA_READWRITE, (BTUINT8 *)"d:\\ftp_server");
	if (hSvc != BTSDK_INVALID_HANDLE)
		Btsdk_FTPRegisterDealReceiveFileCB(AppOBEXServerReceiveFileInd);
}

/*******************************************************************
*																	*
********************************************************************/
void RegisterOPPService(void)
{
	BTSVCHDL hSvc;
	BtSdkLocalServerAttrStru SvrAttr = {0};
	BTSDKHANDLE hEnum = Btsdk_StartEnumLocalServer();
	if (hEnum != BTSDK_INVALID_HANDLE)
	{
		hSvc = Btsdk_EnumLocalServer(hEnum, &SvrAttr);
		while ((hSvc = Btsdk_EnumLocalServer(hEnum, &SvrAttr)) != BTSDK_INVALID_HANDLE)
		{
			if (SvrAttr.service_class == BTSDK_CLS_OBEX_OBJ_PUSH)
				break;
		}
		Btsdk_EndEnumLocalServer(hEnum);
	}
	if (hSvc == BTSDK_INVALID_HANDLE)
		hSvc = Btsdk_RegisterOPPService((BTUINT8 *)"d:\\opp\\inbox_svr", (BTUINT8 *)"d:\\opp\\outbox_svr", (BTUINT8 *)"card.vcf");
	if (hSvc != BTSDK_INVALID_HANDLE)
		Btsdk_OPPRegisterDealReceiveFileCB(AppOBEXServerReceiveFileInd);
}

/*******************************************************************
*																	*
********************************************************************/
void RemoveServer(void)
{
	BTSVCHDL hdl;
	BTINT32 result;

	hdl = SelectLocService();
	if (hdl == BTSDK_INVALID_HANDLE)
		return;
	result = Btsdk_StopServer(hdl);
	if (result == BTSDK_OK)
	{
		result = Btsdk_RemoveServer(hdl);
	}
	if (result == BTSDK_OK)
	{
		PRINTMSG(1, "%0x Server removed  \r\n", hdl);
		g_loc_svc_num = 32;
		Btsdk_GetLocalServiceList(g_loc_svc_hdls, (BTUINT32 *)&g_loc_svc_num);
	}
	else
		PRINTMSG(1, "Server removed fail\r\n");
}

/*******************************************************************
*																	*
********************************************************************/
void SvcRegShowMenu(void)
{	
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, "*    Demo-Service Register Menu    *\n");
	PRINTMSG(1, "0) Register SPP (COM6) service\n");
	PRINTMSG(1, "1) Register SPP (COM7) service\n");
	PRINTMSG(1, "2) Register FTP service\n");
	PRINTMSG(1, "3) Register OPP service\n");
	PRINTMSG(1, "4) Display local service\n");
	PRINTMSG(1, "5) Unregister service\n");
	PRINTMSG(1, "m) Show this menu\n");
	PRINTMSG(1, "q) Return to main menu\n");
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, ">");
    PRINTMSG(1, "Select the function you want to be demonstrated.\n");
	PRINTMSG(1, "Function number = ");	
}

/*******************************************************************
*																	*
********************************************************************/
void SvcRegExecCmd(BTUINT8 choice)
{
	switch (choice) {
	case '0':
		RegisterSPPCom6Service();
		break;
	case '1':
		RegisterSPPCom7Service();
		break;
	case '2':
		RegisterFTPService();
		break;
	case '3':
		RegisterOPPService();
		break;
	case '4':
		DisplayLocalServices();
		break;
	case '5':
		RemoveServer();
		break;	
	case 'm':
		SvcRegShowMenu();
		break;
	case 'q':
		InterlockedDecrement(&g_NumberLevel); 
		break;
	}
}

/*******************************************************************
*																	*
********************************************************************/
void TestSvcRegMgr(void)
{
	BTUINT8 ch = 0;

	SvcRegShowMenu();

	while (ch != 'q')
	{
		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TestSvcRegMgr: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd, g_NumberLevel);
		
		if (ch == '\n')
		{
			PRINTMSG(1, ">");
		}
		else
		{
			SvcRegExecCmd(ch);
		}
	}
}

/*******************************************************************
*																	*
********************************************************************/
void RemoveAllServer(void)
{
	//BTSVCHDL hdl;
	//BTINT32 result;
	//BTSDKHANDLE henum;

	//henum = Btsdk_StartEnumLocalServer();
	//if (henum != BTSDK_INVALID_HANDLE)
	//{
	//	while ((hdl = Btsdk_EnumLocalServer(henum, NULL)) != BTSDK_INVALID_HANDLE)
	//	{
	//		result = Btsdk_StopServer(hdl);
	//		if (result == BTSDK_OK)
	//		{
	//			result = Btsdk_RemoveServer(hdl);
	//		}
	//	}
	//	Btsdk_EndEnumLocalServer(henum);
	//}
}
