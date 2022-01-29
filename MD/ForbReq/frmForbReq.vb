Imports System.Data.SqlClient
Public Class frmForbReq
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByRef FrmTempDefForb As Object, ByVal TempDaysDelq As Integer, ByRef TempRIBM As Object, ByVal TempSSN As String, ByVal TempHPText As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        FrmDefForb = FrmTempDefForb
        DaysDelq = TempDaysDelq
        RIBM = TempRIBM
        SSN = TempSSN
        HPText = TempHPText
        Me.BackColor = FrmDefForb.backcolor
        Me.ForeColor = FrmDefForb.forecolor
        LVHist.BackColor = FrmDefForb.backcolor
        LVHist.ForeColor = FrmDefForb.forecolor
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
    Friend WithEvents lblDaysDlq As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbMonths As System.Windows.Forms.TextBox
    Friend WithEvents tbReason As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents LVHist As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmForbReq))
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblDaysDlq = New System.Windows.Forms.Label
        Me.tbMonths = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbReason = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.LVHist = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("PosterBodoni BT", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(692, 28)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Forbearance Request"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDaysDlq
        '
        Me.lblDaysDlq.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDaysDlq.Location = New System.Drawing.Point(8, 36)
        Me.lblDaysDlq.Name = "lblDaysDlq"
        Me.lblDaysDlq.Size = New System.Drawing.Size(692, 24)
        Me.lblDaysDlq.TabIndex = 7
        Me.lblDaysDlq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbMonths
        '
        Me.tbMonths.Location = New System.Drawing.Point(136, 72)
        Me.tbMonths.MaxLength = 1
        Me.tbMonths.Name = "tbMonths"
        Me.tbMonths.Size = New System.Drawing.Size(32, 20)
        Me.tbMonths.TabIndex = 0
        Me.tbMonths.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(4, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 16)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "# of Months Requested:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(4, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 16)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Reason:"
        '
        'tbReason
        '
        Me.tbReason.Location = New System.Drawing.Point(136, 96)
        Me.tbReason.MaxLength = 125
        Me.tbReason.Multiline = True
        Me.tbReason.Name = "tbReason"
        Me.tbReason.Size = New System.Drawing.Size(564, 60)
        Me.tbReason.TabIndex = 2
        Me.tbReason.Text = ""
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(268, 380)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(364, 380)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 164)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(692, 23)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Deferment && Forbearance History"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LVHist
        '
        Me.LVHist.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8})
        Me.LVHist.FullRowSelect = True
        Me.LVHist.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.LVHist.Location = New System.Drawing.Point(4, 188)
        Me.LVHist.MultiSelect = False
        Me.LVHist.Name = "LVHist"
        Me.LVHist.Size = New System.Drawing.Size(696, 176)
        Me.LVHist.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.LVHist.TabIndex = 3
        Me.LVHist.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Defer Or Forb"
        Me.ColumnHeader1.Width = 181
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Begin Date"
        Me.ColumnHeader2.Width = 73
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "End Date"
        Me.ColumnHeader3.Width = 67
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Cap Ind"
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Days Used"
        Me.ColumnHeader5.Width = 72
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Days Left"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Total Months Used"
        Me.ColumnHeader7.Width = 109
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Cert Date"
        '
        'frmForbReq
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(708, 413)
        Me.ControlBox = False
        Me.Controls.Add(Me.LVHist)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tbReason)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbMonths)
        Me.Controls.Add(Me.lblDaysDlq)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(716, 440)
        Me.MinimumSize = New System.Drawing.Size(716, 440)
        Me.Name = "frmForbReq"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Forbearance Request"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FrmDefForb As Object
    Private DaysDelq As Integer
    Private SSN As String
    Private Cancelled As Boolean = True
    Private HPText As String
    Declare Function OpenIcon Lib "user32" (ByVal hwnd As Long) As Long
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long

    Public Sub ShowFrm()
        Dim PT As New System.Drawing.Point(4, 192)
        Dim Conn As SqlConnection
        'Get list of nonForbearance loans
        Dim NonForbLoanTypes As New ArrayList()

        Dim Comm As SqlCommand
        Dim Reader As SqlDataReader
        If True Then
            Conn = New SqlConnection("Server=OPSDEV;Database=BSYS;Trusted_Connection=True;")
        Else
            Conn = New SqlConnection("Server=NOCHOUSE;Database=BSYS;Trusted_Connection=True;")
        End If
        NonForbLoanTypes.Clear() 'clear out all list items
        'add list items from DB
        Comm = New SqlCommand("EXEC GetNonForbearanceLoanTypes", Conn)
        Try
            Conn.Open()
            Reader = Comm.ExecuteReader()
            While Reader.Read()
                NonForbLoanTypes.Add(Reader("LoanType"))
            End While
            Conn.Close()
        Catch ex As Exception
            'SP.frmWipeOut.WipeOut("DUDE, I had a problem communicating with the database.  Please contact Systems Support and try again later.", "Database Communication Error")
            Conn.Close()
        End Try

        Dim tempLoan As String
        Dim hasForb As Boolean
        Dim hasNonForb As Boolean

        hasForb = False
        hasNonForb = False

        FastPathInput("TX3Z/ITS26" + SSN)

        If TextCheck(1, 72, "TSX28") Then
            Dim row As Integer
            row = 8

            While TextCheck(22, 2, "90007") = False

                tempLoan = Trim(GetText(row, 19, 6))

                If tempLoan = "" Then
                    Exit While 'end of list.
                End If

                If NonForbLoanTypes.Contains(tempLoan) Then
                    hasNonForb = True
                Else
                    hasForb = True
                End If
                row = row + 1
                If row = 20 Then
                    Press("F8")
                    row = 8
                End If
            End While

        ElseIf TextCheck(1, 72, "TSX29") Then
            tempLoan = Trim(GetText(1, 19, 6))
            tempLoan = tempLoan.Replace(";", "")

            If NonForbLoanTypes.Contains(tempLoan) Then
                hasNonForb = True
            Else
                hasForb = True
            End If
        Else
            MsgBox("Borrower not found on Compass.") 'Borrower Not on Compass
            Return
        End If

        If hasForb And hasNonForb Then
            MsgBox("Some of the borrower's loans are not eligible for forbearance, forbearance will only be applied to eligible loans.")
        ElseIf hasForb And hasNonForb = False Then
            MsgBox("The borrower's loans are eligible for forbearance.")
        ElseIf hasForb = False And hasNonForb Then
            MsgBox("The borrower's loans are NOT eligible for forbearance.")
            Return
        End If

 
        'Press("Enter")

        'While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
        '    If SP.Q.Check4Text(1, 74, "TCX02") Then
        '        'Enter gathered info
        '        If SP.Q.Check4Text(8, 2, "_") Then
        '            SP.Q.PutText(8, 2, "X")
        '        End If
        '    ElseIf SP.Q.Check4Text(1, 74, "TCX04") Then
        '        SP.Q.PutText(22, 35, Bor.TCX04SelectionValue)
        '    ElseIf SP.Q.Check4Text(1, 72, "TCX14") Then
        '        'Enter gathered info
        '        If SP.Q.GetText(10, 6, 2) = "BK" Then
        '            Bor.TCX14SelectionValue = "1"
        '        ElseIf SP.Q.GetText(16, 6, 2) = "BK" Then
        '            Bor.TCX14SelectionValue = "2"
        '        Else
        '            Bor.TCX14SelectionValue = ""
        '        End If
        '        SP.Q.PutText(22, 13, Bor.TCX14SelectionValue)
        '    Else
        '        SP.frmWhoaDUDE.WhoaDUDE("Whoa! That's like a totally new screen form me. Im gona need some help with this.  Please contact Systems Support.", "Knarly DUDE", True)
        '        Exit Sub
        '    End If
        '    SP.Q.Hit("Enter")
        'End While

        'check if borrower is eligible
        If DaysDelq > 269 Then
            MsgBox("The borrower is not eligible for verbal forbearance.")
            Exit Sub 'cancel service
        End If

        FastPathInput("TX3Z/ATC00" + SSN) 'This is just to get the screen back to where it was before the previous code executed.
        XYInput(19, 38, "1", True)
        XYInput(22, 22, "N")
        XYInput(22, 41, "N")
        XYInput(22, 60, "N")
        XYInput(22, 79, "N", True)
        XYInput(22, 36, "6", True)
        XYInput(8, 2, "X", True)

        FrmDefForb.process()
        GetAndCopyDataFromLV()
        lblDaysDlq.Text = "The borrower is " & DaysDelq & " days delinquent."
        Me.ShowDialog()
        LActivatePrevInstance(HPText) 'set focus to home page
    End Sub

    'this sub gathers the needed information from the MD Def/Forb hist Form List view and copies it into this services LV
    Private Sub GetAndCopyDataFromLV()
        Dim I As Integer
        Dim TempLV As System.Windows.Forms.ListView
        'remove all data from list view
        While LVHist.Items.Count > 0
            LVHist.Items.RemoveAt(0)
        End While
        TempLV = FrmDefForb.GetLV() 'get data
        'copy data
        While TempLV.Items.Count > I
            LVHist.Items.Add(TempLV.Items(I).Clone)
            I = I + 1
        End While
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'data validation
        If IsNumeric(tbMonths.Text) = False Then
            MsgBox("The ""# of Months Requested"" must be numeric.", MsgBoxStyle.Exclamation)
            Exit Sub
        ElseIf CInt(tbMonths.Text) > 6 Then
            MsgBox("Verbal forbearance may only be granted to cure any delinquency and up to 6 months of forbearance in the future.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If tbReason.TextLength = 0 Then
            MsgBox("You must provide a reason for the forbearance.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        MsgBox("To qualify for forbearance you must acknowledge that you intend to repay your student loans, but are temporarily unable to do so because of financial difficulties.", MsgBoxStyle.Information, "Regulation Requirement")
        If MsgBoxResult.No = MsgBox("Did borrower acknowledge obligation to repay loans, and affirm intent to repay all student loans?", MsgBoxStyle.YesNo, "Borrower Acknowledge Obligation") Then
            MsgBox("Advise the borrower: ""You are not eligible for forbearance because you will not acknowledge the obligation to repay, and you will not confirm your intent to repay the loans.""", MsgBoxStyle.Exclamation, "Borrower not eligible for forbearance")
            Cancelled = True
        Else
            Cancelled = False
        End If
        Me.Hide()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Cancelled = True
        Me.Hide()
    End Sub

    'returns to MD whether notes should be added at end of call or not
    Public Function WasServiceCancelled() As Boolean
        Return Cancelled
    End Function

    Public Sub AddActivityComments()
        Dim AC As New ActivityComments
        AC.AddCommentsToTD22AllLoans(SSN, "Past Due Plus (" & tbMonths.Text & ") Mnths Reqsted; Reason: " & tbReason.Text & " Borr acknowledged obligation to repay lns and affirmed intent to repay lns {MDPFORBREQ}", "XFORB")
    End Sub

    Sub LActivatePrevInstance(ByVal argStrAppToFind As String)
        Dim PrevHndl As Long
        Dim result As Long
        'Variable to hold individual Process.
        Dim objProcess As New Process
        'Collection of all the Processes running on local machine
        Dim objProcesses() As Process
        'Get all processes into the collection
        objProcesses = Process.GetProcesses()
        For Each objProcess In objProcesses
            'Check and exit if we have SMS running already
            If UCase(objProcess.MainWindowTitle) = UCase(argStrAppToFind) Then
                PrevHndl = objProcess.MainWindowHandle.ToInt32()
                Exit For
            End If
        Next
        'If no previous instance found exit the application.
        If PrevHndl = 0 Then Exit Sub
        'If previous instance found.
        result = OpenIcon(PrevHndl) 'Restore the program.
        result = SetForegroundWindow(PrevHndl) 'Activate the application.
    End Sub
End Class

