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
#include "resource.h"

// Global Variables:
CHAR g_ConfigFilePath[MAX_PATH];

HINSTANCE			g_hInst;			// The current instance
HWND                g_hWnd;
HWND				g_hwndCB;			// The command bar handle


HANDLE g_hExcCmdThread = NULL;
HANDLE g_hSdkExcCmdEvt;
HANDLE g_hFuncExcCmdEvt;
LONG g_NumberLevel = 0;
int g_iNum;
CHAR g_cExcCmd;

HANDLE g_h3LevelCmdEvent = NULL;


//////////////////////////////////////////////////////////////////////////////////////////////////////////
typedef enum
{	
	MSG_DRV2UI_OPENING,          // From serial driver to BT UI, notifying that ComPort being opened.       
	MSG_DRV2UI_CLOSED,           // From serial driver to BT UI, notifying that ComPort closed.
	MSG_UI2DRV_OPENING_RSP_OK,   // From BT UI to serial driver, indicating that ComPort allowed to open.
	MSG_UI2DRV_OPENING_RSP_FAIL, // From BT UI to serial driver, indicating that ComPort NOT allowed to open.
} 
BT_DEV_AID_MSG_CODE;

typedef struct
{
	BT_DEV_AID_MSG_CODE		msgCode;		// Message
	DWORD				    dwAuxiliary;	// Additional data, up to specific usage.
} BT_DEV_AID_MSG, *PBT_DEV_AID_MSG;

static UINT g_bExit			= 0;
HANDLE		g_hBTDevAidMgr  = INVALID_HANDLE_VALUE;
//////////////////////////////////////////////////////////////////////////////////////////////////////////

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is trace the log data by several method. 
Arguments:
unsigned int length
BTUINT8 *data
Return:
void 
---------------------------------------------------------------------------*/
void TraceData(unsigned int length, BTUINT8 *data)
{

	RETAILMSG(1, (TEXT("%a"), data));
/*
	FILE *file = fopen(g_pStackDebugLog, "a+t");
	fprintf(file, "Inst %08X >>> %s", g_hInst, data);
	fflush(file);
	fclose(file);
/**/	
}



/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is initial the debug setting.
Arguments:
void
Return:
void 
---------------------------------------------------------------------------*/
void InitDebugSetting(HINSTANCE hInst)
{
	TCHAR szVersion[64] = {0};

#ifdef CONFIG_DEBUG
// 	SetDbgPrint(TraceData);
// 	dbg_flags = 0;
// 	dbg_flags |= 0; // DBG_TRACE_ON | DBG_DATA_ON;// ;// | DBG_TIME_ON	
// 	
// 	LoadString(hInst, IDS_GUI_VERSION, szVersion, 64);
// 	PRINTMSG(1, "#### IVT SDK Sample %S compiling in %S ####\r\n", szVersion, TEXT(__DATE__));

#endif
}

void PRINTMSG(int dwConn,char * pszFormat, ...)
{
	va_list args;
	CHAR szBuffer[1024] = {0};
	CHAR *pBuffer = szBuffer;
	int nBuf = 0;
//	SYSTEMTIME  systime;
	
	if ( dwConn == 0)
	{
		return;
	}

	//if (FLAG_LOGSYSTIME == (g_dwFlag & FLAG_LOGSYSTIME))
	{//Log system time
	//	GetLocalTime(&systime); 
	//	sprintf(pBuffer, "%02d:%02d:%02d:%03d  ", 
	//					systime.wHour,
	//					systime.wMinute, 
	//					systime.wSecond,
	//					systime.wMilliseconds);
	//	pBuffer += 14;
	//	nBuf += 14;
	}
	
	va_start(args, pszFormat);
	nBuf += vsprintf(pBuffer, pszFormat, args);
	va_end(args); 
	TraceData(nBuf, (unsigned char *)szBuffer);	


	LogEditPrint(szBuffer);
}

void LogEditPrint(char *sCont)
{
	static TCHAR LogDisplayBuf[1024];	
	TCHAR		 szBuf[128];
	int          nPosEnter;
	int          nPosReturn;

	
	memset(szBuf, 0x0, 128*sizeof(TCHAR));
	MultiByteToWideChar(CP_ACP, 0,(const char *)sCont,-1,szBuf,128);
	wcscat(LogDisplayBuf, szBuf);
	
	if ((0 ==wcscspn(L"\r", LogDisplayBuf) )||(0 ==wcscspn(L"\n", LogDisplayBuf) ))
	{	
		nPosReturn = wcscspn(LogDisplayBuf, L"\r");		
		nPosEnter  = wcscspn(LogDisplayBuf, L"\n");

		if (nPosReturn)
		{
			memset(LogDisplayBuf+nPosReturn, 0x0, sizeof(TCHAR));
		}

		if (nPosEnter)
		{
			memset(LogDisplayBuf+nPosEnter, 0x0, sizeof(TCHAR));
		}

		SendDlgItemMessage (g_hWnd, IDC_LOG_EDIT, LB_ADDSTRING, 0,(LPARAM)(LPCTSTR)LogDisplayBuf);   
		SendDlgItemMessage (g_hWnd, IDC_LOG_EDIT, WM_VSCROLL, SB_PAGEDOWN, 0);		

		memset(LogDisplayBuf, 0x0, 1024*sizeof(TCHAR));
	}
}

