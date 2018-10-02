Public Class ExtractFromXML

#Region "Category"

    Private Sub AddToCategory(ByRef LocationArray As List(Of Category), ByVal productId As String, ByVal Category As String, ByVal SubCategory As String)

        LocationArray.Add(New Category() With {
            .productId = productId,
            .Category = Category,
            .SubCategory = SubCategory
        })

    End Sub

    Private Sub AddRowsToCategory(ByRef InfoList As List(Of Category))

        Try
            'Category_dt.Fill(HIT_ds.API_PON_Category)
            For Each item As Category In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_Category.NewRow()

                newInsertParameterRow("Category") = item.Category
                newInsertParameterRow("SubCategory") = item.SubCategory
                newInsertParameterRow("productId") = item.productId

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_Category.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            Category_dt.Update(HIT_ds.API_PON_Category)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToCategory: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    Private Class Category

        Property productId As String
        Property Category As String
        Property SubCategory As String

    End Class

    Public Sub GetCategoryExtractFromReply(Reply As HIT_productData.GetProductResponse)
        Try


            Dim Category As New List(Of Category)
            If Not IsNothing(Reply.Product) Then
                If Not IsNothing(Reply.Product.ProductCategoryArray) Then
                    For i = 0 To Reply.Product.ProductCategoryArray.Length - 1
                        AddToCategory(Category, Reply.Product.productId,
                                       Api_Para.CheckObject(Reply.Product.ProductCategoryArray(i).category),
                                       Api_Para.CheckObject(Reply.Product.ProductCategoryArray(i).subCategory))
                    Next

                End If

                AddRowsToCategory(Category)
            End If
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "GetCategoryExtractFromReply: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub
#End Region
#Region "Packaging"

    Public Sub GetPackagingExtractFromReply(Reply As HIT_productData.GetProductResponse)
        Try


            Dim Packaging As New List(Of Packaging)
            Dim ShippingPackage As New List(Of ShippingPackage)

            Dim i As Integer
            Dim ii As Integer
            If Not IsNothing(Reply.Product) Then
                If Not IsNothing(Reply.Product.ProductPartArray) Then
                    For i = 0 To Reply.Product.ProductPartArray.Length - 1
                        If Not IsNothing(Reply.Product.ProductPartArray(i).ProductPackagingArray) Then
                            For ii = 0 To Reply.Product.ProductPartArray(i).ProductPackagingArray.Length - 1

                                AddToPackaging(Packaging, Reply.Product.productId, Reply.Product.ProductPartArray(i).partId,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).default,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).packageType,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).description,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).quantity,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).dimensionUom,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).depth,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).height,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).width,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).weightUom,
                                           Reply.Product.ProductPartArray(i).ProductPackagingArray(ii).weight)
                            Next
                        End If
                    Next

                    For i = 0 To Reply.Product.ProductPartArray.Length - 1
                        If Not IsNothing(Reply.Product.ProductPartArray(i).ShippingPackageArray) Then
                            For ii = 0 To Reply.Product.ProductPartArray(i).ShippingPackageArray.Length - 1

                                AddToShippingPackage(ShippingPackage, Reply.Product.productId, Reply.Product.ProductPartArray(i).partId,
                                           Reply.Product.ProductPartArray(i).ShippingPackageArray(ii).packageType,
                                           Reply.Product.ProductPartArray(i).ShippingPackageArray(ii).description,
                                           Reply.Product.ProductPartArray(i).ShippingPackageArray(ii).quantity,
                                           Reply.Product.ProductPartArray(i).ShippingPackageArray(ii).dimensionUom,
                                           Reply.Product.ProductPartArray(i).ShippingPackageArray(ii).depth,
                                           Reply.Product.ProductPartArray(i).ShippingPackageArray(ii).height,
                                           Reply.Product.ProductPartArray(i).ShippingPackageArray(ii).width,
                                           Reply.Product.ProductPartArray(i).ShippingPackageArray(ii).weightUom,
                                           Reply.Product.ProductPartArray(i).ShippingPackageArray(ii).weight)
                            Next
                        End If
                    Next

                    AddRowsToPackaging(Packaging)
                    AddRowsToShippingPackage(ShippingPackage)
                End If
            End If
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "GetPackagingExtractFromReply: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try
    End Sub

    Private Sub AddToPackaging(ByRef LocationArray As List(Of Packaging), ByVal productId As String, ByVal partId As String,
                    ByVal defaultPackaging As Boolean,
                    ByVal packageType As String,
                    ByVal description As String,
                    ByVal quantity As Decimal,
                    ByVal dimensionUom As String,
                    ByVal depth As Decimal,
                    ByVal height As Decimal,
                    ByVal width As Decimal,
                    ByVal weightUom As String,
                    ByVal weight As Decimal
        )

        LocationArray.Add(New Packaging() With {
                    .productId = productId,
                    .partId = partId,
                    .defaultPackaging = defaultPackaging,
                    .packageType = packageType,
                    .description = description,
                    .quantity = quantity,
                    .dimensionUom = dimensionUom,
                    .depth = depth,
                    .height = height,
                    .width = width,
                    .weightUom = weightUom,
                    .weight = weight
        })

    End Sub
    Private Sub AddToShippingPackage(ByRef LocationArray As List(Of ShippingPackage), ByVal productId As String, ByVal partId As String,
                    ByVal packageType As String,
                    ByVal description As String,
                    ByVal quantity As Decimal,
                    ByVal dimensionUom As String,
                    ByVal depth As Decimal,
                    ByVal height As Decimal,
                    ByVal width As Decimal,
                    ByVal weightUom As String,
                    ByVal weight As Decimal
        )

        LocationArray.Add(New ShippingPackage() With {
                    .productId = productId,
                    .partId = partId,
                    .packageType = packageType,
                    .description = description,
                    .quantity = quantity,
                    .dimensionUom = dimensionUom,
                    .depth = depth,
                    .height = height,
                    .width = width,
                    .weightUom = weightUom,
                    .weight = weight
        })

    End Sub


    Private Sub AddRowsToPackaging(ByRef InfoList As List(Of Packaging))

        Try
            'Packaging_dt.Fill(HIT_ds.API_PON_Packaging)
            For Each item As Packaging In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_Packaging.NewRow()

                newInsertParameterRow("defaultPackaging") = item.defaultPackaging
                newInsertParameterRow("packageType") = item.packageType
                newInsertParameterRow("quantity") = item.quantity
                newInsertParameterRow("dimensionUom") = item.dimensionUom
                newInsertParameterRow("depth") = item.depth
                newInsertParameterRow("height") = item.height
                newInsertParameterRow("width") = item.width
                newInsertParameterRow("weightUom") = item.weightUom
                newInsertParameterRow("weight") = item.weight



                newInsertParameterRow("partId") = item.partId
                newInsertParameterRow("productId") = item.productId
                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_Packaging.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPackaging {0} in:  {1}", productId, item.packageType)
            Next

            Packaging_dt.Update(HIT_ds.API_PON_Packaging)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToPackaging: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    Private Sub AddRowsToShippingPackage(ByRef InfoList As List(Of ShippingPackage))

        Try
            'ShippingPackage_dt.Fill(HIT_ds.API_PON_ShippingPackage)
            For Each item As ShippingPackage In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_ShippingPackage.NewRow()

                newInsertParameterRow("packageType") = item.packageType
                newInsertParameterRow("quantity") = item.quantity
                newInsertParameterRow("dimensionUom") = item.dimensionUom
                newInsertParameterRow("depth") = item.depth
                newInsertParameterRow("height") = item.height
                newInsertParameterRow("width") = item.width
                newInsertParameterRow("weightUom") = item.weightUom
                newInsertParameterRow("weight") = item.weight

                newInsertParameterRow("productId") = item.productId
                newInsertParameterRow("partId") = item.partId

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_ShippingPackage.Rows.Add(newInsertParameterRow)

                Debug.WriteLine("AddRowsToShippingPackage {0} in:  {1}", productId, item.packageType)
            Next

            ShippingPackage_dt.Update(HIT_ds.API_PON_ShippingPackage)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToShippingPackage: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    Private Class Packaging
        Property productId As String
        Property partId As String
        Property defaultPackaging As Boolean
        Property packageType As String
        Property description As String
        Property quantity As Decimal
        Property dimensionUom As String
        Property depth As Decimal
        Property height As Decimal
        Property width As Decimal
        Property weightUom As String
        Property weight As Decimal



    End Class

    Private Class ShippingPackage
        Property productId As String
        Property partId As String
        Property packageType As String
        Property description As String
        Property quantity As Decimal
        Property dimensionUom As String
        Property depth As Decimal
        Property height As Decimal
        Property width As Decimal
        Property weightUom As String
        Property weight As Decimal



    End Class
