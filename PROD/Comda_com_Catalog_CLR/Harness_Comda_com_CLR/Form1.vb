Imports System.Data.SqlClient
Imports COMDA_COM_Product_QA
Public Class Form1


    Private Sub b_LoadData_Click(sender As Object, e As EventArgs) Handles b_LoadData.Click

        Dim UserID As String = "jeromeb"
        Dim API_ID As String = "sophie"

        Dim SessionId As String = Nothing
        Dim MagentoLogin As New Magento_Login(UserID, API_ID, SessionId)
        Dim ConnectionString As String = "Data Source=COCAPIINTERNAL\test,2433;Initial Catalog=API_COMDA_COM;Persist Security Info=True;User ID=sa;Password=apitest2015" '"Data Source=JB-DESKTOP\SQL2016DEV;Initial Catalog=ERP_Data;Persist Security Info=True;User ID=sa;Password=sophie" ' "Data Source=COCAPIINTERNAL\TEST,2433;Initial Catalog=API_COMDA_COM;Persist Security Info=True;User ID=sa;Password=apitest2015" '"Data Source=JB-DESKTOP\SQL2016DEV;Initial Catalog=ERP_Data;Persist Security Info=True;User ID=sa;Password=sophie"

        Dim ImportID As Guid = Guid.NewGuid


        Dim dbConnection As New SqlConnection With {
            .ConnectionString = ConnectionString
        }
        'If Not Restart Then
        Dim BasicCatalog As New BaseProducts(SessionId, dbConnection, ImportID)
            Dim DetailedCatalog As New DetailedProducts(SessionId, dbConnection, ImportID)
        'Else
        '    Dim DetailedCatalog As New DetailedProducts(SessionId, dbConnection, ImportID)
        'End If

    End Sub
End Class
