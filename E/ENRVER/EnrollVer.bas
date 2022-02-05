Attribute VB_Name = "EnrollVer"
Dim SrcTxt As String
Dim TInfo() As String
Dim oneLP9O As Boolean
Dim mtime As Integer
Dim qnumber() As Integer
Dim twoTimes As Boolean
Dim DTPROC As String
Dim UserId As String
Dim PrivateLoans() As Integer
Dim HasFFELPLoan As Boolean
Dim HasGateLoan As Boolean
Public SSN As String
Public ActCd As String
Public ActTyp As String
Public ConTyp As String
Public comment As String
Public PauPlea As String
Public Script As String
Public Queue As String
Public InstID As String
Public InstTyp As String
Public LP9OComment As String
Public LP9OComment1 As String
Public LP9OComment2 As String
Public LP9OComment3 As String
Public LP9OComment4 As String
Public DateDue As String
Public OthUser As String
Public EnrSta As String
Public StaEffDt As String
Public SchCd As String
Public SchCrtDt As String
Public AGD As String
Public Src As String


'process enrollment information
Function EnrollInfoProc()
    Dim row As Integer
    Dim x As Integer
    ReDim TInfo(1, 0)
    With Session
    'set values for script
        Okay = 0
        Script = "  {ENRLINFO}"
        ActCd = "GEVRR"
        ActTyp = "MS"
        ConTyp = "08"
        LP9OComment2 = ""
        LP9OComment3 = ""
        LP9OComment4 = ""
        DateDue = Date
        'prompt the user for SSN, school code, and enrollment history info
        Do
            SSN = ""
            Load EVInfo1
            EVInfo1.Show
            If Okay = 0 Then End
            If EVInfo1.NSLDS = True Then
                Src = "I"
                SrcTxt = "NSLDS"
            ElseIf EVInfo1.NCH = True Then
                Src = "M"
                SrcTxt = "NCH"
            ElseIf EVInfo1.SchOpt = True Then
                Src = "D"
                SrcTxt = "School"
            End If
            
            If EVInfo1.SSN.TextLength = 10 Then
                'do translation from account number if needed
                SSN = ""
                If Not SP.GetLP22(SSN, EVInfo1.SSN) Then
                    FastPath "TX3ZITX1J;" & EVInfo1.SSN
                    If Not Check4Text(1, 71, "TXX1R-01") Then
                        SSN = ""
                        MsgBox "The SSN or account number was not found on OneLINK or Compass.  Reenter the information.", vbCritical, "Invalid Account Information"
                    Else
                        SSN = GetText(1, 11, 9)
                        Exit Do
                    End If
                End If
            Else
                SSN = EVInfo1.SSN
                If Not SP.GetLP22(SSN) Then
                    FastPath "TX3ZITX1J;" & EVInfo1.SSN
                    If Not Check4Text(1, 71, "TXX1R-01") Then
                        SSN = ""
                        MsgBox "The SSN or account number was not found on OneLINK or Compass.  Reenter the information.", vbCritical, "Invalid Account Information"
                    Else
                        SSN = GetText(1, 11, 9)
                        Exit Do
                    End If
                End If
            End If
            If SSN <> "" Then
                SchCd = EVInfo1.SchCd
                'warn the user to correct erroneous data
                If Len(SchCd) <> 8 Then
                    warn = MsgBox("You must enter all 8 digits of the school code.", vbOKOnly, "School Code Error")
                Else
                    .TransmitTerminalKey rcIBMClearKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    .TransmitANSI "LPSCI" & SchCd
                    .TransmitTerminalKey rcIBMEnterKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    If .GetDisplayText(22, 3, 5) = "47004" Then
                        warn = MsgBox("The school code entered was not found on OneLINK.  Click OK to correct the school code.", vbOKOnly, "School Code Error")
                    'verify the SSN
                    Else
                        .TransmitTerminalKey rcIBMClearKey
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        .TransmitANSI "LP22I" & SSN
                        .TransmitTerminalKey rcIBMEnterKey
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        If .GetDisplayText(22, 3, 5) = "47004" Then
                            warn = MsgBox("The SSN entered was not found on OneLINK.  Click OK to correct the SSN.", vbOKOnly, "SSN Error")
                        Else
                            Exit Do
                        End If
                    End If
                End If
            End If
        Loop
        Unload EVInfo1
        
        'determine if there are any Gateway loans on TS26
        HasFFELPLoan = False
        ReDim PrivateLoans(0)
        FastPath "TX3ZITS26" & SSN
        If Check4Text(23, 2, "01527 BORROWER NOT FOUND ON SYSTEM") = False Then 'skip COMPASS processing if no loans
            'review loans on selection screen
            If Check4Text(1, 72, "TSX28") Then
                row = 8
                Do While Not Check4Text(23, 2, "90007")
                    'capture loan info if loan type is listed as "Private" in BSYS table
                    If SP.Common.IsLoanType(GetText(row, 19, 6), "Private") Then
                        If CDbl(GetText(row, 59, 10)) > 0 Then
                            ReDim Preserve PrivateLoans(UBound(PrivateLoans) + 1)
                            PrivateLoans(UBound(PrivateLoans)) = CInt(GetText(row, 14, 4))
                        End If
                    'set indicator if loan type is not private
                    Else
                        HasFFELPLoan = True
                    End If
                    
                    'increment row counter
                    row = row + 1
    
                    'go to next page if the end of list is reached
                    If row = 20 Then
                        Hit "F8"
                        row = 8
                    End If
                Loop
            'review loan on detail screen
            ElseIf Check4Text(1, 72, "TSX29") Then
                If SP.Common.IsLoanType(GetText(6, 66, 6), "Private") Then
                    If CDbl(GetText(11, 12, 10)) > 0 Then
                        ReDim Preserve PrivateLoans(UBound(PrivateLoans) + 1)
                        PrivateLoans(UBound(PrivateLoans)) = CInt(GetText(7, 35, 4))
                    End If
                Else
                    HasFFELPLoan = True
                End If
            
            End If
            
            'create an activity record if any Gateway loans are found
            If UBound(PrivateLoans) > 0 Then
                MsgBox "A queue task will be created in COMPASS to review enrollment.", vbInformation, "Private Loans"
                UserId = SP.Common.GetUserID
                SP.Common.ATD22ByLoan SSN, "PENRL", "", PrivateLoans, "ENRLINFO", UserId
            End If
            
            'if any private loans were found then skip PO1N functionality
            If UBound(PrivateLoans) = 0 Then ''**
                ReDim PrivateLoans(0)
                If HasFFELPLoan Then
                    'review loans on selection screen
                    FastPath "TX3ZIPO1N" & SSN
                    If Check4Text(1, 75, "POX1P") Then
                        row = 9
                        Do While Not Check4Text(22, 2, "90007")
                            If SP.Common.IsLoanType(GetText(row, 5, 6), "Private") Then
                                PutText 21, 16, GetText(row, 2, 2), "ENTER"
                                PutText 2, 78, "DI", "ENTER"
                                CheckDI
                                Hit "F12"
                            End If
                            row = row + 1
                            If Check4Text(row, 2, "  ") Then
                                Hit "F8"
                                row = 8
                            End If
                        Loop
                    'review loan on detail screen
                    ElseIf Check4Text(1, 75, "POX1S") Then
                        If HasPO1NPrivateLoan Then
                            CheckDI
                        End If
                    End If
                End If
                
                'create an activity record if any Gateway loans are found
                If UBound(PrivateLoans) > 0 Then
                    MsgBox "A queue task will be created in COMPASS to review enrollment.", vbInformation, "Private Loans"
                    If UserId = "" Then UserId = SP.Common.GetUserID
                    SP.Common.ATD37FirstLoan SSN, "PENRL", "", UserId, "ENRLINFO"
                End If
            End If
            
            'warn user and end if no private loans were found
            If Not HasFFELPLoan Then
                MsgBox "There are no non private loans to process.  Processing is complete.", vbInformation, "Processing Complete"
                End
            End If
        End If
        loclLP9O
    'cancel the EVSNTFLU tasks for the borrower

        'access LP8Y
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP8YCSCR;EVSNTFLU;;" & SSN
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'review each task
        'qnumber is -1 when the school is not found. This should not ever happen, except in test
        If (UBound(TInfo, 2) = 1 And UBound(qnumber) = 1) Or oneLP9O Then
        'only one task found in LP9O
            If .GetDisplayText(1, 75, 3) = "DET" Then
                row = 7
                Do
                    'cancel available tasks
                    If .GetDisplayText(row, 33, 1) = "A" Then
                        .MoveCursor row, 33
                        .TransmitANSI "X"
                    End If
                    'make tasks available that are being worked and refresh the screen
                    If .GetDisplayText(row, 33, 1) = "W" Then
                        .TransmitANSI "A"
                        .TransmitTerminalKey rcIBMPf6Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        .TransmitTerminalKey rcIBMClearKey
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        .TransmitANSI "LP8YCSCR;" & Queue & ";;" & SSN
                        .TransmitTerminalKey rcIBMEnterKey
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        row = 6
                    End If
                    'increment the row counter
                    row = row + 1
                    'go to the next page if the line is blank
                    If .GetDisplayText(row, 33, 1) = " " Then
                        .TransmitTerminalKey rcIBMPf8Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        'post the changes and stop looking if no more tasks are found
                        If .GetDisplayText(22, 3, 5) = "46004" Then
                            .TransmitTerminalKey rcIBMPf6Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            Exit Do
                        End If
                        row = 7
                    End If
                Loop
            End If
        ElseIf UBound(TInfo, 2) > 1 And twoTimes = False And UBound(qnumber) > 1 Then
            'if multiple tasks are found in LP9O
            If .GetDisplayText(1, 75, 3) = "DET" Then
                row = 6
                
                For x = 1 To UBound(qnumber)
                    If qnumber(x) > 14 Then
                        'go to the next page if the line is blank
                        If .GetDisplayText(row, 33, 1) = " " Then
                            .TransmitTerminalKey rcIBMPf8Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            'post the changes and stop looking if no more tasks are found
                            If .GetDisplayText(22, 3, 5) = "46004" Then
                                
                                MsgBox "Error! Contact Systems Support. " & SP.Q.GetText(22, 3, 30)
                                End
                            End If
                            row = 6
                        End If
                        qnumber(x) = qnumber(x) - 14
                    End If
                    'cancel available tasks
                    If .GetDisplayText(row + qnumber(x), 33, 1) = "A" Then
                        .MoveCursor row + qnumber(x), 33
                        .TransmitANSI "X"
                    End If
                Next x
                SP.Q.Hit "ENTER"
                SP.Q.Hit "F6"
            End If
        ElseIf UBound(TInfo, 2) = 0 Then
        'no queue tasks are found.
        End If

        'no history
        If Okay = 2 Then
            'get the enrollment information
            GetInfo
            If Okay <> 1 Then End
            'access LG29
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "LG29C" & SSN & ";" & SchCd
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'review/update enrollment information if it already exists for borrower/school
            If .GetDisplayText(5, 22, 3) <> "SSN" Then
                'select the row if the target screen is not displayed
                If .GetDisplayText(20, 8, 3) = "SEL" Then
                    row = 9
                    'find the row with the greatest cert date
                    grtscrtdt = "01011900"
                    Do
                        If DateValue(Format(.GetDisplayText(row, 51, 8), "##/##/####")) > _
                           DateValue(Format(grtscrtdt, "##/##/####")) _
                           Then grtscrtdt = .GetDisplayText(row, 51, 8)
                        row = row + 1
                        If .GetDisplayText(row, 6, 1) = " " Then
                            .TransmitTerminalKey rcIBMPf8Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            If .GetDisplayText(22, 3, 5) = "46004" Then Exit Do
                            row = 9
                        End If
                    Loop
                    'go back to page one
                    Do Until .GetDisplayText(22, 3, 5) = "46003"
                        .TransmitTerminalKey rcIBMPf7Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    Loop
                    'select the row
                    row = 9
                    Do Until .GetDisplayText(row, 51, 8) = grtscrtdt
                        row = row + 1
                        If .GetDisplayText(row, 6, 1) = " " Then
                            .TransmitTerminalKey rcIBMPf8Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            row = 9
                        End If
                    Loop
                    'add an activity record if no updates are needed
                    If .GetDisplayText(row, 25, 1) = EnrSta And _
                            .GetDisplayText(row, 29, 8) = Format(StaEffDt, "MMDDYYYY") And _
                            .GetDisplayText(row, 40, 8) = Format(AGD, "MMDDYYYY") Then
                        noUpdate
                    'update LG29 if a matching row is not found
                    Else
                        .TransmitANSI .GetDisplayText(row, 5, 2)
                        .TransmitTerminalKey rcIBMEnterKey
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        'add an activity record and go to the next task if the info cannot be updated
                        If .GetDisplayText(21, 3, 5) <> "46011" Then
                            comment = SchCd & " - " & SrcTxt & "data cannot be updated due to status of loans"
                            LP50
                        'update LG29
                        Else
                            UpdateLG29
                        End If
                    End If
                'review the record if the target is displayed (only one record)
                Else
                    'add an activity record if no updates are needed
                    If .GetDisplayText(8, 20, 1) = EnrSta And _
                            .GetDisplayText(9, 35, 8) = Format(StaEffDt, "MMDDYYYY") And _
                            .GetDisplayText(10, 35, 8) = Format(AGD, "MMDDYYYY") Then
                        noUpdate
                    'update LG29 if the information does not match
                    Else
                        UpdateLG29
                    End If
                End If
            'add enrollment information if none exists for borrower/school
            Else
                .MoveCursor 1, 7
                .TransmitANSI "A"
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                If .GetDisplayText(21, 3, 5) <> "44000" Then
                    comment = SchCd & " - " & SrcTxt & "data cannot be updated due to status of loans"
                    LP50
                Else
                    UpdateLG29
                End If
            End If
        'enter enrollment history information if it is provided
        Else
            loclLG02
            'access LG29
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "LG29C" & SSN & ";" & SchCd
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'review/update enrollment information if it already exists for borrower/school
            If .GetDisplayText(5, 22, 3) <> "SSN" Then
                'instruct the user to review the information
                warn = MsgBox("Compare the EVR to the information on OneLINK to determine the missing information.  Hit Insert when you are ready to continue.", vbOKOnly, "Review for Missing Information")
                'pause the script for the user to review info
                .WaitForTerminalKey rcIBMInsertKey, "1:0:0"
                .TransmitTerminalKey rcIBMInsertKey
                warn = MsgBox("The oldest Guarantee date is " & DTPROC & ". Would you like to add information to OneLINK after this date?", vbYesNo, "Add Information?")
                'prompt the user for information and add the information
                If warn = 6 Then
                    Do
                        GetInfo
                        If Okay = 1 Then
                            'access LG29
                            .TransmitTerminalKey rcIBMClearKey
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            .TransmitANSI "LG29C" & SSN & ";" & SchCd
                            .TransmitTerminalKey rcIBMEnterKey
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            'select the first record
                            If .GetDisplayText(20, 8, 3) = "SEL" Then
                                .TransmitANSI "01"
                                .TransmitTerminalKey rcIBMEnterKey
                            End If
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            'add an activity record and go to the next task if the info cannot be updated
                            If .GetDisplayText(21, 3, 5) <> "46011" Then
                                comment = SchCd & " - " & SrcTxt & "data cannot be updated due to status of loans"
                                LP50
                            Else
                                Src = "H"
                                UpdateLG29
                            End If
                        Else
                            Exit Do
                        End If
                        SP.Q.FastPath "LG29I" & SSN & ";" & SchCd '<4>
                    Loop
                'add an activity record if no updates are needed
                Else
                    noUpdate
                End If
            'get the last guarantee date from LG02 and prompt the user to enter info since that date if no info for borrowe/school exists
            Else
                'access LG02
                .TransmitTerminalKey rcIBMClearKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitANSI "LG02I" & SSN
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                If Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then
                    SP.Q.FastPath "LG02I;" & SSN
                End If
                
                'access the record with the greatest guar date if the selection screen is displayed
                If .GetDisplayText(21, 3, 3) = "SEL" Then
                    'access the last page
                    .MoveCursor 2, 73
                    .TransmitANSI .GetDisplayText(2, 79, 2)
                    .TransmitTerminalKey rcIBMEnterKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    'find the last row on the page
                    row = 20
                    Do Until .GetDisplayText(row, 55, 1) <> " "
                        row = row - 1
                    Loop
                    'select the record
                    .MoveCursor 21, 13
                    .TransmitANSI .GetDisplayText(row, 2, 2)
                    .TransmitTerminalKey rcIBMEnterKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                End If
                'get the guar date
                If .GetDisplayText(1, 56, 3) = "CON" Then           'CL
                    GuarDate = Format(.GetDisplayText(5, 10, 8), "@@/@@/@@@@")
                ElseIf .GetDisplayText(1, 60, 8) = "PLUS LOA" Then  'PL pre-common/common
                    GuarDate = Format(.GetDisplayText(4, 10, 8), "@@/@@/@@@@")
                ElseIf .GetDisplayText(1, 60, 8) = "PLUS MAS" Then  'PL MPN
                    GuarDate = Format(.GetDisplayText(5, 10, 8), "@@/@@/@@@@")
                ElseIf .GetDisplayText(1, 61, 3) = "SLS" Then       'SL
                    GuarDate = Format(.GetDisplayText(4, 10, 8), "@@/@@/@@@@")
                Else                                                'SF,SU, SF/SU MPN
                    GuarDate = Format(.GetDisplayText(5, 10, 8), "@@/@@/@@@@")
                End If
                .TransmitTerminalKey rcIBMClearKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitANSI "LG29A" & SSN & ";" & SchCd
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                'prompt the user to enter all enrollment info since the greatest guar date
                warn = MsgBox("Click OK and enter all enrollment information available since " & GuarDate & ".", vbOKOnly, "Enter Enrollment Information")
                'prompt the user for information and add the information
                cntr = 0
                Do
                    cntr = cntr + 1
                    GetInfo
                    If Okay = 1 Then
                        'add a new school record the first time
                        If cntr = 1 Then
                            'add an activity record and go to the next task if the info cannot be updated
                            If .GetDisplayText(21, 3, 5) <> "46011" And .GetDisplayText(21, 3, 5) <> "44000" Then
                                comment = SchCd & " - " & SrcTxt & "data cannot be updated due to status of loans"
                                LP50
                            Else
                                Src = "H"
                                UpdateLG29
                            End If
                        'update the record displayed the second time to add a new row
                        ElseIf cntr = 2 Then
                            'access LG29
                            .TransmitTerminalKey rcIBMClearKey
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            .TransmitANSI "LG29C" & SSN & ";" & SchCd
                            .TransmitTerminalKey rcIBMEnterKey
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            If .GetDisplayText(21, 3, 5) <> "46011" And .GetDisplayText(21, 3, 5) <> "44000" Then
                                comment = SchCd & " - " & SrcTxt & "data cannot be updated due to status of loans"
                                LP50
                            Else
                                Src = "H"
                                UpdateLG29
                            End If
                        'select the first record thereafter to add new rows
                        Else
                            'access LG29
                            .TransmitTerminalKey rcIBMClearKey
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            .TransmitANSI "LG29C" & SSN & ";" & SchCd
                            .TransmitTerminalKey rcIBMEnterKey
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            'select the first record
                            .TransmitANSI "01"
                            .TransmitTerminalKey rcIBMEnterKey
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            'update LG29
                            If .GetDisplayText(21, 3, 5) <> "46011" And .GetDisplayText(21, 3, 5) <> "44000" Then
                                comment = SchCd & " - " & SrcTxt & "data cannot be updated due to status of loans"
                                LP50
                            'update LG29
                            Else
                                Src = "H"
                                UpdateLG29
                            End If
                        End If
                    Else
                        Exit Do
                    End If
                Loop
            End If
        End If
    End With
