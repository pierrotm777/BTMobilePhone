Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.AutoResetEvent
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.IO


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
        '<MarshalAs(UnmanagedType.ByValArray, SizeConst:=BTSDK_PATH_MAXLENGTH + 1)> _
        Public root_dir As String
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

    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_AGAP_SendBatteryChargeIndicator(ByVal connHandle As UInt32, ByVal ind As Byte) As UInt32
    End Function



    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_AGAP_SetCurIndicatorVal(ByVal connHandle As UInt32, ByVal indicators As Btsdk_HFP_CINDInfoStru) As UInt32
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
    Private Shared Function Btsdk_PBAPRegisterStatusCallback(ByVal conn_hdl As UInt32, ByVal cb_funcs As PbabEventCallback) As UInt32
    End Function
    '** Section 6.3.9.1.23 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPRegisterFileIORoutines(ByVal svc_hdl As UInt32, ByVal cb_funcs As IntPtr) As UInt32
    End Function
    ''** Section 6.3.9.1.25 in SDK 2.1.3 **
    '<DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    'Private Shared Function Btsdk_PBAPRegisterStatusCallback(ByVal svc_hdl As UInt32, func As Btsdk_PBAP_STATUS_INFO_CB) As UInt32
    'End Function
    ''** Section 6.3.9.1.26 in SDK 2.1.3 **
    '<DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    'Public Delegate Sub Btsdk_PBAP_STATUS_INFO_CB(first As String, last As String, filename As String, filesize As UInt32, cursize As UInt32)
    'Private Shared Function Btsdk_PBAP_STATUS_INFO_CB(first As String, last As String, filename As String, filesize As UInt32, cursize As UInt32) As UInt32
    'End Function
    '** Section 6.3.9.1.27 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_PBAPPullPhoneBook(ByVal connHandle As UInt32, ByVal path As Byte, ByRef param As BtSdkPBAPParamStru, ByVal file_hdl As UInt32) As UInt32
        'Private Shared Function Btsdk_PBAPPullPhoneBook(ByVal connHandle As UInt32, ByVal path As Byte(), ByRef param As BtSdkPBAPParamStru, ByVal file_hdl As UInt32) As UInt32
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

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteRSSI(ByVal phone_handle As UInt32, ByVal signal As IntPtr) As UInt32
    End Function



    '** Section 6.3.10.1.27 in SDK 2.1.3 **
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_UnregisterMAPService(ByVal svc_hdl As UInt32) As UInt32
    End Function



#End Region

#Region "Variables"
    Private hfEventGlobal As hfEventCallback
    Private connEventGlobal As connEventCallback
    Private pbapEventGlobal As PbabEventCallback


    Private threadWaitForPhone As Thread
    Private threadStartup As Thread
    Private threadPinCode As Thread

    Private PhoneConnected As New AutoResetEvent(False)
    Private PhoneDisconnected As New AutoResetEvent(False)


    Private name As String
    Private hfService As UInt32
    Private pbapService As UInt32
    'Private mapService As UInt32

    Private phoneAddr(0 To 5) As Byte
    Shared phoneHandle As UInt32
    Private connHandle As UInt32
    Shared pbapHandle As UInt32

    Private rssi As Byte
    Private phoneModel As String
    Private phoneManufacturer As String
    Private battery As Byte
    Private networkName As String
    Private networkAvail As Boolean
    Private bSiri As Boolean
    Private SCO As Boolean
    Private standby As Boolean
    'Private currentCalls As List(Of contact)

    Private phoneConn As Boolean

    Private ContactsRetrieved As New AutoResetEvent(False)
    Private contactNum As UInt32


    Private ServicesArray As New ArrayList, servicedetail() As String
    Private PhoneBookArray As New ArrayList()
    Private PhoneBookArray2 As New ArrayList()


    Dim pbPath As String = "c:\phonebook.vcf"
    Dim historyPath As String = "c:\callhistory.vcf"


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
    'ReadOnly Property batteryStatus() As String
    '    Get
    '        Return BatteryFullCharge
    '    End Get
    'End Property
    'ReadOnly Property batteryincharge() As String
    '    Get
    '        Return ExternalPowerIsConnected
    '    End Get
    'End Property

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

    'ReadOnly Property phoneSubscriberNumber() As String
    '    Get
    '        Return Subscriber
    '    End Get
    'End Property

    'ReadOnly Property ATResultValue() As String
    '    Get
    '        Return ATValueResult
    '    End Get
    'End Property

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

    'ReadOnly Property callerID() As String
    '    Get
    '        Dim str As StringBuilder = New StringBuilder("")
    '        If currentCalls.Count > 0 Then
    '            For i As UInt32 = 0 To currentCalls.Count - 1
    '                str.Append(currentCalls(i).Name & " ")
    '            Next
    '        End If
    '        Return str.ToString()
    '    End Get
    'End Property

    'ReadOnly Property phonebook() As String
    '    Get
    '        Return phoneBookData.List.ToString()
    '    End Get
    'End Property

