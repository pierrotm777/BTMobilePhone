
Namespace SMS

    ''' <summary>
    ''' Message outbox
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Outbox : Inherits Queue

        'Protected msgRefNo As Integer

#Region "Constructor"
        Public Sub New()
            MyBase.New()
            'msgRefNo = 0
        End Sub
#End Region

#Region "Public Functions"

        Public Function Add(ByRef msg As Message) As String
            'msgRefNo += 1
            Dim r As New Random()
            Dim msgRefNo As Integer
            msgRefNo = r.Next(10000, 60000)
            msg.ReferenceNo = msgRefNo
            Enqueue(msg)
            Return msgRefNo.ToString
        End Function

#End Region

    End Class

End Namespace