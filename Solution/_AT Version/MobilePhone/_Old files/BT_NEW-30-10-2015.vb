﻿Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.AutoResetEvent
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.IO

Imports MobilePhone.SMS.Decoder
Imports MobilePhone.SMS.Encoder.SMS
Imports MobilePhone.SMS.Encoder.ConcatenatedShortMessage
'NEW

Public Class BT

    'Type	Definition	Datatype
    'BTINT8	8-bit ANSI character.	Sbyte
    'BTUINT8	8-bit unsigned integer.	Byte
    'BTBOOL	Boolean variable (Should be BTSDK_TRUE or BTSDK_FALSE)	Boolean
    'BTINT16	16-bit signed integer.	Int16
    'BTUINT16	16-bit unsigned integer.	UInt16
    'BTINT32	32-bit signed integer.	Int32
    'BTUINT32	32-bit unsigned integer.	UInt32
    'BTLPVOID	Pointer to any type.	Byte
    'BTDEVHDL	Handle to a device object.	
    'BTSVCHDL	Handle to a service object.	
    'BTCONNHDL	Handle to a connection object.	
    'BTSHCHDL	Handle to a shortcut object.	
    'BTSDKHANDLE	Handle to any object.	Uinteger
    'BTUCHAR	8-bit ANSI character.	Sbyte

#Region "Structures"
    '** Section 5.3.1 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkCallbackStru
        Public type As UInt16                                   'type of callback*/
        Public func As connEventCallback                        'callback function*/
    End Structure
    '** Section 5.3.2 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkLocalLMPInfoStru
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)>
        Public lmp_feature() As Byte
        Public manuf_name As UInt16
        Public lmp_subversion As UInt16
        Public lmp_version As Byte
        Public hci_version As Byte
        Public hci_revision As UInt16
        Public country_code As Byte
    End Structure
    '** Section 5.3.3 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkVendorCmdStru
        Public ocf As UInt16
        Public param_len As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)>
        Public param() As Byte
    End Structure
    '** Section 5.3.4 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkEventParamStru
        Public ev_code As Byte
        Public param_len As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)>
        Public param() As Byte
    End Structure
    '** Section 5.3.5 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkRemoteLMPInfoStru
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)>
        Public lmp_feature() As Byte
        Public manuf_name As UInt16
        Public lmp_subversion As UInt16
        Public lmp_version As Byte
    End Structure
    '** Section 5.3.6 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkRemoteDevicePropertyStru
        Public mask As UInt32
        Public dev_hdl As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_BDADDR_LEN)>
        Public bd_addr() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_DEVNAME_LEN)>
        Public name() As String
        Public dev_class As String
        Public lmp_info As BtSdkRemoteLMPInfoStru
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_LINKKEY_LEN)>
        Public link_key() As String
    End Structure
    '** Section 5.3.7 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkHoldModeStru
        Public conn_hld As UInt16
        Public max As UInt16
        Public min As UInt16
    End Structure
    '** Section 5.3.12 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkRemoteServiceAttrStru
        Public mask As UInt16 'UInt32
        Public service_class As UInt16
        Public dev_hndl As UInt32
        '<MarshalAs(UnmanagedType.ByValArray, SizeConst:=80, ArraySubType:=UnmanagedType.I1)> _
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_SERVICENAME_MAXLENGTH)> _
        Public svc_name() As Byte
        Public ext_attributes As IntPtr
        Public status As UInt16
    End Structure
    '** Section 5.3.14 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkConnectionPropertyStru
        Public ReadOnly raw As UInt32
        Public device_handle As UInt32
        Public service_handle As UInt32
        Public service_class As UInt16
        Public duration As UInt32
        Public received_bytes As UInt32
        Public sent_bytes As UInt32
        Public ReadOnly Property role As Byte 'Public ReadOnly Property role As UInt32
            Get
                Return raw And &H3
            End Get
        End Property
        Public ReadOnly Property result As UInt32
            Get
                Return (raw >> 2)
            End Get
        End Property
    End Structure
    '** Section 6.2.1.3 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkLocalPSEServerAttrStru
        Public size As UInt32
        Public mask As UInt16
        '<MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_PATH_MAXLENGTH)> _
        Public root_dir As String
        '<MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_PBAP_MAX_DELIMITER)> _
        Public path_delimiter As String
        Public repositories As String
    End Structure
    '** Section 6.2.1.4 in SDK 2.1.3 **
    'MAP Structure (Messages)
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkLocalMASServerAttrStru
        Public size As UInt32
        Public mask As UInt16
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_PATH_MAXLENGTH + 1)> _
        Public root_dir() As String
        Public path_delimiter As String
        Public mas_inst_id As String
        Public sup_msg_types As String
    End Structure
    '** Section 6.2.3.1 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Btsdk_HFP_COPSInfo 'Used By +COPS
        Public mode As Byte                                     'current mode and provides no information with regard to the name of the operator */
        Public format As Byte                                   'the format of the operator parameter string */
        Public operator_len As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)> _
        Public operator_name() As SByte
    End Structure
    '** Section 6.2.3.2 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Btsdk_HFP_PhoneInfo 'Used By +BINP, +CNUM, +CLIP, +CCWA
        Public type As Byte                                     'the format of the phone number provided
        Public service As Byte                                  'Indicates which service this phone number relates to. Shall be either 4 (voice) or 5 (fax).
        Public num_len As Byte                                  'the length of the phone number provided
        '<MarshalAs(UnmanagedType.ByValArray, SizeConst:=32, ArraySubType:=UnmanagedType.I1)> _
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> _
        Public number() As SByte                                'subscriber number, the length shall be PHONENUM_MAX_DIGITS
        Public name_len As Byte                                 'length of subaddr
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)> _
        Public alpha_str() As SByte                             'string type subaddress of format specified by <cli_validity>
    End Structure
    '** Section 6.2.3.3 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Btsdk_HFP_CLCCInfo 'Used By +CLCC
        Public idx As Byte                                      'The numbering (start with 1) of the call given by the sequence of setting up or receiving the calls */
        Public dir As Byte                                      'Direction, 0=outgoing, 1=incoming */
        Public status As Byte                                   '0=active, 1=held, 2=dialling(outgoing), 3=alerting(outgoing), 4=incoming(incoming), 5=waiting(incoming) */
        Public mode As Byte                                     '0=voice, 1=data, 2=fax */
        Public mpty As Byte                                     '0=not multiparty, 1=multiparty */
        Public type As Byte                                     'the format of the phone number provided */
        Public num_len As Byte                                  'the length of the phone number provided */	
        '<MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)> _
        Public number As SByte 'Public number() As SByte
    End Structure
    '** Section 6.2.3.4 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Btsdk_HFP_CINDInfoStru 'Used by +CSQ
        Public service As Byte                                  'Indicates the status of service. 0 = unavailable, 1 = available
        Public callstatus As Byte                               'Indicates the status of active call. 0 = no active call, 1 = on an active call
        Public callsetup As Byte                                'Indicates the status of callsetup. 0 = no callsetup, 1 = incoming, 2 = outgoing, 3 = outalert
        Public callheld As Byte                                 'Indicates the status of callheld. 0 = no callheld, 1 = active-hold, 2 = onhold
        Public signal As Byte                                   'The strength of signal. 0~5
        Public roam As Byte                                     'Indicates the status of roam. 0 = no roam, 1 = roam
        Public battchg As Byte                                  'The strength of signal. The range is 0~5
    End Structure
    '** Section 6.2.3.6 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Btsdk_HFP_ATCmdResult
        Public cmd_code As UInt16
        Public result_code As Byte
    End Structure
    '** Section 6.2.3.8 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkRmtPSESvcAttrStru
        Public size As UInt32
        Public mask As UInt16
        Public repositories As String
    End Structure


    '** Section 6.2.3.9 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkPassThrReqStru
        Public dev_hdl As UInt32
        Public state_flag As Byte
        Public op_id As Byte
        Public length As Byte
        Public op_data As Byte
    End Structure



    '** Section 6.2.3.12 in SDK 2.1.3 **
    'Battery AVCR Info
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkInformBattStatusReqStru
        Public size As UInt32
        Public Id As UInt16
    End Structure

    '** Section 6.2.3.12 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkPlayItemReqStru
        Public size As UInt32
        Public scope As Byte
        Public uid() As SByte
        Public uid_counter As UInt16
    End Structure


    '** Section 6.2.3.74 in SDK 2.1.3 **
    'PBAP Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkPBAPParamStru
        Public mask As UInt16
        '<MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> _
        Public filter As Byte
        Public max_count As UInt16
        Public list_offset As UInt16
        Public order As Byte
        Public format As Byte
        Public search_val As IntPtr
        Public search_attrib As Byte
        Public missed_calls As Byte
        Public pb_size As UInt16
    End Structure
    '** Section 6.2.3.75 in SDK 2.1.3 **
    'ajout pierrotm77 9-7-2015
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure BtSdkPBAPParserRoutinesStru
        Public parse_open As Btsdk_vCardParser_Open_Func
        Public get_prop As Btsdk_vCardParser_GetProperty_Func
        Public parse_close As Btsdk_vCardParser_Close_Func
        Public parse_free As Btsdk_vCardParser_FreeProperty_Func
        Public parse_findfirst As Btsdk_vCardParser_FindFirstProperty_Func
        Public parse_findnext As Btsdk_vCardParser_FindNextProperty_Func
        Public parse_findclose As Btsdk_vCardParser_FindPropertyClose_Func
    End Structure
    Private Delegate Function Btsdk_vCardParser_Open_Func(file_hdl As Int32) As Byte    'p.520
    Private Delegate Function Btsdk_vCardParser_GetProperty_Func(ByRef v_obj As Byte, ByRef prop As Byte, ByRef len As Int32) As Byte
    Private Delegate Function Btsdk_vCardParser_Close_Func(ByRef v_obj As Byte) As Byte
    Private Delegate Function Btsdk_vCardParser_FreeProperty_Func(ByRef buf As Byte) As Byte
    Private Delegate Function Btsdk_vCardParser_FindFirstProperty_Func(ByRef v_obj As Byte, ByRef prop As Byte, ByRef len As Int32) As Byte
    Private Delegate Function Btsdk_vCardParser_FindNextProperty_Func(file_hdl As Int32, ByRef len As Int32) As Byte
    Private Delegate Function Btsdk_vCardParser_FindPropertyClose_Func(file_hdl As Int32) As Byte


    '** Section 6.2.3.76 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure BtSdkPBAPFindFileRoutinesStru
        Public find_first As Btsdk_FindFirstFile_Func
        Public find_next As Btsdk_FindNextFile_Func
        Public find_close As Btsdk_FindFileClose_Func
    End Structure
    Private Delegate Function Btsdk_FindFirstFile_Func(path As String) As Int32
    Private Delegate Function Btsdk_FindNextFile_Func(find_hdl As Int32, file_name As String) As Int32    'p.528
    Private Delegate Function Btsdk_FindFileClose_Func(find_hdl As Int32) As Int32
    'ajout pierrotm777 9-7-2015

    '** Section 6.2.3.77 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkPBAPFileIORoutinesStru
        Public open_file As Btsdk_OpenFile_Func '** Section 6.3.9.1.12 in SDK 2.1.3 **
        Public create_file As Btsdk_CreateFile_Func '** Section 6.3.9.1.13 in SDK 2.1.3 **
        Public write_file As Btsdk_WriteFile_Func '** Section 6.3.9.1.14 in SDK 2.1.3 **
        Public read_file As Btsdk_ReadFile_Func '** Section 6.3.9.1.15 in SDK 2.1.3 **
        Public get_file_size As Btsdk_GetFileSize_Func '** Section 6.3.9.1.16 in SDK 2.1.3 **
        Public rewind_file As Btsdk_RewindFile_Func '** Section 6.3.9.1.17 in SDK 2.1.3 **
        Public close_file As Btsdk_CloseFile_Func '** Section 6.3.9.1.18 in SDK 2.1.3 **
    End Structure
    Public Delegate Function Btsdk_OpenFile_Func(file_name As UInt32) As Int32
    Public Delegate Function Btsdk_CreateFile_Func(file_name As UInt32) As Int32
    Public Delegate Function Btsdk_WriteFile_Func(file_hdl As UInt32, buf As IntPtr, bytes_to_write As UInt32) As Int32
    Public Delegate Function Btsdk_ReadFile_Func(file_hdl As UInt32, buf As Byte, len As UInt32) As Int32
    Public Delegate Function Btsdk_GetFileSize_Func(file_hdl As UInt32) As Int32
    Public Delegate Function Btsdk_RewindFile_Func(file_hdl As UInt32, offset As UInt32) As Int32
    Public Delegate Function Btsdk_CloseFile_Func(file_hdl As UInt32) As Int32

    '** Section 6.2.3.78 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkPBAPDirCtrlRoutinesStru
        Public change_dir As Btsdk_ChangDir_Func '** Section 6.3.9.1.19 in SDK 2.1.3 **
        Public create_dir As Btsdk_CreateDir_Func '** Section 6.3.9.1.20 in SDK 2.1.3 **
    End Structure
    Public Delegate Function Btsdk_ChangDir_Func(path As Byte) As Int32
    Public Delegate Function Btsdk_CreateDir_Func(path As Byte) As Int32

    '** Section 6.2.3.79 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure BtSdkPBAPSvrCBStru
        Public cardparser_rtns As BtSdkPBAPParserRoutinesStru
        Public findfile_rtns As BtSdkPBAPFindFileRoutinesStru
        Public fileio_rtns As BtSdkPBAPFileIORoutinesStru
        Public dirctrl_rtns As BtSdkPBAPDirCtrlRoutinesStru
        Public get_new_missedcalls As Btsdk_PBAP_GetMissedCalls_Func '** Sectiuon 6.3.9.1.21 in SDK 2.1.3 **
    End Structure
    Private Delegate Function Btsdk_PBAP_GetMissedCalls_Func(path As Byte) As Int32

    '** Section 6.2.3.82 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkRmtMASSvcAttrStru
        Dim size As UInt32
        Dim mask As UInt16
        Dim mas_inst_id As String
        Dim sup_msg_types As String
    End Structure
    '** Section 6.2.3.83 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPEvReportObjStru
        Public ev_type As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSGTYPE_LEN)> _
        Public msg_type() As Byte '[BTSDK_MAP_MSGTYPE_LEN]
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSGHDL_LEN)> _
        Public msg_handle() As Byte '[BTSDK_MAP_MSGHDL_LEN]
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_PATH_LEN)> _
        Public folder() As Byte '[BTSDK_MAP_PATH_LEN]
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_PATH_LEN)> _
        Public old_folder() As Byte '[BTSDK_MAP_PATH_LEN]
        Public mas_inst_id As Byte
    End Structure
    '** Section 6.2.3.84 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPFindFolderRoutinesStru
        Public find_first_folder As Btsdk_MAP_FindFirstFolder_Func '** Section 6.3.10.1.3 in SDK 2.1.3 **
        Public find_next_folder As Btsdk_MAP_FindNextFolder_Func '** Section 6.3.10.1.4 in SDK 2.1.3 **
        Public find_folder_close As Btsdk_MAP_FindFolderClose_Func '** Section 6.3.10.1.5 in SDK 2.1.3 **
    End Structure
    Public Delegate Function Btsdk_MAP_FindFirstFolder_Func(path As Byte, pfd As BtSdkMAPFolderObjStru) As Int32
    Public Delegate Function Btsdk_MAP_FindNextFolder_Func(find_hdl As UInt32, pfd As BtSdkMAPFolderObjStru) As Int32
    Public Delegate Function Btsdk_MAP_FindFolderClose_Func(find_hdl As UInt32) As Int32

    '** Section 6.2.3.85 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPFindMsgRoutinesStru
        Public find_first_msg As Btsdk_MAP_FindFirstMsg_Func '** Section 6.3.10.1.6 in SDK 2.1.3 **
        Public find_next_msg As Btsdk_MAP_FindNextMsg_Func '** Section 6.3.10.1.7 in SDK 2.1.3 **
        Public find_msg_close As Btsdk_MAP_FindMsgClose_Func '** Section 6.3.10.1.8 in SDK 2.1.3 **
    End Structure
    Public Delegate Function Btsdk_MAP_FindFirstMsg_Func(path As Byte, pfilter As BtSdkMAPMsgFilterStru, pmsg As BtSdkMAPMsgObjStru) As Int32
    Public Delegate Function Btsdk_MAP_FindNextMsg_Func(find_hdl As UInt32, pfilter As BtSdkMAPMsgFilterStru, pmsg As BtSdkMAPMsgObjStru) As Int32
    Public Delegate Function Btsdk_MAP_FindMsgClose_Func(find_hdl As UInt32) As Int32

    '** Section 6.2.3.86 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPFileIORoutinesStru
        Public open_file As Btsdk_OpenFile_Func '** Section 6.3.10.1.13 in SDK 2.1.3 **
        Public create_file As Btsdk_CreateFile_Func '** Section 6.3.10.1.14 in SDK 2.1.3 **
        Public write_file As Btsdk_WriteFile_Func '** Section 6.3.10.1.15 in SDK 2.1.3 **
        Public read_file As Btsdk_ReadFile_Func '** Section 6.3.10.1.16 in SDK 2.1.3 **
        Public get_file_size As Btsdk_GetFileSize_Func '** Section 6.3.10.1.17 in SDK 2.1.3 **
        Public rewind_file As Btsdk_RewindFile_Func '** Section 6.3.10.1.18 in SDK 2.1.3 **
        Public close_file As Btsdk_CloseFile_Func '** Section 6.3.10.1.19 in SDK 2.1.3 **
    End Structure

    '** Section 6.2.3.87 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPMsgIORoutinesStru
        Public modify_msg_status As Btsdk_MAP_ModifyMsgStatus_Func '** Section 6.3.10.1.9 in SDK 2.1.3 **
        Public create_bmsg_file As Btsdk_MAP_CreateBMsgFile_Func '** Section 6.3.10.1.10 in SDK 2.1.3 **
        Public open_bmsg_file As Btsdk_MAP_OpenBMsgFile_Func '** Section 6.3.10.1.11 in SDK 2.1.3 **
        Public push_msg As Btsdk_MAP_PushMsg_Func '** Section 6.3.10.1.12 in SDK 2.1.3 **
    End Structure
    Public Delegate Function Btsdk_MAP_ModifyMsgStatus_Func(msg_info As BtSdkMAPMsgStatusStru) As Int32
    Public Delegate Function Btsdk_MAP_CreateBMsgFile_Func(cur_path As Byte, msg_hdl As BtSdkMAPMsgHandleStru) As Int32
    Public Delegate Function Btsdk_MAP_OpenBMsgFile_Func(msg_info As BtSdkMAPGetMsgParamStru) As Int32
    Public Delegate Function Btsdk_MAP_PushMsg_Func(cur_path As Byte, msg_info As BtSdkMAPPushMsgParamStru) As Int32

    '** Section 6.2.3.88 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPMSEStatusRoutinesStru
        Public register_notification As Btsdk_MAP_RegisterNotification_Func '** Section 6.3.10.1.6 in SDK 2.1.3 **
        Public update_inbox As Btsdk_MAP_UnpdateInbox_Func '** Section 6.3.10.1.6 in SDK 2.1.3 **
        Public get_mse_time As Btsdk_MAP_GetMSETime_Func '** Section 6.3.10.1.6 in SDK 2.1.3 **
    End Structure
    Public Delegate Function Btsdk_MAP_RegisterNotification_Func(mns_conn_hdl As UInt32, mas_svc_hdl As UInt32, turn_on As Boolean) As Int32
    Public Delegate Function Btsdk_MAP_UnpdateInbox_Func() As Int32
    Public Delegate Function Btsdk_MAP_GetMSETime_Func(mse_time As BtSdkMAPMSETimeStru) As Int32

    '** Section 6.2.3.89 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMASSvrCBStru
        Public find_folder_rtns As BtSdkMAPFindFolderRoutinesStru
        Public find_msg_rtns As BtSdkMAPFindMsgRoutinesStru
        Public file_io_rtns As BtSdkMAPFileIORoutinesStru
        Public msg_io_rtns As BtSdkMAPMsgIORoutinesStru
        Public mse_status_rtns As BtSdkMAPMSEStatusRoutinesStru
    End Structure
    '** Section 6.2.3.90 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPGetFolderListParamStru
        Dim mask As UInt16
        Dim max_count As UInt16
        Dim start_off As UInt16
        Dim list_size As UInt16
    End Structure
    '** Section 6.2.3.91 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPGetMsgListParamStru
        Public mask As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_FOLDER_LEN)> _
        Public folder() As Byte
        Public max_count As UInt16
        Public start_off As UInt16
        Public param_mask As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_TIME_LEN)> _
        Public filter_period_begin() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_TIME_LEN)> _
        Public filter_period_end() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_USERNAME_LEN)> _
        Public filter_originator() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_USERNAME_LEN)> _
        Public filter_recipient() As Byte
        Public filter_msg_type As Byte
        Public filter_read_status As Byte
        Public filter_priority As Byte
        Public subject_length As Byte
        Public list_size As Byte
        Public new_msg As Boolean
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSE_TIME_LEN)> _
        Public mse_time() As Byte
    End Structure
    '** Section 6.2.3.92 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPGetMsgParamStru
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSGHDL_LEN)> _
        Public msg_handle() As SByte
        Public charset As String
        Public attachment As Boolean
        Public fraction_req As String
        Public fraction_deliver As String
    End Structure
    '** Section 6.2.3.93 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPPushMsgParamStru
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_FOLDER_LEN)> _
        Public folder() As String
        Public save_copy As Boolean
        Public retry As Boolean
        Public charset As String
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSGHDL_LEN)> _
        Public msg_handle() As String
    End Structure
    '** Section 6.2.3.94 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPFolderObjStru
        Public size As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_FOLDER_LEN)> _
        Public name() As String
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_TIME_LEN)> _
        Public create_time() As String
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_TIME_LEN)> _
        Public access_time() As String
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_TIME_LEN)> _
        Public modify_time() As String
    End Structure
    '** Section 6.2.3.95 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPMsgObjStru
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSGHDL_LEN)> _
        Public msg_handle() As Byte
        Public mask As UInt32
        Public msg_size As UInt32
        Public attachment_size As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_SUBJECT_LEN)> _
        Public subject() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_USERNAME_LEN)> _
        Public sender_name() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_ADDR_LEN)> _
        Public sender_addr() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_ADDR_LEN)> _
        Public replyto_addr() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_USERNAME_LEN)> _
        Public recipient_name() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_ADDR_LEN)> _
        Public recipient_addr() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSGTYPE_LEN)> _
        Public msg_type() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_TIME_LEN)> _
        Public date_time() As Byte
        Public reception_status As Byte
        Public text As Boolean
        Public read As Boolean
        Public sent As Boolean
        Public protect As Boolean
        Public priority As Boolean
    End Structure
    '** Section 6.2.3.96 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPMsgFilterStru
        Public mask As UInt32
        Public param_mask As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_TIME_LEN)> _
        Public filter_period_begin() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_TIME_LEN)> _
        Public filter_period_end() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_USERNAME_LEN)> _
        Public filter_originator() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_USERNAME_LEN)> _
        Public filter_recipient() As Byte
        Public filter_msg_type As Byte        '/* BTSDK_MAP_FILTEROUT_NO, etc.*/
        Public filter_read_status As Byte     '/* BTSDK_MAP_MSG_FILTER_ST_ALL, etc.*/
        Public filter_priority As Byte        '/* BTSDK_MAP_MSG_FILTER_PRI_ALL, etc.*/
        Public subject_length As Byte
    End Structure
    '** Section 6.2.3.97 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPMsgStatusStru
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSGHDL_LEN)>
        Public msg_handle() As Byte    'Handle of the message the status of which shall be modified. It is a null-terminated UTF-8 string with 16 hexadecimal digits.'
        Public status_indicator As Byte
        Public status_value As Byte
    End Structure
    '** Section 6.2.3.98 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPMSETimeStru
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSE_TIME_LEN)>
        Public mse_time() As Byte
    End Structure
    '** Section 6.2.3.99 in SDK 2.1.3 **
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkMAPMsgHandleStru
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_MAP_MSGHDL_LEN)> _
        Public msg_handle() As Byte
    End Structure


#End Region

