Imports System.Collections.Generic

Public Class phoneSms
    Private phoneSmsData As List(Of contact)

    Public Sub New()
        phoneSmsData = New List(Of contact)
    End Sub

    Public Sub add(ByVal c As contact)
        If Not phoneSmsData.Exists(Function(x) c.equalTo(x)) Then
            phoneSmsData.Add(c)
        End If
    End Sub

    ReadOnly Property List() As List(Of contact)
        Get
            Return phoneSmsData
        End Get
    End Property


    Public Function getName(ByVal number As String) As List(Of contact)
        getName = phoneSmsData.FindAll(Function(x) x.IsNumber(number))
    End Function
End Class
