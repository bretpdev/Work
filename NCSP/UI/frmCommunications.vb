Imports System
Imports System.IO

Public Class frmCommunications
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
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents btnLink As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnStudentInfo As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSchedules As System.Windows.Forms.Button
    Friend WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents rdoOut As System.Windows.Forms.RadioButton
    Friend WithEvents label5 As System.Windows.Forms.Label
    Friend WithEvents rdoIn As System.Windows.Forms.RadioButton
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents label6 As System.Windows.Forms.Label
    Friend WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents txtCreated As System.Windows.Forms.TextBox
    Friend WithEvents txtUserID As System.Windows.Forms.TextBox
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnLast As System.Windows.Forms.Button
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents btnFirst As System.Windows.Forms.Button
    Friend WithEvents btnTransHistory As System.Windows.Forms.Button
    Friend WithEvents hdrHeader As NCSP.FormHeader
    Friend WithEvents gbData As System.Windows.Forms.GroupBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCommunications))
        Me.btnSave = New System.Windows.Forms.Button
        Me.gbData = New System.Windows.Forms.GroupBox
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.label5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.label6 = New System.Windows.Forms.Label
        Me.txtSubject = New System.Windows.Forms.TextBox
        Me.txtCreated = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtUserID = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.rdoOut = New System.Windows.Forms.RadioButton
        Me.rdoIn = New System.Windows.Forms.RadioButton
        Me.btnLink = New System.Windows.Forms.Button
        Me.btnView = New System.Windows.Forms.Button
        Me.btnStudentInfo = New System.Windows.Forms.Button
        Me.btnTransHistory = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnSchedules = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.btnLast = New System.Windows.Forms.Button
        Me.btnPrev = New System.Windows.Forms.Button
        Me.btnFirst = New System.Windows.Forms.Button
        Me.hdrHeader = New NCSP.FormHeader
        Me.gbData.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(824, 112)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(144, 23)
        Me.btnSave.TabIndex = 24
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "Save"
        '
        'gbData
        '
        Me.gbData.Controls.Add(Me.txtComments)
        Me.gbData.Controls.Add(Me.Label3)
        Me.gbData.Controls.Add(Me.label5)
        Me.gbData.Controls.Add(Me.Label1)
        Me.gbData.Controls.Add(Me.cmbType)
        Me.gbData.Controls.Add(Me.Label12)
        Me.gbData.Controls.Add(Me.label6)
        Me.gbData.Controls.Add(Me.txtSubject)
        Me.gbData.Controls.Add(Me.txtCreated)
        Me.gbData.Controls.Add(Me.Label18)
        Me.gbData.Controls.Add(Me.txtUserID)
        Me.gbData.Controls.Add(Me.Label11)
        Me.gbData.Controls.Add(Me.rdoOut)
        Me.gbData.Controls.Add(Me.rdoIn)
        Me.gbData.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbData.Location = New System.Drawing.Point(16, 72)
        Me.gbData.Name = "gbData"
        Me.gbData.Size = New System.Drawing.Size(784, 504)
        Me.gbData.TabIndex = 74
        Me.gbData.TabStop = False
        Me.gbData.Text = "Communication Record"
        '
        'txtComments
        '
        Me.txtComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.Location = New System.Drawing.Point(144, 176)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(440, 240)
        Me.txtComments.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 176)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 20)
        Me.Label3.TabIndex = 63
        Me.Label3.Text = "Comments"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'label5
        '
        Me.label5.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label5.Location = New System.Drawing.Point(16, 128)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(120, 20)
        Me.label5.TabIndex = 61
        Me.label5.Text = "Outgoing"
        Me.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 20)
        Me.Label1.TabIndex = 60
        Me.Label1.Text = "Incoming"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbType
        '
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbType.Items.AddRange(New Object() {"Account Maintenance", "Call", "Email", "Fax", "Letter", "Phone"})
        Me.cmbType.Location = New System.Drawing.Point(144, 80)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(288, 21)
        Me.cmbType.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(16, 80)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(120, 20)
        Me.Label12.TabIndex = 51
        Me.Label12.Text = "Type"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'label6
        '
        Me.label6.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label6.Location = New System.Drawing.Point(16, 152)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(120, 20)
        Me.label6.TabIndex = 57
        Me.label6.Text = "Subject"
        Me.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSubject
        '
        Me.txtSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubject.Location = New System.Drawing.Point(144, 152)
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.Size = New System.Drawing.Size(440, 20)
        Me.txtSubject.TabIndex = 3
        '
        'txtCreated
        '
        Me.txtCreated.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtCreated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCreated.Location = New System.Drawing.Point(144, 24)
        Me.txtCreated.Name = "txtCreated"
        Me.txtCreated.ReadOnly = True
        Me.txtCreated.Size = New System.Drawing.Size(144, 20)
        Me.txtCreated.TabIndex = 44
        Me.txtCreated.TabStop = False
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(16, 24)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(120, 20)
        Me.Label18.TabIndex = 43
        Me.Label18.Text = "Date/Time Created"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtUserID
        '
        Me.txtUserID.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtUserID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserID.Location = New System.Drawing.Point(144, 48)
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.ReadOnly = True
        Me.txtUserID.Size = New System.Drawing.Size(144, 20)
        Me.txtUserID.TabIndex = 50
        Me.txtUserID.TabStop = False
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(16, 48)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(120, 20)
        Me.Label11.TabIndex = 49
        Me.Label11.Text = "User ID"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rdoOut
        '
        Me.rdoOut.Location = New System.Drawing.Point(144, 128)
        Me.rdoOut.Name = "rdoOut"
        Me.rdoOut.Size = New System.Drawing.Size(104, 23)
        Me.rdoOut.TabIndex = 2
        '
        'rdoIn
        '
        Me.rdoIn.Location = New System.Drawing.Point(144, 104)
        Me.rdoIn.Name = "rdoIn"
        Me.rdoIn.Size = New System.Drawing.Size(104, 23)
        Me.rdoIn.TabIndex = 1
        '
        'btnLink
        '
        Me.btnLink.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLink.Location = New System.Drawing.Point(824, 144)
        Me.btnLink.Name = "btnLink"
        Me.btnLink.Size = New System.Drawing.Size(144, 23)
        Me.btnLink.TabIndex = 67
        Me.btnLink.TabStop = False
        Me.btnLink.Text = "Link Documents"
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Location = New System.Drawing.Point(824, 176)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(144, 23)
        Me.btnView.TabIndex = 66
        Me.btnView.TabStop = False
        Me.btnView.Text = "View Documents"
        '
        'btnStudentInfo
        '
        Me.btnStudentInfo.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStudentInfo.Location = New System.Drawing.Point(824, 488)
        Me.btnStudentInfo.Name = "btnStudentInfo"
        Me.btnStudentInfo.Size = New System.Drawing.Size(144, 23)
        Me.btnStudentInfo.TabIndex = 40
        Me.btnStudentInfo.TabStop = False
        Me.btnStudentInfo.Text = "Student Information"
        '
        'btnTransHistory
        '
        Me.btnTransHistory.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTransHistory.Location = New System.Drawing.Point(824, 552)
        Me.btnTransHistory.Name = "btnTransHistory"
        Me.btnTransHistory.Size = New System.Drawing.Size(144, 23)
        Me.btnTransHistory.TabIndex = 52
        Me.btnTransHistory.TabStop = False
        Me.btnTransHistory.Text = "Transaction History"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.Panel3.Controls.Add(Me.Label33)
        Me.Panel3.Controls.Add(Me.Label32)
        Me.Panel3.Controls.Add(Me.GroupBox8)
        Me.Panel3.Location = New System.Drawing.Point(0, 630)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1000, 24)
        Me.Panel3.TabIndex = 60
        '
        'Label33
        '
        Me.Label33.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label33.Location = New System.Drawing.Point(544, -2)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(424, 23)
        Me.Label33.TabIndex = 63
        Me.Label33.Text = "Communication Records"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label32
        '
        Me.Label32.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label32.Location = New System.Drawing.Point(16, -2)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(424, 23)
        Me.Label32.TabIndex = 62
        Me.Label32.Text = "New Century Scholarship Program"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox8
        '
        Me.GroupBox8.Location = New System.Drawing.Point(0, -8)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(992, 8)
        Me.GroupBox8.TabIndex = 61
        Me.GroupBox8.TabStop = False
        '
        'GroupBox9
        '
        Me.GroupBox9.Location = New System.Drawing.Point(0, 622)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(1000, 8)
        Me.GroupBox9.TabIndex = 61
        Me.GroupBox9.TabStop = False
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.SystemColors.ControlText
        Me.Panel4.Location = New System.Drawing.Point(0, 62)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(992, 1)
        Me.Panel4.TabIndex = 62
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(824, 80)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(144, 23)
        Me.btnAdd.TabIndex = 68
        Me.btnAdd.TabStop = False
        Me.btnAdd.Text = "Add"
        '
        'btnSchedules
        '
        Me.btnSchedules.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSchedules.Location = New System.Drawing.Point(824, 520)
        Me.btnSchedules.Name = "btnSchedules"
        Me.btnSchedules.Size = New System.Drawing.Size(144, 23)
        Me.btnSchedules.TabIndex = 69
        Me.btnSchedules.TabStop = False
        Me.btnSchedules.Text = "Schedules"
        '
        'btnNext
        '
        Me.btnNext.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNext.Image = CType(resources.GetObject("btnNext.Image"), System.Drawing.Image)
        Me.btnNext.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnNext.Location = New System.Drawing.Point(752, 584)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(24, 24)
        Me.btnNext.TabIndex = 76
        Me.btnNext.TabStop = False
        '
        'btnLast
        '
        Me.btnLast.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLast.Image = CType(resources.GetObject("btnLast.Image"), System.Drawing.Image)
        Me.btnLast.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnLast.Location = New System.Drawing.Point(776, 584)
        Me.btnLast.Name = "btnLast"
        Me.btnLast.Size = New System.Drawing.Size(24, 24)
        Me.btnLast.TabIndex = 75
        Me.btnLast.TabStop = False
        '
        'btnPrev
        '
        Me.btnPrev.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrev.Image = CType(resources.GetObject("btnPrev.Image"), System.Drawing.Image)
        Me.btnPrev.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnPrev.Location = New System.Drawing.Point(40, 584)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(24, 24)
        Me.btnPrev.TabIndex = 74
        Me.btnPrev.TabStop = False
        '
        'btnFirst
        '
        Me.btnFirst.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFirst.Image = CType(resources.GetObject("btnFirst.Image"), System.Drawing.Image)
        Me.btnFirst.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnFirst.Location = New System.Drawing.Point(16, 584)
        Me.btnFirst.Name = "btnFirst"
        Me.btnFirst.Size = New System.Drawing.Size(24, 24)
        Me.btnFirst.TabIndex = 73
        Me.btnFirst.TabStop = False
        '
        'hdrHeader
        '
        Me.hdrHeader.Location = New System.Drawing.Point(-1, -1)
        Me.hdrHeader.Name = "hdrHeader"
        Me.hdrHeader.Size = New System.Drawing.Size(994, 64)
        Me.hdrHeader.TabIndex = 77
        '
        'frmCommunications
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(992, 652)
        Me.Controls.Add(Me.hdrHeader)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnLast)
        Me.Controls.Add(Me.btnPrev)
        Me.Controls.Add(Me.btnFirst)
        Me.Controls.Add(Me.btnSchedules)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.btnStudentInfo)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnTransHistory)
        Me.Controls.Add(Me.gbData)
        Me.Controls.Add(Me.btnLink)
        Me.Controls.Add(Me.btnView)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1000, 680)
        Me.Name = "frmCommunications"
        Me.Text = "Communication Records"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.gbData.ResumeLayout(False)
        Me.gbData.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private DS As New Data.DataSet
    Private DA As Data.SqlClient.SqlDataAdapter
    Private FilterStr As String
    Private Record2BAdded As NewComRec
    Private DisplayedRow As Integer
    Private OnAddRecord As Boolean
    Private ControlsDisabled As Boolean

    Private Sub btnStudentInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStudentInfo.Click
        ShowForms.CloseForms()
    End Sub

    Private Sub btnSchedules_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSchedules.Click
        ShowForms.Schedules()
        Me.Close()
    End Sub

    Private Sub btnTransHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransHistory.Click
        ShowForms.TransHistory()
        Me.Close()
    End Sub

    Public Overloads Sub Show(ByVal tFilterStr As String)
        OnAddRecord = False 'assume that not adding
        FilterStr = tFilterStr 'init object level variable
        'get general data
        frmStudentsForm.StuInfo(hdrHeader.txtAccountID.Text, hdrHeader.txtStudentName.Text, hdrHeader.txtStatus.Text, hdrHeader.txtBalance.Text, hdrHeader.txtHoursRemaining.Text, hdrHeader.txtPaidSchedules.Text)
        'load dataset from DB
        DBLoad()
        DisplayedRow = (DS.Tables("Data").Rows.Count - 1) 'start on last record
        If FilterStr = "Add" Then
            If General.UserAccess = "Full Access" Then
                AddRecord()
            Else
                DisableAll()
                MsgBox("You must have full access to add a communications record.  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Information, "New Century Scholarship Program")
                'decide what navigation buttons to enable
                If DS.Tables("Data").Rows.Count > 0 Then
                    Navigate()
                    If DS.Tables("Data").Rows.Count = 1 Then
                        btnNext.Enabled = False
                        btnLast.Enabled = False
                        btnFirst.Enabled = False
                        btnPrev.Enabled = False
                    Else
                        'disable next and last buttons because navigation always starts on the last record
                        btnNext.Enabled = False
                        btnLast.Enabled = False
                        btnFirst.Enabled = True
                        btnPrev.Enabled = True
                    End If
                Else
                    'disable all buttons if nothing in data set
                    btnNext.Enabled = False
                    btnLast.Enabled = False
                    btnFirst.Enabled = False
                    btnPrev.Enabled = False
                    BlankForm()
                    MsgBox("No results found.", MsgBoxStyle.Information, "New Century Scholarship Program")
                End If
            End If
        Else
            DisableAll()
            If General.UserAccess = "Full Access" Then
                btnAdd.Enabled = True
            End If
            'decide what navigation buttons to enable
            If DS.Tables("Data").Rows.Count > 0 Then
                Navigate()
                If DS.Tables("Data").Rows.Count = 1 Then
                    btnNext.Enabled = False
                    btnLast.Enabled = False
                    btnFirst.Enabled = False
                    btnPrev.Enabled = False
                Else
                    'disable next and last buttons because navigation always starts on the last record
                    btnNext.Enabled = False
                    btnLast.Enabled = False
                    btnFirst.Enabled = True
                    btnPrev.Enabled = True
                End If
            Else
                'disable all buttons if nothing in data set
                btnNext.Enabled = False
                btnLast.Enabled = False
                btnFirst.Enabled = False
                btnPrev.Enabled = False
                BlankForm()
                MsgBox("No results found.", MsgBoxStyle.Information, "New Century Scholarship Program")
            End If
        End If

        'enable header and footer
        hdrHeader.Enabled = True
        Panel3.Enabled = True

        Me.Show()

    End Sub

    'populates dataset from DB
    Private Sub DBLoad()
        If FilterStr = "Add" Or FilterStr = "All" Then
            'set up data adapter
            DA = New Data.SqlClient.SqlDataAdapter("SELECT * FROM ComRec WHERE AcctID = '" + hdrHeader.txtAccountID.Text + "'", General.dbConnection.ConnectionString)
            DA.Fill(DS, "Data")
        Else
            DA = New Data.SqlClient.SqlDataAdapter("SELECT * FROM ComRec WHERE AcctID = '" + hdrHeader.txtAccountID.Text + "' AND ComType = '" + FilterStr + "'", General.dbConnection.ConnectionString)
            DA.Fill(DS, "Data")
        End If
    End Sub

    'coordinates adding a communcation record
    Private Sub AddRecord()
        'if user has full access allow the user to add a record
        If Record2BAdded Is Nothing Then
            'if record to be added hasn't been created then create it
            Record2BAdded = New NewComRec(hdrHeader.txtAccountID.Text)
        End If
        'display record on form
        Record2BAdded.PopulateForm(txtUserID.Text, txtCreated.Text, cmbType, rdoIn.Checked, rdoOut.Checked, txtSubject.Text, txtComments.Text)
        EnableAll()
        'enable and disable based off position in data set
        If DS.Tables("Data").Rows.Count = 0 Or DS.Tables("Data").Rows.Count = 1 Then
            btnNext.Enabled = False
            btnLast.Enabled = False
            btnFirst.Enabled = False
            btnPrev.Enabled = False
        ElseIf (DisplayedRow + 1) = DS.Tables("Data").Rows.Count Then
            btnNext.Enabled = False
            btnLast.Enabled = False
            btnFirst.Enabled = True
            btnPrev.Enabled = True
        ElseIf DisplayedRow = 0 Then
            btnNext.Enabled = True
            btnLast.Enabled = True
            btnFirst.Enabled = False
            btnPrev.Enabled = False
        Else
            btnNext.Enabled = True
            btnLast.Enabled = True
            btnFirst.Enabled = True
            btnPrev.Enabled = True
        End If
        btnAdd.Enabled = False
        btnView.Enabled = False
        OnAddRecord = True
    End Sub

    'disables form controls
    Private Sub DisableAll()
        Dim I As Integer
        While I < Me.Controls.Count
            If Me.Controls(I).Name <> "btnStudentInfo" And Me.Controls(I).Name <> "btnSchedules" And Me.Controls(I).Name <> "btnTransHistory" And Me.Controls(I).Name <> "gbData" And Me.Controls(I).Name <> "Panel2" Then
                Me.Controls(I).Enabled = False 'disable
            End If
            I = I + 1
        End While
        I = 0
        While I < gbData.Controls.Count
            If Me.Controls(I).Name <> "txtCreated" And Me.Controls(I).Name <> "txtUserID" Then
                If gbData.Controls(I).GetType.ToString = "System.Windows.Forms.Button" Then
                    gbData.Controls(I).Enabled = False 'disable all buttons
                ElseIf gbData.Controls(I).GetType.ToString = "System.Windows.Forms.TextBox" Then
                    CType(gbData.Controls(I), System.Windows.Forms.TextBox).ReadOnly = True
                    CType(gbData.Controls(I), System.Windows.Forms.TextBox).BackColor = System.Drawing.SystemColors.ScrollBar
                ElseIf gbData.Controls(I).GetType.ToString = "System.Windows.Forms.ComboBox" Then
                    CType(gbData.Controls(I), System.Windows.Forms.ComboBox).BackColor = System.Drawing.SystemColors.ScrollBar
                    ControlsDisabled = True
                ElseIf gbData.Controls(I).GetType.ToString = "System.Windows.Forms.RadioButton" Then
                    CType(gbData.Controls(I), System.Windows.Forms.RadioButton).Enabled = False
                End If
            End If
            I = I + 1
        End While
    End Sub

    'enables form controls
    Private Sub EnableAll(Optional ByVal ExceptSaveBtn As Boolean = False, Optional ByVal ExceptLinkBtn As Boolean = False, Optional ByVal ExceptViewBtn As Boolean = False)
        Dim I As Integer
        While I < Me.Controls.Count
            If Me.Controls(I).Name <> "txtCreated" And Me.Controls(I).Name <> "txtUserID" Then
                If General.UserAccess = "Read Only Access" And (Me.Controls(I).Name = "btnAdd" Or Me.Controls(I).Name = "btnLink" Or Me.Controls(I).Name = "btnSave") Then
                    Me.Controls(I).Enabled = False
                Else
                    Me.Controls(I).Enabled = True
                End If
            End If
            I = I + 1
        End While
        I = 0
        While I < gbData.Controls.Count
            If gbData.Controls(I).Name <> "txtCreated" And gbData.Controls(I).Name <> "txtUserID" Then
                If gbData.Controls(I).GetType.ToString = "System.Windows.Forms.Button" Then
                    gbData.Controls(I).Enabled = True 'enable all buttons
                ElseIf gbData.Controls(I).GetType.ToString = "System.Windows.Forms.TextBox" Then
                    CType(gbData.Controls(I), System.Windows.Forms.TextBox).ReadOnly = False
                    CType(gbData.Controls(I), System.Windows.Forms.TextBox).BackColor = System.Drawing.SystemColors.Window
                ElseIf gbData.Controls(I).GetType.ToString = "System.Windows.Forms.ComboBox" Then
                    CType(gbData.Controls(I), System.Windows.Forms.ComboBox).BackColor = System.Drawing.SystemColors.Window
                    ControlsDisabled = False
                ElseIf gbData.Controls(I).GetType.ToString = "System.Windows.Forms.RadioButton" Then
                    CType(gbData.Controls(I), System.Windows.Forms.RadioButton).Enabled = True
                End If
            End If
            I = I + 1
        End While
    End Sub

    'blanks values in form
    Private Sub BlankForm()
        txtUserID.Text = ""
        txtCreated.Text = ""
        cmbType.SelectedIndex = -1
        rdoIn.Checked = False
        rdoOut.Checked = False
        txtSubject.Text = ""
        txtComments.Text = ""
    End Sub

    'does general navigation
    Private Sub Navigate()
        txtUserID.Text = DS.Tables("Data").Rows(DisplayedRow).Item("UserID")
        txtCreated.Text = DS.Tables("Data").Rows(DisplayedRow).Item("ComRecDt")
        cmbType.SelectedIndex = cmbType.FindString(DS.Tables("Data").Rows(DisplayedRow).Item("ComType"))
        rdoIn.Checked = CBool(DS.Tables("Data").Rows(DisplayedRow).Item("Incoming"))
        rdoOut.Checked = CBool(DS.Tables("Data").Rows(DisplayedRow).Item("Outgoing"))
        txtSubject.Text = DS.Tables("Data").Rows(DisplayedRow).Item("Subject")
        txtComments.Text = DS.Tables("Data").Rows(DisplayedRow).Item("ComRecCmnt")
        btnView.Enabled = True
        'enable and disable based off position in data set
        If (DisplayedRow + 1) = DS.Tables("Data").Rows.Count Then
            btnNext.Enabled = False
            btnLast.Enabled = False
            btnFirst.Enabled = True
            btnPrev.Enabled = True
        ElseIf DisplayedRow = 0 Then
            btnNext.Enabled = True
            btnLast.Enabled = True
            btnFirst.Enabled = False
            btnPrev.Enabled = False
        Else
            btnNext.Enabled = True
            btnLast.Enabled = True
            btnFirst.Enabled = True
            btnPrev.Enabled = True
        End If
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        If OnAddRecord Then
            'capture data
            Record2BAdded.UpdateObject(cmbType.SelectedItem, rdoIn.Checked, rdoOut.Checked, txtSubject.Text, txtComments.Text)
            OnAddRecord = False
            DisableAll()
            If General.UserAccess = "Full Access" Then
                btnAdd.Enabled = True
            End If
        End If
        DisplayedRow = 0
        Navigate()
    End Sub

    Private Sub btnPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        If OnAddRecord Then
            'capture data
            Record2BAdded.UpdateObject(cmbType.SelectedItem, rdoIn.Checked, rdoOut.Checked, txtSubject.Text, txtComments.Text)
            OnAddRecord = False
            DisableAll()
            If General.UserAccess = "Full Access" Then
                btnAdd.Enabled = True
            End If
        End If
        DisplayedRow = (DisplayedRow - 1)
        Navigate()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If OnAddRecord Then
            'capture data
            Record2BAdded.UpdateObject(cmbType.SelectedItem, rdoIn.Checked, rdoOut.Checked, txtSubject.Text, txtComments.Text)
            OnAddRecord = False
            DisableAll()
            If General.UserAccess = "Full Access" Then
                btnAdd.Enabled = True
            End If
        End If
        DisplayedRow = (DisplayedRow + 1)
        Navigate()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        If OnAddRecord Then
            'capture data
            Record2BAdded.UpdateObject(cmbType.SelectedItem, rdoIn.Checked, rdoOut.Checked, txtSubject.Text, txtComments.Text)
            OnAddRecord = False
            DisableAll()
            If General.UserAccess = "Full Access" Then
                btnAdd.Enabled = True
            End If
        End If
        DisplayedRow = (DS.Tables("Data").Rows.Count - 1)
        Navigate()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        AddRecord()
    End Sub

    Private Sub btnLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLink.Click
        Dim FileOpener As New OpenFileDialog
        Dim NewFile As String = String.Empty
        FileOpener.Title = "Select File To Link"
        FileOpener.ShowDialog()
        If FileOpener.FileName <> "" Then
            'make sure that the file has an extension
            If FileOpener.FileName.Split("\")(FileOpener.FileName.Split("\").GetUpperBound(0)).IndexOf(".") = -1 Then
                MsgBox("The file you are trying to link doesn't have a file extention.  Please add one and try an link it again or contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Information, "New Century Scholarship Program")
            Else
                Record2BAdded.LinkDoc(FileOpener.FileName)
            End If
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'make sure needed data is populated
        If cmbType.SelectedIndex = -1 Then
            MsgBox("You must provide a communication type value.", MsgBoxStyle.Information, "New Century Scholarship Program")
            Exit Sub
        End If
        If rdoIn.Checked = False And rdoOut.Checked = False Then
            MsgBox("You must specify whether then communication was incoming or outgoing.", MsgBoxStyle.Information, "New Century Scholarship Program")
            Exit Sub
        End If
        If txtSubject.Text = "" Then
            MsgBox("You must provide a subject.", MsgBoxStyle.Information, "New Century Scholarship Program")
            Exit Sub
        End If
        If Record2BAdded.AllLinkedRecordsExist() = False Then
            Exit Sub
        End If
        'add record
        Record2BAdded.UpdateObject(cmbType.SelectedItem, rdoIn.Checked, rdoOut.Checked, txtSubject.Text, txtComments.Text)
        Record2BAdded.InsertIntoDB()
        Record2BAdded.CopyOverLinkedDocs()
        DisableAll() 'once a record is saved then lock everything
        'only allow user to view documents from the current record
        btnView.Enabled = True
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Dim FileOpener As New OpenFileDialog
        If Dir(DocPath & hdrHeader.txtAccountID.Text & "\Communications\*" + CDate(txtCreated.Text).ToString("MMddyyyyHHmmss") + ".*") = "" Then
            MsgBox("This record doesn't have any documents linked to it.", MsgBoxStyle.Information, "New Century Scholarship Program")
        Else
            FileOpener.InitialDirectory = DocPath & hdrHeader.txtAccountID.Text & "\Communications\"
            FileOpener.Filter = "Linked Files|*" + CDate(txtCreated.Text).ToString("MMddyyyyHHmmss") + ".*"
            FileOpener.Title = "Select Linked File To View"
            FileOpener.ShowDialog()
            If FileOpener.FileName <> "" Then
                Process.Start(FileOpener.FileName)
            End If
        End If
    End Sub

    Private Sub cmbType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.GotFocus
        If ControlsDisabled Then txtSubject.Focus()
    End Sub

End Class

Public Class NewComRec
    Private CRAcctID As String
    Private CRDateTime As DateTime
    Private CRUserID As String
    Private CRType As String
    Private CRIncoming As Boolean
    Private CROutgoing As Boolean
    Private CRSubject As String
    Private CRComments As String
    Private LinkDocs As ArrayList

    'constructor
    Public Sub New(ByVal tCRAcctID As String)
        CRDateTime = DateTime.Now()
        CRAcctID = tCRAcctID
        CRUserID = General.UserID
        LinkDocs = New ArrayList
    End Sub

    'collect dat to store in object
    Public Sub UpdateObject(ByVal tCRType As String, ByVal tCRIncoming As Boolean, ByVal tCROutgoing As Boolean, ByVal tCRSubject As String, ByVal tCRComments As String)
        CRType = tCRType
        CRIncoming = tCRIncoming
        CROutgoing = tCROutgoing
        CRSubject = tCRSubject
        CRComments = tCRComments
    End Sub

    'retrieve data to populate form
    Public Sub PopulateForm(ByRef tCRUserID As String, ByRef tCRDateTime As String, ByRef tCRType As ComboBox, ByRef tCRIncoming As Boolean, ByRef tCROutgoing As Boolean, ByRef tCRSubject As String, ByRef tCRComments As String)
        tCRUserID = CRUserID
        tCRType.SelectedIndex = tCRType.FindString(CRType)
        tCRIncoming = CRIncoming
        tCROutgoing = CROutgoing
        tCRSubject = CRSubject
        tCRComments = CRComments
        tCRDateTime = CRDateTime.ToString
    End Sub

    'insert communication record into DB
    Public Sub InsertIntoDB()
        ''General.AddCommunications(CRAcctID, CRDateTime.ToString, CRIncoming, CROutgoing, CRType, CRComments.Replace("'", "''"), CRSubject.Replace("'", "''"))
        General.AddCommunications(CRAcctID, CRDateTime.ToString, CRIncoming, CROutgoing, CRType, CRComments, CRSubject)
    End Sub

    'add document to list of linked documents
    Public Sub LinkDoc(ByVal DocPath As String)
        Dim I As Integer
        If LinkDocs.Contains(DocPath) = False Then
            'be sure that all file names are unique
            While I < LinkDocs.Count
                If LinkDocs(I).ToString().Split("\")(LinkDocs(I).ToString().Split("\").GetUpperBound(0)) = DocPath.Split("\")(DocPath.Split("\").GetUpperBound(0)) Then
                    MsgBox("Please rename """ + DocPath.Split("\")(DocPath.Split("\").GetUpperBound(0)) + """.  There is a file linked to the communication record which already has that name.", MsgBoxStyle.Information, "New Century Scholarship Program")
                    Exit Sub
                End If
                I = I + 1
            End While
            LinkDocs.Add(DocPath)
        Else
            MsgBox("""" + DocPath + """ has already been linked to this communication record.  Please link all other files.", MsgBoxStyle.Information, "New Century Scholarship Program")
        End If
    End Sub

    'makes sure that all documents still exist
    Public Function AllLinkedRecordsExist() As Boolean
        Dim I As Integer
        If LinkDocs.Count = 0 Then
            Return True
        Else
            While I < LinkDocs.Count
                If File.Exists(LinkDocs(I).ToString()) = False Then
                    MsgBox("""" + DocPath + """ appears to have been moved or deleted.  The save operation has been cancelled and all linked documents have been cleared from this communication record.  Please try relinking all documents and saving the record again or contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
                    LinkDocs.Clear() 'clear array list
                    Return False
                End If
                I = I + 1
            End While
            Return True
        End If
    End Function

    'copys linked docs to system folder
    Public Sub CopyOverLinkedDocs()
        Dim I As Integer
        Dim TheFileName As String
        Dim NewFileName As String
        While I < LinkDocs.Count
            TheFileName = LinkDocs(I).ToString().Split("\")(LinkDocs(I).ToString().Split("\").GetUpperBound(0))
            'insert time date stamp 
            NewFileName = TheFileName.Insert(TheFileName.LastIndexOf("."), CRDateTime.ToString("_MMddyyyyHHmmss"))
            Try
                File.Move(LinkDocs(I).ToString(), General.DocPath + CRAcctID + "\Communications\" + NewFileName)
            Catch ex As Exception
                MsgBox("The file was not moved to the student's folder for the following reason:  " & ex.Message & "  Please try again or contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            End Try
            I = I + 1
        End While
    End Sub

End Class


