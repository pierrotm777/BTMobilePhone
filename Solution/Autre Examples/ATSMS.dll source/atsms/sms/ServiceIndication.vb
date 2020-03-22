Imports System
Imports System.Collections
Imports System.IO
Imports System.Text


Namespace SMS

    ' Allowed values of the action attribute of the indication tag
    Public Enum ServiceIndicationAction
        NotSet
        signal_none
        signal_low
        signal_medium
        signal_high
        delete
    End Enum 'ServiceIndicationAction

    ' <summary>
    ' Encapsulates the Service Indication WAP Push instruction.
    ' Full documentation can be found at http://www.openmobilealliance.org/tech/affiliates/wap/wap-167-serviceind-20010731-a.pdf?doc=wap-167-serviceind-20010731-a.pdf
    ' </summary>
    Public Class ServiceIndication
        ' Well known DTD token
        Public Const DOCUMENT_DTD_ServiceIndication As Byte = &H5 ' ServiceIndication 1.0 Public Identifier

        ' Tag Tokens
        Public Const TAGTOKEN_si As Byte = &H5
        Public Const TAGTOKEN_indication As Byte = &H6
        Public Const TAGTOKEN_info As Byte = &H7
        Public Const TAGTOKEN_item As Byte = &H8

        ' Attribute Tokens
        Public Const ATTRIBUTESTARTTOKEN_action_signal_none As Byte = &H5
        Public Const ATTRIBUTESTARTTOKEN_action_signal_low As Byte = &H6
        Public Const ATTRIBUTESTARTTOKEN_action_signal_medium As Byte = &H7
        Public Const ATTRIBUTESTARTTOKEN_action_signal_high As Byte = &H8
        Public Const ATTRIBUTESTARTTOKEN_action_signal_delete As Byte = &H9
        Public Const ATTRIBUTESTARTTOKEN_created As Byte = &HA
        Public Const ATTRIBUTESTARTTOKEN_href As Byte = &HB
        Public Const ATTRIBUTESTARTTOKEN_href_http As Byte = &HC ' http://
        Public Const ATTRIBUTESTARTTOKEN_href_http_www As Byte = &HD ' http://www.
        Public Const ATTRIBUTESTARTTOKEN_href_https As Byte = &HE ' https://
        Public Const ATTRIBUTESTARTTOKEN_href_https_www As Byte = &HF ' https://www.
        Public Const ATTRIBUTESTARTTOKEN_si_expires As Byte = &H10
        Public Const ATTRIBUTESTARTTOKEN_si_id As Byte = &H11
        Public Const ATTRIBUTESTARTTOKEN_class As Byte = &H12

        ' Attribute Value Tokens
        Public Const ATTRIBUTEVALUETOKEN_com As Byte = &H85 ' .com/
        Public Const ATTRIBUTEVALUETOKEN_edu As Byte = &H86 ' .edu/
        Public Const ATTRIBUTEVALUETOKEN_net As Byte = &H87 ' .net/
        Public Const ATTRIBUTEVALUETOKEN_org As Byte = &H88 ' .org/
        Private Shared hrefStartTokens As Hashtable
        Private Shared attributeValueTokens As Hashtable

        Public Href As String
        Public text As String
        Public CreatedAt As DateTime
        Public ExpiresAt As DateTime
        Public Action As ServiceIndicationAction


        Shared Sub New()
            hrefStartTokens = New Hashtable()
            hrefStartTokens.Add("https://www.", ATTRIBUTESTARTTOKEN_href_https_www)
            hrefStartTokens.Add("http://www.", ATTRIBUTESTARTTOKEN_href_http_www)
            hrefStartTokens.Add("https://", ATTRIBUTESTARTTOKEN_href_https)
            hrefStartTokens.Add("http://", ATTRIBUTESTARTTOKEN_href_http)

            attributeValueTokens = New Hashtable()
            attributeValueTokens.Add(".com/", ATTRIBUTEVALUETOKEN_com)
            attributeValueTokens.Add(".edu/", ATTRIBUTEVALUETOKEN_edu)
            attributeValueTokens.Add(".net/", ATTRIBUTEVALUETOKEN_net)
            attributeValueTokens.Add(".org/", ATTRIBUTEVALUETOKEN_org)
        End Sub 'New


        Public Sub New(ByVal href As String, ByVal text As String, ByVal action As ServiceIndicationAction)
            Me.Href = href
            Me.Text = text
            Me.Action = action
        End Sub 'New


        Public Sub New(ByVal href As String, ByVal text As String, ByVal createdAt As DateTime, ByVal expiresAt As DateTime)
            MyClass.New(href, text, ServiceIndicationAction.NotSet)
            Me.CreatedAt = createdAt
            Me.ExpiresAt = expiresAt
        End Sub 'New


        Public Sub New(ByVal href As String, ByVal text As String, ByVal createdAt As DateTime, ByVal expiresAt As DateTime, ByVal action As ServiceIndicationAction)
            MyClass.New(href, text, action)
            Me.CreatedAt = createdAt
            Me.ExpiresAt = expiresAt
        End Sub 'New


        ' <summary>
        ' Generates a byte array comprising the encoded Service Indication
        ' </summary>
        ' <returns>The encoded body</returns>
        Public Function GetWBXMLBytes() As Byte()
            Dim stream As New MemoryStream()

            ' wbxml headers
            stream.WriteByte(WBXML.VERSION_1_1)
            stream.WriteByte(DOCUMENT_DTD_ServiceIndication)
            stream.WriteByte(WBXML.CHARSET_UTF_8)
            stream.WriteByte(WBXML.NULL)

            ' start xml doc
            stream.WriteByte(WBXML.SetTagTokenIndications(TAGTOKEN_si, False, True))
            stream.WriteByte(WBXML.SetTagTokenIndications(TAGTOKEN_indication, True, True))

            ' href attribute
            ' this attribute has some well known start tokens that 
            ' are contained within a static hashtable. Iterate through
            ' the table and chose the token.
            Dim i As Integer = 0
            Dim hrefTagToken As Byte = ATTRIBUTESTARTTOKEN_href
            Dim startString As String
            For Each startString In hrefStartTokens.Keys
                If Me.Href.StartsWith(startString) Then
                    hrefTagToken = CByte(hrefStartTokens(startString))
                    i = startString.Length
                    Exit For
                End If
            Next startString
            stream.WriteByte(hrefTagToken)

            WriteInlineString(stream, Me.Href.Substring(i))
            '	
            '
            ' Date elements removed as does not seem to be supported
            '
            ' by all handsets, or I'm doing it incorrectly, or it's a version 1.2
            '
            ' thing. 
            '

            '
            ' created attribute
            '
            stream.WriteByte(ATTRIBUTESTARTTOKEN_created)
            '
            WriteDate(stream, Me.CreatedAt)
            '

            '
            ' si-expires attrbute
            '
            stream.WriteByte(ATTRIBUTESTARTTOKEN_si_expires)
            '
            WriteDate(stream, Me.ExpiresAt)
            '

            ' action attibute
            If Me.Action <> ServiceIndicationAction.NotSet Then
                stream.WriteByte(GetActionToken(Me.Action))
            End If
            ' close indication element attributes
            stream.WriteByte(WBXML.TAGTOKEN_END)

            ' text of indication element
            WriteInlineString(stream, Me.Text)

            ' close indication element
            stream.WriteByte(WBXML.TAGTOKEN_END)
            ' close si element
            stream.WriteByte(WBXML.TAGTOKEN_END)

            Return stream.ToArray()
        End Function 'GetWBXMLBytes


        ' <summary>
        ' Gets the token for the action attribute
        ' </summary>
        ' <param name="action">Interruption level instruction to the handset</param>
        ' <returns>well known byte value for the action attribute</returns>
        Protected Function GetActionToken(ByVal action As ServiceIndicationAction) As Byte
            Select Case action
                Case ServiceIndicationAction.delete
                    Return ATTRIBUTESTARTTOKEN_action_signal_delete
                Case ServiceIndicationAction.signal_high
                    Return ATTRIBUTESTARTTOKEN_action_signal_high
                Case ServiceIndicationAction.signal_low
                    Return ATTRIBUTESTARTTOKEN_action_signal_low
                Case ServiceIndicationAction.signal_medium
                    Return ATTRIBUTESTARTTOKEN_action_signal_medium
                Case Else
                    Return ATTRIBUTESTARTTOKEN_action_signal_none
            End Select
        End Function 'GetActionToken


        ' <summary>
        ' Encodes an inline string into the stream using UTF8 encoding
        ' </summary>
        ' <param name="stream">The target stream</param>
        ' <param name="text">The text to write</param>
        Protected Sub WriteInlineString(ByVal stream As MemoryStream, ByVal text As String)
            ' indicate that the follow bytes comprise a string
            stream.WriteByte(WBXML.TOKEN_INLINE_STRING_FOLLOWS)

            ' write character bytes
            Dim buffer As Byte() = Encoding.UTF8.GetBytes(text)
            stream.Write(buffer, 0, buffer.Length)

            ' end is indicated by a null byte
            stream.WriteByte(WBXML.NULL)
        End Sub 'WriteInlineString

        ' <summary>
        ' Encodes the DateTime to the stream.
        ' DateTimes are encoded as Opaque Data with each number in the string represented
        ' by its 4-bit binary value
        ' eg: 1999-04-30 06:40:00
        ' is encoded as 199904300640.
        ' Trailing zero values are not included.
        ' </summary>
        ' <param name="stream">Target stream</param>
        ' <param name="date">DateTime to encode</param>
        Protected Sub WriteDate(ByVal stream As MemoryStream, ByVal [date] As DateTime)
            Dim buffer(7) As Byte

            buffer(0) = CByte([date].Year / 100)
            buffer(1) = CByte([date].Year Mod 100)
            buffer(2) = CByte([date].Month)
            buffer(3) = CByte([date].Day)

            Dim dateLength As Integer = 4

            If [date].Hour > 0 Then
                buffer(4) = CByte([date].Hour)
                dateLength = 5
            End If

            If [date].Minute > 0 Then
                buffer(5) = CByte([date].Minute)
                dateLength = 6
            End If

            If [date].Second > 0 Then
                buffer(6) = CByte([date].Second)
                dateLength = 7
            End If

            ' write to stream
            stream.WriteByte(WBXML.TOKEN_OPAQUEDATA_FOLLOWS)
            stream.WriteByte(CByte(dateLength))
            stream.Write(buffer, 0, dateLength)
        End Sub 'WriteDate
    End Class 'ServiceIndication 
End Namespace 'com.esendex.articles.wappush