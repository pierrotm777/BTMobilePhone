Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.AutoResetEvent
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Generic

Public Class BT

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkRemoteServiceAttrStru
        Public mask As UInt16
        Public service_class As UInt16
        Public dev_hndl As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=80, ArraySubType:=UnmanagedType.I1)> _
        Public svc_name() As Byte
        Public ext_attributes As IntPtr
        Public status As UInt16
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkCallbackStru
        Public type As UInt16            'type of callback*/
        Public func As connEventCallback 'callback function*/
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Btsdk_HFP_ATCmdResult
        Public cmd_code As UInt16
        Public result_code As Byte
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Btsdk_HFP_PhoneInfo
        Public type As Byte 'the format of the phone number provided
        Public service As Byte 'Indicates which service this phone number relates to. Shall be either 4 (voice) or 5 (fax).
        Public num_len As Byte 'the length of the phone number provided
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32, ArraySubType:=UnmanagedType.I1)> _
        Public number() As SByte 'subscriber number, the length shall be PHONENUM_MAX_DIGITS
        Public name_len As Byte 'length of subaddr
        Public alpha_str As SByte  'string type subaddress of format specified by <cli_validity>
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Btsdk_HFP_COPSInfo
        Public mode As Byte             'current mode and provides no information with regard to the name of the operator */
        Public format As Byte               'the format of the operator parameter string */
        Public operator_len As Byte
        Public operator_name As SByte
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Btsdk_HFP_CLCCInfo
        Public idx As Byte              'The numbering (start with 1) of the call given by the sequence of setting up or receiving the calls */
        Public dir As Byte              'Direction, 0=outgoing, 1=incoming */
        Public status As Byte           '0=active, 1=held, 2=dialling(outgoing), 3=alerting(outgoing), 4=incoming(incoming), 5=waiting(incoming) */
        Public mode As Byte             '0=voice, 1=data, 2=fax */
        Public mpty As Byte             '0=not multiparty, 1=multiparty */
        Public type As Byte             'the format of the phone number provided */
        Public num_len As Byte          'the length of the phone number provided */	
        Public number As SByte
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure BtSdkConnectionProperty
        Private ReadOnly raw As UInt32
        Public device_handle As UInt32
        Public service_handle As UInt32
        Public service_class As UInt16
        Public duration As UInt32
        Public received_bytes As UInt32
        Public sent_bytes As UInt32
        Public ReadOnly Property role As Byte
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

    Const BTSDK_APP_EV_CONN_IND As UInt32 = 1
    Const BTSDK_APP_EV_DISC_IND As UInt32 = 2
    Const BTSDK_APP_EV_CONN_CFM As UInt32 = 7
    Const BTSDK_APP_EV_DISC_CFM As UInt32 = 8
    Const BTSDK_CONNECTION_EVENT_IND As UInt32 = 9

    Const BTSDK_CLS_HANDSFREE_AG As UInt32 = &H111F
    Const BTSDK_CLS_HEADSET_AG As UInt32 = &H1112

    Const BTSDK_AG_BRSF_3WAYCALL As UInt32 = &H1 '/* Three-way calling */
    Const BTSDK_AG_BRSF_NREC As UInt32 = &H2 '/* EC and/or NR function */
    Const BTSDK_AG_BRSF_BVRA As UInt32 = &H4 '/* Voice recognition function */
    Const BTSDK_AG_BRSF_INBANDRING As UInt32 = &H8 '/* In-band ring tone capability */
    Const BTSDK_AG_BRSF_BINP As UInt32 = &H10 '/* Attach a number to a voice tag */
    Const BTSDK_AG_BRSF_REJECT_CALL As UInt32 = &H20 '/* Ability to reject a call */
    Const BTSDK_AG_BRSF_ENHANCED_CALLSTATUS As UInt32 = &H40 '/* Enhanced call status */
    Const BTSDK_AG_BRSF_ENHANCED_CALLCONTROL As UInt32 = &H80 '/* Enhanced call control */
    Const BTSDK_AG_BRSF_EXTENDED_ERRORRESULT As UInt32 = &H100 '/* Extended Error Result Codes */
    Const BTSDK_AG_BRSF_ALL As UInt32 = &H1FF 'All of the above

    Const BTSDK_OK As UInt32 = 0

    Const BTSDK_TRUE As UInt32 = 1
    Const BTSDK_FALSE As UInt32 = 0

    Const BTSDK_INVALID_HANDLE As UInt32 = 0

    Const BTSDK_APP_EV_AGAP_BASE As UInt32 = &H900
    Public Enum BTSDK_HFP_APP_EventCodeEnum
        ' HFP_SetState Callback to Application Event Code */
        ' SLC - Both AG and HF */
        BTSDK_HFP_EV_SLC_ESTABLISHED_IND = BTSDK_APP_EV_AGAP_BASE + 1   ' HFP Service Level connection established. Parameter: Btsdk_HFP_ConnInfoStru */
        BTSDK_HFP_EV_SLC_RELEASED_IND                   ' SPP connection released. Parameter: Btsdk_HFP_ConnInfoStru */

        ' SCO - Both AG and HF  */
        BTSDK_HFP_EV_AUDIO_CONN_ESTABLISHED_IND     ' SCO audio connection established */
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
    End Enum

    Public Delegate Sub hfEventCallback(ByVal hdl As UInt32, ByVal e As UInt16, ByVal param As IntPtr, ByVal param_len As UInt16)
    Public Delegate Sub connEventCallback(ByVal hdl As UInt32, ByVal reason As UInt16, ByVal arg As IntPtr)

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_Init() As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_StartBluetooth() As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_SetLocalDeviceClass(ByVal deviceClass As UInt32) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RegisterCallback4ThirdParty(ByVal cbStruc As IntPtr) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_APPRegCbk4ThirdParty(ByVal hfCB As hfEventCallback) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_RegisterHFPService(ByVal name As String, ByVal svcClass As UInt16, ByVal features As UInt16) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_IsBluetoothReady() As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteDeviceHandle(ByVal addr As IntPtr) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_BrowseRemoteServices(ByVal device_handle As UInt32, ByVal servicesHandles As IntPtr, ByVal serviceHandlesLen As IntPtr) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteServiceAttributes(ByVal serviceHandle As UInt32, ByVal pattributes As IntPtr) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_Connect(ByVal serviceHandle As UInt32, ByVal lParam As UInt32, ByVal connHandle As IntPtr) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_IsSDKInitialized() As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Sub Btsdk_Done()
    End Sub

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_IsDeviceConnected(ByVal phone_handle As UInt32) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_Disconnect(ByVal conn_handle As UInt32) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_UnregisterHFPService(ByVal service_handle As UInt32) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_GetRemoteDeviceAddress(ByVal phone_handle As UInt32, ByVal addr As IntPtr) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFP_ExtendCmd(ByVal connHandle As UInt32, ByVal cmd As String, ByVal cmdLen As UInt16, ByVal Timeout As UInt32) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_VoiceRecognitionReq(ByVal connHandle As UInt32, ByVal enable As Byte) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_AnswerCall(ByVal connHandle As UInt32) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_CancelCall(ByVal connHandle As UInt32) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_TxDTMF(ByVal connHandle As UInt32, ByVal num As Byte) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_GetCurrentCalls(ByVal connhandle As UInt32) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_HFAP_GetCurrHFState(ByVal state As IntPtr) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_StartEnumConnection() As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_EnumConnection(ByVal searchHandle As UInt32, ByVal connStructPtr As IntPtr) As UInt32
    End Function

    <DllImport("BsSDK.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function Btsdk_EndEnumConnection(ByVal searchHandle As UInt32) As UInt32
    End Function

    Private hfEventGlobal As hfEventCallback
    Private connEventGlobal As connEventCallback

    Private threadWaitForPhone As Thread
    Private threadStartup As Thread

    Private PhoneConnected As New AutoResetEvent(False)
    Private PhoneDisconnected As New AutoResetEvent(False)

    Private name As String
    Private hfService As UInt32
    Private agService As UInt32

    Private phoneAddr(0 To 5) As Byte
    Private phoneHandle As UInt32
    Private connHandle As UInt32

    'Status Indicators/Variables
    Private rssi As Byte
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
    Private phoneBookData As phoneBook

    Public Event Ringing()
    Public Event Hungup()
    Public Event VoiceActivation()
    Public Event VoiceDeactivation()

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

    ReadOnly Property signalStrength() As Integer
        Get
            Return rssi
        End Get
    End Property

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

    Public Sub answerCall()
        Btsdk_HFAP_AnswerCall(connHandle)
    End Sub

    Public Sub hangupCall()
        Btsdk_HFAP_CancelCall(connHandle)
    End Sub

    Public Sub dtmf(ByVal num As String)
        If num.Length = 1 Then
            Dim encoding As New System.Text.ASCIIEncoding()
            Dim bytes() As Byte = encoding.GetBytes(num)
            Btsdk_HFAP_TxDTMF(connHandle, bytes(0))
        End If
    End Sub

    Public Sub connEvent(ByVal hdl As UInt32, ByVal reason As UInt16, ByVal arg As IntPtr)
        Dim connProperties As BtSdkConnectionProperty = Marshal.PtrToStructure(arg, GetType(BtSdkConnectionProperty))

        If isPhone(connProperties.device_handle) Then
            Select Case reason
                Case BTSDK_APP_EV_DISC_IND
                    PhoneDisconnected.Set()
                    PhoneConnected.Reset()
                Case BTSDK_APP_EV_DISC_CFM
                    PhoneDisconnected.Set()
                    PhoneConnected.Reset()
                Case BTSDK_APP_EV_CONN_CFM
                    PhoneConnected.Set()
                    PhoneDisconnected.Reset()
                Case BTSDK_APP_EV_CONN_IND
                    PhoneConnected.Set()
                    PhoneDisconnected.Reset()
                Case Else

            End Select
        End If
    End Sub

    Public Function ATExecute(ByVal cmd As String, ByVal timeout As UInt32) As Boolean
        Return Btsdk_HFP_ExtendCmd(connHandle, cmd, cmd.Length, timeout)
    End Function

    Public Sub contactRetrieve(ByVal contactsNum As UInt32)
        Dim nContactsRetrieved As UInt32 = 1

        While (nContactsRetrieved <= contactsNum)

            'Set the next contact to look out for
            contactNum = nContactsRetrieved

            ATExecute("AT+CPBR=" & nContactsRetrieved & "," & nContactsRetrieved & vbCr, 2000)

            If (ContactsRetrieved.WaitOne(2000)) Then
                nContactsRetrieved += 1
                'Position 128 has no contact Unsure of position 256 I dont have that many contacts!
                If (nContactsRetrieved = 128) Then
                    nContactsRetrieved += 1
                End If
            End If
        End While
    End Sub

    Private Sub processATResult(ByVal result As String)
        Dim atRegex As New Regex(".*?\+(?<cmd>[A-Z]+):\s*(?<result>.*)", RegexOptions.IgnoreCase)
        Dim atMatch As Match = atRegex.Match(result.Trim())

        If atMatch.Success Then
            Select Case atMatch.Groups("cmd").Value
                Case "CSQ"
                    'Signal Quality
                    Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),([0-9]+)")
                    rssi = Byte.Parse(resultMatch.Groups(1).Value)
                Case "CGMM"
                    'Model
                    phoneModel = atMatch.Groups("result").Value
                Case "CGMI"
                    'Manufacturer
                    phoneManufacturer = atMatch.Groups("result").Value.Replace("""", "")
                Case "CPBR"
                    'Number of Records on Phone
                    'Time to retrieve them all
                    '(1-XXX),X,X
                    Dim recordMatch As Match = Regex.Match(atMatch.Groups("result").Value, "\(([0-9]+)-([0-9]+)\),([0-9]+),([0-9]+)")
                    If (recordMatch.Success) Then
                        Dim contactRetrieveThread As Thread = New Thread(AddressOf contactRetrieve)
                        contactRetrieveThread.IsBackground = True
                        contactRetrieveThread.Start(UInt32.Parse(recordMatch.Groups(2).Value))
                    Else
                        'Phonebook Record
                        'Add to Phonebook
                        Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),""(\+{0,1}[0-9]+)"",([0-9]+),""(.*?)""")
                        Dim c As contact = New contact(UInt32.Parse(resultMatch.Groups(1).Value), resultMatch.Groups(4).Value, resultMatch.Groups(2).Value)
                        phoneBookData.add(c)
                        'Set the Global Var to indicate we can go to the next contact
                        If (UInt32.Parse(resultMatch.Groups(1).Value) = contactNum) Then
                            ContactsRetrieved.Set()
                        End If

                    End If
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

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_NETWORK_UNAVAILABLE_IND
                networkAvail = False

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SPKVOL_CHANGED_IND
                'Speaker vol not needed

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_MICVOL_CHANGED_IND
                'Micvol not needed

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_VOICETAG_PHONE_NUM_IND
                'Voicetag Not needed

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_EXTEND_CMD_IND
                Dim result As String = Marshal.PtrToStringAnsi(param, param_len)
                processATResult(result)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_ATCMD_RESULT
                'Dim res As Btsdk_HFP_ATCmdResult = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_ATCmdResult))
                'SetText("Receiving  AT command: " & res.cmd_code & " " & res.result_code)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_CLIP_IND
                'SetText("HFP EV")

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_NETWORK_OPERATOR_IND
                Dim network_operator As Btsdk_HFP_COPSInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_COPSInfo))
                'networkName = Marshal.PtrToStringAnsi(IntPtr.Add(param, 3), network_operator.operator_len)
                networkName = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64() + 3), network_operator.operator_len)

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND
                'To get the subscriber number not needed

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_CURRENT_CALLS_IND
                Dim clcc As Btsdk_HFP_CLCCInfo = Marshal.PtrToStructure(param, GetType(Btsdk_HFP_CLCCInfo))
                'Dim number As String = Marshal.PtrToStringAnsi(IntPtr.Add(param, 7), clcc.num_len)
                Dim number As String = Marshal.PtrToStringAnsi(New IntPtr(param.ToInt64() + 7), clcc.num_len)

                currentCalls = phoneBookData.getName(number)
                'MsgBox("got ID")
                If currentCalls.Count = 0 Then
                    Dim c As New contact(0, number, "")
                    currentCalls.Add(c)
                End If

            Case BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_BATTERY_CHARGE_IND
                battery = Marshal.ReadByte(param)
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
            hfService = Btsdk_RegisterHFPService(name, BTSDK_CLS_HANDSFREE_AG, BTSDK_AG_BRSF_3WAYCALL Or _
            BTSDK_AG_BRSF_BVRA Or BTSDK_AG_BRSF_BINP Or BTSDK_AG_BRSF_REJECT_CALL Or _
            BTSDK_AG_BRSF_ENHANCED_CALLSTATUS Or BTSDK_AG_BRSF_ENHANCED_CALLCONTROL Or _
            BTSDK_AG_BRSF_EXTENDED_ERRORRESULT)
            'agService = Btsdk_RegisterHFPService(name, BTSDK_CLS_HEADSET_AG, 0)
            Return True
        End If
        Return False
    End Function

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
    End Sub

    Public Sub New()
        name = "MR2"
        initVars()
        agService = BTSDK_INVALID_HANDLE
        hfService = BTSDK_INVALID_HANDLE

        ''''''''''''''''''''''''''''''
        ''' Set phone_addr Here    '''
        ''''''''''''''''''''''''''''''
        phoneAddr(5) =
        phoneAddr(4) =
        phoneAddr(3) =
        phoneAddr(2) =
        phoneAddr(1) =
        phoneAddr(0) =

        'Prevent Garbage Collection
        connEventGlobal = AddressOf connEvent
        hfEventGlobal = AddressOf hfEvent

        threadWaitForPhone = New Thread(AddressOf waitForPhone)
        threadWaitForPhone.IsBackground = True
        threadWaitForPhone.Start()

    End Sub

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
        Marshal.WriteInt32(psvc_num, 10)

        If Not Btsdk_BrowseRemoteServices(phoneHandle, psvc_hndls, psvc_num) = BTSDK_OK Then
            Return False
        End If

        connHandle = BTSDK_INVALID_HANDLE

        For i As Byte = 0 To Marshal.ReadInt32(psvc_num) - 1
            If Btsdk_GetRemoteServiceAttributes(Marshal.ReadInt32(psvc_hndls, i * Marshal.SizeOf(GetType(UInt32))), psvc_attr) = BTSDK_OK Then
                svc_attr = Marshal.PtrToStructure(psvc_attr, GetType(BtSdkRemoteServiceAttrStru))
                If svc_attr.service_class = BTSDK_CLS_HANDSFREE_AG Then
                    If Btsdk_Connect(Marshal.ReadInt32(psvc_hndls, i * Marshal.SizeOf(GetType(UInt32))), 0, pconn_hndl) = BTSDK_OK Then
                        connHandle = Marshal.ReadInt32(pconn_hndl)
                    End If
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

    Private Function restoreConnection() As Boolean
        Dim searchHandle As UInt32 = Btsdk_StartEnumConnection()
        Dim connPtr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(BtSdkConnectionProperty)))
        Dim connH As UInt32
        Dim conn As BtSdkConnectionProperty

        If Not searchHandle = BTSDK_INVALID_HANDLE Then
            connH = Btsdk_EnumConnection(searchHandle, connPtr)
            While Not connH = BTSDK_INVALID_HANDLE
                conn = Marshal.PtrToStructure(connPtr, GetType(BtSdkConnectionProperty))
                If isPhone(conn.device_handle) And conn.service_class = BTSDK_CLS_HANDSFREE_AG Then
                    phoneHandle = conn.device_handle
                    connHandle = connH
                    Exit While
                End If
                connHandle = Btsdk_EnumConnection(searchHandle, connPtr)
            End While
            Btsdk_EndEnumConnection(searchHandle)
        End If

        Marshal.FreeHGlobal(connPtr)

        If connHandle = BTSDK_INVALID_HANDLE Then
            Return False
        Else
            Return True
        End If

    End Function


    Private Sub waitForPhone()

        While True
            If Btsdk_IsSDKInitialized() = BTSDK_FALSE Then
                If Btsdk_Init() = BTSDK_OK Then
                    Btsdk_SetLocalDeviceClass(&H240404)
                    getPairedPhoneHandle()
                    setCallBacks()
                    HFPInit()
                End If
            Else
                Exit While
            End If
            Thread.Sleep(1000)
        End While

        If (IsNothing(threadStartup)) Then
            threadStartup = New Thread(AddressOf startup)
            threadStartup.IsBackground = True
        End If

        While True
            'If restoreConnection() Then
            ' PhoneConnected.Set()
            ' Else
            connectAG_HANDSFREE()
            'End If
            While True
                If PhoneConnected.WaitOne(5000) Then
                    Exit While
                End If
                'If Not restoreConnection() Then
                'PhoneConnected.Set()
                'Else
                connectAG_HANDSFREE()
                'End If
            End While
            threadStartup.Start()
            phoneConn = True
            PhoneDisconnected.WaitOne()
            initVars()
            threadStartup.Abort()
            threadStartup = New Thread(AddressOf startup)
            threadStartup.IsBackground = True
            phoneConn = False
        End While

    End Sub

    Private Sub getPhoneBook()
        ATExecute("AT+CPBS=""ME""" & vbCr, 2000)
        ATExecute("AT+CSCS=""UTF-8""" & vbCr, 2000)
        ATExecute("AT+CPBR=?" & vbCr, 6000)
    End Sub

    Private Sub startup()
        'Perform Startup Duties in a Seperate Thread
        Try
            requestPhoneInfo()
            getPhoneBook()
            While True
                'Keep requesting signal quality every 30 seconds
                ATExecute("AT+CSQ" & vbCr, 1000)
                Thread.Sleep(30000)
            End While
        Catch
        End Try
    End Sub

    Public Sub dial(ByVal number As String)
        ATExecute("ATD" & number & ";" & vbCr, 1000)
    End Sub

    Public Sub requestPhoneInfo()
        ATExecute("AT+CGMM", 1000)
        ATExecute("AT+CGMI", 1000)
    End Sub

    Private Sub disconnectPhone()
        If Btsdk_IsDeviceConnected(phoneHandle) = BTSDK_TRUE Then
            Btsdk_Disconnect(connHandle)
        End If
    End Sub

    Private Sub HFPDone()
        Btsdk_UnregisterHFPService(hfService)
        Btsdk_UnregisterHFPService(agService)
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

End Class
