/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2005 IVT Corporation
*
* All rights reserved.
* 
---------------------------------------------------------------------------*/

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
* Module Name:
              sdk_tst.h
* Usage:

* Author:
              Guan Tengfei and Hu Yi
* Revision History:
              2007-3-12 15:33      
---------------------------------------------------------------------------*/

#ifndef _SDK_TST_H
#define _SDK_TST_H

#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <conio.h>

#include "Btsdk_ui.h"


#define FIXPINCODEVAL "1"

#define MAX_DEV_NUM			100
#define MAX_LOC_SVC_NUM      32
#define MAX_SERVICENAME_LENGTH 32
#define BD_ADDR_LEN				6

void RegAppIndCallback(void);
void UnRegAppIndCallback(void);

void DisplayRemoteDevices(BTUINT32 dev_class);
BTDEVHDL SelectRemoteDevice(BTUINT32 dev_class);
void StartSearchDevice(BTUINT32 device_class);
void GetRmtDevHdl(BTUINT32 ulDevCls);
void PrintBdAddr(BTUINT8 *bd_addr);
int MultibyteToMultibyte(BTUINT32 dwSrcCodePage, char *lpSrcStr, int cbSrcStr,
						 BTUINT32 dwDestCodePage, char *lpDestStr, int cbDestStr);

BTINT32 BrowseService(BTDEVHDL hDevHdl);
BTSVCHDL SelectRemoteService(BTDEVHDL hDevHdl);

void HfpInit(void);
void HfpDone(void);
void TestLocDevMgr(void);
void TestRmtDevMgr(void);
void TestRmtSvcMgr(void);

/*functions for local service*/
void TestLocSvcMgr(void);
BOOL ServiceWhetherStopped(BTSVCHDL hLocSvcHandle);
BTSVCHDL SelectLocService(void);
BOOL StoreAStoppedSvcHandle(BTSVCHDL hLocSvcHdl);
BOOL DeleteLocalSvcHdlFromList(BTSVCHDL hLocalSvcHdl);
void ReceiveBluetoothStatusInfo(ULONG usMsgType, ULONG pulData, ULONG param, BTUINT8 *arg);

void TestShcMgr(void);
void TestHfpFunc(void);
void TestFTPFunc(void);
void TestOPPFunc(void);
BTSVCHDL SelectLocService(void);

void OppServerReceiveFileInd(BTUINT8 *pFilePathName);

void PrintErrorMessage(BTUINT32 err_code, BTUINT8 is_ptr);
#endif
