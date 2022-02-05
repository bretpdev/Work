Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Reflection
Imports Uheaa.Common.DataAccess
Imports Uheaa.Common.ProcessLogger

Public Class frmMain
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

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

#End Region

    Private _actualDataAdapter As SqlDataAdapter
    Private _actualSqlConnection As SqlConnection
    Private _logOleDbConnection As Data.OleDb.OleDbConnection
    Private _generalDataSet As New DataSet()
    Private _bankruptcyForm As frmBankruptcy
    Private _pleaseWaitForm As New PleaseWait()
    Private _whoAmIWasUsed As Boolean
    Private _bsysSqlConnection As SqlConnection
    Private _userName As String
    Private _caseSearchForm As frmBankruptcySearch
    Private _membersOfQc As String = String.Empty

    Private IsCompassOnly As Boolean
    Public Property CompassOnly() As Boolean
        Get
            Return IsCompassOnly
        End Get
        Set(ByVal value As Boolean)
            IsCompassOnly = value
        End Set
    End Property

    Private PLogData As ProcessLogData
    Public Property ProcLogData() As ProcessLogData
        Get
            Return PLogData
        End Get
        Set(ByVal value As ProcessLogData)
            PLogData = value
        End Set
    End Property


    Enum RefCheckResult
        NotValid = 0
        UserInvalidated = 1
        Valid = 2
    End Enum

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GeneralStuff.InitSession()        'connect to the currently running Reflection session
        _bankruptcyForm = New frmBankruptcy()
        _caseSearchForm = New frmBankruptcySearch((Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Dev" Or Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Tst"), Me)
        'set connection and data adapter variables for the rest of the code
        DetermineMode()

        'do report thing if applicable

        'get user name from BSYS doesn't matter if live or test so always go to live
        Dim sqlText As String = String.Format("SELECT FirstName + ' ' + LastName as TheName FROM SYSA_LST_Users WHERE WindowsUserName = '{0}'", Environment.UserName)
        Dim bsysSqlCommand As New SqlCommand(sqlText, _bsysSqlConnection)
        _bsysSqlConnection.Open()

        Dim dataReader As SqlDataReader = bsysSqlCommand.ExecuteReader()
        dataReader.Read()         'go to first record
        _userName = dataReader("TheName")          'get user name
        dataReader.Close()

        bsysSqlCommand.CommandText = "SELECT WindowsUserID + '@utahsbr.edu' as email FROM GENR_REF_BU_Agent_Xref WHERE BusinessUnit = 'Quality Control' AND Role = 'Member Of'"
        dataReader = bsysSqlCommand.ExecuteReader()
        Dim qcList As New List(Of String)
        While dataReader.Read()
            qcList.Add(dataReader(0).ToString())
        End While
        _membersOfQc = String.Join(",", qcList.ToArray())
        dataReader.Close()
        _bsysSqlConnection.Close()

        'fill Drop Down boxes
        _actualSqlConnection.Open()
        bsysSqlCommand.Connection = _actualSqlConnection
        bsysSqlCommand.CommandText = "SELECT DocType FROM DocTypeFunctionalityChecks ORDER BY DocType"
        dataReader = bsysSqlCommand.ExecuteReader
        cbDocType.Items.Add("")
        While dataReader.Read
            cbDocType.Items.Add(dataReader.GetString(0))
        End While
        dataReader.Close()

        bsysSqlCommand.CommandText = "SELECT * FROM DocIDDocTypeLst ORDER BY DocID"
        dataReader = bsysSqlCommand.ExecuteReader
        cbDocID.Items.Add("")
        While dataReader.Read
            cbDocID.Items.Add(dataReader.GetString(0) & " -- " & dataReader.GetString(1))
        End While
        dataReader.Close()
        _actualSqlConnection.Close()

        'fill needed data tables
        _actualDataAdapter.SelectCommand.CommandText = "SELECT dtf.DocType, dtf.DoLSandSChecks, dtf.DoBankruptcyCheck, dtf.DoGeneralCorr, dtf.DoLVC, dtf.DocIDTranslation, dtl.CompassOnlyArc FROM DocTypeFunctionalityChecks dtf LEFT JOIN DocIDDocTypeLst dtl ON dtf.DocType = dtl.DocumentType"
        _actualDataAdapter.Fill(_generalDataSet, "DocTypeFunc")
        _actualDataAdapter.SelectCommand.CommandText = "SELECT * FROM CorrLog"
        _actualDataAdapter.Fill(_generalDataSet, "CorrLog")
        _actualDataAdapter.SelectCommand.CommandText = "SELECT * FROM DocIDDeterminationCrit"
        _actualDataAdapter.Fill(_generalDataSet, "Crit")
        _actualDataAdapter.SelectCommand.CommandText = "SELECT DocID, DocumentType, CompassOnlyArc FROM DocIDDocTypeLst"
        _actualDataAdapter.Fill(_generalDataSet, "DocIDs")
        _actualDataAdapter.SelectCommand.CommandText = "SELECT * FROM XDocID_Queue"
        _actualDataAdapter.Fill(_generalDataSet, "DocIDQueue")
        AddHandler cbDocID.TextChanged, AddressOf FindComboItem
        AddHandler cbDocType.TextChanged, AddressOf FindComboItem
        _whoAmIWasUsed = False
        DTPMailRcvdDate.Value = DateTime.Today
        DTPMailRcvdDate.MaxDate = DateTime.Today
    End Sub

    ''' <summary>
    ''' Sets up the mode between test and live
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DetermineMode()
        If Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Dev" Or Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Tst" Then
            _bsysSqlConnection = New SqlConnection("Data Source=OPSDEV;Initial Catalog=BSYS;Integrated Security=SSPI;")
            _actualSqlConnection = New SqlConnection("Data Source=OPSDEV;Initial Catalog=DOCID;Integrated Security=SSPI;")
            _actualDataAdapter = New SqlDataAdapter("SELECT * FROM DocTypeFunctionalityChecks", "Data Source=OPSDEV;Initial Catalog=DOCID;Integrated Security=SSPI;")
            _logOleDbConnection = New Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=X:\PADD\AccountServices\TEST\LVC - No Loans Serviced by UHEAA.mdb;User Id=admin;Password=;")
            MsgBox("The application is running in test mode.", MsgBoxStyle.Critical, "TEST MODE")
        Else
            _bsysSqlConnection = New SqlConnection("Data Source=NOCHOUSE;Initial Catalog=BSYS;Integrated Security=SSPI;")
            _actualSqlConnection = New SqlConnection("Data Source=NOCHOUSE;Initial Catalog=DOCID;Integrated Security=SSPI;")
            _actualDataAdapter = New SqlDataAdapter("SELECT * FROM DocTypeFunctionalityChecks", "Data Source=NOCHOUSE;Initial Catalog=DOCID;Integrated Security=SSPI;")
            _logOleDbConnection = New Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=X:\PADD\AccountServices\LVC - No Loans Serviced by UHEAA.mdb;User Id=admin;Password=;")
        End If
    End Sub

