Imports System.Data.SqlClient
Imports COMDA_COM_Product_QA.Magento_API

Public Class Category
    Public Sub New(ByVal SessionId As String, ByVal dbConnection As SqlConnection, ByVal ImportID As Guid)
        InitializeSQLVariables(dbConnection)
        GetCategories(SessionId, ImportID)
    End Sub
    Private Sub GetCategories(ByVal SessionId As String, ByVal ImportID As Guid)

        'Dim MagentoType As String
        'Dim GroupFilter As New filters
        'GroupFilter = addFilter(GroupFilter, "type", "eq", "grouped")
        'MagentoType = "grouped"

        'Dim ChildFilter As New filters
        'ChildFilter = addFilter(ChildFilter, "type", "eq", "simple")
        'MagentoType = "simple"

        'Dim ParentFilter As New filters
        'ParentFilter = addFilter(ParentFilter, "type", "eq", "configurable")
        'MagentoType = "configurable"

        'Get the stores
        Dim storeEntityTable As storeEntity() = MageHandler.storeList(SessionId)

        Dim StoreView As String = Nothing

        For Each storeEntityItem As storeEntity In storeEntityTable
            StoreView = storeEntityItem.code

            Dim catalogCategoryTreeTable As catalogCategoryTree = Nothing
            'Dim catalogProductEntityTable() As catalogProductEntity = Nothing

            Try
                catalogCategoryTreeTable = MageHandler.catalogCategoryTree(SessionId, "4", StoreView)
                'catalogProductEntityTable = MageHandler.catalogProductList(SessionId, ChildFilter, StoreView)
                Stop
            Catch ex As Exception
                ' I need to get OUT because this is my baseFailing will mean NO data
                Throw New Exception("GetCategories : " & ex.Message)
            End Try

            'catalogProduct = catalogProductEntityTable.OrderBy(Function(o) o.sku).ToList()
            'If catalogProduct.Count > 0 Then
            '    UploadCatalog(catalogProduct, StoreView, ImportID)
            'End If

        Next

    End Sub
End Class
