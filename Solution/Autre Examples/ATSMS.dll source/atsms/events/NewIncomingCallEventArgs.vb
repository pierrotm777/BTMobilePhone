Public Class NewIncomingCallEventArgs
    Private strCallerID As String

    Public Property CallerID() As String
        Get
            Return Me.strCallerID
        End Get
        Set(ByVal value As String)
            Me.strCallerID = value
        End Set
    End Property


End Class
