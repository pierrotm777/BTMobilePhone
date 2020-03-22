/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2007 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    profiles_tst.h
Abstract:
	Header file of all profiles
Revision History:
2007-3-21   Huyi  Created


---------------------------------------------------------------------------*/

#ifndef _PROFILES_TST_H
#define _PROFILES_TST_H

#include "sdk_tst.h"

void TestProfiles(void);

/* test whether the expected local service exists or not. */
BTSVCHDL TestWhetherExistLocalServer(BTUINT16 wSvcCls);
/* Get information of the connection. */
void GetConnectionInfo(BTDEVHDL dev_hdl);

/* A2DP relative functions */
void TestA2DPSNKService(void);
BTSVCHDL Test_RegisterA2DPSNKService(BTUINT16 len, const BTUINT8 *audio_card);
void Test_UnregisterA2DPSRCService(void);
BTSVCHDL Test_RegisterA2DPSRCService(void);
void Test_UnregisterA2DPSNKService(void);
void TestAVFunc(void);
void TestA2DPShowMenu(void);

/* AVRCP relative functions */
void Test_AVRCP_RegIndCbk(Btsdk_AVRCP_Event_Ind_Func * pfunc);
void Test_AVRCP_RegPassThrCmdCbk(Btsdk_AVRCP_PassThr_Cmd_Func *pfunc);
void TestAVRCPShowMenu(BTUINT16 svc_class);
void TestAVRCPCTFunc(BTUINT8 choiceID);
void TestAVRCPTGFunc(BTUINT8 choiceID);
/* PAN relative functions */
void TestPANShowMenu(void);
void TestPANFunc(void);

/* FTP relative functions */
void TestFTPFunc(void);

/* OPP relative functions */
void TestOPPFunc(void);

/* HFP relative functions */
void TestHfpFunc(void);

/*SPP relative functions*/
void TestSPPFunc(void);

/*HID relative functions*/
void TestHidFunc(void);

/*PBAP function*/
void TestPBAPFunc(void);

/*MAP function*/
void TestMAPFunc(void);

/*Local SPP Service function*/
void TestLocalSPPServiceFunc();

/* GATT Function test*/
void TestGATT();

/* AVRCP Function test*/
void TestAVRCPFunc();
#endif