#Region "Constantes"
    '/* Max size value used in service attribute structures */
    Const BTSDK_SERVICENAME_MAXLENGTH As UInt16 = 80

    Const BTSDK_BDADDR_LEN As UInt32 = 6
    Const BTSDK_DEVNAME_LEN As UInt32 = 32
    Const BTSDK_LINKKEY_LEN As UInt32 = 32
    Const BTSDK_PATH_MAXLENGTH As UInt32 = 256


    '/* Type of Connection Event */
    Const BTSDK_APP_EV_CONN_IND As UInt32 = 1
    Const BTSDK_APP_EV_DISC_IND As UInt32 = 2
    Const BTSDK_APP_EV_CONN_CFM As UInt32 = 7
    Const BTSDK_APP_EV_DISC_CFM As UInt32 = 8
    Const BTSDK_CONNECTION_EVENT_IND As UInt32 = 9

    '/* Class of Service */ 5.2.2 SDK 2.1.3
    Const BTSDK_CLS_SERIAL_PORT As UInt32 = &H1101              'Serial Port service.
    Const BTSDK_CLS_LAN_ACCESS As UInt32 = &H1102
    Const BTSDK_CLS_DIALUP_NET As UInt32 = &H1103               'Dial-up Networking service.
    Const BTSDK_CLS_IRMC_SYNC As UInt32 = &H1104
    Const BTSDK_CLS_OBEX_OBJ_PUSH As UInt32 = &H1105            'Object Push service.
    Const BTSDK_CLS_OBEX_FILE_TRANS As UInt32 = &H1106          'File Transfer service.
    Const BTSDK_CLS_IRMC_SYNC_CMD As UInt32 = &H1107
    Const BTSDK_CLS_HEADSET As UInt32 = &H1108
    Const BTSDK_CLS_CORDLESS_TELE As UInt32 = &H1109
    Const BTSDK_CLS_AUDIO_SOURCE As UInt32 = &H110A             'Audio Source service.
    Const BTSDK_CLS_AUDIO_SINK As UInt32 = &H110B
    Const BTSDK_CLS_AVRCP_TG As UInt32 = &H110C                 'A/V Remote Control Target service.
    Const BTSDK_CLS_ADV_AUDIO_DISTRIB As UInt32 = &H110D
    Const BTSDK_CLS_AVRCP_CT As UInt32 = &H110E
    Const BTSDK_CLS_VIDEO_CONFERENCE As UInt32 = &H110F
    Const BTSDK_CLS_INTERCOM As UInt32 = &H1110
    Const BTSDK_CLS_FAX As UInt32 = &H1111
    Const BTSDK_CLS_HEADSET_AG As UInt32 = &H1112               'Headset Audio Gateway service.
    Const BTSDK_CLS_WAP As UInt32 = &H1113
    Const BTSDK_CLS_WAP_CLIENT As UInt32 = &H1114
    Const BTSDK_CLS_PAN_PANU As UInt32 = &H1115
    Const BTSDK_CLS_PAN_NAP As UInt32 = &H1116
    Const BTSDK_CLS_PAN_GN As UInt32 = &H1117
    Const BTSDK_CLS_DIRECT_PRINT As UInt32 = &H1118
    Const BTSDK_CLS_REF_PRINT As UInt32 = &H1119
    Const BTSDK_CLS_IMAGING As UInt32 = &H111A
    Const BTSDK_CLS_IMAG_RESPONDER As UInt32 = &H111B
    Const BTSDK_CLS_IMAG_AUTO_ARCH As UInt32 = &H111C
    Const BTSDK_CLS_IMAG_REF_OBJ As UInt32 = &H111D
    Const BTSDK_CLS_HANDSFREE As UInt32 = &H111E
    Const BTSDK_CLS_HANDSFREE_AG As UInt32 = &H111F             'Hands-free Audio Gateway service.
    Const BTSDK_CLS_DPS_REF_OBJ As UInt32 = &H1120
    Const BTSDK_CLS_REFLECTED_UI As UInt32 = &H1121
    Const BTSDK_CLS_BASIC_PRINT As UInt32 = &H1122
    Const BTSDK_CLS_PRINT_STATUS As UInt32 = &H1123
    Const BTSDK_CLS_HID As UInt32 = &H1124
    Const BTSDK_CLS_HCRP As UInt32 = &H1125
    Const BTSDK_CLS_HCR_PRINT As UInt32 = &H1126
    Const BTSDK_CLS_HCR_SCAN As UInt32 = &H1127
    Const BTSDK_CLS_SIM_ACCESS As UInt32 = &H112D               'SIM Card Access service
    Const BTSDK_CLS_PBAP_PCE As UInt32 = &H112E                 'PBAP Phonebook Client Equipment service.
    Const BTSDK_CLS_PBAP_PSE As UInt32 = &H112F                 'PBAP Phonebook Server Equipment service.
    Const BTSDK_CLS_PHONEBOOK_ACCESS As UInt32 = &H1130         'Phonebook Access service.
    Const BTSDK_CLS_PNP_INFO As UInt32 = &H1200                 'Bluetooth Device Identification.

    '5.2.3 Class of Device/Service Field
    Const DEVICE_CLASS_MASK As UInt32 = &H1FFC                  'Mask of device class
    Const BTSDK_DEVCLS_COMPUTER As UInt32 = &H100               'Computer major device class.
    Const BTSDK_COMPCLS_UNCLASSIFIED As UInt32 = &H100          'Uncategorized computer, code for device not assigned.
    Const BTSDK_COMPCLS_DESKTOP As UInt32 = &H104               'Desktop workstation.
    Const BTSDK_COMPCLS_SERVER As UInt32 = &H108                'Server-class computer.
    Const BTSDK_COMPCLS_LAPTOP As UInt32 = &H10C                'Laptop computer.
    Const BTSDK_COMPCLS_HANDHELD As UInt32 = &H110              'Handheld PC/PDA (clam shell).
    Const BTSDK_COMPCLS_PALMSIZED As UInt32 = &H114             'Palm sized PC/PDA.
    Const BTSDK_COMPCLS_WEARABLE As UInt32 = &H118              'Wearable computer (Watch sized).
    Const BTSDK_DEVCLS_PHONE As UInt32 = &H200                  'Phone major device class.
    Const BTSDK_PHONECLS_UNCLASSIFIED As UInt32 = &H200         'Uncategorized phone, code for device not assigned.
    Const BTSDK_PHONECLS_CELLULAR As UInt32 = &H204             'Cellular phone.
    Const BTSDK_PHONECLS_CORDLESS As UInt32 = &H208             'Cordless phone.
    Const BTSDK_PHONECLS_SMARTPHONE As UInt32 = &H20C           'Smart phone.
    Const BTSDK_PHONECLS_WIREDMODEM As UInt32 = &H210           'Wired modem or voice gateway.
    Const BTSDK_PHONECLS_COMMONISDNACCESS As UInt32 = &H214     'Common ISDN Access.
    Const BTSDK_PHONECLS_SIMCARDREADER As UInt32 = &H218        'SIM card reader
    Const BTSDK_DEVCLS_LAP As UInt32 = &H300                    'LAN / Network Access Point major

    Const BTSDK_LAP_FULLY As UInt32 = &H300                     'Fully available.
    Const BTSDK_LAP_17 As UInt32 = &H320                        '1 - 17% utilized.
    Const BTSDK_LAP_33 As UInt32 = &H340                        '17- 33% utilized.
    Const BTSDK_LAP_50 As UInt32 = &H360                        '33 - 50% utilized.
    Const BTSDK_LAP_67 As UInt32 = &H380                        '50 - 67% utilized.
    Const BTSDK_LAP_83 As UInt32 = &H3A0                        '67 - 83% utilized.
    Const BTSDK_LAP_99 As UInt32 = &H3C0                        '83 – 99% utilized.
    Const BTSDK_LAP_NOSRV As UInt32 = &H3E0                     'No service available.
    Const BTSDK_DEVCLS_AUDIO As UInt32 = &H400                  'Audio/Video major device class.
    Const BTSDK_AV_UNCLASSIFIED As UInt32 = &H400               'Uncategorized A/V device, code for device not assigned.
    Const BTSDK_AV_HEADSET As UInt32 = &H404                    'Wearable headset device.
    Const BTSDK_AV_HANDSFREE As UInt32 = &H408                  'Hands-free device.
    Const BTSDK_AV_MICROPHONE As UInt32 = &H410                 'Microphone.
    Const BTSDK_AV_LOUDSPEAKER As UInt32 = &H414                'Loudspeaker.
    Const BTSDK_AV_HEADPHONES As UInt32 = &H418                 'Headphones.
    Const BTSDK_AV_PORTABLEAUDIO As UInt32 = &H41C              'Portable Audio.
    Const BTSDK_AV_CARAUDIO As UInt32 = &H420                   'Car Audio.
    Const BTSDK_AV_SETTOPBOX As UInt32 = &H424                  'Set-top box.
    Const BTSDK_AV_HIFIAUDIO As UInt32 = &H428                  'HiFi Audio device.
    Const BTSDK_AV_VCR As UInt32 = &H42C                        'Videocassette recorder
    Const BTSDK_AV_VIDEOCAMERA As UInt32 = &H430                'Video camera
    Const BTSDK_AV_CAMCORDER As UInt32 = &H434                  'Camcorder
    Const BTSDK_AV_VIDEOMONITOR As UInt32 = &H438               'Video monitor.
    Const BTSDK_AV_VIDEODISPANDLOUDSPK As UInt32 = &H43C        'Video display and loudspeaker.
    Const BTSDK_AV_VIDEOCONFERENCE As UInt32 = &H440            'Video conferencing.
    Const BTSDK_AV_GAMEORTOY As UInt32 = &H448                  'Gaming/Toy
    Const BTSDK_DEVCLS_PERIPHERAL As UInt32 = &H500             'Peripheral major device class
    Const BTSDK_PERIPHERAL_UNCLASSIFIED As UInt32 = &H500       'Uncategorized peripheral device, code for device not assigned.
    Const BTSDK_PERIPHERAL_KEYBOARD As UInt32 = &H540           'Keyboard.
    Const BTSDK_PERIPHERAL_POINT As UInt32 = &H580              'Pointing device.
    Const BTSDK_PERIPHERAL_KEYORPOINT As UInt32 = &H5C0         'Combo keyboard/pointing device.
    Const BTSDK_DEVCLS_IMAGE As UInt32 = &H600                  'Imaging major device class.
    Const BTSDK_IMAGE_DISPLAY As UInt32 = &H610                 'Display.
    Const BTSDK_IMAGE_CAMERA As UInt32 = &H620                  'Camera.
    Const BTSDK_IMAGE_SCANNER As UInt32 = &H640                 'Scanner.
    Const BTSDK_IMAGE_PRINTER As UInt32 = &H680                 'Printer.
    Const BTSDK_DEVCLS_WEARABLE As UInt32 = &H700               'Wearable major device class.
    Const BTSDK_WERABLE_WATCH As UInt32 = &H704                 'Wristwatch.
    Const BTSDK_WERABLE_PAGER As UInt32 = &H708                 'Pager.
    Const BTSDK_WERABLE_JACKET As UInt32 = &H70C                'Jacket
    Const BTSDK_WERABLE_HELMET As UInt32 = &H710                'Helmet.
    Const BTSDK_WERABLE_GLASSES As UInt32 = &H714               'Glasses.

    'IVT BlueSoleil Major Service Class Identifiers
    Const BTSDK_SRVCLS_LDM As UInt32 = &H2000                   'Limited Discoveralbe Mode
    Const BTSDK_SRVCLS_POSITION As UInt32 = &H10000             'Positioning (Location Identification).
    Const BTSDK_SRVCLS_NETWORK As UInt32 = &H20000              'Networking (LAN, AD hoc, …).
    Const BTSDK_SRVCLS_RENDER As UInt32 = &H40000               'Rendering (Printing, Speaker, …).
    Const BTSDK_SRVCLS_CAPTURE As UInt32 = &H80000              'Capturing (Scanner, Microphone, …).
    Const BTSDK_SRVCLS_OBJECT As UInt32 = &H100000              'Object Transfer (v-Inbox, v-Folder, …).
    Const BTSDK_SRVCLS_AUDIO As UInt32 = &H200000               'Audio (Speaker, Microphone, Headset service, …).
    Const BTSDK_SRVCLS_TELEPHONE As UInt32 = &H400000           'Telephony (Cordless telephony, Modem, Headset service, …).
    Const BTSDK_SRVCLS_INFOR As UInt32 = &H800000               'Information (WEB-server, WAP-server, …).

    '/* BRSF feature mask ID for AG*/
    Const BTSDK_AG_BRSF_3WAYCALL As UInt32 = &H1                '/* Three-way calling */
    Const BTSDK_AG_BRSF_NREC As UInt32 = &H2                    '/* EC and/or NR function */
    Const BTSDK_AG_BRSF_BVRA As UInt32 = &H4                    '/* Voice recognition function */
    Const BTSDK_AG_BRSF_INBANDRING As UInt32 = &H8              '/* In-band ring tone capability */
    Const BTSDK_AG_BRSF_BINP As UInt32 = &H10                   '/* Attach a number to a voice tag */
    Const BTSDK_AG_BRSF_REJECT_CALL As UInt32 = &H20            '/* Ability to reject a call */
    Const BTSDK_AG_BRSF_ENHANCED_CALLSTATUS As UInt32 = &H40    '/* Enhanced call status */
    Const BTSDK_AG_BRSF_ENHANCED_CALLCONTROL As UInt32 = &H80   '/* Enhanced call control */
    Const BTSDK_AG_BRSF_EXTENDED_ERRORRESULT As UInt32 = &H100  '/* Extended Error Result Codes */
    Const BTSDK_AG_BRSF_ALL As UInt32 = &H1FF                   'All of the above

    '/* BRSF feature mask ID for HF */
    Const BTSDK_HF_BRSF_NREC As UInt32 = &H1                    '/* EC and/or NR function */
    Const BTSDK_HF_BRSF_3WAYCALL As UInt32 = &H2                '/* Call waiting and 3-way calling */
    Const BTSDK_HF_BRSF_CLIP As UInt32 = &H4                    '/* CLI presentation capability */
    Const BTSDK_HF_BRSF_BVRA As UInt32 = &H8                    '/* Voice recognition activation */
    Const BTSDK_HF_BRSF_RMTVOLCTRL As UInt32 = &H10             '/* Remote volume control */
    Const BTSDK_HF_BRSF_ENHANCED_CALLSTATUS As UInt32 = &H20    '/* Enhanced call status */
    Const BTSDK_HF_BRSF_ENHANCED_CALLCONTROL As UInt32 = &H40   '/* Enhanced call control */
    Const BTSDK_HF_BRSF_ALL As UInt32 = &H7F                    '/* Support all the upper features */ 

    Const BTSDK_OK As UInt32 = 0

    Const BTSDK_TRUE As UInt32 = 1
    Const BTSDK_FALSE As UInt32 = 0

    Const BTSDK_INVALID_HANDLE As UInt32 = 0

    '/* HSP/HFP AG specific event. */
    Const BTSDK_APP_EV_AGAP_BASE As UInt32 = &H900

    '/* Possible flags for member 'mask' in _BtSdkRemoteServiceAttrStru */
    Const BTSDK_RSAM_SERVICENAME As UInt32 = &H1
    Const BTSDK_RSAM_EXTATTRIBUTES As UInt32 = &H2




    Public s_rmt_dev_hdls() As Integer = {0}
    Public s_rmt_dev_num As Integer = 0
    Public s_rmt_dev_cls As Integer = 0

    Public s_currMAPConnHdl As UInt32 = BTSDK_INVALID_HANDLE
    Public s_currRmtMAPDevHdl As UInt32 = BTSDK_INVALID_HANDLE
    Public s_currMAPSvcHdl As UInt32 = BTSDK_INVALID_HANDLE

    Dim WithEvents Timer1 As System.Timers.Timer
    Dim WithEvents Timer2 As System.Timers.Timer


#End Region

#Region "AVCR Constants"
    'see 6.2.3.34 p232 of SDK 2.1.3
    'No.                        Octet   Bit             Parameter Description

    Const BTSDK_AVRCP_FBM_SELECT As UInt32 = 0 << 0                  'Select. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_UP As UInt32 = 1 << 1                     'Up. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_DOWN As UInt32 = 1 << 2                  'Down. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_LEFT As UInt32 = 1 << 3                 'Left. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_RIGHT As UInt32 = 1 << 4               'Right. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_RIGHTUP As UInt32 = 1 << 5            'right-up. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_RIGHTDOWN As UInt32 = 1 << 6         'right-down. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_LEFTUP As UInt32 = 1 << 7            'left-up. This PASSTHROUGH command is supported.

    Const BTSDK_AVRCP_FBM_LEFTDOWN As UInt32 = 1 << 8             'left-down. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_ROOTMENU As UInt32 = 1 << 9             'root menu. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_SETUPMENU As UInt32 = 1 << 10            'setup menu. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_CONENTMENU As UInt32 = 1 << 11           'contents menu. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_FAVORITEMNUMU As UInt32 = 1 << 12        'favorite menu. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_EXIT As UInt32 = 1 << 13                'Exit. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_0 As UInt32 = 1 << 14                   '0. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_1 As UInt32 = 1 << 15                   '1. This PASSTHROUGH command is supported.

    Const BTSDK_AVRCP_FBM_2 As UInt32 = 1 << 16                    '2. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_3 As UInt32 = 1 << 17                    '3. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_4 As UInt32 = 1 << 18                    '4. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_5 As UInt32 = 1 << 19                    '5. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_6 As UInt32 = 1 << 20                     '6. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_7 As UInt32 = 1 << 21                     '7. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_8 As UInt32 = 1 << 22                     '8. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_9 As UInt32 = 1 << 23                     '9. This PASSTHROUGH command is supported.

    Const BTSDK_AVRCP_FBM_DO As UInt32 = 1 << 24                    'Dot. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_ENTER As UInt32 = 1 << 25                 'Enter. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_CLEAR As UInt32 = 1 << 26               'Clear. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_CHANNLEUP As UInt32 = 1 << 27            'channel up. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_CHANNLEDOWN As UInt32 = 1 << 28          'channel down. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_PREVIOUSCHANNEL As UInt32 = 1 << 29      'previous channel. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_SOUNDSELECT As UInt32 = 1 << 30          'sound select. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_INPUTSELCET As Long = 1 << 31          'input select. This PASSTHROUGH command is supported.

    Const BTSDK_AVRCP_FBM_DISPLAY_INFORMATION As Long = 1 << 32  'Display information. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_HELP As Long = 1 << 33                 'Help. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_PAGEUP As Long = 1 << 34               'page up. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_PAGEDOWN As Long = 1 << 35             'page down. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_POWER As Long = 1 << 36                'Power. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_VOLUMEUP As Long = 1 << 37             'volume up. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_VOLUMEDOWN As Long = 1 << 38           'volume down. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_MUTE As Long = 1 << 39                 'Mute. This PASSTHROUGH command is supported.

    Const BTSDK_AVRCP_FBM_PLAY As Long = 1 << 40                 'Play. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_STOP As Long = 1 << 41                 'Stop. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_PAUSE As Long = 1 << 42                'Pause. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_RECORD As Long = 1 << 43               'Record. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_REWIND As Long = 1 << 44               'Rewind. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_FASTFORWARD As Long = 1 << 45          'fast forward. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_EJECT As Long = 1 << 46                'Eject. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_FORWARD As Long = 1 << 47              'Forward. This PASSTHROUGH command is supported.

    Const BTSDK_AVRCP_FBM_BACKWARD As Long = 1 << 48             'Backward. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_ANGLE As Long = 1 << 49                'Angle. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_SUBPICTURE As Long = 1 << 50           'Subpicture. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_F1 As Long = 1 << 51                   'F1. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_F2 As Long = 1 << 52                   'F2. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_F3 As Long = 1 << 53                   'F3. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_F4 As Long = 1 << 54                   'F4. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_F5 As Long = 1 << 55                   'F5. This PASSTHROUGH command is supported.

    Const BTSDK_AVRCP_FBM_VENDOR_UNIQUE As Long = 1 << 55        'Vendor unique. This PASSTHROUGH command is supported.
    Const BTSDK_AVRCP_FBM_BASIC_GROUP_NAVIGATION As Long = 1 << 56 'Basic Group Navigation. This overrules the SDP entry as it is set per player.
    Const BTSDK_AVRCP_FBM_ADVANCED_CONTROL_PLAYER As Long = 1 << 57 'Advanced Control Player. This bit is set if the player supports at least AVRCP 1.4.
    Const BTSDK_AVRCP_FBM_BROWSING As Long = 1 << 58             'Browsing. This bit is set if the player supports browsing.
    Const BTSDK_AVRCP_FBM_SEARCHING As Long = 1 << 59            'Searching. This bit is set if the player supports searching.
    Const BTSDK_AVRCP_FBM_ADDTO_NOWPLAYING As Long = 1 << 60     'AddToNowPlaying. This bit is set if the player supports the AddToNowPlaying command.
    Const BTSDK_AVRCP_FBM_UIDS_UNIQUE_INPLAYERVBROWSE_TREE As Long = 1 << 61 'UIDs unique in player browse tree. This bit is set if the player is able to maintain unique UIDs across the player browse tree.
    Const BTSDK_AVRCP_FBM_ONLY_BROWSABLE_WHEN_ADDRESSED As Long = 1 << 62 'OnlyBrowsableWhenAddressed. This bit is set if the player is only able to be browsed when it is set as the Addressed Player.

    Const BTSDK_AVRCP_FBM_ONLY_SERCHABLE_WHEN_ADDRESSED As Long = 1 << 63 'OnlySearchableWhenAddressed. This bit is set if the player is only able to be searched when it is set as the Addressed player.
    Const BTSDK_AVRCP_FBM_NOWPLAYING As Long = 1 << 64           'NowPlaying. This bit is set if the player supports the NowPlaying folder. Note that for all players that support browsing this bit shall be set.
    Const BTSDK_AVRCP_FBM_UIDPERSISTENCY As Long = 1 << 65       'UIDPersistency. This bit is set if the Player is able to persist UID values between AVRCP Browse Reconnects
#End Region

#Region "MAP Constants"
    '/* Maximum length */
    Const BTSDK_MAP_PATH_LEN As UInt32 = 512
    Const BTSDK_MAP_FOLDER_LEN As UInt32 = 32
    Const BTSDK_MAP_TIME_LEN As UInt32 = 20
    Const BTSDK_MAP_MSE_TIME_LEN As UInt32 = 24
    Const BTSDK_MAP_MSGHDL_LEN As UInt32 = 20
    Const BTSDK_MAP_MSGTYPE_LEN As UInt32 = 16
    Const BTSDK_MAP_SUBJECT_LEN As UInt32 = 256
    Const BTSDK_MAP_USERNAME_LEN As UInt32 = 256
    Const BTSDK_MAP_ADDR_LEN As UInt32 = 256

    '/* Bit mask of SupportedMessagetypes - possible values of BtSdkRmtMASSvcAttrStru::sup_msg_type */
    Const BTSDK_MAP_SUP_MSG_EMAIL As UInt32 = &H1
    Const BTSDK_MAP_SUP_MSG_SMSGSM As UInt32 = &H2
    Const BTSDK_MAP_SUP_MSG_SMSCDMA As UInt32 = &H4
    Const BTSDK_MAP_SUP_MSG_MMS As UInt32 = &H8

    '/* Message types */
    Const BTSDK_MAP_MSG_TYPE_EMAIL As UInt32 = &H1
    Const BTSDK_MAP_MSG_TYPE_SMSGSM As UInt32 = &H2
    Const BTSDK_MAP_MSG_TYPE_SMSCDMA As UInt32 = &H3
    Const BTSDK_MAP_MSG_TYPE_MMS As UInt32 = &H4
    Const BTSDK_MAP_MSG_TYPE_APPEXT As UInt32 = &HFF

    '/* Event report type - possible values of BtSdkEvReportObjStru::ev_type */
    Const BTSDK_MAP_EVT_NEWMSG As UInt32 = &H1
    Const BTSDK_MAP_EVT_DELIVERY_OK As UInt32 = &H2
    Const BTSDK_MAP_EVT_SEND_OK As UInt32 = &H3
    Const BTSDK_MAP_EVT_DELIVERY_FAIL As UInt32 = &H4
    Const BTSDK_MAP_EVT_SEND_FAIL As UInt32 = &H5
    Const BTSDK_MAP_EVT_MEM_FULL As UInt32 = &H6
    Const BTSDK_MAP_EVT_MEM_READY As UInt32 = &H7
    Const BTSDK_MAP_EVT_MSG_DELETED As UInt32 = &H8
    Const BTSDK_MAP_EVT_MSG_SHIFT As UInt32 = &H9
    Const BTSDK_MAP_EVT_APPEXT As UInt32 = &HFF

    '/* Bit mask - possible values of BtSdkMAPGetFolderListParamStru::mask */
    Const BTSDK_MAP_GFLP_MAXCOUNT As UInt32 = &H1
    Const BTSDK_MAP_GFLP_STARTOFF As UInt32 = &H2
    Const BTSDK_MAP_GFLP_LISTSIZE As UInt32 = &H4

    '/* Bit mask - possible values of BtSdkMAPGetMsgListParamStru::mask */
    Const BTSDK_MAP_GMLP_MAXCOUNT As UInt32 = &H1
    Const BTSDK_MAP_GMLP_STARTOFF As UInt32 = &H2
    Const BTSDK_MAP_GMLP_MSGTYPE As UInt32 = &H4
    Const BTSDK_MAP_GMLP_PERIODBEGIN As UInt32 = &H8
    Const BTSDK_MAP_GMLP_PERIODEND As UInt32 = &H10
    Const BTSDK_MAP_GMLP_READSTATUS As UInt32 = &H20
    Const BTSDK_MAP_GMLP_RECIPIENT As UInt32 = &H40
    Const BTSDK_MAP_GMLP_ORIGINATOR As UInt32 = &H80
    Const BTSDK_MAP_GMLP_PRIORITY As UInt32 = &H100
    Const BTSDK_MAP_GMLP_NEWMSG As UInt32 = &H1000
    Const BTSDK_MAP_GMLP_PARAMMASK As UInt32 = &H8000
    Const BTSDK_MAP_GMLP_LISTSIZE As UInt32 = &H20000
    Const BTSDK_MAP_GMLP_SUBJECTLENTH As UInt32 = &H40000
    Const BTSDK_MAP_GMLP_MSETIME As UInt32 = &H1000000

    '/* Bit mask - possible values of BtSdkMAPGetMsgListParamStru::param_mask, BtSdkMAPMsgObjStru::mask */
    Const BTSDK_MAP_MP_SUBJECT As UInt32 = &H1
    Const BTSDK_MAP_MP_DATATIME As UInt32 = &H2
    Const BTSDK_MAP_MP_SENDERNAME As UInt32 = &H4
    Const BTSDK_MAP_MP_SENDERADDR As UInt32 = &H8
    Const BTSDK_MAP_MP_RECIPIENTNAME As UInt32 = &H10
    Const BTSDK_MAP_MP_RECIPIENTADDR As UInt32 = &H20
    Const BTSDK_MAP_MP_TYPE As UInt32 = &H40
    Const BTSDK_MAP_MP_SIZE As UInt32 = &H80
    Const BTSDK_MAP_MP_RECPSTATUS As UInt32 = &H100
    Const BTSDK_MAP_MP_TEXT As UInt32 = &H200
    Const BTSDK_MAP_MP_ATTACHSIZE As UInt32 = &H400
    Const BTSDK_MAP_MP_PRIORITY As UInt32 = &H800
    Const BTSDK_MAP_MP_READ As UInt32 = &H1000
    Const BTSDK_MAP_MP_SENT As UInt32 = &H2000
    Const BTSDK_MAP_MP_PROTECTED As UInt32 = &H4000
    Const BTSDK_MAP_MP_REPLY2ADDR As UInt32 = &H8000

    '/* Bit mask - possible values of BtSdkMAPGetMsgListParamStru::filter_msg_type */
    Const BTSDK_MAP_FILTEROUT_NO As UInt32 = &H0
    Const BTSDK_MAP_FILTEROUT_SMSGSM As UInt32 = &H1
    Const BTSDK_MAP_FILTEROUT_SMSCDMA As UInt32 = &H2
    Const BTSDK_MAP_FILTEROUT_EMAIL As UInt32 = &H3
    Const BTSDK_MAP_FILTEROUT_MMS As UInt32 = &H4

    '/* Read status filter - possible values of BtSdkMAPGetMsgListParamStru::filter_read_status */
    Const BTSDK_MAP_MSG_FILTER_ST_ALL As UInt32 = &H0
    Const BTSDK_MAP_MSG_FILTER_ST_UNREAD As UInt32 = &H1
    Const BTSDK_MAP_MSG_FILTER_ST_READ As UInt32 = &H2

    '/* Priority filter - possible values of BtSdkMAPGetMsgListParamStru::filter_priority */
    Const BTSDK_MAP_MSG_FILTER_PRI_ALL As UInt32 = &H0
    Const BTSDK_MAP_MSG_FILTER_PRI_HIGH As UInt32 = &H1
    Const BTSDK_MAP_MSG_FILTER_PRI_NOHIGH As UInt32 = &H2

    '/* Char-set requirement - possible values of BtSdkMAPGetMsgParamStru::charset, BtSdkMAPPushMsgParamStru::charset */
    Const BTSDK_MAP_CHARSET_NATIVE As UInt32 = &H0 'return integrated SMS PDU in intrinsic encoder mode. The MSE does not do any change to the type.
    Const BTSDK_MAP_CHARSET_UTF8 As UInt32 = &H1 'the MSE only transfer the textual content of message, and change the type to UTF-8 before sending.

    '/* Fraction requirement - possible values of BtSdkMAPGetMsgParamStru::fraction_req */
    Const BTSDK_MAP_FRACT_NONE As UInt32 = &H0
    Const BTSDK_MAP_FRACT_REQFIRST As UInt32 = &H1
    Const BTSDK_MAP_FRACT_REQNEXT As UInt32 = &H2

    '/* Fraction indication - possible values of BtSdkMAPGetMsgParamStru::fraction_deliver */
    Const BTSDK_MAP_FRACT_RSPMORE As UInt32 = &H0
    Const BTSDK_MAP_FRACT_RSPLAST As UInt32 = &H1

    '/* Reception status - possible values of BtSdkMAPMsgObjStru::reception_status */
    Const BTSDK_MAP_MSG_RCVST_COMPLETE As UInt32 = &H0
    Const BTSDK_MAP_MSG_RCVST_FRACTION As UInt32 = &H1
    Const BTSDK_MAP_MSG_RCVST_NOTIFICATION As UInt32 = &H2

    '/* Message status indicator value - possible values of Btsdk_MAPSetMessageStatus::status */
    Const BTSDK_MAP_MSG_SETST_READ As UInt32 = &H2
    Const BTSDK_MAP_MSG_SETST_UNREAD As UInt32 = &H0
    Const BTSDK_MAP_MSG_SETST_DELETED As UInt32 = &H3
    Const BTSDK_MAP_MSG_SETST_UNDELETED As UInt32 = &H1

    '/* Message status indicator - possible values of BtSdkMAPMsgStatusStru::status_indicator */
    Const BTSDK_MAP_MSG_READ_STATUS As UInt32 = &H0
    Const BTSDK_MAP_MSG_DELETE_STATUS As UInt32 = &H1

    Const BT_FILE_HDL As String = Nothing

    Const IDX_PROP_BMSG_VERSION As UInt32 = &H1
    Const IDX_RPOP_BMSG_READSTATUS As UInt32 = &H2
    Const IDX_RPOP_BMSG_TYPE As UInt32 = &H3
    Const IDX_RPOP_BMSG_FOLDER As UInt32 = &H4
    Const IDX_RPOP_BMSG_SENDER_VESION As UInt32 = &H5
    Const IDX_RPOP_BMSG_SENDER_N As UInt32 = &H6
    Const IDX_RPOP_BMSG_SENDER_FN As UInt32 = &H7
    Const IDX_RPOP_BMSG_SENDER_TEL As UInt32 = &H8
    Const IDX_RPOP_BMSG_SENDER_EMAIL As UInt32 = &H9
    Const IDX_RPOP_BMSG_RECIPIENT_VESION As UInt32 = &HA
    Const IDX_RPOP_BMSG_RECIPIENT_N As UInt32 = &HB
    Const IDX_RPOP_BMSG_RECIPIENT_FN As UInt32 = &HC
    Const IDX_RPOP_BMSG_RECIPIENT_TEL As UInt32 = &HD
    Const IDX_RPOP_BMSG_RECIPIENT_EMAIL As UInt32 = &HE

    Const IDX_RPOP_BMSG_PART_ID As UInt32 = &HF
    Const IDX_RPOP_BMSG_BODY_ENCODING As UInt32 = &H10
    Const IDX_RPOP_BMSG_BODY_CHARSET As UInt32 = &H11
    Const IDX_RPOP_BMSG_BODY_LANGUAGE As UInt32 = &H12
    Const IDX_RPOP_BMSG_BODY_LENGTH As UInt32 = &H13
    Const IDX_PROP_BMSG_BODY_CONTENT As UInt32 = &H14
#End Region

