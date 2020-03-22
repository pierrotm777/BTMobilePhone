Imports System.Collections.Generic
Imports System.Globalization
Imports System.Threading
Imports System.Reflection
Imports System.Web

Imports System.Runtime.Remoting.Contexts

Imports System.Xml
Imports System.Xml.XPath
Imports System.Text

Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions

Module modGlobalDeclare

#Region "Variables"
    Public PluginSettings As cMySettings
    Public TempPluginSettings As cMySettings
    Public MainPath As String
    Public Mac As String
    Public MacSplit() As String
    Public ADDRS(6) As Byte
    Public dialNumber As String = ""
    Public PhoneNumberOfContactsFound As Integer
    Public PhoneNumberOfContactsFound1 As Integer = 0
    Public PhoneNumberOfContactsLoop As Integer = 0
    Public PhoneNumberOfContactsFoundFinal As UInt32
    Public CurrentPhoneBook As String
    Public phoneBookData As phoneBook
    Public PhoneBookList_A As String
    Public PhoneBookList_B As String
    Public FileName_A As String
    Public FileName_B As String
    Public TextLine_A As String
    Public TextLine_B As String
    Public TextLine_Match As String
    Public PhoneListType As String
    Public PhoneListTypeUsable As String
    Public PhoneSatus As String
    Public Subscriber As String
    Public PhoneDate As String
    Public PhoneTime As String
    Public SmsType As String
    Public SmsToSend As String
    Public SmsToRead As String
    Public PDUSMSDecoded As String
    Public PDUSMSEncoded As String
    Public PDUSMSEncodedLenght As String
    Public PDUTextModesUsable As Integer
    Public PDUSMSIsReadyToRead As Boolean = False
    Public PDUEventNb As Integer = 0
    Public PDUSMSReceivedPhoneNumber As String
    Public PDUSMSReceivedDate As String
    Public PDUSMSReceivedTime As String
    Public PDUSMSReceivedMessage As String
    Public NewSmsIsReceived As String
    Public NewSmsIsReceivedNumber As String
    Public NewSmsIsReceivedMemory As String
    Public SmsIsReceivedStatus As Boolean = False
    Public SmsIsReceivedType As String
    Public NumberOfSMSInMemoryOld As String
    Public NumberOfSMSInMemoryNew As String
    Public SmsCenterServiceNumber As String
    Public IncomingCall As Boolean = False
    Public CallerName As String
    Public CallerID As String
    Public ImeiNumber As String
    Public IsmiNumber As String
    Public RevisionNumber As String
    Public OperatorNames As String
    Public SynchroningInProgress As Boolean = False
    Public PhoneBookSortInProgress As Boolean = False
    Public PhoneBookSyncInProgress As Boolean = False
    Public PhoneBookMergeInProgress As Boolean = False
    Public PhoneBookSyncFlag As Integer = 0
    Public CallIsActif As Boolean = False
    Public ExternalPowerIsConnected As Boolean = False
    Public BatteryFullCharge As Boolean = False
    Public debugFrm As New frmDebugBox
    Public ATValueResult As String
    Public IconDeviceType As String
    Public MAPServiceIsReady As Boolean = False
    Public MAPInfo As String
    Public ServiceList As New Dictionary(Of String, String)
    Public PBAPServiceIsReady As Boolean = False
    Public CountryList As New Dictionary(Of String, Integer)
    Dim phoneprefix As String
    Public PhoneMacAddress As String

    Public PhoneCheckedIs As Integer = 1
    Public Phone2IsSelected As Boolean = False
    Public Phone2 As Integer = 1

    Public SpeechRecognitionIsActive As Boolean = False
    Public SpeechRecognizedIs As String = ""
    Public SpeechToNumberError As String = ""
    Public SpeechToNumberList As New Dictionary(Of String, Integer)
    Public SpeechArray() As String
    Public MyCultureInfo As String = ""
    Public MyPhoneCultureInfo As String = ""
    Public PhoneBookNameArray() As String

    Public SendBytes As UInt32
    Public ReceivedBytes As UInt32

    Public SearchThread As System.Threading.Thread
    Public workerThread As Threading.Thread
    'Public threadRunBTInit As Thread

    'Alarm variables
    Public AlarmIndicator_Monday As Boolean = False
    Public AlarmIndicator_Tuesday As Boolean = False
    Public AlarmIndicator_Wednesday As Boolean = False
    Public AlarmIndicator_Thirsday As Boolean = False
    Public AlarmIndicator_Friday As Boolean = False
    Public AlarmIndicator_Saturday As Boolean = False
    Public AlarmIndicator_Sunday As Boolean = False
    Public AlarmIndicator_Week As Boolean = False
    Public Alarm1IsOn As Boolean = False
    Public Alarm2IsOn As Boolean = False
    Public Alarm3IsOn As Boolean = False
    Public Alarm4IsOn As Boolean = False
    Public Alarm5IsOn As Boolean = False
    Public AlarmAttributes As String



    Public CellularPhoneValue As String
    Public DepartmentValue As String
    Public EmailAddressValue As String
    Public FormattedNameValue As String
    Public HomePhoneValue As String
    Public JobTitleValue As String
    Public OfficeValue As String
    Public OrganizationValue As String
    Public PagerValue As String
    Public PersonalWebSiteValue As String
    Public WorkFaxValue As String
    Public WorkPhoneValue As String
    Public WorkWebSiteValue As String

    Public SDKErrorList As New Dictionary(Of UInteger, String)
