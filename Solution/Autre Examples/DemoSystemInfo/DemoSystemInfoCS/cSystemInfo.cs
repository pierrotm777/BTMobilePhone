using System;
using System.Collections.Generic;

using System.IO;
using System.Text.RegularExpressions;
using System.Management;
using Microsoft.Win32;
using System.Windows.Forms;

namespace DemoSystemInfoCS
{

    public class cSystemInfo
    {

        #region "Declarations"

        //Possible Office applications
        public enum MSOfficeApp
        {
            Access_Application,
            Excel_Application,
            Outlook_Application,
            PowerPoint_Application,
            Word_Application
        }

        //Possible Office versions    
        public enum Version
        {
            Version95 = 7,
            Version97 = 8,
            Version2000 = 9,
            Version2002 = 10,
            Version2003 = 11,
            Version2007 = 12,
            Version2010 = 14
        }

        #endregion

        #region " - Battery - "

        public static bool IsLaptop
        {
            get
            {
                ManagementClass mgmtProc;
                try
                {
                    mgmtProc = new ManagementClass("Win32_Battery");
                }
                catch
                {
                    mgmtProc = null;
                }
                if ((mgmtProc == null))
                    return false;

                foreach (ManagementObject objInstance in mgmtProc.GetInstances())
                {
                    return true;
                }

                return false;
            }
        }

        #endregion

        #region " --- Disks --- "

        public static List<string> DiskSpace
        {
            get
            {
                List<string> arrInfo = new List<string>();
                ManagementObjectSearcher Searcher = default(ManagementObjectSearcher);
                Searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
                foreach (ManagementObject mgmtDisk in Searcher.Get())
                {
                    if (Convert.ToString(mgmtDisk["MediaType"]) == "12")
                    {
                        arrInfo.Add(mgmtDisk.Properties["DeviceID"].Value.ToString() + " Size=" + readableLength(Convert.ToInt64(mgmtDisk.Properties["Size"].Value)) + " Free=" + readableLength(Convert.ToInt64(mgmtDisk.Properties["FreeSpace"].Value)));
                    }
                }

                return arrInfo;
            }
        }

        //READABLE LENGTH
        public static string readableLength(long length)
        {
            string[] suffix = new string[4] {
			"b",
			"Kb",
			"Mb",
			"Gb"
		};
            int i = suffix.GetLowerBound(0);
            while ((length > 1024) && (i < suffix.GetUpperBound(0)))
            {
                length /= 1024;
                i += 1;
            }
            return length.ToString() + suffix[i];
        }

        #endregion

        #region " - Framework - "

        private const string FRAMEWORK_PATH = "\\Microsoft.NET\\Framework";
        private const string WINDIR1 = "windir";

        private const string WINDIR2 = "SystemRoot";
        public static string HighestFrameworkVersion
        {
            get
            {
                try
                {
                    return GetHighestVersion(NetFrameworkInstallationPath);
                }
                catch
                {
                    return "Unknown";
                }
            }
        }

        public static List<string> ListFrameworkVersions
        {
            get
            {
                List<string> arrVersions = new List<string>();
                string strVersion = "Unknown";

                foreach (string strX in Directory.GetDirectories(NetFrameworkInstallationPath, "v*"))
                {
                    strVersion = ExtractVersion(strX);
                    if (PatternIsVersion(strVersion))
                    {
                        arrVersions.Add(strVersion);
                    }
                }

                return arrVersions;
            }
        }

        private static string GetHighestVersion(string pInstallationPath)
        {
            string[] arrVersions = Directory.GetDirectories(pInstallationPath, "v*");
            string strVersion = "Unknown";

            for (int i = arrVersions.Length - 1; i >= 0; i += -1)
            {
                strVersion = ExtractVersion(arrVersions[i]);
                if (PatternIsVersion(strVersion))
                    return strVersion;
            }

            return strVersion;
        }

        private static string ExtractVersion(string pdirectory)
        {
            int intStartIndex = pdirectory.LastIndexOf("\\") + 2;
            return pdirectory.Substring(intStartIndex, pdirectory.Length - intStartIndex);
        }

        private static bool PatternIsVersion(string pVersion)
        {
            return new Regex("[0-9](.[0-9]){0,3}").IsMatch(pVersion);
        }

        public static string NetFrameworkInstallationPath
        {
            get { return WindowsPath + FRAMEWORK_PATH; }
        }

