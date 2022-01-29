Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Class LMBinFrmBtns
    Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'set autodude stateless var
        If AutoDudeTextBox Is Nothing Then
            AutoDudeTextBox = txtAuto
        Else
            txtAuto.Text = AutoDudeTextBox.Text
            txtAuto.ForeColor = AutoDudeTextBox.ForeColor
        End If
    End Sub

    'UserControl overrides dispose to clean up the component list.
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
    Friend WithEvents btnLogOff As System.Windows.Forms.Button
    Friend WithEvents btnReports As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnContinue As System.Windows.Forms.Button
    Friend WithEvents tbSSNOrAN As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ckbOverflow As System.Windows.Forms.CheckBox
    Friend WithEvents btnWhoAmI As System.Windows.Forms.Button
    Friend WithEvents btnForb As System.Windows.Forms.Button
    Friend WithEvents btnAutoDude As System.Windows.Forms.Button
    Friend WithEvents txtAuto As System.Windows.Forms.TextBox
    Friend WithEvents btnAcctMain As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnLogOff = New System.Windows.Forms.Button
        Me.btnReports = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnAcctMain = New System.Windows.Forms.Button
        Me.ckbOverflow = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnContinue = New System.Windows.Forms.Button
        Me.tbSSNOrAN = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtAuto = New System.Windows.Forms.TextBox
        Me.btnAutoDude = New System.Windows.Forms.Button
        Me.btnForb = New System.Windows.Forms.Button
        Me.btnWhoAmI = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLogOff
        '
        Me.btnLogOff.Location = New System.Drawing.Point(693, 30)
        Me.btnLogOff.Name = "btnLogOff"
        Me.btnLogOff.Size = New System.Drawing.Size(78, 23)
        Me.btnLogOff.TabIndex = 0
        Me.btnLogOff.Text = "Log Off"
        '
        'btnReports
        '
        Me.btnReports.Location = New System.Drawing.Point(613, 30)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Size = New System.Drawing.Size(78, 23)
        Me.btnReports.TabIndex = 1
        Me.btnReports.Text = "Reports"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnAcctMain)
        Me.GroupBox1.Controls.Add(Me.ckbOverflow)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.btnContinue)
        Me.GroupBox1.Controls.Add(Me.tbSSNOrAN)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(336, 64)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'btnAcctMain
        '
        Me.btnAcctMain.Location = New System.Drawing.Point(241, 22)
        Me.btnAcctMain.Name = "btnAcctMain"
        Me.btnAcctMain.Size = New System.Drawing.Size(85, 23)
        Me.btnAcctMain.TabIndex = 14
        Me.btnAcctMain.Text = "Acct. Maint."
        '
        'ckbOverflow
        '
        Me.ckbOverflow.Location = New System.Drawing.Point(37, 45)
        Me.ckbOverflow.Name = "ckbOverflow"
        Me.ckbOverflow.Size = New System.Drawing.Size(72, 16)
        Me.ckbOverflow.TabIndex = 13
        Me.ckbOverflow.TabStop = False
        Me.ckbOverflow.Text = "Overflow"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "SSN/Acct #"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnContinue
        '
        Me.btnContinue.Location = New System.Drawing.Point(153, 22)
        Me.btnContinue.Name = "btnContinue"
        Me.btnContinue.Size = New System.Drawing.Size(85, 23)
        Me.btnContinue.TabIndex = 3
        Me.btnContinue.Text = "Continue"
        '
        'tbSSNOrAN
        '
        Me.tbSSNOrAN.Location = New System.Drawing.Point(6, 24)
        Me.tbSSNOrAN.MaxLength = 10
        Me.tbSSNOrAN.Name = "tbSSNOrAN"
        Me.tbSSNOrAN.Size = New System.Drawing.Size(132, 20)
        Me.tbSSNOrAN.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtAuto)
        Me.GroupBox2.Controls.Add(Me.btnAutoDude)
        Me.GroupBox2.Controls.Add(Me.btnForb)
        Me.GroupBox2.Controls.Add(Me.btnWhoAmI)
        Me.GroupBox2.Controls.Add(Me.GroupBox1)
        Me.GroupBox2.Controls.Add(Me.btnLogOff)
        Me.GroupBox2.Controls.Add(Me.btnReports)
        Me.GroupBox2.Location = New System.Drawing.Point(4, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(778, 80)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        '
        'txtAuto
        '
        Me.txtAuto.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtAuto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAuto.ForeColor = System.Drawing.Color.Red
        Me.txtAuto.Location = New System.Drawing.Point(349, 54)
        Me.txtAuto.Name = "txtAuto"
        Me.txtAuto.ReadOnly = True
        Me.txtAuto.Size = New System.Drawing.Size(78, 20)
        Me.txtAuto.TabIndex = 10
        Me.txtAuto.TabStop = False
        Me.txtAuto.Text = "Off"
        Me.txtAuto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnAutoDude
        '
        Me.btnAutoDude.Location = New System.Drawing.Point(349, 30)
        Me.btnAutoDude.Name = "btnAutoDude"
        Me.btnAutoDude.Size = New System.Drawing.Size(78, 23)
        Me.btnAutoDude.TabIndex = 5
        Me.btnAutoDude.Text = "Auto DUDE"
        '
        'btnForb
        '
        Me.btnForb.Location = New System.Drawing.Point(511, 30)
        Me.btnForb.Name = "btnForb"
        Me.btnForb.Size = New System.Drawing.Size(78, 23)
        Me.btnForb.TabIndex = 4
        Me.btnForb.Text = "Forbearance"
        '
        'btnWhoAmI
        '
        Me.btnWhoAmI.Location = New System.Drawing.Point(430, 30)
        Me.btnWhoAmI.Name = "btnWhoAmI"
        Me.btnWhoAmI.Size = New System.Drawing.Size(78, 23)
        Me.btnWhoAmI.TabIndex = 3
        Me.btnWhoAmI.Text = "Who Am I"
        '
        'LMBinFrmBtns
        '
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "LMBinFrmBtns"
        Me.Size = New System.Drawing.Size(787, 84)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Shared AutoDudeTextBox As TextBox

    Private Sub btnLogOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogOff.Click
        SP.FastPath("LOG")
        SP.RIBM.Exit()
        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub btnReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReports.Click
        SP.frmUnderConstruct.ShowUnderConstruct()
    End Sub

    Private Function ExecuteList(ByVal CommandName As String, ByVal Param As SqlParameter, Optional ByVal Column As Integer = 0) As List(Of String)
        Dim connectionString As String = String.Format("Data Source={0};Initial Catalog=UDW;Integrated Security=SSPI;", IIf(SP.TestModeBool, "OPSDEV", "UHEAASQLDB"))
        Dim conn As SqlConnection = New SqlConnection(connectionString)
        conn.Open()
        Dim comm As SqlCommand = New SqlCommand(CommandName, conn)
        comm.CommandType = CommandType.StoredProcedure
        comm.Parameters.Add(Param)
        Dim results As List(Of String) = New List(Of String)()
        Dim reader As SqlDataReader = comm.ExecuteReader()
        While reader.Read()
            results.Add(reader(Column))
        End While
        conn.Close()
        Return results
    End Function

    Private Function IsCustomerServiceAccount(ByVal SsnOrAccountNum As String) As Boolean
        Try
            Dim ssn As String = SsnOrAccountNum
            If ssn.Length = 9 Then
                ssn = ExecuteList("[spGetAccountNumberFromSSN]", New SqlParameter("Ssn", ssn))(0)
            End If
            Dim guarantorIds As List(Of String) = ExecuteList("[spMD_GetCallForwardingData]", New SqlParameter("@AccountNumber", ssn), 1)
            Dim idsAreValid As Boolean = False
            If Not guarantorIds.Any() Then
                Return False
            End If
            For Each guarantor As String In guarantorIds
                If guarantor = "000749" Or guarantor = "I00059" Then
                    idsAreValid = True
                End If
            Next
            Return Not idsAreValid
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub btnContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        If IsCustomerServiceAccount(tbSSNOrAN.Text) Then
            MessageBox.Show("Please transfer this call to Customer Service")
            Return
        End If
        Dim NoACPBSVCall As Boolean
        'do AutoDUDE functionality
        If ckbOverflow.Checked Then 'use ssn provided by user
            If tbSSNOrAN.TextLength = 9 And Not IsNumeric(tbSSNOrAN.Text) Then
                tbSSNOrAN.Text = SP.ACSTranslation(tbSSNOrAN.Text)
            End If
            If tbSSNOrAN.TextLength >= 9 And IsNumeric(tbSSNOrAN.Text) Then
                SP.FastPath("TX3Z/ATC00" & tbSSNOrAN.Text)
                If SP.Q.Check4Text(23, 2, "01019") = False And SP.Q.Check4Text(23, 2, "03363") = False Then
                    SP.Q.PutText(19, 38, "1", True)
                Else
                    'No ACP BSV Call
                    NoACPBSVCall = True
                End If
            End If
            'note flag to send through based off ACP results
            If NoACPBSVCall Then
                CType(Me.ParentForm, frmLMBins).SetOptionSelected(frmLMBins.WhatIAmGoingToGiveYou.SSN4OverflowNotACPCall)
            Else
                CType(Me.ParentForm, frmLMBins).SetOptionSelected(frmLMBins.WhatIAmGoingToGiveYou.SSN4OverflowACPCall)
            End If
        Else
            CType(Me.ParentForm, frmLMBins).SetOptionSelected(frmLMBins.WhatIAmGoingToGiveYou.SSN)
        End If
        'note ssn
        CType(Me.ParentForm, frmLMBins).SetSSN(tbSSNOrAN.Text)
        CType(Me.ParentForm, frmLMBins).Hide()
    End Sub

    Private Sub btnWhoAmI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWhoAmI.Click
        Dim who As New SP.frmWhoAmI
        who.ShowDialog()
        tbSSNOrAN.Text = who.SSN
        If tbSSNOrAN.Text <> "" Then
            btnContinue.PerformClick()
        End If
    End Sub

    Private Sub btnForb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForb.Click
        SP.frmUnderConstruct.ShowUnderConstruct()
    End Sub

    Private Sub btnAutoDude_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutoDude.Click
        If txtAuto.Text = "On" Then
            txtAuto.Text = "Off"
            txtAuto.ForeColor = System.Drawing.Color.Red
        Else
            txtAuto.Text = "On"
            txtAuto.ForeColor = System.Drawing.Color.Green
        End If
        AutoDudeTextBox = txtAuto
        tbSSNOrAN.Focus()
    End Sub

    Private Sub btnAcctMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAcctMain.Click
        'note button clicked
        CType(Me.ParentForm, frmLMBins).SetOptionSelected(frmLMBins.WhatIAmGoingToGiveYou.AccountMaintenance)
        'note ssn
        CType(Me.ParentForm, frmLMBins).SetSSN(tbSSNOrAN.Text)
        CType(Me.ParentForm, frmLMBins).Hide()
    End Sub
End Class