End Function

Sub loclLG02()
Dim r As Integer
Dim dt As String
SP.Q.FastPath "LG02I" & SSN
If Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then
    SP.Q.FastPath "LG02I;" & SSN
    If Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then
        MsgBox "No loans were found LG02", vbOKOnly, "No Loans"
        End
    End If
End If
DTPROC = Date
    If SP.Q.Check4Text(1, 58, "LOAN APPLICATION SELECT") Then
    'selection screen
        For r = 0 To 10
            If SP.Q.GetText(10 + r, 2, 2) <> "" Then
                dt = SP.Q.GetText(10 + r, 55, 2) & "/" & SP.Q.GetText(10 + r, 57, 2) & "/" & SP.Q.GetText(10 + r, 59, 4)
                If CDate(dt) < CDate(DTPROC) Then
                    DTPROC = dt
                End If
            End If
            If r = 10 Then
                SP.Q.Hit "F8"
                If SP.Q.Check4Text(22, 3, "46004") Then
                    Exit For
                End If
                r = 0
            End If
        Next r
    Else
    'target screen
        dt = SP.Q.GetText(5, 10, 2) & "/" & SP.Q.GetText(5, 12, 2) & "/" & SP.Q.GetText(5, 14, 4)
        If CDate(dt) < CDate(DTPROC) Then
            DTPROC = dt
        End If
    End If
