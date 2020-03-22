Module OsName
    Public Enum OsNames
        Windows95
        Windows98
        WindowsME
        WindowsNT3
        WindowsNT4
        Windows2000
        WindowsXP
        Windows2003
        WindowsVista
        Windows7
        Windows8
        Windows8_1
        Windows10
        Unknown

    End Enum

    'How to use this module
    'Select Case GetOSName()
    '    Case OsNames.Windows7, OsNames.WindowsVista
    '        ' j’active ici mes contrôles pour Windows 7 et Vista
    '    Case OsNames.WindowsXP
    '        ' Je désactive ici mes contrôles pour Windows 7 et j’active ceux pour Windows XP
    'End Select
    Public Function GetOSName() As OsNames
        Dim os As System.OperatingSystem = System.Environment.OSVersion
        Dim osName As String = OsNames.Unknown
        Select Case os.Platform
            Case System.PlatformID.Win32Windows
                Select Case os.Version.Minor
                    Case 0
                        osName = OsNames.Windows95
                        Exit Select
                    Case 10
                        osName = OsNames.Windows98
                        Exit Select
                    Case 90
                        osName = OsNames.WindowsME
                        Exit Select
                End Select
                Exit Select
            Case System.PlatformID.Win32NT
                Select Case os.Version.Major
                    Case 3
                        osName = OsNames.WindowsNT3
                        Exit Select
                    Case 4
                        osName = OsNames.WindowsNT4
                        Exit Select
                    Case 5
                        If os.Version.Minor = 0 Then
                            osName = OsNames.Windows2000
                        ElseIf os.Version.Minor = 1 Then
                            osName = OsNames.WindowsXP
                        ElseIf os.Version.Minor = 2 Then
                            osName = OsNames.Windows2003
                        End If
                        Exit Select
                    Case 6
                        osName = OsNames.WindowsVista
                        If os.Version.Minor = 0 Then
                            osName = OsNames.WindowsVista
                        ElseIf os.Version.Minor = 1 Then
                            osName = OsNames.Windows7
                        ElseIf os.Version.Minor = 2 Then
                            osName = OsNames.Windows8
                        ElseIf os.Version.Minor = 3 Then
                            osName = OsNames.Windows8_1
                        End If
                        Exit Select
                    Case 10
                        osName = OsNames.Windows10

                End Select
                Exit Select
        End Select
        Return osName
    End Function
End Module

