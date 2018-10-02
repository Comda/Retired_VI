Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server


Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub HIT_Catalog_GET(ByVal DBConnection As String,
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
        If ExtractTask.ToUpper = "CATEGORY" Then
            Dim pon As New PON.Initialize_HIT_Call
            pon.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        End If

        If ExtractTask.ToUpper = "PACKAGING" Then
            Dim pon As New PON.Initialize_HIT_Call
            pon.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        End If

        If ExtractTask.ToUpper = "PRODUCTINFO" Then
            Dim pon As New PON.Initialize_HIT_Call
            pon.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        End If

        If ExtractTask.ToUpper = "CONFIGURATION" Then
            Dim PACS As New PACS.Initialize_HIT_Call
            PACS.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        End If

        If ExtractTask.ToUpper = "PROPERTY" Then

            Dim PODO As New PODO.Initialize_HIT_Call
            PODO.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        End If

        If ExtractTask.ToUpper = "IMAGES" Then

            Dim PODO_DETAIL As New PODO_DETAIL.Initialize_HIT_Call
            PODO_DETAIL.SelectProductList(ExtractTask, productId, VendorImportID, CreatedBy, DBConnection, id, password, currency, fobId, priceType, localizationCountry, localizationLanguage, configurationType, RequestID, Comment)
        End If


    End Sub

End Class
