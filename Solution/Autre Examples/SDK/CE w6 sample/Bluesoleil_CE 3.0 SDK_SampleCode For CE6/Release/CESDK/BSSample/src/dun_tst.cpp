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
#include <ras.h>

#define MAX_REGQUERY_LENGTH		512
#define MAX_USERNAME_LENGTH		33
#define MAX_PHONENO_LENGTH		33
#define MAX_PASSWORD_LENGTH		33
#define MAX_DOMAIN_LENGTH			17
#define MAX_DIALMODE_LENGTH		115

//----------------------------------------------------------------------------------------------------
// extern global variables and data structures
//----------------------------------------------------------------------------------------------------
typedef struct _ModemConfigDataStru
{
	WORD wVersion;
	WORD wWaitBong;
	WORD wConnectFailTime;
	WORD wReserver0;
	BYTE bModemOptions;
	BYTE bReserver0;
	WORD wReserver1;
	DWORD dwBaudRate;
	WORD wTermOptions;
	BYTE bByteSize;
	BYTE bStopBits;
	BYTE bParity; 
	BYTE bReserver2;
	TCHAR szDialModifier[MAX_DIALMODE_LENGTH];
} ModemConfigDataStru, *PModemConfigDataStru;


//----------------------------------------------------------------------------------------------------
// global variables
//----------------------------------------------------------------------------------------------------
BTCONNHDL g_hDUNConnection = BTSDK_INVALID_HANDLE;
HRASCONN g_hRasConn = NULL;
const TCHAR DUN_ENTRYNAME[] = TEXT("DUN Connection");
const TCHAR	BT_MODEM_NAME[ ] = TEXT("IVT BT Modem VCOM");
const TCHAR IVT_DUN_CONNECTION[] = TEXT("\\Comm\\RasBook\\DUN Connection");
const TCHAR CTRLPANEL_DIAL[] = TEXT("ControlPanel\\Dial");
const TCHAR CTRLPANEL_DAIL_LOCATION[] = TEXT("ControlPanel\\Dial\\Locations");

//----------------------------------------------------------------------------------------------------
// forward declarations of functions included in this code module
//----------------------------------------------------------------------------------------------------
void DUNMenu(void);
void ConnectDUNDevice(void);
void DisconnectDUNDevice(void);
BTINT32 DUNDial(BTCONNHDL hConnection);
BTINT32 DUNDial_2(BTCONNHDL hConnection);
void ChangeLocationSettings(void);
DWORD ChangeMultiString(LPTSTR lpSrc);
BOOL WriteModemConfigToReg(HKEY hMainKey, LPCWSTR lpSubKey, PModemConfigDataStru pModemCfg);

