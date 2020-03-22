Imports System.Data.SQLite
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.Data

Public Class PhoneInterface
    Dim t As DataTable = New DataTable()
    Private Sub PhoneInterface_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim cs As String = "URI=file:C:\Temp\phonebook.s3db"
        Dim con As New SQLiteConnection(cs)
        Dim comm As SQLiteCommand
        con.Open()
        comm = con.CreateCommand
        comm.CommandText = "SELECT Name from Contacts order by Name"
        Dim oAdapter As New SQLiteDataAdapter(comm)

        oAdapter.Fill(t)
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None

        AddHandler Me.lstContacts.ItemDataBound, AddressOf radListView1_ItemDataBound
        AddHandler Me.lstContacts.VisualItemFormatting, AddressOf radListView1_VisualItemFormatting
        AddHandler Me.lstContacts.CellFormatting, AddressOf radListView1_CellFormatting


        Me.lstContacts.AllowEdit = False
        Me.lstContacts.AllowRemove = False
        Me.lstContacts.DisplayMember = "Name"
        Me.lstContacts.ValueMember = "Name"
        Me.lstContacts.DataSource = t
        Me.lstContacts.ItemSpacing = 10
        Me.lstContacts.AllowArbitraryItemHeight = True
        comm.Dispose()
        con.Close()


    End Sub

    Private Sub radListView1_ItemDataBound(sender As Object, e As Telerik.WinControls.UI.ListViewItemEventArgs)
        e.Item.Image = Image.FromFile("c:\Temp\unknown.png")
    End Sub

    Private Sub radListView1_VisualItemFormatting(sender As Object, e As Telerik.WinControls.UI.ListViewVisualItemEventArgs)

        Dim cs As String = "URI=file:C:\Temp\phonebook.s3db"
        Dim con As New SQLiteConnection(cs)
        Dim comm As SQLiteCommand
        If e.VisualItem.Data.Image IsNot Nothing Then
            e.VisualItem.Image = e.VisualItem.Data.Image.GetThumbnailImage(32, 32, Nothing, IntPtr.Zero)
        End If

        If e.VisualItem.Selected Then
            con.Open()
            comm = con.CreateCommand
            comm.CommandText = "SELECT MobilePhone,HomePhone,Work from Contacts where Name = """ & e.VisualItem.Data.Value & """"
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

    Private Sub radListView1_CellFormatting(sender As Object, e As ListViewCellFormattingEventArgs)
        If e.CellElement.Image IsNot Nothing Then
            e.CellElement.Image = e.CellElement.Image.GetThumbnailImage(32, 32, Nothing, IntPtr.Zero)
        End If
    End Sub


    Private Sub commandBarTextBoxFilter_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Me.lstContacts.FilterDescriptors.Clear()

        If [String].IsNullOrEmpty(Me.txtSearch.Text) Then
            Me.lstContacts.EnableFiltering = False
        Else
            Me.lstContacts.FilterDescriptors.LogicalOperator = FilterLogicalOperator.[Or]
            Me.lstContacts.FilterDescriptors.Add("Name", FilterOperator.Contains, Me.txtSearch.Text)
            Me.lstContacts.EnableFiltering = True
        End If
    End Sub

End Class
