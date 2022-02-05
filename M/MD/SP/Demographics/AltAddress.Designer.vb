<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AltAddress
    Inherits Address

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
        Me.components = New System.ComponentModel.Container
        Me.DemographicsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cbState
        '
        Me.cbState.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "State", True))
        '
        'txtAddr2
        '
        Me.txtAddr2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "Addr2", True))
        '
        'txtCity
        '
        Me.txtCity.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "City", True))
        '
        'txtZip
        '
        Me.txtZip.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "Zip", True))
        '
        'txtAddr1
        '
        Me.txtAddr1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "Addr1", True))
        '
        'DemographicsBindingSource
        '
        Me.DemographicsBindingSource.DataSource = GetType(SP.Demographics)
        '
        'AltAddress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Name = "AltAddress"
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DemographicsBindingSource As System.Windows.Forms.BindingSource

End Class
