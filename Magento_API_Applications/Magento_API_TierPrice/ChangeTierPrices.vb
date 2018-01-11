Imports Magento_API_Parameters.Mage_API

Public Class ChangeTierPrices
    Public Sub New(ByVal dbConnection As SqlClient.SqlConnection, ByVal ERP_type As String, ByVal MagentoType As String, catalogProduct_temp As List(Of catalogProductEntity), ByVal CurrentSessionID As String)

        ProductUpdate_distinct_da = New Magento_StoreTableAdapters.ProductUpdate_distinctTableAdapter
        ProductUpdate_distinct_da.Connection = dbConnection
        Magento_catalogProductUpdate_da = New Magento_StoreTableAdapters.Magento_catalogProductUpdateTableAdapter
        ProductByFamilyDistinct_da = New Magento_StoreTableAdapters.ProductByFamilyDistinctTableAdapter
        ProductByFamilyDistinct_da.Connection = dbConnection

        Magento_Store_ds = New Magento_Store
        Magento_catalogProductUpdate_da.Connection = dbConnection

        Magento_catalogProductUpdate_da.Fill(Magento_Store_ds.Magento_catalogProductUpdate, ERP_type)

        SessionId = CurrentSessionID
        catalogProduct = catalogProduct_temp

        UpdatePrices(ERP_type)

    End Sub

    Private Sub UpdatePrices(ByVal ERP_type As String)
        'array of product ID
        'Array of Tier prices - from a string

        Try
            Try
                ProductByFamilyDistinct_da.Fill(Magento_Store_ds.ProductByFamilyDistinct, ERP_type)

                Dim ProdUnique As DataTable = Magento_Store_ds.ProductByFamilyDistinct ' Mage_API_Store_DS.ProductUpdate_distinct

                Dim ProdList As DataTable = Magento_Store_ds.Magento_catalogProductUpdate

                Dim ProdAll As DataView = ProdList.DefaultView
                Dim RowFilter As String = Nothing
                ProdAll.Sort = "store ASC"


                For id As Integer = 0 To ProdUnique.Rows.Count - 1 'figure out the sku 'separate by store/description type
                    RowFilter = "sku='" & ProdUnique.Rows(id).Item("sku") & "'"
                    ProdAll.RowFilter = RowFilter 'we have 4 or more rows we need to group by store

                    Dim ProductDescription = New List(Of PartDescription)
                    Dim ProductIndex As New List(Of ProductIndex)

                    For ip As Integer = 0 To ProdAll.Count - 1
                        'select a store
                        Dim ProductId As Integer = GetProductId(ProdAll(ip).Item("sku"))
                        Dim NewDescription As String = ProdAll(ip).Item("value")
                        Dim DescriptionType As String = ProdAll(ip).Item("name")
                        Dim store As String = ProdAll(ip).Item("store")
                        If ProductId <> 0 Then
                            AddToPartDescription(ProductDescription, ProductId, store, DescriptionType, NewDescription, ip)
                            AddToProductIndex(ProductIndex, ip)
                        End If

                    Next
                    If ProductDescription.Count = 0 Then
                        GoTo SKIP_Prod
                    End If

                    Dim Updated As Boolean = False
                    Dim DefaultSore As Boolean = True
                    Dim CurStore As String = Nothing
                    Dim FoOBJ As New List(Of PartDescription)
                    For Each item As PartDescription In ProductDescription
                        If CurStore <> item.store Then
                            CurStore = item.store
                            Dim curIndex As Integer = item.CurIndex

                            'Dim prodid As Integer = 0
                            If Not IsNothing(ProductDescription.FindAll(Function(x) x.store.Equals(item.store.Trim))) Then
                                FoOBJ = ProductDescription.FindAll(Function(x) x.store.Equals(item.store.Trim))
                            End If

                            TierPrice = New catalogProductTierPriceEntity

                            For ix = 0 To FoOBJ(0).description.Split("|").Length - 1
                                'TierPrice(ix).qty = FoOBJ(0).description.Split("|")(ix).Split(",")(0)
                            Next







                            TierPrice.qty = 100
                            TierPrice.price = 1.23

                            'productdata = New catalogProductCreateEntity
                            'For x As Integer = 0 To FoOBJ.Count - 1
                            '    'Debug.WriteLine("Store  {3} Type {4}   SKU  {0} , ProductId {1} Description  {2}", FoOBJ(x).productId, 0, FoOBJ(x).description, FoOBJ(x).store, FoOBJ(x).type)
                            '    Select Case FoOBJ(x).type.ToLower
                            '        Case "description"
                            '            productdata.description = FoOBJ(x).description
                            '        Case "short description"
                            '            productdata.short_description = FoOBJ(x).description
                            '        Case "name"
                            '            productdata.name = FoOBJ(x).description
                            '    End Select
                            'Next

                            'do it once for Default
                            If DefaultSore Then
                                Magento_TierPricWrite(TierPrice, item.productId, CurStore, MagentoType, Updated)
                                'Magento_DescriptionWrite(productdata, item.productId, "", MagentoType, Updated)
                                DefaultSore = False
                            End If
                            'Magento_DescriptionWrite(productdata, item.productId, CurStore, MagentoType, Updated)
                            Magento_TierPricWrite(TierPrice, item.productId, CurStore, MagentoType, Updated)

                        End If

                    Next

                    For Each Index As ProductIndex In ProductIndex
                        ProdAll(Index.CurIndex).Item("DateUpdated") = Now()
                        ProdAll(Index.CurIndex).Item("Result") = Updated
                        ProdAll(Index.CurIndex).Item("InUpdateUniverse") = Not Updated
                        Debug.WriteLine(ProdAll(Index.CurIndex).Item("store"))
                    Next