#End Region

#Region "Fonctions & Sub"
    Public Function BlueSoleil_GetSDKDLLfilename() As String
        Dim tempStr As String = System.Environment.GetFolderPath(Environment.SpecialFolder.System)
        If Right(tempStr, 1) <> "\" Then tempStr = tempStr & "\"
        tempStr = tempStr & "bsSDK.dll"
        Return (tempStr)
    End Function

    Public Function BlueSoleil_IsInstalled() As Boolean
        Dim retBool As Boolean = False
        Dim tempStr As String = BlueSoleil_GetSDKDLLfilename()
        retBool = IO.File.Exists(tempStr)
        Return retBool
    End Function

    Public Function GetVersionSdkDll() As String
        ' Get the file version for the bsSDK.dll. 
        Dim tempStr As String = BlueSoleil_GetSDKDLLfilename()
        FileVersionInfo.GetVersionInfo(tempStr)
        Dim myFileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(tempStr)
        ' Print the file name and version number.
        'Console.WriteLine("File: " + myFileVersionInfo.FileDescription + vbLf + "Version number: " + myFileVersionInfo.FileVersion)
        tempStr = myFileVersionInfo.FileVersion
        Return tempStr
    End Function

    '''''''''''''''''''''''''''''''''''
    ''' Convert Hexa --> Decimal    '''
    '''''''''''''''''''''''''''''''''''
    Public Function HexaToDec(ValHex As String) As Integer
        Return Val("&H" & ValHex) '& "&")
    End Function

    Public Sub CreateServicesList()
        ServiceList.Add("1101", "Serial Port service.")
        ServiceList.Add("1102", "LAN Access service.")
        ServiceList.Add("1103", "Dial-up Networking service.")
        ServiceList.Add("1104", "Synchronization service.")
        ServiceList.Add("1105", "Object Push service.")
        ServiceList.Add("1106", "File Transfer service.")
        ServiceList.Add("1107", "IrMC Sync Command service.")
        ServiceList.Add("1108", "Headset service.")
        ServiceList.Add("1109", "Cordless Telephony service.")
        ServiceList.Add("110A", "Audio Source service.")
        ServiceList.Add("110B", "Audio Sink service.")
        ServiceList.Add("110C", "A/V Remote Control Target service.")
        ServiceList.Add("110D", "Advanced Audio Distribution service.")
        ServiceList.Add("110E", "A/V Remote Control service.")
        ServiceList.Add("110F", "Video conference service.")
        ServiceList.Add("1110", "Intercom service.")
        ServiceList.Add("1111", "Fax service.")
        ServiceList.Add("1112", "Headset Audio Gateway service.")
        ServiceList.Add("1113", "WAP service.")
        ServiceList.Add("1114", "WAP client service.")
        ServiceList.Add("1115", "PANU service.")
        ServiceList.Add("1116", "NAP service.")
        ServiceList.Add("1117", "GN service.")
        ServiceList.Add("1118", "Direct Print service.")
        ServiceList.Add("1119", "Referenced Print service.")
        ServiceList.Add("111A", "Imaging service.")
        ServiceList.Add("111B", "Imaging Responder service.")
        ServiceList.Add("111C", "Imaging Automatic Archive service.")
        ServiceList.Add("111D", "Imaging Referenced Objects service.")
        ServiceList.Add("111E", "Hands-free service.")
        ServiceList.Add("111F", "Hands-free Audio Gateway service.")
        ServiceList.Add("1120", "DPS Referenced Objects service.")
        ServiceList.Add("1121", "Reflected UI service.")
        ServiceList.Add("1122", "Basic Print service.")
        ServiceList.Add("1123", "Print Status service.")
        ServiceList.Add("1124", "Human Interface Device service.")
        ServiceList.Add("1125", "Hardcopy Cable Replacement service.")
        ServiceList.Add("1126", "HCRP Print service.")
        ServiceList.Add("1127", "HCRP Scan service.")
        ServiceList.Add("112D", "SIM Card Access service.")
        ServiceList.Add("112E", "PBAP Phonebook Client Equipment service.")
        ServiceList.Add("112F", "PBAP Phonebook Server Equipment service.")
        ServiceList.Add("1130", "Phonebook Access service.")
        ServiceList.Add("1200", "Bluetooth Device Identification.")
    End Sub

    Public Function PhonePrefixFromCountry() As String
        Dim sp() As String
        Try
            phoneprefix = ""
            sp = CultureInfo.CurrentCulture.Name.Split("-")
            phoneprefix = sp(1)
            Return phoneprefix
        Catch ex As Exception
            Return "error"
            MsgBox(ex.Message)
        End Try
    End Function

    Public Sub CreateCountryList()
        CountryList.Add("Afghanistan", 93)
        CountryList.Add("Albania", 355)
        CountryList.Add("Algeria", 213)
        CountryList.Add("American Samoa", 1684)
        CountryList.Add("Andorra", 376)
        CountryList.Add("Angola", 244)
        CountryList.Add("Anguilla", 1264)
        CountryList.Add("Antarctica", 672)
        CountryList.Add("Antigua and Barbuda", 1268)
        CountryList.Add("Argentina", 54)
        CountryList.Add("Armenia", 374)
        CountryList.Add("Aruba", 297)
        CountryList.Add("Australia", 61)
        CountryList.Add("Austria", 43)
        CountryList.Add("Azerbaijan", 994)
        CountryList.Add("Bahamas", 1242)
        CountryList.Add("Bahrain", 973)
        CountryList.Add("Bangladesh", 880)
        CountryList.Add("Barbados", 1246)
        CountryList.Add("Belarus", 375)
        CountryList.Add("Belgium", 32)
        CountryList.Add("Belize", 501)
        CountryList.Add("Benin", 229)
        CountryList.Add("Bermuda", 1441)
        CountryList.Add("Bhutan", 975)
        CountryList.Add("Bolivia", 591)
        CountryList.Add("Bosnia and Herzegovina", 387)
        CountryList.Add("Botswana", 267)
        CountryList.Add("Brazil", 55)
        'CountryList.Add("British Indian Ocean Territory", )
        CountryList.Add("British Virgin Islands", 1284)
        CountryList.Add("Brunei", 673)
        CountryList.Add("Bulgaria", 359)
        CountryList.Add("Burkina Faso", 226)
        CountryList.Add("Burma (Myanmar)", 95)
        CountryList.Add("Burundi", 257)
        CountryList.Add("Cambodia", 855)
        CountryList.Add("Cameroon", 237)
        CountryList.Add("Canada", 1)
        CountryList.Add("Cape Verde", 238)
        CountryList.Add("Cayman Islands", 1345)
        CountryList.Add("Central African Republic", 236)
        CountryList.Add("Chad", 235)
        CountryList.Add("Chile", 56)
        CountryList.Add("China", 86)
        CountryList.Add("Christmas Island", 61)
        CountryList.Add("Cocos (Keeling) Islands", 61)
        CountryList.Add("Colombia", 57)
        CountryList.Add("Comoros", 269)
        CountryList.Add("Cook Islands", 682)
        CountryList.Add("Costa Rica", 506)
        CountryList.Add("Croatia", 385)
        CountryList.Add("Cuba", 53)
        CountryList.Add("Cyprus", 357)
        CountryList.Add("Czech Republic", 420)
        CountryList.Add("Democratic Republic of the Congo", 243)
        CountryList.Add("Denmark", 45)
        CountryList.Add("Djibouti", 253)
        CountryList.Add("Dominica", 1767)
        CountryList.Add("Dominican Republic", 1809)
        CountryList.Add("Ecuador", 593)
        CountryList.Add("Egypt", 20)
        CountryList.Add("El Salvador", 503)
        CountryList.Add("Equatorial Guinea", 240)
        CountryList.Add("Eritrea", 291)
        CountryList.Add("Estonia", 372)
        CountryList.Add("Ethiopia", 251)
        CountryList.Add("Falkland Islands", 500)
        CountryList.Add("Faroe Islands", 298)
        CountryList.Add("Fiji", 679)
        CountryList.Add("Finland", 358)
        CountryList.Add("France", 33)
        CountryList.Add("French Polynesia", 689)
        CountryList.Add("Gabon", 241)
        CountryList.Add("Gambia", 220)
        CountryList.Add("Gaza Strip", 970)
        CountryList.Add("Georgia", 995)
        CountryList.Add("Germany", 49)
        CountryList.Add("Ghana", 233)
        CountryList.Add("Gibraltar", 350)
        CountryList.Add("Greece", 30)
        CountryList.Add("Greenland", 299)
        CountryList.Add("Grenada", 1473)
        CountryList.Add("Guam", 1671)
        CountryList.Add("Guatemala", 502)
        CountryList.Add("Guinea", 224)
        CountryList.Add("Guinea-Bissau", 245)
        CountryList.Add("Guyana", 592)
        CountryList.Add("Haiti", 509)
        CountryList.Add("Holy See (Vatican City)", 39)
        CountryList.Add("Honduras", 504)
        CountryList.Add("Hong Kong", 852)
        CountryList.Add("Hungary", 36)
        CountryList.Add("Iceland", 354)
        CountryList.Add("India", 91)
        CountryList.Add("Indonesia", 62)
        CountryList.Add("Iran", 98)
        CountryList.Add("Iraq", 964)
        CountryList.Add("Ireland", 353)
        CountryList.Add("Isle of Man", 44)
        CountryList.Add("Israel", 972)
        CountryList.Add("Italy", 39)
        CountryList.Add("Ivory Coast", 225)
        CountryList.Add("Jamaica", 1876)
        CountryList.Add("Japan", 81)
        'CountryList.Add("Jersey", )
        CountryList.Add("Jordan", 962)
        CountryList.Add("Kazakhstan", 7)
        CountryList.Add("Kenya", 254)
        CountryList.Add("Kiribati", 686)
        CountryList.Add("Kosovo", 381)
        CountryList.Add("Kuwait", 965)
        CountryList.Add("Kyrgyzstan", 996)
        CountryList.Add("Laos", 856)
        CountryList.Add("Latvia", 371)
        CountryList.Add("Lebanon", 961)
        CountryList.Add("Lesotho", 266)
        CountryList.Add("Liberia", 231)
        CountryList.Add("Libya", 218)
        CountryList.Add("Liechtenstein", 423)
        CountryList.Add("Lithuania", 370)
        CountryList.Add("Luxembourg", 352)
        CountryList.Add("Macau", 853)
        CountryList.Add("Macedonia", 389)
        CountryList.Add("Madagascar", 261)
        CountryList.Add("Malawi", 265)
        CountryList.Add("Malaysia", 60)
        CountryList.Add("Maldives", 960)
        CountryList.Add("Mali", 223)
        CountryList.Add("Malta", 356)
        CountryList.Add("Marshall Islands", 692)
        CountryList.Add("Mauritania", 222)
        CountryList.Add("Mauritius", 230)
        CountryList.Add("Mayotte", 262)
        CountryList.Add("Mexico", 52)
        CountryList.Add("Micronesia", 691)
        CountryList.Add("Moldova", 373)
        CountryList.Add("Monaco", 377)
        CountryList.Add("Mongolia", 976)
        CountryList.Add("Montenegro", 382)
        CountryList.Add("Montserrat", 1664)
        CountryList.Add("Morocco", 212)
        CountryList.Add("Mozambique", 258)
        CountryList.Add("Namibia", 264)
        CountryList.Add("Nauru", 674)
        CountryList.Add("Nepal", 977)
        CountryList.Add("Netherlands", 31)
        CountryList.Add("Netherlands Antilles", 599)
        CountryList.Add("New Caledonia", 687)
        CountryList.Add("New Zealand", 64)
        CountryList.Add("Nicaragua", 505)
        CountryList.Add("Niger", 227)
        CountryList.Add("Nigeria", 234)
        CountryList.Add("Niue", 683)
        CountryList.Add("Norfolk Island", 672)
        CountryList.Add("North Korea", 850)
        CountryList.Add("Northern Mariana Islands", 1670)
        CountryList.Add("Norway", 47)
        CountryList.Add("Oman", 968)
        CountryList.Add("Pakistan", 92)
        CountryList.Add("Palau", 680)
        CountryList.Add("Panama", 507)
        CountryList.Add("Papua New Guinea", 675)
        CountryList.Add("Paraguay", 595)
        CountryList.Add("Peru", 51)
        CountryList.Add("Philippines", 63)
        CountryList.Add("Pitcairn Islands", 870)
        CountryList.Add("Poland", 48)
        CountryList.Add("Portugal", 351)
        CountryList.Add("Puerto Rico", 1)
        CountryList.Add("Qatar", 974)
        CountryList.Add("Republic of the Congo", 242)
        CountryList.Add("Romania", 40)
        CountryList.Add("Russia", 7)
        CountryList.Add("Rwanda", 250)
        CountryList.Add("Saint Barthelemy", 590)
        CountryList.Add("Saint Helena", 290)
        CountryList.Add("Saint Kitts and Nevis", 1869)
        CountryList.Add("Saint Lucia", 1758)
        CountryList.Add("Saint Martin", 1599)
        CountryList.Add("Saint Pierre and Miquelon", 508)
        CountryList.Add("Saint Vincent and the Grenadines", 1784)
        CountryList.Add("Samoa", 685)
        CountryList.Add("San Marino", 378)
        CountryList.Add("Sao Tome and Principe", 239)
        CountryList.Add("Saudi Arabia", 966)
        CountryList.Add("Senegal", 221)
        CountryList.Add("Serbia", 381)
        CountryList.Add("Seychelles", 248)
        CountryList.Add("Sierra Leone", 232)
        CountryList.Add("Singapore", 65)
        CountryList.Add("Slovakia", 421)
        CountryList.Add("Slovenia", 386)
        CountryList.Add("Solomon Islands", 677)
        CountryList.Add("Somalia", 252)
        CountryList.Add("South Africa", 27)
        CountryList.Add("South Korea", 82)
        CountryList.Add("Spain", 34)
        CountryList.Add("Sri Lanka", 94)
        CountryList.Add("Sudan", 249)
        CountryList.Add("Suriname", 597)
        'CountryList.Add("Svalbard", )
        CountryList.Add("Swaziland", 268)
        CountryList.Add("Sweden", 46)
        CountryList.Add("Switzerland", 41)
        CountryList.Add("Syria", 963)
        CountryList.Add("Taiwan", 886)
        CountryList.Add("Tajikistan", 992)
        CountryList.Add("Tanzania", 255)
        CountryList.Add("Thailand", 66)
        CountryList.Add("Timor-Leste", 670)
        CountryList.Add("Togo", 228)
        CountryList.Add("Tokelau", 690)
        CountryList.Add("Tonga", 676)
        CountryList.Add("Trinidad and Tobago", 1868)
        CountryList.Add("Tunisia", 216)
        CountryList.Add("Turkey", 90)
        CountryList.Add("Turkmenistan", 993)
        CountryList.Add("Turks and Caicos Islands", 1649)
        CountryList.Add("Tuvalu", 688)
        CountryList.Add("Uganda", 256)
        CountryList.Add("Ukraine", 380)
        CountryList.Add("United Arab Emirates", 971)
        CountryList.Add("United Kingdom", 44)
        CountryList.Add("United States", 1)
        CountryList.Add("Uruguay", 598)
        CountryList.Add("US Virgin Islands", 1340)
        CountryList.Add("Uzbekistan", 998)
        CountryList.Add("Vanuatu", 678)
        CountryList.Add("Venezuela", 58)
        CountryList.Add("Vietnam", 84)
        CountryList.Add("Wallis and Futuna", 681)
        CountryList.Add("West Bank", 970)
        'CountryList.Add("Western Sahara", )
        CountryList.Add("Yemen", 967)
        CountryList.Add("Zambia", 260)
        CountryList.Add("Zimbabwe", 263)

    End Sub

    Public Function CountryToPhoneIndicatif(ByVal country As String) As String
        CountryToPhoneIndicatif = ""
        Dim finalstring As String = country.Substring(country.IndexOf("("), (country.LastIndexOf(")") - country.IndexOf("(")) + 1)
        finalstring = finalstring.Replace("(", "").Replace(")", "")
        CountryList.Clear()
        CreateCountryList()
        For Each pair In CountryList
            If pair.Key = finalstring Then CountryToPhoneIndicatif = pair.Value.ToString
        Next
        CountryList.Clear()
        Return CountryToPhoneIndicatif
    End Function

    Public Sub CreateListSDKError()
        SDKErrorList.Add(&H0, "The operation completed successfully.")
        SDKErrorList.Add(&HC0, "Local service is still active. When the application tries to remove or activate an active service, this error code is returned.")
        SDKErrorList.Add(&HC1, "No service record with the specified search pattern is found on the remote device.")
        SDKErrorList.Add(&HC2, "The specified service record does not exist on the remote device.")
        SDKErrorList.Add(&H301, "The object specified by the handle does not exist in local BlueSoleil SDK database.")
        SDKErrorList.Add(&H302, "The operation fails for an undefined reason.")
        SDKErrorList.Add(&H303, "BlueSoleil SDK has not been initialized.")
        SDKErrorList.Add(&H304, "The parameter value is invalid.")
        SDKErrorList.Add(&H305, "The pointer value is NULL.")
        SDKErrorList.Add(&H306, "Not enough storage is available to process this function.")
        SDKErrorList.Add(&H307, "The specified buffer size is too small to hold the required information.")
        SDKErrorList.Add(&H308, "The specified function is not supported by the BlueSoleil.")
        SDKErrorList.Add(&H309, "No fixed PIN code is available.")
        SDKErrorList.Add(&H30A, "The specified service has been connected already.")
        SDKErrorList.Add(&H30B, "The request can.t be processed since a same request is being processed.")
        SDKErrorList.Add(&H30C, "The limit of connection number is reached.")
        SDKErrorList.Add(&H30D, "An object with the specified attribute exists.")
        SDKErrorList.Add(&H30E, "The specified object is accessed by other process. It can‟t be removed or modified.")
        SDKErrorList.Add(&H30F, "The specified remote device is not paired.")
        SDKErrorList.Add(&H401, "HCI error 'Unknown ,HCI Command (&H01)' is received.")
        SDKErrorList.Add(&H402, "HCI error 'Unknown Connection Identifier (&H02)' is received.")
        SDKErrorList.Add(&H403, "HCI error 'Hardware Failure (&H03)' is received.")
        SDKErrorList.Add(&H404, "HCI error 'Page Timeout (&H04)' is received.")
        SDKErrorList.Add(&H405, "HCI error 'Authentication Failure (&H05)' is received.")
        SDKErrorList.Add(&H406, "HCI error 'PIN or Key Missing (&H06)' is received.")
        SDKErrorList.Add(&H407, "HCI error 'Memory Capacity Exceeded (&H07)' is received.")
        SDKErrorList.Add(&H408, "HCI error 'Connection Timeout (&H08)' is received.")
        SDKErrorList.Add(&H409, "HCI error 'Connection Limit Exceeded (&H09)' is received.")
        SDKErrorList.Add(&H40A, "HCI error 'Synchronous Connection Limit to a Device Exceeded (&H0A)' is received.")
        SDKErrorList.Add(&H40B, "HCI error 'ACL Connection Already Exists (&H0B)' is received.")
        SDKErrorList.Add(&H40C, "HCI error 'Command Disallowed (&H0C)' is received.")
        SDKErrorList.Add(&H40D, "HCI error 'Connection Rejected due to Limited Resources (&H0D)' is received.")
        SDKErrorList.Add(&H40E, "HCI error 'Connection Rejected due to Security Reasons (&H0E)' is received.")
        SDKErrorList.Add(&H40F, "HCI error 'Connection Rejected due to Unacceptable BD_ADDR (&H0F)' is received.")
        SDKErrorList.Add(&H410, "HCI error 'Connection Accept Timeout Exceeded (0X10)' is received.")
        SDKErrorList.Add(&H411, "HCI error 'Unsupported Feature or Parameter Value (0X11)' is received.")
        SDKErrorList.Add(&H412, "HCI error 'Invalid ,HCI Command parameters (0X12)' is received.")
        SDKErrorList.Add(&H413, "HCI error 'Remote User Terminated Connection (0X13)' is received.")
        SDKErrorList.Add(&H414, "HCI error 'Remote Device Terminated Connection due to Low Resources (0X14)' is received.")
        SDKErrorList.Add(&H415, "HCI error 'Remote Device Terminated Connection due to Power Off (0X15)' is received.")
        SDKErrorList.Add(&H416, "HCI error 'Connection Terminated by Local Host (0X16)' is received.")
        SDKErrorList.Add(&H417, "HCI error 'Repeated Attempts (0X17)' is received.")
        SDKErrorList.Add(&H418, "HCI error 'Pairing Not Allowed (0X18)' is received.")
        SDKErrorList.Add(&H419, "HCI error 'Unknown LMP PDU (0X19)' is received.")
        SDKErrorList.Add(&H41A, "HCI error 'Unsupported Remote Feature / Unsupported LMP Feature (0X1A)' is received.")
        SDKErrorList.Add(&H41B, "HCI error 'SCO Offset Rejected (0X1B)' is received.")
        SDKErrorList.Add(&H41C, "HCI error 'SCO Interval Rejected (0X1C)' is received.")
        SDKErrorList.Add(&H41D, "HCI error 'SCO Air Mode Rejected (0X1D)' is received.")
        SDKErrorList.Add(&H41E, "HCI error 'Invalid LMP Parameters (0X1E)' is received.")
        SDKErrorList.Add(&H41F, "HCI error 'Unspecified Error (0X1F)' is received.")
        SDKErrorList.Add(&H420, "HCI error 'Unsupported LMP Parameter Value (0X20)' is received.")
        SDKErrorList.Add(&H421, "HCI error 'Role Change Not Allowed (0X21)' is received.")
        SDKErrorList.Add(&H422, "HCI error 'LMP Response Timeout (0X22)' is received.")
        SDKErrorList.Add(&H423, "HCI error 'LMP Error Transaction Collision (0X23)' is received.")
        SDKErrorList.Add(&H424, "HCI error 'LMP PDU Not Allowed (0X24)' is received.")
        SDKErrorList.Add(&H425, "HCI error 'Encryption Mode Not Acceptable (0X25)' is received.")
        SDKErrorList.Add(&H426, "HCI error 'Link Key Can not be Changed (0X26)' is received.")
        SDKErrorList.Add(&H427, "HCI error 'Requested QOS Not Supported (0X27)' is received.")
        SDKErrorList.Add(&H428, "HCI error 'Instant Passed (0X28)' is received.")
        SDKErrorList.Add(&H429, "HCI error 'Pairing with Unit Key Not Supported (0X29)' is received.")
        SDKErrorList.Add(&H42A, "HCI error 'Different Transaction Collision (0X2A)' is received.")
        SDKErrorList.Add(&H42C, "HCI error 'QOS Unacceptable Parameter (0X2C)' is received.")
        SDKErrorList.Add(&H42D, "HCI error 'QOS Rejected (0X2D)' is received.")
        SDKErrorList.Add(&H42E, "HCI error 'Channel Classification Not Supported (0X2E)' is received.")
        SDKErrorList.Add(&H42F, "HCI error 'Insufficient Security (0X2F)' is received.")
        SDKErrorList.Add(&H430, "HCI error 'Parameter Out of Mandatory Range (0X30)' is received.")
        SDKErrorList.Add(&H432, "HCI error 'Role Switch Pending (0X32)' is received.")
        SDKErrorList.Add(&H434, "HCI error 'Reserved Slot Violation (0X34)' is received.")
        SDKErrorList.Add(&H435, "HCI error 'Role Switch Failed (0X35)' is received.")
        'see supplement err codes: 
        '6.1.1 Error Codes
        '6.1.2 AVRCP Error Codes
    End Sub

    Public Function SDKErrorToString(SDKError As UInt32) As String
        SDKErrorList.Clear()
        CreateListSDKError()
        Dim stringfinal As String = ""
        For Each pair In SDKErrorList
            If pair.Key = "&H0" & SDKError Then stringfinal = pair.Value.ToString
        Next
        SDKErrorList.Clear()
        Return stringfinal
    End Function


    Public Function ContactNameFromNumber(ByVal path As String, ByVal number As String) As String
        ContactNameFromNumber = ""
        Dim sp() As String
        Try
            If File.Exists(path) Then
                Dim BookArray() As String = File.ReadAllLines(path)
                For line As Integer = 0 To BookArray.Length - 1
                    If BookArray(line).StartsWith("LST") AndAlso BookArray(line).Contains(number) Then
                        sp = BookArray(line).Replace("LST", "").Replace("||", ",").Replace(" Mobile", "").Replace(" Home", "").Replace(" Work", "").Split(",")
                        ContactNameFromNumber = sp(1)
                        'Return ContactNameFromNumber
                        Exit For
                        'MessageBox.Show(ContactNameFromNumber)
                    End If
                Next
            End If
            'MessageBox.Show(ContactNameFromNumber)
            Return ContactNameFromNumber
        Catch ex As Exception
            MessageBox.Show("Bad format in file " & path) 'MsgBox("Contact check Error", MsgBoxStyle.Information, ex.Message)
        End Try
    End Function

    Public Function NumberFromContactName(ByVal path As String, ByVal name As String) As String
        NumberFromContactName = ""
        Try
            If File.Exists(path) Then
                Dim BookArray() As String = File.ReadAllLines(path)
                For Each line In BookArray
                    If line <> "0" And line.Contains(name) Then
                        NumberFromContactName = line.Replace("LST", "").Replace("||", ",")
                        Dim sp() As String = NumberFromContactName.Split(",")
                        If sp(1).Contains(name) Then
                            NumberFromContactName = sp(0)
                        End If
                    Else
                        Continue For
                    End If
                Next
            End If
            Return NumberFromContactName
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Function