End Sub

Sub loclLP9O()
      SP.Q.FastPath "LP9OI" & SSN
      If SP.Q.Check4Text(1, 63, "ACTIVITY SELECTION") Then
        'multiple queue tasks
        For x = 0 To 12
            If SP.Q.GetText(8 + x, 6, 2) = "" Then
                x = 0
                SP.Q.Hit "F8"
                If SP.Q.Check4Text(22, 3, "46004") Then
                    Exit For
                End If
            End If
            If SP.Q.Check4Text(8 + x, 11, "EVSNTFLU") Then
                'if found queue
                ReDim Preserve TInfo(1, UBound(TInfo, 2) + 1)
                'get create time
                TInfo(0, UBound(TInfo, 2)) = SP.Q.GetText(8 + x, 72, 4)
                'get school
                SP.Q.PutText 21, 12, SP.Q.GetText(8 + x, 6, 2), "ENTER"
                TInfo(1, UBound(TInfo, 2)) = SP.Q.GetText(9, 25, 8)
                If IsNumeric(TInfo(1, UBound(TInfo, 2))) = False Then
                    TInfo(1, UBound(TInfo, 2)) = "0"
                End If
                SP.Q.Hit "F12"
            End If
        Next x
        oneLP9O = False
      ElseIf SP.Q.Check4Text(22, 3, "49201") Then
        '49201 NO TASK PRESENT FOR KEY DATA ENTERED
      Else
        'only one queue task
        oneLP9O = True
      End If
      SortInfo
      sameTime
