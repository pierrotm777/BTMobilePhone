using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;


namespace Temperature
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

        }

        private temperature localtemp = new temperature();

        private string GetCpuTemp()
        {
            string result = String.Empty;
            string rawdata = String.Empty;
            Double temp;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI",
                                 "SELECT * FROM MSAcpi_ThermalZoneTemperature");

            ManagementObjectCollection.ManagementObjectEnumerator enumerator =
                searcher.Get().GetEnumerator();

            while (enumerator.MoveNext())
            {
                int x;
                ManagementBaseObject tempObject = enumerator.Current;

                temp = Convert.ToDouble(tempObject["CurrentTemperature"].ToString());
                temp = (temp - 2732) / 10.0;
                result = " " + (1.8 * temp + 32).ToString() + " F";

            }
            searcher.Dispose();
            return result;

        }

        private void MainForm_Load(object sender, EventArgs e)
        {


            timer1.Interval = 1000;
            timer1.Start();

        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*label1.Text = localtemp.cputemperature;*/
            label1.Text = CPUTempFromOH();
            this.Text = label1.Text;
        }
        // From The Web
        // using OHML.Dll
        private string CPUTempFromOH()
        {

            string result = String.Empty;
           OpenHardwareMonitor.Hardware.Computer computerHardware = new OpenHardwareMonitor.Hardware.Computer();
        	computerHardware.Open();
            computerHardware.CPUEnabled = true;
            var temps = new List<decimal>();
         	foreach (var hardware in computerHardware.Hardware)
        	{  
             if (hardware.HardwareType != OpenHardwareMonitor.Hardware.HardwareType.CPU)
     	        continue;
                hardware.Update();
        	    foreach (var sensor in hardware.Sensors)
                 {
        	        if (sensor.SensorType == OpenHardwareMonitor.Hardware.SensorType.Temperature)
                    {
        	            if (sensor.Value != null)
     	                temps.Add((decimal)sensor.Value);
        	        }
        	    }
         	}
         	var maxTemp = temps.Count == 0 ? 0 : temps.Max();
            result = " " + ((Convert.ToDecimal(1.8) * maxTemp) + 32).ToString() + " F";
            //result = " " + ((maxTemp)).ToString() + " C";
            //result =  maxTemp.ToString();
            return result;
            

        }
        // temperature class
        // ver 1 01-29-2014
        // Reports 2 WMI Temps which do not appear to be current, compared to OH Monitor
        public class temperature
        {
            public temperature()
            {
            }
            public string systemtemperature
            {
                get
                {
                    GetTemperatures();
                    return _systemtemperature;
                }
                set { }
            }
            public string cputemperature
            {
                get
                {
                    GetTemperatures();
                    return _cputemperature;
                }
                set { }
            }
            public int numberoftemperatures
            {
                get
                {
                    return _numberoftemperatures;
                }
                set { }
            }

            private string _systemtemperature;
            private string _cputemperature;
            private int _numberoftemperatures;
            private void GetTemperatures()
            {
                string result = String.Empty;
                Double temp;
                int count = 0;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI",
                                     "SELECT * FROM MSAcpi_ThermalZoneTemperature");

                ManagementObjectCollection.ManagementObjectEnumerator enumerator =
                    searcher.Get().GetEnumerator();

                while (enumerator.MoveNext())
                {
                    ManagementBaseObject tempObject = enumerator.Current;
                    temp = Convert.ToDouble(tempObject["CurrentTemperature"].ToString());
                    temp = (temp - 2732) / 10.0;
                    if (count == 0)
                    {
                        _cputemperature = " " + (1.8 * temp + 32).ToString() + " F";
                    }
                    if (count == 1)
                    {
                        _systemtemperature = " " + (1.8 * temp + 32).ToString() + " F";
                    }
                    count++;

                }
                searcher.Dispose();
            }

        }
    }
}
