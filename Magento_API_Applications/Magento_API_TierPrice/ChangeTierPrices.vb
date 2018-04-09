Imports System.IO
Imports Magento_API_Parameters
Imports Magento_API_Parameters.Mage_Api
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class ChangeTierPrices


#Region "Instance Creation"
#Region "Create Tier Price and Update Magento"
    Public Sub New(ByVal dbConnection As SqlClient.SqlConnection, ByVal CurrentSessionID As String, ByVal TransactionID As Guid, ByVal CreateTierPrice As Boolean)
        'This NEW sub will upate Tier Prices based on Magento_catalogProductUpdate  where TansactionID select one, many or all Products to be updated

        'Create the Tier Price Universe Option 1 ALL products from Magento_ProductCatalogImport
        '                               Option 2 Products in the Magento_catalogProductUpdate 

        'in all events current table will be updated with a unique ID
        CreatePriceData = CreateTierPrice

        SessionId = CurrentSessionID

        MageHandler = New Mage_Api.MagentoService
        InitializeSQLVariables(dbConnection)

        Magento_ProductCatalog_TierPrice_Universe_GET_da.Fill(Magento_Store_ds.Magento_ProductCatalog_TierPrice_Universe_GET, TransactionID)


        Dim ProductId As Integer
        Dim storeView As String
        Dim TransID As DataTable = Magento_Store_ds.Magento_ProductCatalog_TierPrice_Universe_GET


        Dim TransactionID_v As DataView = New DataView(TransID)
        Dim TransactionID_dt As DataTable = TransactionID_v.ToTable(True, "TransactionID")
        For iTrans As Integer = 0 To TransactionID_dt.Rows.Count - 1

            Dim TransactionID_Filter As String
            TransactionID_Filter = "TransactionID ='" & TransactionID_dt.Rows(iTrans).Item("TransactionID").ToString & "'"

            Dim foundRows() As DataRow
            ' Use the Select method to find all rows matching the filter.
            foundRows = TransID.Select(TransactionID_Filter)

            Dim Prd As DataTable
            Prd = foundRows.CopyToDataTable()



            For iPrd As Integer = 0 To Prd.Rows.Count - 1
                ProductId = CInt(Prd.Rows(iPrd).Item("product_id"))
                storeView = CStr(Prd.Rows(iPrd).Item("store"))
                Dim dr As DataRow = Prd.Rows(iPrd)
                Status = "Get Prices"
                GetPrices(SessionId, ProductId, storeView, dr, Status)

            Next

        Next


        If CreateTierPrice Then

            For iTrans As Integer = 0 To TransactionID_dt.Rows.Count - 1

                Dim TransactionID_Filter As String
                TransactionID_Filter = "TransactionID ='" & TransactionID_dt.Rows(0).Item("TransactionID").ToString & "'"

                Dim foundRows() As DataRow
                ' Use the Select method to find all rows matching the filter.
                foundRows = TransID.Select(TransactionID_Filter)

                Magento_ProductCatalog_TierPrice_QA_da.Fill(Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA, TransactionID_dt.Rows(0).Item("TransactionID"))

                Dim Prd As DataTable = Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA
                For iPrd = 0 To Prd.Rows.Count - 1
                    Dim dr As DataRow = Prd.Rows(iPrd)
                    'only run for rows where comprae is 0 and PRICE GRID EXISTS
                    If dr.Item("Compared") = 0 And dr.Item("Status") = "PRICE GRID EXISTS" Then
                        UpdateMagentoTierPriceData(SessionId, dr)
                    End If
                Next
            Next
        End If


    End Sub
