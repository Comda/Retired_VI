
Module PrivateMethods

    Private Function GetProductId(ByVal SKU As String) As Integer
        Dim prodid As Integer = 0
        Try
            If Not IsNothing(catalogProduct.Find(Function(x) x.sku.Equals(SKU.Trim))) Then
                prodid = catalogProduct.Find(Function(x) x.sku.Equals(SKU.Trim)).product_id
            End If
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "GetProductId", ex.Message, StopwatchLocal, Guid.Parse(TransactionID_Current), ControlRoot_Current)
        End Try

        Return prodid
    End Function
End Module