#End Region


#Region "Phone Formatter by Country"
    'see http://www.beansoftware.com/NET-Tutorials/format-string-phone-number.aspx
    'Country	Phone format
    'Denmark:	## ## ## ##
    'France:	## ## ## ## ##
    'Germany:	##### ######
    'Iceland:	###-#####
    'Netherlands:	###-######
    'Norway:	## ## ## ##
    'Poland:	###-###-###
    'Switzerland:	### ### ## ##
    'Turkey:	#### ### ## ##
    'Pakistan:	### ### ###
    'India:	####-#######
    'India (mobile):	#####-#####
    'China:	(####) ####-####
    'China (mobile):	### #### ####
    'Hong Kong:	#### ####
    'Japan:	(###) ####-####
    'Australia:	## #### ####
    'Australia (mobile):	#### ### ###
    Public Function formatPhoneNumberToNational(phoneNum As String, phoneFormat As String) As String

        If phoneFormat = "" Then
            ' Default format is (###) ###-####
            phoneFormat = "(###) ###-####"
        End If

        ' First, remove everything except of numbers
        Dim regexObj As Regex = New Regex("[^\d]")
        phoneNum = regexObj.Replace(phoneNum, "")

        ' Second, format numbers to phone string
        If phoneNum.Length > 0 Then
            phoneNum = Convert.ToInt64(phoneNum).ToString(phoneFormat)
        End If

        Return phoneNum
    End Function


    ''' <summary>
    ''' Formats the telephone number.
    ''' </summary>
    ''' <param name="phoneNumber">The phone number.</param>
    ''' <param name="country">The country.</param>
    Public Function formatPhoneNumberToInterNational(phoneNumber As String, country As String) As String
        Select Case country
            Case "China"
                If phoneNumber.Length <> 10 Then
                    phoneNumber = "China numbers must contain 10 digits!"

                Else
                    ' if (txtPhoneNumber.Text)
                    phoneNumber = "China Local Number: " + String.Format("({0})-{1}", phoneNumber.Substring(0, 2), phoneNumber.Substring(2, 8))

                End If
                ' else
                Exit Select
            Case "United Kingdom"
                If phoneNumber.Length <> 11 Then
                    phoneNumber = "United Kingdom numbers must contain 11 digits!"

                Else
                    ' if (txtPhoneNumber.Text)
                    phoneNumber = "United Kingdom Local Number: " + String.Format("{0}-{1}-{2}", phoneNumber.Substring(0, 5), phoneNumber.Substring(5, 3), phoneNumber.Substring(8))

                End If
                ' else
                Exit Select
            Case "United States"
                If phoneNumber.Length <> 10 Then
                    phoneNumber = "United States numbers must contain 10 digits!"

                Else
                    ' if (txtPhoneNumber.Text)
                    phoneNumber = "United States Local Number: " + String.Format("({0}) {1}-{2}", phoneNumber.Substring(0, 3), phoneNumber.Substring(3, 3), phoneNumber.Substring(6))

                End If
                ' else
                Exit Select
            Case "Venezuela"
                If phoneNumber.Length <> 11 Then
                    phoneNumber = "Venezuela numbers must contain 11 digits!"

                Else
                    ' if (txtPhoneNumber.Text)
                    phoneNumber = "Venezuela Local Number: " + String.Format("({0})-{1},{2},{3}", phoneNumber.Substring(0, 4), phoneNumber.Substring(4, 3), phoneNumber.Substring(7, 2), phoneNumber.Substring(9))

                End If
                ' else
                Exit Select
            Case Else
                phoneNumber = "An unknown error occurred. " + "Please verify your entry and try again. " + "If the problem persists, please contact your system administrator."

                Exit Select
        End Select
        Return phoneNumber
    End Function


#End Region

#Region "PBAP for Driveline addon"
    Public Function File_DirExists(dirpath As String) As Boolean
        If Directory.Exists(dirpath) = True Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function File_CreateDir(dirpath As String) As Boolean
        If Directory.Exists(dirpath) = False Then
            Directory.CreateDirectory(dirpath)
        End If
        If Directory.Exists(dirpath) = True Then
            Return True
        Else
            Return False
        End If
    End Function
	
    Public Function Math_HexToByte(ByVal inpHex As String) As Byte

        Math_HexToByte = CByte(Val("&H" & inpHex))

    End Function

    Private Sub Save_Icon(ByVal Source As Bitmap, ByVal Filename As String)

        'Try
        '    If Not Directory.Exists(AppDir) Then Directory.CreateDirectory(AppDir)
        '    If Not Directory.Exists(AppIcons) Then Directory.CreateDirectory(AppIcons)
        '    Source.MakeTransparent()
        '    Source.Save(Path.Combine(AppIcons, Filename), ImageFormat.Icon)
        'Catch ex As Exception
        '    Throw New Exception(ex.Message)
        'End Try

    End Sub

    Private Function Resize_Image(ByVal img As Image, ByVal Width As Int32, ByVal Height As Int32) As Bitmap
        Dim Bitmap_Source As New Bitmap(img)
        Dim Bitmap_Dest As New Bitmap(CInt(Width), CInt(Height))
        Dim Graphic As Graphics = Graphics.FromImage(Bitmap_Dest)
        Graphic.DrawImage(Bitmap_Source, 0, 0, Bitmap_Dest.Width + 1, Bitmap_Dest.Height + 1)
        Return Bitmap_Dest
    End Function
#End Region
End Module
