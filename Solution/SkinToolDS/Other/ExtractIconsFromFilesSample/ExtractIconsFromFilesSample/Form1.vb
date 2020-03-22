''' <summary>
''' Author : Check-Kay Wong
''' 
''' the Shared funtion Icon.ExtractAssociatedIcon from System.Drawing.Icon
''' only have one single argument, choosing the first icon from a file.
''' Because the basic extractIcon from Framework.NET don't have enought options,
''' you will need to import and use ExtractAssociatedIcon from Shell32.dll
''' </summary>
''' <remarks></remarks>
Public Class Form1

    ''' <summary>
    ''' this is one way to import a dll
    ''' </summary>
    ''' <param name="hinst"></param>
    ''' <param name="lpiconpath">ENGLISH : file path ; FRANCAIS : chemin d'accès au fichier</param>
    ''' <param name="lpiicon">ENGLISH : icon number or index ; FRANCAIS : numéro d'icône</param>
    ''' <returns>ENGLISH : return a handle ; FRANCAIS: retourne un pointeur</returns>
    ''' <remarks></remarks>
    <System.Runtime.InteropServices.DllImport("shell32.dll")> Shared Function _
    ExtractAssociatedIcon(ByVal hinst As IntPtr, ByVal lpiconpath As String, _
                           ByRef lpiicon As Integer) As IntPtr
    End Function


    'Private Declare Function ExtractAssociatedIcon(ByVal hinst As IntPtr, ByVal lpiconpath As String, ByRef lpiicon As Integer) As IntPtr
    ''' <summary>
    ''' this is another way to import a dll
    ''' </summary>
    ''' <param name="lpiconpath">ENGLISH : file path ; FRANCAIS : chemin d'accès au fichier</param>
    ''' <param name="lpiicon">ENGLISH : icon number or index ; FRANCAIS : numéro d'icône</param>
    ''' <returns>ENGLISH : return a handle ; FRANCAIS: retourne un pointeur</returns>
    ''' <remarks></remarks>
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.TextBox1.Text = My.Application.Info.AssemblyName.ToString
        TextBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.System) & "\shell32.dll"
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

    End Sub



    Private Sub Button1_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseUp
        '--------------------------------------------------------
        ' this part only help getting a file name
        '--------------------------------------------------------
        If Not IO.File.Exists(TextBox1.Text) Then
            Using oOpenFileDialog As New OpenFileDialog
                oOpenFileDialog.Multiselect = False
                If oOpenFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                    TextBox1.Text = oOpenFileDialog.FileName
                End If
            End Using
        End If



        '--------------------------------------------------------
        ' this part only help getting a file name
        '--------------------------------------------------------
        Dim hIcon As IntPtr
        Dim sPath As String = TextBox1.Text
        Dim oIcon As Icon
        Dim index1 As Integer

        Try

            ListView1.Items.Clear()
            ImageList1.Images.Clear()
            ImageList2.Images.Clear()



            ListView1.SmallImageList = ImageList1
            ListView1.LargeImageList = ImageList2

            'i decided to load the first 300 icons.
            'using the shell32.dll function and
            ' and not the one from the Framework .NET
            For index1 = 0 To 300 Step 1
                hIcon = ExtractAssociatedIcon(Me.Handle, sPath, index1)
                oIcon = Icon.FromHandle(hIcon)
                ImageList1.Images.Add(oIcon)
                ImageList2.Images.Add(oIcon)
                ListView1.Items.Add(index1.ToString, index1)
            Next index1


        Catch ex As Exception
            MsgBox(index1)
        End Try
    End Sub
End Class
