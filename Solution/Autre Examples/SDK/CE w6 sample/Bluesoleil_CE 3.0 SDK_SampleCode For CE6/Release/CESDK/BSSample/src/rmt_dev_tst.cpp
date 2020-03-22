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



BTDEVHDL g_rmt_dev_hdls[MAX_DEV_NUM];
BTINT32 g_rmt_dev_num = 0;
static BTDEVHDL g_curr_dev;
BOOL    m_SearcingThread = FALSE;


int MultibyteToMultibyte(BTUINT32 dwSrcCodePage, char *lpSrcStr, int cbSrcStr,
						 BTUINT32 dwDestCodePage, char *lpDestStr, int cbDestStr)
{
	short *wzTemp;
	int srcLen;

	if (dwSrcCodePage == dwDestCodePage)
	{
		return 0;
	}

	if (lpSrcStr == NULL)
	{
		return 0;
	}

	if (cbSrcStr == -1)
	{
		srcLen = strlen(lpSrcStr) + 1;
	}
	else
	{
		srcLen = cbSrcStr;
	}

	if (lpDestStr == NULL || cbDestStr == 0)
	{
		return srcLen;
	}

	wzTemp = (short*)malloc(srcLen * sizeof(short));
	memset(wzTemp, 0, srcLen * sizeof(short));
	MultiByteToWideChar(dwSrcCodePage, 0, lpSrcStr, cbSrcStr, (LPWSTR)wzTemp, srcLen);
	srcLen = WideCharToMultiByte(dwDestCodePage, 0, (LPWSTR)wzTemp, -1, lpDestStr, cbDestStr, NULL, FALSE);

	free(wzTemp);
	return srcLen;
}


void PrintBdAddr(BTUINT8 *bd_addr)
{
	int j;
	for(j = 5; j > 0; j--)
		PRINTMSG(1, "%02X:", bd_addr[j]);
	PRINTMSG(1, "%02X", bd_addr[0]);
}
		
void AppInquiryInd(BTDEVHDL dev_hdl)
{
#if 0 // Enable to test remote interaction during inquiry. At least one connection shall be existed.
	BtSdkRemoteDevicePropertyStru rmt_dev_prop;
	Btsdk_GetRemoteDeviceProperty(dev_hdl, &rmt_dev_prop);
#endif
	g_rmt_dev_hdls[g_rmt_dev_num++] = dev_hdl;
}

void AppInqCompInd(void)
{
	HANDLE hEvent;

	hEvent = CreateEvent(NULL, FALSE, FALSE, TEXT("WaitInqCompEvent"));
	SetEvent(hEvent);
	CloseHandle(hEvent);
}

void AppPinReqInd(BTDEVHDL dev_hdl)
{
	BTUINT8 pin_code[50] = {0};
	BTUINT16 size;
	PRINTMSG(1, "PIN Code Ind\r\n");
	PRINTMSG(1, "Input PIN Code = %s\r\n", FIXPINCODEVAL);
	strcpy((char *)pin_code, (char *)FIXPINCODEVAL);
	size = strlen((char *)pin_code);
	Btsdk_SetRemoteDevicePinCode(dev_hdl, pin_code, size);
}

BTUINT8  AppConnReqInd(BTDEVHDL dev_hdl, BTUINT32 dev_class)
{
	BTUINT8 bd_addr[BTSDK_BDADDR_LEN];
	BTUINT8 dev_name[BTSDK_DEVNAME_LEN];
	BTUINT16 j;
	Btsdk_GetRemoteDeviceBDAddr(dev_hdl, bd_addr);
	PRINTMSG(1, "\r\nConnection Request Indication (");
	for(j = 5; j > 0; j--)
		PRINTMSG(1, "%02X:", bd_addr[j]);
	PRINTMSG(1, "%02X)", bd_addr[0]);
	j = BTSDK_DEVNAME_LEN;
	if (Btsdk_GetRemoteDeviceName(dev_hdl, dev_name, &j) == BTSDK_OK)
	{
		PRINTMSG(1, "from %s", dev_name);
	}
	PRINTMSG(1, "\r\n");
	return BTSDK_CONNREQ_ACCEPT;
}