#Region "PBAP Constantes"
    '/*Phone Book Access Profile*/
    Const BTSDK_PBAP_MAX_DELIMITER As UInt32 = &H2


    '/* Possible values for member 'order' in _BtSdkPBAPParamStru */
    Const BTSDK_PBAP_ORDER_INDEXED As UInt32 = &H0
    Const BTSDK_PBAP_ORDER_NAME As UInt32 = &H1
    Const BTSDK_PBAP_ORDER_PHONETIC As UInt32 = &H2

    '/* Possible flags for member 'mask' in _BtSdkPBAPParamStru */
    Const BTSDK_PBAP_PM_ORDER As UInt32 = &H1
    Const BTSDK_PBAP_PM_SCHVALUE As UInt32 = &H2
    Const BTSDK_PBAP_PM_SCHATTR As UInt32 = &H4
    Const BTSDK_PBAP_PM_MAXCOUNT As UInt32 = &H8
    Const BTSDK_PBAP_PM_LISTOFFSET As UInt32 = &H10
    Const BTSDK_PBAP_PM_FILTER As UInt32 = &H20
    Const BTSDK_PBAP_PM_FORMAT As UInt32 = &H40
    Const BTSDK_PBAP_PM_PBSIZE As UInt32 = &H80
    Const BTSDK_PBAP_PM_MISSEDCALLS As UInt32 = &H100

    '/* Possible values for member 'format' in _BtSdkPBAPParamStru */
    Const BTSDK_PBAP_FMT_VCARD21 As UInt32 = &H0
    Const BTSDK_PBAP_FMT_VCARD30 As UInt32 = &H1

    Const BTSDK_PBAP_REPO_LOCAL As UInt32 = &H1
    Const BTSDK_PBAP_REPO_SIM As UInt32 = &H2

    '/* Filter bit mask supported by PBAP1.0. Possible values for parameter
    'flag' of Btsdk_PBAPFilterComposer. */
    Const BTSDK_PBAP_FILTER_VERSION As UInt32 = &H0
    Const BTSDK_PBAP_FILTER_FN As UInt32 = &H1
    Const BTSDK_PBAP_FILTER_N As UInt32 = &H2
    Const BTSDK_PBAP_FILTER_PHOTO As UInt32 = &H3
    Const BTSDK_PBAP_FILTER_BDAY As UInt32 = &H4
    Const BTSDK_PBAP_FILTER_ADR As UInt32 = &H5
    Const BTSDK_PBAP_FILTER_LABEL As UInt32 = &H6
    Const BTSDK_PBAP_FILTER_TEL As UInt32 = &H7
    Const BTSDK_PBAP_FILTER_EMAIL As UInt32 = &H8
    Const BTSDK_PBAP_FILTER_MAILER As UInt32 = &H9
    Const BTSDK_PBAP_FILTER_TZ As UInt32 = &HA
    Const BTSDK_PBAP_FILTER_GEO As UInt32 = &HB
    Const BTSDK_PBAP_FILTER_TITLE As UInt32 = &HC
    Const BTSDK_PBAP_FILTER_ROLE As UInt32 = &HD
    Const BTSDK_PBAP_FILTER_LOGO As UInt32 = &HE
    Const BTSDK_PBAP_FILTER_AGENT As UInt32 = &HF
    Const BTSDK_PBAP_FILTER_ORG As UInt32 = &H10
    Const BTSDK_PBAP_FILTER_NOTE As UInt32 = &H11
    Const BTSDK_PBAP_FILTER_REV As UInt32 = &H12
    Const BTSDK_PBAP_FILTER_SOUND As UInt32 = &H13
    Const BTSDK_PBAP_FILTER_URL As UInt32 = &H14
    Const BTSDK_PBAP_FILTER_UID As UInt32 = &H15
    Const BTSDK_PBAP_FILTER_KEY As UInt32 = &H16
    Const BTSDK_PBAP_FILTER_NICKNAME As UInt32 = &H17
    Const BTSDK_PBAP_FILTER_CATEGORIES As UInt32 = &H18
    Const BTSDK_PBAP_FILTER_PROID As UInt32 = &H19
    Const BTSDK_PBAP_FILTER_CLASS As UInt32 = &H1A
    Const BTSDK_PBAP_FILTER_SORT_STRING As UInt32 = &H1B
    Const BTSDK_PBAP_FILTER_X_IRMC_CALL_DATETIME As Byte = &H1C
    Const BTSDK_PBAP_FILTER_PROPRIETARY_FILTER As UInt32 = &H27
    Const BTSDK_PBAP_FILTER_INVALID As UInt32 = &HFF
#End Region

#Region "Events"
    Public Enum BTSDK_HFP_APP_EventCodeEnum
        ' HFP_SetState Callback to Application Event Code */
        ' SLC - Both AG and HF */
        BTSDK_HFP_EV_SLC_ESTABLISHED_IND = BTSDK_APP_EV_AGAP_BASE + 1   ' HFP Service Level connection established. Parameter: Btsdk_HFP_ConnInfoStru */
        BTSDK_HFP_EV_SLC_RELEASED_IND                   ' SPP connection released. Parameter: Btsdk_HFP_ConnInfoStru */

        ' SCO - Both AG and HF  */
        BTSDK_HFP_EV_AUDIO_CONN_ESTABLISHED_IND         ' SCO audio connection established */
        BTSDK_HFP_EV_AUDIO_CONN_RELEASED_IND            ' SCO audio connection released */

        ' Status Changed Indication */
        BTSDK_HFP_EV_STANDBY_IND                        ' STANDBY Menu, the incoming call or outgoing call or ongoing call is canceled  */
        BTSDK_HFP_EV_ONGOINGCALL_IND                    ' ONGOING-CALL Menu, a call (incoming call or outgoing call) is established (ongoing) */
        BTSDK_HFP_EV_RINGING_IND                        ' RINGING Menu, a call is incoming. Parameter: BTBOOL - in-band ring tone or not.   */
        BTSDK_HFP_EV_OUTGOINGCALL_IND                   ' OUTGOING-CALL Menu, an outgoing call is being established, 3Way in Guideline P91 */
        BTSDK_HFP_EV_CALLHELD_IND                       ' BTRH-HOLD Menu, +BTRH:0, AT+BTRH=0, incoming call is put on hold */
        BTSDK_HFP_EV_CALL_WAITING_IND                   ' Call Waiting Menu, +CCWA, When Call=Active, call waiting notification. Parameter: Btsdk_HFP_PhoneInfoStru */
        BTSDK_HFP_EV_TBUSY_IND                          ' GSM Network Remote Busy, TBusy Timer Activated */

        ' AG & HF APP General Event Indication */
        BTSDK_HFP_EV_GENERATE_INBAND_RINGTONE_IND       ' AG Only, Generate the in-band ring tone */
        BTSDK_HFP_EV_TERMINATE_LOCAL_RINGTONE_IND       ' Terminate local generated ring tone or the in-band ring tone */
        BTSDK_HFP_EV_VOICE_RECOGN_ACTIVATED_IND         ' +BVRA:1, voice recognition activated indication or HF request to start voice recognition procedure */
        BTSDK_HFP_EV_VOICE_RECOGN_DEACTIVATED_IND       ' +BVRA:0, voice recognition deactivated indication or requests AG to deactivate the voice recognition procedure */
        BTSDK_HFP_EV_NETWORK_AVAILABLE_IND              ' +CIEV:<service><value>, cellular network is available */
        BTSDK_HFP_EV_NETWORK_UNAVAILABLE_IND            ' +CIEV:<service><value>, cellular network is unavailable */
        BTSDK_HFP_EV_ROAMING_RESET_IND                  ' +CIEV:<roam><value>, roaming is not active */
        BTSDK_HFP_EV_ROAMING_ACTIVE_IND                 ' +CIEV:<roam><value>, a roaming is active */
        BTSDK_HFP_EV_SIGNAL_STRENGTH_IND                ' +CIEV:<signal><value>, signal strength indication. Parameter: BTUINT8 - indicator value */	
        BTSDK_HFP_EV_BATTERY_CHARGE_IND                 ' +CIEV:<battchg><value>, battery charge indication. Parameter: BTUINT8 - indicator value  */
        BTSDK_HFP_EV_CHLDHELD_ACTIVATED_IND             ' +CIEV:<callheld><1>, Call on CHLD Held to be or has been actived. */
        BTSDK_HFP_EV_CHLDHELD_RELEASED_IND              ' +CIEV:<callheld><0>, Call on CHLD Held to be or has been released. */	
        BTSDK_HFP_EV_MICVOL_CHANGED_IND                 ' +VGM, AT+VGM, microphone volume changed indication */
        BTSDK_HFP_EV_SPKVOL_CHANGED_IND                 ' +VGS, AT+VGS, speaker volume changed indication */

        ' OK and Error Code - HF only */
        BTSDK_HFP_EV_ATCMD_RESULT                       ' HF Received OK, Error/+CME ERROR from AG or Wait for AG Response Timeout. Parameter: Btsdk_HFP_ATCmdResultStru */

        ' To HF APP, Call Related, AG Send information to HF */
        BTSDK_HFP_EV_CLIP_IND                           ' +CLIP, Phone Number Indication. Parameter: Btsdk_HFP_PhoneInfoStru */
        BTSDK_HFP_EV_CURRENT_CALLS_IND                  ' +CLCC, the current calls of AG. Parameter: Btsdk_HFP_CLCCInfoStru */
        BTSDK_HFP_EV_NETWORK_OPERATOR_IND               ' +COPS, the current network operator name of AG. Parameter: Btsdk_HFP_COPSInfoStru */
        BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND              ' +CNUM, the subscriber number of AG. Parameter: Btsdk_HFP_PhoneInfoStru */
        BTSDK_HFP_EV_VOICETAG_PHONE_NUM_IND             ' +BINP, AG inputted phone number for voice-tag; requests AG to input a phone number for the voice-tag at the HF side. Parameter: Btsdk_HFP_PhoneInfoStru */

        ' AG APP, HF Request or Indicate AG */
        BTSDK_HFP_EV_CURRENT_CALLS_REQ                  ' AT+CLCC, query the list of current calls in AG. */
        BTSDK_HFP_EV_NETWORK_OPERATOR_FORMAT_REQ        ' AT+COPS=3,0, indicate app the network operator name should be set to long alphanumeric */
        BTSDK_HFP_EV_NETWORK_OPERATOR_REQ               ' AT+COPS?, requests AG to respond with +COPS response indicating the currently selected operator */
        BTSDK_HFP_EV_SUBSCRIBER_NUMBER_REQ              ' AT+CNUM, query the AG subscriber number information. */
        BTSDK_HFP_EV_VOICETAG_PHONE_NUM_REQ             ' AT+BINP, requests AG to input a phone number for the voice-tag at the HF */
        BTSDK_HFP_EV_CUR_INDICATOR_VAL_REQ              ' AT+CIND?, get the current indicator during the service level connection initialization procedure */
        BTSDK_HFP_EV_HF_DIAL_REQ                        ' ATD, instructs AG to dial the specific phone number. Parameter: (HFP only) BTUINT8* - phone number */
        BTSDK_HFP_EV_HF_MEM_DIAL_REQ                    ' ATD>, instructs AG to dial the phone number indexed by the specific memory location of SIM card. Parameter: BTUINT8* - memory location */
        BTSDK_HFP_EV_HF_LASTNUM_REDIAL_REQ              ' AT+BLDN, instructs AG to redial the last dialed phone number */
        BTSDK_HFP_EV_MANUFACTURER_REQ                   ' AT+CGMI, requests AG to respond with the Manufacturer ID */
        BTSDK_HFP_EV_MODEL_REQ                          ' AT+CGMM, requests AG to respond with the Model ID */
        BTSDK_HFP_EV_NREC_DISABLE_REQ                   ' AT+NREC=0, requests AG to disable NREC function */
        BTSDK_HFP_EV_DTMF_REQ                           ' AT+VTS, instructs AG to transmit the specific DTMF code. Parameter: BTUINT8 - DTMF code */
        BTSDK_HFP_EV_ANSWER_CALL_REQ                    ' inform AG app to answer the call. Parameter: BTUINT8 - One of BTSDK_HFP_TYPE_INCOMING_CALL, BTSDK_HFP_TYPE_HELDINCOMING_CALL. */	
        BTSDK_HFP_EV_CANCEL_CALL_REQ                    ' inform AG app to cancel the call. Parameter: BTUINT8 - One of BTSDK_HFP_TYPE_ALL_CALLS, BTSDK_HFP_TYPE_INCOMING_CALL, 
        '   BTSDK_HFP_TYPE_HELDINCOMING_CALL, BTSDK_HFP_TYPE_OUTGOING_CALL, BTSDK_HFP_TYPE_ONGOING_CALL. */	
        BTSDK_HFP_EV_HOLD_CALL_REQ                      ' inform AG app to hold the incoming call (AT+BTRH=0) */

        ' AG APP, 3-Way Calling */
        BTSDK_HFP_EV_REJECTWAITINGCALL_REQ              ' AT+CHLD=0, Release all held calls or reject waiting call. */	
        BTSDK_HFP_EV_ACPTWAIT_RELEASEACTIVE_REQ         ' AT+CHLD=1, Accept the held or waiting call and release all avtive calls. Parameter: BTUINT8 - value of <idx>*/
        BTSDK_HFP_EV_HOLDACTIVECALL_REQ                 ' AT+CHLD=2, Held Specified Active Call.  Parameter: BTUINT8 - value of <idx>*/
        BTSDK_HFP_EV_ADD_ONEHELDCALL_2ACTIVE_REQ        ' AT+CHLD=3, Add One CHLD Held Call to Active Call. */
        BTSDK_HFP_EV_LEAVE3WAYCALLING_REQ               ' AT+CHLD=4, Leave The 3-Way Calling. */

        ' Extended */
        BTSDK_HFP_EV_EXTEND_CMD_IND                     ' indicate app extend command received. Parameter: BTUINT8* - Full extended AT command or result code. */
        BTSDK_HFP_EV_PRE_SCO_CONNECTION_IND             ' indicate app to create SCO connection. Parameter: Btsdk_AGAP_PreSCOConnIndStru. */
        BTSDK_HFP_EV_SPP_ESTABLISHED_IND                ' SPP connection created. Parameter: Btsdk_HFP_ConnInfoStru. added 2008-7-3 */
        BTSDK_HFP_EV_HF_MANUFACTURERID_IND              ' ManufacturerID indication. Parameter: BTUINT8* - Manufacturer ID of the AG device, a null-terminated ASCII string. */
        BTSDK_HFP_EV_HF_MODELID_IND                     ' ModelID indication.  Parameter: BTUINT8* - Model ID of the AG device, a null-terminated ASCII string. */

        ' Battery Info in AVRCP mode
        'BTSDK_AVRCP_BATTERYSTATUS_NORMAL                ' Battery operation is in normal state
        'BTSDK_AVRCP_BATTERYSTATUS_WARNING               ' Unable to operate soon. Specified when battery going down
        'BTSDK_AVRCP_BATTERYSTATUS_CRITICAL              ' Can not operate any more. Specified when battery going down.
        'BTSDK_AVRCP_BATTERYSTATUS_EXTERNAL              ' Connecting to external power supply
        'BTSDK_AVRCP_BATTERYSTATUS_FULL_CHARGE           ' When the device is completely charged.

        BTSDK_INQUIRY_RESULT_IND                        ' This message indicates that a Bluetooth device has responded so far during the current inquiry process.
        BTSDK_INQUIRY_COMPLETE_IND                      ' This message indicates that the inquiry is finished.

        'Message
        BTSDK_MAP_EVT_NEWMSG                            ' NewMessage
        BTSDK_MAP_EVT_DELIVERY_OK                       ' DeliverySuccess
        BTSDK_MAP_EVT_SEND_OK                           ' SendingSuccess
        BTSDK_MAP_EVT_DELIVERY_FAIL                     ' DeliveryFailure
        BTSDK_MAP_EVT_SEND_FAIL                         ' SendingFailure
        BTSDK_MAP_EVT_MEM_FULL                          ' MemoryFull
        BTSDK_MAP_EVT_MEM_READY                         ' MemoryAvailable, the event only occurs when BTSDK_MAP_EVT_MEM_FULL event has occurred. 
        BTSDK_MAP_EVT_MSG_DELETED                       ' MessageDeleted
        BTSDK_MAP_EVT_MSG_SHIFT                         ' MessageShift.
    End Enum
#End Region

    Public Delegate Sub hfEventCallback(ByVal hdl As UInt32, ByVal e As UInt16, ByVal param As IntPtr, ByVal param_len As UInt16)
    Public Delegate Sub connEventCallback(ByVal hdl As UInt32, ByVal reason As UInt16, ByVal arg As IntPtr)
    Public Delegate Sub PbabEventCallback(ByVal first As Byte, ByVal last As Byte, ByVal filename As IntPtr, ByVal filesize As UInt32, ByVal cursize As UInt32)
    'Public Delegate Sub mapEventCallback(ByVal hdl As UInt32, ByVal reason As UInt16, ByVal arg As IntPtr) 'Event for MAP

#Region "SDK Function DLL Import"


    '****************************************************
    '* Section 5.4.1 Initialization/Termination * 
    '****************************************************
    '** Section 5.4.1.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_Init() As UInt32
    End Function
    '** Section 5.4.1.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Sub Btsdk_Done()
    End Sub
    '** Section 5.4.1.3 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_IsSDKInitialized() As UInt32
    End Function
    '** Section 5.4.1.5 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RegisterCallback4ThirdParty(ByVal cbStruc As IntPtr) As UInt32
    End Function

    '***************************************************
    '* Section 5.4.2 Memory Management *
    '***************************************************
    '** Section 5.4.2.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MallocMemory(size As UInt32) As UInt32
    End Function
    '** Section 5.4.2.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_FreeMemory() As UInt32
    End Function


    '***************************************************
    '* Section 5.4.3 Local Bluetooth Device Management * 
    '***************************************************
    '** Section 5.4.3.1.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_StartBluetooth() As UInt32
    End Function
    '** Section 5.4.3.1.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_StopBluetooth() As UInt32
    End Function
    '** Section 5.4.3.1.3 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_IsBluetoothReady() As UInt32
    End Function
    '** Section 5.4.3.1.4 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_IsBluetoothHardwareExisted() As Boolean
    End Function
    '** Section 5.4.3.1.5 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_SetSecurityMode(secu_mode As UInt16) As UInt32
    End Function
    '** Section 5.4.3.2.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_SetDiscoveryMode(mode As UInt16) As UInt32
    End Function
    '** Section 5.4.3.2.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetDiscoveryMode(pmode As UInt16) As UInt32
    End Function
    '** Section 5.4.3.3.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_SetLocalName(name As Byte, len As UInt16) As UInt32
    End Function
    '** Section 5.4.3.3.3 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetLocalName(name As Byte, plen As UInt16) As UInt32
    End Function
    '** Section 5.4.3.3.4 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_SetLocalDeviceClass(ByVal deviceClass As UInt32) As UInt32
    End Function
    '** Section 5.4.3.3.7 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_SetFixedPincode(ByRef pin_code As String, ByVal pincode_len As UInt16) As UInt32
    End Function
    '** Section 5.4.3.3.8 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetFixedPincode(ByRef pin_code As String, ByRef pincode_len As UInt16) As UInt32
    End Function


    '****************************************************
    '* Section 5.4.4 Remote Bluetooth Device Management * 
    '****************************************************
    '** Section 5.4.4.1.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_StartDeviceDiscovery(ByVal dvcClass As UInt32, ByVal maxDevices As UInt16, ByVal maxDurations As UInt16) As UInt32  'default durations is 10.
    End Function
    '** Section 5.4.4.1.4 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_StopDeviceDiscovery() As UInt32
    End Function
    'Search Remote Devices
    '** Section 5.4.4.1.5 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_UpdateRemoteDeviceName(ByVal dvcHandle As UInt32, ByRef arrayNameBytes As Byte, ByVal arrayLen As UInt16) As UInt32
    End Function
    '** Section 5.4.4.1.6 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_CancelUpdateRemoteDeviceName(ByVal dvcHandle As UInt32) As UInt32
    End Function
    '** Section 5.4.4.2.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_IsDevicePaired(ByVal dvcHandle As UInt32, ByRef retIsPaired As UInt16) As UInt32
    End Function
    '** Section 5.4.4.2.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PairDevice(ByVal dvcHandle As UInt32) As UInt32
    End Function
    '** Section 5.4.4.2.3 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_UnPairDevice(ByVal dvcHandle As UInt32) As UInt32
    End Function
    '** Section 5.4.4.2.7 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PinCodeReply(ByVal dvcHandle As UInt32, ByVal pin_code As String, ByVal len_code As UInt16) As UInt32
    End Function
    '** Section 5.4.4.3.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_IsDeviceConnected(ByVal phone_handle As UInt32) As UInt32
    End Function
    '** Section 5.4.4.3.4 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteRSSI(ByVal connHandle As UInt32, prssi As Integer) As UInt32
    End Function
    '** Section 5.4.4.3.5 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteLinkQuality(ByVal connHandle As UInt32, plink_quality As UInt16) As UInt32
    End Function
    '** Section 5.4.4.3.6 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetSupervisionTimeout(ByVal connHandle As UInt32, ptimeout As UInt16) As UInt32
    End Function
    '** Section 5.4.4.4.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteDeviceHandle(ByVal addr As IntPtr) As UInt32
    End Function
    '** Section 5.4.4.4.3 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_DeleteRemoteDeviceByHandle(ByVal dvcHandle As UInt32) As UInt32
    End Function '
    '** Section 5.4.4.4.5 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetStoredDevicesByClass(ByVal dvcClass As UInt32, ByRef arrayDvcHandles As UInt32, ByVal arrayMaxNumEntries As UInt32) As Integer
    End Function
    '** Section 5.4.4.4.5 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl, entrypoint:="Btsdk_GetStoredDevicesByClass")> _
    Private Shared Function Btsdk_GetStoredDevicesByClass_ByVal(ByVal dvcClass As UInt32, ByVal nullInt1 As UInt32, ByVal nullInt2 As UInt32) As Integer
    End Function
    '** Section 5.4.4.4.11 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteDeviceAddress(ByVal phone_handle As UInt32, ByVal addr As IntPtr) As UInt32
    End Function
    '** Section 5.4.4.4.12 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteDeviceName(ByVal dvcHandle As UInt32, ByRef arrayBytes As Byte, ByRef arrayLen As UInt16) As UInt32
    End Function
    '** Section 5.4.4.4.13 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteDeviceClass(ByVal dvcHandle As UInt32, ByRef retDvcClass As UInt32) As UInt32
    End Function
    '** Section 5.4.4.4.15 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RemoteDeviceFlowStatistic(ByVal connHandle As UInt32, rx_bytes As UInt32, tx_bytes As UInt32) As UInt32
    End Function
    '** Section 5.4.5.1.5 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteServices(ByVal dvcHandle As UInt32, ByRef retArraySvcHandles As UInt32, ByRef arrayMaxNumEntries As UInt32) As UInt32
    End Function


    '***************************************
    '* Section 5.4.5 Connection Management * 
    '***************************************
    '** Section 5.4.5.1.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_BrowseRemoteServices(ByVal device_handle As UInt32, ByVal servicesHandles As IntPtr, ByVal serviceHandlesLen As IntPtr) As UInt32
    End Function
    '** Section 5.4.5.1.6 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteServiceAttributes(ByVal serviceHandle As UInt32, ByVal pattributes As IntPtr) As UInt32
    End Function
    '** Section 5.4.5.3.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_Connect(ByVal serviceHandle As UInt32, ByVal lParam As UInt32, ByVal connHandle As IntPtr) As UInt32
    End Function
    '** Section 5.4.5.3.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_ConnectEx(ByVal deviceHandle As UInt32, ByVal serviceClass As UInt16, ByVal lParam As UInt32, ByVal connHandle As IntPtr) As UInt32
    End Function
    '** Section 5.4.5.4.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetConnectionProperty(connection_handle As UInt32, pproperty As BtSdkConnectionPropertyStru) As UInt32
    End Function
    '** Section 5.4.5.4.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_StartEnumConnection() As UInt32
    End Function
    '** Section 5.4.5.4.3 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_EnumConnection(ByVal searchHandle As UInt32, ByVal connStructPtr As IntPtr) As UInt32
    End Function
    '** Section 5.4.5.4.4 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_EndEnumConnection(ByVal searchHandle As UInt32) As UInt32
    End Function
    '** Section 5.4.5.5.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_Disconnect(ByVal conn_handle As UInt32) As UInt32
    End Function


    '****************************************************
    '* Section 6.3.4 Audio/Video Remote Control Profile * 
    '****************************************************
    '** Section 6.3.4.1.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_AVRCP_RegPassThrCmdCbk4ThirdParty(pfunc As Btsdk_AVRCP_PassThr_Cmd_Func) As UInt32
    End Function
    '** Section 6.3.4.1.2 in SDK 2.1.3 **
    Private Delegate Sub Btsdk_AVRCP_PassThr_Cmd_Func(op_id As Byte, state_flag As Byte)
    'BTSDK_AVRCP_OPID_AVC_PANEL_POWER           Power operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_UP       Volume Up operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_DOWN     Volume Down operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_MUTE            Mute operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_PLAY            Play operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_STOP            Stop operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_PAUSE           Pause operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_RECORD          Record operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_REWIND          Rewind operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_FAST_FORWARD    Fast Forward operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_EJECT           Reject operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_FORWARD         Forward operation.
    'BTSDK_AVRCP_OPID_AVC_PANEL_BACKWARD        Backward operation.
    'state_flag:
    'BTSDK_AVRCP_BUTTON_STATE_PRESSED           Button is pressed down.
    'BTSDK_AVRCP_BUTTON_STATE_RELEASED          Button is released.

    '** Section 6.3.4.1.3 in SDK 2.1.3 **
    '<DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    'Private Shared Function Btsdk_AVRCP_RegIndCbk4ThirdParty(pfunc As Btsdk_AVRCP_Event_Ind_Func) As UInt32
    'End Function
    'Private Delegate Sub Btsdk_AVRCP_Event_Ind_Func(op_id As Byte, state_flag As Byte)
    '** Section 6.3.4.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_AVRCP_PlayItemReq(hdl As UInt32, param As BtSdkPlayItemReqStru) As UInt32
    End Function

    '*************************************
    '* Section 6.3.5 Serial Port Profile * 
    '*************************************
    '** Section 6.3.5.3 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetClientPort(ByVal connHandle As UInt32) As UInt32
    End Function
    '** Section 6.3.5.7 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetASerialNum() As UInt32
    End Function


    '************************************************
    '* Section 6.3.6 Hands-free and Headset Profile * 
    '************************************************
    '** Section 6.3.6.1.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RegisterHFPService(ByVal name As String, ByVal svcClass As UInt16, ByVal features As UInt16) As UInt32
    End Function
    '** Section 6.3.6.1.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_UnregisterHFPService(ByVal service_handle As UInt32) As UInt32
    End Function
    '** Section 6.3.6.1.4 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFP_ExtendCmd(ByVal connHandle As UInt32, ByVal cmd As String, ByVal cmdLen As UInt16, ByVal Timeout As UInt32) As UInt32
    End Function
    '** Section 6.3.6.2.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_AGAP_APPRegCbk4ThirdParty(ByVal hfCB As hfEventCallback) As UInt32
    End Function
    '** Section 6.3.6.2.16 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_AGAP_ManufacturerIDRsp(ByVal searchHandle As UInt32, mid As String, len As Byte) As UInt32
    End Function
    '** Section 6.3.6.3.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_APPRegCbk4ThirdParty(ByVal hfCB As hfEventCallback) As UInt32
    End Function
    '** Section 6.3.6.3.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_AnswerCall(ByVal connHandle As UInt32) As UInt32
    End Function
    '** Section 6.3.6.3.3 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_CancelCall(ByVal connHandle As UInt32) As UInt32
    End Function
    '** Section 6.3.6.3.6 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_Dial(ByVal connHandle As UInt32, ByRef arrayPhoneNumberBytes As Byte, ByVal arrayLen As UInt16) As UInt32
    End Function
    '** Section 6.3.6.3.7 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_VoiceRecognitionReq(ByVal connHandle As UInt32, ByVal enable As Byte) As UInt32
    End Function
    '** Section 6.3.6.3.10 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_TxDTMF(ByVal connHandle As UInt32, ByVal num As Byte) As UInt32
    End Function
    '** Section 6.3.6.3.24 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_GetCurrentCalls(ByVal connhandle As UInt32) As UInt32
    End Function
    '** Section 6.3.6.3.26 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_GetCurrHFState(ByVal state As IntPtr) As UInt32
    End Function



    '************************************************
    '* Section 6.3.6 Hands-free and Headset Profile * 
    '************************************************
    '** Section 6.3.6.3.4 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_LastNumRedial(ByVal connHandle As UInt32) As UInt32
    End Function
    '** Section 6.3.6.3.11 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_SetSpkVol(ByVal connHandle As UInt32, ByVal micVol0to15 As Byte) As UInt32
    End Function
    '** Section 6.3.6.3.12 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_SetMicVol(ByVal connHandle As UInt32, ByVal micVol0to15 As Byte) As UInt32
    End Function
    '** Section 6.3.6.3.14 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_GetManufacturerID(ByVal connHandle As UInt32, ByRef array256bytes As Byte, ByRef arrayLen As UInt16) As UInt32
    End Function
    '** Section 6.3.6.3.15 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_GetModelID(ByVal connHandle As UInt32, ByRef array256bytes As Byte, ByRef arrayLen As UInt16) As UInt32
    End Function
    '** Section 6.3.6.3.16 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_AudioConnTrans(ByVal connHandle As UInt32) As UInt32
    End Function
    '** Section 6.3.6.3.17 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_NetworkOperatorReq(ByVal connHandle As UInt32) As UInt32
    End Function
    '** Section 6.3.6.3.23 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_GetSubscriberNumber(ByVal connHandle As UInt32) As UInt32
    End Function

    '***********************************************************
    '* Section 6.3.7 Advancecd Audio Distribute Profile (A2DP) * 
    '***********************************************************
    '** Section 6.3.7.1.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RegisterA2DPSRCService() As UInt32
    End Function
    '** Section 6.3.7.1.2 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function UnregisterA2DPSRCService() As UInt32
    End Function
    '** Section 6.3.7.1.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RegisterA2DPSRCService(len As UInt16, audio_card As Byte) As UInt32
    End Function

    '*******************************************
    '* Section 6.3.9 Phone Book Access Profile * 
    '*******************************************
    '** Section 6.3.9.1.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RegisterPBAPService(ByVal svc_name As String, svr_attr As BtSdkLocalPSEServerAttrStru, cb_funs As BtSdkPBAPSvrCBStru) As UInt32
    End Function

    '** Section 6.3.9.1.22 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPRegisterSvrCallback(ByVal conn_hdl As UInt32, ByVal cb_funcs As PbabEventCallback) As UInt32
    End Function
    '** Section 6.3.9.1.23 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPRegisterFileIORoutines(ByVal svc_hdl As UInt32, ByVal cb_funcs As IntPtr) As UInt32
    End Function
    '** Section 6.3.9.1.25 in SDK 2.1.3 **
    '<DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    'Private Shared Function Btsdk_PBAPRegisterStatusCallback(ByVal svc_hdl As UInt32, func As Btsdk_PBAP_STATUS_INFO_CB) As UInt32
    'End Function
    '** Section 6.3.9.1.26 in SDK 2.1.3 **
    '<DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    'Public Delegate Sub Btsdk_PBAP_STATUS_INFO_CB(first As String, last As String, filename As String, filesize As UInt32, cursize As UInt32)
    'Private Shared Function Btsdk_PBAP_STATUS_INFO_CB(first As String, last As String, filename As String, filesize As UInt32, cursize As UInt32) As UInt32
    'End Function
    '** Section 6.3.9.1.27 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPPullPhoneBook(ByVal connHandle As UInt32, ByVal path As Byte(), param As BtSdkPBAPParamStru, ByVal file_hdl As UInt32) As UInt32
    End Function
    '** Section 6.3.9.1.28 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPFilterComposer(ByVal filter As Byte, ByVal flag As Byte) As UInt32
    End Function
    '** Section 6.3.9.1.29 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPSetPath(ByVal connHandle As UInt32, ByVal path As String) As UInt32
    End Function
    '** Section 6.3.9.1.30 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPPullCardList(ByVal connHandle As UInt32, ByVal folder As String, param As BtSdkPBAPParamStru, ByVal file_hdl As String) As UInt32
    End Function
    '** Section 6.3.9.1.31 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPPullCardEntry(ByVal connHandle As UInt32, ByVal name As String, param As BtSdkPBAPParamStru, ByVal file_hdl As String) As UInt32
    End Function
    '** Section 6.3.9.1.32 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPCancelTransfer(ByVal connHandle As UInt32) As UInt32
    End Function


    '******************************
    '* Section 6.3.10 MAP Profile * 
    '******************************
    '** Section 6.3.10.1.1 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAP_STATUS_INFO_CB(ByVal first As Byte, ByVal last As Byte, ByVal filename As Byte, ByVal filesize As UInt32, ByVal cursize As UInt32) As UInt32
    End Function

    '** Section 6.3.10.1.23 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RegisterMASService(ByVal svc_name As String, ByVal svr_attr As BtSdkLocalMASServerAttrStru, ByVal cb_funcs As BtSdkMASSvrCBStru) As UInt32
    End Function
    '** Section 6.3.10.1.25 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RegisterMNSService(ByVal svc_name As Byte, ByVal st_func As Btsdk_MAP_RegisterNotification_Func, ByVal file_ios As BtSdkMAPFileIORoutinesStru) As UInt32
    End Function
    Private Delegate Function Btsdk_MNS_MessageNotification_Func(ByVal svc_hdl As UInt32, ByVal ev_obj As BtSdkMAPEvReportObjStru) As UInt32 '** Section 6.3.10.1.2 in SDK 2.1.3 **

    '** Section 6.3.10.1.26 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPRegisterFileIORoutines(ByVal conn_hdl As UInt32, ByVal cb_funcs As BtSdkMAPFileIORoutinesStru) As UInt32
    End Function

    '** Section 6.3.10.1.27 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_UnregisterMAPService(ByVal svc_hdl As UInt32) As UInt32
    End Function

    '** Section 6.3.10.1.29 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPSetNotificationRegistration(ByVal conn_hdl As UInt32, ByVal turn_on As Boolean) As UInt32
    End Function
    '** Section 6.3.10.1.31 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPSetFolder(ByVal conn_hdl As UInt32, ByVal folder As String) As UInt32
    End Function
    '** Section 6.3.10.1.32 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPGetFolderList(ByVal conn_hdl As UInt32, ByVal param As BtSdkMAPGetFolderListParamStru, ByVal file_hdl As UInteger) As UInt32
    End Function
    '** Section 6.3.10.1.33 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPGetMessageList(ByVal conn_hdl As UInt32, ByVal param As BtSdkMAPGetMsgListParamStru, ByVal file_hdl As UInteger) As UInt32
    End Function
    '** Section 6.3.10.1.34 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPGetMessage(ByVal conn_hdl As UInt32, ByVal param As BtSdkMAPGetMsgParamStru, ByVal file_hdl As UInteger) As UInt32
    End Function
    '** Section 6.3.10.1.35 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPSetMessageStatus(ByVal conn_hdl As UInt32, ByVal msg_hdl As String, ByVal status As String) As UInt32
    End Function
    '** Section 6.3.10.1.36 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPPushMessage(ByVal conn_hdl As UInt32, ByVal param As BtSdkMAPPushMsgParamStru, ByVal file_hdl As UInteger) As UInt32
    End Function
    '** Section 6.3.10.1.37 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPUpdateInbox(ByVal conn_hdl As UInt32) As UInt32
    End Function
    '** Section 6.3.10.1.38 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPCancelTransfer(ByVal conn_hdl As UInt32) As UInt32
    End Function
    '** Section 6.3.10.1.39 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPStartEnumFolderList(ByVal func_read As Btsdk_ReadFile_Func, ByVal func_rewind As Btsdk_RewindFile_Func, ByVal file_hdl As UInteger) As UInt32
    End Function
    '** Section 6.3.10.1.40 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPEnumFolderList(ByVal enum_hdl As UInt32, ByVal item As BtSdkMAPFolderObjStru) As UInt32
    End Function
    '** Section 6.3.10.1.41 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPEndEnumFolderList(ByVal enum_hdl As UInt32) As UInt32
    End Function
    '** Section 6.3.10.1.42 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPStartEnumMessageList(ByVal func_read As Btsdk_ReadFile_Func, ByVal func_rewind As Btsdk_RewindFile_Func, ByVal file_hdl As UInteger) As UInt32
    End Function
    '** Section 6.3.10.1.43 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPEnumMessageList(ByVal enum_hdl As UInteger, ByVal item As BtSdkMAPMsgObjStru) As BtSdkMAPMsgObjStru
    End Function
    '** Section 6.3.10.1.44 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_MAPEndEnumMessageList(ByVal enum_hdl As UInteger) As UInt32
    End Function


