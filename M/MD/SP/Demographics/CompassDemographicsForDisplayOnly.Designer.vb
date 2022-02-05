<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CompassDemographicsForDisplayOnly
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
        Me.lblWorkExt = New System.Windows.Forms.Label
        Me.Label60 = New System.Windows.Forms.Label
        Me.lblCWorkDate = New System.Windows.Forms.Label
        Me.lblCWorkInd = New System.Windows.Forms.Label
        Me.Label49 = New System.Windows.Forms.Label
        Me.lblWork = New System.Windows.Forms.Label
        Me.lblOther2MBL = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.lblOther2Consent = New System.Windows.Forms.Label
        Me.gbSystem.SuspendLayout()
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblEmailDate
        '
        Me.lblEmailDate.Location = New System.Drawing.Point(376, 152)
        '
        'lblEmailInd
        '
        Me.lblEmailInd.Location = New System.Drawing.Point(352, 152)
        '
        'Label28
        '
        Me.Label28.Location = New System.Drawing.Point(8, 152)
        '
        'lblEmail
        '
        Me.lblEmail.Location = New System.Drawing.Point(72, 152)
        '
        'gbSystem
        '
        Me.gbSystem.Controls.Add(Me.lblOther2Consent)
        Me.gbSystem.Controls.Add(Me.Label4)
        Me.gbSystem.Controls.Add(Me.Label5)
        Me.gbSystem.Controls.Add(Me.Label6)
        Me.gbSystem.Controls.Add(Me.Label7)
        Me.gbSystem.Controls.Add(Me.Label9)
        Me.gbSystem.Controls.Add(Me.Label12)
        Me.gbSystem.Controls.Add(Me.lblWorkExt)
        Me.gbSystem.Controls.Add(Me.Label60)
        Me.gbSystem.Controls.Add(Me.lblCWorkDate)
        Me.gbSystem.Controls.Add(Me.lblCWorkInd)
        Me.gbSystem.Controls.Add(Me.Label49)
        Me.gbSystem.Controls.Add(Me.lblWork)
        Me.gbSystem.Controls.Add(Me.lblOther2MBL)
        Me.gbSystem.Text = "Compass Address"
        Me.gbSystem.Controls.SetChildIndex(Me.lblOtherPhoneDate, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblSystemError, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblOther2MBL, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblWork, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label49, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblCWorkInd, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblCWorkDate, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label60, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblWorkExt, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label12, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label9, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label7, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label6, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label5, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label4, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblOther2Consent, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblEmailDate, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblEmailInd, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label28, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblEmail, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblAddr2, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.btnUseThisAddr, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblZip, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblState, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblAddrInd, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label30, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblCity, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblAddr1, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblPhone, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label22, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label14, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label13, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label11, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label10, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label8, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblOther, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label43, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblAddrDate, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblPhoneInd, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblPhoneDate, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblOtherInd, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblExt, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label53, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.lblOtherExt, 0)
        Me.gbSystem.Controls.SetChildIndex(Me.Label54, 0)
        '
        'btnUseThisAddr
        '
        Me.btnUseThisAddr.Text = "Use &Compass Info"
        '
        'lblSystemError
        '
        Me.lblSystemError.Location = New System.Drawing.Point(13, 25)
        Me.lblSystemError.Text = "Borrower not Found on Compass"
        '
        'lblWorkExt
        '
        Me.lblWorkExt.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "OtherPhone2Ext", True))
        Me.lblWorkExt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWorkExt.Location = New System.Drawing.Point(192, 136)
        Me.lblWorkExt.Name = "lblWorkExt"
        Me.lblWorkExt.Size = New System.Drawing.Size(32, 16)
        Me.lblWorkExt.TabIndex = 90
        '
        'Label60
        '
        Me.Label60.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label60.Location = New System.Drawing.Point(160, 136)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(24, 16)
        Me.Label60.TabIndex = 89
        Me.Label60.Text = "Ext:"
        '
        'lblCWorkDate
        '
        Me.lblCWorkDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCWorkDate.Location = New System.Drawing.Point(376, 136)
        Me.lblCWorkDate.Name = "lblCWorkDate"
        Me.lblCWorkDate.Size = New System.Drawing.Size(64, 16)
        Me.lblCWorkDate.TabIndex = 88
        '
        'lblCWorkInd
        '
        Me.lblCWorkInd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCWorkInd.Location = New System.Drawing.Point(352, 136)
        Me.lblCWorkInd.Name = "lblCWorkInd"
        Me.lblCWorkInd.Size = New System.Drawing.Size(16, 16)
        Me.lblCWorkInd.TabIndex = 87
        '
        'Label49
        '
        Me.Label49.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(8, 136)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(40, 16)
        Me.Label49.TabIndex = 86
        Me.Label49.Text = "Work:"
        '
        'lblWork
        '
        Me.lblWork.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWork.Location = New System.Drawing.Point(72, 136)
        Me.lblWork.Name = "lblWork"
        Me.lblWork.Size = New System.Drawing.Size(80, 16)
        Me.lblWork.TabIndex = 85
        '
        'lblOther2MBL
        '
        Me.lblOther2MBL.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "OtherPhone2MBL", True))
        Me.lblOther2MBL.Location = New System.Drawing.Point(234, 135)
        Me.lblOther2MBL.Name = "lblOther2MBL"
        Me.lblOther2MBL.Size = New System.Drawing.Size(20, 16)
        Me.lblOther2MBL.TabIndex = 91
        '
        'Label4
        '
        Me.Label4.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "OtherPhoneConsent", True))
        Me.Label4.Location = New System.Drawing.Point(273, 119)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(20, 16)
        Me.Label4.TabIndex = 97
        '
        'Label5
        '
        Me.Label5.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "HomePhoneConsent", True))
        Me.Label5.Location = New System.Drawing.Point(273, 105)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(20, 16)
        Me.Label5.TabIndex = 96
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(270, 88)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 17)
        Me.Label6.TabIndex = 95
        Me.Label6.Text = "Consent:"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(217, 88)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 13)
        Me.Label7.TabIndex = 94
        Me.Label7.Text = "Ph Type:"
        '
        'Label9
        '
        Me.Label9.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "OtherPhoneMBL", True))
        Me.Label9.Location = New System.Drawing.Point(234, 120)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(20, 13)
        Me.Label9.TabIndex = 93
        '
        'Label12
        '
        Me.Label12.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "HomePhoneMBL", True))
        Me.Label12.Location = New System.Drawing.Point(234, 104)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(20, 13)
        Me.Label12.TabIndex = 92
        '
        'lblOther2Consent
        '
        Me.lblOther2Consent.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "OtherPhone2Consent", True))
        Me.lblOther2Consent.Location = New System.Drawing.Point(273, 135)
        Me.lblOther2Consent.Name = "lblOther2Consent"
        Me.lblOther2Consent.Size = New System.Drawing.Size(20, 16)
        Me.lblOther2Consent.TabIndex = 100
        '
        'CompassDemographicsForDisplayOnly
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Name = "CompassDemographicsForDisplayOnly"
        Me.Size = New System.Drawing.Size(463, 239)
        Me.gbSystem.ResumeLayout(False)
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblWorkExt As System.Windows.Forms.Label
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents lblCWorkDate As System.Windows.Forms.Label
    Friend WithEvents lblCWorkInd As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents lblWork As System.Windows.Forms.Label
    Friend WithEvents lblOther2MBL As System.Windows.Forms.Label
    Protected WithEvents Label4 As System.Windows.Forms.Label
    Protected WithEvents Label5 As System.Windows.Forms.Label
    Protected WithEvents Label6 As System.Windows.Forms.Label
    Protected WithEvents Label7 As System.Windows.Forms.Label
    Protected WithEvents Label9 As System.Windows.Forms.Label
    Protected WithEvents Label12 As System.Windows.Forms.Label
    Protected WithEvents lblOther2Consent As System.Windows.Forms.Label

End Class