void AppAuthorReqInd(BTSVCHDL svc_hdl, BTDEVHDL dev_hdl)
{
	Btsdk_AuthorizationResponse(svc_hdl, dev_hdl, BTSDK_AUTHORIZATION_GRANT);
}

extern BTDEVHDL g_curr_gw_hdl;

void AppConnCompleteInd(BTDEVHDL dev_hdl)
{
	PRINTMSG(1, "SDK Sample:: Connection Complete Indication\r\n");
}

void AppConnEventInd(BTCONNHDL conn_hdl, BTUINT16 event, BTUINT8 *arg)
{
	PBtSdkConnectionPropertyStru prop = (PBtSdkConnectionPropertyStru)arg;
	BTUINT8 bd_addr[6] = {0};
	BTUINT8 dev_name[120] = {0};

	if (prop != NULL)
	{
		Btsdk_GetRemoteDeviceAddress(prop->device_handle, bd_addr);
		Btsdk_GetRemoteDeviceName(prop->device_handle, dev_name, NULL);
	}
	switch (event)
	{
	case BTSDK_APP_EV_CONN_IND:
		PRINTMSG(1, "Receive BTSDK_APP_EV_CONN_IND from %s\r\n", dev_name);
		break;
	case BTSDK_APP_EV_CONN_CFM:
		PRINTMSG(1, "Receive BTSDK_APP_EV_CONN_CFM to %s\r\n", dev_name);
		break;
	case BTSDK_APP_EV_DISC_IND:
		PRINTMSG(1, "Receive BTSDK_APP_EV_DISC_IND from %s\r\n", dev_name);
		break;
	case BTSDK_APP_EV_DISC_CFM:
		PRINTMSG(1, "Receive BTSDK_APP_EV_DISC_CFM to %s\r\n", dev_name);
		break;
	default:
		PRINTMSG(1, "Receive unknown message %04x\r\n", event);
		break;
	}
}

void DecodeHexString(char *inp, int in_len, UCHAR *outp, int out_len)
{
	int i, j;
	UCHAR lb, hb;

	for (i = 0, j = 0; i < in_len && j < out_len; i += 3, j++) {
		lb = inp[i + 1];
		hb = inp[i];
		if (hb >= '0' && hb <= '9')
			outp[j] = hb - '0';
		else if (hb >= 'A' && hb < 'G')
			outp[j] = hb - 'A' + 10;
		else if (hb >= 'a' && hb < 'g')
			outp[j] = hb - 'a' + 10;
		else
			outp[j] = 0;
		
		outp[j] <<= 4;
		if (lb >= '0' && lb <= '9')
			outp[j] += lb - '0';
		else if (lb >= 'A' && lb < 'G')
			outp[j] += lb - 'A' + 10;
		else if (lb >= 'a' && lb < 'g')
			outp[j] += lb - 'a' + 10;
	}
}

void AppPasskeyNotifInd(BTDEVHDL dev_hdl, BTUINT32 num_value)
{
	PRINTMSG(1, "Type in these digits on the remote device: %06lu\r\n", num_value);
}

void AppSPCompleteInd(BTDEVHDL dev_hdl, BTUINT8 result)
{
	if (!result)
		PRINTMSG(1, "Simple pairing succeed.\n");
	else
		PRINTMSG(1, "Simple pairing fail for reason %02X\n", result);
}
/* endif: BT2.1 supported by the local device. */

void RegAppIndCallback(void)
{
	BtSdkCallBackStru cb;

	cb.type = BTSDK_INQUIRY_RESULT_IND;
	cb.func = (void*)AppInquiryInd;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_INQUIRY_COMPLETE_IND;
	cb.func = (void*)AppInqCompInd;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_PIN_CODE_IND;
	cb.func = (void*)AppPinReqInd;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_CONNECTION_REQUEST_IND;
	cb.func = (void*)AppConnReqInd;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_AUTHORIZATION_IND;
	cb.func = AppAuthorReqInd;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_CONNECTION_COMPLETE_IND;
	cb.func = (void*)AppConnCompleteInd;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_CONNECTION_EVENT_IND;
	cb.func = (void*)AppConnEventInd;
	Btsdk_RegisterSdkCallBack(&cb);

/* if: BT2.1 supported by the local device. */
	cb.type = BTSDK_PASSKEY_NOTIF_IND;
	cb.func = (void*)AppPasskeyNotifInd;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_SIMPLE_PAIR_COMPLETE_IND;
	cb.func = (void*)AppSPCompleteInd;
	Btsdk_RegisterSdkCallBack(&cb);
/* endif: BT2.1 supported by the local device. */
}

