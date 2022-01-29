<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCSQueueTask
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCSQueueTask))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblScript = New System.Windows.Forms.Label
        Me.lblAddy = New System.Windows.Forms.Label
        Me.lblDrvLic = New System.Windows.Forms.Label
        Me.lblRef1Rel = New System.Windows.Forms.Label
        Me.lblRef1FName = New System.Windows.Forms.Label
        Me.lblRef1Addy1 = New System.Windows.Forms.Label
        Me.lblRef1Phone = New System.Windows.Forms.Label
        Me.lblPhone = New System.Windows.Forms.Label
        Me.lblDOB = New System.Windows.Forms.Label
        Me.lblRef2Rel = New System.Windows.Forms.Label
        Me.lblRef2FName = New System.Windows.Forms.Label
        Me.lblRef2Addy1 = New System.Windows.Forms.Label
        Me.lblRef2Phone = New System.Windows.Forms.Label
        Me.tbxBorAddy = New System.Windows.Forms.TextBox
        Me.tbxBorPhone = New System.Windows.Forms.TextBox
        Me.tbxDrvLic = New System.Windows.Forms.TextBox
        Me.tbxDOB = New System.Windows.Forms.TextBox
        Me.tbxRef1FName = New System.Windows.Forms.TextBox
        Me.tbxRef2FName = New System.Windows.Forms.TextBox
        Me.tbxRef1Addy1 = New System.Windows.Forms.TextBox
        Me.tbxRef2Addy1 = New System.Windows.Forms.TextBox
        Me.tbxRef1Phone = New System.Windows.Forms.TextBox
        Me.tbxRef2Phone = New System.Windows.Forms.TextBox
        Me.cbxRef1Rel = New System.Windows.Forms.ComboBox
        Me.cbxRef2Rel = New System.Windows.Forms.ComboBox
        Me.btnQueue = New System.Windows.Forms.Button
        Me.gbxBor = New System.Windows.Forms.GroupBox
        Me.gbxRef1 = New System.Windows.Forms.GroupBox
        Me.btnSaveRef1 = New System.Windows.Forms.Button
        Me.btnClear1 = New System.Windows.Forms.Button
        Me.tbxRef1Zip = New System.Windows.Forms.TextBox
        Me.lblRef1Zip = New System.Windows.Forms.Label
        Me.tbxRef1State = New System.Windows.Forms.TextBox
        Me.tbxRef1City = New System.Windows.Forms.TextBox
        Me.tbxRef1Addy2 = New System.Windows.Forms.TextBox
        Me.lblRef1Addy2 = New System.Windows.Forms.Label
        Me.lblRef1State = New System.Windows.Forms.Label
        Me.lblRef1City = New System.Windows.Forms.Label
        Me.tbxRef1LName = New System.Windows.Forms.TextBox
        Me.lblRef1LName = New System.Windows.Forms.Label
        Me.mtbxDate = New System.Windows.Forms.MaskedTextBox
        Me.gbxRef2 = New System.Windows.Forms.GroupBox
        Me.btnRef2Update = New System.Windows.Forms.Button
        Me.tbxRef2Zip = New System.Windows.Forms.TextBox
        Me.lblRef2Zip = New System.Windows.Forms.Label
        Me.tbxRef2State = New System.Windows.Forms.TextBox
        Me.tbxRef2City = New System.Windows.Forms.TextBox
        Me.tbxRef2Addy2 = New System.Windows.Forms.TextBox
        Me.lblRef2Addy2 = New System.Windows.Forms.Label
        Me.lblRef2State = New System.Windows.Forms.Label
        Me.lblRef2City = New System.Windows.Forms.Label
        Me.tbxRef2LName = New System.Windows.Forms.TextBox
        Me.lblRef2LName = New System.Windows.Forms.Label
        Me.lblAmt = New System.Windows.Forms.Label
        Me.lblDate = New System.Windows.Forms.Label
        Me.tbxAmt = New System.Windows.Forms.TextBox
        Me.gbxBor.SuspendLayout()
        Me.gbxRef1.SuspendLayout()
        Me.gbxRef2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(208, 576)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(103, 23)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblScript
        '
        Me.lblScript.AutoSize = True
        Me.lblScript.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScript.Location = New System.Drawing.Point(25, 13)
        Me.lblScript.MaximumSize = New System.Drawing.Size(700, 0)
        Me.lblScript.Name = "lblScript"
        Me.lblScript.Size = New System.Drawing.Size(686, 60)
        Me.lblScript.TabIndex = 1
        Me.lblScript.Text = resources.GetString("lblScript.Text")
        '
        'lblAddy
        '
        Me.lblAddy.AutoSize = True
        Me.lblAddy.Location = New System.Drawing.Point(15, 26)
        Me.lblAddy.Name = "lblAddy"
        Me.lblAddy.Size = New System.Drawing.Size(48, 13)
        Me.lblAddy.TabIndex = 2
        Me.lblAddy.Text = "Address:"
        '
        'lblDrvLic
        '
        Me.lblDrvLic.AutoSize = True
        Me.lblDrvLic.Location = New System.Drawing.Point(351, 26)
        Me.lblDrvLic.Name = "lblDrvLic"
        Me.lblDrvLic.Size = New System.Drawing.Size(83, 13)
        Me.lblDrvLic.TabIndex = 3
        Me.lblDrvLic.Text = "Drivers License:"
        '
        'lblRef1Rel
        '
        Me.lblRef1Rel.AutoSize = True
        Me.lblRef1Rel.Location = New System.Drawing.Point(15, 20)
        Me.lblRef1Rel.Name = "lblRef1Rel"
        Me.lblRef1Rel.Size = New System.Drawing.Size(68, 13)
        Me.lblRef1Rel.TabIndex = 4
        Me.lblRef1Rel.Text = "Relationship:"
        '
        'lblRef1FName
        '
        Me.lblRef1FName.AutoSize = True
        Me.lblRef1FName.Location = New System.Drawing.Point(183, 20)
        Me.lblRef1FName.Name = "lblRef1FName"
        Me.lblRef1FName.Size = New System.Drawing.Size(60, 13)
        Me.lblRef1FName.TabIndex = 5
        Me.lblRef1FName.Text = "First Name:"
        '
        'lblRef1Addy1
        '
        Me.lblRef1Addy1.AutoSize = True
        Me.lblRef1Addy1.Location = New System.Drawing.Point(15, 68)
        Me.lblRef1Addy1.Name = "lblRef1Addy1"
        Me.lblRef1Addy1.Size = New System.Drawing.Size(57, 13)
        Me.lblRef1Addy1.TabIndex = 6
        Me.lblRef1Addy1.Text = "Address 1:"
        '
        'lblRef1Phone
        '
        Me.lblRef1Phone.AutoSize = True
        Me.lblRef1Phone.Location = New System.Drawing.Point(519, 20)
        Me.lblRef1Phone.Name = "lblRef1Phone"
        Me.lblRef1Phone.Size = New System.Drawing.Size(41, 13)
        Me.lblRef1Phone.TabIndex = 7
        Me.lblRef1Phone.Text = "Phone:"
        '
        'lblPhone
        '
        Me.lblPhone.AutoSize = True
        Me.lblPhone.Location = New System.Drawing.Point(183, 26)
        Me.lblPhone.Name = "lblPhone"
        Me.lblPhone.Size = New System.Drawing.Size(41, 13)
        Me.lblPhone.TabIndex = 8
        Me.lblPhone.Text = "Phone:"
        '
        'lblDOB
        '
        Me.lblDOB.AutoSize = True
        Me.lblDOB.Location = New System.Drawing.Point(519, 26)
        Me.lblDOB.Name = "lblDOB"
        Me.lblDOB.Size = New System.Drawing.Size(69, 13)
        Me.lblDOB.TabIndex = 9
        Me.lblDOB.Text = "Date of Birth:"
        '
        'lblRef2Rel
        '
        Me.lblRef2Rel.AutoSize = True
        Me.lblRef2Rel.Location = New System.Drawing.Point(15, 23)
        Me.lblRef2Rel.Name = "lblRef2Rel"
        Me.lblRef2Rel.Size = New System.Drawing.Size(68, 13)
        Me.lblRef2Rel.TabIndex = 10
        Me.lblRef2Rel.Text = "Relationship:"
        '
        'lblRef2FName
        '
        Me.lblRef2FName.AutoSize = True
        Me.lblRef2FName.Location = New System.Drawing.Point(183, 23)
        Me.lblRef2FName.Name = "lblRef2FName"
        Me.lblRef2FName.Size = New System.Drawing.Size(60, 13)
        Me.lblRef2FName.TabIndex = 11
        Me.lblRef2FName.Text = "First Name:"
        '
        'lblRef2Addy1
        '
        Me.lblRef2Addy1.AutoSize = True
        Me.lblRef2Addy1.Location = New System.Drawing.Point(15, 80)
        Me.lblRef2Addy1.Name = "lblRef2Addy1"
        Me.lblRef2Addy1.Size = New System.Drawing.Size(57, 13)
        Me.lblRef2Addy1.TabIndex = 12
        Me.lblRef2Addy1.Text = "Address 1:"
        '
        'lblRef2Phone
        '
        Me.lblRef2Phone.AutoSize = True
        Me.lblRef2Phone.Location = New System.Drawing.Point(519, 23)
        Me.lblRef2Phone.Name = "lblRef2Phone"
        Me.lblRef2Phone.Size = New System.Drawing.Size(41, 13)
        Me.lblRef2Phone.TabIndex = 13
        Me.lblRef2Phone.Text = "Phone:"
        '
        'tbxBorAddy
        '
        Me.tbxBorAddy.Location = New System.Drawing.Point(18, 44)
        Me.tbxBorAddy.Name = "tbxBorAddy"
        Me.tbxBorAddy.Size = New System.Drawing.Size(152, 20)
        Me.tbxBorAddy.TabIndex = 1
        '
        'tbxBorPhone
        '
        Me.tbxBorPhone.Location = New System.Drawing.Point(186, 44)
        Me.tbxBorPhone.Name = "tbxBorPhone"
        Me.tbxBorPhone.Size = New System.Drawing.Size(152, 20)
        Me.tbxBorPhone.TabIndex = 2
        '
        'tbxDrvLic
        '
        Me.tbxDrvLic.Location = New System.Drawing.Point(354, 44)
        Me.tbxDrvLic.Name = "tbxDrvLic"
        Me.tbxDrvLic.Size = New System.Drawing.Size(152, 20)
        Me.tbxDrvLic.TabIndex = 3
        '
        'tbxDOB
        '
        Me.tbxDOB.Location = New System.Drawing.Point(522, 44)
        Me.tbxDOB.Name = "tbxDOB"
        Me.tbxDOB.Size = New System.Drawing.Size(152, 20)
        Me.tbxDOB.TabIndex = 4
        '
        'tbxRef1FName
        '
        Me.tbxRef1FName.Location = New System.Drawing.Point(186, 39)
        Me.tbxRef1FName.Name = "tbxRef1FName"
        Me.tbxRef1FName.Size = New System.Drawing.Size(152, 20)
        Me.tbxRef1FName.TabIndex = 6
        '
        'tbxRef2FName
        '
        Me.tbxRef2FName.Location = New System.Drawing.Point(186, 43)
        Me.tbxRef2FName.Name = "tbxRef2FName"
        Me.tbxRef2FName.Size = New System.Drawing.Size(152, 20)
        Me.tbxRef2FName.TabIndex = 17
        '
        'tbxRef1Addy1
        '
        Me.tbxRef1Addy1.Location = New System.Drawing.Point(18, 86)
        Me.tbxRef1Addy1.Name = "tbxRef1Addy1"
        Me.tbxRef1Addy1.Size = New System.Drawing.Size(152, 20)
        Me.tbxRef1Addy1.TabIndex = 9
        '
        'tbxRef2Addy1
        '
        Me.tbxRef2Addy1.Location = New System.Drawing.Point(18, 99)
        Me.tbxRef2Addy1.Name = "tbxRef2Addy1"
        Me.tbxRef2Addy1.Size = New System.Drawing.Size(152, 20)
        Me.tbxRef2Addy1.TabIndex = 20
        '
        'tbxRef1Phone
        '
        Me.tbxRef1Phone.Location = New System.Drawing.Point(522, 38)
        Me.tbxRef1Phone.Name = "tbxRef1Phone"
        Me.tbxRef1Phone.Size = New System.Drawing.Size(152, 20)
        Me.tbxRef1Phone.TabIndex = 8
        '
        'tbxRef2Phone
        '
        Me.tbxRef2Phone.Location = New System.Drawing.Point(522, 43)
        Me.tbxRef2Phone.Name = "tbxRef2Phone"
        Me.tbxRef2Phone.Size = New System.Drawing.Size(149, 20)
        Me.tbxRef2Phone.TabIndex = 19
        '
        'cbxRef1Rel
        '
        Me.cbxRef1Rel.FormattingEnabled = True
        Me.cbxRef1Rel.ItemHeight = 13
        Me.cbxRef1Rel.Items.AddRange(New Object() {"Employer", "Friend", "Guardian", "Spouse", "Not Available", "Neighbor", "Other", "Parent", "Relative", "Roommate", "Sibling", "Spouse"})
        Me.cbxRef1Rel.Location = New System.Drawing.Point(18, 38)
        Me.cbxRef1Rel.Name = "cbxRef1Rel"
        Me.cbxRef1Rel.Size = New System.Drawing.Size(152, 21)
        Me.cbxRef1Rel.TabIndex = 5
        '
        'cbxRef2Rel
        '
        Me.cbxRef2Rel.FormattingEnabled = True
        Me.cbxRef2Rel.Items.AddRange(New Object() {"Employer", "Friend", "Guardian", "Spouse", "Not Available", "Neighbor", "Other", "Parent", "Relative", "Roommate", "Sibling", "Spouse"})
        Me.cbxRef2Rel.Location = New System.Drawing.Point(18, 42)
        Me.cbxRef2Rel.Name = "cbxRef2Rel"
        Me.cbxRef2Rel.Size = New System.Drawing.Size(152, 21)
        Me.cbxRef2Rel.TabIndex = 16
        '
        'btnQueue
        '
        Me.btnQueue.Location = New System.Drawing.Point(425, 576)
        Me.btnQueue.Name = "btnQueue"
        Me.btnQueue.Size = New System.Drawing.Size(103, 23)
        Me.btnQueue.TabIndex = 26
        Me.btnQueue.Text = "Create Queue"
        Me.btnQueue.UseVisualStyleBackColor = True
        '
        'gbxBor
        '
        Me.gbxBor.Controls.Add(Me.tbxBorAddy)
        Me.gbxBor.Controls.Add(Me.lblAddy)
        Me.gbxBor.Controls.Add(Me.lblDrvLic)
        Me.gbxBor.Controls.Add(Me.lblPhone)
        Me.gbxBor.Controls.Add(Me.lblDOB)
        Me.gbxBor.Controls.Add(Me.tbxBorPhone)
        Me.gbxBor.Controls.Add(Me.tbxDrvLic)
        Me.gbxBor.Controls.Add(Me.tbxDOB)
        Me.gbxBor.Location = New System.Drawing.Point(22, 122)
        Me.gbxBor.Name = "gbxBor"
        Me.gbxBor.Size = New System.Drawing.Size(689, 85)
        Me.gbxBor.TabIndex = 29
        Me.gbxBor.TabStop = False
        Me.gbxBor.Text = "Borrower"
        '
        'gbxRef1
        '
        Me.gbxRef1.Controls.Add(Me.btnSaveRef1)
        Me.gbxRef1.Controls.Add(Me.btnClear1)
        Me.gbxRef1.Controls.Add(Me.tbxRef1Zip)
        Me.gbxRef1.Controls.Add(Me.lblRef1Zip)
        Me.gbxRef1.Controls.Add(Me.tbxRef1State)
        Me.gbxRef1.Controls.Add(Me.tbxRef1City)
        Me.gbxRef1.Controls.Add(Me.tbxRef1Addy2)
        Me.gbxRef1.Controls.Add(Me.lblRef1Addy2)
        Me.gbxRef1.Controls.Add(Me.lblRef1State)
        Me.gbxRef1.Controls.Add(Me.lblRef1City)
        Me.gbxRef1.Controls.Add(Me.tbxRef1LName)
        Me.gbxRef1.Controls.Add(Me.lblRef1LName)
        Me.gbxRef1.Controls.Add(Me.lblRef1Rel)
        Me.gbxRef1.Controls.Add(Me.lblRef1FName)
        Me.gbxRef1.Controls.Add(Me.lblRef1Addy1)
        Me.gbxRef1.Controls.Add(Me.cbxRef1Rel)
        Me.gbxRef1.Controls.Add(Me.lblRef1Phone)
        Me.gbxRef1.Controls.Add(Me.tbxRef1Phone)
        Me.gbxRef1.Controls.Add(Me.tbxRef1Addy1)
        Me.gbxRef1.Controls.Add(Me.tbxRef1FName)
        Me.gbxRef1.Location = New System.Drawing.Point(22, 213)
        Me.gbxRef1.Name = "gbxRef1"
        Me.gbxRef1.Size = New System.Drawing.Size(689, 167)
        Me.gbxRef1.TabIndex = 30
        Me.gbxRef1.TabStop = False
        Me.gbxRef1.Text = "References 1"
        '
        'btnSaveRef1
        '
        Me.btnSaveRef1.Location = New System.Drawing.Point(18, 130)
        Me.btnSaveRef1.Name = "btnSaveRef1"
        Me.btnSaveRef1.Size = New System.Drawing.Size(118, 23)
        Me.btnSaveRef1.TabIndex = 14
        Me.btnSaveRef1.UseVisualStyleBackColor = True
        '
        'btnClear1
        '
        Me.btnClear1.Location = New System.Drawing.Point(189, 130)
        Me.btnClear1.Name = "btnClear1"
        Me.btnClear1.Size = New System.Drawing.Size(102, 23)
        Me.btnClear1.TabIndex = 15
        Me.btnClear1.Text = "Clear All"
        Me.btnClear1.UseVisualStyleBackColor = True
        '
        'tbxRef1Zip
        '
        Me.tbxRef1Zip.Location = New System.Drawing.Point(525, 130)
        Me.tbxRef1Zip.Name = "tbxRef1Zip"
        Me.tbxRef1Zip.Size = New System.Drawing.Size(146, 20)
        Me.tbxRef1Zip.TabIndex = 13
        '
        'lblRef1Zip
        '
        Me.lblRef1Zip.AutoSize = True
        Me.lblRef1Zip.Location = New System.Drawing.Point(522, 111)
        Me.lblRef1Zip.Name = "lblRef1Zip"
        Me.lblRef1Zip.Size = New System.Drawing.Size(25, 13)
        Me.lblRef1Zip.TabIndex = 17
        Me.lblRef1Zip.Text = "Zip:"
        '
        'tbxRef1State
        '
        Me.tbxRef1State.Location = New System.Drawing.Point(522, 86)
        Me.tbxRef1State.Name = "tbxRef1State"
        Me.tbxRef1State.Size = New System.Drawing.Size(149, 20)
        Me.tbxRef1State.TabIndex = 12
        '
        'tbxRef1City
        '
        Me.tbxRef1City.Location = New System.Drawing.Point(354, 86)
        Me.tbxRef1City.Name = "tbxRef1City"
        Me.tbxRef1City.Size = New System.Drawing.Size(149, 20)
        Me.tbxRef1City.TabIndex = 11
        '
        'tbxRef1Addy2
        '
        Me.tbxRef1Addy2.Location = New System.Drawing.Point(189, 85)
        Me.tbxRef1Addy2.Name = "tbxRef1Addy2"
        Me.tbxRef1Addy2.Size = New System.Drawing.Size(149, 20)
        Me.tbxRef1Addy2.TabIndex = 10
        '
        'lblRef1Addy2
        '
        Me.lblRef1Addy2.AutoSize = True
        Me.lblRef1Addy2.Location = New System.Drawing.Point(186, 68)
        Me.lblRef1Addy2.Name = "lblRef1Addy2"
        Me.lblRef1Addy2.Size = New System.Drawing.Size(57, 13)
        Me.lblRef1Addy2.TabIndex = 13
        Me.lblRef1Addy2.Text = "Address 2:"
        '
        'lblRef1State
        '
        Me.lblRef1State.AutoSize = True
        Me.lblRef1State.Location = New System.Drawing.Point(519, 68)
        Me.lblRef1State.Name = "lblRef1State"
        Me.lblRef1State.Size = New System.Drawing.Size(35, 13)
        Me.lblRef1State.TabIndex = 12
        Me.lblRef1State.Text = "State:"
        '
        'lblRef1City
        '
        Me.lblRef1City.AutoSize = True
        Me.lblRef1City.Location = New System.Drawing.Point(351, 68)
        Me.lblRef1City.Name = "lblRef1City"
        Me.lblRef1City.Size = New System.Drawing.Size(27, 13)
        Me.lblRef1City.TabIndex = 11
        Me.lblRef1City.Text = "City:"
        '
        'tbxRef1LName
        '
        Me.tbxRef1LName.Location = New System.Drawing.Point(354, 38)
        Me.tbxRef1LName.Name = "tbxRef1LName"
        Me.tbxRef1LName.Size = New System.Drawing.Size(152, 20)
        Me.tbxRef1LName.TabIndex = 7
        '
        'lblRef1LName
        '
        Me.lblRef1LName.AutoSize = True
        Me.lblRef1LName.Location = New System.Drawing.Point(351, 20)
        Me.lblRef1LName.Name = "lblRef1LName"
        Me.lblRef1LName.Size = New System.Drawing.Size(61, 13)
        Me.lblRef1LName.TabIndex = 9
        Me.lblRef1LName.Text = "Last Name:"
        '
        'mtbxDate
        '
        Me.mtbxDate.Location = New System.Drawing.Point(425, 96)
        Me.mtbxDate.Mask = "00/00/0000"
        Me.mtbxDate.Name = "mtbxDate"
        Me.mtbxDate.Size = New System.Drawing.Size(103, 20)
        Me.mtbxDate.TabIndex = 28
        Me.mtbxDate.ValidatingType = GetType(Date)
        '
        'gbxRef2
        '
        Me.gbxRef2.Controls.Add(Me.btnRef2Update)
        Me.gbxRef2.Controls.Add(Me.tbxRef2Zip)
        Me.gbxRef2.Controls.Add(Me.lblRef2Zip)
        Me.gbxRef2.Controls.Add(Me.tbxRef2State)
        Me.gbxRef2.Controls.Add(Me.tbxRef2City)
        Me.gbxRef2.Controls.Add(Me.tbxRef2Addy2)
        Me.gbxRef2.Controls.Add(Me.lblRef2Addy2)
        Me.gbxRef2.Controls.Add(Me.lblRef2State)
        Me.gbxRef2.Controls.Add(Me.lblRef2City)
        Me.gbxRef2.Controls.Add(Me.tbxRef2LName)
        Me.gbxRef2.Controls.Add(Me.lblRef2LName)
        Me.gbxRef2.Controls.Add(Me.tbxRef2FName)
        Me.gbxRef2.Controls.Add(Me.cbxRef2Rel)
        Me.gbxRef2.Controls.Add(Me.tbxRef2Addy1)
        Me.gbxRef2.Controls.Add(Me.lblRef2Addy1)
        Me.gbxRef2.Controls.Add(Me.lblRef2FName)
        Me.gbxRef2.Controls.Add(Me.lblRef2Phone)
        Me.gbxRef2.Controls.Add(Me.lblRef2Rel)
        Me.gbxRef2.Controls.Add(Me.tbxRef2Phone)
        Me.gbxRef2.Location = New System.Drawing.Point(22, 386)
        Me.gbxRef2.Name = "gbxRef2"
        Me.gbxRef2.Size = New System.Drawing.Size(689, 174)
        Me.gbxRef2.TabIndex = 33
        Me.gbxRef2.TabStop = False
        Me.gbxRef2.Text = "Reference 2"
        '
        'btnRef2Update
        '
        Me.btnRef2Update.Location = New System.Drawing.Point(18, 139)
        Me.btnRef2Update.Name = "btnRef2Update"
        Me.btnRef2Update.Size = New System.Drawing.Size(118, 23)
        Me.btnRef2Update.TabIndex = 25
        Me.btnRef2Update.Text = "Update Reference"
        Me.btnRef2Update.UseVisualStyleBackColor = True
        '
        'tbxRef2Zip
        '
        Me.tbxRef2Zip.Location = New System.Drawing.Point(522, 142)
        Me.tbxRef2Zip.Name = "tbxRef2Zip"
        Me.tbxRef2Zip.Size = New System.Drawing.Size(146, 20)
        Me.tbxRef2Zip.TabIndex = 24
        '
        'lblRef2Zip
        '
        Me.lblRef2Zip.AutoSize = True
        Me.lblRef2Zip.Location = New System.Drawing.Point(522, 123)
        Me.lblRef2Zip.Name = "lblRef2Zip"
        Me.lblRef2Zip.Size = New System.Drawing.Size(25, 13)
        Me.lblRef2Zip.TabIndex = 21
        Me.lblRef2Zip.Text = "Zip:"
        '
        'tbxRef2State
        '
        Me.tbxRef2State.Location = New System.Drawing.Point(522, 99)
        Me.tbxRef2State.Name = "tbxRef2State"
        Me.tbxRef2State.Size = New System.Drawing.Size(149, 20)
        Me.tbxRef2State.TabIndex = 23
        '
        'tbxRef2City
        '
        Me.tbxRef2City.Location = New System.Drawing.Point(354, 99)
        Me.tbxRef2City.Name = "tbxRef2City"
        Me.tbxRef2City.Size = New System.Drawing.Size(149, 20)
        Me.tbxRef2City.TabIndex = 22
        '
        'tbxRef2Addy2
        '
        Me.tbxRef2Addy2.Location = New System.Drawing.Point(186, 99)
        Me.tbxRef2Addy2.Name = "tbxRef2Addy2"
        Me.tbxRef2Addy2.Size = New System.Drawing.Size(152, 20)
        Me.tbxRef2Addy2.TabIndex = 21
        '
        'lblRef2Addy2
        '
        Me.lblRef2Addy2.AutoSize = True
        Me.lblRef2Addy2.Location = New System.Drawing.Point(183, 80)
        Me.lblRef2Addy2.Name = "lblRef2Addy2"
        Me.lblRef2Addy2.Size = New System.Drawing.Size(57, 13)
        Me.lblRef2Addy2.TabIndex = 18
        Me.lblRef2Addy2.Text = "Address 2:"
        '
        'lblRef2State
        '
        Me.lblRef2State.AutoSize = True
        Me.lblRef2State.Location = New System.Drawing.Point(519, 80)
        Me.lblRef2State.Name = "lblRef2State"
        Me.lblRef2State.Size = New System.Drawing.Size(35, 13)
        Me.lblRef2State.TabIndex = 17
        Me.lblRef2State.Text = "State:"
        '
        'lblRef2City
        '
        Me.lblRef2City.AutoSize = True
        Me.lblRef2City.Location = New System.Drawing.Point(351, 80)
        Me.lblRef2City.Name = "lblRef2City"
        Me.lblRef2City.Size = New System.Drawing.Size(27, 13)
        Me.lblRef2City.TabIndex = 16
        Me.lblRef2City.Text = "City:"
        '
        'tbxRef2LName
        '
        Me.tbxRef2LName.Location = New System.Drawing.Point(354, 43)
        Me.tbxRef2LName.Name = "tbxRef2LName"
        Me.tbxRef2LName.Size = New System.Drawing.Size(149, 20)
        Me.tbxRef2LName.TabIndex = 18
        '
        'lblRef2LName
        '
        Me.lblRef2LName.AutoSize = True
        Me.lblRef2LName.Location = New System.Drawing.Point(351, 23)
        Me.lblRef2LName.Name = "lblRef2LName"
        Me.lblRef2LName.Size = New System.Drawing.Size(61, 13)
        Me.lblRef2LName.TabIndex = 14
        Me.lblRef2LName.Text = "Last Name:"
        '
        'lblAmt
        '
        Me.lblAmt.AutoSize = True
        Me.lblAmt.Location = New System.Drawing.Point(208, 77)
        Me.lblAmt.Name = "lblAmt"
        Me.lblAmt.Size = New System.Drawing.Size(46, 13)
        Me.lblAmt.TabIndex = 34
        Me.lblAmt.Text = "Amount:"
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Location = New System.Drawing.Point(425, 77)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(33, 13)
        Me.lblDate.TabIndex = 35
        Me.lblDate.Text = "Date:"
        '
        'tbxAmt
        '
        Me.tbxAmt.Location = New System.Drawing.Point(211, 96)
        Me.tbxAmt.Name = "tbxAmt"
        Me.tbxAmt.Size = New System.Drawing.Size(100, 20)
        Me.tbxAmt.TabIndex = 27
        '
        'frmCSQueueTask
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(725, 611)
        Me.Controls.Add(Me.tbxAmt)
        Me.Controls.Add(Me.lblDate)
        Me.Controls.Add(Me.lblAmt)
        Me.Controls.Add(Me.gbxRef2)
        Me.Controls.Add(Me.mtbxDate)
        Me.Controls.Add(Me.gbxRef1)
        Me.Controls.Add(Me.gbxBor)
        Me.Controls.Add(Me.btnQueue)
        Me.Controls.Add(Me.lblScript)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "frmCSQueueTask"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Create Queue Task"
        Me.gbxBor.ResumeLayout(False)
        Me.gbxBor.PerformLayout()
        Me.gbxRef1.ResumeLayout(False)
        Me.gbxRef1.PerformLayout()
        Me.gbxRef2.ResumeLayout(False)
        Me.gbxRef2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblScript As System.Windows.Forms.Label
    Friend WithEvents lblAddy As System.Windows.Forms.Label
    Friend WithEvents lblDrvLic As System.Windows.Forms.Label
    Friend WithEvents lblRef1Rel As System.Windows.Forms.Label
    Friend WithEvents lblRef1FName As System.Windows.Forms.Label
    Friend WithEvents lblRef1Addy1 As System.Windows.Forms.Label
    Friend WithEvents lblRef1Phone As System.Windows.Forms.Label
    Friend WithEvents lblPhone As System.Windows.Forms.Label
    Friend WithEvents lblDOB As System.Windows.Forms.Label
    Friend WithEvents lblRef2Rel As System.Windows.Forms.Label
    Friend WithEvents lblRef2FName As System.Windows.Forms.Label
    Friend WithEvents lblRef2Addy1 As System.Windows.Forms.Label
    Friend WithEvents lblRef2Phone As System.Windows.Forms.Label
    Friend WithEvents tbxBorAddy As System.Windows.Forms.TextBox
    Friend WithEvents tbxBorPhone As System.Windows.Forms.TextBox
    Friend WithEvents tbxDrvLic As System.Windows.Forms.TextBox
    Friend WithEvents tbxDOB As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef1FName As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef2FName As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef1Addy1 As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef2Addy1 As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef1Phone As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef2Phone As System.Windows.Forms.TextBox
    Friend WithEvents cbxRef1Rel As System.Windows.Forms.ComboBox
    Friend WithEvents cbxRef2Rel As System.Windows.Forms.ComboBox
    Friend WithEvents btnQueue As System.Windows.Forms.Button
    Friend WithEvents gbxBor As System.Windows.Forms.GroupBox
    Friend WithEvents gbxRef1 As System.Windows.Forms.GroupBox
    Friend WithEvents mtbxDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents gbxRef2 As System.Windows.Forms.GroupBox
    Friend WithEvents tbxRef1LName As System.Windows.Forms.TextBox
    Friend WithEvents lblRef1LName As System.Windows.Forms.Label
    Friend WithEvents lblRef2LName As System.Windows.Forms.Label
    Friend WithEvents lblAmt As System.Windows.Forms.Label
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents tbxRef1State As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef1City As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef1Addy2 As System.Windows.Forms.TextBox
    Friend WithEvents lblRef1Addy2 As System.Windows.Forms.Label
    Friend WithEvents lblRef1State As System.Windows.Forms.Label
    Friend WithEvents lblRef1City As System.Windows.Forms.Label
    Friend WithEvents tbxRef2State As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef2City As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef2Addy2 As System.Windows.Forms.TextBox
    Friend WithEvents lblRef2Addy2 As System.Windows.Forms.Label
    Friend WithEvents lblRef2State As System.Windows.Forms.Label
    Friend WithEvents lblRef2City As System.Windows.Forms.Label
    Friend WithEvents tbxRef2LName As System.Windows.Forms.TextBox
    Friend WithEvents lblRef1Zip As System.Windows.Forms.Label
    Friend WithEvents lblRef2Zip As System.Windows.Forms.Label
    Friend WithEvents tbxRef1Zip As System.Windows.Forms.TextBox
    Friend WithEvents tbxRef2Zip As System.Windows.Forms.TextBox
    Friend WithEvents btnClear1 As System.Windows.Forms.Button
    Friend WithEvents btnSaveRef1 As System.Windows.Forms.Button
    Friend WithEvents btnRef2Update As System.Windows.Forms.Button
    Friend WithEvents tbxAmt As System.Windows.Forms.TextBox
End Class
