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
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.btnChooseFile = New System.Windows.Forms.Button
        Me.FilenameBox = New System.Windows.Forms.TextBox
        Me.BillFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.btnBill = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.msgBox = New System.Windows.Forms.TextBox
        Me.detailCheck = New System.Windows.Forms.CheckBox
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnChooseFile)
        Me.GroupBox3.Controls.Add(Me.FilenameBox)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(216, 72)
        Me.GroupBox3.TabIndex = 14
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Select Bill File"
        '
        'btnChooseFile
        '
        Me.btnChooseFile.Location = New System.Drawing.Point(6, 19)
        Me.btnChooseFile.Name = "btnChooseFile"
        Me.btnChooseFile.Size = New System.Drawing.Size(78, 24)
        Me.btnChooseFile.TabIndex = 8
        Me.btnChooseFile.Text = "Choose..."
        Me.btnChooseFile.UseVisualStyleBackColor = True
        '
        'FilenameBox
        '
        Me.FilenameBox.Location = New System.Drawing.Point(6, 44)
        Me.FilenameBox.Name = "FilenameBox"
        Me.FilenameBox.ReadOnly = True
        Me.FilenameBox.Size = New System.Drawing.Size(200, 20)
        Me.FilenameBox.TabIndex = 5
        '
        'btnBill
        '
        Me.btnBill.Location = New System.Drawing.Point(11, 135)
        Me.btnBill.Name = "btnBill"
        Me.btnBill.Size = New System.Drawing.Size(216, 36)
        Me.btnBill.TabIndex = 15
        Me.btnBill.Text = "Bill Ads"
        Me.btnBill.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(7, 277)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(221, 36)
        Me.btnClear.TabIndex = 16
        Me.btnClear.Text = "CAUTION - RESET BILL STATUS "
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'msgBox
        '
        Me.msgBox.Location = New System.Drawing.Point(11, 191)
        Me.msgBox.Multiline = True
        Me.msgBox.Name = "msgBox"
        Me.msgBox.ReadOnly = True
        Me.msgBox.Size = New System.Drawing.Size(215, 71)
        Me.msgBox.TabIndex = 17
        '
        'detailCheck
        '
        Me.detailCheck.AutoSize = True
        Me.detailCheck.Location = New System.Drawing.Point(12, 102)
        Me.detailCheck.Name = "detailCheck"
        Me.detailCheck.Size = New System.Drawing.Size(167, 17)
        Me.detailCheck.TabIndex = 18
        Me.detailCheck.Text = "Include Ad Instance line items"
        Me.detailCheck.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(246, 327)
        Me.Controls.Add(Me.detailCheck)
        Me.Controls.Add(Me.msgBox)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnBill)
        Me.Controls.Add(Me.GroupBox3)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "AT 1.5 Biller"
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnChooseFile As System.Windows.Forms.Button
    Friend WithEvents FilenameBox As System.Windows.Forms.TextBox
    Friend WithEvents BillFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents btnBill As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents msgBox As System.Windows.Forms.TextBox
    Friend WithEvents detailCheck As System.Windows.Forms.CheckBox

End Class
