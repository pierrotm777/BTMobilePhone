/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2007 IVT Corporation
*
* All rights reserved.
* 
---------------------------------------------------------------------------*/
 
/////////////////////////////////////////////////////////////////////////////
// Module Name:
//     SDK_API.h
// Abstract:
//     This module defines exported BlueSoleil SDK API prototypes. 
// Usage:
//     #include "SDK_API.h"
// 
// Author://     
//     
// Revision History:
//     2007-12-25		Created
// 
/////////////////////////////////////////////////////////////////////////////



#ifndef _BTSDK_API_H
#define _BTSDK_API_H



/*************** Function Prototype ******************/

/* Initialization and Termination */
typedef void Func_ReceiveBluetoothStatusInfo(ULONG usMsgType, ULONG pulData, ULONG param, BTUINT8 *arg);
BTINT32 Btsdk_Init(void);
BTINT32 Btsdk_Done(void);
BTBOOL Btsdk_IsSDKInitialized(void);
BTBOOL Btsdk_IsServerConnected();
BTINT32	Btsdk_RegisterGetStatusInfoCB4ThirdParty(Func_ReceiveBluetoothStatusInfo* statusCBK);
BTINT32 Btsdk_RegisterCallback4ThirdParty(BtSdkCallbackStru* call_back);
BTINT32 Btsdk_RegisterCallbackEx(BtSdkCallbackStru* call_back, DWORD priority);
BTINT32 Btsdk_RegisterCallback(BtSdkCallbackStru* call_back);
BTINT32	Btsdk_SetStatusInfoFlag(USHORT usMsgType);


/* Memory Management */
void *Btsdk_MallocMemory(BTUINT32 size);
void Btsdk_FreeMemory(void *memblock);

/* Local Bluetooth Device Initialization */
BTINT32 Btsdk_StartBluetooth(void);
BTINT32 Btsdk_StopBluetooth(void);
BTBOOL Btsdk_IsBluetoothReady(void);
BTBOOL Btsdk_IsBluetoothHardwareExisted();

/* Local Device Mode */
BTINT32 Btsdk_SetDiscoveryMode(BTUINT16 mode);
BTINT32 Btsdk_GetDiscoveryMode(BTUINT16 *mode);

/* Local Device Information */
BTINT32 Btsdk_GetLocalDeviceAddress(BTUINT8 *bd_addr);
BTINT32 Btsdk_SetLocalName(BTUINT8* name, BTUINT16 len);
BTINT32 Btsdk_GetLocalName(BTUINT8* name, BTUINT16 *len);
BTINT32 Btsdk_SetLocalDeviceClass(BTUINT32 device_class);
BTINT32 Btsdk_GetLocalDeviceClass(BTUINT32 *device_class);
BTINT32 Btsdk_GetLocalLMPInfo(BtSdkLocalLMPInfoStru *lmp_info);
BTINT32 Btsdk_SetFixedPinCode(BTUINT8 *pin_code, BTUINT16 size);
BTINT32 Btsdk_GetFixedPinCode(BTUINT8 *pin_code, BTUINT16 *psize);

/* Local Device Application Extension */
BTINT32 Btsdk_VendorCommand(BTUINT32 ev_flag, PBtSdkVendorCmdStru in_cmd, PBtSdkEventParamStru out_ev);
BTUINT32 Btsdk_EnumAVDriver();
void Btsdk_DeEnumAVDriver();
BTINT32 Btsdk_ActivateEx(const BTINT8 *pszSN, BTINT32 iSnlen);

/* Remote Device Discovery */
FUNC_EXPORT BTINT32 Btsdk_StartDeviceDiscovery(BTUINT32 device_class, BTUINT16 max_num, BTUINT16 max_seconds);
typedef void  (Btsdk_Inquiry_Result_Ind_Func)(BTDEVHDL dev_hdl);
typedef void  (Btsdk_Inquiry_Complete_Ind_Func)(void);
FUNC_EXPORT BTINT32 Btsdk_StopDeviceDiscovery(void);
FUNC_EXPORT BTINT32 Btsdk_UpdateRemoteDeviceName(BTDEVHDL dev_hdl, BTUINT8 *name, BTUINT16 *plen);
FUNC_EXPORT BTINT32 Btsdk_CancelUpdateRemoteDeviceName(BTDEVHDL dev_hdl);
FUNC_EXPORT BTINT32 Btsdk_LinkKeyReply(BTDEVHDL dev_hdl, BTUINT8* link_key);

/* Device Pairing */
FUNC_EXPORT BTINT32 Btsdk_IsDevicePaired(BTDEVHDL dev_hdl, BTBOOL *pis_paired);
FUNC_EXPORT BTINT32 Btsdk_PairDevice(BTDEVHDL dev_hdl);
FUNC_EXPORT BTINT32 Btsdk_UnPairDevice(BTDEVHDL dev_hdl);
FUNC_EXPORT BTINT32 Btsdk_PinCodeReply(BTDEVHDL dev_hdl, BTUINT8* pin_code, BTUINT16 size);
FUNC_EXPORT BTUINT32 Btsdk_AuthorizationResponse(BTSVCHDL svc_hdl, BTDEVHDL dev_hdl, BTUINT16 author_result);

/* Callback Prototype */
typedef BTUINT8  (Btsdk_UserHandle_Pin_Req_Ind_Func)(BTDEVHDL dev_hdl);
typedef BTUINT8  (Btsdk_UserHandle_Authorization_Req_Ind_Func)(BTSVCHDL svc_hdl, BTDEVHDL dev_hdl);
typedef void  (Btsdk_Link_Key_Notif_Ind_Func)(BTDEVHDL dev_hdl, BTUINT8 *link_key);
typedef void  (Btsdk_Authentication_Fail_Ind_Func)(BTDEVHDL dev_hdl);

