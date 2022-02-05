<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SystemDemographicsForDisplayOnly
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
        Me.components = New System.ComponentModel.Container()
        Me.gbSystem = New System.Windows.Forms.GroupBox()
        Me.lblOtherPhoneDate = New System.Windows.Forms.Label()
        Me.DemographicsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.lblSystemError = New System.Windows.Forms.Label()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.lblOtherExt = New System.Windows.Forms.Label()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.lblExt = New System.Windows.Forms.Label()
        Me.lblEmailDate = New System.Windows.Forms.Label()
        Me.lblEmailInd = New System.Windows.Forms.Label()
        Me.lblOtherInd = New System.Windows.Forms.Label()
        Me.lblPhoneDate = New System.Windows.Forms.Label()
        Me.lblPhoneInd = New System.Windows.Forms.Label()
        Me.lblAddrDate = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.lblOther = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.lblPhone = New System.Windows.Forms.Label()
        Me.lblAddr1 = New System.Windows.Forms.Label()
        Me.lblAddr2 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.lblAddrInd = New System.Windows.Forms.Label()
        Me.lblState = New System.Windows.Forms.Label()
        Me.lblZip = New System.Windows.Forms.Label()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.btnUseThisAddr = New System.Windows.Forms.Button()
        Me.gbSystem.SuspendLayout()
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbSystem
        '
        Me.gbSystem.Controls.Add(Me.lblOtherPhoneDate)
        Me.gbSystem.Controls.Add(Me.lblSystemError)
        Me.gbSystem.Controls.Add(Me.Label54)
        Me.gbSystem.Controls.Add(Me.lblOtherExt)
        Me.gbSystem.Controls.Add(Me.Label53)
        Me.gbSystem.Controls.Add(Me.lblExt)
        Me.gbSystem.Controls.Add(Me.lblEmailDate)
        Me.gbSystem.Controls.Add(Me.lblEmailInd)
        Me.gbSystem.Controls.Add(Me.lblOtherInd)
        Me.gbSystem.Controls.Add(Me.lblPhoneDate)
        Me.gbSystem.Controls.Add(Me.lblPhoneInd)
        Me.gbSystem.Controls.Add(Me.lblAddrDate)
        Me.gbSystem.Controls.Add(Me.Label43)
        Me.gbSystem.Controls.Add(Me.lblOther)
        Me.gbSystem.Controls.Add(Me.Label8)
        Me.gbSystem.Controls.Add(Me.Label10)
        Me.gbSystem.Controls.Add(Me.Label11)
        Me.gbSystem.Controls.Add(Me.Label13)
        Me.gbSystem.Controls.Add(Me.Label14)
        Me.gbSystem.Controls.Add(Me.Label22)
        Me.gbSystem.Controls.Add(Me.lblPhone)
        Me.gbSystem.Controls.Add(Me.lblAddr1)
        Me.gbSystem.Controls.Add(Me.lblAddr2)
        Me.gbSystem.Controls.Add(Me.Label28)
        Me.gbSystem.Controls.Add(Me.lblCity)
        Me.gbSystem.Controls.Add(Me.Label30)
        Me.gbSystem.Controls.Add(Me.lblAddrInd)
        Me.gbSystem.Controls.Add(Me.lblState)
        Me.gbSystem.Controls.Add(Me.lblZip)
        Me.gbSystem.Controls.Add(Me.lblEmail)
        Me.gbSystem.Controls.Add(Me.btnUseThisAddr)
        Me.gbSystem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.gbSystem.Location = New System.Drawing.Point(3, 3)
        Me.gbSystem.Name = "gbSystem"
        Me.gbSystem.Size = New System.Drawing.Size(456, 232)
        Me.gbSystem.TabIndex = 25
        Me.gbSystem.TabStop = False
        Me.gbSystem.Text = "System Address"
        '
        'lblOtherPhoneDate
        '
        Me.lblOtherPhoneDate.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "OtherPhoneVerificationDate", True))
        Me.lblOtherPhoneDate.Location = New System.Drawing.Point(376, 121)
        Me.lblOtherPhoneDate.Name = "lblOtherPhoneDate"
        Me.lblOtherPhoneDate.Size = New System.Drawing.Size(64, 16)
        Me.lblOtherPhoneDate.TabIndex = 63
        '
        'DemographicsBindingSource
        '
        Me.DemographicsBindingSource.DataSource = GetType(SP.Demographics)
        '
        'lblSystemError
        '
        Me.lblSystemError.Location = New System.Drawing.Point(13, 25)
        Me.lblSystemError.Name = "lblSystemError"
        Me.lblSystemError.Size = New System.Drawing.Size(432, 184)
        Me.lblSystemError.TabIndex = 62
        Me.lblSystemError.Text = "Borrower not Found on System"
        Me.lblSystemError.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblSystemError.Visible = False
        '
        'Label54
        '
        Me.Label54.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.Location = New System.Drawing.Point(160, 120)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(24, 16)
        Me.Label54.TabIndex = 61
        Me.Label54.Text = "Ext:"
        '
        'lblOtherExt
        '
        Me.lblOtherExt.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "OtherPhoneExt", True))
        Me.lblOtherExt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOtherExt.Location = New System.Drawing.Point(192, 120)
        Me.lblOtherExt.Name = "lblOtherExt"
        Me.lblOtherExt.Size = New System.Drawing.Size(32, 16)
        Me.lblOtherExt.TabIndex = 60
        '
        'Label53
        '
        Me.Label53.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label53.Location = New System.Drawing.Point(160, 104)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(24, 16)
        Me.Label53.TabIndex = 59
        Me.Label53.Text = "Ext:"
        '
        'lblExt
        '
        Me.lblExt.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "HomePhoneExt", True))
        Me.lblExt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExt.Location = New System.Drawing.Point(192, 104)
        Me.lblExt.Name = "lblExt"
        Me.lblExt.Size = New System.Drawing.Size(32, 16)
        Me.lblExt.TabIndex = 58
        '
        'lblEmailDate
        '
        Me.lblEmailDate.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "SPEmailVerDt", True))
        Me.lblEmailDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmailDate.Location = New System.Drawing.Point(376, 136)
        Me.lblEmailDate.Name = "lblEmailDate"
        Me.lblEmailDate.Size = New System.Drawing.Size(64, 16)
        Me.lblEmailDate.TabIndex = 57
        '
        'lblEmailInd
        '
        Me.lblEmailInd.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "SPEmailInd", True))
        Me.lblEmailInd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmailInd.Location = New System.Drawing.Point(352, 136)
        Me.lblEmailInd.Name = "lblEmailInd"
        Me.lblEmailInd.Size = New System.Drawing.Size(16, 16)
        Me.lblEmailInd.TabIndex = 56
        '
        'lblOtherInd
        '
        Me.lblOtherInd.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "OtherPhoneValidityIndicator", True))
        Me.lblOtherInd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOtherInd.Location = New System.Drawing.Point(352, 120)
        Me.lblOtherInd.Name = "lblOtherInd"
        Me.lblOtherInd.Size = New System.Drawing.Size(16, 16)
        Me.lblOtherInd.TabIndex = 54
        '
        'lblPhoneDate
        '
        Me.lblPhoneDate.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "HomePhoneVerificationDate", True))
        Me.lblPhoneDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhoneDate.Location = New System.Drawing.Point(376, 104)
        Me.lblPhoneDate.Name = "lblPhoneDate"
        Me.lblPhoneDate.Size = New System.Drawing.Size(64, 16)
        Me.lblPhoneDate.TabIndex = 53
        '
        'lblPhoneInd
        '
        Me.lblPhoneInd.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "HomePhoneValidityIndicator", True))
        Me.lblPhoneInd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhoneInd.Location = New System.Drawing.Point(352, 104)
        Me.lblPhoneInd.Name = "lblPhoneInd"
        Me.lblPhoneInd.Size = New System.Drawing.Size(16, 16)
        Me.lblPhoneInd.TabIndex = 52
        '
        'lblAddrDate
        '
        Me.lblAddrDate.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "SPAddrVerDt", True))
        Me.lblAddrDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddrDate.Location = New System.Drawing.Point(376, 40)
        Me.lblAddrDate.Name = "lblAddrDate"
        Me.lblAddrDate.Size = New System.Drawing.Size(64, 16)
        Me.lblAddrDate.TabIndex = 51
        '
        'Label43
        '
        Me.Label43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(8, 120)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(40, 16)
        Me.Label43.TabIndex = 49
        Me.Label43.Text = "Other:"
        '
        'lblOther
        '
        Me.lblOther.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOther.Location = New System.Drawing.Point(72, 120)
        Me.lblOther.Name = "lblOther"
        Me.lblOther.Size = New System.Drawing.Size(76, 16)
        Me.lblOther.TabIndex = 47
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 40)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 16)
        Me.Label8.TabIndex = 45
        Me.Label8.Text = "Address 1:"
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(8, 104)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(40, 16)
        Me.Label10.TabIndex = 44
        Me.Label10.Text = "Home:"
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(8, 56)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 16)
        Me.Label11.TabIndex = 43
        Me.Label11.Text = "Address 2:"
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(8, 72)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(32, 16)
        Me.Label13.TabIndex = 42
        Me.Label13.Text = "City:"
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(8, 88)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(40, 16)
        Me.Label14.TabIndex = 41
        Me.Label14.Text = "State:"
        '
        'Label22
        '
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(104, 88)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(24, 16)
        Me.Label22.TabIndex = 40
        Me.Label22.Text = "Zip:"
        '
        'lblPhone
        '
        Me.lblPhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhone.Location = New System.Drawing.Point(72, 104)
        Me.lblPhone.Name = "lblPhone"
        Me.lblPhone.Size = New System.Drawing.Size(76, 16)
        Me.lblPhone.TabIndex = 0
        '
        'lblAddr1
        '
        Me.lblAddr1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "Addr1", True))
        Me.lblAddr1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddr1.Location = New System.Drawing.Point(72, 40)
        Me.lblAddr1.Name = "lblAddr1"
        Me.lblAddr1.Size = New System.Drawing.Size(232, 16)
        Me.lblAddr1.TabIndex = 0
        '
        'lblAddr2
        '
        Me.lblAddr2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "Addr2", True))
        Me.lblAddr2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddr2.Location = New System.Drawing.Point(72, 56)
        Me.lblAddr2.Name = "lblAddr2"
        Me.lblAddr2.Size = New System.Drawing.Size(232, 16)
        Me.lblAddr2.TabIndex = 0
        '
        'Label28
        '
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(8, 136)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(40, 16)
        Me.Label28.TabIndex = 34
        Me.Label28.Text = "E-mail:"
        '
        'lblCity
        '
        Me.lblCity.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "City", True))
        Me.lblCity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCity.Location = New System.Drawing.Point(72, 72)
        Me.lblCity.Name = "lblCity"
        Me.lblCity.Size = New System.Drawing.Size(200, 16)
        Me.lblCity.TabIndex = 0
        '
        'Label30
        '
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(344, 16)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(104, 16)
        Me.Label30.TabIndex = 0
        Me.Label30.Text = "Information Validity:"
        '
        'lblAddrInd
        '
        Me.lblAddrInd.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "SPAddrInd", True))
        Me.lblAddrInd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddrInd.Location = New System.Drawing.Point(352, 40)
        Me.lblAddrInd.Name = "lblAddrInd"
        Me.lblAddrInd.Size = New System.Drawing.Size(16, 16)
        Me.lblAddrInd.TabIndex = 0
        '
        'lblState
        '
        Me.lblState.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "State", True))
        Me.lblState.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblState.Location = New System.Drawing.Point(72, 88)
        Me.lblState.Name = "lblState"
        Me.lblState.Size = New System.Drawing.Size(24, 16)
        Me.lblState.TabIndex = 0
        '
        'lblZip
        '
        Me.lblZip.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "Zip", True))
        Me.lblZip.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblZip.Location = New System.Drawing.Point(128, 88)
        Me.lblZip.Name = "lblZip"
        Me.lblZip.Size = New System.Drawing.Size(104, 16)
        Me.lblZip.TabIndex = 0
        '
        'lblEmail
        '
        Me.lblEmail.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "Email", True))
        Me.lblEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmail.Location = New System.Drawing.Point(72, 136)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(272, 24)
        Me.lblEmail.TabIndex = 0
        '
        'btnUseThisAddr
        '
        Me.btnUseThisAddr.Location = New System.Drawing.Point(16, 192)
        Me.btnUseThisAddr.Name = "btnUseThisAddr"
        Me.btnUseThisAddr.Size = New System.Drawing.Size(128, 23)
        Me.btnUseThisAddr.TabIndex = 1
        Me.btnUseThisAddr.Text = "Use System Info"
        '
        'SystemDemographicsForDisplayOnly
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbSystem)
        Me.Name = "SystemDemographicsForDisplayOnly"
        Me.Size = New System.Drawing.Size(465, 241)
        Me.gbSystem.ResumeLayout(False)
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Protected WithEvents Label54 As System.Windows.Forms.Label
    Protected WithEvents lblOtherExt As System.Windows.Forms.Label
    Protected WithEvents Label53 As System.Windows.Forms.Label
    Protected WithEvents lblExt As System.Windows.Forms.Label
    Protected WithEvents lblEmailDate As System.Windows.Forms.Label
    Protected WithEvents lblEmailInd As System.Windows.Forms.Label
    Protected WithEvents lblOtherInd As System.Windows.Forms.Label
    Protected WithEvents lblPhoneDate As System.Windows.Forms.Label
    Protected WithEvents lblPhoneInd As System.Windows.Forms.Label
    Protected WithEvents lblAddrDate As System.Windows.Forms.Label
    Protected WithEvents Label43 As System.Windows.Forms.Label
    Protected WithEvents lblOther As System.Windows.Forms.Label
    Protected WithEvents Label8 As System.Windows.Forms.Label
    Protected WithEvents Label10 As System.Windows.Forms.Label
    Protected WithEvents Label11 As System.Windows.Forms.Label
    Protected WithEvents Label13 As System.Windows.Forms.Label
    Protected WithEvents Label14 As System.Windows.Forms.Label
    Protected WithEvents Label22 As System.Windows.Forms.Label
    Protected WithEvents lblPhone As System.Windows.Forms.Label
    Protected WithEvents lblAddr1 As System.Windows.Forms.Label
    Protected WithEvents lblAddr2 As System.Windows.Forms.Label
    Protected WithEvents Label28 As System.Windows.Forms.Label
    Protected WithEvents lblCity As System.Windows.Forms.Label
    Protected WithEvents Label30 As System.Windows.Forms.Label
    Protected WithEvents lblAddrInd As System.Windows.Forms.Label
    Protected WithEvents lblState As System.Windows.Forms.Label
    Protected WithEvents lblZip As System.Windows.Forms.Label
    Protected WithEvents lblEmail As System.Windows.Forms.Label
    Protected WithEvents gbSystem As System.Windows.Forms.GroupBox
    Protected WithEvents DemographicsBindingSource As System.Windows.Forms.BindingSource
    Public WithEvents btnUseThisAddr As System.Windows.Forms.Button
    Public WithEvents lblSystemError As System.Windows.Forms.Label
    Protected WithEvents lblOtherPhoneDate As System.Windows.Forms.Label

End Class
