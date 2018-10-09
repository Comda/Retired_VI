Imports System.Data.SqlClient
Imports System.Net
Imports <xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
Imports <xmlns:urn="urn:Magento">
Imports <xmlns:xsd="http://www.w3.org/2001/XMLSchema">
Imports <xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
Imports <xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/">
Imports <xmlns:ns1="urn:Magento">
Imports <xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/">
Imports COMDA_COM_Product_QA.Magento_API

Public Class DetailedProducts
    Public Sub New(ByVal SessionID As String, ByVal dbConnection As SqlConnection, ByVal ImportID As Guid)
        Try


            InitializeSQLVariables(dbConnection)

            Magento_ProductCatalogImport_da.Fill(Magento_Store_ds.Magento_ProductCatalogImport, ImportID, "API_CALL")
            'Magento_ProductCatalogImport_da.Fill(Magento_Store_ds.Magento_ProductCatalogImport, ImportID, "UPDATED")
            Dim ProductData As DataTable = Magento_Store_ds.Magento_ProductCatalogImport
            'GoTo DetailsOnly
            Dim Category As String = "Catalog"
            Dim RequestName As String = "catalogProductInfo"

            Magento_SOAP_Requests_da.Fill(Magento_Store_ds.Magento_SOAP_Requests, Category, RequestName)

            Dim doc As XDocument = XDocument.Parse(Magento_Store_ds.Magento_SOAP_Requests.Rows(0).Item("SOAPRequest"))

DetailsOnly:
            Dim ProductId As Integer = Nothing
            Dim store_name As String = Nothing
            Dim sku As String = Nothing
            Dim Status As Integer = Nothing


            If ProductData.Rows.Count > 0 Then
                For i As Integer = 0 To ProductData.Rows.Count - 1
                    ProductId = CInt(ProductData.Rows(i).Item("product_id"))
                    store_name = CStr(ProductData.Rows(i).Item("store"))
                    sku = ProductData.Rows(i).Item("sku").ToString
                    'Debug.Print("Sku {0}  Des: {1}", sku, ProductData.Rows(i).Item("name").ToString)

                    'If ProductId <> 386 Then GoTo SKIP

                    Dim ResponseDoc As XDocument = XML_Request_catalogProductInfo(SessionID, ProductId, Status, doc)

                    If Not IsNothing(ResponseDoc) Then
                        ProductData.Rows(i).Item("ImportDescription") = "UPDATED"
                        ProductData.Rows(i).Item("Magento_Status") = Status
                        ProductData.Rows(i).Item("Magento_Product_Info") = ResponseDoc

                        Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)

                        GetCatalogOptions(SessionID, ProductId, store_name, sku, ImportID)
                    Else
                        ProductData.Rows(i).Item("ImportDescription") = "NOT UPDATED"
                        ProductData.Rows(i).Item("Magento_Status") = 0
                    End If

                    GetCatalogOptions(SessionID, ProductId, store_name, sku, ImportID)