End Sub

'sort the array so that it is in the same order as LP8Y
Sub SortInfo()
    Dim x As Integer
    Dim Y As Integer
    Dim temptime As String
    Dim tempsch As String
    Dim size As Integer
    Dim tt As Integer
    Dim TS As String
    
    size = UBound(TInfo, 2)
    
    For x = 1 To size
        For Y = x + 1 To size
            If CInt(TInfo(0, Y)) > CInt(TInfo(0, x)) Then
                temptime = TInfo(0, x)
                tempsch = TInfo(1, x)
                
                TInfo(0, x) = TInfo(0, Y)
                TInfo(1, x) = TInfo(1, Y)
                
                TInfo(0, Y) = temptime
                TInfo(1, Y) = tempsch
            End If
        Next Y
    Next x
    
End Sub

'get the most recent time and school, determine if there are two of the same times.
Sub sameTime()
Dim k As Integer
Dim newest As Integer
Dim time As Integer
Dim sch
    ReDim qnumber(0)
    twoTimes = False
    For k = 1 To UBound(TInfo, 2)
        time = CInt(TInfo(0, k))
        sch = TInfo(1, k)
        
        If time = mtime Then
            twoTimes = True
        End If
        
        If SchCd = sch Then
            mtime = time
            ReDim Preserve qnumber(UBound(qnumber) + 1)
            qnumber(UBound(qnumber)) = k
        End If
        
    Next k
    
