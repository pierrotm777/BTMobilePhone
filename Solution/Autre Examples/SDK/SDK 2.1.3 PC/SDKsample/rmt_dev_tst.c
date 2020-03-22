/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2005 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    rmt_dev_tst.c
Abstract:
	Sample codes of remote device manager

Revision History:
2007-6-7   Guan Tengfei Created

---------------------------------------------------------------------------*/

#include "sdk_tst.h"

/*remote device handle through device discovery*/
BTDEVHDL s_rmt_dev_hdls[MAX_DEV_NUM] = {0};
/*remote devices number through device discovery*/
BTINT32 s_rmt_dev_num = 0; 
/*remote device's class.*/
BTUINT32 s_rmt_dev_cls = 0;
/*current used remote device handle*/
static BTDEVHDL s_curr_dev = BTSDK_INVALID_HANDLE;
/*event to sync device discovery*/
static HANDLE s_hBrowseDevEventHdl = NULL;
static HANDLE s_waitAuthor = NULL;

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
    This function is to map a string between two different code pages. 
	Help function, all arguments refer to Windows API              
Arguments:
Return:
        0 if two code pages are same or pointer to source string is NULL.
		The number of characters is translated.		 
--------------------------------------------------------------------------*/
int MultibyteToMultibyte(BTUINT32 dwSrcCodePage, char *lpSrcStr, int cbSrcStr,
						 BTUINT32 dwDestCodePage, char *lpDestStr, int cbDestStr)
{
	short *wzTemp = NULL;
	int nSrcLen = 0;

	if (dwSrcCodePage == dwDestCodePage)
	{
		return 0;
	}

	if (lpSrcStr == NULL)
	{
		return 0;
	}

	if (-1 == cbSrcStr)
	{
		nSrcLen = strlen(lpSrcStr) + 1;
	}
	else
	{
		nSrcLen = cbSrcStr;
	}

	if ((NULL == lpDestStr) || (0 == cbDestStr))
	{
		return nSrcLen;
	}

	wzTemp = (short*) malloc(nSrcLen * sizeof(short));
	if (NULL == wzTemp)
	{
		return 0;
	}
	memset(wzTemp, 0, nSrcLen * sizeof(short));
	MultiByteToWideChar(dwSrcCodePage, 0, lpSrcStr, cbSrcStr, wzTemp, nSrcLen);
	nSrcLen = WideCharToMultiByte(dwDestCodePage, 0, wzTemp, -1, lpDestStr, cbDestStr, NULL, FALSE);

	free(wzTemp);
	wzTemp = NULL;
	
	return nSrcLen;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function enable/disable simple pair function.
	If both local and remote device is BT2.1 chip, BlueSoleil will use simple 
	pair function in default. With this function users can change the pair way.
Arguments:
	BOOL bEnable: [in] TRUE: Enable Simple pair
					   FALSE: Disable simple pair
Return:
	void 
---------------------------------------------------------------------------*/
#define IDX_WRITE_SP_MODE_CMD	131
BTINT32 WriteSimplePairMode(BOOL bEnable)
{
	BTINT32 ret = BTSDK_OK;
	UCHAR sp_mode = 0;
	UCHAR status;
	if (bEnable)
	{
		sp_mode = 1;
	}
	ret = Btsdk_ExecuteHCICommandEx(IDX_WRITE_SP_MODE_CMD, sizeof(UCHAR), &sp_mode, sizeof(UCHAR), &status);
	return ret;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function prints bluetooth address
Arguments:
	BTUINT8 *bd_addr: [in] bluetooth address
Return:
	void 
---------------------------------------------------------------------------*/
void PrintBdAddr(BTUINT8 *bd_addr)
{	
	int j;

	if (NULL == bd_addr)
	{
		return;
	}

	for (j = 5; j > 0; j--)
	{
		printf("%02X:", bd_addr[j]);
	}
	printf("%02X", bd_addr[0]);
}
	
	
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to report each device discovered separately
Arguments:
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
void AppInquiryInd(BTDEVHDL dev_hdl)
{
	BTUINT32 dev_class = 0;
	Btsdk_GetRemoteDeviceClass(dev_hdl, &dev_class);
	/*Just store devices of specified device class.*/
	if ( (s_rmt_dev_cls == 0)||(s_rmt_dev_cls == BTSDK_DEVCLS_MASK(dev_class & DEVICE_CLASS_MASK)))
	{
		s_rmt_dev_hdls[s_rmt_dev_num++] = dev_hdl;
	}	
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the callback function to process inquiry complete result ind
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void AppInqCompInd(void)
{
	/* notify the thread which starts device discovery */
	s_hBrowseDevEventHdl = OpenEvent(EVENT_ALL_ACCESS, FALSE, "completeBrowseDevice");
	SetEvent(s_hBrowseDevEventHdl);
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the callback function for connection complete event
Arguments:
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
void AppConnCompleteInd(BTDEVHDL dev_hdl)
{
	BTUINT32 dev_class = 0;
	Btsdk_GetRemoteDeviceClass(dev_hdl, &dev_class);
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the callback function for connection event
Arguments:
	BTCONNHDL conn_hdl: [in] connection handle
	BTUINT16 event: [in] event
	BTUINT8 *arg: [in] reserved
Return:
	void 
---------------------------------------------------------------------------*/
extern void AVRCP_App_GetElementAttrReq();
extern BTBOOL AVRCP_CT_Response_Cbk_Func();
extern void AVRCP_App_GetCurPlayerAppSetValReq();
extern void AVRCP_App_GetCapabilitiesReq();
extern void AVRCP_APP_InformCharSetReq();
extern void AVRCP_App_ListPlayerAppSetAttrReq();
extern void AVRCP_App_ListPlayerAppSetValReq();
extern void AVRCP_APP_GetPlayerAppSetAttrTxtReq();
extern void AVRCP_APP_GetPlayerAppSetValTxtReq();
void AppConnEventInd(BTCONNHDL conn_hdl, BTUINT16 event, BTUINT8 *arg)
{
	PBtSdkConnectionPropertyStru prop = (PBtSdkConnectionPropertyStru)arg;
	BTUINT8 bd_addr[6] = {0};
	BTUINT8 dev_name[120] = {0};
	BTDEVHDL hRmtDevHdl = 0;
	if (prop != NULL)
	{
		if (BTSDK_CONNROLE_CLIENT == prop->role)
		{
			if (BTSDK_APP_EV_CONN_CFM == event)//connect
			{
				if(BTSDK_CLS_AVRCP_TG == prop->service_class)
				{
					//Btsdk_AVRCP_CTRegResponseCbk(AVRCP_CT_Response_Cbk_Func);
					AVRCP_App_GetCapabilitiesReq();
					AVRCP_App_GetElementAttrReq();
					AVRCP_APP_InformCharSetReq();
					AVRCP_App_GetCurPlayerAppSetValReq();
					AVRCP_App_ListPlayerAppSetAttrReq();
					AVRCP_App_ListPlayerAppSetValReq();
					AVRCP_APP_GetPlayerAppSetAttrTxtReq();				
					AVRCP_APP_GetPlayerAppSetValTxtReq();
				}
			}
		}
	}
	switch (event)
	{
	case BTSDK_APP_EV_CONN_IND:
		printf("Receive BTSDK_APP_EV_CONN_IND from %s\n", dev_name);
		break;
	case BTSDK_APP_EV_CONN_CFM:
		printf("Receive BTSDK_APP_EV_CONN_CFM to %s\n", dev_name);
		break;
	case BTSDK_APP_EV_DISC_IND:
		printf("Receive BTSDK_APP_EV_DISC_IND from %s\n", dev_name);
		break;
	case BTSDK_APP_EV_DISC_CFM:
		printf("Receive BTSDK_APP_EV_DISC_CFM to %s\n", dev_name);
		break;
	default:
		printf("Receive unknown message %04x\n", event);
		break;
	}
}

/* start another thread to handle pin code event */
DWORD WINAPI ThirdpartyPinCodeThread(BTDEVHDL dev_hdl)
{
	BTUINT8 ch = 0;
	BTINT32 res = 0;
	char szPincode[16] = {0};
	printf("The pin code is:0000\n");
	sprintf(szPincode, "0000");
	res = Btsdk_PinCodeReply(dev_hdl, szPincode, (BTUINT16)strlen(szPincode));
	return (DWORD)res;
}

/* start another thread to handle io capability event */
DWORD WINAPI ThirdpartyIOCapReqThread(BTDEVHDL dev_hdl)
{
	Btsdk_IOCapReqReply(dev_hdl, BTSDK_IOCAP_DISPLAY_YESNO, 0, BTSDK_WITH_MITM_PROTECTION);
	return 0;
}

typedef struct _PassDataStruct
{
	BTDEVHDL dev_hdl;
	BTUINT32 param;
}PassDataStruct,*PPassDataStruct;

DWORD WINAPI ThirdpartyUserCfmReqThread(void *lParam)
{
	char szChoice[4] = {0};
	if (lParam != NULL)
	{
		PPassDataStruct pData = (PPassDataStruct)lParam;
		BTUINT32 num_value = pData->param;
		BTUINT32 dev_hdl = pData->dev_hdl;
		TCHAR tcNum[10];
		itoa(num_value, tcNum, 10);
		printf("The number is %s.\r\n", tcNum);
		printf("Please input whether this number is euqal to the other device(y/n):");
		scanf(" %s", szChoice);
		getchar();
		if ('y' == szChoice[0])
		{
			Btsdk_UsrCfmReqReply(dev_hdl, BTSDK_TRUE);
		}
		else if ('n' == szChoice[0])
		{
			Btsdk_UsrCfmReqReply(dev_hdl, BTSDK_FALSE);
		}
		free(pData);
	}
	return 0;
}

DWORD ThirdpartyAppPassKeyReqThread(BTDEVHDL dev_hdl)
{
	char tcPassKey[10] = {0};
	BTUINT32 num_value = 0;
	printf("Please input the key displayed in the other  device:");
	
	scanf(" %s", tcPassKey);
	getchar();	
	num_value = atoi(tcPassKey);
	Btsdk_PasskeyReqReply(dev_hdl, num_value);
	//Btsdk_PasskeyReqReply(dev_hdl, 0xFFFFFFFF);
	return 0;
}

DWORD ThirdpartyAppRemoteOOBDataReqThread(BTDEVHDL dev_hdl)
{
	//This callback ask for remote device's OOB data.
	//Application should get the OOB data by other media. and user
	//Btsdk_RemOOBDataReqReply to reply to the other device.
	return 0;
}

DWORD ThirdpartyAppPassKeyNotifyThread(void *lParam)
{
	if (lParam != NULL)
	{
		PPassDataStruct pData = (PPassDataStruct)lParam;
		BTUINT32 num_value = pData->param;
		BTUINT32 dev_hdl = pData->dev_hdl;
		printf("Wait for other device input the number %d.", num_value);
	}
	return 0;
}

DWORD WINAPI ThirdpartySimplePairCompleteThread(void *lParam)
{
	if (lParam != NULL)
	{
		PPassDataStruct pData = (PPassDataStruct)lParam;
		BTUINT8 result = (BTUINT8)pData->param;
		BTUINT32 dev_hdl = pData->dev_hdl;
		if (result == BTSDK_OK)
		{
			printf("Simple pair succeed.\r\n");
		}
		else
		{
			printf("Simple pair failed. The result code = %d\r\n", result);
		}
	}
	return 0;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the callback function for pin code request event
Arguments:
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
BTUINT8  ApppincodeInd(BTDEVHDL dev_hdl)
{
	HANDLE hThread= INVALID_HANDLE_VALUE;
	printf("Create other thread to handle this event\n");	
 	hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ThirdpartyPinCodeThread, (LPVOID)dev_hdl, 0, NULL);
	CloseHandle(hThread);
	return BTSDK_CLIENTCBK_HANDLED;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the callback function for pin code request event
Arguments:
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
BTUINT8 AppIOCapReqInd(BTDEVHDL dev_hdl)
{
	HANDLE hThread= INVALID_HANDLE_VALUE;
	printf("Create other thread to handle this event\n");	
 	hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ThirdpartyIOCapReqThread, (LPVOID)dev_hdl, 0, NULL);
	CloseHandle(hThread);
	return BTSDK_CLIENTCBK_HANDLED;
}

BTUINT8 AppUserCfmReqInd(BTDEVHDL dev_hdl, BTUINT32 num_value)
{
	HANDLE hThread = INVALID_HANDLE_VALUE;
	PPassDataStruct pData =(PPassDataStruct) malloc(sizeof(PassDataStruct));
	memset((void*)pData, 0, sizeof(PassDataStruct));
	pData->dev_hdl = dev_hdl;
	pData->param = num_value;
	hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ThirdpartyUserCfmReqThread, (void*)pData, 0, NULL);
	CloseHandle(hThread);
	return BTSDK_CLIENTCBK_HANDLED;
}

BTUINT8 AppPassKeyRequest(BTDEVHDL dev_hdl)
{
	HANDLE hThread = INVALID_HANDLE_VALUE;
	hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ThirdpartyAppPassKeyReqThread, (LPVOID)dev_hdl, 0, NULL);
	CloseHandle(hThread);
	return BTSDK_CLIENTCBK_HANDLED;
}

BTUINT8 AppRemoteOOBDataRequest(BTDEVHDL dev_hdl)
{
	HANDLE hThread = INVALID_HANDLE_VALUE;
	hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ThirdpartyAppRemoteOOBDataReqThread, (LPVOID)dev_hdl, 0, NULL);
	CloseHandle(hThread);
	return BTSDK_CLIENTCBK_HANDLED;
}

BTUINT8 AppPassKeyNotify(BTDEVHDL dev_hdl, BTUINT32 num_value)
{
	HANDLE hThread = INVALID_HANDLE_VALUE;
	PPassDataStruct pData =(PPassDataStruct) malloc(sizeof(PassDataStruct));
	memset((void*)pData, 0, sizeof(PassDataStruct));
	pData->dev_hdl = dev_hdl;
	pData->param = num_value;
	hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ThirdpartyAppPassKeyNotifyThread, (void*)pData, 0, NULL);
	CloseHandle(hThread);
	return BTSDK_CLIENTCBK_HANDLED;
}

BTUINT8 AppSimplePairCompleteInd(BTDEVHDL dev_hdl, BTUINT8 result)
{
	HANDLE hThread= INVALID_HANDLE_VALUE;
	BTUINT32 devType = Btsdk_GetRemoteDeviceType(dev_hdl);
	if (devType == BTSDK_DEV_TYPE_BREDR_ONLY || devType == BTSDK_DEV_TYPE_BREDR_LE)
	{
		PPassDataStruct pData =(PPassDataStruct) malloc(sizeof(PassDataStruct));
		memset((void*)pData, 0, sizeof(PassDataStruct));
		pData->dev_hdl = dev_hdl;
		pData->param = result;
		hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ThirdpartySimplePairCompleteThread, (void*)pData, 0, NULL);
		CloseHandle(hThread);
	}
	return BTSDK_CLIENTCBK_HANDLED;
}

