Imports System.Data.SqlClient

Public Class frmSelection
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal tTestMode As Boolean, ByRef tTILPMain As frmTILPMain, ByRef tTheUser As user)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        TestMode = tTestMode
        TILPMain = tTILPMain
        TheUser = tTheUser

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents rbSSN As System.Windows.Forms.RadioButton
    Friend WithEvents rbNameSearch As System.Windows.Forms.RadioButton
    Friend WithEvents gbSearchCrit As System.Windows.Forms.GroupBox
    Friend WithEvents tbLastName As System.Windows.Forms.TextBox
    Friend WithEvents tbFirstName As System.Windows.Forms.TextBox
    Friend WithEvents tbSSN As System.Windows.Forms.TextBox
    Friend WithEvents gbResults As System.Windows.Forms.GroupBox
    Friend WithEvents lvResults As System.Windows.Forms.ListView
    Friend WithEvents gbMain As System.Windows.Forms.GroupBox
    Friend WithEvents gbSearch As System.Windows.Forms.GroupBox
    Friend WithEvents btSearch As System.Windows.Forms.Button
    Friend WithEvents rbCNA As System.Windows.Forms.RadioButton
    Friend WithEvents rbSFA As System.Windows.Forms.RadioButton
    Friend WithEvents btContinue As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSelection))
        Me.rbSSN = New System.Windows.Forms.RadioButton
        Me.rbNameSearch = New System.Windows.Forms.RadioButton
        Me.gbSearchCrit = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbLastName = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbFirstName = New System.Windows.Forms.TextBox
        Me.tbSSN = New System.Windows.Forms.TextBox
        Me.gbResults = New System.Windows.Forms.GroupBox
        Me.lvResults = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.gbMain = New System.Windows.Forms.GroupBox
        Me.rbCNA = New System.Windows.Forms.RadioButton
        Me.rbSFA = New System.Windows.Forms.RadioButton
        Me.gbSearch = New System.Windows.Forms.GroupBox
        Me.btSearch = New System.Windows.Forms.Button
        Me.btContinue = New System.Windows.Forms.Button
        Me.gbSearchCrit.SuspendLayout()
        Me.gbResults.SuspendLayout()
        Me.gbMain.SuspendLayout()
        Me.gbSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'rbSSN
        '
        Me.rbSSN.Location = New System.Drawing.Point(8, 16)
        Me.rbSSN.Name = "rbSSN"
        Me.rbSSN.TabIndex = 1
        Me.rbSSN.Text = "SSN Search"
        '
        'rbNameSearch
        '
        Me.rbNameSearch.Location = New System.Drawing.Point(8, 88)
        Me.rbNameSearch.Name = "rbNameSearch"
        Me.rbNameSearch.TabIndex = 3
        Me.rbNameSearch.Text = "Name Search"
        '
        'gbSearchCrit
        '
        Me.gbSearchCrit.Controls.Add(Me.Label3)
        Me.gbSearchCrit.Controls.Add(Me.tbLastName)
        Me.gbSearchCrit.Controls.Add(Me.Label2)
        Me.gbSearchCrit.Controls.Add(Me.Label1)
        Me.gbSearchCrit.Controls.Add(Me.tbFirstName)
        Me.gbSearchCrit.Controls.Add(Me.tbSSN)
        Me.gbSearchCrit.Controls.Add(Me.rbSSN)
        Me.gbSearchCrit.Controls.Add(Me.rbNameSearch)
        Me.gbSearchCrit.Location = New System.Drawing.Point(8, 16)
        Me.gbSearchCrit.Name = "gbSearchCrit"
        Me.gbSearchCrit.Size = New System.Drawing.Size(300, 208)
        Me.gbSearchCrit.TabIndex = 1
        Me.gbSearchCrit.TabStop = False
        Me.gbSearchCrit.Text = "Search Criteria"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(24, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 16)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "SSN"
        '
        'tbLastName
        '
        Me.tbLastName.Location = New System.Drawing.Point(92, 144)
        Me.tbLastName.Name = "tbLastName"
        Me.tbLastName.Size = New System.Drawing.Size(200, 20)
        Me.tbLastName.TabIndex = 5
        Me.tbLastName.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(24, 148)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Last Name"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(24, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "First Name"
        '
        'tbFirstName
        '
        Me.tbFirstName.Location = New System.Drawing.Point(92, 120)
        Me.tbFirstName.Name = "tbFirstName"
        Me.tbFirstName.Size = New System.Drawing.Size(200, 20)
        Me.tbFirstName.TabIndex = 4
        Me.tbFirstName.Text = ""
        '
        'tbSSN
        '
        Me.tbSSN.Location = New System.Drawing.Point(92, 48)
        Me.tbSSN.MaxLength = 9
        Me.tbSSN.Name = "tbSSN"
        Me.tbSSN.TabIndex = 2
        Me.tbSSN.Text = ""
        '
        'gbResults
        '
        Me.gbResults.Controls.Add(Me.lvResults)
        Me.gbResults.Location = New System.Drawing.Point(444, 16)
        Me.gbResults.Name = "gbResults"
        Me.gbResults.Size = New System.Drawing.Size(296, 208)
        Me.gbResults.TabIndex = 3
        Me.gbResults.TabStop = False
        Me.gbResults.Text = "Results"
        '
        'lvResults
        '
        Me.lvResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvResults.FullRowSelect = True
        Me.lvResults.Location = New System.Drawing.Point(8, 24)
        Me.lvResults.MultiSelect = False
        Me.lvResults.Name = "lvResults"
        Me.lvResults.Size = New System.Drawing.Size(280, 176)
        Me.lvResults.TabIndex = 0
        Me.lvResults.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "SSN"
        Me.ColumnHeader1.Width = 75
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Name"
        Me.ColumnHeader2.Width = 200
        '
        'gbMain
        '
        Me.gbMain.Controls.Add(Me.rbCNA)
        Me.gbMain.Controls.Add(Me.rbSFA)
        Me.gbMain.Location = New System.Drawing.Point(4, 0)
        Me.gbMain.Name = "gbMain"
        Me.gbMain.Size = New System.Drawing.Size(748, 68)
        Me.gbMain.TabIndex = 1
        Me.gbMain.TabStop = False
        '
        'rbCNA
        '
        Me.rbCNA.Location = New System.Drawing.Point(8, 40)
        Me.rbCNA.Name = "rbCNA"
        Me.rbCNA.Size = New System.Drawing.Size(144, 24)
        Me.rbCNA.TabIndex = 2
        Me.rbCNA.Text = "Create New Account"
        '
        'rbSFA
        '
        Me.rbSFA.Location = New System.Drawing.Point(8, 16)
        Me.rbSFA.Name = "rbSFA"
        Me.rbSFA.Size = New System.Drawing.Size(128, 24)
        Me.rbSFA.TabIndex = 1
        Me.rbSFA.Text = "Search For Account"
        '
        'gbSearch
        '
        Me.gbSearch.Controls.Add(Me.gbSearchCrit)
        Me.gbSearch.Controls.Add(Me.gbResults)
        Me.gbSearch.Controls.Add(Me.btSearch)
        Me.gbSearch.Location = New System.Drawing.Point(4, 68)
        Me.gbSearch.Name = "gbSearch"
        Me.gbSearch.Size = New System.Drawing.Size(748, 232)
        Me.gbSearch.TabIndex = 3
        Me.gbSearch.TabStop = False
        '
        'btSearch
        '
        Me.btSearch.Image = CType(resources.GetObject("btSearch.Image"), System.Drawing.Image)
        Me.btSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btSearch.Location = New System.Drawing.Point(316, 64)
        Me.btSearch.Name = "btSearch"
        Me.btSearch.Size = New System.Drawing.Size(120, 23)
        Me.btSearch.TabIndex = 2
        Me.btSearch.Text = "Search"
        '
        'btContinue
        '
        Me.btContinue.Enabled = False
        Me.btContinue.Location = New System.Drawing.Point(324, 308)
        Me.btContinue.Name = "btContinue"
        Me.btContinue.Size = New System.Drawing.Size(120, 23)
        Me.btContinue.TabIndex = 4
        Me.btContinue.Text = "Continue"
        '
        'frmSelection
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(756, 360)
        Me.ControlBox = False
        Me.Controls.Add(Me.gbMain)
        Me.Controls.Add(Me.btContinue)
        Me.Controls.Add(Me.gbSearch)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(764, 368)
        Me.MinimumSize = New System.Drawing.Size(764, 368)
        Me.Name = "frmSelection"
        Me.Text = "Account Selection"
        Me.gbSearchCrit.ResumeLayout(False)
        Me.gbResults.ResumeLayout(False)
        Me.gbMain.ResumeLayout(False)
        Me.gbSearch.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private TestMode As Boolean
    Private TILPMain As frmTILPMain
    Private Conn As Data.SqlClient.SqlConnection
    Private TheUser As user

    Private Sub rbCNA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCNA.CheckedChanged
        Dim i As Integer
        Dim i2 As Integer
        If rbCNA.Checked Then
            While i < gbSearch.Controls.Count
                gbSearch.Controls(i).Enabled = False
                If gbSearch.Controls(i).GetType.ToString = "System.Windows.Forms.GroupBox" Then
                    i2 = 0
                    While i2 < CType(gbSearch.Controls(i), GroupBox).Controls.Count
                        CType(gbSearch.Controls(i), GroupBox).Controls(i2).Enabled = False
                        i2 = i2 + 1
                    End While
                End If
                i = i + 1
            End While
            gbSearch.Enabled = False
            btContinue.Enabled = True
            Me.AcceptButton = btContinue
            rbSSN.Checked = False
            rbNameSearch.Checked = False
        End If
    End Sub

    Private Sub rbSFA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSFA.CheckedChanged
        Dim i As Integer
        Dim i2 As Integer
        If rbSFA.Checked Then
            While i < gbSearch.Controls.Count
                'don't enable search button
                If gbSearch.Controls(i).Name <> "btSearch" Then
                    gbSearch.Controls(i).Enabled = True
                End If
                If gbSearch.Controls(i).GetType.ToString = "System.Windows.Forms.GroupBox" Then
                    i2 = 0
                    While i2 < CType(gbSearch.Controls(i), GroupBox).Controls.Count
                        'don't enable crit text boxes
                        If CType(gbSearch.Controls(i), GroupBox).Controls(i2).Name <> "tbSSN" And CType(gbSearch.Controls(i), GroupBox).Controls(i2).Name <> "tbFirstName" And CType(gbSearch.Controls(i), GroupBox).Controls(i2).Name <> "tbLastName" Then
                            CType(gbSearch.Controls(i), GroupBox).Controls(i2).Enabled = True
                        End If
                        i2 = i2 + 1
                    End While
                End If
                i = i + 1
            End While
            gbSearch.Enabled = True
            rbSSN.Checked = True 'default search by SSN
            tbSSN.Focus()
        End If
    End Sub

    Private Sub frmSelection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer
        Dim i2 As Integer
        'set up DB connection
        If TestMode Then
            'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            'if in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        'set up enabled controls
        While i < gbSearch.Controls.Count
            If gbSearch.Controls(i).GetType.ToString = "System.Windows.Forms.GroupBox" Then
                i2 = 0
                While i2 < CType(gbSearch.Controls(i), GroupBox).Controls.Count
                    If CType(gbSearch.Controls(i), GroupBox).Controls(i2).GetType.ToString <> "System.Windows.Forms.GroupBox" Then
                        CType(gbSearch.Controls(i), GroupBox).Controls(i2).Enabled = False
                    Else
                        CType(gbSearch.Controls(i), GroupBox).Controls(i2).Enabled = True
                    End If
                    i2 = i2 + 1
                End While
            Else
                gbSearch.Controls(i).Enabled = False
            End If
            i = i + 1
        End While
        rbSFA.Checked = True 'default search for account
        rbSSN.Checked = True 'default search by SSN
        'do authority check
        If TILPMain.TheUser.GetAccessLevel = 4 Then
            rbCNA.Enabled = False
        Else
            rbCNA.Enabled = True
        End If
    End Sub


    Private Sub btContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btContinue.Click
        HandleBorrowerSelection()
    End Sub

    Private Sub HandleBorrowerSelection()
        Dim WhoHasItLocked As String
        'Creating a new borrower record
        If rbCNA.Checked = False Then
            'be sure that one of the search results are selected
            If lvResults.SelectedItems.Count = 0 Then
                MessageBox.Show("You must select a borrower to continue.", "Please Select A Borrower", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            Else
                If IAmLocked(lvResults.SelectedItems(0).Text, TheUser.GetUID(), WhoHasItLocked) = False Then
                    'account isn't locked
                    TILPMain.selBor = New borrower(TestMode, rbCNA.Checked, lvResults.SelectedItems(0).Text)
                    TILPMain.selBor.LockAccount(TheUser.GetUID()) 'lock account
                    'do borrower sub object calls to load data from DB
                    TILPMain.selBor.borLoadDat() 'global borrower demographic info
                    TILPMain.selBor.BorLoans.DoDBLoad() 'loan info
                    TILPMain.selBor.BorDemo.loadDemo() 'borrower demographic info
                    TILPMain.selBor.BorRefs.DoDBLoad() 'borrower reference info
                    TILPMain.MenuOptionCoor(frmTILPMain.OpSelected.GeneralNav)
                Else
                    'account is locked
                    MessageBox.Show("Account " + lvResults.SelectedItems(0).Text + " has been locked by " + WhoHasItLocked + ".  Please try again later.", "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    Exit Sub
                End If
            End If
        Else
            TILPMain.MenuOptionCoor(frmTILPMain.OpSelected.NewBorr)
            TILPMain.selBor = New borrower(TestMode, rbCNA.Checked, "")
        End If
        'create borrower demographics form
        TILPMain.BorDem = New frmBorDemo(TILPMain.selBor, TestMode, TheUser, TILPMain)
        TILPMain.BorDem.MdiParent = TILPMain
        TILPMain.BorDem.Show()
        Me.Close()
    End Sub

    'handles when search button is clicked
    Private Sub btSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSearch.Click
        Dim CritStr As String = ""
        Dim Comm As New SqlClient.SqlCommand
        Dim Reader As SqlClient.SqlDataReader
        Dim IdxHldr As Integer
        Dim LVI As ListViewItem
        Comm.Connection = Conn
        While 0 < lvResults.Items.Count
            lvResults.Items.RemoveAt(0)
        End While
        If rbSSN.Checked Then
            'SSN search
            If tbSSN.TextLength <> 9 Then
                MessageBox.Show("You must provide an social security number for an SSN search.", "SSN Needed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            Else
                Conn.Open()
                Comm.CommandText = "SELECT SSN, FirstName + ' ' + LastName as TheName FROM BorrowerDat WHERE SSN = '" + tbSSN.Text + "'"
                Reader = Comm.ExecuteReader
                While Reader.Read
                    LVI = New ListViewItem(Reader("SSN").ToString)
                    LVI.SubItems.Add(Reader("TheName").ToString)
                    lvResults.Items.Add(LVI)
                End While
                Reader.Close()
                Conn.Close()
            End If
        ElseIf rbNameSearch.Checked Then
            'Name Search
            If tbFirstName.TextLength = 0 And tbLastName.TextLength = 0 Then
                MessageBox.Show("You must provide some type of name criteria for the search.", "Criteria Needed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            Else
                If tbFirstName.TextLength <> 0 Then
                    CritStr = "FirstName LIKE '" + tbFirstName.Text.Replace("'", "''") + "'"
                End If
                If tbLastName.TextLength <> 0 Then
                    If CritStr.Length = 0 Then
                        CritStr = "LastName LIKE '" + tbLastName.Text.Replace("'", "''") + "'"
                    Else
                        CritStr = CritStr + "AND LastName LIKE '" + tbLastName.Text.Replace("'", "''") + "'"
                    End If
                End If
                Conn.Open()
                Comm.CommandText = "SELECT SSN, FirstName + ' ' + LastName as TheName FROM BorrowerDat WHERE " + CritStr + " ORDER BY TheName"
                Reader = Comm.ExecuteReader
                While Reader.Read
                    LVI = New ListViewItem(Reader("SSN").ToString)
                    LVI.SubItems.Add(Reader("TheName").ToString)
                    lvResults.Items.Add(LVI)
                End While
                Reader.Close()
                Conn.Close()
            End If
        End If
        If lvResults.Items.Count = 0 Then
            btContinue.Enabled = False
            MessageBox.Show("The entered search criteria didn't yield results.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            btContinue.Enabled = True
            lvResults.Items(0).Selected = True
            lvResults.Focus()
        End If
    End Sub

    Private Sub rbSSN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSSN.CheckedChanged
        If rbSSN.Checked Then
            tbSSN.Enabled = True
            btSearch.Enabled = True
            tbFirstName.Enabled = False
            tbLastName.Enabled = False
        End If
    End Sub

    Private Sub rbNameSearch_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbNameSearch.CheckedChanged
        If rbNameSearch.Checked Then
            tbFirstName.Enabled = True
            tbLastName.Enabled = True
            btSearch.Enabled = True
            tbSSN.Enabled = False
        End If
    End Sub

    Public Function IAmLocked(ByVal SSN As String, ByVal UserID As String, ByRef WhoHasItLocked As String) As Boolean
        Dim Conn As SqlConnection
        Dim Comm As New SqlCommand
        'set up DB connection
        If TestMode Then
            'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            'if in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        Conn.Open()
        Comm.Connection = Conn
        'unlock account
        Comm.CommandText = "SELECT ByWho FROM LockedAccounts WHERE LockedSSN = '" + SSN + "' AND ByWho <> '" + UserID + "'"
        'check if nothing is returned then there is no lock record
        If Comm.ExecuteScalar() Is Nothing Then
            Conn.Close()
            Return False 'account is not locked
        Else
            WhoHasItLocked = Comm.ExecuteScalar()
            Conn.Close()
            Return True 'account is locked
        End If
    End Function

    Private Sub gbSearchCrit_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gbSearchCrit.Enter
        Me.AcceptButton = btSearch
    End Sub

    Private Sub gbResults_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gbResults.Enter
        Me.AcceptButton = btContinue
    End Sub

    Private Sub frmSelection_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        tbSSN.Focus()
    End Sub

    Private Sub lvResults_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvResults.DoubleClick
        HandleBorrowerSelection()
    End Sub
End Class
