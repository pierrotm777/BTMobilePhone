Option Strict On

Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Show()
        GetSystemInfo()
    End Sub

    Private Sub GetSystemInfo()
        ListBox1.Items.Clear()

        ListBox1.Items.Add("Computer")
        ListBox1.Items.Add("    Name = " + My.Computer.Name)
        ListBox1.Items.Add("    Is a Laptop = " + cSystemInfo.IsLaptop.ToString)
        ListBox1.Items.Add("    UI Culture = " + My.Computer.Info.InstalledUICulture.ToString)
        ListBox1.Items.Add("    OS FullName = " + My.Computer.Info.OSFullName.ToString)
        ListBox1.Items.Add("    OS Platform = " + My.Computer.Info.OSPlatform.ToString)
        ListBox1.Items.Add("    OS Version = " + My.Computer.Info.OSVersion.ToString)

        ListBox1.Items.Add("Processor")
        ListBox1.Items.Add("    Number Physical = " + cSystemInfo.NumberOfCPUsPhysical)
        ListBox1.Items.Add("    Number Logical = " + cSystemInfo.NumberOfCPUsLogical)
        ListBox1.Items.Add("    Manufacturer = " + cSystemInfo.CPUManufacturer)
        ListBox1.Items.Add("    Name = " + cSystemInfo.CPUName)
        ListBox1.Items.Add("    Description = " + cSystemInfo.CPUDescription)
        ListBox1.Items.Add("    Address Width = " + cSystemInfo.AddressWidth)
        ListBox1.Items.Add("    Clock Speed Max = " + cSystemInfo.ClockSpeedMax)
        ListBox1.Items.Add("    Clock Speed Current = " + cSystemInfo.ClockSpeedCurrent)

        ListBox1.Items.Add("Memory")
        ListBox1.Items.Add("    Total Physical = " + My.Computer.Info.TotalPhysicalMemory.ToString("#,##0"))
        ListBox1.Items.Add("    Available Physical = " + My.Computer.Info.AvailablePhysicalMemory.ToString("#,##0"))
        ListBox1.Items.Add("    Total Virtual = " + My.Computer.Info.TotalVirtualMemory.ToString("#,##0"))
        ListBox1.Items.Add("    Available Virtual = " + My.Computer.Info.AvailableVirtualMemory.ToString("#,##0"))

        ListBox1.Items.Add("Hard Drives")
        For Each strX In cSystemInfo.DiskSpace
            ListBox1.Items.Add("    " + strX)
        Next

        ListBox1.Items.Add("Screens")
        For Each strX In cSystemInfo.Screens
            ListBox1.Items.Add("    " + strX)
        Next

        ListBox1.Items.Add(".Net Framework")
        ListBox1.Items.Add("    Highest = " + cSystemInfo.HighestFrameworkVersion)
        ListBox1.Items.Add("    All")
        For Each strX In cSystemInfo.ListFrameworkVersions
            ListBox1.Items.Add("        " + strX)
        Next

        ListBox1.Items.Add("Office")
        For Each strX In cSystemInfo.AllOfficeVersions
            ListBox1.Items.Add("    " + strX)
        Next

        ListBox1.Items.Add("-----------------------------------")
    End Sub

End Class