//OBEX Authentication
struct TargetMapStru
{
	const BTUINT8 *descrption;
	const BTUINT8 *target;
	BTUINT8 target_len;
};

static const BTUINT8 TARGET_IRMC[]			 = "IRMC-SYNC";				
static const BTUINT8 TARGET_FOLDER_LISTING[] = {0xF9,0xEC,0x7B,0xC4,0x95,0x3C,0x11,0xD2,0x98,0x4E,0x52,0x54,0x00,0xDC,0x9E,0x09};
static const BTUINT8 TARGET_IMG_PUSH[]		 = {0xE3,0x3D,0x95,0x45,0x83,0x74,0x4A,0xD7,0x9E,0xC5,0xC1,0x6B,0xE3,0x1E,0xDE,0x8E};
static const BTUINT8 TARGET_IMG_PULL[]		 = {0x8E,0xE9,0xB3,0xD0,0x46,0x08,0x11,0xD5,0x84,0x1A,0x00,0x02,0xA5,0x32,0x5B,0x4E};
static const BTUINT8 TARGET_IMG_PRINT[]		 = {0x92,0x35,0x33,0x50,0x46,0x08,0x11,0xD5,0x84,0x1A,0x00,0x02,0xA5,0x32,0x5B,0x4E};
static const BTUINT8 TARGET_IMG_AUTO_ARCH[]	 = {0x94,0x01,0x26,0xC0,0x46,0x08,0x11,0xD5,0x84,0x1A,0x00,0x02,0xA5,0x32,0x5B,0x4E};
static const BTUINT8 TARGET_IMG_REM_CAM[]	 = {0x94,0x7E,0x74,0x20,0x46,0x08,0x11,0xD5,0x84,0x1A,0x00,0x02,0xA5,0x32,0x5B,0x4E};
static const BTUINT8 TARGET_IMG_REM_DISP[]   = {0x94,0xC7,0xCD,0x20,0x46,0x08,0x11,0xD5,0x84,0x1A,0x00,0x02,0xA5,0x32,0x5B,0x4E};
static const BTUINT8 TARGET_IMG_REF_OBJ[]	 = {0x8E,0x61,0xF9,0x5D,0x1A,0x79,0x11,0xD4,0x8E,0xA4,0x00,0x80,0x5F,0x9B,0x98,0x34};
static const BTUINT8 TARGET_IMG_ARCH_OBJ[]	 = {0x8E,0x61,0xF9,0x5E,0x1A,0x79,0x11,0xD4,0x8E,0xA4,0x00,0x80,0x5F,0x9B,0x98,0x34};
static const BTUINT8 TARGET_BPP_DPS[]		 = {0x00,0x00,0x11,0x18,0x00,0x00,0x10,0x00,0x80,0x00,0x00,0x80,0x5F,0x9B,0x34,0xFB};
static const BTUINT8 TARGET_BPP_PBR[]		 = {0x00,0x00,0x11,0x19,0x00,0x00,0x10,0x00,0x80,0x00,0x00,0x80,0x5F,0x9B,0x34,0xFB};
static const BTUINT8 TARGET_BPP_RUI[]		 = {0x00,0x00,0x11,0x21,0x00,0x00,0x10,0x00,0x80,0x00,0x00,0x80,0x5F,0x9B,0x34,0xFB};
static const BTUINT8 TARGET_BPP_STS[]		 = {0x00,0x00,0x11,0x23,0x00,0x00,0x10,0x00,0x80,0x00,0x00,0x80,0x5F,0x9B,0x34,0xFB};
static const BTUINT8 TARGET_BPP_REFOBJ[]	 = {0x00,0x00,0x11,0x20,0x00,0x00,0x10,0x00,0x80,0x00,0x00,0x80,0x5F,0x9B,0x34,0xFB};
static const BTUINT8 TARGET_PBAP_PSE[]		 = {0X79,0X61,0X35,0XF0,0XF0,0XC5,0X11,0XD8,0X09,0X66,0X08,0X00,0X20,0X0C,0X9A,0X66};
static const BTUINT8	TARGET_MAP_MAS[]	 = {0xbb,0x58,0x2b,0x40,0x42,0x0c,0x11,0xdb,0xb0,0xde,0x08,0x00,0x20,0x0c,0x9a,0x66};
static const BTUINT8	TARGET_MAP_MNS[]	 = {0xbb,0x58,0x2b,0x41,0x42,0x0c,0x11,0xdb,0xb0,0xde,0x08,0x00,0x20,0x0c,0x9a,0x66};