/* Link Management */
FUNC_EXPORT BTBOOL Btsdk_IsDeviceConnected(BTDEVHDL dev_hdl);
FUNC_EXPORT BTINT32 Btsdk_GetRemoteDeviceRole(BTDEVHDL dev_hdl, BTUINT16 *prole);
FUNC_EXPORT BTINT32 Btsdk_GetRemoteLMPInfo(BTDEVHDL dev_hdl, BtSdkRemoteLMPInfoStru *info);
FUNC_EXPORT BTINT32 Btsdk_GetRemoteRSSI(BTDEVHDL dev_hdl, BTINT8* prssi);
FUNC_EXPORT BTINT32 Btsdk_GetRemoteLinkQuality(BTDEVHDL dev_hdl, BTUINT16 *plink_quality);
FUNC_EXPORT BTINT32 Btsdk_GetSupervisionTimeout(BTDEVHDL dev_hdl, BTUINT16 *ptimeout);
FUNC_EXPORT BTINT32 Btsdk_SetSupervisionTimeout(BTDEVHDL dev_hdl, BTUINT16 timeout);
FUNC_EXPORT BTINT32 Btsdk_ChangeConnectionPacketType(BTDEVHDL dev_hdl, BTUINT16 packet_type);

/* Remote Device Database Management */
FUNC_EXPORT BTDEVHDL Btsdk_GetRemoteDeviceHandle(BTUINT8 *bd_addr);
FUNC_EXPORT BTDEVHDL Btsdk_AddRemoteDevice(BTUINT8 *bd_addr);
FUNC_EXPORT BTINT32 Btsdk_DeleteRemoteDeviceByHandle(BTDEVHDL dev_hdl);
FUNC_EXPORT BTINT32 Btsdk_DeleteUnpairedDevicesByClass(BTUINT32 device_class);
FUNC_EXPORT BTUINT32 Btsdk_GetStoredDevicesByClass(BTUINT32 dev_class, BTDEVHDL *pdev_hdl, BTUINT32 max_dev_num);
FUNC_EXPORT BTUINT32 Btsdk_GetInquiredDevices(BTDEVHDL *pdev_hdl, BTUINT32 max_dev_num);
FUNC_EXPORT BTUINT32 Btsdk_GetPairedDevices(BTDEVHDL *pdev_hdl, BTUINT32 max_dev_num);
FUNC_EXPORT BTSDKHANDLE Btsdk_StartEnumRemoteDevice(BTUINT32 flag, BTUINT32 dev_class);
FUNC_EXPORT BTDEVHDL Btsdk_EnumRemoteDevice(BTSDKHANDLE enum_handle, PBtSdkRemoteDevicePropertyStru rmt_dev_prop);
FUNC_EXPORT BTINT32 Btsdk_EndEnumRemoteDevice(BTSDKHANDLE enum_handle);
FUNC_EXPORT BTINT32 Btsdk_GetRemoteDeviceAddress(BTDEVHDL dev_hdl, BTUINT8 *bd_addr);
FUNC_EXPORT BTINT32 Btsdk_GetRemoteDeviceName(BTDEVHDL dev_hdl, BTUINT8 *name, BTUINT16 *plen);
FUNC_EXPORT BTINT32 Btsdk_GetRemoteDeviceClass(BTDEVHDL dev_hdl, BTUINT32 *pdevice_class);
FUNC_EXPORT BTINT32 Btsdk_GetRemoteDeviceProperty(BTDEVHDL dev_hdl, PBtSdkRemoteDevicePropertyStru rmt_dev_prop);
FUNC_EXPORT BTINT32 Btsdk_RemoteDeviceFlowStatistic(BTDEVHDL dev_hdl, BTUINT32* rx_bytes, BTUINT32* tx_bytes);

/* Service Discovery */
BTINT32 Btsdk_BrowseRemoteServicesEx(BTDEVHDL dev_hdl, PBtSdkSDPSearchPatternStru psch_ptn, BTUINT32 ptn_num, BTSVCHDL *svc_hdl, BTUINT32 *svc_count);
BTINT32 Btsdk_BrowseRemoteServices(BTDEVHDL dev_hdl, BTSVCHDL *svc_hdl, BTUINT32 *svc_count);
BTINT32 Btsdk_RefreshRemoteServiceAttributes(BTSVCHDL svc_hdl, BtSdkRemoteServiceAttrStru *attribute);
BTINT32 Btsdk_GetRemoteServicesEx(BTDEVHDL dev_hdl, PBtSdkSDPSearchPatternStru psch_ptn, BTUINT32 ptn_num, BTSVCHDL *svc_hdl, BTUINT32 *svc_count);
BTINT32 Btsdk_GetRemoteServices(BTDEVHDL dev_hdl, BTSVCHDL *svc_hdl, BTUINT32 *svc_count);
BTINT32 Btsdk_GetRemoteServiceAttributes(BTSVCHDL svc_hdl, BtSdkRemoteServiceAttrStru *attribute);
BTSDKHANDLE Btsdk_StartEnumRemoteService(BTDEVHDL dev_hdl);
BTSVCHDL Btsdk_EnumRemoteService(BTSDKHANDLE enum_handle, PBtSdkRemoteServiceAttrStru attribute);
BTINT32 Btsdk_EndEnumRemoteService(BTSDKHANDLE enum_handle);

/* Connection Management Application Extension */
BTINT32 Btsdk_SetRemoteServiceParam(BTSVCHDL svc_hdl, BTUINT32 app_param);
BTINT32 Btsdk_GetRemoteServiceParam(BTSVCHDL svc_hdl, BTUINT32 *papp_param);

