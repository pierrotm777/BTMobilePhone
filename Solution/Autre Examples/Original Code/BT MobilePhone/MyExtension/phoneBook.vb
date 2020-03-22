Imports System.Collections.Generic

Public Class phoneBook
    Private phoneBookData As List(Of contact)

    Public Sub New()
        phoneBookData = New List(Of contact)
    End Sub

    Public Sub add(ByVal c As contact)
        If Not phoneBookData.Exists(Function(x) c.equalTo(x)) Then
            phoneBookData.Add(c)
        End If
    End Sub

    ReadOnly Property List() As List(Of contact)
        Get
            Return phoneBookData
        End Get
    End Property


    Public Function getName(ByVal number As String) As List(Of contact)
        getName = phoneBookData.FindAll(Function(x) x.IsNumber(number))
    End Function
End Class
