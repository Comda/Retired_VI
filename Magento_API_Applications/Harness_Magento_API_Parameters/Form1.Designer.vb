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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tb_UserID = New System.Windows.Forms.TextBox()
        Me.tb_API_ID = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tb_SessionID = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.bGetSessionID = New System.Windows.Forms.Button()
        Me.tb_ControlRoot = New System.Windows.Forms.TextBox()
        Me.tb_TransactionGUID = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.b_GetCatalog = New System.Windows.Forms.Button()
        Me.b_synchNames = New System.Windows.Forms.Button()
        Me.b_SynchOptions = New System.Windows.Forms.Button()
        Me.b_SetUp = New System.Windows.Forms.Button()
        Me.B_LikeCLR = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "UserID"
        '
        'tb_UserID
        '
        Me.tb_UserID.Location = New System.Drawing.Point(76, 33)
        Me.tb_UserID.Name = "tb_UserID"
        Me.tb_UserID.Size = New System.Drawing.Size(100, 20)
        Me.tb_UserID.TabIndex = 1
        Me.tb_UserID.Text = "jeromeb"
        '
        'tb_API_ID
        '
        Me.tb_API_ID.Location = New System.Drawing.Point(76, 67)
        Me.tb_API_ID.Name = "tb_API_ID"
        Me.tb_API_ID.Size = New System.Drawing.Size(100, 20)
        Me.tb_API_ID.TabIndex = 3
        Me.tb_API_ID.Text = "sophie"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "API _ID"
        '
        'tb_SessionID
        '
        Me.tb_SessionID.Location = New System.Drawing.Point(266, 33)
        Me.tb_SessionID.Name = "tb_SessionID"
        Me.tb_SessionID.Size = New System.Drawing.Size(301, 20)
        Me.tb_SessionID.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(202, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "SessionID"
        '
        'bGetSessionID
        '
        Me.bGetSessionID.Location = New System.Drawing.Point(426, 273)
        Me.bGetSessionID.Name = "bGetSessionID"
        Me.bGetSessionID.Size = New System.Drawing.Size(100, 23)
        Me.bGetSessionID.TabIndex = 6
        Me.bGetSessionID.Text = "Get Session ID"
        Me.bGetSessionID.UseVisualStyleBackColor = True
        '
        'tb_ControlRoot
        '
        Me.tb_ControlRoot.Location = New System.Drawing.Point(14, 6)
        Me.tb_ControlRoot.Name = "tb_ControlRoot"
        Me.tb_ControlRoot.Size = New System.Drawing.Size(142, 20)
        Me.tb_ControlRoot.TabIndex = 7
        Me.tb_ControlRoot.Text = "TESTING PARAMETERS"
        '
        'tb_TransactionGUID
        '
        Me.tb_TransactionGUID.Location = New System.Drawing.Point(189, 6)
        Me.tb_TransactionGUID.Name = "tb_TransactionGUID"
        Me.tb_TransactionGUID.Size = New System.Drawing.Size(160, 20)
        Me.tb_TransactionGUID.TabIndex = 8
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(532, 273)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Exit"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'b_GetCatalog
        '
        Me.b_GetCatalog.Location = New System.Drawing.Point(320, 273)
        Me.b_GetCatalog.Name = "b_GetCatalog"
        Me.b_GetCatalog.Size = New System.Drawing.Size(75, 23)
        Me.b_GetCatalog.TabIndex = 10
        Me.b_GetCatalog.Text = "Get Catalog"
        Me.b_GetCatalog.UseVisualStyleBackColor = True
        '
        'b_synchNames
        '
        Me.b_synchNames.Location = New System.Drawing.Point(189, 273)
        Me.b_synchNames.Name = "b_synchNames"
        Me.b_synchNames.Size = New System.Drawing.Size(91, 23)
        Me.b_synchNames.TabIndex = 11
        Me.b_synchNames.Text = "Synch Names"
        Me.b_synchNames.UseVisualStyleBackColor = True
        '
        'b_SynchOptions
        '
        Me.b_SynchOptions.Location = New System.Drawing.Point(65, 273)
        Me.b_SynchOptions.Name = "b_SynchOptions"
        Me.b_SynchOptions.Size = New System.Drawing.Size(91, 23)
        Me.b_SynchOptions.TabIndex = 12
        Me.b_SynchOptions.Text = "Synch  Options"
        Me.b_SynchOptions.UseVisualStyleBackColor = True
        '
        'b_SetUp
        '
        Me.b_SetUp.Location = New System.Drawing.Point(532, 205)
        Me.b_SetUp.Name = "b_SetUp"
        Me.b_SetUp.Size = New System.Drawing.Size(75, 23)
        Me.b_SetUp.TabIndex = 15
        Me.b_SetUp.Text = "Set Up Fees"
        Me.b_SetUp.UseVisualStyleBackColor = True
        '
        'B_LikeCLR
        '
        Me.B_LikeCLR.Location = New System.Drawing.Point(242, 105)
        Me.B_LikeCLR.Name = "B_LikeCLR"
        Me.B_LikeCLR.Size = New System.Drawing.Size(222, 23)
        Me.B_LikeCLR.TabIndex = 16
        Me.B_LikeCLR.Text = "Like CLR"
        Me.B_LikeCLR.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(376, 205)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 17
        Me.Button2.Text = "Tier Pricing"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(771, 320)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.B_LikeCLR)
        Me.Controls.Add(Me.b_SetUp)
        Me.Controls.Add(Me.b_SynchOptions)
        Me.Controls.Add(Me.b_synchNames)
        Me.Controls.Add(Me.b_GetCatalog)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.tb_TransactionGUID)
        Me.Controls.Add(Me.tb_ControlRoot)
        Me.Controls.Add(Me.bGetSessionID)
        Me.Controls.Add(Me.tb_SessionID)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tb_API_ID)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tb_UserID)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents tb_UserID As TextBox
    Friend WithEvents tb_API_ID As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tb_SessionID As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents bGetSessionID As Button
    Friend WithEvents tb_ControlRoot As TextBox
    Friend WithEvents tb_TransactionGUID As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents b_GetCatalog As Button
    Friend WithEvents b_synchNames As Button
    Friend WithEvents b_SynchOptions As Button
    Friend WithEvents b_SetUp As Button
    Friend WithEvents B_LikeCLR As Button
    Friend WithEvents Button2 As Button
End Class
