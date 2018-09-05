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
        Me.tb_API_ID = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tb_UserID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.bLogin = New System.Windows.Forms.Button()
        Me.tb_SessionID = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.bBasicCatalog = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tb_API_ID
        '
        Me.tb_API_ID.Location = New System.Drawing.Point(85, 55)
        Me.tb_API_ID.Name = "tb_API_ID"
        Me.tb_API_ID.Size = New System.Drawing.Size(100, 20)
        Me.tb_API_ID.TabIndex = 7
        Me.tb_API_ID.Text = "sophie"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "API _ID"
        '
        'tb_UserID
        '
        Me.tb_UserID.Location = New System.Drawing.Point(85, 21)
        Me.tb_UserID.Name = "tb_UserID"
        Me.tb_UserID.Size = New System.Drawing.Size(100, 20)
        Me.tb_UserID.TabIndex = 5
        Me.tb_UserID.Text = "jeromeb"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "UserID"
        '
        'bLogin
        '
        Me.bLogin.Location = New System.Drawing.Point(48, 377)
        Me.bLogin.Name = "bLogin"
        Me.bLogin.Size = New System.Drawing.Size(75, 23)
        Me.bLogin.TabIndex = 8
        Me.bLogin.Text = "Login"
        Me.bLogin.UseVisualStyleBackColor = True
        '
        'tb_SessionID
        '
        Me.tb_SessionID.Location = New System.Drawing.Point(336, 58)
        Me.tb_SessionID.Name = "tb_SessionID"
        Me.tb_SessionID.Size = New System.Drawing.Size(301, 20)
        Me.tb_SessionID.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(272, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "SessionID"
        '
        'bBasicCatalog
        '
        Me.bBasicCatalog.Location = New System.Drawing.Point(171, 377)
        Me.bBasicCatalog.Name = "bBasicCatalog"
        Me.bBasicCatalog.Size = New System.Drawing.Size(102, 23)
        Me.bBasicCatalog.TabIndex = 11
        Me.bBasicCatalog.Text = "Basic Catalog"
        Me.bBasicCatalog.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.bBasicCatalog)
        Me.Controls.Add(Me.tb_SessionID)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.bLogin)
        Me.Controls.Add(Me.tb_API_ID)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tb_UserID)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Harness for Product QA"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tb_API_ID As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tb_UserID As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents bLogin As Button
    Friend WithEvents tb_SessionID As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents bBasicCatalog As Button
End Class
