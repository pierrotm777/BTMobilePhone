Imports System
Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.InteropServices

Public Class Slideshow
    Dim SDK As Object
    Private PluginSettings As cMySettings
    Private TempPluginSettings As cMySettings
    Private allowedExtensions() As String = {".gif", ".jpg", ".bmp", ".png"} ', ".tiff"}
    Private Const WM_VSCROLL As Integer = &H115
    Private Const SB_LINEDOWN As Integer = 1

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(hWnd As IntPtr, Msg As UInt32, wParam As Integer, lParam As Integer) As IntPtr
    End Function

    Private Sub listView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView2.SelectedIndexChanged
        SendMessage(ListView2.Handle, WM_VSCROLL, SB_LINEDOWN, 0)
    End Sub

    Private Sub Slideshow_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If Not Directory.Exists(Application.StartupPath & "\Photo") Then
            Directory.CreateDirectory(Application.StartupPath & "\Photo")
        End If

        PluginSettings = New cMySettings(Path.Combine(Application.StartupPath, "SlideShow"))
        ' read in defaults
        cMySettings.DeseralizeFromXML(PluginSettings)

        ' copy to temp
        TempPluginSettings = New cMySettings(PluginSettings)
        SDK = CreateObject("RideRunner.SDK")

        'ListView2.MultiSelect = False
        'ListView2.Scrollable = False

        ListView2.BorderStyle = BorderStyle.Fixed3D
        Dim BacGroundColor() As String = TempPluginSettings.BackGroundColor.Split(",")
        Me.BackColor = Color.FromArgb(CInt(BacGroundColor(0)), CInt(BacGroundColor(1)), CInt(BacGroundColor(2)))
        ListView2.BackColor = Color.FromArgb(CInt(BacGroundColor(0)), CInt(BacGroundColor(1)), CInt(BacGroundColor(2)))
        Dim textColor() As String = TempPluginSettings.TextColor.Split(",")
        ListView2.ForeColor = Color.FromArgb(CInt(textColor(0)), CInt(textColor(1)), CInt(textColor(2)))

        ListView2.Font = New Font(New FontFamily(TempPluginSettings.TextFont), TempPluginSettings.TextSize, FontStyle.Bold)


        mylist()
    End Sub


    Private Sub mylist()
        'Dim imlTemp As New ImageList
        Dim dirFiles() As String = IO.Directory.GetFiles(Application.StartupPath & "\Photo")
        Dim _imgList As New ImageList
        Dim imgSize As New Size

        imgSize.Width = TempPluginSettings.PictureSize_X
        imgSize.Height = TempPluginSettings.PictureSize_Y

        _imgList.ColorDepth = ColorDepth.Depth32Bit
        ListView2.LargeImageList = _imgList


        Dim count As Integer = 0
        Dim item As New ListViewItem

        ' Set the view to show details.
        'ListView2.View = View.LargeIcon
        ' Sort the items in the list in ascending order.
        'ListView2.Sorting = SortOrder.Ascending

        For Each dirFile As String In dirFiles

            For Each extension As String In allowedExtensions
                If extension = IO.Path.GetExtension(dirFile) Then

                    Dim img As New System.Drawing.Bitmap(dirFile)
                    _imgList.ImageSize = imgSize
                    _imgList.Images.Add(img)

                    img.Dispose()

                    ListView2.Items.Add(Path.GetFileName(dirFile), count)
                    count += 1

                End If
            Next
        Next

        'go to the last item 
        'ListView2.EnsureVisible(ListView2.Items.Count - 1)
    End Sub

    Private Sub listView2_MouseDown(sender As Object, e As MouseEventArgs) Handles ListView2.MouseDown
        Try
            Dim results = ListView2.HitTest(e.Location)

            If results.Item IsNot Nothing Then
                'MessageBox.Show(Application.StartupPath & "\Photo")
                'MessageBox.Show(results.Item.Text & vbCrLf & results.Location)
                Dim PicturePath As String = Path.GetDirectoryName(results.Item.Text)
                Dim PictureName As String = Path.GetFileName(results.Item.Text)
                If SDK IsNot Nothing Then
                    SDK.Execute(TempPluginSettings.SendCommandToRR & PictureName)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "SlideShow Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub


End Class