#End Region
#Region "Unused NEW sub"
    Public Sub New(ByVal TransactionId As String, ByVal dbConnection As SqlClient.SqlConnection, ByVal CurrentSessionID As String, ByVal CreateTierPrice As Boolean)
        'The Updates assume: a) Magento_ProductCatalog_TierPrice_QA is populated, b) Current Tier Price GRID is in

        SessionId = CurrentSessionID
        MageHandler = New Mage_Api.MagentoService
        InitializeSQLVariables(dbConnection)

        Magento_ProductCatalog_TierPrice_QA_da = New Magento_StoreTableAdapters.Magento_ProductCatalog_TierPrice_QATableAdapter
        Magento_ProductCatalog_TierPrice_QA_da.Connection = dbConnection


        Dim storeView As String = Nothing
        Dim ProductId As Integer = 0

        Dim Prd As DataTable = Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA
        For iPrd As Integer = 0 To Prd.Rows.Count - 1
            ProductId = CInt(Prd.Rows(iPrd).Item("product_id"))
            storeView = CStr(Prd.Rows(iPrd).Item("store"))
            Dim dr As DataRow = Prd.Rows(iPrd)
            Dim NewTierPrice As String = CreateTierPriceData(SessionId, ProductId, storeView, CStr(dr.Item("TierPriceGrid")))
            UpdateTierPriceData(CurrentSessionID, ProductId, NewTierPrice, storeView, dr)
        Next


    End Sub

    Public Sub New(ByVal dbConnection As SqlClient.SqlConnection, ByVal CurrentSessionID As String, ByVal CompareAll As Integer)


        InitializeSQLVariables(dbConnection)

        SessionId = CurrentSessionID

        Magento_ProductCatalog_TierPrice_QA_Compare_da.Fill(Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA_Compare)

        Dim ProductId As Integer
        Dim storeView As String


        Dim Prd As DataTable = Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA_Compare

        For iPrd As Integer = 0 To Prd.Rows.Count - 1
            ProductId = CInt(Prd.Rows(iPrd).Item("product_id"))
            storeView = CStr(Prd.Rows(iPrd).Item("store"))
            Dim dr As DataRow = Prd.Rows(iPrd)
            If Not CheckDBnull(dr.Item("TierPriceGrid")) Then
                If Not CheckDBnull(dr.Item("TierPriceData")) Then
                    Dim NewTierPrice As String = CreateTierPriceData(SessionId, ProductId, storeView, CStr(dr.Item("TierPriceGrid")))
                    dr.Item("TierPriceData_Created") = NewTierPrice
                    If Not CheckDBnull(NewTierPrice) Then
                        If NewTierPrice.ToLower.Equals(CStr(dr.Item("TierPriceData")).ToLower) Then
                            dr.Item("Compared") = 1
                        End If
                    End If
                End If
            End If

        Next

        Magento_ProductCatalog_TierPrice_QA_Compare_da.Update(Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA_Compare)

    End Sub

