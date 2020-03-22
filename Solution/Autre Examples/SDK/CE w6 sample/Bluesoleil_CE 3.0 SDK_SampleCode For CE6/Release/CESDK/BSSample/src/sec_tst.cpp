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

BTUINT32 g_aux_thread_id;
HANDLE g_AuxInputEvent = NULL;
BTUINT8 g_aux_choice;


#define MSG_AUTHORIZATION_REQ	(WM_USER+1)
#define MSG_CONNECTION_IND		(WM_USER+2)

struct DeviceStru
{
	BTDEVHDL hdl;
	BTUINT8 bd_addr[6];
	BTUINT8 name[BTSDK_DEVNAME_LEN];
	struct DeviceStru *next;
};

struct DeviceStru *g_DevHead = NULL;

struct ServiceStru
{
	BTSVCHDL hdl;
	BTUINT8 name[BTSDK_SERVICENAME_MAXLENGTH];
	struct ServiceStru *next;
};
struct ServiceStru *g_SvcHead = NULL;

BTUINT8 ListAllDev()
{
	int i = 0, j;
	struct DeviceStru *dev;

	dev = g_DevHead;

	while (dev != NULL)
	{
		i++;
		PRINTMSG(1, "\t<%d> ", i);
		for (j=0; j<5; j++)
		{
			PRINTMSG(1, "%02x:", dev->bd_addr[j]);
		}
		PRINTMSG(1, "%02x\t", dev->bd_addr[j]);
		PRINTMSG(1, "%s\n", dev->name);
		dev = dev->next;
	}
	return (BTUINT8)i;
}

struct DeviceStru *GetDevByIndex(BTUINT8 index)
{
	BTUINT8 i;
	struct DeviceStru *dev = g_DevHead;
	for (i=0; dev!= NULL, i<(index-1); i++)
	{
		dev = dev->next;
	}
	return dev;
}

void ReleaseAllDev()
{
	struct DeviceStru *dev;

	dev = g_DevHead;
	while (dev != NULL)
	{
		g_DevHead = dev->next;
		Btsdk_FreeMemory(dev);
		dev = g_DevHead;
	}
}

struct DeviceStru *FindDev(BTDEVHDL hdl)
{
	struct DeviceStru *dev;
	
	while (dev = g_DevHead, dev != NULL)
	{
		if (dev->hdl == hdl)
		{
			break;
		}
		dev = dev->next;
	}	
	return dev;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This routine inserts DeviceStru into g_DevHead.
	NOTE: it is inserted at the header of g_DevHead.
Arguments:
	dev    pointer to struct DeviceStru.
Return Value:
	NONE.
---------------------------------------------------------------------------*/
void InsertDev(struct DeviceStru *dev)
{	
	if (dev == NULL)
	{
		return;
	}

	if (FindDev(dev->hdl) != NULL)
	{
		Btsdk_FreeMemory(dev);
		return;
	}

	if (g_DevHead == NULL)
	{
		g_DevHead = dev;
		dev->next = NULL;
	}
	else
	{
		dev->next = g_DevHead;
		g_DevHead = dev;
	}
}

BTUINT8 ListAllSvc()
{
	int i = 0;
	struct ServiceStru *svc;

	svc = g_SvcHead;

	while (svc != NULL)
	{
		i++;
		PRINTMSG(1, "\t<%d> ", i);
		PRINTMSG(1, "%s\n", svc->name);
		svc = svc->next;
	}
	return i;
}

void ReleaseAllSvc()
{
	struct ServiceStru *svc;

	svc = g_SvcHead;
	while (svc != NULL)
	{
		g_SvcHead = svc->next;
		Btsdk_FreeMemory(svc);
		svc = g_SvcHead;
	}
}

struct ServiceStru *GetSvcByIndex(BTUINT8 index)
{
	BTUINT8 i;
	struct ServiceStru *svc = g_SvcHead;
	for (i=0; svc!= NULL, i<(index-1); i++)
	{
		svc = svc->next;
	}
	return svc;
}

struct ServiceStru *FindSvc(BTSVCHDL hdl)
{
	struct ServiceStru *svc;
	