        public static string WindowsPath
        {
            get
            {
                string strWinDir = Environment.GetEnvironmentVariable(WINDIR1);
                if (string.IsNullOrEmpty(strWinDir))
                {
                    strWinDir = Environment.GetEnvironmentVariable(WINDIR2);
                }

                return strWinDir;
            }
        }

        #endregion

        #region " - Screens - "

        public static List<string> Screens
        {
            get
            {
                List<string> arrInfo = new List<string>();

                int intI = 0;
                foreach (Screen objX in Screen.AllScreens)
                {
                    intI += 1;

                    arrInfo.Add(string.Format("Screen {0} - Primary {1} - Bounds {2} - BitsPerPixel {3}", intI, objX.Primary, objX.Bounds.ToString(), objX.BitsPerPixel.ToString()));
                }

                return arrInfo;
            }
        }

        #endregion

        #region " - Office - "

        //ALL OFFICE VERSIONS
        public static List<string> AllOfficeVersions
        {
            get
            {
                List<string> arrInfo = new List<string>();
                foreach (string s in Enum.GetNames(typeof(MSOfficeApp)))
                {
                    arrInfo.Add(s.Replace("_Application", "") + "=" + GetVersionsString((MSOfficeApp)Enum.Parse(typeof(MSOfficeApp), s)));
                }

                return arrInfo;
            }
        }

        //GET VERSIONS STRING
        public static string GetVersionsString(MSOfficeApp app)
        {
            try
            {
                string strProgID = Enum.GetName(typeof(MSOfficeApp), app);
                strProgID = strProgID.Replace("_", ".");
                RegistryKey regKey = null;
                regKey = Registry.LocalMachine.OpenSubKey("Software\\Classes\\" + strProgID + "\\CurVer", false);
                if (regKey == null)
                    return "No version detected.";
                string strV = regKey.GetValue("", null, RegistryValueOptions.None).ToString();
                regKey.Close();
                strV = strV.Replace(strProgID, "").Replace(".", "");
                return Enum.GetName(typeof(Version), Convert.ToInt32(strV));
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region " --- WMI Properties --- "

        public static string NumberOfCPUsPhysical
        {
            get { return "Physical = " + WMIComputerSystemProperties("NumberOfProcessors"); }
        }

        public static string NumberOfCPUsLogical
        {
            get { return "Logical = " + WMIComputerSystemProperties("NumberOfLogicalProcessors"); }
        }

        public static string ClockSpeedMax
        {
            get { return "Max = " + WMIProcessorProperties("MaxClockSpeed"); }
        }

        public static string ClockSpeedCurrent
        {
            get { return "Current = " + WMIProcessorProperties("MaxClockSpeed"); }
        }

        public static string AddressWidth
        {
            get { return WMIProcessorProperties("AddressWidth"); }
        }

        public static string CPUManufacturer
        {
            get { return WMIProcessorProperties("Manufacturer"); }
        }

        public static string CPUName
        {
            get { return WMIProcessorProperties("Name"); }
        }

        public static string CPUDescription
        {
            get { return WMIProcessorProperties("Description"); }
        }

        public static string WMIProcessorProperties(String pProperties)
        {
            ManagementClass mgmtProc = new ManagementClass("Win32_Processor");
            string strInfo = string.Empty;
            if ((mgmtProc == null))
                return string.Empty;

            foreach (ManagementObject objInstance in mgmtProc.GetInstances())
            {
                string strValue = string.Empty;
                try
                {
                    strValue = objInstance.Properties[pProperties].Value.ToString();
                }
                catch
                {
                    strValue = string.Empty;
                }

                if ((!string.IsNullOrEmpty(strValue)) && (!strInfo.Contains(strValue)))
                {
                    if (!string.IsNullOrEmpty(strInfo))
                        strInfo += ", ";
                    strInfo += strValue;
                }
            }

            return strInfo;
        }

        public static string WMIComputerSystemProperties(String pProperties)
        {
            ManagementClass mgmtCS = new ManagementClass("Win32_ComputerSystem");
            string strInfo = string.Empty;

            if ((mgmtCS == null))
                return string.Empty;

            foreach (ManagementObject objInstance in mgmtCS.GetInstances())
            {
                string strValue = string.Empty;
                try
                {
                    strValue = objInstance.Properties[pProperties].Value.ToString();
                }
                catch
                {
                    strValue = string.Empty;
                }

                if ((!string.IsNullOrEmpty(strValue)) && (!strInfo.Contains(strValue)))
                {
                    if (!string.IsNullOrEmpty(strInfo))
                        strInfo += ", ";
                    strInfo += strValue;
                }
            }

            return strInfo;
        }

        #endregion

    }

}