#End Region
#End Region
#Region "Create Tier Price PSEUDO JSON"
    Private Function CreateTierPriceData(ByVal SessionId As String, ByVal ProductId As Integer, ByVal storeView As String, ByVal TierPriceGrid As String) As String

        Dim TierPriceBuilt As String = Nothing
        Try


            If TierPriceGrid.Length < 10 Then
                Return TierPriceBuilt
            End If
            '"website_id":"1"
            '"all_groups""1"
            '"cust_group":"32000"

            '{"32000-50":{"website_id":"1","all_groups":"1","price":"6.95","cust_group":"32000","price_qty":"50.0000","website_price":"6.95"}}
            'If TierPriceGrid.Length > 10 Then Stop

            Dim t_Grid_all = New List(Of RootObject)
            t_Grid_all = JsonConvert.DeserializeObject(Of List(Of RootObject))(TierPriceGrid)
            'find all the levels
            'select by store
            Dim t_Grid_Store = New List(Of RootObject)


            Dim website As String = Nothing
            Select Case storeView
                Case "en_ca"
                    website = "mapleleaf_cad"
                Case "en_us"
                    website = "mapleleaf_usd"
            End Select

            Dim tierPseudo_string As New List(Of TierPseudo)

            t_Grid_Store.AddRange(t_Grid_all.Where(Function(x) x.website = website).ToList)

            Dim itp As Integer = 0


            For Each tp As RootObject In t_Grid_Store
                'Debug.Print(tp.qty)
                ' need to fill the pseudo

                Dim website_id As String = "1"
                Dim all_groups As String = "1"
                Dim cust_group As String = "32000"
                Dim Root As String = cust_group & "-" & tp.qty
                Dim price As String = tp.price.ToString("f2")
                Dim price_qty As String = tp.qty.ToString("f4")
                Dim website_price As String = price

                AddToTierPseudo_List(tierPseudo_string,
                                    Root,
                                    website_id,
                                    all_groups,
                                    cust_group,
                                    price,
                                    price_qty,
                                    website_price)

            Next


            'we need to format the pseudo... can we use serialize? by list row and catenate?
            itp = 0
            TierPriceBuilt = ""


            For Each tp As TierPseudo In tierPseudo_string
                Dim setOUT As String = JsonConvert.SerializeObject(tp)
                'Debug.Print(setOUT)
                If itp = 0 Then
                    'the values in tierPseudo_string FIRST row must be adjusted
                    setOUT = setOUT.Replace("""Root"":", "")
                    setOUT = setOUT.Replace(",""website_id""", ":{""website_id""")
                    TierPriceBuilt = TierPriceBuilt & setOUT
                    Debug.Print(setOUT)
                End If


                If itp > 0 Then
                    'the values in tierPseudo_string SECOND and AFTER rows must be adjusted
                    setOUT = setOUT.Replace("{""Root"":", ",")
                    setOUT = setOUT.Replace(",""website_id""", ":{""website_id""")
                    TierPriceBuilt = TierPriceBuilt & setOUT
                    Debug.Print(setOUT)
                End If

                itp = itp + 1
            Next

            TierPriceBuilt = TierPriceBuilt & "}"


        Catch ex As Exception
            'Throw New System.Exception(ex.Message)
        End Try

        Return TierPriceBuilt

        Debug.Print(TierPriceBuilt)
        '[{"customer_group_id":"all","website":"comda_usd","qty":150,"qtySpecified":true,"price":0.85,"priceSpecified":true},{"customer_group_id":"all","website":"mapleleaf_cad","qty":150,"qtySpecified":true,"price":1.16,"priceSpecified":true},{"customer_group_id":"all","website":"mapleleaf_usd","qty":150,"qtySpecified":true,"price":0.85,"priceSpecified":true},{"customer_group_id":"all","website":"comda_usd","qty":250,"qtySpecified":true,"price":0.79,"priceSpecified":true},{"customer_group_id":"all","website":"mapleleaf_cad","qty":250,"qtySpecified":true,"price":1.1,"priceSpecified":true},{"customer_group_id":"all","website":"mapleleaf_usd","qty":250,"qtySpecified":true,"price":0.79,"priceSpecified":true},{"customer_group_id":"all","website":"mapleleaf_usd","qty":500,"qtySpecified":true,"price":0.75,"priceSpecified":true},{"customer_group_id":"all","website":"mapleleaf_cad","qty":500,"qtySpecified":true,"price":1.05,"priceSpecified":true},{"customer_group_id":"all","website":"comda_usd","qty":500,"qtySpecified":true,"price":0.75,"priceSpecified":true},{"customer_group_id":"all","website":"comda_usd","qty":1000,"qtySpecified":true,"price":0.72,"priceSpecified":true},{"customer_group_id":"all","website":"mapleleaf_cad","qty":1000,"qtySpecified":true,"price":0.96,"priceSpecified":true},{"customer_group_id":"all","website":"mapleleaf_usd","qty":1000,"qtySpecified":true,"price":0.72,"priceSpecified":true}]
        '{"32000-150":{"website_id":"1","all_groups":"1","price":"1.16","cust_group":"32000","price_qty":"150.0000","website_price":"1.16"},"32000-250":{"website_id":"1","all_groups":"1","price":"1.1","cust_group":"32000","price_qty":"250.0000","website_price":"1.1"},"32000-500":{"website_id":"1","all_groups":"1","price":"1.05","cust_group":"32000","price_qty":"500.0000","website_price":"1.05"},"32000-1000":{"website_id":"1","all_groups":"1","price":"0.96","cust_group":"32000","price_qty":"1000.0000","website_price":"0.96"}}


        'original
        '{"32000-48":{"website_id":"1","all_groups":"1","price":"9.94","cust_group":"32000","price_qty":"48.0000","website_price":"9.94"},"32000-96":{"website_id":"1","all_groups":"1","price":"8.64","cust_group":"32000","price_qty":"96.0000","website_price":"8.64"},"32000-144":{"website_id":"1","all_groups":"1","price":"7.52","cust_group":"32000","price_qty":"144.0000","website_price":"7.52"},"32000-288":{"website_id":"1","all_groups":"1","price":"6.54","cust_group":"32000","price_qty":"288.0000","website_price":"6.54"},"32000-576":{"website_id":"1","all_groups":"1","price":"5.68","cust_group":"32000","price_qty":"576.0000","website_price":"5.68"}}
        '{"32000-48":{"website_id":"1","all_groups":"1","price":"9.94","cust_group":"32000","price_qty":"48.0000","website_price":"9.94"},"32000-96":{"website_id":"1","all_groups":"1","price":"8.64","cust_group":"32000","price_qty":"96.0000","website_price":"8.64"},"32000-144":{"website_id":"1","all_groups":"1","price":"7.52","cust_group":"32000","price_qty":"144.0000","website_price":"7.52"},"32000-288":{"website_id":"1","all_groups":"1","price":"6.54","cust_group":"32000","price_qty":"288.0000","website_price":"6.54"},"32000-576":{"website_id":"1","all_groups":"1","price":"5.68","cust_group":"32000","price_qty":"576.0000","website_price":"5.68"}}





    End Function
