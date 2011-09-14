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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.INDDFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.TreePanel = New System.Windows.Forms.Panel
        Me.ContentPanel = New System.Windows.Forms.Panel
        Me.SuspendLayout()
        '
        'INDDFileDialog
        '
        Me.INDDFileDialog.FileName = "OpenFileDialog1"
        '
        'TreePanel
        '
        Me.TreePanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TreePanel.Location = New System.Drawing.Point(8, 4)
        Me.TreePanel.Name = "TreePanel"
        Me.TreePanel.Size = New System.Drawing.Size(243, 467)
        Me.TreePanel.TabIndex = 1
        '
        'ContentPanel
        '
        Me.ContentPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ContentPanel.Location = New System.Drawing.Point(257, 4)
        Me.ContentPanel.Name = "ContentPanel"
        Me.ContentPanel.Size = New System.Drawing.Size(535, 467)
        Me.ContentPanel.TabIndex = 2
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(799, 480)
        Me.Controls.Add(Me.ContentPanel)
        Me.Controls.Add(Me.TreePanel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "AT 1.5 Production Wizard"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents INDDFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TreePanel As System.Windows.Forms.Panel
    Friend WithEvents ContentPanel As System.Windows.Forms.Panel

End Class
