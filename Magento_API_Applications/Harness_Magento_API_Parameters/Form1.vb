'RELEASED and PUBLISHED Aug 18, 2017, Jan 11 2018
'ONLY for:   'https://www.mapleleafpromostore.com/index.php/api/v2_soap/index/
Public Class Form1
    Private Sub bGetSessionID_Click(sender As Object, e As EventArgs) Handles bGetSessionID.Click

        Dim UserID As String = Me.tb_UserID.Text ' "jeromeb"
        Dim API_ID As String = Me.tb_API_ID.Text ' "sophie"
        Dim ControlRoot As String = Me.tb_ControlRoot.Text ' "GetProductCatalog"
        Dim TransactionID As String = Guid.NewGuid().ToString
        Me.tb_TransactionGUID.Text = TransactionID
        'Dim DatabaseConnectionString As String = "Data Source=COCAPIINTERNAL\prod,1433;Initial Catalog=API_Store;Persist Security Info=True;User ID=sa;Password=apiprod2015" '"Data Source=JB-DESKTOP\SQL2016DEV;Initial Catalog=ERP_Data;Persist Security Info=True;User ID=sa;Password=sophie" ' "Data Source=COCAPIINTERNAL\TEST,2433;Initial Catalog=API_Store;Persist Security Info=True;User ID=sa;Password=apitest2015" '"Data Source=JB-DESKTOP\SQL2016DEV;Initial Catalog=ERP_Data;Persist Security Info=True;User ID=sa;Password=sophie"
        'Dim Details As Boolean = True

        Dim dbContext As String = "WEB_C60"

        Dim init As New Magento_API_Parameters.Initialize
        init.GetMagentoAPI_Credentials(UserID, API_ID, ControlRoot, TransactionID, dbContext)
        Me.tb_SessionID.Text = init.CurrentSessionID
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()

    End Sub

    Private Sub CLR_Click()
        'Public Shared Sub Magento_Synchronize_ERP(ByVal UserID As String, ByVal API_ID As String, ByVal ConnectionString As String, ByVal Action As String, ByVal dbContext As String, ByVal TransactionID As String)

        Dim UserID As String = Me.tb_UserID.Text ' "jeromeb"
        Dim API_ID As String = Me.tb_API_ID.Text ' "sophie"
        Dim Action As String = "UPDATE CATALOG"
        Dim ControlRoot As String = Action.ToUpper   'Me.tb_ControlRoot.Text ' "GetProductCatalog"
        Dim TransactionID As String = Guid.NewGuid().ToString
        Dim ConnectionString As String = "Data Source=COCAPIINTERNAL\prod,1433;Initial Catalog=API_Store;Persist Security Info=True;User ID=sa;Password=apiprod2015"
        Dim dbContext As String = "WEB_DEMAC"
        Dim dbcon As New SqlClient.SqlConnection
        dbcon.ConnectionString = ConnectionString


        Select Case Action.ToUpper
            Case "UPDATE CATALOG"
                Dim MagentoType As String = Nothing

                Dim init As New Magento_API_Parameters.Initialize
                init.GetMagentoAPI_Credentials(UserID, API_ID, ControlRoot, TransactionID, dbContext)
                If init.CurrentSessionID.Length > 0 Then

                    init.InitializeSQLVariables(dbcon, "PARENT")
                    init.GetCurrentCatalog(init.CurrentSessionID, "PARENT", MagentoType)

                    init.InitializeSQLVariables(dbcon, "FAMILY")
                    init.GetCurrentCatalog(init.CurrentSessionID, "FAMILY", MagentoType)

                    init.InitializeSQLVariables(dbcon, "CHILD")
                    init.GetCurrentCatalog(init.CurrentSessionID, "CHILD", MagentoType)
                End If

        End Select
    End Sub



    Private Sub b_GetCatalog_Click(sender As Object, e As EventArgs) Handles b_GetCatalog.Click

        Dim UserID As String = Me.tb_UserID.Text ' "jeromeb"
        Dim API_ID As String = Me.tb_API_ID.Text ' "sophie"
        Dim ControlRoot As String = Me.tb_ControlRoot.Text ' "GetProductCatalog"
        Dim TransactionID As String = Me.tb_TransactionGUID.Text
        Dim ERP_Type As String = "PARENT"
        Dim MagentoType As String = Nothing
        Dim dbContext As String = "WEB_C60"

        TransactionID = Guid.NewGuid.ToString
        'Dim catalogProduct As List(Of catalogProductEntity) = Nothing

        Dim ERP_Types As New List(Of String)
        ERP_Types.Add("PARENT")
        ERP_Types.Add("FAMILY")
        ERP_Types.Add("CHILD")

        Dim init As New Magento_API_Parameters.Initialize
        init.GetMagentoAPI_Credentials(UserID, API_ID, ControlRoot, TransactionID, dbContext)
        Me.tb_SessionID.Text = init.CurrentSessionID
        'get the data set connected
        Dim dbcon As New SqlClient.SqlConnection
        dbcon.ConnectionString = "Data Source=COCAPIINTERNAL\prod,1433;Initial Catalog=API_Store;Persist Security Info=True;User ID=sa;Password=apiprod2015"

        For I As Integer = 0 To ERP_Types.Count - 1
            init.InitializeSQLVariables(dbcon, ERP_Types(I))
            init.GetCurrentCatalog(init.CurrentSessionID, ERP_Types(I), MagentoType)

        Next



        'Dim tp As New Magento_API_TierPrice.ChangeTierPrices(dbcon, ERP_Type, MagentoType, init.catalogProduct, init.CurrentSessionID)
        MessageBox.Show("done")
    End Sub

    Private Sub b_synchNames_Click(sender As Object, e As EventArgs) Handles b_synchNames.Click

        Dim UserID As String = Me.tb_UserID.Text ' "jeromeb"
        Dim API_ID As String = Me.tb_API_ID.Text ' "sophie"
        Dim ControlRoot As String = Me.tb_ControlRoot.Text ' "GetProductCatalog"
        Dim TransactionID As String = Me.tb_TransactionGUID.Text
        Dim dbContext As String = "WEB_C60"

        '34CB7A04-644F-48D1-B6C0-8E6C3622D5DF
        '67A497FE-C27F-4CD3-8302-3DAF03BD802D

        TransactionID = "5E992A2A-079A-471B-A58A-6FAE2427034B" ' Guid.NewGuid.ToString

        Dim dbcon As New SqlClient.SqlConnection
        dbcon.ConnectionString = "Data Source=COCAPIINTERNAL\prod,1433;Initial Catalog=API_Store;Persist Security Info=True;User ID=sa;Password=apiprod2015"


        Dim init As New Magento_API_Parameters.Initialize
        init.GetMagentoAPI_Credentials(UserID, API_ID, ControlRoot, TransactionID, dbContext)
        Me.tb_SessionID.Text = init.CurrentSessionID

        Dim pu As New Magento_API_UpdateNameDescription.UpdateDescription(dbcon, init.CurrentSessionID, TransactionID, ControlRoot)

        MessageBox.Show("done")
    End Sub

    Private Sub b_SynchOptions_Click(sender As Object, e As EventArgs) Handles b_SynchOptions.Click
        Dim UserID As String = Me.tb_UserID.Text ' "jeromeb"
        Dim API_ID As String = Me.tb_API_ID.Text ' "sophie"
        Dim ControlRoot As String = Me.tb_ControlRoot.Text ' "GetProductCatalog"
        Dim TransactionID As String = Guid.NewGuid().ToString

        Dim dbContext As String = "WEB_C60"

        Dim dbcon As New SqlClient.SqlConnection
        dbcon.ConnectionString = "Data Source=COCAPIINTERNAL\prod,1433;Initial Catalog=API_Store;Persist Security Info=True;User ID=sa;Password=apiprod2015"


        Dim init As New Magento_API_Parameters.Initialize
        init.GetMagentoAPI_Credentials(UserID, API_ID, ControlRoot, TransactionID, dbContext)
        Me.tb_SessionID.Text = init.CurrentSessionID


        Dim po As New Magento_API_CustomOptions.CustomOptions(dbcon, init.CurrentSessionID, TransactionID, ControlRoot)

        MessageBox.Show("done")



    End Sub

    Private Sub b_SetUp_Click(sender As Object, e As EventArgs) Handles b_SetUp.Click
        Dim UserID As String = Me.tb_UserID.Text ' "jeromeb"
        Dim API_ID As String = Me.tb_API_ID.Text ' "sophie"
        Dim ControlRoot As String = Me.tb_ControlRoot.Text ' "GetProductCatalog"
        Dim TransactionID As String = Me.tb_TransactionGUID.Text
        Dim dbContext As String = "WEB_C60"

        '34CB7A04-644F-48D1-B6C0-8E6C3622D5DF -- 92717d36-d337-457f-8a20-6a8f541e3c92
        '67A497FE-C27F-4CD3-8302-3DAF03BD802D  92717d36-d337-457f-8a20-6a8f541e3c92  "8132634a-78c4-4939-a89f-049ded583ccc" TEST that works '
        '403C653E-BBE2-4D29-A2C7-7220A59D50D0

        TransactionID = "403C653E-BBE2-4D29-A2C7-7220A59D50D0"  'current universes' "403C653E-BBE2-4D29-A2C7-7220A59D50D0"   "374C9C4D-DFFC-424D-8497-967C6B27698A"

        Dim dbcon As New SqlClient.SqlConnection
        dbcon.ConnectionString = "Data Source=COCAPIINTERNAL\prod,1433;Initial Catalog=API_Store;Persist Security Info=True;User ID=sa;Password=apiprod2015"


        Dim init As New Magento_API_Parameters.Initialize
        init.GetMagentoAPI_Credentials(UserID, API_ID, ControlRoot, TransactionID, dbContext)
        Me.tb_SessionID.Text = init.CurrentSessionID

        Dim pu As New Magento_API_SetupFees.SetUpFees(dbcon, init.CurrentSessionID, TransactionID, ControlRoot)

        MessageBox.Show("done")
    End Sub

    Private Sub B_LikeCLR_Click(sender As Object, e As EventArgs) Handles B_LikeCLR.Click
        CLR_Click()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim UserID As String = Me.tb_UserID.Text ' "jeromeb"
        Dim API_ID As String = Me.tb_API_ID.Text ' "sophie"
        Dim Action As String = "UPDATE CATALOG"
        Dim ControlRoot As String = Action.ToUpper   'Me.tb_ControlRoot.Text ' "GetProductCatalog"
        Dim TransactionID As String = Guid.NewGuid().ToString
        Dim ConnectionString As String = "Data Source=COCAPIINTERNAL\prod,1433;Initial Catalog=API_Store;Persist Security Info=True;User ID=sa;Password=apiprod2015"
        Dim dbContext As String = "WEB_DEMAC"
        Dim dbcon As New SqlClient.SqlConnection
        dbcon.ConnectionString = ConnectionString

        Dim MagentoType As String = Nothing

        Dim init As New Magento_API_Parameters.Initialize
        init.GetMagentoAPI_Credentials(UserID, API_ID, ControlRoot, TransactionID, dbContext)
        If init.CurrentSessionID.Length > 0 Then
            'Dim tp As New Magento_API_TierPrice.ChangeTierPrices(dbcon, init.CurrentSessionID)

            ' Dim tp As New Magento_API_TierPrice.ChangeTierPrices(dbcon, init.CurrentSessionID, True)

            'Compare
            Dim tp As New Magento_API_TierPrice.ChangeTierPrices(dbcon, init.CurrentSessionID, 1)
        End If
        MessageBox.Show("done TIER PRICING")
    End Sub
End Class
