Imports System
Imports System.IO


Namespace SMS

    ' <summary>
    ' Encapsulates an SMS WAP Push message
    ' </summary>
    Public Class PushMessage
        ' Ports for the WDP information element, instructing the handset which 
        ' application to load on receving the message
        Protected Shared WDP_DESTINATIONPORT() As Byte = {&HB, &H84}
        Protected Shared WDP_SOURCEPORT() As Byte = {&H23, &HF0}

        Private serviceIndication As ServiceIndication


        Public Sub New(ByVal href As String, ByVal text As String)
            Me.serviceIndication = New ServiceIndication(href, text, ServiceIndicationAction.signal_high)
        End Sub 'New


        ' <summary>
        ' Generates the body of the SMS message
        ' </summary>
        ' <returns>byte array</returns>
        Public Function GetSMSBytes() As Byte()
            Dim stream As New MemoryStream()
            Dim wdpHeader As Byte() = GetWDPHeaderBytes()
            stream.Write(wdpHeader, 0, wdpHeader.Length)

            Dim pdu As Byte() = GetPDUBytes()
            stream.Write(pdu, 0, pdu.Length)

            Return stream.ToArray()
        End Function 'GetSMSBytes

        ' <summary>
        ' Generates the PDU (Protocol Data Unit) comprising the encoded Service Indication
        ' and the WSP (Wireless Session Protocol) headers
        ' </summary>
        ' <returns>byte array comprising the PDU</returns>
        Public Function GetPDUBytes() As Byte()
            Dim body As Byte() = serviceIndication.GetWBXMLBytes()
            Dim headerBuffer As Byte() = GetWSPHeaderBytes(CByte(body.Length))
            Dim stream As New MemoryStream()
            stream.Write(headerBuffer, 0, headerBuffer.Length)
            stream.Write(body, 0, body.Length)
            Return stream.ToArray()
        End Function 'GetPDUBytes

        ' <summary>
        ' Generates the WSP (Wireless Session Protocol) headers with the well known
        ' byte values specfic to a Service Indication
        ' </summary>
        ' <param name="contentLength">the length of the encoded Service Indication</param>
        ' <returns>byte array comprising the headers</returns>
        Public Function GetWSPHeaderBytes(ByVal contentLength As Byte) As Byte()
            Dim stream As New MemoryStream()

            stream.WriteByte(WSP.TRANSACTIONID_CONNECTIONLESSWSP)
            stream.WriteByte(WSP.PDUTYPE_PUSH)

            Dim headersStream As New MemoryStream()
            headersStream.Write(WSP.HEADER_CONTENTTYPE_application_vnd_wap_sic_utf_8, 0, WSP.HEADER_CONTENTTYPE_application_vnd_wap_sic_utf_8.Length)

            headersStream.WriteByte(WSP.HEADER_APPLICATIONTYPE)
            headersStream.WriteByte(WSP.HEADER_APPLICATIONTYPE_x_wap_application_id_w2)

            '			headersStream.WriteByte(WSP.HEADER_CONTENTLENGTH);
            '			headersStream.WriteByte((byte)(contentLength + 128));
            '
            headersStream.Write(WSP.HEADER_PUSHFLAG, 0, WSP.HEADER_PUSHFLAG.Length)

            stream.WriteByte(CByte(headersStream.Length))

            headersStream.WriteTo(stream)

            Return stream.ToArray()
        End Function 'GetWSPHeaderBytes


        ' <summary>
        ' Generates the WDP (Wireless Datagram Protocol) or UDH (User Data Header) for the 
        ' SMS message. In the case comprising the Application Port information element
        ' indicating to the handset which application to start on receipt of the message
        ' </summary>
        ' <returns>byte array comprising the header</returns>
        Public Function GetWDPHeaderBytes() As Byte()
            Dim stream As New MemoryStream()
            stream.WriteByte(WDP.INFORMATIONELEMENT_IDENTIFIER_APPLICATIONPORT)
            stream.WriteByte(CByte(WDP_DESTINATIONPORT.Length + WDP_SOURCEPORT.Length))
            stream.Write(WDP_DESTINATIONPORT, 0, WDP_DESTINATIONPORT.Length)
            stream.Write(WDP_SOURCEPORT, 0, WDP_SOURCEPORT.Length)

            Dim headerStream As New MemoryStream()

            ' write length of header
            headerStream.WriteByte(CByte(stream.Length))

            stream.WriteTo(headerStream)
            Return headerStream.ToArray()
        End Function 'GetWDPHeaderBytes
    End Class 'PushMessage
End Namespace