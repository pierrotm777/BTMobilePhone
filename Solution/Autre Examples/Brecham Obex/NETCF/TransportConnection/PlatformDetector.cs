using System;
using System.Text;

public class PlatformDetector
{
    [System.Runtime.InteropServices.DllImport("coredll.dll")]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, StringBuilder pvParam, uint fWinIni);

    private static uint SPI_GETPLATFORMTYPE = 257;

    public static Platform GetPlatform()
    {
        Platform plat = Platform.Unknown;
        switch (System.Environment.OSVersion.Platform) {
            case PlatformID.Win32NT:
                plat = Platform.Win32NT;
                break;
            case PlatformID.Win32S:
                plat = Platform.Win32S;
                break;
            case PlatformID.Win32Windows:
                plat = Platform.Win32Windows;
                break;
            case PlatformID.WinCE:
                plat = CheckWinCEPlatform();
                break;
        }

        return plat;
    }


    // TODO Also: [[On the Smartphone, the call "SystemParametersInfo()" might fail 
    // with "Access Denied" depending on the lock-down of the Smartphone and the 
    // certificate your application is signed with. Since it's the only platform 
    // where that call fails with Access Denied you can also use this as a sign 
    // for Smartphone.]]
    static Platform CheckWinCEPlatform()
    {
        Platform plat = Platform.WindowsCE;
        StringBuilder strbuild = new StringBuilder(200);
        SystemParametersInfo(SPI_GETPLATFORMTYPE, 200, strbuild, 0);
        string str = strbuild.ToString();
        switch (str) {
            case "PocketPC":
                plat = Platform.PocketPC;
                break;
            case "SmartPhone":
                // Note that the strbuild parameter from the
                // PInvoke returns "SmartPhone" with an
                // upper case P. The correct casing is
                // "Smartphone" with a lower case p.
                plat = Platform.Smartphone;
                break;
        }
        return plat;
    }
}

public enum Platform
{
    PocketPC, WindowsCE, Smartphone, Win32NT, Win32S, Win32Windows, Unknown
}



