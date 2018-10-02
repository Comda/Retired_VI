Imports System.Data.SqlClient

Module PrivateValues

    Friend WithEvents HIT_ds As HIT_Product
    Friend WithEvents PartLocation_dt As HIT_ProductTableAdapters.API_PACS_PartLocationTableAdapter
    Friend WithEvents PartArray_dt As HIT_ProductTableAdapters.API_PACS_PartTableAdapter
    Friend WithEvents PartPrice_dt As HIT_ProductTableAdapters.API_PACS_PartPriceTableAdapter
    Friend WithEvents LocationArray_dt As HIT_ProductTableAdapters.API_PACS_LocationTableAdapter
    Friend WithEvents DecorationArray_dt As HIT_ProductTableAdapters.API_PACS_DecorationTableAdapter
    Friend WithEvents ChargeArray_dt As HIT_ProductTableAdapters.API_PACS_ChargeTableAdapter
    Friend WithEvents ChargePriceArray_dt As HIT_ProductTableAdapters.API_PACS_ChargePriceTableAdapter

    Public Property PACS As HIT_PricingConfiguration.PricingAndConfigurationService
    Public Property Reply As HIT_PricingConfiguration.GetConfigurationAndPricingResponse
    Public Property Request As HIT_PricingConfiguration.GetConfigurationAndPricingRequest


End Module