/* Connection Establishment */
FUNC_EXPORT BTINT32 Btsdk_Connect(BTSVCHDL svc_hdl, BTUINT32 lParam, BTCONNHDL *conn_hdl);
FUNC_EXPORT BTINT32 Btsdk_ConnectEx(BTDEVHDL dev_hdl, BTUINT16 service_class, BTUINT32 lParam, BTCONNHDL *conn_hdl);
typedef void  (Btsdk_Connection_Event_Ind_Func)(BTCONNHDL conn_hdl, BTUINT16 event, BTUINT8 *arg);

/* Connection Database Management */
FUNC_EXPORT BTINT32 Btsdk_GetConnectionProperty(BTCONNHDL conn_hdl, PBtSdkConnectionPropertyStru conn_prop);
FUNC_EXPORT BTSDKHANDLE Btsdk_StartEnumConnection(void);
FUNC_EXPORT BTCONNHDL Btsdk_EnumConnection(BTSDKHANDLE enum_handle, PBtSdkConnectionPropertyStru conn_prop);
FUNC_EXPORT BTINT32 Btsdk_EndEnumConnection(BTSDKHANDLE enum_handle);

/* Connection Release */
FUNC_EXPORT BTINT32 Btsdk_Disconnect(BTCONNHDL handle);

/* BlueSoleil Extend APIs */
BTUINT32 Btsdk_VDIInstallDev(BTINT8 *HardwareID,  BTINT8 *COMName);
BTUINT32 Btsdk_VDIDelModem( BTINT8 *COMName);
BTUINT32 Btsdk_GetActivationInformation(BTINT8 *SerialNumber, BTINT8 *ActivateInformation, BTUINT32 ActiveInformationLen);
BTUINT32 Btsdk_EnterUnlockCode(BTINT8 *UnlockCode);

/* 
		File Transfer Profile 
*/

/*Register a function for transfering file status
	first:		[in]first callback flag 
	last:		[in]last callback flag 
	filename:	[in]file name, only valid when first flag is set.
	filesize:	[in]total transfer file size, only valid when first flag is set.
	cursize:	[in]current transfer size
*/
typedef void  (Btsdk_FTP_STATUS_INFO_CB)(BTUINT8 first, BTUINT8 last, BTUINT8* filename, BTUINT32 filesize, BTUINT32 cursize);
void Btsdk_FTPRegisterStatusCallback4ThirdParty(BTCONNHDL conn_hdl, Btsdk_FTP_STATUS_INFO_CB *func);


/* FTP Server */
typedef BTBOOL (BTSDK_FTP_UIDealReceiveFile)(PBtSdkFileTransferReqStru pFileInfo);
void Btsdk_FTPRegisterDealReceiveFileCB4ThirdParty(BTSDK_FTP_UIDealReceiveFile* func);


/* FTP Client */
typedef void (BTSDK_FTP_UIShowBrowseFile)(BTUINT8* SYS_FIND_DATA);
BTINT32 Btsdk_FTPBrowseFolder(BTCONNHDL conn_hdl, BTUINT8 *szPath, BTSDK_FTP_UIShowBrowseFile* pShowFunc, BTUINT8 op_type);
BTINT32 Btsdk_FTPSetRmtDir(BTCONNHDL conn_hdl,BTUINT8 *szDir);	
BTINT32 Btsdk_FTPGetRmtDir(BTCONNHDL conn_hdl,BTUINT8 *szDir);
BTINT32 Btsdk_FTPCreateDir(BTCONNHDL conn_hdl,BTINT8 *szDir);
BTINT32 Btsdk_FTPDeleteDir(BTCONNHDL conn_hdl,BTINT8 *szDir);
BTINT32 Btsdk_FTPDeleteFile(BTCONNHDL conn_hdl,BTINT8 *szDir);
BTINT32 Btsdk_FTPCancelTransfer(BTCONNHDL conn_hdl);
BTINT32 Btsdk_FTPPutDir(BTCONNHDL conn_hdl, BTUINT8 *loc_dir, BTUINT8 *new_dir);
BTINT32 Btsdk_FTPPutFile(BTCONNHDL conn_hdl, BTUINT8 *loc_file, BTUINT8 *new_file);
BTINT32 Btsdk_FTPGetDir(BTCONNHDL conn_hdl, BTUINT8 *rem_dir, BTUINT8 *new_dir);
BTINT32 Btsdk_FTPGetFile(BTCONNHDL conn_hdl, BTUINT8 *rem_file, BTUINT8 *new_file);
BTBOOL Btsdk_FTPBackDir(BTCONNHDL conn_hdl);


/* 
		Object Push Profile 
*/
typedef void (Btsdk_OPP_STATUS_INFO_CB)(BTUINT8 first, BTUINT8 last, BTUINT8* filename, BTUINT32 filesize, BTUINT32 cursize);
void Btsdk_OPPRegisterStatusCallback4ThirdParty(BTCONNHDL conn_hdl, Btsdk_OPP_STATUS_INFO_CB *func);

/* Register a function for transfering file status
	first:		[in]first callback flag 
	last:		[in]last callback flag 
	filename:	[in]file name, only valid when first flag is setted.
	filesize:	[in]total transfer file size, only valid when first flag is setted.
	cursize:	[in]current transfer size
*/

/* OPP Server */
typedef BTBOOL (BTSDK_OPP_UIDealReceiveFile)(PBtSdkFileTransferReqStru pFileInfo);
void Btsdk_OPPRegisterDealReceiveFileCB4ThirdParty(BTSDK_OPP_UIDealReceiveFile* func);

/* Register a function for uploading file request from remote device.
   pFileInfo	[in/out] Specify the information of the file uploaded or to be uploaded.
				When input pFileInfo->flag is set to BTSDK_ER_CONTINUE, following operation is allowed:
				(1)If the application wants to save the file using a different name, copy the 
				new file name to pFileInfo->file_name.
				(2)If the application wants to reject the file uploading request, change pFileInfo->flag
				to one of OBEX error code except for BTSDK_ER_CONTINUE and BTSDK_ER_SUCCESS.
				(3)If the application allow to save the file, just keep pFileInfo->flag unchanged.

	return value is ignored.
*/

