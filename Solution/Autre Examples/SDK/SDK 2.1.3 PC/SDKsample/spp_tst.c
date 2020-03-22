/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2008 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    spp_tst.c
Abstract:
    this module is to test SPP profiles relative functionality. 
Revision History:
	2008-12-05   Chu Enlai  Created

---------------------------------------------------------------------------*/

#include "sdk_tst.h"
#include "profiles_tst.h"
#include "Btsdk_Stru.h"

static BTCONNHDL s_currSPPConnHdl = BTSDK_INVALID_HANDLE;
static BTDEVHDL s_currRmtSppDevHdl = BTSDK_INVALID_HANDLE;
static BTSHCHDL s_currSppSvcHdl = BTSDK_INVALID_HANDLE;
static BTSVCHDL s_CurrLocalSppSvcHdl = BTSDK_INVALID_HANDLE;

static BTUINT8 s_ComSerialNum = 0;

void AddOneSPPServiceByName();
void DeleteTheSPPServiceByName();
void SetSecureLevelForLocalService();
void SetFixPinCodeForRemoteDevice();
void GetSPPServiceInfo();
void ChangeCOMPortForLocalSPPService();
#define LOCAL_SPP_SERVICE_NAME TEXT("KSERIALPORT")
#define LOCAL_SPP_FOR_DELETE_NAME TEXT("FORDELETENAME")

