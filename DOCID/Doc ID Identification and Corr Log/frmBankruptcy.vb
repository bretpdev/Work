Imports System.Collections.Generic
Imports System.IO

Public Class frmBankruptcy
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'load Lender information
        Dim LenderName As String = String.Empty
        Dim LenderID As String = String.Empty
        FileOpen(1, "X:\Sessions\Lists\LenderList.txt", OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
        While Not EOF(1)
            Input(1, LenderID)
            Input(1, LenderName)
            cbLenderList.Items.Add(LenderName & " -- " & LenderID)
        End While
        FileClose(1)
        cbLenderList.Items.Add("Non-Lender")
        'get user id for bankruptcy log
        FastPathInput("LP40I")
        Press("Enter")
        _uid = GetText(3, 14, 7)
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
    Friend WithEvents cbLenderList As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbList As System.Windows.Forms.RadioButton
    Friend WithEvents rbType As System.Windows.Forms.RadioButton
    Friend WithEvents tbLender As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmBankruptcy))
        Me.cbLenderList = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbLender = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.rbList = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbType = New System.Windows.Forms.RadioButton
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbLenderList
        '
        Me.cbLenderList.Enabled = False
        Me.cbLenderList.Location = New System.Drawing.Point(24, 40)
        Me.cbLenderList.Name = "cbLenderList"
        Me.cbLenderList.Size = New System.Drawing.Size(280, 21)
        Me.cbLenderList.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(296, 23)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Please select a lender,"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(296, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "or enter a lender."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbLender
        '
        Me.tbLender.Enabled = False
        Me.tbLender.Location = New System.Drawing.Point(24, 88)
        Me.tbLender.Name = "tbLender"
        Me.tbLender.Size = New System.Drawing.Size(280, 20)
        Me.tbLender.TabIndex = 3
        Me.tbLender.Text = ""
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(88, 136)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(72, 24)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        '
        'rbList
        '
        Me.rbList.Location = New System.Drawing.Point(8, 40)
        Me.rbList.Name = "rbList"
        Me.rbList.Size = New System.Drawing.Size(16, 24)
        Me.rbList.TabIndex = 5
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbType)
        Me.GroupBox1.Controls.Add(Me.rbList)
        Me.GroupBox1.Controls.Add(Me.tbLender)
        Me.GroupBox1.Controls.Add(Me.cbLenderList)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(312, 120)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        '
        'rbType
        '
        Me.rbType.Location = New System.Drawing.Point(8, 88)
        Me.rbType.Name = "rbType"
        Me.rbType.Size = New System.Drawing.Size(16, 24)
        Me.rbType.TabIndex = 6
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(168, 136)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        '
        'frmBankruptcy
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(328, 166)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnOK)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(336, 193)
        Me.MinimumSize = New System.Drawing.Size(336, 193)
        Me.Name = "frmBankruptcy"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bankruptcy Check"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _ssn As String
    Private _formWasCancelledOrNoDocIdWasSelected As Boolean
    Private _ssnIsFoundOnOneLink As Boolean
    Private _pleaseWaitForm As New PleaseWait()
    Private _firstName As String
    Private _lastName As String
    Private _calledForBKRPFunc As Boolean
    Private _uid As String
    Private _docSource As DocumentSource
    Private _userSelection As String

    Public ReadOnly Property FormWasCancelledOrNoDocIdWasSelected() As Boolean
        Get
            Return _formWasCancelledOrNoDocIdWasSelected
        End Get
    End Property

    Public Shadows Sub Show(ByVal userSelection As String, ByVal docSource As DocumentSource, ByVal ssn As String, Optional ByVal ssnWasFoundOnOneLink As Boolean = True, Optional ByVal firstName As String = "", Optional ByVal lastName As String = "")
        _ssn = ssn
        _ssnIsFoundOnOneLink = ssnWasFoundOnOneLink
        _firstName = firstName
        _lastName = lastName
        _calledForBKRPFunc = False
        _formWasCancelledOrNoDocIdWasSelected = False
        _docSource = docSource
        _userSelection = userSelection
        Me.ShowDialog()
    End Sub

    Public Shadows Sub Show(ByVal userSelection As String, ByVal docSource As DocumentSource, Optional ByVal calledForBKRPFunc As Boolean = True)
        _calledForBKRPFunc = calledForBKRPFunc
        _formWasCancelledOrNoDocIdWasSelected = False
        _docSource = docSource
        _userSelection = userSelection
        Me.ShowDialog()
    End Sub

    Private Sub rbList_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbList.CheckedChanged
        If rbList.Checked Then
            cbLenderList.Enabled = True
            tbLender.Enabled = False
            tbLender.Clear()
        End If
    End Sub

    Private Sub rbType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbType.CheckedChanged
        If rbType.Checked Then
            cbLenderList.Enabled = False
            cbLenderList.SelectedIndex = -1
            tbLender.Enabled = True
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        _formWasCancelledOrNoDocIdWasSelected = True
        Me.Hide()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        'check if needed information is provided
        If cbLenderList.SelectedIndex = -1 AndAlso tbLender.TextLength = 0 Then
            MessageBox.Show("You must select or type in a lender.")
            Return
        End If

        _pleaseWaitForm.Show()
        _pleaseWaitForm.Refresh()
        If _calledForBKRPFunc = False Then
            BankruptcyProc(_ssn, _userSelection, _docSource)
            Me.Hide()
        Else 'if called for who am I and bankruptcy
            Me.Hide()
            Dim commandLineSwitches() As String = RIBM.CommandLineSwitches.Replace("""", "").Split("\")
            ActivatePreviousInstance(commandLineSwitches.Last())
            RIBM.RunMacro("SP.WhoAmI.Main", "Doc ID Bankruptcy")
            'Process the borrower identified by Who Am I.
            Dim resultWasFound As Boolean = False
            If CheckForText(1, 69, "DEMOGRAPHICS") Then
                resultWasFound = True
                _ssn = GetText(3, 23, 9)
                _firstName = GetText(4, 44, 12)
                _lastName = GetText(4, 5, 35)
            ElseIf CheckForText(1, 71, "TXX1R") Then
                resultWasFound = True
                _ssn = GetText(3, 12, 11).Replace(" ", "")
                _firstName = GetText(4, 34, 13)
                _lastName = GetText(4, 6, 23)
            Else
                _ssnIsFoundOnOneLink = False
                _ssn = ""
                _firstName = GetText(9, 16, 13).Replace("_", "")
                _lastName = GetText(8, 16, 23).Replace("_", "")
                BankruptcyProc(_ssn, _userSelection, _docSource)
            End If
            'if results aren't found then let user know that the documentation can be shredded
            If resultWasFound = False Then
                _formWasCancelledOrNoDocIdWasSelected = True
            End If
        End If
        _pleaseWaitForm.Hide()
    End Sub

    'this sub does the main BKRUPCY processing
    Sub BankruptcyProc(ByVal ssn As String, ByVal userSelection As String, ByVal docSource As DocumentSource)
        'In cases where the selected item contains the " -- " separator, we only use the part after the separator.
        'Define the separator as a string to help pick out the desired substring.
        Const SEPARATOR As String = " -- "

        'process based off LP22 check in main functionality
        Dim firstName As String = String.Empty
        Dim lastName As String = String.Empty
        If _ssnIsFoundOnOneLink Then
            'gather the first and last name from LP22
            FastPathInput(String.Format("LP22I{0}", ssn))
            Dim accountNumber As String = GetText(3, 60, 13)
            lastName = GetText(4, 5, 35)
            firstName = GetText(4, 44, 12)
            LG10Processing(ssn, userSelection, docSource)
        Else
            'just get the first and last name from the user
            If _firstName = String.Empty Then
                firstName = InputBox("Please enter the first name.", "First Name")
                If firstName = String.Empty Then
                    Me.Hide()
                    _pleaseWaitForm.Hide()
                    Return
                End If
            End If
            If _lastName = String.Empty Then
                lastName = InputBox("Please enter the last name.", "Last Name")
                If lastName = String.Empty Then
                    Me.Hide()
                    _pleaseWaitForm.Hide()
                    Return
                End If
            End If
            If _calledForBKRPFunc = False Then
                _formWasCancelledOrNoDocIdWasSelected = True
                MessageBox.Show("The documentation may be shredded.", "Shred", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If

        'append to log files
        If rbList.Checked Then
            'check if there is a delimiter
            If cbLenderList.SelectedItem.ToString().Contains(SEPARATOR) Then
                'has lender ID listed
                'Get the selected item in string form.
                Dim itemString As String = cbLenderList.SelectedItem.ToString()
                UpdateBankruptcyLogs(ssn, firstName, lastName, itemString.Substring(itemString.IndexOf(SEPARATOR) + SEPARATOR.Length))
            Else 'Non-Lender option
                UpdateBankruptcyLogs(ssn, firstName, lastName, "Other")
            End If
        Else
            UpdateBankruptcyLogs(ssn, firstName, lastName, tbLender.Text)
        End If
    End Sub

    Private Sub LG10Processing(ByVal ssn As String, ByVal userSelection As String, ByVal docSource As DocumentSource)
        Dim DEFINITE_UHEAA_CODES() As String = {"CR", "CP"}
        Dim POSSIBLE_UHEAA_CODES() As String = {"DA", "FB", "IA", "ID", "IG", "IM", "RP", "UA", "UB", "UC", "UD", "UI"}

        FastPathInput(String.Format("LG10I{0}", ssn))
        If CheckForText(22, 3, "47004") Then
            _formWasCancelledOrNoDocIdWasSelected = True
            MessageBox.Show("There are no open loans for the SSN entered.  The bankruptcy document may be shredded.", "No Open Loans", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim uheaaWasFound As Boolean
        Dim nelnetWasFound As Boolean
        Dim sallieMaeWasFound As Boolean
        If CheckForText(1, 53, "LOAN BWR STATUS RECAP SELECT") = False Then  'if a target screen is displayed
            Dim row As Integer = 11
            While CheckForText(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                If DEFINITE_UHEAA_CODES.Contains(GetText(row, 59, 2)) Then
                    uheaaWasFound = True
                ElseIf POSSIBLE_UHEAA_CODES.Contains(GetText(row, 59, 2)) AndAlso CheckForText(5, 27, "700126") Then
                    uheaaWasFound = True
                Else
                    Dim amountText As String = GetText(row, 47, 9)
                    Dim amount As Double = 0
                    If (amountText.Length > 0 AndAlso IsNumeric(amountText)) Then amount = Double.Parse(amountText)
                    If amount <> 0 AndAlso CheckForText(5, 27, "700121") Then
                        nelnetWasFound = True
                    End If
                    If amount <> 0 AndAlso CheckForText(5, 27, "700191") Then
                        sallieMaeWasFound = True
                    End If
                End If
                row += 1
                If row = 21 Then
                    row = 11
                    Press("F8")
                End If
            End While
        Else 'if a selection screen is displayed
            Dim masterRow As Integer = 7
            Dim lookupRow As Integer = 11
            While CheckForText(20, 3, "46004 NO MORE DATA TO DISPLAY") = False
                PutText(19, 15, GetText(masterRow, 5, 2))
                If Len(GetText(masterRow, 5, 2)) = 1 Then
                    Press("End")
                End If
                Press("Enter")
                While CheckForText(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                    If DEFINITE_UHEAA_CODES.Contains(GetText(lookupRow, 59, 2)) Then
                        uheaaWasFound = True
                    ElseIf POSSIBLE_UHEAA_CODES.Contains(GetText(lookupRow, 59, 2)) AndAlso CheckForText(5, 27, "700126") Then
                        uheaaWasFound = True
                    Else
                        Dim amountText As String = GetText(lookupRow, 47, 9)
                        Dim amount As Double = 0
                        If (amountText.Length > 0 AndAlso IsNumeric(amountText)) Then amount = Double.Parse(amountText)
                        If amount <> 0 AndAlso CheckForText(5, 27, "700121") Then
                            nelnetWasFound = True
                        End If
                        If amount <> 0 AndAlso CheckForText(5, 27, "700191") Then
                            sallieMaeWasFound = True
                        End If
                    End If
                    lookupRow += 1
                    If lookupRow = 21 Then
                        lookupRow = 11
                        Press("F8")
                    End If
                End While
                Press("F12")
                lookupRow = 11
                masterRow += 1
                If CheckForText(masterRow, 46, "      ") Then
                    Press("F8")
                    masterRow = 7
                End If
            End While
        End If

        If uheaaWasFound Then
            _formWasCancelledOrNoDocIdWasSelected = False
            Return
        End If

        If sallieMaeWasFound OrElse nelnetWasFound Then
            Dim agencyList As New List(Of String)()
            If sallieMaeWasFound Then
                agencyList.Add("Sallie Mae")
            End If
            If nelnetWasFound Then
                agencyList.Add("Nelnet")
            End If
            Dim agencyString As String = String.Join(" and ", agencyList.ToArray())

            If (frmMain.CompassOnly = False) Then
                AddLP50(Date.Now, ssn, "MDCID", "AM", "10", String.Format("{0} forwarded to {1} FROM:{2}", userSelection, agencyString, docSource.ToString()))
            Else
                ATD22AllLoans(ssn, "MDCID", "Bankruptcy document received.")
            End If
            MessageBox.Show(String.Format("Please forward to {0}.", agencyString), "Forward", MessageBoxButtons.OK, MessageBoxIcon.Information)
            _formWasCancelledOrNoDocIdWasSelected = True
            Return
        End If

        'default is to show ASBKP as doc id
        _formWasCancelledOrNoDocIdWasSelected = False
    End Sub

    'this function updates the network and local logs
    Private Sub UpdateBankruptcyLogs(ByVal ssn As String, ByVal firstName As String, ByVal lastName As String, ByVal lender As String)
        'update master file
        Dim bankruptcyLogDirectory As String = "X:\PADD\Bankruptcy\Log\"
        If TestMode() Then bankruptcyLogDirectory += "Test\"
        Dim bankruptcyLogFile As String = "BankruptcyNotificationLog.txt"
        While True  'try and access file until successful
            Try
                Using bankruptcyLogWriter As New StreamWriter(bankruptcyLogDirectory + bankruptcyLogFile)
                    bankruptcyLogWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6}", ssn, firstName, lastName, Date.Now.ToString("MM/dd/yyyy"), lender, _uid, Date.Now.ToShortTimeString())
                    bankruptcyLogWriter.Close()
                End Using
                Exit While  'once the file can be accessed then exit loop
            Catch ex As Exception
                'Wait one second and continue the loop.
                System.Threading.Thread.Sleep(New TimeSpan(0, 0, 1))
            End Try
        End While

        'update or create local file
        bankruptcyLogFile = "BankruptcyNotificationLog.*.txt"
        'Get a list of existing log files.
        Dim existingLogFiles As List(Of String) = Directory.GetFiles(TempDir, bankruptcyLogFile).ToList()
        'Delete any old log files.
        For Each logFile As String In existingLogFiles
            If Not logFile.Contains(Date.Now.ToString("MM-dd-yy")) Then
                File.Delete(logFile)
            End If
        Next logFile

        'Define the exact file name to use for today's log.
        bankruptcyLogFile = String.Format("BankruptcyNotificationLog.{0}.txt", Date.Now.ToString("MM-dd-yy"))
        'Specify that we'll be appending to today's log if it already exists.
        Dim append As Boolean = File.Exists(TempDir + bankruptcyLogFile)
        Using bankruptcyLogWriter As New StreamWriter(TempDir + bankruptcyLogFile, append)
            bankruptcyLogWriter.WriteLine("{0},{1},{2},{3},{4}", ssn, firstName, lastName, Date.Now.ToString("MM/dd/yyyy"), lender)
            bankruptcyLogWriter.Close()
        End Using
    End Sub
End Class