End Sub

'get enrollment information from the user
Sub GetInfo()
    With Session
        Load EVInfo
        'display the dialog box until erroneous information is resolved
        Do
            'display the dialog box
            EVInfo.Show
            'end the script if the dialog box is canceled
            If Okay <> 1 Then
                Exit Sub
            End If
            'read info from dialog box to variables
            EnrSta = EVInfo.EnrSta
            'format the status effective date
            If Mid(EVInfo.StaEffDt, 2, 1) <> "/" And Mid(EVInfo.StaEffDt, 3, 1) <> "/" Then
                If Len(EVInfo.StaEffDt) = 6 Then
                    StaEffDt = Format(EVInfo.StaEffDt, "##/##/##")
                ElseIf Len(EVInfo.StaEffDt) = 8 Then
                    StaEffDt = Format(EVInfo.StaEffDt, "##/##/####")
                Else
                    StaEffDt = EVInfo.StaEffDt
                End If
            Else
                StaEffDt = EVInfo.StaEffDt
            End If
            'format the school cert date
            If Mid(EVInfo.SchCrtDt, 2, 1) <> "/" And Mid(EVInfo.SchCrtDt, 3, 1) <> "/" Then
                If Len(EVInfo.SchCrtDt) = 6 Then
                    SchCrtDt = Format(EVInfo.SchCrtDt, "##/##/##")
                ElseIf Len(EVInfo.SchCrtDt) = 8 Then
                    SchCrtDt = Format(EVInfo.SchCrtDt, "##/##/####")
                Else
                    SchCrtDt = EVInfo.SchCrtDt
                End If
            Else
                SchCrtDt = EVInfo.SchCrtDt
            End If
            'format the AGD
            If Mid(EVInfo.AGD, 2, 1) <> "/" And Mid(EVInfo.AGD, 3, 1) <> "/" Then
                If Len(EVInfo.AGD) = 6 Then
                    AGD = Format(EVInfo.AGD, "##/##/##")
                ElseIf Len(EVInfo.AGD) = 8 Then
                    AGD = Format(EVInfo.AGD, "##/##/####")
                Else
                    AGD = EVInfo.AGD
                End If
            Else
                AGD = EVInfo.AGD
            End If
            
            If IsDate(StaEffDt) = False Or CDate(StaEffDt) < CDate("01/01/1970") Then
                warn = MsgBox("The enrollment status effective date entered is not a valid date.", vbOKOnly, "Date Format Error")
            ElseIf IsDate(SchCrtDt) = False Or CDate(SchCrtDt) < CDate("01/01/1970") Then
                warn = MsgBox("The school certification date entered is not a valid date.", vbOKOnly, "Date Format Error")
            ElseIf IsDate(AGD) = False Or CDate(AGD) < CDate("01/01/1970") Then
                warn = MsgBox("The anticipated graduation date entered is not a valid date.", vbOKOnly, "Date Format Error")
            Else
                Exit Do
            End If
        Loop
        Unload EVInfo
    End With
