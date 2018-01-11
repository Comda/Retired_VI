Imports System.Data.SqlClient

Module PrivateMethods
    Public Sub SetConnectionString(ByVal ImportedConnectionString As String)
        My.MySettings.Default("API_StoreConnectionString") = ImportedConnectionString  'API_StoreConnectionString2 (MySettings)
        CurrentConString = My.MySettings.Default("API_StoreConnectionString")
        CurrentConnection = New SqlConnection
        CurrentConnection.ConnectionString = CurrentConString
        Try
            If CurrentConnection.State = ConnectionState.Open Then
                CurrentConnection.Close()
            End If
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "SetConnectionString", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try
    End Sub
End Module
