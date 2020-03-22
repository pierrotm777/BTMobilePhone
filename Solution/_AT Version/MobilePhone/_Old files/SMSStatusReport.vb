Namespace SMSPDULib

	Public Enum ReportStatus
		NoResponseFromSME = &H62
		NotSend = &H60
		Success = 0
	End Enum

	Public Class SMSStatusReport
		Inherits SMSBase
		#Region "Members"
		Protected _reportTimeStamp As DateTime
		Protected _reportStatus As ReportStatus
		Protected _messageReference As Byte
		Protected _phoneNumber As String
		Protected _serviceCenterTimeStamp As DateTime
		#End Region

		#Region "Public Properties"
		Public ReadOnly Property MessageReference() As Byte
			Get
				Return _messageReference
			End Get
		End Property
		Public ReadOnly Property PhoneNumber() As String
			Get
				Return _phoneNumber
			End Get
		End Property
		Public ReadOnly Property ServiceCenterTimeStamp() As DateTime
			Get
				Return _serviceCenterTimeStamp
			End Get
		End Property
		Public ReadOnly Property ReportTimeStamp() As DateTime
			Get
				Return _reportTimeStamp
			End Get
		End Property
		Public ReadOnly Property ReportStatus() As ReportStatus
			Get
				Return _reportStatus
			End Get
		End Property

		Public Overrides ReadOnly Property Type() As SMSType
			Get
				Return SMSType.StatusReport
			End Get
		End Property
		#End Region

		#Region "Public Statics"
		Public Shared Sub Fetch(statusReport As SMSStatusReport, ByRef source As String)
			SMSBase.Fetch(statusReport, source)

			statusReport._messageReference = PopByte(source)
			statusReport._phoneNumber = PopPhoneNumber(source)
			statusReport._serviceCenterTimeStamp = PopDate(source)
			statusReport._reportTimeStamp = PopDate(source)
			statusReport._reportStatus = CType(PopByte(source), ReportStatus)
		End Sub
		#End Region
	End Class

End Namespace