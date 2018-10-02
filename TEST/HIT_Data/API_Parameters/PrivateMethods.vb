Module PrivateMethods


    Public Function GetListOfProducts(ByVal productId As String, ByVal id As String, ByVal password As String) As List(Of String)

        Dim Active As New List(Of String)
        'get a list of products first if productId = nothing
        If IsNothing(productId) Then
            'Dim plist As New HIT_GetProductList.ListProducts
            Active = ActiveProducts(id, password)
            Return Active
        Else
            Select Case productId.IndexOf("#")
                Case -1
                    For i As Integer = 0 To productId.Split(",").Length - 1
                        Dim product As String = productId.Split(",")(i).Trim
                        If product.Length > 0 Then
                            Active.Add(product)
                        End If
                    Next
                Case 0
                    Active = ActiveProducts(id, password)
                    Dim RemoveAt As Integer
                    Dim ii As Integer
                    If productId.IndexOf("#") = 0 Then
                        For ii = 0 To Active.Count - 1
                            If Active(ii) = productId.Substring(1, productId.Length - 1) Then
                                RemoveAt = ii
                                Exit For
                            End If
                        Next
                        For ii = Active.Count - 1 To 0 Step -1
                            If ii < RemoveAt Then
                                Active.RemoveAt(ii)
                            End If
                        Next
                    End If
                    Return Active
            End Select

        End If

        Return Active

    End Function

    Private Function ActiveProducts(ByVal HIT_UserID As String, ByVal HIT_Secret As String) As List(Of String)
        Dim ActProd As New List(Of String)

        Dim result = ObtainListOfProducts(HIT_UserID, HIT_Secret)

        For i As Integer = 0 To result.Length - 1
            ActProd.Add(result(i).number)
        Next

        Return ActProd

    End Function

    Private Function ObtainListOfProducts(ByVal HIT_UserID As String, ByVal HIT_Secret As String) As Object
        Dim pcs As New net.hitpromo.ds.ProductControllerService
        'pcs.getActiveProducts(HIT_UserID, HIT_Secret, True)
        Return pcs.getActiveProducts(HIT_UserID, HIT_Secret, False)
    End Function




End Module
