Imports System
Imports System.Diagnostics
Imports System.Threading

Public Class EventLogMessaging

    Public Shared Sub WriteToLog(ByVal EventType As String, ByVal EventMessage As String, Optional ByVal EventID As Integer = 1, Optional ByVal EV_Data As Byte() = Nothing)
        ' Create an EventLog instance and assign its source.

        'EventNumber = EventNumber + 1

        Dim ev As New EventLog()
        Dim entryType As System.Diagnostics.EventLogEntryType = EventLogEntryType.Information


        ev.Source = "MAGE_Requests"
        'ev.Log = "" '"API_Calls"

        'If IsNothing(EventSource) Then EventSource = "MAGE_Requests"

        Select Case EventType
            Case "Error"
                entryType = EventLogEntryType.Error
            Case "Warning"
                entryType = EventLogEntryType.Warning
            Case "Info"
                entryType = EventLogEntryType.Information
            Case Else


        End Select
        Try
            'myEventLog.WriteEntry(myMessage, myEventLogEntryType, myApplicatinEventId, myApplicatinCategoryId, myRawData)
            ev.WriteEntry(EventMessage, entryType, 1, 0, EV_Data)
        Catch ex As Exception
            'When created, EvenType is nothing
        End Try


        ev.Close()

    End Sub

    Public Shared Sub CreateLog()

        If Not EventLog.SourceExists("HIT_GetImages") Then
            ' Create the source, if it does not already exist.
            ' An event log source should not be created and immediately used.
            ' There is a latency time to enable the source, it should be created
            ' prior to executing the application that uses the source.
            ' Execute this sample a second time to use the new source.
            EventLog.CreateEventSource("HIT_GetImages", "COMDA_API")
            'Console.WriteLine("CreatingEventSource")
            'The source is created.  Exit the application to allow it to be registered.
            Return
        End If

    End Sub

End Class
