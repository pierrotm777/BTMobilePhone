using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoSystemInfoCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Show();
            GetSystemInfo();
        }

        private void GetSystemInfo()
        {
            ListBox1.Items.Clear();

            Microsoft.VisualBasic.Devices.Computer myComputer = new Microsoft.VisualBasic.Devices.Computer();

            ListBox1.Items.Add("Computer");
            ListBox1.Items.Add("    Name = " + myComputer.Name);
            ListBox1.Items.Add("    Is a Laptop = " + cSystemInfo.IsLaptop.ToString());
            ListBox1.Items.Add("    UI Culture = " + myComputer.Info.InstalledUICulture.ToString());
            ListBox1.Items.Add("    OS FullName = " + myComputer.Info.OSFullName.ToString());
            ListBox1.Items.Add("    OS Platform = " + myComputer.Info.OSPlatform.ToString());
            ListBox1.Items.Add("    OS Version = " + myComputer.Info.OSVersion.ToString());

            ListBox1.Items.Add("Processor");
            ListBox1.Items.Add("    Number Physical = " + cSystemInfo.NumberOfCPUsPhysical);
            ListBox1.Items.Add("    Number Logical = " + cSystemInfo.NumberOfCPUsLogical);
            ListBox1.Items.Add("    Manufacturer = " + cSystemInfo.CPUManufacturer);
            ListBox1.Items.Add("    Name = " + cSystemInfo.CPUName);
            ListBox1.Items.Add("    Description = " + cSystemInfo.CPUDescription);
            ListBox1.Items.Add("    Address Width = " + cSystemInfo.AddressWidth);
            ListBox1.Items.Add("    Clock Speed Max = " + cSystemInfo.ClockSpeedMax);
            ListBox1.Items.Add("    Clock Speed Current = " + cSystemInfo.ClockSpeedCurrent);

            ListBox1.Items.Add("Memory");
            ListBox1.Items.Add("    Total Physical = " + myComputer.Info.TotalPhysicalMemory.ToString("#,##0"));
            ListBox1.Items.Add("    Available Physical = " + myComputer.Info.AvailablePhysicalMemory.ToString("#,##0"));
            ListBox1.Items.Add("    Total Virtual = " + myComputer.Info.TotalVirtualMemory.ToString("#,##0"));
            ListBox1.Items.Add("    Available Virtual = " + myComputer.Info.AvailableVirtualMemory.ToString("#,##0"));

            ListBox1.Items.Add("Hard Drives");
            foreach (String strX in cSystemInfo.DiskSpace)
            {
                ListBox1.Items.Add("    " + strX);
            }

            ListBox1.Items.Add("Screens");
            foreach (String strX in cSystemInfo.Screens)
            {
                ListBox1.Items.Add("    " + strX);
            }

            ListBox1.Items.Add(".Net Framework");
            ListBox1.Items.Add("    Highest = " + cSystemInfo.HighestFrameworkVersion);
            ListBox1.Items.Add("    All");
            foreach (String strX in cSystemInfo.ListFrameworkVersions)
            {
                ListBox1.Items.Add("        " + strX);
            }

            ListBox1.Items.Add("Office");
            foreach (String strX in cSystemInfo.AllOfficeVersions)
            {
                ListBox1.Items.Add("    " + strX);
            }

            ListBox1.Items.Add("-----------------------------------");
        }

    }
}
