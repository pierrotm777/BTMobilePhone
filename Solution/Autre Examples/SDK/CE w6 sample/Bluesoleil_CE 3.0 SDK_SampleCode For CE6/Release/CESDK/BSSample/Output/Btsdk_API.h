/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2008 IVT Corporation
*
* All rights reserved.
* 
---------------------------------------------------------------------------*/

/////////////////////////////////////////////////////////////////////////////
// Module Name:
//     Btsdk_API.h
// Abstract:
//     This Module defines APIs used by CESDK and Client.
// Usage:
//     #include "Btsdk_API.h"
// 
// Author:    
//     chenjinfeng
// Revision History:
//     2008-8-4		Created
// 
/////////////////////////////////////////////////////////////////////////////
#ifndef _BTSDK_API_H
#define _BTSDK_API_H

#include "Btsdk_Macro.h"
#include "Btsdk_Stru.h"
#include "PIM_Macro.h"
#include "PIM_Stru.h"
#include <windows.h>

//-------------------------------------------------------------------
// you should not define CESDK_EXPORTS in client project
//-------------------------------------------------------------------
#ifdef CESDK_EXPORTS
#define CESDK_API __declspec(dllexport)
#else
#define CESDK_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

//please give detail about the point parameter!!!!

#define FIRST_CLIENT_ID		1
#define SECOND_CLIENT_ID	2

CESDK_API  long MiddleWareInitWithClientID(UINT Client_ID);
CESDK_API  long MiddleWareInit(); //FIRST_CLIENT_ID ;the default value
CESDK_API  long MiddleWareUnInit();

//-----------------------------------------------------------------------------------------------------
//for Definitions for Compatibility begin--------------------------------------------------------------
#define BTSDK_PIN_CODE_IND				BTSDK_PIN_CODE_REQ_IND

//ini
#define _BtSdkCallBackStru				_BtSdkCallbackStru
#define BtSdkCallBackStru				BtSdkCallbackStru
#define PBtSdkCallBackStru				PBtSdkCallbackStru
#define Btsdk_PowerOffBluetooth()						Btsdk_StopBluetooth()
#define Btsdk_ResetHardware()							Btsdk_StartBluetooth()
#define Btsdk_RegisterSdkCallBack(pcb)					Btsdk_RegisterCallback(pcb)

//sec
#define Btsdk_GetTrustedDeviceList(hsvc, phdevs, pcnt)	Btsdk_GetTrustedDevices(hsvc, phdevs, pcnt)
#define Btsdk_AuthorRsp(hsvc, hdev, resp)				Btsdk_AuthorizationResponse(hsvc, hdev, resp)

//svc
#define	Btsdk_GetRemoteServiceList(hdev, phsvcs, pcnt)	Btsdk_GetRemoteServices(hdev, phsvcs, pcnt)

//loc
#define Btsdk_GetLocalBDAddr(bd_addr)					Btsdk_GetLocalDeviceAddress(bd_addr)
#define Btsdk_GetLocalPinCode(PinCode, PinCodeSize)		Btsdk_GetFixedPinCode(PinCode, PinCodeSize)
#define Btsdk_GetLocalServiceList(phsvcs, pcnt)			Btsdk_GetLocalServers(phsvcs, pcnt)
#define Btsdk_SetLocalClassDevice(dev_cls)				Btsdk_SetLocalDeviceClass(dev_cls)
#define Btsdk_GetLocalClassDevice(dev_cls)	            Btsdk_GetLocalDeviceClass(dev_cls)
//rmt
#define Btsdk_GetDeviceHandle(addr)						Btsdk_GetRemoteDeviceHandle(addr)
#define Btsdk_GetInquiredRemoteDevice(phdevs, num)		Btsdk_GetInquiredDevices(phdevs, num)
#define Btsdk_GetKnownDevicesByClass(cls, phdevs, num)	Btsdk_GetStoredDevicesByClass(cls, phdevs, num)
#define Btsdk_GetRemoteDeviceBDAddr(hdev, addr)			Btsdk_GetRemoteDeviceAddress(hdev, addr)
#define Btsdk_RejAclConn(hdev)							Btsdk_RejectConnectionRequest(hdev, ER_LOCAL_HOST_TERMI_CONN, BTSDK_ACL_LINK)
#define Btsdk_SetRemoteDevicePinCode(hdev, pin, size)	Btsdk_PinCodeReply(hdev, pin, size)	
#define Btsdk_StartDiscoverDevice(cls, num, sec)		Btsdk_StartDeviceDiscovery(cls, num, sec)
#define Btsdk_StopDiscoverDevice()						Btsdk_StopDeviceDiscovery()
#define Btsdk_ActivateACLLink(dev_hdl)					Btsdk_SetLinkMode(dev_hdl, BTSDK_LPM_ACTIVE_MODE, NULL)	
#define Btsdk_EnterHoldMode(dev_hdl, param)				Btsdk_SetLinkMode(dev_hdl, BTSDK_LPM_HOLD_MODE, (BTUINT8*)param)
#define Btsdk_EnterSniffMode(dev_hdl, param)			Btsdk_SetLinkMode(dev_hdl, BTSDK_LPM_SNIFF_MODE, (BTUINT8*)param)
#define Btsdk_EnterParkMode(dev_hdl, param)				Btsdk_SetLinkMode(dev_hdl, BTSDK_LPM_PARK_MODE, (BTUINT8*)param)

//shc
#define Btsdk_StartShortCutEx(hshc, param, phconn)		Btsdk_ConnectShortCutEx(hshc, param, phconn)

//conn
#define Btsdk_DisconnectConnection(hconn)				Btsdk_Disconnect(hconn)
#define Btsdk_StartClient(hsvc, param, phconn)			Btsdk_Connect(hsvc, param, phconn)
#define Btsdk_StartClientEx(hdev, svc_cls, lParam, phconn)	Btsdk_ConnectEx(hdev, svc_cls, lParam, phconn)
//for Definitions for Compatibility end----------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//ini begin--------------------------------------------------------------------------------------------
CESDK_API void *Btsdk_MallocMemory(/* in */ BTUINT32 size);
CESDK_API void Btsdk_FreeMemory(/* in */ void *memblock);

