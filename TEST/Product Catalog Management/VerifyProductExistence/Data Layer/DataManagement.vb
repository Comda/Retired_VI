Imports System.Data.SqlClient

Public Class DataManagement
    Public Function GetProductUniverse(dbConnection As SqlConnection, ByVal Criteria As String) As DataTable
        Initialize(dbConnection)
        SelectByMagentoCriteria_da.Fill(ProductData_ds.SelectByMagentoCriteria, Criteria, "N", Nothing)
        Return ProductData_ds.SelectByMagentoCriteria
    End Function

    Public Function GetReconstructedMasterNo(dbConnection As SqlConnection, ByVal ProductId As Integer, ByRef CountMasterNo As Integer) As DataTable
        Initialize(dbConnection)
        Reconstructing_Masterno_From_Magento_da.Fill(ProductData_ds.Reconstructing_Masterno_From_Magento, ProductId, "N", CountMasterNo)
        Return ProductData_ds.Reconstructing_Masterno_From_Magento
    End Function

End Class
