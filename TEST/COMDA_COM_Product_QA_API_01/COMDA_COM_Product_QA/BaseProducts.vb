Imports COMDA_COM_Product_QA.Magento_API
Imports System.Data.SqlClient

Public Class BaseProducts
    Private Property catalogProduct As List(Of catalogProductEntity)

    Public Sub New(ByVal SessionId As String, ByVal dbConnection As SqlConnection, ByVal ImportID As Guid)
        InitializeSQLVariables(dbConnection)
        GetCurrentCatalog(SessionId, ImportID)
    End Sub

    Private Sub GetCurrentCatalog(ByVal SessionId As String, ByVal ImportID As Guid)
        Try


            Dim MagentoType As String
            Dim GroupFilter As New filters
            GroupFilter = addFilter(GroupFilter, "type", "eq", "grouped")
            MagentoType = "grouped"

            Dim ChildFilter As New filters
            ChildFilter = addFilter(ChildFilter, "type", "eq", "simple")
            MagentoType = "simple"

            Dim ParentFilter As New filters
            ParentFilter = addFilter(ParentFilter, "type", "eq", "configurable")
            MagentoType = "configurable"

            'Get the stores
            Dim storeEntityTable As storeEntity() = MageHandler.storeList(SessionId)

            Dim StoreView As String = Nothing

            For Each storeEntityItem As storeEntity In storeEntityTable
                StoreView = storeEntityItem.code

                Dim catalogProductEntityTable() As catalogProductEntity = Nothing

                Try
                    catalogProductEntityTable = MageHandler.catalogProductList(SessionId, ChildFilter, StoreView)

                Catch ex As Exception
                    ' I need to get OUT because this is my baseFailing will mean NO data
                    Throw New Exception("GetCurrentCatalog : " & ex.Message)
                End Try

                catalogProduct = catalogProductEntityTable.OrderBy(Function(o) o.sku).ToList()
                If catalogProduct.Count > 0 Then
                    UploadCatalog(catalogProduct, StoreView, ImportID)
                End If

            Next
        Catch ex As Exception
            Throw New Exception("GetCurrentCatalog : " & ex.Message)
        End Try
    End Sub

End Class
