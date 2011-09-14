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
        Me.PubDD = New System.Windows.Forms.ComboBox
        Me.EditionDD = New System.Windows.Forms.ComboBox
        Me.INDDFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.INDDFilenameBox = New System.Windows.Forms.TextBox
        Me.btnChooseINDD = New System.Windows.Forms.Button
        Me.btnRun = New System.Windows.Forms.Button
        Me.StatusBox = New System.Windows.Forms.ListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.StatusDD = New System.Windows.Forms.ComboBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.DumpProgress = New System.Windows.Forms.ProgressBar
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'PubDD
        '
        Me.PubDD.FormattingEnabled = True
        Me.PubDD.Location = New System.Drawing.Point(6, 32)
        Me.PubDD.Name = "PubDD"
        Me.PubDD.Size = New System.Drawing.Size(200, 21)
        Me.PubDD.TabIndex = 0
        '
        'EditionDD
        '
        Me.EditionDD.FormattingEnabled = True
        Me.EditionDD.Location = New System.Drawing.Point(6, 59)
        Me.EditionDD.Name = "EditionDD"
        Me.EditionDD.Size = New System.Drawing.Size(200, 21)
        Me.EditionDD.TabIndex = 1
        '
        'INDDFileDialog
        '
        Me.INDDFileDialog.FileName = "OpenFileDialog1"
        '
        'INDDFilenameBox
        '
        Me.INDDFilenameBox.Location = New System.Drawing.Point(6, 44)
        Me.INDDFilenameBox.Name = "INDDFilenameBox"
        Me.INDDFilenameBox.ReadOnly = True
        Me.INDDFilenameBox.Size = New System.Drawing.Size(200, 20)
        Me.INDDFilenameBox.TabIndex = 5
        '
        'btnChooseINDD
        '
        Me.btnChooseINDD.Location = New System.Drawing.Point(6, 19)
        Me.btnChooseINDD.Name = "btnChooseINDD"
        Me.btnChooseINDD.Size = New System.Drawing.Size(78, 24)
        Me.btnChooseINDD.TabIndex = 8
        Me.btnChooseINDD.Text = "Choose..."
        Me.btnChooseINDD.UseVisualStyleBackColor = True
        '
        'btnRun
        '
        Me.btnRun.Location = New System.Drawing.Point(6, 19)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(78, 24)
        Me.btnRun.TabIndex = 9
        Me.btnRun.Text = "RUN"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'StatusBox
        '
        Me.StatusBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusBox.FormattingEnabled = True
        Me.StatusBox.Location = New System.Drawing.Point(6, 64)
        Me.StatusBox.Name = "StatusBox"
        Me.StatusBox.Size = New System.Drawing.Size(200, 108)
        Me.StatusBox.TabIndex = 10
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.StatusDD)
        Me.GroupBox1.Controls.Add(Me.EditionDD)
        Me.GroupBox1.Controls.Add(Me.PubDD)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(216, 121)
        Me.GroupBox1.TabIndex = 11
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select Publication, Edition and Status"
        '
        'StatusDD
        '
        Me.StatusDD.FormattingEnabled = True
        Me.StatusDD.Location = New System.Drawing.Point(6, 86)
        Me.StatusDD.Name = "StatusDD"
        Me.StatusDD.Size = New System.Drawing.Size(200, 21)
        Me.StatusDD.TabIndex = 2
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnChooseINDD)
        Me.GroupBox3.Controls.Add(Me.INDDFilenameBox)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 139)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(216, 72)
        Me.GroupBox3.TabIndex = 13
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Select INDD Template"
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.DumpProgress)
        Me.GroupBox4.Controls.Add(Me.btnRun)
        Me.GroupBox4.Controls.Add(Me.StatusBox)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 217)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(216, 175)
        Me.GroupBox4.TabIndex = 13
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Run Process"
        '
        'DumpProgress
        '
        Me.DumpProgress.Location = New System.Drawing.Point(6, 46)
        Me.DumpProgress.Name = "DumpProgress"
        Me.DumpProgress.Size = New System.Drawing.Size(200, 12)
        Me.DumpProgress.TabIndex = 11
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(237, 401)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "AT 1.5 Classad Paginator"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PubDD As System.Windows.Forms.ComboBox
    Friend WithEvents EditionDD As System.Windows.Forms.ComboBox
    Friend WithEvents INDDFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents INDDFilenameBox As System.Windows.Forms.TextBox
    Friend WithEvents btnChooseINDD As System.Windows.Forms.Button
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents StatusBox As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents DumpProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents StatusDD As System.Windows.Forms.ComboBox

End Class
