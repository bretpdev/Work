Imports System.Drawing
Public Class frmWhoAmI
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents txtZip As System.Windows.Forms.TextBox
    Friend WithEvents txtmi As System.Windows.Forms.TextBox
    Friend WithEvents txtlname As System.Windows.Forms.TextBox
    Friend WithEvents txtfname As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Address1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvSheep As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cboState As System.Windows.Forms.ComboBox
    Friend WithEvents lblState As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWhoAmI))
        Me.lvSheep = New System.Windows.Forms.ListView
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.Address1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblState = New System.Windows.Forms.Label
        Me.cboState = New System.Windows.Forms.ComboBox
        Me.txtZip = New System.Windows.Forms.TextBox
        Me.txtmi = New System.Windows.Forms.TextBox
        Me.txtlname = New System.Windows.Forms.TextBox
        Me.txtfname = New System.Windows.Forms.TextBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvSheep
        '
        Me.lvSheep.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSheep.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader3, Me.Address1, Me.ColumnHeader1, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lvSheep.FullRowSelect = True
        Me.lvSheep.Location = New System.Drawing.Point(8, 176)
        Me.lvSheep.Name = "lvSheep"
        Me.lvSheep.Size = New System.Drawing.Size(712, 248)
        Me.lvSheep.TabIndex = 2
        Me.lvSheep.UseCompatibleStateImageBehavior = False
        Me.lvSheep.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "SSN"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "First Name"
        Me.ColumnHeader7.Width = 69
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "MI"
        Me.ColumnHeader8.Width = 24
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Last Name"
        Me.ColumnHeader9.Width = 77
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "DOB"
        Me.ColumnHeader3.Width = 80
        '
        'Address1
        '
        Me.Address1.Text = "Address 1"
        Me.Address1.Width = 142
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Address 2"
        Me.ColumnHeader1.Width = 68
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "City"
        Me.ColumnHeader4.Width = 74
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "State"
        Me.ColumnHeader5.Width = 37
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Zip"
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOK.Location = New System.Drawing.Point(296, 432)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCancel.Location = New System.Drawing.Point(384, 432)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 23)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "First Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 48)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 23)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Last Name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 23)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Middle Initial"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 96)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(72, 23)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "State"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 120)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(72, 23)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Zip"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblState)
        Me.GroupBox1.Controls.Add(Me.cboState)
        Me.GroupBox1.Controls.Add(Me.txtZip)
        Me.GroupBox1.Controls.Add(Me.txtmi)
        Me.GroupBox1.Controls.Add(Me.txtlname)
        Me.GroupBox1.Controls.Add(Me.txtfname)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(712, 152)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search Criteria"
        '
        'lblState
        '
        Me.lblState.Location = New System.Drawing.Point(144, 96)
        Me.lblState.Name = "lblState"
        Me.lblState.Size = New System.Drawing.Size(304, 16)
        Me.lblState.TabIndex = 21
        '
        'cboState
        '
        Me.cboState.Location = New System.Drawing.Point(88, 96)
        Me.cboState.Name = "cboState"
        Me.cboState.Size = New System.Drawing.Size(48, 21)
        Me.cboState.TabIndex = 4
        '
        'txtZip
        '
        Me.txtZip.Location = New System.Drawing.Point(88, 120)
        Me.txtZip.MaxLength = 17
        Me.txtZip.Name = "txtZip"
        Me.txtZip.Size = New System.Drawing.Size(100, 20)
        Me.txtZip.TabIndex = 5
        '
        'txtmi
        '
        Me.txtmi.Location = New System.Drawing.Point(88, 72)
        Me.txtmi.MaxLength = 1
        Me.txtmi.Name = "txtmi"
        Me.txtmi.Size = New System.Drawing.Size(16, 20)
        Me.txtmi.TabIndex = 3
        '
        'txtlname
        '
        Me.txtlname.Location = New System.Drawing.Point(88, 48)
        Me.txtlname.Name = "txtlname"
        Me.txtlname.Size = New System.Drawing.Size(160, 20)
        Me.txtlname.TabIndex = 2
        '
        'txtfname
        '
        Me.txtfname.Location = New System.Drawing.Point(88, 24)
        Me.txtfname.Name = "txtfname"
        Me.txtfname.Size = New System.Drawing.Size(160, 20)
        Me.txtfname.TabIndex = 1
        '
        'btnSearch
        '
        Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearch.BackgroundImage = CType(resources.GetObject("btnSearch.BackgroundImage"), System.Drawing.Image)
        Me.btnSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.ForeColor = System.Drawing.Color.Black
        Me.btnSearch.Location = New System.Drawing.Point(496, 32)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(184, 96)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "Search"
        Me.ToolTip1.SetToolTip(Me.btnSearch, "Search")
        '
        'frmWhoAmI
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(728, 485)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lvSheep)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(736, 512)
        Me.Name = "frmWhoAmI"
        Me.Text = "Who Am I"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Structure Sheep
        Dim FName As String
        Dim LName As String
        Dim MI As String
        Dim DOB As String
        Dim Addr1 As String
        Dim Addr2 As String
        Dim City As String
        Dim ST As String
        Dim Zip As String
    End Structure
    Public SSN As String
    Private ds As DataSet

    Function getStateDesc(ByVal st As String) As String
        Dim r As DataRow
        getStateDesc = ""
        For Each r In ds.Tables(0).Rows
            If r.Item("Code") = st Then
                Return r.Item("Description")
            End If
        Next
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim x As Integer
        Dim lstSSN As New ArrayList
        SP.Q.FastPath("LP22I;" & txtlname.Text & ";" & txtfname.Text & ";" & txtmi.Text)
        If SP.Q.Check4Text(1, 65, "PERSON SELECTION") Then
            Do While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                For x = 5 To 17 Step 3
                    If SP.Q.GetText(x, 3, 1) <> "" Then
                        lstSSN.Add(SP.Q.GetText(x, 72, 9))
                    End If
                Next
                SP.Q.Hit("F8")
            Loop
            lvSheep.Items.Clear()
            For x = 0 To lstSSN.Count - 1
                SP.Q.FastPath("LP22I" & lstSSN.Item(x))
                Dim li As New System.Windows.Forms.ListViewItem
                li.Text = lstSSN.Item(x)
                li.SubItems.Add(SP.Q.GetText(4, 44, 12)) 'FName
                li.SubItems.Add(SP.Q.GetText(4, 60, 1)) 'MI
                li.SubItems.Add(SP.Q.GetText(4, 5, 35)) 'LName
                li.SubItems.Add(SP.Q.GetText(4, 72, 2) & "/" & SP.Q.GetText(4, 74, 2) & "/" & SP.Q.GetText(4, 76, 4)) 'DOB
                li.SubItems.Add(SP.Q.GetText(11, 9, 35)) 'Add1
                li.SubItems.Add(SP.Q.GetText(12, 9, 35)) 'Add2
                li.SubItems.Add(SP.Q.GetText(13, 9, 30)) 'City
                li.SubItems.Add(SP.Q.GetText(13, 52, 2)) 'State
                li.SubItems.Add(SP.Q.GetText(13, 60, 9)) 'Zip
                If (cboState.Text = "" Or SP.Q.GetText(13, 52, 2) = cboState.Text) _
                    And (txtZip.Text = "" Or SP.Q.GetText(13, 60, 9) = txtZip.Text) Then
                    lvSheep.Items.Add(li)
                End If
            Next
        ElseIf SP.Q.Check4Text(1, 62, "PERSON DEMOGRAPHICS") Then
            SSN = SP.Q.GetText(1, 9, 9)
            Me.Hide()
        ElseIf SP.Q.Check4Text(22, 3, "47013") Then
            SP.frmKnarlyDUDE.KnarlyDude("Well, That would include half of Russia and most of China. Lets narrow it down to the islands. Three characters are required for a wildcard search. Try again.", "Invalid Syntax", False)
            Exit Sub
        Else
            SP.frmKnarlyDUDE.KnarlyDude("Like, I think that is some kind of fish! Can you give me another name?", "No Matches Found", False)
            Exit Sub
        End If
    End Sub

    Private Sub cboState_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboState.SelectedIndexChanged
        lblState.Text = getStateDesc(cboState.Text)
    End Sub

    Private Sub frmWhoAmI_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'populate state combo
        Dim conn As SqlClient.SqlConnection
        If SP.Q.TestMode Then
            conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=BSYS;Integrated Security=SSPI;")
        Else
            conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=BSYS;Integrated Security=SSPI;")
        End If
        Dim da As New SqlClient.SqlDataAdapter("select * from GENR_LST_States", conn)
        ds = New DataSet
        da.Fill(ds)
        Dim r As DataRow
        For Each r In ds.Tables(0).Rows
            cboState.Items.Add(r.Item("Code"))
        Next
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If lvSheep.SelectedItems().Count > 0 Then
            SSN = lvSheep.SelectedItems(0).Text
            Me.Hide()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        SSN = ""
        Me.Hide()
    End Sub
End Class