#End Region

#Region "Variables"
    Private hfEventGlobal As hfEventCallback
    Private connEventGlobal As connEventCallback
    Private pbapEventGlobal As PbabEventCallback

    'Private connEventSendReceivedGlobal As connEventSendReceivedBytes
    'Private mapEventGlobal As mapEventCallback 'Event for MAP

    Private threadWaitForPhone As Thread
    Private threadStartup As Thread
    Private threadPinCode As Thread

    Private PhoneConnected As New AutoResetEvent(False)
    Private PhoneDisconnected As New AutoResetEvent(False)


    Private name As String
    Private hfService As UInt32
    Private hsService As UInt32
    Private hfagService As UInt32
    Private hsagService As UInt32
    Private pbapService As UInt32
    Private mapService As UInt32

    Private phoneAddr(0 To 5) As Byte
    Private phoneHandle As UInt32
    Private connHandle As UInt32
    Private pbapHandle As UInt32

    'Status Indicators/Variables
    Private rssi As Byte
    'Private plink_quality As UInt16
    'Private pTimeOut As UInt16
    'Private RxBytes As UInt32
    'Private TxBytes As UInt32
    Private phoneModel As String
    Private phoneManufacturer As String
    Private battery As Byte
    Private networkName As String
    Private networkAvail As Boolean
    Private bSiri As Boolean
    Private SCO As Boolean
    Private standby As Boolean
    Private currentCalls As List(Of contact)

    Private phoneConn As Boolean

    Private ContactsRetrieved As New AutoResetEvent(False)
    Private contactNum As UInt32

    Private ServicesArray As New ArrayList, servicedetail() As String
    Private PhoneBookArray As New ArrayList()
    Private PhoneBookArray2 As New ArrayList()

    Dim pbPath As String = MainPath & "phonebook.vcf"
    Dim historyPath As String = MainPath & "callhistory.vcf"

    Private PhoneBookArray_A As New ArrayList()
    Private PhoneBookArray_B As New ArrayList()
    Private PhoneBookArray_B2 As New ArrayList()
#End Region

#Region "Events to RR"
    Public Event PhoneIsConnected()     'Raises an event when the phone is connected.
    Public Event PhoneIsDisConnected()  'Raises an event when the phone is disconnected.
    Public Event PhoneInCall()          'Raises an event when you first answer a call
    Public Event PhoneNetAvail()        'Raises an event when first connect to a network.
    Public Event PhoneNetUnAvail()      'Raises an event when first disconnect from a network.
    Public Event Ringing()              'Raises an event when an incoming call is detected.
    Public Event BatteryIsFull()        'Raises an event when the battery is 100%
    Public Event ExtPowerBattON()       'Raises an event when an external power is connected to the phone.
    Public Event ExtPowerBattOFF()      'Raises an event when an external power is unconnected to the phone.
    Public Event EmergencyAlarm()       'Raises an event when the phone number = TempPluginSettings.EmergencyNumber
    Public Event Hungup()
    Public Event VoiceActivation()
    Public Event VoiceDeactivation()
    Public Event FirstPhoneNotFound()   'Raises an event when the first phone isn't found.
    Public Event SecondPhoneNotFound()  'Raises an event when the second phone isn't found.

    Public Event PBAPIsReady()          'Raises an event when the PBAP service is ready.
    Public Event MAPIsReady()           'Raises an event when the MAP service is ready.

    Public Event SMSIsSend()            'Raises an event when the SMS is send.
    Public Event SMSIsReceived()        'Raises an event when the SMS is received.

    Public Event SetIndBlinkOn()        'Raise an event for run blink of synch indicator
    Public Event SetIndBlinkOff()       'Raise an event for stop blink of synch indicator

#End Region


#Region "Properties"
    Public Event PhoneInCharge()

    ReadOnly Property phoneConnection() As String
        Get
            Return phoneConn.ToString()
        End Get
    End Property

    ReadOnly Property networkOperator() As String
        Get
            Return networkName
        End Get
    End Property

    ReadOnly Property networkAvailable() As String
        Get
            Return networkAvail.ToString()
        End Get
    End Property

    ReadOnly Property batteryLevel() As Integer
        Get
            Return battery
        End Get
    End Property
    ReadOnly Property batteryStatus() As String
        Get
            Return BatteryFullCharge
        End Get
    End Property
    ReadOnly Property batteryincharge() As String
        Get
            Return ExternalPowerIsConnected
        End Get
    End Property

    ReadOnly Property phoneModelName() As String
        Get
            Return phoneModel
        End Get
    End Property

    ReadOnly Property phoneManufacturerName() As String
        Get
            Return phoneManufacturer
        End Get
    End Property

    ReadOnly Property phoneSubscriberNumber() As String
        Get
            Return Subscriber
        End Get
    End Property

    ReadOnly Property ATResultValue() As String
        Get
            Return ATValueResult
        End Get
    End Property

    ReadOnly Property signalStrength() As Integer
        Get
            Return rssi
        End Get
    End Property

    'ReadOnly Property plinkQuality() As Integer
    '    Get
    '        Return plink_quality
    '    End Get
    'End Property

    'ReadOnly Property tTimeo() As Integer
    '    Get
    '        Return pTimeOut
    '    End Get
    'End Property

    ReadOnly Property callerID() As String
        Get
            Dim str As StringBuilder = New StringBuilder("")
            If currentCalls.Count > 0 Then
                For i As UInt32 = 0 To currentCalls.Count - 1
                    str.Append(currentCalls(i).Name & " ")
                Next
            End If
            Return str.ToString()
        End Get
    End Property

    ReadOnly Property phonebook() As String
        Get
            Return phoneBookData.List.ToString()
        End Get
    End Property