void UnRegAppIndCallback(void)
{
	BtSdkCallBackStru cb;

	cb.type = BTSDK_INQUIRY_RESULT_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_INQUIRY_COMPLETE_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_PIN_CODE_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_CONNECTION_REQUEST_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_AUTHORIZATION_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_CONNECTION_COMPLETE_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_CONNECTION_EVENT_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);

/* if: BT2.1 supported by the local device. */
	cb.type = BTSDK_IOCAP_REQ_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_USR_CFM_REQ_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_PASSKEY_REQ_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_REM_OOBDATA_REQ_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_PASSKEY_NOTIF_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
	cb.type = BTSDK_SIMPLE_PAIR_COMPLETE_IND;
	cb.func = NULL;
	Btsdk_RegisterSdkCallBack(&cb);
/* endif: BT2.1 supported by the local device. */
}

void DisplayRemoteDevices(BTUINT32 dev_class)
{
	int i, j;
	BTUINT8 DevName[32];
	BTUINT16 len;
	BTUINT32 cls;
	BTUINT8 BdAddr[6];
	char quote = ' ';

	PRINTMSG(1, "++++++++++++++++++++++     total  %d  Devices  Found     +++++++++++++++++++++++\r\n", g_rmt_dev_num);
	PRINTMSG(1, "number  device name %21hc device address %4hc device class\r\n", quote, quote);
	for (i = 0; i < g_rmt_dev_num; i++)
	{
		Btsdk_GetRemoteDeviceClass(g_rmt_dev_hdls[i], &cls);
		if (!IS_SAME_TYPE_DEVICE_CLASS(cls, dev_class))
		{
			continue;
		}
		PRINTMSG(1, "  %d%5hc", i + 1, quote);
			
		len = 32;
		memset(DevName, 0x0, 32*sizeof(BTUINT8));
		if (Btsdk_GetRemoteDeviceName(g_rmt_dev_hdls[i], DevName, &len) != BTSDK_OK)
		{
			if (Btsdk_UpdateRemoteDeviceName(g_rmt_dev_hdls[i], DevName, &len) != BTSDK_OK)
				strcpy((char*)DevName, "Unknown");
		}
		PRINTMSG(1, "%-34hs", DevName);

		Btsdk_GetRemoteDeviceBDAddr(g_rmt_dev_hdls[i], BdAddr);
		for(j = 5; j > 0; j --)
			PRINTMSG(1, "%02X:", BdAddr[j]);
		PRINTMSG(1, "%02X%3hc", BdAddr[0], quote);

		Btsdk_GetRemoteDeviceClass(g_rmt_dev_hdls[i], &cls);
		PRINTMSG(1, "0X%08X\r\n", cls);
	}
}

BTDEVHDL SelectRemoteDevice(BTUINT32 dev_class)
{
	int idx = 0;
	char s[10] = {0};

	if (g_rmt_dev_num == 0)
	{
		g_rmt_dev_num = Btsdk_GetStoredDevicesByClass(dev_class, g_rmt_dev_hdls, MAX_DEV_NUM);
	}
	if (g_rmt_dev_num == 0)
	{
		StartSearchDevice(dev_class);
	}
	else
	{
		DisplayRemoteDevices(dev_class);
	}
	PRINTMSG(1, "Select the target device :\n"); 
	PRINTMSG(1, "(Press key 'q' to return to the function list.)\n");
	do
	{
		PRINTMSG(1, "Target device number = ");
		OnSelDlg(g_hWnd, _T("Select Remote Device"), _T("Please enter number"), s, 10);

		PRINTMSG(1, "%s\r\n", s);
		if(strlen(s) == 0)
		{
			PRINTMSG(1, "\nUser abort the operation.\n");
			return BTSDK_INVALID_HANDLE;
		}
		idx = atoi(s);
		if((idx <= 0) || (idx > g_rmt_dev_num)){
			PRINTMSG(1, "%d is not a valid datum, please select again.\n", idx);
			continue;
		}
		else
		{
			PRINTMSG(1, " %d  \r\n ", idx);
			break;
		}
	}while(1);

	return (g_rmt_dev_hdls[idx - 1]);
}

