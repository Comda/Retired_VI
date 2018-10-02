Imports System.Data.SqlClient

Module PrivateValues

    Friend WithEvents HIT_ds As HIT_Product
    Friend WithEvents Images_dt As HIT_ProductTableAdapters.API_PODO_ImagesTableAdapter

    Public Property PODO As HIT_productData.ProductObjectControllerService
    Public Property Reply As HIT_productData.ProductImageResult
    'Public Property Request As HIT_PricingConfiguration.GetConfigurationAndPricingRequest


End Module