SKIP:
                Next
            End If
        Catch ex As Exception
            Throw New Exception("DetailedProducts : " & ex.Message)
        End Try
    End Sub

    Public Function XML_Request_catalogProductInfo(ByVal SessionID As String, ByVal ProductId As Integer, ByRef Status As Integer, ByVal doc As XDocument) As XDocument
        Dim ResponseDoc As XDocument = Nothing
        Try
            Status = Nothing
            Using client = New WebClient()

                'Dim doc As XDocument = XDocument.Load("C:\Users\jboyer\source\repos\request_catalogProductInfo.xml")

                For Each desc In From element In doc.<soapenv:Envelope>.<soapenv:Body>.<urn:catalogProductInfo>
                    desc.Element("sessionId").Value = SessionID
                    desc.Element("product").Value = ProductId
                Next

                Dim Request As String = doc.ToString

                client.Headers.Add("Content-Type", "text/xml;charset=utf-8")
                Dim response = client.UploadString("https://secure.comda.com/index.php/api/v2_soap/index/", Request)


                Dim content As String = response
                content = RemoveInvalidXmlChars(content)

                ResponseDoc = XDocument.Parse(content)

                'some heavy lifting to get the status
                For Each resp In From element In ResponseDoc.<SOAP-ENV:Envelope>.<SOAP-ENV:Body>.<ns1:catalogProductInfoResponse>.<info>
                    Status = resp.Element("status").Value
                Next
            End Using
        Catch ex As Exception
            'Error not trapped. ResponseDoc will be null and Sataus will be Nothing
            'Throw New Exception("XML_Request_catalogProductInfo : " & ex.Message)
        End Try

        Return ResponseDoc
    End Function

    Private Sub GetCatalogOptions(ByVal SessionID As String, ByVal ProductId As Integer, ByVal store_name As String, ByVal Short_Sku As String, ByVal ImportID As Guid)

        Dim ImportDate As Date = Now()
        Dim catalogProductCustomOptionInfo_t As catalogProductCustomOptionInfoEntity
        Try

            Dim catalogProductCustomOptionListEntity_t As catalogProductCustomOptionListEntity()

            'get some values
            catalogProductCustomOptionListEntity_t = MageHandler.catalogProductCustomOptionList(SessionID, ProductId, store_name)
            'find the Option ID
            'Dim OptionID As Integer = 0

            If catalogProductCustomOptionListEntity_t.Length > 0 Then

                Dim Options_List_Color = New List(Of Options)
                Dim Options_List_Size = New List(Of Options)
                Dim Options_List_Other = New List(Of Options)
                Dim OptionGroupList As New List(Of OptionGroup)
                Dim SKU_BY_Order_List As New List(Of SKU_BY_Order)
                'Dim SKU_BY_Order_List_2 As New List(Of SKU_BY_Order)

                Dim name As String = Nothing
                Dim product_id As Integer = ProductId
                Dim option_id As Integer = Nothing
                Dim base_sku As String = Nothing
                Dim option_sku As String = Nothing
                Dim sku As String = Nothing
                Dim ImportDescription As String = Nothing
                Dim SortOrder As Integer = 0

                'Dim dbContext As String = Nothing

                For Each item As catalogProductCustomOptionListEntity In catalogProductCustomOptionListEntity_t
                    'add the to Option Group...
                    OptionGroupList.Add(New OptionGroup() With {
                        .option_id = item.option_id,
                        .name = item.title,
                        .SortOrder = item.sort_order
                    })

                Next
                'Sort the OptionGroup
                OptionGroupList = OptionGroupList.OrderByDescending(Function(o) o.SortOrder).ToList()

                'For eac OptionGroup, get the SKU. Sort 2 will catenate to Sort 1, one at a time. do we neeed by reverse sort order
                For Each Opt As OptionGroup In OptionGroupList
                    Dim OptionGroup_id As Integer = Opt.option_id
                    Dim SortOrder_x As Integer = Opt.SortOrder
                    Try
                        catalogProductCustomOptionInfo_t = MageHandler.catalogProductCustomOptionInfo(SessionID, OptionGroup_id, store_name)
                        'we now have have the Options by Sort order
                        'We put them in a list and catenate all the skus
                        Dim AdditionalFields As catalogProductCustomOptionAdditionalFieldsEntity() = catalogProductCustomOptionInfo_t.additional_fields
                        Try
                            For Each itemx As catalogProductCustomOptionAdditionalFieldsEntity In AdditionalFields
                                'add to a list with sort order and sku
                                'Debug.Print("Sort: {0}  SKU:   {1}", SortOrder_x, itemx.sku)
                                SKU_BY_Order_List.Add(New SKU_BY_Order() With {
                                    .option_sku = itemx.sku,
                                    .SortOrder = SortOrder_x,
                                    .title = Opt.name
                    })

                                'option_sku = option_sku & itemx.sku
                            Next
                        Catch ex As Exception

                        End Try
                    Catch ex As Exception
                        GoTo NextItem
                    End Try



