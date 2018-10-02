Public Class ExtractFromXML
    Friend Property ImprintGroup As Object

    Public Sub GetPairsExtractFromReply(ByVal Reply As Object)
        'Dim InfoList As New List(Of InfoList)

        Try
            Dim PartProperties As New List(Of PartProperty)

            For iDetails As Integer = 7 To 7 ' InfoData.Length - 1  '7 is Properties
                Dim currXML As New XElement(XElement.Parse(Reply(iDetails).outerxml))
                'Debug.WriteLine(currXML.ToString)
                Dim Prop = currXML.Descendants()

                ExtractProperty(Prop(1))

                For ii As Integer = 0 To ImprintGroup.Count - 1
                    AddToPartProperty(PartProperties, productId, ImprintGroup(ii).ItemName, ImprintGroup(ii).ItemValue)
                    'Debug.WriteLine("Name {0}  Value {1}", ImprintGroup(ii).ItemName, ImprintGroup(ii).ItemValue)
                Next

            Next

            AddRowsToPartProperty(PartProperties)

        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "Process Result: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try


    End Sub
    Private Sub AddRowsToPartProperty(ByRef InfoList As List(Of PartProperty))

        Try
            For Each item As PartProperty In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PODO_PropertyPairs.NewRow()

                newInsertParameterRow("productId") = item.productId

                newInsertParameterRow("PropName") = item.NameOfProperty
                newInsertParameterRow("PropValue") = item.ValueOfProperty

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                'newInsertParameterRow("VendorImportID") = VendorImportID
                'newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PODO_PropertyPairs.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.locationId)
            Next

            PropertyPairs_dt.Update(HIT_ds.API_PODO_PropertyPairs)


        Catch ex As Exception
            'Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToPartProperty: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot)
        End Try

    End Sub
    Private Sub AddToPartProperty(ByRef PartProperty As List(Of PartProperty), ByVal productId As String, ByVal NameOfProperty As String, ByVal ValueOfProperty As String)

        PartProperty.Add(New PartProperty() With {
            .productId = productId,
            .NameOfProperty = NameOfProperty,
            .ValueOfProperty = ValueOfProperty
        })

    End Sub
    Private Class PartProperty
        Property productId As String
        Property NameOfProperty As String
        Property ValueOfProperty As String
    End Class

    Private Sub ExtractProperty(currXML As XElement)

        ImprintGroup = (From ImprintGroup In currXML.Elements("item") Select New With {
       .ItemName = ImprintGroup.Element("key").Value,
       .ItemValue = ImprintGroup.Element("value").Value
       }).ToList()

    End Sub

End Class
