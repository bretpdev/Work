<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LetterDeliveryMethod
    Inherits FormBase

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LetterDeliveryMethod))
        Me.tabMethods = New System.Windows.Forms.TabControl
        Me.tabLetter = New System.Windows.Forms.TabPage
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabEmail = New System.Windows.Forms.TabPage
        Me.grpUpdateOpt = New System.Windows.Forms.GroupBox
        Me.radDoNotUpdateEmail = New System.Windows.Forms.RadioButton
        Me.radUpdateEmail = New System.Windows.Forms.RadioButton
        Me.txtEmailMessage = New System.Windows.Forms.TextBox
        Me.txtEmailSubject = New System.Windows.Forms.TextBox
        Me.txtEmailConfirmTo = New System.Windows.Forms.TextBox
        Me.txtEmailTo = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.tabFax = New System.Windows.Forms.TabPage
        Me.cmbFaxBU = New System.Windows.Forms.ComboBox
        Me.txtFaxPhoneNum = New System.Windows.Forms.TextBox
        Me.txtFaxSender = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtFaxMessage = New System.Windows.Forms.TextBox
        Me.txtFaxSubject = New System.Windows.Forms.TextBox
        Me.txtFaxAt = New System.Windows.Forms.TextBox
        Me.txtFaxTo = New System.Windows.Forms.TextBox
        Me.txtFaxConfirmFaxNum = New System.Windows.Forms.TextBox
        Me.txtFaxFaxNum = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.tabMethods.SuspendLayout()
        Me.tabLetter.SuspendLayout()
        Me.tabEmail.SuspendLayout()
        Me.grpUpdateOpt.SuspendLayout()
        Me.tabFax.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMethods
        '
        Me.tabMethods.Appearance = System.Windows.Forms.TabAppearance.Buttons
        Me.tabMethods.Controls.Add(Me.tabLetter)
        Me.tabMethods.Controls.Add(Me.tabEmail)
        Me.tabMethods.Controls.Add(Me.tabFax)
        Me.tabMethods.Location = New System.Drawing.Point(2, 3)
        Me.tabMethods.Multiline = True
        Me.tabMethods.Name = "tabMethods"
        Me.tabMethods.SelectedIndex = 0
        Me.tabMethods.Size = New System.Drawing.Size(512, 393)
        Me.tabMethods.TabIndex = 0
        '
        'tabLetter
        '
        Me.tabLetter.Controls.Add(Me.Label2)
        Me.tabLetter.Controls.Add(Me.Label1)
        Me.tabLetter.Location = New System.Drawing.Point(4, 25)
        Me.tabLetter.Name = "tabLetter"
        Me.tabLetter.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLetter.Size = New System.Drawing.Size(504, 364)
        Me.tabLetter.TabIndex = 0
        Me.tabLetter.Text = "Letter"
        Me.tabLetter.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(492, 40)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Send As Letter"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(3, 176)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(498, 57)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "No Further Information Needed"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabEmail
        '
        Me.tabEmail.Controls.Add(Me.grpUpdateOpt)
        Me.tabEmail.Controls.Add(Me.txtEmailMessage)
        Me.tabEmail.Controls.Add(Me.txtEmailSubject)
        Me.tabEmail.Controls.Add(Me.txtEmailConfirmTo)
        Me.tabEmail.Controls.Add(Me.txtEmailTo)
        Me.tabEmail.Controls.Add(Me.Label8)
        Me.tabEmail.Controls.Add(Me.Label7)
        Me.tabEmail.Controls.Add(Me.Label6)
        Me.tabEmail.Controls.Add(Me.Label5)
        Me.tabEmail.Controls.Add(Me.Label3)
        Me.tabEmail.Location = New System.Drawing.Point(4, 25)
        Me.tabEmail.Name = "tabEmail"
        Me.tabEmail.Padding = New System.Windows.Forms.Padding(3)
        Me.tabEmail.Size = New System.Drawing.Size(504, 364)
        Me.tabEmail.TabIndex = 1
        Me.tabEmail.Text = "E-mail"
        Me.tabEmail.UseVisualStyleBackColor = True
        '
        'grpUpdateOpt
        '
        Me.grpUpdateOpt.Controls.Add(Me.radDoNotUpdateEmail)
        Me.grpUpdateOpt.Controls.Add(Me.radUpdateEmail)
        Me.grpUpdateOpt.Location = New System.Drawing.Point(318, 48)
        Me.grpUpdateOpt.Name = "grpUpdateOpt"
        Me.grpUpdateOpt.Size = New System.Drawing.Size(178, 49)
        Me.grpUpdateOpt.TabIndex = 11
        Me.grpUpdateOpt.TabStop = False
        '
        'radDoNotUpdateEmail
        '
        Me.radDoNotUpdateEmail.AutoSize = True
        Me.radDoNotUpdateEmail.Location = New System.Drawing.Point(7, 29)
        Me.radDoNotUpdateEmail.Name = "radDoNotUpdateEmail"
        Me.radDoNotUpdateEmail.Size = New System.Drawing.Size(128, 17)
        Me.radDoNotUpdateEmail.TabIndex = 1
        Me.radDoNotUpdateEmail.TabStop = True
        Me.radDoNotUpdateEmail.Text = "Do Not Update E-mail"
        Me.radDoNotUpdateEmail.UseVisualStyleBackColor = True
        '
        'radUpdateEmail
        '
        Me.radUpdateEmail.AutoSize = True
        Me.radUpdateEmail.Location = New System.Drawing.Point(7, 9)
        Me.radUpdateEmail.Name = "radUpdateEmail"
        Me.radUpdateEmail.Size = New System.Drawing.Size(91, 17)
        Me.radUpdateEmail.TabIndex = 0
        Me.radUpdateEmail.TabStop = True
        Me.radUpdateEmail.Text = "Update E-mail"
        Me.radUpdateEmail.UseVisualStyleBackColor = True
        '
        'txtEmailMessage
        '
        Me.txtEmailMessage.Location = New System.Drawing.Point(82, 120)
        Me.txtEmailMessage.Multiline = True
        Me.txtEmailMessage.Name = "txtEmailMessage"
        Me.txtEmailMessage.Size = New System.Drawing.Size(414, 236)
        Me.txtEmailMessage.TabIndex = 10
        '
        'txtEmailSubject
        '
        Me.txtEmailSubject.Location = New System.Drawing.Point(82, 98)
        Me.txtEmailSubject.Name = "txtEmailSubject"
        Me.txtEmailSubject.Size = New System.Drawing.Size(414, 20)
        Me.txtEmailSubject.TabIndex = 9
        '
        'txtEmailConfirmTo
        '
        Me.txtEmailConfirmTo.Location = New System.Drawing.Point(82, 76)
        Me.txtEmailConfirmTo.Name = "txtEmailConfirmTo"
        Me.txtEmailConfirmTo.Size = New System.Drawing.Size(233, 20)
        Me.txtEmailConfirmTo.TabIndex = 8
        '
        'txtEmailTo
        '
        Me.txtEmailTo.Location = New System.Drawing.Point(82, 54)
        Me.txtEmailTo.Name = "txtEmailTo"
        Me.txtEmailTo.Size = New System.Drawing.Size(233, 20)
        Me.txtEmailTo.TabIndex = 7
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 123)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Message:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 101)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Subject:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 79)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Confirm To:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 57)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(23, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "To:"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(492, 40)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Send As E-Mail"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabFax
        '
        Me.tabFax.Controls.Add(Me.cmbFaxBU)
        Me.tabFax.Controls.Add(Me.txtFaxPhoneNum)
        Me.tabFax.Controls.Add(Me.txtFaxSender)
        Me.tabFax.Controls.Add(Me.Label18)
        Me.tabFax.Controls.Add(Me.txtFaxMessage)
        Me.tabFax.Controls.Add(Me.txtFaxSubject)
        Me.tabFax.Controls.Add(Me.txtFaxAt)
        Me.tabFax.Controls.Add(Me.txtFaxTo)
        Me.tabFax.Controls.Add(Me.txtFaxConfirmFaxNum)
        Me.tabFax.Controls.Add(Me.txtFaxFaxNum)
        Me.tabFax.Controls.Add(Me.Label17)
        Me.tabFax.Controls.Add(Me.Label13)
        Me.tabFax.Controls.Add(Me.Label14)
        Me.tabFax.Controls.Add(Me.Label15)
        Me.tabFax.Controls.Add(Me.Label16)
        Me.tabFax.Controls.Add(Me.Label9)
        Me.tabFax.Controls.Add(Me.Label10)
        Me.tabFax.Controls.Add(Me.Label11)
        Me.tabFax.Controls.Add(Me.Label12)
        Me.tabFax.Controls.Add(Me.Label4)
        Me.tabFax.Location = New System.Drawing.Point(4, 25)
        Me.tabFax.Name = "tabFax"
        Me.tabFax.Padding = New System.Windows.Forms.Padding(3)
        Me.tabFax.Size = New System.Drawing.Size(504, 364)
        Me.tabFax.TabIndex = 2
        Me.tabFax.Text = "Fax"
        Me.tabFax.UseVisualStyleBackColor = True
        '
        'cmbFaxBU
        '
        Me.cmbFaxBU.FormattingEnabled = True
        Me.cmbFaxBU.Location = New System.Drawing.Point(82, 293)
        Me.cmbFaxBU.Name = "cmbFaxBU"
        Me.cmbFaxBU.Size = New System.Drawing.Size(238, 21)
        Me.cmbFaxBU.TabIndex = 25
        '
        'txtFaxPhoneNum
        '
        Me.txtFaxPhoneNum.Location = New System.Drawing.Point(82, 337)
        Me.txtFaxPhoneNum.Name = "txtFaxPhoneNum"
        Me.txtFaxPhoneNum.Size = New System.Drawing.Size(238, 20)
        Me.txtFaxPhoneNum.TabIndex = 24
        '
        'txtFaxSender
        '
        Me.txtFaxSender.Location = New System.Drawing.Point(82, 315)
        Me.txtFaxSender.Name = "txtFaxSender"
        Me.txtFaxSender.Size = New System.Drawing.Size(238, 20)
        Me.txtFaxSender.TabIndex = 23
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.Red
        Me.Label18.Location = New System.Drawing.Point(326, 54)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(170, 82)
        Me.Label18.TabIndex = 22
        Me.Label18.Text = "DO NOT ADD A ""9"" TO THE BEGINNING OF THE FAX NUMBER, AND BE SURE TO ADD A ""1"" IF " & _
            "IT IS LONG DISTANCE."
        '
        'txtFaxMessage
        '
        Me.txtFaxMessage.Location = New System.Drawing.Point(82, 164)
        Me.txtFaxMessage.Multiline = True
        Me.txtFaxMessage.Name = "txtFaxMessage"
        Me.txtFaxMessage.Size = New System.Drawing.Size(414, 128)
        Me.txtFaxMessage.TabIndex = 21
        '
        'txtFaxSubject
        '
        Me.txtFaxSubject.Location = New System.Drawing.Point(82, 142)
        Me.txtFaxSubject.Name = "txtFaxSubject"
        Me.txtFaxSubject.Size = New System.Drawing.Size(414, 20)
        Me.txtFaxSubject.TabIndex = 20
        '
        'txtFaxAt
        '
        Me.txtFaxAt.Location = New System.Drawing.Point(82, 120)
        Me.txtFaxAt.Name = "txtFaxAt"
        Me.txtFaxAt.Size = New System.Drawing.Size(238, 20)
        Me.txtFaxAt.TabIndex = 19
        '
        'txtFaxTo
        '
        Me.txtFaxTo.Location = New System.Drawing.Point(82, 98)
        Me.txtFaxTo.Name = "txtFaxTo"
        Me.txtFaxTo.Size = New System.Drawing.Size(238, 20)
        Me.txtFaxTo.TabIndex = 18
        '
        'txtFaxConfirmFaxNum
        '
        Me.txtFaxConfirmFaxNum.Location = New System.Drawing.Point(82, 76)
        Me.txtFaxConfirmFaxNum.Name = "txtFaxConfirmFaxNum"
        Me.txtFaxConfirmFaxNum.Size = New System.Drawing.Size(238, 20)
        Me.txtFaxConfirmFaxNum.TabIndex = 17
        '
        'txtFaxFaxNum
        '
        Me.txtFaxFaxNum.Location = New System.Drawing.Point(82, 54)
        Me.txtFaxFaxNum.Name = "txtFaxFaxNum"
        Me.txtFaxFaxNum.Size = New System.Drawing.Size(238, 20)
        Me.txtFaxFaxNum.TabIndex = 16
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(7, 340)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(76, 13)
        Me.Label17.TabIndex = 15
        Me.Label17.Text = "Sender Phn #:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(7, 318)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(44, 13)
        Me.Label13.TabIndex = 14
        Me.Label13.Text = "Sender:"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(7, 296)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(74, 13)
        Me.Label14.TabIndex = 13
        Me.Label14.Text = "Business Unit:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(7, 167)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(53, 13)
        Me.Label15.TabIndex = 12
        Me.Label15.Text = "Message:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(7, 145)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(46, 13)
        Me.Label16.TabIndex = 11
        Me.Label16.Text = "Subject:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 123)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(20, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "At:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(7, 101)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(23, 13)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "To:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(7, 79)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(75, 13)
        Me.Label11.TabIndex = 8
        Me.Label11.Text = "Fax # Confirm:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(7, 57)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(37, 13)
        Me.Label12.TabIndex = 7
        Me.Label12.Text = "Fax #:"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(492, 40)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Send As Fax"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(166, 409)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(273, 409)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'LetterDeliveryMethod
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(514, 444)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tabMethods)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "LetterDeliveryMethod"
        Me.Text = "Letter Delivery Method"
        Me.TopMost = True
        Me.tabMethods.ResumeLayout(False)
        Me.tabLetter.ResumeLayout(False)
        Me.tabEmail.ResumeLayout(False)
        Me.tabEmail.PerformLayout()
        Me.grpUpdateOpt.ResumeLayout(False)
        Me.grpUpdateOpt.PerformLayout()
        Me.tabFax.ResumeLayout(False)
        Me.tabFax.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMethods As System.Windows.Forms.TabControl
    Friend WithEvents tabLetter As System.Windows.Forms.TabPage
    Friend WithEvents tabEmail As System.Windows.Forms.TabPage
    Friend WithEvents tabFax As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents grpUpdateOpt As System.Windows.Forms.GroupBox
    Friend WithEvents txtEmailMessage As System.Windows.Forms.TextBox
    Friend WithEvents txtEmailSubject As System.Windows.Forms.TextBox
    Friend WithEvents txtEmailConfirmTo As System.Windows.Forms.TextBox
    Friend WithEvents txtEmailTo As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents radDoNotUpdateEmail As System.Windows.Forms.RadioButton
    Friend WithEvents radUpdateEmail As System.Windows.Forms.RadioButton
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtFaxTo As System.Windows.Forms.TextBox
    Friend WithEvents txtFaxConfirmFaxNum As System.Windows.Forms.TextBox
    Friend WithEvents txtFaxFaxNum As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtFaxPhoneNum As System.Windows.Forms.TextBox
    Friend WithEvents txtFaxSender As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtFaxMessage As System.Windows.Forms.TextBox
    Friend WithEvents txtFaxSubject As System.Windows.Forms.TextBox
    Friend WithEvents txtFaxAt As System.Windows.Forms.TextBox
    Friend WithEvents cmbFaxBU As System.Windows.Forms.ComboBox
End Class
