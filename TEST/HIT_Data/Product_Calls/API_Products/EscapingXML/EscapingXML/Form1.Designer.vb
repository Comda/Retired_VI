<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.VI_import1 = New EscapingXML.VI_import()
        Me.ApI_PODO_PropertyPairsTableAdapter1 = New EscapingXML.VI_importTableAdapters.API_PODO_PropertyPairsTableAdapter()
        Me.QueriesTableAdapter1 = New EscapingXML.VI_importTableAdapters.QueriesTableAdapter()
        CType(Me.VI_import1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'VI_import1
        '
        Me.VI_import1.DataSetName = "VI_import"
        Me.VI_import1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ApI_PODO_PropertyPairsTableAdapter1
        '
        Me.ApI_PODO_PropertyPairsTableAdapter1.ClearBeforeFill = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 337)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.VI_import1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents VI_import1 As VI_import
    Friend WithEvents ApI_PODO_PropertyPairsTableAdapter1 As VI_importTableAdapters.API_PODO_PropertyPairsTableAdapter
    Friend WithEvents QueriesTableAdapter1 As VI_importTableAdapters.QueriesTableAdapter
End Class
