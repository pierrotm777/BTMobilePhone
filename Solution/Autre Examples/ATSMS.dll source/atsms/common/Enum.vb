Imports System.IO.Ports

Namespace Common

    Public Enum EnumEncoding
        GSM_Default_7Bit = 0
        Unicode_16Bit = 2
        Class2_7_Bit = 3
        Hex_Message = 4
        Class2_8_Bit = 5
    End Enum

    Public Enum EnumBaudRate
        BaudRate_110 = 100      ' Sets the serial port baud rate to 110 bits per second 
        BaudRate_300 = 300      ' Sets the serial port baud rate to 300 bits per second 
        BaudRate_1200 = 1200    ' Sets the serial port baud rate to 1200 bits per second 
        BaudRate_2400 = 2400    ' Sets the serial port baud rate to 2400 bits per second 
        BaudRate_4800 = 4800    ' Sets the serial port baud rate to 4800 bits per second 
        BaudRate_9600 = 9600    ' Sets the serial port baud rate to 9600 bits per second 
        BaudRate_14400 = 14400  ' Sets the serial port baud rate to 14400 bits per second 
        BaudRate_1920 = 19200  ' Sets the serial port baud rate to 19200 bits per second 
        BaudRate_38400 = 38400  ' Sets the serial port baud rate to 38400 bits per second 
        BaudRate_57600 = 57600  ' Sets the serial port baud rate to 57600 bits per second 
        BaudRate_115200 = 115200 ' Sets the serial port baud rate to 115200 bits per second 
    End Enum

    Public Enum EnumDataBits
        Four = 4
        Five = 5
        Six = 6
        Seven = 7
        Eight = 8
    End Enum

    Public Enum EnumParity
        None = Parity.None
        Odd = Parity.Odd
        Even = Parity.Even
        Mark = Parity.Mark
        Space = Parity.Space
    End Enum

    Public Enum EnumStopBits
        One = StopBits.One
        OnePointFive = StopBits.OnePointFive
        Two = StopBits.Two
        None = StopBits.None
    End Enum

    Public Enum EnumFlowControl
        None = 0
        RTS_CTS = 1
        Xon_Xoff = 2
    End Enum

    Public Enum EnumLogType
        NoLog = 0
        ErrorLog = 1
        ErrorEventLog
    End Enum

    Public Enum EnumLongMessage
        Truncate = 0
        SimpleSplit = 1
        FormattedSplit = 2
        Concatenate = 3
    End Enum

    Public Enum EnumMessageMemory
        SM = 1    ' SIM memory
        PHONE = 2 ' ME (Phone) memory
    End Enum

    Public Enum EnumQueuePriority
        High = 0
        Normal = 1
        Low = 2
    End Enum

    Public Enum EnumHandsake
        None = System.IO.Ports.Handshake.None
        RequestToSend = System.IO.Ports.Handshake.RequestToSend
        RequestToSendXOnXOff = System.IO.Ports.Handshake.RequestToSendXOnXOff
        XOnXOff = System.IO.Ports.Handshake.XOnXOff
    End Enum

    Public Enum EnumMessageType
        ReceivedUnreadMessages = 0  ' Received unread messages 
        ReceivedReadMessages = 1    ' Received read messages 
        ReceivedAllMessages = 2     ' Received read and unread messages 
    End Enum

End Namespace
