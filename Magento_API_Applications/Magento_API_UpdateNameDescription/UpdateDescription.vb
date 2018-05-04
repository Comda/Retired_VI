Imports Magento_API_Parameters.Mage_API

Public Class UpdateDescription
    Public Sub New(ByVal dbConnection As SqlClient.SqlConnection, ByVal CurrentSessionID As String, ByVal TransactionID_Current As String, ByVal ControlRoot_Current As String)
        SessionId = CurrentSessionID

        'first loop is unique ERP
        ProductUpdateTypeTableAdapter_da = New Magento_StoreTableAdapters.ProductUpdateTypeTableAdapter
        ProductByFamilyDistinct_da = New Magento_StoreTableAdapters.ProductByFamilyDistinctTableAdapter
        Magento_catalogProductUpdate_da = New Magento_StoreTableAdapters.Magento_catalogProductUpdateTableAdapter
        Magento_ProductCatalogImport_da = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter


        Magento_Store_ds = New Magento_Store

        ProductUpdateTypeTableAdapter_da.Connection = dbConnection
        ProductByFamilyDistinct_da.Connection = dbConnection
        Magento_catalogProductUpdate_da.Connection = dbConnection
        Magento_ProductCatalogImport_da.Connection = dbConnection

        ProductUpdateTypeTableAdapter_da.Fill(Magento_Store_ds.ProductUpdateType)
        Magento_ProductCatalogImport_da.Fill(Magento_Store_ds.Magento_ProductCatalogImport)

        MageHandler = New MagentoService
        TransactionID = TransactionID_Current
        ControlRoot = ControlRoot_Current

        StopwatchLocal = New Stopwatch()
        StopwatchLocal.Start()

        Api_Para = New Magento_API_Parameters.Initialize
        Api_Para.WriteEventToLog("Info", "Synchronize Names/Description", "", StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)

        Api_Para.WriteEventToLog("Info", "Synchronize Names/Description " + dbConnection.ConnectionString, "", StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)

        Dim product_id As DataColumn = Magento_Store_ds.Magento_catalogProductUpdate.Columns.Add("product_id", Type.GetType("System.Int32"))
        product_id.AllowDBNull = True

        Dim option_id As DataColumn = Magento_Store_ds.Magento_catalogProductUpdate.Columns.Add("option_id", Type.GetType("System.Int32"))
        option_id.AllowDBNull = True

        'GetProductsToUpdate("PARENT")

        Try


            If Magento_Store_ds.ProductUpdateType.Rows.Count > 0 Then
                For i As Integer = 0 To Magento_Store_ds.ProductUpdateType.Rows.Count - 1
                    GetProductsToUpdate(Magento_Store_ds.ProductUpdateType.Rows(i).Item("type"), TransactionID)
                    'Magento_Store_ds.Magento_catalogProductUpdate.AcceptChanges()
                    Magento_catalogProductUpdate_da.Update(Magento_Store_ds.Magento_catalogProductUpdate)
                Next
            End If
        Catch ex As Exception

        End Try
        Api_Para.WriteEventToLog("Info", "Synchronize Names/Description Completed", "", StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)


    End Sub


    Private Sub GetProductsToUpdate(ByVal ERP_Type As String, ByVal TransactionID As String)
        Select Case ERP_Type.ToUpper
            Case "FAMILY"
                MagentoType = "grouped"
            Case "CHILD"
                MagentoType = "simple"
            Case "PARENT"
                MagentoType = "configurable"
        End Select

        Try
            Dim id As Integer = 0
            Dim ip As Integer = 0
            Try
                ProductByFamilyDistinct_da.Fill(Magento_Store_ds.ProductByFamilyDistinct, ERP_Type, Guid.Parse(TransactionID))
                Magento_catalogProductUpdate_da.Fill(Magento_Store_ds.Magento_catalogProductUpdate, ERP_Type, Guid.Parse(TransactionID))
                Dim UpdateView As DataView = Magento_Store_ds.Magento_catalogProductUpdate.DefaultView

                GetProductId()

                Dim ProdUnique As DataTable = Magento_Store_ds.ProductByFamilyDistinct ' Mage_API_Store_DS.ProductUpdate_distinct
                Dim ProdList As DataTable = Magento_Store_ds.Magento_catalogProductUpdate
                Dim ProdAll As DataView = ProdList.DefaultView

                Dim RowFilter As String = Nothing
                ProdAll.Sort = "store ASC"

                'Magento_Store_ds.Magento_catalogProductUpdate.AcceptChanges()
                'Magento_catalogProductUpdate_da.Update(Magento_Store_ds.Magento_catalogProductUpdate)

                For id = 0 To ProdUnique.Rows.Count - 1 'figure out the sku 'separate by store/description type
                    RowFilter = "sku='" & ProdUnique.Rows(id).Item("sku") & "'"
                    ProdAll.RowFilter = RowFilter 'we have 4 or more rows we need to group by store

                    Dim ProductDescription = New List(Of PartDescription)
                    Dim ProductIndex As New List(Of ProductIndex)
                    Dim sku_2 As String = Nothing

                    For ip = 0 To ProdAll.Count - 1
                        'select a store
                        Dim ProductId As Integer = 0
                        Dim OptionId As Integer = 0

                        Dim ProdIDNull = ProdAll(ip).Item("product_id")
                        If Not IsDBNull(ProdIDNull) Then
                            ProductId = ProdAll(ip).Item("product_id")
                        End If
                        If ProductId = 0 Then GoTo SKIP_Prod

                        Dim OptionIDNull = ProdAll(ip).Item("option_id")
                        If Not IsDBNull(OptionIDNull) Then
                            OptionId = ProdAll(ip).Item("option_id")
                        End If


                        Dim NewDescription As String = ProdAll(ip).Item("value")
                        Dim DescriptionType As String = ProdAll(ip).Item("name")
                        Dim store As String = ProdAll(ip).Item("store")
                        Dim sku As String = ProdAll(ip).Item("sku")


                        Dim rownull = ProdAll(ip).Item("sku_2")

                        If Not IsDBNull(rownull) Then
                            sku_2 = ProdAll(ip).Item("sku_2")
                        End If
                        'If ProdAll(ip).Item("sku_2").IsDBNull(ProdAll(ip).Item("sku_2")) Then
                        '    ' or whatever
                        '    sku_2 = [String].Empty
                        'Else
                        '    sku_2 = ProdAll(ip).Item("sku_2")

                        'End If


                        If ProductId <> 0 Then
                            AddToPartDescription(ProductDescription, ProductId, OptionId, store, DescriptionType, NewDescription, ip, sku, sku_2)
                            AddToProductIndex(ProductIndex, ip)
                        End If

                    Next
                    If ProductDescription.Count = 0 Then
                        GoTo SKIP_Prod
                    End If
                    Dim Updated As Boolean = False
                    If ProductDescription(0).type = "Imprint Location" Then
                        'execute option code and fall out
                        GetCatalogOptions(ProductDescription, Updated)
                        GoTo Update_ERP
                    End If


                    Dim DefaultSore As Boolean = True
                    Dim CurStore As String = Nothing
                    Dim FoOBJ As New List(Of PartDescription)
                    For Each item As PartDescription In ProductDescription
                        'we need a select OUT if 

                        If CurStore <> item.store Then
                            CurStore = item.store

                            If CurStore = "default" Then
                                CurStore = Nothing
                            End If
                            Dim curIndex As Integer = item.CurIndex

                            'Dim prodid As Integer = 0
                            If Not IsNothing(ProductDescription.Find(Function(x) x.store.Equals(item.store.Trim))) Then
                                FoOBJ = ProductDescription.FindAll(Function(x) x.store.Equals(item.store.Trim))
                            End If

                            productdata = New catalogProductCreateEntity
                            If FoOBJ.Count > 0 Then

                                For x As Integer = FoOBJ.Count - 1 To 0 Step -1
                                    'Debug.WriteLine("Store  {3} Type {4}   SKU  {0} , ProductId {1} Description  {2}", FoOBJ(x).productId, 0, FoOBJ(x).description, FoOBJ(x).store, FoOBJ(x).type)
                                    Select Case FoOBJ(x).type.ToLower
                                        Case "description"
                                            productdata.description = FoOBJ(x).description
                                        Case "short description"
                                            productdata.short_description = FoOBJ(x).description
                                        Case "name"
                                            productdata.name = FoOBJ(x).description
                                        Case "base price"
                                            productdata.price = FoOBJ(x).description
                                        Case "status"
                                            productdata.status = FoOBJ(x).description
                                            'Select Case CStr(FoOBJ(x).description).ToLower
                                            '    Case "0" Or "2" Or "disabled"
                                            '        FoOBJ(x).description = "2"
                                            '    Case "1" Or "enabled"
                                            '        FoOBJ(x).description = "1"
                                            'End Select
                                            'Case Else
                                            '    'remove from 
                                            '    ProductIndex.RemoveAt(x)
                                            '    FoOBJ.RemoveAt(x)
                                    End Select
                                Next

                            End If
                            'do it once for Default

                            'If DefaultSore Then
                            '    Debug.WriteLine("Entering Default store First:  Type {0}  Count {1}  Time {2}  ProdID {3}  Store {4}", MagentoType, id, StopwatchLocal.Elapsed, item.productId, "DEFAULT")
                            '    Magento_DescriptionWrite(productdata, item.productId, "", MagentoType, Updated)
                            '    DefaultSore = False
                            'End If
                            Debug.WriteLine("Type {0}  Count {1}  Time {2}  ProdID {3}  Entering Store {4}", MagentoType, id, StopwatchLocal.Elapsed, item.productId, CurStore)
                            Magento_DescriptionWrite(productdata, item.productId, CurStore, MagentoType, Updated)
                            Debug.WriteLine("Type {0}  Count {1}  Time {2}  ProdID {3}  Leaving Store {4}", MagentoType, id, StopwatchLocal.Elapsed, item.productId, CurStore)

                            Dim RowFilterUpdate As String = Nothing
                            RowFilterUpdate = "sku='" & ProdUnique.Rows(id).Item("sku") & "' and store='" & CurStore & "'"
                            UpdateView.RowFilter = RowFilterUpdate
                            Dim RowToUpdate As DataRowView = UpdateView(0)
                            RowToUpdate.Item("Result") = Updated
                            RowToUpdate.Item("InUpdateUniverse") = Not Updated
                            RowToUpdate.Item("DateUpdated") = Now()


