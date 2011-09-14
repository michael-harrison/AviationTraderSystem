<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ButtonBarButton
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
        Dim resources As system.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ButtonBarButton))
        Me.btn = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btn
        '
        Me.btn.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.btn.FlatAppearance.BorderSize = 0
        Me.btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn.Image = CType(resources.GetObject("btn.Image"), System.Drawing.Image)
        Me.btn.Location = New System.Drawing.Point(0, 0)
        Me.btn.Name = "btn"
        Me.btn.Size = New System.Drawing.Size(117, 36)
        Me.btn.TabIndex = 4
        Me.btn.Text = "Busy"
        Me.btn.UseVisualStyleBackColor = True
        '
        'FaciaButton
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btn)
        Me.Name = "FaciaButton"
        Me.Size = New System.Drawing.Size(117, 36)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn As system.Windows.Forms.Button

End Class
