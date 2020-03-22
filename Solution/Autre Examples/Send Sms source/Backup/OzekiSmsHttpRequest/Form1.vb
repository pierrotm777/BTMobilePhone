Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web



Public Class fMain

    Private Sub bSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bSend.Click
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse = Nothing
        Dim url As String
        Dim username As String = "admin"
        Dim password As String = "abc123"
        Dim host As String = "http://127.0.0.1:9501"
        Dim originator As String = "06201234567"

        Try

            url = host + "/api?action=sendmessage&" _
                     & "username=" & HttpUtility.UrlEncode(username) _
                     & "&password=" + HttpUtility.UrlEncode(password) _
                     & "&recipient=" + HttpUtility.UrlEncode(tbReceiver.Text) _
                     & "&messagetype=SMS:TEXT" _
                     & "&messagedata=" + HttpUtility.UrlEncode(tbMessage.Text) _
                     & "&originator=" + HttpUtility.UrlEncode(originator) _
                     & "&serviceprovider=" _
                     & "&responseformat=html"


            request = DirectCast(WebRequest.Create(url), HttpWebRequest)

            response = DirectCast(request.GetResponse(), HttpWebResponse)

            MessageBox.Show("Response: " & response.StatusDescription)

        Catch ex As Exception

        End Try

    End Sub
End Class
