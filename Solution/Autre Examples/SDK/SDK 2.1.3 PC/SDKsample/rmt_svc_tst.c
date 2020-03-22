/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2005 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    rmt_svc_tst.c
Abstract:
	Sample codes of remote service manager
Revision History:
2007-6-7   Guan Tengfei  Created

---------------------------------------------------------------------------*/

#include "sdk_tst.h"

/* maximum service handle number */
#define MAX_SVC_HDL_NUM  32
BTSVCHDL g_RmtSvcHdlArray[MAX_SVC_HDL_NUM] = { 0 };
BTUINT32 g_ulRmtSvcNum = MAX_SVC_HDL_NUM;


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to display a remote device's services
Arguments:
	BTDEVHDL hDevHdl
Return:
	void 
---------------------------------------------------------------------------*/
void DisplayRemoteServices(BTDEVHDL hDevHdl)
{
	unsigned int iTemp = 0;
	char cQuote = ' ';
	BTUINT32 ulResult = BTSDK_OK;
	BTUINT8 szDevName[BTSDK_DEVNAME_LEN] = {0};
	BTUINT16 usDevNameLen = BTSDK_DEVNAME_LEN;
	BtSdkRemoteServiceAttrStru struRmtSvc = {0};	
    
	if ((BTSDK_INVALID_HANDLE == hDevHdl) || (0 == g_ulRmtSvcNum))
	{
		return;
	}
	/* display remote device name and service number */
	if (BTSDK_OK != Btsdk_GetRemoteDeviceName(hDevHdl, szDevName, &usDevNameLen))
	{
		if (BTSDK_OK != Btsdk_UpdateRemoteDeviceName(hDevHdl, szDevName, &usDevNameLen))
		{
			strcpy((char*)szDevName, "Unknown");
		}
	}
	MultibyteToMultibyte(CP_UTF8, (char*)szDevName, -1, CP_ACP, szDevName, BTSDK_DEVNAME_LEN);
	printf("If fail to get remote service's name, please assign one according to it's service class.\n");
	printf("+++++++++++ total %d services available on %s +++++++++++\n", g_ulRmtSvcNum, szDevName);

	/* 3.display all services */
	printf("number  service name %21hc service class\n", cQuote);
	for (iTemp = 0; iTemp < g_ulRmtSvcNum; iTemp++)
	{
		printf("  %d%5hc", iTemp + 1, cQuote);
		memset(&struRmtSvc, 0, sizeof(BtSdkRemoteServiceAttrStru));
		struRmtSvc.mask = BTSDK_RSAM_SERVICENAME | BTSDK_RSAM_EXTATTRIBUTES;
		ulResult = Btsdk_GetRemoteServiceAttributes(g_RmtSvcHdlArray[iTemp], &struRmtSvc);
		if (BTSDK_OK != ulResult)
		{
			printf("%-34hs %s\n", "Unknown", "Unkonwn");
		}
		else
		{
			MultibyteToMultibyte(CP_UTF8, (char*)struRmtSvc.svc_name, -1, CP_ACP, szDevName, BTSDK_DEVNAME_LEN);
			printf("%-34hs", szDevName);
			printf(" %04x\n", struRmtSvc.svc_class);
			Btsdk_FreeMemory(struRmtSvc.ext_attributes);
			struRmtSvc.ext_attributes = NULL;
		}
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to browse and display a remote device's services.
Arguments:
	BTDEVHDL hDevHdl
Return:
	void 
---------------------------------------------------------------------------*/
void BrowseAndDisplayRemoteSvc(BTDEVHDL hDevHdl)
{
	if (BTSDK_INVALID_HANDLE == hDevHdl)
	{
		return;
	}
	g_ulRmtSvcNum = MAX_SVC_HDL_NUM;
	Btsdk_BrowseRemoteServices(hDevHdl, g_RmtSvcHdlArray, &g_ulRmtSvcNum);
	DisplayRemoteServices(hDevHdl);	
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select a service on the remote device 
Arguments:
	BTDEVHDL hDevHdl: [in] device handle
Return:
	service handle on the remote device
---------------------------------------------------------------------------*/
BTSVCHDL SelectRemoteService(BTDEVHDL hDevHdl)
{
	BTINT32 ulResult = BTSDK_OK;	
	char szChoice[4] = {0};
	unsigned int nIdx = 0;
	BTUINT32 ulSvcIndex = 0;
	BTSVCHDL hSvcHdl = BTSDK_INVALID_HANDLE;
	
	
	/* get remote services */
	Btsdk_GetRemoteServices(hDevHdl, g_RmtSvcHdlArray, &g_ulRmtSvcNum);
	if (0 == g_ulRmtSvcNum)
	{
		g_ulRmtSvcNum = MAX_SVC_HDL_NUM;
		Btsdk_BrowseRemoteServices(hDevHdl, g_RmtSvcHdlArray, &g_ulRmtSvcNum);		
		if (0 == g_ulRmtSvcNum)
		{
			printf("Fail to get the remote service handle.\n");
			return BTSDK_INVALID_HANDLE;
		}
	}
	DisplayRemoteServices(hDevHdl);
	printf("Please select the target service:\n"); 
	printf("if there is no expected service, please press 'a' to browse again!\n");
	printf("if you want to exit this procedure, please press 'q' to quit.\n");
	
	do
	{
		printf("Target service number = ");
		scanf(" %s", szChoice);
		getchar();
		if ('a' == szChoice[0])
		{
			BrowseAndDisplayRemoteSvc(hDevHdl);		
			continue;
		}
		if(('q' == szChoice[0]) || ('Q' == szChoice[0]))
		{
			printf("\nUser abort the operation.\n");
			return BTSDK_INVALID_HANDLE;
		}
		nIdx = atoi(szChoice);
		if((nIdx <= 0) || (nIdx > g_ulRmtSvcNum))
		{
			printf("%d is not a valid datum, please select again.\n", nIdx);
			continue;
		}
		else
		{
			break;
		}
	} while (1);
	
	return (g_RmtSvcHdlArray[nIdx - 1]);
}