#Region "Buttons"
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim returnedAccNum As String = String.Empty
        SetProcessLogger()
        ClearLabels() 'Sets all the labels to empty

        CheckSSN() 'Determines if the borrower is in OneLink or Compass
        If CompassOnly Then
            FastPathInput("TX3Z/ITX1JB;" + tbSSN.Text)
            returnedAccNum = GetText(3, 34, 12).Replace(" ", "")
        End If

        _pleaseWaitForm.Show()
        _pleaseWaitForm.Refresh()

        'Determine the correct document source code.
        Dim fromCode As DocumentSource.Code = SetSourceCode()


        'if reference is given
        If tbSSN.Text.ToUpper().StartsWith("RF@") Or tbSSN.Text.ToUpper().StartsWith("P") Then
            If (Not CheckIfSkip()) Then 'Check to see if skip data is entered
                Return
            End If

            'do ref check
            If (CompassOnly = False) Then
                If (Not GetLP2CRefCheck(fromCode)) Then
                    Return
                End If
            Else
                FastPathInput("TX3Z/ITX1JR;" + tbSSN.Text) 'Got to TX1J and search as reference to find the borrower info
                Dim borSSN As String = GetText(1, 11, 9)
                ATD22AllLoans(GetText(1, 11, 9), "KREFL", "Reference letter received.")
            End If

            AddSuccessfulInfo2DB(tbSSN.Text, fromCode)
            tbSSN.Clear()
            _pleaseWaitForm.Hide()
            tbSSN.Focus()
            tbSSN.SelectAll()
            Return
        End If

        Dim worked As Boolean = True

        'if account # or SSN is given
        If cbDocID.Text.Length > 0 AndAlso tbSSN.TextLength > 8 Then    'selected Doc ID
            If (Not SelectedDocId(fromCode, returnedAccNum)) Then
                worked = False
            End If
        ElseIf tbDocID.TextLength = 5 AndAlso tbSSN.TextLength > 8 Then 'typed doc ID
            If (Not TypedDocId(fromCode, returnedAccNum)) Then
                worked = False
            End If
        ElseIf cbDocType.Text.Length > 0 AndAlso tbSSN.TextLength > 8 Then  'doc type
            If (Not DetermineDocType(fromCode, returnedAccNum)) Then
                worked = False
            End If
        Else    'if valid information wasn't provided
            If (Not ValidateInput()) Then
                worked = False
            End If
        End If
        _pleaseWaitForm.Hide()
        'application should only exit through here if it was successful in finding a doc id for the entered criteria
        If worked Then
            AddSuccessfulInfo2DB(returnedAccNum, fromCode)
            tbSSN.Clear()
            tbSSN.Focus()
            tbSSN.SelectAll()
            ProcessLogger.LogEnd(ProcLogData.ProcessLogId)
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        End
    End Sub

    Private Sub CaseSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CaseSearch.Click
        If _caseSearchForm.Visible Then
            _caseSearchForm.Activate()
        End If
        _caseSearchForm.Show()
    End Sub

    Private Sub btnWhoAmI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWhoAmI.Click
        If cbDocID.Text = "ASBKP -- Bankruptcy Document/Correspondence" OrElse _
        cbDocType.Text = "Bankruptcy Document/Correspondence" OrElse _
         cbDocType.Text = "Bankruptcy Discharge/Dismissal Documents" OrElse _
         cbDocType.Text = "Bankruptcy Meeting of Creditors + Zions Report" OrElse _
         cbDocID.Text = "ASCON -- Bankruptcy Discharge/Dismissal Documents" OrElse _
         cbDocID.Text = "ASMOC -- Bankruptcy Meeting of Creditors + Zions Report" OrElse _
         tbDocID.Text = "ASCON" OrElse _
         tbDocID.Text = "ASBKP" OrElse _
         tbDocID.Text = "ASMOC" Then
            Dim docSourceCode As DocumentSource.Code
            Select Case True
                Case rbDocType.Checked
                    docSourceCode = DocumentSource.Code.PO
                Case rbDocIDLst.Checked
                    docSourceCode = DocumentSource.Code.BU
                Case cbDocIDCorrFax.Checked OrElse cbDocTypeCorrFax.Checked
                    docSourceCode = DocumentSource.Code.CF
                Case rbDocIDType.Checked
                    docSourceCode = DocumentSource.Code.OT
            End Select
            _bankruptcyForm.Show(UserSelection(), New DocumentSource(docSourceCode))
            If _bankruptcyForm.FormWasCancelledOrNoDocIdWasSelected Then
                ActivatePreviousInstance("Document Type/ID and Corr Log")
                MsgBox("The documentation may be shredded.", MsgBoxStyle.Information)
            Else
                ActivatePreviousInstance("Document Type/ID and Corr Log")
            End If
        Else
            Dim switches() As String = RIBM.CommandLineSwitches.ToString().Replace("""", "").Split("\")
            ActivatePreviousInstance(switches(switches.GetUpperBound(0)))
            RIBM.RunMacro("SP.WhoAmI.Main", "")
            ActivatePreviousInstance("Document Type/ID and Corr Log")
        End If

        btnSearch.Focus()
        If CheckForText(1, 69, "DEMOGRAPHICS") Then
            _whoAmIWasUsed = True
            tbSSN.Text = GetText(3, 23, 9)
        ElseIf CheckForText(1, 71, "TXX1R") Then
            _whoAmIWasUsed = True
            tbSSN.Text = GetText(3, 12, 11).Replace(" ", "")
        Else
            _whoAmIWasUsed = False
            tbSSN.Text = ""
        End If
    End Sub

    Private Sub btnMailRvcd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMailRvcd.Click
        If btnMailRvcd.Text = "Chg Mail Rcvd:" Then
            'enable mail received date and change button text
            DTPMailRcvdDate.Enabled = True
            btnMailRvcd.Text = "Lock Mail Rcvd:"
        ElseIf btnMailRvcd.Text = "Lock Mail Rcvd:" Then
            'disable mail received date and change button text
            DTPMailRcvdDate.Enabled = False
            btnMailRvcd.Text = "Chg Mail Rcvd:"
        End If
    End Sub
#End Region

#Region "Radio buttons, which handle their respective check boxes"
    Private Sub rbDocType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDocType.CheckedChanged
        RemoveHandler cbDocID.TextChanged, AddressOf FindComboItem
        RemoveHandler cbDocType.TextChanged, AddressOf FindComboItem
        If rbDocType.Checked Then
            cbDocTypeCorrFax.Enabled = True
            DocType.Enabled = True
            For Each docTypeControl As Control In DocType.Controls
                docTypeControl.Enabled = True
            Next docTypeControl
        Else
            cbDocTypeCorrFax.Checked = False
            cbDocTypeCorrFax.Enabled = False
            cbDocType.SelectedIndex = 0
            DocType.Enabled = False
            For Each docTypeControl As Control In DocType.Controls
                docTypeControl.Enabled = False
            Next docTypeControl
        End If
        AddHandler cbDocID.TextChanged, AddressOf FindComboItem
        AddHandler cbDocType.TextChanged, AddressOf FindComboItem
        btnMailRvcd.Visible = rbDocType.Checked
        DTPMailRcvdDate.Visible = rbDocType.Checked
    End Sub

    Private Sub rbDocIDLst_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDocIDLst.CheckedChanged
        RemoveHandler cbDocID.TextChanged, AddressOf FindComboItem
        RemoveHandler cbDocType.TextChanged, AddressOf FindComboItem
        If rbDocIDLst.Checked Then
            cbDocIDCorrFax.Enabled = True
            DocIDLst.Enabled = True
            For Each docIdLstControl As Control In DocIDLst.Controls
                docIdLstControl.Enabled = True
            Next
        Else
            cbDocIDCorrFax.Checked = False
            cbDocIDCorrFax.Enabled = False
            cbDocID.SelectedIndex = 0
            DocIDLst.Enabled = False
            For Each docIdLstControl As Control In DocIDLst.Controls
                docIdLstControl.Enabled = False
            Next
        End If
        AddHandler cbDocID.TextChanged, AddressOf FindComboItem
        AddHandler cbDocType.TextChanged, AddressOf FindComboItem
    End Sub

    Private Sub rbDocIDType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDocIDType.CheckedChanged
        RemoveHandler cbDocID.TextChanged, AddressOf FindComboItem
        RemoveHandler cbDocType.TextChanged, AddressOf FindComboItem
        If rbDocIDType.Checked Then
            DocIDType.Enabled = True
            For Each docIdTypeControl As Control In DocIDType.Controls
                docIdTypeControl.Enabled = True
            Next
        Else
            tbDocID.Clear()
            DocIDType.Enabled = False
            For Each docIdTypeControl As Control In DocIDType.Controls
                docIdTypeControl.Enabled = False
            Next
        End If
        AddHandler cbDocID.TextChanged, AddressOf FindComboItem
        AddHandler cbDocType.TextChanged, AddressOf FindComboItem
    End Sub
#End Region

#Region "Other widgets (combo boxes, text boxes)"
    Private Sub cbDocType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDocType.SelectedIndexChanged
        lblDocType.Text = cbDocType.Text
    End Sub

    Private Sub cbDocID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDocID.SelectedIndexChanged
        lblDocIDLst.Text = cbDocID.Text
    End Sub

    Private Sub tbDocID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDocID.TextChanged
        lblDocIDType.Text = tbDocID.Text
    End Sub

    Private Sub tbSSN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbSSN.TextChanged
        _whoAmIWasUsed = False
    End Sub
#End Region

    Private Sub SetProcessLogger()
        DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa
        DataAccessHelper.CurrentMode = If(TestMode(), DataAccessHelper.CurrentMode.Dev, DataAccessHelper.CurrentMode.Live)
        ProcLogData = ProcessLogger.RegisterApplication(ScriptID, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly)
    End Sub

    ''' <summary>
    ''' Sets the DocumentSource code
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSourceCode() As DocumentSource.Code
        Select Case True
            Case cbDocIDCorrFax.Checked OrElse cbDocTypeCorrFax.Checked
                Return DocumentSource.Code.CF
            Case rbDocType.Checked
                Return DocumentSource.Code.PO
            Case rbDocIDLst.Checked
                Return DocumentSource.Code.BU
            Case rbDocIDType.Checked
                Return DocumentSource.Code.OT
        End Select
    End Function

    ''' <summary>
    ''' Checks to see if any data was entered as skip data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckIfSkip() As Boolean
        If (Not cbDocID.Text.StartsWith("SKREF")) AndAlso cbDocType.Text <> "Reference Skip Letter" AndAlso tbDocID.Text.ToUpper() <> "SKREF" Then
            MsgBox("Only skip reference letters should be entered using the reference ID number.", MsgBoxStyle.Critical)
            _pleaseWaitForm.Hide()
            tbSSN.Clear()
            tbSSN.Focus()
            tbSSN.SelectAll()
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Determines if the doc is a reference
    ''' </summary>
    ''' <param name="fromCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLP2CRefCheck(ByVal fromCode As DocumentSource.Code) As Boolean
        Select Case LP2CRefCheck(tbSSN.Text)
            Case RefCheckResult.NotValid
                MessageBox.Show("The reference ID provided was invalid.", "Invalid Reference ID", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return True
            Case RefCheckResult.Valid
                AddLP9O(tbSSN.Text, "SKREF", tbSSN.Text)
                If rbDocIDLst.Checked Then
                    rbDocIDLst.Checked = False
                End If
                If rbDocType.Checked Then
                    rbDocType.Checked = False
                End If
                If rbDocIDType.Checked Then
                    rbDocIDType.Checked = False
                End If
                ReferenceAddLP50(tbSSN.Text, "MDCID", "AM", "10", String.Format("Reference Skip Letter received, Doc ID SKREF.  From: {0}", New DocumentSource(fromCode).ToString()))
                UpdateDocIds("SKREF")
                Return True
            Case Else    'UserInvalidated
                UpdateDocIds()
                _pleaseWaitForm.Hide()
                tbSSN.Clear()
                tbSSN.Focus()
                tbSSN.SelectAll()
                Return False
        End Select
    End Function

    ''' <summary>
    ''' User selected the Doc ID
    ''' </summary>
    ''' <param name="fromCode"></param>
    ''' <param name="returnedAccNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SelectedDocId(ByVal fromCode As DocumentSource.Code, ByRef returnedAccNum As String) As Boolean
        'check if SSN or account number is valid and get SSN translation if valid
        Dim ssn As String = String.Empty
        Select Case GatherSSN(ssn, tbSSN.Text, cbDocID.Text, _whoAmIWasUsed, returnedAccNum)
            Case SsnStatus.Invalid  'SSN wasn't found
                _pleaseWaitForm.Hide()
                tbSSN.Clear()
                tbSSN.Focus()
                tbSSN.SelectAll()
                Return False
            Case SsnStatus.SemiValid  'SSN wasn't found but it's a bankrupcty document
                _pleaseWaitForm.Hide()
                _bankruptcyForm.Show(UserSelection(), New DocumentSource(fromCode), tbSSN.Text, False)
                UpdateDocIds()
                tbSSN.Clear()
                tbSSN.Focus()
                tbSSN.SelectAll()
                Return False
        End Select

        Dim bankruptcyFunctionWasUsed As Boolean = False
        If CompassOnly = False Then
            'if bankruptcy document then do bankruptcy functionality
            DetermineBankruptcyUsed(fromCode, ssn)
        End If

        'check if bankruptcy form was cancelled
        If bankruptcyFunctionWasUsed = False OrElse _bankruptcyForm.FormWasCancelledOrNoDocIdWasSelected = False Then
            'Parse the doc ID and description from the combo box.
            Dim separator As String = " -- "
            Dim docId As String = cbDocID.Text.Substring(0, cbDocID.Text.IndexOf(separator))
            Dim docDescription As String = cbDocID.Text.Substring(cbDocID.Text.IndexOf(separator) + separator.Length)
            docId = CorrLog(docId, docDescription, ssn)
            UpdateDocIds(docId)
            If (CompassOnly = False) Then
                QueueEntry(docId, ssn)
                AddLP50(Date.Now, ssn, "MDCID", "AM", "10", String.Format("{0} received, Doc ID {1}.  From: {2}", docDescription, docId, New DocumentSource(fromCode).ToString()))
            Else
                Dim docIds As String = _generalDataSet.Tables("DocIDs").Select(String.Format("DocID = '{0}' AND CompassOnlyArc IS NOT NULL", cbDocID.Text.Substring(0, 5))).FirstOrDefault()(0)
                Dim arc As String = _generalDataSet.Tables("DocIDs").Select(String.Format("DocID = '{0}' AND CompassOnlyArc IS NOT NULL", docIds)).FirstOrDefault()(2)
                Dim message As String = String.Format("{0} received, Doc ID {1}.  From: {2}", docDescription, docIds, New DocumentSource(fromCode).ToString())
                If arc <> "" OrElse arc IsNot Nothing Then
                    ATD22AllLoans(ssn, arc, message)
                Else
                    ATC00GeneralComment(ssn, message)
                End If
                UpdateDocIds(docIds)
            End If
        Else
            'if bankruptcy form was cancelled
            UpdateDocIds()
            tbSSN.Clear()
            tbSSN.Focus()
            tbSSN.SelectAll()
            Return False
        End If
        'Reset the input fields by unchecking the radio button.
        rbDocIDLst.Checked = False
        Return True
    End Function

    ''' <summary>
    ''' The user typed in a Doc Id
    ''' </summary>
    ''' <param name="fromCode"></param>
    ''' <param name="returnedAccNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TypedDocId(ByVal fromCode As DocumentSource.Code, ByRef returnedAccNum As String) As Boolean
        tbDocID.Text = tbDocID.Text.ToUpper()
        Dim rows() As DataRow = _generalDataSet.Tables("DocIDs").Select(String.Format("DocID = '{0}'", tbDocID.Text))
        Dim userResponse As DialogResult = DialogResult.No
        If rows.Length = 0 Then
            userResponse = MessageBox.Show("The doc ID you provided is not currently an active doc ID.  Do you wish to continue?", "Inactive Doc ID", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        End If
        'if the user wishes to continue or the query returned results then continue processing
        If userResponse = DialogResult.Yes OrElse rows.Length <> 0 Then
            'check if SSN or account number is valid and get SSN translation if valid
            Dim ssn As String = String.Empty
            Select Case GatherSSN(ssn, tbSSN.Text, tbDocID.Text, _whoAmIWasUsed, returnedAccNum)
                Case SsnStatus.Invalid  'SSN wasn't found
                    _pleaseWaitForm.Hide()
                    tbSSN.Clear()
                    tbSSN.Focus()
                    tbSSN.SelectAll()
                    Return False
                Case SsnStatus.SemiValid  'if SSN wasn't found but it's a bankrupcty document
                    _pleaseWaitForm.Hide()
                    _bankruptcyForm.Show(UserSelection(), New DocumentSource(fromCode), tbSSN.Text, False)
                    UpdateDocIds()
                    tbSSN.Clear()
                    tbSSN.Focus()
                    tbSSN.SelectAll()
                    Return False
            End Select

            Dim bankruptcyFunctionWasUsed As Boolean = False
            If tbDocID.Text = "ASBKP" OrElse _
             tbDocID.Text = "ASCON" OrElse _
             tbDocID.Text = "ASMOC" Then
                _bankruptcyForm.Show(UserSelection(), New DocumentSource(fromCode), ssn, True)
                bankruptcyFunctionWasUsed = True
            End If
            'check if bankruptcy form was cancelled
            If bankruptcyFunctionWasUsed = False OrElse _bankruptcyForm.FormWasCancelledOrNoDocIdWasSelected = False Then
                UpdateDocIds(tbDocID.Text)
                If (CompassOnly = False) Then
                    QueueEntry(tbDocID.Text, ssn)
                    AddLP50(Date.Now, ssn, "MDCID", "AM", "10", String.Format("Undefined Document Type received, Doc ID {0}.  From: {1}", tbDocID.Text, New DocumentSource(fromCode).ToString()))
                Else
                    Dim arc As String = DataAccess.GetArcFromDocID(TestMode, tbDocID.Text)
                    Dim message As String = String.Format("Undefined Document Type received, Doc ID {0}.  From: {1}", tbDocID.Text, New DocumentSource(fromCode).ToString())
                    If arc <> "" OrElse arc IsNot Nothing Then
                        ATD22AllLoans(ssn, arc, message)
                    Else
                        ATC00GeneralComment(ssn, message)
                    End If
                    UpdateDocIds("LSCOR")
                End If
            Else
                'if bankruptcy form was cancelled
                UpdateDocIds()
                tbSSN.Clear()
                tbSSN.Focus()
                tbSSN.SelectAll()
                Return False
            End If
        Else
            UpdateDocIds()
            tbSSN.Clear()
            tbDocID.Clear()
            Return False
        End If
        'Reset the input fields by unchecking the radio button.
        rbDocIDType.Checked = False
        Return True
    End Function

    ''' <summary>
    ''' Determines what the doc type is
    ''' </summary>
    ''' <param name="fromCode"></param>
    ''' <param name="returnedAccNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DetermineDocType(ByVal fromCode As DocumentSource.Code, ByRef returnedAccNum As String) As Boolean
        'check if SSN or account number is valid and get SSN translation if valid
        Dim ssn As String = String.Empty
        Dim doLvcTracking As Boolean
        Dim lp22Results As SsnStatus = GatherSSN(ssn, tbSSN.Text, cbDocType.Text, _whoAmIWasUsed, returnedAccNum, doLvcTracking)
        Select Case lp22Results
            Case SsnStatus.Invalid  'SSN wasn't found
                _pleaseWaitForm.Hide()
                tbSSN.Clear()
                tbSSN.Focus()
                tbSSN.SelectAll()
                Return False
            Case SsnStatus.SemiValid  'if SSN wasn't found but it's a bankrupcty document
                _pleaseWaitForm.Hide()
                _bankruptcyForm.Show(UserSelection(), New DocumentSource(fromCode), tbSSN.Text, False)
                UpdateDocIds()
                tbSSN.Clear()
                tbSSN.Focus()
                tbSSN.SelectAll()
                Return False
        End Select

        If CompassOnly = False Then
            If doLvcTracking AndAlso cbDocType.Text = "LVC Blank" Then    'be sure the right document type is selected
                Me.Hide()
                If tbSSN.Text = New frmLVCSSNConf().Show Then
                    LVCTracking()
                End If
                Me.Show()
            End If
            'Place refs in array
            Dim resultLabels As New List(Of Label)(6)
            resultLabels.Add(lblFinalDocID1)
            resultLabels.Add(lblFinalDocID2)
            resultLabels.Add(lblFinalDocID3)
            resultLabels.Add(lblFinalDocID4)
            resultLabels.Add(lblFinalDocID5)
            resultLabels.Add(lblFinalDocID6)
            Dim loanList As New List(Of Loan)()
            Dim postDocDetermineProcessing As Boolean = True
            DocumentIdDetermination(ssn, postDocDetermineProcessing, loanList)

            'check for corr log functionality
            If postDocDetermineProcessing Then
                For Each thisLoan As Loan In loanList
                    'if indicator is set to use in results given back to user then corr log
                    If thisLoan.UseForResultsDisplay Then
                        thisLoan.DocId = CorrLog(thisLoan.DocId, cbDocType.Text, ssn)
                    End If
                Next thisLoan
            End If
            'remove all duplicates from display results
            Dim foundDocIds As New List(Of String)()
            For Each thisLoan As Loan In loanList
                If thisLoan.UseForResultsDisplay Then
                    If foundDocIds.Contains(thisLoan.DocId) Then
                        thisLoan.UseForResultsDisplay = False
                    Else
                        foundDocIds.Add(thisLoan.DocId)
                    End If
                End If
            Next thisLoan

            'enter results into the labels for display to user
            Dim filledLabelCount As Integer = 0
            For Each thisLoan As Loan In loanList
                'if indicator is set to use in results given back to user then enter into label
                If thisLoan.UseForResultsDisplay Then
                    resultLabels(filledLabelCount).Text = thisLoan.DocId
                    QueueEntry(thisLoan.DocId, ssn)
                    filledLabelCount += 1
                End If
            Next thisLoan
            'blank the remaining labels
            While filledLabelCount < resultLabels.Count - 1
                resultLabels(filledLabelCount).Text = String.Empty
                filledLabelCount += 1
            End While

            'do LP50 comments for results
            'figure out string to enter into comments
            Dim lp50DocId As String = String.Empty
            If Not String.IsNullOrEmpty(lblFinalDocID1.Text) Then
                lp50DocId = lblFinalDocID1.Text
                If Not String.IsNullOrEmpty(lblFinalDocID2.Text) Then
                    lp50DocId += String.Format(",{0}", lblFinalDocID2.Text)
                    If Not String.IsNullOrEmpty(lblFinalDocID3.Text) Then
                        lp50DocId += String.Format(",{0}", lblFinalDocID3.Text)
                        If Not String.IsNullOrEmpty(lblFinalDocID4.Text) Then
                            lp50DocId += String.Format(",{0}", lblFinalDocID4.Text)
                            If Not String.IsNullOrEmpty(lblFinalDocID5.Text) Then
                                lp50DocId += String.Format(",{0}", lblFinalDocID5.Text)
                                If Not String.IsNullOrEmpty(lblFinalDocID6.Text) Then
                                    lp50DocId += String.Format(",{0}", lblFinalDocID6.Text)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            If Not String.IsNullOrEmpty(lp50DocId) Then
                Dim receiveDate As Date = Date.Now
                If rbDocType.Checked Then receiveDate = DTPMailRcvdDate.Value
                AddLP50(receiveDate, ssn, "MDCID", "AM", "10", String.Format("{0} received, Doc ID {1}.  From: {2}", cbDocType.Text, lp50DocId, New DocumentSource(fromCode).ToString()))
            End If
            'Reset input fields by unchecking the radio button.
            rbDocType.Checked = False
            Return True
        Else
            If CheckCompassOpenLoans(ssn) = True Then
                Dim ids() As DataRow = _generalDataSet.Tables("DocIDs").Select(String.Format("DocumentType = '{0}' AND CompassOnlyArc IS NOT NULL", cbDocType.Text))
                If ids.Count > 0 Then
                    Dim docId As String = _generalDataSet.Tables("DocIDs").Select(String.Format("DocumentType = '{0}' AND CompassOnlyArc IS NOT NULL", cbDocType.Text)).FirstOrDefault()(0)
                    Dim arc As String = _generalDataSet.Tables("DocIDs").Select(String.Format("DocumentType = '{0}' AND CompassOnlyArc IS NOT NULL", cbDocType.Text)).FirstOrDefault()(2)

                    lblFinalDocID1.Text = docId
                    Dim comment As String = String.Format("{0} received, Doc ID {1}. From : {2}", cbDocType.Text, docId, New DocumentSource(fromCode).ToString())
                    If arc <> "" OrElse arc IsNot Nothing Then
                        ATD22AllLoans(ssn, arc, comment)
                    Else
                        ATC00GeneralComment(ssn, comment)
                    End If
                    'Reset input fields by unchecking the radio button.
                    rbDocType.Checked = False
                    Return True
                Else
                    MessageBox.Show("There are no Doc ID's for the letter type when a borrower is not in OneLink", "No Doc ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            End If
        End If
    End Function

    ''' <summary>
    ''' Validates that data input by the user
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateInput() As Boolean
        Dim message As New StringBuilder("You have provided invalid data.  Please check the following:")
        message.Append(Environment.NewLine)
        message.Append(Environment.NewLine)
        message.Append("- Have you selected a processing mode")
        message.Append(Environment.NewLine)
        message.Append("- If using one of the drop down lists have you selected one of the options")
        message.Append(Environment.NewLine)
        message.Append("- If using the type in option have you provided a 5 character document ID")
        message.Append(Environment.NewLine)
        message.Append("- Have you provided an SSN or account number")
        message.Append(Environment.NewLine)
        message.Append(Environment.NewLine)
        MessageBox.Show(message.ToString(), "Invalid Data")
        _pleaseWaitForm.Hide()
        Return False
    End Function

    ''' <summary>
    ''' Determines if the borrower is in bankruptcy
    ''' </summary>
    ''' <param name="fromCode"></param>
    ''' <param name="ssn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DetermineBankruptcyUsed(ByVal fromCode As DocumentSource.Code, ByVal ssn As String) As Boolean
        If cbDocID.Text = "ASBKP -- Bankruptcy Document/Correspondence" OrElse _
                cbDocID.Text = "ASCON -- Bankruptcy Discharge/Dismissal Documents" OrElse _
                cbDocID.Text = "ASMOC -- Bankruptcy Meeting of Creditors + Zions Report" Then
            _bankruptcyForm.Show(UserSelection(), New DocumentSource(fromCode), ssn, True)
            Return True
        End If
        Return False
    End Function

    'determines the docID
    Private Sub DocumentIdDetermination(ByVal ssn As String, ByRef postDocDetermineProcessing As Boolean, ByRef loanList As List(Of Loan))
        loanList = New List(Of Loan)
        'query to figure what functionality needs to be performed to determine the Doc ID
        Dim docRows() As DataRow = _generalDataSet.Tables("DocTypeFunc").Select(String.Format("DocType = '{0}'", cbDocType.Text))
        'if all functionality is bypassed then default to listed DocID and return it
        If CBool(docRows(0).Item("DoLSandSChecks")) = False AndAlso _
        CBool(docRows(0).Item("DoBankruptcyCheck")) = False AndAlso _
        CBool(docRows(0).Item("DoGeneralCorr")) = False AndAlso _
        CBool(docRows(0).Item("DoLVC")) = False Then
            loanList.Add(New Loan(, , docRows(0).Item("DocIDTranslation")))
            Return
        End If
        If CBool(docRows(0).Item("DoGeneralCorr")) Then
            GeneralCorrespondenceCheck(ssn, loanList)
            postDocDetermineProcessing = False
            Return
        End If
        If CBool(docRows(0).Item("DoLVC")) Then
            LVCCheck(ssn, loanList)
            Return
        End If
        If CBool(docRows(0).Item("DoLSandSChecks")) = False AndAlso CBool(docRows(0).Item("DoBankruptcyCheck")) Then
            _pleaseWaitForm.Hide()
            If BankruptcyCheck(ssn) Then    'do bankruptcy processing
                loanList.Add(New Loan(, , docRows(0).Item("DocIDTranslation")))
            End If
        End If
        If CBool(docRows(0).Item("DoLSandSChecks")) Then
            Dim useDefault As Boolean = True
            LoanStatusAndServicerCheck(ssn, loanList)
            For Each thisLoan As Loan In loanList
                Dim sqlCondition As String = String.Format("DocumentType = '{0}' AND (LoanStatus LIKE '%{1}%' OR LoanStatus = '' OR LoanStatus IS NULL)", cbDocType.Text, thisLoan.LoanStatus)
                If Not String.IsNullOrEmpty(thisLoan.Reason) Then
                    sqlCondition += String.Format(" AND (ReasonCode LIKE '%{0}%' OR ReasonCode = '' OR ReasonCode IS NULL) AND (Servicer LIKE '%{1}%' OR Servicer = '' OR Servicer IS NULL)", thisLoan.Reason, thisLoan.Servicer)
                Else
                    sqlCondition += String.Format(" AND (ReasonCode = '' OR ReasonCode IS NULL) AND (Servicer LIKE '%{0}%' OR Servicer = '' OR Servicer IS NULL)", thisLoan.Servicer)
                End If
                Dim critRows() As DataRow = _generalDataSet.Tables("Crit").Select(sqlCondition, "ResultOrder")
                If critRows.Length <> 0 Then
                    thisLoan.DocId = critRows(0).Item("DocID")
                    useDefault = False  'if any of the queries yield results then don't use default
                Else
                    thisLoan.UseForResultsDisplay = False   'don't use in display because there is nothing there
                End If
            Next thisLoan
            'use default if none of the queries yielded results
            If useDefault Then
                'use default
                loanList.Add(New Loan(, , docRows(0).Item("DocIDTranslation")))
            Else
                'mark duplicates to not be shown
                Dim foundDocIds As New List(Of String)()
                For Each thisLoan As Loan In loanList
                    If thisLoan.UseForResultsDisplay Then
                        'check for duplicates
                        If foundDocIds.Contains(thisLoan.DocId) Then
                            'if doc id is already in list then don't use it again
                            thisLoan.UseForResultsDisplay = False
                        Else
                            'if doc id is not in list then add to list and leave indicator = true
                            foundDocIds.Add(thisLoan.DocId)
                        End If
                    End If
                Next thisLoan
            End If
        End If
    End Sub

    'this function does the LVC check
    Private Sub LVCCheck(ByVal ssn As String, ByRef loanList As List(Of Loan))
        Dim foundServicer700126 As Boolean = False
        Dim queues As New List(Of String)()
        FastPathInput("LG02I;" & ssn)   'access LG02
        If CheckForText(1, 58, "LOAN APPLICATION SELECT") Then     'for selection screen
            Dim row As Integer = 10
            While CheckForText(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                If CheckForText(row, 3, " ") Then  'if blank then press "F8" and start on next screen if appliable
                    Press("F8")
                    row = 10
                Else    'if line is populated
                    'check for different status
                    If CheckForText(row, 75, "CP") OrElse CheckForText(row, 75, "DN") Then
                        If Not queues.Contains("PCLVC") Then
                            queues.Add("PCLVC")
                        End If
                    Else
                        If CheckForText(row, 46, "700126") Then
                            foundServicer700126 = True
                        End If
                    End If
                    row += 1    'go to next row
                End If
            End While
        Else          'for target screen
            FastPathInput("LG10I" & ssn)
            'check for different status
            If (CheckForText(11, 59, "CP") Or CheckForText(11, 59, "DN")) Then
                If queues.Contains("PCLVC") = False Then
                    queues.Add("PCLVC")
                End If
            Else
                If CheckForText(5, 27, "700126") Then
                    foundServicer700126 = True
                End If
            End If
        End If
        'if Servicer 700126 is found skip ts26 functionality 
        If foundServicer700126 Then
            FastPathInput("TX3ZITS26" & ssn)
            If CheckForText(23, 2, "01527") = False AndAlso _
            AllLoansArePaidInFull() = False AndAlso _
            queues.Contains("LSLVC") = False Then
                queues.Add("LSLVC")
            End If
        End If
        If queues.Count > 0 Then
            loanList = New List(Of Loan)(queues.Count)
            For Each result As String In queues
                loanList.Add(New Loan(, , result))
            Next result
        Else
            LVCTracking()
            MessageBox.Show("Borrower currently does not have loans being serviced by UHEAA.", "No UHEAA Loans", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    'The AllLoansPaidInFull function checks ITS26 to see if all of a borrower's loans are PAID IN FULL.
    'It's used as a helper to LVCVheck, and assumes we're already on ITS26 for the borrower.
    Private Function AllLoansArePaidInFull() As Boolean
        'Loop through all pages of a borrower's loans.
        Do While Not CheckForText(23, 2, "90007")
            'Select each loan on the page.
            For row As Integer = 8 To 19
                'Select the current row only if it's not blank.
                If Not CheckForText(row, 3, " ") Then
                    PutText(21, 12, (row - 7).ToString, True)
                    'Any status other than "PAID IN FULL" means the loans aren't all PIF, so our work is done.
                    If Not CheckForText(3, 10, "PAID IN FULL") Then
                        Return False
                    End If
                    'If we didn't just return false, then back out to get the next loan.
                    Press("F12")
                    'Clear the input field.
                    Press("END")
                End If
            Next row
            'Got all the rows on this page; go to the next page.
            Press("F8")
        Loop
    End Function

    Sub LVCTracking()
        Dim logCommand As New OleDb.OleDbCommand(String.Format("INSERT INTO Log (SSN) VALUES ('{0}')", tbSSN.Text), _logOleDbConnection)
        'add information to DB log
        logCommand.Connection.Open()
        logCommand.ExecuteNonQuery()
        logCommand.Connection.Close()
        'send email to QC Staff
        SendMail(_membersOfQc, "Entry added to ""LVC - No Loans Serviced by UHEAA"" log.")
    End Sub

    'this function does the general corresponence Check
    Private Sub GeneralCorrespondenceCheck(ByVal ssn As String, ByRef loanList As List(Of Loan))
        Dim queues As New List(Of String)
        Dim desiredNumbers As New List(Of String)()
        desiredNumbers.Add("700079")
        desiredNumbers.Add("700004")
        desiredNumbers.Add("700789")
        desiredNumbers.Add("700191")
        desiredNumbers.Add("700190")
        desiredNumbers.Add("700121")
        Dim forbiddenCodes As New List(Of String)()
        forbiddenCodes.Add("CA")
        forbiddenCodes.Add("PC")
        forbiddenCodes.Add("PM")
        forbiddenCodes.Add("PF")
        forbiddenCodes.Add("PN")
        forbiddenCodes.Add("RF")
        Dim isContingency As Boolean

        FastPathInput("LG02I;" & ssn)   'access LG02
        If CheckForText(1, 58, "LOAN APPLICATION SELECT") Then     'for selection screen
            Dim row As Integer = 10
            While CheckForText(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                If CheckForText(row, 3, " ") Then  'if blank then press "F8" and start on next screen if appliable
                    Press("F8")
                    row = 10
                Else    'if line is populated
                    'check for different status
                    If CheckForText(row, 75, "CP") OrElse CheckForText(row, 75, "DN") Then
                        'if specific preclaim status is found
                        If CheckForText(row, 78, "BC") OrElse CheckForText(row, 78, "BO") OrElse CheckForText(row, 78, "BH") Then
                            'if CP
                            If CheckForText(row, 75, "CP") Then
                                If queues.Contains("PCCOR") = False Then
                                    queues.Add("PCCOR")
                                End If
                                If queues.Contains("ASBKP") = False Then
                                    queues.Add("ASBKP")
                                End If
                            End If
                        Else
                            If queues.Contains("PCCOR") = False Then
                                queues.Add("PCCOR")
                            End If
                        End If
                    End If
                    If CheckForText(row, 75, "CR") Then
                        'if specific preclaim status is found
                        If CheckForText(row, 78, "BC") OrElse CheckForText(row, 78, "BO") OrElse CheckForText(row, 78, "BH") Then
                            If queues.Contains("ASBKP") = False Then
                                queues.Add("ASBKP")
                            End If
                        Else
                            If queues.Contains("DACOR") = False Then
                                queues.Add("DACOR")
                            End If
                        End If
                    End If
                    If CheckForText(row, 46, "700126") Then
                        If CheckForText(row, 75, "CA") = False AndAlso _
                        CheckForText(row, 75, "PC") = False AndAlso _
                           CheckForText(row, 75, "PM") = False AndAlso _
                           CheckForText(row, 75, "PF") = False AndAlso _
                           CheckForText(row, 75, "PN") = False AndAlso _
                           CheckForText(row, 75, "RF") = False Then
                            If queues.Contains("LSCOR") = False Then
                                queues.Add("LSCOR")
                            End If
                        End If
                        'if status is blank
                        If CheckForText(row, 75, "  ") Then
                            If queues.Contains("UGCOR") = False Then
                                queues.Add("UGCOR")
                            End If
                        End If
                    End If
                    'check for contingency LoanManagement label
                    If desiredNumbers.Contains(GetText(row, 46, 6)) AndAlso _
                    Not forbiddenCodes.Contains(GetText(row, 75, 2)) Then
                        isContingency = True
                    End If
                    row += 1    'go to next row
                End If
            End While
        Else    'for target screen
            FastPathInput("LG10I" & ssn)
            'check for different status
            If CheckForText(11, 59, "CP") OrElse CheckForText(11, 59, "DN") Then
                'if specific preclaim status is found
                If CheckForText(11, 46, "BC") OrElse CheckForText(11, 46, "BO") OrElse CheckForText(11, 46, "BH") Then
                    'if CP
                    If CheckForText(11, 59, "CP") Then
                        If queues.Contains("PCCOR") = False Then
                            queues.Add("PCCOR")
                        End If
                        If queues.Contains("ASBKP") = False Then
                            queues.Add("ASBKP")
                        End If
                    End If
                Else
                    If queues.Contains("PCCOR") = False Then
                        queues.Add("PCCOR")
                    End If
                End If
            End If
            If CheckForText(11, 59, "CR") Then
                'if specific preclaim status is found
                If CheckForText(11, 46, "BC") OrElse CheckForText(11, 46, "BO") OrElse CheckForText(11, 46, "BH") Then
                    If queues.Contains("ASBKP") = False Then
                        queues.Add("ASBKP")
                    End If
                Else
                    If queues.Contains("DACOR") = False Then
                        queues.Add("DACOR")
                    End If
                End If
            End If
            If CheckForText(5, 27, "700126") Then
                If Not forbiddenCodes.Contains(GetText(11, 59, 2)) Then
                    If queues.Contains("LSCOR") = False Then
                        queues.Add("LSCOR")
                    End If
                End If
                'if status is blank
                If CheckForText(11, 59, "  ") Then
                    If queues.Contains("UGCOR") = False Then
                        queues.Add("UGCOR")
                    End If
                End If
            End If
            'check for contingency LoanManagement label
            If desiredNumbers.Contains(GetText(5, 27, 6)) AndAlso _
            Not forbiddenCodes.Contains(GetText(11, 59, 2)) Then
                isContingency = True
            End If
        End If

        'if none of the above status were found then do additional checks
        If queues.Count = 0 Then
            'for testing
            If TestMode() Then
                MessageBox.Show("Please log into LCO test and then click OK.", "Change System", MessageBoxButtons.OK)
            End If
            FastPathInput("TPDD" & ssn)
            'check if borrower is on LCO
            If CheckForText(1, 19, "LCO PERSONAL INFORMATION DISPLAY") Then
                If queues.Contains("LSCOR") = False Then
                    queues.Add("LSCOR")
                End If
            End If
            'for testing
            If TestMode() Then
                MessageBox.Show("Please log into regular test and then click OK.", "Change System", MessageBoxButtons.OK)
            End If
            'display Loan Management label if needed from above
            If isContingency Then
                If queues.Contains("DACOR") = False Then
                    queues.Add("DACOR")
                End If
            End If
            'if none of the labels still aren't visible then display borrower services
            If queues.Count = 0 Then
                If queues.Contains("LSCOR") = False Then
                    queues.Add("LSCOR")
                End If
            End If
        End If

        loanList = New List(Of Loan)(queues.Count)
        For Each result As String In queues
            loanList.Add(New Loan(, , result))
        Next result
    End Sub

    'this function does the loan status check on LG02 and or LG10 for OneLink and ITS26 for Compass loans
    Private Sub LoanStatusAndServicerCheck(ByVal ssn As String, ByRef loanList As List(Of Loan))
        Try
            If CompassOnly = False Then
                'Gather Loan Statuses from LG02 or LG10, check for super statuses as the data is collected
                FastPathInput("LG02I;" & ssn)
                If CheckForText(1, 60, "LOAN APPLICATION MENU") Then   'if record wasn't found
                    FastPathInput("LG02I" & ssn)
                    If CheckForText(1, 60, "LOAN APPLICATION MENU") Then
                        'borrower doesn't have loans on OneLINK
                        MessageBox.Show("This borrower does not have a loan in OneLINK.", "No OneLINK Loans", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                End If
                'check if selection or target screen is displayed
                If CheckForText(1, 58, "LOAN APPLICATION SELECT") Then
                    'selection screen on LG02
                    Dim row As Integer = 10
                    While CheckForText(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                        If GetText(row, 75, 2) <> String.Empty Then
                            'collect status, Reason and servicer info 
                            loanList.Add(New Loan(GetText(row, 75, 2), GetText(row, 46, 6), , GetText(row, 78, 2)))
                        End If
                        row += 1
                        If row = 21 OrElse CheckForText(row, 3, " ") Then
                            row = 10
                            Press("F8")                      'page forward to next
                        End If
                    End While
                Else
                    'target screen on LG02; go to LG10
                    FastPathInput("LG10I" & ssn)
                    'collect status, reason and servicer info
                    loanList.Add(New Loan(GetText(11, 59, 2), GetText(5, 27, 6), , GetText(11, 64, 2)))
                End If
            Else
                CheckCompassOpenLoans(ssn)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function CheckCompassOpenLoans(ByVal ssn As String) As Boolean
        FastPathInput("TX3Z/ITS26" + ssn)
        Dim amount As Double = 0
        If CheckForText(1, 72, "TSX29") Then
            If GetText(11, 12, 12) <> "" Then
                Dim amt As Double = CDbl(GetText(11, 12, 12))
                If amt > 0 Then
                    amount += amt
                Else
                    Return False
                End If
            End If
        Else
            Dim index As Integer
            While CheckForText(23, 2, "90007") = False
                For index = 8 To 20
                    If GetText(index, 2, 2) <> "" Then
                        Dim amt As Double = CDbl(GetText(index, 59, 10))
                        If amt > 0 Then
                            Return True
                        Else
                            amount += amt
                        End If
                    End If
                Next
                Press("F8")
            End While
        End If

        If amount = 0 Then
            MessageBox.Show("No loans in compass", "No Loans", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        Return True
    End Function

    'this function does the bankruptcy check 
    Private Function BankruptcyCheck(ByVal ssn As String) As Boolean
        Me.Hide()
        Dim docSourceCode As DocumentSource.Code
        Select Case True
            Case rbDocType.Checked
                docSourceCode = DocumentSource.Code.PO
            Case rbDocIDLst.Checked
                docSourceCode = DocumentSource.Code.BU
            Case cbDocIDCorrFax.Checked OrElse cbDocTypeCorrFax.Checked
                docSourceCode = DocumentSource.Code.CF
            Case rbDocIDType.Checked
                docSourceCode = DocumentSource.Code.OT
        End Select
        _bankruptcyForm.Show(UserSelection(), New DocumentSource(docSourceCode), ssn)          'show form and start bankruptcy check functionality
        Me.Show()
        Return Not _bankruptcyForm.FormWasCancelledOrNoDocIdWasSelected
    End Function

    'this function is the starting of the corr log process
    Private Function CorrLog(ByRef docId As String, ByVal docType As String, ByVal ssn As String) As String
        'check if DocID and Doc type are in the table to be Corr logged
        Dim docRows() As DataRow = _generalDataSet.Tables("CorrLog").Select(String.Format("DocID = '{0}' AND DocType = '{1}'", docId, docType))
        'check for results
        If docRows.GetUpperBound(0) = -1 Then
            Return docId
        End If

        'Do LG10 and TS24 checks
        If CorrLogLG10andTS24Confirm(ssn) = False Then
            docId = docRows(0).Item("NotOnTS24ChangeTo")
        Else
            TX8C(ssn, docRows(0).Item("ARC"), CalculateDocumentIDNumber(docRows(0).Item("CodeNumber"), docRows(0).Item("SeqNum")))
        End If
        Return docId
    End Function

    'adds TX8C batch
    Private Sub TX8C(ByVal ssn As String, ByVal arc As String, ByVal corrLogNum As String)
        FastPathInput("TX3ZATX8C")
        PutText(4, 43, Date.Now.ToString("MMddyy"))     'date received
        PutText(9, 10, corrLogNum)
        PutText(9, 29, ssn)
        PutText(9, 42, "01")
        PutText(9, 46, "B")
        PutText(9, 52, "B")
        PutText(9, 66, ssn)
        PutText(10, 11, arc, True)
        If CheckForText(23, 2, "02154") = False Then
            MessageBox.Show("Please set this document aside and notify Systems Support that this document errored in Corr Logging.", "Error in Corr Logging", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Press("F11")
        End If
    End Sub

    'this function calculates the document ID number for Corr Log
    Private Function CalculateDocumentIDNumber(ByVal codeNumber As String, ByVal sequenceNumber As String) As String
        'Use just the first part of the sequence number.
        If sequenceNumber.Contains("-") Then
            sequenceNumber = sequenceNumber.Substring(0, sequenceNumber.IndexOf("-"))
        End If

        Dim documentIdBuilder As New StringBuilder()
        documentIdBuilder.Append(Date.Now.ToString("MMddyy"))   'first six digits are the date
        documentIdBuilder.Append(codeNumber)                    'seventh digit is code number
        documentIdBuilder.Append("10000")                       '5 zeros for filler

        Dim logDirectory As String = "X:\PADD\CorrLogging\"
        Dim sequenceBump As Integer = 0
        If TestMode() Then
            logDirectory += "Test\"
            sequenceBump = 1000
        End If

        'check if a file for today exists
        Dim logFile As String = String.Format("{0}{1} {2}", logDirectory, sequenceNumber, Date.Now.ToString("MMddyy"))
        If Not File.Exists(logFile) Then
            'delete all previous files
            For Each existingFile As String In Directory.GetFiles(logDirectory, sequenceNumber)
                File.Delete(existingFile)
            Next
            'Create new file for the current date
            File.WriteAllText(logFile, (Integer.Parse(sequenceNumber) + sequenceBump).ToString())
        End If

        'gather number, use in corr log number, increment number by one and insert back into file without loosing lock on file
        For failedFileAccessAttempt As Integer = 0 To 120
            Dim fileAccess As FileStream
            Try
                fileAccess = File.Open(logFile, FileMode.Open)
                Dim byteArray() As Byte = New Byte(sequenceNumber.Length + 2) {}
                fileAccess.Read(byteArray, 0, sequenceNumber.Length + 2)
                Dim sequenceNumberFromFile As String = New UTF8Encoding().GetString(byteArray)
                documentIdBuilder.Append(Integer.Parse(sequenceNumberFromFile).ToString("0000#"))
                sequenceNumberFromFile = (Integer.Parse(sequenceNumberFromFile) + 1).ToString("0000#")
                'Move back to the beginning of the file.
                fileAccess.Seek(0, SeekOrigin.Begin)
                byteArray = New UTF8Encoding().GetBytes(sequenceNumberFromFile)
                fileAccess.Write(byteArray, 0, byteArray.Length)
                fileAccess.Close()
                fileAccess.Dispose()
                Return documentIdBuilder.ToString()
            Catch EX As Exception
                If (fileAccess IsNot Nothing) Then
                    fileAccess.Close()
                    fileAccess.Dispose()
                End If
                'if the file is busy then let the user know what is happening
                If failedFileAccessAttempt = 0 Then
                    MessageBox.Show("A file the application is trying to access is busy.  The application will continue to try and access the file for the next two minutes.  If it hasn't accessed the file by then the application will end, and you will need to process the document again.  Please click OK to start the two minutes.", "Can't Access Needed File", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                'end app if two minutes have passed
                If failedFileAccessAttempt = 120 Then
                    MessageBox.Show("The needed file couldn't be accessed.  Please contact Systems Support.", "File Inaccessible", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Application.Exit()
                    End
                End If
                'pause app for one second
                System.Threading.Thread.Sleep(New TimeSpan(0, 0, 1))
            End Try
        Next failedFileAccessAttempt
        Return Nothing  'This will never be reached, but the compiler warns about code paths returning a value if this isn't here.
    End Function

    'This gives the combo boxes the functionality of looking up values in them
    Private Sub FindComboItem(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cboSent As ComboBox = CType(sender, ComboBox)
        If cboSent.FindString(cboSent.Text) <> -1 Then
            Dim intPos As Integer = cboSent.Text.Length
            RemoveHandler cboSent.TextChanged, AddressOf FindComboItem
            cboSent.SelectedItem = cboSent.Items(cboSent.FindString(cboSent.Text))
            AddHandler cboSent.TextChanged, AddressOf FindComboItem
            cboSent.SelectionStart = intPos
            cboSent.SelectionLength = cboSent.Text.Length - cboSent.SelectionStart
        Else
            MsgBox("That was an invalid entry.", MsgBoxStyle.Critical)
            cboSent.SelectAll()
        End If
    End Sub

    'this sub does queue data entry if needed
    Sub QueueEntry(ByVal docId As String, ByVal ssn As String)
        Dim docIdRows() As DataRow = _generalDataSet.Tables("DocIDQueue").Select(String.Format("DocID = '{0}'", docId))
        'check if results were found
        If docIdRows.Length > 0 Then
            If AddLP9O(ssn, docIdRows(0).Item("Queue"), docIdRows(0).Item("CommentText")) = False Then
                MessageBox.Show("An error occurred while trying to update LP9O.  Please contact Systems Support.")
                End
            End If
        End If
    End Sub

    'adds a task to a queue
    Function AddLP9O(ByVal ssn As String, ByVal queue As String, Optional ByVal comment1 As String = "", Optional ByVal comment2 As String = "", Optional ByVal comment3 As String = "", Optional ByVal comment4 As String = "") As Boolean
        'keep trying to access LP9O until the system is ready or three tries
        For attempt As Integer = 1 To 3
            If CheckForText(22, 3, "44000") Then
                Exit For
            End If
            'wait if the system is not ready
            Thread.Sleep(New TimeSpan(0, 0, 5))
            FastPathInput("LP9OA" & ssn & ";;" & queue)
        Next attempt
        If Not CheckForText(22, 3, "44000") Then
            Return False
        End If

        'Enter info.
        PutText(12, 25, TimeOfDay().AddHours(2).ToString("HHmm"))
        PutText(16, 12, comment1)
        PutText(17, 12, comment2)
        PutText(18, 12, comment3)
        PutText(19, 12, comment4)
        Press("F6")
        Thread.Sleep(New TimeSpan(0, 0, 2))    'go figure why this has to be here but it wouldn't work otherwise
        'verify update
        Return CheckForText(22, 3, "48003")
    End Function

    Sub AddSuccessfulInfo2DB(ByVal accountNumber As String, ByVal fromCode As DocumentSource.Code)
        If String.IsNullOrEmpty(accountNumber) Then
            MessageBox.Show("An error occurred while trying to add reconciliation data to the database.  Please contact Systems Support.", "Error Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End If

        'update DB with doc id results
        Dim documentId As String = String.Empty
        If lblFinalDocID1.Text <> String.Empty Then
            documentId = lblFinalDocID1.Text
        End If
        If lblFinalDocID2.Text <> String.Empty Then
            documentId = lblFinalDocID2.Text
        End If
        If lblFinalDocID3.Text <> String.Empty Then
            documentId = lblFinalDocID3.Text
        End If
        If lblFinalDocID4.Text <> String.Empty Then
            documentId = lblFinalDocID4.Text
        End If
        If lblFinalDocID5.Text <> String.Empty Then
            documentId = lblFinalDocID5.Text
        End If
        If lblFinalDocID6.Text <> String.Empty Then
            documentId = lblFinalDocID6.Text
        End If

        Dim timeStamp As String = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")
        Dim sqlString As String = String.Format("INSERT INTO ProcessingData (UserName, AccountNumber, DocID, TimeOfTransaction, Source) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", _userName, accountNumber, documentId, timeStamp, New DocumentSource(fromCode).ToString())
        Try
            Using sqlClientCommand As New SqlCommand(sqlString, _actualSqlConnection)
                _actualSqlConnection.Open()
                sqlClientCommand.ExecuteNonQuery()
            End Using
        Finally
            _actualSqlConnection.Close()
        End Try
    End Sub


    'send an e-mail message using SMTP
    Public Function SendMail(ByVal messageTo As String, Optional ByVal messageSubject As String = "", Optional ByVal messageBody As String = "", Optional ByVal messageCC As String = "", Optional ByVal messageBCC As String = "", Optional ByVal messageAttachments As String = "") As Boolean
        Dim mail As New OSSMTP.SMTPSession()
        mail.Server = "mail.utahsbr.edu"

        'create message
        mail.MailFrom = Environment.UserName() + "@utahsbr.edu"
        If TestMode() Then
            mail.SendTo = Environment.UserName() + "@utahsbr.edu," + messageTo
        Else
            mail.SendTo = messageTo
        End If
        mail.CC = messageCC
        mail.BCC = messageBCC
        If TestMode() Then
            messageSubject = "THIS IS A TEST - " + messageSubject
        End If
        mail.MessageSubject = messageSubject
        mail.MessageText = messageBody

        'add attachments if there are any
        If messageAttachments.Length > 0 Then
            'split file names from string
            Dim attachments() As String = Split(messageAttachments, ",")

            'add attachments
            For Each attachment As String In attachments
                mail.Attachments.Add(Convert.ChangeType(attachment, TypeCode.Object))
            Next attachment
        End If

        'send message
        Dim attemptNumber As Integer = 0
        mail.SendEmail()
        While mail.Status <> "SMTP connection closed"
            Thread.Sleep(New TimeSpan(0, 0, 1))
            If attemptNumber = 5 Then
                Return False
            End If
            mail.SendEmail()
            attemptNumber += 1
        End While
        Return True
    End Function

    Function UserSelection() As String
        If rbDocType.Checked Then
            Return lblDocType.Text
        End If
        If rbDocIDLst.Checked Then
            Return lblDocIDLst.Text
        End If
        If rbDocIDType.Checked Then
            Return lblDocIDType.Text
        End If
        'Nothing is checked:
        Return String.Empty
    End Function

    'this function checks LP2C for reference and confirms ref name with user
    Private Function LP2CRefCheck(ByVal referenceId As String) As RefCheckResult
        FastPathInput(String.Format("LP2CI;{0}", referenceId))
        If CheckForText(1, 67, "REFERENCE MENU") = True Then
            Return RefCheckResult.NotValid           'ref id not valid
        End If

        'check with user that ref name is correct
        Dim interrogative As String = String.Format("   -First Name = {0}{1}   -Last Name = {2}{1}{1}Is this the reference?", GetText(4, 44, 12), Environment.NewLine, GetText(4, 5, 35))
        If MessageBox.Show(interrogative, "Check Reference", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Return RefCheckResult.Valid
        Else
            Return RefCheckResult.UserInvalidated
        End If
    End Function

    'Logfile writer for debugging.
    Private Sub Log(ByVal LogMessage As String)
        Using writer As IO.StreamWriter = IO.File.AppendText(TempDir + "DocIdLog.txt")
            writer.WriteLine(LogMessage)
            writer.Flush()
            writer.Close()
        End Using
    End Sub

    Private Sub CheckSSN()
        CompassOnly = False
        FastPathInput("TX3Z/ITX1JB;" + tbSSN.Text)
        If (CheckForText(1, 71, "TXX1R")) Then
            Dim acctId As String = tbSSN.Text
            If (acctId.Length = 9) Then
                FastPathInput("LP22I" + acctId)
            Else
                FastPathInput("LP22I;;;;;;" + acctId)
            End If
            If (CheckForText(1, 60, "PERSON NAME/ID SEARCH")) Then
                CompassOnly = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Set the label text to the doc id being passed in for each label
    ''' </summary>
    ''' <param name="docId1"></param>
    ''' <param name="docId2"></param>
    ''' <param name="docId3"></param>
    ''' <param name="docId4"></param>
    ''' <param name="docId5"></param>
    ''' <param name="docId6"></param>
    ''' <remarks></remarks>
    Private Sub UpdateDocIds(Optional ByVal docId1 As String = "", Optional ByVal docId2 As String = "", Optional ByVal docId3 As String = "", Optional ByVal docId4 As String = "", Optional ByVal docId5 As String = "", Optional ByVal docId6 As String = "")
        lblFinalDocID1.Text = docId1
        lblFinalDocID2.Text = docId2
        lblFinalDocID3.Text = docId3
        lblFinalDocID4.Text = docId4
        lblFinalDocID5.Text = docId5
        lblFinalDocID6.Text = docId6
    End Sub

    Private Sub ClearLabels()
        lblFinalDocID1.Text = ""
        lblFinalDocID2.Text = ""
        lblFinalDocID3.Text = ""
        lblFinalDocID4.Text = ""
        lblFinalDocID5.Text = ""
        lblFinalDocID6.Text = ""
    End Sub

End Class