static const struct TargetMapStru s_targets_table[] =
{
	{(const unsigned char *)"OPP",					NULL,					0},
	{(const unsigned char *)"Sync",				TARGET_IRMC,			9},
	{(const unsigned char *)"FTP",					TARGET_FOLDER_LISTING,  16},
	{(const unsigned char *)"IMG-Push",			TARGET_IMG_PUSH,		16},
	{(const unsigned char *)"IMG-Pull",			TARGET_IMG_PULL,		16},
	{(const unsigned char *)"IMG-Print",			TARGET_IMG_PRINT,		16},
	{(const unsigned char *)"IMG-AutoArchive",		TARGET_IMG_AUTO_ARCH,	16},
	{(const unsigned char *)"IMG-Camera",			TARGET_IMG_REM_CAM,		16},
	{(const unsigned char *)"IMG-Display",			TARGET_IMG_REM_DISP,	16},
	{(const unsigned char *)"IMG-ReferencedObject",TARGET_IMG_REF_OBJ,		16},
	{(const unsigned char *)"IMG-ArchivedObject",	TARGET_IMG_ARCH_OBJ,	16},
	{(const unsigned char *)"BPP-DirectPrint",		TARGET_BPP_DPS,			16},
	{(const unsigned char *)"BPP-ReferencePrint",	TARGET_BPP_PBR,			16},
	{(const unsigned char *)"BPP-RefelectedUI",	TARGET_BPP_RUI,			16},
	{(const unsigned char *)"BPP-Status",			TARGET_BPP_STS,			16},
	{(const unsigned char *)"BPP-ReferencedObject",TARGET_BPP_REFOBJ,		16},
	{(const unsigned char *)"PBAP",				TARGET_PBAP_PSE,		16},
	{(const unsigned char *)"MAP-MAS",				TARGET_MAP_MAS,		16},
	{(const unsigned char *)"MAP-MNS",				TARGET_MAP_MNS,		16},
};

