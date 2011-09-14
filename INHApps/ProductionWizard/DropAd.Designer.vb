<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DropAd
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.ErrorBox = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(79, 150)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(305, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Drag and Drop ad from browser Pricing screen"
        '
        'ErrorBox
        '
        Me.ErrorBox.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ErrorBox.ForeColor = System.Drawing.Color.Red
        Me.ErrorBox.Location = New System.Drawing.Point(22, 18)
        Me.ErrorBox.Name = "ErrorBox"
        Me.ErrorBox.ReadOnly = True
        Me.ErrorBox.Size = New System.Drawing.Size(425, 20)
        Me.ErrorBox.TabIndex = 14
        '
        'DropAd
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Beige
        Me.Controls.Add(Me.ErrorBox)
        Me.Controls.Add(Me.Label1)
        Me.Name = "DropAd"
        Me.Size = New System.Drawing.Size(619, 516)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ErrorBox As System.Windows.Forms.TextBox

End Class
