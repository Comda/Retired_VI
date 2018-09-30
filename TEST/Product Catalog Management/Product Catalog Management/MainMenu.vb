
Imports System.Data.SqlClient

Public Class MainMenu
    Public Property FamilyFromCriteria As DataTable
    Public Property ProductIDFromCriteria As Integer
    Public Property dbConnection As SqlConnection
    Public Property dm As VerifyProductExistence.DataManagement
    Public Property NewMasterNo As DataTable

    Private Sub lb_Criteria_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lb_Criteria.MouseDoubleClick
        Dim Criteria As String = lb_Criteria.SelectedItem
        dm = New VerifyProductExistence.DataManagement

        dbConnection = New SqlConnection
        dbConnection.ConnectionString = "Data Source=cocapiinternal\test;Initial Catalog=API_COMDA_COM;Persist Security Info=True;User ID=sa;Password=apitest2015"

        FamilyFromCriteria = dm.GetProductUniverse(dbConnection, Criteria)

        DGV_FamilyByCriteria.DataSource = FamilyFromCriteria
    End Sub

    Private Sub DGV_FamilyByCriteria_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_FamilyByCriteria.CellContentClick
        ProductIDFromCriteria = FamilyFromCriteria.Rows(e.RowIndex).Item("productid")
        tb_ProductIDFromCriteria.Text = ProductIDFromCriteria
        lb_FamilyFromCriteria.Text = FamilyFromCriteria.Rows(e.RowIndex).Item("family")
    End Sub

    Private Sub b_GetMasterNO_Click(sender As Object, e As EventArgs) Handles b_GetMasterNO.Click
        Dim ProductId As Integer = CInt(tb_ProductIDFromCriteria.Text)
        Dim CountMasterNO As Integer = 0

        NewMasterNo = dm.GetReconstructedMasterNo(dbConnection, ProductId, CountMasterNO)

        DGV_NewMasterNo.DataSource = NewMasterNo
        tb_CountMasterNO.Text = CountMasterNO
    End Sub
End Class
