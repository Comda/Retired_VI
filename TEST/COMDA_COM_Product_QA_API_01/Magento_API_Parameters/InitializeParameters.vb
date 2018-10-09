Imports Magento_API_Parameters.Mage_API
Public Class Initialize
    Public Property CurrentSessionID As String = Nothing
    Public Property catalogProduct As List(Of catalogProductEntity)
    Public Property dbContext As String
#Region "Magento API"

    Public Sub GetMagentoAPI_Credentials(ByVal UserID As String, ByVal API_ID As String)

        'ControlRoot = ControlRoot_Current
        'TransactionID = TransactionID_Current
        'dbContext = dbContext_Current

        'StopwatchLocal = New Stopwatch()
        'StopwatchLocal.Start()
        Try
            'WriteEventToLog("Info", "GetMagento API_Credentials (00:10): ", "  " & UserID & "  " & API_ID & "  ", StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
            MageHandler = New Mage_API.MagentoService
            SessionId = MageHandler.login(UserID, API_ID)
            'get some store list data
            storeEntityTable = MageHandler.storeList(SessionId)
            CurrentSessionID = SessionId
            ' WriteEventToLog("Info", "GetMagento API_Credentials: ", "SessionId", StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        Catch ex As Exception
            Throw New Exception("Magento_Login : " & ex.Message)
            'WriteEventToLog("Error", "GetMagento API_Credentials (00:10):", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try



    End Sub
    Public Sub InitializeSQLVariables(ByVal dbConnection As SqlClient.SqlConnection, ByVal ERP_Type As String)


        Magento_Store_ds = New Magento_Store

        Magento_ProductCatalogImport_da = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter
        Magento_ProductCatalogImport_da.Connection = dbConnection


    End Sub
    Public Sub GetCurrentCatalog(ByVal SessionId As String, ByVal ERP_Type As String, ByRef MagentoType As String, ByVal ImportID As Guid)
        Dim Filtres As New filters

        Select Case ERP_Type.ToUpper
            Case "FAMILY"
                Filtres = addFilter(Filtres, "type", "eq", "grouped")
                MagentoType = "grouped"
            Case "CHILD"
                Filtres = addFilter(Filtres, "type", "eq", "simple")
                MagentoType = "simple"
            Case "PARENT"
                Filtres = addFilter(Filtres, "type", "eq", "configurable")
                MagentoType = "configurable"
        End Select

        'Dim catalogProduct_temp = New List(Of catalogProductEntity)
        Dim StoreView As String = Nothing
        Try

            For Each storeEntityItem As storeEntity In storeEntityTable
                StoreView = storeEntityItem.code

                'ServiceCall = "http://secure.comda.com/api/v2_soap?wsdl=1"

                Dim catalogProductEntityTable() As catalogProductEntity = Nothing

                Try
                    catalogProductEntityTable = MageHandler.catalogProductList(SessionId, Filtres, StoreView)

                Catch ex As Exception
                    'WriteEventToLog("Error", "GetCurrentCatalog: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
                    Exit Sub
                End Try

                catalogProduct = catalogProductEntityTable.OrderBy(Function(o) o.sku).ToList()
                If catalogProduct.Count > 0 Then
                    UploadCatalog(catalogProduct, StoreView, ImportID)
                End If


                'catalogProduct_temp.AddRange(catalogProductEntityTable.ToList)
            Next
        Catch ex As Exception
            'WriteEventToLog("Error", "GetCurrentCatalog: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try



    End Sub
    Private Function GetOptionID(ByVal product_id As Integer, ByVal CurStore As String) As Integer
        'find the option
        Dim catalogProductCustomOptionListEntity_t As catalogProductCustomOptionListEntity()
        catalogProductCustomOptionListEntity_t = MageHandler.catalogProductCustomOptionList(SessionId, product_id, CurStore)

        Dim OptionID As Integer = 0

        For Each item_a As catalogProductCustomOptionListEntity In catalogProductCustomOptionListEntity_t
            If item_a.title = "Imprint location" Then
                OptionID = item_a.option_id
                Exit For
            End If
        Next
        Return OptionID
    End Function


    Private Sub UploadCatalog(ByVal catalogProduct As List(Of catalogProductEntity), ByVal StoreView As String, ByRef ImportID As Guid)

        'Magento_ProductCatalogMatch_da.Fill(Magento_Store_ds.Magento_ProductCatalogMatch)
        'Magento_Store_ds.Magento_ProductCatalogMatch.Clear()
        Dim ImportDate As Date = Now()

        Try
            For Each catalogProductEntityItem As catalogProductEntity In catalogProduct

                Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogImport.NewRow()
                newProductsRow("product_id") = catalogProductEntityItem.product_id
                newProductsRow("option_id") = 0
                newProductsRow("sku") = catalogProductEntityItem.sku
                newProductsRow("type") = catalogProductEntityItem.type

                newProductsRow("category_ids") = ReturnStringFromIds(catalogProductEntityItem.category_ids)
                newProductsRow("name") = catalogProductEntityItem.name

                newProductsRow("set") = catalogProductEntityItem.set

                newProductsRow("website_ids") = ReturnStringFromIds(catalogProductEntityItem.website_ids)
                newProductsRow("store") = StoreView
                newProductsRow("dbContext") = dbContext


                newProductsRow("ImportID") = ImportID
                newProductsRow("ImportDescription") = "API_CALL"
                newProductsRow("ImportDate") = ImportDate


                Magento_Store_ds.Magento_ProductCatalogImport.Rows.Add(newProductsRow)

            Next

            Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)


        Catch ex As Exception
            'WriteEventToLog("Error", "InsertProductListToDatabase: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try



    End Sub


#End Region
#Region "Events"
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
#End Region

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
#Region "Magento"

    Public Function addFilter(filtresIn As filters, key As String, op As String, value As String) As filters
        Dim filtres As filters = filtresIn
        If filtres Is Nothing Then
            filtres = New filters()
        End If

        Dim compfiltres As New complexFilter()
        compfiltres.key = key
        Dim ass As New associativeEntity()
        ass.key = op
        ass.value = value
        compfiltres.value = ass

        Dim tmpLst As List(Of complexFilter)
        If filtres.complex_filter IsNot Nothing Then
            tmpLst = filtres.complex_filter.ToList()
        Else
            tmpLst = New List(Of complexFilter)()
        End If

        tmpLst.Add(compfiltres)

        filtres.complex_filter = tmpLst.ToArray()

        Return filtres
    End Function

#End Region

End Class
