Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports COMDA_COM_Product_QA_API_1.Magento_API_01
Imports Newtonsoft.Json
Imports <xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
Imports <xmlns:urn="urn:Magento">
Imports <xmlns:xsd="http://www.w3.org/2001/XMLSchema">
Imports <xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
Imports <xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/">
Imports <xmlns:ns1="urn:Magento">
Imports <xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/">

Public Class Category
    Public Sub New(ByVal SessionId As String, ByVal dbConnection As SqlConnection, ByVal ImportID As Guid)
        InitializeSQLVariables(dbConnection)
        GetCategories(SessionId, ImportID)
    End Sub
    Private Sub GetCategories(ByVal SessionId As String, ByVal ImportID As Guid)

        'Dim MagentoType As String
        'Dim GroupFilter As New filters
        'GroupFilter = addFilter(GroupFilter, "type", "eq", "grouped")
        'MagentoType = "grouped"

        'Dim ChildFilter As New filters
        'ChildFilter = addFilter(ChildFilter, "type", "eq", "simple")
        'MagentoType = "simple"

        'Dim ParentFilter As New filters
        'ParentFilter = addFilter(ParentFilter, "type", "eq", "configurable")
        'MagentoType = "configurable"

        'Get the stores
        'Dim storeEntityTable As storeEntity() = MageHandler.storeList(SessionId)

        'Dim StoreView As String = Nothing

        'For Each storeEntityItem As storeEntity In storeEntityTable
        '    StoreView = storeEntityItem.code

        'Dim catalogCategoryTreeTable As catalogCategoryTree = Nothing
        'Dim catalogProductEntityTable() As catalogProductEntity = Nothing
        Dim cat_0() As XmlNode

        Dim args(1) As String
        args(0) = "1"
        Try
            'catalogCategoryTreeTable = MageHandler.catalogCategoryTree(SessionId, "4", StoreView)
            'catalogProductEntityTable = MageHandler.catalogProductList(SessionId, ChildFilter, StoreView)
            cat_0 = MageHandler.call(SessionId, "catalog_category.tree", args)

            'Dim doc As XDocument = XDocument.Parse(cat_0(0))
            Dim xDoc As XDocument = XDocument.Load(New XmlNodeReader(cat_0(1)))


            'Dim doc As XDocument = XDocument.Load(cat_0)

            'Public Shared Function Deserialize(ByVal xml As String, ByVal type As Type) As ObjectArray
            'Dim s = New XmlSerializer(GetType(Array), New Type())
            'Dim o = CType(s.Deserialize(New StringReader(cat_0)), Array)
            'Return o
            'End Function

            'Dim xdoc As XDocument = XDocument.Parse(cat_0)
            Console.WriteLine(cat_0)


            'Dim xDoc As XDocument = XDocument.Load(New StringReader(request))
            'Dim unwrappedResponse = xdoc.Descendants(CType("http://schemas.xmlsoap.org/soap/envelope/", XNamespace) + "Body").First().FirstNode

            'Dim json As String = JsonConvert.SerializeXmlNode(xdoc)

            Dim s As XmlNode = cat_0(0)
            Debug.Print(s.Name)
            Debug.Print(s.Value)
            Dim ss As XmlNode = s.LastChild

            Stop
        Catch ex As Exception
            ' I need to get OUT because this is my baseFailing will mean NO data
            Throw New Exception("GetCategories : " & ex.Message)
        End Try

        'catalogProduct = catalogProductEntityTable.OrderBy(Function(o) o.sku).ToList()
        'If catalogProduct.Count > 0 Then
        '    UploadCatalog(catalogProduct, StoreView, ImportID)
        'End If

        'Next

    End Sub
End Class
