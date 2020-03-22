Imports System.Runtime.InteropServices

Public Class Send
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    Private Structure SENDSTRUCT
        Dim var1 As Integer
        Dim str1 As String
    End Structure

    Dim r As receive
    Private WithEvents BS As New BuildString

    Private Sub Send_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Top = 100
        Me.Left = 100
        ShowReceive()
    End Sub

    Private Sub ShowReceive()
        If FindWindow(vbNullString, "Receive") <> 0 Then Exit Sub
        r = New receive
        r.Show()
        r.Top = Me.Top
        r.Left = Me.Left + Me.Width + 100
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' "Receive" parameter is the caption of destination window
        Dim hwnd As Integer = FindWindow(vbNullString, "Receive")
        If hwnd <> 0 And TextBox1.Text <> "" Then
            BS.PostString(hwnd, &H400, 0, TextBox1.Text)
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ShowReceive()
    End Sub

End Class