End Sub

'update LG29
Sub UpdateLG29()
    
    With Session
    'enter data
        .MoveCursor 7, 41
        .TransmitANSI Src
        .MoveCursor 8, 20
        .TransmitANSI EnrSta
        .MoveCursor 9, 35
        .TransmitANSI Format(StaEffDt, "MMDDYYYY")
        .MoveCursor 10, 35
        .TransmitANSI Format(AGD, "MMDDYYYY")
        .MoveCursor 10, 71
        .TransmitANSI SchCd
        .MoveCursor 11, 35
        .TransmitANSI Format(SchCrtDt, "MMDDYYYY")
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        InstID = SchCd
        InstTyp = "001"
    'error handling
        Select Case .GetDisplayText(21, 3, 5)
            Case "49000", "48003"
                Updated
            Case "10000", "10001"
                LP9OComment1 = "call school to verify agd"
                CALLSCHL
            Case "10002"
                'add task to HISTREVR queue
                Queue = "HISTREVR"
                LP9OComment1 = "please request a full enrollment history"
                LP9O
                'add activity record
                comment = SchCd & " - " & SrcTxt & " no update, enrollment history being requested"
                LP50
                'prompt user
                warn = MsgBox("No update on OneLINK.  Enrollment history being requested.", vbOKOnly, "Enrollment History Requested")
            Case "10003"
                LP9OComment1 = "call school to verify accuracy of LOA information"
                LOACALL1
            Case "10004", "10007", "10009", "10011", "10017"
                Src = "H"
                UpdateLG29
            Case "10005"
                LP9OComment1 = "call school to verify accuracy of L status effective date"
                LOACALL1
            Case "10006"
                LP9OComment1 = "call school to verify accuracy of L status"
                LOACALL1
            Case "10008"
                LP9OComment1 = "call school to verify W or L status and effective date"
                CALLSCHL
            Case "10010"
                UpdateNN
            Case "10012"
                LP9OComment1 = "call school to verify accuracy of G status"
                CALLSCHL
            Case "10013"
                UpdateNN
            Case "10014"
                LP9OComment1 = "call school to verify X status"
                NVRENRLC
            Case "10015"
                LP9OComment1 = "call school to verify correct status and date"
                NVRENRLC
            Case "10016", "14000"
                EVRDEATH
            Case "10052"
                noUpdate
            Case Else
                warn = MsgBox("Unknown error code encountered.  No action taken.", vbOKOnly, "Unknown Error Code")
        End Select
    End With
    