#End Region
#Region "Update Tier Price PSEUDO JSON"
    Private Sub UpdateMagentoTierPriceData(ByVal CurrentSessionID As String, ByVal dr As DataRow)
        StopwatchLocal = New Stopwatch()
        StopwatchLocal.Start()

        Dim ProductId As Integer = 0
        Dim TierPriceBuilt As String = ""
        Dim CurStore As String = Nothing
        Dim Updated As Boolean = False

        Try

            ProductId = dr.Item("product_id")
            CurStore = dr.Item("store")


            'If Not CheckDBnull(dr.Item("TierPriceData_Created")) Then
            '    TierPriceBuilt = dr.Item("TierPriceData_Created")
            'Else
            '    GoTo NoUpdate
            'End If



            Dim KeyName As String = Nothing
            Dim KeyValue As String = Nothing

            MagentoType = "configurable"

            productdata = New catalogProductCreateEntity

            Dim AdditionalAttributes As associativeEntity() = New associativeEntity(0) {}
            Dim setup_feeAdditionalAttribute As New associativeEntity()
            KeyName = "tier_price_data"

            KeyValue = TierPriceBuilt

            setup_feeAdditionalAttribute.key = KeyName
            setup_feeAdditionalAttribute.value = KeyValue

            'Debug.WriteLine("Key Name {0} :  {1}", KeyName, KeyValue)
            AdditionalAttributes(0) = setup_feeAdditionalAttribute

            Dim AdditionalAttributesEntity As New catalogProductAdditionalAttributesEntity()
            AdditionalAttributesEntity.single_data = AdditionalAttributes
            productdata.additional_attributes = AdditionalAttributesEntity

            Dim id As Integer = 0
            Dim RowFilter As String = "PROD"

            'Debug.WriteLine("Type {0}  Count {1}  Time {2}  ProdID {3}  Entering Store {4}", MagentoType, id, StopwatchLocal.Elapsed, ProductId, CurStore)
            Magento_DescriptionWrite(productdata, ProductId, CurStore, MagentoType, Updated)
            Debug.WriteLine("Type {0}  Count {1}  Time {2}  SKU {3}  Leaving Store {4}", MagentoType, id, StopwatchLocal.Elapsed, RowFilter, CurStore)
        Catch ex As Exception
            'Throw New System.Exception(ex.Message)
        End Try
        Try
            Dim tier_price_data As String = Nothing
            If Updated Then
                dr.Item("Processed") = Now()
                'we want to check if Price data is in 
                tier_price_data = CheckForPriceData(SessionId, ProductId, CurStore, dr, Status)
                If CheckDBnull(tier_price_data) Then
                    Status = "Tier_Price_Data NOT FOUND"
                Else
                    Status = "Tier_Price_Data  UPDATED"
                    dr.Item("Compared") = 1
                End If
            Else
                Status = "Tier_Price_Data NOT UPDATED"
            End If
            dr.Item("Status") = Status
            Magento_ProductCatalog_TierPrice_QA_da.Update(Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA)

        Catch ex As Exception
            'Throw New System.Exception(ex.Message)
        End Try
NoUpdate:


    End Sub

#End Region
#Region "Support Code"
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
    Private Function CheckDBnull(ByVal testVar As Object) As Boolean
        Return IsDBNull(testVar) Or IsNothing(testVar)
    End Function
    Private Sub AddToTierPseudo_List(ByRef tierPseudo_string As List(Of TierPseudo),
                                    ByVal Root As String,
                                    ByVal website_id As String,
                                    ByVal all_groups As String,
                                    ByVal cust_group As String,
                                    ByVal price As String,
                                    ByVal price_qty As String,
                                    ByVal website_price As String)

        tierPseudo_string.Add(New TierPseudo() With {
           .all_groups = all_groups,
           .cust_group = cust_group,
           .price = price,
           .price_qty = price_qty,
           .Root = Root,
           .website_id = website_id,
           .website_price = website_price
        })

    End Sub