#End Region
#Region "ProductInfo"

    Public Sub GetProductInfoExtractFromReply(Reply As HIT_productData.GetProductResponse)
        Try

            Dim ProductInfo As New List(Of ProductInfo)
            Dim ProductDescription As New List(Of ProductDescription)
            Dim ProductMarketingPointArray As New List(Of ProductMarketingPointArray)
            Dim ProductKeywordArray As New List(Of ProductKeywordArray)
            Dim ProductPartArray As New List(Of ProductPartArray)
            Dim PartDescription As New List(Of PartDescription)
            Dim PartApparelSize As New List(Of PartApparelSize)
            Dim PartDimension As New List(Of PartDimension)
            Dim PartColor As New List(Of PartColor)

            Dim i As Integer
            Dim i2 As Integer

            If Not IsNothing(Reply.Product) Then
                AddToProductInfo(ProductInfo, Reply.Product.productId,
                                    Api_Para.CheckObject(Reply.Product.productName),
                                    Api_Para.CheckObject(Reply.Product.productBrand),
                                    Api_Para.CheckObject(Reply.Product.export),
                                    Api_Para.CheckObject(Reply.Product.lastChangeDate),
                                    Api_Para.CheckObject(Reply.Product.creationDate),
                                    Api_Para.CheckObject(Reply.Product.endDate),
                                    Api_Para.CheckObject(Reply.Product.effectiveDate),
                                    Api_Para.CheckObject(Reply.Product.isCaution),
                                    Api_Para.CheckObject(Reply.Product.cautionComment),
                                    Api_Para.CheckObject(Reply.Product.isCloseout),
                                    Api_Para.CheckObject(Reply.Product.lineName))


            End If

            If Not IsNothing(Reply.Product) Then
                If Not IsNothing(Reply.Product.description) Then
                    For i = 0 To Reply.Product.description.Length - 1
                        AddToProductDescription(ProductDescription,
                                                Api_Para.CheckObject(Reply.Product.productId),
                                                Api_Para.CheckObject(Reply.Product.description(i)))
                    Next

                End If

                If Not IsNothing(Reply.Product.ProductMarketingPointArray) Then
                    For i = 0 To Reply.Product.ProductMarketingPointArray.Length - 1
                        AddToProductMarketingPointArray(ProductMarketingPointArray, Reply.Product.productId,
                                                        Api_Para.CheckObject(Reply.Product.ProductMarketingPointArray(i).pointType),
                                                        Api_Para.CheckObject(Reply.Product.ProductMarketingPointArray(i).pointCopy))
                    Next

                End If

                If Not IsNothing(Reply.Product.ProductKeywordArray) Then
                    For i = 0 To Reply.Product.ProductKeywordArray.Length - 1
                        AddToProductKeywordArray(ProductKeywordArray, Reply.Product.productId,
                                                 Api_Para.CheckObject(Reply.Product.ProductKeywordArray(i).keyword))

                    Next
                End If

                If Not IsNothing(Reply.Product.ProductPartArray) Then
                    For i = 0 To Reply.Product.ProductPartArray.Length - 1
                        AddToProductPartArray(ProductPartArray, Reply.Product.productId,
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).partId),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).countryOfOrigin),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).primaryMaterial),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).shape),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).leadTime),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).unspsc),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).gtin),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).isRushService),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).endDate),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).effectiveDate),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).isCloseout),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).isCaution),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).cautionComment),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).nmfcCode),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).nmfcDescription),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).isOnDemand),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).isHazmat)
                                       )


                        'description
                        If Not IsNothing(Reply.Product.ProductPartArray(i).description) Then
                            For i2 = 0 To Reply.Product.description.Length - 1
                                AddToPartDescription(PartDescription, Reply.Product.ProductPartArray(i).partId,
                                                     Api_Para.CheckObject(Reply.Product.ProductPartArray(i).description(i2)))
                            Next

                        End If
                        'PartApparelZize
                        If Not IsNothing(Reply.Product.ProductPartArray(i).ApparelSize) Then
                            AddToPartApparelSize(PartApparelSize, Reply.Product.ProductPartArray(i).partId, Reply.Product.ProductPartArray(i).ApparelSize.apparelStyle,
                                                 Api_Para.CheckObject(Reply.Product.ProductPartArray(i).ApparelSize.customSize),
                                                 Api_Para.CheckObject(Reply.Product.ProductPartArray(i).ApparelSize.labelSize))

                        End If
                        'partDimension
                        If Not IsNothing(Reply.Product.ProductPartArray(i).Dimension) Then
                            AddToPartDimension(PartDimension, Reply.Product.ProductPartArray(i).partId,
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).Dimension.depth),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).Dimension.dimensionUom),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).Dimension.height),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).Dimension.weight),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).Dimension.weightUom),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).Dimension.width)
                            )

                        End If
                        'PartColor
                        If Not IsNothing(Reply.Product.ProductPartArray(i).ColorArray) Then
                            For i2 = 0 To Reply.Product.ProductPartArray(i).ColorArray.Length - 1

                                AddToPartColor(PartColor, Reply.Product.ProductPartArray(i).partId,
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).ColorArray(i2).approximatePms),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).ColorArray(i2).colorName),
                                                Api_Para.CheckObject(Reply.Product.ProductPartArray(i).ColorArray(i2).hex))
                            Next

                        End If


                    Next
                End If
            End If

            'testing
            'GoTo Nodb
            AddRowsToProductInfo(ProductInfo)
            AddRowsToProductDescription(ProductDescription)
            AddRowsToProductMarketingPointArray(ProductMarketingPointArray)
            AddRowsToProductKeywordArray(ProductKeywordArray)
            AddRowsToProductPartArray(ProductPartArray)
            AddRowsToPartDescription(PartDescription)
            AddRowsToPartApparelSize(PartApparelSize)
            AddRowsToPartDimension(PartDimension)
            AddRowsToPartColor(PartColor)