#End Region

    Private Sub initVars()
        battery = 0
        phoneModel = ""
        phoneManufacturer = ""
        rssi = 0
        networkAvail = False
        networkName = ""
        currentCalls = New List(Of contact)
        SCO = False
        standby = True
        phoneConn = False
        bSiri = False
        phoneBookData = New phoneBook()
        PhoneMacAddress = ""
    End Sub
    Public Sub New()
        Try
            name = "MR2"
            initVars()
            hfService = BTSDK_INVALID_HANDLE

            'mapService = BTSDK_INVALID_HANDLE 'service SMS

            ''''''''''''''''''''''''''''''
            ''' Set phone_addr Here    '''
            ''''''''''''''''''''''''''''''

            'If TempPluginSettings.AutoSwapPhone = "True" Then
            '    If PhoneCheckedIs = 1 Then PhoneMacAddress = TempPluginSettings.PhoneMacAddress
            '    If PhoneCheckedIs = 2 Then PhoneMacAddress = TempPluginSettings.PhoneMacAddress2
            'Else
            PhoneMacAddress = TempPluginSettings.PhoneMacAddress
            'End If

            If Not TempPluginSettings.PhoneMacAddress.Contains(":") Or Not TempPluginSettings.PhoneMacAddress2.Contains(":") Then
                MessageBox.Show("Phone Mac Address bad format")
                Exit Sub
            End If

            'If Not InStr(LCase(FileName), ".wav") Then Throw New Exception("Invalid File Extension Specified!")
            'If Not My.Computer.FileSystem.FileExists(FileName) Then Throw New Exception("File Does Not Exist!")
            Dim dt1 = Split(PhoneMacAddress, ":") 'découpage de l'adresse mac lu dans le fichier mobilephone.xml
            'My mac address is : 44:4E:1A:1C:C6:15
            phoneAddr(5) = HexaToDec(dt1(0)) '("44")
            phoneAddr(4) = HexaToDec(dt1(1)) '("4E")
            phoneAddr(3) = HexaToDec(dt1(2)) '("1A")
            phoneAddr(2) = HexaToDec(dt1(3)) '("1C")
            phoneAddr(1) = HexaToDec(dt1(4)) '("C6")
            phoneAddr(0) = HexaToDec(dt1(5)) '("15")


            'Prevent Garbage Collection
            connEventGlobal = AddressOf connEvent
            hfEventGlobal = AddressOf hfEvent

            'PBAP event (contacts)
            pbapEventGlobal = AddressOf pbapEvent

            'MAP event (messages)
            'mapEventGlobal = AddressOf mapEvent
            'connEventSendReceivedGlobal = AddressOf connSendReceivedUpdate

            threadWaitForPhone = New Thread(AddressOf waitForPhone)
            threadWaitForPhone.IsBackground = True
            threadWaitForPhone.Start()

            ''LogValues timer
            'Timer1 = New System.Timers.Timer()
            'Timer1.Enabled = True
            'Timer1.Interval = 30000
            'Timer1.AutoReset = True

        Catch ex As Exception
            MsgBox("Sub New Catch - " & ex.Message)
        End Try

    End Sub
    Private Sub waitForPhone()





        While True
            If Btsdk_IsSDKInitialized() = BTSDK_FALSE Then
                If Btsdk_Init() = BTSDK_OK Then
                    'see 5.2.3 of SDK 2.1.3
                    'BTSDK_DEVCLS_PHONE -> 0X000200 Phone major device class.
                    'BTSDK_PHONECLS_CELLULAR -> 0X000204 Cellular phone.
                    'BTSDK_SRVCLS_AUDIO -> 0x200000 Audio (Speaker, Microphone, Headset service, …).
                    'BTSDK_AV_HEADSET   -> 0X000404 Wearable headset device.
                    'BTSDK_SRVCLS_AUDIO -> 0x200000 Audio (Speaker, Microphone, Headset service, …).
                    'Btsdk_SetLocalDeviceClass(&H240404) 'Original
                    'Btsdk_SetLocalDeviceClass(BTSDK_DEVCLS_PHONE + BTSDK_PHONECLS_CELLULAR + BTSDK_SRVCLS_AUDIO) 'OK
                    Btsdk_SetLocalDeviceClass(BTSDK_PHONECLS_UNCLASSIFIED + BTSDK_PHONECLS_CELLULAR + BTSDK_SRVCLS_AUDIO) 'OK
                    getPairedPhoneHandle()
                    setCallBacks()
                    HFPInit()

                End If
            Else 'BTSDK_TRUE car Btsdk_Init() = BTSDK_OK
                'If TempPluginSettings.AutoSwapPhone = "True" Then
                'If PhoneCheckedIs = 1 Then RaiseEvent FirstPhoneNotFound()
                'If PhoneCheckedIs = 2 Then RaiseEvent SecondPhoneNotFound()
                'End If
                Exit While
            End If
            Thread.Sleep(1000)
        End While

        If (IsNothing(threadStartup)) Then
            threadStartup = New Thread(AddressOf startup)
            threadStartup.IsBackground = True
        End If
        While True
            'getPairedPhoneHandle
            connectAG_HANDSFREE()
            'connectPBAPService()
            'getPBAPBook("Contacts")
            'getPBAPBook("History")

            While True
                If PhoneConnected.WaitOne(5000) Then
                    Exit While
                End If
                connectAG_HANDSFREE()
            End While
            threadStartup.Start()
            phoneConn = True
            PhoneDisconnected.WaitOne()
            initVars()
            'getPairedPhoneHandle()
            threadStartup.Abort()
            threadStartup = New Thread(AddressOf startup)
            threadStartup.IsBackground = True
            phoneConn = False
        End While
    End Sub

    Private Sub startup()
        Dim MT_Exists As Integer = 0
        'Perform Startup Duties in a Seperate Thread
        Dim PhoneListBypass As Boolean = False


        On Error Resume Next
        If TempPluginSettings.PhoneBookList = "" Then
            getPhoneBookTypeList() 'Read what Phone Book are Usable from the phone
        End If
        Thread.Sleep(500)
        CheckReceiveSMS() 'prepare au test de l'arrivée d'un nouveau SMS
        Thread.Sleep(500)
        requestPhoneInfo() 'Read Info from Phone
        Thread.Sleep(400)


        RaiseEvent SetIndBlinkOn()
        If TempPluginSettings.PhoneBookUpdate = True Then 'mise à jour des phone book
            Dim PhoneBooKListArray() As String = TempPluginSettings.PhoneBookList.Split(",")
            For Each PhoneBookList As String In PhoneBooKListArray 'Read each phone book usable and sav it
                CurrentPhoneBook = PhoneBookList
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Extracting " & CurrentPhoneBook & " phonebook ..." & vbCrLf & vbCrLf)
                If PhoneBookList <> "MT" Then
                    File.Delete(MainPath & "PhoneBook\MobilePhone_" & PhoneBookList & ".txt")
                    PhoneBookSyncInProgress = True
                    If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Setting Sync Status ..." & vbCrLf & vbCrLf)
                    getPhoneBook(PhoneBookList)
                    Do Until PhoneBookSyncInProgress = False
                        If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Sync Status Is ..." & PhoneBookSyncInProgress & vbCrLf & vbCrLf)
                        Thread.Sleep(3000)
                    Loop
                End If
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText(PhoneBookList & " phonebook extract is done ..." & vbCrLf & vbCrLf)
            Next
            If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Update phonebooks is done ..." & vbCrLf & vbCrLf)

            PhoneBookMerge("SM", "MT")

            PhoneBookMerge("ME", "MT")
            If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Merge phonebooks is done ..." & vbCrLf & vbCrLf)

            Do While PhoneBookMergeInProgress = True
                Thread.Sleep(3000)
            Loop

            RaiseEvent SetIndBlinkOff()
            'PhoneBookSortAll("ME")
        Else
            If debugFrm.Visible Then debugFrm.debugTextBox.AppendText(vbCrLf & vbCrLf & "If you want to update the phone book, turn in TRUE" & vbCrLf & "the PhoneBookUpdate settings into your .xml file ..." & vbCrLf & vbCrLf)
        End If

        'ATExecute("AT+CMEE=2" & vbCr, 500) 'define extract ERROR to verbose mode
        ATExecute("AT+CMGF=?" & vbCr, 500)

        Dim LocalName As String = "MyPhone"
        Btsdk_SetLocalName(LocalName, LocalName.Length)

        'Thread.Sleep(1000)
        'sort all phonebook in a new thread
        'workerThread = New Threading.Thread(AddressOf PhoneBookSort)
        'workerThread.Start()

        'LogValues timer
        Timer1 = New System.Timers.Timer()
        Timer1.Enabled = True
        Timer1.Interval = 30000
        Timer1.AutoReset = True

        'While True
        '    'Keep requesting signal quality every 30 seconds
        '    ATExecute("AT+CSQ" & vbCr, 2000)
        '    'Btsdk_GetRemoteRSSI(connHandle, rssi)
        '    ATExecute("AT+CPMS=""SM""" & vbCr, 2000) 'check if sms is received ?
        '    'ATExecute("AT+CPMS=""ME""" & vbCr, 1000)'check if sms is received ?
        '    'ATExecute("AT+CNMI=?" & vbCr, 1000)
        '    Thread.Sleep(30000)
        'End While
    End Sub

    Public Sub answerCall()
        If Btsdk_HFAP_AnswerCall(connHandle) = BTSDK_OK Then
            CallIsActif = True
            RaiseEvent PhoneInCall()
        End If
    End Sub

    Public Sub hangupCall()
        If Btsdk_HFAP_CancelCall(connHandle) = BTSDK_OK Then
            CallIsActif = False
        End If
    End Sub

    Public Sub dtmf(ByVal num As String)
        If num.Length = 1 Then
            Dim encoding As New System.Text.ASCIIEncoding()
            Dim bytes() As Byte = encoding.GetBytes(num)
            Btsdk_HFAP_TxDTMF(connHandle, bytes(0))
        End If
    End Sub

    Public Sub connEvent(ByVal hdl As UInt32, ByVal reason As UInt16, ByVal arg As IntPtr)
        Dim connProperties As BtSdkConnectionPropertyStru = Marshal.PtrToStructure(arg, GetType(BtSdkConnectionPropertyStru))

        If isPhone(connProperties.device_handle) Then
            Select Case reason
                Case BTSDK_APP_EV_CONN_IND 'A remote device connects to a local service record.
                    PhoneConnected.Set()
                    PhoneDisconnected.Reset()
                Case BTSDK_APP_EV_DISC_IND 'The remote device disconnects the connection, or the connection is lost due to radio communication problems, e.g. the remote device is out of communication range.
                    PhoneDisconnected.Set()
                    PhoneConnected.Reset()
                Case BTSDK_APP_EV_CONN_CFM 'A local device connects to a remote service record.
                    PhoneConnected.Set()
                    PhoneDisconnected.Reset()
                Case BTSDK_APP_EV_DISC_CFM 'The local device disconnects the connection from remote service.
                    PhoneDisconnected.Set()
                    PhoneConnected.Reset()
                Case Else

            End Select
        End If
        'If isPhone(connProperties.device_handle) Then
        '    connProperties.received_bytes.ToString()
        'End If
    End Sub

    Public Function ATExecute(ByVal cmd As String, ByRef timeout As UInt32) As Boolean
        Return Btsdk_HFP_ExtendCmd(connHandle, cmd, cmd.Length, timeout)
        'Return Btsdk_HFP_ExtendCmd(connHandle, cmd, cmd.Length, 0)
    End Function

    Public Sub contactRetrieve(ByVal contactsNum As UInt32)
        Dim nContactsRetrieved As UInt32 = 1
        If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Extracting " & CurrentPhoneBook & vbCrLf)
        If CurrentPhoneBook = "DC" Or CurrentPhoneBook = "RC" Or CurrentPhoneBook = "MC" Then
            If PhoneNumberOfContactsFound > 30 Then PhoneNumberOfContactsFound = 30
            If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Setting Extract Limit to " & PhoneNumberOfContactsFound & vbCrLf)
        End If
        For PhoneNumberOfContactsLoop As Integer = 1 To PhoneNumberOfContactsFound
            ATExecute("AT+CPBR=" & PhoneNumberOfContactsLoop & vbCrLf, 0)
        Next
        If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Sleeping for " & (PhoneNumberOfContactsFound * 100) & vbCrLf)
        Thread.Sleep(PhoneNumberOfContactsFound * 100)
        If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Setting Sync Flag to False" & vbCrLf)
        PhoneBookSyncInProgress = False
        '        While (nContactsRetrieved <= contactsNum)

        'Set the next contact to look out for
        '       contactNum = nContactsRetrieved

        '        ATExecute("AT+CPBR=" & nContactsRetrieved & "," & nContactsRetrieved & vbCr, 2000)
        '        If (ContactsRetrieved.WaitOne(2000)) Then
        '        nContactsRetrieved += 1
        '        'Position 128 has no contact Unsure of position 256 I dont have that many contacts!
        '        If (nContactsRetrieved = 128) Then
        '        nContactsRetrieved += 1
        '        End If
        '        End If
        '        End While
    End Sub
    Private Sub processATResult(ByVal result As String)
        'MessageBox.Show(result)
        Dim atRegex As New Regex(".*?\+(?<cmd>[A-Z]+):\s*(?<result>.*)", RegexOptions.IgnoreCase)
        Dim atMatch As Match = atRegex.Match(result.Trim())
        'If debugFrm.Visible Then debugFrm.debugTextBox.AppendText(atMatch.Groups("cmd").Value & ": " & atMatch.Groups("result").Value & vbCrLf)
        If debugFrm.Visible Then debugFrm.debugTextBox.AppendText(result)
        'If debugFrm.Visible And Not result.Contains("CLAC") Then debugFrm.debugTextBox.AppendText(result)
        'Dim MyATValueResult = atMatch.Value
        If atMatch.Success Then
            Select Case atMatch.Groups("cmd").Value
                Case "CSQ"
                    'Signal Quality
                    Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),([0-9]+)")
                    rssi = Byte.Parse(resultMatch.Groups(1).Value)
                Case "CBC"
                    'External power check
                    Dim battinfo() As String, ExternalPowerIsON As String
                    ExternalPowerIsON = Replace(atMatch.Value, Chr(0), "")
                    If Left(ExternalPowerIsON, 4) = "+CBC" Then
                        ExternalPowerIsON = Mid(ExternalPowerIsON, 6).Replace(Chr(34), "").Trim
                    End If
                    battinfo = ExternalPowerIsON.Split(",")
                    If battinfo(0) = 1 And ExternalPowerIsConnected = False Then
                        ExternalPowerIsConnected = True
                        RaiseEvent ExtPowerBattON()
                    ElseIf battinfo(0) = 0 And ExternalPowerIsConnected = True Then
                        ExternalPowerIsConnected = False
                        RaiseEvent ExtPowerBattOFF()
                    End If
                    'Case "CGMM"
                    '    'Model
                    '    phoneModel = atMatch.Groups("result").Value
                    'Case "CGMI"
                    '    'Manufacturer
                    '    phoneManufacturer = atMatch.ToString 'atMatch.Groups("result").Value.Replace("""", "")
                Case "CPMS" 'check le nombre de sms reçus
                    'AT+CPMS="SM" return +CPMS: 0,50,0,50,17,1000
                    'AT+CPMS="ME" return +CPMS: 17,1000,0,50,17,1000
                    'AT+CPMS? return +CPMS: "ME",17,1000,"SM",0,50,"ME",17,1000 if AT+CPMS="SM"
                    'AT+CPMS? return +CPMS: "SM",0,50,"SM",0,50,"ME",17,1000 if AT+CPMS="ME"
                    Dim NumberOfSMSInString() As String
                    'If atMatch.Groups("cmd").Value.Contains("SM") Then
                    '    NumberOfSMSInMemory = atMatch.Value.ToString 'atMatch.Groups("result").Value.Replace("+CPMS:", "")
                    '    NumberOfSMSInMemory = Trim(NumberOfSMSInMemory)
                    '    sp = NumberOfSMSInMemory.Split(",")
                    '    NumberOfSMSInMemory = sp(0) & "/" & sp(1)
                    'End If
                    'If atMatch.Groups("cmd").Value.Contains("ME") Then
                    '    NumberOfSMSInMemory = atMatch.Groups("result").Value.Replace("+CPMS:", "")
                    '    NumberOfSMSInMemory = Trim(NumberOfSMSInMemory)
                    '    sp = NumberOfSMSInMemory.Split(",")
                    '    NumberOfSMSInMemory = sp(0) & "/" & sp(1)
                    'End If
                    'MsgBox(atMatch.Value)
                    '20-7-2015
                    NumberOfSMSInMemoryNew = atMatch.Value.Replace(Chr(34), "").Replace("+CPMS:", "")
                    NumberOfSMSInString = NumberOfSMSInMemoryNew.Split(",")
                    NumberOfSMSInMemoryNew = NumberOfSMSInString(0)
                    If NumberOfSMSInMemoryOld < NumberOfSMSInMemoryNew Then
                        SmsIsReceivedStatus = True
                    End If
                    NumberOfSMSInMemoryOld = NumberOfSMSInMemoryNew
                    'MsgBox(NumberOfSMSInMemory)
                    '20-7-2015
                Case "CMGW" 'This command is used to store message in the SIM
                    'Number on which message has been stored.
                    'SmsToSend = atMatch.Groups("result").Value 'Replace(atMatch.Value, Chr(0), "")
                    'If Left(smstosend, 5) = "+CMGW" Then
                    '    smstosend = Mid(smstosend, 7).Replace(Chr(34), "").Trim
                    'End If
                    'ATValueResult = atMatch.Groups("result").Value
                    'MsgBox(atMatch.Value)
                    'If debugFrm.Visible Then debugFrm.debugTextBox.AppendText(result)
                Case "CMTI" 'check new message received(incompatible with my phone)
                    '+CMTI: "ME",3
                    Dim sms() As String
                    NewSmsIsReceived = Replace(atMatch.Value, Chr(0), "")
                    If Left(NewSmsIsReceived, 5) = "+CMTI" Then
                        NewSmsIsReceived = Mid(NewSmsIsReceived, 7).Replace(Chr(34), "").Trim
                    End If
                    sms = NewSmsIsReceived.Split(",")
                    'NewSmsIsReceivedMemory = sms(0)
                    NewSmsIsReceivedNumber = sms(1)
                    SmsIsReceivedStatus = True
                    'MsgBox(atMatch.Value) 'OK
                    RaiseEvent SMSIsReceived() 'MessageBox.Show("The new message is on the memory '" & NewSmsIsReceivedNumber & "'")
                Case "COPS" 'lecture de noms d'opérateurs
                    OperatorNames = atMatch.Value
                Case "CGSN" 'lecture N°IMEI
                    ImeiNumber = atMatch.Value
                    'MsgBox(ImeiNumber)
                Case "CIMI" 'lecture N°ISMI
                    IsmiNumber = atMatch.Value
                Case "CGMR" 'lecture revision number
                    RevisionNumber = atMatch.Value
                    'MsgBox(RevisionNumber)

                    'Case "CNUM" 'lecture du numéro de mon tel
                    '    Dim Subs() As String = atMatch.Value.Split(",")
                    '    Subscriber = Subs(1).Replace(Chr(34), "")

                Case "ELIB" 'Ericsson list Bluetooth devices
                    'MessageBox.Show(atMatch.Value.ToString)

                Case "CCLK" 'return date  and time from phone 
                    '+CCLK: "15/07/17,18:44:29+04"
                    Dim PhoneDateTime() As String = atMatch.Groups("result").Value.Replace(Chr(34), "").Split(",")
                    PhoneDate = PhoneDateTime(0)
                    PhoneTime = Left(PhoneDateTime(1), 8)

                Case "CPBR"
                    'Number of Records on Phone
                    'Time to retrieve them all
                    '(1-XXX),X,X
                    Dim recordMatch As Match = Regex.Match(atMatch.Groups("result").Value, "\(([0-9]+)-([0-9]+)\),([0-9]+),([0-9]+)")
                    Dim CallName As String
                    If (recordMatch.Success) Then
                        PhoneNumberOfContactsFound = Mid(recordMatch.ToString, 4, (InStr(1, recordMatch.ToString, ")") - 4))
                        Dim contactRetrieveThread As Thread = New Thread(AddressOf contactRetrieve)
                        contactRetrieveThread.IsBackground = True
                        contactRetrieveThread.Start(UInt32.Parse(recordMatch.Groups(2).Value))
                    Else
                        On Error Resume Next
                        'MsgBox("adding" & atMatch.Groups("result").Value)
                        'Phonebook Record
                        'Add to Phonebook
                        Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),""(\+{0,1}[0-9]+)"",([0-9]+),""(.*?)""")
                        Dim c As contact = New contact(UInt32.Parse(resultMatch.Groups(1).Value), resultMatch.Groups(4).Value, resultMatch.Groups(2).Value)
                        phoneBookData.add(c)
                        'If CheckIfContactIsInList(MainPath & "ContactsList.lst", resultMatch.Groups(2).Value) = False Then
                        'AddCustomList(MainPath & "PhoneBook\MobilePhone_" & PhoneListType & ".txt", resultMatch.Groups(2).Value, resultMatch.Groups(4).Value.Replace("/M", " Mobile").Replace("/H", " Home").Replace("/W", " Work"), "")
                        'End If
                        'Public Sub AddCustomList(ByVal CustomList As String, ByVal Number As String, ByVal Name As String, ByVal IconType As String)
                        If resultMatch.Groups(4).Value = "" Then
                            CallName = resultMatch.Groups(2).Value
                        Else
                            CallName = resultMatch.Groups(4).Value
                        End If
                        Dim objStreamWriter As StreamWriter
                        objStreamWriter = New StreamWriter(MainPath & "PhoneBook\MobilePhone_" & PhoneListType & ".txt", True, Encoding.Unicode)
                        If File.Exists(MainPath & "PhoneBook\MobilePhone_" & PhoneListType & ".txt") = False Then
                            objStreamWriter.WriteLine("0")
                        End If
                        objStreamWriter.WriteLine("LST" & resultMatch.Groups(2).Value & "||" & CallName.Replace("/M", " Mobile").Replace("/H", " Home").Replace("/W", " Work")) ', resultMatch.Groups(2).Value, resultMatch.Groups(4).Value.Replace("/M", " Mobile").Replace("/H", " Home").Replace("/W", " Work"), ""
                        'If IconType <> "" Then
                        '   objStreamWriter.WriteLine("ICO" & IconType)
                        'End If
                        objStreamWriter.Close()

                        PhoneNumberOfContactsFound = UInt32.Parse(resultMatch.Groups(1).Value)
                        'Set the Global Var to indicate we can go to the next contact
                        If (UInt32.Parse(resultMatch.Groups(1).Value) = contactNum) Then
                            ContactsRetrieved.Set()
                        End If
                    End If
                Case "CPBS"
                    'Phone Lists usable
                    If TempPluginSettings.PhoneBookListUpdate = True Then 'force la mise à jour des listes et leur update
                        PhoneListTypeUsable = atMatch.Groups("result").Value.Replace("(", "").Replace(")", "").Replace(Chr(34), "")
                        'MsgBox(PhoneListTypeUsable)
                        If PhoneListTypeUsable <> "" Then TempPluginSettings.PhoneBookList = PhoneListTypeUsable
                        If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                            cMySettings.Copy(TempPluginSettings, PluginSettings)
                            cMySettings.SerializeToXML(PluginSettings)
                        End If
                    Else
                        PhoneListTypeUsable = TempPluginSettings.PhoneBookList
                    End If

                Case "CSCA"
                    '+CSCA: "+33695000695",145
                    Dim SmsCenter() As String = atMatch.Groups("result").Value.Replace(Chr(34), "").Split(",")
                    SmsCenterServiceNumber = SmsCenter(0)
                    'MessageBox.Show(SmsCenterServiceNumber)
                    If SmsCenterServiceNumber <> TempPluginSettings.SmsServiceCentreAddress Then TempPluginSettings.SmsServiceCentreAddress = SmsCenterServiceNumber
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If

                    'MsgBox(PhoneListTypeUsable)
                Case "CMGF"
                    'Check if device accept text or pdu modes
                    Dim PduAccepted As String = atMatch.Groups("result").Value.Replace("(", "").Replace(")", "").Replace(Chr(34), "")
                    If PduAccepted.Contains("0") Then
                        PDUTextModesUsable = 0
                        'MessageBox.Show("Your device accept only the PDU Mode")
                    ElseIf PduAccepted.Contains("1") Then
                        PDUTextModesUsable = 1
                        'MessageBox.Show("Your device accept only the Text Mode")
                    ElseIf PduAccepted.Contains("0") AndAlso PduAccepted.Contains("1") Then
                        PDUTextModesUsable = 2
                        'MessageBox.Show("Your device accept the PDU and Text Modes")
                    End If

                Case "CMGR" 'lecture du message sms reçu (wait an AT commans as --> AT+CMGR=3)
                    'SmsToRead = atMatch.Value.ToString.Replace(vbCr, "")
                    'SmsToRead = atMatch.Value.ToString.Replace(vbCrLf, "")

                    'MsgBox(atMatch.Groups(0).Value.Trim.Replace(vbCrLf, ""))
                    'MsgBox(atMatch.Groups(1).Value.Trim.Replace(vbCrLf, ""))
                    'MsgBox(atMatch.Groups(2).Value.Trim.Replace(vbCrLf, ""))
                    'MsgBox(atMatch.Groups(3).Value.Trim.Replace(vbCrLf, ""))
                    'MsgBox(atMatch.Groups(4).Value.Trim.Replace(vbCrLf, ""))

                    'MessageBox.Show(atMatch.Groups("result").Value)
                    'MessageBox.Show(atMatch.Value)
                    'SmsToRead = result
                    'Thread.Sleep(1000)
                    'MessageBox.Show(SmsToRead)

                    Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+)")
                    MessageBox.Show(resultMatch.Groups(1).Value)

                Case "CMGL" 'lecture par type de sms en  mode pdu
                    'Number of Records on SMS
                    'Time to retrieve them all
                    '(1-XXX),X,X
                    MessageBox.Show(atMatch.Groups("result").Value.Replace(vbCrLf, ""))
                    'Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),""(\+{0,1}[0-9]+)"",([0-9]+),""(.*?)""")
                    'If (recordMatch.Success) Then
                    '    MessageBox.Show(recordMatch.ToString)
                    '    'Dim smsNowFound As Integer = Mid(recordMatch.ToString, 4, (InStr(1, recordMatch.ToString, ")") - 4))
                    '    'Dim contactRetrieveThread As Thread = New Thread(AddressOf contactRetrieve)
                    '    'contactRetrieveThread.IsBackground = True
                    '    'contactRetrieveThread.Start(UInt32.Parse(recordMatch.Groups(2).Value))
                    'Else
                    '    On Error Resume Next
                    '    'MsgBox("adding" & atMatch.Groups("result").Value)
                    '    'SMS Record
                    '    'Add to SMS
                    '    'SynchroningInProgress = True
                    '    'Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),""(\+{0,1}[0-9]+)"",([0-9]+),""(.*?)""")
                    '    'Dim c As contact = New contact(UInt32.Parse(resultMatch.Groups(1).Value), resultMatch.Groups(4).Value, resultMatch.Groups(2).Value)
                    '    'phoneBookData.add(c)
                    '    ''If CheckIfContactIsInList(MainPath & "PhoneBook\MobilePhone_" & PhoneListType & ".txt", resultMatch.Groups(2).Value) = False Then
                    '    'AddCustomList(MainPath & "PhoneBook\MobilePhone_" & PhoneListType & ".txt", resultMatch.Groups(2).Value, resultMatch.Groups(4).Value.Replace("/M", " Mobile").Replace("/H", " Home").Replace("/W", " Work"), "")
                    '    ''End If
                    '    'PhoneNumberOfContactsFound = UInt32.Parse(resultMatch.Groups(1).Value)
                    '    ''Set the Global Var to indicate we can go to the next contact
                    '    'If (UInt32.Parse(resultMatch.Groups(1).Value) = contactNum) Then
                    '    '    ContactsRetrieved.Set()
                    '    'End If
                    '    MessageBox.Show(atMatch.Groups(1).Value & vbCrLf & atMatch.Groups(4).Value & vbCrLf & atMatch.Groups(2).Value)

                    'End If
                    'On Error Resume Next
                    'Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),([0-9]+)")
                    'For i = 1 To resultMatch.Length
                    '    'resultMatch.Captures
                    '    Dim smsresult As Byte = Byte.Parse(resultMatch.Groups(i).Value)
                    '    MessageBox.Show(smsresult.ToString)
                    'Next
                    'Dim SMSInString() As String
                    ''NumberOfSMSInMemoryNew = atMatch.Value.Replace(Chr(34), "").Replace("+CPMS:", "")
                    'SMSInString = NumberOfSMSInMemoryNew.Split(vbCrLf)
                    ''NumberOfSMSInMemoryNew = SMSInString(0)
                    'For i = 0 To SMSInString.Length - 1
                    '    MsgBox(SMSInString(i))
                    'Next
                    'NumberOfSMSInMemoryOld = NumberOfSMSInMemoryNew

                Case "CDS"
                    '    MessageBox.Show(atMatch.Groups(1).Value & vbCrLf & atMatch.Groups(4).Value & vbCrLf & atMatch.Groups(2).Value)
                    '    'MessageBox.Show(atMatch.Groups("mobilephone_indicatorsevent").Value)
                    MessageBox.Show(atMatch.Value)

                Case "CMER"
                    'MER: 3,0,0,1,0

                Case "CIND"
                    '1 Battery charge level indicator
                    '2 Signal quality indicator
                    '3 Battery warning indicator
                    '4 Charger connected indicator
                    '5 Service availability indicator
                    '6 Sounder activity indicator
                    '7 Message received indicator
                    '8 Call-in-progress indicator
                    '9 Transmit activated by voice activity indicator
                    '10:Roaming(indicator)
                    '11 Short message
                    'IND: 1,0,0,0,1,0,5,0,0,1
                    Dim PhoneEvent() As String = atMatch.Groups("result").Value.Split(":")
                    'Dim PhoneEventStatus() As String = PhoneEvent(1).Split(",")
                    'MessageBox.Show(atMatch.Value)
                    MessageBox.Show(PhoneEvent(1).ToString)

                    'Case "CLAC" 'check the accepted AT commands
                    '    MessageBox.Show(atMatch.Value)
                Case Else

            End Select

        End If
    End Sub

    Public Sub hfEvent(ByVal hdl As UInt32, ByVal e As UInt16, ByVal param As IntPtr, ByVal param_len As UInt16)

        Select Case (e)
            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SLC_ESTABLISHED_IND
                'Not Needed
                'Service Level Connection made

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SLC_RELEASED_IND
                'Not Needed
                'Service Level Connection not made

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_AUDIO_CONN_ESTABLISHED_IND
                'Phone is transmitting Audio Now!
                'Set Event to Show Screen
                'This is in case we are on a call before the BT Link is setup
                'But could be because of Siri?
                'Trigger only if another event hasnt already
                RaiseEvent PhoneInCall()
                SCO = True
                If standby And bSiri = False Then
                    standby = False
                    RaiseEvent Ringing()
                End If

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_AUDIO_CONN_RELEASED_IND
                'Phone has stopped sending us audio
                'Doesnt mean the call has finished
                SCO = False

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_STANDBY_IND
                'In Standby ie. No active calls
                'either with us as the audio gateway or not
                'Trigger only when we have come out of a call
                If Not standby And bSiri = False Then
                    standby = True
                    RaiseEvent Hungup()
                    currentCalls.Clear()
                End If

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_RINGING_IND
                'Phone is Ringing Get Call Info
                standby = False
                Btsdk_HFAP_GetCurrentCalls(connHandle)
                RaiseEvent Ringing()

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_ONGOINGCALL_IND
                'SetText("Ongoing Call")
                IncomingCall = True
                RaiseEvent PhoneInCall()
            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_OUTGOINGCALL_IND
                'Manually Dialled from Phone Get Info
                standby = False
                Btsdk_HFAP_GetCurrentCalls(connHandle)
                RaiseEvent Ringing()

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_TERMINATE_LOCAL_RINGTONE_IND
                'SetText("Terminate local ringtone Call>")

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_VOICE_RECOGN_ACTIVATED_IND
                bSiri = True
                RaiseEvent VoiceActivation()

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_VOICE_RECOGN_DEACTIVATED_IND
                bSiri = False
                RaiseEvent VoiceDeactivation()

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_NETWORK_AVAILABLE_IND
                networkAvail = True
                RaiseEvent PhoneNetAvail()
            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_NETWORK_UNAVAILABLE_IND
                networkAvail = False
                RaiseEvent PhoneNetUnAvail()

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SPKVOL_CHANGED_IND
                'Speaker vol not needed

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_MICVOL_CHANGED_IND
                'Micvol not needed

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_VOICETAG_PHONE_NUM_IND
                'Voicetag Not needed

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_EXTEND_CMD_IND
                Dim result As String = Marshal.PtrToStringAnsi(param, param_len)
                processATResult(result)
                'ATValueResult = e.ToString

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_ATCMD_RESULT
                'Dim res As Btsdk_HFP_ATCmdResult = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_ATCmdResult))
                'SetText("Receiving  AT command: " & res.cmd_code & " " & res.result_code)
                'MessageBox.Show("Receiving  AT command: " & res.cmd_code & " " & res.result_code)
                'ATValueResult = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64 + 3), res.result_code) 'res.result_code 'ajout pierre
                'ATValueResult = Hex(Marshal.ReadByte(param)) 'Marshal.PtrToStringAnsi(param.ToInt64, res.result_code)
                'Dim result As String = Marshal.PtrToStringAnsi(param, param_len)
                'ATValueResult = Hex(e.ToString)
                'MsgBox(ATValueResult)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_CLIP_IND
                'SetText("HFP EV")
                Dim res As Btsdk_HFP_PhoneInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_PhoneInfo))
                Subscriber = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64 + 3), res.num_len)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_NETWORK_OPERATOR_IND 'OK
                Dim network_operator As Btsdk_HFP_COPSInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_COPSInfo))
                networkName = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64() + 3), network_operator.operator_len)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND 'OK
                'To get the subscriber number not needed
                Dim res As Btsdk_HFP_PhoneInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_PhoneInfo))
                Subscriber = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64 + 3), res.num_len)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_CURRENT_CALLS_IND 'voir p197
                Dim clcc As Btsdk_HFP_CLCCInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_CLCCInfo))
                'Dim number As String = Marshal.PtrToStringAnsi(IntPtr.Add(param, 7), clcc.num_len)
                Dim number As String = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64() + 7), clcc.num_len)

                currentCalls = phoneBookData.getName(number)
                'MsgBox("got ID")
                If currentCalls.Count = 0 Then
                    Dim c As New contact(0, number, "")
                    currentCalls.Add(c)
                    '******************************************************
                    'ajouter nouvel appelant si n'existe pas ds custom list
                    '******************************************************
                End If


            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SIGNAL_STRENGTH_IND
                rssi = Marshal.ReadByte(param)
            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_BATTERY_CHARGE_IND 'OK
                'Dim csq As Btsdk_HFP_CINDInfoStru = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_CINDInfoStru))
                battery = Marshal.ReadByte(param) 'Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64() + 3), csq.battchg)

                'Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_MAP_EVT_NEWMSG
                '    Dim mapreport As BtSdkMAPEvReportObjStru = Marshal.PtrToStructure(param, GetType(BtSdkMAPEvReportObjStru))
                '    Dim ms_type As String = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64()), mapreport.msg_type(1))
                '    SmsIsReceivedStatus = True
                '    SmsIsReceivedType = ms_type
                '    'Select Case ev_type

                '    'End Select
                '    MsgBox(SmsIsReceivedType)

            Case Else

        End Select
    End Sub
    Public Sub pbapEvent(ByVal first As Byte, ByVal last As Byte, ByVal filename As IntPtr, ByVal filesize As UInt32, ByVal cursize As UInt32)
    End Sub

    Public Sub Siri()
        If (bSiri) Then
            Btsdk_HFAP_VoiceRecognitionReq(connHandle, 0)
            bSiri = False
        Else
            Btsdk_HFAP_VoiceRecognitionReq(connHandle, 1)
            bSiri = True
        End If
    End Sub

#Region "Structure Functions"

    Public Function APP_WriteFile(file_hdl As UInt32, buf As IntPtr, bytes_to_write As UInt32) As UInt32

        Dim data As String = Marshal.PtrToStringAnsi(buf)
        Dim Path As String
        If data Is Nothing Then
            Return 0
        ElseIf Not data.Contains("X-IRMC-CALL-DATETIME") Then
            Path = pbPath




        Else
            Path = historyPath


        End If

        Dim file As New IO.FileInfo(Path)

        Using fs As New FileStream(Path, FileMode.Append)
            Using s As StreamWriter = New StreamWriter(fs)
                s.WriteLine(data)
                s.Close()
            End Using
        End Using
        Return 0

    End Function

    Private Function APP_CreateFile(pth As String) As IntPtr
        Dim fs As FileStream = New FileStream(pth, FileMode.Create, FileAccess.Write)
        Dim filePtr = fs.SafeFileHandle.DangerousGetHandle
        fs.Close()
        Return filePtr
    End Function

#End Region

    Private Function setCallBacks() As Boolean
        Dim cb As BtSdkCallbackStru = New BtSdkCallbackStru()
        cb.func = connEventGlobal
        cb.type = BTSDK_CONNECTION_EVENT_IND

        Dim ptr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(BtSdkCallbackStru)))
        Marshal.StructureToPtr(cb, ptr, True)

        If Btsdk_RegisterCallback4ThirdParty(ptr) = BTSDK_OK Then
            Marshal.FreeHGlobal(ptr)
            Return True
        Else
            Marshal.FreeHGlobal(ptr)
            Return False
        End If
    End Function


    Private Function HFPInit() As Boolean
        If Btsdk_HFAP_APPRegCbk4ThirdParty(hfEventGlobal) = BTSDK_OK Then
            'ORINAL INIT
            'hfService = Btsdk_RegisterHFPService(name, BTSDK_CLS_HANDSFREE_AG, BTSDK_AG_BRSF_3WAYCALL Or _
            '                                        BTSDK_AG_BRSF_BVRA Or BTSDK_AG_BRSF_BINP Or BTSDK_AG_BRSF_REJECT_CALL Or _
            '                                        BTSDK_AG_BRSF_ENHANCED_CALLSTATUS Or BTSDK_AG_BRSF_ENHANCED_CALLCONTROL Or _
            '                                        BTSDK_AG_BRSF_EXTENDED_ERRORRESULT)
            'ORINAL INIT

            'use all AG options + PBAP services
            'Const BTSDK_CLS_PBAP_PCE As UInt32 = &H112E                 'PBAP Phonebook Client Equipment service.
            'Const BTSDK_CLS_PBAP_PSE As UInt32 = &H112F                 'PBAP Phonebook Server Equipment service.
            'Const BTSDK_CLS_PHONEBOOK_ACCESS As UInt32 = &H1130         'Phonebook Access service.
            hfService = Btsdk_RegisterHFPService(name, BTSDK_CLS_HANDSFREE_AG, BTSDK_AG_BRSF_ALL Or _
                                                 BTSDK_CLS_PBAP_PCE Or BTSDK_CLS_PBAP_PSE Or BTSDK_CLS_PHONEBOOK_ACCESS)
            'use all AG options + PBAP services
            Return True
        End If
        Return False
    End Function









    Private Function isPhone(ByVal handle As UInt32) As Boolean
        If handle = BTSDK_INVALID_HANDLE Then
            Return False
        End If

        Dim paddr As IntPtr = Marshal.AllocHGlobal(6)
        If Btsdk_GetRemoteDeviceAddress(phoneHandle, paddr) = BTSDK_OK Then
            For i As Byte = 0 To 5
                If Not Marshal.ReadByte(paddr, i) = phoneAddr(i) Then
                    Marshal.FreeHGlobal(paddr)
                    Return False
                End If
            Next
            Marshal.FreeHGlobal(paddr)
            Return True
        Else
            Marshal.FreeHGlobal(paddr)
            Return False
        End If
    End Function

    Private Function connectAG_HANDSFREE() As Boolean
        Dim svc_attr As BtSdkRemoteServiceAttrStru

        Dim psvc_hndls As IntPtr = Marshal.AllocHGlobal(10 * Marshal.SizeOf(GetType(UInt32))) '32 bits * 10 / 8 = 40 bytes
        Dim psvc_num As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(UInt32))) ' 32 Bits = 4 bytes
        Dim psvc_attr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(BtSdkRemoteServiceAttrStru)))
        Dim pconn_hndl As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(UInt32))) ' 32 Bits = 4 bytes

        'Marshal.StructureToPtr(svc_attr, psvc_attr, False)
        Marshal.WriteInt32(psvc_num, 15)

        If Not Btsdk_BrowseRemoteServices(phoneHandle, psvc_hndls, psvc_num) = BTSDK_OK Then
            Return False
        End If

        connHandle = BTSDK_INVALID_HANDLE

        For i As Byte = 0 To Marshal.ReadInt32(psvc_num) - 1
            If Btsdk_GetRemoteServiceAttributes(Marshal.ReadInt32(psvc_hndls, i * Marshal.SizeOf(GetType(UInt32))), psvc_attr) = BTSDK_OK Then
                svc_attr = Marshal.PtrToStructure(psvc_attr, GetType(BtSdkRemoteServiceAttrStru))
                If svc_attr.service_class = BTSDK_CLS_HANDSFREE_AG Then
                    If Btsdk_Connect(Marshal.ReadInt32(psvc_hndls, i * Marshal.SizeOf(GetType(UInt32))), 0, pconn_hndl) = BTSDK_OK Then
                        'MessageBox.Show(Marshal.ReadInt32(psvc_hndls, i * Marshal.SizeOf(GetType(UInt32))))
                        connHandle = Marshal.ReadInt32(pconn_hndl)
                    End If
                    RaiseEvent PhoneIsConnected()
                    Exit For
                End If

            End If
        Next

        Marshal.FreeHGlobal(psvc_hndls)
        Marshal.FreeHGlobal(psvc_num)
        Marshal.FreeHGlobal(psvc_attr)
        Marshal.FreeHGlobal(pconn_hndl)

        Return Not (connHandle = BTSDK_INVALID_HANDLE)
    End Function


    Private Function connectPBAPService() As Boolean
        Dim svc_attr As BtSdkRemoteServiceAttrStru





        Dim psvc_hndls As IntPtr = Marshal.AllocHGlobal(10 * Marshal.SizeOf(GetType(UInt32))) '32 bits * 10 / 8 = 40 bytes
        Dim psvc_num As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(UInt32))) ' 32 Bits = 4 bytes
        Dim psvc_attr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(BtSdkRemoteServiceAttrStru)))
        Dim pconn_hndl As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(UInt32))) ' 32 Bits = 4 bytes

        Dim sdk As UInt32 = Btsdk_IsSDKInitialized()



        Marshal.WriteInt32(psvc_num, 15)
        If Not Btsdk_BrowseRemoteServices(phoneHandle, psvc_hndls, psvc_num) = BTSDK_OK Then
            Return False
        End If


        pbapHandle = BTSDK_INVALID_HANDLE

        For i As Byte = 0 To Marshal.ReadInt32(psvc_num) - 1
            If Btsdk_GetRemoteServiceAttributes(Marshal.ReadInt32(psvc_hndls, i * Marshal.SizeOf(GetType(UInt32))), psvc_attr) = BTSDK_OK Then
                svc_attr = Marshal.PtrToStructure(psvc_attr, GetType(BtSdkRemoteServiceAttrStru))


                If svc_attr.service_class = BTSDK_CLS_PBAP_PSE Then

                    If Btsdk_Connect(Marshal.ReadInt32(psvc_hndls, i * Marshal.SizeOf(GetType(UInt32))), 0, pconn_hndl) = BTSDK_OK Then
                        pbapHandle = Marshal.ReadInt32(pconn_hndl)
                    End If
                    RaiseEvent PhoneIsConnected()

                    Exit For
                End If

            End If
        Next



        Marshal.FreeHGlobal(psvc_hndls)
        Marshal.FreeHGlobal(psvc_num)
        Marshal.FreeHGlobal(psvc_attr)
        Marshal.FreeHGlobal(pconn_hndl)

        Return Not (pbapHandle = BTSDK_INVALID_HANDLE)
    End Function


    Private Function getPairedPhoneHandle() As Boolean
        If Btsdk_IsBluetoothReady() = BTSDK_FALSE Then
            Return False
        End If

        Dim ptr As IntPtr = Marshal.AllocHGlobal(6)
        Marshal.Copy(phoneAddr, 0, ptr, 6)
        phoneHandle = Btsdk_GetRemoteDeviceHandle(ptr)
        Marshal.FreeHGlobal(ptr)

        If phoneHandle = BTSDK_INVALID_HANDLE Then
            Return False
        End If

        Return True

    End Function
    'Private Function restoreConnection() As Boolean
    '    Dim searchHandle As UInt32 = Btsdk_StartEnumConnection()
    '    Dim connPtr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(BtSdkConnectionProperty)))
    '    Dim connH As UInt32
    '    Dim conn As BtSdkConnectionProperty

    '    If Not searchHandle = BTSDK_INVALID_HANDLE Then
    '        connH = Btsdk_EnumConnection(searchHandle, connPtr)
    '        While Not connH = BTSDK_INVALID_HANDLE
    '            conn = Marshal.PtrToStructure(connPtr, GetType(BtSdkConnectionProperty))
    '            If isPhone(conn.device_handle) And conn.service_class = BTSDK_CLS_HANDSFREE_AG Then
    '                'If isPhone(conn.device_handle) And conn.service_class = BTSDK_CLS_HANDSFREE Then
    '                phoneHandle = conn.device_handle
    '                connHandle = connH
    '                Exit While
    '            End If
    '            connHandle = Btsdk_EnumConnection(searchHandle, connPtr)
    '        End While
    '        Btsdk_EndEnumConnection(searchHandle)
    '    End If

    '    Marshal.FreeHGlobal(connPtr)

    '    If connHandle = BTSDK_INVALID_HANDLE Then
    '        Return False
    '    Else
    '        Return True
    '    End If

    'End Function

    Public Sub getPBAPBook(pbType As String)

        Dim param As New BtSdkPBAPParamStru
        Dim localRepo As String = "telecom/"
        Dim pbName As String = "pb.vcf"
        Dim cchName As String = "cch.vcf"
        Dim combine As String = Nothing
        Dim hFile As UInt32
        Dim result As UInt32 = BTSDK_OK
        Dim hconn As UInt32 = pbapHandle
        Dim fileio_rtns As BtSdkPBAPFileIORoutinesStru = New BtSdkPBAPFileIORoutinesStru
        fileio_rtns.write_file = AddressOf APP_WriteFile
        Dim ptr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(BtSdkPBAPFileIORoutinesStru)))
        Marshal.StructureToPtr(fileio_rtns, ptr, False)

        If Btsdk_IsBluetoothReady() = BTSDK_FALSE Or hconn = BTSDK_INVALID_HANDLE Then
            'MsgBox("No PBAP connection selected!")
            Marshal.FreeHGlobal(ptr)
            Exit Sub

        End If

        Dim regFileIO As UInt32 = Btsdk_PBAPRegisterFileIORoutines(hconn, ptr)

        If pbType Is "History" Then
            If File.Exists(historyPath) Then
                File.Delete(historyPath)
                combine = localRepo & cchName & ControlChars.NullChar
                Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_X_IRMC_CALL_DATETIME)
                hFile = APP_CreateFile(historyPath)
            End If
        ElseIf pbType Is "Contacts" Then
            If File.Exists(pbPath) Then
                File.Delete(pbPath)
                combine = localRepo & pbName & ControlChars.NullChar
                hFile = APP_CreateFile(pbPath)
            End If
        Else
            Exit Sub

        End If

        Dim pbBytes = System.Text.Encoding.ASCII.GetBytes(combine)

        result = Btsdk_PBAPPullPhoneBook(hconn, pbBytes, param, hFile)

        Marshal.FreeHGlobal(ptr)

    End Sub

    Public Sub getPhoneBookTypeList() 'OK
        ATExecute("AT+CPBS=?" & vbCrLf, 500)
        'ATExecute("AT+CPBS=?", 2000)
    End Sub

    'AT+CPBS=? --> +CPBS: ("DC","EN","FD","MC","ON","RC","SM"") Samsung
    'AT+CPBS=? --> +CPBS: ("FD","LD","ME","SM","DC","RC","MC","MV","HP","BC") Sony
    'AT+CPBS?
    '"DC" ME dialled calls list (+CPBW may not be applicable for this storage) $(AT R97)$
    '"EN" SIM (or ME) emergency number (+CPBW is not be applicable for this storage) $(AT R97)$
    '"FD" SIM fixdialling-phonebook
    '"LD" SIM last-dialling-phonebook
    '"MC" ME missed (unanswered received) calls list (+CPBW may not be applicable for this storage)
    '"ME" ME phonebook
    '"MT" combined ME and SIM phonebook
    '"ON" SIM (or ME) own numbers (MSISDNs) list (reading of this storage may be available through +CNUM also) $(AT R97)$
    '"RC" ME received calls list (+CPBW may not be applicable for this storage) $(AT R97)$
    '"SM" SIM phonebook
    '"TA" TA phonebook

    'AT+CPBS=<mem> : Sélection de la mémoire du PB (Phone Book).
    'DC : Liste des appels du ME.
    'EN : Liste des numéros d’urgence (SIM ou ME).
    'FD : Liste des numéros fixes de la SIM.
    'LD : Liste du dernier numéro appelé de la SIM.
    'MC : Liste des numéros d’urgence (SIM ou ME).
    'ME : Liste des numéros du ME.
    'MT : Liste des numéros combinée de la SIM et du ME.
    'ON : Liste des numéros propres de la SIM.
    'RC : Liste des numéros reçus sur le ME.
    'SM : Liste des numéros de la SIM.
    'TA : Liste des numéros du TA.

    ': integer type value indicating the number of used locations in selected memory
    ': integer type value indicating the total number of locations in selected memory
    Public Sub getPhoneBook(ByVal PhoneBookListType As String)
        PhoneListType = PhoneBookListType
        ATExecute("ATE1" & vbCrLf, 500) 'ajout pm
        ATExecute("AT+CPBS=" & PhoneBookListType & "" & vbCrLf, 500) 'define the phonebook to read my phone return +CPBS: ("FD","LD","ME","SM","DC","RC","MC","MV","HP","BC")
        ATExecute("AT+CSCS=""8859-1""" & vbCrLf, 500) 'my phone return +CSCS: ("GSM","IRA","8859-1","UTF-8","UCS2")
        ATExecute("AT+CPBR=?" & vbCrLf, 100)
        'ATExecute("AT+CPBR=1,100" & vbCrLf, 500)
    End Sub


    Public Sub RequestOperatorNames() 'lecture des opérateurs
        'at+cops=? --> +COPS: (2,"Orange F","","80201"),(3,"F SFR","","80210")
        'ATExecute("AT" & vbCr, 500)
        ATExecute("AT+COPS=?" & vbCr, 500)
    End Sub

