<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExceptionForm
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
        Me.lblMessage = New System.Windows.Forms.Label
        Me.btnDetails = New System.Windows.Forms.Button
        Me.btnContinue = New System.Windows.Forms.Button
        Me.btnQuit = New System.Windows.Forms.Button
        Me.txtStackTrace = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Location = New System.Drawing.Point(12, 9)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(184, 13)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "Mr. Snuffleupagus barfed on your lap."
        '
        'btnDetails
        '
        Me.btnDetails.Location = New System.Drawing.Point(12, 41)
        Me.btnDetails.Name = "btnDetails"
        Me.btnDetails.Size = New System.Drawing.Size(75, 23)
        Me.btnDetails.TabIndex = 1
        Me.btnDetails.Text = "View Details"
        Me.btnDetails.UseVisualStyleBackColor = True
        '
        'btnContinue
        '
        Me.btnContinue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnContinue.Location = New System.Drawing.Point(203, 41)
        Me.btnContinue.Name = "btnContinue"
        Me.btnContinue.Size = New System.Drawing.Size(75, 23)
        Me.btnContinue.TabIndex = 2
        Me.btnContinue.Text = "Continue"
        Me.btnContinue.UseVisualStyleBackColor = True
        '
        'btnQuit
        '
        Me.btnQuit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnQuit.Location = New System.Drawing.Point(308, 41)
        Me.btnQuit.Name = "btnQuit"
        Me.btnQuit.Size = New System.Drawing.Size(75, 23)
        Me.btnQuit.TabIndex = 3
        Me.btnQuit.Text = "Quit"
        Me.btnQuit.UseVisualStyleBackColor = True
        '
        'txtStackTrace
        '
        Me.txtStackTrace.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStackTrace.Location = New System.Drawing.Point(0, 75)
        Me.txtStackTrace.Multiline = True
        Me.txtStackTrace.Name = "txtStackTrace"
        Me.txtStackTrace.ReadOnly = True
        Me.txtStackTrace.Size = New System.Drawing.Size(395, 0)
        Me.txtStackTrace.TabIndex = 4
        '
        'ExceptionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(395, 74)
        Me.Controls.Add(Me.txtStackTrace)
        Me.Controls.Add(Me.btnQuit)
        Me.Controls.Add(Me.btnContinue)
        Me.Controls.Add(Me.btnDetails)
        Me.Controls.Add(Me.lblMessage)
        Me.Name = "ExceptionForm"
        Me.Text = "Internal Error"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents btnDetails As System.Windows.Forms.Button
    Friend WithEvents btnContinue As System.Windows.Forms.Button
    Friend WithEvents btnQuit As System.Windows.Forms.Button
    Friend WithEvents txtStackTrace As System.Windows.Forms.TextBox
End Class
