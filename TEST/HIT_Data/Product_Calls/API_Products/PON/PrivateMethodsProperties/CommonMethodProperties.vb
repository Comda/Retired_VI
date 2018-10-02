Imports System.Data.SqlClient

Module CommonMethodProperties


    Public Property Connection As SqlConnection = Nothing
    Public Property VendorImportID As Integer = 0
    Public Property NumberOfProducts As Integer = 0
    Public Property CreatedBy As String = Nothing
    Public Property StopwatchLocal As Stopwatch
    Public Property TransactionID As String = Nothing
    Public Property ControlRoot_Current As String = Nothing
    Public Property SessionId As String = Nothing
    Public Property DBConnectionString As String = Nothing

    Public Property wsVersion As String = Nothing
    Public Property id As String = Nothing
    Public Property password As String = Nothing
    Public Property productId As String = Nothing
    Public Property currency As String = Nothing
    Public Property fobId As Integer = 0
    Public Property priceType As String = Nothing
    Public Property localizationCountry As String = Nothing
    Public Property localizationLanguage As String = Nothing
    Public Property configurationType As String = Nothing
    Public Property ControlRoot As String = Nothing
    Public Property Comment As String = Nothing = Nothing

    Public Api_Para As API_Parameters.Initialize

End Module