#Region "Test Read Write SMS"
    Public Sub RequestTypeSMS() 'lecture d'un sms
        'ATExecute("AT" & vbCr, 500)
        ATExecute("AT+CMGF=0" & vbCr, 500) '0 = PDU Mode, 1 = Text Mode
        ATExecute("AT+CMGL=?" & vbCr, 500) 'my phone return in pdu mode +CMGL: (0-4)
    End Sub

    '20-7-2015
    Public Sub ReadAllSMS(ByVal SmsNumber As String) 'lecture d'un sms
        If PDUTextModesUsable = 0 Then 'PDU MODE
            ATExecute("AT+CMGF=0" & vbCrLf, 500)
            'set my character set to Unicode:
            ATExecute("AT+CSCS=""8859-1""" & vbCrLf, 500)
            'set my preferred message storage to MT
            ATExecute("AT+CPMS=""ME""" & vbCrLf, 500)  'my phone return +CPMS: 15,1000,0,50,15,1000
            ATExecute("AT+CMGL=4" & vbCrLf, 500) 'list all my SMS out in PDU mode

        ElseIf PDUTextModesUsable = 1 Or PDUTextModesUsable = 2 Then 'TEXT MODE
            ATExecute("AT+CMGF=1" & vbCr, 500)
            'set my character set to Unicode:
            ATExecute("AT+CSCS=""8859-1""" & vbCr, 500)
            'set my preferred message storage to MT
            ATExecute("AT+CPMS=""ME""" & vbCr, 500)  'my phone return +CPMS: 15,1000,0,50,15,1000
            ATExecute("AT+CMGL=""REC UNREAD""" & vbCr, 500) 'list UNREAD SMS out in Text mode
            'ATExecute("AT+CMGL=""ALL""" & vbCr, 500) 'read ALL SMS out in Text mode
            'AT+CMGR=2 return
            '+CMGL: 2,"REC UNREAD","+31625012354",,"07/07/05,09:56:03+08"
            'Test message 2

        End If


    End Sub
    Public Sub ReadOneSMS(ByVal SmsNumber As String) 'lecture d'un sms
        ATExecute("AT" & vbCr, 500)
        ATExecute("AT+CMGF=0" & vbCr, 500) '0 = PDU Mode, 1 = Text Mode
        'set my character set to Unicode:
        ATExecute("AT+CSCS=""8859-1""" & vbCr, 500)
        'set my preferred message storage to MT
        ATExecute("AT+CPMS=""ME""" & vbCr, 500)  'my phone return +CPMS: 15,1000,0,50,15,1000
        '0 = UNREAD. It refers to the message status "received unread". It is the default value.
        '1 = READ. It refers to the message status "received read".
        '2 = UNSENT. It refers to the message status "stored unsent".
        '3 = SENT. It refers to the message status "stored sent".
        '4 = ALL. It tells the +CMGL AT command to list all messages.
        'ATExecute("AT+CMGL=4" & vbCr, 500) 'list the messages
        ATExecute("AT+CMGR=" & SmsNumber & vbCr, 500) 'read the message
    End Sub
    Public Sub ReadSMSByType(ByVal SmsType As String) 'lecture d'un sms
        ATExecute("AT" & vbCr, 500)
        ATExecute("AT+CMGF=0" & vbCr, 500) '0 = PDU Mode, 1 = Text Mode
        'set my character set to Unicode:
        ATExecute("AT+CSCS=""8859-1""" & vbCr, 500)
        'set my preferred message storage to MT
        ATExecute("AT+CPMS=""ME""" & vbCr, 500)  'my phone return +CPMS: 15,1000,0,50,15,1000
        '0 = UNREAD. It refers to the message status "received unread". It is the default value.
        '1 = READ. It refers to the message status "received read".
        '2 = UNSENT. It refers to the message status "stored unsent".
        '3 = SENT. It refers to the message status "stored sent".
        '4 = ALL. It tells the +CMGL AT command to list all messages.
        ATExecute("AT+CMGL=" & SmsType & vbCr, 500) 'list the messages
    End Sub
    '20-7-2015

    Public Function SendSMS(numberphone As String, message As String) As Boolean
        If PDUTextModesUsable = 0 Then 'PDU MODE
            'return PDUSMSEncodedLenght and PDUSMSEncoded
            Dim pdu As String = WritePDUMessage(TempPluginSettings.SmsServiceCentreAddress, numberphone, message)
            'set command message format to PDU mode(0),(1=Text mode)
            ATExecute("AT+CMGF=0" & vbCr, 2000)
            'force les messages à être écrit dand ME (ME,SM aussi possible)
            ATExecute("AT+CPMS=""ME""" & vbCr, 500)
            'set service center address (which varies for service providers (idea, airtel))
            ATExecute("AT+CSCA=" & Chr(34) & TempPluginSettings.SmsServiceCentreAddress & Chr(34) & vbCr, 2000)
            'enter the lenght of the PDU message without save in phone
            ATExecute("AT+CMGS=" & PDUSMSEncodedLenght & vbCrLf, 2000)
            'enter the lenght of the PDU message and send the message in phone
            ATExecute(PDUSMSEncoded & Chr(26), 2000) 'CTRL Z = Chr(26) et ESC = Chr(1B)
            'save the message into the phone (option)
            ATExecute("AT+CMGW=" & CStr(CInt(NewSmsIsReceivedMemory) + 1) & vbCrLf, 2000)
        ElseIf PDUTextModesUsable = 1 Or PDUTextModesUsable = 2 Then 'TEXT MODE
            'set command message format to text mode(1),(0=PDU mode)
            ATExecute("AT+CMGF=1" & vbCr, 2000)
            'force les messages à être écrit dand ME (ME,SM aussi possible)
            ATExecute("AT+CPMS=""ME""" & vbCr, 500)
            'set service center address (which varies for service providers (idea, airtel))
            ATExecute("AT+CSCA=" & Chr(34) & TempPluginSettings.SmsServiceCentreAddress & Chr(34) & vbCr, 2000)
            ' enter the mobile number whom you want to send the SMS
            ATExecute("AT+CMGS=" & message & vbCrLf & Chr(26), 2000)
        End If

        RaiseEvent SMSIsSend()
    End Function

    Public Sub CheckReceiveSMS() 'lancement check d'sms
        'Samsung
        'at+cnmi=? --> +CNMI: (0,1,2),(0,1,2),(0,2),(0),(0,1) 
        'Sony
        'AT+CNMI=? --> +CNMI: (2),(0,1,3),(0,2),(0,1),(0)
        '--> (list of supported <mode>s),(list of supported <mt>s),(list of supported <bm>s),(list of supported <ds>s)
        'configuration pour pouvoir checker arrive d'un sms(est valid en permanence)
        ATExecute("AT+CNMI=2,1,0,0,0" & vbCr, 500) 'AT+CNMI=<mode>,<mt>,<bm>,<ds>,<bfr>
        'ATExecute("AT+CNMI=2,1,2,1,0" & vbCr, 500)
        ATExecute("AT+CPMS=""ME""" & vbCr, 500) 'force les messages à être écrit dand ME (ME,SM aussi possible)

        'AT+CSCS Character set.
        'AT+CSMS Select message service.
        'AT+CPMS Preferred storage.
        'AT+CSDH Show text mode parameters. 
    End Sub

#End Region

#Region "PDU SMS mode 'Read'"
    Public Sub ReadPDUMessage(txtPDU As String)
        Try
            Dim s As Object
            Dim PDUCode As String = txtPDU.Replace(vbCrLf, "")
            Dim T As SMSBase.SMSType = SMSBase.GetSMSType(PDUCode)
            Dim txtResult As String = ""
            txtResult = txtResult & T.ToString & vbCrLf
            Select Case T
                Case SMSBase.SMSType.EMS_RECEIVED
                    s = New EMS_RECEIVED(PDUCode)
                    txtResult &= "From:" & s.SrcAddressValue & "  Time:" & s.TP_SCTS & vbCrLf & vbCrLf
                Case SMSBase.SMSType.SMS_RECEIVED
                    s = New SMS_RECEIVED(PDUCode)
                    txtResult &= "From:" & s.SrcAddressValue & "  Time:" & s.TP_SCTS & vbCrLf & vbCrLf
                Case SMSBase.SMSType.EMS_SUBMIT
                    s = New EMS_SUBMIT(PDUCode)
                    txtResult &= "Send to:" & s.DesAddressValue & vbCrLf & vbCrLf
                Case SMSBase.SMSType.SMS_SUBMIT
                    s = New SMS_SUBMIT(PDUCode)
                    txtResult &= "Send to:" & s.DesAddressValue & vbCrLf & vbCrLf
                Case SMSBase.SMSType.SMS_STATUS_REPORT
                    s = New SMS_STATUS_REPORT(PDUCode)
                    txtResult &= "Send time:" & s.TP_SCTS & "  Receive time:" & s.TP_DP & "xxxx" & (s.status).ToString & vbCrLf & vbCrLf
                Case Else
                    txtResult = "Sorry, maybe it is a wrong PDU Code"
            End Select
            '###########################
            'Correct when s is SMS type, no TP_UDL is found.
            'Note:Only EMS has the TP_UDHL and TP_UDH see 3GPP TS 23.040 V6.5.0 (2004-09)
            '###########################
            If s.tp_DCS = 0 Then
                If T = SMSBase.SMSType.SMS_RECEIVED Or T = SMSBase.SMSType.SMS_STATUS_REPORT Or T = SMSBase.SMSType.SMS_SUBMIT Then
                    '#############################
                    'add a parameter
                    '############################
                    txtResult &= s.decode7bit(s.tp_UD, s.TP_UDL) & vbCrLf
                End If
                If T = SMSBase.SMSType.EMS_RECEIVED Or T = SMSBase.SMSType.EMS_SUBMIT Then
                    txtResult &= s.decode7bit(s.tp_ud, s.tp_udl - 8 * (1 + s.tp_udhl) / 7) & vbCrLf
                End If
            Else
                txtResult = txtResult & s.DecodeUnicode(s.TP_UD) & vbCrLf
            End If
            MessageBox.Show(txtResult)
        Catch err As Exception
            MessageBox.Show(err.Message)
        End Try
    End Sub

#End Region

#Region "PDU SMS mode 'Write'"
    Dim SMSObject 'Object To Store SMS or ConcatenatedShortMessage. Late Blinding.
    Dim DataCodingScheme As ENUM_TP_DCS = ENUM_TP_DCS.UCS2
    Dim ValidPeriod As ENUM_TP_VPF = ENUM_TP_VPF.Relative_Format
    Dim PDUCodes() As String
    Public Function WritePDUMessage(txtServiceCenterNum As String, txtDestNum As String, txtUserData As String) As String
        'Check all the information has input.
        If txtServiceCenterNum.Length = 0 Then MsgBox("Please Enter Service Center Number") : Return Nothing
        If txtDestNum.Length = 0 Then MsgBox("Please Enter Destination Number") : Return Nothing
        If txtUserData = "" Then MsgBox("Please Enter UserData") : Return Nothing

        'Get PDU Code
        'PDUCodes = GetPDU(txtServiceCenterNum, txtDestNum, Val(cmbDataCodingScheme.Text), Val(cmbValidPeriod.Text), Val(txtMsgRef.Text), chkStatusReport.Checked, txtUserData)
        PDUCodes = GetPDU(txtServiceCenterNum, txtDestNum, Val(DataCodingScheme), Val(ValidPeriod), Val(0), True, txtUserData)
        'PDUCodes = GetPDU(txtServiceCenterNum, txtDestNum, 0, 0, 0, False, txtUserData) 'OK
        'Add PDU Codes to Text
        Dim i As Integer
        'stsPDULength.Text = ""
        Dim txtPDU As String = ""
        For i = 0 To PDUCodes.Length - 1
            txtPDU += "PDU Number:" & i + 1
            txtPDU += vbTab + "Length For AT:" & (PDUCodes(i).Length - Val("&H" & Mid(PDUCodes(i), 1, 2)) * 2 - 2) / 2 & vbCrLf    'Calculate PDU Length for AT command
            txtPDU += PDUCodes(i) & vbCrLf
            PDUSMSEncodedLenght = (PDUCodes(i).Length - Val("&H" & Mid(PDUCodes(i), 1, 2)) * 2 - 2) / 2
            PDUSMSEncoded = PDUCodes(i)
        Next
        Return txtPDU
    End Function

    Private Function GetPDU(ByVal ServiceCenterNumber As String, ByVal DestNumber As String, ByVal DataCodingScheme As ENUM_TP_DCS, ByVal ValidPeriod As ENUM_TP_VALID_PERIOD, _
                            ByVal MsgReference As Integer, ByVal StatusReport As Boolean, ByVal UserData As String) As String()
        'Check for SMS type
        Dim Type As Integer '0 for SMS;1 For ConcatenatedShortMessage
        Dim Result() As String
        SMSObject = New SMS.Encoder.SMS
        Select Case DataCodingScheme
            Case ENUM_TP_DCS.DefaultAlphabet
                If UserData.Length > 160 Then
                    SMSObject = New SMS.Encoder.ConcatenatedShortMessage
                    Type = 1
                End If
            Case ENUM_TP_DCS.UCS2
                If UserData.Length > 70 Then
                    SMSObject = New SMS.Encoder.ConcatenatedShortMessage
                    Type = 1
                End If
        End Select

        With SMSObject
            .ServiceCenterNumber = ServiceCenterNumber
            If StatusReport = True Then
                .TP_Status_Report_Request = SMS.Encoder.SMS.ENUM_TP_SRI.Request_SMS_Report
            Else
                .TP_Status_Report_Request = SMS.Encoder.SMS.ENUM_TP_SRI.No_SMS_Report
            End If
            .TP_Destination_Address = DestNumber
            .TP_Data_Coding_Scheme = DataCodingScheme
            .TP_Message_Reference = 0 'CInt(txtMsgRef.Text)
            .TP_Validity_Period = ValidPeriod
            .TP_User_Data = UserData
        End With

        If Type = 0 Then
            ReDim Result(0)
            Result(0) = SMSObject.GetSMSPDUCode
        Else
            Result = SMSObject.GetEMSPDUCode            'Note here must use GetEMSPDUCode to get right PDU codes
        End If
        Return Result
    End Function

#End Region

    'Public Sub dial(ByVal number As String)
    '    ATExecute("ATD" & number & ";" & vbCr, 1000)
    'End Sub
    Public Function dial(ByVal phoneNumber As String) As Boolean
        If phoneNumber = "" Then Return False
        Dim tempBytes(0 To 0) As Byte
        tempBytes = System.Text.Encoding.UTF8.GetBytes(phoneNumber & Chr(0))
        Dim retUInt32 As UInt32
        retUInt32 = Btsdk_HFAP_Dial(connHandle, tempBytes(0), CUShort(tempBytes.Length - 1))
        If phoneNumber = TempPluginSettings.EmergencyNumber Then
            RaiseEvent EmergencyAlarm()
        End If
        If retUInt32 = BTSDK_OK Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub requestPhoneInfo()
        ATExecute("ATE1" & vbCrLf, 500) 'Echo ON
        'ATExecute("AT+CGMM" & vbCrlf, 2000) 'phone manufacturer
        'ATExecute("AT+CGMI" & vbCrlf, 2000) 'phone model
        Thread.Sleep(100)
        ATExecute("AT+CGMR" & vbCrLf, 500) 'Revision number
        Thread.Sleep(100)
        ATExecute("AT+CIMI" & vbCrLf, 500) 'ISMI number
        Thread.Sleep(100)
        ATExecute("AT+CGSN" & vbCrLf, 500) 'IMEI number
        Thread.Sleep(100)
        ATExecute("AT+CNUM" & vbCrLf, 500) 'subscriber number
        'Btsdk_HFAP_GetSubscriberNumber(connHandle) 'subscriber number
        Thread.Sleep(100)
        ATExecute("AT+CSCA?" & vbCrLf, 500) 'SMS service center number
        Thread.Sleep(100)
        ATExecute("AT+CCLK?" & vbCrLf, 500) 'Phone Time and Date
        Thread.Sleep(100)
        'ATExecute("AT+CSQ" & vbCrlf, 2000) 'signal power
        'Btsdk_GetRemoteRSSI(connHandle, rssi)
        'Thread.Sleep(500)
        'ATExecute("AT+CGMR;+CIMI;+CGSN;+CNUM" & vbCrlf, 2000)
        phoneModel = BlueSoleil_GetRemoteDeviceName(phoneHandle)
        Thread.Sleep(100)
        'phoneModel = BlueSoleil_HandsFree_GetModel(connHandle)
        phoneManufacturer = BlueSoleil_HandsFree_GetManufacturer(connHandle)
        Thread.Sleep(100)
        requestcurrentNetworkName() 'force le networkname
    End Sub

    Private Sub disconnectPhone()
        If Btsdk_IsDeviceConnected(phoneHandle) = BTSDK_TRUE Then
            Btsdk_Disconnect(connHandle)
            RaiseEvent PhoneIsDisConnected()
        End If
    End Sub

    Private Sub HFPDone()
        Btsdk_UnregisterHFPService(hfService)
        Btsdk_UnregisterHFPService(pbapService)
        'Btsdk_UnregisterHFPService(mapService)
    End Sub

    Public Sub unload()
        If Btsdk_IsSDKInitialized() = BTSDK_TRUE Then
            disconnectPhone()
            HFPDone()
            Btsdk_Done()
        End If
    End Sub

    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        unload()
    End Sub

    Protected Overrides Sub Finalize()
        ' Simply call Dispose(False).
        Dispose(False)
    End Sub

#Region "Phone Info"
    Public Function BlueSoleil_GetRemoteDeviceName(ByVal dvcHandle As UInt32) As String
        Dim retUInt32 As UInt32
        Dim retStr As String = ""
        Dim byteArray(0 To 255) As Byte
        Dim retCount As UInt16 = 1000
        ReDim byteArray(0 To retCount - 1)
        'Btsdk_GetRemoteDeviceName(BTDEVHDL dev_hdl, BTUINT8 *name, BTUINT16 *plen)
        retUInt32 = Btsdk_GetRemoteDeviceName(dvcHandle, byteArray(0), retCount)
        If retCount < 1 Then
            Return ""
            Exit Function
        End If
        ReDim Preserve byteArray(0 To retCount - 1)

        If retUInt32 = BTSDK_OK Then
            retStr = System.Text.Encoding.UTF8.GetString(byteArray)
            retStr = Replace(retStr, Chr(0), "")
            If Left(retStr, 3) = "+CG" Then
                retStr = Mid(retStr, 7)
            End If
            retStr = Trim(retStr)
            'MsgBox("1-BTSDK_OK")
        Else
            'If Btsdk_UpdateRemoteDeviceName(dvcHandle, byteArray(0), retCount) = BTSDK_OK Then
            '    retStr = System.Text.Encoding.UTF8.GetString(byteArray)
            '    retStr = Replace(retStr, Chr(0), "")
            '    If Left(retStr, 3) = "+CG" Then
            '        retStr = Mid(retStr, 7)
            '    End If
            '    retStr = Trim(retStr)
            'MsgBox("2-BTSDK_NotOK")
            'Else
            retStr = ""
            '    If Btsdk_CancelUpdateRemoteDeviceName(dvcHandle) = BTSDK_OK Then
            '        MsgBox("CancelUpdateRemoteDeviceName")
            '    End If

            'End If

        End If
        'MsgBox(retStr)
        Return retStr
    End Function

    Public Function BlueSoleil_HandsFree_GetModel(ByVal hdl As UInt32) As String
        Dim retUInt32 As UInt32
        Dim retCount As UInt16 = 1000
        Dim byteArray(0 To 255) As Byte
        Dim retModel As String = ""
        ReDim byteArray(0 To retCount - 1)
        retUInt32 = Btsdk_HFAP_GetModelID(hdl, byteArray(0), retCount)
        If retCount < 1 Then
            retModel = ""
            Return False
        End If
        ReDim Preserve byteArray(0 To retCount - 1)
        If retUInt32 = BTSDK_OK Then
            retModel = System.Text.Encoding.UTF8.GetString(byteArray)
            retModel = Replace(retModel, Chr(0), "")
            If Left(retModel, 3) = "+CG" Then
                retModel = Mid(retModel, 1)
            End If
            retModel = Trim(retModel)
        Else
            retModel = ""
        End If
        Return retModel
    End Function

    Public Function BlueSoleil_HandsFree_GetManufacturer(ByVal hdl As UInt32) As String
        Dim retUInt32 As UInt32
        Dim retCount As UInt16 = 1000
        Dim byteArray(0 To retCount) As Byte
        Dim retManufacturer As String
        ReDim byteArray(0 To retCount - 1)
        retUInt32 = Btsdk_HFAP_GetManufacturerID(hdl, byteArray(0), retCount)
        If retCount < 1 Then
            retManufacturer = ""
            Return retManufacturer
        End If
        ReDim Preserve byteArray(0 To retCount - 1)
        If retUInt32 = BTSDK_OK Then
            retManufacturer = System.Text.Encoding.UTF8.GetString(byteArray)
            retManufacturer = Replace(retManufacturer, Chr(0), "")
            If Left(retManufacturer, 3) = "+CG" Then
                retManufacturer = Mid(retManufacturer, 7)
            End If
            retManufacturer = Trim(retManufacturer)
            If retManufacturer.Contains("+CGMI") Then retManufacturer = retManufacturer.Replace("+CGMI:", "")
            Return retManufacturer
        Else
            retManufacturer = ""
            Return retManufacturer
        End If

    End Function

    Public Function BlueSoleil_GetRemoteDeviceAddres(ByVal dvcHandle As UInt32) As String
        Dim retStr As String = ""
        Dim HexValue As String = ""
        Dim paddr As IntPtr = Marshal.AllocHGlobal(6)

        If Btsdk_GetRemoteDeviceAddress(dvcHandle, paddr) = BTSDK_OK Then
            For i As Byte = 0 To 5
                HexValue = Hex(Marshal.ReadByte(paddr, i))
                If HexValue.Length < 2 Then HexValue = "0" & HexValue
                retStr = HexValue & retStr
            Next
            'MsgBox(retStr)
        Else
            Marshal.FreeHGlobal(paddr)
        End If

        Return retStr
    End Function


    'Public Sub connSendReceivedUpdate(ByVal hdl As UInt32, ByVal reason As UInt16, ByVal arg As IntPtr)
    '    Dim connProperties As BtSdkConnectionProperty = Marshal.PtrToStructure(arg, GetType(BtSdkConnectionProperty))

    '    If isPhone(connProperties.device_handle) Then
    '        SendBytes = connProperties.sent_bytes
    '        ReceivedBytes = connProperties.received_bytes
    '    End If
    'End Sub



#End Region

#Region "Phone Divers"
    'Sony alarm
    'AT+CALA=? --> +CALA: (1-5),(0-1),(20),(13),(0-1)
    'ex add: AT+CALA="11:15",3,1,,"1,2,3,4,5" ... Monday (1), Sunday (7)
    'ex del:

    'Sony 'Ericsson list Bluetooth devices
    'AT*ELIB

    'Sony
    'AT+CPROT=? --> +CPROT: (0),("1.2"),(8)
#End Region

#Region "Search Devices"
    Public Function BlueSoleil_HandsFree_GetDevicesList() As Boolean
        Dim retBool As Boolean = False
        Dim dvcHandles(0 To 0) As UInt32, dvcAddress(0 To 0) As String, dvcNames(0 To 0) As String, dvcCount As Integer = 0
        If Btsdk_StartDeviceDiscovery(0, 8, 45) = BTSDK_OK Then
            'do this a second time after starting BlueTooth... just for the hell of it.
            BlueSoleil_GetPairedDevices_NamesAndHandlesAndAddress(dvcAddress, dvcNames, dvcHandles, dvcCount)
            Return retBool
        End If

    End Function
    Public Function BlueSoleil_GetPairedDevices_NamesAndHandlesAndAddress(ByRef retDvcAddress() As String, ByRef retDvcNames() As String, ByRef retDvcHandles() As UInt32, ByRef retDvcCount As Integer) As Boolean
        Dim retBool As Boolean = True
        ReDim retDvcAddress(0 To 0)
        ReDim retDvcNames(0 To 0)
        ReDim retDvcHandles(0 To 0)
        Dim retdevClass(0 To 0)
        retDvcCount = 0
        'we changed this to BlueSoleil_GetStoredDevicesByClass so we can call it whether BlueTooth is currently enabled or not.
        retBool = BlueSoleil_GetStoredDevicesByClass(0, retDvcHandles, retDvcCount)
        If retDvcCount = 0 Then
            Return retBool
        End If
        ReDim retDvcNames(0 To retDvcCount - 1)
        ReDim retDvcAddress(0 To retDvcCount - 1)
        ReDim retdevClass(0 To retDvcCount - 1)
        Dim i As Integer, DeviceClass As String

        For i = 0 To retDvcCount - 1
            retDvcNames(i) = BlueSoleil_GetRemoteDeviceName(retDvcHandles(i))
            retDvcAddress(i) = BlueSoleil_GetRemoteDeviceAddres(retDvcHandles(i))
            Btsdk_GetRemoteDeviceClass(retDvcHandles(i), retdevClass(i))

            DeviceClass = ClassVal2String(retdevClass(i))

            'MsgBox("Name: " & retDvcNames(i) & vbCr & "Address: " & retDvcAddress(i) & vbCr & "Type : 0x" & Hex(retdevClass(i)))
            'DevicesListArray.Add(retDvcNames(i) & " " & retDvcAddress(i))
            Select Case BlueSoleil_DeviceIsPaired(retDvcHandles(i))
                Case True
                    IconDeviceType = "$SKINPATH$Indicators\BT_Paired.png"
                Case False
                    IconDeviceType = "$SKINPATH$Indicators\BT_UnPaired.png"
            End Select
            AddCustomList(MainPath & "MobilePhone_DevicesList.lst", ConvertToMacAddress(retDvcAddress(i)), retDvcNames(i) & " --> " & DeviceClass, IconDeviceType)
        Next (i)
        Return retBool
    End Function
    Public Function ConvertToMacAddress(ByVal MacAddress As String) As String
        Dim ch() As Char = MacAddress.ToCharArray()
        Dim Mac As String = ""
        Dim u As Integer = 0
        For n As Integer = 0 To ch.Length - 1
            If u = 2 Or u = 5 Or u = 8 Or u = 11 Or u = 14 Then
                Mac = Mac & ":" & ch(n)
                u = u + 2
            Else
                Mac = Mac & ch(n)
                u = u + 1
            End If
        Next
        Return Mac
    End Function
    Public Function BlueSoleil_GetStoredDevicesByClass(ByVal bsClassCode As UInt32, ByRef dvcHandles() As UInt32, ByRef dvcCount As Integer) As Boolean
        ReDim dvcHandles(0 To 0)
        dvcCount = 0
        'first, get total handle count.
        Dim tempArray(0 To 0) As UInt32
        Dim retCount As Integer = Btsdk_GetStoredDevicesByClass_ByVal(bsClassCode, 0, 0)
        If retCount = 0 Then
            Return True
            Exit Function
        End If
        dvcCount = retCount
        ReDim tempArray(0 To retCount - 1)
        retCount = Btsdk_GetStoredDevicesByClass(bsClassCode, tempArray(0), CUInt(retCount))
        dvcHandles = tempArray
        Return True
    End Function
    'Public Function BlueSoleil_SvcClass_IsHandsFree(ByVal svcClass As UInt16) As Boolean
    '    Dim retBool As Boolean = False
    '    Select Case svcClass
    '        Case BTSDK_CLS_HANDSFREE, BTSDK_CLS_HANDSFREE_AG
    '            retBool = True
    '    End Select
    '    Return retBool
    'End Function
