Imports System.Data.SQLite

Public Class PhoneInterface
    Dim t As DataTable = New DataTable()
    Dim appPath As String = Application.StartupPath

    Private Sub PhoneInterface_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            'Dim cs As String = "URI=file:" & appPath & "\phonebook.s3db"
            ''Dim cs As String = "URI=file:phonebook.s3db"
            'Dim con As New SQLiteConnection(cs)
            'Dim comm As SQLiteCommand
            'con.Open()
            'comm = con.CreateCommand
            'comm.CommandText = "SELECT Name from Contacts order by Name"
            'Dim oAdapter As New SQLiteDataAdapter(comm)

            'oAdapter.Fill(t)
            ''Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
            'Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D

            ''AddHandler Me.lstContacts.ItemChecked, AddressOf lstContacts_ItemChecked 'radListView1_ItemDataBound
            ''AddHandler Me.lstContacts.VisualItemFormatting, AddressOf radListView1_VisualItemFormatting
            'AddHandler Me.lstContacts.Click, AddressOf radListView1_VisualItemFormatting
            ''AddHandler Me.lstContacts.CellFormatting, AddressOf radListView1_CellFormatting


            'lstContacts.Enabled = True

            'lstContacts.View = View.Details
            'lstContacts.GridLines = True
            'lstContacts.FullRowSelect = True
            'lstContacts.HideSelection = False
            'lstContacts.MultiSelect = False
            'lstContacts.Columns.Add("Name", 100, HorizontalAlignment.Left)
            'lstContacts.Columns.Add("Mobile", 100, HorizontalAlignment.Left)
            'lstContacts.Columns.Add("Home", 100, HorizontalAlignment.Left)
            'lstContacts.Columns.Add("Work", 100, HorizontalAlignment.Left)


            'Me.lstContacts.AllowEdit = False
            'Me.lstContacts.AllowRemove = False
            'Me.lstContacts.DisplayMember = "Name"
            'Me.lstContacts.ValueMember = "Name"
            'Me.lstContacts.DataSource = t
            'Me.lstContacts.ItemSpacing = 10
            'Me.lstContacts.AllowArbitraryItemHeight = True

            Dim Con As New SQLiteConnection("Data Source= " & Environment.CurrentDirectory & "\phonebook.s3db")
            Con.Open()
            Dim Cmd As New SQLiteCommand("SELECT Name, MobilePhone, HomePhone, Fax FROM Contacts", Con)
            Dim table As New DataTable()
            Dim dataAdapter As New SQLiteDataAdapter(Cmd)
            dataAdapter.Fill(table)
            DataGridView1.DataSource = table
            DataGridView1.Columns(0).Width = 200 ' Cette commande définit la largeur des colonnes
            DataGridView1.Columns(1).Width = 120
            DataGridView1.Columns(2).Width = 120
            DataGridView1.Columns(3).Width = 120
            Cmd.Dispose()
            Con.Close()


            'comm.Dispose()
            'con.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    'Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles Button2.TextChanged
    '    ListView.Items.Clear()
    '    Dim Con As New SQLite.SQLiteConnection("Data Source= " & Environment.CurrentDirectory & "\phonebook.s3db")
    '    Con.Open()
    '    Dim Cmd As New SQLiteCommand("SELECT * FROM Contacts WHERE Nazwisko LIKE @Nazwisko", Con)
    '    Cmd.Parameters.AddWithValue("@Nazwisko", Button2.Text & "%")
    '    Dim myReader As SQLite.SQLiteDataReader = Cmd.ExecuteReader()
    '    While myReader.Read
    '        Dim newListViewItem As New ListViewItem
    '        newListViewItem.Text = myReader.GetInt32(0)
    '        newListViewItem.SubItems.Add(myReader.GetString(1))
    '        newListViewItem.SubItems.Add(myReader.GetString(2))
    '        newListViewItem.SubItems.Add(myReader.GetString(3))
    '        ListView.Items.Add(newListViewItem)
    '    End While
    '    Con.Close()
    'End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    'Private Sub radListView1_ItemDataBound(sender As Object, e As Telerik.WinControls.UI.ListViewItemEventArgs)
    'Private Sub lstContacts_ItemChecked(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles lstContacts.Click
    '    'e.Item.Image = Image.FromFile("c:\Temp\unknown.png")
    'End Sub

    Private Sub radListView1_VisualItemFormatting(sender As Object, e As ListViewItemSelectionChangedEventArgs)

        'MessageBox.Show(appPath)
        Dim cs As String = "URI=file:" & appPath & "\phonebook.s3db"
        'Dim cs As String = "URI=file:phonebook.s3db"
        Dim con As New SQLiteConnection(cs)
        Dim comm As SQLiteCommand

        'If e.VisualItem.Data.Image IsNot Nothing Then
        '    e.VisualItem.Image = e.VisualItem.Data.Image.GetThumbnailImage(32, 32, Nothing, IntPtr.Zero)
        'End If

        'If e.VisualItem.Selected Then
        If e.Item.Selected Then
            con.Open()
            comm = con.CreateCommand
            comm.CommandText = "SELECT MobilePhone,HomePhone,Work from Contacts where Name = """ & e.Item.Name & """"

            Dim rdr As SQLiteDataReader = comm.ExecuteReader

            If Not rdr.IsDBNull(0) Then
                btnCallMobile.Show()
                txtMobilePhone.Text = rdr("MobilePhone")
            Else
                btnCallMobile.Hide()
                txtMobilePhone.Clear()
            End If
            If Not rdr.IsDBNull(1) Then
                btnCallHome.Show()
                txtHomePhone.Text = rdr("HomePhone")
            Else
                btnCallHome.Hide()
                txtHomePhone.Clear()
            End If
            If Not rdr.IsDBNull(2) Then
                btnCallWork.Show()
                txtWorkPhone.Text = rdr("Work")
            Else
                btnCallWork.Hide()
                txtWorkPhone.Clear()
            End If
            comm.Dispose()
            con.Close()
        Else

        End If
    End Sub

    'Private Sub radListView1_CellFormatting(sender As Object, e As ListViewCellFormattingEventArgs)
    '    If e.CellElement.Image IsNot Nothing Then
    '        e.CellElement.Image = e.CellElement.Image.GetThumbnailImage(32, 32, Nothing, IntPtr.Zero)
    '    End If
    'End Sub


    Private Sub commandBarTextBoxFilter_TextChanged(sender As Object, e As EventArgs)
        Me.lstContacts.Clear()

        '    If [String].IsNullOrEmpty(Me.txtSearch.Text) Then
        '        Me.lstContacts.EnableFiltering = False
        '    Else
        '        Me.lstContacts.FilterDescriptors.LogicalOperator = FilterLogicalOperator.[Or]
        '        Me.lstContacts.FilterDescriptors.Add("Name", FilterOperator.Contains, Me.txtSearch.Text)
        '        Me.lstContacts.EnableFiltering = True
        '    End If
    End Sub





End Class