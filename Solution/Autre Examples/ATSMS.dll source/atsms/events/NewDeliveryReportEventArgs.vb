Public Class NewDeliveryReportEventArgs

    Private dtmDeliveryTimeStamp As DateTime
    Private iMessageReference As Integer
    Private strPhone As String
    Private dtmSentTimeStamp As DateTime
    Private blnStatus As Boolean


    Public Property DeliveryDateTime() As DateTime
        Get
            Return Me.dtmDeliveryTimeStamp
        End Get
        Set(ByVal Value As DateTime)
            Me.dtmDeliveryTimeStamp = Value
        End Set
    End Property

    Public Property MessageReference() As Integer
        Get
            Return Me.iMessageReference
        End Get
        Set(ByVal Value As Integer)
            Me.iMessageReference = Value
        End Set
    End Property

    Public Property Phone() As String
        Get
            Return Me.strPhone
        End Get
        Set(ByVal Value As String)
            Me.strPhone = Value
        End Set
    End Property

    Public Property SentTimeStamp() As DateTime
        Get
            Return Me.dtmSentTimeStamp
        End Get
        Set(ByVal Value As DateTime)
            Me.dtmSentTimeStamp = Value
        End Set
    End Property

    Public Property Status() As Boolean
        Get
            Return Me.blnStatus
        End Get
        Set(ByVal Value As Boolean)
            Me.blnStatus = Value
        End Set
    End Property

End Class
