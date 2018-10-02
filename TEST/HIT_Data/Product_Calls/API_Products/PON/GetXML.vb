Public Class GetXML


    Public Sub GetReplyData(ByVal ProductList As List(Of String), ByVal FunctionRequested As String)

        'Create a NEW request 

        PON = New HIT_productData.ProductDataService
        PON.Timeout = 1000000
        Reply = New HIT_productData.GetProductResponse
        Request = New HIT_productData.GetProductRequest

        Api_Para.SourceAPI = "http://ppds.hitpromo.net/productData"
        'Api_Para.AddRowsToRequest(FunctionRequested)
        SessionId = Api_Para.GetRequestID(TransactionID)

        Request.wsVersion = wsVersion
        Request.id = id
        Request.password = password
        Request.localizationCountry = localizationCountry
        Request.localizationLanguage = localizationLanguage

        'variable data
        'Request.productId = 0

        'not used for this API Call
        'Request.configurationType = configurationTypeCode
        'Request.currency = CurrencyCode
        'Request.fobId = fobIdCode
        'Request.priceType = priceTypeCode


        Dim Extract As New ExtractFromXML
        Dim i As Integer

        For i = 0 To ProductList.Count - 1
            productId = ProductList(i)
            Request.productId = productId

            Select Case FunctionRequested
                Case "GetProductPackaging"
                    Try
                        Reply = PON.getProduct(Request)
                    Catch ex As Exception
                        Debug.WriteLine(ex.Message)
                        Api_Para.WriteEventToLog("Error", FunctionRequested & "->" & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
                    End Try

                    If Not IsNothing(Reply) Then
                        Extract.GetPackagingExtractFromReply(Reply)
                    End If
                   ' Debug.WriteLine("Processed {0} out of {1} Total Products Time {2}", i + 1, ProductList.Count, Now())

                Case "GetProductInfo"
                    Try
                        Reply = PON.getProduct(Request)
                    Catch ex As Exception
                        Debug.WriteLine(ex.Message)
                        Api_Para.WriteEventToLog("Error", FunctionRequested & "->" & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
                    End Try

                    If Not IsNothing(Reply) Then
                        Extract.GetProductInfoExtractFromReply(Reply)
                    End If
                    'Debug.WriteLine("Processed {0} out of {1} Total Products Time {2}", i + 1, ProductList.Count, Now())

                Case "GetProductCategory"
                    Try
                        Reply = PON.getProduct(Request)
                    Catch ex As Exception
                        Debug.WriteLine(ex.Message)
                        Api_Para.WriteEventToLog("Error", FunctionRequested & "->" & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
                    End Try

                    If Not IsNothing(Reply) Then
                        Extract.GetCategoryExtractFromReply(Reply)
                    End If

            End Select
        Next


    End Sub


End Class
