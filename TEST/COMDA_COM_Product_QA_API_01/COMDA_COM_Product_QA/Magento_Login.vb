Imports COMDA_COM_Product_QA_API_1.Magento_API_01

Public Class Magento_Login

    Public Sub New(ByVal UserID As String, ByVal API_ID As String, ByRef SessionId As String)
        Try
            MageHandler = New MagentoService
            SessionId = MageHandler.login(UserID, API_ID)
        Catch ex As Exception
            Throw New Exception("Magento_Login : " & ex.Message)
        End Try
    End Sub


End Class
