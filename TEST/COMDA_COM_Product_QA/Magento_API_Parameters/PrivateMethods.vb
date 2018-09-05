Imports Magento_API_Parameters.Mage_API
Module PrivateMethods
    Public Function ReturnStringFromIds(ByVal Entity As Object) As String

        Dim EntityString As String = Nothing

        For i As Integer = 0 To Entity.Length - 1
            EntityString = EntityString & Entity(i) & ","
        Next
        If Not IsNothing(EntityString) Then
            Return EntityString.Substring(0, EntityString.Length - 1)
        Else
            Return ""
        End If
    End Function
End Module
