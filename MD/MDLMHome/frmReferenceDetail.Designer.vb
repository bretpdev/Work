<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReferenceDetail
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReferenceDetail))
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbName = New System.Windows.Forms.TextBox
        Me.tbID = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbRelationship = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbThirdPartyAuth = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbStatus = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.tbAddr1 = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.tbAddr2 = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.tbAddrVal = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.tbCity = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.tbState = New System.Windows.Forms.TextBox
        Me.tbZip = New System.Windows.Forms.MaskedTextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.tbPhone = New System.Windows.Forms.MaskedTextBox
        Me.tbPhoneVal = New System.Windows.Forms.TextBox
        Me.tbOPhoneVal = New System.Windows.Forms.TextBox
        Me.tbOPhone = New System.Windows.Forms.MaskedTextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.tbEmail = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(143, 284)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(5, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 18)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Name:"
        '
        'tbName
        '
        Me.tbName.Location = New System.Drawing.Point(118, 34)
        Me.tbName.Name = "tbName"
        Me.tbName.ReadOnly = True
        Me.tbName.Size = New System.Drawing.Size(238, 20)
        Me.tbName.TabIndex = 2
        '
        'tbID
        '
        Me.tbID.Location = New System.Drawing.Point(118, 12)
        Me.tbID.Name = "tbID"
        Me.tbID.ReadOnly = True
        Me.tbID.Size = New System.Drawing.Size(123, 20)
        Me.tbID.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(5, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 18)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "ID:"
        '
        'tbRelationship
        '
        Me.tbRelationship.Location = New System.Drawing.Point(118, 56)
        Me.tbRelationship.Name = "tbRelationship"
        Me.tbRelationship.ReadOnly = True
        Me.tbRelationship.Size = New System.Drawing.Size(123, 20)
        Me.tbRelationship.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(5, 59)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 18)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Relationship:"
        '
        'tbThirdPartyAuth
        '
        Me.tbThirdPartyAuth.Location = New System.Drawing.Point(118, 78)
        Me.tbThirdPartyAuth.Name = "tbThirdPartyAuth"
        Me.tbThirdPartyAuth.ReadOnly = True
        Me.tbThirdPartyAuth.Size = New System.Drawing.Size(33, 20)
        Me.tbThirdPartyAuth.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(5, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(119, 18)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Third Party Auth:"
        '
        'tbStatus
        '
        Me.tbStatus.Location = New System.Drawing.Point(118, 100)
        Me.tbStatus.Name = "tbStatus"
        Me.tbStatus.ReadOnly = True
        Me.tbStatus.Size = New System.Drawing.Size(33, 20)
        Me.tbStatus.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(5, 103)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(119, 18)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Status:"
        '
        'tbAddr1
        '
        Me.tbAddr1.Location = New System.Drawing.Point(118, 122)
        Me.tbAddr1.Name = "tbAddr1"
        Me.tbAddr1.ReadOnly = True
        Me.tbAddr1.Size = New System.Drawing.Size(238, 20)
        Me.tbAddr1.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(5, 125)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(119, 18)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Address #1:"
        '
        'tbAddr2
        '
        Me.tbAddr2.Location = New System.Drawing.Point(118, 144)
        Me.tbAddr2.Name = "tbAddr2"
        Me.tbAddr2.ReadOnly = True
        Me.tbAddr2.Size = New System.Drawing.Size(238, 20)
        Me.tbAddr2.TabIndex = 14
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(5, 147)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(119, 18)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Address #2:"
        '
        'tbAddrVal
        '
        Me.tbAddrVal.Location = New System.Drawing.Point(118, 188)
        Me.tbAddrVal.Name = "tbAddrVal"
        Me.tbAddrVal.ReadOnly = True
        Me.tbAddrVal.Size = New System.Drawing.Size(33, 20)
        Me.tbAddrVal.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(5, 190)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(119, 18)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Address Valid:"
        '
        'tbCity
        '
        Me.tbCity.Location = New System.Drawing.Point(118, 166)
        Me.tbCity.Name = "tbCity"
        Me.tbCity.ReadOnly = True
        Me.tbCity.Size = New System.Drawing.Size(123, 20)
        Me.tbCity.TabIndex = 18
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(5, 169)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(119, 18)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "City/State/Zip:"
        '
        'tbState
        '
        Me.tbState.Location = New System.Drawing.Point(243, 166)
        Me.tbState.Name = "tbState"
        Me.tbState.ReadOnly = True
        Me.tbState.Size = New System.Drawing.Size(41, 20)
        Me.tbState.TabIndex = 20
        '
        'tbZip
        '
        Me.tbZip.Location = New System.Drawing.Point(286, 166)
        Me.tbZip.Mask = "00000-9999"
        Me.tbZip.Name = "tbZip"
        Me.tbZip.ReadOnly = True
        Me.tbZip.Size = New System.Drawing.Size(70, 20)
        Me.tbZip.TabIndex = 21
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(5, 212)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(119, 18)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "Phone/Valid:"
        '
        'tbPhone
        '
        Me.tbPhone.Location = New System.Drawing.Point(118, 210)
        Me.tbPhone.Mask = "(999) 000-0000"
        Me.tbPhone.Name = "tbPhone"
        Me.tbPhone.ReadOnly = True
        Me.tbPhone.Size = New System.Drawing.Size(123, 20)
        Me.tbPhone.TabIndex = 23
        '
        'tbPhoneVal
        '
        Me.tbPhoneVal.Location = New System.Drawing.Point(243, 210)
        Me.tbPhoneVal.Name = "tbPhoneVal"
        Me.tbPhoneVal.ReadOnly = True
        Me.tbPhoneVal.Size = New System.Drawing.Size(33, 20)
        Me.tbPhoneVal.TabIndex = 27
        '
        'tbOPhoneVal
        '
        Me.tbOPhoneVal.Location = New System.Drawing.Point(243, 233)
        Me.tbOPhoneVal.Name = "tbOPhoneVal"
        Me.tbOPhoneVal.ReadOnly = True
        Me.tbOPhoneVal.Size = New System.Drawing.Size(33, 20)
        Me.tbOPhoneVal.TabIndex = 30
        '
        'tbOPhone
        '
        Me.tbOPhone.Location = New System.Drawing.Point(118, 233)
        Me.tbOPhone.Mask = "(999) 000-0000"
        Me.tbOPhone.Name = "tbOPhone"
        Me.tbOPhone.ReadOnly = True
        Me.tbOPhone.Size = New System.Drawing.Size(123, 20)
        Me.tbOPhone.TabIndex = 29
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(5, 235)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(119, 18)
        Me.Label11.TabIndex = 28
        Me.Label11.Text = "Other Phone/Valid:"
        '
        'tbEmail
        '
        Me.tbEmail.Location = New System.Drawing.Point(118, 255)
        Me.tbEmail.Name = "tbEmail"
        Me.tbEmail.ReadOnly = True
        Me.tbEmail.Size = New System.Drawing.Size(238, 20)
        Me.tbEmail.TabIndex = 32
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(5, 258)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(119, 18)
        Me.Label12.TabIndex = 31
        Me.Label12.Text = "Email:"
        '
        'frmReferenceDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(364, 316)
        Me.Controls.Add(Me.tbEmail)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.tbOPhoneVal)
        Me.Controls.Add(Me.tbOPhone)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.tbPhoneVal)
        Me.Controls.Add(Me.tbPhone)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.tbZip)
        Me.Controls.Add(Me.tbState)
        Me.Controls.Add(Me.tbCity)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.tbAddrVal)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.tbAddr2)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbAddr1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.tbStatus)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tbThirdPartyAuth)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbRelationship)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbID)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmReferenceDetail"
        Me.Text = "Reference Detail"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents tbID As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbRelationship As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbThirdPartyAuth As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbAddr1 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tbAddr2 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbAddrVal As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tbCity As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents tbState As System.Windows.Forms.TextBox
    Friend WithEvents tbZip As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbPhone As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbPhoneVal As System.Windows.Forms.TextBox
    Friend WithEvents tbOPhoneVal As System.Windows.Forms.TextBox
    Friend WithEvents tbOPhone As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tbEmail As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
End Class
