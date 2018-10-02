Imports System.Data.SqlClient

Public Class Initialize_HIT_Call

    Public Sub SelectProductList(ByVal ExtractTask As String, ByVal SKU_Current As String, ByVal VendorImportID_Current As Integer, ByVal CreatedBy_Current As String, ByVal DBConnection_Current As String,
                                 ByVal HIT_UserID As String, ByVal HIT_Secret As String, ByVal currency_Current As String, ByVal fobId_Current As String, ByVal priceType_Current As String,
                                 ByVal localizationCountry_Current As String, ByVal localizationLanguage_Current As String, ByVal configurationType_Current As String, ByRef RequestID As Integer, ByVal Comment_current As String)




        Api_Para = New API_Parameters.Initialize(ExtractTask, SKU_Current, VendorImportID_Current, CreatedBy_Current, DBConnection_Current,
                                  HIT_UserID, HIT_Secret, currency_Current, fobId_Current, priceType_Current,
                                  localizationCountry_Current, localizationLanguage_Current, configurationType_Current, RequestID, Comment_current)

        StopwatchLocal = New Stopwatch()
        StopwatchLocal.Start()

        TransactionID = Api_Para.TransactionID
        ControlRoot = Api_Para.ControlRoot

        Connection = New SqlConnection
        'Connection = Api_Para.Connection
        Connection.ConnectionString = Api_Para.DBConnectionString

        HIT_ds = New HIT_Product

        PropertyPairs_dt = New HIT_ProductTableAdapters.API_PODO_PropertyPairsTableAdapter


        DBConnectionString = Api_Para.DBConnectionString

        PropertyPairs_dt.Connection.ConnectionString = DBConnectionString


        'update all the properties from api.parameters

        VendorImportID = Api_Para.VendorImportID
        CreatedBy = Api_Para.CreatedBy
        wsVersion = Api_Para.wsVersion
        id = Api_Para.id
        password = Api_Para.password
        currency = Api_Para.currency
        fobId = Api_Para.fobId
        priceType = Api_Para.priceType
        localizationCountry = Api_Para.localizationCountry
        localizationLanguage = Api_Para.localizationLanguage
        configurationType = Api_Para.configurationType
        ControlRoot = Api_Para.FunctionRequested
        Comment = Api_Para.Comment

        Dim Active As New List(Of String)
        Active = Api_Para.Active

        Dim GetData As New GetXML
        GetData.GetReplyData(Active, Api_Para.FunctionRequested)

        'when done Update requestID
        Api_Para.UpdateRequestData(TransactionID)

        Api_Para.WriteEventToLog("Info", "Completed-HIT " & ExtractTask, SessionId, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)


    End Sub



End Class
