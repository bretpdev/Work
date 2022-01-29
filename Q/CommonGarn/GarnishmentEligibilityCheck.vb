Imports Reflection

Public Class GarnishmentEligibilityCheck

    Private _ri As ReflectionInterface
    Private _ssn As String = String.Empty
    Private _willDo As String = String.Empty
    Private _cancAct As String = String.Empty
    Private _amount As Double = 0
    Private _feeAmt As New List(Of Double)
    Private _loan As New List(Of String)
    Private _lp9OComment1 As String = String.Empty
    Private _aRate As Double = 0
    Private _moInt As Double = 0

    Private _padf As String = String.Empty
    ''' <summary>
    ''' Primary action filed date 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PADF() As String
        Get
            Return _padf
        End Get
        Set(ByVal value As String)
            _padf = value
        End Set
    End Property


    Private _balance As Double = 0
    ''' <summary>
    ''' Calculated balance from LC05 functionality
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Balance() As Double
        Get
            Return _balance
        End Get
    End Property


    Private _garnElig As Boolean = False
    ''' <summary>
    ''' results of whether the ssn provided was garnishment eligible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GarnElig() As Boolean
        Get
            Return _garnElig
        End Get
        Set(ByVal value As Boolean)
            _garnElig = value
        End Set
    End Property



    Private _comment As String = String.Empty
    ''' <summary>
    ''' Comment string that was decided during processing
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Comment() As String
        Get
            Return _comment
        End Get
    End Property

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="ri">Reflection Interface</param>
    ''' <param name="ssn">Borrower's SSN</param>
    ''' <param name="willDo">Text added to message box popups</param>
    ''' <param name="cancAct">Text prepended to LP90 queue task text</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal willDo As String, ByVal cancAct As String)
        _ri = ri
        _ssn = ssn
        _willDo = willDo
        _cancAct = cancAct
    End Sub

    ''' <summary>
    ''' Verify that the SSN is still eligible (check GarnElig for results).
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LC67GarnEligible()
        Dim scSta As String = String.Empty
        Dim sccntr As Integer
        Dim dckNo As String
        Dim lrow As Integer
        'access LC67
        _ri.FastPath("LC67I" & _ssn & "SC")
        'if no LC67 SC record is found, warn the user and go to CantDo subroutine to process the task
        If _ri.Check4Text(22, 3, "47004") Then
            'warn the user
            MsgBox("No active LC67 SC record exists for the borrower.  " & _willDo)
            'set comment variables to indicate nature of error
            _lp9OComment1 = _cancAct & ", no LC67 record"
            _comment = "no LC67 record found" & _comment
            'go to subroutine
            CantDo()
            'select the oldest record if the SKS is displayed
        Else
            'use the hotkey to go to the next page of records until the last page is reached if the target is not displayed
            If _ri.Check4Text(1, 54, "SUMMONS") = False Then
                Do While _ri.Check4Text(22, 3, "46004") = False
                    _ri.Hit(ReflectionInterface.Key.F8)
                Loop
            End If
            'set the counter to 12 (the maximum number of records displayed on one page)
            sccntr = 12
            'enter the record number indicated by the counter (since the oldest record is displayed last on the screen, the counter starts with 12--the last potential record on the screen--and counts backward) until the target screen is displayed,
            Do While _ri.Check4Text(1, 54, "SUMMONS") = False
                'enter the record number
                _ri.ReflectionSession.TransmitANSI(Format(sccntr, "0#"))
                _ri.Hit(ReflectionInterface.Key.Enter)
                'reduce the counter by one
                sccntr = sccntr - 1
            Loop
            'look for the oldest active record
            Do
                'if the withdrawn reason and inactive reason are not both blank, return to the SKS and try the next record
                If _ri.Check4Text(8, 19, "  ") = False OrElse _ri.Check4Text(15, 63, "  ") = False Then
                    'go back to the SKS
                    _ri.Hit(ReflectionInterface.Key.F12)
                    If _ri.Check4Text(1, 60, "SUB") Then
                        'warn the user
                        MsgBox("No active LC67 SC record exists for the borrower.  " & _willDo)
                        'set comment variables to indicate nature of error
                        _lp9OComment1 = _cancAct & ", no LC67 record"
                        _comment = "no LC67 record found" & _comment
                        'go to subroutine
                        CantDo()
                        Exit Do
                    End If
                    'if the counter is 0, reset the counter and go back a page
                    If sccntr = 0 Then
                        sccntr = 12
                        _ri.Hit(ReflectionInterface.Key.F7)
                        'if there are no more pages, the script has looked at all of the S&C records and did not find an active one, warn the user and go to CantDo subroutine to process the task
                        If _ri.Check4Text(22, 3, "46003") OrElse _ri.Check4Text(22, 3, "46000") OrElse _ri.Check4Text(22, 2, "46003") Then
                            'warn the user
                            MsgBox("No active LC67 SC record exists for the borrower.  " & _willDo)
                            'set comment variables to indicate nature of error
                            _lp9OComment1 = _cancAct & ", no LC67 record"
                            _comment = "no LC67 record found" & _comment
                            'go to subroutine
                            CantDo()
                            Exit Do
                        End If
                    End If
                    'enter the record number
                    _ri.ReflectionSession.TransmitANSI(sccntr)
                    _ri.Hit(ReflectionInterface.Key.Enter)
                    'decrease the record number by 1
                    sccntr = sccntr - 1
                    'set the status to "Y" and continue processing if the withdrawn reason and inactive reason are both blank
                Else
                    scSta = "Y"
                    Exit Do
                End If
            Loop
            'get the loan IDs linked to the S&C if the status is "Y"
            If scSta = "Y" Then
                'get the docket no
                dckNo = _ri.GetText(4, 64, 15)
                'get the primary action filed date                   
                PADF = _ri.GetText(4, 35, 8)
                'use the hotkey to go to the loan link display
                _ri.Hit(ReflectionInterface.Key.F4)
                'set the row to '5 (the first row of loans)
                lrow = 5
                'get loan id's until there are no more or until 15 rows have been processed
                Do
                    'check first column of loan ids on the row indicated by lrow
                    'if the loan id is blank, stop looking
                    If _ri.Check4Text(lrow, 6, "                   ") Then
                        Exit Do
                    End If
                    'if the link indicator is "S", increment the loan indicator by 1 and add the id to the "loan" variable array in the place indicated by lncntr
                    If _ri.Check4Text(lrow, 4, "S") Then
                        _loan.Add(_ri.GetText(lrow, 6, 19))
                    End If
                    'check second column of loan ids on the row indicated by lrow
                    If _ri.Check4Text(lrow, 31, "                   ") Then
                        Exit Do
                    End If
                    If _ri.Check4Text(lrow, 29, "S") Then
                        _loan.Add(_ri.GetText(lrow, 31, 19))
                    End If
                    'check third column of loan ids on the row indicated by lrow
                    If _ri.Check4Text(lrow, 56, "                   ") Then
                        Exit Do
                    End If
                    If _ri.Check4Text(lrow, 54, "S") Then
                        _loan.Add(_ri.GetText(lrow, 56, 19))
                    End If
                    'stop checking loans if row 15 is reached
                    If lrow = 15 Then
                        MsgBox("This borrower may have more than 45 loans linked to one S&C.  Only the balance from the first 45 will be included on the legal documents.")
                        Exit Do
                    End If
                    'increment the row indicator by 1
                    lrow = lrow + 1
                Loop
                'go to LC05 to verify the loans are still open and the balance is not too low
                LC05()
            End If
        End If
    End Sub

    'verify the loans are still open and the balance is not too low
    Private Sub LC05()
        Dim lnrate As New List(Of Double)
        Dim lnbal As New List(Of Double)
        Dim claimID As New List(Of String)
        Dim princCur As Double = 0
        Dim otherCur As Double = 0
        Dim lbal As Double = 0
        Dim lpi As Double = 0
        Dim loth As Double = 0
        Dim lint As Double = 0
        Dim lid As Double = 0
        Dim cntr As Integer = 0
        'access LC05
        _ri.FastPath("LC05I" & _ssn)
        'access the first loan
        _ri.ReflectionSession.TransmitANSI("01")
        _ri.Hit(ReflectionInterface.Key.Enter)
        Do
            'get the loan balance (outstanding balance due)
            lbal = CDbl(_ri.GetText(20, 32, 12))
            'get the current principal & interest balance
            lpi = CDbl(_ri.GetText(9, 32, 12)) - CDbl(_ri.GetText(10, 32, 12)) + CDbl(_ri.GetText(11, 32, 12)) - CDbl(_ri.GetText(12, 32, 12))
            'get the current other/legal/collection costs balance
            loth = CDbl(_ri.GetText(13, 32, 12)) - CDbl(_ri.GetText(14, 32, 12)) + CDbl(_ri.GetText(15, 32, 12)) - CDbl(_ri.GetText(16, 32, 12)) + CDbl(_ri.GetText(17, 32, 12)) - CDbl(_ri.GetText(18, 32, 12))
            lint = CDbl(_ri.GetText(6, 8, 6))
            lid = CDbl(_ri.GetText(21, 11, 4))
            'if the DC10 status and aux status indicate default, use the hotkey to go to page 2
            If _ri.Check4Text(4, 10, "03") AndAlso _ri.Check4Text(4, 26, "  ") AndAlso _ri.Check4Text(19, 73, "MMDDCCYY") Then
                _ri.Hit(ReflectionInterface.Key.F10)
                'if the borr garn code is 06 or 07, use the hotkey to go to page 3
                If _ri.Check4Text(9, 71, "06") OrElse _ri.Check4Text(9, 71, "07") Then
                    _ri.Hit(ReflectionInterface.Key.F10)
                    'check the unique id on LC05 page 3 with the list of unique id's saved in the "loan" variable array (lncntr = the number of loan id's to check, stop checking when a match is found or that number is exceeded)
                    Do Until cntr > _loan.Count - 1
                        'if the id matches the id in the "loan" variable array (in the place indicated by cntr), add the loan balances to the total balances of the loans selected for legal and stop comparing id's
                        If _ri.GetText(3, 13, 19) = _loan(cntr) Then
                            _balance = _balance + lbal
                            princCur = princCur + lpi
                            otherCur = otherCur + loth
                            lnrate.Add(lint)
                            lnbal.Add(lbal)
                            claimID.Add(lid)
                            Exit Do
                            'if the id's don't match, increment the counter by 1 and check the next value
                        Else
                            cntr = cntr + 1
                        End If
                    Loop
                End If
            End If
            'go to the next loan
            _ri.Hit(ReflectionInterface.Key.F8)
            'stop looking for more loans if none are found
            If _ri.Check4Text(22, 3, "46004") Then
                Exit Do
            End If
        Loop
        For i As Integer = 0 To (lnrate.Count - 1)
            _aRate = _aRate + (lnrate(i) * lnbal(i) / _balance)
            _feeAmt.Add(lnbal(i) / _balance * _amount)
        Next i
        'if account principal and interest is < $25 and other/legal/collection costs are < $100, the account balance is too low to garnish, warn the user and go to CantDo subroutine to process the task
        If princCur < 25 AndAlso otherCur < 100 Then
            'warn the user
            MsgBox("Account balance too low or no eligible loans.  " & _willDo)
            'set comment variables to indicate nature of error
            _lp9OComment1 = _cancAct & ", account balance too low or no eligible loans"
            _comment = "account balance too low or no eligible loans" & _comment
            'go to subroutine
            CantDo()
        Else
            _garnElig = True
        End If
    End Sub

    'create a task in QGARNERR for DBQ to review, enter and LP50 activity record, and cancel the task
    Private Sub CantDo()
        'create task in QGARNERR
        Common.AddQueueTaskInLP9O(_ri.ReflectionSession, _ssn, "QGARNERR", Today().ToString(), _lp9OComment1, "", "", "")
    End Sub

    ''' <summary>
    ''' determine the number of payments to pay off the account at the amount of voluntar payments made in the past 60 days
    ''' </summary>
    ''' <remarks></remarks>
    Sub SatVolPmts()
        Dim pmts As Double = 0
        Dim row As Integer = 7
        Dim nopmts As Long = 0
        'access LC41
        _ri.FastPath("LC41I" & _ssn)
        'select all payments
        _ri.PutText(7, 36, "X", ReflectionInterface.Key.Enter)
        'sum all voluntary borrower payments made in the last 60 days
        If _ri.Check4Text(5, 9, "SOC") Then
            pmts = 0
        ElseIf _ri.Check4Text(21, 3, "SEL") Then
            Do Until CDate(_ri.GetText(row, 5, 8).ToDateFormat()) < Today.AddDays(-60) OrElse _ri.Check4Text(22, 3, "46004")
                '<4>            If .GetDisplayText(row, 34, 2) = "BR" Or .GetDisplayText(row, 34, 2) = "CS" Or .GetDisplayText(row, 34, 2) = "EP" Then Pmts = Pmts + Val(.GetDisplayText(row, 14, 10))
                If (_ri.Check4Text(row, 34, "BR") OrElse _ri.Check4Text(row, 34, "CS") _
                    OrElse _ri.Check4Text(row, 34, "EP")) AndAlso _ri.Check4Text(row, 39, "  ") _
                    Then pmts = pmts + Val(_ri.GetText(row, 14, 10))
                row = row + 1
                If _ri.Check4Text(row, 34, "  ") Then
                    row = 7
                    _ri.Hit(ReflectionInterface.Key.F8)
                End If
            Loop
        ElseIf (_ri.Check4Text(4, 28, "BRWR PAYMT") OrElse _
                _ri.Check4Text(4, 28, "COSGNR PMT") OrElse _
                _ri.Check4Text(4, 28, "EMPLOYER  ")) AndAlso _
                 CDate(_ri.GetText(4, 72, 8).ToDateFormat()) >= Today.AddDays(-60) Then
            pmts = Val(_ri.GetText(3, 28, 10))
        End If
        'calculate the number of payments
        If Pmts < Balance * 0.01 Then
            nopmts = 190
        Else
            nopmts = Math.Round(CDbl(NPer(_aRate / 100 / 12, -pmts, _balance)), 0)
        End If
        'set garnishment eligibility based on the number of payments
        If pmts < _moInt OrElse pmts < 50 OrElse nopmts > 84 Then
            _garnElig = True
        Else
            MsgBox("Payments received over the past 60 days are sufficient to stop the garnishment process.")
            _garnElig = False
        End If
    End Sub

    'cancel a task
    Sub CancelTask(ByVal Queue As String)
        Dim cancsta As String
        Dim cntr As Integer = 0
        Dim row As Integer = 0
        'access LP8Y

        _ri.FastPath("LP8YCDFT;" & Queue & ";;" & _ssn)

        '<2>
        '<6>    If .GetDisplayText(22, 3, 5) = "47430" Then


        If _ri.GetText(1, 75, 3) <> "DET" Then                    '<6>
            '<2>        comment = comment & ", no task found in " & Queue & " queue"
        Else
            '<2>        cntr = 0
            row = 7
            Do
                If _ri.GetText(row, 33, 1) = "A" Then
                    If cntr > 0 Then
                        Do Until _ri.GetText(22, 3, 5) = "49000"
                            MsgBox("There is more than one available " & Queue & " task for the borrower.  complete the appropriate task(s) and hit Insert to resume processing.")
                            Q.Common.PauseForInsert(_ri.ReflectionSession)

                        Loop
                        Exit Do
                    Else
                        _ri.PutText(row, 33, "X", ReflectionInterface.Key.Enter)
                        cntr = cntr + 1
                    End If
                    'Comment = Comment & ", task completed in " & Queue & " queue"
                    _comment = _comment & ", task completed in " & Queue & " queue"
                End If
                If _ri.GetText(row, 33, 1) = "W" Then
                    _ri.PutText(row, 33, "A", ReflectionInterface.Key.F8)

                    _ri.FastPath("LP8YCDFT;" & Queue & ";;" & _ssn)

                    row = 6
                End If
                If _ri.GetText(row, 33, 1) = " " Then
                    _ri.Hit(ReflectionInterface.Key.F8)

                    If _ri.GetText(22, 3, 5) = "46004" Then

                        Exit Do
                    End If
                    row = 6
                End If
                row = row + 1
            Loop
        End If
        If cntr = 0 Then                                                '<2>
            'Comment = Comment & ", no task found in " & Queue & " queue"
            _comment = _comment & ", no task found in " & Queue & " queue"
            cancsta = "N"
        End If                                                         '</2>
    End Sub

End Class
