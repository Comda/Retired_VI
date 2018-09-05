Imports System.Xml
Imports COMDA_COM_Product_QA.Magento_API

Module CommonMethods
    Friend Sub InitializeSQLVariables(ByVal dbConnection As SqlClient.SqlConnection)

        Magento_Store_ds = New MagentoStore

        Magento_ProductCatalogImport_da = New MagentoStoreTableAdapters.Magento_ProductCatalogImportTableAdapter With {
            .Connection = dbConnection
        }

        Magento_ProductCatalogImport_SKU_Details_da = New MagentoStoreTableAdapters.Magento_ProductCatalogImport_SKU_DetailsTableAdapter With {
            .Connection = dbConnection
        }

        Magento_SOAP_Requests_da = New MagentoStoreTableAdapters.Magento_SOAP_RequestsTableAdapter With {
            .Connection = dbConnection
        }

    End Sub
    Friend Sub UploadCatalog(ByVal catalogProduct As List(Of catalogProductEntity), ByVal StoreView As String, ByRef ImportID As Guid)

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
                'newProductsRow("dbContext") = dbContext


                newProductsRow("ImportID") = ImportID
                newProductsRow("ImportDescription") = "API_CALL"
                newProductsRow("ImportDate") = ImportDate


                Magento_Store_ds.Magento_ProductCatalogImport.Rows.Add(newProductsRow)

            Next

            Magento_ProductCatalogImport_da.Update(Magento_Store_ds.Magento_ProductCatalogImport)


        Catch ex As Exception
            Throw New Exception("UploadCatalog : " & ex.Message)
        End Try
    End Sub

    Public Function ReturnStringFromIds(ByVal Entity As Object) As String

        Dim EntityString As String = Nothing

        For i As Integer = 0 To Entity.Length - 1
            EntityString = EntityString & Entity(i) & ","
        Next
        If Not IsNothing(EntityString) Then
            Return EntityString.Substring(0, EntityString.Length - 1)
        Else
            Return ""
        End If
    End Function

#Region "Magento"

    Public Function addFilter(filtresIn As filters, key As String, op As String, value As String) As filters
        Dim filtres As filters = filtresIn
        If filtres Is Nothing Then
            filtres = New filters()
        End If

        Dim compfiltres As New complexFilter With {
            .key = key
        }
        Dim ass As New associativeEntity With {
            .key = op,
            .value = value
        }
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

    Friend Function IsValidXmlString(ByVal text As String) As Boolean
        Try
            XmlConvert.VerifyXmlChars(text)
            Return True
        Catch
            Return False
        End Try

    End Function
    Friend Function RemoveInvalidXmlChars(ByVal text As String) As String
        Dim validXmlChars As Char() = text.Where(Function(ch) XmlConvert.IsXmlChar(ch)).ToArray()
        Return New String(validXmlChars)
    End Function




End Module
