Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim VendorImportID As Integer = -699
        Dim CreatedBy As String = "JB_Test_VB_NeCurrencyENUM"
        Dim DBConnection As String = "Data Source=COCAPIINTERNAL\VNDR_INTEGRATION,4433;Initial Catalog=ERP_Import;Persist Security Info=True;User ID=sa;Password=it$upport" '"Data Source=JB-DESKTOP\SQL2016DEV;Initial Catalog=ERP_Data;Integrated Security=True"
        Dim wsVersion As String = "1.0.0"
        Dim id As String = "726555"
        Dim password As String = "1acd29db4c989a24143dda9e8adcaff0"
        Dim productId As String = "1060" ',1003,7156,2602,11001" '"5000" 'Nothing '"30" ' "7050" ' "5000" ' , 1025" '"1025" '"2000" '"1001" '"5000" 'Nothing
        Dim currency As String = "usd"
        Dim fobId As String = "usa"
        Dim priceType As String = "customer"
        Dim localizationCountry As String = "CA"
        Dim localizationLanguage As String = "en"
        Dim configurationType As String = "decorated"

        Dim RequestID As Integer = 0
        Dim Comment As String = "Request Only. No db rows added"

        Dim ExtractTask As String = "CONFIGURATION" '"PRODUCTINFO" '"CONFIGURATION"

        If ExtractTask.ToUpper = "CONFIGURATION" Then
            Dim pon As New PACS.Initialize_HIT_Call
            pon.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        End If



        MessageBox.Show("done:  " & RequestID.ToString)

        Close()

    End Sub
End Class