#End Region

#Region "Search Services"
    Public Function GetServicesList() As Boolean
        Dim svc_attr As BtSdkRemoteServiceAttrStru

        Dim psvc_hndls As IntPtr = Marshal.AllocHGlobal(10 * Marshal.SizeOf(GetType(UInt32))) '32 bits * 10 / 8 = 40 bytes
        Dim psvc_num As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(UInt32))) ' 32 Bits = 4 bytes
        Dim psvc_attr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(BtSdkRemoteServiceAttrStru)))

        Marshal.WriteInt32(psvc_num, 10)

        If Not Btsdk_BrowseRemoteServices(phoneHandle, psvc_hndls, psvc_num) = BTSDK_OK Then
            Return False
        End If

        'Services Dictionnary
        CreateServicesList()
        If File.Exists(MainPath & "MobilePhone_Services.lst") Then File.Delete(MainPath & "MobilePhone_Services.lst")

        For i As Byte = 0 To Marshal.ReadInt32(psvc_num) - 1
            If Btsdk_GetRemoteServiceAttributes(Marshal.ReadInt32(psvc_hndls, i * Marshal.SizeOf(GetType(UInt32))), psvc_attr) = BTSDK_OK Then
                svc_attr = Marshal.PtrToStructure(psvc_attr, GetType(BtSdkRemoteServiceAttrStru))
                'AddCustomList(MainPath & "MobilePhone_Services.lst", Hex(svc_attr.service_class), "&H" & Hex(svc_attr.service_class) & " --> " & ServiceList(Hex(svc_attr.service_class)), "$SKINPATH$Indicators\BT_01.png")
                ServicesArray.Add(Hex(svc_attr.service_class) & "," & ServiceList(Hex(svc_attr.service_class)))
            End If
        Next
        'Sort the array
        ServicesArray.Sort()
        'For Each line As String In ServicesArray
        For Each line In ServicesArray
            servicedetail = line.Split(",")
            AddCustomList(MainPath & "MobilePhone_Services.lst", servicedetail(0), "&H" & servicedetail(0) & " --> " & servicedetail(1), "$SKINPATH$Indicators\BT_01.png")
        Next

        ServiceList.Clear()

        Marshal.FreeHGlobal(psvc_hndls)
        Marshal.FreeHGlobal(psvc_num)
        Marshal.FreeHGlobal(psvc_attr)

    End Function
    Public Function BlueSoleil_GetRemoteServiceAttributes(ByVal svcHandle As UInt32, ByRef retSvcName As String, ByRef retSvcClass As UInt16) As Boolean
        Dim struRemoteServiceAttrs(0 To 0) As Byte
        BlueSoleil_Stru_RemoteServiceAttribs_Init(struRemoteServiceAttrs)
        Dim retUInt32 As UInt32
        retUInt32 = Btsdk_GetRemoteServiceAttributes(svcHandle, struRemoteServiceAttrs(0))
        BlueSoleil_Stru_RemoteServiceAttribs_GetInfo(struRemoteServiceAttrs, retSvcClass, retSvcName)
        'MsgBox(retSvcName)
        If retUInt32 = BTSDK_OK Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub BlueSoleil_Stru_RemoteServiceAttribs_Init(ByRef bArray() As Byte)
        Dim struSize As Integer = 4 + 2 + 4 + BTSDK_SERVICENAME_MAXLENGTH + 4 + 2
        '2 + 2 + 4 + BTSDK_SERVICENAME_MAXLENGTH + 4 + 2
        ReDim bArray(0 To struSize - 1)
        Dim tempMask As UInt32 = BTSDK_RSAM_SERVICENAME
        Dim temp4bytes(0 To 3) As Byte
        'copy the MASK value into the structure, as position 0.
        temp4bytes = BitConverter.GetBytes(tempMask)
        Array.Copy(temp4bytes, 0, bArray, 0, 4)
    End Sub
    Private Sub BlueSoleil_Stru_RemoteServiceAttribs_GetInfo(ByRef bArray() As Byte, ByRef retSvcClass As UInt16, ByRef retSvcName As String)
        retSvcName = ""
        Dim tempBytes(0 To BTSDK_SERVICENAME_MAXLENGTH - 1) As Byte
        Array.Copy(bArray, 8, tempBytes, 0, BTSDK_SERVICENAME_MAXLENGTH)
        retSvcName = System.Text.Encoding.UTF8.GetString(tempBytes)
        retSvcName = Replace(retSvcName, Chr(0), "")
        retSvcClass = BitConverter.ToUInt16(bArray, 2)
        'MsgBox(retSvcName)
    End Sub
    'Public Function BlueSoleil_ConnectService_BySvcHandle(ByVal svcHandle As UInt32, ByRef retConnHandle As UInt32) As Boolean
    '    Dim retUInt32 As UInt32
    '    retUInt32 = Btsdk_Connect(svcHandle, 0, retConnHandle)
    '    If retUInt32 = BTSDK_OK Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
    'Public Function BlueSoleil_GetRemoteDeviceServiceHandles(ByVal dvcHandle As UInt32, ByRef svcHandleArray() As UInt32, ByRef svcHandleCount As Integer) As Boolean
    '    'device must be CONNECTED in order to get Services.
    '    ReDim svcHandleArray(0 To 0)
    '    svcHandleCount = 0
    '    Dim tempArray(0 To 0) As UInt32
    '    Dim retCount As UInt32
    '    Dim retUInt32 As UInt32 = Btsdk_GetRemoteServices_ByValArray(dvcHandle, tempArray(0), retCount)
    '    If retCount = 0 Then
    '        Return True
    '        Exit Function
    '    End If
    '    svcHandleCount = CInt(retCount)
    '    ReDim tempArray(0 To svcHandleCount - 1)
    '    retUInt32 = Btsdk_GetRemoteServices(dvcHandle, tempArray(0), retCount)
    '    svcHandleArray = tempArray
    '    If retUInt32 = BTSDK_OK Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
    'Public Function BlueSoleil_GetRemoteDeviceServiceHandles_Refresh(ByVal dvcHandle As UInt32, ByRef svcClassArray() As UInt32, ByRef svcClassCount As Integer) As Boolean
    '    'device must be CONNECTED in order to get Services.
    '    ReDim svcClassArray(0 To 0)
    '    svcClassCount = 0
    '    Dim tempArray(0 To 0) As UInt32
    '    Dim retCount As UInt32
    '    Dim retUInt32 As UInt32 = Btsdk_BrowseRemoteServices_ByValArray(dvcHandle, tempArray(0), retCount)
    '    If retCount = 0 Then
    '        Return True
    '        Exit Function
    '    End If
    '    svcClassCount = CInt(retCount)
    '    ReDim tempArray(0 To svcClassCount - 1)
    '    retUInt32 = Btsdk_BrowseRemoteServices(dvcHandle, tempArray(0), CUInt(retCount))
    '    svcClassArray = tempArray
    '    If retUInt32 = BTSDK_OK Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function

#End Region

#Region "Paired Unpaired"
    Public Function BlueSoleil_DeviceIsPaired(ByVal phonehdl As Integer) As Boolean
        Dim retValue As UShort, retUint32 As UInt32
        retUint32 = Btsdk_IsDevicePaired(phonehdl, retValue)
        If retUint32 = BTSDK_OK Then
            If retValue = BTSDK_TRUE Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Function BlueSoleil_PairedUnpairedDelete(ByVal phonehdlclpos As Integer, ByVal cmd As Integer, ByVal pincode As String) As Boolean
        Dim retBool As Boolean = False
        Dim dvcHandles(0 To 0) As UInt32, dvcCount As Integer = 0
        'do this a second time after starting BlueTooth... just for the hell of it.
        Select Case cmd
            Case 1
                BlueSoleil_PairedByHandle(dvcHandles, dvcCount, phonehdlclpos, pincode)
            Case 2
                BlueSoleil_UnPairedByHandle(dvcHandles, dvcCount, phonehdlclpos)
            Case 3
                BlueSoleil_DeleteByHandle(dvcHandles, dvcCount, phonehdlclpos)
        End Select

        Return retBool
    End Function
    Public Function BlueSoleil_PairedByHandle(ByRef retDvcHandles() As UInt32, ByRef retDvcCount As Integer, ByVal retCLPOS As Integer, ByVal pincode As String) As String
        Dim retBool As String = True, retUint32 As UInt32
        ReDim retDvcHandles(0 To 0)
        retDvcCount = 0

        retBool = BlueSoleil_GetStoredDevicesByClass(0, retDvcHandles, retDvcCount)
        If retDvcCount = 0 Then
            Return retBool
        End If

        For i As Integer = 0 To retDvcCount - 1
            If retCLPOS = i Then
                retUint32 = Btsdk_PinCodeReply(retDvcHandles(i), pincode, pincode.Length)
                If retUint32 = BTSDK_OK Then
                    Btsdk_PairDevice(retDvcHandles(i))
                    'MsgBox("BTSDK_OK")
                End If
            End If
        Next (i)
        Return retCLPOS
    End Function
    Private Sub Paired_Bluetooth_Device()
        'threadPinCode = New System.Threading.Thread(AddressOf Paired_Bluetooth_Device)
        'threadPinCode.Priority = Threading.ThreadPriority.Normal
        'threadPinCode.Start()
        'threadPinCode.Join() ' Attendre la fin du thread 1.
        ' Afficher la valeur de retour.
        'MsgBox("Le thread 1 a retourné la valeur " & Tasks.RetVal)
    End Sub
    Public Function BlueSoleil_UnPairedByHandle(ByRef retDvcHandles() As UInt32, ByRef retDvcCount As Integer, ByVal retValue As Integer) As String
        Dim retBool As String = True, retUint32 As UInt32
        ReDim retDvcHandles(0 To 0)
        retDvcCount = 0

        retBool = BlueSoleil_GetStoredDevicesByClass(0, retDvcHandles, retDvcCount)
        If retDvcCount = 0 Then
            Return retBool
        End If

        For i As Integer = 0 To retDvcCount - 1
            If retValue = i Then
                If retUint32 = BTSDK_OK Then
                    Btsdk_UnPairDevice(retDvcHandles(i))
                End If
            End If
        Next (i)
        Return retValue
    End Function

    Public Function BlueSoleil_DeleteByHandle(ByRef retDvcHandles() As UInt32, ByRef retDvcCount As Integer, ByVal retValue As Integer) As String
        Dim retBool As String = True, retUint32 As UInt32
        ReDim retDvcHandles(0 To 0)
        retDvcCount = 0

        retBool = BlueSoleil_GetStoredDevicesByClass(0, retDvcHandles, retDvcCount)
        If retDvcCount = 0 Then
            Return retBool
        End If

        For i As Integer = 0 To retDvcCount - 1
            If retValue = i Then
                If retUint32 = BTSDK_OK Then
                    Btsdk_DeleteRemoteDeviceByHandle(retDvcHandles(i))
                End If
            End If
        Next (i)
        Return retValue
    End Function


#End Region

#Region "Class List converter"
    Private Function ClassVal2String(ByVal classofdevice As UInteger) As String
        ClassVal2String = ""
        Select Case Right(Hex(classofdevice), 3)
            Case "1FFC"
                ClassVal2String = "Mask of device class"
            Case "100"
                ClassVal2String = "Computer major device class"
            Case "100"
                ClassVal2String = "Uncategorized computer, code for device not assigned"
            Case "104"
                ClassVal2String = "Desktop workstation"
            Case "108"
                ClassVal2String = "Server-class computer"
            Case "10C"
                ClassVal2String = "Laptop computer"
            Case "110"
                ClassVal2String = "Handheld PC/PDA (clam shell)"
            Case "114"
                ClassVal2String = "Palm sized PC/PDA"
            Case "118"
                ClassVal2String = "Wearable computer (Watch sized)"
            Case "200"
                ClassVal2String = "Phone major device class"
            Case "200"
                ClassVal2String = "Uncategorized phone, code for device not assigned"
            Case "204"
                ClassVal2String = "Cellular phone"
            Case "208"
                ClassVal2String = "Cordless phone"
            Case "20C"
                ClassVal2String = "Smart phone"
            Case "210"
                ClassVal2String = "Wired modem or voice gateway"
            Case "214"
                ClassVal2String = "Common ISDN Access"
            Case "218"
                ClassVal2String = "SIM card reader"
            Case "300"
                ClassVal2String = "LAN / Network Access Point major"
            Case "300"
                ClassVal2String = "Fully available"
            Case "320"
                ClassVal2String = "1 - 17% utilized"
            Case "340"
                ClassVal2String = "17- 33% utilized"
            Case "360"
                ClassVal2String = "33 - 50% utilized"
            Case "380"
                ClassVal2String = "50 - 67% utilized"
            Case "3A0"
                ClassVal2String = "67 - 83% utilized"
            Case "3C0"
                ClassVal2String = "83 – 99% utilized"
            Case "3E0"
                ClassVal2String = "No service available"
            Case "400"
                ClassVal2String = "Audio/Video major device class"
            Case "400"
                ClassVal2String = "Uncategorized A/V device, code for device not assigned"
            Case "404"
                ClassVal2String = "Wearable headset device"
            Case "408"
                ClassVal2String = "Hands-free device"
            Case "410"
                ClassVal2String = "Microphone"
            Case "414"
                ClassVal2String = "Loudspeaker"
            Case "418"
                ClassVal2String = "Headphones"
            Case "41C"
                ClassVal2String = "Portable Audio"
            Case "420"
                ClassVal2String = "Car Audio"
            Case "424"
                ClassVal2String = "Set-top box"
            Case "428"
                ClassVal2String = "HiFi Audio device"
            Case "42C"
                ClassVal2String = "Videocassette recorder"
            Case "430"
                ClassVal2String = "Video camera"
            Case "434"
                ClassVal2String = "Camcorder"
            Case "438"
                ClassVal2String = "Video monitor"
            Case "43C"
                ClassVal2String = "Video display and loudspeaker"
            Case "440"
                ClassVal2String = "Video conferencing"
            Case "448"
                ClassVal2String = "Gaming/Toy"
            Case "500"
                ClassVal2String = "Peripheral major device class"
            Case "500"
                ClassVal2String = "Uncategorized peripheral device, code for device not assigned"
            Case "540"
                ClassVal2String = "Keyboard"
            Case "580"
                ClassVal2String = "Pointing device"
            Case "5C0"
                ClassVal2String = "Combo keyboard/pointing device"
            Case "600"
                ClassVal2String = "Imaging major device class"
            Case "610"
                ClassVal2String = "Display"
            Case "620"
                ClassVal2String = "Camera"
            Case "640"
                ClassVal2String = "Scanner"
            Case "680"
                ClassVal2String = "Printer"
            Case "700"
                ClassVal2String = "Wearable major device class"
            Case "704"
                ClassVal2String = "Wristwatch"
            Case "708"
                ClassVal2String = "Pager"
            Case "70C"
                ClassVal2String = "Jacket"
            Case "710"
                ClassVal2String = "Helmet"
            Case "714"
                ClassVal2String = "Glasses"
            Case Else
                ClassVal2String = "Unknow"
        End Select
        Return ClassVal2String
    End Function
#End Region

#Region "PBAP"
    Public Sub Phone_WriteContact(callnumber As String, callname As String, numbertype As String)
        'AT+CPBW=? --> +CPBW: (1-2500),80,(128-255),62 
        '1-2500 is Index i.e sim memory location
        '80 is mobile number 
        '128-255 is type 
        '62 is text name for mobile no 

        '129 – National number type
        '161 – National number type
        '145 – INternational number type
        '177 – Network specific number
        ATExecute("AT+CPBW=," & Chr(34) & callnumber & Chr(34) & "," & numbertype & "," & Chr(34) & callname & Chr(34) & vbCrLf, 500)
    End Sub
    Public Sub PBAPRead()
        Dim myarray() = phoneBookData.List.ToArray
        Dim t As String = ""
        For n As Integer = 0 To myarray.Length - 1
            'For n = 0 To myarray.Length - 1
            t &= n & " " & phoneBookData.List.Item(n).Name & vbTab & vbTab & vbTab & phoneBookData.List.Item(n).Number & vbCr
        Next
        MsgBox("Sub PBAPRead - " & vbCrLf & t)
    End Sub

    Public Sub Phone_LoadPhoneBook(ByVal type As String)
        'LOAD stuff from phone and save it into files on pc
        getPhoneBook(type)
    End Sub
    Public Sub Phone_SortPhoneBook(ByVal type As String)
        'LOAD stuff from phone and save it into files on pc
        PhoneBookSortAll(type)
    End Sub
    'Helper methods

    Public Sub MobilePhoneSyncProcess()
        If Not SynchroningInProgress Then
            SynchroningInProgress = True
            Threading.Thread.Sleep(1000) 'wait some time before engaging this process
            'Phone_LoadPhoneBook("RC")
            'Phone_LoadPhoneBook("DC") '"DC" ME dialled calls list (+CPBW may not be applicable for this storage) $(AT R97)$
            'Phone_LoadPhoneBook("MC")
            Dim PhoneBooKListArray() As String = TempPluginSettings.PhoneBookList.Split(",")
            For Each PhoneBookList As String In PhoneBooKListArray 'Read each phone book usable and sav it
                Phone_LoadPhoneBook(PhoneBookList)
            Next
            SynchroningInProgress = False
        End If
        workerThread.Abort()
    End Sub
    Public Sub RRListSyncProcess()
        If Not SynchroningInProgress Then
            SynchroningInProgress = True
            Threading.Thread.Sleep(1000) 'wait some time before engaging this process
            Dim PhoneBooKListArray() As String = TempPluginSettings.PhoneBookList.Split(",")
            For Each PhoneBookList As String In PhoneBooKListArray 'Read each phone book usable and sav it
                Phone_LoadPhoneBook(PhoneBookList)
                Thread.Sleep(1000)
            Next
            SynchroningInProgress = False
        End If 'ME,SM,RC,DC,MC
        workerThread.Abort()
    End Sub

    Public Sub PhoneBookSort()
        If Not PhoneBookSortInProgress Then
            PhoneBookSortInProgress = True
            Threading.Thread.Sleep(1000) 'wait some time before engaging this process
            Dim PhoneBooKListArray() As String = TempPluginSettings.PhoneBookList.Split(",")
            For Each PhoneBookList As String In PhoneBooKListArray 'Read each phone book usable and sav it
                If File.Exists(MainPath & "PhoneBook\MobilePhone_" & PhoneBookList & ".txt") Then
                    Phone_SortPhoneBook(PhoneBookList)
                    Thread.Sleep(1000)
                Else
                    Continue For
                End If

            Next
            PhoneBookSortInProgress = False
        End If
        workerThread.Abort()
    End Sub

    Public Sub PhoneBookSortAll(ByVal PhoneListType As String)

        Try
            Dim spLine() As String
            Dim allLines As String() = IO.File.ReadAllLines(MainPath & "PhoneBook\MobilePhone_" & PhoneListType & ".txt")
            PhoneBookSortInProgress = True
            PhoneBookArray.Add("0")
            For Each line As String In allLines
                If line <> "0" Then
                    line = line.Replace("||", ",")
                    spLine = line.Split(",")
                    PhoneBookArray.Add(spLine(1) & "," & spLine(0))
                Else
                    Continue For
                End If
            Next

            PhoneBookArray.Sort()

            PhoneBookArray2.Add("0")
            For Each line As String In PhoneBookArray
                If line <> "0" Then
                    spLine = line.Split(",")
                    PhoneBookArray2.Add(spLine(1) & "||" & spLine(0))
                Else
                    Continue For
                End If
            Next

            Dim strw As New StreamWriter(MainPath & "PhoneBook\MobilePhone_" & UCase(PhoneListType) & "2.txt", False, Encoding.Unicode)
            For i As Integer = 0 To PhoneBookArray2.Count - 1
                strw.Write(PhoneBookArray2.Item(i) & vbCrLf)
            Next

            strw.Close()
            PhoneBookArray.Clear()
            PhoneBookArray2.Clear()
            'File.Delete(MainPath & "PhoneBook\MobilePhone_" & PhoneListType & ".txt")
            File.Replace(MainPath & "PhoneBook\MobilePhone_" & UCase(PhoneListType) & "2.txt", MainPath & "PhoneBook\MobilePhone_" & UCase(PhoneListType) & ".txt", Nothing)
        Catch ex As Exception
            MsgBox("Sub PhoneBook Sort - " & ex.Message)
        End Try
        PhoneBookSortInProgress = False
    End Sub

    Public Sub PhoneBookMerge(ByVal PhoneBookList_A As String, ByVal PhoneBookList_B As String)

        PhoneBookMergeInProgress = True

        Dim spLine_A() As String
        Dim allLines_A As String() = IO.File.ReadAllLines(MainPath & "PhoneBook\MobilePhone_" & PhoneBookList_A & ".txt")
        For Each line As String In allLines_A
            If line <> "0" Then
                line = line.Replace("||", ",")
                line = line.Replace("(", "")
                line = line.Replace(")", "")
                spLine_A = line.Split(",")
                PhoneBookArray_A.Add(spLine_A(1) & "," & spLine_A(0))
            Else
                Continue For
            End If
        Next

        'For Each line As String In PhoneBookArray_A
        'If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Array A - " & line & vbCrLf)
        'Next

        If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Merge Function - Reading " & PhoneBookList_A & " Complete" & vbCrLf)

        Dim objStreamWriter As StreamWriter
        If File.Exists(MainPath & "PhoneBook\MobilePhone_" & PhoneBookList_B & ".txt") = False Then
            objStreamWriter = New StreamWriter(MainPath & "PhoneBook\MobilePhone_" & PhoneBookList_B & ".txt", True, Encoding.Unicode)
            objStreamWriter.WriteLine("0")
            objStreamWriter.Close()
        End If

        Dim spLine_B() As String
        Dim list As New List(Of String)
        Using r As StreamReader = New StreamReader(MainPath & "PhoneBook\MobilePhone_" & PhoneBookList_B & ".txt")
            Dim line As String
            Dim line0 As String
            Dim line1 As String
            Dim line2 As String
            Dim line3 As String
            line0 = r.ReadLine()
            Do While r.Peek >= 0
                line1 = r.ReadLine()
                line2 = r.ReadLine()
                line3 = r.ReadLine()
                line = line1 & "||" & line3
                line = line.Replace("||", ",")
                line = line.Replace("(", "")
                line = line.Replace(")", "")
                spLine_B = line.Split(",")
                PhoneBookArray_B.Add(spLine_B(1) & "," & spLine_B(0) & "," & spLine_B(2))
            Loop
        End Using

        'For Each line As String In PhoneBookArray_B
        'If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Array B - " & line & vbCrLf)
        'Next

        For Each TextLine_A As String In PhoneBookArray_A
            TextLine_Match = 0
            For Each TextLine_B As String In PhoneBookArray_B
                If TextLine_A = Left(TextLine_B, Len(TextLine_A)) Then
                    If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Match - " & TextLine_A & vbCrLf)
                    TextLine_Match = 1
                End If
            Next
            If TextLine_Match = 0 Then
                If TextLine_A <> ",LST" Then
                    PhoneBookArray_B.Add(TextLine_A & ",ICO")
                    If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Adding " & TextLine_A & vbCrLf)
                End If
            End If
        Next

        PhoneBookArray_B.Add("0")

        PhoneBookArray_B.Sort()

        Dim spLine_B2() As String
        For Each line As String In PhoneBookArray_B
            'If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Array B2 - " & line & vbCrLf)
            If line = "0" Then
                PhoneBookArray_B2.Add("0")
            Else
                spLine_B2 = line.Split(",")
                PhoneBookArray_B2.Add(spLine_B2(1) & "||" & spLine_B2(0) & vbCr)
                PhoneBookArray_B2.Add(spLine_B2(2))
            End If
        Next

        Dim strw As New StreamWriter(MainPath & "PhoneBook\MobilePhone_" & UCase(PhoneBookList_B) & "2.txt", False, Encoding.Unicode)
        For i As Integer = 0 To PhoneBookArray_B2.Count - 1
            strw.Write(PhoneBookArray_B2.Item(i) & vbCrLf)
        Next

        strw.Close()
        PhoneBookArray_A.Clear()
        PhoneBookArray_B.Clear()
        PhoneBookArray_B2.Clear()
        'File.Delete(MainPath & "PhoneBook\MobilePhone_" & PhoneListType & ".txt")
        File.Replace(MainPath & "PhoneBook\MobilePhone_" & UCase(PhoneBookList_B) & "2.txt", MainPath & "PhoneBook\MobilePhone_" & UCase(PhoneBookList_B) & ".txt", Nothing)

        PhoneBookMergeInProgress = False
    End Sub
#End Region


