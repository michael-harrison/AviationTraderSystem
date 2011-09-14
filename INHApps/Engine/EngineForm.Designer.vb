<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EngineForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EngineForm))
        Me.Facia1 = New ATControls.Facia
        Me.SuspendLayout()
        '
        'Facia1
        '
        Me.Facia1.DetailVisible = False
        Me.Facia1.Location = New System.Drawing.Point(0, 12)
        Me.Facia1.Name = "Facia1"
        Me.Facia1.Size = New System.Drawing.Size(704, 300)
        Me.Facia1.TabIndex = 0
        '
        'EngineForm
        '
        Me.ClientSize = New System.Drawing.Size(712, 362)
        Me.ControlBox = False
        Me.Controls.Add(Me.Facia1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "EngineForm"
        Me.Text = "AT 1.5 Engine"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Facia1 As ATControls.Facia

End Class
