

Namespace SMS


    Public Class SMSMessage

#Region "Private Members"

        Private iIndex As Integer
        Private strPhoneNumber As String
        Private strSMSC As String
        Private strText As String
        Private dtTimestamp As DateTime
        Private strTimeStampRFC As String
        Private oGSMModem As GSMModem


#End Region

#Region "Public Properties"

        Public Property Index() As Integer
            Get
                Return Me.iIndex
            End Get
            Friend Set(ByVal value As Integer)
                Me.iIndex = value
            End Set
        End Property

        Public Property PhoneNumber() As String
            Get
                Return Me.strPhoneNumber
            End Get
            Friend Set(ByVal value As String)
                Me.strPhoneNumber = value
            End Set
        End Property

        Public Property SMSC() As String
            Get
                Return Me.strSMSC
            End Get
            Friend Set(ByVal value As String)
                Me.strSMSC = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return Me.strText
            End Get
            Friend Set(ByVal value As String)
                Me.strText = value
            End Set
        End Property

        Public Property Timestamp() As DateTime
            Get
                Return Me.dtTimestamp
            End Get
            Friend Set(ByVal value As DateTime)
                Me.dtTimestamp = value
            End Set
        End Property

        Public Property TimestampRFC() As String
            Get
                Return Me.strTimeStampRFC
            End Get
            Friend Set(ByVal value As String)
                Me.strTimeStampRFC = value
            End Set
        End Property

        Public Property Modem() As GSMModem
            Get
                Return Me.oGSMModem
            End Get
            Set(ByVal value As GSMModem)
                Me.oGSMModem = value
            End Set
        End Property

#End Region

#Region "Public Procedures"

        Public Sub Delete()
            If Not oGSMModem Is Nothing And oGSMModem.IsConnected Then
                oGSMModem.DeleteSMS(Me.Index)
            End If

        End Sub

#End Region
    End Class

End Namespace
