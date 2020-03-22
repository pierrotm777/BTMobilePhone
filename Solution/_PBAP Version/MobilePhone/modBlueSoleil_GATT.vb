'modBlueSoleil_FTP - Written by Jesse Yeager.    www.CompulsiveCode.com



Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices

Module modBlueSoleil_GATT

    Private Const BTSDK_OK As UInt32 = 0
    Private Const BTSDK_TRUE As Byte = 1
    Private Const BTSDK_FALSE As Byte = 0


    Private Const BS_GATT_SVC_ALERTNOTIFICATION As UShort = &H1811
    Private Const BS_GATT_SVC_BATTERY As UShort = &H180F
    '
    '17 more to do


    Private Const BS_GATT_CH_ALERTCATEGORYID As UShort = &H2A43
    Private Const BS_GATT_CH_ALERTCATEGORYIDBITMASK As UShort = &H2A42
    Private Const BS_GATT_CH_ALERTLEVEL As UShort = &H2A06
    Private Const BS_GATT_CH_ALERTNOTIFCTRLPOINT As UShort = &H2A44
    Private Const BS_GATT_CH_ALERTSTATUS As UShort = &H2A3F
    Private Const BS_GATT_CH_APPEARANCE As UShort = &H2A01
    Private Const BS_GATT_CH_BATTERYLEVEL As UShort = &H2A19
    '
    '65 more to do


    Private Const BS_GATT_DSCR_chAGGREGATEFORMAT As UShort = &H2905
    Private Const BS_GATT_DSCR_chEXTENDEDPROPS As UShort = &H2900
    Private Const BS_GATT_DSCR_chPRESENTATIONFORMAT As UShort = &H2904
    Private Const BS_GATT_DSCR_chUSERDESCRIPTION As UShort = &H2901


    'get services
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)>
    Private Function Btsdk_GATTGetServices(ByVal dvcHandle As UInt32, ByRef inpSvcBufferCount As UInt16, ByRef ptrArrayGATTServiceStru As Byte, ByRef retNumSvcs As UInt16, ByVal gattFunctionFlags As UInt32) As UInt32
    End Function


    'get characteristics
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)>
    Private Function Btsdk_GATTGetCharacteristics(ByVal dvcHandle As UInt32, ByRef ptrGATTsvcStru As Byte, ByVal inpCharacteristicsBufferCount As UInt16, ByRef ptrArrayGATTcharacteristicStru As Byte, ByRef retNumCharacteristics As UInt16, ByVal gattFunctionFlags As UInt32) As UInt32
    End Function


    'get descriptors
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)>
    Private Function Btsdk_GATTGetDescriptors(ByVal dvcHandle As UInt32, ByRef ptrGATTcharacteristicStru As Byte, ByVal inpDescriptorsBufferCount As UInt16, ByRef ptrArrayGATTdescriptorStru As Byte, ByRef retNumDescriptors As UInt16, ByVal gattFunctionFlags As UInt32) As UInt32
    End Function


    'get characteristic value
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)>
    Private Function Btsdk_GATTGetCharacteristicValue(ByVal dvcHandle As UInt32, ByRef ptrGATTcharacteristicStru As Byte, ByVal inpCharacteristicValueDataSize As UInt16, ByRef ptrGATTcharacteristicValueStru As Byte, ByRef retCharacteristicValueSizeReqd As UInt16, ByVal gattFunctionFlags As UInt32) As UInt32
    End Function


    'get descriptor value
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)>
    Private Function Btsdk_GATTGetDescriptorValue(ByVal dvcHandle As UInt32, ByRef ptrGATTdescriptorStru As Byte, ByVal inpDescriptorValueDataSize As UInt16, ByRef ptrGATTdescriptorValueStru As Byte, ByRef retDescriptorValueSizeReqd As UInt16, ByVal gattFunctionFlags As UInt32) As UInt32
    End Function


    'get LE device type code.
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)>
    Private Function Btsdk_GetLEDeviceAppearance(ByVal dvcHandle As UInt32, ByRef retDvcAppearanceCode As UInt16) As UInt32
    End Function


    'register event.  this one looks rough.  not sure about eventType declaration.
    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)>
    Private Function Btsdk_GATTRegisterEvent(ByVal dvcHandle As UInt32, ByVal eventType As UInt32, ByRef evtParam_ptrGATTcharacteristicStru As Byte, ByRef ptrGATTnotificationCallbackStr As Byte, ByVal callbackContext As UInt32, ByRef retEventHandle As UInt32, ByVal gattFunctionFlags As UInt32) As UInt32
    End Function


    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)>
    Private Function Btsdk_GATTUnregisterEvent(ByVal evtHandle As UInt32, ByVal gattFunctionFlags As UInt32) As UInt32
    End Function


    <DllImport("BsSDK.dll", CallingConvention:=System.Runtime.InteropServices.CallingConvention.Cdecl)>
    Private Function Btsdk_GATTCloseSession(ByVal dvcHandle As UInt32, ByVal gattFunctionFlags As UInt32) As UInt32
    End Function



    Public Function BlueSoleil_GATT_GetDeviceServices(ByVal dvcHandle As UInt32, ByRef retSvcUUIDs() As String, ByRef retSvcAttribHandles() As UInt16, ByRef retSvcCount As Integer) As Boolean


        'get size of a single GATTserviceStru
        Dim struGATTservice(0 To 0) As Byte
        BlueSoleil_GATT_InitStruBytes_GATTserviceStru(struGATTservice, "", 0)
        Dim struSizeGATTservice As Integer = struGATTservice.Length

        Dim struArrayGATTservice(0 To 0) As Byte
        ReDim struArrayGATTservice(0 To 50 * struSizeGATTservice)

        'get services
        Dim struArrayServiceCount As UInt16 = 0
        Dim retInt As UInt32 = Btsdk_GATTGetServices(dvcHandle, 50, struArrayGATTservice(0), struArrayServiceCount, 0)

        Dim i As Integer
        Dim currByteIdx As Integer = 0

        For i = 0 To struArrayServiceCount - 1
            'parse stru.


        Next '


    End Function


    Public Function BlueSoleil_GATT_GetDeviceBatteryPct(ByVal dvcHandle As UInteger) As Integer

        'if return is -1, it failed.



        'get size of a single GATTserviceStru
        Dim struGATTservice(0 To 0) As Byte
        BlueSoleil_GATT_InitStruBytes_GATTserviceStru(struGATTservice, "", 0)
        Dim struSizeGATTservice As Integer = struGATTservice.Length

        Dim struArrayGATTservice(0 To 0) As Byte
        ReDim struArrayGATTservice(0 To 50 * struSizeGATTservice)

        'get services
        Dim struArrayServiceCount As UInt16 = 0
        Btsdk_GATTGetServices(dvcHandle, 50, struArrayGATTservice(0), struArrayServiceCount, 0)

        'get characteristics

        'get characteristic value

        'close session.

    End Function



    Public Function BlueSoleil_GATT_InitStruBytes_UUIDstru_FromString(ByRef inpByteArray() As Byte, ByVal inpUUID As String) As Boolean

        'convert hex string to structure of bytes.

        Dim sizeOfStru As Integer = 4 + 2 + 2 + 8
        ReDim inpByteArray(0 To sizeOfStru - 1)

        inpUUID = Replace(inpUUID, "-", "")
        If Len(inpUUID) <> 32 Then Return False

        Dim i As Integer
        Dim outByteIdx As Integer = 0

        For i = 1 To 32 Step 2

            inpByteArray(outByteIdx) = Convert.ToByte(Mid(inpUUID, i, 2), 16)
            outByteIdx = outByteIdx + 1
        Next i

        Return True

    End Function


    Private Function BlueSoleil_GATT_InitStruBytes_GATTUUIDStru_FromString(ByRef inpByteArray() As Byte, ByVal inpUUID As String, ByVal isShortUUID As Boolean) As Boolean

        Dim sizeOfStru As Integer = 4 + 2 + (4 + 2 + 2 + 8)
        ReDim inpByteArray(0 To sizeOfStru - 1)

        inpUUID = Replace(inpUUID, "-", "")
        If Len(inpUUID) <> 4 And Len(inpUUID) <> 32 Then Return False


        If isShortUUID = True Then
            'use short UUID only.
            inpByteArray(0) = BTSDK_TRUE

            Dim i As Integer
            Dim outByteIdx As Integer = 4
            For i = 1 To 4 Step 2
                inpByteArray(outByteIdx) = Convert.ToByte(Mid(inpUUID, i, 2), 16)
                outByteIdx = outByteIdx + 1
            Next i
        Else

            'populate long UUID info.
            inpByteArray(0) = BTSDK_FALSE

            Dim struLongUUID(0 To 0) As Byte
            BlueSoleil_GATT_InitStruBytes_UUIDstru_FromString(struLongUUID, inpUUID)
            Array.Copy(struLongUUID, 0, inpByteArray, 6, struLongUUID.Length)

        End If

        Return True

    End Function


    Private Function BlueSoleil_GATT_InitStruBytes_GATTserviceStru(ByRef inpByteArray() As Byte, ByVal inpUUID As String, ByVal hdlAttributes As UInt16) As Boolean


        Dim sizeOfStru As Integer = 2 + (4 + 2) + (4 + 2 + 2 + 8)
        ReDim inpByteArray(0 To sizeOfStru - 1)

        Dim struLongUUID(0 To 0) As Byte
        BlueSoleil_GATT_InitStruBytes_UUIDstru_FromString(struLongUUID, inpUUID)
        Array.Copy(struLongUUID, 0, inpByteArray, 0, struLongUUID.Length)

        Dim tempBytes(0 To 1) As Byte
        tempBytes = BitConverter.GetBytes(hdlAttributes)

        Dim outByteIndex As Integer = struLongUUID.Length
        Array.Copy(tempBytes, 0, inpByteArray, outByteIndex, tempBytes.Length)





        Return True

    End Function

End Module
