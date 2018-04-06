Imports Magento_API_Parameters.Mage_API
Imports Magento_API_Parameters
Imports System.Data.SqlClient

Module PrivateProperties
#Region "SQL Varialbles"

    Friend WithEvents Magento_catalogProductUpdate_da As Magento_StoreTableAdapters.Magento_catalogProductUpdateTableAdapter
    Friend WithEvents ProductUpdate_distinct_da As Magento_StoreTableAdapters.ProductUpdate_distinctTableAdapter
    Friend WithEvents ProductByFamilyDistinct_da As Magento_StoreTableAdapters.ProductByFamilyDistinctTableAdapter

    Friend WithEvents Magento_ProductCatalogImport_TierPrice_da As Magento_StoreTableAdapters.Magento_ProductCatalogImport_TierPriceTableAdapter
    Friend WithEvents Magento_ProductCatalog_TierPrice_QA_da As Magento_StoreTableAdapters.Magento_ProductCatalog_TierPrice_QATableAdapter
    Friend WithEvents Magento_ProductCatalog_TierPrice_QA_Compare_da As Magento_StoreTableAdapters.Magento_ProductCatalog_TierPrice_QA_CompareTableAdapter
    Friend WithEvents Magento_Store_ds As Magento_Store

#End Region

#Region "Magento Entities"
    Public Property storeEntityTable As storeEntity()
    Public Property MageHandler As MagentoService
    Public Property MagentoType As String = Nothing
    Public Property catalogProduct As List(Of catalogProductEntity)
    Public Property TierPrice As catalogProductTierPriceEntity

#End Region

    Public Api_Para As Magento_API_Parameters.Initialize


    Public Property TransactionID_Current As String
    Public Property ControlRoot_Current As String
    Public Property RowsImported As Integer

    Public Property SessionId As String = Nothing

    Public Property StopwatchLocal As Stopwatch = Nothing
    Public Property elapsedTime As String = Nothing
    Public Property EventLogData As Byte() = Nothing
    Public Property TransactionControlRoot As String = Nothing
    Public Property DBConnection As SqlConnection = Nothing
    Public Property DBConnection_Current As String = Nothing

    ' Public Property StoreView As String = Nothing
    Public Property ServiceCall As String
    Public Property productdata As catalogProductCreateEntity
    Public Property CurrentConString As Object
    Public Property CurrentConnection As SqlConnection

    Public Property ERP_type As String





















End Module
