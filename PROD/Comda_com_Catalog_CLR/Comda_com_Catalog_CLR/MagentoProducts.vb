Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports COMDA_COM_Product_QA
Imports Microsoft.SqlServer.Server


Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub MagentoProducts_GET(ByVal UserId As String, ByVal API_ID As String, ByVal ConnectionString As String, ByVal ImportID As Guid)

        Dim SessionId As String = Nothing
        Dim MagentoLogin As New Magento_Login(UserId, API_ID, SessionId)
        Dim dbConnection As New SqlConnection With {
            .ConnectionString = ConnectionString
        }

        Dim BasicCatalog As New BaseProducts(SessionId, dbConnection, ImportID)
        Dim DetailedCatalog As New DetailedProducts(SessionId, dbConnection, ImportID)

    End Sub
End Class
