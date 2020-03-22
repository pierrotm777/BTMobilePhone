

Option Strict On
Option Explicit On

Imports System
Imports System.IO   'e.g. FileStream, File, etc.

Imports Brecham.Obex

Imports InTheHand.Net           'e.g. IrDAEndPoint
Imports InTheHand.Net.Sockets   'e.g. IrDAClient
' Available from http://32feet.net/.


Class VbPutSampleSample
    Public Shared Sub Main_SimpleNoErrorHandling(ByVal args() As String)
        ' Open file as selected by the user
        If args.Length <> 1 Then
            Console.WriteLine("No filename given.")
            Exit Sub
        End If
        Dim filename As String = args(0)
        Using srcFile As FileStream = File.OpenRead(filename)
            ' Connect
            Dim cli As New IrDAClient("OBEX")
            Dim sess As New ObexClientSession(cli.GetStream, 4096)
            sess.Connect()
            ' And Send
            Dim name As String = Path.GetFileName(filename)
            Dim contentLength As Int64 = srcFile.Length
            sess.PutFrom(srcFile, name, Nothing, contentLength)
            cli.Close()
        End Using
    End Sub
End Class


Class VbPutSample
    Public Shared Sub Main(ByVal args() As String)
        '--------------------------------------------
        ' Open file as selected by the user
        '--------------------------------------------
        If args.Length <> 1 Then
            Console.WriteLine("No filename given.")
            Exit Sub
        End If
        Dim filename As String = args(0)
        Using srcFile As FileStream = File.OpenRead(filename)

            '--------------------------------------------
            ' Create client object and connect.
            '--------------------------------------------
            Dim cli As IrDAClient
            Try
                cli = New IrDAClient("OBEX")
            Catch iopEx As InvalidOperationException
                Console.WriteLine("No peer IrDA device found.")
                Exit Sub
            Catch sex As System.Net.Sockets.SocketException
                Console.WriteLine("IrDA connect failed: " + sex.Message)
                Exit Sub
            End Try

            '--------------------------------------------
            ' Ensure we close the connection.
            '--------------------------------------------
            Try

                '--------------------------------------------
                ' Create the OBEX session object and send a Connect.
                '--------------------------------------------
                Dim sess As New ObexClientSession(cli.GetStream, 4096)
                sess.Connect()

                '--------------------------------------------
                ' Now send the file's contents
                '--------------------------------------------
                Dim name As String = Path.GetFileName(filename)
                Dim contentLength As Int64 = srcFile.Length
                Try
                    sess.PutFrom(srcFile, name, Nothing, contentLength)
                Catch orEx As ObexResponseException
                    Console.WriteLine("Put failed with: " + orEx.Message)
                End Try
                Console.WriteLine("File sent.")

                '--------------------------------------------
                ' Clean up
                '--------------------------------------------
            Finally
                cli.Close()
            End Try
        End Using
        '--------------------------------------------
    End Sub

End Class


'-------------------------------------------------------------------------------
Class VbGetSampleSample
    Public Shared Sub Main_SimpleNoErrorHandling(ByVal args() As String)
        ' GET file as specified by the user
        If args.Length <> 1 Then
            Console.WriteLine("No filename given.")
            Exit Sub
        End If
        Dim filename As String = args(0)
        Using dstFile As FileStream = File.OpenWrite(filename)
            ' Connect
            Dim cli As New IrDAClient("OBEX")
            Dim sess As New ObexClientSession(cli.GetStream, 4096)
            ' Must connect to the Folder-Browsing service, as the default Inbox 
            ' service does not support GET
            sess.Connect(ObexConstant.Target.FolderBrowsing)
            ' And GET
            ' Use the complete filename given
            Dim name As String = filename
            sess.GetTo(dstFile, name, Nothing)
            cli.Close()
        End Using
    End Sub

    Public Shared Sub Main_SimpleNoErrorHandling_Bluetooth(ByVal args() As String)
        ' GET file as specified by the user
        If args.Length <> 1 Then
            Console.WriteLine("No filename given.")
            Exit Sub
        End If
        Dim filename As String = args(0)
        Using dstFile As FileStream = File.OpenWrite(filename)
            ' Connect
            Dim addr As BluetoothAddress = BluetoothAddress.Parse("000a3a6865bb")
            Dim ep As New BluetoothEndPoint(addr, InTheHand.Net.Bluetooth.BluetoothService.ObexFileTransfer)
            Dim cli As New BluetoothClient()
            cli.Connect(ep)
            Dim sess As New ObexClientSession(cli.GetStream, 4096)
            ' Must connect to the Folder-Browsing service, as the default Inbox 
            ' service does not support GET
            sess.Connect(ObexConstant.Target.FolderBrowsing)
            ' And GET
            ' Use the complete filename given
            Dim name As String = filename
            sess.GetTo(dstFile, name, Nothing)
            cli.Close()
        End Using
    End Sub
End Class
