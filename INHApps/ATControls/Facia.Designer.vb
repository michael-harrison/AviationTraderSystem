<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Facia
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As system.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.ButtonBar1 = New ATControls.ButtonBar
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(3, 49)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(695, 212)
        Me.ListBox1.TabIndex = 7
        '
        'ButtonBar1
        '
        Me.ButtonBar1.Location = New System.Drawing.Point(8, 4)
        Me.ButtonBar1.Name = "ButtonBar1"
        Me.ButtonBar1.Size = New System.Drawing.Size(690, 45)
        Me.ButtonBar1.TabIndex = 8
        '
        'Facia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ButtonBar1)
        Me.Controls.Add(Me.ListBox1)
        Me.Name = "Facia"
        Me.Size = New System.Drawing.Size(704, 300)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListBox1 As system.Windows.Forms.ListBox
    Friend WithEvents ButtonBar1 As ATControls.ButtonBar

End Class