CESDK_API BTBOOL Btsdk_IsBluetoothReady(void);
CESDK_API BTBOOL Btsdk_IsSDKInitialized(void);
CESDK_API BTUINT32 Btsdk_GetVersionString( /* out */BTUINT8 *pver_str, /* in */ BTUINT32 length);//the version string.(c string)

CESDK_API BTINT32 Btsdk_Init(void);
CESDK_API BTINT32 Btsdk_InitializeLocalDevice(void);
CESDK_API BTINT32 Btsdk_PowerOnBluetooth(void);
CESDK_API BTINT32 Btsdk_StartBluetooth(void);
CESDK_API BTINT32 Btsdk_StopBluetooth(void);
CESDK_API BTINT32 Btsdk_Done(void);
CESDK_API BTINT32 Btsdk_SetSaveFileRootDir(/* in */ BTUINT8 *pFilePath);

// Callback Function 
typedef void    (Btsdk_Pin_Req_Ind_Func)             ( /* in */ BTDEVHDL dev_hdl );
typedef void    (Btsdk_Link_Key_Req_Ind_Func)        ( /* in */ BTDEVHDL dev_hdl );
typedef void    (Btsdk_Link_Key_Notif_Ind_Func)      ( /* in */ BTDEVHDL dev_hdl, /* in */ BTUINT8 *link_key);//link_key[BTSDK_LINK_KEY_LENGTH=16] not string
typedef void    (Btsdk_Authentication_Fail_Ind_Func) ( /* in */ BTDEVHDL dev_hdl);
typedef void    (Btsdk_Inquiry_Result_Ind_Func)      ( /* in */ BTDEVHDL dev_hdl);
typedef void    (Btsdk_Inquiry_Complete_Ind_Func)    ( void );
typedef void    (Btsdk_Authorization_Req_Ind_Func)   ( /* in */ BTSVCHDL svc_hdl, /* in */ BTDEVHDL dev_hdl );
typedef BTUINT8 (Btsdk_Connection_Request_Ind_Func)  ( /* in */ BTDEVHDL dev_hdl, /* in */ BTUINT32 dev_class, /* in */ BTUINT8 link_type );
typedef void    (Btsdk_Connection_Complete_Ind_Func) ( /* in */ BTDEVHDL dev_hdl );
typedef void    (Btsdk_Connection_Event_Ind_Func)    ( /* in */ BTCONNHDL conn_hdl, /* in */ BTUINT16 event, /* in */ BTUINT8 *arg );//arg = PBtSdkConnectionPropertyStru
typedef void    (Btsdk_Shortcut_Event_Ind_Func)      ( /* in */ BTSHCHDL shc_hdl, /* in */ BTUINT16 event );
typedef void    (Btsdk_Author_Abort_Ind_Func)        ( /* in */ BTSVCHDL svc_hdl, /* in */ BTDEVHDL dev_hdl );
typedef void    (Btsdk_Vendor_Event_Ind_Func)        ( /* in */ BTUINT8 ev_code, /* in */ BTUINT8 ev_param_size, /* in */ BTUINT8 *ev_param );//ev_param = ?,but know size
// if: BT2.1 Supported 
typedef void    (Btsdk_IO_Cap_Req_Ind_Func)          ( /* in */ BTDEVHDL dev_hdl );
typedef void    (Btsdk_Usr_Cfm_Req_Ind_Func)         ( /* in */ BTDEVHDL dev_hdl, /* in */ BTUINT32 num_value );
typedef void    (Btsdk_Passkey_Req_Ind_Func)         ( /* in */ BTDEVHDL dev_hdl);
typedef void    (Btsdk_Rem_OOBData_Req_Ind_Func)     ( /* in */ BTDEVHDL dev_hdl);
typedef void    (Btsdk_Passkey_Notif_Ind_Func)       ( /* in */ BTDEVHDL dev_hdl, /* in */ BTUINT32 num_value );
typedef void    (Btsdk_Simple_Pair_Complete_Ind_Func)( /* in */ BTDEVHDL dev_hdl, /* in */ BTUINT8 result );
/* endif BT2.1 Supported */
typedef BTBOOL (Btsdk_OBEX_Authen_Ind_Func)( /* in */ BTCONNHDL conn_hdl,  /* in/out */ PBtSdkObexAuthInfoStru auth_info);

