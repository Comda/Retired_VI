Public Class GetXML
    Public Property FunctionRequested As String
    Public Sub GetReplyData(ByVal ProductList As List(Of String), ByVal FunctionRequested As String)

        'Create a NEW request 

        PACS = New HIT_PricingConfiguration.PricingAndConfigurationService
        PACS.Timeout = 1000000
        Reply = New HIT_PricingConfiguration.GetConfigurationAndPricingResponse
        Request = New HIT_PricingConfiguration.GetConfigurationAndPricingRequest


        Api_Para.SourceAPI = "https://ppds.hitpromo.net/pricingAndConfiguration/"

        SessionId = Api_Para.GetRequestID(TransactionID)

        Request.wsVersion = wsVersion
        Request.id = id
        Request.password = password
        Request.localizationCountry = localizationCountry
        Request.localizationLanguage = localizationLanguage

        'variable data
        'Request.productId = 0

        'not used for this API Call
        Request.configurationType = configurationType

        Request.currency = currency
        Request.fobId = fobId
        Request.priceType = priceType

        'CurrencyCodeType.CAD

        Dim Extract As New ExtractFromXML
        Dim i As Integer

        For i = 0 To ProductList.Count - 1
            productId = ProductList(i)
            Request.productId = productId

            Select Case FunctionRequested
                Case "GetConfigurationAndPricing"
                    Try
                        Reply = PACS.getConfigurationAndPricing(Request)
                        'If Not IsNothing(Reply) Then
                        '    Debug.WriteLine("Currency  {0}  Error {1}", currency, Reply.ErrorMessage.description)
                        'End If
                    Catch ex As Exception
                        Debug.WriteLine("Error {0}", Reply.ErrorMessage.description)
                        Api_Para.WriteEventToLog("Error", FunctionRequested & "->" & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
                    End Try
                    If Not IsNothing(Reply) Then
                        Extract.GetConfigurationAndPricingExtractFromReply(Reply)
                    End If
                    Debug.WriteLine("Processed {0} out of {1} Total Products Time {2}", i + 1, ProductList.Count, Now())
            End Select
        Next


    End Sub


End Class
