Imports System.Text.RegularExpressions

Public Class contact
    Private sName As String
    Private nID As UInt32
    Private sNumber As String
    Dim regex As Regex
    Dim cc As Byte ' Country Code

    Public Sub New(ByVal i As UInt32, ByVal n As String, ByVal num As String)
        cc = 61
        regex = New Regex("^\+" & cc & "(\d+)")
        sName = n.Trim()
        nID = i
        sNumber = regex.Replace(num.Trim(), "0$1", 1)
    End Sub

    Public Function equalTo(ByRef c As contact) As Boolean
        If c.ID = nID Then
            equalTo = True
        Else
            equalTo = False
        End If
    End Function

    Public Function IsNumber(ByRef num As String) As Boolean
        Return (regex.Replace(num, "0$1", 1) = sNumber)
    End Function

    ReadOnly Property Name() As String
        Get
            Return sName
        End Get
    End Property

    ReadOnly Property Number() As String
        Get
            Return sNumber
        End Get
    End Property

    ReadOnly Property ID() As UInt32
        Get
            Return nID
        End Get
    End Property

End Class