SKIP_Prod:


                Next
            Catch ex As Exception
                Api_Para.WriteEventToLog("Error", "GetProductsToUpdate_B4 Update DB", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
            End Try

            Magento_catalogProductUpdate_da.Update(Magento_Store_ds.Magento_catalogProductUpdate)

        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "GetProductsToUpdate", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        End Try

        Api_Para.WriteEventToLog("Info", "GetProductsToUpdate", "COMPLETED", StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
    End Sub


    Private Function GetProductId(ByVal SKU As String) As Integer

        Dim prodid As Integer = 0

        Try
            If Not IsNothing(catalogProduct.Find(Function(x) x.sku.Equals(SKU.Trim))) Then
                prodid = catalogProduct.Find(Function(x) x.sku.Equals(SKU.Trim)).product_id
            End If
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "GetProductId", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        End Try

        Return prodid
    End Function

    Private Sub Magento_DescriptionWrite(ByVal productdata As catalogProductCreateEntity, ByVal productID As Integer, ByVal storeView As String, ByVal identifierType As String, ByRef Updated As Boolean)
        Try
            Dim SoapReturn As String = MageHandler.catalogProductUpdate(SessionId, productID, productdata, storeView, identifierType)
            If SoapReturn.ToLower = "true" Then
                Updated = True
            End If

        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "Magento_DescriptionWrite", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        End Try

    End Sub
    Private Sub Magento_TierPricWrite(ByVal TierPriceData As catalogProductTierPriceEntity, ByVal productID As Integer, ByVal storeView As String, ByVal identifierType As String, ByRef Updated As Boolean)

        Exit Sub

        Try
            Dim SoapReturn As String = MageHandler.catalogProductUpdate(SessionId, productID, productdata, storeView, identifierType)
            If SoapReturn.ToLower = "true" Then
                Updated = True
            End If

        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "Magento_DescriptionWrite", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        End Try

    End Sub

    'PartDescription
    Private Sub AddToPartDescription(ByRef ProductDescription As List(Of PartDescription),
                                            ByVal productId As Integer,
                                            ByVal store As String,
                                            ByVal type As String,
                                            ByVal description As String,
                                            ByVal CurIndex As Integer)

        Try

            ProductDescription.Add(New PartDescription() With {
                            .productId = productId,
                            .store = store,
                            .type = type,
                            .description = description,
                            .CurIndex = CurIndex
        })
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "AddToPartDescription", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        End Try
    End Sub
    Private Sub AddToProductIndex(ByRef ProductIndex As List(Of ProductIndex),
                                            ByVal CurIndex As Integer)

        Try

            ProductIndex.Add(New ProductIndex() With {
                                     .CurIndex = CurIndex
        })
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "AddToPartDescription", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        End Try
    End Sub


    Private Class PartDescription
        Property productId As Integer
        Property store As String
        Property type As String
        Property description As String
        Property CurIndex As Integer
    End Class

    Private Class ProductIndex
        Property CurIndex As Integer
    End Class
End Class
