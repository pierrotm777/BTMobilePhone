
Namespace SMS

    Public Class vCalendar
#Region "vCalendar Class"
        Public Events As vEvents

        Public Overrides Function ToString() As String
            Dim result As New System.Text.StringBuilder()
            result.AppendFormat("//SCKE4 BEGIN:VCALENDAR{0}", System.Environment.NewLine)
            'The following two lines seem to be required by Outlook to get the alarm settings
            result.AppendFormat("VERSION:2.0{0}", System.Environment.NewLine)
            result.AppendFormat("METHOD:PUBLISH{0}", System.Environment.NewLine)
            Dim item As vEvent
            For Each item In Events
                result.Append(item.ToString())
            Next
            result.AppendFormat("END:VCALENDAR{0}", System.Environment.NewLine)
            Return result.ToString
        End Function

        Public Sub New(ByVal Value As vEvent)
            Me.Events = New vEvents()
            Me.Events.Add(Value)
        End Sub

        Public Sub New()
            Me.Events = New vEvents()
        End Sub
#End Region

#Region "vAlarm Class"
        Public Class vAlarm
            Public Trigger As TimeSpan      'Amount of time before event to display alarm
            Public Action As String      'Action to take to notify user of alarm
            Public Description As String          'Description of the alarm

            Public Sub New()
                Trigger = TimeSpan.FromDays(1)
                Action = "DISPLAY"
                Description = "Reminder"
            End Sub

            Public Sub New(ByVal SetTrigger As TimeSpan)
                Trigger = SetTrigger
                Action = "DISPLAY"
                Description = "Reminder"
            End Sub

            Public Sub New(ByVal SetTrigger As TimeSpan, ByVal SetAction As String, ByVal SetDescription As String)
                Trigger = SetTrigger
                Action = SetAction
                Description = SetDescription
            End Sub

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()
                result.AppendFormat("BEGIN:VALARM{0}", System.Environment.NewLine)
                result.AppendFormat("TRIGGER:P{0}DT{1}H{2}M{3}", Trigger.Days, Trigger.Hours, Trigger.Minutes, System.Environment.NewLine)
                result.AppendFormat("ACTION:{0}{1}", Action, System.Environment.NewLine)
                result.AppendFormat("DESCRIPTION:{0}{1}", Description, System.Environment.NewLine)
                result.AppendFormat("END:VALARM{0}", System.Environment.NewLine)
                Return result.ToString
            End Function
        End Class
#End Region

#Region "vEvent Class"
        Public Class vEvent
            Public UID As String          'Unique identifier for the event
            Public DTStart As Date      'Start date of event.  Will be automatically converted to GMT
            Public DTEnd As Date         'End date of event.  Will be automatically converted to GMT
            Public DTStamp As Date      'Timestamp.  Will be automatically converted to GMT
            Public Summary As String          'Summary/Subject of event
            Public Organizer As String      'Can be mailto: url or just a name
            Public Location As String
            Public Description As String
            Public URL As String
            Public Alarms As vAlarms        'Alarms needed for this event

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()
                result.AppendFormat("BEGIN:VEVENT{0}", System.Environment.NewLine)
                result.AppendFormat("UID:{0}{1}", UID, System.Environment.NewLine)
                result.AppendFormat("SUMMARY:{0}{1}", Summary, System.Environment.NewLine)
                result.AppendFormat("ORGANIZER:{0}{1}", Organizer, System.Environment.NewLine)
                result.AppendFormat("LOCATION:{0}{1}", Location, System.Environment.NewLine)
                result.AppendFormat("DTSTART:{0}{1}", DTStart.ToUniversalTime.ToString("yyyyMMdd\THHmmss\Z"), System.Environment.NewLine)
                result.AppendFormat("DTEND:{0}{1}", DTEnd.ToUniversalTime.ToString("yyyyMMdd\THHmmss\Z"), System.Environment.NewLine)
                result.AppendFormat("DTSTAMP:{0}{1}", Now.ToUniversalTime.ToString("yyyyMMdd\THHmmss\Z"), System.Environment.NewLine)
                result.AppendFormat("DESCRIPTION:{0}{1}", Description, System.Environment.NewLine)
                If Not URL Is Nothing Then
                    If URL.Length > 0 Then
                        result.AppendFormat("URL:{0}{1}", URL, System.Environment.NewLine)
                    End If
                End If
                Dim item As vAlarm
                For Each item In Alarms
                    result.Append(item.ToString())
                Next
                result.AppendFormat("END:VEVENT{0}", System.Environment.NewLine)
                Return result.ToString
            End Function

            Public Sub New()
                Me.Alarms = New vAlarms()
            End Sub
        End Class
#End Region

#Region "vAlarms Class"
        Public Class vAlarms
            ' The first thing to do when building a CollectionBase class is to inherit from System.Collections.CollectionBase
            Inherits System.Collections.CollectionBase

            Public Overloads Function Add(ByVal Value As vAlarm) As vAlarm
                ' After you inherit the CollectionBase class, you can access an intrinsic object
                ' called InnerList that represents your collection. InnerList is of type ArrayList.
                Me.InnerList.Add(Value)
                Return Value
            End Function

            Public Overloads Function Item(ByVal Index As Integer) As vAlarm
                ' To retrieve an item from the InnerList, pass the index of that item to the .Item property.
                Return CType(Me.InnerList.Item(Index), vAlarm)
            End Function

            Public Overloads Sub Remove(ByVal Index As Integer)
                ' This Remove expects an index.
                Dim cust As vAlarm

                cust = CType(Me.InnerList.Item(Index), vAlarm)
                If Not cust Is Nothing Then
                    Me.InnerList.Remove(cust)
                End If
            End Sub

        End Class
#End Region

#Region "vEvents Class"
        Public Class vEvents
            ' The first thing to do when building a CollectionBase class is to inherit from System.Collections.CollectionBase
            Inherits System.Collections.CollectionBase

            Public Overloads Function Add(ByVal Value As vEvent) As vEvent
                ' After you inherit the CollectionBase class, you can access an intrinsic object
                ' called InnerList that represents your collection. InnerList is of type ArrayList.
                Me.InnerList.Add(Value)
                Return Value
            End Function

            Public Overloads Function Item(ByVal Index As Integer) As vEvent
                ' To retrieve an item from the InnerList, pass the index of that item to the .Item property.
                Return CType(Me.InnerList.Item(Index), vEvent)
            End Function

            Public Overloads Sub Remove(ByVal Index As Integer)
                ' This Remove expects an index.
                Dim cust As vEvent

                cust = CType(Me.InnerList.Item(Index), vEvent)
                If Not cust Is Nothing Then
                    Me.InnerList.Remove(cust)
                End If
            End Sub
        End Class
#End Region
    End Class


End Namespace