#End Region
#Region "Get Prices From Grid"
    Private Function CheckForPriceData(ByVal SessionId As String, ByVal ProductId As Integer, ByVal storeView As String, ByVal dr As DataRow, ByVal Status As String) As String

        Dim identifierType As String = "configurable"
        Dim NewTierPrice As String = Nothing
        'Dim Compare As Boolean
        Dim TierPrice_Data As String = Nothing
        Dim TierPrice_Grid As String = Nothing

        Try


            Dim attributes As catalogProductRequestAttributes
            attributes = New catalogProductRequestAttributes

            Dim AttribNameString As String
            Dim AttribNameArray() As String

            AttribNameString = "tier_price_data"
            AttribNameArray = AttribNameString.Split(",")

            attributes.additional_attributes = AttribNameArray

            Dim cpre As catalogProductReturnEntity = MageHandler.catalogProductInfo(SessionId, ProductId, storeView, attributes, identifierType)
            TierPrice_Data = cpre.additional_attributes(0).value

            'Dim TierPrice As catalogProductTierPriceEntity() = MageHandler.catalogProductAttributeTierPriceInfo(SessionId, ProductId, 4)
            'TierPrice_Grid = JsonConvert.SerializeObject(TierPrice)

            'Status = "PRICE GRID EXISTS"

            If CheckDBnull(TierPrice_Data) Then
                Status = "NO PRICE DATA"
            End If

            If TierPrice_Data.Length < 10 Then
                UpdateMagentoTierPriceData(SessionId, dr)
                Status = "NO PRICE DATA"
            End If

            'Dim website As String = Nothing
            'Select Case storeView
            '    Case "en_ca"
            '        website = "mapleleaf_cad"
            '    Case "en_us"
            '        website = "mapleleaf_usd"
            'End Select

            'If TierPrice_Grid.IndexOf(website) = -1 Then
            '    TierPrice_Grid = ""
            'End If


            'If Not CheckDBnull(TierPrice_Grid) Then
            '    'If Not CheckDBnull(TierPrice_Data) Then
            '    NewTierPrice = CreateTierPriceData(SessionId, ProductId, storeView, TierPrice_Grid)
            '    If Not CheckDBnull(NewTierPrice) Then
            '        If Not CheckDBnull(TierPrice_Data) Then
            '            If NewTierPrice.ToLower.Equals(TierPrice_Data.ToLower) Then
            '                Compare = 1
            '            End If
            '        End If
            '    End If
            '    'End If
            'End If
        Catch ex As Exception
            Dim maxLength As Integer = Math.Min(ex.Message.Length, 200)
            Dim ErrMess As String = ex.Message.Substring(0, maxLength)
            Status = ErrMess
            'Throw New System.Exception(ex.Message)
        End Try

        Return TierPrice_Data
    End Function

    Private Sub GetPrices(ByVal SessionId As String, ByVal ProductId As Integer, ByVal storeView As String, ByVal dr As DataRow, ByVal Status As String)

        Dim identifierType As String = "configurable"
        Dim NewTierPrice As String = Nothing
        Dim Compare As Boolean
        Dim TierPrice_Data As String = Nothing
        Dim TierPrice_Grid As String = Nothing

        Try


            Dim attributes As catalogProductRequestAttributes
            attributes = New catalogProductRequestAttributes

            Dim AttribNameString As String
            Dim AttribNameArray() As String

            AttribNameString = "tier_price_data"
            AttribNameArray = AttribNameString.Split(",")

            attributes.additional_attributes = AttribNameArray

            Dim cpre As catalogProductReturnEntity = MageHandler.catalogProductInfo(SessionId, ProductId, storeView, attributes, identifierType)
            TierPrice_Data = cpre.additional_attributes(0).value

            Dim TierPrice As catalogProductTierPriceEntity() = MageHandler.catalogProductAttributeTierPriceInfo(SessionId, ProductId, 4)
            TierPrice_Grid = JsonConvert.SerializeObject(TierPrice)

            Status = "PRICE GRID EXISTS"

            If CheckDBnull(TierPrice_Grid) Then
                Status = "NO PRICE GRID"
            End If

            If TierPrice_Grid.Length < 10 Then
                'UpdateMagentoTierPriceData(SessionId, dr)
                Status = "NO PRICE GRID"
            End If
            'ICS  notes if TierPrice_Grid has no values for the store then treat it as blank.

            Dim website As String = Nothing
            Select Case storeView
                Case "en_ca"
                    website = "mapleleaf_cad"
                Case "en_us"
                    website = "mapleleaf_usd"
            End Select

            If TierPrice_Grid.IndexOf(website) = -1 Then
                Status = "PRICE GRID EXISTS - NO VALID STORE"
            End If


            If Not CheckDBnull(TierPrice_Grid) Then
                If TierPrice_Grid.Length > 10 Then
                    'If Not CheckDBnull(TierPrice_Data) Then
                    NewTierPrice = CreateTierPriceData(SessionId, ProductId, storeView, TierPrice_Grid)
                    If Not CheckDBnull(NewTierPrice) Then
                        If Not CheckDBnull(TierPrice_Data) Then
                            If NewTierPrice.ToLower.Equals(TierPrice_Data.ToLower) Then
                                Compare = 1
                                Status = "PRICE GRID EXISTS"
                            End If
                        End If
                    End If
                    End If
                    'End If
                End If
        Catch ex As Exception
            Dim maxLength As Integer = Math.Min(ex.Message.Length, 200)
            Dim ErrMess As String = ex.Message.Substring(0, maxLength)
            Status = ErrMess
            'Throw New System.Exception(ex.Message)
        End Try
        'Insert into the database
        'Dim Processed As Date
        'If CreatePriceData Then
        '    Processed = Now()
        'End If
        Try


            Dim newProductsRow = Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA.NewRow()


            newProductsRow("product_id") = ProductId

            newProductsRow("sku") = dr.Item("sku")
            newProductsRow("type") = identifierType
            newProductsRow("name") = dr.Item("name")
            newProductsRow("website_ids") = dr.Item("website_ids")
            newProductsRow("store") = storeView
            newProductsRow("dbContext") = "PROD"
            newProductsRow("TierPriceData") = TierPrice_Data
            newProductsRow("TierPriceGrid") = TierPrice_Grid
            newProductsRow("ImportDate") = Now()
            newProductsRow("Compared") = Compare
            newProductsRow("TransactionID") = dr.Item("TransactionID")
            newProductsRow("TierPriceData_Created") = NewTierPrice
            newProductsRow("OriginalTransactionID") = dr.Item("OriginalTransactionID")
            newProductsRow("Status") = Status
            newProductsRow("StatusDateTime") = Now
            Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA.Rows.Add(newProductsRow)

            Magento_ProductCatalog_TierPrice_QA_da.Update(Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA)
            Status = ""
        Catch ex As Exception

        End Try

    End Sub
