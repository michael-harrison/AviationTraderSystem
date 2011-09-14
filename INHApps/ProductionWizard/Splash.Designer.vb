<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Splash
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ErrorBox = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(65, 123)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(402, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Expand the tree on the left to select your working environment."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(56, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(411, 29)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Aviation Trader Production Wizard"
        '
        'ErrorBox
        '
        Me.ErrorBox.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ErrorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ErrorBox.ForeColor = System.Drawing.Color.Red
        Me.ErrorBox.Location = New System.Drawing.Point(56, 218)
        Me.ErrorBox.Multiline = True
        Me.ErrorBox.Name = "ErrorBox"
        Me.ErrorBox.ReadOnly = True
        Me.ErrorBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ErrorBox.ShortcutsEnabled = False
        Me.ErrorBox.Size = New System.Drawing.Size(398, 209)
        Me.ErrorBox.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(53, 149)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(424, 17)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Use the Advertiser Selector to filter ads for the selected advertiser"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(55, 178)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(421, 17)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Note: Performance is enhanced if you select a specific advertiser."
        '
        'Splash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ErrorBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Splash"
        Me.Size = New System.Drawing.Size(550, 467)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ErrorBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label

End Class
