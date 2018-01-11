Imports Magento_API_Parameters.Mage_API

Public Class CustomOptions
    Public Sub New(ByVal dbConnection As SqlClient.SqlConnection, ByVal CurrentSessionID As String, ByVal TransactionID_Current As String, ByVal ControlRoot_Current As String)

        SessionId = CurrentSessionID

        'first loop is unique ERP
        'ProductUpdateTypeTableAdapter_da = New Magento_StoreTableAdapters.ProductUpdateTypeTableAdapter
        'ProductByFamilyDistinct_da = New Magento_StoreTableAdapters.ProductByFamilyDistinctTableAdapter
        'Magento_catalogProductUpdate_da = New Magento_StoreTableAdapters.Magento_catalogProductUpdateTableAdapter
        'Magento_ProductCatalogImport_da = New Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter


        'Magento_Store_ds = New Magento_Store

        'ProductUpdateTypeTableAdapter_da.Connection = dbConnection
        'ProductByFamilyDistinct_da.Connection = dbConnection
        'Magento_catalogProductUpdate_da.Connection = dbConnection
        'Magento_ProductCatalogImport_da.Connection = dbConnection

        'ProductUpdateTypeTableAdapter_da.Fill(Magento_Store_ds.ProductUpdateType)
        'Magento_ProductCatalogImport_da.Fill(Magento_Store_ds.Magento_ProductCatalogImport)

        MageHandler = New MagentoService
        TransactionID = TransactionID_Current
        ControlRoot = ControlRoot_Current

        StopwatchLocal = New Stopwatch()
        StopwatchLocal.Start()

        Api_Para = New Magento_API_Parameters.Initialize
        Api_Para.WriteEventToLog("Info", "Synchronize Options", "", StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)

        'If Magento_Store_ds.ProductUpdateType.Rows.Count > 0 Then
        '    For i As Integer = 0 To Magento_Store_ds.ProductUpdateType.Rows.Count - 1
        '        GetProductsToUpdate(Magento_Store_ds.ProductUpdateType.Rows(i).Item("type"))
        '    Next
        'End If
        Dim ProductId As Integer = 9841
        Dim store_name As String = "mapleleaf_usd_en"
        GetCatalogOptions(ProductId, store_name)
    End Sub
    'get catalaog Options

    Private Sub GetCatalogOptions(ByVal ProductId As Integer, ByVal store_name As String)
        'catalogProductCustomOptionList
        MageHandler = New MagentoService
        Dim catalogProductCustomOptionListEntity_t As catalogProductCustomOptionListEntity()

        'get some values
        catalogProductCustomOptionListEntity_t = MageHandler.catalogProductCustomOptionList(SessionId, ProductId, store_name)
        'find the Option ID
        Dim OptionID As Integer = 0

        For Each item As catalogProductCustomOptionListEntity In catalogProductCustomOptionListEntity_t
            If item.title = "Imprint location" Then
                OptionID = item.option_id
                Exit For
            End If
        Next
        'get the Option data
        'catalogProductCustomOptionInfo

        Dim catalogProductCustomOptionInfo_t As catalogProductCustomOptionInfoEntity = MageHandler.catalogProductCustomOptionInfo(SessionId, OptionID, store_name)
        'the data is in additional fields (associative entity) A bummer to update?

        'the sku looks like: MG_W_CRAYON_S|IMPLOC_FRONT -- we can as many skus as we have locations. Keep all the main sku together and loopthorugh location.
        'Dim sku As String = "MG_W_CRAYON_S|IMPLOC_FRONT"
        'Dim ActualSKU As String = sku.Split("|")(0)


        Dim AdditionalFields As catalogProductCustomOptionAdditionalFieldsEntity() = catalogProductCustomOptionInfo_t.additional_fields
        For Each item As catalogProductCustomOptionAdditionalFieldsEntity In AdditionalFields
            Debug.WriteLine("SKU  {0}, Title {1}  ID {2}", item.sku, item.title, item.value_id)
        Next

        'update   FRONT Imprint Location, Max 2 ⅜" x ¾"
        AdditionalFields(0).title = "NO IMPRINT LOCATION"
        Dim data As New catalogProductCustomOptionToUpdate
        data.additional_fields = AdditionalFields
        'data.title = ""

        'Dim x As Object = MageHandler.catalogProductCustomOptionUpdate(SessionId, OptionID, data, store_name)

        'Dim yx As Object = MageHandler.catalogProductCustomOptionUpdate(SessionId, OptionID, data, "")
    End Sub

End Class
