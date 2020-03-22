
Namespace Common

    ''' <summary>
    ''' Library resource string class
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Resource

        ' Messages
        Private Shared messages As Hashtable

        ''' <summary>
        ''' Static constructor
        ''' </summary>
        ''' <remarks></remarks>
        Shared Sub New()
            messages = New Hashtable
            messages.Add("300", "ME failure")
            messages.Add("301", "SMS service of ME reserved")
            messages.Add("302", "Operation not allowed")
            messages.Add("303", "Operation not supported")
            messages.Add("304", "Invalid PDU mode parameter")
            messages.Add("305", "Invalid text mode parameter")
            messages.Add("310", "(U)SIM not inserted")
            messages.Add("311", "(U)SIM PIN required")
            messages.Add("312", "PH-(U)SIM PIN required")
            messages.Add("313", "(U)SIM failure")
            messages.Add("314", "(U)SIM busy")
            messages.Add("315", "(U)SIM wrong")
            messages.Add("316", "(U)SIM PUK required")
            messages.Add("317", "(U)SIM PIN2 required")
            messages.Add("318", "(U)SIM PUK2 required")
            messages.Add("320", "Memory failure")
            messages.Add("321", "Invalid memory index")
            messages.Add("322", "Memory full")
            messages.Add("330", "SMSC address unknown")
            messages.Add("331", "No network service")
            messages.Add("332", "Network timeout")
            messages.Add("340", "No +CNMA acknowledgement expected")
            messages.Add("500", "Unknown error")

            messages.Add("ABOUT", "AT Communication Library Version Beta Version")

        End Sub

        ''' <summary>
        ''' Return a particular message based on message code
        ''' </summary>
        ''' <param name="msgCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetMessage(ByVal msgCode As String) As String
            If Not messages.Contains(msgCode) Then
                Return String.Empty
            End If
            Return Convert.ToString(messages.Item(msgCode))
        End Function


    End Class

End Namespace