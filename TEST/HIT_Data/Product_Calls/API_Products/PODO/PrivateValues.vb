Imports System.Data.SqlClient

Module PrivateValues

    Friend WithEvents HIT_ds As HIT_Product
    Friend WithEvents PropertyPairs_dt As HIT_ProductTableAdapters.API_PODO_PropertyPairsTableAdapter

    Public Property PODO As HIT_productData.ProductControllerService
    Public Property Reply As Object
    'Public Property Request As HIT_PricingConfiguration.GetConfigurationAndPricingRequest


End Module