/* OPP Client */
BTINT32 Btsdk_OPPCancelTransfer(BTCONNHDL conn_hdl);
BTINT32 Btsdk_OPPPushObj(BTCONNHDL conn_hdl,BTUINT8 *szPushFilePath);
BTINT32 Btsdk_OPPPullObj(BTCONNHDL conn_hdl,BTUINT8 *szPullFilePath);
BTINT32 Btsdk_OPPExchangeObj(BTCONNHDL conn_hdl,BTUINT8 *szPushFilePath,BTUINT8 *szPullFilePath,BTINT32 *npushError, BTINT32 *npullError);

/* Personal Area Networking Profile */
typedef void (Btsdk_PAN_Event_Ind_Func)(BTUINT16 event, BTUINT16 len, BTUINT8 *param);
void Btsdk_PAN_RegIndCbk4ThirdParty(Btsdk_PAN_Event_Ind_Func *pfunc);


/* 
		Audio/Video Remote Control Profile
*/
/*AVRCP pass through cmd process callback function type*/
typedef void (Btsdk_AVRCP_PassThr_Cmd_Func)(BTUINT8 op_id, BTUINT8 state_flag);
/* AVRCP Target (TG) */
void Btsdk_AVRCP_RegPassThrCmdCbk4ThirdParty(Btsdk_AVRCP_PassThr_Cmd_Func *pfunc);
/* AVRCP event processing callback function type */
typedef void (Btsdk_AVRCP_Event_Ind_Func)(BTUINT16 event, BTUINT8 *param);
void Btsdk_AVRCP_RegIndCbk4ThirdParty(Btsdk_AVRCP_Event_Ind_Func * pfunc);
typedef BTBOOL (Btsdk_AVRCP_TG_Command_Cbk_Func)(BTDEVHDL hdl, BTUINT8 tl, BTUINT16 cmd_type, BTUINT8 *param);
void Btsdk_AVRCP_TGRegCommandCbk(Btsdk_AVRCP_TG_Command_Cbk_Func *pfunc);

/* AVRCP CT Response callback function type.
   Return value: Ignored currently, always return BTSDK_TRUE. */
typedef BTBOOL (Btsdk_AVRCP_CT_Response_Cbk_Func)(BTDEVHDL hdl, BTUINT16 rsp_type, BTUINT8 *param);
void Btsdk_AVRCP_CTRegResponseCbk(Btsdk_AVRCP_CT_Response_Cbk_Func *pfunc);

/*  AVRCP Function*/
void Btsdk_AVRCP_RegIndCbk(Btsdk_AVRCP_Event_Ind_Func * pfunc);
void Btsdk_AVRCP_RegPassThrCmdCbk(Btsdk_AVRCP_PassThr_Cmd_Func *pfunc);
void Btsdk_AVRCP_TGRegCommandCbk(Btsdk_AVRCP_TG_Command_Cbk_Func *pfunc);
void Btsdk_AVRCP_CTRegResponseCbk(Btsdk_AVRCP_CT_Response_Cbk_Func *pfunc);

/* CT functions */
BTINT32 Btsdk_AVRCP_PassThroughReq(PBtSdkPassThrReqStru preq);
BTINT32 Btsdk_AVRCP_Group_NavigateReq(PBtSdkGroupNaviReqStru preq);

BTINT32 Btsdk_AVRCP_GetCapabilitiesReq(BTDEVHDL hdl, PBtSdkGetCapabilitiesReqStru param);
BTINT32 Btsdk_AVRCP_ListPlayerAppSetAttrReq(BTDEVHDL hdl);
BTINT32 Btsdk_AVRCP_ListPlayerAppSetValReq(BTDEVHDL hdl, PBtSdkListPlayerAppSetValReqStru param);
BTINT32 Btsdk_AVRCP_GetCurPlayerAppSetValReq(BTDEVHDL hdl, PBtSdkGetCurPlayerAppSetValReqStru param);
BTINT32 Btsdk_AVRCP_SetCurPlayerAppSetValReq(BTDEVHDL hdl, PBtSdkSetCurPlayerAppSetValReqStru param);
BTINT32 Btsdk_AVRCP_GetPlayerAppSetAttrTxtReq(BTDEVHDL hdl, PBtSdkGetPlayerAppSetAttrTxtReqStru param);
BTINT32 Btsdk_AVRCP_GetPlayerAppSetValTxtReq(BTDEVHDL hdl, PBtSdkGetPlayerAppSetValTxtReqStru param);
BTINT32 Btsdk_AVRCP_InformCharSetReq(BTDEVHDL hdl, PBtSdkInformCharSetReqStru param);
BTINT32 Btsdk_AVRCP_InformBattStatusReq(BTDEVHDL hdl, PBtSdkInformBattStatusReqStru param);
BTINT32 Btsdk_AVRCP_GetElementAttrReq(BTDEVHDL hdl, PBtSdkGetElementAttrReqStru param);
BTINT32 Btsdk_AVRCP_GetPlayStatusReq(BTDEVHDL hdl);
BTINT32 Btsdk_AVRCP_RegNotifReq(BTDEVHDL hdl, PBtSdkRegisterNotifReqStru param);
BTINT32 Btsdk_AVRCP_SetAddressedPlayerReq(BTDEVHDL hdl, PBtSdkSetAddresedPlayerReqStru param);
BTINT32 Btsdk_AVRCP_SetBrowsedPlayerReq(BTDEVHDL hdl, PBtSdkSetAddresedPlayerReqStru param);
BTINT32 Btsdk_AVRCP_ChangePathReq(BTDEVHDL hdl, PBtSdkChangePathReqStru param);
BTINT32 Btsdk_AVRCP_GetFolderItemsReq(BTDEVHDL hdl, PBtSdkGetFolderItemReqStru param);
BTINT32 Btsdk_AVRCP_GetItemAttrReq(BTDEVHDL hdl, PBtSdkGetItemAttrReqStru param);
BTINT32 Btsdk_AVRCP_SearchReq(BTDEVHDL hdl, PBtSdkSearchReqStru param);
BTINT32 Btsdk_AVRCP_PlayItemReq(BTDEVHDL hdl, PBtSdkPlayItemReqStru param);
BTINT32 Btsdk_AVRCP_AddToNowPlayingReq(BTDEVHDL hdl, PBtSdkAddToNowPlayingReqStru param);
BTINT32 Btsdk_AVRCP_SetAbsoluteVolReq(BTDEVHDL hdl, PBtSdkSetAbsoluteVolReqStru param);

