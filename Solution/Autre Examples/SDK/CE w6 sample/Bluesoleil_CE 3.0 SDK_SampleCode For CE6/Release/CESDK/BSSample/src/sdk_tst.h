#ifndef _SDK_TST_H
#define _SDK_TST_H

#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <Winsock2.h>


#ifdef __cplusplus
extern "C" {
#endif
//#include "Btsdk_ui.h"//PC°æused

#ifndef USINGLIBVERSION

//CE MV °æ
#include "Btsdk_ui.h"
#include "SelDlg.h"
#include "Btsdk_API.h"

#else

//CE Lib °æ
#include "global.h"
#include "sdk_ui.h"
#include "sdk_hlp.h"
#include "SelDlg.h"
#include "winCEdep.h" 

extern char *tl_ini_name;

#endif


extern HINSTANCE	g_hInst;		// The current instance
extern HWND         g_hWnd;
extern HWND			g_hwndCB;		// The command bar handle
extern BTDEVHDL     g_cur_hf_hdev;  // Device Handle    
extern BTSVCHDL     g_cur_hf_hconn; // Connection Handle

extern HANDLE g_hSdkExcCmdEvt;
extern HANDLE g_hFuncExcCmdEvt;
extern LONG g_NumberLevel;
extern int g_iNum;
extern CHAR g_cExcCmd;

// max path
#ifndef MAX_PATH
#define MAX_PATH 260
#endif
//#define FIXPINCODEVAL "0000"
#define FIXPINCODEVAL "0000"

#define MAX_DEV_NUM			32

extern BTDEVHDL g_rmt_dev_hdls[MAX_DEV_NUM];
extern BTINT32 g_rmt_dev_num;

void InitDebugSetting(HINSTANCE hInst);
void PRINTMSG(int dwConn,char * pszFormat, ...);
void LogEditPrint(char *sCont);
BOOL GetExcCmd(int *Num, CHAR *cCmd);
int TstSdkThread(PVOID pParam);

BOOL InitSdk(HINSTANCE hInst);
BOOL DeinitSdk(void);
void RegAppIndCallback(void);
void UnRegAppIndCallback(void);

void DisplayRemoteDevices(BTUINT32 dev_class);
BTDEVHDL SelectRemoteDevice(BTUINT32 dev_class);
void StartSearchDevice(BTUINT32 device_class);
void PrintBdAddr(BTUINT8 *bd_addr);
int MultibyteToMultibyte(BTUINT32 dwSrcCodePage, char *lpSrcStr, int cbSrcStr,
						 BTUINT32 dwDestCodePage, char *lpDestStr, int cbDestStr);

void HfpAppInit(void);
void HfpAppDone(void);

void TestLocDevMgr(void);
void TestRmtDevMgr(void);
void TestSecMgr(void);
void TestSvcRegMgr(void);
void TestSvcSch(void);

void TestFTPFunc(void);
void TestOPPFunc(void);
void TestHfp(void);


void TestPIMMgr(void);
void PIMTestInit(void);
void PIMTestConnect(void);
void PIMTestDisconnect(void);

/* Core routines */
void RemoveAllServer(void);

/* GAP routines */
void AppGetLocalOOBData(void);

BTCONNHDL SelectConnection(BTUINT16 svc_class);

/* General OBEX routines */
void DisconnectOBEXConnection(BTUINT16 svc_class);
BTBOOL AppOBEXServerReceiveFileInd(PBtSdkFileTransferReqStru pFileInfo);


//cjf add
void Test_Btsdk_PairDevice(BTDEVHDL dev_hdl);
extern HANDLE g_h3LevelCmdEvent;
BTDEVHDL SelectRmtService(BTDEVHDL dev_hdl);
void DUNManager(void);
void TestPANFunc(void);
void TestAVFunc(void);

void SaveMainWndHdl(HWND hWnd);
HWND GetMainWndHdl();


#ifndef USINGLIBVERSION

#define BTSDK_CHECK_RETURN_CODE(func) \
{ \
	BTUINT32 result; \
	result = (func); \
}

#endif


/* c - Complete device class; p - Device class with only interest field(s) set. */
#define IS_SAME_TYPE_DEVICE_CLASS(c, p)	((p)==0||((!((p)&MAJOR_DEVICE_CLASS_MASK)||(((p)&MAJOR_DEVICE_CLASS_MASK)==((c)&MAJOR_DEVICE_CLASS_MASK)&&(!((p)&MINOR_DEVICE_CLASS_MASK)||((p)&MINOR_DEVICE_CLASS_MASK)==((c)&MINOR_DEVICE_CLASS_MASK))))&&(!((p)&SERVICE_CLASS_MASK)||((p)&(c)&SERVICE_CLASS_MASK)==((p)&SERVICE_CLASS_MASK))))


#ifdef __cplusplus
}
#endif

#endif
