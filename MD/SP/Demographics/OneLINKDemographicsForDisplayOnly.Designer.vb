<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OneLINKDemographicsForDisplayOnly
    Inherits SystemDemographicsForDisplayOnly

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
        Me.gbSystem.SuspendLayout()
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbSystem
        '
        Me.gbSystem.Text = "OneLINK Address"
        '
        'btnUseThisAddr
        '
        Me.btnUseThisAddr.Text = "Use &OneLINK Info"
        '
        'lblSystemError
        '
        Me.lblSystemError.Location = New System.Drawing.Point(235, 197)
        Me.lblSystemError.Text = "Borrower not Found on OneLINK"
        '
        'OneLINKDemographicsForDisplayOnly
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Name = "OneLINKDemographicsForDisplayOnly"
        Me.gbSystem.ResumeLayout(False)
        Me.gbSystem.PerformLayout()
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

End Class
