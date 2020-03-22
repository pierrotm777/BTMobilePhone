Imports System.Net.Mail
Public Class Form1
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        smtpServer.Credentials = New Net.NetworkCredential(TextBox1.Text & "@gmail.com", TextBox2.Text)
        smtpServer.Port = 587
        smtpServer.Host = "smtp.gmail.com"
        smtpServer.EnableSsl = True
        mail.From = New MailAddress(TextBox1.Text & "@gmail.com")
        If RadioButton1.Checked = True Then
            mail.To.Add("91" & TextBox3.Text & "@m3m.in")
        ElseIf RadioButton2.Checked = True Then
            mail.To.Add(TextBox3.Text)
        End If
        mail.Subject = TextBox4.Text
        mail.Body = TextBox5.Text()
        smtpServer.Send(mail)
        MsgBox("mail is sent", MsgBoxStyle.OkOnly, "Report")
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        TextBox3.Enabled = True
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        TextBox3.Enabled = True
    End Sub

    
End Class
