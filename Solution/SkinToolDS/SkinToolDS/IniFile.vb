Imports System.Collections.Generic
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text

'see http://www.codeproject.com/Articles/20053/A-Complete-Win-INI-File-Utility-Class

''' <summary>
''' Provides methods for reading and writing to an INI file.
''' </summary>
Public Class IniFile
    ''' <summary>
    ''' The maximum size of a section in an ini file.
    ''' </summary>
    ''' <remarks>
    ''' This property defines the maximum size of the buffers 
    ''' used to retreive data from an ini file.  This value is 
    ''' the maximum allowed by the win32 functions 
    ''' GetPrivateProfileSectionNames() or 
    ''' GetPrivateProfileString().
    ''' </remarks>
    Public Const MaxSectionSize As Integer = 32767
    ' 32 KB
#Region "P/Invoke declares"

    ''' <summary>
    ''' A static class that provides the win32 P/Invoke signatures 
    ''' used by this class.
    ''' </summary>
    ''' <remarks>
    ''' Note:  In each of the declarations below, we explicitly set CharSet to 
    ''' Auto.  By default in C#, CharSet is set to Ansi, which reduces 
    ''' performance on windows 2000 and above due to needing to convert strings
    ''' from Unicode (the native format for all .Net strings) to Ansi before 
    ''' marshalling.  Using Auto lets the marshaller select the Unicode version of 
    ''' these functions when available.
    ''' </remarks>
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Private NotInheritable Class NativeMethods
        Private Sub New()
        End Sub
        <DllImport("kernel32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetPrivateProfileSectionNames(lpszReturnBuffer As IntPtr, nSize As UInteger, lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetPrivateProfileString(lpAppName As String, lpKeyName As String, lpDefault As String, lpReturnedString As StringBuilder, nSize As Integer, lpFileName As String) As UInteger
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetPrivateProfileString(lpAppName As String, lpKeyName As String, lpDefault As String, <[In](), Out()> lpReturnedString As Char(), nSize As Integer, lpFileName As String) As UInteger
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetPrivateProfileString(lpAppName As String, lpKeyName As String, lpDefault As String, lpReturnedString As IntPtr, nSize As UInteger, lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetPrivateProfileInt(lpAppName As String, lpKeyName As String, lpDefault As Integer, lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetPrivateProfileSection(lpAppName As String, lpReturnedString As IntPtr, nSize As UInteger, lpFileName As String) As Integer
        End Function

        'We explicitly enable the SetLastError attribute here because
        ' WritePrivateProfileString returns errors via SetLastError.
        ' Failure to set this can result in errors being lost during 
        ' the marshal back to managed code.
        <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function WritePrivateProfileString(lpAppName As String, lpKeyName As String, lpString As String, lpFileName As String) As Boolean
        End Function


    End Class
#End Region

    'The path of the file we are operating on.
    Private ReadOnly m_path As String

    ''' <summary>
    ''' Initializes a new instance of the <see cref="IniFile"/> class.
    ''' </summary>
    ''' <param name="path">The ini file to read and write from.</param>
    Public Sub New(path As String)
        'Convert to the full path.  Because of backward compatibility, 
        ' the win32 functions tend to assume the path should be the 
        ' root Windows directory if it is not specified.  By calling 
        ' GetFullPath, we make sure we are always passing the full path
        ' the win32 functions.
        m_path = System.IO.Path.GetFullPath(path)
    End Sub

    ''' <summary>
    ''' Gets the full path of ini file this object instance is operating on.
    ''' </summary>
    ''' <value>A file path.</value>
    Public ReadOnly Property Path() As String
        Get
            Return m_path
        End Get
    End Property

#Region "Get Value Methods"

    ''' <summary>
    ''' Gets the value of a setting in an ini file as a <see cref="T:System.String"/>.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to read from.</param>
    ''' <param name="keyName">The name of the key in section to read.</param>
    ''' <param name="defaultValue">The default value to return if the key
    ''' cannot be found.</param>
    ''' <returns>The value of the key, if found.  Otherwise, returns 
    ''' <paramref name="defaultValue"/></returns>
    ''' <remarks>
    ''' The retreived value must be less than 32KB in length.
    ''' </remarks>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Function GetString(sectionName As String, keyName As String, defaultValue As String) As String
        If sectionName Is Nothing Then
            Throw New ArgumentNullException("sectionName")
        End If

        If keyName Is Nothing Then
            Throw New ArgumentNullException("keyName")
        End If

        Dim retval As New StringBuilder(IniFile.MaxSectionSize)

        NativeMethods.GetPrivateProfileString(sectionName, keyName, defaultValue, retval, IniFile.MaxSectionSize, m_path)

        Return retval.ToString()
    End Function

    ''' <summary>
    ''' Gets the value of a setting in an ini file as a <see cref="T:System.Int16"/>.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to read from.</param>
    ''' <param name="keyName">The name of the key in section to read.</param>
    ''' <param name="defaultValue">The default value to return if the key
    ''' cannot be found.</param>
    ''' <returns>The value of the key, if found.  Otherwise, returns 
    ''' <paramref name="defaultValue"/>.</returns>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Function GetInt16(sectionName As String, keyName As String, defaultValue As Short) As Short
        Dim retval As Integer = GetInt32(sectionName, keyName, defaultValue)

        Return Convert.ToInt16(retval)
    End Function

    ''' <summary>
    ''' Gets the value of a setting in an ini file as a <see cref="T:System.Int32"/>.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to read from.</param>
    ''' <param name="keyName">The name of the key in section to read.</param>
    ''' <param name="defaultValue">The default value to return if the key
    ''' cannot be found.</param>
    ''' <returns>The value of the key, if found.  Otherwise, returns 
    ''' <paramref name="defaultValue"/></returns>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Function GetInt32(sectionName As String, keyName As String, defaultValue As Integer) As Integer
        If sectionName Is Nothing Then
            Throw New ArgumentNullException("sectionName")
        End If

        If keyName Is Nothing Then
            Throw New ArgumentNullException("keyName")
        End If

        Return NativeMethods.GetPrivateProfileInt(sectionName, keyName, defaultValue, m_path)
    End Function

    ''' <summary>
    ''' Gets the value of a setting in an ini file as a <see cref="T:System.Double"/>.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to read from.</param>
    ''' <param name="keyName">The name of the key in section to read.</param>
    ''' <param name="defaultValue">The default value to return if the key
    ''' cannot be found.</param>
    ''' <returns>The value of the key, if found.  Otherwise, returns 
    ''' <paramref name="defaultValue"/></returns>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Function GetDouble(sectionName As String, keyName As String, defaultValue As Double) As Double
        Dim str As String = GetString(sectionName, keyName, "")

        If [String].IsNullOrEmpty(str) Then
            Return defaultValue
        End If

        Dim retval As Double
        If Not [Double].TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, retval) Then
            retval = defaultValue
        End If

        Return retval
    End Function

    ''' <summary>
    ''' Gets the value of a setting in an ini file as a <see cref="T:System.Boolean"/>.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to read from.</param>
    ''' <param name="keyName">The name of the key in section to read.</param>
    ''' <param name="defaultValue">The default value to return if the key
    ''' cannot be found.</param>
    ''' <returns>The value of the key, if found.  Otherwise, returns 
    ''' <paramref name="defaultValue"/></returns>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Function GetBoolean(sectionName As String, keyName As String, defaultValue As Boolean) As Boolean
        Dim str As String = GetString(sectionName, keyName, "")
        If [String].IsNullOrEmpty(str) Then
            Return defaultValue
        End If

        Dim retval As Boolean
        If Not [Boolean].TryParse(str, retval) Then
            retval = defaultValue
        End If

        Return retval
    End Function

#End Region

#Region "GetSectionValues Methods"

    ''' <summary>
    ''' Gets all of the values in a section as a list.
    ''' </summary>
    ''' <param name="sectionName">
    ''' Name of the section to retrieve values from.
    ''' </param>
    ''' <returns>
    ''' A <see cref="List{T}"/> containing <see cref="KeyValuePair{T1, T2}"/> objects 
    ''' that describe this section.  Use this verison if a section may contain
    ''' multiple items with the same key value.  If you know that a section 
    ''' cannot contain multiple values with the same key name or you don't 
    ''' care about the duplicates, use the more convenient 
    ''' <see cref="GetSectionValues"/> function.
    ''' </returns>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> is a null reference  (Nothing in VB)
    ''' </exception>
    Public Function GetSectionValuesAsList(sectionName As String) As List(Of KeyValuePair(Of String, String))
        Dim retval As List(Of KeyValuePair(Of String, String))
        Dim keyValuePairs As String()
        Dim key As String, value As String
        Dim equalSignPos As Integer

        If sectionName Is Nothing Then
            Throw New ArgumentNullException("sectionName")
        End If

        'Allocate a buffer for the returned section names.
        Dim ptr As IntPtr = Marshal.AllocCoTaskMem(IniFile.MaxSectionSize)

        Try
            'Get the section key/value pairs into the buffer.
            Dim len As Integer = NativeMethods.GetPrivateProfileSection(sectionName, ptr, IniFile.MaxSectionSize, m_path)

            keyValuePairs = ConvertNullSeperatedStringToStringArray(ptr, len)
        Finally
            'Free the buffer
            Marshal.FreeCoTaskMem(ptr)
        End Try

        'Parse keyValue pairs and add them to the list.
        retval = New List(Of KeyValuePair(Of String, String))(keyValuePairs.Length)

        For i As Integer = 0 To keyValuePairs.Length - 1
            'Parse the "key=value" string into its constituent parts
            equalSignPos = keyValuePairs(i).IndexOf("="c)

            key = keyValuePairs(i).Substring(0, equalSignPos)

            value = keyValuePairs(i).Substring(equalSignPos + 1, keyValuePairs(i).Length - equalSignPos - 1)

            retval.Add(New KeyValuePair(Of String, String)(key, value))
        Next

        Return retval
    End Function

    ''' <summary>
    ''' Gets all of the values in a section as a dictionary.
    ''' </summary>
    ''' <param name="sectionName">
    ''' Name of the section to retrieve values from.
    ''' </param>
    ''' <returns>
    ''' A <see cref="Dictionary{T, T}"/> containing the key/value 
    ''' pairs found in this section.  
    ''' </returns>
    ''' <remarks>
    ''' If a section contains more than one key with the same name, 
    ''' this function only returns the first instance.  If you need to 
    ''' get all key/value pairs within a section even when keys have the 
    ''' same name, use <see cref="GetSectionValuesAsList"/>.
    ''' </remarks>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> is a null reference  (Nothing in VB)
    ''' </exception>
    Public Function GetSectionValues(sectionName As String) As Dictionary(Of String, String)
        Dim keyValuePairs As List(Of KeyValuePair(Of String, String))
        Dim retval As Dictionary(Of String, String)

        keyValuePairs = GetSectionValuesAsList(sectionName)

        'Convert list into a dictionary.
        retval = New Dictionary(Of String, String)(keyValuePairs.Count)

        For Each keyValuePair As KeyValuePair(Of String, String) In keyValuePairs
            'Skip any key we have already seen.
            If Not retval.ContainsKey(keyValuePair.Key) Then
                retval.Add(keyValuePair.Key, keyValuePair.Value)
            End If
        Next

        Return retval
    End Function

#End Region

#Region "Get Key/Section Names"

    ''' <summary>
    ''' Gets the names of all keys under a specific section in the ini file.
    ''' </summary>
    ''' <param name="sectionName">
    ''' The name of the section to read key names from.
    ''' </param>
    ''' <returns>An array of key names.</returns>
    ''' <remarks>
    ''' The total length of all key names in the section must be 
    ''' less than 32KB in length.
    ''' </remarks>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> is a null reference  (Nothing in VB)
    ''' </exception>
    Public Function GetKeyNames(sectionName As String) As String()
        Dim len As Integer
        Dim retval As String()

        If sectionName Is Nothing Then
            Throw New ArgumentNullException("sectionName")
        End If

        'Allocate a buffer for the returned section names.
        Dim ptr As IntPtr = Marshal.AllocCoTaskMem(IniFile.MaxSectionSize)

        Try
            'Get the section names into the buffer.
            len = NativeMethods.GetPrivateProfileString(sectionName, Nothing, Nothing, ptr, IniFile.MaxSectionSize, m_path)

            retval = ConvertNullSeperatedStringToStringArray(ptr, len)
        Finally
            'Free the buffer
            Marshal.FreeCoTaskMem(ptr)
        End Try

        Return retval
    End Function

    ''' <summary>
    ''' Gets the names of all sections in the ini file.
    ''' </summary>
    ''' <returns>An array of section names.</returns>
    ''' <remarks>
    ''' The total length of all section names in the section must be 
    ''' less than 32KB in length.
    ''' </remarks>
    Public Function GetSectionNames() As String()
        Dim retval As String()
        Dim len As Integer

        'Allocate a buffer for the returned section names.
        Dim ptr As IntPtr = Marshal.AllocCoTaskMem(IniFile.MaxSectionSize)

        Try
            'Get the section names into the buffer.
            len = NativeMethods.GetPrivateProfileSectionNames(ptr, IniFile.MaxSectionSize, m_path)

            retval = ConvertNullSeperatedStringToStringArray(ptr, len)
        Finally
            'Free the buffer
            Marshal.FreeCoTaskMem(ptr)
        End Try

        Return retval
    End Function

    ''' <summary>
    ''' Converts the null seperated pointer to a string into a string array.
    ''' </summary>
    ''' <param name="ptr">A pointer to string data.</param>
    ''' <param name="valLength">
    ''' Length of the data pointed to by <paramref name="ptr"/>.
    ''' </param>
    ''' <returns>
    ''' An array of strings; one for each null found in the array of characters pointed
    ''' at by <paramref name="ptr"/>.
    ''' </returns>
    Private Shared Function ConvertNullSeperatedStringToStringArray(ptr As IntPtr, valLength As Integer) As String()
        Dim retval As String()

        If valLength = 0 Then
            'Return an empty array.
            retval = New String(-1) {}
        Else
            'Convert the buffer into a string.  Decrease the length 
            'by 1 so that we remove the second null off the end.
            Dim buff As String = Marshal.PtrToStringAuto(ptr, valLength - 1)

            'Parse the buffer into an array of strings by searching for nulls.
            retval = buff.Split(ControlChars.NullChar)
        End If

        Return retval
    End Function