void StartSearchDevice(BTUINT32 device_class)
{
	PRINTMSG(1, "+ StartSearchDevice, device_class(0x%x)\r\n", device_class);
	memset(g_rmt_dev_hdls, 0, sizeof(g_rmt_dev_hdls));
	g_rmt_dev_num = 0;
	PRINTMSG(1, "Please wait while searching for other devices......\r\n");
	if (Btsdk_StartDiscoverDevice(device_class, MAX_DEV_NUM, 8) == BTSDK_OK)
	{
		HANDLE hEvent = CreateEvent(NULL, FALSE, FALSE, TEXT("WaitInqCompEvent"));
		WaitForSingleObject(hEvent, 20000);
		CloseHandle(hEvent);
		DisplayRemoteDevices(device_class);
	}
	else
	{
		PRINTMSG(1, "Fail to initiate device search!\r\n");
	}
	PRINTMSG(1, "- StartSearchDevice\r\n");
}

void Test_StopSearchDevice(void)
{
	BTSDK_CHECK_RETURN_CODE(Btsdk_StopDiscoverDevice());
}

void Test_Btsdk_GetRemoteLMPInfo(BTDEVHDL dev_hdl)
{
	int j = 0;
	BtSdkRemoteLMPInfoStru lmp_info ={0};
	BTUINT32 result = 0;

	result = Btsdk_GetRemoteLMPInfo(dev_hdl, &lmp_info);

	if(result == BTSDK_OK)
	{
		PRINTMSG(1, "LMP feature=");
		for(j = 0; j < 7; j++)
			PRINTMSG(1, "%02X:", lmp_info.lmp_feature[j]);
		PRINTMSG(1, "%02X\n", lmp_info.lmp_feature[j]);
		PRINTMSG(1, "The name of the manufacturer = %04X\n", lmp_info.manuf_name);
		PRINTMSG(1, "The sub version of the LMP firmware = %04X\n", lmp_info.lmp_subversion);
		PRINTMSG(1, "The main version of the LMP firmware = %02X\n", lmp_info.lmp_version);
	}
	else if(result == BTSDK_ER_NO_CONNECTION)
	{
		PRINTMSG(1, "please setup connection with the specify device before get the LMP \n");
	}
}

void Test_Print_Remote_Basic_Info(BTDEVHDL dev_hdl)
{
	int j;
	BtSdkRemoteDevicePropertyStru dev_prop;
	BTUINT32 result;

	result = Btsdk_GetRemoteDeviceProperty(dev_hdl, &dev_prop);

	if(result == BTSDK_OK)
	{
		PRINTMSG(1, "Name = \"%s\"\n", dev_prop.name);
		PRINTMSG(1, "BD Addr: ");
		for(j = 5; j > 0; j--)
			PRINTMSG(1, "%02X:", dev_prop.bd_addr[j]);
		PRINTMSG(1, "%02X\n", dev_prop.bd_addr[0]);
		PRINTMSG(1, "Device Class: %08lX\n", dev_prop.dev_class);
	}
}


void Test_Btsdk_PairDevice(BTDEVHDL dev_hdl)
{
	BTUINT32 result;
	if (dev_hdl == BTSDK_INVALID_HANDLE)
	{
		dev_hdl = SelectRemoteDevice(0);
		if (dev_hdl == BTSDK_INVALID_HANDLE)
			return;
	}
	PRINTMSG(1, "\n\nPairing...");
#if 0 // Enable to test pairing cancellation
	{
		DWORD id;
		BTDEVHDL *pDev = (BTDEVHDL*)malloc(sizeof(BTDEVHDL));
		*pDev = dev_hdl;
		CreateThread(NULL, 0, AppCancelPairDevice, pDev, 0, &id);
	}
#endif
	result = Btsdk_PairDevice(dev_hdl);
	if (result == BTSDK_OK)
	{
		PRINTMSG(1, "It is OK to Pair Device\n\n");
	}
	else
	{
		PRINTMSG(1, "It is Failed to Pair Device\n\n");
	}
}