BOOL GetExcCmd(int *Num, CHAR *cCmd)
{
	TCHAR ExcEdit[MAX_PATH];
	HWND hExcEditWnd = GetDlgItem(g_hWnd,IDC_EXCCMD_EDIT);
	
	GetWindowText(/*g_hExcEditWnd*/hExcEditWnd, ExcEdit, MAX_PATH);
	PRINTMSG(1, "ExcEdit(%S)\r\n", ExcEdit);
	if(Num)
	{
		*Num = _ttoi(ExcEdit);
	}
	if(cCmd)
	{
		*cCmd = (UCHAR)ExcEdit[0];
	}
	SetWindowText(hExcEditWnd, _T(""));
	SetFocus(hExcEditWnd);
	
	return TRUE;
}


void SdkTestShowMenu()
{
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, "*         BTSDK Testing Menu            *\n");
	PRINTMSG(1, "* <1) Local Device Manager              *\n");
	PRINTMSG(1, "* <2) Remote Device Manager             *\n");
	PRINTMSG(1, "* <3) Security Manager                  *\n");
	PRINTMSG(1, "* <4) Start Service                     *\n");
	PRINTMSG(1, "* <5) Browse Service                    *\n");
	PRINTMSG(1, "* <7) HFP Manager                       *\n");
	PRINTMSG(1, "* <8) FTP Manager                       *\n");
	PRINTMSG(1, "* <9) OPP Manager                       *\n");
	PRINTMSG(1, "* <e) PAN Manager                        *\n");
	PRINTMSG(1, "* <f) DUN Manager                        *\n");
	PRINTMSG(1, "* <g) AV  Manager                        *\n");
	PRINTMSG(1, "* <p) PIM Manager                        *\n");
	PRINTMSG(1, "* <q> Quit                              *\n");
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, ">");
}

void SExecCmd(BTUINT8 choice)
{
	switch (choice) 
	{
		case '1':
			TestLocDevMgr();
			break;
		case '2':
			TestRmtDevMgr();
			break;
		case '3':
			TestSecMgr();
			break;
		case '4':
			TestSvcRegMgr();
			break;
		case '5':
			TestSvcSch();
			break;
		case '7':
			TestHfp();
			break;
		case '8':
			TestFTPFunc();
			break;
		case '9':
			TestOPPFunc();
			break;
		case 'e':
			TestPANFunc();
			break;
		case 'f':
			DUNManager();
			break;
		case 'g':
			TestAVFunc();
			break;
		case 'm':
			SdkTestShowMenu();
			break;
		case 'p':
			TestPIMMgr();
			break;
		case 'q':
			break;
		default:
			PRINTMSG(1, "Invalid command.\n");
			break;
	}
}

void SetLocalName(void)
{
	WSADATA wsadata;
	DWORD lerror;
	char host_name[MAX_PATH], dev_name[MAX_PATH];

	PRINTMSG(1, "+ SetLocalName\r\n");
	strcpy(dev_name, "BTSDK");
	lerror = WSAStartup(0x202, &wsadata);
	if (lerror == 0)
	{
		if (gethostname((char FAR *)host_name , 50) == 0)
		{
			strcat(dev_name, "-");
			strcat(dev_name, host_name);
		}
		WSACleanup();
	}
	Btsdk_SetLocalName((BTUINT8 *)dev_name, (BTUINT16)(strlen(dev_name) + 1));
	PRINTMSG(1, "- SetLocalName\r\n");
}

DWORD WINAPI AppSimuDeviceRemoved(void *lParam) // Simulating Unplug device.
{
	Sleep(5000);
	Btsdk_BTDeviceUnplug();
	return 1;
}

