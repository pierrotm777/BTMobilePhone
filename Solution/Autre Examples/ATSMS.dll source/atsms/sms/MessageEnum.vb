Namespace SMS

#Region "Enums"

    Public Enum ENUM_TP_VPF
        Relative_Format = 16    'b4=1 b3=0
    End Enum

    Public Enum ENUM_TP_SRI
        Request_SMS_Report = 32
        No_SMS_Report = 0
    End Enum

    Public Enum ENUM_TP_SRR
        Service_Report_Request = 32         ' 00100000 = 0x20
        No_Service_Report_Request = 0       ' 00000000 = 0x00
    End Enum

    Public Enum ENUM_TP_DCS
        DefaultAlphabet = 0             ' 0x00 = default alphabet is 7 bit ASCII
        UCS2 = 8                        ' 0X08 = 16bit Unicode UCS2 coding 

        VoiceMail_Ind_Discard_Msg = 192 '11000000 = 0xC0, Discard message, voicemail indicator active
        Fax_Ind_Discard_Msg = 193       '11000001 = 0xC1, Discard message, fax indicator active
        Email_Ind_Discard_Msg = 194     '11000010 = 0xC2, Discard message, email indicator active
        Other_Ind_Discard_Msg = 195     '11000011 = 0xC3, Discard message, other indicator active

        'This could be useless values as it does not turn on the indicator or icon display on the mobile phone message waiting indicator!
        No_Voicemail_Ind_Discard_Msg = 200  '11001000 = 0xC8, Discard message, voicemail indicator inactive
        No_Fax_Ind_Discard_Msg = 201        '11001001 = 0xC9, Discard message, fax indicator inactive
        No_Email_Ind_Discard_Msg = 202      '11001010 = 0xCA, Discard message, email indicator inactive
        No_Other_Ind_Discard_Msg = 203      '11001011 = 0xCB, Discard message, other indicator inactive

        Voicemail_Ind_Store_7bit_Msg = 208  '11010000 = 0xD0, Store message, GSM 7bit alphabet, voicemail indicator active
        Fax_Ind_Store_7bit_Msg = 209        '11010001 = 0xD1, Store message, GSM 7bit alphabet, fax indicator active
        Email_Ind_Store_7bit_Msg = 210      '11010010 = 0xD2, Store message, GSM 7bit alphabet, email indicator active
        Other_Ind_Store_7bit_Msg = 211      '11010011 = 0xD3, Store message, GSM 7bit alphabet, other indicator active

        ' This could be useless values as it does not turn on the indicator or icon display on the mobile phone message waiting indicator!
        No_Voicemail_Ind_Store_7bit_Msg = 216   '11011000 = 0xD8, Store message, GSM 7bit alphabet, voicemail indicator active
        No_Fax_Ind_Store_7bit_Msg = 217         '11011001 = 0xD9, Store message, GSM 7bit alphabet, fax indicator active
        No_Email_Ind_Store_7bit_Msg = 218       '11011010 = 0xDA, Store message, GSM 7bit alphabet, email indicator active
        No_Other_Ind_Store_7bit_Msg = 219       '11011011 = 0xDB, Store message, GSM 7bit alphabet, other indicator active

        Voicemail_Ind_Store_UCS2_Msg = 224  '11100000 = 0xE0, Store message, GSM 7bit alphabet, voicemail indicator active
        Fax_Ind_Store_UCS2_Msg = 225    '11100001 = 0xE1, Store message, GSM 7bit alphabet, fax indicator active
        Email_Ind_Store_UCS2_Msg = 226  '11100010 = 0xE2, Store message, GSM 7bit alphabet, email indicator active
        Other_Ind_Store_UCS2_Msg = 227  '11100011 = 0xE3, Store message, GSM 7bit alphabet, other indicator active

        ' This could be useless values as it does not turn on the indicator or icon display on the mobile phone message waiting indicator!
        No_Voicemail_Ind_Store_UCS2_Msg = 232   '11101000 = 0xE8, Store message, GSM 7bit alphabet, voicemail indicator active
        No_Fax_Ind_Store_UCS2_Msg = 233         '11101001 = 0xE9, Store message, GSM 7bit alphabet, fax indicator active
        No_Email_Ind_Store_UCS2_Msg = 234       '11101010 = 0xEA, Store message, GSM 7bit alphabet, email indicator active
        No_Other_Ind_Store_UCS2_Msg = 235       '11101011 = 0xEB, Store message, GSM 7bit alphabet, other indicator active

        Class0_UD_7bits = 240   '11110000 = 0xF0 = Class0 immediate display, 7 bit data coding in User Data
        Class1_UD_7bits = 241   '11110001 = 0xF1 = Class1 ME (Mobile Equipment) specific, 7 bit data coding in User Data
        Class2_UD_7bits = 242   '11110010 = 0xF2 = Class2 SIM specific, 7 bit data coding in User Data
        Class3_UD_7bits = 243   '11110011= 0xF3 = Class3 TE (Terminate Equipment) specific, 7 bit data coding in User Data

        Class0_UD_8bits = 244   '11110100 = 0xF4 = Class0 immediate display, 8 bit data coding in User Data
        Class1_UD_8bits = 245   '11110101 = 0xF5 = Class1 ME (Mobile Equipment) specific, 8 bit data coding in User Data
        Class2_UD_8bits = 246   '11110110 = 0xF6 = Class2 SIM specific, 8 bit data coding in User Data
        Class3_UD_8bits = 247   '11110111= 0xF7 = Class3 TE (Terminate Equipment) specific, 8 bit data coding in User Data

    End Enum

    Public Enum ENUM_TP_VALID_PERIOD
        OneHour = 11 '0 to 143:(TP-VP+1)*5Min
        ThreeHours = 29
        SixHours = 71
        TwelveHours = 143
        OneDay = 167
        OneWeek = 196
        Maximum = 255
    End Enum

    Public Enum ENUM_TP_MTI
        MTI_Sms_Deliver = 0             ' 00000000 = 0x00, SMS-DELIVER PDU
        MTI_Sms_Submit = 1              ' 00000001 = 0x01, SMS-SUBMIT PDU
        MTI_Sms_Status_Report = 2       ' 00000010 = 0x02, SMS-STATUS-REPORT PDU
        MTI_Sms_Command = 2             ' 00000010 = 0x02, SMS-COMMAND PDU
    End Enum

    Public Enum SMSType
        SMS_RECEIVED = 0
        SMS_STATUS_REPORT = 2
        SMS_SUBMIT = 1
        EMS_RECEIVED = 64 'It is "Reserved" on my phone??
        EMS_SUBMIT = 65
    End Enum

    Public Enum ENUM_TP_RP
        No_Reply_Path = 0       ' 00000000 = 0x00
        Reply_Path = 128        ' 10000000 = 0x80
    End Enum

    Public Enum ENUM_TP_UDHI
        User_Data_Header_Indication = 64        ' 01000000 = 0x40
        No_User_Data_Header_Indication = 0      ' 00000000 = 0x00
    End Enum
#End Region


End Namespace