skipDescription:
                        End If

                    Next
Update_ERP:
                    'We need a row from Magento_Store_ds.Magento_catalogProductUpdate to update


                    'For Each Index As ProductIndex In ProductIndex
                    '    If Updated Then ProdAll(Index.CurIndex).Item("DateUpdated") = Now()
                    '    ProdAll(Index.CurIndex).Item("Result") = Updated
                    '    ProdAll(Index.CurIndex).Item("InUpdateUniverse") = Not Updated
                    '    'Debug.WriteLine(ProdAll(Index.CurIndex).Item("store"))
                    '    'Debug.WriteLine("Type {0}  Count {1}  Time {2}", MagentoType, id, Now())
                    'Next

SKIP_Prod:


                Next
                'Magento_Store_ds.Magento_catalogProductUpdate.AcceptChanges()
                'Magento_catalogProductUpdate_da.Update(Magento_Store_ds.Magento_catalogProductUpdate)
            Catch ex As Exception
                Debug.WriteLine("id {0} ip {1}", id, ip)
                Api_Para.WriteEventToLog("Error", "GetProductsToUpdate_B4 Update DB", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
            End Try

            '    Magento_catalogProductUpdate_da.Update(Magento_Store_ds.Magento_catalogProductUpdate)

        Catch ex As Exception
            '    Api_Para.WriteEventToLog("Error", "GetProductsToUpdate", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try

        'Api_Para.WriteEventToLog("Info", "GetProductsToUpdate", "COMPLETED", StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
    End Sub

    Private Sub Magento_DescriptionWrite(ByVal productdata As catalogProductCreateEntity, ByVal productID As Integer, ByVal storeView As String, ByVal identifierType As String, ByRef Updated As Boolean)
        Updated = False

        Try
            Dim SoapReturn As String = MageHandler.catalogProductUpdate(SessionId, productID, productdata, storeView, identifierType)
            If SoapReturn.ToLower = "true" Then
                Updated = True
            End If

        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "Magento_DescriptionWrite", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try

    End Sub

    'PartDescription
    Private Sub AddToPartDescription(ByRef ProductDescription As List(Of PartDescription),
                                    ByVal productId As Integer, optionId As Integer,
                                    ByVal store As String,
                                    ByVal type As String,
                                    ByVal description As String,
                                    ByVal CurIndex As Integer,
                                    ByVal sku As String,
                                    ByVal sku_2 As String)

        Try

            ProductDescription.Add(New PartDescription() With {
                            .productId = productId,
                            .optionId = optionId,
                            .store = store,
                            .type = type,
                            .description = description,
                            .CurIndex = CurIndex,
                            .sku = sku,
                            .sku_2 = sku_2
        })
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "AddToPartDescription", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try
    End Sub
    Private Sub AddToProductIndex(ByRef ProductIndex As List(Of ProductIndex),
                                            ByVal CurIndex As Integer)

        Try

            ProductIndex.Add(New ProductIndex() With {
                                     .CurIndex = CurIndex
        })
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "AddToPartDescription", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try
    End Sub


    Private Class PartDescription
        Property productId As Integer
        Property optionId As Integer
        Property store As String
        Property type As String
        Property description As String
        Property CurIndex As Integer
        Property sku As String
        Property sku_2 As String

    End Class

    Private Class ProductIndex
        Property CurIndex As Integer
    End Class
    Private Sub GetProductId()

        Dim relation As DataRelation
        ' Dim row As DataRow
        Dim childRows() As DataRow

        For Each relation In Magento_Store_ds.Magento_catalogProductUpdate.ChildRelations

            For ix = 0 To Magento_Store_ds.Magento_catalogProductUpdate.Rows.Count - 1
                If Not IsNothing(Magento_Store_ds.Magento_catalogProductUpdate.Rows(ix).GetChildRows(relation)) Then
                    childRows = Magento_Store_ds.Magento_catalogProductUpdate.Rows(ix).GetChildRows(relation)
                    If childRows.Count > 0 Then
                        Magento_Store_ds.Magento_catalogProductUpdate.Rows(ix).Item("product_id") = childRows(0).Item("product_id")
                        Magento_Store_ds.Magento_catalogProductUpdate.Rows(ix).Item("option_id") = childRows(0).Item("option_id")
                    End If

                End If
            Next
        Next relation



        'Try
        '    If Not IsNothing(catalogProduct.Find(Function(x) x.sku.Equals(SKU.Trim))) Then
        '        prodid = catalogProduct.Find(Function(x) x.sku.Equals(SKU.Trim)).product_id
        '    End If
        'Catch ex As Exception
        '    Api_Para.WriteEventToLog("Error", "GetProductId", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        'End Try


    End Sub
    'get catalaog Options

    Private Sub GetCatalogOptions(ByVal ProductDescription As List(Of PartDescription), ByRef Updated As Boolean)
        Try

            Dim CurStore As String = Nothing
            Dim FoOBJ As New List(Of PartDescription)
            For Each item As PartDescription In ProductDescription
                'we need a select OUT if 

                If CurStore <> item.store Then
                    CurStore = item.store

                    If CurStore = "default" Then
                        CurStore = Nothing
                    End If
                    Dim curIndex As Integer = item.CurIndex

                    'Dim prodid As Integer = 0
                    If Not IsNothing(ProductDescription.FindAll(Function(x) x.store.Equals(item.store.Trim))) Then
                        FoOBJ = ProductDescription.FindAll(Function(x) x.store.Equals(item.store.Trim))
                    End If

                    'same store - same product - multi locations
                    Debug.WriteLine("SKU  {0}  Product id   {1} Store {2}", FoOBJ(0).sku, FoOBJ(0).productId, FoOBJ(0).store)

                    Dim product_id As Integer = FoOBJ(0).productId

                    'find the option
                    'Dim catalogProductCustomOptionListEntity_t As catalogProductCustomOptionListEntity()
                    'catalogProductCustomOptionListEntity_t = MageHandler.catalogProductCustomOptionList(SessionId, product_id, CurStore)

                    Dim OptionID As Integer = FoOBJ(0).optionId
                    If OptionID = 0 Then
                        GoTo NoOption
                    End If
                    'For Each item_a As catalogProductCustomOptionListEntity In catalogProductCustomOptionListEntity_t
                    '    If item_a.title = "Imprint location" Then
                    '        OptionID = item_a.option_id
                    '        Exit For
                    '    End If
                    'Next

                    For Each ERP_item As PartDescription In FoOBJ

                        'Debug.WriteLine("Product ID {0}", product_id)
                        Dim sku_2 As String = ERP_item.sku_2
                        Dim Title = ERP_item.description

                        Dim catalogProductCustomOptionInfo_t As catalogProductCustomOptionInfoEntity = MageHandler.catalogProductCustomOptionInfo(SessionId, OptionID, CurStore)

                        Dim AdditionalFields As catalogProductCustomOptionAdditionalFieldsEntity() = catalogProductCustomOptionInfo_t.additional_fields

                        For Each item_b As catalogProductCustomOptionAdditionalFieldsEntity In AdditionalFields
                            'Debug.WriteLine("B4 SKU  {0}, Title {1}  ID {2}", item.sku, item.title, item.value_id)
                            If sku_2 = item_b.sku Then
                                item_b.title = Title
                                Debug.WriteLine("AFTER SKU  {0}, Title {1}  ID {2}", item_b.sku, item_b.title, item_b.value_id)
                            End If

                        Next

                        Dim data As New catalogProductCustomOptionToUpdate
                        data.additional_fields = AdditionalFields

                        Updated = MageHandler.catalogProductCustomOptionUpdate(SessionId, OptionID, data, CurStore)
                        Debug.WriteLine("store {0}", CurStore)

                    Next


                End If
NoOption:


            Next

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub


End Class

