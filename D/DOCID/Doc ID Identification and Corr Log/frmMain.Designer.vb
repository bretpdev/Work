Partial Public Class frmMain
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbDocType As System.Windows.Forms.ComboBox
    Friend WithEvents lblDocType As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents rbDocIDType As System.Windows.Forms.RadioButton
    Friend WithEvents rbDocIDLst As System.Windows.Forms.RadioButton
    Friend WithEvents rbDocType As System.Windows.Forms.RadioButton
    Friend WithEvents cbDocID As System.Windows.Forms.ComboBox
    Friend WithEvents tbDocID As System.Windows.Forms.TextBox
    Friend WithEvents DocType As System.Windows.Forms.GroupBox
    Friend WithEvents DocIDLst As System.Windows.Forms.GroupBox
    Friend WithEvents DocIDType As System.Windows.Forms.GroupBox
    Friend WithEvents lblDocIDLst As System.Windows.Forms.Label
    Friend WithEvents lblDocIDType As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tbSSN As System.Windows.Forms.TextBox
    Friend WithEvents btnWhoAmI As System.Windows.Forms.Button
    Friend WithEvents lblFinalDocID1 As System.Windows.Forms.Label
    Friend WithEvents lblFinalDocID3 As System.Windows.Forms.Label
    Friend WithEvents lblFinalDocID5 As System.Windows.Forms.Label
    Friend WithEvents lblFinalDocID6 As System.Windows.Forms.Label
    Friend WithEvents lblFinalDocID4 As System.Windows.Forms.Label
    Friend WithEvents lblFinalDocID2 As System.Windows.Forms.Label
    Friend WithEvents CaseSearch As System.Windows.Forms.Button
    Friend WithEvents cbDocTypeCorrFax As System.Windows.Forms.CheckBox
    Friend WithEvents cbDocIDCorrFax As System.Windows.Forms.CheckBox
    Friend WithEvents DTPMailRcvdDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnMailRvcd As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cbDocIDCorrFax = New System.Windows.Forms.CheckBox
        Me.cbDocTypeCorrFax = New System.Windows.Forms.CheckBox
        Me.rbDocIDType = New System.Windows.Forms.RadioButton
        Me.rbDocIDLst = New System.Windows.Forms.RadioButton
        Me.rbDocType = New System.Windows.Forms.RadioButton
        Me.DocType = New System.Windows.Forms.GroupBox
        Me.lblDocType = New System.Windows.Forms.Label
        Me.cbDocType = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.DocIDLst = New System.Windows.Forms.GroupBox
        Me.lblDocIDLst = New System.Windows.Forms.Label
        Me.cbDocID = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.DocIDType = New System.Windows.Forms.GroupBox
        Me.tbDocID = New System.Windows.Forms.TextBox
        Me.lblDocIDType = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.tbSSN = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblFinalDocID1 = New System.Windows.Forms.Label
        Me.btnWhoAmI = New System.Windows.Forms.Button
        Me.lblFinalDocID3 = New System.Windows.Forms.Label
        Me.lblFinalDocID5 = New System.Windows.Forms.Label
        Me.lblFinalDocID6 = New System.Windows.Forms.Label
        Me.lblFinalDocID4 = New System.Windows.Forms.Label
        Me.lblFinalDocID2 = New System.Windows.Forms.Label
        Me.CaseSearch = New System.Windows.Forms.Button
        Me.DTPMailRcvdDate = New System.Windows.Forms.DateTimePicker
        Me.btnMailRvcd = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.DocType.SuspendLayout()
        Me.DocIDLst.SuspendLayout()
        Me.DocIDType.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cbDocIDCorrFax)
        Me.GroupBox1.Controls.Add(Me.cbDocTypeCorrFax)
        Me.GroupBox1.Controls.Add(Me.rbDocIDType)
        Me.GroupBox1.Controls.Add(Me.rbDocIDLst)
        Me.GroupBox1.Controls.Add(Me.rbDocType)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(224, 288)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select a Processing Mode:"
        '
        'cbDocIDCorrFax
        '
        Me.cbDocIDCorrFax.Enabled = False
        Me.cbDocIDCorrFax.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(0, Byte))
        Me.cbDocIDCorrFax.Location = New System.Drawing.Point(144, 180)
        Me.cbDocIDCorrFax.Name = "cbDocIDCorrFax"
        Me.cbDocIDCorrFax.Size = New System.Drawing.Size(68, 24)
        Me.cbDocIDCorrFax.TabIndex = 4
        Me.cbDocIDCorrFax.Text = "Corr Fax"
        '
        'cbDocTypeCorrFax
        '
        Me.cbDocTypeCorrFax.Enabled = False
        Me.cbDocTypeCorrFax.ForeColor = System.Drawing.Color.DarkRed
        Me.cbDocTypeCorrFax.Location = New System.Drawing.Point(144, 72)
        Me.cbDocTypeCorrFax.Name = "cbDocTypeCorrFax"
        Me.cbDocTypeCorrFax.Size = New System.Drawing.Size(68, 24)
        Me.cbDocTypeCorrFax.TabIndex = 3
        Me.cbDocTypeCorrFax.Text = "Corr Fax"
        '
        'rbDocIDType
        '
        Me.rbDocIDType.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(64, Byte))
        Me.rbDocIDType.Image = CType(resources.GetObject("rbDocIDType.Image"), System.Drawing.Image)
        Me.rbDocIDType.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbDocIDType.Location = New System.Drawing.Point(16, 228)
        Me.rbDocIDType.Name = "rbDocIDType"
        Me.rbDocIDType.Size = New System.Drawing.Size(196, 40)
        Me.rbDocIDType.TabIndex = 2
        Me.rbDocIDType.Text = "By Document ID (Type Document ID)"
        '
        'rbDocIDLst
        '
        Me.rbDocIDLst.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(0, Byte))
        Me.rbDocIDLst.Image = CType(resources.GetObject("rbDocIDLst.Image"), System.Drawing.Image)
        Me.rbDocIDLst.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbDocIDLst.Location = New System.Drawing.Point(16, 140)
        Me.rbDocIDLst.Name = "rbDocIDLst"
        Me.rbDocIDLst.Size = New System.Drawing.Size(196, 40)
        Me.rbDocIDLst.TabIndex = 1
        Me.rbDocIDLst.Text = "By Document ID (Use Provided Document ID List)"
        '
        'rbDocType
        '
        Me.rbDocType.ForeColor = System.Drawing.Color.DarkRed
        Me.rbDocType.Image = CType(resources.GetObject("rbDocType.Image"), System.Drawing.Image)
        Me.rbDocType.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbDocType.Location = New System.Drawing.Point(16, 48)
        Me.rbDocType.Name = "rbDocType"
        Me.rbDocType.Size = New System.Drawing.Size(196, 24)
        Me.rbDocType.TabIndex = 0
        Me.rbDocType.Text = "By Document Type"
        '
        'DocType
        '
        Me.DocType.Controls.Add(Me.lblDocType)
        Me.DocType.Controls.Add(Me.cbDocType)
        Me.DocType.Controls.Add(Me.Label1)
        Me.DocType.Enabled = False
        Me.DocType.Location = New System.Drawing.Point(232, 8)
        Me.DocType.Name = "DocType"
        Me.DocType.Size = New System.Drawing.Size(632, 96)
        Me.DocType.TabIndex = 1
        Me.DocType.TabStop = False
        '
        'lblDocType
        '
        Me.lblDocType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDocType.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocType.ForeColor = System.Drawing.Color.DarkRed
        Me.lblDocType.Location = New System.Drawing.Point(8, 36)
        Me.lblDocType.Name = "lblDocType"
        Me.lblDocType.Size = New System.Drawing.Size(616, 56)
        Me.lblDocType.TabIndex = 2
        Me.lblDocType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbDocType
        '
        Me.cbDocType.ForeColor = System.Drawing.Color.DarkRed
        Me.cbDocType.Location = New System.Drawing.Point(144, 12)
        Me.cbDocType.Name = "cbDocType"
        Me.cbDocType.Size = New System.Drawing.Size(412, 21)
        Me.cbDocType.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.DarkRed
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(136, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select A Document Type:"
        '
        'DocIDLst
        '
        Me.DocIDLst.Controls.Add(Me.lblDocIDLst)
        Me.DocIDLst.Controls.Add(Me.cbDocID)
        Me.DocIDLst.Controls.Add(Me.Label3)
        Me.DocIDLst.Enabled = False
        Me.DocIDLst.Location = New System.Drawing.Point(232, 104)
        Me.DocIDLst.Name = "DocIDLst"
        Me.DocIDLst.Size = New System.Drawing.Size(632, 96)
        Me.DocIDLst.TabIndex = 2
        Me.DocIDLst.TabStop = False
        '
        'lblDocIDLst
        '
        Me.lblDocIDLst.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDocIDLst.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocIDLst.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(0, Byte))
        Me.lblDocIDLst.Location = New System.Drawing.Point(8, 36)
        Me.lblDocIDLst.Name = "lblDocIDLst"
        Me.lblDocIDLst.Size = New System.Drawing.Size(616, 56)
        Me.lblDocIDLst.TabIndex = 2
        Me.lblDocIDLst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbDocID
        '
        Me.cbDocID.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(0, Byte))
        Me.cbDocID.Location = New System.Drawing.Point(144, 12)
        Me.cbDocID.Name = "cbDocID"
        Me.cbDocID.Size = New System.Drawing.Size(412, 21)
        Me.cbDocID.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(136, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Select A Document ID:"
        '
        'DocIDType
        '
        Me.DocIDType.Controls.Add(Me.tbDocID)
        Me.DocIDType.Controls.Add(Me.lblDocIDType)
        Me.DocIDType.Controls.Add(Me.Label5)
        Me.DocIDType.Enabled = False
        Me.DocIDType.Location = New System.Drawing.Point(232, 200)
        Me.DocIDType.Name = "DocIDType"
        Me.DocIDType.Size = New System.Drawing.Size(632, 96)
        Me.DocIDType.TabIndex = 3
        Me.DocIDType.TabStop = False
        '
        'tbDocID
        '
        Me.tbDocID.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(64, Byte))
        Me.tbDocID.Location = New System.Drawing.Point(144, 12)
        Me.tbDocID.MaxLength = 5
        Me.tbDocID.Name = "tbDocID"
        Me.tbDocID.TabIndex = 3
        Me.tbDocID.Text = ""
        '
        'lblDocIDType
        '
        Me.lblDocIDType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDocIDType.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocIDType.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(64, Byte))
        Me.lblDocIDType.Location = New System.Drawing.Point(8, 36)
        Me.lblDocIDType.Name = "lblDocIDType"
        Me.lblDocIDType.Size = New System.Drawing.Size(616, 56)
        Me.lblDocIDType.TabIndex = 2
        Me.lblDocIDType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(64, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(136, 16)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Enter A Document ID:"
        '
        'tbSSN
        '
        Me.tbSSN.Location = New System.Drawing.Point(148, 304)
        Me.tbSSN.MaxLength = 10
        Me.tbSSN.Name = "tbSSN"
        Me.tbSSN.Size = New System.Drawing.Size(108, 20)
        Me.tbSSN.TabIndex = 4
        Me.tbSSN.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(12, 308)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(136, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "SSN, Account # or Ref ID:"
        '
        'btnSearch
        '
        Me.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearch.Location = New System.Drawing.Point(260, 304)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 20)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "Search/Process"
        '
        'btnCancel
        '
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(364, 304)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 20)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        '
        'lblFinalDocID1
        '
        Me.lblFinalDocID1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFinalDocID1.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalDocID1.ForeColor = System.Drawing.Color.Navy
        Me.lblFinalDocID1.Location = New System.Drawing.Point(8, 332)
        Me.lblFinalDocID1.Name = "lblFinalDocID1"
        Me.lblFinalDocID1.Size = New System.Drawing.Size(424, 68)
        Me.lblFinalDocID1.TabIndex = 8
        Me.lblFinalDocID1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnWhoAmI
        '
        Me.btnWhoAmI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnWhoAmI.Location = New System.Drawing.Point(572, 304)
        Me.btnWhoAmI.Name = "btnWhoAmI"
        Me.btnWhoAmI.Size = New System.Drawing.Size(100, 20)
        Me.btnWhoAmI.TabIndex = 9
        Me.btnWhoAmI.Text = "Who Am I?"
        '
        'lblFinalDocID3
        '
        Me.lblFinalDocID3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFinalDocID3.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalDocID3.ForeColor = System.Drawing.Color.Navy
        Me.lblFinalDocID3.Location = New System.Drawing.Point(8, 400)
        Me.lblFinalDocID3.Name = "lblFinalDocID3"
        Me.lblFinalDocID3.Size = New System.Drawing.Size(424, 68)
        Me.lblFinalDocID3.TabIndex = 10
        Me.lblFinalDocID3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFinalDocID5
        '
        Me.lblFinalDocID5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFinalDocID5.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalDocID5.ForeColor = System.Drawing.Color.Navy
        Me.lblFinalDocID5.Location = New System.Drawing.Point(8, 468)
        Me.lblFinalDocID5.Name = "lblFinalDocID5"
        Me.lblFinalDocID5.Size = New System.Drawing.Size(424, 68)
        Me.lblFinalDocID5.TabIndex = 11
        Me.lblFinalDocID5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFinalDocID6
        '
        Me.lblFinalDocID6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFinalDocID6.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalDocID6.ForeColor = System.Drawing.Color.Navy
        Me.lblFinalDocID6.Location = New System.Drawing.Point(440, 468)
        Me.lblFinalDocID6.Name = "lblFinalDocID6"
        Me.lblFinalDocID6.Size = New System.Drawing.Size(424, 68)
        Me.lblFinalDocID6.TabIndex = 14
        Me.lblFinalDocID6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFinalDocID4
        '
        Me.lblFinalDocID4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFinalDocID4.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalDocID4.ForeColor = System.Drawing.Color.Navy
        Me.lblFinalDocID4.Location = New System.Drawing.Point(440, 400)
        Me.lblFinalDocID4.Name = "lblFinalDocID4"
        Me.lblFinalDocID4.Size = New System.Drawing.Size(424, 68)
        Me.lblFinalDocID4.TabIndex = 13
        Me.lblFinalDocID4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFinalDocID2
        '
        Me.lblFinalDocID2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFinalDocID2.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalDocID2.ForeColor = System.Drawing.Color.Navy
        Me.lblFinalDocID2.Location = New System.Drawing.Point(440, 332)
        Me.lblFinalDocID2.Name = "lblFinalDocID2"
        Me.lblFinalDocID2.Size = New System.Drawing.Size(424, 68)
        Me.lblFinalDocID2.TabIndex = 12
        Me.lblFinalDocID2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CaseSearch
        '
        Me.CaseSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.CaseSearch.Location = New System.Drawing.Point(468, 304)
        Me.CaseSearch.Name = "CaseSearch"
        Me.CaseSearch.Size = New System.Drawing.Size(100, 20)
        Me.CaseSearch.TabIndex = 8
        Me.CaseSearch.Text = "Case # Search"
        '
        'DTPMailRcvdDate
        '
        Me.DTPMailRcvdDate.Enabled = False
        Me.DTPMailRcvdDate.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.DTPMailRcvdDate.Location = New System.Drawing.Point(780, 304)
        Me.DTPMailRcvdDate.Name = "DTPMailRcvdDate"
        Me.DTPMailRcvdDate.Size = New System.Drawing.Size(84, 20)
        Me.DTPMailRcvdDate.TabIndex = 15
        Me.DTPMailRcvdDate.TabStop = False
        Me.DTPMailRcvdDate.Visible = False
        '
        'btnMailRvcd
        '
        Me.btnMailRvcd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMailRvcd.Location = New System.Drawing.Point(676, 304)
        Me.btnMailRvcd.Name = "btnMailRvcd"
        Me.btnMailRvcd.Size = New System.Drawing.Size(100, 20)
        Me.btnMailRvcd.TabIndex = 16
        Me.btnMailRvcd.TabStop = False
        Me.btnMailRvcd.Text = "Chg Mail Rcvd:"
        Me.btnMailRvcd.Visible = False
        '
        'frmMain
        '
        Me.AcceptButton = Me.btnSearch
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(872, 540)
        Me.Controls.Add(Me.btnMailRvcd)
        Me.Controls.Add(Me.DTPMailRcvdDate)
        Me.Controls.Add(Me.CaseSearch)
        Me.Controls.Add(Me.lblFinalDocID6)
        Me.Controls.Add(Me.lblFinalDocID4)
        Me.Controls.Add(Me.lblFinalDocID2)
        Me.Controls.Add(Me.lblFinalDocID5)
        Me.Controls.Add(Me.lblFinalDocID3)
        Me.Controls.Add(Me.btnWhoAmI)
        Me.Controls.Add(Me.lblFinalDocID1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbSSN)
        Me.Controls.Add(Me.DocIDType)
        Me.Controls.Add(Me.DocIDLst)
        Me.Controls.Add(Me.DocType)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(880, 568)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Document Type/ID and Corr Log"
        Me.GroupBox1.ResumeLayout(False)
        Me.DocType.ResumeLayout(False)
        Me.DocIDLst.ResumeLayout(False)
        Me.DocIDType.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
End Class
