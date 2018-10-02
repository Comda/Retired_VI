Imports System.Xml

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Me.ApI_PODO_PropertyPairsTableAdapter1.Fill(VI_import1.API_PODO_PropertyPairs, "104", "IMPRINT AREA")

        'Dim unescaped = VI_import1.API_PODO_PropertyPairs.Rows(0).Item("PropValue")
        Dim Masterno As String = "HT_W_1035"  '"HT_W_104"
        Dim shortdescription As String
        Dim description As String
        Dim comment As String
        Me.QueriesTableAdapter1.Magento_API_Description(Masterno, shortdescription, description, comment)
        Dim unescaped = comment
        Dim Escaped As String = XmlEscape(unescaped)
    End Sub


    Public Shared Function XmlEscape(unescaped As String) As String
        Dim doc As New XmlDocument()
        Dim node As XmlNode = doc.CreateElement("root")
        node.InnerText = unescaped
        Return node.InnerXml
    End Function

    Public Shared Function XmlUnescape(escaped As String) As String
        Dim doc As New XmlDocument()
        Dim node As XmlNode = doc.CreateElement("root")
        node.InnerXml = escaped
        Return node.InnerText
    End Function

    '=======================================================
    'Service provided by Telerik (www.telerik.com)
    'Conversion powered by NRefactory.
    'Twitter: @telerik
    'Facebook: facebook.com/telerik
    '=======================================================


End Class
