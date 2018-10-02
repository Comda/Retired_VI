Imports System.IO
Imports System.Net

Public Class ExtractFromXML
    Public Sub ProcessImageResult(imageData As HIT_productData.ProductImageResult, ByVal MediaNumber As String)
        Try
            Dim FileNameSource As String = Nothing
            Dim FileClass As String = Nothing
            Dim FileSubClass As String = Nothing
            Dim tasks(imageData.all.Length) As Task
            Dim MedialistInput As New List(Of MediaList)

            If CheckURI(imageData.main) Then

                tasks(0) = Task.Factory.StartNew(Sub() OrdererThread(MedialistInput, imageData.main, "", "MAIN", MediaNumber), TaskCreationOptions.LongRunning)

                If imageData.all.Length > 0 Then
                    Debug.WriteLine("NumberOfImages {0} in  {1}", imageData.all.Length, MediaNumber)
                    For i As Integer = 0 To imageData.all.Length - 1
                        Dim URIStringMain = imageData.main
                        Dim URIString As String = imageData.all(i)
                        tasks(i + 1) = Task.Factory.StartNew(Sub() OrdererThread(MedialistInput, URIString, URIStringMain, "LIST", MediaNumber), TaskCreationOptions.LongRunning)
                    Next
                End If

            End If

            Task.WaitAll(tasks)

            AddRows(MedialistInput)
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "ProcessImageResult: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try


    End Sub

    Private Sub OrdererThread(ByRef MedialistInput As List(Of MediaList), ByVal URIString As String, ByVal URIStringMain As String, ByVal TypeOfImage As String, ByVal MediaNumber As String)
        Try
            If CheckURI(URIString) And (URIStringMain <> URIString) Then
                NumberOfProducts = NumberOfProducts + 1
                Dim result As Byte() = Nothing
                Dim FileNameSource As String = Nothing
                Dim FileClass As String = Nothing
                Dim FileSubClass As String = Nothing
                result = GetImageData(URIString)
                If Not IsNothing(result) Then
                    GetFileInfo(URIString, FileNameSource, FileClass, FileSubClass, TypeOfImage, MediaNumber)
                    AddToMedia(MedialistInput, result, URIString, FileNameSource, FileClass, FileSubClass, VendorImportID, MediaNumber, CreatedBy)
                End If
            End If
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "OrdererThread: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try
    End Sub
    Private Function CheckURI(URI_SourceString As String) As Boolean
        Dim Checked As Boolean = False
        Try
            Dim URI_Source As Uri = New Uri(URI_SourceString)
            Checked = True
            Return Checked
        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "CheckURI: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try
        Return Checked
    End Function


    Private Function GetImageData(URI_SourceString As String) As Byte()
        Dim HttpWReq As HttpWebRequest
        Dim URI_Source As Uri = New Uri(URI_SourceString)
        Dim result As Byte() = Nothing


        Try
            HttpWReq = CType(WebRequest.Create(URI_Source), HttpWebRequest)
            HttpWReq.ContentType = "Image/Jpeg"
            HttpWReq.Method = "GET"

            Dim Buffer() As Byte = New Byte(24500) {}

            Using response As WebResponse = HttpWReq.GetResponse()
                Using responseStream As Stream = response.GetResponseStream()
                    Using memoryStream As New MemoryStream()
                        Dim count As Integer = 0
                        Do
                            count = responseStream.Read(Buffer, 0, Buffer.Length)

                            memoryStream.Write(Buffer, 0, count)
                        Loop While count <> 0
                        result = memoryStream.ToArray()
                        'ImageSize = result.Length
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "GetImageData: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

        Return result

    End Function

    Private Sub GetFileInfo(URI_SourceString As String, ByRef FileName_Source As String, ByRef FileClass As String, ByRef FileSubClass As String, ByVal FixedClass As String, ByVal MediaNumber As String)

        Dim Path_Source As Object
        Dim URI_Source As Uri = New Uri(URI_SourceString)
        Path_Source = URI_Source.PathAndQuery
        FileName_Source = URI_Source.Segments(URI_Source.Segments.Count - 1)
        FileClass = Nothing
        FileSubClass = Nothing
        Try

            Select Case FileName_Source.Split("_").Length
                Case 1
                    FileClass = FileName_Source.Replace(MediaNumber, "").Substring(0, FileName_Source.IndexOf("."))
                Case 2
                    FileClass = FileName_Source.Split("_")(1)
                    If FileClass.IndexOf(".") > 0 Then FileClass = FileClass.Substring(0, FileClass.IndexOf("."))
                Case 3
                    FileClass = FileName_Source.Split("_")(1)
                    FileSubClass = FileName_Source.Split("_")(2)
                    If FileSubClass.IndexOf(".") > 0 Then FileSubClass = FileSubClass.Substring(0, FileSubClass.IndexOf("."))
                Case Else
                    FileClass = FileName_Source.Split("_")(1)
                    FileSubClass = FileName_Source.Split("_")(2)

            End Select
            If FixedClass.Length > 0 Then
                FileClass = FixedClass
                FileSubClass = Nothing
            End If

        Catch ex As Exception
            Api_Para.WriteEventToLog("Error", "GetFileInfo: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub

    Private Sub AddRows(ByRef MediaList As List(Of MediaList))

        Try
            For Each item As MediaList In MediaList

                'VendorImportMedia_dt.Fill(HIT_ds.VendorImportMedia)

                Dim ImageSize As Integer = 0
                If Not IsNothing(item.result) Then
                    ImageSize = item.result.Length
                Else
                    ImageSize = 0
                End If

                Dim newInsertParameterRow As DataRow = HIT_ds.API_PODO_Images.NewRow()
                'newInsertParameterRow("VendorImportMediaID") = SourceRow.Item("VendorImportMediaID")
                newInsertParameterRow("URL") = item.URI_SourceString
                newInsertParameterRow("FileName") = item.FileName_Source

                newInsertParameterRow("Class") = item.FileClass
                newInsertParameterRow("SubClass") = item.FileSubClass

                newInsertParameterRow("ProductId") = item.MediaNumber
                newInsertParameterRow("MediaSize") = ImageSize
                newInsertParameterRow("MediaBinary") = item.result

                newInsertParameterRow("VendorImportID") = VendorImportID
                newInsertParameterRow("RequestID") = SessionId

                HIT_ds.API_PODO_Images.Rows.Add(newInsertParameterRow)

                Images_dt.Update(HIT_ds.API_PODO_Images)

                'Debug.WriteLine("ImageName {0} in:  {1}", item.FileName_Source, item.MediaNumber)
            Next

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Api_Para.WriteEventToLog("Error", "AddRows: ", ex.Message, StopwatchLocal, Guid.Parse(TransactionID), ControlRoot_Current)
        End Try

    End Sub


    Private Sub AddToMedia(ByRef MediaList As List(Of MediaList), ByVal result As Byte(), ByVal URI_SourceString As String, ByVal FileName_Source As String, ByVal FileClass As String, ByVal FileSubClass As String, VendorImportID As Integer, MediaNumber As String, CreatedBy As String)

        MediaList.Add(New MediaList() With {
            .result = result,
            .URI_SourceString = URI_SourceString,
            .FileName_Source = FileName_Source,
            .FileClass = FileClass,
            .FileSubClass = FileSubClass,
            .VendorImportID = VendorImportID,
            .MediaNumber = MediaNumber,
            .CreatedBy = CreatedBy
       })

    End Sub



    Private Class MediaList
        Property result As Byte()
        Property URI_SourceString As String
        Property FileName_Source As String
        Property FileClass As String
        Property FileSubClass As String
        Property VendorImportID As Integer
        Property MediaNumber As String
        Property CreatedBy As String

    End Class


End Class