//this function is for handle OBEX Authentication
//If user register this callback it must handle it in this callback and return BTSDK_TRUE
BTUINT8 AppObexAuthenticationReqInd(BTCONNHDL conn_hdl, PBtSdkObexAuthInfoStru auth_info)
{
	//The conn_hdl is invalid, so do not use it.
	BTUINT8 dev_name[BTSDK_DEVNAME_LEN] = {0};
	int i;
	int num = sizeof(s_targets_table)/sizeof(struct TargetMapStru);
	if (!auth_info)
	{
		return BTSDK_FALSE;
	}
	Btsdk_GetRemoteDeviceName(auth_info->dev_hdl, dev_name, NULL);
	for (i=0; i<num; i++)
	{
		//Judge which service is connected.
		if ((s_targets_table[i].target_len == auth_info->target_len) 
			&& (s_targets_table[i].target == NULL || 
			!memcmp(s_targets_table[i].target, auth_info->target, 
			auth_info->target_len)))
		{
			break;
		}
	}
	
	//There is some console application related bug, so when you are testing this callback function.
	//The userID is test, the first character will be omit, and so does the pwd.
	//So if the real pwd is "1234", Please input"x1234", x can be any character.
	if (i < num)
	{
		if (!auth_info->pwd_only)
		{
			printf("Please input the user ID for remote OBEX service:\n");
			//You can get the userID from other GUI.
			scanf(" %s", auth_info->user_id);
			
		}
		printf("Please input the password for remote OBEX service:\r\n");
		//You can get the pwd from other GUI.
		scanf(" %s", auth_info->pwd);
		return BTSDK_TRUE;
	}
	else
	{
		return BTSDK_FALSE;
	}
}