BOOL StartBSServer()
{
#ifndef USINGLIBVERSION
	// Start BSServer for CESDK.
	//Do not call CreateProcess from a DllMain function.
	//This causes the application to stop responding.(in MSDN document)
	//and i had tried to put those code in 	MiddleWareInit(), but it startup BSServer.exe.
	//PROCESS_INFORMATION pi = {0};
	//BOOL  bRet = 0;

	//bRet = CreateProcess(TEXT("\\NandFLASH\\20090324\\Bluesoleil\\BSServerD.exe"),0,NULL,NULL,0,0,NULL,NULL,NULL,&pi);

	//if (bRet != 0)
	//{
	//	CloseHandle(pi.hThread);
	//	CloseHandle(pi.hProcess);
	//}

	//Sleep(4000);

//#define FIRST_CLIENT_ID		1
//#define SECOND_CLIENT_ID	2
//CESDK_API  long MiddleWareInitWithClientID(UINT Client_ID);

	MiddleWareInit();//default Client_ID = FIRST_CLIENT_ID
	//MiddleWareInitWithClientID(SECOND_CLIENT_ID);

#endif

	return 0;
}

BOOL StopBSServer()
{
#ifndef USINGLIBVERSION
	//// Close BSServer for CESDK.
	//i had tried to terminal the exe by CloseHandle(pi.hThread);CloseHandle(pi.hProcess),then pi was a global var),it doesn't work;
	//and by TerminateProcess(pi.hProcess), (then pi was a global var),it doesn't work;
	BOOL bRet = 0;
	HWND hBSServer = NULL;

	MiddleWareUnInit();

	//hBSServer = FindWindow( L"CEBase30 Server", L"BSServer" );
	//if (hBSServer != NULL)
	//{
	//	bRet = 	PostMessage(hBSServer,WM_CLOSE,0,0);
	//}
#endif
	return 0;
}



///////////////////////////////////////////////////////////////////////////////
// [Arguments:]
//		dwComIndex		[in] COM index.
//		msgCode			[in] Message code.
////////////////////////////////////////////////////////////////////////////////
void SPPBTDevAidMsgHandler(HANDLE hBTDevAidMgr, DWORD dwComIndex, BT_DEV_AID_MSG_CODE msgCode)
{	
	BT_DEV_AID_MSG msgUI2Drv;
	DWORD dwNum = 0;

	msgUI2Drv.dwAuxiliary = dwComIndex;

	switch (msgCode)
	{
	case MSG_DRV2UI_OPENING:
		msgUI2Drv.msgCode = MSG_UI2DRV_OPENING_RSP_OK;
		PRINTMSG(1, "SPPBTDevAidMsgHandler: MSG_DRV2UI_OPENING received!\r\n");
		break;
	case MSG_DRV2UI_CLOSED:
		PRINTMSG(1, "SPPBTDevAidMsgHandler: MSG_DRV2UI_CLOSED received!\r\n");
		return;
	default:
		PRINTMSG(1, "SPPBTDevAidMsgHandler: msgCode=%d received!\r\n", msgCode);
		return;
	}
	
	WriteFile(hBTDevAidMgr,(LPVOID)(&msgUI2Drv), sizeof(BT_DEV_AID_MSG), &dwNum, NULL);
}

static DWORD WINAPI MsgReceiver(LPVOID lpParameter)
{
	HANDLE  hBTDevAidMgr = (HANDLE)lpParameter;

	DWORD			dwNum;
	BT_DEV_AID_MSG  msg ;

	while ( !g_bExit)
	{
		BOOL bRet = ReadFile( hBTDevAidMgr, (LPVOID)&msg, sizeof(BT_DEV_AID_MSG), &dwNum, NULL );
		if ( bRet == FALSE || dwNum == 0 ) 
		{
			break;
		}
		SPPBTDevAidMsgHandler(hBTDevAidMgr, msg.dwAuxiliary,  msg.msgCode);
	}
	CloseHandle( hBTDevAidMgr);
	hBTDevAidMgr = INVALID_HANDLE_VALUE;

	return 0;
}

////////////////////////////////////////////////////////////////////////////////
//just for set up dun connection. to communicate with the ce device driver.
//please just do like this.
// [Return Value:]
//    TRUE:   Succeed,  FALUSE:	Failed.
////////////////////////////////////////////////////////////////////////////////
HANDLE BTDevAidMgrInit( )
{
	DWORD     id;
	HANDLE hThread = NULL;
	TCHAR szDevName[12] = TEXT("BMG4:");

	HANDLE hBTDevAidMgr = CreateFile( szDevName, GENERIC_READ|GENERIC_WRITE, 
		FILE_SHARE_READ|FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL); 

	if (INVALID_HANDLE_VALUE == hBTDevAidMgr )
	{
		return INVALID_HANDLE_VALUE;
	}

	g_hBTDevAidMgr = hBTDevAidMgr;
	g_bExit = 0;
	hThread = CreateThread(NULL, 0, MsgReceiver, (LPVOID)hBTDevAidMgr, 0, &id);
	if ( hThread == NULL )
	{
		CloseHandle(hBTDevAidMgr);	
		return INVALID_HANDLE_VALUE;
	}
	CloseHandle(hThread);
	return hBTDevAidMgr;
}

