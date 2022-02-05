<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AuthorizedThirdPartyControl
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
        Me.components = New System.ComponentModel.Container
        Me.txtDemographicsName = New System.Windows.Forms.TextBox
        Me.AuthorizedThirdPartyBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.txtDemographicsStreetAddress2 = New System.Windows.Forms.TextBox
        Me.txtDemographicsStreetAddress1 = New System.Windows.Forms.TextBox
        Me.lblDemographicsStreetAddress2 = New System.Windows.Forms.Label
        Me.lblDemographicsStreetAddress1 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblDemographicsCountry = New System.Windows.Forms.Label
        Me.txtDemographicsCountry = New System.Windows.Forms.TextBox
        Me.chkDemographicsValidAddress = New System.Windows.Forms.CheckBox
        Me.txtDemographicsZip = New System.Windows.Forms.TextBox
        Me.cmbDemographicsState = New System.Windows.Forms.ComboBox
        Me.txtDemographicsCity = New System.Windows.Forms.TextBox
        Me.lblDemographicsCityStateZip = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        CType(Me.AuthorizedThirdPartyBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDemographicsName
        '
        Me.txtDemographicsName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AuthorizedThirdPartyBindingSource, "Name", True))
        Me.txtDemographicsName.Location = New System.Drawing.Point(98, 12)
        Me.txtDemographicsName.MaxLength = 45
        Me.txtDemographicsName.Name = "txtDemographicsName"
        Me.txtDemographicsName.Size = New System.Drawing.Size(326, 20)
        Me.txtDemographicsName.TabIndex = 0
        '
        'AuthorizedThirdPartyBindingSource
        '
        Me.AuthorizedThirdPartyBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.AuthorizedThirdParty)
        '
        'txtDemographicsStreetAddress2
        '
        Me.txtDemographicsStreetAddress2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AuthorizedThirdPartyBindingSource, "Address2", True))
        Me.txtDemographicsStreetAddress2.Location = New System.Drawing.Point(98, 65)
        Me.txtDemographicsStreetAddress2.MaxLength = 35
        Me.txtDemographicsStreetAddress2.Name = "txtDemographicsStreetAddress2"
        Me.txtDemographicsStreetAddress2.Size = New System.Drawing.Size(326, 20)
        Me.txtDemographicsStreetAddress2.TabIndex = 15
        '
        'txtDemographicsStreetAddress1
        '
        Me.txtDemographicsStreetAddress1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AuthorizedThirdPartyBindingSource, "Address1", True))
        Me.txtDemographicsStreetAddress1.Location = New System.Drawing.Point(98, 39)
        Me.txtDemographicsStreetAddress1.MaxLength = 35
        Me.txtDemographicsStreetAddress1.Name = "txtDemographicsStreetAddress1"
        Me.txtDemographicsStreetAddress1.Size = New System.Drawing.Size(326, 20)
        Me.txtDemographicsStreetAddress1.TabIndex = 14
        '
        'lblDemographicsStreetAddress2
        '
        Me.lblDemographicsStreetAddress2.AutoSize = True
        Me.lblDemographicsStreetAddress2.Location = New System.Drawing.Point(7, 68)
        Me.lblDemographicsStreetAddress2.Name = "lblDemographicsStreetAddress2"
        Me.lblDemographicsStreetAddress2.Size = New System.Drawing.Size(85, 13)
        Me.lblDemographicsStreetAddress2.TabIndex = 13
        Me.lblDemographicsStreetAddress2.Text = "Street Address 2"
        '
        'lblDemographicsStreetAddress1
        '
        Me.lblDemographicsStreetAddress1.AutoSize = True
        Me.lblDemographicsStreetAddress1.Location = New System.Drawing.Point(7, 42)
        Me.lblDemographicsStreetAddress1.Name = "lblDemographicsStreetAddress1"
        Me.lblDemographicsStreetAddress1.Size = New System.Drawing.Size(85, 13)
        Me.lblDemographicsStreetAddress1.TabIndex = 12
        Me.lblDemographicsStreetAddress1.Text = "Street Address 1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Name"
        '
        'lblDemographicsCountry
        '
        Me.lblDemographicsCountry.AutoSize = True
        Me.lblDemographicsCountry.Location = New System.Drawing.Point(185, 122)
        Me.lblDemographicsCountry.Name = "lblDemographicsCountry"
        Me.lblDemographicsCountry.Size = New System.Drawing.Size(43, 13)
        Me.lblDemographicsCountry.TabIndex = 31
        Me.lblDemographicsCountry.Text = "Country"
        '
        'txtDemographicsCountry
        '
        Me.txtDemographicsCountry.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AuthorizedThirdPartyBindingSource, "Country", True))
        Me.txtDemographicsCountry.Location = New System.Drawing.Point(234, 119)
        Me.txtDemographicsCountry.MaxLength = 25
        Me.txtDemographicsCountry.Name = "txtDemographicsCountry"
        Me.txtDemographicsCountry.Size = New System.Drawing.Size(190, 20)
        Me.txtDemographicsCountry.TabIndex = 30
        '
        'chkDemographicsValidAddress
        '
        Me.chkDemographicsValidAddress.AutoSize = True
        Me.chkDemographicsValidAddress.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.AuthorizedThirdPartyBindingSource, "IsValid", True))
        Me.chkDemographicsValidAddress.Location = New System.Drawing.Point(10, 122)
        Me.chkDemographicsValidAddress.Name = "chkDemographicsValidAddress"
        Me.chkDemographicsValidAddress.Size = New System.Drawing.Size(90, 17)
        Me.chkDemographicsValidAddress.TabIndex = 29
        Me.chkDemographicsValidAddress.Text = "Valid Address"
        Me.chkDemographicsValidAddress.UseVisualStyleBackColor = True
        '
        'txtDemographicsZip
        '
        Me.txtDemographicsZip.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AuthorizedThirdPartyBindingSource, "Zip", True))
        Me.txtDemographicsZip.Location = New System.Drawing.Point(342, 93)
        Me.txtDemographicsZip.MaxLength = 9
        Me.txtDemographicsZip.Name = "txtDemographicsZip"
        Me.txtDemographicsZip.Size = New System.Drawing.Size(82, 20)
        Me.txtDemographicsZip.TabIndex = 28
        '
        'cmbDemographicsState
        '
        Me.cmbDemographicsState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbDemographicsState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbDemographicsState.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AuthorizedThirdPartyBindingSource, "State", True))
        Me.cmbDemographicsState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDemographicsState.FormattingEnabled = True
        Me.cmbDemographicsState.Location = New System.Drawing.Point(294, 92)
        Me.cmbDemographicsState.Name = "cmbDemographicsState"
        Me.cmbDemographicsState.Size = New System.Drawing.Size(42, 21)
        Me.cmbDemographicsState.Sorted = True
        Me.cmbDemographicsState.TabIndex = 27
        '
        'txtDemographicsCity
        '
        Me.txtDemographicsCity.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AuthorizedThirdPartyBindingSource, "City", True))
        Me.txtDemographicsCity.Location = New System.Drawing.Point(98, 93)
        Me.txtDemographicsCity.MaxLength = 20
        Me.txtDemographicsCity.Name = "txtDemographicsCity"
        Me.txtDemographicsCity.Size = New System.Drawing.Size(190, 20)
        Me.txtDemographicsCity.TabIndex = 26
        '
        'lblDemographicsCityStateZip
        '
        Me.lblDemographicsCityStateZip.AutoSize = True
        Me.lblDemographicsCityStateZip.Location = New System.Drawing.Point(7, 96)
        Me.lblDemographicsCityStateZip.Name = "lblDemographicsCityStateZip"
        Me.lblDemographicsCityStateZip.Size = New System.Drawing.Size(76, 13)
        Me.lblDemographicsCityStateZip.TabIndex = 25
        Me.lblDemographicsCityStateZip.Text = "City, State, Zip"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtDemographicsName)
        Me.GroupBox1.Controls.Add(Me.lblDemographicsCountry)
        Me.GroupBox1.Controls.Add(Me.lblDemographicsStreetAddress1)
        Me.GroupBox1.Controls.Add(Me.txtDemographicsCountry)
        Me.GroupBox1.Controls.Add(Me.lblDemographicsStreetAddress2)
        Me.GroupBox1.Controls.Add(Me.chkDemographicsValidAddress)
        Me.GroupBox1.Controls.Add(Me.txtDemographicsStreetAddress1)
        Me.GroupBox1.Controls.Add(Me.txtDemographicsZip)
        Me.GroupBox1.Controls.Add(Me.txtDemographicsStreetAddress2)
        Me.GroupBox1.Controls.Add(Me.cmbDemographicsState)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtDemographicsCity)
        Me.GroupBox1.Controls.Add(Me.lblDemographicsCityStateZip)
        Me.GroupBox1.Location = New System.Drawing.Point(3, -3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(429, 146)
        Me.GroupBox1.TabIndex = 32
        Me.GroupBox1.TabStop = False
        '
        'AuthorizedThirdPartyControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "AuthorizedThirdPartyControl"
        Me.Size = New System.Drawing.Size(435, 147)
        CType(Me.AuthorizedThirdPartyBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtDemographicsName As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsStreetAddress2 As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsStreetAddress1 As System.Windows.Forms.TextBox
    Friend WithEvents lblDemographicsStreetAddress2 As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsStreetAddress1 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsCountry As System.Windows.Forms.Label
    Friend WithEvents txtDemographicsCountry As System.Windows.Forms.TextBox
    Friend WithEvents chkDemographicsValidAddress As System.Windows.Forms.CheckBox
    Friend WithEvents txtDemographicsZip As System.Windows.Forms.TextBox
    Friend WithEvents cmbDemographicsState As System.Windows.Forms.ComboBox
    Friend WithEvents txtDemographicsCity As System.Windows.Forms.TextBox
    Friend WithEvents lblDemographicsCityStateZip As System.Windows.Forms.Label
    Friend WithEvents AuthorizedThirdPartyBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox

End Class
