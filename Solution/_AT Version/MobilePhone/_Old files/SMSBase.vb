Imports System.Text
Imports System.Collections.Generic

Namespace SMSPDULib

	Public Enum SMSDirection
		Received = 0
		Submited = 1
	End Enum

	Public Enum SMSType
		SMS = 0
		StatusReport = 1
	End Enum

	Public Enum ValidityPeriodFormat
		FieldNotPresent = 0
		Relative = &H10
		Enhanced = &H8
		Absolute = &H18
	End Enum

	Public MustInherit Class SMSBase
		#Region "Public Statics (Pops, Peeks, Gets)"

		Public Shared Function GetInvertBytes(source As String) As Byte()
			Dim bytes As Byte() = GetBytes(source)

			Array.Reverse(bytes)

			Return bytes
		End Function

		Public Shared Function GetBytes(source As String) As Byte()
			Return GetBytes(source, 16)
		End Function

		Public Shared Function GetBytes(source As String, fromBase As Integer) As Byte()
			Dim bytes As New List(Of Byte)()

			For i As Integer = 0 To source.Length / 2 - 1
				bytes.Add(Convert.ToByte(source.Substring(i * 2, 2), fromBase))
			Next

			Return bytes.ToArray()
		End Function

		Public Shared Function Decode7bit(source As String, length As Integer) As String
			Dim bytes As Byte() = GetInvertBytes(source)

			Dim binary As String = String.Empty

			For Each b As Byte In bytes
				binary += Convert.ToString(b, 2).PadLeft(8, "0"C)
			Next

			binary = binary.PadRight(length * 7, "0"C)

			Dim result As String = String.Empty

			For i As Integer = 1 To length
                result += CChar(CStr(Convert.ToByte(binary.Substring(binary.Length - i * 7, 7), 2)))
			Next

			Return result.Replace(ControlChars.NullChar, "@"C)
		End Function

		Public Shared Function Decode8bit(source As String, length As Integer) As String
			Dim bytes As Byte() = GetBytes(source.Substring(0, length * 2))

			'or ASCII?
			Return Encoding.UTF8.GetString(bytes)
		End Function


		Public Shared Function DecodeUCS2(source As String, length As Integer) As String
			Dim bytes As Byte() = GetBytes(source.Substring(0, length * 2))

			Return Encoding.BigEndianUnicode.GetString(bytes)
		End Function

		Public Shared Function EncodeUCS2(s As String) As Byte()
			Return Encoding.BigEndianUnicode.GetBytes(s)
		End Function

		Public Shared Function ReverseBits(source As String) As String
			Return ReverseBits(source, source.Length)
		End Function

		Public Shared Function ReverseBits(source As String, length As Integer) As String
			Dim result As String = String.Empty

			For i As Integer = 0 To length - 1
				result = result.Insert(If(i Mod 2 = 0, i, i - 1), source(i).ToString())
			Next

			Return result
		End Function

		Public Shared Function PeekByte(source As String) As Byte
			Return PeekByte(source, 0)
		End Function

		Public Shared Function PeekByte(source As String, byteIndex As Integer) As Byte
			Return Convert.ToByte(source.Substring(byteIndex * 2, 2), 16)
		End Function

		Public Shared Function PopByte(ByRef source As String) As Byte
			Dim b As Byte = Convert.ToByte(source.Substring(0, 2), 16)

			source = source.Substring(2)

			Return b
		End Function

		Public Shared Function PopBytes(ByRef source As String, length As Integer) As Byte()
			Dim bytes As String = source.Substring(0, length * 2)

			source = source.Substring(length * 2)

			Return GetBytes(bytes)
		End Function

		Public Shared Function PopDate(ByRef source As String) As DateTime
			Dim bytes As Byte() = GetBytes(ReverseBits(source.Substring(0, 12)), 10)

			source = source.Substring(14)

			Return New DateTime(2000 + bytes(0), bytes(1), bytes(2), bytes(3), bytes(4), bytes(5))
		End Function

		Public Shared Function PopPhoneNumber(ByRef source As String) As String
			Dim numberLength As Integer = PopByte(source)

			If (InlineAssignHelper(numberLength, numberLength + 2)) = 2 Then
				Return String.Empty
			End If

			Return PopAddress(source, numberLength + (numberLength Mod 2))
		End Function

		Public Shared Function PopServiceCenterAddress(ByRef source As String) As String
			Dim addressLength As Integer = PopByte(source)

			If (InlineAssignHelper(addressLength, addressLength * 2)) = 0 Then
				Return String.Empty
			End If

			Return PopAddress(source, addressLength)
		End Function

		Public Shared Function PopAddress(ByRef source As String, length As Integer) As String
			Dim address As String = source.Substring(0, length)

			source = source.Substring(address.Length)

			Dim addressType As Byte = PopByte(address)

			address = ReverseBits(address).Trim("F"C)

			If &H9 = addressType >> 4 Then
				address = Convert.ToString("+") & address
			End If

			Return address
		End Function

		Public Shared Function GetSMSType(source As String) As SMSType
			Dim scaLength As Byte = PeekByte(source)
			Dim pduType As Byte = PeekByte(source, scaLength + 1)

			Dim smsType As Byte = CByte((pduType And 3) >> 1)

			If Not [Enum].IsDefined(GetType(SMSType), CInt(smsType)) Then
				Throw New UnknownSMSTypeException(pduType)
			End If

			Return CType(smsType, SMSType)
		End Function

		Public Shared Sub Fetch(sms As SMSBase, ByRef source As String)
			sms._serviceCenterNumber = PopServiceCenterAddress(source)

			sms._pduType = PopByte(source)

			Dim bits As New System.Collections.BitArray(New Byte() {sms._pduType})

			sms.ReplyPathExists = bits(7)
			sms.UserDataStartsWithHeader = bits(6)
			sms.StatusReportIndication = bits(5)

			sms.ValidityPeriodFormat = CType(sms._pduType And &H18, ValidityPeriodFormat)
			sms.Direction = CType(sms._pduType And 1, SMSDirection)
		End Sub

		#End Region

		#Region "Public Statics (Push)"

		Public Shared Function EncodePhoneNumber(phoneNumber As String) As String
			Dim isInternational As Boolean = phoneNumber.StartsWith("+")

			If isInternational Then
				phoneNumber = phoneNumber.Remove(0, 1)
			End If

			Dim header As Integer = (phoneNumber.Length << 8) + &H81 Or (If(isInternational, &H10, &H20))

			If phoneNumber.Length Mod 2 = 1 Then
				phoneNumber = phoneNumber.PadRight(phoneNumber.Length + 1, "F"C)
			End If

			phoneNumber = ReverseBits(phoneNumber)

			Return Convert.ToString(header, 16).PadLeft(4, "0"C) & phoneNumber
		End Function

		#End Region

		#Region "Publics"

		Public Overridable Sub ComposePDUType()
			_pduType = CByte(Direction)
			_pduType = CByte(_pduType Or CInt(ValidityPeriodFormat))

			If StatusReportIndication Then
				_pduType = CByte(_pduType Or &H20)
			End If
		End Sub

		#End Region

		#Region "Members"
		'protected SMSType _type;
		Protected _pduType As Byte
		Protected _serviceCenterNumber As String

		#Region "PDU type (first octet) data"
		'Reply path. Parameter indicating that reply path exists.
		Protected _replyPathExists As Boolean
		'User data header indicator. This bit is set to 1 if the User Data field starts with a header. (EMS?)
		Protected _userDataStartsWithHeader As Boolean
		'Status report indication. This bit is set to 1 if a status report is going to be returned to the SME.
		Protected _statusReportIndication As Boolean
		'Validity Period Format
		Protected _validityPeriodFormat As ValidityPeriodFormat = ValidityPeriodFormat.FieldNotPresent
		'Message type indication
		'First bit
		Protected _direction As SMSDirection
		#End Region

		#End Region

		#Region "Proreties"
		Public Property ServiceCenterNumber() As String
			Get
				Return _serviceCenterNumber
			End Get
			Set
				_serviceCenterNumber = value
			End Set
		End Property

		Public Property ReplyPathExists() As Boolean
			Get
				Return _replyPathExists
			End Get
			Set
				_replyPathExists = value
			End Set
		End Property

		Public Property UserDataStartsWithHeader() As Boolean
			Get
				Return _userDataStartsWithHeader
			End Get
			Set
				_userDataStartsWithHeader = value
			End Set
		End Property

		Public Property StatusReportIndication() As Boolean
			Get
				Return _statusReportIndication
			End Get
			Set
				_statusReportIndication = value
			End Set
		End Property

		Public Property ValidityPeriodFormat() As ValidityPeriodFormat
			Get
				Return _validityPeriodFormat
			End Get
			Set
				_validityPeriodFormat = value
			End Set
		End Property

		Public Property Direction() As SMSDirection
			Get
				Return _direction
			End Get
			Set
				_direction = value
			End Set
		End Property

		Public MustOverride ReadOnly Property Type() As SMSType
		Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
			target = value
			Return value
		End Function
		#End Region

	End Class

End Namespace