/* create a structure to package parameters */
typedef struct _AuthParaStru
{ 
	BTSVCHDL svc_hdl;
	BTDEVHDL dev_hdl;
}AuthParaStru,*PAuthParaStru;

/* start another thread to handle authentication event */
DWORD WINAPI ThirdpartyAuthThread(PAuthParaStru stru)
{
	/* authorization allowed */
	BTDEVHDL devHdl = stru->dev_hdl;
	BTSVCHDL svcHdl = stru->svc_hdl;
	SetEvent(s_waitAuthor);
	/*Wait for user handle authorization*/
	Btsdk_AuthorizationResponse(svcHdl, devHdl, BTSDK_AUTHORIZATION_GRANT);
	
	/* authorization denied */
	/* Btsdk_AuthorizationResponse(stru->svc_hdl, stru->dev_hdl, BTSDK_AUTHORIZATION_DENY);*/
	
	return 1;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the callback function for authentication event
Arguments:
    BTSVCHDL svc_hdl: [in] service handle
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
BTUINT8 AppAuthInd(BTSVCHDL svc_hdl, BTDEVHDL dev_hdl)
{
	HANDLE hThread= INVALID_HANDLE_VALUE;
	char szChoice[4] = {0};
	AuthParaStru AuthStru = {0};
	AuthStru.svc_hdl = svc_hdl;
	AuthStru.dev_hdl = dev_hdl;
	printf("Authentication event is handled by the third party.\n");
	printf("Create other thread to handle this event\n");
	if (!s_waitAuthor)
	{
		s_waitAuthor = CreateEvent(NULL, FALSE, FALSE, NULL);
	}
 	hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ThirdpartyAuthThread, (LPVOID)&AuthStru, 0, NULL);
	WaitForSingleObject(s_waitAuthor, INFINITE);
	CloseHandle(s_waitAuthor);
	s_waitAuthor = NULL;
	CloseHandle(hThread);
	return BTSDK_CLIENTCBK_HANDLED;
}