	while (svc = g_SvcHead, svc != NULL)
	{
		if (svc->hdl == hdl)
		{
			break;
		}
		svc = svc->next;
	}	
	return svc;
}

void InsertSvc(struct ServiceStru *svc)
{
	if (svc == NULL)
	{
		return;
	}

	if (g_SvcHead == NULL)
	{
		g_SvcHead = svc;
		svc->next = NULL;
	}
	else
	{
		svc->next = g_SvcHead;
		g_SvcHead = svc;
	}
}


DWORD WINAPI AuxThread(LPVOID lpParameter)
{
	MSG msg;
	while (1) {
		GetMessage(&msg, NULL, 0, 0);
		switch (msg.message) 
		{
		case MSG_AUTHORIZATION_REQ:
			{
				struct ServiceStru *svc;
				struct DeviceStru *dev;
				PRINTMSG(1, "AuxThread: message(MSG_AUTHORIZATION_REQ), svchdl|wParam(0x%x), devhdl|lParam(0x%x)\r\n", msg.wParam, msg.lParam);
				if ((svc = FindSvc(msg.wParam)) == NULL)
				{
					break;
				}
				if ((dev = FindDev(msg.lParam)) == NULL)
				{
					break;
				}
				PRINTMSG(1, "\t\"%s\" requests to access the %s service\n", dev->name, svc->name);
				PRINTMSG(1, "\t<1> Accept the access\n");
				PRINTMSG(1, "\t<2> Deny the access\n");
				PRINTMSG(1, "\tMake decision:");
				g_AuxInputEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
				WaitForSingleObject(g_AuxInputEvent, INFINITE);
				CloseHandle(g_AuxInputEvent);
				g_AuxInputEvent = NULL;
				
				switch (g_aux_choice)
				{
				case '1':
					Btsdk_AuthorRsp(msg.wParam, msg.lParam, BTSDK_AUTHORIZATION_GRANT);
					break;
				case '2':
				default:
					Btsdk_AuthorRsp(msg.wParam, msg.lParam, BTSDK_AUTHORIZATION_DENY);
					break;
				}
			}			
			break;
		case MSG_CONNECTION_IND:
			{
				BTUINT16 len;
				struct DeviceStru *dev = (DeviceStru *)Btsdk_MallocMemory(sizeof(struct DeviceStru));

				PRINTMSG(1, "AuxThread: message(MSG_CONNECTION_IND), svchdl|wParam(0x%x), devhdl|lParam(0x%x)\r\n", msg.wParam, msg.lParam);
				dev->hdl = msg.wParam;
				Btsdk_GetRemoteDeviceBDAddr(dev->hdl, dev->bd_addr);
				if (Btsdk_GetRemoteDeviceName(dev->hdl, dev->name, &len) != BTSDK_OK)
				{
					Btsdk_UpdateRemoteDeviceName(dev->hdl, dev->name, &len);
				}
				PRINTMSG(1, "AuxThread: message(MSG_CONNECTION_IND), dev hdl(0x%x), name(%s), bd_addr[%02x:%02x:%02x:%02x:%02x:%02x]\r\n", \
					dev->hdl, dev->name, dev->bd_addr[0], dev->bd_addr[1], dev->bd_addr[2], dev->bd_addr[3], dev->bd_addr[4], dev->bd_addr[5]);
				InsertDev(dev);
			}
			break;
		default:
			break;
		}
	}
	return 0;
}

void AuthorGrantAccess(BTSVCHDL svc_hdl, BTDEVHDL dev_hdl)
{
	Btsdk_AuthorizationResponse(svc_hdl, dev_hdl, BTSDK_AUTHORIZATION_GRANT);
}

BTUINT8 ConnectionReqInd(BTDEVHDL dev_hdl)
{
	return BTSDK_CONNREQ_DEFAULT;
}

void RegIndCallback(void)
{
	BtSdkCallBackStru cb;

	cb.type = BTSDK_AUTHORIZATION_IND;
	cb.func = AuthorGrantAccess;
	Btsdk_RegisterSdkCallBack(&cb);

	cb.type = BTSDK_CONNECTION_REQUEST_IND;
	cb.func = ConnectionReqInd;
	Btsdk_RegisterSdkCallBack(&cb);
}

void SecInit()
{
	CreateThread(NULL, 0, AuxThread, NULL, 0, &g_aux_thread_id);
//	Btsdk_SetFixedPinCode(FIXPINCODEVAL, (BTUINT16)strlen(FIXPINCODEVAL));
	Btsdk_SetSecurityMode(BTSDK_SECURITY_MEDIUM);
	RegIndCallback();
}

void SecDone()
{
	ReleaseAllSvc();
	ReleaseAllDev();
}

void SetSecMode()
{
	BTUINT8 choice;
	
	PRINTMSG(1, "\t<1>. Low\n");
	PRINTMSG(1, "\t<2>. Medium\n");
	PRINTMSG(1, "\t<3>. High\n");
	PRINTMSG(1, "\t<4>. Security mode 4\n");	
	choice = OnSelDlg(g_hWnd, _T("Set Security Mode"), _T("Please enter number according to the prompt"), NULL, 0);
	PRINTMSG(1, "\tSelect Security Mode is: ");

	switch(choice)
	{
	case 1:
		Btsdk_SetSecurityMode(BTSDK_SECURITY_LOW);
		PRINTMSG(1, "\tLow");
		break;
	case 2:
		Btsdk_SetSecurityMode(BTSDK_SECURITY_MEDIUM);
		PRINTMSG(1, "\tMedium");
		break;
	case 3:
		Btsdk_SetSecurityMode(BTSDK_SECURITY_HIGH);
		PRINTMSG(1, "\tHigh");
		break;
	case 4:
		Btsdk_SetSecurityMode(BTSDK_SECURITY_ENCRYPT_MODE1);
		PRINTMSG(1, "\tSecurity mode 4");
		break;
	}

	PRINTMSG(1, "\n");
}

void GetSecMode()
{
	BTUINT16 mode = 0;

	Btsdk_GetSecurityMode(&mode);
	PRINTMSG(1, "\tSecurity mode is:");
	switch(mode)
	{
	case BTSDK_SECURITY_LOW:
		PRINTMSG(1, "\tLow");
		break;
	case BTSDK_SECURITY_MEDIUM:
		PRINTMSG(1, "\tMedium");
		break;
	case BTSDK_SECURITY_HIGH:
		PRINTMSG(1, "\tHigh");
		break;
	case BTSDK_SECURITY_ENCRYPT_MODE1:
		PRINTMSG(1, "\tSecurity mode 4");
		break;
	}
	PRINTMSG(1, "\n");
}

void StartSppService(void)
{
	BTSVCHDL hSvc;
	struct ServiceStru *svc;
	BtSdkLocalServerAttrStru attr = {0};

	attr.mask = BTSDK_LSAM_SERVICENAME|BTSDK_LSAM_SECURITYLEVEL|BTSDK_LSAM_AUTHORMETHOD;
	attr.service_class = BTSDK_CLS_SERIAL_PORT;
	attr.security_level = BTSDK_SSL_NO_SECURITY;
	attr.author_method = BTSDK_AUTHORIZATION_PROMPT;
	strcpy((char *)attr.svc_name, (char *)"Serial Port (COM7)");
	hSvc = Btsdk_AddServer(&attr);
	Btsdk_SetServiceSecurityLevel(hSvc, BTSDK_DEFAULT_SECURITY);
	Btsdk_SetAuthorizationMethod(hSvc, BTSDK_AUTHORIZATION_PROMPT);
	Btsdk_StartServer(hSvc);
	svc = (ServiceStru *)Btsdk_MallocMemory(sizeof(struct ServiceStru));
	svc->hdl = hSvc;
	strcpy((char *)svc->name, (char *)"SPP");
	InsertSvc(svc);
}

void StartSvc()
{
	BTUINT8 choice = 0;

	PRINTMSG(1, "\t<1> SPP service\n");

	choice = OnSelDlg(g_hWnd, _T("Set Security Mode"), _T("Please enter number according to the prompt"), NULL, 0);
	PRINTMSG(1, "\tSelect service: %d\r\n", choice);

	switch (choice)
	{
	case 1:
		StartSppService();
		PRINTMSG(1, "\tOK\n");
		break;	
	default:
		PRINTMSG(1, "\tInvalid choice\n");
		break;
	}
} 

void SetSvcSecuLev()
{
	BTUINT8 choice;
	struct ServiceStru *svc;

	if (ListAllSvc() == 0)
	{
		PRINTMSG(1, "\tNo service is started\n");
		return;
	}

	choice = OnSelDlg(g_hWnd, _T("Select the service"), _T("type number"), NULL, 0);
	PRINTMSG(1, "\tSelect the service: %d\r\n", choice);
	svc = GetSvcByIndex(choice);
	if (svc != NULL)
	{
		PRINTMSG(1, "\t<1> None\n");
		PRINTMSG(1, "\t<2> Authentication\n");
		PRINTMSG(1, "\t<3> Authentication and Authorization\n");
		PRINTMSG(1, "\t<4> Authentication and Encryption\n");
		PRINTMSG(1, "\t<5> Authentication, Encryption and Authorization\n");	

		choice = OnSelDlg(g_hWnd, _T("Security level"), _T("Select a security level"), NULL, 0);		
		PRINTMSG(1, "\tSelect a security level: %d\r\n", choice);
		switch (choice)
		{
		case 1:
			Btsdk_SetServiceSecurityLevel(svc->hdl, 0);
			PRINTMSG(1, "\tOK\n");
			break;
		case 2:
			Btsdk_SetServiceSecurityLevel(svc->hdl, 
				BTSDK_SSL_AUTHENTICATION);
			PRINTMSG(1, "\tOK\n");
			break;
		case 3:
			Btsdk_SetServiceSecurityLevel(svc->hdl, 
				BTSDK_SSL_AUTHENTICATION | BTSDK_SSL_AUTHORIZATION);
			PRINTMSG(1, "\tOK\n");
			break;
		case 4:
			Btsdk_SetServiceSecurityLevel(svc->hdl, 
				BTSDK_SSL_AUTHENTICATION | BTSDK_SSL_ENCRYPTION);
			PRINTMSG(1, "\tOK\n");
			break;
		case 5:
			Btsdk_SetServiceSecurityLevel(svc->hdl, 
				BTSDK_SSL_AUTHENTICATION | BTSDK_SSL_ENCRYPTION | BTSDK_SSL_AUTHORIZATION);
			PRINTMSG(1, "\tOK\n");
			break;
		default:
			PRINTMSG(1, "Invalid choice\n");
			break;
		}
	}
	else
	{
		PRINTMSG(1, "\tNo such service\n");
	}
}

void SetAuthorMethod()
{
	BTUINT8 choice;
	struct ServiceStru *svc;

	if (ListAllSvc() == 0)
	{
		PRINTMSG(1, "\tNo service is started\n");
		return;
	}
	
	choice = OnSelDlg(g_hWnd, _T("Set the service"), _T("Select a service"), NULL, 0);
	PRINTMSG(1, "\tSelect the service: %d\r\n", choice);

	svc = GetSvcByIndex(choice);
	if (svc != NULL)
	{
		PRINTMSG(1, "\t<1> Accept, even the requesting device is untrusted\n");
		PRINTMSG(1, "\t<2> Deny, if the requesting device is untrusted\n");
		PRINTMSG(1, "\t<3> Prompt, if the requesting device is untrusted\n");

		choice = OnSelDlg(g_hWnd, _T("Select Authorization"), _T("Select a authorization method"), NULL, 0);
		PRINTMSG(1, "\tSelect a authorization method: %d\r\n", choice);
		
		switch (choice)
		{
		case 1:
			Btsdk_SetAuthorizationMethod(svc->hdl, BTSDK_AUTHORIZATION_ACCEPT);
			PRINTMSG(1, "\tOK\n");
			break;
		case 2:
			Btsdk_SetAuthorizationMethod(svc->hdl, BTSDK_AUTHORIZATION_REJECT);
			PRINTMSG(1, "\tOK\n");
			break;
		case 3:
			Btsdk_SetAuthorizationMethod(svc->hdl, BTSDK_AUTHORIZATION_PROMPT);
			PRINTMSG(1, "\tOK\n");
			break;
		default:
			PRINTMSG(1, "Invalid choice\n");
			break;
		}
	}
}

void GetSvcSecuLev()
{
	BTUINT8 i = 1;
	BTUINT8 level;
	struct ServiceStru *svc;

	while (svc = GetSvcByIndex(i), svc != NULL)
	{
		i++;
		if (Btsdk_GetServiceSecurityLevel(svc->hdl, &level) != BTSDK_OK)
		{
			continue;
		}
		else
		{
			PRINTMSG(1, "\tService\t\tLevel\n");
			PRINTMSG(1, "\t%s", svc->name);
			if (level & BTSDK_SSL_AUTHENTICATION)
			{
				PRINTMSG(1, "\t\t%s", "AUTHEN ");
			}
			if (level & BTSDK_SSL_AUTHORIZATION)
			{
				PRINTMSG(1, "%s", "AUTHOR ");
			}
			if (level & BTSDK_SSL_ENCRYPTION)
			{
				PRINTMSG(1, "%s", "ENCRY");
			}
			PRINTMSG(1, "\n");
		}
		svc = GetSvcByIndex(i);
	}
}

void SetTrustedDev()
{
	BTUINT8 choice;
	struct ServiceStru *svc;
	BTDEVHDL rmt_dev = SelectRemoteDevice(0);
	if (rmt_dev == BTSDK_INVALID_HANDLE)
	{
		PRINTMSG(1, "No device selected\r\n");
		return;
	}

	if (ListAllSvc() == 0)
	{
		PRINTMSG(1, "\tNo service is started\n");
		return;
	}

	choice = OnSelDlg(g_hWnd, _T("Select Service"), _T("Select a trusted service"), NULL, 0);
	PRINTMSG(1, "\tSelect the service the device is trusted for: %d  \n", choice);
	svc = GetSvcByIndex(choice);
	if (svc != NULL)
	{
		int nRet = Btsdk_SetTrustedDevice(svc->hdl, rmt_dev, BTSDK_TRUSTED);
		if (BTSDK_OK == nRet)
		{
			PRINTMSG(1, "\tSet trusted device success.\n");
		}
		else
		{
			PRINTMSG(1, "\tSet trusted device error %d.\n", nRet);
		}		
	}
}

void SecMgrShowMenu()
{
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, "*    Security Manager T Testing Menu    *\n");
	PRINTMSG(1, "* <1> Set security mode                 *\n");
	PRINTMSG(1, "* <2> Get security mode                 *\n");
	PRINTMSG(1, "* <3> Start service                     *\n");
	PRINTMSG(1, "* <4> Set service security level        *\n");
	PRINTMSG(1, "* <5> Get service security level        *\n");
	PRINTMSG(1, "* <6> Set service authorization method  *\n");
	PRINTMSG(1, "* <7> Set a trusted device              *\n");
	PRINTMSG(1, "* <m> Show this menu                    *\n");
	PRINTMSG(1, "* <q> Quit                              *\n");
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, ">");
}

void SecExecCmd(BTUINT8 choice)
{
	switch (choice)
	{
	case '1':
		SetSecMode();
		break;
	case '2':
		GetSecMode();
		break;
	case '3':
		StartSvc();
		break;
	case '4':
		SetSvcSecuLev();
		break;
	case '5':
		GetSvcSecuLev();
		break;
	case '6':
		SetAuthorMethod();
		break;
	case '7':
		SetTrustedDev();
		break;
	case 'm':
		SecMgrShowMenu();
		break;
	case 'q':
		InterlockedDecrement(&g_NumberLevel); 
		break;
	default:
		PRINTMSG(1, "Invalid command.\n");
		break;
	}
}

void TestSecMgr(void)
{
	BTUINT8 ch = 0;

	SecInit();
	
	SecMgrShowMenu();
	
	while (ch != 'q')
	{		
		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TestSecMgr: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd, g_NumberLevel);

		if (ch == '\n')
		{
			PRINTMSG(1, ">");
		}
		else if (SetEvent(g_AuxInputEvent))
		{
			g_aux_choice = ch;
			continue;
		}
		else
		{
			SecExecCmd(ch);
		}
	}
	SecDone();
}