CESDK_API BTINT32 Btsdk_RegisterCallback( /* in */ BtSdkCallbackStru *call_back);
//ini end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//sec begin--------------------------------------------------------------------------------------------
CESDK_API BTUINT32 Btsdk_AuthorizationResponse(/* in */ BTSVCHDL svc_hdl, /* in */BTDEVHDL dev_hdl, /* in */BTUINT16 author_result);
CESDK_API BTUINT32 Btsdk_GetTrustedDevices( /* in */ BTSVCHDL svc_hdl, /* out */BTDEVHDL *dev_hdl, /* in/out */ BTUINT32 *dev_count);//the buffer to receive the device handles
CESDK_API BTUINT32 Btsdk_SetSecurityMode( /* in */ BTUINT16 secu_mode);
CESDK_API BTUINT32 Btsdk_GetSecurityMode( /* out */ BTUINT16 *security_mode );
CESDK_API BTUINT32 Btsdk_SetServiceSecurityLevel(/* in */BTSVCHDL svc_hdl, /* in */BTUINT8 level);
CESDK_API BTUINT32 Btsdk_GetServiceSecurityLevel(/* in */BTSVCHDL svc_hdl, /* out */ BTUINT8 *level);
CESDK_API BTUINT32 Btsdk_SetAuthorizationMethod(/* in */BTSVCHDL svc_hdl, /* in */BTUINT32 method);
CESDK_API BTUINT32 Btsdk_GetAuthorizationMethod(/* in */BTSVCHDL svc_hdl, /* out */ BTUINT32* pmethod);
CESDK_API BTUINT32 Btsdk_SetTrustedDevice(/* in */BTSVCHDL svc_hdl, /* in */BTDEVHDL dev_hdl, /* in */BTBOOL bIsTrusted);
//sec end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//svc begin--------------------------------------------------------------------------------------------
CESDK_API BTINT32 Btsdk_StartServer( /* in */ BTSVCHDL svc_hdl);
CESDK_API BTINT32 Btsdk_StopServer( /* in */ BTSVCHDL svc_hdl);
CESDK_API BTINT32 Btsdk_RemoveServer( /* in */ BTSVCHDL svc_hdl);
CESDK_API BTINT32 Btsdk_BrowseRemoteServices (/* in */ BTDEVHDL dev_hdl, /* out */BTSVCHDL *svc_hdl, /* in/out */BTUINT32 *svc_count);//buffer to receive the remote service handles	
CESDK_API BTINT32 Btsdk_BrowseRemoteServicesEx(/* in */BTDEVHDL dev_hdl, /* in */ PBtSdkSDPSearchPatternStru psch_ptn, /* in */BTUINT32 ptn_num, /* out */BTSVCHDL *svc_hdl, /* in/out */ BTUINT32 *svc_count);	//psch_ptn＝BtSdkSDPSearchPatternStru  Array ; svc_hdl = buffer to receive the remote service handles
CESDK_API BTINT32 Btsdk_GetRemoteServices(/* in */ BTDEVHDL dev_hdl, /* out */ BTSVCHDL *svc_hdl, /* in/out */ BTUINT32 *svc_count);//buffer to receive the remote service handles
CESDK_API BTINT32 Btsdk_GetRemoteServicesEx(/* in */ BTDEVHDL dev_hdl, /* in */ PBtSdkSDPSearchPatternStru psch_ptn, /* in */ BTUINT32 ptn_num, /* out */ BTSVCHDL *svc_hdl, /* in/out */ BTUINT32 *svc_count);//psch_ptn＝BtSdkSDPSearchPatternStru  Array ; svc_hdl = buffer to receive the remote service handles
CESDK_API BTSDKHANDLE Btsdk_StartEnumRemoteService( /* in */ BTDEVHDL dev_hdl);
CESDK_API BTINT32 Btsdk_EndEnumRemoteService( /* in */ BTSDKHANDLE enum_handle);
CESDK_API BTSVCHDL Btsdk_EnumRemoteService( /* in */ BTSDKHANDLE enum_handle, /* in/out */ PBtSdkRemoteServiceAttrStru attribute);//PBtSdkRemoteServiceAttrStru 's ext_attributes can point to 3 kinds of struct. //we include these 3 struct in idl RemoteServiceAttrStru!!!
CESDK_API BTINT32 Btsdk_GetRemoteServiceAttributes(  /* in */ BTSVCHDL svc_hdl, /* in/out */ BtSdkRemoteServiceAttrStru *attribute);//PBtSdkRemoteServiceAttrStru 's ext_attributes can point to 3 kinds of struct. //we include these 3 struct in idl RemoteServiceAttrStru!!!
CESDK_API BTINT32 Btsdk_GetServerAttributes( /* in */ BTSVCHDL svc_hdl, /* in/out */ BtSdkLocalServerAttrStru *attribute);//BtSdkLocalServerAttrStru 's ext_attributes can point to 9 kind struct// we use 9 idl api to get different struct;
CESDK_API BTINT32 Btsdk_UpdateServerAttributes(/* in */ BTSVCHDL svc_hdl,/* in */  BtSdkLocalServerAttrStru *attribute);//BtSdkLocalServerAttrStru 's ext_attributes can point to 9 kind struct// we use 9 idl api to get different struct;
CESDK_API BTSVCHDL Btsdk_AddServer(/* in */ BtSdkLocalServerAttrStru *attribute);
CESDK_API BTINT32 Btsdk_GetServerStatus(/* in */BTSVCHDL svc_hdl, /* out */ BTUINT16 *status);
CESDK_API BTSDKHANDLE Btsdk_StartEnumLocalServer(void);
CESDK_API BTSVCHDL Btsdk_EnumLocalServer(/* in */BTSDKHANDLE enum_handle, /* in/out */ PBtSdkLocalServerAttrStru attribute);//BtSdkLocalServerAttrStru 's ext_attributes can point to 9 kind struct// we don't do special things ,it may have error????
CESDK_API BTINT32 Btsdk_EndEnumLocalServer(/* in */BTSDKHANDLE enum_handle);
CESDK_API BTINT32 Btsdk_RefreshRemoteServiceAttributes(/* in */BTSVCHDL svc_hdl,  /* in/out */ BtSdkRemoteServiceAttrStru *attribute);//PBtSdkRemoteServiceAttrStru 's ext_attributes can point to 3 kinds of struct. //we include these 3 struct in idl RemoteServiceAttrStru!!!
CESDK_API BTINT32 Btsdk_SetRemoteServiceParam(/* in */BTSVCHDL svc_hdl, /* in */BTUINT32 app_param);
CESDK_API BTINT32 Btsdk_GetRemoteServiceParam(/* in */BTSVCHDL svc_hdl, /* out */ BTUINT32 *papp_param);
//svc end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//loc begin--------------------------------------------------------------------------------------------
CESDK_API BTINT32 Btsdk_GetDiscoveryMode(/* out */ BTUINT16 *pmode);
CESDK_API BTINT32 Btsdk_SetDiscoveryMode ( /* in */ BTUINT16 mode);
CESDK_API BTINT32 Btsdk_GetLocalDeviceAddress(/* out */ BTUINT8 *bd_addr);//buffer that receives the device address.Array[BTSDK_BDADDR_LEN=6] not string
CESDK_API BTINT32 Btsdk_SetLocalDeviceClass( /* in */ BTUINT32 device_class);
CESDK_API BTINT32 Btsdk_GetLocalServers(/* out */ BTSVCHDL *svc_hdl, /* in/out */ BTUINT32 *svc_count);// buffer to receive the service handles
CESDK_API BTINT32 Btsdk_GetLocalName(/* out */ BTUINT8* name, /* in/out */ BTUINT16 *plen);//the buffer that receives the device name.(c string)
CESDK_API BTINT32 Btsdk_SetLocalName( /* in */ BTUINT8* name, /* in */ BTUINT16 len);//c string
CESDK_API BTUINT32 Btsdk_GetLocalComPortHandle(void);
CESDK_API BTINT32 Btsdk_SetDefaultCommSettings( /* in */ PBtSdkCommSettingStru pcs);
CESDK_API BTINT32 Btsdk_GetFixedPinCode(/* out */ BTUINT8 *pin_code, /* in/out */ BTUINT16 *psize);//string[psize],buffer should be no less than BTSDK_PIN_CODE_LEN=16 //stackdoc6.4中无此接口
CESDK_API BTINT32 Btsdk_VendorCommand(/* in */ BTUINT32 ev_flag, /* in */ PBtSdkVendorCmdStru in_cmd, /* out */  PBtSdkEventParamStru out_ev);
CESDK_API BTINT32 Btsdk_BTDeviceUnplug (void);	
CESDK_API BTINT32 Btsdk_SetAFHChannelClassification(/* in */ BTUINT8 *afh_channels);//Specify the AFH host channel classification.It shall be a buffer no smaller than 10 bytes. But only 79bits (from byte 0 to byte 9) are meaningful
CESDK_API BTINT32 Btsdk_GetLocalDeviceClass(/* out */ BTUINT32* pdevice_class);
CESDK_API BTINT32 Btsdk_GetLocalLMPInfo(/* out */ BtSdkLocalLMPInfoStru *plmp_info);
CESDK_API BTINT32 Btsdk_SendDataToHostController(/* in */ BTUINT32 size, /* in */ BTUINT8 *data);//data = ?,but know size; Pointed to the buffer contains the data to be transmitted to the host controller.
//loc end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//rmt begin--------------------------------------------------------------------------------------------
CESDK_API BTBOOL Btsdk_IsDevicePairedExt(/* in */ BTDEVHDL dev_hdl, /* out */ BTUINT8 *plink_key);//link_key[BTSDK_LINK_KEY_LENGTH=16] not string
CESDK_API BTINT32 Btsdk_IsDevicePaired(/* in */ BTDEVHDL dev_hdl, /* out */ BTBOOL *pis_paired);
CESDK_API BTINT32 Btsdk_PairDevice(  /* in */ BTDEVHDL dev_hdl);
CESDK_API BTINT32 Btsdk_UnPairDevice( /* in */ BTDEVHDL dev_hdl);
CESDK_API BTINT32 Btsdk_StartDeviceDiscovery( /* in */ BTUINT32 device_class, /* in */ BTUINT16 max_num, /* in */ BTUINT16 max_seconds);
CESDK_API BTINT32 Btsdk_StopDeviceDiscovery(void);
CESDK_API BTUINT32 Btsdk_GetInquiredDevices(/* out */ BTDEVHDL *pdev_hdl, /* in */  BTUINT32 max_dev_num);//buffer to receive the device handles.
CESDK_API BTINT32 Btsdk_GetRemoteDeviceAddress(/* in */ BTDEVHDL dev_hdl, /* out */ BTUINT8 *bd_addr);//Array[BTSDK_BDADDR_LEN=6] not string
CESDK_API BTINT32 Btsdk_UpdateRemoteDeviceName(/* in */ BTDEVHDL dev_hdl, /* out */ BTUINT8 *name, /* in/out */ BTUINT16 *plen);//c string
CESDK_API BTINT32 Btsdk_CancelUpdateRemoteDeviceName (/* in */ BTDEVHDL dev_hdl);	
CESDK_API BTUINT32 Btsdk_GetStoredDevicesByClass(/* in */ BTUINT32 dev_class,/* out */ BTDEVHDL *pdev_hdl, /* in */ BTUINT32 max_dev_num);//buffer to receive the device handles. 
CESDK_API BTDEVHDL Btsdk_GetRemoteDeviceHandle(/* in */ BTUINT8 *bd_addr);//Array[BTSDK_BDADDR_LEN=6] not string
CESDK_API BTINT32 Btsdk_DeleteRemoteDeviceByHandle( /* in */ BTDEVHDL dev_hdl);
CESDK_API BTINT32 Btsdk_GetRemoteDeviceClass(/* in */ BTDEVHDL dev_hdl, /* out */ BTUINT32 * pdevice_class);
CESDK_API BTINT32 Btsdk_GetRemoteDeviceName(/* in */ BTDEVHDL dev_hdl, /* out */ BTUINT8 *name, /* in/out */ BTUINT16 *plen);//c string
CESDK_API BTINT32 Btsdk_GetRemoteDeviceProperty( /* in */ BTDEVHDL dev_hdl, /* in/out */ PBtSdkRemoteDevicePropertyStru rmt_dev_prop);
CESDK_API BTINT32 Btsdk_GetRemoteLinkQuality( /* in */ BTDEVHDL dev_hdl, /* out */ BTUINT16 *plink_quality);
CESDK_API BTBOOL Btsdk_IsDeviceConnected( /* in */ BTDEVHDL dev_hdl);
CESDK_API BTINT32 Btsdk_RejectConnectionRequest( /* in */ BTDEVHDL dev_hdl,  /* in */ BTUINT8 reason,  /* in */ BTUINT8 link_type);
CESDK_API BTINT32 Btsdk_PinCodeReply( /* in */ BTDEVHDL dev_hdl,  /* in */ BTUINT8* pin_code, /* in */ BTUINT16 size);//string[psize],buffer should be no less than BTSDK_PIN_CODE_LEN=16
CESDK_API BTINT32 Btsdk_WriteLinkPolicy( /* in */ BTDEVHDL dev_hdl, /* in */  BTUINT16 policy);
CESDK_API BTINT32 Btsdk_LinkKeyReply(/* in */ BTDEVHDL dev_hdl, /* in */ BTUINT8* link_key);//link_key[BTSDK_LINK_KEY_LENGTH=16] not string
CESDK_API BTINT32 Btsdk_GetRemoteDeviceRole(/* in */ BTDEVHDL dev_hdl, /* out */ BTUINT16 *prole);
CESDK_API BTINT32 Btsdk_GetRemoteLMPInfo(/* in */BTDEVHDL dev_hdl,/* out */ BtSdkRemoteLMPInfoStru *pinfo);
CESDK_API BTINT32 Btsdk_GetSupervisionTimeout(/* in */BTDEVHDL dev_hdl, /* out */ BTUINT16 *ptimeout);
CESDK_API BTINT32 Btsdk_GetCurrentLinkMode(/* in */BTDEVHDL dev_hdl, /* out */ BTUINT8* link_mode);
CESDK_API BTINT32 Btsdk_SetLinkMode(/* in */BTDEVHDL dev_hdl,/* in */ BTUINT8 link_mode, /* in */ BTUINT8* param);//NULL(BTSDK_LPM_ACTIVE_MODE);BtSdkHoldModeStru(BTSDK_LPM_HOLD_MODE);BtSdkSniffModeStru(BTSDK_LPM_SNIFF_MODE);BtSdkParkModeStru(BTSDK_LPM_PARK_MODE) //stackdoc6.4中无此接口
CESDK_API BTINT32 Btsdk_ChangeConnectionPacketType(/* in */BTDEVHDL dev_hdl, /* in */BTUINT16 packet_type);
CESDK_API BTINT32 Btsdk_ReadLinkPolicy(/* in */BTDEVHDL dev_hdl,/* out */ BTUINT16 *policy);
CESDK_API BTDEVHDL Btsdk_AddRemoteDevice(/* in */BTUINT8 *bd_addr);//Array[BTSDK_BDADDR_LEN=6] not string
CESDK_API BTINT32 Btsdk_DeleteUnpairedDevicesByClass(/* in */BTUINT32 device_class);
CESDK_API BTUINT32 Btsdk_GetPairedDevices(/* out */ BTDEVHDL *pdev_hdl, /* in */BTUINT32 max_dev_num);//buffer to receive the device handles.
CESDK_API BTSDKHANDLE Btsdk_StartEnumRemoteDevice(/* in */BTUINT32 flag, /* in */BTUINT32 dev_class);
CESDK_API BTDEVHDL Btsdk_EnumRemoteDevice(/* in */BTSDKHANDLE enum_handle, /* in/out */ PBtSdkRemoteDevicePropertyStru rmt_dev_prop);
CESDK_API BTINT32 Btsdk_EndEnumRemoteDevice(/* in */BTSDKHANDLE enum_handle);
CESDK_API BTINT32 Btsdk_SetRemoteDeviceParam(/* in */BTDEVHDL dev_hdl, /* in */BTUINT32 app_param);
CESDK_API BTINT32 Btsdk_GetRemoteDeviceParam(/* in */BTDEVHDL dev_hdl, /* out */ BTUINT32 *papp_param);
//rmt end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//shc begin--------------------------------------------------------------------------------------------
CESDK_API BTINT32 Btsdk_CreateShortCutEx(/* in/out */ PBtSdkShortCutPropertyStru shc_prop); //stackdoc6.4中无此接口
CESDK_API BTINT32 Btsdk_DeleteShortCut(/* in */ BTSHCHDL shc_hdl);//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_GetAllShortCuts(/* out */ BTSHCHDL *pshc_hdl,  /* in */ BTUINT32 max_shc_num);//buffer to receive the handles//stackdoc6.4中无此接口
CESDK_API BTSHCHDL Btsdk_GetDefaultShortCut(/* in */ BTUINT32 service_class);
CESDK_API BTUINT32 Btsdk_GetShortCutByConnectionHandle( /* in */ BTCONNHDL conn_hdl, /* out */ BTSHCHDL *pshc_hdl, /* in */ BTUINT32 max_shc_num);//buffer to receive the handles//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_GetShortCutByDeviceHandle( /* in */ BTDEVHDL dev_hdl, /* in */ BTUINT16 service_class,  /* out */ BTSHCHDL *pshc_hdl, /* in */ BTUINT32 max_shc_num);//buffer to receive the handles//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_GetShortCutByServiceClass( /* in */ BTUINT32 service_class, /* out */ BTSHCHDL *pshc_hdl, /* in */ BTUINT32 max_shc_num);//buffer to receive the handles//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_GetShortCutByServiceHandle( /* in */ BTDEVHDL dev_hdl, /* in */ BTSVCHDL svc_hdl,  /* out */ BTSHCHDL *pshc_hdl, /* in */ BTUINT32 max_shc_num);//buffer to receive the handles//stackdoc6.4中无此接口
CESDK_API BTINT32 Btsdk_GetShortCutProperty(/* in/out */ PBtSdkShortCutPropertyStru pshc_prop);//stackdoc6.4中无此接口
CESDK_API BTINT32 Btsdk_SetShortCutProperty(/* in */ PBtSdkShortCutPropertyStru pshc_prop);//stackdoc6.4中无此接口
CESDK_API BTINT32 Btsdk_ConnectShortCutEx(/* in */ BTSHCHDL shc_hdl, /* in */ BTUINT32 lParam, /* out */ BTCONNHDL *conn_hdl);//deciede by svc_hdl;lParam is the same as Btsdk_Connect;//stackdoc6.4中无此接口
CESDK_API BTSHCHDL Btsdk_GetShortCutByName(BTUINT8 *short_name);
//shc end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//------------------------------------------------------------------------------------------------------
//conn begin--------------------------------------------------------------------------------------------
CESDK_API BTINT32 Btsdk_Connect( /* in */ BTSVCHDL svc_hdl, /* in */ BTUINT32 lParam, /* out */ BTCONNHDL *conn_hdl);//deciede by svc_hdl;lParam may be the addr of var: BtSdkLAPConnParamStru; BtSdkDUNConnParamStru; BtSdkFAXConnParamStru; BtSdkSPPConnParamStru; BtSdkOPPClientParamStru; BTUINT8; BtSdkLocalA2DPServerAttrStru; SDAP_DIInfoStru; BtSdkBIPImgConnParamStru; BtSdkBIPObjConnParamStru; BtSdkBPPConnParamStru or NULL
CESDK_API BTINT32 Btsdk_ConnectEx( /* in */ BTDEVHDL dev_hdl, /* in */ BTUINT16 service_class, /* in */ BTUINT32 lParam, /* out */ BTCONNHDL *conn_hdl);//deciede by svc_hdl;lParam is the same as Btsdk_Connect;
CESDK_API BTINT32 Btsdk_Disconnect(/* in */ BTCONNHDL handle);
CESDK_API BTSDKHANDLE Btsdk_StartEnumConnection(void);
CESDK_API BTINT32 Btsdk_EndEnumConnection( /* in */ BTSDKHANDLE enum_handle);
CESDK_API BTCONNHDL Btsdk_EnumConnection( /* in */ BTSDKHANDLE enum_handle, /* out */ PBtSdkConnectionPropertyStru conn_prop);
CESDK_API BTUINT32 Btsdk_GetAllIncomingConnections(  /* out */ BTCONNHDL *conn_hdl,   /* in */ BTUINT32 count);//buffer to receive the connection handles
CESDK_API BTUINT32 Btsdk_GetAllOutgoingConnections( /* out */ BTCONNHDL *conn_hdl, /* in */ BTUINT32 count);//buffer to receive the connection handles
CESDK_API BTINT32 Btsdk_GetConnectionProperty( /* in */ BTCONNHDL conn_hdl, /* out */ PBtSdkConnectionPropertyStru conn_prop);
CESDK_API BTINT16 Btsdk_GetServerPort(/* in */ BTSVCHDL svc_hdl);//stackdoc6.4中无此接口
CESDK_API BTINT16 Btsdk_GetClientPort( /* in */ BTCONNHDL conn_hdl);//stackdoc6.4中无此接口
//conn end---------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//HF HF 15 begin  ------------------------------------------------------------------------------------------

