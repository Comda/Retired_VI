Imports System.Data.SqlClient
Imports COMDA_COM_Product_QA_API_1


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

        MessageBox.Show("DONE")

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

        Dim ImportID As Guid = Guid.Parse("A23FAF46-51B3-42E1-BE00-01235E5D021E") 'Guid.NewGuid()
        'Dim BasicCatalog As New BaseProducts(SessionId, dbConnection, ImportID)
        Dim DetailedCatalog As New DetailedProducts(SessionId, dbConnection, ImportID)

        MessageBox.Show("DONE")
    End Sub

    Private Sub b_Category_Click(sender As Object, e As EventArgs) Handles b_Category.Click
        Dim UserId As String = tb_UserID.Text
        Dim API_ID As String = tb_API_ID.Text
        Dim SessionId As String = Nothing

        Dim MagentoLogin As New Magento_Login(UserId, API_ID, SessionId)

        tb_SessionID.Text = SessionId

        Dim dbConnection As New SqlConnection With {
            .ConnectionString = "Data Source=cocapiinternal\test,2433;Initial Catalog=API_COMDA_COM;Persist Security Info=True;User ID=sa;Password=apitest2015" '"Data Source=JB-FAST\DEVEL_2016;Initial Catalog=API_Internal_SVS_CLR;Persist Security Info=True;User ID=sa;Password=sophie;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True"
        }

        Dim ImportID As Guid = Guid.Parse("B31A1B13-656D-4866-8E09-9262E0D6024D")
        Dim Categories As New Category(SessionId, dbConnection, ImportID)
    End Sub

    Private Sub b_PricingGrid_Click(sender As Object, e As EventArgs) Handles b_PricingGrid.Click

        'Dim UserId As String = tb_UserID.Text
        'Dim API_ID As String = tb_API_ID.Text
        'Dim SessionId As String = Nothing

        'Dim MagentoLogin As New Magento_Login(UserId, API_ID, SessionId)

        'tb_SessionID.Text = SessionId

        'Dim dbConnection As New SqlConnection With {
        '    .ConnectionString = "Data Source=cocapiinternal\test,2433;Initial Catalog=API_COMDA_COM;Persist Security Info=True;User ID=sa;Password=apitest2015" '"Data Source=JB-FAST\DEVEL_2016;Initial Catalog=API_Internal_SVS_CLR;Persist Security Info=True;User ID=sa;Password=sophie;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True"
        '}

        'Dim ImportID As Guid = Guid.Parse("DF14A094-CEE6-40B1-B651-963F549C69E0") '- -Guid.NewGuid()
        ''Dim BasicCatalog As New BaseProducts(SessionId, dbConnection, ImportID)
        ''Dim DetailedCatalog As New DetailedProducts(SessionId, dbConnection, ImportID)

        'Dim Pricing As New PricingData
        'Pricing.GetPrices(SessionId:=SessionId, dr:=Nothing, Status:=Nothing, dbConnection:=dbConnection, ImportID:=ImportID)



        'MessageBox.Show("DONE")


    End Sub
End Class
