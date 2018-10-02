Public Class GetXML
    Public Property FunctionRequested As String
    Public Sub GetReplyData(ByVal ProductList As List(Of String), ByVal FunctionRequested As String)

        'Create a NEW request 

        PODO = New HIT_productData.ProductObjectControllerService
        PODO.Timeout = 1000000
        'Reply = New HIT_PricingConfiguration.GetConfigurationAndPricingResponse
        'Request = New HIT_PricingConfiguration.GetConfigurationAndPricingRequest


        Api_Para.SourceAPI = "http://ds.hitpromo.net/productObject"

        SessionId = Api_Para.GetRequestID(TransactionID)

        'Request.wsVersion = wsVersion
        'Request.id = id
        'Request.password = password
        'Request.localizationCountry = localizationCountry
        'Request.localizationLanguage = localizationLanguage

        'variable data
        'Request.productId = 0

        'not used for this API Call
        'Request.configurationType = configurationType
        'Request.currency = currency
        'Request.fobId = fobId
        'Request.priceType = priceType


        Dim Extract As New ExtractFromXML
        Dim i As Integer

        For i = 0 To ProductList.Count - 1
            productId = ProductList(i)
            'Request.productId = productId

            Select Case FunctionRequested
                Case "GetImages"
                    Try
                        Reply = PODO.getImages(id, password, productId)
                    Catch ex As Exception
                        Debug.WriteLine(ex.Message)
                        Api_Para.WriteEventToLog("Error", FunctionRequested & "->" & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
                    End Try

                    If Not IsNothing(Reply) Then
                        Extract.ProcessImageResult(Reply, productId)
                    End If
                    ' Debug.WriteLine("Processed {0} out of {1} Total Products Time {2}", i + 1, ProductList.Count, Now())
            End Select
        Next


    End Sub


End Class
