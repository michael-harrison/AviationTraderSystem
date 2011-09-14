<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PicItem
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
        Me.PicBox = New System.Windows.Forms.Panel
        Me.btnCheckout = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.mmSizeBox = New System.Windows.Forms.TextBox
        Me.PixelSizeBox = New System.Windows.Forms.TextBox
        Me.ResolutionBox = New System.Windows.Forms.TextBox
        Me.filenameBox = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnRecover = New System.Windows.Forms.Button
        Me.btnCopy = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PicBox
        '
        Me.PicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PicBox.Location = New System.Drawing.Point(7, 4)
        Me.PicBox.Name = "PicBox"
        Me.PicBox.Size = New System.Drawing.Size(206, 155)
        Me.PicBox.TabIndex = 0
        '
        'btnCheckout
        '
        Me.btnCheckout.Location = New System.Drawing.Point(7, 163)
        Me.btnCheckout.Name = "btnCheckout"
        Me.btnCheckout.Size = New System.Drawing.Size(96, 24)
        Me.btnCheckout.TabIndex = 1
        Me.btnCheckout.Text = "Checkout"
        Me.btnCheckout.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.mmSizeBox)
        Me.GroupBox1.Controls.Add(Me.PixelSizeBox)
        Me.GroupBox1.Controls.Add(Me.ResolutionBox)
        Me.GroupBox1.Controls.Add(Me.filenameBox)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(222, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(252, 134)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Source Image Stats"
        '
        'mmSizeBox
        '
        Me.mmSizeBox.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.mmSizeBox.Location = New System.Drawing.Point(107, 104)
        Me.mmSizeBox.Name = "mmSizeBox"
        Me.mmSizeBox.ReadOnly = True
        Me.mmSizeBox.Size = New System.Drawing.Size(139, 20)
        Me.mmSizeBox.TabIndex = 8
        '
        'PixelSizeBox
        '
        Me.PixelSizeBox.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.PixelSizeBox.Location = New System.Drawing.Point(107, 78)
        Me.PixelSizeBox.Name = "PixelSizeBox"
        Me.PixelSizeBox.ReadOnly = True
        Me.PixelSizeBox.Size = New System.Drawing.Size(139, 20)
        Me.PixelSizeBox.TabIndex = 7
        '
        'ResolutionBox
        '
        Me.ResolutionBox.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.ResolutionBox.Location = New System.Drawing.Point(107, 51)
        Me.ResolutionBox.Name = "ResolutionBox"
        Me.ResolutionBox.ReadOnly = True
        Me.ResolutionBox.Size = New System.Drawing.Size(139, 20)
        Me.ResolutionBox.TabIndex = 6
        '
        'filenameBox
        '
        Me.filenameBox.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.filenameBox.Location = New System.Drawing.Point(107, 23)
        Me.filenameBox.Name = "filenameBox"
        Me.filenameBox.ReadOnly = True
        Me.filenameBox.Size = New System.Drawing.Size(139, 20)
        Me.filenameBox.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 54)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Resolution (PPI):"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Size (pixels):"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 107)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Size (mm):"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Filename:"
        '
        'btnRecover
        '
        Me.btnRecover.Location = New System.Drawing.Point(117, 163)
        Me.btnRecover.Name = "btnRecover"
        Me.btnRecover.Size = New System.Drawing.Size(96, 24)
        Me.btnRecover.TabIndex = 3
        Me.btnRecover.Text = "Recover Original"
        Me.btnRecover.UseVisualStyleBackColor = True
        '
        'btnCopy
        '
        Me.btnCopy.Location = New System.Drawing.Point(222, 163)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(96, 24)
        Me.btnCopy.TabIndex = 4
        Me.btnCopy.Text = "Copy Image"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'PicItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.btnCheckout)
        Me.Controls.Add(Me.btnRecover)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PicBox)
        Me.Name = "PicItem"
        Me.Size = New System.Drawing.Size(483, 198)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PicBox As System.Windows.Forms.Panel
    Friend WithEvents btnCheckout As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents mmSizeBox As System.Windows.Forms.TextBox
    Friend WithEvents PixelSizeBox As System.Windows.Forms.TextBox
    Friend WithEvents ResolutionBox As System.Windows.Forms.TextBox
    Friend WithEvents filenameBox As System.Windows.Forms.TextBox
    Friend WithEvents btnRecover As System.Windows.Forms.Button
    Friend WithEvents btnCopy As System.Windows.Forms.Button

End Class