/* TG function */
BTINT32 Btsdk_AVRCP_PassThroughRsp(PBtSdkPassThrRspStru param);
BTINT32 Btsdk_AVRCP_GetCapabilitiesRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetCapabilitiesRspStru param);
BTINT32 Btsdk_AVRCP_ListPlayerAppSetAttrRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkListPlayerAppSetAttrRspStru param);
BTINT32 Btsdk_AVRCP_ListPlayerAppSetValRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkListPlayerAppSetValRspStru param);
BTINT32 Btsdk_AVRCP_GetCurPlayerAppSetValRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetCurPlayerAppSetValRspStru param);
BTINT32 Btsdk_AVRCP_SetCurPlayerAppSetValRsp(BTDEVHDL hdl, BTUINT8 tl);
BTINT32 Btsdk_AVRCP_GetPlayerAppSetAttrTxtRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetPlayerAppSettingAttrTxtRspStru param);
BTINT32 Btsdk_AVRCP_GetPlayerAppSetValTxtRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetPlayerAppSettingValTxtRspStru param);
BTINT32 Btsdk_AVRCP_InformCharSetRsp(BTDEVHDL hdl, BTUINT8 tl);
BTINT32 Btsdk_AVRCP_InformBattStatusRsp(BTDEVHDL hdl, BTUINT8 tl);
BTINT32 Btsdk_AVRCP_GetElementAttrRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetElementAttrRspStru param);
BTINT32 Btsdk_AVRCP_GetPlayStatusRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkPlayStatusRspStru param);
BTINT32 Btsdk_AVRCP_SetAddressedPlayerRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkSetAddresedPlayerRspStru param);
BTINT32 Btsdk_AVRCP_SetBrowsedPlayerRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkSetBrowsedPlayerRspStru param);
BTINT32 Btsdk_AVRCP_ChangePathRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkChangePathRspStru param);
BTINT32 Btsdk_AVRCP_GetFolderItemsRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetFolderItemRspStru param);
BTINT32 Btsdk_AVRCP_GetItemAttrRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetItemAttrRspStru param);
BTINT32 Btsdk_AVRCP_SearchRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkSearchRspStru param);
BTINT32 Btsdk_AVRCP_PlayItemRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkPlayItemRspStru param);
BTINT32 Btsdk_AVRCP_AddToNowPlayingRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkAddToNowPlayingRspStru param);
BTINT32 Btsdk_AVRCP_SetAbsoluteVolRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkSetAbsoluteVolRspStru param);
BTINT32 Btsdk_AVRCP_GeneralRejectRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGeneralRejectRspStru param);

