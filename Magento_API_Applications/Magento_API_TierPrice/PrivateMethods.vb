
Module PrivateMethods

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

    Public Sub InitializeSQLVariables(ByVal dbConnection As SqlClient.SqlConnection)

        Magento_ProductCatalogImport_TierPrice_da = New Magento_StoreTableAdapters.Magento_ProductCatalogImport_TierPriceTableAdapter
        Magento_ProductCatalog_TierPrice_QA_da = New Magento_StoreTableAdapters.Magento_ProductCatalog_TierPrice_QATableAdapter
        Magento_ProductCatalog_TierPrice_QA_da.Connection = dbConnection
        Magento_ProductCatalogImport_TierPrice_da.Connection = dbConnection
        Magento_Store_ds = New Magento_Store

        Magento_ProductCatalogImport_TierPrice_da.Fill(Magento_Store_ds.Magento_ProductCatalogImport_TierPrice)


    End Sub
End Module
