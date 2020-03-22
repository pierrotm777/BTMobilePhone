Public Class GenSet

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        lblIsMoving.Text = "Yes"
    End Sub
    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        lblIsMoving.Text = "No"
    End Sub

    Private Sub btnM1MPH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnM1MPH.Click
        If IsNumeric(tbSpeed.Text) Then
            Dim speed As Double = CDbl(tbSpeed.Text)
            If speed < -9 Then
                MsgBox("Speed is out of range. Needs to be more than -10.")
            Else
                tbSpeed.Text = (speed - 1).ToString
            End If
        Else
            MsgBox("Speed is not a number")
        End If
    End Sub
    Private Sub btnP1MPH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnP1MPH.Click
        If IsNumeric(tbSpeed.Text) Then
            Dim speed As Double = CDbl(tbSpeed.Text)
            tbSpeed.Text = (speed + 1).ToString
        Else
            MsgBox("Speed is not a number")
        End If
    End Sub

    Private Sub btnTurnLeft90_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTurnLeft90.Click
        If IsNumeric(tbTHeading.Text) Then
            Dim theading As Integer = CInt(tbTHeading.Text)
            theading = (theading + 270) Mod 360
            tbTHeading.Text = theading.ToString
            Simulator.LastTurnDirection = -1
        Else
            MsgBox("Heading is not numeric")
        End If
    End Sub
    Private Sub btnTurnRight90_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTurnRight90.Click
        If IsNumeric(tbTHeading.Text) Then
            Dim theading As Integer = CInt(tbTHeading.Text)
            theading = (theading + 90) Mod 360
            tbTHeading.Text = theading.ToString
            Simulator.LastTurnDirection = 1
        Else
            MsgBox("Heading is not numeric")
        End If
    End Sub

    Private Sub btnAutoTurn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutoTurn.Click
        If Simulator.AutoTurnEnabled Then 'Turn it off
            Simulator.AutoTurnEnabled = False
            btnAutoTurn.BackColor = Color.FromKnownColor(KnownColor.Control)
        Else 'Turn it on
            'Show a form to ask for time
            AutoTurnTimer.tbAutoTurnTimeDelay.Text = Simulator.AutoTurnStraightTarget.ToString
            AutoTurnTimer.tbAutoTurnTimeDelay.Focus()
            Dim result As Integer
            Dim DialogResult As Integer = AutoTurnTimer.ShowDialog()
            result = Convert.ToInt32(DialogResult)

            If result = 1 Then
                'AutoTurnStraightCounter = 0
                Simulator.AutoTurnEnabled = True
                btnAutoTurn.BackColor = Color.LightGreen
            End If
        End If
    End Sub

    Private Sub btnElevP5cm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElevP5cm.Click
        If IsNumeric(tbElevation.Text) Then
            Dim elev As Decimal = CDec(tbElevation.Text)
            elev += 0.05
            tbElevation.Text = elev.ToString
        End If
    End Sub
    Private Sub btnElevP1m_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElevP1m.Click
        If IsNumeric(tbElevation.Text) Then
            Dim elev As Decimal = CDec(tbElevation.Text)
            elev += 1
            tbElevation.Text = elev.ToString
        End If
    End Sub
    Private Sub btnElevM1m_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElevM1m.Click
        If IsNumeric(tbElevation.Text) Then
            Dim elev As Decimal = CDec(tbElevation.Text)
            elev -= 1
            tbElevation.Text = elev.ToString
        End If
    End Sub
    Private Sub btnElevM5cm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElevM5cm.Click
        If IsNumeric(tbElevation.Text) Then
            Dim elev As Decimal = CDec(tbElevation.Text)
            elev -= 0.05
            tbElevation.Text = elev.ToString
        End If
    End Sub
End Class