void Test_Btsdk_UnPairDevice(BTDEVHDL dev_hdl)
{
	BTUINT32 result;
	PRINTMSG(1, "\n\nUnPairing...");
	result = Btsdk_UnPairDevice(dev_hdl);
	if (result == BTSDK_OK)
	{
		PRINTMSG(1, "OK\n\n");
	}
	else
	{
		PRINTMSG(1, "Failed\n\n");
	}
}

void Test_Btsdk_DelRemoteDevice(BTDEVHDL dev_hdl)
{
	BTUINT32 result;
	int i;

	result  = Btsdk_DeleteRemoteDeviceByHandle(dev_hdl);

	if (result == BTSDK_OK)
	{
		for (i = 0; i < g_rmt_dev_num; i++)
		{
			if (g_rmt_dev_hdls[i] == dev_hdl)
			{
				break;
			}
		}
		if (i != g_rmt_dev_num)
		{
			for (; i < g_rmt_dev_num - 1; i++)
			{
				g_rmt_dev_hdls[i] = g_rmt_dev_hdls[i + 1];
			}
			if (dev_hdl == g_curr_dev)
				g_curr_dev = 0;
			g_rmt_dev_num--;
		}
	}
}


void RmtDevShowMenu()
{
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, "*  Remote Device Manager Testing Menu   *\n");
	PRINTMSG(1, "* <1) Search Devices                    *\n");
	PRINTMSG(1, "* <2) Stop Search Devices               *\n");
	PRINTMSG(1, "* <3) Select Devices                    *\n");
	PRINTMSG(1, "* <4) Get Basic Info                    *\n");
	PRINTMSG(1, "* <5) Get LMP Info                      *\n");
	PRINTMSG(1, "* <7) Pair Device                       *\n");
	PRINTMSG(1, "* <8) Unpair Device                     *\n");
	PRINTMSG(1, "* <9) Delete Device                     *\n");
	PRINTMSG(1, "* <m> Show this menu                    *\n");
	PRINTMSG(1, "* <q> Quit                              *\n");
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, ">");
}

void StartSearchDeviceThread(void)
{
	m_SearcingThread = TRUE;
	StartSearchDevice(0);
	m_SearcingThread = FALSE;
}

void RmtDevExecCmd(BTUINT8 choice)
{
	switch (choice) 
	{
		case '1':	
			CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)StartSearchDeviceThread, (LPVOID)NULL, 0, 0);
			break;
		case '2':
			Test_StopSearchDevice();
			break;
		case '3':
			g_curr_dev = SelectRemoteDevice(0);
			PRINTMSG(1, "RmtDevExecCmd: SelectRemoteDevice, g_curr_dev(0x%x)\r\n", g_curr_dev);
			break;
		case '4':
			Test_Print_Remote_Basic_Info(g_curr_dev);
			break;
		case '5':
			Test_Btsdk_GetRemoteLMPInfo(g_curr_dev);
			break;
		case '7':
			Test_Btsdk_PairDevice(g_curr_dev);
			break;
		case '8':
			Test_Btsdk_UnPairDevice(g_curr_dev);
			break;
		case '9':
			Test_Btsdk_DelRemoteDevice(SelectRemoteDevice(0));
			break;
		case 'm':
			RmtDevShowMenu();
			break;
		case 'q':
			InterlockedDecrement(&g_NumberLevel); 
			break;
		default:
			PRINTMSG(1, "Invalid command.\n");
			break;
	}
}

void TestRmtDevMgr(void)
{
	BTUINT8 ch = 0;

	RmtDevShowMenu();
	
	while (ch != 'q')
	{
		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TestRmtDevMgr: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd, g_NumberLevel);
		
		if ((TRUE == m_SearcingThread)&&(ch != '2'))
		{
			PRINTMSG(1, "Searching device, please choose '<2> Stop Search Devices' to stop first.\r\n");
			ch = '0';
			continue;
		}

		RmtDevExecCmd(ch);		
	}
}