End Sub

'add an activity record and prompt user if the data already exisits
Sub noUpdate()
    With Session
        comment = SchCd & " - " & SrcTxt & " no update"
        LP50
        warn = MsgBox("The data already exists on OneLINK.", vbOKOnly, "Data Already Exists")
    End With
End Sub

'add an activity record and prompt user if no updates were necessary
Sub UpdateNN()
    With Session
        comment = SchCd & " - " & SrcTxt & " no update"
        LP50
        warn = MsgBox("Update not necessary on OneLINK.", vbOKOnly, "Update not Necessary")
    End With
End Sub

'add an activity record and prompt user if the data was updated
Sub Updated()
    With Session
        comment = SchCd & " - " & SrcTxt & " updated"
        LP50
        warn = MsgBox("Data updated on OneLINK.", vbOKOnly, "Data Updated")
    End With
End Sub

'add task to CALLSCHL queue, add an activity record and prompt user with results
Sub CALLSCHL()
    With Session
    'add task to CALLSCHL queue
        Queue = "CALLSCHL"
        LP9O
    'add activity record
        comment = SchCd & " - " & SrcTxt & " sent to queue for review"
        LP50
    'prompt user
        warn = MsgBox("Sent to queue for follow up with school.", vbOKOnly, "Sent to Queue")
    End With
End Sub

'add task to LOACALL1 queue, add an activity record and prompt user with results
Sub LOACALL1()
    With Session
    'add task to LOACALL1 queue
        Queue = "LOACALL1"
        LP9O
    'add activity record
        comment = SchCd & " - " & SrcTxt & " sent to queue for review"
        LP50
    'prompt user
        warn = MsgBox("Sent to queue for follow up with school.", vbOKOnly, "Sent to Queue")
    End With
End Sub

'add task to NVRENRLC queue, add an activity record and prompt user with results
Sub NVRENRLC()
    With Session
    'add task to NVRENRLC queue
        Queue = "NVRENRLC"
        LP9O
    'add activity record
        comment = SchCd & " - " & SrcTxt & " sent to queue for review"
        LP50
    'prompt user
        warn = MsgBox("Sent to queue for follow up with school.", vbOKOnly, "Sent to Queue")
    End With
End Sub

'add task to EVRDEATH queue, add an activity record and prompt user with results
Sub EVRDEATH()
    With Session
    'add task to EVRDEATH queue
        Queue = "EVRDEATH"
        LP9OComment1 = "follow up to verify student's death"
        LP9O
    'add activity record
        comment = SchCd & " - " & SrcTxt & " sent to death queue for review"
        LP50
    'prompt user
        warn = MsgBox("Sent to queue for review.", vbOKOnly, "Sent to Queue")
    End With
End Sub

