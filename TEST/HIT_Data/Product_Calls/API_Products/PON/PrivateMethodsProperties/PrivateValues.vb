
Module PrivateValues

    'UNIQUE to PON

    Friend WithEvents HIT_ds As HIT_Product
    Friend WithEvents Category_dt As HIT_ProductTableAdapters.API_PON_CategoryTableAdapter
    Friend WithEvents Packaging_dt As HIT_ProductTableAdapters.API_PON_PackagingTableAdapter
    Friend WithEvents ShippingPackage_dt As HIT_ProductTableAdapters.API_PON_ShippingPackageTableAdapter
    Friend WithEvents Product_dt As HIT_ProductTableAdapters.API_PON_ProductTableAdapter
    Friend WithEvents ProductDescription_dt As HIT_ProductTableAdapters.API_PON_ProductDescriptionTableAdapter
    Friend WithEvents ProductMarketingPoint_dt As HIT_ProductTableAdapters.API_PON_ProductMarketingPointTableAdapter
    Friend WithEvents ProductKeyword_dt As HIT_ProductTableAdapters.API_PON_ProductKeywordTableAdapter
    Friend WithEvents ProductPart_dt As HIT_ProductTableAdapters.API_PON_ProductPartTableAdapter
    Friend WithEvents PartDescription_dt As HIT_ProductTableAdapters.API_PON_PartDescriptionTableAdapter
    Friend WithEvents PartApparelSize_dt As HIT_ProductTableAdapters.API_PON_PartApparelSizeTableAdapter
    Friend WithEvents PartDimension_dt As HIT_ProductTableAdapters.API_PON_PartDimensionTableAdapter
    Friend WithEvents PartColor_dt As HIT_ProductTableAdapters.API_PON_PartColorTableAdapter

    Public Property PON As HIT_productData.ProductDataService
    Public Property Reply As HIT_productData.GetProductResponse
    Public Property Request As HIT_productData.GetProductRequest


End Module
