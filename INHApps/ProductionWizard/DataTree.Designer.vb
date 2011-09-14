<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DataTree
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.TreeView = New System.Windows.Forms.TreeView
        Me.AliasDD = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.PubDD = New System.Windows.Forms.ComboBox
        Me.EdnDD = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'TreeView
        '
        Me.TreeView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeView.Location = New System.Drawing.Point(5, 87)
        Me.TreeView.Name = "TreeView"
        Me.TreeView.Size = New System.Drawing.Size(200, 247)
        Me.TreeView.TabIndex = 0
        '
        'AliasDD
        '
        Me.AliasDD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AliasDD.FormattingEnabled = True
        Me.AliasDD.Location = New System.Drawing.Point(60, 4)
        Me.AliasDD.MaxDropDownItems = 20
        Me.AliasDD.Name = "AliasDD"
        Me.AliasDD.Size = New System.Drawing.Size(145, 21)
        Me.AliasDD.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Advertiser:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Edition:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 34)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Publication:"
        '
        'PubDD
        '
        Me.PubDD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PubDD.FormattingEnabled = True
        Me.PubDD.Location = New System.Drawing.Point(60, 31)
        Me.PubDD.MaxDropDownItems = 20
        Me.PubDD.Name = "PubDD"
        Me.PubDD.Size = New System.Drawing.Size(145, 21)
        Me.PubDD.TabIndex = 5
        '
        'EdnDD
        '
        Me.EdnDD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EdnDD.FormattingEnabled = True
        Me.EdnDD.Location = New System.Drawing.Point(60, 59)
        Me.EdnDD.MaxDropDownItems = 20
        Me.EdnDD.Name = "EdnDD"
        Me.EdnDD.Size = New System.Drawing.Size(145, 21)
        Me.EdnDD.TabIndex = 6
        '
        'DataTree
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.EdnDD)
        Me.Controls.Add(Me.PubDD)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.AliasDD)
        Me.Controls.Add(Me.TreeView)
        Me.Name = "DataTree"
        Me.Size = New System.Drawing.Size(211, 344)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TreeView As System.Windows.Forms.TreeView
    Friend WithEvents AliasDD As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PubDD As System.Windows.Forms.ComboBox
    Friend WithEvents EdnDD As System.Windows.Forms.ComboBox

End Class
