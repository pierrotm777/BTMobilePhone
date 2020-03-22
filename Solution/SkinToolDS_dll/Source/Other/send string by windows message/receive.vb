Public Class receive
    Private WithEvents BS As New BuildString

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case &H400
                BS.BuildString(m.LParam)
            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub

    Private Sub SB_StringOK(ByVal Result As String) Handles BS.StringOK
        TextBox1.AppendText(Result & vbNewLine)
    End Sub

End Class