#End Region

    Public Function HexaToDec(ValHex As String) As Integer
        Return Val("&H" & ValHex) '& "&")
    End Function

    Private Sub initVars()
        battery = 0
        phoneModel = ""
        phoneManufacturer = ""
        rssi = 0
        networkAvail = False
        networkName = ""
        'currentCalls = New List(Of contact)
        SCO = False
        standby = True
        phoneConn = False
        bSiri = False
        'phoneBookData = New phoneBook()
        'PhoneMacAddress = ""
    End Sub

    Public Sub New(ByVal Address() As SByte)
        Try
            name = "MR2"
            initVars()

            hfService = BTSDK_INVALID_HANDLE

            'Dim PhoneMacAddress As String = "00:19:63:48:E7:DA"

            'Dim dt1 = Split(PhoneMacAddress, ":") 'découpage de l'adresse mac lu dans le fichier mobilephone.xml
            ''My mac address is : 44:4E:1A:1C:C6:15
            'phoneAddr(5) = HexaToDec(dt1(0)) '("44")
            'phoneAddr(4) = HexaToDec(dt1(1)) '("4E")
            'phoneAddr(3) = HexaToDec(dt1(2)) '("1A")
            'phoneAddr(2) = HexaToDec(dt1(3)) '("1C")
            'phoneAddr(1) = HexaToDec(dt1(4)) '("C6")
            'phoneAddr(0) = HexaToDec(dt1(5)) '("15")


            'Prevent Garbage Collection
            connEventGlobal = AddressOf connEvent
            hfEventGlobal = AddressOf hfEvent
            pbapEventGlobal = AddressOf pbapEvent


            threadWaitForPhone = New Thread(AddressOf waitForPhone)
            threadWaitForPhone.IsBackground = True
            threadWaitForPhone.Start()

        Catch ex As Exception
            MsgBox("Sub New Catch - " & ex.Message)
        End Try

    End Sub


    'Public Sub runPBAP()
    '    Try
    '        connectPBAPService()
    '        getPBAPBook("Contacts")
    '        getPBAPBook("History")
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '    End Try

    'End Sub
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
                    If Btsdk_IsBluetoothReady() = BTSDK_FALSE Then
                        If Not Btsdk_StartBluetooth() = BTSDK_OK Then
                            MsgBox("Check Bluetooth device, it could not be started!")
                        End If
                    End If
                    Btsdk_SetLocalDeviceClass(&H240404) 'Original
                    'Btsdk_SetLocalDeviceClass(BTSDK_DEVCLS_PHONE + BTSDK_PHONECLS_CELLULAR + BTSDK_SRVCLS_AUDIO) 'OK
                    'Btsdk_SetLocalDeviceClass(BTSDK_PHONECLS_UNCLASSIFIED + BTSDK_PHONECLS_CELLULAR + BTSDK_SRVCLS_AUDIO) 'OK
                    getPairedPhoneHandle()
                    'If Not Btsdk_StartEnumConnection() = BTSDK_INVALID_HANDLE Then
                    '    Btsdk_StopBluetooth()
                    '    Btsdk_StartBluetooth()
                    'End If
                    setCallBacks()
                    HFPInit()
                End If
                'Else 'BTSDK_TRUE car Btsdk_Init() = BTSDK_OK
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
            getPairedPhoneHandle()
            connectAG_HANDSFREE()
            connectPBAPService()
            getPBAPBook("Contacts")
            'getPBAPBook("History")

            While True
                If PhoneConnected.WaitOne(5000) Then
                    Exit While
                End If
            End While
            threadStartup.Start()
            phoneConn = True
            PhoneDisconnected.WaitOne()
            initVars()
            getPairedPhoneHandle()
            threadStartup.Abort()
            threadStartup = New Thread(AddressOf startup)
            threadStartup.IsBackground = True
            phoneConn = False
        End While

    End Sub

    Private Sub startup()
        'Perform Startup Duties in a Seperate Thread
        On Error Resume Next
        'If TempPluginSettings.PhoneBookList = "" Then
        '    getPhoneBookTypeList() 'Read what Phone Book are Usable from the phone
        'End If
        'Thread.Sleep(500)
        'CheckReceiveSMS() 'prepare au test de l'arrivée d'un nouveau SMS
        'Thread.Sleep(500)
        'requestPhoneInfo() 'Read Info from Phone
        'Thread.Sleep(500)

        'If TempPluginSettings.PhoneBookUpdate = True Then 'mise à jour des phone book
        '    Dim PhoneBooKListArray() As String = TempPluginSettings.PhoneBookList.Split(",")
        '    For Each PhoneBookList As String In PhoneBooKListArray 'Read each phone book usable and sav it
        '        PhoneBookSyncInProgress = True
        '        getPhoneBook(PhoneBookList)
        '        Do While PhoneBookSyncInProgress = True
        '            Thread.Sleep(3000)
        '        Loop
        '        'PhoneBookSyncFlag = PhoneBookSyncFlag + 1
        '        'If PhoneBookSyncFlag < 2 Then
        '        Thread.Sleep(30000)
        '        'End If
        '    Next
        '    debugFrm.debugTextBox.AppendText("Update phonebooks is done ..." & vbCrLf & vbCrLf)
        '    Do While PhoneBookSyncInProgress = True
        '        Thread.Sleep(3000)
        '    Loop
        '    PhoneBookMerge("SM", "ME")
        '    debugFrm.debugTextBox.AppendText("Merge phonebooks is done ..." & vbCrLf & vbCrLf)
        '    Do While PhoneBookMergeInProgress = True
        '        Thread.Sleep(3000)
        '    Loop
        '    'PhoneBookSortAll("ME")
        'Else
        '    debugFrm.debugTextBox.AppendText(vbCrLf & vbCrLf & "If you want to update the phone book, turn in TRUE" & vbCrLf & "the PhoneBookUpdate settings into your .xml file ..." & vbCrLf & vbCrLf)
        'End If

        'ATExecute("AT+CMEE=2" & vbCr, 500) 'define extract ERROR to verbose mode
        'ATExecute("AT+CMGF=?" & vbCr, 500)

        'Dim LocalName As String = "MyPhone"
        'Btsdk_SetLocalName(LocalName, LocalName.Length)

        'Thread.Sleep(1000)
        'sort all phonebook in a new thread
        'workerThread = New Threading.Thread(AddressOf PhoneBookSort)
        'workerThread.Start()


        'LogValues timer
        'Timer1 = New System.Timers.Timer()
        'Timer1.Enabled = True
        'Timer1.Interval = 30000
        'Timer1.AutoReset = True

        While True
            'Keep requesting signal quality every 10 seconds
            requestPhoneInfo()
            Thread.Sleep(10000)
        End While
    End Sub

    Public Sub answerCall()
        If Btsdk_HFAP_AnswerCall(connHandle) = BTSDK_OK Then
            'CallIsActif = True
            RaiseEvent PhoneInCall()
        End If
    End Sub

    Public Sub hangupCall()
        If Btsdk_HFAP_CancelCall(connHandle) = BTSDK_OK Then
            'CallIsActif = False
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
    End Function

    'Public Sub contactRetrieve(ByVal contactsNum As UInt32)
    '    Dim nContactsRetrieved As UInt32 = 1
    '    For PhoneNumberOfContactsLoop As Integer = 1 To PhoneNumberOfContactsFound
    '        ATExecute("AT+CPBR=" & PhoneNumberOfContactsLoop & vbCrLf, 500)
    '    Next

    '    '        While (nContactsRetrieved <= contactsNum)

    '    'Set the next contact to look out for
    '    '       contactNum = nContactsRetrieved

    '    '        ATExecute("AT+CPBR=" & nContactsRetrieved & "," & nContactsRetrieved & vbCr, 2000)
    '    '        If (ContactsRetrieved.WaitOne(2000)) Then
    '    '        nContactsRetrieved += 1
    '    '        'Position 128 has no contact Unsure of position 256 I dont have that many contacts!
    '    '        If (nContactsRetrieved = 128) Then
    '    '        nContactsRetrieved += 1
    '    '        End If
    '    '        End If
    '    '        End While
    'End Sub

    Private Sub processATResult(ByVal result As String)
        Dim atRegex As New Regex(".*?\+(?<cmd>[A-Z]+):\s*(?<result>.*)", RegexOptions.IgnoreCase)
        Dim atMatch As Match = atRegex.Match(result.Trim())
        If atMatch.Success Then
            Select Case atMatch.Groups("cmd").Value
                Case "CSQ"
                    'Signal Quality
                    Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),([0-9]+)")
                    rssi = Byte.Parse(resultMatch.Groups(1).Value)
                Case "CIND"
                    Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),([0-9]+),([0-9]+),([0-9]+),([0-9]+),([0-9]+),([0-9]+)")
                    battery = Byte.Parse(resultMatch.Groups(6).Value)
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
                    'currentCalls.Clear()
                End If

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_RINGING_IND
                'Phone is Ringing Get Call Info
                standby = False
                Btsdk_HFAP_GetCurrentCalls(connHandle)
                RaiseEvent Ringing()

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_ONGOINGCALL_IND
                'SetText("Ongoing Call")
                'IncomingCall = True
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
                'ATValueResult = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64 + 3), res.result_code) 'res.result_code 'ajout pierre
                'ATValueResult = Hex(Marshal.ReadByte(param)) 'Marshal.PtrToStringAnsi(param.ToInt64, res.result_code)
                'Dim result As String = Marshal.PtrToStringAnsi(param, param_len)
                'ATValueResult = Hex(e.ToString)
                'MsgBox(ATValueResult)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_CLIP_IND
                'SetText("HFP EV")
                Dim res As Btsdk_HFP_PhoneInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_PhoneInfo))
                'Subscriber = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64 + 3), res.num_len)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_NETWORK_OPERATOR_IND 'OK
                Dim network_operator As Btsdk_HFP_COPSInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_COPSInfo))
                networkName = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64() + 3), network_operator.operator_len)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND 'OK
                'To get the subscriber number not needed
                Dim res As Btsdk_HFP_PhoneInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_PhoneInfo))
                'Subscriber = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64 + 3), res.num_len)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_CURRENT_CALLS_IND 'voir p197
                Dim clcc As Btsdk_HFP_CLCCInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_CLCCInfo))
                'Dim number As String = Marshal.PtrToStringAnsi(IntPtr.Add(param, 7), clcc.num_len)
                Dim number As String = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64() + 7), clcc.num_len)

                'currentCalls = phoneBookData.getName(number)
                ''MsgBox("got ID")
                'If currentCalls.Count = 0 Then
                '    Dim c As New contact(0, number, "")
                '    currentCalls.Add(c)
                '    '******************************************************
                '    'ajouter nouvel appelant si n'existe pas ds custom list
                '    '******************************************************
                'End If


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

    Public Sub Siri()
        If (bSiri) Then
            Btsdk_HFAP_VoiceRecognitionReq(connHandle, 0)
            bSiri = False
        Else
            Btsdk_HFAP_VoiceRecognitionReq(connHandle, 1)
            bSiri = True
        End If
    End Sub



    Public Sub pbapEvent(ByVal first As Byte, ByVal last As Byte, ByVal filename As IntPtr, ByVal filesize As UInt32, ByVal cursize As UInt32)
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

    Shared Function APP_CreateFile(pth As String) As IntPtr
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
            hfService = Btsdk_RegisterHFPService(name, BTSDK_CLS_HANDSFREE_AG, BTSDK_AG_BRSF_3WAYCALL Or BTSDK_AG_BRSF_BVRA Or BTSDK_AG_BRSF_BINP Or BTSDK_AG_BRSF_REJECT_CALL Or BTSDK_AG_BRSF_ENHANCED_CALLSTATUS Or BTSDK_AG_BRSF_ENHANCED_CALLCONTROL Or BTSDK_AG_BRSF_EXTENDED_ERRORRESULT)
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
        If Not Btsdk_IsSDKInitialized() = BTSDK_OK Or Not Btsdk_StartBluetooth() = BTSDK_OK Then
            If Not Btsdk_Init() = BTSDK_OK And Not Btsdk_StartBluetooth() = BTSDK_OK Then
                Return False
            End If
        End If
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


    Public Sub getPBAPBook(pbType As String)
        Try
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

            'Dim pbBytes = System.Text.Encoding.ASCII.GetBytes(combine)

            'result = Btsdk_PBAPPullPhoneBook(hconn, pbBytes, param, hFile)
            result = Btsdk_PBAPPullPhoneBook(hconn, Convert.ToByte(combine), param, hFile)

            Marshal.FreeHGlobal(ptr)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Public Function dial(ByVal phoneNumber As String) As Boolean
        If phoneNumber = "" Then Return False
        Dim tempBytes(0 To 0) As Byte
        tempBytes = System.Text.Encoding.UTF8.GetBytes(phoneNumber & Chr(0))
        Dim retUInt32 As UInt32
        retUInt32 = Btsdk_HFAP_Dial(connHandle, tempBytes(0), CUShort(tempBytes.Length - 1))
        If retUInt32 = BTSDK_OK Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub requestPhoneInfo()
        'Battery Level 0-5
        ATExecute("AT+CIND?" & vbCr, 200)
        Thread.Sleep(500)
        'Signal Level
        ATExecute("AT+CSQ" & vbCr, 200)
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


    'for test only
    Public Sub CallEventTest()
        RaiseEvent Ringing()
    End Sub


End Class