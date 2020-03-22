
Option Explicit
Option Strict

Imports System.Windows.Forms
Imports System.ComponentModel   'e.g. BackgroundWorker
Imports System.Drawing  ' e.g. Point

Imports System.IO
Imports System.Net.Sockets  'e.g. NetworkStream

'Imports InTheHand.Net
'Imports InTheHand.Net.Sockets
'Imports InTheHand.Net.Bluetooth   'e.g. BluetoothService
'' Available from http://32feet.net/.

Imports Brecham.Obex
Imports Brecham.Obex.Objects


Class GetFirstListedFile

    Public Shared Sub Main(ByVal args() As String)
        ' Create client connection object and connect, ensuring its disposal.
        Using conn As New Brecham.Obex.Net.ConsoleMenuObexSessionConnection
            conn.Connect
            '
            DoIt(conn.ObexClientSession)
        End Using
    End Sub


    '
    ' Get the first file in the folder-listing.
    '
    Shared Sub DoIt(sess As ObexClientSession)
        Dim depth As Int32 = 0
        While (True)
            Dim listing As ObexFolderListing = sess.GetFolderListing
            If (listing.Files.Count <> 0) Then
                Dim name As String = listing.Files(0).Name
                Console.WriteLine("Downloading file ‘"+ name +"’")
                Using dest As Stream = New FileStream(name, FileMode.Create)
                   sess.GetTo(dest, name, Nothing)
                End Using 'dest.Close
                Console.WriteLine("Success")
                Exit While
            Else If (listing.Folders.Count <> 0) Then
                Dim name As String = listing.Folders(0).Name
                Console.WriteLine("Changing to child folder ‘"+ name +"’")
                sess.SetPath(name)
                depth = depth + 1
            Else
                Console.WriteLine("Found not one file!")
                Exit While
            End If 
        End While
    End Sub

    '
    ' As included in the class documentation for the ObexFolderListingParser class.
    '
    Sub DisplayCurrentFoldersListing(sess As ObexClientSession)
       Dim listing As ObexFolderListing = sess.GetFolderListing
       ' Use the listing's HasParentFolder, Folders, and Files properties.
       If (listing.HasParentFolder) Then
           Console.WriteLine("<DIR> ..")
       End If
       For Each folder As ObexFolderItem In listing.Folders
          Console.WriteLine("<DIR> {0}", folder.Name)
       Next
       For Each file As ObexFileItem In listing.Files
          Console.WriteLine("      {0}", file.Name)
       Next
    End Sub

End Class
