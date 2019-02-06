Imports Magento_API_Parameters.Mage_Api
Imports System.Threading
Public Class Initialize
    Public Property CurrentSessionID As String = Nothing
    Public Property catalogProduct As List(Of catalogProductEntity)
    Public Property dbContext As String
#Region "Magento API"

    Public Sub GetMagentoAPI_Credentials(ByVal UserID As String, ByVal API_ID As String, ByVal ControlRoot_Current As String, ByVal TransactionID_Current As String, ByVal dbContext_Current As String)
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12

        ControlRoot = ControlRoot_Current
        TransactionID = TransactionID_Current
        dbContext = dbContext_Current

        StopwatchLocal = New Stopwatch()
        StopwatchLocal.Start()
        Try
            WriteEventToLog("Info", "GetMagento API_Credentials (00:10): ", "  " & UserID & "  " & API_ID & "  ", StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
            MageHandler = New Mage_Api.MagentoService
            SessionId = MageHandler.login(UserID, API_ID)
            'get some store list data
            storeEntityTable = MageHandler.storeList(SessionId)
            CurrentSessionID = SessionId
            WriteEventToLog("Info", "GetMagento API_Credentials: ", "SessionId", StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        Catch ex As Exception
            Throw New ApplicationException(String.Format("Function {0} Error {1} Site {2} ", "GetMagentoAPI_Credentials", ex, "https://www.mapleleafpromostore.com/index.php/api/v2_soap/index/"))
            WriteEventToLog("Error", "GetMagento API_Credentials (00:10):", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try
    End Sub
    Public Sub InitializeSQLVariables(ByVal dbConnection As SqlClient.SqlConnection, ByVal ERP_Type As String)

        Magento_ProductCatalogMatch_da = New Magento_StoreTableAdapters.Magento_ProductCatalogMatchTableAdapter
        Magento_Store_ds = New Magento_Store
        Magento_ProductCatalogMatch_da.Connection = dbConnection

        Magento_ProductCatalogImport_da = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter
        Magento_ProductCatalogImport_da.Connection = dbConnection

        MatchTable_da = New Magento_StoreTableAdapters.Magento_ProductCatalogImportMatchTableAdapter
        MatchTable_da.Connection = dbConnection

        Magento_ProductCatalogImport_Control_da = New Magento_StoreTableAdapters.Magento_ProductCatalogImport_ControlTableAdapter
        Magento_ProductCatalogImport_Control_da.Connection = dbConnection


        'ProductCatalogImport_Family = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter
        'ProductCatalogImport_Child = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter
        'ProductCatalogImport_Parent = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter


        MatchTable_da.Fill(Magento_Store_ds.Magento_ProductCatalogImportMatch)

    End Sub
    '    Public Sub Catalog_Threads()

    '        Dim SessionId As String = CurrentSessionID


    '        Dim FamilyThread _
    '           As New Threading.Thread(
    '           AddressOf GetCurrentCatalog_Thread_Family)
    '        FamilyThread.Start(SessionId)


    '        Dim ParentThread _
    '           As New Threading.Thread(
    '           AddressOf GetCurrentCatalog_Thread_PARENT)
    '        ParentThread.Start(SessionId)

    '        Dim ChildThread _
    '      As New Threading.Thread(
    '      AddressOf GetCurrentCatalog_Thread_CHILD)
    '        ChildThread.Start(SessionId)

    '        If FamilyThread.ThreadState <> 0 And ParentThread.ThreadState <> 0 And ChildThread.ThreadState <> 0 Then
    '            GoTo endProcesses
    '        End If
    'endProcesses:
    '    End Sub
    'Public Sub GetCurrentCatalog_Thread_CHILD(ByVal SessionId As String)

    '    Dim ERP_Type As String = "CHILD"
    '    Dim Filtres As New filters

    '    Select Case ERP_Type.ToUpper
    '        Case "FAMILY"
    '            Filtres = addFilter(Filtres, "type", "eq", "grouped")
    '            MagentoType = "grouped"
    '        Case "CHILD"
    '            Filtres = addFilter(Filtres, "type", "eq", "simple")
    '            MagentoType = "simple"
    '        Case "PARENT"
    '            Filtres = addFilter(Filtres, "type", "eq", "configurable")
    '            MagentoType = "configurable"
    '    End Select

    '    Dim ProductCatalogImport_Parent = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter
    '    'Dim catalogProduct_temp = New List(Of catalogProductEntity)
    '    Dim StoreView As String = Nothing
    '    Try

    '        For Each storeEntityItem As storeEntity In storeEntityTable
    '            StoreView = storeEntityItem.code

    '            'ServiceCall = "https://www.mapleleafpromostore.com/index.php/api/v2_soap/index/"

    '            Dim catalogProductEntityTable() As catalogProductEntity = Nothing

    '            Try
    '                catalogProductEntityTable = MageHandler.catalogProductList(SessionId, Filtres, StoreView)

    '            Catch ex As Exception
    '                WriteEventToLog("Error", "GetCurrentCatalog: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
    '                Exit Sub
    '            End Try

    '            catalogProduct = catalogProductEntityTable.OrderBy(Function(o) o.sku).ToList()

    '            'UploadCatalog(catalogProduct, StoreView)

    '            For Each catalogProductEntityItem As catalogProductEntity In catalogProduct

    '                Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogImport.NewRow()
    '                newProductsRow("product_id") = catalogProductEntityItem.product_id
    '                newProductsRow("option_id") = 0
    '                newProductsRow("sku") = catalogProductEntityItem.sku
    '                newProductsRow("type") = catalogProductEntityItem.type

    '                newProductsRow("category_ids") = ReturnStringFromIds(catalogProductEntityItem.category_ids)
    '                newProductsRow("name") = catalogProductEntityItem.name

    '                newProductsRow("set") = catalogProductEntityItem.set

    '                newProductsRow("website_ids") = ReturnStringFromIds(catalogProductEntityItem.website_ids)
    '                newProductsRow("store") = StoreView
    '                newProductsRow("dbContext") = dbContext


    '                Magento_Store_ds.Magento_ProductCatalogImport.Rows.Add(newProductsRow)

    '            Next
    '            Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)

    '        Next
    '    Catch ex As Exception
    '        WriteEventToLog("Error", "GetCurrentCatalog_Thread_Family: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
    '    End Try



    'End Sub
    'Public Sub GetCurrentCatalog_Thread_PARENT(ByVal SessionId As String)

    '    Dim ERP_Type As String = "PARENT"
    '    Dim Filtres As New filters

    '    Select Case ERP_Type.ToUpper
    '        Case "FAMILY"
    '            Filtres = addFilter(Filtres, "type", "eq", "grouped")
    '            MagentoType = "grouped"
    '        Case "CHILD"
    '            Filtres = addFilter(Filtres, "type", "eq", "simple")
    '            MagentoType = "simple"
    '        Case "PARENT"
    '            Filtres = addFilter(Filtres, "type", "eq", "configurable")
    '            MagentoType = "configurable"
    '    End Select

    '    Dim ProductCatalogImport_Parent = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter
    '    'Dim catalogProduct_temp = New List(Of catalogProductEntity)
    '    Dim StoreView As String = Nothing
    '    Try

    '        For Each storeEntityItem As storeEntity In storeEntityTable
    '            StoreView = storeEntityItem.code

    '            'ServiceCall = "https://www.mapleleafpromostore.com/index.php/api/v2_soap/index/"

    '            Dim catalogProductEntityTable() As catalogProductEntity = Nothing

    '            Try
    '                catalogProductEntityTable = MageHandler.catalogProductList(SessionId, Filtres, StoreView)

    '            Catch ex As Exception
    '                WriteEventToLog("Error", "GetCurrentCatalog: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
    '                Exit Sub
    '            End Try

    '            catalogProduct = catalogProductEntityTable.OrderBy(Function(o) o.sku).ToList()

    '            'UploadCatalog(catalogProduct, StoreView)

    '            For Each catalogProductEntityItem As catalogProductEntity In catalogProduct

    '                Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogImport.NewRow()
    '                newProductsRow("product_id") = catalogProductEntityItem.product_id
    '                newProductsRow("option_id") = 0
    '                newProductsRow("sku") = catalogProductEntityItem.sku
    '                newProductsRow("type") = catalogProductEntityItem.type

    '                newProductsRow("category_ids") = ReturnStringFromIds(catalogProductEntityItem.category_ids)
    '                newProductsRow("name") = catalogProductEntityItem.name

    '                newProductsRow("set") = catalogProductEntityItem.set

    '                newProductsRow("website_ids") = ReturnStringFromIds(catalogProductEntityItem.website_ids)
    '                newProductsRow("store") = StoreView
    '                newProductsRow("dbContext") = dbContext


    '                Magento_Store_ds.Magento_ProductCatalogImport.Rows.Add(newProductsRow)

    '            Next
    '            Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)

    '        Next
    '    Catch ex As Exception
    '        WriteEventToLog("Error", "GetCurrentCatalog_Thread_Family: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
    '    End Try



    'End Sub
    'Public Sub GetCurrentCatalog_Thread_Family(ByVal SessionId As String)

    '    Dim ERP_Type As String = "FAMILY"
    '    Dim Filtres As New filters

    '    Select Case ERP_Type.ToUpper
    '        Case "FAMILY"
    '            Filtres = addFilter(Filtres, "type", "eq", "grouped")
    '            MagentoType = "grouped"
    '        Case "CHILD"
    '            Filtres = addFilter(Filtres, "type", "eq", "simple")
    '            MagentoType = "simple"
    '        Case "PARENT"
    '            Filtres = addFilter(Filtres, "type", "eq", "configurable")
    '            MagentoType = "configurable"
    '    End Select

    '    Dim ProductCatalogImport_Family = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter
    '    'Dim catalogProduct_temp = New List(Of catalogProductEntity)
    '    Dim StoreView As String = Nothing
    '    Try

    '        For Each storeEntityItem As storeEntity In storeEntityTable
    '            StoreView = storeEntityItem.code

    '            'ServiceCall = "https://www.mapleleafpromostore.com/index.php/api/v2_soap/index/"

    '            Dim catalogProductEntityTable() As catalogProductEntity = Nothing

    '            Try
    '                catalogProductEntityTable = MageHandler.catalogProductList(SessionId, Filtres, StoreView)

    '            Catch ex As Exception
    '                WriteEventToLog("Error", "GetCurrentCatalog: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
    '                Exit Sub
    '            End Try

    '            catalogProduct = catalogProductEntityTable.OrderBy(Function(o) o.sku).ToList()

    '            'UploadCatalog(catalogProduct, StoreView)

    '            For Each catalogProductEntityItem As catalogProductEntity In catalogProduct

    '                Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogImport.NewRow()
    '                newProductsRow("product_id") = catalogProductEntityItem.product_id
    '                newProductsRow("option_id") = 0
    '                newProductsRow("sku") = catalogProductEntityItem.sku
    '                newProductsRow("type") = catalogProductEntityItem.type

    '                newProductsRow("category_ids") = ReturnStringFromIds(catalogProductEntityItem.category_ids)
    '                newProductsRow("name") = catalogProductEntityItem.name

    '                newProductsRow("set") = catalogProductEntityItem.set

    '                newProductsRow("website_ids") = ReturnStringFromIds(catalogProductEntityItem.website_ids)
    '                newProductsRow("store") = StoreView
    '                newProductsRow("dbContext") = dbContext


    '                Magento_Store_ds.Magento_ProductCatalogImport.Rows.Add(newProductsRow)

    '            Next
    '            Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)

    '        Next
    '    Catch ex As Exception
    '        WriteEventToLog("Error", "GetCurrentCatalog_Thread_Family: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
    '    End Try



    'End Sub


    Public Sub GetCurrentCatalog(ByVal SessionId As String, ByVal ERP_Type As String, ByRef MagentoType As String)
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

                'ServiceCall = "https://www.mapleleafpromostore.com/index.php/api/v2_soap/index/"

                Dim catalogProductEntityTable() As catalogProductEntity = Nothing
                'Dim salesOrderListEntityTable() As salesOrderListEntity
                'Dim DayOfHistory = 100

                Try


                    'Dim FilterDate As String = DateAdd(DateInterval.Day, -1 * DayOfHistory, Now().ToUniversalTime).ToString("yyyy-MM-dd HH:mm:ss")

                    'Filtres = addFilter(Filtres, "created_at", "from", FilterDate)


                    'salesOrderListEntityTable = MageHandler.salesOrderList(SessionId, Filtres)
                    catalogProductEntityTable = MageHandler.catalogProductList(SessionId, Filtres, StoreView)

                Catch ex As Exception
                    WriteEventToLog("Error", "GetCurrentCatalog: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
                    Exit Sub
                End Try

                catalogProduct = catalogProductEntityTable.OrderBy(Function(o) o.sku).ToList()


                'UploadCatalogControl(catalogProduct, StoreView)
                UploadCatalog(catalogProduct, StoreView)

                'catalogProduct_temp.AddRange(catalogProductEntityTable.ToList)
            Next
        Catch ex As Exception
            WriteEventToLog("Error", "GetCurrentCatalog: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try



    End Sub
    'Private Sub UploadCatalogControl(ByVal catalogProduct As List(Of catalogProductEntity), ByVal StoreView As String)
    '    Try


    '        For Each catalogProductEntityItem As catalogProductEntity In catalogProduct

    '            Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogImport_Control.NewRow()
    '            newProductsRow("product_id") = catalogProductEntityItem.product_id
    '            'newProductsRow("option_id") = 0
    '            newProductsRow("sku") = catalogProductEntityItem.sku
    '            'newProductsRow("type") = catalogProductEntityItem.type

    '            'newProductsRow("category_ids") = ReturnStringFromIds(catalogProductEntityItem.category_ids)
    '            'newProductsRow("name") = catalogProductEntityItem.name

    '            'newProductsRow("set") = catalogProductEntityItem.set

    '            'newProductsRow("website_ids") = ReturnStringFromIds(catalogProductEntityItem.website_ids)
    '            newProductsRow("store") = StoreView
    '            newProductsRow("dbContext") = dbContext


    '            Magento_Store_ds.Magento_ProductCatalogImport_Control.Rows.Add(newProductsRow)

    '        Next
    '    Catch ex As Exception

    '    End Try
    '    Magento_ProductCatalogImport_Control_da.Update(Magento_Store_ds.Magento_ProductCatalogImport_Control)
    'End Sub
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

    Private Sub UploadCatalog(ByVal catalogProduct As List(Of catalogProductEntity), ByVal StoreView As String)

        'Magento_ProductCatalogMatch_da.Fill(Magento_Store_ds.Magento_ProductCatalogMatch)
        Magento_Store_ds.Magento_ProductCatalogMatch.Clear()

        Try
            For Each catalogProductEntityItem As catalogProductEntity In catalogProduct

                Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogMatch.NewRow()
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


                Magento_Store_ds.Magento_ProductCatalogMatch.Rows.Add(newProductsRow)

            Next

            Dim relation As DataRelation = Magento_Store_ds.Magento_ProductCatalogMatch.ChildRelations(0)
            Dim childRows() As DataRow

            'For Each relation In Magento_Store_ds.Magento_ProductCatalogMatch.ChildRelations

            For ix = Magento_Store_ds.Magento_ProductCatalogMatch.Rows.Count - 1 To 0 Step -1
                If Not IsNothing(Magento_Store_ds.Magento_ProductCatalogMatch.Rows(ix).GetChildRows(relation)) Then
                    childRows = Magento_Store_ds.Magento_ProductCatalogMatch.Rows(ix).GetChildRows(relation)

                    If childRows.Count > 0 Then
                        Magento_Store_ds.Magento_ProductCatalogMatch.Rows(ix).Delete()
                    End If
                End If
            Next
            ' Next relation
            'now copy the NEW data 
            Try
                If Magento_Store_ds.Magento_ProductCatalogMatch.Rows.Count > 0 Then

                    For Each rowx As DataRow In Magento_Store_ds.Magento_ProductCatalogMatch.Rows
                        Dim OptionId As Integer = 0
                        'we need to add the OptionID here based on store and productID
                        If rowx.Item("type") = "configurable" And rowx.Item("name") <> "BLANK" Then
                            OptionId = GetOptionID(rowx.Item("product_id"), StoreView)
                        End If

                        Dim ProductGUID As Guid = Guid.NewGuid
                        Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogImport.NewRow()

                        newProductsRow("category_ids") = rowx.Item("category_ids")
                        newProductsRow("name") = rowx.Item("name")
                        newProductsRow("product_id") = rowx.Item("product_id")
                        newProductsRow("option_id") = OptionId
                        newProductsRow("set") = rowx.Item("set")
                        newProductsRow("sku") = rowx.Item("sku")
                        newProductsRow("type") = rowx.Item("type")
                        newProductsRow("website_ids") = rowx.Item("website_ids")
                        newProductsRow("store") = StoreView

                        'newProductsRow("ProductGUID") = ProductGUID
                        newProductsRow("ImportDescription") = "SYSTEM_GEN"
                        newProductsRow("ImportDate") = Now()
                        newProductsRow("dbContext") = dbContext

                        Magento_Store_ds.Magento_ProductCatalogImport.Rows.Add(newProductsRow)

                    Next

                    Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)

                End If
            Catch ex As Exception
                WriteEventToLog("Error", "CopyProductListToDatabase_a: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
            End Try

        Catch ex As Exception
            WriteEventToLog("Error", "CopyProductListToDatabase_b: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try

    End Sub


    '    Private Sub UploadCatalog(ByVal catalogProduct As List(Of catalogProductEntity), ByVal StoreView As String)
    '        Try


    '            For Each catalogProductEntityItem As catalogProductEntity In catalogProduct

    '                Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogImport.NewRow()
    '                newProductsRow("product_id") = catalogProductEntityItem.product_id
    '                newProductsRow("option_id") = 0
    '                newProductsRow("sku") = catalogProductEntityItem.sku
    '                newProductsRow("type") = catalogProductEntityItem.type

    '                newProductsRow("category_ids") = ReturnStringFromIds(catalogProductEntityItem.category_ids)
    '                newProductsRow("name") = catalogProductEntityItem.name

    '                newProductsRow("set") = catalogProductEntityItem.set

    '                newProductsRow("website_ids") = ReturnStringFromIds(catalogProductEntityItem.website_ids)
    '                newProductsRow("store") = StoreView
    '                newProductsRow("dbContext") = dbContext


    '                Magento_Store_ds.Magento_ProductCatalogImport.Rows.Add(newProductsRow)

    '            Next
    '        Catch ex As Exception

    '        End Try

    '        Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)


    '        Exit Sub











    '        'Magento_ProductCatalogMatch_da.Fill(Magento_Store_ds.Magento_ProductCatalogMatch)
    '        Magento_Store_ds.Magento_ProductCatalogMatch.Clear()

    '        'Try
    '        For Each catalogProductEntityItem As catalogProductEntity In catalogProduct

    '                Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogMatch.NewRow()
    '                newProductsRow("product_id") = catalogProductEntityItem.product_id
    '                newProductsRow("option_id") = 0
    '                newProductsRow("sku") = catalogProductEntityItem.sku
    '                newProductsRow("type") = catalogProductEntityItem.type

    '                newProductsRow("category_ids") = ReturnStringFromIds(catalogProductEntityItem.category_ids)
    '                newProductsRow("name") = catalogProductEntityItem.name

    '                newProductsRow("set") = catalogProductEntityItem.set

    '                newProductsRow("website_ids") = ReturnStringFromIds(catalogProductEntityItem.website_ids)
    '                newProductsRow("store") = StoreView
    '                newProductsRow("dbContext") = dbContext


    '                Magento_Store_ds.Magento_ProductCatalogMatch.Rows.Add(newProductsRow)

    '            Next

    '            GoTo skipMatch

    '            Dim relation As DataRelation = Magento_Store_ds.Magento_ProductCatalogMatch.ChildRelations(0)
    '            Dim childRows() As DataRow

    '            'For Each relation In Magento_Store_ds.Magento_ProductCatalogMatch.ChildRelations

    '            For ix = Magento_Store_ds.Magento_ProductCatalogMatch.Rows.Count - 1 To 0 Step -1
    '                If Not IsNothing(Magento_Store_ds.Magento_ProductCatalogMatch.Rows(ix).GetChildRows(relation)) Then
    '                    childRows = Magento_Store_ds.Magento_ProductCatalogMatch.Rows(ix).GetChildRows(relation)

    '                    If childRows.Count > 0 Then
    '                        Magento_Store_ds.Magento_ProductCatalogMatch.Rows(ix).Delete()
    '                    End If
    '                End If
    '            Next
    '            ' Next relation
    '            'now copy the NEW data 
    '            Try
    '                If Magento_Store_ds.Magento_ProductCatalogMatch.Rows.Count > 0 Then

    '                    For Each rowx As DataRow In Magento_Store_ds.Magento_ProductCatalogMatch.Rows
    '                        Dim OptionId As Integer = 0
    '                        'we need to add the OptionID here based on store and productID
    '                        If rowx.Item("type") = "configurable" And rowx.Item("name") <> "BLANK" Then
    '                            OptionId = GetOptionID(rowx.Item("product_id"), StoreView)
    '                        End If

    '                        Dim ProductGUID As Guid = Guid.NewGuid
    '                        Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogImport.NewRow()

    '                        newProductsRow("category_ids") = rowx.Item("category_ids")
    '                        newProductsRow("name") = rowx.Item("name")
    '                        newProductsRow("product_id") = rowx.Item("product_id")
    '                        newProductsRow("option_id") = OptionId
    '                        newProductsRow("set") = rowx.Item("set")
    '                        newProductsRow("sku") = rowx.Item("sku")
    '                        newProductsRow("type") = rowx.Item("type")
    '                        newProductsRow("website_ids") = rowx.Item("website_ids")
    '                        newProductsRow("store") = StoreView

    '                        'newProductsRow("ProductGUID") = ProductGUID
    '                        newProductsRow("ImportDescription") = "SYSTEM_GEN"
    '                        newProductsRow("ImportDate") = Now()
    '                        newProductsRow("dbContext") = dbContext

    '                        Magento_Store_ds.Magento_ProductCatalogImport.Rows.Add(newProductsRow)

    '                    Next


    '                Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)

    '                End If
    '            Catch ex As Exception
    '                WriteEventToLog("Error", "CopyProductListToDatabase_a: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
    '            End Try
    'skipMatch:
    '        Try
    '            Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)
    '        Catch ex As Exception
    '            WriteEventToLog("Error", "CopyProductListToDatabase_b: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
    '        End Try

    '    End Sub


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
