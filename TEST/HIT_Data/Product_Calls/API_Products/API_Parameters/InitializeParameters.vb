Imports System.Data.SqlClient

Public Class Initialize
    Public Property Connection As SqlConnection = Nothing
    Public Property VendorImportID As Integer
    Public Property NumberOfProducts As Integer
    Public Property CreatedBy As String
    Public Property StopwatchLocal As Stopwatch
    Public Property TransactionID As String
    Public Property ControlRoot As String
    Public Property SessionId As String
    Public Property DBConnectionString As String = Nothing

    Public Property wsVersion As String = Nothing
    Public Property id As String = Nothing
    Public Property password As String = Nothing
    Public Property productId As String = Nothing
    Public Property currency As String = Nothing
    Public Property fobId As Integer = 0
    Public Property priceType As String = Nothing
    Public Property localizationCountry As String = Nothing
    Public Property localizationLanguage As String = Nothing
    Public Property configurationType As String = Nothing
    Public Property Comment As String = Nothing
    Public Property FunctionRequested As String = Nothing

    Public WithEvents Request_data As Request_ds
    Public WithEvents Request_dt As Request_dsTableAdapters.API_RequestTableAdapter

    Public Property Active As List(Of String)
    Public Property SourceAPI As String = Nothing

    Private Property currency_str As String
    Private Property fobId_str As String
    Private Property priceType_str As String
    Private Property configurationType_str As String

    Public Sub New(ByVal ExtractTask As String, ByVal SKU_Current As String, ByVal VendorImportID_Current As Integer, ByVal CreatedBy_Current As String, ByVal DBConnection_Current As String,
                                 ByVal HIT_UserID As String, ByVal HIT_Secret As String, ByVal currency_Current As String, ByVal fobId_Current As String, ByVal priceType_Current As String,
                                 ByVal localizationCountry_Current As String, ByVal localizationLanguage_Current As String, ByVal configurationType_Current As String, ByRef RequestID As Integer, ByVal Comment_current As String)



        'API
        id = HIT_UserID
        password = HIT_Secret
        productId = SKU_Current
        localizationCountry = localizationCountry_Current
        localizationLanguage = localizationLanguage_Current
        currency_str = currency_Current
        fobId_str = fobId_Current
        priceType_str = priceType_Current
        configurationType_str = configurationType_Current
        'ERP
        VendorImportID = VendorImportID_Current
        CreatedBy = CreatedBy_Current
        DBConnectionString = DBConnection_Current
        Comment = Comment_current


        Select Case currency_Current.ToUpper
            Case "USD"
                currency = CurrencyCodes.USD
            Case "CAD"
                currency = CurrencyCodes.CAD
        End Select

        Select Case fobId_Current.ToUpper
            Case "CANADA"
                fobId = FOBLocations.Canada
            Case "USA"
                fobId = FOBLocations.Florida
        End Select

        Select Case priceType_Current.ToUpper
            Case "LIST"
                priceType = PriceTypes.LIST
            Case "CUSTOMER"
                priceType = PriceTypes.CUSTOMER
            Case "NET"
                priceType = PriceTypes.NET
        End Select

        Select Case configurationType_Current.ToUpper
            Case "DECORATED"
                configurationType = ConfigurationTypes.DECORATED
            Case "BLANK"
                configurationType = ConfigurationTypes.BLANK
        End Select

        Select Case ExtractTask.ToUpper
            Case "CATEGORY"
                FunctionRequested = "GetProductCategory"
            Case "PACKAGING"
                FunctionRequested = "GetProductPackaging"

            Case "PRODUCTINFO"
                FunctionRequested = "GetProductInfo"

            Case "CONFIGURATION"
                FunctionRequested = "GetConfigurationAndPricing"
            Case "IMAGES"
                FunctionRequested = "GetImages"

            Case "COLORS"
                FunctionRequested = "GetColorss"

            Case "SIZES"
                FunctionRequested = "GetSizes"
            Case = "INFO"
                FunctionRequested = "GetInfo"

            Case "PROPERTY"
                FunctionRequested = "GetProperties"

        End Select

        ControlRoot = FunctionRequested
        wsVersion = "1.0.0"

        'we need some Product List
        Active = New List(Of String)
        Active = GetListOfProducts(productId, id, password)

        Request_data = New Request_ds
        Request_dt = New Request_dsTableAdapters.API_RequestTableAdapter
        Request_dt.Connection.ConnectionString = DBConnectionString

        TransactionID = Guid.NewGuid().ToString

        AddRowsToRequest(FunctionRequested)

        RequestID = GetRequestID(TransactionID)

    End Sub
    Public Sub UpdateRequestData(ByVal TransactionID As String)

        Request_dt.Fill(Request_data.API_Request, Guid.Parse(TransactionID))

        Request_data.API_Request.Rows(0).Item("CompletedON") = Now()
        Request_data.API_Request.Rows(0).Item("CompletedCall") = 1
        Request_data.API_Request.Rows(0).Item("SourceAPI") = SourceAPI

        Request_dt.Update(Request_data.API_Request)


    End Sub
    Public Function GetRequestID(ByVal TransactionID As String) As Integer
        Request_dt.Fill(Request_data.API_Request, Guid.Parse(TransactionID))
        Return Request_data.API_Request.Rows(0).Item("RequestID")
    End Function

    Public Sub AddRowsToRequest(ByVal FunctionRequested As String)
        Try
            Request_dt.Fill(Request_data.API_Request, Guid.Parse(TransactionID))

            Dim newInsertParameterRow As DataRow = Request_data.API_Request.NewRow()

            newInsertParameterRow("wsVersion") = wsVersion
            newInsertParameterRow("id") = id
            newInsertParameterRow("CompletedCall") = 0
            newInsertParameterRow("productId") = productId
            newInsertParameterRow("currency") = currency_str.ToUpper

            newInsertParameterRow("fobId") = fobId_str.ToUpper
            newInsertParameterRow("priceType") = priceType_str.ToUpper
            newInsertParameterRow("localizationCountry") = localizationCountry
            newInsertParameterRow("localizationLanguage") = localizationLanguage
            newInsertParameterRow("configurationType") = configurationType_str.ToUpper

            newInsertParameterRow("FunctionRequested") = FunctionRequested
            newInsertParameterRow("RequestedBy") = CreatedBy


            newInsertParameterRow("Comment") = Comment
            newInsertParameterRow("SourceAPI") = SourceAPI


            newInsertParameterRow("VendorImportID") = VendorImportID
            newInsertParameterRow("RequestedOn") = Now()
            newInsertParameterRow("TransactionID") = TransactionID
            newInsertParameterRow("CompletedON") = Now()

            Request_data.API_Request.Rows.Add(newInsertParameterRow)

            Request_dt.Update(Request_data.API_Request)

            'Debug.WriteLine("AddRowsToRequest {0} in:  {1}", TransactionID, FunctionRequested)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            'push error back to calling proc
            'WriteEventToLog("Error", "AddRowsToRequest: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try

    End Sub

    Public Sub WriteEventToLog(ByVal ErrorType As String, ByVal Process As String, ByVal ErrorDetails As String, ByRef StopwatchLocal As Stopwatch, ByVal TransactionGUID As Guid, ByVal TransactionControlRoot As String)

        Dim elapsedTime As String = GetTheElapsedTime(StopwatchLocal)
        Dim EventLogData As Byte() = UnicodeStringToBytes("RunTime " + elapsedTime & vbCrLf)

        Select Case ErrorType
            Case "Error"
                EventLogMessaging.WriteToLog("Error", String.Format("API_Call {0} | ID =>{2} | Completed_at => {1} ", TransactionControlRoot & "?" & Process & "~" & ErrorDetails, Now, TransactionGUID), 1, EventLogData)
            Case "Info"
                EventLogMessaging.WriteToLog("Info", String.Format("API_Call {0} | ID =>{1} |", TransactionControlRoot & "?" & "Completed_at: " & Now() & "~" & Process & "~" & ErrorDetails, TransactionGUID), 1, EventLogData)
            Case "Warning"
                EventLogMessaging.WriteToLog("Warning", String.Format("API_Call {0} | ID =>{2} | Completed_at => {1} ", TransactionControlRoot & "?" & Process & "~" & ErrorDetails, Now, TransactionGUID), 1, EventLogData)
        End Select

    End Sub
    Public Function GetTheElapsedTime(ByRef CurrStopWatch As Stopwatch) As String
        Dim elapsedTime As String = Nothing

        CurrStopWatch.Stop()
        ' Get the elapsed time as a TimeSpan value.
        Dim ts As TimeSpan = CurrStopWatch.Elapsed
        ' Format and display the TimeSpan value.
        elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10)
        'Console.WriteLine("RunTime " + elapsedTime)
        CurrStopWatch.Start()

        Return elapsedTime
    End Function

    Public Function UnicodeStringToBytes(ByVal str As String) As Byte()
        Return System.Text.Encoding.Unicode.GetBytes(str)
    End Function

    Public Function CheckObject(ByRef Input As Object) As Object
        If Not IsNothing(Input) Then
            Return Input
        Else
            Return Nothing
        End If
    End Function

    Public Function NothingToDBNull(ByVal Input As Object) As Object

        If IsNothing(Input) Then
            Return DBNull.Value
        Else
            Return Input
        End If

    End Function

End Class
