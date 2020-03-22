Imports System
Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.IO

Public Class Slideshow
    Dim SDK As Object
    Private PluginSettings As cMySettings
    Private TempPluginSettings As cMySettings
    Private allowedExtensions() As String = {".gif", ".jpg", ".bmp", ".png"} ', ".tiff"}

    'http://www.dotnetheaven.com/article/vertical-scrollbar-in-vb.net
    ' Dim vScroller As New VScrollBar()

    Private Sub Slideshow_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If Not Directory.Exists(Application.StartupPath & "\Photo") Then
            Directory.CreateDirectory(Application.StartupPath & "\Photo")
        End If

        PluginSettings = New cMySettings(Path.Combine(Application.StartupPath, "SlideShow"))
        ' read in defaults
        cMySettings.DeseralizeFromXML(PluginSettings)

        ' copy to temp
        TempPluginSettings = New cMySettings(PluginSettings)
        'SDK = CreateObject("RideRunner.SDK")

        ListView2.MultiSelect = False

        ListView2.Scrollable = False

        Listview2.BorderStyle = BorderStyle.Fixed3D
        Dim BacGroundColor() As String = TempPluginSettings.BackGroundColor.Split(",")
        Me.BackColor = Color.FromArgb(CInt(BacGroundColor(0)), CInt(BacGroundColor(1)), CInt(BacGroundColor(2)))
        Listview2.BackColor = Color.FromArgb(CInt(BacGroundColor(0)), CInt(BacGroundColor(1)), CInt(BacGroundColor(2)))
        Dim textColor() As String = TempPluginSettings.TextColor.Split(",")
        Listview2.ForeColor = Color.FromArgb(CInt(textColor(0)), CInt(textColor(1)), CInt(textColor(2)))

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
        Listview2.LargeImageList = _imgList


        Dim count As Integer = 0
        'Dim item As New ListViewItem

        ' Set the view to show details.
        'ListView2.View = View.LargeIcon
        ' Sort the items in the list in ascending order.
        Listview2.Sorting = SortOrder.Ascending

        For Each dirFile As String In dirFiles

            For Each extension As String In allowedExtensions
                If extension = IO.Path.GetExtension(dirFile) Then

                    Dim img As New System.Drawing.Bitmap(dirFile)
                    _imgList.ImageSize = imgSize
                    _imgList.Images.Add(img)

                    img.Dispose()

                    Listview2.Items.Add(Path.GetFileName(dirFile), count)
                    count += 1

                End If
            Next
        Next
        ListView2.EnsureVisible(ListView2.Items.Count - 1)
    End Sub

    Private Sub listView2_MouseDown(sender As Object, e As MouseEventArgs)
        Dim results = Listview2.HitTest(e.Location)

        If results.Item IsNot Nothing Then
            'MessageBox.Show(Application.StartupPath & "\Photo")
            'MessageBox.Show(results.Item.Text & vbCrLf & results.Location)
            Dim PicturePath As String = Path.GetDirectoryName(results.Item.Text)
            Dim PictureName As String = Path.GetFileName(results.Item.Text)
            If SDK IsNot Nothing Then
                SDK.Execute(TempPluginSettings.SendCommandToRR & PictureName)
            End If
        End If

    End Sub

    'Private Sub cmdDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles cmdDown.Click
    '    If cmdDown.Tag <> totalRecordsReturned Then
    '        cmdDown.Tag += 1
    '        ListView1.Items(cmdDown.Tag).Selected = True
    '        ListView1.Focus()
    '    End If
    'End Sub
    'Private Sub lvSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListView2.KeyDown
    '    If e.KeyCode = Keys.Down Then

    '        If ListView2.SelectedItems(0).Index < (ListView2.Items.Count - 1) Then

    '            txtName.Text = ListView2.Items(ListView2.SelectedItems(0).Index + 1).SubItems(1).Text

    '        End If
    '    End If
    'End Sub

    'Private Sub lvSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListView2.KeyUp
    '    If e.KeyCode = Keys.Up Then
    '        If ListView2.SelectedItems(0).Index >= 0 Then

    '            txtName.Text = ListView2.Items(ListView2.SelectedItems(0).Index).SubItems(1).Text

    '        End If
    '    End If
    'End Sub
End Class
