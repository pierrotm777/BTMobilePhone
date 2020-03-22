Option Strict Off
Option Explicit On
Friend Class frmKey
	Inherits System.Windows.Forms.Form
	Private Sub frmKey_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Dim KeyN As Integer
		
		Debug.Print(KeyCode)
		
		If KeyCode >= 16 And KeyCode <= 18 Then Exit Sub
		
		KeyN = 1000 * Shift + KeyCode
		
		lblKey.Text = CStr(KeyN)
		
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            'cancel the close
            e.Cancel = True
            Me.Hide()
        End If
    End Sub
End Class