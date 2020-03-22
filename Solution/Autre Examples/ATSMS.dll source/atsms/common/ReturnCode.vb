
Namespace Common

    Public Class ReturnCode

        Public Const NO_ERROR As Integer = 0

        ' Communication error codes
        Public Const INVALID_COM_PORT As Integer = 1001                     ' Invalid COM port setting 
        Public Const CANNOT_FIND_COM_PORT As Integer = 1002                 ' Cannot find COM port 
        Public Const INVALID_BAUD_RATE As Integer = 1003                    ' Invalid baudrate setting 
        Public Const INVALID_PARITY_SETTING As Integer = 1004               ' Invalid parity setting 
        Public Const INVALID_DATA_BITS As Integer = 1005                    ' Invalid data bits setting 
        Public Const INVALID_STOP_BITS As Integer = 1006                    ' Invalid stop bits setting 
        Public Const INVALID_TIMEOUT_SETTING = 1007                         ' Invalid timeout setting 
        Public Const CANNOT_OPEN_COM_PORT As Integer = 1008                 ' Cannot open COM port 
        Public Const ACCESS_DENIED_COM_PORT As Integer = 1009               ' Access denied to COM port 
        Public Const COM_PORT_NOT_DEFINED_FOR_MODEM As Integer = 1010       ' COM port not defined for modem 
        Public Const COM_PORT_CLOSED As Integer = 1011                      ' COM port is already closed 
        Public Const CANNOT_SEND_COMMAND_PORT_NOT_OPENED As Integer = 1012  ' Cannot send command to modem (Port not open) 
        Public Const CANNOT_SEND_COMMAND_TIMEOUT As Integer = 1013          ' Cannot send command to modem (Timeout) 
        Public Const CANNOT_READ_MODEM_RESPONSE As Integer = 1014           ' Cannot read modem response 
        Public Const CONNECTION_VERIFICATION_FAILED As Integer = 1015       ' Connection verification failed 
        Public Const MODEM_DEFAULT_INIT_FAILED As Integer = 1016            ' At least one default modem initialization command failed
        Public Const NO_RESPONSE_FULL_FUNCTION_MODE As Integer = 1017       ' No response to full functionality mode command 

        ' MODEM and SIM Errors
        Public Const CANNOT_READ_MESSAGE_MEMORY_SETTING As Integer = 2001  ' Cannot read message memory setting 
        Public Const ERROR_SELECT_MEMORY_SETTING As Integer = 2002         ' Error setting selected message memory 
        Public Const INVALID_MESSAGE_MEMORY_SETTING As Integer = 2003      ' Invalid message memory setting 
        Public Const INVALID_MESSAGE_MEMORY_TYPE As Integer = 2004         ' Invalid message memory type 
        Public Const CANNOT_READ_MODEM_MANUFACTURER As Integer = 2005      ' Cannot read modem manufacturer 
        Public Const CANNOT_READ_MODEM_MODEL As Integer = 2006             ' Cannot read modem model 
        Public Const CANNOT_READ_MODEM_IMEI As Integer = 2007              ' Cannot read modem IMEI 
        Public Const CANNOT_READ_MODEL_FIRMWARE_VERSION As Integer = 2008  ' Cannot read modem firmware revision 
        Public Const INVALID_PIN As Integer = 2009                         ' Invalid PIN 
        Public Const UNKNOWN_ERROR_AFTER_SENDING_PIN As Integer = 2010     ' Unknown error after sending PIN 
        Public Const UNKNOWN_ERROR_WHILE_SENDING_PIN As Integer = 2011     ' Unknown error while sending PIN 
        Public Const COMM_ERROR_WHILE_SENDING_PIN As Integer = 2012        ' Communication error while sending PIN 
        Public Const BLANK_PIN_NOT_ALLOWED As Integer = 2013               ' Blank PIN is not allowed 
        Public Const ERROR_SENDING_PIN As Integer = 2014                   ' Error sending PIN. 
        Public Const MODEM_WAITING_SIM_PUK As Integer = 2015               ' Modem is waiting SIM PUK to be given 
        Public Const UNKNOWN_ERROR_CHECKING_PIN_STATUS = 2016              ' Unknown error while checking PIN status 
        Public Const SIM_CARD_DAMAGED As Integer = 2017                    ' SIM card is damaged 
        Public Const NO_SIM_CARD_INSERTED As Integer = 2018                ' No SIM card is inserted 
        Public Const INVALID_OR_WRONG_PIN As Integer = 2019                ' Invalid or wrong PIN 
        Public Const INVALID_OR_WRONG_PUK As Integer = 2020                ' Invalid or wrong PUK 
        Public Const INVALID_PIN_STATUS_RESPONSE As Integer = 2021         ' Invalid PIN status response 
        Public Const COMM_ERROR_CHECKING_PIN_STATUS As Integer = 2022      ' Communication error while checking PIN status 
        Public Const ERROR_CHECKING_PIN_STATUS As Integer = 2023           ' Error checking PIN Status 
        Public Const UNABLE_TO_READ_SMSC As Integer = 2024                 ' Unable to read SMSC from SIM 
        Public Const UNABLE_TO_SAVE_SMSC As Integer = 2025                 ' Unable to save SMSC 
        Public Const CANNOT_READ_BATTERY_CHARGE_LEVEL As Integer = 2026    ' Cannot read battery charge level 
        Public Const CANNOT_READ_MSISDN As Integer = 2027                  ' Cannot read own number 
        Public Const INVALID_VALUE_FOR_DELAY_AFTER_PIN As Integer = 2028   ' Invalid value for delay after PIN 
        Public Const CANNOT_READ_IMSI_INFORMATION As Integer = 2029        ' Cannot read IMSI information 


        ' Network error
        Public Const SIGNAL_STRENGTH_NOT_KNOWN As Integer = 3001           ' Signal strength not known or not detectable 
        Public Const UNKNOWN_NETWORK_REGISTRATION_STATUS As Integer = 3002 ' Unknown network registration status 
        Public Const ERROR_CHECKING_NETWORK_REGISTRATION_STATUS As Integer = 3003 ' Error checking network registration status. 
        Public Const NETWORK_REGISTRATION_FAILED As Integer = 3004         ' Network registration failed 
        Public Const NETWORK_REGISTRATION_DENIED As Integer = 3005         ' Network registration denied 
        Public Const UNABLE_TO_READ_NETWORK_INFORMATION As Integer = 3009  ' Unable to read network information 

        ' SMS send errors
        Public Const UNABLE_TO_SET_VALIDITY_PERIOD As Integer = 4001       ' Unable to set validity period 
        Public Const INVALID_LONG_MESSAGE_ACTION As Integer = 4002         ' Invalid long message action 
        Public Const INVALID_SEND_DELAY_VALUE As Integer = 4003            ' Invalid send delay value 
        Public Const INVALID_SEND_RETRY_VALUE As Integer = 4004            ' Invalid send retry value 
        Public Const INVALID_CHARACTER_ENCODING As Integer = 4005          ' Invalid character encoding 
        Public Const SEND_MESSAGE_ERROR As Integer = 4006                  ' Send message error 
        Public Const INVALID_DESTINATINO_NUMBER As Integer = 4007          ' Invalid destination number 
        Public Const CANNOT_CHECK_PDU_MODE_SUPPORT As Integer = 4008       ' Cannot check PDU mode support 
        Public Const MODEM_NOT_SUPPORT_PDU_MODE As Integer = 4009          ' Modem does not support PDU mode 
        Public Const ERROR_GENERATING_MESSAGE_PDU As Integer = 4010        ' Error generating message PDU 
        Public Const SMSC_NOT_SPECIFIED As Integer = 4011                  ' SMSC not specified 
        Public Const CANNOT_CONNECT_TO_MODEM_TO_SEND_MESSAGE As Integer = 4012             ' Cannot connect to modem for sending message 
        Public Const SEND_MESSAGE_COMMAND_ERROR_PORT_NOT_OPENED As Integer = 4013          ' Send message command error (Port not open) 
        Public Const SEND_MESSAGE_COMMAND_ERROR_TIME_OUT As Integer = 4014 ' Send message command error (Timeout) 
        Public Const INVALID_RESPONSE_TO_SEND_COMMAND As Integer = 4015    ' Invalid response to send message command 
        Public Const SUBMIT_PDU_ERROR_PORT_NOT_OPENED As Integer = 4016    ' Submit PDU error (Port not open) 
        Public Const SUBMIT_PDU_ERROR_TIME_OUT As Integer = 4017           ' Submit PDU error (Timeout) 
        Public Const INVALID_RESPONSE_TO_SUBMIT_PDU As Integer = 4018      ' Invalid response to submit PDU 
        Public Const UNKNOW_ERROR_WHILE_SENDING_MESSAGE As Integer = 4019  ' Unknown error while sending message 
        Public Const INSUFFICIENT_LICENSE_RIGHTS As Integer = 4020         ' Insufficient license rights 

        ' SMS read errors
        Public Const INVALID_INBOX_MSG_INDEX As Integer = 5001             ' Invalid inbox message index 
        Public Const INVALID_MSG_ITEM_INDEX As Integer = 5002              ' Invalid message item index 
        Public Const READ_MSG_ERROR As Integer = 5003                      ' Read message error 
        'Public Const CANNOT_CHECK_PDU_MODE_SUPPORT As Integer = 5004      ' Cannot check PDU mode support 
        Public Const MODEM_DOES_NOT_SUPPORT_PDU_MODE As Integer = 5005     ' Modem does not support PDU mode 
        Public Const CANNOT_CONNECT_MODEM_READ_MSG As Integer = 5006       ' Cannot connect to modem for reading message 
        Public Const READ_MSG_COMMAND_ERROR_PORT_NOT_OPENED As Integer = 5007 ' Read message command error (Port not open) 
        Public Const READM_MSG_COMMAND_ERROR_TIME_OUT As Integer = 5008    ' Read message command error (Timeout) 
        Public Const INVALID_RESPONSE_READ_MSG_COMMAND As Integer = 5009   ' Invalid response to read message command 
        'Public Const READ_MSG_ERROR As Integer = 5010                     ' Read message error 
        Public Const INVALID_READ_MSG_TYPE As Integer = 5011               ' Invalid read message type option 


        ' General error
        Public Const INVALID_LOG_TYPE_OPTION As Integer = 9001      ' Invalid log type option 
        Public Const ERROR_CREATING_LOG_FILE_PATH As Integer = 9002 ' Error creating log file path 
        Public Const INVALID_LOG_FILE_PATH As Integer = 9003        ' Invalid log file path 
        Public Const ERROR_GETTING_LOG_FILE_SIZE As Integer = 9004  ' Error getting log file size 
        Public Const ERROR_OPENING_LOG_FILE As Integer = 9005       ' Error opening log file 
        Public Const ERROR_CLEARING_LOG_FILE As Integer = 9006      ' Error clearing log file 
        Public Const ERROR_SETTING_LOG_FILE_FOLDER_PATH As Integer = 9007 ' Error setting log folder path 
        Public Const INVALID_LICENSE_KEY As Integer = 9008          ' Invalid license key 
        Public Const INVALID_LICENSE_INFORMATION As Integer = 9009  ' Invalid license information 



    End Class

End Namespace