//----------------------------------------------------------------------------------------------------
// DUN manager function entry
//----------------------------------------------------------------------------------------------------
void DUNManager(void)
{
	BTUINT8 cCmd = 0;
	BOOL bShowMenu = TRUE;

	DUNMenu();

	while (cCmd != 'q')
	{
		PRINTMSG(1, " \r\n <Please Tap 'M' to show 'DUN Manager Menu'> \r\n");
		PRINTMSG(1, " \r\n <Tap 'Q' to go back to 'Main Menu'> \r\n");

		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		cCmd = g_cExcCmd;
		PRINTMSG(1, "CbRingingIndication() > g_cInput: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd,g_NumberLevel);

		// parse the command
		switch (cCmd) 
		{
		case '1':
			ConnectDUNDevice();
			break;
		case '2':
			PRINTMSG(1, "DIALING,,, PLEASE WAIT ... \r\n");
			DUNDial(g_hDUNConnection);
			break;
		case '3':
			DisconnectDUNDevice();
			break;
		case 'm':
			DUNMenu();
			break;
		case 'q':
			RasHangUp(g_hRasConn);
			InterlockedDecrement(&g_NumberLevel);
			break;

		default:
			PRINTMSG(1, "DUNManager() > Invilade cCmd: %c\r\n", cCmd);
			break;
		}
	}
}

//----------------------------------------------------------------------------------------------------
// DUN menu
//----------------------------------------------------------------------------------------------------
void DUNMenu(void)
{
	PRINTMSG(1, "**************************************************\r\n");
	PRINTMSG(1, "*  <DUN Manager Menu>\r\n");
	PRINTMSG(1, "**************************************************\r\n");
	PRINTMSG(1, "* <1> Connect to DUN Device\r\n");
	PRINTMSG(1, "* <2> Dial Up to Network\r\n");
	PRINTMSG(1, "* <3> Disconnect DUN Connection\r\n");
	PRINTMSG(1, "* <M> Show DUN Manager Menu\r\n");
	PRINTMSG(1, "* <Q> Return to the 'Main Menu'\r\n");
	PRINTMSG(1, "**************************************************\r\n");
}

//----------------------------------------------------------------------------------------------------
// connect DUN device
//----------------------------------------------------------------------------------------------------
void ConnectDUNDevice(void)
{
	BTINT32 lResult = 0;
	BTDEVHDL hDevice = BTSDK_INVALID_HANDLE;

	hDevice = SelectRemoteDevice(BTSDK_DEVCLS_PHONE);
	if (hDevice != BTSDK_INVALID_HANDLE)
	{
		lResult = Btsdk_ConnectEx(hDevice, BTSDK_CLS_DIALUP_NET, 0, &g_hDUNConnection);
		if(lResult == BTSDK_OK)
		{
			PRINTMSG(1, "CONNECTION 0x%x ESTABLISHED\r\n", g_hDUNConnection);
		}
		else
		{
			PRINTMSG(1, "- Btsdk_ConnectEx() > FAILED: 0x%x\r\n", lResult);
		}
	}
	else
	{
		PRINTMSG(1, "PLEASE SELECT A REMOTE DEVICE\r\n");
	}
}

//----------------------------------------------------------------------------------------------------
// disconnect DUN device
//----------------------------------------------------------------------------------------------------
void DisconnectDUNDevice(void)
{
	BTINT32 lResult = 0;
	DWORD dwResult = 0;
	HRASCONN hRasConn= NULL;

	if (g_hDUNConnection == BTSDK_INVALID_HANDLE)
	{
		PRINTMSG(1, "NO DUN CONNECTION\r\n");
	}
	else
	{
		if (g_hRasConn != NULL)
		{
			dwResult = RasHangUp(g_hRasConn);
			if(dwResult != 0)
			{
				PRINTMSG(1, "- WINDOWS: RasHangUp() > FAILED: %d\r\n", dwResult);
			}
		}
		lResult = Btsdk_Disconnect(g_hDUNConnection);
		if(lResult == BTSDK_OK)
		{
			PRINTMSG(1, "CONNECTION 0x%x DISCONNECTED\r\n", g_hDUNConnection);
			g_hDUNConnection = BTSDK_INVALID_HANDLE;
		}
		else
		{
			PRINTMSG(1, "- Btsdk_Disconnect() > FAILED: 0x%x\r\n", lResult);
		}
	}
}

//----------------------------------------------------------------------------------------------------
// dial-up network and create/update DUN shortcut
//----------------------------------------------------------------------------------------------------
BTINT32 DUNDial(BTCONNHDL hConnection)
{
	BTINT32 lResult = BTSDK_OK;
	DWORD dwResult = 0;
	BOOL bRet = FALSE;
	ModemConfigDataStru ModemCfgData = {0};
	RASENTRY rasEntry = {0};
	RASDIALPARAMS rasDialParam = {0};
	UINT nComIndex = 0;
	CHAR tszBuf[256] = {0};	
	DWORD pb = 0;

	if (hConnection == 0)
	{
		return FALSE;
	}


	// Initialize modem line configuration to the default value.
	ModemCfgData.dwBaudRate = 19200;
	ModemCfgData.bByteSize = 8;
	ModemCfgData.wConnectFailTime = 120;
	ModemCfgData.bModemOptions = 0x10;		
	ModemCfgData.wVersion = 0x20;

	// create dial-up entry
	nComIndex = Btsdk_GetClientPort(hConnection);

	rasEntry.dwSize = sizeof(RASENTRY);
	_tcscpy(rasEntry.szLocalPhoneNumber, L"*99#");
	_tcscpy(rasEntry.szDeviceType, RASDT_Modem); 
	_stprintf(rasEntry.szDeviceName, TEXT("%s%d:"), BT_MODEM_NAME, nComIndex);
	rasEntry.dwfNetProtocols = RASNP_Ip;   		
	rasEntry.dwFramingProtocol = RASFP_Ppp;
	rasEntry.dwfOptions = RASEO_ProhibitEAP| RASEO_SwCompression|RASEO_IpHeaderCompression;//0x00400208;


	memset(tszBuf, 0x0, 256*sizeof(TCHAR));	
	WideCharToMultiByte(CP_ACP, 0, rasEntry.szDeviceName , -1, tszBuf , 256-1, NULL, NULL);
	PRINTMSG(1, "rasEntry.szDeviceName: %s\r\n", tszBuf);

	dwResult = RasValidateEntryName(NULL, DUN_ENTRYNAME);
	if ( dwResult != 0 && dwResult != ERROR_ALREADY_EXISTS  )
	{
		return dwResult;
	}

	dwResult = RasSetEntryProperties(NULL, (LPWSTR)DUN_ENTRYNAME, &rasEntry, sizeof(rasEntry), NULL, 0);
	if (dwResult != 0)
	{
		PRINTMSG(1, "- WINDOWS: RasSetEntryProperties() > FAILED: %d\r\n", dwResult);
		return dwResult;
	}

	// write the modem configuration to the registery
	bRet = WriteModemConfigToReg(HKEY_CURRENT_USER, IVT_DUN_CONNECTION, &ModemCfgData);
	if ( bRet == FALSE )
	{
		return bRet;
	}

	// change the location settings
	ChangeLocationSettings();


	// dial up
	if (g_hRasConn != NULL)
	{
		dwResult = RasHangUp(g_hRasConn);
		if(dwResult != 0)
		{
			PRINTMSG(1, "- WINDOWS: RasHangUp() > FAILED: %d\r\n", dwResult);
		}
		g_hRasConn = NULL;
	}

	rasDialParam.dwSize = sizeof(RASDIALPARAMS);
	_tcscpy(rasDialParam.szEntryName, DUN_ENTRYNAME);
	_tcscpy(rasDialParam.szPhoneNumber,  L"*99#");
	_tcscpy(rasDialParam.szUserName, L"");
	_tcscpy(rasDialParam.szPassword, L"");
	_tcscpy(rasDialParam.szDomain, L"");

	//dwResult = RasDial(NULL, NULL, &rasDialParam, 0, NULL, &g_hRasConn);
	dwResult = RasDial(NULL, NULL, &rasDialParam, 0xFFFFFFFF, GetMainWndHdl(), &g_hRasConn);
	if (dwResult != 0)
	{
		PRINTMSG(1, "- WINDOWS: RasDial() > FAILED: %d\r\n", dwResult);
		return dwResult;
	}

	return lResult;
}


//----------------------------------------------------------------------------------------------------
// change location settings
//----------------------------------------------------------------------------------------------------
void ChangeLocationSettings(void)
{
	HKEY hDialLocKey;
	DWORD dwKeyType;
	TCHAR Buffer[MAX_REGQUERY_LENGTH];
	DWORD dwDatasize = MAX_REGQUERY_LENGTH;
	DWORD dwData = 0;

	RegOpenKeyEx(HKEY_CURRENT_USER,	CTRLPANEL_DIAL, 0, 0, &hDialLocKey);	
	RegSetValueEx(hDialLocKey, TEXT("CurrentLoc"), 0, REG_DWORD, (LPBYTE)&dwData, sizeof(DWORD));
	RegCloseKey(hDialLocKey);

	RegOpenKeyEx(HKEY_CURRENT_USER, CTRLPANEL_DAIL_LOCATION, 0, 0, &hDialLocKey);
	RegQueryValueEx(hDialLocKey, TEXT("0"), 0, &dwKeyType, (LPBYTE)Buffer, &dwDatasize);
	dwDatasize = ChangeMultiString(Buffer);
	RegSetValueEx(hDialLocKey, TEXT("0"), 0, REG_MULTI_SZ, (LPBYTE)Buffer, dwDatasize);
	RegCloseKey(hDialLocKey);
}

//----------------------------------------------------------------------------------------------------
// change multi string
//----------------------------------------------------------------------------------------------------
DWORD ChangeMultiString(LPTSTR lpSrc)
{
	TCHAR szBuff[8][MAX_REGQUERY_LENGTH]= {0};
	LPTSTR lpIndex = NULL;
	int i = 0;
	int j = 0;

	lpIndex = lpSrc;
	while (*lpIndex != 0)
	{
		_tcscpy(szBuff[i], lpIndex);
		lpIndex += _tcslen(szBuff[i])+1;
		i++;
	}
	_tcscpy(szBuff[1], L"G");
	_tcscpy(szBuff[2], L"G");
	_tcscpy(szBuff[3], L"G");

	lpIndex = lpSrc;
	while (j < i)
	{
		_tcscpy(lpIndex, szBuff[j]);
		lpIndex += _tcslen(szBuff[j]) + 1;
		j++;
	}

	*lpIndex = 0;
	return ((lpIndex - lpSrc + 1) << 1);
}

//----------------------------------------------------------------------------------------------------
// write the modem configuration to the registery
//----------------------------------------------------------------------------------------------------
BOOL WriteModemConfigToReg(HKEY hMainKey, LPCWSTR lpSubKey, PModemConfigDataStru pModemCfg)
{
	DWORD dwDisp = 0;
	HKEY  hKey;

	if (RegCreateKeyEx(hMainKey, lpSubKey, 0, NULL, 0, 0, NULL, &hKey, &dwDisp) != ERROR_SUCCESS)
	{	    	
		return FALSE;
	}    
	RegSetValueEx(hKey, TEXT("DevCfg"), 0, REG_BINARY, (LPBYTE)pModemCfg, sizeof(ModemConfigDataStru));
	RegCloseKey(hKey);
	return TRUE;
}
