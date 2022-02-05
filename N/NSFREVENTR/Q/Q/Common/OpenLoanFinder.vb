Imports Q.ReflectionInterface
Imports Q.Common

Friend Class OpenLoanFinder
#Region "COMPASS"
    ''' <summary>
    ''' Checks TS26 for a loan with a non-zero current balance.
    ''' </summary>
    ''' <param name="ri">Reflection Interface</param>
    ''' <param name="ssn">Borrower's SSN</param>
    ''' <returns>Common.AesSystem.Compass if an open loan is found, otherwise Common.AesSystem.None.</returns>
    Public Shared Function LookForOpenLoanOnCompass(ByVal ri As ReflectionInterface, ByVal ssn As String) As AesSystem
        ri.FastPath("TX3Z/ITS26" + ssn)
        If (ri.Check4Text(1, 72, "TSX29")) Then
            'Target screen. Check the total balance.
            If (ri.Check4Text(10, 16, "          ") = False AndAlso Double.Parse(ri.GetText(10, 16, 10)) > 0) Then
                Return AesSystem.Compass
            End If
        ElseIf (ri.Check4Text(1, 72, "TSX28")) Then
            'Selection screen. Check until a non-zero balance is found.
            Do While Not ri.Check4Text(23, 2, "90007")
                For row As Integer = 8 To 19
                    'Stop checking this page if this row's current balance is blank.
                    If (ri.Check4Text(row, 59, "          ")) Then Exit For
                    'Check for a non-zero current balance.
                    If (Double.Parse(ri.GetText(row, 59, 10))) > 0 Then
                        Return AesSystem.Compass
                        Exit Do
                    End If
                Next row
                ri.Hit(Key.F8)
            Loop
        End If
        Return AesSystem.None
    End Function
#End Region 'COMPASS