Nodb:

        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "GetProductInfoExtractFromReply: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try
    End Sub
    'PartColor
    Private Sub AddToPartColor(ByRef PartColor As List(Of PartColor), ByVal partId As String,
                                ByVal approximatePms As String,
                                ByVal colorName As String,
                                ByVal hex As String)

        PartColor.Add(New PartColor() With {
                           .partiD = partId,
                           .approximatePms = approximatePms,
                           .colorName = colorName,
                           .hex = hex
        })

    End Sub
    Private Sub AddRowsToPartColor(ByRef InfoList As List(Of PartColor))

        Try
            PartColor_dt.Fill(HIT_ds.API_PON_PartColor)
            For Each item As PartColor In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_PartColor.NewRow()

                newInsertParameterRow("partId") = item.partiD
                newInsertParameterRow("approximatePms") = item.approximatePms
                newInsertParameterRow("colorName") = item.colorName
                newInsertParameterRow("hex") = item.hex

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_PartColor.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            PartColor_dt.Update(HIT_ds.API_PON_PartColor)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToPartColor: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    'partDimension
    Private Sub AddToPartDimension(ByRef PartDimension As List(Of PartDimension), ByVal partId As String,
                                ByVal depth As Decimal,
                                ByVal dimensionUom As String,
                                ByVal height As Decimal,
                                ByVal weight As Decimal,
                                ByVal weightUom As String,
                                ByVal width As Decimal)

        PartDimension.Add(New PartDimension() With {
                           .partiD = partId,
                           .depth = depth,
                           .dimensionUom = dimensionUom,
                           .height = height,
                           .weight = weight,
                           .weightUom = weightUom,
                           .width = width
        })

    End Sub
    Private Sub AddRowsToPartDimension(ByRef InfoList As List(Of PartDimension))

        Try
            PartDimension_dt.Fill(HIT_ds.API_PON_PartDimension)
            For Each item As PartDimension In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_PartDimension.NewRow()

                newInsertParameterRow("partId") = item.partiD
                newInsertParameterRow("depth") = item.depth
                newInsertParameterRow("dimensionUom") = item.dimensionUom
                newInsertParameterRow("height") = item.height
                newInsertParameterRow("weight") = item.weight
                newInsertParameterRow("weightUom") = item.weightUom
                newInsertParameterRow("width") = item.width

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_PartDimension.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            PartDimension_dt.Update(HIT_ds.API_PON_PartDimension)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToPartDimension: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    'PartApparelZize
    Private Sub AddToPartApparelSize(ByRef PartApparelSize As List(Of PartApparelSize), ByVal partId As String,
                                                ByVal apparelStyle As String,
                                                ByVal customSize As String,
                                                ByVal labelSize As String)

        PartApparelSize.Add(New PartApparelSize() With {
                            .partiD = partId,
                            .apparelStyle = apparelStyle,
                            .customSize = customSize,
                            .labelSize = labelSize
        })

    End Sub
    Private Sub AddRowsToPartApparelSize(ByRef InfoList As List(Of PartApparelSize))

        Try
            PartApparelSize_dt.Fill(HIT_ds.API_PON_PartApparelSize)
            For Each item As PartApparelSize In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_PartApparelSize.NewRow()

                newInsertParameterRow("partId") = item.partiD
                newInsertParameterRow("apparelStyle") = item.apparelStyle
                newInsertParameterRow("customSize") = item.customSize
                newInsertParameterRow("labelSize") = item.labelSize

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_PartApparelSize.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            PartApparelSize_dt.Update(HIT_ds.API_PON_PartApparelSize)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToPartApparelSize: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    'PartDescription
    Private Sub AddToPartDescription(ByRef ProductDescription As List(Of PartDescription), ByVal partId As String,
                                               ByVal description As String)

        ProductDescription.Add(New PartDescription() With {
                            .description = description,
                            .partiD = partId
        })

    End Sub
    Private Sub AddRowsToPartDescription(ByRef InfoList As List(Of PartDescription))

        Try
            PartDescription_dt.Fill(HIT_ds.API_PON_PartDescription)
            For Each item As PartDescription In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_PartDescription.NewRow()

                newInsertParameterRow("partId") = item.partiD
                newInsertParameterRow("description") = item.description

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_PartDescription.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            PartDescription_dt.Update(HIT_ds.API_PON_PartDescription)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToPartDescription: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub



    'ProductPartArray
    Private Sub AddToProductPartArray(ByRef ProductInfo As List(Of ProductPartArray), ByVal productId As String,
                                            ByVal partId As String,
                                            ByVal countryOfOrigin As String,
                                            ByVal primaryMaterial As String,
                                            ByVal shape As String,
                                            ByVal leadTime As String,
                                            ByVal unspsc As String,
                                            ByVal gtin As String,
                                            ByVal isRushService As Boolean,
                                            ByVal endDate As String,
                                            ByVal effectiveDate As String,
                                            ByVal isCloseout As Boolean,
                                            ByVal isCaution As Boolean,
                                            ByVal cautionComment As String,
                                            ByVal nmfcCode As String,
                                            ByVal nmfcDescription As String,
                                            ByVal isOnDemand As Boolean,
                                            ByVal isHazmat As Boolean)

        ProductInfo.Add(New ProductPartArray() With {
                            .cautionComment = cautionComment,
                            .countryOfOrigin = countryOfOrigin,
                            .effectiveDate = effectiveDate,
                            .endDate = endDate,
                            .gtin = gtin,
                            .isCaution = isCaution,
                            .isCloseout = isCloseout,
                            .isHazmat = isHazmat,
                            .isOnDemand = isOnDemand,
                            .isRushService = isRushService,
                            .leadTime = leadTime,
                            .nmfcCode = nmfcCode,
                            .nmfcDescription = nmfcDescription,
                            .partId = partId,
                            .primaryMaterial = primaryMaterial,
                            .shape = shape,
                            .unspsc = unspsc
                              })
    End Sub
    Private Sub AddRowsToProductPartArray(ByRef InfoList As List(Of ProductPartArray))

        Try
            ProductPart_dt.Fill(HIT_ds.API_PON_ProductPart)
            For Each item As ProductPartArray In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_ProductPart.NewRow()

                newInsertParameterRow("productId") = productId

                newInsertParameterRow("cautionComment") = Api_Para.NothingToDBNull(item.cautionComment)
                newInsertParameterRow("countryOfOrigin") = Api_Para.NothingToDBNull(item.countryOfOrigin)
                newInsertParameterRow("effectiveDate") = Api_Para.NothingToDBNull(item.effectiveDate)
                newInsertParameterRow("endDate") = Api_Para.NothingToDBNull(item.endDate)
                newInsertParameterRow("gtin") = Api_Para.NothingToDBNull(item.gtin)
                newInsertParameterRow("isCaution") = Api_Para.NothingToDBNull(item.isCaution)
                newInsertParameterRow("isCloseout") = Api_Para.NothingToDBNull(item.isCloseout)
                newInsertParameterRow("isHazmat") = Api_Para.NothingToDBNull(item.isHazmat)
                newInsertParameterRow("isOnDemand") = Api_Para.NothingToDBNull(item.isOnDemand)
                newInsertParameterRow("isRushService") = Api_Para.NothingToDBNull(item.isRushService)
                newInsertParameterRow("leadTime") = Api_Para.NothingToDBNull(item.leadTime)
                newInsertParameterRow("nmfcCode") = Api_Para.NothingToDBNull(item.nmfcCode)
                newInsertParameterRow("nmfcDescription") = Api_Para.NothingToDBNull(item.nmfcDescription)
                newInsertParameterRow("partId") = Api_Para.NothingToDBNull(item.partId)
                newInsertParameterRow("primaryMaterial") = Api_Para.NothingToDBNull(item.primaryMaterial)
                newInsertParameterRow("shape") = Api_Para.NothingToDBNull(item.shape)
                newInsertParameterRow("unspsc") = Api_Para.NothingToDBNull(item.unspsc)

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_ProductPart.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            ProductPart_dt.Update(HIT_ds.API_PON_ProductPart)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToProductKeywordArray: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub


    'ProductKeywordArray
    Private Sub AddToProductKeywordArray(ByRef ProductInfo As List(Of ProductKeywordArray), ByVal productId As String,
                                               ByVal keyword As String)

        ProductInfo.Add(New ProductKeywordArray() With {
                            .keyword = keyword
        })

    End Sub
    Private Sub AddRowsToProductKeywordArray(ByRef InfoList As List(Of ProductKeywordArray))

        Try
            ProductKeyword_dt.Fill(HIT_ds.API_PON_ProductKeyword)
            For Each item As ProductKeywordArray In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_ProductKeyword.NewRow()

                newInsertParameterRow("productId") = productId

                newInsertParameterRow("keyword") = item.keyword

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_ProductKeyword.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            ProductKeyword_dt.Update(HIT_ds.API_PON_ProductKeyword)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToProductKeywordArray: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub


    'ProductMarketingPointArray
    Private Sub AddToProductMarketingPointArray(ByRef ProductInfo As List(Of ProductMarketingPointArray), ByVal productId As String,
                                               ByVal pointType As String,
                                               ByVal pointCopy As String)

        ProductInfo.Add(New ProductMarketingPointArray() With {
                            .pointType = pointType,
                            .pointCopy = pointCopy
        })

    End Sub
    Private Sub AddRowsToProductMarketingPointArray(ByRef InfoList As List(Of ProductMarketingPointArray))

        Try
            ProductMarketingPoint_dt.Fill(HIT_ds.API_PON_ProductMarketingPoint)
            For Each item As ProductMarketingPointArray In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_ProductMarketingPoint.NewRow()

                newInsertParameterRow("productId") = productId

                newInsertParameterRow("pointType") = item.pointType
                newInsertParameterRow("pointCopy") = item.pointCopy

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_ProductMarketingPoint.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            ProductMarketingPoint_dt.Update(HIT_ds.API_PON_ProductMarketingPoint)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToProductMarketingPointArray: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub


    Private Sub AddToProductInfo(ByRef ProductInfo As List(Of ProductInfo), ByVal productId As String,
                                                ByVal productName As String,
                                                ByVal productBrand As String,
                                                ByVal export As Boolean,
                                                ByVal lastChangeDate As String,
                                                ByVal creationDate As String,
                                                ByVal endDate As String,
                                                ByVal effectiveDate As String,
                                                ByVal isCaution As Boolean,
                                                ByVal cautionComment As String,
                                                ByVal isCloseout As Boolean,
                                                ByVal lineName As String)

        ProductInfo.Add(New ProductInfo() With {
                            .productId = productId,
                            .productName = productName,
                            .productBrand = productBrand,
                            .export = export,
                            .lastChangeDate = lastChangeDate,
                            .creationDate = creationDate,
                            .endDate = endDate,
                            .effectiveDate = effectiveDate,
                            .isCaution = isCaution,
                            .cautionComment = cautionComment,
                            .isCloseout = isCloseout,
                            .lineName = lineName
        })


    End Sub

    'ProductDescription
    Private Sub AddToProductDescription(ByRef ProductDescription As List(Of ProductDescription), ByVal productId As String,
                                               ByVal description As String)

        ProductDescription.Add(New ProductDescription() With {
                            .description = description
        })

    End Sub
    Private Sub AddRowsToProductDescription(ByRef InfoList As List(Of ProductDescription))

        Try
            ProductDescription_dt.Fill(HIT_ds.API_PON_ProductDescription)
            For Each item As ProductDescription In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_ProductDescription.NewRow()

                newInsertParameterRow("productId") = productId

                newInsertParameterRow("description") = item.description

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_ProductDescription.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            ProductDescription_dt.Update(HIT_ds.API_PON_ProductDescription)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToProductDescription: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub


    Private Sub AddRowsToProductInfo(ByRef InfoList As List(Of ProductInfo))

        Try
            Product_dt.Fill(HIT_ds.API_PON_Product)
            For Each item As ProductInfo In InfoList

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PON_Product.NewRow()

                newInsertParameterRow("productId") = item.productId
                newInsertParameterRow("productName") = item.productName
                newInsertParameterRow("productBrand") = item.productBrand

                newInsertParameterRow("export") = item.export
                newInsertParameterRow("lastChangeDate") = Api_Para.NothingToDBNull(item.lastChangeDate)
                newInsertParameterRow("creationDate") = Api_Para.NothingToDBNull(item.creationDate)

                newInsertParameterRow("endDate") = Api_Para.NothingToDBNull(item.endDate)
                newInsertParameterRow("effectiveDate") = Api_Para.NothingToDBNull(item.effectiveDate)
                newInsertParameterRow("isCaution") = item.isCaution

                newInsertParameterRow("cautionComment") = item.cautionComment
                newInsertParameterRow("isCloseout") = item.isCloseout
                newInsertParameterRow("lineName") = item.lineName

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PON_Product.Rows.Add(newInsertParameterRow)

                'Debug.WriteLine("AddRowsToPartLocationArray {0} in:  {1}", productId, item.Category)
            Next

            Product_dt.Update(HIT_ds.API_PON_Product)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRowsToProductInfo: " & productId, ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    Private Class ProductInfo

        Property productId As String
        Property productName As String
        Property productBrand As String
        Property export As Boolean
        Property lastChangeDate As String
        Property creationDate As String
        Property endDate As String
        Property effectiveDate As String
        Property isCaution As Boolean
        Property cautionComment As String
        Property isCloseout As Boolean
        Property lineName As String

    End Class
    Private Class ProductMarketingPointArray
        Property pointType As String
        Property pointCopy As String
    End Class
    Private Class ProductKeywordArray
        Property keyword As String
    End Class
    Private Class ProductCategoryArray
        Property Category As String
        Property SubCategory As String
    End Class
    Private Class RelatedProductArray
        Property relationType As String
        Property productId As String
        Property partId As String
    End Class
    Private Class ProductPartArray
        Property partId As String
        Property description As String
        Property countryOfOrigin As String
        Property primaryMaterial As String
        Property shape As String
        Property leadTime As String
        Property unspsc As String
        Property gtin As String
        Property isRushService As Boolean
        Property endDate As String
        Property effectiveDate As String
        Property isCloseout As Boolean
        Property isCaution As Boolean
        Property cautionComment As String
        Property nmfcCode As String
        Property nmfcDescription
        Property isOnDemand As Boolean
        Property isHazmat As Boolean

    End Class
    Private Class ProductSpecification
        Property specificationType As String
        Property specificationUom As String
        Property measurementValue As String

    End Class
    Private Class PartColor
        Property partiD As String
        Property colorName As String
        Property hex As String
        Property approximatePms As String
    End Class
    Private Class PartApparelSize
        Property partiD As String
        Property apparelStyle As String
        Property labelSize As String
        Property customSize As String
    End Class
    Private Class PartDimension
        Property partiD As String
        Property dimensionUom As String
        Property depth As String
        Property height As String
        Property width As String
        Property weightUom As String
        Property weight As String
    End Class
    Private Class ProductDescription
        Property description As String
    End Class
    Private Class PartDescription
        Property partiD As String
        Property description As String
    End Class


#End Region
End Class