#End Region

#Region "Write Methods"

    ''' <summary>
    ''' Writes a <see cref="T:System.String"/> value to the ini file.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to write to .</param>
    ''' <param name="keyName">The name of the key to write to.</param>
    ''' <param name="value">The string value to write</param>
    ''' <exception cref="T:System.ComponentModel.Win32Exception">
    ''' The write failed.
    ''' </exception>
    Private Sub WriteValueInternal(sectionName As String, keyName As String, value As String)
        If Not NativeMethods.WritePrivateProfileString(sectionName, keyName, value, m_path) Then
            Throw New System.ComponentModel.Win32Exception()
        End If
    End Sub

    ''' <summary>
    ''' Writes a <see cref="T:System.String"/> value to the ini file.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to write to .</param>
    ''' <param name="keyName">The name of the key to write to.</param>
    ''' <param name="value">The string value to write</param>
    ''' <exception cref="T:System.ComponentModel.Win32Exception">
    ''' The write failed.
    ''' </exception>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> or 
    ''' <paramref name="value"/>  are a null reference  (Nothing in VB)
    ''' </exception>
    Public Sub WriteValue(sectionName As String, keyName As String, value As String)
        If sectionName Is Nothing Then
            Throw New ArgumentNullException("sectionName")
        End If

        If keyName Is Nothing Then
            Throw New ArgumentNullException("keyName")
        End If

        If value Is Nothing Then
            Throw New ArgumentNullException("value")
        End If

        WriteValueInternal(sectionName, keyName, value)
    End Sub

    ''' <summary>
    ''' Writes an <see cref="T:System.Int16"/> value to the ini file.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to write to .</param>
    ''' <param name="keyName">The name of the key to write to.</param>
    ''' <param name="value">The value to write</param>
    ''' <exception cref="T:System.ComponentModel.Win32Exception">
    ''' The write failed.
    ''' </exception>
    Public Sub WriteValue(sectionName As String, keyName As String, value As Short)
        WriteValue(sectionName, keyName, CInt(value))
    End Sub

    ''' <summary>
    ''' Writes an <see cref="T:System.Int32"/> value to the ini file.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to write to .</param>
    ''' <param name="keyName">The name of the key to write to.</param>
    ''' <param name="value">The value to write</param>
    ''' <exception cref="T:System.ComponentModel.Win32Exception">
    ''' The write failed.
    ''' </exception>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Sub WriteValue(sectionName As String, keyName As String, value As Integer)
        WriteValue(sectionName, keyName, value.ToString(CultureInfo.InvariantCulture))
    End Sub

    ''' <summary>
    ''' Writes an <see cref="T:System.Single"/> value to the ini file.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to write to .</param>
    ''' <param name="keyName">The name of the key to write to.</param>
    ''' <param name="value">The value to write</param>
    ''' <exception cref="T:System.ComponentModel.Win32Exception">
    ''' The write failed.
    ''' </exception>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Sub WriteValue(sectionName As String, keyName As String, value As Single)
        WriteValue(sectionName, keyName, value.ToString(CultureInfo.InvariantCulture))
    End Sub

    ''' <summary>
    ''' Writes a <see cref="T:System.Double"/> value to the ini file.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to write to .</param>
    ''' <param name="keyName">The name of the key to write to.</param>
    ''' <param name="value">The value to write</param>
    ''' <exception cref="T:System.ComponentModel.Win32Exception">
    ''' The write failed.
    ''' </exception>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Sub WriteValue(sectionName As String, keyName As String, value As Double)
        WriteValue(sectionName, keyName, value.ToString(CultureInfo.InvariantCulture))
    End Sub

    ''' <summary>
    ''' Writes a <see cref="T:System.Boolean"/> value to the ini file.
    ''' </summary>
    ''' <param name="sectionName">The name of the section to write to .</param>
    ''' <param name="keyName">The name of the key to write to.</param>
    ''' <param name="value">The value to write</param>
    ''' <exception cref="T:System.ComponentModel.Win32Exception">
    ''' The write failed.
    ''' </exception>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Sub WriteValue(sectionName As String, keyName As String, value As Boolean)
        WriteValue(sectionName, keyName, value.ToString(CultureInfo.InvariantCulture))
    End Sub

#End Region

#Region "Delete Methods"

    ''' <summary>
    ''' Deletes the specified key from the specified section.
    ''' </summary>
    ''' <param name="sectionName">
    ''' Name of the section to remove the key from.
    ''' </param>
    ''' <param name="keyName">
    ''' Name of the key to remove.
    ''' </param>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> or <paramref name="keyName"/> are 
    ''' a null reference  (Nothing in VB)
    ''' </exception>
    Public Sub DeleteKey(sectionName As String, keyName As String)
        If sectionName Is Nothing Then
            Throw New ArgumentNullException("sectionName")
        End If

        If keyName Is Nothing Then
            Throw New ArgumentNullException("keyName")
        End If

        WriteValueInternal(sectionName, keyName, Nothing)
    End Sub

    ''' <summary>
    ''' Deletes a section from the ini file.
    ''' </summary>
    ''' <param name="sectionName">
    ''' Name of the section to delete.
    ''' </param>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="sectionName"/> is a null reference (Nothing in VB)
    ''' </exception>
    Public Sub DeleteSection(sectionName As String)
        If sectionName Is Nothing Then
            Throw New ArgumentNullException("sectionName")
        End If

        WriteValueInternal(sectionName, Nothing, Nothing)
    End Sub

#End Region
End Class
