Public Class Main
    Public Shared Sub HIT_Products(ByVal DBConnection As String,
                                    ByVal id As String,
                                    ByVal password As String,
                                    ByVal wsVersion As String,
                                    ByVal productId As String,
                                    ByVal currency As String,
                                    ByVal fobId As String,
                                    ByVal priceType As String,
                                    ByVal localizationCountry As String,
                                    ByVal localizationLanguage As String,
                                    ByVal configurationType As String,
                                    ByVal ExtractTask As String,
                                    ByVal VendorImportID As Integer,
                                    ByVal CreatedBy As String,
                                    ByVal Comment As String,
                                    ByRef RequestID As Integer)


        'NEW
        'If ExtractTask.ToUpper = "CATEGORY" Then
        '    Dim pon As New PON.Initialize_HIT_Call
        '    pon.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        'End If

        'If ExtractTask.ToUpper = "PACKAGING" Then
        '    Dim pon As New PON.Initialize_HIT_Call
        '    pon.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        'End If

        'If ExtractTask.ToUpper = "PRODUCTINFO" Then
        '    Dim pon As New PON.Initialize_HIT_Call
        '    pon.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        'End If

        'If ExtractTask.ToUpper = "CONFIGURATION" Then
        '    Dim PACS As New PACS.Initialize_HIT_Call
        '    PACS.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        'End If

        'If ExtractTask.ToUpper = "PROPERTY" Then

        '    Dim PODO As New PODO.Initialize_HIT_Call
        '    PODO.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        'End If

        'If ExtractTask.ToUpper = "IMAGES" Then

        '    Dim PODO_DETAIL As New PODO_DETAIL.Initialize_HIT_Call
        '    PODO_DETAIL.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        'End If


    End Sub

    Private Sub b_PACS_Click(sender As Object, e As EventArgs) Handles b_PACS.Click


        Dim VendorImportID As Integer = -699
            Dim CreatedBy As String = "JB_Test_VB_NeCurrencyENUM"
            Dim DBConnection As String = "Data Source=COCAPIINTERNAL\VNDR_INTEGRATION,4433;Initial Catalog=ERP_Import;Persist Security Info=True;User ID=sa;Password=it$upport" '"Data Source=JB-DESKTOP\SQL2016DEV;Initial Catalog=ERP_Data;Integrated Security=True"
            Dim wsVersion As String = "1.0.0"
            Dim id As String = "726555"
            Dim password As String = "1acd29db4c989a24143dda9e8adcaff0"
        Dim productId As String = "7050" '"1060" ',1003,7156,2602,11001" '"5000" 'Nothing '"30" ' "7050" ' "5000" ' , 1025" '"1025" '"2000" '"1001" '"5000" 'Nothing
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
