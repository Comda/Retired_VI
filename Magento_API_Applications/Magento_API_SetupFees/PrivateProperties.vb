Imports System.Data.SqlClient
Imports Magento_API_Parameters.Mage_API

Module PrivateProperties
    Public Api_Para As Magento_API_Parameters.Initialize


#Region "Magento Entities"
    Public Property MageHandler As MagentoService
    Public Property MagentoType As String = Nothing
    Public Property productdata As catalogProductCreateEntity
    Public Property ProductCatalogAttributeEntity As catalogAttributeEntity()

#End Region
#Region "SQL Varialbles"

    Friend WithEvents ProductUpdateTypeTableAdapter_da As Magento_StoreTableAdapters.ProductUpdateTypeTableAdapter
    Friend WithEvents ProductByFamilyDistinct_da As Magento_StoreTableAdapters.ProductByFamilyDistinctTableAdapter
    Friend WithEvents Magento_catalogProductUpdate_da As Magento_StoreTableAdapters.Magento_catalogProductUpdateTableAdapter
    Friend WithEvents Magento_ProductCatalogImport_da As Magento_StoreTableAdapters.Magento_ProductCatalogImportTableAdapter

    Friend WithEvents Magento_Store_ds As Magento_Store

    Public Property CurrentConString As String
    Public Property CurrentConnection As SqlConnection

#End Region

    Public Property TransactionID As String
    Public Property ControlRoot As String

    Public Property SessionId As String = Nothing

    Public Property StopwatchLocal As Stopwatch = Nothing
    Public Property elapsedTime As String = Nothing
    Public Property EventLogData As Byte() = Nothing
    Public Property TransactionControlRoot As String = Nothing


End Module
