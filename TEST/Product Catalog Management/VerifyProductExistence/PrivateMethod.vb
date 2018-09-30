Imports System.Data.SqlClient

Module PrivateMethod
    Public Sub Initialize(dbConnection As SqlConnection)
        ProductData_ds = New ProductData
        SelectByMagentoCriteria_da = New ProductDataTableAdapters.SelectByMagentoCriteriaTableAdapter With {
            .Connection = dbConnection
        }

        Reconstructing_Masterno_From_Magento_da = New ProductDataTableAdapters.Reconstructing_Masterno_From_MagentoTableAdapter With {
            .Connection = dbConnection
        }

    End Sub
End Module