#Region "MAP tests"
    '    Private Sub MAP_noti_func(ByVal hdl As UInt32, ByVal ev_ob As BtSdkMAPEvReportObjStru)
    '        Select Case ev_ob.ev_type
    '            Case BTSDK_MAP_EVT_NEWMSG
    '                MsgBox("NewMessage")
    '            Case BTSDK_MAP_EVT_DELIVERY_OK
    '                MsgBox("DeliverySuccess")
    '            Case BTSDK_MAP_EVT_SEND_OK
    '                MsgBox("SendingSuccess")
    '            Case BTSDK_MAP_EVT_DELIVERY_FAIL
    '                MsgBox("DeliveryFailure")
    '            Case BTSDK_MAP_EVT_SEND_FAIL
    '                MsgBox("SendingFailure")
    '            Case BTSDK_MAP_EVT_MEM_FULL
    '                MsgBox("MemoryFull")
    '            Case BTSDK_MAP_EVT_MEM_READY
    '                MsgBox("MemoryAvailable")
    '            Case BTSDK_MAP_EVT_MSG_DELETED
    '                MsgBox("MessageDeleted")
    '            Case BTSDK_MAP_EVT_MSG_SHIFT
    '                MsgBox("MessageShift")
    '        End Select
    '        MsgBox("A new message is coming:")
    '    End Sub

    '    Public Function MAP_CheckFilterMsgType(ByVal msg_type As String, ByVal filter_type As String) As Boolean
    '        Dim ret As Boolean = False
    '        If filter_type & BTSDK_MAP_FILTEROUT_SMSGSM And msg_type <> "SMS_GSM" Then
    '            ret = False
    '        ElseIf filter_type & BTSDK_MAP_FILTEROUT_SMSCDMA And msg_type <> "SMS_CDMA" Then
    '            ret = False
    '        ElseIf filter_type & BTSDK_MAP_FILTEROUT_EMAIL And msg_type <> "EMAIL" Then
    '            ret = False
    '        ElseIf filter_type & BTSDK_MAP_FILTEROUT_MMS And msg_type <> "MMS" Then
    '            ret = False
    '        Else
    '            ret = True
    '        End If
    '        Return ret
    '    End Function



    '    Public Sub RegisterMNSService()

    '        Dim svc_hdl As UInt32 = BTSDK_INVALID_HANDLE
    '        Dim st_func As Btsdk_MNS_MessageNotification_Func
    '        Dim st_mapsvr_cb As New BtSdkMAPFileIORoutinesStru

    '        'st_func = AddressOf MAP_noti_func
    '        With st_mapsvr_cb
    '            .open_file = AddressOf MAP_OpenFile
    '            .create_file = AddressOf MAP_CreateFile
    '            '.write_file = AddressOf MAP_WriteFile
    '            '.read_file = AddressOf MAP_ReadFile
    '            '.get_file_size = AddressOf MAP_GetFilesize
    '            '.rewind_file = AddressOf MAP_Rewind
    '            '.close_file = AddressOf MAP_CloseFile
    '        End With

    '        'svc_hdl = Btsdk_RegisterMNSService("message notification server", st_func, st_mapsvr_cb)
    '        If svc_hdl <> BTSDK_INVALID_HANDLE Then
    '            MessageBox.Show("Register MNS service successfully!")
    '        Else
    '            MessageBox.Show("Register MNS service Failed!")
    '        End If

    '    End Sub

    '    Public Sub MSERegisterMASService()
    '        Dim svc_hdl = BTSDK_INVALID_HANDLE
    '        Dim cbks As BtSdkMASSvrCBStru
    '        Dim svr_info As BtSdkLocalMASServerAttrStru

    '        'memset(svr_info, 0, sizeof(BtSdkLocalMASServerAttrStru))
    '        'svr_info.size = sizeof(BtSdkLocalMASServerAttrStru)
    '        'MAP_GetRootDir(svr_info.root_dir)
    '        svr_info.path_delimiter = "\"
    '        svr_info.sup_msg_types = BTSDK_MAP_SUP_MSG_SMSGSM

    '        'memset(&cbks, 0, sizeof(BtSdkMASSvrCBStru))

    '        With cbks
    '            .file_io_rtns.create_file = AddressOf MAP_CreateFile
    '            '.file_io_rtns.read_file = AddressOf MAP_ReadFile
    '            '.file_io_rtns.write_file = AddressOf MAP_WriteFile
    '            '.file_io_rtns.rewind_file = AddressOf MAP_Rewind
    '            '.file_io_rtns.close_file = AddressOf MAP_CloseFile
    '            .file_io_rtns.open_file = AddressOf MAP_OpenFile
    '            .file_io_rtns.get_file_size = AddressOf MAP_GetFilesize

    '            '.find_folder_rtns.find_first_folder = AddressOf MAP_FindFirstFolder
    '            '.find_folder_rtns.find_next_folder = AddressOf MAP_FindNextFolder
    '            '.find_folder_rtns.find_folder_close = AddressOf MAP_FindFolderClose

    '            '.find_msg_rtns.find_first_msg = AddressOf MAP_FindFirstMessage
    '            '.find_msg_rtns.find_next_msg = AddressOf MAP_FindNextMessage
    '            '.find_msg_rtns.find_msg_close = AddressOf MAP_FindMessageClose

    '            '.msg_io_rtns.create_bmsg_file = AddressOf MAP_CreateBMsgFile
    '            '.msg_io_rtns.open_bmsg_file = AddressOf MAP_OpenBMsgFile
    '            '.msg_io_rtns.modify_msg_status = AddressOf MAP_ModifyMsgStatus
    '            '.msg_io_rtns.push_msg = AddressOf MAP_PushMessage

    '            '.mse_status_rtns.register_notification = AddressOf MAP_RegisterNotification
    '            '.mse_status_rtns.update_inbox = AddressOf MAP_UnpdateInbox
    '            '.mse_status_rtns.get_mse_time = AddressOf MAP_GetMSETime
    '        End With

    '        svc_hdl = Btsdk_RegisterMASService("Sample MAP SMS Server", svr_info, cbks)
    '        If svc_hdl <> BTSDK_INVALID_HANDLE Then
    '            MsgBox("Register MAS service successfully")
    '        Else
    '            MsgBox("Register MAS service Failed")
    '        End If

    '    End Sub

    '    Public Sub TestSelectRmtMAPDev()
    '        s_currRmtMAPDevHdl = phoneHandle 'SelectRemoteDevice(0)
    '        If s_currRmtMAPDevHdl = BTSDK_INVALID_HANDLE Then
    '            MsgBox("Please make sure that the expected device is in discoverable state and search again.")
    '        End If
    '    End Sub
    '    Public Sub TestSelectMAPSvc()
    '        s_currMAPSvcHdl = s_currRmtMAPDevHdl 'SelectRemoteService(s_currRmtMAPDevHdl)
    '        If s_currMAPSvcHdl = BTSDK_INVALID_HANDLE Then
    '            MsgBox("Cann't get expected service handle")
    '        End If
    '    End Sub
    '    Public Sub TestConnectMAPSvc()
    '        Dim ulRet As UInt32 = BTSDK_FALSE
    '        ulRet = Btsdk_Connect(s_currMAPSvcHdl, 0, s_currMAPConnHdl)
    '        If ulRet <> BTSDK_OK Then
    '            MsgBox("Please make sure that the expected device is powered on and connectable.\n")
    '        End If
    '    End Sub

    '    'Public Sub testMAPService()
    '    '    TestSelectRmtMAPDev()
    '    '    TestSelectMAPSvc()
    '    '    TestConnectMAPSvc()
    '    'End Sub

    '    Public Sub TestSetMessageStatus(ch As String)

    '        Dim hFile As UInt32
    '        Dim result As UInt32 = BTSDK_OK
    '        Dim hdlEnumMsg As UInt32
    '        Dim hconn As UInt32 = s_currMAPConnHdl
    '        Dim fileio_rtns As BtSdkMAPFileIORoutinesStru
    '        Dim msgparam As BtSdkMAPGetMsgParamStru

    '        'printf("the function will set messages status\n");
    '        'If s_currstrFile = 0 Then
    '        'printf("No message list file available, please get message list first. \n")
    '        'End If

    '        fileio_rtns.open_file = AddressOf MAP_OpenFile
    '        fileio_rtns.create_file = AddressOf MAP_CreateFile
    '        'fileio_rtns.write_file = AddressOf MAP_WriteFile
    '        'fileio_rtns.read_file = AddressOf MAP_ReadFile
    '        fileio_rtns.get_file_size = AddressOf MAP_GetFilesize
    '        'fileio_rtns.rewind_file = AddressOf MAP_Rewind
    '        'fileio_rtns.close_file = AddressOf MAP_CloseFile
    '        'Btsdk_MAPRegisterFileIORoutines(hconn, fileio_rtns)
    '        'Btsdk_MAPRegisterStatusCallback(hconn, MAPAppStatusCB)

    '        hFile = MAP_OpenFile(MainPath & "s_currstrFile.txt") 'MAP_OpenFile(s_currstrFile)
    '        If hFile = BTSDK_INVALID_HANDLE Then
    '            MessageBox.Show("Open message list file fail, please check the file.")
    '        End If
    '        hdlEnumMsg = Btsdk_MAPStartEnumMessageList(fileio_rtns.read_file, fileio_rtns.rewind_file, hFile)
    '        If hdlEnumMsg <> BTSDK_INVALID_HANDLE Then

    '            'Dim struMsg As BtSdkMAPMsgObjStru
    '            'ShowStatusMenu()
    '            'printf("please select a status you want to set\n")
    '            'scanf(" %c", &ch)
    '            'GetChar()

    '            While True 'Btsdk_MAPEnumMessageList(hdlEnumMsg, struMsg)

    '                msgparam.charset = BTSDK_MAP_CHARSET_NATIVE 'return integrated SMS PDU in intrinsic encoder mode. The MSE does not do any change to the type.
    '                'BTSDK_MAP_CHARSET_UTF8 – the MSE only transfer the textual content of message, and change the type to UTF-8 before sending.
    '                msgparam.attachment = BTSDK_TRUE 'A flag which specifies attachment should be contained or not in a returned message. It could be one of the values below:
    '                'BTSDK_TRUE – need to contain attachment(if there is any) BTSDK_FALSE – do not need to contain attachment
    '                'memcpy(msgparam.msg_handle, struMsg.msg_handle, BTSDK_MAP_MSGHDL_LEN)

    '                msgparam.fraction_deliver = BTSDK_MAP_FRACT_NONE 'get integrated email one time. It means MSE will be responsibility packaging all separated part.
    '                'BTSDK_MAP_FRACT_REQFIRST – get the first separated part of message 
    '                'BTSDK_MAP_FRACT_REQNEXT – get the next separated part of message Fraction_deliver is used to decide whether it is the last separated part of message or not

    '                msgparam.fraction_req = BTSDK_MAP_FRACT_RSPMORE 'there is still more separated part of message should be received
    '                'BTSDK_MAP_FRACT_RSPLAST – this is the last separated part of message. And it is also suitable for the situation that getting integrated email one time.


    '                Select Case ch
    '                    Case "1"
    '                        result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle(0), BTSDK_MAP_MSG_SETST_READ)
    '                    Case "2"
    '                        result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle(0), BTSDK_MAP_MSG_SETST_UNREAD)
    '                    Case "3"
    '                        result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle(0), BTSDK_MAP_MSG_SETST_DELETED)
    '                    Case "4"
    '                        result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle(0), BTSDK_MAP_MSG_SETST_UNDELETED)
    '                End Select

    '                If result <> BTSDK_OK Then
    '                    'printf("0X%04X\n", result)
    '                    'break()
    '                    Select Case ch
    '                        Case "1"
    '                            MessageBox.Show("a message has been set to READ status")
    '                        Case "2"
    '                            MessageBox.Show("a message has been set to UNREAD status")
    '                        Case "3"
    '                            MessageBox.Show("a message has been set to DELETED status")
    '                        Case "4"
    '                            MessageBox.Show("a message has been set to UNDELETED status")
    '                    End Select
    '                End If
    '            End While
    '        End If
    '        Btsdk_MAPEndEnumMessageList(hdlEnumMsg)
    '        'MAP_CloseFile(hFile)
    '    End Sub

    '    '/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    '    'This function is to change the working directory.
    '    '---------------------------------------------------------------------------*/
    '    Private Function MAP_ChangeDir(cur_path As String) As Boolean
    '        If cur_path = "" Then
    '            Return BTSDK_TRUE
    '        Else
    '            Return BTSDK_FALSE
    '        End If

    '    End Function
    '    Private Function MAP_OpenFile(file_name As String) As UInt32
    '        '{
    '        '	FILE *filehdl = fopen(file_name, "rb");
    '        '	if (filehdl != NULL)
    '        '	{
    '        '		return (BTSDKHANDLE)filehdl;
    '        '	}
    '        '	return (BTSDKHANDLE)NULL;
    '        '}
    '    End Function
    '    Private Function MAP_CreateFile(file_name As String) As UInt32
    '        Dim ret As UInt32
    '        If file_name <> "" Then
    '            'Return fopen(file_name, "wb+")
    '        Else
    '            'Return tmpfile()
    '        End If
    '    End Function
    '    Private Function MAP_WriteFile(file_hdl As UInt32, buf() As String, bytes_to_write As UInt32) As UInt32
    '        If (file_hdl <> BTSDK_INVALID_HANDLE) Then
    '            'return (fwrite((const void*)buf, 1, bytes_to_write, (FILE*)file_hdl));
    '        Else
    '            Return 0
    '        End If
    '    End Function
    '    Private Function MAP_ReadFile(file_hdl As UInt32, buf() As String, len As UInt32, is_end As Boolean) As UInt32
    '        If (file_hdl <> BTSDK_INVALID_HANDLE) Then

    '            Dim read_len As UInt32 '= fread(buf, sizeof(char), len, (FILE*)file_hdl);
    '            If (read_len <> len) Then
    '                is_end = True
    '            Else
    '                is_end = False
    '            End If
    '            Return read_len
    '        Else
    '            Return 0
    '        End If

    '    End Function
    '    Private Function MAP_GetFilesize(file_hdl As UInt32) As UInt32
    '        If (file_hdl <> BTSDK_INVALID_HANDLE) Then
    '            Dim ori_pos As UInt32
    '            Dim len As UInt32
    '            'ori_pos = ftell((FILE*)file_hdl);
    '            If ori_pos = &HFFFFFFFF Then
    '                'fseek((FILE*)file_hdl, 0, SEEK_END)
    '                'len = ftell((FILE*)file_hdl)
    '                'fseek((FILE*)file_hdl, ori_pos, SEEK_SET)
    '            End If
    '            If len = &HFFFFFFFF Then len = 0
    '            Return len
    '        Else

    '            Return 0
    '        End If
    '    End Function


    '    Private Sub TestGetMessage()
    '        Dim hFile As UInt32
    '        Dim hdlEnumMsg As UInt32
    '        Dim strPath As String = ""
    '        Dim result As UInt32 = BTSDK_OK
    '        Dim msgNum As Integer = 1
    '        Dim msgPath As String = ""
    '        Dim hconn As UInt32 = s_currMAPConnHdl
    '        Dim hdlMessage As UInt32 = BTSDK_INVALID_HANDLE
    '        Dim fileio_rtns As BtSdkMAPFileIORoutinesStru
    '        Dim msgparam As BtSdkMAPGetMsgParamStru

    '        'If (s_currstrFile = "") Then
    '        '    MessageBox.Show("No message list file available, please get message list first.")
    '        '    Return
    '        'End If

    '        With fileio_rtns
    '            fileio_rtns.open_file = AddressOf MAP_OpenFile
    '            fileio_rtns.create_file = AddressOf MAP_CreateFile
    '            'fileio_rtns.write_file = AddressOf MAP_WriteFile
    '            'fileio_rtns.read_file = AddressOf MAP_ReadFile
    '            fileio_rtns.get_file_size = AddressOf MAP_GetFilesize
    '            'fileio_rtns.rewind_file = AddressOf MAP_Rewind
    '            'fileio_rtns.close_file = AddressOf MAP_CloseFile
    '            'Btsdk_MAPRegisterFileIORoutines(hconn, &fileio_rtns);
    '            'Btsdk_MAPRegisterStatusCallback(hconn, MAPAppStatusCB);
    '        End With

    '        'hFile = MAP_OpenFile(s_currstrFile)
    '        'If (hFile = BTSDK_INVALID_HANDLE) Then
    '        '    MessageBox.Show("Open message list file fail, please check the file.")
    '        '    Return
    '        'End If

    '        hdlEnumMsg = Btsdk_MAPStartEnumMessageList(fileio_rtns.read_file, fileio_rtns.rewind_file, hFile)
    '        If (hdlEnumMsg <> BTSDK_INVALID_HANDLE) Then

    '            Dim struMsg As BtSdkMAPMsgObjStru

    '            MessageBox.Show("please input a folder name to store message (e.g. c:\\gl.vcf)")
    '            'strPath = (char*)malloc(sizeof(char)*BTSDK_PATH_MAXLENGTH)
    '            'memset(strPath, 0, BTSDK_PATH_MAXLENGTH)
    '            'AppWaitForInput(strPath, BTSDK_PATH_MAXLENGTH)
    '            '
    '            'CreateDirectory(strPath, NULL)

    '            'msgPath = (char*)malloc(sizeof(char)*BTSDK_PATH_MAXLENGTH);
    '            'memset(msgPath, 0, BTSDK_PATH_MAXLENGTH);


    '            'While Btsdk_MAPEnumMessageList(hdlEnumMsg, struMsg)

    '            msgparam.charset = BTSDK_MAP_CHARSET_NATIVE 'return integrated SMS PDU in intrinsic encoder mode. The MSE does not do any change to the type.
    '            'BTSDK_MAP_CHARSET_UTF8 – the MSE only transfer the textual content of message, and change the type to UTF-8 before sending.
    '            msgparam.attachment = BTSDK_TRUE
    '            'memcpy(msgparam.msg_handle, struMsg.msg_handle, BTSDK_MAP_MSGHDL_LEN)

    '            'sprintf(msgPath, "%s\\%d", strPath, msgNum)
    '            hdlMessage = MAP_CreateFile(msgPath)
    '            result = Btsdk_MAPGetMessage(hconn, msgparam, hdlMessage)
    '            If (result <> BTSDK_OK) Then
    '                'MAP_CloseFile(hdlMessage)
    '                Return

    '            End If

    '            result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle(0), BTSDK_MAP_MSG_SETST_READ)
    '            If (result = BTSDK_OK) Then
    '                MessageBox.Show("a message has been set to READ\n")
    '            End If
    '            'MAP_CloseFile(hdlMessage)
    '            'msgNum++
    '            'End While


    '            Btsdk_MAPEndEnumMessageList(hdlEnumMsg)
    '            NumberOfSMSInMemoryOld = msgNum
    '            'MAP_CloseFile(hdlMessage);
    '            hdlMessage = BTSDK_INVALID_HANDLE
    '            'End If
    '            'MAP_CloseFile(hFile)
    '            hFile = BTSDK_INVALID_HANDLE

    '            If (result = BTSDK_OK) Then
    '                MessageBox.Show("messages have been stored under " & strPath)
    '            Else
    '                MessageBox.Show("sth wrong happened, error code is " & result)

    '            End If
    '            'free(strPath);
    '        End If
    '    End Sub


    '    Public Sub MapOn()
    '        Dim ret As UInt32
    '        'ret = Btsdk_MAPSetNotificationRegistration(s_currMAPConnHdl, BTSDK_TRUE)
    '        ret = Btsdk_MAPSetNotificationRegistration(connHandle, BTSDK_TRUE)
    '        If ret = BTSDK_OK Then
    '            MsgBox("MAPSetNotification is OK")
    '        Else
    '            MsgBox("MAPSetNotification is NotOK")
    '        End If
    '    End Sub
    '    Public Sub MapOff()
    '        Dim ret As UInt32
    '        'ret = Btsdk_MAPSetNotificationRegistration(s_currMAPConnHdl, BTSDK_FALSE)
    '        ret = Btsdk_MAPSetNotificationRegistration(connHandle, BTSDK_FALSE)
    '        If ret = BTSDK_OK Then
    '            MsgBox("MAPisOK")
    '        Else
    '            MsgBox("MAPisNotOK")
    '        End If


    '    End Sub
#End Region


#Region "AVR"
    Private Function SendPassThroughCommand(op_id As Byte) As UInt32

        'Dim bRet As Boolean = False
        'Dim lRet1 As UInt32 = BTSDK_FALSE
        'Dim lRet2 As UInt32 = BTSDK_FALSE

        'Dim CommandStru As BtSdkPassThrReqStru

        'CommandStru.dev_hdl = s_currAudioRmtDevHdl
        'CommandStru.state_flag = BTSDK_AVRCP_BUTTON_STATE_PRESSED
        'CommandStru.op_id = op_id
        'CommandStru.length = 0
        'lRet1 = Btsdk_AVRCP_PassThroughReq(CommandStru)

        'CommandStru.state_flag = BTSDK_AVRCP_BUTTON_STATE_RELEASED
        'lRet2 = Btsdk_AVRCP_PassThroughReq(CommandStru)

        'If (BTSDK_OK = lRet1) & (BTSDK_OK = lRet2) Then
        '    bRet = True
        'End If
        'Return bRet


    End Function

    Private Sub AVRCPPlayItemReq()

        Dim pram As New BtSdkPlayItemReqStru

        Dim len As Integer = 0, count = 4, ret = 0

        len = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(BtSdkPlayItemReqStru)))
        Marshal.StructureToPtr(pram, len, True)
        'len = sizeof(BtSdkPlayItemReqStru)
        'pram=(PBtSdkPlayItemReqStru)malloc(len)

        'memset(pram,0,len);

        pram.size = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(BtSdkPlayItemReqStru)))
        Marshal.StructureToPtr(pram, pram.size, True)
        'pram.scope = BTSDK_AVRCP_SCOPE_MEDIAPLAYER_VIRTUAL_FILESYSTEM + BTSDK_AVRCP_SCOPE_MEDIAPLAYER_LIST + BTSDK_AVRCP_SCOPE_MEDIAPLAYER_SEARCH + BTSDK_AVRCP_SCOPE_MEDIAPLAYER_NOWPLAYING
        pram.uid(0) = "&H" 'sprintf(pram.uid, "0x0")
        pram.uid_counter = 1
        'ret = Btsdk_AVRCP_PlayItemReq(s_currAudioRmtDevHdl, pram)
        If ret = BTSDK_OK Then
            MessageBox.Show("Get folder Btsdk_AVRCP_PlayItemReq success")
        Else
            MessageBox.Show("Get folder Btsdk_AVRCP_PlayItemReq fail error code " & ret)
        End If




    End Sub


    Private Function AVRInit() As Boolean
        'If Btsdk_AVRCP_RegPassThrCmdCbk4ThirdParty(Btsdk_AVRCP_PassThr_Cmd_Func) = BTSDK_OK Then

        '    Return True
        'End If
        Return False
    End Function

    '/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    'Description:
    'This function is to deal with the event generated by a user's operation.
    'Arguments:
    'op_id:      [in] operation code ID
    'state_flag: [in] states flag
    '        Return
    '        Void()
    '---------------------------------------------------------------------------*/
    Private Sub AVRCP_PassThr_Cmd_CbkFunc(op_id As Byte, state_flag As Byte)
        '//******************************************************
        '/* state_flag indicates the button status (up or down). */
        MessageBox.Show("Inside AVRCP_PassThr_Cmd_CbkFunc function.")

        'If BTSDK_AVRCP_BUTTON_STATE_PRESSED <> state_flag Then
        '    MessageBox.Show("AVRCP:DATA:BTSDK_AVRCP_BUTTON_STATE_PRESSED event")
        '    Return
        'End If
        '/* op_id could be one of the following status. please add code for each case. */
        Select Case op_id
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_PLAY
            '    MessageBox.Show("The user has pressed down 'Play' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_STOP
            '    MessageBox.Show("The user has pressed down 'Stop' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_PAUSE
            '    MessageBox.Show("The user has pressed down 'Pause' button on the AV device's panel. ")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_FAST_FORWARD
            '    MessageBox.Show("The user has pressed down 'Fast forward' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_FORWARD
            '    MessageBox.Show("The user has pressed down 'Forward' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_BACKWARD
            '    MessageBox.Show("The user has pressed down 'Backward' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_UP
            '    MessageBox.Show("The user has pressed down 'Volume up' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_DOWN
            '    MessageBox.Show("The user has pressed down 'Volume down' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_RECORD
            '    MessageBox.Show("The user has pressed down 'Record' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_POWER
            '    MessageBox.Show("The user has pressed down 'POWER' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_MUTE
            '    MessageBox.Show("The user has pressed down 'MUTE' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_REWIND
            '    MessageBox.Show("The user has pressed down 'REWIND' button on the AV device's panel.")
            'Case BTSDK_AVRCP_OPID_AVC_PANEL_EJECT
            '    MessageBox.Show("The user has pressed down 'EJECT' button on the AV device's panel.")
            Case Else
                MessageBox.Show("AVRCP_PassThr_Cmd_CbkFunc  Default " & op_id)
        End Select

    End Sub
#End Region

#Region "Add Pierrot"
    Public Sub GetIndicatorsEvent()
        'Enables and disables the uplink voice muting during a voice call.
        ATExecute("AT+CIND?" & vbCrLf, 500)
    End Sub
    Public Sub MutePhone(mute As String)
        'Orders the phone to be in silent mode or orders the phone to leave the silent mode.
        'When the phone is in silent mode, all sounds from the phone
        'must be prevented. An icon will show the user that silent mode is active. If
        'no parameter is given to the SET command it will use <mode> = 0 as
        'parameter.
        ATExecute("AT+CSIL=" & mute & vbCrLf, 500) '0=mute off, 1= mute on
    End Sub
    Public Sub ExternalSpeaker(speaker As String)
        'Enables and disables the uplink voice muting during a voice call.
        ATExecute("AT+CMUT=" & speaker & vbCrLf, 500) '0=speaker off, 1= speaker on
    End Sub
    Public Function phonemicrovolMax(ByVal micVolPct As Double) As Boolean 'Turn microphone volume of cell phone to the Max.
        Dim tempVal As Integer = CInt(micVolPct * 15 / 100)
        Dim micVolByte As Byte = CByte(tempVal)
        If micVolByte > 15 Then micVolByte = 15
        If micVolByte < 0 Then micVolByte = 0

        Dim retUInt32 As UInt32
        retUInt32 = Btsdk_HFAP_SetMicVol(connHandle, micVolByte)

        If retUInt32 = BTSDK_OK Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function phonespeakervolMax(ByVal spkVolPct As Double) As Boolean 'Turn speaker volume of cell phone to the Max.
        Dim tempVal As Integer = CInt(spkVolPct * 15 / 100)
        Dim micVolByte As Byte = CByte(tempVal)
        If micVolByte > 15 Then micVolByte = 15
        If micVolByte < 0 Then micVolByte = 0

        Dim retUInt32 As UInt32
        retUInt32 = Btsdk_HFAP_SetSpkVol(connHandle, micVolByte)

        If retUInt32 = BTSDK_OK Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub requestcurrentNetworkName() 'Request for Current Network Operator Name  
        'The network operator name will be returned by BTSDK_HFP_EV_NETWORK_OPERATOR_IND event.
        Btsdk_HFAP_NetworkOperatorReq(connHandle)
    End Sub
    Public Function AudioConnTrans() As Boolean 'SCO Audio Switching   
        If Btsdk_IsDeviceConnected(phoneHandle) = BTSDK_TRUE Then
            'this sounds like it transfers the audio between PC and phone.
            Dim retUInt32 As UInt32
            retUInt32 = Btsdk_HFAP_AudioConnTrans(connHandle)

            If retUInt32 = BTSDK_OK Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Function BlueSoleil_HandsFree_Redial() As Boolean
        Dim retUInt32 As UInt32
        retUInt32 = Btsdk_HFAP_LastNumRedial(connHandle)
        If retUInt32 = BTSDK_OK Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function BlueSoleil_Btsdk_StopBluetooth() As Boolean
        Dim retUInt32 As UInt32
        retUInt32 = Btsdk_StopBluetooth()
        If retUInt32 = BTSDK_OK Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function BlueSoleil_Btsdk_StartBluetooth() As Boolean
        Dim retUInt32 As UInt32
        retUInt32 = Btsdk_StartBluetooth()
        If retUInt32 = BTSDK_OK Then
            Return True
        Else
            Return False
        End If
    End Function
    'Btsdk_GetClientPort
    Public Function BlueSoleil_Btsdk_GetClientPort() As String
        Dim retUInt32 As String
        'retUInt32 = Btsdk_GetClientPort(connHandle)
        retUInt32 = "COM" & Btsdk_GetASerialNum().ToString & " is available !"
        'retUInt32 = "COM" & Btsdk_GetClientPort(phoneHandle).ToString & " is used !"
        'If retUInt32 <> 0 Then
        Return retUInt32
        'End If
    End Function

#End Region

#Region "Contacts RR List"
    Public Sub AddCustomList(ByVal CustomList As String, ByVal Number As String, ByVal Name As String, ByVal IconType As String)
        Dim objStreamWriter As StreamWriter
        'MsgBox("Writing " & Name & "||" & Number)
        objStreamWriter = New StreamWriter(CustomList, True, Encoding.Unicode)
        If File.Exists(CustomList) = False Then
            objStreamWriter.WriteLine("0")
        End If
        objStreamWriter.WriteLine("LST" & Number & "||" & Name & "||ICOZZ" & IconType)
        objStreamWriter.Close()

        'Create the custom list if don't exist
        '       If File.Exists(CustomList) = False Then 'création d'un fichier custom list au format UNICODE avec le 0 de départ !
        '        'Open the file.
        '        objStreamWriter = New StreamWriter(CustomList, True, Encoding.Unicode)
        '        objStreamWriter.WriteLine("0")
        '        'If Number = "" Or Name = "" Then
        '        '    objStreamWriter.WriteLine("LST" & Number & Name)
        '        'Else
        '        objStreamWriter.WriteLine("LST" & Number & "||" & Name)
        '        'End If
        '        If IconType <> "" Then objStreamWriter.WriteLine("ICO" & IconType)
        '        objStreamWriter.Close()
        '        Else
        '        'Open the file.
        '        If CheckIfContactIsInList(CustomList, Number) = False Then
        '        objStreamWriter = New StreamWriter(CustomList, True, Encoding.Unicode)
        '        'If Number = "" Or Name = "" Then
        '        '    objStreamWriter.WriteLine("LST" & Number & Name)
        '        'Else
        '           objStreamWriter.WriteLine("LST" & Number & "||" & Name)
        '        'End If
        '        If IconType <> "" Then objStreamWriter.WriteLine("ICO" & IconType)
        '        objStreamWriter.Close()
        '        End If
        '        End If
    End Sub

    Public Function CheckIfContactIsInList(ByVal Path As String, ByVal number As String) As Boolean
        Dim testTxt As New StreamReader(Path)
        Dim allRead As String = testTxt.ReadToEnd()     'Reads the whole text file to the end
        testTxt.Close()                                 'Closes the text file after it is fully read.
        If Regex.IsMatch(allRead, number) Then          'If the match is found in allRead                    
            Return True
        Else
            Return False
        End If
    End Function
#End Region

    'for test only
    Public Sub CallEventTest()
        RaiseEvent Ringing()
    End Sub

#Region "BT Update Timer"
    Private Sub Timer1_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer1.Elapsed
        'Keep requesting signal quality every 30 seconds
        ATExecute("AT+CSQ" & vbCrLf, 500)
        'Messages
        ATExecute("AT+CNMI=2,1,0,0,0" & vbCrLf, 500)     'configuration pour pouvoir checker l'arrivée d'un sms(est valid en permanence)
        ATExecute("AT+CPMS=""ME""" & vbCrLf, 500)        'force les message à arriver ds ME (ME,SM aussi possible)
        'ATExecute("AT+CMTI=" & vbCrLf, 500)              'check new sms received (unknow by my phone)
        ATExecute("AT+CIMI" & vbCrLf, 500)
        ATExecute("AT+GSN" & vbCrLf, 500)
        ATExecute("AT+CBC" & vbCrLf, 500)                'test si batterie en charge (0=pas en charge, 1=en charge)
        requestPhoneInfo()

        'Dim uResult = 0
        'uResult = Btsdk_GetRemoteRSSI(phoneHandle, rssi)
        'If uResult = BTSDK_OK Then
        '    uResult = Btsdk_GetRemoteLinkQuality(phoneHandle, plink_quality)
        '    uResult = Btsdk_RemoteDeviceFlowStatistic(phoneHandle, RxBytes, TxBytes)
        '    uResult = Btsdk_GetSupervisionTimeout(phoneHandle, pTimeout)
        'End If

        'MsgBox(rssi.ToString & vbCrLf & plink_quality.ToString & vbCrLf & pTimeOut.ToString & vbCrLf & RxBytes.ToString & vbCrLf & TxBytes.ToString)

        If batteryLevel / 5 * 100 >= 100 Then
            If BatteryFullCharge = False Then RaiseEvent BatteryIsFull()
            BatteryFullCharge = True
        End If

    End Sub

    Private Sub Timer2_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer2.Elapsed
        MsgBox("Timer Finished !!")
    End Sub

#End Region

#Region "A2DP"
    Public Sub RunA2DP()
        Dim retUInt32 As UInt32
        retUInt32 = Btsdk_RegisterA2DPSRCService()
        If retUInt32 = BTSDK_OK Then
            Try
                Btsdk_RegisterA2DPSRCService(1000, Nothing)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        End If
    End Sub
#End Region

End Class