char* HexNumber2String(unsigned char* hex_num, int hex_len)
{
	int i = 0;
	char* str_num = NULL;
	
	if (hex_len <= 0)
		return NULL;
	
	str_num = (char*) malloc(hex_len*2+1);
	memset(str_num, 0, hex_len*2+1);
	
	for (i=0;i<hex_len;i++)
		sprintf(str_num+i*2, "%02X", hex_num[i]);
	
	return str_num;
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the callback function for link key notification event
Arguments:
	BTDEVHDL device_handle: [in] device handle
	BTUINT8* link_key: [in] link key
Return:
	void 
---------------------------------------------------------------------------*/
BTUINT32 AppLinkNotifInd(BTDEVHDL device_handle, BTUINT8* link_key)
{
	BTUINT8 linkKey[BTSDK_LINKKEY_LEN] = {0};
	char *pLinkKeyStr = NULL;
	memcpy(linkKey, link_key, BTSDK_LINKKEY_LEN);
	//Link key is hex number convert to string and show it
	pLinkKeyStr = HexNumber2String(linkKey, BTSDK_LINKKEY_LEN);
	printf("Received the link key of this device handle. linkkey=%s\n", pLinkKeyStr);
	if (pLinkKeyStr != NULL)
	{
		free(pLinkKeyStr);
	}
	return 1;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the callback function for authentication failure event
Arguments:
	BTDEVHDL device_handle: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
void AppAuthFailedInd(BTDEVHDL device_handle)
{
	BTUINT32 devType = Btsdk_GetRemoteDeviceType(device_handle);
	if (devType == BTSDK_DEV_TYPE_BREDR_LE || devType == BTSDK_DEV_TYPE_BREDR_ONLY)
	{
		printf("Authentication Failed! the device handle is:%x\n", device_handle);
	}
}

void AppLinkRequest(BTDEVHDL device_handle)
{
	printf("linkkey request! the device handle is:%x\n", device_handle);
	//Reply there is not link key stored.
	Btsdk_LinkKeyReply(device_handle, NULL);
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function registers callbacks.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
extern void AppConnectionCompleteCbk(BTDEVHDL dev_hdl);
extern void AppDisconnectionCompleteCbk(BTDEVHDL dev_hdl, BTUINT32 reason);
extern void AppDeviceFoundFuncCbk(BTDEVHDL dev_hdl);
void RegAppIndCallback(void)
{
	BtSdkCallbackStru cb = {0};
	/* inquiry result ind*/
	cb.type = BTSDK_INQUIRY_RESULT_IND;
	cb.func = (void*)AppInquiryInd;
	Btsdk_RegisterCallback4ThirdParty(&cb);

	/* inquiry complete result ind*/
	cb.type = BTSDK_INQUIRY_COMPLETE_IND;
	cb.func = (void*)AppInqCompInd;
	Btsdk_RegisterCallback4ThirdParty(&cb);

	/* connection event ind */
	cb.type = BTSDK_CONNECTION_EVENT_IND;
	cb.func = (void*)AppConnEventInd;
	Btsdk_RegisterCallback4ThirdParty(&cb);

    /* pin code ind */
	cb.type = BTSDK_PIN_CODE_IND;
	cb.func = (BTUINT8*)ApppincodeInd;
    Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	/* authorization ind */
	cb.type = BTSDK_AUTHORIZATION_IND;
	cb.func = (BTUINT8*)AppAuthInd;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);
	
	/* link key notification ind */
	cb.type = BTSDK_LINK_KEY_NOTIF_IND;
	cb.func = (BTUINT8*)AppLinkNotifInd;
	Btsdk_RegisterCallback4ThirdParty(&cb);	
	
	/* authentication fail ind */
	cb.type = BTSDK_AUTHENTICATION_FAIL_IND;
	cb.func = (BTUINT8*)AppAuthFailedInd;
	Btsdk_RegisterCallback4ThirdParty(&cb);

	/* Link key request */
	cb.type = BTSDK_LINK_KEY_REQ_IND;
	cb.func = (void*)AppLinkRequest;
	Btsdk_RegisterCallback4ThirdParty(&cb);

	/* IO capability request ind */
	cb.type = BTSDK_IOCAP_REQ_IND;
	cb.func = (BTUINT8*)AppIOCapReqInd;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_USR_CFM_REQ_IND;
	cb.func = (BTUINT8*)AppUserCfmReqInd;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);
	
	cb.type = BTSDK_PASSKEY_REQ_IND;
	cb.func = (BTUINT8*)AppPassKeyRequest;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_REM_OOBDATA_REQ_IND;
	cb.func = (BTUINT8*)AppRemoteOOBDataRequest;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_PASSKEY_NOTIF_IND;
	cb.func = (BTUINT8*)AppPassKeyNotify;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_SIMPLE_PAIR_COMPLETE_IND;
	cb.func = (BTUINT8*)AppSimplePairCompleteInd;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_OBEX_AUTHEN_REQ_IND;
	cb.func = (BTUINT8*)AppObexAuthenticationReqInd;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);
	
	cb.type = BTSDK_CONNECTION_COMPLETE_IND;
	cb.func = (void*)AppConnectionCompleteCbk;
	Btsdk_RegisterCallback4ThirdParty(&cb);
	cb.type = BTSDK_DISCONNECTION_COMPLETE_IND;
	cb.func = (void*)AppDisconnectionCompleteCbk;
	Btsdk_RegisterCallback4ThirdParty(&cb);
	//LE Device is advertising
	cb.type = BTSDK_DEVICE_FOUND_IND;
	cb.func = (void*)AppDeviceFoundFuncCbk;
	Btsdk_RegisterCallback4ThirdParty(&cb);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function unregisters callbacks.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void UnRegAppIndCallback(void)
{
	BtSdkCallbackStru cb;

	cb.type = BTSDK_INQUIRY_RESULT_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);
	cb.type = BTSDK_INQUIRY_COMPLETE_IND;
	cb.func = NULL;	
	Btsdk_RegisterCallback4ThirdParty(&cb);
	cb.type = BTSDK_CONNECTION_EVENT_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);
	cb.type = BTSDK_LINK_KEY_NOTIF_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);	
	cb.type = BTSDK_AUTHENTICATION_FAIL_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);	
	cb.type = BTSDK_LINK_KEY_REQ_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);
	/* IO capability request ind */
	cb.type = BTSDK_IOCAP_REQ_IND;
	cb.func = NULL;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_USR_CFM_REQ_IND;
	cb.func = NULL;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_PASSKEY_REQ_IND ;
	cb.func = NULL;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_REM_OOBDATA_REQ_IND;
	cb.func = NULL;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_PASSKEY_NOTIF_IND;
	cb.func = NULL;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);
	
	cb.type = BTSDK_SIMPLE_PAIR_COMPLETE_IND;
	cb.func = NULL;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_OBEX_AUTHEN_REQ_IND;
	cb.func = NULL;
	Btsdk_RegisterCallbackEx(&cb, BTSDK_CLIENTCBK_PRIORITY_HIGH);

	cb.type = BTSDK_CONNECTION_COMPLETE_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);
	cb.type = BTSDK_DISCONNECTION_COMPLETE_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);

	//LE Device is advertising
	cb.type = BTSDK_DEVICE_FOUND_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to display remote devices
Arguments:
	BTUINT32 dev_class: [in] device class
Return:
	void 
