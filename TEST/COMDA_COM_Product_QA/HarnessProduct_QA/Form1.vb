Imports System.Data.SqlClient
Imports COMDA_COM_Product_QA


Public Class Form1
    Private Sub bLogin_Click(sender As Object, e As EventArgs) Handles bLogin.Click
        Try
            Dim UserId As String = tb_UserID.Text
            Dim API_ID As String = tb_API_ID.Text
            Dim SessionId As String = Nothing

            Dim MagentoLogin As New Magento_Login(UserId, API_ID, SessionId)

            tb_SessionID.Text = SessionId
        Catch ex As Exception
            Console.WriteLine(" bLogin_Click: " & ex.Message)
        End Try


    End Sub

    Private Sub bBasicCatalog_Click(sender As Object, e As EventArgs) Handles bBasicCatalog.Click

        Dim UserId As String = tb_UserID.Text
        Dim API_ID As String = tb_API_ID.Text
        Dim SessionId As String = Nothing

        Dim MagentoLogin As New Magento_Login(UserId, API_ID, SessionId)

        tb_SessionID.Text = SessionId

        Dim dbConnection As New SqlConnection With {
            .ConnectionString = "Data Source=cocapiinternal\test,2433;Initial Catalog=API_COMDA_COM;Persist Security Info=True;User ID=sa;Password=apitest2015" '"Data Source=JB-FAST\DEVEL_2016;Initial Catalog=API_Internal_SVS_CLR;Persist Security Info=True;User ID=sa;Password=sophie;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True"
        }

        Dim ImportID As Guid = Guid.Parse("DF14A094-CEE6-40B1-B651-963F549C69E0") '- -Guid.NewGuid()
        'Dim BasicCatalog As New BaseProducts(SessionId, dbConnection, ImportID)
        Dim DetailedCatalog As New DetailedProducts(SessionId, dbConnection, ImportID)

        MessageBox.Show("DONE")
    End Sub
End Class
