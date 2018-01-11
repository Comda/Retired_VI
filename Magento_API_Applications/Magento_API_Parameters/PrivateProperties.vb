Imports Magento_API_Parameters.Mage_API

Module PrivateProperties
#Region "Common Variables"
    Public Property CreatedBy As String
    Public Property StopwatchLocal As Stopwatch
    Public Property TransactionID As String
    Public Property ControlRoot As String
    Public Property SessionId As String
    Public Property DBConnectionString As String = Nothing


    Public Property Comment As String = Nothing
    Public Property FunctionRequested As String = Nothing

    Public Property Active As List(Of String)
    Public Property SourceAPI As String = Nothing
#End Region


#Region "Magento Entities"
    Public Property storeEntityTable As storeEntity()
    Public Property MageHandler As MagentoService
    Public Property MagentoType As String = Nothing


#End Region
#Region "SQL Varialbles"

    Friend WithEvents Magento_ProductCatalogMatch_da As Magento_StoreTableAdapters.Magento_ProductCatalogMatchTableAdapter
    Friend WithEvents Magento_ProductCatalogImport_da As Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter
    Friend WithEvents MatchTable_da As Magento_StoreTableAdapters.Magento_ProductCatalogImportMatchTableAdapter
    Friend WithEvents Magento_Store_ds As Magento_Store

#End Region

End Module