Function Inn(row As Integer, col As Integer, CheckFor As String) As Boolean
    Dim aCheckFor() As String
    Dim i As Integer
    
    aCheckFor = Split(CheckFor, ",")
    For i = 0 To UBound(aCheckFor)
        If Check4Text(row, col, aCheckFor(i)) Then
            Inn = True
            Exit Function
        End If
    Next i
    Inn = False
End Function

'determine if the loan type as displayed on PO1N is a private loan
Function HasPO1NPrivateLoan() As Boolean
'this function is more complicated than would seem necessary except there is no way to
'know exactly where the loan type will be in the loan type description at the top of PO1N so
'the script gets all of the loan types and checks to see if any of them are anywhere in the
'description
    
    Dim loanTypes() As String
    Dim loanTypeDescription As String
    
    'set variables
    HasPO1NPrivateLoan = False
    loanTypeDescription = GetText(2, 24, 46)
    
    'get private loan types from table
    loanTypes = SP.SQL("SELECT LoanType FROM GENR_REF_LoanTypes WHERE TypeKey = 'Private'")
    
    'review each loan type from table to determine if it appears in the loan type description
    For i = 1 To UBound(loanTypes)
        If InStr(1, loanTypeDescription, " " & loanTypes(i) & " ") Then
            HasPO1NPrivateLoan = True
            Exit Function
        End If
    Next i
End Function

'determine if the loan has a future active disbursement
Function CheckDI()
    Dim col As Integer
    Dim disbDate As String


    col = 30
    While Not Check4Text(22, 2, "90007")
        'get disbursement date in case it is blank so the script won't debug when using the datevalue function on it
        If IsDate(Replace(GetText(11, col, 10), " ", "/")) Then disbDate = Replace(GetText(11, col, 10), " ", "/") Else disbDate = Date

        'apply edits to determine disbursement status
        If DateValue(disbDate) > DateValue(Date) Then
            If (Not Check4Text(8, col, "INACTIVE") And Check4Text(12, col - 5, "____")) Or _
               (Not Check4Text(12, col - 5, "____") And GetText(12, col, 10) <> GetText(10, col, 10)) Then
               'add loan to list if has a future active disbursement
                ReDim Preserve PrivateLoans(UBound(PrivateLoans) + 1)
                PrivateLoans(UBound(PrivateLoans)) = CInt(GetText(3, 76, 4))
                Exit Function
            End If
        End If
        
        'go to the next column/page
        col = col + 20
        If col > 70 Then
            Hit "F8"
            col = 30
        End If
    Wend
End Function


'enter comments in LP50
Sub LP50()
    With Session
        'access LP50
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP50A" & SSN
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI ActCd
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        'enter contact and activity type if no blank or pause for the user to enter them
        If ActTyp = "" Then
            .WaitForTerminalKey rcIBMInsertKey, "1:00:00"
        Else
            .TransmitANSI ActTyp
            .TransmitANSI ConTyp
        End If
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        'enter the comment
        .MoveCursor 13, 2
        If PauPlea = "Y" Then
            .TransmitANSI comment
            .WaitForTerminalKey rcIBMInsertKey, "1:00:00"
            .TransmitTerminalKey rcIBMInsertKey
            row = 13
            col = 2
            Do Until row = 21
                If .GetDisplayText(row, col, 12) = "____________" Then
                    .MoveCursor row, col
                    .TransmitANSI Script
                    Exit Do
                ElseIf col = 62 Then
                    row = row + 1
                    col = 2
                Else
                    col = col + 1
                End If
            Loop
        Else
            .TransmitANSI comment & Script
        End If
        .TransmitTerminalKey rcIBMPf6Key
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
    End With
End Sub

'add a task to the specified queue
Sub LP9O()
    With Session
        'access LP9O
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP9OA" & SSN & ";" & InstID & ";" & Queue
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'prompt user if unable to add task
        If .GetDisplayText(22, 3, 5) <> "44000" Then
            MsgBox "Unable to add the task.  Wait for the script to finish and then enter the task manually."
        'enter info
        Else
            .MoveCursor 9, 34
            .TransmitANSI InstTyp
            .MoveCursor 11, 25
            .TransmitANSI Format(DateDue, "MMDDYYYY")
            .MoveCursor 16, 12
            .TransmitANSI LP9OComment1
            .MoveCursor 17, 12
            .TransmitANSI LP9OComment2
            .MoveCursor 18, 12
            .TransmitANSI LP9OComment3
            .MoveCursor 19, 12
            .TransmitANSI LP9OComment4
            If PauPlea = "Y" Then
                .WaitForTerminalKey rcIBMInsertKey, "1:00:00"
            End If
            .TransmitTerminalKey rcIBMPf6Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .Wait 2 'go figure why this has to be here but it wouldn't work otherwise
        End If
    End With
End Sub