INT LocalToUTF8(LPSTR pUFT8, LPCSTR pLocal, DWORD dwChars);
void GetCommonSettingFilePath(TCHAR *pszcsFilePath, UINT uiStrLen);
BOOL IsX64Platform();
void SyncIniFileInfo(BOOL bAddService, INT comportNumber);

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select device's handle.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectRmtSppDev()
{   
    s_currRmtSppDevHdl = SelectRemoteDevice(0);
	if (BTSDK_INVALID_HANDLE == s_currRmtSppDevHdl)
	{
		printf("Please make sure that the expected device \
				is in discoverable state and search again.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select service handle of SPP service.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectSppSvc()
{
	s_currSppSvcHdl = SelectRemoteService(s_currRmtSppDevHdl);
	if (BTSDK_INVALID_HANDLE == s_currSppSvcHdl)
	{
		printf("Cann't get expected service handle.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to connect SPP service by its service handle.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestConnectSppSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;
	BTUINT32 OSComPort;

	s_ComSerialNum = Btsdk_GetASerialNum();
	Btsdk_PlugInVComm(s_ComSerialNum , &OSComPort ,1 ,COMM_SET_RECORD|COMM_SET_USAGETYPE, 2200);
	ulRet = Btsdk_InitCommObj((BTUINT8)OSComPort, BTSDK_CLS_SERIAL_PORT);
	
	if(ulRet == BTSDK_OK)		
	{	
		BtSdkSPPConnParamStru sppStru = {0};
		sppStru.size = sizeof(BtSdkSPPConnParamStru);
		sppStru.com_index = OSComPort ;
		ulRet = Btsdk_ConnectEx(s_currRmtSppDevHdl, BTSDK_CLS_SERIAL_PORT, 
					(BTINT32)&sppStru, &s_currSPPConnHdl);
		if (ulRet == BTSDK_OK)
		{
			printf("Connect remote SPP Service with local COM%d\n", OSComPort);
		}
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show SPP Services user interface.
Arguments:
    void
Return:
	void 
---------------------------------------------------------------------------*/
void SppShowMenu()
{
	printf("*****************************************\n");
	printf("*           SPP Testing Menu            *\n");
	printf("* <1> Connect Remote SPP Service        *\n");
	printf("* <2> Disconnect                        *\n");
	printf("* <r> Return to upper menu              *\n");
	printf("*****************************************\n");
	printf(">");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution for SPP sample.
Arguments:
	BTUINT8 choice
Return:
	void 
---------------------------------------------------------------------------*/
void SppExecCmd(BTUINT8 choice)
{
	BTINT8 ch = 0;
	BTUINT8 comNum = 0;

	if (choice == '1')
	{
		TestSelectRmtSppDev();
		TestSelectSppSvc();
		TestConnectSppSvc();
		SppShowMenu();
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
				SppExecCmd(ch);
				printf("\n");
				SppShowMenu();
			}
		}
	}
	else if (choice == '2')
	{
		if (s_currSPPConnHdl)
		{
			comNum = Btsdk_GetClientPort(s_currSPPConnHdl);
			Btsdk_Disconnect(s_currSPPConnHdl);
			s_currSPPConnHdl = BTSDK_INVALID_HANDLE;
			Btsdk_DeinitCommObj(comNum);
			Btsdk_PlugOutVComm(s_ComSerialNum, COMM_SET_RECORD);
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
	This function is the entry function for SPP sample.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestSPPFunc(void)
{
	BTUINT8 ch = 0;
	BTUINT8 comNum = 0;

	SppShowMenu();	
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
			SppExecCmd(ch);
			printf("\n");
			SppShowMenu();
		}
	}

	if (BTSDK_INVALID_HANDLE != s_currSPPConnHdl)
	{
		comNum = Btsdk_GetClientPort(s_currSPPConnHdl);
		Btsdk_Disconnect(s_currSPPConnHdl);
		s_currSPPConnHdl = BTSDK_INVALID_HANDLE;
		Btsdk_DeinitCommObj(comNum);
		Btsdk_PlugOutVComm(s_ComSerialNum, COMM_SET_RECORD);
	}
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show SPP Services user interface.
Arguments:
    void
Return:
	void 
---------------------------------------------------------------------------*/
void LocalSppServiceShowMenu()
{
	printf("*************************************************\n");
	printf("*               SPP Testing Menu                *\n");
	printf("* <1> Add one SPP Service by name               *\n");
	printf("* <2> Delete the SPP service by name            *\n");
	printf("* <3> Set security level for local spp service  *\n");
	printf("* <4> Set fix pincode for remote device         *\n");
	printf("* <5> Get local service information             *\n");
	printf("* <6> Change comport for local service	        *\n");
	printf("* <r> Return to upper menu                      *\n");
	printf("*************************************************\n");
	printf(">");
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution for SPP sample.
Arguments:
	BTUINT8 choice
Return:
	void 
---------------------------------------------------------------------------*/
void LocalSppServiceExecCmd(BTUINT8 choice)
{
	BTINT8 ch = 0;
	BTUINT8 comNum = 0;

	if (choice == '1')
	{
		AddOneSPPServiceByName();
	}
	else if (choice == '2')
	{
		DeleteTheSPPServiceByName();		
	}
	else if (choice == '3')
	{
		if (s_CurrLocalSppSvcHdl != BTSDK_INVALID_HANDLE)
		{
			SetSecureLevelForLocalService();
		}
		else
		{
			printf("Please make sure you have add the SPP service");
		}		
	}
	else if (choice == '4')
	{
		SetFixPinCodeForRemoteDevice();		
	}
	else if (choice == '5')
	{
		GetSPPServiceInfo();
	}
	else if (choice == '6')
	{
		ChangeCOMPortForLocalSPPService();
	}
	else
	{
		printf("Invalid command.\n");
	}
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the entry function for local SPP service sample.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestLocalSPPServiceFunc(void)
{
	BTUINT8 ch = 0;
	BTUINT8 comNum = 0;
	LocalSppServiceShowMenu();
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
			LocalSppServiceExecCmd(ch);
			printf("\n");
			LocalSppServiceShowMenu();
		}
	}
	
	if (BTSDK_INVALID_HANDLE != s_CurrLocalSppSvcHdl)
	{
		DeleteTheSPPServiceByName();		
	}

}

void AddOneSPPServiceByName()
{
	BTSVCHDL svc_hdl = BTSDK_INVALID_HANDLE;
	//1. First judge whether the SPP service KSERIALPORT has already exist	
	BTSDKHANDLE enum_handle = BTSDK_INVALID_HANDLE;
	BtSdkLocalServerAttrStru attribute = {0};
	int comportNumber;
	int serialNum;
	attribute.mask = BTSDK_LSAM_SERVICENAME;
	enum_handle = Btsdk_StartEnumLocalServer();
	if (enum_handle != BTSDK_INVALID_HANDLE)
	{
		while(BTSDK_INVALID_HANDLE != (svc_hdl = Btsdk_EnumLocalServer(enum_handle, &attribute)))
		{
			if (attribute.service_class == BTSDK_CLS_SERIAL_PORT && !stricmp(attribute.svc_name, LOCAL_SPP_SERVICE_NAME))
			{
				s_CurrLocalSppSvcHdl = svc_hdl;
				break;
			}
		}
	}
	if (s_CurrLocalSppSvcHdl != BTSDK_INVALID_HANDLE)
	{
		printf("Already has one local SPP service with the same name.");
		return;
	}
	// 2. Do not have the service, so add now

	serialNum = Btsdk_GetASerialNum();
	if (Btsdk_PlugInVComm(serialNum, (ULONG*)&comportNumber, 1, COMM_SET_RECORD|COMM_SET_USAGETYPE, 5000))
	{
		Btsdk_InitCommObj((BTUINT8)comportNumber, BTSDK_CLS_SERIAL_PORT);
	}
	printf("Plug in one comport for local service. COM number is %d", comportNumber);
	if (comportNumber != 0)
	{
		char ucSvcName[BTSDK_SERVICENAME_MAXLENGTH] = {0};
		char serverName[BTSDK_SERVICENAME_MAXLENGTH] = {0};
		BtSdkLocalServerAttrStru attr = {0};
		BtSdkLocalSPPServerAttrStru spp_attr = {0};
		int length;
		sprintf(serverName, "%s", LOCAL_SPP_SERVICE_NAME); //This is the servic name		
		//Set general service attribute
		attr.mask = BTSDK_LSAM_SERVICENAME|BTSDK_LSAM_SECURITYLEVEL
			|BTSDK_LSAM_AUTHORMETHOD|BTSDK_LSAM_EXTATTRIBUTES;
		attr.service_class = BTSDK_CLS_SERIAL_PORT;
		attr.security_level = BTSDK_SSL_NO_SECURITY;
		attr.author_method = BTSDK_AUTHORIZATION_ACCEPT;
		//Set SPP service special attribute
		spp_attr.size = sizeof(BtSdkLocalSPPServerAttrStru);
		spp_attr.mask = BTSDK_LSPPSAM_COMINDEX;
		spp_attr.com_index = (BTUINT8)comportNumber;
		attr.ext_attributes = &spp_attr;		
		//Make sure the service name is UTF8
		length = LocalToUTF8((BTINT8 *)ucSvcName, (BTINT8 *)serverName, BTSDK_SERVICENAME_MAXLENGTH);	
		strcpy((char *)attr.svc_name, (LPCSTR)ucSvcName);
		SyncIniFileInfo(TRUE, comportNumber);//This is for ADD local SPP service		
		s_CurrLocalSppSvcHdl = Btsdk_AddServer(&attr);
		if (s_CurrLocalSppSvcHdl != BTSDK_INVALID_HANDLE)
		{
			Btsdk_StartServer(s_CurrLocalSppSvcHdl);
		}
	}
	
}

INT LocalToUTF8(LPSTR pUFT8, LPCSTR pLocal, DWORD dwChars)
{
	WCHAR *pWChar = NULL;
	int nWideLength;
	*pUFT8 = 0;	
	
#ifdef UNICODE
#error This function only can be used to ansi.
#else

	nWideLength = MultiByteToWideChar(CP_ACP, 0, pLocal, -1, NULL, 0);
	if (nWideLength <= 0)
	{
		return 0;
	}
	
	pWChar = malloc(nWideLength*sizeof(WCHAR));	
	if (NULL != pWChar)
	{
		MultiByteToWideChar(CP_ACP,0, pLocal, -1, pWChar,nWideLength);
		
		WideCharToMultiByte( CP_UTF8, 
			0, 
			pWChar, 
			-1, 
			pUFT8, 
			dwChars, 
			NULL, 
			NULL);
		free(pWChar);
		pWChar = NULL;
	}	
#endif
	
	return lstrlen(pUFT8);
}

#define BTSDK_DEFAULT_SPPSVC_NUMBER 2
void SyncIniFileInfo(BOOL bAddService, INT comportNumber)
{
	TCHAR szBscsPath[MAX_PATH] = {0};
	TCHAR sectionName[20] = {0};
	TCHAR comName[20] = {0};
	UINT sppSvcNum;
	TCHAR newLocation[20] = {0};
	TCHAR comTemp[20] = {0};
	TCHAR nameTemp[BTSDK_SERVICENAME_MAXLENGTH] = {0};
	GetCommonSettingFilePath(szBscsPath, MAX_PATH);
	sppSvcNum = Btsdk_GetPrivateProfileInt(TEXT("LocalBluetooth"), TEXT("SPPSvcNum"), BTSDK_DEFAULT_SPPSVC_NUMBER, szBscsPath);
	if (bAddService)
	{
		Btsdk_WritePrivateProfileInt(TEXT("LocalBluetooth"), TEXT("SPPSvcNum"), sppSvcNum+1, szBscsPath);
		sprintf(newLocation, "%s%d", "LocalSPPService", sppSvcNum);
		sprintf(comTemp, "%s%d", "COM", comportNumber);
		sprintf(nameTemp, "%s", LOCAL_SPP_SERVICE_NAME);
		Btsdk_WritePrivateProfileInt(newLocation, TEXT("Class"), BTSDK_CLS_SERIAL_PORT, szBscsPath);	
		Btsdk_WritePrivateProfileString(newLocation, TEXT("VirtualDevice"), comTemp, szBscsPath);
		Btsdk_WritePrivateProfileString(newLocation, TEXT("Name"), nameTemp, szBscsPath);
	}
	else
	{
		int i;
		if (sppSvcNum > BTSDK_DEFAULT_SPPSVC_NUMBER)
		{
			for (i=BTSDK_DEFAULT_SPPSVC_NUMBER; i<sppSvcNum; i++)
			{
				sprintf(sectionName, TEXT("LocalSPPService%d"), i);
				Btsdk_GetPrivateProfileString(sectionName, TEXT("VirtualDevice"),
					TEXT(""), comName, 20, szBscsPath);
				if (comportNumber == atoi(&comName[3]))
				{
					char sectionSrc[20] = {0};
					char sectionTarget[20] = {0};
					char comTemp[20] = {0};
					char nameTemp[BTSDK_SERVICENAME_MAXLENGTH] = {0};
					int j = 0;
					Btsdk_WritePrivateProfileString(sectionName, NULL, TEXT(""), szBscsPath);
					for (j=i; j<(sppSvcNum-1); j++)
					{
						sprintf(sectionSrc, TEXT("LocalSPPService%d"), j+1);
						sprintf(sectionTarget, TEXT("LocalSPPService%d"), j);
						Btsdk_GetPrivateProfileString(sectionSrc, TEXT("VirtualDevice"), TEXT(""), comTemp, 20, szBscsPath);
						Btsdk_WritePrivateProfileString(sectionTarget, TEXT("VirtualDevice"), comTemp, szBscsPath);
						Btsdk_GetPrivateProfileString(sectionSrc, TEXT("Name"), TEXT(""), nameTemp, BTSDK_SERVICENAME_MAXLENGTH, szBscsPath);
						Btsdk_WritePrivateProfileString(sectionTarget, TEXT("Name"), nameTemp, szBscsPath);
						Btsdk_WritePrivateProfileInt(sectionTarget, TEXT("Class"), BTSDK_CLS_SERIAL_PORT, szBscsPath);
					}
					sprintf(sectionName, TEXT("LocalSPPService%d"), j);
					Btsdk_WritePrivateProfileString(sectionName, NULL, TEXT(""), szBscsPath);
					break;
				}	
			}
			Btsdk_WritePrivateProfileInt(TEXT("LocalBluetooth"),TEXT("SPPSvcNum"), sppSvcNum-1, szBscsPath);
		}
	}
}
#define BSCSINIFILE TEXT("bscs.ini")
void GetCommonSettingFilePath(TCHAR *pszcsFilePath, UINT uiStrLen)
{
	DWORD wowLen;
	if (NULL == pszcsFilePath || 0 == uiStrLen)
	{
		return;
	}
	GetWindowsDirectory(pszcsFilePath, uiStrLen);
	wowLen = strlen(pszcsFilePath);
	if (wowLen > 0 && wowLen < MAX_PATH)
	{
		if (IsX64Platform())
		{		
			strcat(pszcsFilePath, TEXT("\\SysWOW64"));
		}
		else
		{
			strcat(pszcsFilePath, TEXT("\\System32"));
		}
	}	
	strcat(pszcsFilePath, TEXT("\\"));
	strcat(pszcsFilePath, BSCSINIFILE);
}

BOOL IsX64Platform()
{
	BOOL bIsX64 = FALSE;
#ifdef _WIN64
	bIsX64 =  TRUE;
#else
	bIsX64 = FALSE;
#endif
	return bIsX64;
}

void DeleteTheSPPServiceByName()
{
	BTSVCHDL svc_hdl = BTSDK_INVALID_HANDLE;
	//1 First enum the service
	BOOL bFound = FALSE;
	BTSDKHANDLE enum_handle = BTSDK_INVALID_HANDLE;
	BtSdkLocalServerAttrStru attribute = {0};
	INT comport;
	attribute.mask = BTSDK_LSAM_SERVICENAME;
	enum_handle = Btsdk_StartEnumLocalServer();
	if (enum_handle != BTSDK_INVALID_HANDLE)
	{
		while(BTSDK_INVALID_HANDLE != (svc_hdl = Btsdk_EnumLocalServer(enum_handle, &attribute)))
		{
			if (attribute.service_class == BTSDK_CLS_SERIAL_PORT && !stricmp(attribute.svc_name, LOCAL_SPP_SERVICE_NAME))
			{
				bFound = TRUE;
				s_CurrLocalSppSvcHdl = svc_hdl;
				break;
			}
		}
	}
	//2 if found the service then do the delete operation
	if (bFound)
	{
		int serialNum;
		//2.1 First the the comport assigned to the service
		memset(&attribute, 0, sizeof(BtSdkLocalServerAttrStru));
		attribute.mask = BTSDK_LSAM_EXTATTRIBUTES;
		Btsdk_GetServerAttributes(s_CurrLocalSppSvcHdl, &attribute);
		comport = ((PBtSdkLocalSPPServerAttrStru)(attribute.ext_attributes))->com_index;
		Btsdk_FreeMemory(attribute.ext_attributes);
		//2.2 Stop the service
		Btsdk_StopServer(s_CurrLocalSppSvcHdl);	
		//2.3 remote the service
		Btsdk_RemoveServer(s_CurrLocalSppSvcHdl);
		//2.4 Delete the comport
		Btsdk_DeinitCommObj((BTUINT8)comport);
		serialNum = Btsdk_CommNumToSerialNum(comport);
		Btsdk_PlugOutVComm(serialNum, COMM_SET_RECORD);
		//2.5 Sync the ini file
		SyncIniFileInfo(FALSE, comport);
	}
	s_CurrLocalSppSvcHdl = BTSDK_INVALID_HANDLE;
}

void SetSecureLevelForLocalService()
{
	BTSVCHDL svc_hdl = BTSDK_INVALID_HANDLE;
	BTSDKHANDLE enum_handle = BTSDK_INVALID_HANDLE;
	BtSdkLocalServerAttrStru attribute = {0};
	attribute.mask = BTSDK_LSAM_SERVICENAME;
	enum_handle = Btsdk_StartEnumLocalServer();
	if (enum_handle != BTSDK_INVALID_HANDLE)
	{
		while(BTSDK_INVALID_HANDLE != (svc_hdl = Btsdk_EnumLocalServer(enum_handle, &attribute)))
		{
			if (attribute.service_class == BTSDK_CLS_SERIAL_PORT && !stricmp(attribute.svc_name, LOCAL_SPP_SERVICE_NAME))
			{
				s_CurrLocalSppSvcHdl = svc_hdl;
				break;
			}
		}
	}
	if (s_CurrLocalSppSvcHdl != BTSDK_INVALID_HANDLE)
	{
		Btsdk_SetServiceSecurityLevel(s_CurrLocalSppSvcHdl, BTSDK_SSL_NO_SECURITY);//No secure or any other value
	}
}

void SetFixPinCodeForRemoteDevice()
{
	TCHAR tcBscsPath[MAX_PATH] = {0};
	TCHAR tcFixedPincode[32] = TEXT("0000");
	GetCommonSettingFilePath(tcBscsPath, MAX_PATH);
	Btsdk_WritePrivateProfileString(TEXT("FixedPincode"), TEXT("DefaultPincode"), tcFixedPincode, tcBscsPath);
}

void GetSPPServiceInfo()
{
	BTSVCHDL svc_hdl = BTSDK_INVALID_HANDLE;
	BTSDKHANDLE enum_handle = BTSDK_INVALID_HANDLE;
	BtSdkLocalServerAttrStru attribute = {0};
	BTUINT16 status;
	attribute.mask = BTSDK_LSAM_SERVICENAME;
	enum_handle = Btsdk_StartEnumLocalServer();
	if (enum_handle != BTSDK_INVALID_HANDLE)
	{
		while(BTSDK_INVALID_HANDLE != (svc_hdl = Btsdk_EnumLocalServer(enum_handle, &attribute)))
		{
			if (attribute.service_class == BTSDK_CLS_SERIAL_PORT && !stricmp(attribute.svc_name, LOCAL_SPP_SERVICE_NAME))
			{
				s_CurrLocalSppSvcHdl = svc_hdl;
				break;
			}
		}
	}

	if (s_CurrLocalSppSvcHdl != BTSDK_INVALID_HANDLE)
	{
		memset(&attribute, 0, sizeof(BtSdkLocalServerAttrStru));
		attribute.mask =  BTSDK_LSAM_SERVICENAME | BTSDK_LSAM_EXTATTRIBUTES | BTSDK_LSAM_AUTHORMETHOD |BTSDK_LSAM_SECURITYLEVEL;
		//With this function, you can get the service's attribute
		Btsdk_GetServerAttributes(s_CurrLocalSppSvcHdl, &attribute);
		printf("comport is %d\n", ((PBtSdkLocalSPPServerAttrStru)attribute.ext_attributes)->com_index);
		Btsdk_FreeMemory(attribute.ext_attributes);
		//With this function, you can get the status
		//it can be 
		/*
		#define BTSDK_SERVER_STARTED					0x01
		#define BTSDK_SERVER_STOPPED					0x02
		#define BTSDK_SERVER_CONNECTED					0x03
		*/
		Btsdk_GetServerStatus(s_CurrLocalSppSvcHdl, &status);
	}
}

void ChangeCOMPortForLocalSPPService()
{
	BTSVCHDL svc_hdl = BTSDK_INVALID_HANDLE;
	BTSDKHANDLE enum_handle = BTSDK_INVALID_HANDLE;
	BtSdkLocalServerAttrStru attribute = {0};
	BtSdkLocalSPPServerAttrStru sppAttri = {0};
	
	int old_comport;
	int new_comport;
	attribute.mask = BTSDK_LSAM_SERVICENAME;
	enum_handle = Btsdk_StartEnumLocalServer();
	if (enum_handle != BTSDK_INVALID_HANDLE)
	{
		while(BTSDK_INVALID_HANDLE != (svc_hdl = Btsdk_EnumLocalServer(enum_handle, &attribute)))
		{
			if (attribute.service_class == BTSDK_CLS_SERIAL_PORT && !stricmp(attribute.svc_name, LOCAL_SPP_SERVICE_NAME))
			{
				s_CurrLocalSppSvcHdl = svc_hdl;
				break;
			}
		}
	}

	if (s_CurrLocalSppSvcHdl != BTSDK_INVALID_HANDLE)
	{
		INT serialNum;
		int oldSerialNum;
		memset(&attribute, 0 ,sizeof(BtSdkLocalServerAttrStru));
		attribute.mask = BTSDK_LSAM_EXTATTRIBUTES | BTSDK_LSAM_SERVICENAME | BTSDK_LSAM_SECURITYLEVEL;
		Btsdk_GetServerAttributes(s_CurrLocalSppSvcHdl, &attribute);
		Btsdk_StopServer(s_CurrLocalSppSvcHdl);
		old_comport = ((PBtSdkLocalSPPServerAttrStru)(attribute.ext_attributes))->com_index;//Save the old com number
		Btsdk_FreeMemory(attribute.ext_attributes);
		serialNum = Btsdk_GetASerialNum();
		if (Btsdk_PlugInVComm(serialNum, (ULONG*)&new_comport, 1, COMM_SET_RECORD|COMM_SET_USAGETYPE, 5000))
		{
			Btsdk_InitCommObj((BTUINT8)new_comport, BTSDK_CLS_SERIAL_PORT);
		}
		sppAttri.com_index = new_comport;
		sppAttri.mask = BTSDK_LSPPSAM_COMINDEX;
		sppAttri.size = sizeof(BtSdkLocalSPPServerAttrStru);
		attribute.ext_attributes = &sppAttri;
		Btsdk_UpdateServerAttributes(s_CurrLocalSppSvcHdl, &attribute);
		Btsdk_StartServer(s_CurrLocalSppSvcHdl);
		//Release the old comport
		oldSerialNum = Btsdk_CommNumToSerialNum(old_comport);
		Btsdk_DeinitCommObj((BTUINT8)old_comport);
		Btsdk_PlugOutVComm(oldSerialNum, COMM_SET_RECORD);
		//Sync ini
		SyncIniFileInfo(FALSE, old_comport);
		SyncIniFileInfo(TRUE, new_comport);
	}	
}
