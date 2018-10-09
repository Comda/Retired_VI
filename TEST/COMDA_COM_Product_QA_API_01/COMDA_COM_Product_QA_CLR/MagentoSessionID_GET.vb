Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports COMDA_COM_Product_QA
Imports Microsoft.SqlServer.Server

Partial Public Class UserDefinedFunctions
    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function MagentoSessionID_GET(ByVal UserId As String, ByVal API_ID As String) As SqlString
        Dim SessionId As String = Nothing
        Dim MagentoLogin As New Magento_Login(UserId, API_ID, SessionId)
        Return SessionId
    End Function

End Class
