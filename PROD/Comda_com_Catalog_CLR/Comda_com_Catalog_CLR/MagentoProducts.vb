Imports System
Imports System.Data.SqlClient
Imports COMDA_COM_Product_QA



Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub MagentoProducts_GET(ByVal UserId As String, ByVal API_ID As String, ByVal ConnectionString As String, ByVal ImportID As Guid, ByVal Restart As Boolean)

        Dim SessionId As String = Nothing
        Dim MagentoLogin As New Magento_Login(UserId, API_ID, SessionId)
        Dim dbConnection As New SqlConnection With {
            .ConnectionString = ConnectionString
        }
        If Not Restart Then
            Dim BasicCatalog As New BaseProducts(SessionId, dbConnection, ImportID)
            Dim DetailedCatalog As New DetailedProducts(SessionId, dbConnection, ImportID)
        Else
            Dim DetailedCatalog As New DetailedProducts(SessionId, dbConnection, ImportID)
        End If


    End Sub
End Class
