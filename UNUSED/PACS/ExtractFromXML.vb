Public Class ExtractFromXML

    Public Sub GetConfigurationAndPricingExtractFromReply(Reply As HIT_PricingConfiguration.GetConfigurationAndPricingResponse, ByVal api_para As Object)
        Try

            Dim PartArray As New List(Of PartArray)
            Dim PartPrice As New List(Of PartPrice)
            Dim PartLocationArray As New List(Of PartLocationArray)
            Dim LocationArray As New List(Of LocationArray)
            Dim DecorationArray As New List(Of DecorationArray)
            Dim ChargeArray As New List(Of ChargeArray)
            Dim ChargePriceArray As New List(Of ChargePriceArray)

            Dim i As Integer
            Dim ii As Integer
            Dim i2 As Integer
            Dim i3 As Integer
            Dim iii As Integer
            Dim i4 As Integer
            Dim iV As Integer

            If Not IsNothing(Reply.Configuration) Then
                For i = 0 To Reply.Configuration.PartArray.Length - 1
                    AddToPartArray(PartArray,
                                    api_para.CheckObject(Reply.Configuration.PartArray(i).partId),
                                    api_para.CheckObject(Reply.Configuration.PartArray(i).partDescription),
                                    api_para.CheckObject(Reply.Configuration.PartArray(i).partGroup),
                                    api_para.CheckObject(Reply.Configuration.PartArray(i).nextPartGroup),
                                    api_para.CheckObject(Reply.Configuration.PartArray(i).partGroupRequired),
                                    api_para.CheckObject(Reply.Configuration.PartArray(i).partGroupDescription),
                                    api_para.CheckObject(Reply.Configuration.PartArray(i).ratio),
                                    api_para.CheckObject(Reply.Configuration.PartArray(i).defaultPart))

                    If Not IsNothing(Reply.Configuration.PartArray(i).PartPriceArray) Then

                        For i4 = 0 To Reply.Configuration.PartArray(i).PartPriceArray.Length - 1
                            AddToPartPrice(PartPrice, Reply.Configuration.PartArray(i).partId,
                                        api_para.CheckObject(Reply.Configuration.PartArray(i).PartPriceArray(i4).minQuantity),
                                        api_para.CheckObject(Reply.Configuration.PartArray(i).PartPriceArray(i4).priceUom),
                                        api_para.CheckObject(Reply.Configuration.PartArray(i).PartPriceArray(i4).price),
                                        api_para.CheckObject(Reply.Configuration.PartArray(i).PartPriceArray(i4).discountCode),
                                        api_para.CheckObject(Reply.Configuration.PartArray(i).PartPriceArray(i4).priceEffectiveDate),
                                        api_para.CheckObject(Reply.Configuration.PartArray(i).PartPriceArray(i4).priceExpiryDate))

                        Next
                    End If

                    If Not IsNothing(Reply.Configuration.PartArray(i).LocationIdArray) Then
                        For i3 = 0 To Reply.Configuration.PartArray(i).LocationIdArray.Length - 1
                            AddToPartLocationArray(PartLocationArray, Reply.Configuration.PartArray(i).partId,
                                                   api_para.CheckObject(Reply.Configuration.PartArray(i).LocationIdArray(i3).locationId))
                        Next
                    End If
                Next
            End If
            If Not IsNothing(Reply.Configuration) Then
                If Not IsNothing(Reply.Configuration.LocationArray) Then

                    For i2 = 0 To Reply.Configuration.LocationArray.Length - 1
                        AddToLocationArray(LocationArray,
                                api_para.CheckObject(Reply.Configuration.LocationArray(i2).locationId),
                                api_para.CheckObject(Reply.Configuration.LocationArray(i2).locationName),
                                api_para.CheckObject(Reply.Configuration.LocationArray(i2).decorationsIncluded),
                                api_para.CheckObject(Reply.Configuration.LocationArray(i2).defaultLocation),
                                api_para.CheckObject(Reply.Configuration.LocationArray(i2).maxDecoration),
                                api_para.CheckObject(Reply.Configuration.LocationArray(i2).minDecoration),
                                api_para.CheckObject(Reply.Configuration.LocationArray(i2).locationRank))

                        If Not IsNothing(Reply.Configuration.LocationArray(i2).DecorationArray) Then
                            For ii = 0 To Reply.Configuration.LocationArray(i2).DecorationArray.Length - 1
                                AddToDecorationArray(DecorationArray,
                                    Reply.Configuration.LocationArray(i2).locationId,
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationId),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationName),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationGeometry),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationHeight),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationWidth),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationDiameter),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationUom),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).allowSubForDefaultLocation),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).allowSubForDefaultMethod),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).itemPartQuantityLTM),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationUnitsIncluded),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationUnitsIncludedUom),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationUnitsMax),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).defaultDecoration),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).leadTime),
                                    api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).rushLeadTime), api_para)

                                If Not IsNothing(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray) Then
                                    For iii = 0 To Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray.Length - 1
                                        AddToChargeArray(ChargeArray, Reply.Configuration.LocationArray(i2).DecorationArray(ii).decorationId,
                                        api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).chargeId),
                                        api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).chargeName),
                                        api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).chargeType),
                                        api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).chargeDescription),
                                        api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).chargesAppliesLTM))

                                        'add prices...
                                        If Not IsNothing(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray) Then
                                            For iV = 0 To Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray.Length - 1
                                                AddToChargePriceArray(ChargePriceArray, Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).chargeId,
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).xMinQty),
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).xUom),
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).yMinQty),
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).yUom),
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).price),
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).discountCode),
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).repeatPrice),
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).repeatDiscountCode),
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).priceEffectiveDate),
                                            api_para.CheckObject(Reply.Configuration.LocationArray(i2).DecorationArray(ii).ChargeArray(iii).ChargePriceArray(iV).priceExpiryDate)
                                        )
                                            Next
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    Next
                End If
            End If



            AddRowsToPartArray(PartArray, api_para)
            AddRowsToPartPrice(PartPrice, api_para)
            AddRowsToPartLocationArray(PartLocationArray, api_para)
            AddRowsToLocationArray(LocationArray, api_para)
            AddRowsToDecorationArray(DecorationArray, api_para)
            AddRowsToChargeArray(ChargeArray, api_para)
            AddRowsToChargePriceArray(ChargePriceArray, api_para)
        Catch ex As Exception
            api_para.WriteEventToLog("Error", "GetConfigurationAndPricingExtractFromReply: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try


    End Sub

    Private Sub AddToPartPrice(ByRef PartPrice As List(Of PartPrice), ByVal partId As String, ByVal minQuantity As Integer, ByVal priceUom As String,
                                        ByVal price As Decimal, ByVal discountCode As String,
                                        ByVal priceEffectiveDate As String, ByVal priceExpiryDate As String)

        PartPrice.Add(New PartPrice() With {
       .partId = partId,
       .minQuantity = minQuantity,
       .priceUom = priceUom,
       .price = price,
       .discountCode = discountCode,
       .priceEffectiveDate = priceEffectiveDate,
       .priceExpiryDate = priceExpiryDate
   })
    End Sub



    Private Sub AddToChargeArray(ByRef ChargeArray As List(Of ChargeArray), ByVal decorationId As Integer, ByVal chargeId As Integer, ByVal chargeName As String, ByVal chargeType As String,
                           ByVal chargeDescription As String, ByVal chargeAppliesLTM As Boolean)

        ChargeArray.Add(New ChargeArray() With {
       .decorationId = decorationId,
       .chargeId = chargeId,
       .chargeName = chargeName,
       .chargeType = chargeType,
       .chargeDescription = chargeDescription,
       .chargeAppliesLTM = chargeAppliesLTM
   })
    End Sub


    Private Sub AddToChargePriceArray(ByRef ChargeArray As List(Of ChargePriceArray), ByVal chargeId As Integer, ByVal xMinQty As Integer, ByVal xUom As String,
                                        ByVal yMinQty As Integer, ByVal yUom As String, ByVal price As Decimal, ByVal discountCode As String, ByVal repeatPrice As Decimal, ByVal repeatDiscountCode As String,
                                        ByVal priceEffectiveDate As String, ByVal priceExpiryDate As String)

        ChargeArray.Add(New ChargePriceArray() With {
       .chargeId = chargeId,
       .xMinQty = xMinQty,
       .xUom = xUom,
       .yMinQty = yMinQty,
       .yUom = yUom,
       .price = price,
       .discountCode = discountCode,
       .repeatPrice = repeatPrice,
       .repeatDiscountCode = repeatDiscountCode,
       .priceEffectiveDate = priceEffectiveDate,
       .priceExpiryDate = priceExpiryDate
   })
    End Sub



    Private Sub AddToLocationArray(ByRef LocationArray As List(Of LocationArray), ByVal locationId As Integer, ByVal locationName As String, ByVal decorationsIncluded As Integer, ByVal defaultLocation As Boolean,
                           ByVal maxDecoration As Integer, ByVal minDecoration As Integer, ByVal locationRank As Integer)

        LocationArray.Add(New LocationArray() With {
           .locationId = locationId,
           .locationName = locationName,
           .decorationsIncluded = decorationsIncluded,
           .defaultLocation = defaultLocation,
           .maxDecoration = maxDecoration,
           .minDecoration = minDecoration,
           .locationRank = locationRank
       })

    End Sub

    Private Sub AddToPartLocationArray(ByRef LocationArray As List(Of PartLocationArray), ByVal partId As String, ByVal locationId As Integer)

        LocationArray.Add(New PartLocationArray() With {
            .partId = partId,
            .locationId = locationId
        })

    End Sub


    Private Sub AddToPartArray(ByRef PartArray As List(Of PartArray), ByVal partId As String, ByVal partDescription As String, ByVal partGroup As Integer, ByVal nextPartGroup As Integer,
                           ByVal partGroupRequired As Boolean, ByVal partGroupDescription As String, ByVal ratio As Decimal, ByVal defaultPart As Boolean)

        PartArray.Add(New PartArray() With {
           .partId = partId,
           .partDescription = partDescription,
           .partGroup = partGroup,
           .nextPartGroup = nextPartGroup,
           .partGroupRequired = partGroupRequired,
           .partGroupDescription = partGroupDescription,
           .ratio = ratio,
           .defaultPart = defaultPart
       })

    End Sub

    Private Sub AddToDecorationArray(ByRef DecorationArray As List(Of DecorationArray), ByVal locationId As Integer, ByVal decorationId As Integer, ByVal decorationName As String, ByVal decorationGeometry As String, ByVal decorationHeight As Decimal,
                                    ByVal decorationWidth As Decimal,
                                    ByVal decorationDiameter As Decimal, ByVal decorationUom As String, ByVal allowSubForDefaultLocation As Boolean, ByVal allowSubForDefaultMethod As Boolean,
                                    ByVal itemPartQuantityLTM As Boolean, ByVal decorationUnitsIncluded As String, ByVal decorationUnitsIncludedUom As String,
                                    ByVal decorationUnitsMax As Integer, ByVal defaultDecoration As Boolean, ByVal leadTime As Integer, ByVal rushLeadTime As Integer,
                                    ByVal api_para As Object)

        DecorationArray.Add(New DecorationArray() With {
           .locationId = locationId,
           .decorationId = decorationId,
           .decorationName = decorationName,
           .decorationGeometry = decorationGeometry,
           .decorationHeight = decorationHeight,
           .decorationWidth = decorationWidth,
           .decorationDiameter = decorationDiameter,
           .decorationUom = decorationUom,
           .allowSubForDefaultLocation = allowSubForDefaultLocation,
           .allowSubForDefaultMethod = allowSubForDefaultMethod,
           .itemPartQuantityLTM = itemPartQuantityLTM,
           .decorationUnitsIncluded = decorationUnitsIncluded,
           .decorationUnitsIncludedUom = decorationUnitsIncludedUom,
           .decorationUnitsMax = decorationUnitsMax,
           .defaultDecoration = defaultDecoration,
           .leadTime = leadTime,
           .rushLeadTime = rushLeadTime
       })

    End Sub

    Private Sub AddRowsToDecorationArray(ByRef InfoList As List(Of DecorationArray), ByVal api_para As Object)
        '      Property locationId As Integer
        'Property decorationId As Integer
        'Property decorationName As String
        'Property decorationGeometry As String
        'Property decorationHeight As Decimal
        'Property decorationWidth As Decimal
        'Property decorationDiameter As Decimal
        'Property decorationUom As String
        'Property allowSubForDefaultLocation As Boolean
        'Property allowSubForDefaultMethod As Boolean
        'Property itemPartQuantityLTM As Integer
        'Property decorationUnitsIncluded As String
        'Property decorationUnitsIncludedUom As String
        'Property decorationUnitsMax As Integer
        'Property defaultDecoration As Boolean
        'Property leadTime As Integer
        'Property rushLeadTime As Integer
        Try
            'DecorationArray_dt.Fill(HIT_ds.API_PACS_Decoration)
            For Each item As DecorationArray In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PACS_Decoration.NewRow()

                newInsertParameterRow("locationId") = item.locationId
                newInsertParameterRow("decorationId") = item.decorationId
                newInsertParameterRow("decorationName") = item.decorationName
                newInsertParameterRow("decorationGeometry") = item.decorationGeometry
                newInsertParameterRow("decorationHeight") = item.decorationHeight
                newInsertParameterRow("decorationWidth") = item.decorationWidth

                newInsertParameterRow("decorationDiameter") = item.decorationDiameter
                newInsertParameterRow("decorationUom") = item.decorationUom
                newInsertParameterRow("allowSubForDefaultLocation") = item.allowSubForDefaultLocation
                newInsertParameterRow("allowSubForDefaultMethod") = item.allowSubForDefaultMethod
                newInsertParameterRow("itemPartQuantityLTM") = item.itemPartQuantityLTM
                newInsertParameterRow("decorationUnitsIncluded") = item.decorationUnitsIncluded

                newInsertParameterRow("decorationUnitsIncludedUom") = item.decorationUnitsIncludedUom
                newInsertParameterRow("defaultDecoration") = item.defaultDecoration
                newInsertParameterRow("leadTime") = item.leadTime
                newInsertParameterRow("rushLeadTime") = item.rushLeadTime

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PACS_Decoration.Rows.Add(newInsertParameterRow)

                ' Debug.WriteLine("AddRowsToDecorationArray {0} in:  {1}", productId, item.decorationName)
            Next

            DecorationArray_dt.Update(HIT_ds.API_PACS_Decoration)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            api_para.WriteEventToLog("Error", "AddRowsToDecorationArray: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub
    Private Sub AddRowsToLocationArray(ByRef InfoList As List(Of LocationArray), ByVal api_para As Object)

        '.locationId = locationId,
        '   .locationName = locationName,
        '   .decorationsIncluded = decorationsIncluded,
        '   .defaultLocation = defaultLocation,
        '   .maxDecoration = maxDecoration,
        '   .minDecoration = minDecoration,
        '   .locationRank = locationRank



        Try
            'LocationArray_dt.Fill(HIT_ds.API_PACS_Location)
            For Each item As LocationArray In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PACS_Location.NewRow()

                newInsertParameterRow("locationId") = item.locationId
                newInsertParameterRow("locationName") = item.locationName
                newInsertParameterRow("decorationsIncluded") = item.decorationsIncluded
                newInsertParameterRow("maxDecoration") = item.maxDecoration
                newInsertParameterRow("minDecoration") = item.minDecoration
                newInsertParameterRow("locationRank") = item.locationRank

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PACS_Location.Rows.Add(newInsertParameterRow)


                Debug.WriteLine("AddRowsToLocationArray {0} in:  {1}", productId, item.locationName)
            Next

            LocationArray_dt.Update(HIT_ds.API_PACS_Location)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            api_para.WriteEventToLog("Error", "AddRows: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try
    End Sub
    Private Sub AddRowsToChargePriceArray(ByRef InfoList As List(Of ChargePriceArray), ByVal api_para As Object)
        '     Property chargeId As Integer
        'Property xMinQty As Integer
        'Property xUom As String
        'Property yMinQty As Integer
        'Property yUom As String
        'Property price As Decimal
        'Property discountCode As String
        'Property repeatPrice As Decimal
        'Property repeatDiscountCode As String
        'Property priceEffectiveDate As String
        'Property priceExpiryDate As Stri

        Try
            'ChargePriceArray_dt.Fill(HIT_ds.API_PACS_ChargePrice)
            For Each item As ChargePriceArray In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PACS_ChargePrice.NewRow()

                newInsertParameterRow("chargeId") = item.chargeId
                newInsertParameterRow("xMinQty") = item.xMinQty
                newInsertParameterRow("xUom") = item.xUom
                newInsertParameterRow("yMinQty") = item.yMinQty
                newInsertParameterRow("yUom") = item.yUom

                newInsertParameterRow("price") = item.price
                newInsertParameterRow("discountCode") = item.discountCode
                newInsertParameterRow("repeatPrice") = item.repeatPrice
                newInsertParameterRow("repeatDiscountCode") = item.repeatDiscountCode
                newInsertParameterRow("priceEffectiveDate") = item.priceEffectiveDate
                newInsertParameterRow("priceExpiryDate") = item.priceExpiryDate


                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PACS_ChargePrice.Rows.Add(newInsertParameterRow)


                'Debug.WriteLine("AddRowsToChargePriceArray {0} in:  {1}", productId, item.chargeId)
            Next
            ChargePriceArray_dt.Update(HIT_ds.API_PACS_ChargePrice)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            api_para.WriteEventToLog("Error", "AddRowsToChargePriceArray: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    Private Sub AddRowsToPartPrice(ByRef InfoList As List(Of PartPrice), ByVal api_para As Object)

        '        Private Class PartPrice
        '    Property partId As String
        '    Property minQuantity As Integer
        '    Property price As Decimal
        '    Property discountCode As String
        '    Property priceUom As String
        '    Property priceEffectiveDate As String
        '    Property priceExpiryDate As String
        'End Class

        Try
            'PartPrice_dt.Fill(HIT_ds.API_PACS_PartPrice)
            For Each item As PartPrice In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PACS_PartPrice.NewRow()

                newInsertParameterRow("partId") = item.partId
                newInsertParameterRow("minQuantity") = item.minQuantity
                newInsertParameterRow("priceUom") = item.priceUom
                newInsertParameterRow("price") = item.price
                newInsertParameterRow("discountCode") = item.discountCode
                newInsertParameterRow("priceEffectiveDate") = item.priceEffectiveDate
                newInsertParameterRow("priceExpiryDate") = item.priceExpiryDate

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PACS_PartPrice.Rows.Add(newInsertParameterRow)

                ' Debug.WriteLine("AddRowsToPartPrice {0} in:  {1}", productId, item.partId)
            Next
            PartPrice_dt.Update(HIT_ds.API_PACS_PartPrice)


        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            api_para.WriteEventToLog("Error", "AddRowsToPartPrice: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    Private Sub AddRowsToChargeArray(ByRef InfoList As List(Of ChargeArray), ByVal api_para As Object)
        '   Private Class ChargeArray
        '    Property decorationId As Integer
        '    Property chargeId As Integer
        '    Property chargeName As String
        '    Property chargeType As String
        '    Property chargeDescription As String
        '    Property chargeAppliesLTM As Boolean
        'End Class
        Try
            'ChargeArray_dt.Fill(HIT_ds.API_PACS_Charge)
            For Each item As ChargeArray In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PACS_Charge.NewRow()

                newInsertParameterRow("decorationId") = item.decorationId
                newInsertParameterRow("chargeId") = item.chargeId
                newInsertParameterRow("chargeName") = item.chargeName
                newInsertParameterRow("chargeType") = item.chargeType
                newInsertParameterRow("chargeDescription") = item.chargeDescription
                newInsertParameterRow("chargeAppliesLTM") = item.chargeAppliesLTM


                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PACS_Charge.Rows.Add(newInsertParameterRow)

                Debug.WriteLine("AddRowsToChargeArray {0} in:  {1}", productId, item.chargeName)
            Next
            ChargeArray_dt.Update(HIT_ds.API_PACS_Charge)


        Catch ex As Exception
            'Debug.WriteLine(ex.Message)
            api_para.WriteEventToLog("Error", "AddRowsToChargeArray: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    Private Sub AddRowsToPartArray(ByRef InfoList As List(Of PartArray), ByVal api_para As Object)

        Try
            'PartArray_dt.Fill(HIT_ds.API_PACS_Part)
            For Each item As PartArray In InfoList

                'VendorImportID    Int			  Not NULL,
                'productId            VARCHAR(64)	  Not NULL,
                'partId               VARCHAR(64)	  Not NULL,
                'partDescription      NVARCHAR(256)  NULL,
                'locationId           Int			  NULL,
                'partGroup            NVARCHAR(100)  NULL,
                'partGroupRequired    Int			  NULL,
                'partGroupDescription NVARCHAR(100)  NULL,
                'ratio                Decimal(12, 4) NULL,
                'defaultPart          BIT			  NULL,

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PACS_Part.NewRow()


                newInsertParameterRow("partGroup") = item.partGroup
                newInsertParameterRow("partDescription") = item.partDescription
                newInsertParameterRow("partGroupRequired") = item.partGroupRequired
                newInsertParameterRow("partGroupRequired") = item.partGroupRequired
                newInsertParameterRow("partGroupDescription") = item.partGroupDescription
                newInsertParameterRow("partId") = item.partId
                newInsertParameterRow("productId") = productId
                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PACS_Part.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToParts {0} in:  {1}", productId, item.partId)
            Next
            PartArray_dt.Update(HIT_ds.API_PACS_Part)


        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            api_para.WriteEventToLog("Error", "AddRowsToPartArray: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub
    Private Sub AddRowsToPartLocationArray(ByRef InfoList As List(Of PartLocationArray), ByVal api_para As Object)

        Try
            'PartLocation_dt.Fill(HIT_ds.API_PACS_PartLocation)
            For Each item As PartLocationArray In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PACS_PartLocation.NewRow()

                newInsertParameterRow("locationId") = item.locationId
                newInsertParameterRow("partId") = item.partId

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PACS_PartLocation.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.locationId)
            Next

            PartLocation_dt.Update(HIT_ds.API_PACS_PartLocation)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            api_para.WriteEventToLog("Error", "AddRowsToPartLocationArray: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    Private Class PartArray

        Property partId As String
        Property partDescription As String
        Property partGroup As Integer
        Property nextPartGroup As Integer
        Property partGroupRequired As Boolean
        Property partGroupDescription As String
        Property ratio As Decimal
        Property defaultPart As Boolean
    End Class
    Private Class PartPrice
        Property partId As String
        Property minQuantity As Integer
        Property price As Decimal
        Property discountCode As String
        Property priceUom As String
        Property priceEffectiveDate As String
        Property priceExpiryDate As String
    End Class
    Private Class PartLocationArray
        Property partId As String
        Property locationId As Integer
    End Class


    Private Class LocationArray

            Property locationId As Integer
            Property locationName As String
            Property decorationsIncluded As Integer
            Property defaultLocation As Boolean
            Property maxDecoration As Integer
            Property minDecoration As Integer
            Property locationRank As Integer

        End Class


    Private Class DecorationArray
        Property locationId As Integer
        Property decorationId As Integer
        Property decorationName As String
        Property decorationGeometry As String
        Property decorationHeight As Decimal
        Property decorationWidth As Decimal
        Property decorationDiameter As Decimal
        Property decorationUom As String
        Property allowSubForDefaultLocation As Boolean
        Property allowSubForDefaultMethod As Boolean
        Property itemPartQuantityLTM As Integer
        Property decorationUnitsIncluded As String
        Property decorationUnitsIncludedUom As String
        Property decorationUnitsMax As Integer
        Property defaultDecoration As Boolean
        Property leadTime As Integer
        Property rushLeadTime As Integer
    End Class

    Private Class ChargePriceArray
        Property chargeId As Integer
        Property xMinQty As Integer
        Property xUom As String
        Property yMinQty As Integer
        Property yUom As String
        Property price As Decimal
        Property discountCode As String
        Property repeatPrice As Decimal
        Property repeatDiscountCode As String
        Property priceEffectiveDate As String
        Property priceExpiryDate As String
    End Class

    Private Class ChargeArray
        Property decorationId As Integer
        Property chargeId As Integer
        Property chargeName As String
        Property chargeType As String
        Property chargeDescription As String
        Property chargeAppliesLTM As Boolean
    End Class
End Class
