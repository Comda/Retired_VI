Imports System.Data.SqlClient
Imports COMDA_COM_Product_QA.Magento_API

Public Class PricingData

    Private Property catalogProductPrices As List(Of catalogProductTierPriceEntity)

    Public Sub GetPrices(ByVal SessionId As String, ByVal dr As DataRow, ByVal Status As String, ByVal dbConnection As SqlConnection, ByVal ImportID As Guid)

        InitializeSQLVariables(dbConnection)

        Magento_ProductCatalogImport_da.Fill(Magento_Store_ds.Magento_ProductCatalogImport, ImportID, "API_CALL")
        'Magento_ProductCatalogImport_da.Fill(Magento_Store_ds.Magento_ProductCatalogImport, ImportID, "UPDATED")
        Dim ProductData As DataTable = Magento_Store_ds.Magento_ProductCatalogImport

        Dim ProductId As Integer
        Dim store_name As String
        Dim TierPrice As catalogProductTierPriceEntity()

        If ProductData.Rows.Count > 0 Then
            For i As Integer = 0 To ProductData.Rows.Count - 1
                ProductId = CInt(ProductData.Rows(i).Item("product_id"))
                store_name = CStr(ProductData.Rows(i).Item("store"))

                TierPrice = MageHandler.catalogProductAttributeTierPriceInfo(SessionId, ProductId, 4)
                catalogProductPrices = TierPrice.OrderBy(Function(o) o.qty).ToList()
                If catalogProductPrices.Count > 0 Then
                    UploadPrices(catalogProductPrices, store_name, ProductId, ImportID)
                End If
            Next
        End If

    End Sub



End Class
