Imports System.Windows.Forms

Public Class AutoTurnTimer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'Confirm text box is numeric
        If IsNumeric(tbAutoTurnTimeDelay.Text) Then
            If CInt(tbAutoTurnTimeDelay.Text) > 0 Then
                Simulator.AutoTurnStraightTarget = CInt(tbAutoTurnTimeDelay.Text)

                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                MsgBox("Time delay between turn has to be a positive number.")
                tbAutoTurnTimeDelay.Focus()
            End If
        Else
            MsgBox("Time delay between turn has to be numeric.")
            tbAutoTurnTimeDelay.Focus()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