NextItem:
                Next

                Dim SKU_BY_Order_List_1 As New List(Of SKU_BY_Order)
                SKU_BY_Order_List_1.AddRange(SKU_BY_Order_List.FindAll(Function(x) x.SortOrder = 1))

                Dim SKU_BY_Order_List_2 As New List(Of SKU_BY_Order)
                SKU_BY_Order_List_2.AddRange(SKU_BY_Order_List.FindAll(Function(x) x.SortOrder = 2))

                Dim SKU_BY_Order_List_3 As New List(Of SKU_BY_Order)
                SKU_BY_Order_List_2.AddRange(SKU_BY_Order_List.FindAll(Function(x) x.SortOrder = 3))

                'Dim Final_SKU_Option As String = Nothing

                'name = "" 'opt.title
                'product_id = ProductId
                'option_id = option_id
                base_sku = Short_Sku
                ImportDescription = store_name
                'sku = base_sku & option_sku

                'we need to catenate all 2 to all 1
                If SKU_BY_Order_List_1.Count > 0 Then
                    For Each sku_1 In SKU_BY_Order_List_1
                        option_id = sku_1.SortOrder
                        If SKU_BY_Order_List_2.Count > 0 Then
                            For Each sku_2 In SKU_BY_Order_List_2
                                option_id = sku_2.SortOrder
                                If SKU_BY_Order_List_3.Count > 0 Then
                                    For Each sku_3 In SKU_BY_Order_List_3
                                        option_id = sku_3.SortOrder
                                        Debug.Print(Short_Sku & sku_1.option_sku & sku_2.option_sku & sku_3.option_sku)
                                        name = sku_1.title & "|" & sku_2.title & "|" & sku_3.title
                                        sku = Short_Sku & sku_1.option_sku & sku_2.option_sku & sku_3.option_sku
                                        option_sku = sku_1.option_sku & "|" & sku_2.option_sku & "|" & sku_3.option_sku
                                        CatalogDetails_SET(
                                                name,
                                                product_id,
                                                option_id,
                                                base_sku,
                                                option_sku,
                                                sku,
                                                ImportDescription,
                                                ImportDate,
                                                ImportID)
                                    Next
                                Else
                                    Debug.Print(Short_Sku & sku_1.option_sku & sku_2.option_sku)
                                    sku = Short_Sku & sku_1.option_sku & sku_2.option_sku
                                    option_sku = sku_1.option_sku & "|" & sku_2.option_sku
                                    name = sku_1.title & "|" & sku_2.title
                                    CatalogDetails_SET(
                                            name,
                                            product_id,
                                            option_id,
                                            base_sku,
                                            option_sku,
                                            sku,
                                            ImportDescription,
                                            ImportDate,
                                            ImportID)
                                End If
                            Next
                        Else
                            Debug.Print(Short_Sku & sku_1.option_sku)
                            sku = Short_Sku & sku_1.option_sku
                            option_sku = sku_1.option_sku & "|"
                            name = sku_1.title & "|"
                            CatalogDetails_SET(
                                    name,
                                    product_id,
                                    option_id,
                                    base_sku,
                                    option_sku,
                                    sku,
                                    ImportDescription,
                                    ImportDate,
                                    ImportID)
                        End If


                    Next
                End If

                '
                'Stop

                'SKU_BY_Order_List.Exists(Function(x) x.SortOrder = 2)

                '            Dim query = SKU_BY_Order_List.GroupBy(Function(m) m.SortOrder).
                'Select(Function(g) New MovieCategory With {
                '    .Title = g.Key,
                '    .Items = g.ToList()
                '})

                'Dim List2Count As Integer = SKU_BY_Order_List.Count(SortOrder >= SortOrder = 2)

                'For i As Integer = 1 To SKU_BY_Order_List_Count

                'Next




                'Debug.WriteLine("SKU  {0}, ", option_sku)

                'Debug.Print("ID {0}  Option Title {1}", item.option_id, item.title)

                'Select Case name.ToUpper
                '    Case "COLOR", "COLORS"
                '        Dim AdditionalFields As catalogProductCustomOptionAdditionalFieldsEntity() = catalogProductCustomOptionInfo_t.additional_fields
                '        For Each itemx As catalogProductCustomOptionAdditionalFieldsEntity In AdditionalFields
                '            Debug.WriteLine("SKU  {0}, Title {1}  ID {2} New SKU:  {3}", itemx.sku, item.title, itemx.value_id, Short_Sku & itemx.sku)

                '            name = item.title
                '            product_id = ProductId
                '            option_id = option_id
                '            base_sku = Short_Sku
                '            option_sku = itemx.sku
                '            sku = base_sku & option_sku


                '            AddRowToOptions(Options_List_Color,
                '                     name,
                '                     product_id,
                '                     option_id,
                '                     option_sku)

                '        Next
                '    Case "SIZE"
                '        Dim AdditionalFields As catalogProductCustomOptionAdditionalFieldsEntity() = catalogProductCustomOptionInfo_t.additional_fields
                '        For Each itemx As catalogProductCustomOptionAdditionalFieldsEntity In AdditionalFields
                '            Debug.WriteLine("SKU  {0}, Title {1}  ID {2} New SKU:  {3}", itemx.sku, item.title, itemx.value_id, Short_Sku & itemx.sku)

                '            name = item.title
                '            product_id = ProductId
                '            option_id = option_id
                '            base_sku = Short_Sku
                '            option_sku = itemx.sku
                '            sku = base_sku & option_sku


                '            AddRowToOptions(Options_List_Size,
                '                     name,
                '                     product_id,
                '                     option_id,
                '                     option_sku)

                '        Next
                '    Case Else
                '        Dim AdditionalFields As catalogProductCustomOptionAdditionalFieldsEntity() = catalogProductCustomOptionInfo_t.additional_fields
                '        For Each itemx As catalogProductCustomOptionAdditionalFieldsEntity In AdditionalFields
                '            Debug.WriteLine("SKU  {0}, Title {1}  ID {2} New SKU:  {3}", itemx.sku, item.title, itemx.value_id, Short_Sku & itemx.sku)

                '            name = item.title
                '            product_id = ProductId
                '            option_id = option_id
                '            base_sku = Short_Sku
                '            option_sku = itemx.sku
                '            sku = base_sku & option_sku


                '            AddRowToOptions(Options_List_Other,
                '                     name,
                '                     product_id,
                '                     option_id,
                '                     option_sku)

                '        Next

                'End Select



                'If I have SIZE, I need to add Size + Colors as sku decoration...

                'If Options_List_Size.Count > 0 Then
                '    'catenate base_sku with option_sku. Add to sku list. Add each color.

                '    For Each Size As Options In Options_List_Size
                '        For Each Color As Options In Options_List_Color
                '            Debug.Print(base_sku & Size.option_sku & Color.option_sku)
                '            sku = base_sku & Size.option_sku & Color.option_sku
                '            CatalogDetails_SET(
                '                                    name,
                '                                    product_id,
                '                                    option_id,
                '                                    base_sku,
                '                                    option_sku,
                '                                    sku,
                '                                    ImportDescription,
                '                                    ImportDate,
                '                                    ImportID)

                '        Next
                '    Next

                'Else
                '    'treat all options as post master code except the colors as above
                '    For Each opt As Options In Options_List_Color
                '        'name = opt.title
                '        'product_id = ProductId
                '        'option_id = option_id
                '        base_sku = Short_Sku
                '        option_sku = opt.option_sku
                '        sku = base_sku & option_sku

                '        CatalogDetails_SET(
                '                                     name,
                '                                     product_id,
                '                                     option_id,
                '                                     base_sku,
                '                                     option_sku,
                '                                     sku,
                '                                     ImportDescription,
                '                                     ImportDate,
                '                                     ImportID)
                '    Next

                '    For Each opt As Options In Options_List_Other
                '        'name = opt.title
                '        'product_id = ProductId
                '        'option_id = option_id
                '        base_sku = Short_Sku
                '        option_sku = opt.option_sku
                '        sku = base_sku & option_sku

                '        CatalogDetails_SET(
                '                                     name,
                '                                     product_id,
                '                                     option_id,
                '                                     base_sku,
                '                                     option_sku,
                '                                     sku,
                '                                     ImportDescription,
                '                                     ImportDate,
                '                                     ImportID)
                '    Next


            End If


            'End If
        Catch ex As Exception
            'Throw New Exception("GetCatalogOptions : " & ex.Message)
        End Try

        Magento_ProductCatalogImport_SKU_Details_da.Update(Magento_Store_ds.Magento_ProductCatalogImport_SKU_Details)
    End Sub
    Private Sub AddRowToOptions(ByRef Options_List As List(Of Options),
                                    ByVal name As String,
                                    ByVal product_id As Integer,
                                    ByVal option_id As Integer,
                                    ByVal option_sku As String)
        'Dim parts As New List(Of Part)()

        ' Add parts to the list.
        Options_List.Add(New Options() With {
             .name = name,
             .product_id = product_id,
             .option_id = option_id,
             .option_sku = option_sku
        })

    End Sub

    Friend Sub CatalogDetails_SET(
                                    ByVal name As String,
                                    ByVal product_id As Integer,
                                    ByVal option_id As Integer,
                                    ByVal base_sku As String,
                                    ByVal option_sku As String,
                                    ByVal sku As String,
                                    ByVal store As String,
                                    ByVal ImportDate As Date,
                                    ByVal ImportID As Guid)


        Dim newProductsRow As DataRow = Magento_Store_ds.Magento_ProductCatalogImport_SKU_Details.NewRow()

        newProductsRow("name") = name
        newProductsRow("product_id") = product_id
        newProductsRow("option_id") = option_id
        newProductsRow("base_sku") = base_sku
        newProductsRow("option_sku") = option_sku
        newProductsRow("sku") = sku
        newProductsRow("store") = store
        newProductsRow("ImportDate") = ImportDate
        newProductsRow("ImportID") = ImportID

        Magento_Store_ds.Magento_ProductCatalogImport_SKU_Details.Rows.Add(newProductsRow)


    End Sub
    Public Class Options
        Public Property name As String
        Public Property product_id As Integer
        Public Property option_id As Integer
        Public Property option_sku As String
    End Class
    Public Class OptionGroup
        Public Property option_id As Integer
        Public Property name As String
        Public Property SortOrder As Integer
    End Class
    Public Class SKU_BY_Order
        Public Property SortOrder As Integer
        Public Property option_sku As String
        Public Property title As String
    End Class
End Class
