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


static BTSVCHDL g_rmt_svc_hdls[32];
static BTINT32 g_rmt_svc_num;
static BTDEVHDL g_sel_rmt_dev = BTSDK_INVALID_HANDLE;
static BTSVCHDL g_sel_rmt_svc = BTSDK_INVALID_HANDLE;
static BTCONNHDL g_cur_conn_hdl = BTSDK_INVALID_HANDLE;

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

---------------------------------------------------------------------------*/
void DisplayRemoteServices(void)
{
	int i;
	BTUINT8 DevName[32];
	BTUINT16 len;
	BtSdkRemoteServiceAttrStru rmt_svc;
	BTINT32 result;
	char quote = ' ';

	len = 32;
	if (Btsdk_GetRemoteDeviceName(g_sel_rmt_dev, DevName, &len) != BTSDK_OK)
	{
		len = 32;
		if (Btsdk_UpdateRemoteDeviceName(g_sel_rmt_dev, DevName, &len) != BTSDK_OK)
			strcpy((char*)DevName, "Unknown");
	}
	MultibyteToMultibyte(CP_UTF8, (char*)DevName, -1, CP_ACP, (char*)DevName, 32);
	PRINTMSG(1, "+++++++++++ total %d service available on %s +++++++++++\n", g_rmt_svc_num, DevName);

	PRINTMSG(1, "number  service name %21hc service class\n", quote);
	for (i = 0; i < g_rmt_svc_num; i++)
	{
		PRINTMSG(1, "  %d%5hc", i + 1, quote);

		rmt_svc.mask = BTSDK_RSAM_SERVICENAME|BTSDK_RSAM_EXTATTRIBUTES;
		result = Btsdk_GetRemoteServiceAttributes(g_rmt_svc_hdls[i], &rmt_svc);
		if (result != BTSDK_OK)
		{
			PRINTMSG(1, "%-34hs %s\r\n", "Unknown", "Unkonwn");
		}
		else
		{
			MultibyteToMultibyte(CP_UTF8, (char*)rmt_svc.svc_name, -1, CP_ACP, (char*)DevName, 32);
			PRINTMSG(1, "%-34hs", DevName);
			PRINTMSG(1, " %04x\r\n", rmt_svc.svc_class);
			Btsdk_FreeMemory(rmt_svc.ext_attributes);
		}
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

---------------------------------------------------------------------------*/
BTDEVHDL SelectRmtService(BTDEVHDL dev_hdl)
{
	int idx = 0;
	char s[10] = {0};
    int nRet = 0;
	if (g_rmt_svc_num == 0)
	{
		g_rmt_svc_num = 32;
		nRet = Btsdk_BrowseRemoteServices(dev_hdl, g_rmt_svc_hdls, (BTUINT32 *)&g_rmt_svc_num);
		PRINTMSG(1, "Return Value is 0x%x\r\n", nRet);
		PRINTMSG(1, "Service number is %d\r\n", g_rmt_svc_num);
		if (nRet != BTSDK_OK || g_rmt_svc_num == 0)
			return BTSDK_INVALID_HANDLE;
	}

	DisplayRemoteServices();
	PRINTMSG(1, "Select the target service :\n"); 
	PRINTMSG(1, "(Press key 'q' to return to the function list.)\n");
	do
	{
		PRINTMSG(1, "Target service number = ");
		
		OnSelDlg(g_hWnd, _T("Enter the service number"), _T("Please Enter the service number "), s, 10);		

		if((s[0] == 'q') || (s[0] == 'Q'))
		{
			PRINTMSG(1, "\nUser abort the operation.\n");
			return BTSDK_INVALID_HANDLE;
		}
		idx = atoi(s);
		if((idx <= 0) || (idx > g_rmt_svc_num)){
			PRINTMSG(1, "%d is not a valid datum, please select again.\n", idx);
			continue;
		}
		else
		{
			break;
		}
	}while(1);

	return (g_rmt_svc_hdls[idx - 1]);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

---------------------------------------------------------------------------*/
BTCONNHDL SelectConnection(BTUINT16 svc_class)
{
	BTSDKHANDLE hEnum;
	BTCONNHDL hConn[20];
	BtSdkConnectionPropertyStru conn_prop;
	BtSdkLocalServerAttrStru loc_svc;
	BtSdkRemoteServiceAttrStru rmt_svc;
	BTUINT8 dev_name[27];
	BTUINT16 len;
	int i = 0;

	hEnum = Btsdk_StartEnumConnection();
	if (hEnum == BTSDK_INVALID_HANDLE)
		return BTSDK_INVALID_HANDLE;
	
	memset(hConn, 0, sizeof(hConn));
	
	PRINTMSG(1, "+++++++++ Connection List +++++++++\n");
	PRINTMSG(1, "number  device name %14hc service name %19hc direction\n", ' ', ' ');
	loc_svc.mask = BTSDK_LSAM_SERVICENAME;
	rmt_svc.mask = BTSDK_RSAM_SERVICENAME;
	while ((hConn[i] = Btsdk_EnumConnection(hEnum, &conn_prop)) != BTSDK_INVALID_HANDLE)
	{
		if (svc_class != 0 && conn_prop.service_class != svc_class)
			continue;
		i++;
		PRINTMSG(1, "  %d%5hc", i, ' ');
		len = 27;
		Btsdk_GetRemoteDeviceName(conn_prop.device_handle, dev_name, &len);
		PRINTMSG(1, "%-27hs", dev_name);
		if (conn_prop.role == BTSDK_CONNROLE_INITIATOR)
		{
			Btsdk_GetRemoteServiceAttributes(conn_prop.service_handle, &rmt_svc);
			rmt_svc.svc_name[31] = 0;
			MultibyteToMultibyte(CP_UTF8, (char*)rmt_svc.svc_name, -1, CP_ACP, (char *)rmt_svc.svc_name, 24);
			PRINTMSG(1, "%-31hs %8hs\n", rmt_svc.svc_name, "Out");
		}
		else
		{
			Btsdk_GetServerAttributes(conn_prop.service_handle, &loc_svc);
			loc_svc.svc_name[31] = 0;
			MultibyteToMultibyte(CP_UTF8, (char*)loc_svc.svc_name, -1, CP_ACP, (char *)loc_svc.svc_name, 24);
			PRINTMSG(1, "%-31hs %8hs\n", loc_svc.svc_name, "in");
		}
	}
	Btsdk_EndEnumConnection(hEnum);

	if (i == 0)
	{
		return BTSDK_INVALID_HANDLE;
	}

	PRINTMSG(1, "Select the target service :\n"); 
	PRINTMSG(1, "(Press key 'q' to return to the function list.)\n");
	do
	{
		int number = i;
		char s[10] = {0};
		PRINTMSG(1, "Target device number = ");
		
		OnSelDlg(g_hWnd, _T("Enter the device number"), _T("Please Enter the device number "), s, 10);

		if((s[0] == 'q') || (s[0] == 'Q'))
		{
			PRINTMSG(1, "\nUser abort the operation.\n");
			return BTSDK_INVALID_HANDLE;
		}
		i = atoi(s);
		if ((i <= 0) || (i > number)) 
		{
			PRINTMSG(1, "%d is not a valid datum, please select again.\n", i);
			continue;
		}
		else
		{
			PRINTMSG(1, " %d  \r\n ", i);
			break;
		}
	}while(1);
	return hConn[i - 1];
}

/*******************************************************************
*																	*
********************************************************************/
void BrowseService(void)
{
	BTINT32 result;

	g_sel_rmt_dev = SelectRemoteDevice(0);
	if (g_sel_rmt_dev == BTSDK_INVALID_HANDLE)
		return;
	g_rmt_svc_num = 32;
	result = Btsdk_BrowseRemoteServices(g_sel_rmt_dev, g_rmt_svc_hdls, (BTUINT32 *)&g_rmt_svc_num);
	if (result == BTSDK_OK)
		DisplayRemoteServices();
	else
		PRINTMSG(1, "Btsdk_BrowseRemoteServices fail\r\n");
}

/*******************************************************************
*																	*
********************************************************************/
void Connect2Service(void)
{
	BTINT32 result;

	if (g_sel_rmt_dev == BTSDK_INVALID_HANDLE)
	{
		PRINTMSG(1, "No device selected\r\n");
		return;
	}
	g_sel_rmt_svc = SelectRmtService(g_sel_rmt_dev);
	if (g_sel_rmt_svc == BTSDK_INVALID_HANDLE)
		return;
	result = Btsdk_StartClient(g_sel_rmt_svc, 0, &g_cur_conn_hdl);
	if (result == BTSDK_OK)
	{
		PRINTMSG(1, "Connect to service successfully\n");
	}		
	else
		PRINTMSG(1, "Connect to service fail\n");

}

/*******************************************************************
*																	*
********************************************************************/
void Connect2SPPService(void)
{
	BTINT32 result;
	BtSdkConnectionPropertyStru conn_prop;

	g_sel_rmt_dev = SelectRemoteDevice(0);
	if (g_sel_rmt_dev == BTSDK_INVALID_HANDLE)
	{
		PRINTMSG(1, "No device selected\r\n");
		return;
	}
	result = Btsdk_StartClientEx(g_sel_rmt_dev, BTSDK_CLS_SERIAL_PORT, 0, &g_cur_conn_hdl);
	Btsdk_GetConnectionProperty(g_cur_conn_hdl, &conn_prop);
	if (result == BTSDK_OK)
		PRINTMSG(1, "Connect to SPP service successfully\n");
	else
		PRINTMSG(1, "Connect to SPP service fail\n");
}

/*******************************************************************
*																	*
********************************************************************/
void ReleaseConnection(void)
{
	BTCONNHDL hConn = SelectConnection(0);
	BTINT32 result;

	if (hConn == BTSDK_INVALID_HANDLE)
	{
		PRINTMSG(1, "No connection selected\r\n");
		return;
	}
	result = Btsdk_Disconnect(hConn);
	if (result == BTSDK_OK)
		PRINTMSG(1, "Release connection successfully\n");
	else
		PRINTMSG(1, "Release connection fail\n");
}

/*******************************************************************
*																	*
********************************************************************/
void UpdateServiceAttribute(void)
{
	BTINT32 result;
	BTSVCHDL svc_hdl;
	if (g_sel_rmt_dev == BTSDK_INVALID_HANDLE)
	{
		g_sel_rmt_dev = SelectRemoteDevice(0);
		if (g_sel_rmt_dev == BTSDK_INVALID_HANDLE)
		{
			PRINTMSG(1, "No device selected\r\n");
			return;
		}
	}

	svc_hdl = SelectRmtService(g_sel_rmt_dev);
	if (svc_hdl == BTSDK_INVALID_HANDLE)
	{
		PRINTMSG(1, "Please browse service befor. \n");
		return;
	}

	result = Btsdk_RefreshRemoteServiceAttributes(svc_hdl, NULL);
	if (result == BTSDK_OK)
		PRINTMSG(1, "Refresh service attributes successfully\n");
	else
		PRINTMSG(1, "Refresh service attributes fail\n");
}

void SvcSchShowMenu()
{
	PRINTMSG(1, "******************************************\n");
	PRINTMSG(1, "*     Service Search Testing Menu        *\n");
	PRINTMSG(1, "* <1> Browse Service                     *\n");
	PRINTMSG(1, "* <2> Connect Remote Service             *\n");
	PRINTMSG(1, "* <3> Connect Remote SPP Service         *\n");
	PRINTMSG(1, "* <4> Release connection                 *\n");
	PRINTMSG(1, "* <6> Update Service Attributes          *\n");
	PRINTMSG(1, "* <m> Show this menu                     *\n");
	PRINTMSG(1, "* <q> Quit                               *\n");
	PRINTMSG(1, "******************************************\n");
	PRINTMSG(1, ">");
}


void SvcSchExecCmd(BTUINT8 choice)
{
	switch (choice) 
	{
		case '1':
			BrowseService();
			break;
		case '2':
			Connect2Service();
			break;
		case '3':
			Connect2SPPService();
			break;
		case '4':
			ReleaseConnection();
			break;
		case '6':
			UpdateServiceAttribute();
			break;
		case 'm':
			SvcSchShowMenu();
			break;
		case 'q':
			InterlockedDecrement(&g_NumberLevel); 
			break;
		default:
			PRINTMSG(1, "Invalid command.\n");
			break;
	}
}


void TestSvcSch(void)
{
	BTUINT8 ch = 0;

	SvcSchShowMenu();
	while (ch != 'q')
	{
		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TestSvcSch: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd, g_NumberLevel);
		
		if (ch == '\n')
		{
			PRINTMSG(1, ">");
		}
		else
		{
			SvcSchExecCmd(ch);
		}
	}
}