---------------------------------------------------------------------------*/
void DisplayRemoteDevices(BTUINT32 dev_class)
{
	int i = 0;
	int j = 0;
	BTUINT8 szDevName[BTSDK_DEVNAME_LEN] = { 0 };
	BTUINT8 szTmp[32] = { 0 };
	BTUINT16 usLen = 0;
	BTUINT32 ulDevClass = 0;
	BTUINT8 szBdAddr[BD_ADDR_LEN] = {0};
	char cQuote = ' ';

	printf("Remote devices searched:\n");
	printf("number  device name %21hc device address %4hc device class\n", cQuote, cQuote);

	for (i = 0; i < s_rmt_dev_num; i++) /* s_rmt_dev_num is get by inquiry callback */
	{
		Btsdk_GetRemoteDeviceClass(s_rmt_dev_hdls[i], &ulDevClass);
		if ((dev_class != 0) && (dev_class != BTSDK_DEVCLS_MASK(ulDevClass & DEVICE_CLASS_MASK)))
		{			
			for (j=i; j<s_rmt_dev_num-1; j++)
			{
				s_rmt_dev_hdls[j] = s_rmt_dev_hdls[j+1];
			}
			s_rmt_dev_hdls [j] = BTSDK_INVALID_HANDLE;
			s_rmt_dev_num--;
			i--;			
			continue;
		}
		/*In order to display neatly.*/
 		if (i<9)
 		{
			printf("  %d%5hc", i + 1, cQuote);
 		}
		else
		{
			printf("  %d%4hc", i + 1, cQuote);
 		}
		
		usLen = 32;
		memset(szDevName, 0, sizeof(szDevName));
		if (Btsdk_GetRemoteDeviceName(s_rmt_dev_hdls[i], szDevName, &usLen) != BTSDK_OK)
		{
			if (Btsdk_UpdateRemoteDeviceName(s_rmt_dev_hdls[i], szDevName, &usLen) != BTSDK_OK)
			{
				strcpy((char*)szDevName, "Unknown");
			}
		}

		strcpy(szTmp, szDevName);
		MultibyteToMultibyte(CP_UTF8, szTmp, -1, CP_ACP, szDevName, BTSDK_DEVNAME_LEN);
		printf("%-34hs", szDevName);

		Btsdk_GetRemoteDeviceAddress(s_rmt_dev_hdls[i], szBdAddr);
		for(j = 5; j > 0; j --)
		{
			printf("%02X:", szBdAddr[j]);
		}
		printf("%02X%3hc", szBdAddr[0], cQuote);

		Btsdk_GetRemoteDeviceClass(s_rmt_dev_hdls[i], &ulDevClass);
		printf("0X%08X\r\n", ulDevClass);
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to get stored handles of remote device.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void GetAllRmtDevHdl()
{
	BTSDKHANDLE hEnumHdl = BTSDK_INVALID_HANDLE;
	INT i = 0;
	s_rmt_dev_num = 0;
	memset(s_rmt_dev_hdls, 0, sizeof(s_rmt_dev_hdls));

	hEnumHdl = Btsdk_StartEnumRemoteDevice(BTSDK_ERD_FLAG_NOLIMIT, 0);
	if (BTSDK_INVALID_HANDLE != hEnumHdl)
	{
		for (i = 0; i < MAX_DEV_NUM; i++)
		{
			s_rmt_dev_hdls[i] = Btsdk_EnumRemoteDevice(hEnumHdl, NULL);
			if (BTSDK_INVALID_HANDLE == s_rmt_dev_hdls[i])
			{
				s_rmt_dev_hdls[i] = BTSDK_INVALID_HANDLE;
				break;
			}
			s_rmt_dev_num++;
		}
		Btsdk_EndEnumRemoteDevice(hEnumHdl);
		hEnumHdl = BTSDK_INVALID_HANDLE;
	}

}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select device you want.
Arguments:
	BTUINT32 dev_class: [in] device class
Return:
	BTDEVHDL 
---------------------------------------------------------------------------*/
BTDEVHDL SelectRemoteDevice(BTUINT32 dev_class)
{
	int nIdx = 0;
	char szChoice[4] = {0};

	GetAllRmtDevHdl();
	/*show remote devices. If there is no device shown, search for them at first*/
	if (0 == s_rmt_dev_num)
	{
		StartSearchDevice(dev_class);
	}
	else
	{
		DisplayRemoteDevices(dev_class);
	}
	
	printf("Select the target device :\n"); 
	printf("if there is no expected device, please press 'a' to search again!\n");
	printf("if you want to exit this procedure, please press 'q' to quit.\n");
	
	do
	{
		printf("Target device number = ");
		scanf(" %s", szChoice);
		getchar();
		if ('a' == szChoice[0])
		{
			StartSearchDevice(dev_class);		
			continue;
		}
		if(('q' == szChoice[0]) || ('Q' == szChoice[0]))
		{
			printf("\nUser abort the operation.\n");
			return BTSDK_INVALID_HANDLE;
 		}
		nIdx = atoi(szChoice);
		if((nIdx <= 0) || (nIdx > s_rmt_dev_num))
		{
			printf("%d is not a valid datum, please select again.\n", nIdx);
			continue;
		}
		else
		{
			break;
		}
	} while (1);

	return (s_rmt_dev_hdls[nIdx - 1]);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function searches for devices
Arguments:
	BTUINT32 device_class: [in] device class
Return:
	void 
---------------------------------------------------------------------------*/
void StartSearchDevice(BTUINT32 device_class)
{
	BtSdkCallbackStru cb = {0};
	/*init remote devices info discovered.remote device handle and number will be set in AppInquiryInd*/
	memset(s_rmt_dev_hdls, 0, sizeof(s_rmt_dev_hdls));
	s_rmt_dev_num = 0;
	s_rmt_dev_cls = device_class;

	s_hBrowseDevEventHdl = CreateEvent(NULL, FALSE, FALSE, "completeBrowseDevice");
	printf("Please wait for a while searching for remote devices......\n");

	cb.type = BTSDK_INQUIRY_RESULT_IND;
	cb.func = (void*)AppInquiryInd;
	Btsdk_RegisterCallback4ThirdParty(&cb);
	
	cb.type = BTSDK_INQUIRY_COMPLETE_IND;
	cb.func = (void*)AppInqCompInd;
	Btsdk_RegisterCallback4ThirdParty(&cb);
	
	if (BTSDK_OK == Btsdk_StartDeviceDiscovery(0, MAX_DEV_NUM, 45))
	{
		/*wait BTSDK_INQUIRY_COMPLETE_IND. When complete inquiry, AppInqCompInd will notify*/
		WaitForSingleObject(s_hBrowseDevEventHdl, INFINITE);		
		
		DisplayRemoteDevices(0);
		printf("Search for remote devices completely.\n");
	}
	else
	{
		printf("Fail to initiate device searching!\n");
	}

	cb.type = BTSDK_INQUIRY_RESULT_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);
	
	cb.type = BTSDK_INQUIRY_COMPLETE_IND;
	cb.func = NULL;
	Btsdk_RegisterCallback4ThirdParty(&cb);

	if (NULL != s_hBrowseDevEventHdl)
	{
		CloseHandle(s_hBrowseDevEventHdl);
		s_hBrowseDevEventHdl = NULL;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function prints basic information of remote device
Arguments:
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
void Test_Print_Remote_Basic_Info(BTDEVHDL dev_hdl)
{
	int j = 0;
	BTUINT32 ulResult = BTSDK_OK;
	BTUINT8 szDevName[BTSDK_DEVNAME_LEN] = { 0 };
	BtSdkRemoteDevicePropertyStru struDevProperty = { 0 };

	ulResult = Btsdk_GetRemoteDeviceProperty(dev_hdl, &struDevProperty);
	PrintErrorMessage(ulResult, BTSDK_TRUE);
	if(BTSDK_OK == ulResult)
	{
		MultibyteToMultibyte(CP_UTF8, struDevProperty.name, -1, CP_ACP, szDevName, BTSDK_DEVNAME_LEN);
		printf("The basic information of the selected device is :\n");
		printf("Name = \"%s\"\n", szDevName);
		printf("BD Addr: ");
		for(j = 5; j > 0; j--)
		{
			printf("%02X:", struDevProperty.bd_addr[j]);
		}
		printf("%02X\n", struDevProperty.bd_addr[0]);
		printf("Device Class: %08lX\n", struDevProperty.dev_class);		
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function pairs a remote device
Arguments:
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
void Test_Btsdk_PairDevice(BTDEVHDL dev_hdl)
{
	BTUINT32 ulResult = BTSDK_OK;

	printf("Pairing...\n");
	//WriteSimplePairMode(FALSE) //With function user can disable simple pair
	ulResult = Btsdk_PairDevice(dev_hdl);
	if (BTSDK_OK == ulResult)
	{

		printf("Succeed in pairing with the remote device.\n");
	}
	else
	{
		PrintErrorMessage(ulResult,  BTSDK_TRUE);
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function unpairs a remote device
Arguments:
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
void Test_Btsdk_UnPairDevice(BTDEVHDL dev_hdl)
{
	BTUINT32 ulResult = BTSDK_OK;
	printf("Unpairing with the remote device...\n");
	ulResult = Btsdk_UnPairDevice(dev_hdl);
	if (BTSDK_OK == ulResult)
	{
		printf("Unpair with the remote device successfully.\n");
	}
	else
	{
		PrintErrorMessage(ulResult,  BTSDK_TRUE);
	}
}



/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function deletes a remote device
Arguments:
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
void Test_Btsdk_DelRemoteDevice(BTDEVHDL dev_hdl)
{
	BTUINT32 ulResult = BTSDK_OK;
	int i = 0;

	ulResult  = Btsdk_DeleteRemoteDeviceByHandle(dev_hdl);
	PrintErrorMessage(ulResult, BTSDK_TRUE);
	
	if (BTSDK_OK == ulResult)
	{
		for (i = 0; i < s_rmt_dev_num; i++)
		{
			if (s_rmt_dev_hdls[i] == s_curr_dev)
			{
				break;
			}
		}
	
		if ( i != s_rmt_dev_num)
		{
			for (; i < s_rmt_dev_num - 1; i++)
			{
				s_rmt_dev_hdls[i] = s_rmt_dev_hdls[i + 1];
			}
			s_curr_dev = 0;
			printf("Delete the remote device successfully.\n");
		}
	}
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function shows the remote device manager sample menu
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void RmtDevShowMenu()
{
	printf("*****************************************\n");
	printf("*  Remote Device Manager Testing Menu   *\n");
	printf("* <1> Search remote devices             *\n");
	printf("* <2> Select a remote device            *\n");
	printf("* <3> Get a remote device's basic Info  *\n");
	printf("* <4> Pair with the selected device     *\n");
	printf("* <5> Unpair with the selected device   *\n");
	printf("* <6> Delete the selected device        *\n");
	printf("* <r> Return to the upper menu          *\n");
	printf("*****************************************\n");    
	printf(">");
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to execute user's choice
Arguments:
	BTUINT8 choice: [in] user's choice
Return:
	void 
---------------------------------------------------------------------------*/
void RmtDevExecCmd(BTUINT8 choice)
{
	switch (choice) 
	{
	case '1':
		StartSearchDevice(0);/*search all devices*/
		break;
	case '2':
		s_curr_dev = SelectRemoteDevice(0);
		if (BTSDK_INVALID_HANDLE != s_curr_dev)
		{
			printf("You have succeeded in getting a remote device's handle.");
		}
		break;
	case '3':
		Test_Print_Remote_Basic_Info(s_curr_dev);
		break;
	case '4':
		Test_Btsdk_PairDevice(s_curr_dev);
		break;
	case '5':
		Test_Btsdk_UnPairDevice(s_curr_dev);
		break;
	case '6':
		Test_Btsdk_DelRemoteDevice(s_curr_dev);
		break;
	default:
		printf("Invalid command.\n");
		break;
	}
}



/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the main function of remote device manager
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestRmtDevMgr(void)
{
	BTUINT8 cChoice = 0;

	printf("Please carry out the following menu step by step because of the continuity in function.\n");
	RmtDevShowMenu();	
	while (cChoice != 'r')
	{
		scanf(" %c", &cChoice);
		getchar();		
		if (cChoice == '\n')
		{
			printf(">>");
		}
		else if('r' == cChoice)
		{
			break;
		}
		else
		{
			RmtDevExecCmd(cChoice);
			printf("\n");
			RmtDevShowMenu();
		}
	}
}