/* Prototype of APP defined callback function. */
typedef void (Btsdk_HFP_Callback)(BTCONNHDL hdl, BTUINT16 event,  /* in */BTUINT8 *param, BTUINT16 len);

/* SDK HFP APIs */
CESDK_API BTUINT32 Btsdk_HFAP_APPRegCbk( Btsdk_HFP_Callback *pfunc);

CESDK_API BTSVCHDL Btsdk_RegisterHFPService( /* in */BTUINT8 *svc_name, BTUINT16 svc_class, BTUINT16 features);
CESDK_API BTUINT32 Btsdk_UnregisterHFPService(BTSVCHDL svc_hdl);

CESDK_API BTUINT32 Btsdk_HFAP_GetManufacturerID(BTCONNHDL hdl,  /* out */BTUINT8 *manufacturer_id,  /* in/out */BTUINT16 *id_len);
CESDK_API BTUINT32 Btsdk_HFAP_GetModelID(BTCONNHDL hdl,  /* out */BTUINT8 *model_id,  /* in/out */BTUINT16 *id_len);
CESDK_API BTUINT32 Btsdk_HFAP_Dial(BTCONNHDL hdl,  /* in */void *phone_num, BTUINT16 len);
CESDK_API BTUINT32 Btsdk_HFAP_MemNumDial(BTCONNHDL hdl,  /* in */void *mem_location, BTUINT16 len);
CESDK_API BTUINT32 Btsdk_HFAP_LastNumRedial(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_SetSpkVol(BTCONNHDL hdl, BTUINT8 spk_vol);
CESDK_API BTUINT32 Btsdk_HFAP_SetMicVol(BTCONNHDL hdl, BTUINT8 mic_vol);
CESDK_API BTUINT32 Btsdk_HFAP_TxDTMF(BTCONNHDL hdl, BTUINT8 chr);
CESDK_API BTUINT32 Btsdk_HFAP_AudioConnTrans(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_VoiceTagPhoneNumReq(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_AnswerCall(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_CancelCall(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_VoiceRecognitionReq(BTCONNHDL hdl, BTUINT8 param);
CESDK_API BTUINT32 Btsdk_HFAP_DisableNREC(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_3WayCallingHandler(BTCONNHDL hdl, BTUINT16 op_code, BTUINT8 idx);
CESDK_API BTUINT32 Btsdk_HFAP_NetworkOperatorReq(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_SetExtendedErrors(BTCONNHDL hdl, BTUINT8 enable);
CESDK_API BTUINT32 Btsdk_HFAP_GetResponseHoldStatus(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_HoldIncomingCall(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_AcceptHeldIncomingCall(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_RejectHeldIncomingCall(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_GetSubscriberNumber(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_GetCurrentCalls(BTCONNHDL hdl);
CESDK_API BTUINT32 Btsdk_HFAP_GetAGFeatures(BTCONNHDL hdl);

CESDK_API BTUINT32 Btsdk_HFP_ExtendCmd(BTCONNHDL hdl,  /* in */void *cmd, BTUINT16 len, BTUINT32 timeout);

CESDK_API BTUINT32 Btsdk_HFSwrap_BoundClnt(BTCONNHDL conn_hdl,  /* out */BTUINT8 *com_idx);//not in stack7.0 doc
CESDK_API BTUINT32 Btsdk_HFSwrap_Unbound(BTCONNHDL conn_hdl); //not in stack7.0 doc
CESDK_API BTUINT32 Btsdk_HFSwrap_ResultCodeInd(BTCONNHDL conn_hdl,  /* in */BTUINT8 *buff, BTUINT16 len);//not in stack7.0 doc
//HF HF 15 end--------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//FTP begin--------------------------------------------------------------------------------------------
typedef BTBOOL (BTSDK_FTP_UIDealReceiveFile)( /* in */ PBtSdkFileTransferReqStru pFileInfo);
CESDK_API void Btsdk_FTPRegisterDealReceiveFileCB( /* in */ BTSDK_FTP_UIDealReceiveFile *func);
typedef void  (Btsdk_FTP_STATUS_INFO_CB)(/* in */ BTUINT8 first, /* in */ BTUINT8 last, /* in */ BTUINT8* filename, /* in */ BTUINT32 filesize, /* in */ BTUINT32 cursize);
CESDK_API void Btsdk_FTPRegisterStatusCallback( /* in */ BTCONNHDL conn_hdl, /* in */ Btsdk_FTP_STATUS_INFO_CB *func);
typedef void (BTSDK_FTP_UIShowBrowseFile)(BTUINT8* SYS_FIND_DATA);//old is (/* in */ WIN32_FIND_DATA* pFileFindData );//*SYS_FIND_DATA: This structure describes a file found by the FindFirstFile(Depends on the system) // WIN32_FIND_DATA is a struct in Winbase.h.
CESDK_API BTINT32 Btsdk_FTPBrowseFolder( /* in */ BTCONNHDL conn_hdl, /* in */ BTUINT8 *szPath, /* in */ BTSDK_FTP_UIShowBrowseFile *pShowFunc, /* in */ BTUINT8 op_type);//c string
CESDK_API BTSVCHDL Btsdk_RegisterFTPService(  /* in */ BTUINT16 desired_access,  /* in */ BTUINT8 *root_dir);//c string
CESDK_API BTINT32 Btsdk_FTPCancelTransfer( /* in */ BTCONNHDL conn_hdl);
CESDK_API BTINT32 Btsdk_FTPCreateDir( /* in */ BTCONNHDL conn_hdl,/* in */ BTINT8 *szDir);// c string
CESDK_API BTINT32 Btsdk_FTPGetDir( /* in */ BTCONNHDL conn_hdl, /* in */ BTUINT8 *rem_dir, /* in */ BTUINT8 *new_dir);	// c string
CESDK_API BTINT32 Btsdk_FTPGetFile( /* in */ BTCONNHDL conn_hdl, /* in */ BTUINT8 *rem_file, /* in */ BTUINT8 *new_file);// c string
CESDK_API BTINT32 Btsdk_FTPGetRmtDir( /* in */ BTCONNHDL conn_hdl, /* out */ BTUINT8 *szDir);// c string
CESDK_API BTINT32 Btsdk_FTPPutDir(/* in */ BTCONNHDL conn_hdl, /* in */ BTUINT8 *loc_dir, /* in */ BTUINT8 *new_dir);// c string
CESDK_API BTINT32 Btsdk_FTPPutFile( /* in */ BTCONNHDL conn_hdl, /* in */ BTUINT8 *loc_file, /* in */ BTUINT8 *new_file);// c string
CESDK_API BTINT32 Btsdk_FTPSetRmtDir( /* in */ BTCONNHDL conn_hdl, /* in */ BTUINT8 *szDir );// c string
CESDK_API BTUINT32 Btsdk_UnregisterFTPService(void);
CESDK_API BTINT32 Btsdk_FTPDeleteDir( /* in */ BTCONNHDL conn_hdl, /* in */ BTINT8 *szDir);// c string
CESDK_API BTINT32 Btsdk_FTPDeleteFile( /* in */ BTCONNHDL conn_hdl, /* in */ BTINT8 *szFile);// c string
CESDK_API BTBOOL Btsdk_FTPBackDir( /* in */ BTCONNHDL conn_hdl);
//FTP end  --------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//CTP/ICP begin  --------------------------------------------------------------------------------------
typedef void  (Btsdk_Ctpicp_Event_Ind_Func)(  /* in */ BTUINT16 event,  /* in */ BTUINT8 *parg,  /* in */ BTUINT32 arg);//parg decide by event
void Btsdk_RegisterCtpicpCallback(  /* in */ Btsdk_Ctpicp_Event_Ind_Func *func);//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_CtpIcpInit(void);
CESDK_API BTUINT32 Btsdk_CtpIcpDone(void);

CESDK_API BTUINT32 Btsdk_CtpAnswer ( /* in */  BTDEVHDL dev_hdl) ;//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_CtpDial(/* in */ BTDEVHDL dev_hdl, /* in */ BTUINT8 *phone_no );// c string//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_CtpHangup( /* in */ BTDEVHDL dev_hdl);//stackdoc6.4中无此接口

CESDK_API BTUINT32 Btsdk_CtpSendDTMF( /* in */ BTDEVHDL dev_hdl, /* in */ BTUINT8 number);//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_JoinGateway( /* in */ BTDEVHDL dev_hdl,  /* out */ BTCONNHDL *pconn_hdl);//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_LeaveGateway( /* in */ BTCONNHDL conn_hdl);
CESDK_API BTUINT32 Btsdk_RegisterNextCall( /* in */ BTDEVHDL dev_hdl, /* in */ BTUINT8 *phone_no);// c string//stackdoc6.4中无此接口

CESDK_API BTUINT32 Btsdk_IcpAnswer( /* in */ BTCONNHDL conn_hdl);//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_IcpDial( /* in */ BTDEVHDL dev_hdl, /* out */ BTCONNHDL *pconn_hdl);//stackdoc6.4中无此接口
CESDK_API BTUINT32 Btsdk_IcpHangup( /* in */ BTCONNHDL conn_hdl);//stackdoc6.4中无此接口
//CTP/ICP end------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//OPP begin-------------------------------------------------------------------------------------------
typedef BTBOOL (BTSDK_OPP_UIDealReceiveFile)( /* in */ PBtSdkFileTransferReqStru pFileInfo);
CESDK_API void Btsdk_OPPRegisterDealReceiveFileCB( /* in */ BTSDK_OPP_UIDealReceiveFile *func);
typedef void (Btsdk_OPP_STATUS_INFO_CB)( /* in */ BTUINT8 first,  /* in */ BTUINT8 last,  /* in */ BTUINT8* filename,  /* in */ BTUINT32 filesize, /* in */  BTUINT32 cursize);
CESDK_API void Btsdk_OPPRegisterStatusCallback( /* in */ BTCONNHDL conn_hdl, /* in */ Btsdk_OPP_STATUS_INFO_CB *func);
CESDK_API BTSVCHDL Btsdk_RegisterOPPService( /* in */  BTUINT8 *inbox_path, /* in */ BTUINT8 *outbox_path, /* in */ BTUINT8 *own_card);// c string
CESDK_API BTINT32 Btsdk_OPPPullObj( /* in */ BTCONNHDL conn_hdl, /* in */ BTUINT8 *szPullFilePath);// c string
CESDK_API BTINT32 Btsdk_OPPPushObj(  /* in */ BTCONNHDL conn_hdl,   /* in */ BTUINT8 *szPushFilePath);// c string
CESDK_API BTUINT32 Btsdk_UnregisterOPPService(void);
CESDK_API BTINT32 Btsdk_OPPCancelTransfer(/* in */BTCONNHDL conn_hdl);
CESDK_API BTINT32 Btsdk_OPPExchangeObj(/* in */BTCONNHDL conn_hdl, /* in */BTUINT8 *szPushFilePath, /* in */BTUINT8 *szPullFilePath, /* out */BTINT32 *nPushError, /* out */BTINT32 *nPullError);// c string; nPushError and nPullError just BTINT32;
//OPP end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//DUN begin-------------------------------------------------------------------------------------------
CESDK_API BTSVCHDL Btsdk_RegisterDUNService( /* in */ BTUINT16 index);
CESDK_API BTUINT32 Btsdk_UnregisterDUNService(void);
//DUN end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//PAN begin-------------------------------------------------------------------------------------------
CESDK_API BTSVCHDL Btsdk_RegisterPANService( /* in */ BTUINT16 svcUUID, /* in */ BTUINT16 len, /* in */ const BTINT8 *param);//param =? but know len
CESDK_API BTUINT32 Btsdk_UnregisterPANService(void);
typedef void (Btsdk_PAN_Event_Ind_Func)(/* in */BTUINT16 event, /* in */BTUINT16 len, /* in */BTUINT8 *param); //now event can only be  BTSDK_PAN_EV_IP_CHANGE; and param = pointer to a 32bit integer contains the new IP address value
CESDK_API void Btsdk_PAN_RegIndCbk(/* in */Btsdk_PAN_Event_Ind_Func *pfunc);
//PAN end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//SPP begin-------------------------------------------------------------------------------------------
CESDK_API BTSVCHDL Btsdk_RegisterSPPService( /* in */ BTUINT16 index);
CESDK_API BTUINT32 Btsdk_UnregisterSPPService(/* in */BTSVCHDL svc_hdl);
CESDK_API BTSVCHDL Btsdk_RegisterAppExtSPPService(/* in/out */ PBtSdkAppExtSPPAttrStru psvc, /* out */ BTUINT32 *result);
CESDK_API BTUINT32 Btsdk_UnregisterAppExtSPPService(/* in */BTSVCHDL svc_hdl);
CESDK_API BTUINT32 Btsdk_ConnectAppExtSPPService(/* in */BTDEVHDL dev_hdl, /* in/out */ PBtSdkAppExtSPPAttrStru psvc, /* out */BTCONNHDL *conn_hdl);
CESDK_API BTUINT32 Btsdk_SearchAppExtSPPService(/* in */BTDEVHDL dev_hdl, /* in/out */ PBtSdkAppExtSPPAttrStru psvc);
//SPP end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//FAX begin-------------------------------------------------------------------------------------------
CESDK_API BTSVCHDL Btsdk_RegisterFAXService(/* in */BTUINT16 index);
CESDK_API BTUINT32 Btsdk_UnregisterFAXService(void);
//FAX end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//A2DP begin-------------------------------------------------------------------------------------------
CESDK_API BTSVCHDL Btsdk_RegisterA2DPSRCService(void);
CESDK_API BTUINT32 Btsdk_UnregisterA2DPSRCService(void);
CESDK_API BTSVCHDL Btsdk_RegisterA2DPSNKService(/* in */BTUINT16 len, /* in */const BTUINT8 *audio_card);// the audio card name used (c string)// specifies the playback device used to play the audio stream
CESDK_API BTUINT32 Btsdk_UnregisterA2DPSNKService(void);
//A2DP end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//AVRCP begin-------------------------------------------------------------------------------------------
CESDK_API BTSVCHDL Btsdk_RegisterAVRCPTGService();
CESDK_API BTUINT32 Btsdk_UnregisterAVRCPTGService(void);
typedef void (Btsdk_AVRCP_PassThr_Cmd_Func)(/* in */BTUINT8 op_id, /* in */BTUINT8 state_flag);
CESDK_API void Btsdk_AVRCP_RegPassThrCmdCbk(/* in */Btsdk_AVRCP_PassThr_Cmd_Func *pfunc);
typedef void (Btsdk_AVRCP_Event_Ind_Func)(/* in */BTUINT16 event, /* in */BTUINT8 *param);//now ,param all ignored 
CESDK_API void Btsdk_AVRCP_RegIndCbk(/* in */Btsdk_AVRCP_Event_Ind_Func * pfunc);
CESDK_API BTSVCHDL Btsdk_RegisterAVRCPCTService(void);
//CESDK_API BTUINT32 Btsdk_UnregisterAVRCPCTService(void);
CESDK_API BTINT32 Btsdk_AVRCP_PassThroughReq(/* in */PBtSdkPassThrReqStru preq);
//AVRCP end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//PIM begin-------------------------------------------------------------------------------------------
CESDK_API BOOL PIM_MGR_Init(TCHAR *pszPIMWorkingPath, TCHAR *pszPBDBWorkingPath, TCHAR *pszSMSDBWorkingPath);

CESDK_API INT  PIM_MGR_Connect(BTDEVHDL dvhdl, BTCONNHDL connhdl, TCHAR * pszManu, TCHAR *pszModel, int SvcType);
CESDK_API BOOL PIM_MGR_Disconnect();
CESDK_API INT  PIM_MGR_GetPhoneList(PPHONELIST pPhoneList);

CESDK_API BOOL PIM_MGR_UpdatePatch();

CESDK_API INT  PIM_MGR_SyncContacts(PPBDATA pPBdata);
CESDK_API INT  PIM_MGR_GetContacts(PPBDATA pPBdata);
CESDK_API BOOL PIM_MGR_AddContacts(int icount, PPBDATA pPBdata);
CESDK_API BOOL PIM_MGR_ClearContacts();

CESDK_API INT  PIM_MGR_SyncSMS(PSMSDATA pSMSData);
CESDK_API BOOL PIM_MGR_SendSMS(TCHAR *pszNumber, TCHAR *pszSMSbody);
CESDK_API INT  PIM_MGR_GetSMS(PSMSDATA pPSMSdata);
CESDK_API BOOL PIM_MGR_AddSMS(int icount, PSMSDATA pPSMSdata);
CESDK_API BOOL PIM_MGR_DelSMS(PSMSDATA pPSMSdata);
CESDK_API BOOL PIM_MGR_ClearSMS();
CESDK_API BOOL PIM_MGR_Uninit();
CESDK_API BOOL PIM_MGR_SetAsyncSMSCB(PPIMCB NewAsyncCallBack);
//PIM end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------
#ifdef __cplusplus
}
#endif

#endif