#End Region
#Region "Support Classes"
    Public Class TierPseudo
        Property Root As String
        Property website_id As String
        Property all_groups As String
        Property price As String
        Property cust_group As String
        Property price_qty As String
        Property website_price As String
    End Class

    Public Class TierPriceLevels
        Public Property website As String
        Public Property price As Decimal
        Public Property customer_group_id As String
        Public Property qty As Integer

    End Class
    Public Class RootObject
        Public Property customer_group_id As String
        Public Property website As String
        Public Property qty As Integer
        Public Property qtySpecified As Boolean
        Public Property price As Double
        Public Property priceSpecified As Boolean
    End Class
#End Region

#Region "Unused code"
    Private Sub UpdateTierPriceData(ByVal CurrentSessionID As String, ByVal ProductId As Integer, ByVal TierPriceBuilt As String, ByVal CurStore As String, ByVal dr As DataRow)
        StopwatchLocal = New Stopwatch()
        StopwatchLocal.Start()
        Try


            'Dim CurStore As String = "en_us"
            Dim Updated As Boolean = False
            Dim KeyName As String = Nothing
            Dim KeyValue As String = Nothing

            MagentoType = "configurable"

            productdata = New catalogProductCreateEntity

            Dim AdditionalAttributes As associativeEntity() = New associativeEntity(0) {}
            Dim setup_feeAdditionalAttribute As New associativeEntity()
            KeyName = "tier_price_data"

            KeyValue = TierPriceBuilt

            setup_feeAdditionalAttribute.key = KeyName
            setup_feeAdditionalAttribute.value = KeyValue

            'Debug.WriteLine("Key Name {0} :  {1}", KeyName, KeyValue)
            AdditionalAttributes(0) = setup_feeAdditionalAttribute

            Dim AdditionalAttributesEntity As New catalogProductAdditionalAttributesEntity()
            AdditionalAttributesEntity.single_data = AdditionalAttributes
            productdata.additional_attributes = AdditionalAttributesEntity

            Dim id As Integer = 0
            Dim RowFilter As String = "TEST"

            Debug.WriteLine("Type {0}  Count {1}  Time {2}  ProdID {3}  Entering Store {4}", MagentoType, id, StopwatchLocal.Elapsed, ProductId, CurStore)
            Magento_DescriptionWrite(productdata, ProductId, CurStore, MagentoType, Updated)
            Debug.WriteLine("Type {0}  Count {1}  Time {2}  SKU {3}  Leaving Store {4}", MagentoType, id, StopwatchLocal.Elapsed, RowFilter, CurStore)
        Catch ex As Exception

        End Try
        Try
            dr.Item("Processed") = Now()
            Magento_ProductCatalog_TierPrice_QA_da.Update(Magento_Store_ds.Magento_ProductCatalog_TierPrice_QA)
        Catch ex As Exception

        End Try



    End Sub
    Private Sub UpdatePrices(ByVal CurrentSessionID As String, ByVal ProductId As Integer)
        'array of product ID
        'Array of Tier prices - from a string
        Dim txtFileName As String = "Q:\COMDA_REPO\VI\Magento_API_Applications\Magento_API_TierPrice\json3.json"


        Dim txtFileText As String
        ' The StreamReader must be defined outside of the Try-Catch block
        '   in order to reference it in the Finally block.
        Dim myStreamReader As StreamReader

        ' Ensure that the creation of the new StreamReader is wrapped in a 
        '   Try-Catch block, since an invalid filename could have been used.
        Try
            ' Create a StreamReader using a Shared (static) File class.
            myStreamReader = File.OpenText(txtFileName)
            ' Read the entire file in one pass, and add the contents to 
            '   txtFileText text box.
            txtFileText = myStreamReader.ReadToEnd()
        Catch exc As Exception
            ' Show the exception to the user.
            MsgBox("File could not be opened or read." + vbCrLf +
                "Please verify that the filename is correct, " +
                "and that you have read permissions for the desired " +
                "directory." + vbCrLf + vbCrLf + "Exception: " + exc.Message)
        Finally
            ' Close the object if it has been created.
            If Not myStreamReader Is Nothing Then
                myStreamReader.Close()
            End If
        End Try

        'get the json extract into the tire price data
        'txtFileText = txtFileText.Replace("vbCrLf", "")
        'Dim reader As JsonTextReader = New JsonTextReader(New StringReader(txtFileName))

        'Dim FileData As StreamReader = File.OpenText(txtFileName)

        'Dim reader As JsonTextReader = New JsonTextReader(FileData)

        'Dim TP As New TierPriceLevels
        'Dim deserializedProduct As List(Of TierPriceLevels) = JsonConvert.DeserializeObject(TP)(txtFileText)

        'Dim json As String = File.ReadAllText("Q:\COMDA_REPO\VI\Magento_API_Applications\Magento_API_TierPrice\TierPrice.json")
        'Dim o As JObject = JObject.Parse(json)

        'For Each x As Object In o
        '    'Debug.Print(JsonConvert.DeserializeObject(Of TierPriceLevels)(x.ToString))
        '    Debug.Print(x.ToString)
        '    JsonConvert.DeserializeObject(Of TierPriceLevels)(x.ToString)
        'Next

        Dim tp As New List(Of TierPriceLevels)
        tp.Add(New TierPriceLevels() With {
            .customer_group_id = "32000",
            .price = "1.09",
            .qty = "150.0000",
            .website = "mapleleaf_usd"
                    })

        tp.Add(New TierPriceLevels() With {
            .customer_group_id = "32000",
            .price = "1.03",
            .qty = "250.0000",
            .website = "mapleleaf_usd"
        })

        tp.Add(New TierPriceLevels() With {
            .customer_group_id = "32000",
            .price = "0.99",
            .qty = "500.0000",
            .website = "mapleleaf_usd"
        })

        tp.Add(New TierPriceLevels() With {
            .customer_group_id = "32000",
            .price = "0.90",
            .qty = "1000.0000",
            .website = "mapleleaf_usd"
        })


        'Dim TierPrice As catalogProductTierPriceEntity() = catalogProductTierPriceEntity()
        'TierPrice = New catalogProductTierPriceEntity()

        Dim TierPrice As Magento_API_Parameters.Mage_Api.catalogProductTierPriceEntity() = MageHandler.catalogProductAttributeTierPriceInfo(SessionId, ProductId, 4)

        'Dim catalogProductCustomOptionInfo_t As catalogProductCustomOptionInfoEntity = MageHandler.catalogProductCustomOptionInfo(SessionId, OptionID, store_name)
        'Dim AdditionalFields As catalogProductCustomOptionAdditionalFieldsEntity() = catalogProductCustomOptionInfo_t.additional_fields

        Dim strserialize As String = JsonConvert.SerializeObject(tp)
        Dim strserialize_Source As String = JsonConvert.SerializeObject(TierPrice)
        'Dim tp_New = JsonConvert.DeserializeObject(Of List(Of TierPriceLevels))(strserialize)

        Dim tp_New = JsonConvert.DeserializeObject(Of List(Of catalogProductTierPriceEntity))(strserialize)

        'For Each tpl As TierPriceLevels In tp_New
        '    TierPrice(0).qty = tpl.price_qty
        '    TierPrice(0).price = tpl.price
        '    TierPrice(0).customer_group_id = tpl.cust_group
        '    TierPrice(0).website = tpl.website_id

        'Next
        Dim TierPrice_List As List(Of catalogProductTierPriceEntity) = TierPrice.ToList

        TierPrice_List.AddRange(tp_New)

        'Dim a As List(Of String) = ("One,Two").Split(",").ToList
        'a.Add("Three")

        ' I need to ADD a new range of values per Custom group per site...
        For i As Integer = 0 To TierPrice_List.Count - 1
            Debug.Print("Web Site {0}, Qty_Inc {1}, Group_Id {2}", TierPrice_List(i).website, TierPrice_List(i).qty, TierPrice_List(i).customer_group_id)
        Next

        Dim strserialize_complete As String = JsonConvert.SerializeObject(TierPrice_List)
        Stop
        'Try
        '    MageHandler.catalogProductAttributeTierPriceUpdate(CurrentSessionID, ProductId, tp_New.ToArray, 1)
        'Catch ex As Exception

        'End Try
        StopwatchLocal = New Stopwatch()
        StopwatchLocal.Start()

        Dim CurStore As String = "en_us"
        Dim Updated As Boolean = False
        Dim KeyName As String = Nothing
        Dim KeyValue As String = Nothing
        MagentoType = "configurable"

        productdata = New catalogProductCreateEntity

        Dim AdditionalAttributes As associativeEntity() = New associativeEntity(0) {}
        Dim setup_feeAdditionalAttribute As New associativeEntity()
        KeyName = "tier_price_data"
        'KeyName = CStr(ProdList.Rows(id).Item("name")) ' "setup_fee"
        'Dim SetupFee_Current As Decimal = CDec(ProdList.Rows(id).Item("value"))
        'KeyValue = CStr(SetupFee_Current)
        KeyValue = txtFileText 'strserialize 'strserialize_complete

        setup_feeAdditionalAttribute.key = KeyName
        setup_feeAdditionalAttribute.value = KeyValue

        Debug.WriteLine("Key Name {0} :  {1}", KeyName, KeyValue)
        AdditionalAttributes(0) = setup_feeAdditionalAttribute

        Dim AdditionalAttributesEntity As New catalogProductAdditionalAttributesEntity()
        AdditionalAttributesEntity.single_data = AdditionalAttributes
        productdata.additional_attributes = AdditionalAttributesEntity

        Dim id As Integer = 0
        Dim RowFilter As String = "TEST"

        Debug.WriteLine("Type {0}  Count {1}  Time {2}  ProdID {3}  Entering Store {4}", MagentoType, id, StopwatchLocal.Elapsed, ProductId, CurStore)
        'Magento_DescriptionWrite(productdata, ProductId, CurStore, MagentoType, Updated)
        Debug.WriteLine("Type {0}  Count {1}  Time {2}  SKU {3}  Leaving Store {4}", MagentoType, id, StopwatchLocal.Elapsed, RowFilter, CurStore)

        'From the synch table


        'Try
        '    Try
        'ProductByFamilyDistinct_da.Fill(Magento_Store_ds.ProductByFamilyDistinct, ERP_type)

        'Dim ProdUnique As DataTable = Magento_Store_ds.ProductByFamilyDistinct ' Mage_API_Store_DS.ProductUpdate_distinct

        'Dim ProdList As DataTable = Magento_Store_ds.Magento_catalogProductUpdate

        'Dim ProdAll As DataView = ProdList.DefaultView
        'Dim RowFilter As String = Nothing
        'ProdAll.Sort = "store ASC"


        'For id As Integer = 0 To ProdUnique.Rows.Count - 1 'figure out the sku 'separate by store/description type
        '    RowFilter = "sku='" & ProdUnique.Rows(id).Item("sku") & "'"
        '    ProdAll.RowFilter = RowFilter 'we have 4 or more rows we need to group by store

        '    Dim ProductDescription = New List(Of PartDescription)
        '    Dim ProductIndex As New List(Of ProductIndex)

        '    For ip As Integer = 0 To ProdAll.Count - 1
        '        'select a store
        '        Dim ProductId As Integer = GetProductId(ProdAll(ip).Item("sku"))
        '        Dim NewDescription As String = ProdAll(ip).Item("value")
        '        Dim DescriptionType As String = ProdAll(ip).Item("name")
        '        Dim store As String = ProdAll(ip).Item("store")
        '        If ProductId <> 0 Then
        '            AddToPartDescription(ProductDescription, ProductId, store, DescriptionType, NewDescription, ip)
        '            AddToProductIndex(ProductIndex, ip)
        '        End If

        '    Next
        '    If ProductDescription.Count = 0 Then
        '        GoTo SKIP_Prod
        '    End If

        '    Dim Updated As Boolean = False
        '    Dim DefaultSore As Boolean = True
        '    Dim CurStore As String = Nothing
        '    Dim FoOBJ As New List(Of PartDescription)
        '    For Each item As PartDescription In ProductDescription
        '        If CurStore <> item.store Then
        '            CurStore = item.store
        '            Dim curIndex As Integer = item.CurIndex

        '            'Dim prodid As Integer = 0
        '            If Not IsNothing(ProductDescription.FindAll(Function(x) x.store.Equals(item.store.Trim))) Then
        '                FoOBJ = ProductDescription.FindAll(Function(x) x.store.Equals(item.store.Trim))
        '            End If




        '            For ix = 0 To FoOBJ(0).description.Split("|").Length - 1
        '                'TierPrice(ix).qty = FoOBJ(0).description.Split("|")(ix).Split(",")(0)
        '            Next









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
        '                If DefaultSore Then
        '                    Magento_TierPricWrite(TierPrice, item.productId, CurStore, MagentoType, Updated)
        '                    'Magento_DescriptionWrite(productdata, item.productId, "", MagentoType, Updated)
        '                    DefaultSore = False
        '                End If
        '                'Magento_DescriptionWrite(productdata, item.productId, CurStore, MagentoType, Updated)
        '                Magento_TierPricWrite(TierPrice, item.productId, CurStore, MagentoType, Updated)

        '                End If

        '                Next

        '                For Each Index As ProductIndex In ProductIndex
        '                    ProdAll(Index.CurIndex).Item("DateUpdated") = Now()
        '                    ProdAll(Index.CurIndex).Item("Result") = Updated
        '                    ProdAll(Index.CurIndex).Item("InUpdateUniverse") = Not Updated
        '                    Debug.WriteLine(ProdAll(Index.CurIndex).Item("store"))
        '                Next
        'SKIP_Prod:


        '                Next
        '            Catch ex As Exception
        '                Api_Para.WriteEventToLog("Error", "GetProductsToUpdate_B4 Update DB", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        '            End Try

        '            Magento_catalogProductUpdate_da.Update(Magento_Store_ds.Magento_catalogProductUpdate)

        '        Catch ex As Exception
        '            Api_Para.WriteEventToLog("Error", "GetProductsToUpdate", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        '        End Try

        '        Api_Para.WriteEventToLog("Info", "GetProductsToUpdate", "COMPLETED", StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
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

#End Region
End Class
