Imports System
Imports System.IO

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
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents clmFirst As System.Windows.Forms.ColumnHeader
    Friend WithEvents clmMI As System.Windows.Forms.ColumnHeader
    Friend WithEvents clmLast As System.Windows.Forms.ColumnHeader
    Friend WithEvents clmStreet As System.Windows.Forms.ColumnHeader
    Friend WithEvents clmCity As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtAccountID As System.Windows.Forms.TextBox
    Friend WithEvents txtSSN As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents txtLast As System.Windows.Forms.TextBox
    Friend WithEvents txtFirst As System.Windows.Forms.TextBox
    Friend WithEvents lstResults As System.Windows.Forms.ListView
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDataAdapter1 As System.Data.SqlClient.SqlDataAdapter
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.txtAccountID = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtSSN = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.txtLast = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtFirst = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.lstResults = New System.Windows.Forms.ListView
        Me.clmFirst = New System.Windows.Forms.ColumnHeader
        Me.clmMI = New System.Windows.Forms.ColumnHeader
        Me.clmLast = New System.Windows.Forms.ColumnHeader
        Me.clmStreet = New System.Windows.Forms.ColumnHeader
        Me.clmCity = New System.Windows.Forms.ColumnHeader
        Me.btnAdd = New System.Windows.Forms.Button
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SqlSelectCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlDeleteCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlDataAdapter1 = New System.Data.SqlClient.SqlDataAdapter
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnOK)
        Me.GroupBox1.Controls.Add(Me.txtAccountID)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtSSN)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(512, 40)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(432, 128)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Student Account Selection"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(144, 88)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(144, 23)
        Me.btnOK.TabIndex = 34
        Me.btnOK.Text = "OK"
        '
        'txtAccountID
        '
        Me.txtAccountID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountID.Location = New System.Drawing.Point(144, 48)
        Me.txtAccountID.Name = "txtAccountID"
        Me.txtAccountID.Size = New System.Drawing.Size(144, 20)
        Me.txtAccountID.TabIndex = 33
        Me.txtAccountID.Text = ""
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 20)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "Account ID"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSSN
        '
        Me.txtSSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSSN.Location = New System.Drawing.Point(144, 24)
        Me.txtSSN.Name = "txtSSN"
        Me.txtSSN.Size = New System.Drawing.Size(144, 20)
        Me.txtSSN.TabIndex = 31
        Me.txtSSN.Text = ""
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(16, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(120, 20)
        Me.Label10.TabIndex = 30
        Me.Label10.Text = "SSN"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnSearch)
        Me.GroupBox2.Controls.Add(Me.txtLast)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.txtFirst)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.lstResults)
        Me.GroupBox2.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(512, 200)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(432, 344)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Student Account Search"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(144, 88)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(144, 23)
        Me.btnSearch.TabIndex = 39
        Me.btnSearch.Text = "Search"
        '
        'txtLast
        '
        Me.txtLast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLast.Location = New System.Drawing.Point(144, 48)
        Me.txtLast.Name = "txtLast"
        Me.txtLast.Size = New System.Drawing.Size(144, 20)
        Me.txtLast.TabIndex = 38
        Me.txtLast.Text = ""
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 20)
        Me.Label2.TabIndex = 37
        Me.Label2.Text = "Last Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFirst
        '
        Me.txtFirst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFirst.Location = New System.Drawing.Point(144, 24)
        Me.txtFirst.Name = "txtFirst"
        Me.txtFirst.Size = New System.Drawing.Size(144, 20)
        Me.txtFirst.TabIndex = 36
        Me.txtFirst.Text = ""
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 20)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "First Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lstResults
        '
        Me.lstResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmFirst, Me.clmMI, Me.clmLast, Me.clmStreet, Me.clmCity})
        Me.lstResults.FullRowSelect = True
        Me.lstResults.Location = New System.Drawing.Point(16, 128)
        Me.lstResults.MultiSelect = False
        Me.lstResults.Name = "lstResults"
        Me.lstResults.Size = New System.Drawing.Size(400, 200)
        Me.lstResults.TabIndex = 2
        Me.lstResults.View = System.Windows.Forms.View.Details
        '
        'clmFirst
        '
        Me.clmFirst.Text = "First"
        Me.clmFirst.Width = 75
        '
        'clmMI
        '
        Me.clmMI.Text = "MI"
        Me.clmMI.Width = 30
        '
        'clmLast
        '
        Me.clmLast.Text = "Last"
        Me.clmLast.Width = 75
        '
        'clmStreet
        '
        Me.clmStreet.Text = "Street"
        Me.clmStreet.Width = 120
        '
        'clmCity
        '
        Me.clmCity.Text = "City"
        Me.clmCity.Width = 96
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(656, 584)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(144, 23)
        Me.btnAdd.TabIndex = 40
        Me.btnAdd.Text = "Add New Account"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(40, 40)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(432, 568)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox1.TabIndex = 41
        Me.PictureBox1.TabStop = False
        '
        'SqlDataAdapter1
        '
        Me.SqlDataAdapter1.DeleteCommand = Me.SqlDeleteCommand1
        Me.SqlDataAdapter1.InsertCommand = Me.SqlInsertCommand1
        Me.SqlDataAdapter1.SelectCommand = Me.SqlSelectCommand1
        Me.SqlDataAdapter1.UpdateCommand = Me.SqlUpdateCommand1
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(985, 646)
        Me.ControlBox = False
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Student Account Selection"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private sqlCmd As New SqlClient.SqlCommand
    Private sqlRdr As SqlClient.SqlDataReader

    Private Sub frmMain_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.LostFocus
        Me.WindowState = FormWindowState.Normal
    End Sub

    'display account by SSN or account ID
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click


        Dim dataAdapter As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet

        'warn user if SSN and account ID left blank
        If txtSSN.Text = "" And txtAccountID.Text = "" Then
            MsgBox("You must enter either the SSN or account ID.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        End If

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'get account information
        If txtSSN.Text <> "" Then
            sqlCmd.CommandText = "SELECT * FROM Account WHERE SSN = '" & Replace(txtSSN.Text, "-", "") & "' AND RowStatus = 'A'"
        Else
            sqlCmd.CommandText = "SELECT * FROM Account WHERE AcctID = '" & txtAccountID.Text & "' AND RowStatus = 'A'"
        End If
        dataAdapter = New SqlClient.SqlDataAdapter(sqlCmd)

        'fill data set if the account was found or warn user if account not found
        Try
            dataAdapter.Fill(dataSet)
        Catch ex As Exception
            MsgBox("No accounts were found for the SSN or account ID entered.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            dbConnection.Close()
            Exit Sub
        End Try

        'warn user if account not found
        If dataSet.Tables(0).Rows.Count = 0 Then
            MsgBox("No accounts were found for the SSN or account ID entered.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            dbConnection.Close()
            Exit Sub
        End If

        dbConnection.Close()

        'display account screen and clear search fields
        ShowForms.Students(dataSet)
        ClearFields()
    End Sub

    'search for account by first and/or last name
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim dataAdapter As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet

        dataSet.Clear()
        lstResults.Items.Clear()

        'warn user if search criteria left blank
        If txtFirst.Text = "" And txtLast.Text = "" Then
            MsgBox("You must enter at least a partial first or last name .", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        End If

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'search for account
        sqlCmd.CommandText = String.Format("EXEC spNameSearch '{0}', '{1}'", txtFirst.Text, txtLast.Text)

        dataAdapter = New SqlClient.SqlDataAdapter(sqlCmd)

        'fill data set if accounts were found or warn user if not
        Try
            dataAdapter.Fill(dataSet)
        Catch ex As Exception
            MsgBox("No accounts were found for the search criteria entered.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            dbConnection.Close()
            Exit Sub
        End Try

        'warn user if no accounts found
        If dataSet.Tables(0).Rows.Count = 0 Then
            MsgBox("No accounts were found for the search criteria entered.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            dbConnection.Close()
            Exit Sub
        End If

        dbConnection.Close()

        'populate selection list view
        Dim r As DataRow
        For Each r In dataSet.Tables(0).Rows
            lstResults.Items.Add(New MainFormSearchResult(r.Item("FName"), r.Item("MI"), r.Item("LName"), r.Item("Add1"), r.Item("City"), r.Item("AcctID")))
        Next
    End Sub

    'display the account selected
    Private Sub lstResults_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstResults.DoubleClick
        Dim dataAdapter As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim AcctID As String

        AcctID = CType(lstResults.SelectedItems(0), MainFormSearchResult).AccountID

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        sqlCmd.CommandText = "SELECT * FROM Account WHERE AcctID = '" & AcctID & "' AND RowStatus = 'A'"
        dataAdapter = New SqlClient.SqlDataAdapter(sqlCmd)
        dataAdapter.Fill(dataSet)

        dbConnection.Close()

        ShowForms.Students(dataSet)
        ClearFields()
    End Sub

    'add a new account
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim dataAdapter As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim newAcctID As String

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'determine new account ID by finding the first unused number starting with account zero for the current year and 
        newAcctID = Format(Today, "yy") & "00000"
        sqlCmd.CommandText = "SELECT Count(AcctID) AS AcctID FROM Account WHERE AcctID = '" & newAcctID & "'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        While Val(sqlRdr("AcctID")) > 0
            sqlRdr.Close()
            newAcctID = Trim(Str(Val(newAcctID) + 1))
            sqlCmd.CommandText = "SELECT Count(AcctID) AS AcctID FROM Account WHERE AcctID = '" & newAcctID & "'"
            sqlRdr = sqlCmd.ExecuteReader
            sqlRdr.Read()
        End While
        sqlRdr.Close()

        'sqlCmd.CommandText = "SELECT MAX(AcctID) AS AcctID FROM Account WHERE SUBSTRING(AcctID,1,2) = SUBSTRING(LTRIM(STR(YEAR(GETDATE()))),3,2)"
        'sqlRdr = sqlCmd.ExecuteReader
        'sqlRdr.Read()
        'If Not IsDBNull(sqlRdr("AcctID")) Then
        '    newAcctID = sqlRdr("AcctID")
        '    newAcctID = Str(Val(Mid(newAcctID, 3)) + 1)
        'Else
        '    newAcctID = "00000"
        'End If
        'sqlRdr.Close()
        'newAcctID = Format(Today, "yy") & CLng(newAcctID).ToString("00000")

        'add folders for documents
        Directory.CreateDirectory(DocPath & newAcctID & "\Apps")
        Directory.CreateDirectory(DocPath & newAcctID & "\Schedules\1")
        Directory.CreateDirectory(DocPath & newAcctID & "\Communications")

        'create new account record
        sqlCmd.CommandText = "INSERT INTO Account (AcctID, LastUpdateUser) VALUES ('" & newAcctID & "', '" & UserID & "')"
        sqlCmd.ExecuteNonQuery()

        'get new account information to fill dataset
        sqlCmd.CommandText = "SELECT * FROM Account WHERE AcctID = '" & newAcctID & "'"
        dataAdapter = New SqlClient.SqlDataAdapter(sqlCmd)
        dataAdapter.Fill(dataSet)

        dbConnection.Close()

        ShowForms.Students(dataSet)
        ClearFields()
    End Sub

    'clear fields
    Sub ClearFields()
        txtSSN.Text = ""
        txtAccountID.Text = ""
        txtLast.Text = ""
        txtFirst.Text = ""
        lstResults.Items.Clear()
    End Sub

    'set OK button to click when user hits Enter
    Private Sub txtAccountID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAccountID.TextChanged
        Me.AcceptButton = btnOK
    End Sub

    'set OK button to click when user hits Enter
    Private Sub txtSSN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSSN.TextChanged
        Me.AcceptButton = btnOK
    End Sub

    'set search button to click when user hits Enter
    Private Sub txtFirst_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFirst.TextChanged
        Me.AcceptButton = btnSearch
    End Sub

    'set search button to click when user hits Enter
    Private Sub txtLast_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLast.TextChanged
        Me.AcceptButton = btnSearch
    End Sub
End Class
