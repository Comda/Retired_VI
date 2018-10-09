<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainMenu
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
        Me.lb_Criteria = New System.Windows.Forms.ListBox()
        Me.DGV_FamilyByCriteria = New System.Windows.Forms.DataGridView()
        Me.tb_ProductIDFromCriteria = New System.Windows.Forms.TextBox()
        Me.lb_FamilyFromCriteria = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.tb_CountMasterNO = New System.Windows.Forms.TextBox()
        Me.b_GetMasterNO = New System.Windows.Forms.Button()
        Me.DGV_NewMasterNo = New System.Windows.Forms.DataGridView()
        CType(Me.DGV_FamilyByCriteria, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DGV_NewMasterNo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lb_Criteria
        '
        Me.lb_Criteria.FormattingEnabled = True
        Me.lb_Criteria.Items.AddRange(New Object() {"Some Masterno in Magento ONLY"})
        Me.lb_Criteria.Location = New System.Drawing.Point(12, 4)
        Me.lb_Criteria.Name = "lb_Criteria"
        Me.lb_Criteria.Size = New System.Drawing.Size(176, 43)
        Me.lb_Criteria.TabIndex = 0
        '
        'DGV_FamilyByCriteria
        '
        Me.DGV_FamilyByCriteria.AllowUserToAddRows = False
        Me.DGV_FamilyByCriteria.AllowUserToDeleteRows = False
        Me.DGV_FamilyByCriteria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_FamilyByCriteria.Location = New System.Drawing.Point(12, 66)
        Me.DGV_FamilyByCriteria.Name = "DGV_FamilyByCriteria"
        Me.DGV_FamilyByCriteria.ReadOnly = True
        Me.DGV_FamilyByCriteria.Size = New System.Drawing.Size(286, 721)
        Me.DGV_FamilyByCriteria.TabIndex = 1
        '
        'tb_ProductIDFromCriteria
        '
        Me.tb_ProductIDFromCriteria.Location = New System.Drawing.Point(110, 19)
        Me.tb_ProductIDFromCriteria.Name = "tb_ProductIDFromCriteria"
        Me.tb_ProductIDFromCriteria.Size = New System.Drawing.Size(100, 20)
        Me.tb_ProductIDFromCriteria.TabIndex = 2
        '
        'lb_FamilyFromCriteria
        '
        Me.lb_FamilyFromCriteria.AutoSize = True
        Me.lb_FamilyFromCriteria.Location = New System.Drawing.Point(12, 22)
        Me.lb_FamilyFromCriteria.Name = "lb_FamilyFromCriteria"
        Me.lb_FamilyFromCriteria.Size = New System.Drawing.Size(45, 13)
        Me.lb_FamilyFromCriteria.TabIndex = 3
        Me.lb_FamilyFromCriteria.Text = "FAMILY"
        Me.lb_FamilyFromCriteria.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Linen
        Me.GroupBox1.Controls.Add(Me.tb_CountMasterNO)
        Me.GroupBox1.Controls.Add(Me.b_GetMasterNO)
        Me.GroupBox1.Controls.Add(Me.tb_ProductIDFromCriteria)
        Me.GroupBox1.Controls.Add(Me.lb_FamilyFromCriteria)
        Me.GroupBox1.Location = New System.Drawing.Point(328, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(460, 52)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Selected Family - ProductID"
        '
        'tb_CountMasterNO
        '
        Me.tb_CountMasterNO.Location = New System.Drawing.Point(326, 17)
        Me.tb_CountMasterNO.Name = "tb_CountMasterNO"
        Me.tb_CountMasterNO.Size = New System.Drawing.Size(100, 20)
        Me.tb_CountMasterNO.TabIndex = 6
        '
        'b_GetMasterNO
        '
        Me.b_GetMasterNO.Location = New System.Drawing.Point(213, 17)
        Me.b_GetMasterNO.Name = "b_GetMasterNO"
        Me.b_GetMasterNO.Size = New System.Drawing.Size(96, 23)
        Me.b_GetMasterNO.TabIndex = 5
        Me.b_GetMasterNO.Text = "Get MasterNO"
        Me.b_GetMasterNO.UseVisualStyleBackColor = True
        '
        'DGV_NewMasterNo
        '
        Me.DGV_NewMasterNo.AllowUserToAddRows = False
        Me.DGV_NewMasterNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_NewMasterNo.Location = New System.Drawing.Point(328, 66)
        Me.DGV_NewMasterNo.Name = "DGV_NewMasterNo"
        Me.DGV_NewMasterNo.ReadOnly = True
        Me.DGV_NewMasterNo.Size = New System.Drawing.Size(460, 721)
        Me.DGV_NewMasterNo.TabIndex = 5
        '
        'MainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 799)
        Me.Controls.Add(Me.DGV_NewMasterNo)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.DGV_FamilyByCriteria)
        Me.Controls.Add(Me.lb_Criteria)
        Me.Name = "MainMenu"
        Me.Text = "Form1"
        CType(Me.DGV_FamilyByCriteria, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.DGV_NewMasterNo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lb_Criteria As ListBox
    Friend WithEvents DGV_FamilyByCriteria As DataGridView
    Friend WithEvents tb_ProductIDFromCriteria As TextBox
    Friend WithEvents lb_FamilyFromCriteria As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents b_GetMasterNO As Button
    Friend WithEvents DGV_NewMasterNo As DataGridView
    Friend WithEvents tb_CountMasterNO As TextBox
End Class