/////////////////////////////////////////////////////////////
BOOL BTDevAidMgrDeinit( void )
{
	g_bExit = 1;
	if ( g_hBTDevAidMgr != INVALID_HANDLE_VALUE )
	{
		CloseHandle(g_hBTDevAidMgr);
	}
	g_hBTDevAidMgr =INVALID_HANDLE_VALUE;
	return TRUE;
}



/***   ***/
BOOL InitSdk(HINSTANCE hInst)
{
	BTUINT8 ch = 0;

	TCHAR szTmp[MAX_PATH] = {0};
	ULONG  thread_id = 0;
	
	BOOL BtRadioStatus = 0;

	PRINTMSG(1, "+ InitSdk, hInst(0x%x)\r\n", hInst);
	//InitDebugSetting(hInst);
	
	g_hSdkExcCmdEvt = CreateEvent (NULL, FALSE, FALSE, NULL);
	g_hFuncExcCmdEvt = CreateEvent (NULL, FALSE, FALSE, NULL);
	g_h3LevelCmdEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

	StartBSServer();

#ifdef USINGLIBVERSION
	tl_ini_name = g_ConfigFilePath;
#endif

	PRINTMSG(1, "SDK Intializing....\r\n");

	//SetMemLeakFlag();
	//	dbg_flags = 0;//DBG_TRACE_ON | DBG_DATA_ON | DBG_WARNING_ON;
	//	dbg_flags = DBG_TRACE_ON | (1 << PROT_FTP) | (1<<PROT_GOEP);


	GetModuleFileName( NULL, szTmp, MAX_PATH);
	*(_tcsrchr(szTmp, _T('\\'))) = 0;
	WideCharToMultiByte(CP_ACP, 0, szTmp, MAX_PATH, g_ConfigFilePath, MAX_PATH, NULL, NULL);
	strcat(g_ConfigFilePath, "\\config\\bttl.ini");
	//Btsdk_SetSaveFileRootDir(g_ConfigFilePath);
	PRINTMSG(1, "g_ConfigFilePath = %s....\r\n", g_ConfigFilePath);

#ifdef USINGLIBVERSION
	InitWinCEStack();
#endif
	Btsdk_Init();

	if (Btsdk_ResetHardware() == BTSDK_OK)
	{
		SetLocalName();
//		Btsdk_SetLocalClassDevice(BTSDK_PHONECLS_CORDLESS);
//		Btsdk_SetLocalClassDevice(BTSDK_SRVCLS_AUDIO|BTSDK_AV_HANDSFREE);
		Btsdk_SetLocalClassDevice(BTSDK_SRVCLS_RENDER|BTSDK_SRVCLS_OBJECT|BTSDK_IMAGE_PRINTER);
		RegAppIndCallback();
		HfpAppInit();
	}
	else
	{
		PRINTMSG(1, "BTSDK reset hardware fail...\r\n");
		PRINTMSG(1, "SDK Quitting....\r\n");
		Btsdk_Done();
		return FALSE;
	}

	g_hExcCmdThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)TstSdkThread, 
											   (LPVOID)NULL, 0, &thread_id);
	PRINTMSG(1, "- InitSdk, g_hExcCmdThread(0x%x)\r\n", g_hExcCmdThread);

	BTDevAidMgrInit();

	return TRUE;
}

BOOL DeinitSdk(void)
{
	g_cExcCmd = 'q';
	SetEvent(g_hFuncExcCmdEvt);
	g_cExcCmd = 'q';
	SetEvent(g_hSdkExcCmdEvt);	
	g_cExcCmd = 'q';
	SetEvent(g_h3LevelCmdEvent);
	
	UnRegAppIndCallback();
	HfpAppDone();
	RemoveAllServer();

	BTDevAidMgrDeinit();

	Btsdk_Done();

#ifdef USINGLIBVERSION
	DoneWinCEStack();
#endif

	StopBSServer();


	CloseHandle(g_hSdkExcCmdEvt);
	CloseHandle(g_hFuncExcCmdEvt);
	CloseHandle(g_h3LevelCmdEvent);

	return TRUE;
}

int TstSdkThread(PVOID pParam)
{
	BTUINT8 ch = 0;
	
	
	while (ch != 'q')
	{
		SdkTestShowMenu();
		WaitForSingleObject (g_hSdkExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TstSdkThread:ExcCmdThread: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd, g_NumberLevel);
		
		if(ch != 'q')
			InterlockedIncrement(&g_NumberLevel);
		SExecCmd(ch);		
	}
	return 0;
}