/* Response of the RegisterNotification command */
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventPlayStatusChanged(PBtSdkPlayStatusChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventTrackChanged(PBtSdkTrackChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventTrackReachEnd(PBtSdkTrackReachEndStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventTrackReachStart(PBtSdkTrackReachStartStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventPlayPosChanged(PBtSdkPlayPosChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventBattStatusChanged(PBtSdkBattStatusChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventSysStatusChanged(PBtSdkSysStatusChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventPlayerAppSetChanged(PBtSdkPlayerAppSetChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventVolChanged(PBtSdkVolChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventAddrPlayerChanged(PBtSdkAddrPlayerChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventAvailablePlayerChanged(PBtSdkAvailablePlayerChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventUIDSChanged(PBtSdkUIDSChangedStru param);
FUNC_EXPORT BTINT32 Btsdk_AVRCP_EventNowPlayingContentChanged(PBtSdkNowPlayingContentChangedStru param);

/* 
		Serial Port Profile 
*/
BTINT32 Btsdk_InitCommObj(BTUINT8 com_idx, BTUINT16 svc_class);
BTINT32 Btsdk_DeinitCommObj(BTUINT8 com_idx);
FUNC_EXPORT BTINT16 Btsdk_GetClientPort(BTCONNHDL conn_hdl);
BTUINT8 Btsdk_GetAvailableExtSPPCOMPort(BTBOOL bIsForLocalSPPService);
BTUINT32 Btsdk_SearchAppExtSPPService(BTDEVHDL dev_hdl, PBtSdkAppExtSPPAttrStru psvc);
BTUINT32 Btsdk_ConnectAppExtSPPService(BTDEVHDL dev_hdl, PBtSdkAppExtSPPAttrStru psvc, BTCONNHDL *conn_hdl);

BTUINT32 Btsdk_CommNumToSerialNum(int comportNum);
void Btsdk_PlugOutVComm(UINT serialNum, ULONG flag);
BOOL Btsdk_PlugInVComm(UINT serialNum, ULONG *comportNumber, UINT usageType, ULONG flag, DWORD dwTimeout);
BTUINT32 Btsdk_GetASerialNum();

/* 
		Hands-Free Profile
*/

/* Prototype of APP defined callback function. */
typedef void (Btsdk_HFP_Callback)(BTCONNHDL hdl, BTUINT16 event, BTUINT8 *param, BTUINT16 len);

/* SDK HFP APIs */
BTSVCHDL Btsdk_RegisterHFPService(BTUINT8 *svc_name, BTUINT16 svc_class, BTUINT16 features);
BTUINT32 Btsdk_UnregisterHFPService(BTSVCHDL svc_hdl);
BTUINT32 Btsdk_HFP_ExtendCmd(BTCONNHDL hdl, void *cmd, BTUINT16 len, BTUINT32 timeout);

BTUINT32 Btsdk_AGAP_APPRegCbk4ThirdParty(Btsdk_HFP_Callback *pfunc);
BTUINT32 Btsdk_AGAP_ChangeInbandRingSetting(BTCONNHDL hdl, BTUINT8 inband_ring);
BTUINT32 Btsdk_AGAP_AnswerCall(BTCONNHDL hdl, BTUINT8 mode);
BTUINT32 Btsdk_AGAP_CancelCall(BTCONNHDL hdl, BTUINT8 type);
BTUINT32 Btsdk_AGAP_OriginateCall(BTCONNHDL hdl, BTUINT8 mode);
BTUINT32 Btsdk_AGAP_NetworkEvent(BTCONNHDL hdl, BTUINT8 ev, void *param);
BTUINT32 Btsdk_AGAP_VoiceRecognitionReq(BTCONNHDL hdl, BTUINT8 param);
BTUINT32 Btsdk_AGAP_VoiceTagPhoneNumRsp(BTCONNHDL hdl, void *phone_num, BTUINT8 len);
BTUINT32 Btsdk_AGAP_DialRsp(BTCONNHDL hdl, BTUINT8 err_code);
BTUINT32 Btsdk_AGAP_HoldIncomingCall(BTCONNHDL hdl);
BTUINT32 Btsdk_AGAP_AcceptHeldIncomingCall(BTCONNHDL hdl, BTUINT8 mode);
BTUINT32 Btsdk_AGAP_RejectHeldIncomingCall(BTCONNHDL hdl);
BTUINT32 Btsdk_AGAP_NetworkOperatorRsp(BTCONNHDL hdl, PBtsdk_HFP_COPSInfoStru op_info);
BTUINT32 Btsdk_AGAP_SubscriberNumberRsp(BTCONNHDL hdl, PBtsdk_HFP_PhoneInfoStru usr_info, BTUINT8 complete);
BTUINT32 Btsdk_AGAP_CurrentCallRsp(BTCONNHDL hdl, PBtsdk_HFP_CLCCInfoStru call_info, BTUINT8 complete);
BTUINT32 Btsdk_AGAP_ManufacturerIDRsp(BTCONNHDL hdl, BTINT8 *mid, BTUINT16 len);
BTUINT32 Btsdk_AGAP_ModelIDRsp(BTCONNHDL hdl, BTINT8 *mid, BTUINT16 len);
BTUINT32 Btsdk_AGAP_SendBatteryChargeIndicator(BTCONNHDL hdl, BTUINT8 indicator);
BTUINT32 Btsdk_AGAP_SendErrorMessage(BTCONNHDL hdl, BTUINT8 err_code);
BTUINT32 Btsdk_AGAP_SetSpkVol(BTCONNHDL hdl, BTUINT8 spk_vol);
BTUINT32 Btsdk_AGAP_SetMicVol(BTCONNHDL hdl, BTUINT8 mic_vol);
BTUINT32 Btsdk_AGAP_SetCurIndicatorVal(BTCONNHDL hdl, PBtsdk_HFP_CINDInfoStru indicators);
BTUINT32 Btsdk_AGAP_AudioConnTrans(BTCONNHDL hdl);
BTUINT32 Btsdk_AGAP_GetAGState(BTUINT16* agstate);
BTUINT32 Btsdk_AGAP_CurrentCallSync(BTCONNHDL hdl, PBtsdk_HFP_CLCCInfoStru call_info, BTUINT8 complete);
BTUINT32 Btsdk_AGAP_3WayCallingHandler(BTCONNHDL hdl, BTUINT16 op_code, BTUINT8 idx);
BTUINT32 Btsdk_AGAP_IsAudioConnExisted(BTBOOL*  audioconn);
BTBOOL Btsdk_AGAP_SetDialHandlerFlag (BTBOOL bFlag);

BTUINT32 Btsdk_HFAP_APPRegCbk4ThirdParty(Btsdk_HFP_Callback *pfunc);
BTUINT32 Btsdk_HFAP_GetManufacturerID(BTCONNHDL hdl, BTUINT8 *manufacturer_id, BTUINT16 *id_len);
BTUINT32 Btsdk_HFAP_GetModelID(BTCONNHDL hdl, BTUINT8 *model_id, BTUINT16 *id_len);
BTUINT32 Btsdk_HFAP_Dial(BTCONNHDL hdl, void *phone_num, BTUINT16 len);
BTUINT32 Btsdk_HFAP_MemNumDial(BTCONNHDL hdl, void *mem_location, BTUINT16 len);
BTUINT32 Btsdk_HFAP_LastNumRedial(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_SetSpkVol(BTCONNHDL hdl, BTUINT8 spk_vol);
BTUINT32 Btsdk_HFAP_SetMicVol(BTCONNHDL hdl, BTUINT8 mic_vol);
BTUINT32 Btsdk_HFAP_TxDTMF(BTCONNHDL hdl, BTUINT8 chr);
BTUINT32 Btsdk_HFAP_AudioConnTrans(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_VoiceTagPhoneNumReq(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_AnswerCall(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_CancelCall(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_VoiceRecognitionReq(BTCONNHDL hdl, BTUINT8 param);
BTUINT32 Btsdk_HFAP_DisableNREC(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_3WayCallingHandler(BTCONNHDL hdl, BTUINT16 op_code, BTUINT8 idx);
BTUINT32 Btsdk_HFAP_NetworkOperatorReq(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_SetExtendedErrors(BTCONNHDL hdl, BTUINT8 enable);
BTUINT32 Btsdk_HFAP_GetResponseHoldStatus(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_HoldIncomingCall(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_AcceptHeldIncomingCall(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_RejectHeldIncomingCall(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_GetSubscriberNumber(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_GetCurrentCalls(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_GetAGFeatures(BTCONNHDL hdl);
BTUINT32 Btsdk_HFAP_GetCurrHFState (BTUINT16 *agstate);
BTBOOL  Btsdk_HFAP_SetWaveInDevice(BTUINT8* pWaveInDevice, BTUINT32 devNamelen);
BTBOOL  Btsdk_HFAP_SetWaveOutDevice(BTUINT8* pWaveOutDevice, BTUINT32 devNamelen);

/*A2DP APIs*/
BTSVCHDL Btsdk_RegisterA2DPSRCService(void);
BTUINT32 Btsdk_UnregisterA2DPSRCService(void);
BTSVCHDL Btsdk_RegisterA2DPSNKService(BTUINT16 len, const BTUINT8 *audio_card);
BTUINT32 Btsdk_UnregisterA2DPSNKService(void);

/*HID APIS*/
BTUINT32 Btsdk_Hid_ClntUnPluggedDev(BTUINT8 * bdaddr);

/*Local SPP service*/
/* Service Manager */
BTSVCHDL Btsdk_AddServer(BtSdkLocalServerAttrStru *attribute);
BTINT32 Btsdk_RemoveServer(BTSVCHDL handle);
BTINT32 Btsdk_UpdateServerAttributes(BTSVCHDL handle, BtSdkLocalServerAttrStru *attribute);
BTINT32 Btsdk_StartServer(BTSVCHDL handle);
BTINT32 Btsdk_StopServer(BTSVCHDL handle);
BTINT32 Btsdk_GetServerAttributes(BTSVCHDL handle, BtSdkLocalServerAttrStru *attribute);
BTINT32 Btsdk_GetLocalServers(BTSVCHDL * svc_hdl, BTUINT32 * svc_count);


BTSDKHANDLE Btsdk_StartEnumLocalServer(void);
BTSVCHDL Btsdk_EnumLocalServer(BTSDKHANDLE enum_handle, PBtSdkLocalServerAttrStru attribute);
BTINT32 Btsdk_EndEnumLocalServer(BTSDKHANDLE enum_handle);

/*INI file operation API*/
/*get/write from/to file*/
BTUINT32 Btsdk_GetPrivateProfileString(BTINT8 *lpAppName, BTINT8 *lpKeyName, BTINT8 *lpDefault, BTINT8 *lpReturnedString, BTUINT32 nSize, BTINT8 *lpFileName);
BOOL Btsdk_WritePrivateProfileString(BTINT8 *lpAppName, BTINT8 *lpKeyName, BTINT8 *lpString, BTINT8 *lpFileName);
BTINT32 Btsdk_GetPrivateProfileInt(BTINT8 *lpAppName, BTINT8 *lpKeyName, BTINT32 nDefault, BTINT8 *lpFileName);
BOOL Btsdk_WritePrivateProfileInt(BTINT8 *lpAppName, BTINT8 *lpKeyName, BTINT32 nNumber, BTINT8 *lpFileName);

/*secure level for local service*/
BTUINT32 Btsdk_SetServiceSecurityLevel(BTSVCHDL svc_hdl, BTUINT8 level);
BTUINT32 Btsdk_GetServiceSecurityLevel(BTSVCHDL svc_hdl, BTUINT8 *level);

BTINT32 Btsdk_GetServerStatus(BTSVCHDL svc_hdl, BTUINT16 *status);
BTUINT32 Btsdk_SetSecurityMode(BTUINT16 secu_mode);

/*Phone Book Access Profile APIs*/
/*************** Callback function prototype ******************/
typedef void (Btsdk_PBAP_STATUS_INFO_CB)(BTUINT8 first, BTUINT8 last, BTUINT8* filename, BTUINT32 filesize, BTUINT32 cursize);

/*************** Function Prototype ******************/
BTSVCHDL Btsdk_RegisterPBAPService(BTUINT8* svc_name, PBtSdkLocalPSEServerAttrStru svr_attr, PBtSdkPBAPSvrCBStru cb_funcs);
BTINT32 Btsdk_PBAPRegisterFileIORoutines(BTCONNHDL conn_hdl, PBtSdkPBAPFileIORoutinesStru funcs);
BTINT32 Btsdk_PBAPRegisterSvrCallback(BTSVCHDL svc_hdl, PBtSdkPBAPSvrCBStru cb_funcs);
BTINT32 Btsdk_UnregisterPBAPService(BTSVCHDL svc_hdl);

BTINT32 Btsdk_PBAPRegisterStatusCallback(BTCONNHDL conn_hdl, Btsdk_PBAP_STATUS_INFO_CB *func);

void Btsdk_PBAPFilterComposer(BTUINT8 *filter, BTUINT8 flag);
BTINT32 Btsdk_PBAPCancelTransfer(BTCONNHDL conn_hdl);
BTINT32 Btsdk_PBAPPullPhoneBook(BTCONNHDL conn_hdl, BTUINT8 *path, PBtSdkPBAPParamStru param, BTSDKHANDLE file_hdl);
BTINT32 Btsdk_PBAPSetPath(BTCONNHDL conn_hdl, BTUINT8 *folder);
BTINT32 Btsdk_PBAPPullCardList(BTCONNHDL conn_hdl, BTUINT8 *folder, PBtSdkPBAPParamStru param, BTSDKHANDLE file_hdl);
BTINT32 Btsdk_PBAPPullCardEntry(BTCONNHDL conn_hdl, BTUINT8 *name, PBtSdkPBAPParamStru param, BTSDKHANDLE file_hdl);

/************************************************************************/
/* BT2.1 support  begin                                                                    */
/************************************************************************/
BTINT32 Btsdk_IOCapReqReply(BTDEVHDL dev_hdl, BTUINT8 io_cap, BTUINT8 oob_data_present, BTUINT8 authen_req);
BTINT32 Btsdk_IOCapReqReject(BTDEVHDL dev_hdl);
BTINT32 Btsdk_UsrCfmReqReply(BTDEVHDL dev_hdl, BTBOOL accepted);
BTINT32 Btsdk_PasskeyReqReply(BTDEVHDL dev_hdl, BTUINT32 num_value);
BTINT32 Btsdk_RemOOBDataReqReply(BTDEVHDL dev_hdl, BTUINT8 *c_val, BTUINT8 *r_val);

/************************************************************************/
/* BT2.1 support end                                                                     */
/************************************************************************/
BTINT32 Btsdk_ExecuteHCICommandEx(BTUINT16 cmd_idx, BTUINT32 cmd_len, BTUINT8 *cmd_param, BTUINT32 ev_len, BTUINT8 *ev_param);

/************************************************************************/
/* LE support end                                                                     */
/************************************************************************/
BTINT32 Btsdk_GATTGetServices(BTDEVHDL hDevice, BTUINT16 ServicesBufferCount, PBtsdkGATTServiceStru ServicesBuffer, BTUINT16* ServicesBufferActual, BTUINT32 Flags);
BTINT32 Btsdk_GATTGetIncludedServices(BTDEVHDL hDevice, PBtsdkGATTServiceStru ParentService, BTUINT16 IncludedServicesBufferCount, PBtsdkGATTServiceStru IncludedServicesBuffer, BTUINT16* IncludedServicesBufferActual, BTUINT32 Flags);
BTINT32 Btsdk_GATTGetCharacteristics(BTDEVHDL hDevice, PBtsdkGATTServiceStru Service, BTUINT16 CharacteristicsBufferCount, PBtsdkGATTCharacteristicStru CharacteristicsBuffer, BTUINT16* CharacteristicsBufferActual, BTUINT32 Flags);
BTINT32 Btsdk_GATTGetCharacteristicValue(BTDEVHDL hDevice, PBtsdkGATTCharacteristicStru Characteristic, BTUINT16 CharacteristicValueDataSize, PBtsdkGATTCharacteristicValueStru CharacteristicValue, BTUINT16* CharacteristicValueSizeRequired, BTUINT32 Flags);
BTINT32 Btsdk_GATTGetDescriptors(BTDEVHDL hDevice, PBtsdkGATTCharacteristicStru Characteristic, BTUINT16 DescriptorsBufferCount, PBtsdkGATTDescriptorStru DescriptorsBuffer, BTUINT16* DescriptorsBufferActual, BTUINT32 Flags);
BTINT32 Btsdk_GATTGetDescriptorValue(BTDEVHDL hDevice, PBtsdkGATTDescriptorStru Descriptor, BTUINT16 DescriptorValueDataSize, PBtsdkGATTDescriptorValueStru DescriptorValue, BTUINT16* DescriptorValueSizeRequired, BTUINT32 Flags);
BTINT32 Btsdk_GATTBeginReliableWrite(BTDEVHDL hDevice, BTSDKHANDLE *ReliableWriteContext, BTUINT32 Flags);
BTINT32 Btsdk_GATTSetCharacteristicValue( BTDEVHDL hDevice, PBtsdkGATTCharacteristicStru Characteristic, PBtsdkGATTCharacteristicValueStru CharacteristicValue, BTSDKHANDLE ReliableWriteContext, BTUINT32 Flags);
BTINT32 Btsdk_GATTEndReliableWrite(BTDEVHDL hDevice, BTSDKHANDLE ReliableWriteContext, BTUINT32 Flags);
BTINT32 Btsdk_GATTAbortReliableWrite(BTDEVHDL hDevice, BTSDKHANDLE ReliableWriteContext, BTUINT32 Flags);
BTINT32 Btsdk_GATTSetDescriptorValue(BTDEVHDL hDevice, PBtsdkGATTDescriptorStru Descriptor, PBtsdkGATTDescriptorValueStru DescriptorValue, BTUINT32 Flags);
BTINT32 Btsdk_GATTCloseSession(BTDEVHDL hDevice, BTUINT32 Flags);
BTINT32 Btsdk_GetLEDeviceAppearance(BTDEVHDL dev_hdl, BTUINT16* appearance);

typedef void FNBLUETOOTH_GATT_NOTIFICATION_CALLBACK(BTUINT16 ChangedAttributeHandle, BTUINT32 CharacteristicValueDataSize, PBtsdkGATTCharacteristicValueStru CharacteristicValue, BTLPVOID Context);
BTINT32 Btsdk_GATTRegisterEvent(BTDEVHDL hDevice, BTSDK_GATT_EVENT_TYPE EventType, BTLPVOID EventParameter, FNBLUETOOTH_GATT_NOTIFICATION_CALLBACK *Callback, BTLPVOID CallbackContext, BTSDKHANDLE* pEventHandle, BTUINT32 Flags);
BTINT32 Btsdk_GATTUnregisterEvent(BTSDKHANDLE EventHandle, BTUINT32 Flags);
BTINT32 Btsdk_GetRemoteDeviceType(BTDEVHDL dev_hdl);

#endif