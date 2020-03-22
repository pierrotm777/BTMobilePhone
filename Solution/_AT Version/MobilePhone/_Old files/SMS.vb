Imports System.Collections.Generic
Imports System.Text

Namespace SMSPDULib
	Public Class SMS
		Inherits SMSBase
		#Region "Members"
		Protected _moreMessagesToSend As Boolean
		Protected _rejectDuplicates As Boolean
		Protected _messageReference As Byte
		Protected _phoneNumber As String
		Protected _protocolIdentifier As Byte
		Protected _dataCodingScheme As Byte
		Protected _validityPeriod As Byte
		Protected _serviceCenterTimeStamp As DateTime
		Protected _userData As String
		Protected _userDataHeader As Byte()
		Protected _message As String
		#End Region

		#Region "Properties"
		Public ReadOnly Property ServiceCenterTimeStamp() As DateTime
			Get
				Return _serviceCenterTimeStamp
			End Get
		End Property

		Public Property MessageReference() As Byte
			Get
				Return _messageReference
			End Get
			Set
				_messageReference = value
			End Set
		End Property

		Public Property PhoneNumber() As String
			Get
				Return _phoneNumber
			End Get
			Set
				_phoneNumber = value
			End Set
		End Property

		Public Property Message() As String
			Get
				Return _message
			End Get
			Set
				If value.Length > 70 Then
					Throw New ArgumentOutOfRangeException("Message.Length", value.Length, "Message length can not be greater that 70 chars.")
				End If

				_message = value
			End Set
		End Property

		Public Property RejectDuplicates() As Boolean
			Get
				If Direction = SMSDirection.Received Then
					Throw New InvalidOperationException("Received message can not contains 'reject duplicates' property")
				End If

				Return _rejectDuplicates
			End Get
			Set
				If Direction = SMSDirection.Received Then
					Throw New InvalidOperationException("Received message can not contains 'reject duplicates' property")
				End If

				_rejectDuplicates = value
			End Set
		End Property

		Public Property MoreMessagesToSend() As Boolean
			Get
				If Direction = SMSDirection.Received Then
					Throw New InvalidOperationException("Submited message can not contains 'more message to send' property")
				End If

				Return _moreMessagesToSend
			End Get
			Set
				If Direction = SMSDirection.Received Then
					Throw New InvalidOperationException("Submited message can not contains 'more message to send' property")
				End If

				_moreMessagesToSend = value
			End Set
		End Property

		Public Property ValidityPeriod() As TimeSpan
			Get
				If _validityPeriod > 196 Then
					Return New TimeSpan((_validityPeriod - 192) * 7, 0, 0, 0)
				End If

				If _validityPeriod > 167 Then
					Return New TimeSpan((_validityPeriod - 166), 0, 0, 0)
				End If

				If _validityPeriod > 143 Then
					Return New TimeSpan(12, (_validityPeriod - 143) * 30, 0)
				End If

				Return New TimeSpan(0, (_validityPeriod + 1) * 5, 0)
			End Get
			Set
				If value.Days > 441 Then
					Throw New ArgumentOutOfRangeException("TimeSpan.Days", value.Days, "Value must be not greater 441 days.")
				End If

				If value.Days > 30 Then
					'Up to 441 days
					_validityPeriod = CByte(192 + CInt(value.Days / 7))
				ElseIf value.Days > 1 Then
					'Up to 30 days
					_validityPeriod = CByte(166 + value.Days)
				ElseIf value.Hours > 12 Then
					'Up to 24 hours
					_validityPeriod = CByte(143 + (value.Hours - 12) * 2 + value.Minutes / 30)
				ElseIf value.Hours > 1 OrElse value.Minutes > 1 Then
					'Up to 12 days
					_validityPeriod = CByte(value.Hours * 12 + value.Minutes / 5 - 1)
				Else
					_validityPeriodFormat = ValidityPeriodFormat.FieldNotPresent

					Return
				End If

				_validityPeriodFormat = ValidityPeriodFormat.Relative
			End Set
		End Property

		Public Overridable ReadOnly Property UserDataHeader() As Byte()
			Get
				Return _userDataHeader
			End Get
		End Property