#Region "OneLINK"
    Private Shared _openLoanIndicators As String() = {"CR", "AL", "DA", "FB", "IA", "ID", "IG", "IM", "RP", "UA", "UB"}
    Private Shared _uheaaServicerIds As String() 'One-time lazy initialization is done in the LookForOpenLoanOnOneLink() method.

    ''' <summary>
    ''' Checks LG02 and LG10 for a loan whose status indicates it's open.
    ''' </summary>
    ''' <param name="ri">Reflection Interface</param>
    ''' <param name="ssn">Borrower's SSN</param>
    ''' <returns>
    ''' Common.AesSystem.Compass if a loan is found with an open status code and a UHEAA servicer code,
    ''' Common.AesSystem.OneLink if a loan is found with an open status code and a non-UHEAA servicer code,
    ''' a combination of the above two if both UHEAA and non-UHEAA servicer codes are found for loans with open status codes,
    ''' or Common.AesSystem.None if no open loans are found.
    ''' </returns>
    ''' <remarks>
    ''' Open status codes include CR, AL, DA, FB, IA, ID, IG, IM, RP, UA, and UB.
    ''' A status of CP is considered open if the LC05 status is 04, or 03 if there is no transfer date.
    ''' </remarks>
    Public Shared Function LookForOpenLoanOnOneLink(ByVal ri As ReflectionInterface, ByVal ssn As String) As AesSystem
        'If there are multiple loans, more than one system may be involved. Be sure to get all of the applicable systems.
        Dim foundSystems As AesSystem = AesSystem.None

        'Make sure the UHEAA servicer ID array is initialized.
        If (_uheaaServicerIds Is Nothing OrElse _uheaaServicerIds.Length = 0) Then
            _uheaaServicerIds = DataAccess.GetAffiliatedLenderIds("UHEAA", ri.TestMode).ToArray()
        End If

        ri.FastPath("LG02I;" + ssn)
        If ri.Check4Text(1, 60, "LOAN APPLICATION MENU") Then
            'Nothing found on LG02.
        ElseIf ri.Check4Text(1, 58, "LOAN APPLICATION SELECT") Then
            'Selection screen.
            Dim loanRow As Integer = 10
            Do While Not ri.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY")
                Dim lg02Page As Integer = Integer.Parse(ri.GetText(2, 73, 2))
                Dim servicerCode As String = ri.GetText(loanRow, 46, 6)
                If ri.Check4Text(loanRow, 75, _openLoanIndicators) Then
                    If _uheaaServicerIds.Contains(servicerCode) Then
                        foundSystems = foundSystems Or AesSystem.Compass
                    Else
                        foundSystems = foundSystems Or AesSystem.OneLink
                    End If
                ElseIf ri.Check4Text(loanRow, 75, "CP") Then
                    'Select this loan to go to the detail screen.
                    ri.PutText(21, 13, ri.GetText(loanRow, 2, 2), Key.Enter)
                    'Get the loan ID and check LC05 for an open status.
                    Dim loanId As String = GetLoanInfoFromLG02(ri).Cluid
                    If LoanIsOpenClaimPaid(ri, ssn, "LG02I;", lg02Page, loanId) Then
                        If _uheaaServicerIds.Contains(servicerCode) Then
                            foundSystems = foundSystems Or AesSystem.Compass
                        Else
                            foundSystems = foundSystems Or AesSystem.OneLink
                        End If
                    End If
                End If
                loanRow += 1
                If ri.Check4Text(loanRow, 3, "SELECTION", " ") Then
                    ri.Hit(Key.F8)
                    loanRow = 10
                End If
            Loop
        Else
            'Target screen. Go to LG10.
            ri.FastPath("LG10I" + ssn)
            If ri.Check4Text(1, 74, "DISPLAY") Then
                'Target screen. Check the status of each loan.
                Return CheckAllLoansOnLG10(ri)
            ElseIf ri.Check4Text(1, 75, "SELECT") Then
                'Multiple loan holders. Select each one in turn.
                Dim servicerRow As Integer = 7
                Do While Not ri.Check4Text(servicerRow, 6, " ")
                    ri.PutText(19, 15, ri.GetText(servicerRow, 6, 1), Key.Enter)
                    foundSystems = foundSystems Or CheckAllLoansOnLG10(ri)
                    'We can stop looking if we've found both systems.
                    If ((foundSystems And AesSystem.Compass) AndAlso (foundSystems And AesSystem.OneLink)) Then Exit Do
                    ri.Hit(Key.F12)
                    servicerRow += 1
                Loop
            End If
        End If

        Return foundSystems
    End Function

    Private Shared Function CheckAllLoansOnLG10(ByVal ri As ReflectionInterface) As AesSystem
        Dim foundSystems As AesSystem = AesSystem.None

        'Loop through loan rows until both systems are found or there are no more loans.
        Dim loanRow As Integer = 11
        Do While Not ri.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY")
            Dim lg10Page As Integer = Integer.Parse(ri.GetText(2, 73, 2))
            Dim servicerCode As String = ri.GetText(5, 27, 6)
            If ri.Check4Text(loanRow, 59, _openLoanIndicators) Then
                If _uheaaServicerIds.Contains(servicerCode) Then
                    foundSystems = foundSystems Or AesSystem.Compass
                Else
                    foundSystems = foundSystems Or AesSystem.OneLink
                End If
            ElseIf ri.Check4Text(loanRow, 59, "CP") Then
                Dim fastPathArguments As String = ri.GetText(1, 9, 16)
                'Get the loan ID and check LC05 for an open status.
                Dim loanId As String = ri.GetText(loanRow, 4, 19)
                If LoanIsOpenClaimPaid(ri, fastPathArguments, "LG10I", lg10Page, loanId) Then
                    If _uheaaServicerIds.Contains(servicerCode) Then
                        foundSystems = foundSystems Or AesSystem.Compass
                    Else
                        foundSystems = foundSystems Or AesSystem.OneLink
                    End If
                End If
            End If
            If ((foundSystems And AesSystem.Compass) AndAlso (foundSystems And AesSystem.OneLink)) Then Exit Do
            loanRow += 1
            If (ri.Check4Text(21, 3, "0 LOANS SELECTED", " ")) Then
                ri.Hit(Key.F8)
                loanRow = 11
            End If
        Loop

        Return foundSystems
    End Function

    Private Shared Function LoanIsOpenClaimPaid(ByVal ri As ReflectionInterface, ByVal fastPathArguments As String, ByVal screen As String, ByVal lgPage As Integer, ByVal loanId As String) As Boolean
        ri.FastPath("LC05I" + fastPathArguments)
        'Check each loan to see if it matches the LoanID from LG02 or LG10.
        Dim LC05Record As Integer = 1
        Do While Not ri.Check4Text(LC05Record * 3 + 4, 3, " ")
            'Loans are listed one every three lines, starting on line 7 (four per page).
            Dim selection As String = ri.GetText(LC05Record * 3 + 4, 3, 1)
            ri.PutText(21, 13, selection, Key.Enter)
            'Page down twice to get to the page with the loan ID.
            ri.Hit(Key.F10)
            ri.Hit(Key.F10)
            'Check the loan ID here against what we got from LG02 or LG10.
            If ri.Check4Text(3, 13, loanId) Then
                'The is the loan we're looking for. Page down once to get back to page 1, where the status code is.
                ri.Hit(Key.F10)
                Dim isSatisfied As Boolean = ri.Check4Text(4, 10, "04")
                Dim isDefaultedButNotTransferred As Boolean = ri.Check4Text(4, 10, "03") AndAlso ri.Check4Text(19, 73, "MM")
                If isSatisfied OrElse isDefaultedButNotTransferred Then
                    'The loan is open.
                    Return True
                Else
                    'If we didn't find '03' status, or the loan was transferred, then this loan isn't open;
                    'return the script to LG02 or LG10 on the last page it was looking at.
                    ri.FastPath(screen + fastPathArguments)
                    Do While ri.GetText(2, 74, 1) <> lgPage
                        ri.Hit(Key.F8)
                    Loop
                End If
                'End of logic for a matching loan ID. Whatever the outcome, if the ID matched, we're done here.
                Return False
            Else
                'This isn't the loan we're looking for. Back out to LC05 and check the next one.
                ri.Hit(Key.F12)
                LC05Record += 1
                'Before re-starting the loop, check to see if this is the last record on the page.
                If LC05Record = 5 Then
                    'There are only four records per page, so a record number of 5 means next page.
                    ri.Hit(Key.F8)
                    LC05Record = 1
                End If
            End If
        Loop

        'If we don't find the loan on LC05, then it's safe to say there's no open status associated with the loan.
        Return False
    End Function
#End Region 'OneLINK

#Region "Obsolete"
    ''' <summary>
    ''' Checks LG02 and LG10 to determine if the borrower has open loans on OneLINK
    ''' </summary>
    ''' <param name="rs">Reflection Session</param>
    ''' <param name="SSN">Borrower's SSN</param>
    ''' <returns>True if the borrower has open loans on OneLINK else false.</returns>
    ''' <remarks></remarks>
    Public Shared Function HasOpenLoanOnOneLINK(ByVal rs As Reflection.Session, ByVal ssn As String) As Boolean
        Dim openLoanIndicators As String() = {"CR", "AL", "DA", "FB", "IA", "ID", "IG", "IM", "RP", "UA", "UB"}
        FastPath(rs, "LG02I;" + ssn)
        If Check4Text(rs, 1, 60, "LOAN APPLICATION MENU") Then
            'nothing found on LG02
        ElseIf Check4Text(rs, 1, 58, "LOAN APPLICATION SELECT") Then
            'selection screen was encountered
            Dim row As Integer = 10
            While Check4Text(rs, 22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                Dim lg02Page As Integer = Integer.Parse(GetText(rs, 2, 73, 2))
                If Check4Text(rs, row, 75, openLoanIndicators) OrElse HasOpenClaimPaid(rs, "LG02I;", ssn, lg02Page, row) Then
                    Return True
                End If
                row += 1
                If Check4Text(rs, row, 3, "S", " ") Then
                    'page forward
                    Hit(rs, Key.F8)
                    row = 10
                End If
            End While
        Else
            'target screen on LG02 was encountered
            'go to LG10
            FastPath(rs, "LG10I" + ssn)
            If Check4Text(rs, 1, 74, "DISPLAY") Then
                'Target screen. Check the status of each loan.
                Dim lg10Page As Integer = Integer.Parse(GetText(rs, 2, 73, 2))
                If Check4Text(rs, 11, 59, openLoanIndicators) OrElse HasOpenClaimPaid(rs, "LG10I", ssn, lg10Page, 11) Then
                    Return True
                End If
            ElseIf Check4Text(rs, 1, 75, "SELECT") Then
                'Multiple loan holders. Select each one in turn.
                Dim row As Integer = 7
                Do While Not Check4Text(rs, row, 6, " ")
                    PutText(rs, 19, 15, GetText(rs, row, 6, 1), Key.Enter)
                    Dim lg10Page As Integer = Integer.Parse(GetText(rs, 2, 73, 2))
                    If Check4Text(rs, 11, 59, openLoanIndicators) OrElse HasOpenClaimPaid(rs, "LG10I", GetText(rs, 1, 9, 16), lg10Page, 11) Then
                        Return True
                    End If
                    Hit(rs, Key.F12)
                    row += 1
                Loop
            End If
        End If
        Return False
    End Function

    'The HasOpenClaimPaid function is called by HasOpenLoanOnOneLink() from LG02 or LG10 to see if
    'a loan's status is 'CP' (Claim Paid) and has an open status on LC05.
    Private Shared Function HasOpenClaimPaid(ByVal rs As Reflection.Session, ByVal screen As String, ByVal ssn As String, ByVal lgPage As Integer, ByVal row As Integer) As Boolean
        'Still on LG02 or LG10, check the loan status to see if it's 'CP'.
        If screen = "LG02I;" AndAlso Not Check4Text(rs, row, 75, "CP") Then
            'This loan isn't a CP status, so no need to check LC05.
            Return False
        ElseIf screen = "LG10I" AndAlso Not Check4Text(rs, row, 59, "CP") Then
            'This loan isn't a CP status, so no need to check LC05.
            Return False
        ElseIf screen = "LG02I;" AndAlso Check4Text(rs, row, 75, "CP") Then
            'Select this loan to go to the detail screen.
            PutText(rs, 21, 13, GetText(rs, row, 2, 2), Key.Enter)
            'Get the loan ID and check LC05 for an open status.
            Dim loanId As String = GetLoanInfoFromLG02(rs).Cluid
            Return LC05ShowsOpenStatus(rs, ssn, screen, lgPage, loanId)
        ElseIf screen = "LG10I" AndAlso Check4Text(rs, row, 59, "CP") Then
            'Get the loan ID and check LC05 for an open status.
            Dim loanId As String = GetText(rs, row, 4, 19)
            Return LC05ShowsOpenStatus(rs, ssn, screen, lgPage, loanId)
        End If
    End Function

    Private Shared Function LC05ShowsOpenStatus(ByVal rs As Reflection.Session, ByVal ssn As String, ByVal screen As String, ByVal lgPage As Integer, ByVal loanId As String) As Boolean
        FastPath(rs, "LC05I" + ssn)
        'Check each loan to see if it matches the LoanID from LG02 or LG10.
        Dim LC05Record As Integer = 1
        Do While Not Check4Text(rs, LC05Record * 3 + 4, 3, " ")
            'Loans are listed one every three lines, starting on line 7 (four per page).
            Dim selection As String = GetText(rs, LC05Record * 3 + 4, 3, 1)
            PutText(rs, 21, 13, selection, Key.Enter)
            'Page down twice to get to the page with the loan ID.
            Hit(rs, Key.F10)
            Hit(rs, Key.F10)
            'Check the loan ID here against what we got from LG02 or LG10.
            If Check4Text(rs, 3, 13, loanId) Then
                'The is the loan we're looking for. Page down once to get back to page 1, where the status code is.
                Dim foundOpenStatus As Boolean = False
                Hit(rs, Key.F10)
                Dim isSatisfied As Boolean = Check4Text(rs, 4, 10, "04")
                Dim isDefaultedButNotTransferred As Boolean = Check4Text(rs, 4, 10, "03") AndAlso Check4Text(rs, 19, 73, "MM")
                If isSatisfied OrElse isDefaultedButNotTransferred Then
                    'Bingo! Our work here is finished.
                    foundOpenStatus = True
                Else
                    'If we didn't find '03' status, or the loan was transferred, then this loan isn't open;
                    'return the script to LG02 or LG10 on the last page it was looking at.
                    FastPath(rs, screen + ssn)
                    Do While GetText(rs, 2, 74, 1) <> lgPage
                        Hit(rs, Key.F8)
                    Loop
                End If
                'End of logic for a matching loan ID. Whatever the outcome, if the ID matched, we're done here.
                Return foundOpenStatus
            Else
                'Start of logic for a non-match on the loan ID.
                'In this case, we need to back out to LC05...
                Hit(rs, Key.F12)
                'and look for a match on the next loan.
                LC05Record += 1
                'Before re-starting the loop, check to see if this is the last record on the page.
                If LC05Record = 5 Then
                    'There are only four records per page, so a record number of 5 means next page.
                    Hit(rs, Key.F8)
                    LC05Record = 1
                End If
            End If
        Loop

        'If we come out of LC05 with no matches, then it's safe to say there's no open status associated with the loan.
        Return False
    End Function
#End Region 'Obsolete
End Class