#Region "in parts" 'message properties
        Public ReadOnly Property InParts() As Boolean
            Get
                If _userDataHeader Is Nothing OrElse _userDataHeader.Length < 5 Then
                    Return False
                End If

                ' | 08 04 00 | 9F 02 | i have this header from siemenes in "in parts" message
                Return (_userDataHeader(0) = &H0 AndAlso _userDataHeader(1) = &H3)
            End Get
        End Property

        Public ReadOnly Property InPartsID() As Integer
            Get
                If Not InParts Then
                    Return 0
                End If

                Return (_userDataHeader(2) << 8) + _userDataHeader(3)
            End Get
        End Property

        Public ReadOnly Property Part() As Integer
            Get
                If Not InParts Then
                    Return 0
                End If

                Return _userDataHeader(4)
            End Get
        End Property
#End Region

		Public Overrides ReadOnly Property Type() As SMSType
			Get
				Return SMSType.SMS
			End Get
		End Property
		#End Region

		#Region "Public Statics"
		Public Shared Sub Fetch(sms As SMS, ByRef source As String)
			SMSBase.Fetch(sms, source)

			If sms._direction = SMSDirection.Submited Then
				sms._messageReference = PopByte(source)
			End If

			sms._phoneNumber = PopPhoneNumber(source)
			sms._protocolIdentifier = PopByte(source)
			sms._dataCodingScheme = PopByte(source)

			If sms._direction = SMSDirection.Submited Then
				sms._validityPeriod = PopByte(source)
			End If

			If sms._direction = SMSDirection.Received Then
				sms._serviceCenterTimeStamp = PopDate(source)
			End If

			sms._userData = source

			If source = String.Empty Then
				Return
			End If

			Dim userDataLength As Integer = PopByte(source)

			If userDataLength = 0 Then
				Return
			End If

			If sms._userDataStartsWithHeader Then
				Dim userDataHeaderLength As Byte = PopByte(source)

				sms._userDataHeader = PopBytes(source, userDataHeaderLength)

				userDataLength -= userDataHeaderLength + 1
			End If

			If userDataLength = 0 Then
				Return
			End If

			Select Case CType(sms._dataCodingScheme, SMSEncoding) And SMSEncoding.ReservedMask
				Case SMSEncoding._7bit
					sms._message = Decode7bit(source, userDataLength)
					Exit Select
				Case SMSEncoding._8bit
					sms._message = Decode8bit(source, userDataLength)
					Exit Select
				Case SMSEncoding.UCS2
					sms._message = DecodeUCS2(source, userDataLength)
					Exit Select
			End Select
		End Sub
		#End Region

		#Region "Publics"

		Public Overrides Sub ComposePDUType()
			MyBase.ComposePDUType()

			If _moreMessagesToSend OrElse _rejectDuplicates Then
				_pduType = CByte(_pduType Or &H4)
			End If
		End Sub

		Public Overridable Function Compose(messageEncoding As SMSEncoding) As String
			ComposePDUType()

			Dim encodedData As String = "00"
			'Length of SMSC information. Here the length is 0, which means that the SMSC stored in the phone should be used. Note: This octet is optional. On some phones this octet should be omitted! (Using the SMSC stored in phone is thus implicit)
			encodedData += Convert.ToString(_pduType, 16).PadLeft(2, "0"C)
			'PDU type (forst octet)
			encodedData += Convert.ToString(MessageReference, 16).PadLeft(2, "0"C)
			encodedData += EncodePhoneNumber(PhoneNumber)
			encodedData += "00"
			'Protocol identifier (Short Message Type 0)
			encodedData += Convert.ToString(CInt(messageEncoding), 16).PadLeft(2, "0"C)
			'Data coding scheme
			If _validityPeriodFormat <> ValidityPeriodFormat.FieldNotPresent Then
				encodedData += Convert.ToString(_validityPeriod, 16).PadLeft(2, "0"C)
			End If
			'Validity Period

			Dim messageBytes As Byte() = Nothing

			Select Case messageEncoding
				Case SMSEncoding.UCS2
					messageBytes = EncodeUCS2(_message)
					Exit Select
				Case Else
					messageBytes = New Byte(-1) {}
					Exit Select
			End Select

			encodedData += Convert.ToString(messageBytes.Length, 16).PadLeft(2, "0"C)
			'Length of message
			For Each b As Byte In messageBytes
				encodedData += Convert.ToString(b, 16).PadLeft(2, "0"C)
			Next

			Return encodedData.ToUpper()
		End Function

		#End Region

		Public Enum SMSEncoding
			ReservedMask = &Hc
			'1100
			_7bit = 0
			_8bit = &H4
			'0100
			UCS2 = &H8
			'1000
		End Enum
	End Class
End Namespace