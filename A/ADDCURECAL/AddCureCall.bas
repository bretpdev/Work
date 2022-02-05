Attribute VB_Name = "AddCureCall"
Sub Main()
    Dim ssn As String
    Dim row As Integer
    Dim RecoveryPhase As Integer
    Dim RecoveryLog As String
    Dim lSSN As String
    Dim UID As String
    Dim LnSeq(1) As Integer
    Dim TaskDoesNotExist As Boolean
    LnSeq(0) = 1
    If Not SP.Common.CalledByMBS Then If vbOK <> MsgBox("This script will create tasks in the CU IN queue.  Click OK to continue or Cancel to end the script.", vbOKCancel) Then End '<1>
    SP.Common.TestMode , , RecoveryLog
    UID = SP.Common.GetUserID()
    'check recovery log
    If Dir(RecoveryLog & "AddCureLog.txt") = "" Then
        'not in recovery
        RecoveryPhase = 0
        lSSN = ""
    Else
        'Recovery
        MsgBox "The script is now in recovery.", vbInformation
        Open RecoveryLog & "AddCureLog.txt" For Input As #1
        Input #1, RecoveryPhase, lSSN
        Close #1
    End If
    'check if recovery phase
    If RecoveryPhase < 1 Then
        'gather queue information into data file
        FastPath "TX3ZITX6XGV;IV"
        'fastpath "TX3ZITX6XRA;01"
        If Check4Text(1, 74, "TXX71") = False Then
            MsgBox "The ""GVIV"" queue appears to be empty.  Processing complete.", vbInformation
            ProcComp "MBSADDCURECAL.TXT", False '<1>
            End
        End If
        'open data file
        Open "T:\Add Cure Call Dat.txt" For Output As #1
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            row = 8
            While row <= 17
                'pick up queue information if task exists
                If Check4Text(row, 4, " ") = False Then Write #1, GetText(row, 6, 9)
                row = row + 3
            Wend
            'page forward
            If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                Hit "Enter"
            Else
                Hit "F8"
            End If
        Wend
        Close #1
        'update recovery
        Open RecoveryLog & "AddCureLog.txt" For Output As #1
        Write #1, 1, ""
        Close #1
    End If
    'second phase
    If RecoveryPhase < 2 Then
        'open data file
        Open "T:\Add Cure Call Dat.txt" For Input As #1
        'find last SSN Processed successfully
        If RecoveryPhase = 2 Then
            While lSSN <> ssn
                Input #1, ssn
            Wend
        End If
        'process file
        While Not EOF(1)
            Input #1, ssn
            'add queue task
            If LATD22FirstLoan(ssn, "CUINC", "", "ADDCURECAL", UID, TaskDoesNotExist) = False Then
                If TaskDoesNotExist Then
                    MsgBox "You need access to the ""CUINC"" ARC to use this script successfully.  Please contact Systems Support.", vbCritical
                    End
                End If
            End If
            'update recovery
            Open RecoveryLog & "AddCureLog.txt" For Output As #2
            Write #2, 2, ssn
            Close #2
        Wend
        Close #1
    End If
    'clean up data and log files
    Kill "T:\Add Cure Call Dat.txt"
    Kill RecoveryLog & "AddCureLog.txt"
    '<1>MsgBox "Processing Complete.", vbInformation
    ProcComp "MBSADDCURECAL.TXT" '</1>
    End
End Sub


'enters an activity record/action request in COMPASS selecting only the loans specified
Function LATD22FirstLoan(ssn As String, ARC As String, comment As String, Script As String, UserID As String, TaskDoesNotExist As Boolean, Optional PauPls As Boolean = False) As Boolean
    Dim row As Integer
    TaskDoesNotExist = True
    LATD22FirstLoan = True
    
    FastPath "TX3Z/ATD22" & ssn
    If Not Check4Text(1, 72, "TDX23") Then
        LATD22FirstLoan = False
        Exit Function
    End If
    'find the ARC
    Do
        found = Session.FindText(ARC, 8, 8)
        If found Then Exit Do
        Hit "F8"
        If Check4Text(23, 2, "90007") Then
            LATD22FirstLoan = False
            Exit Function
        End If
    Loop
    'select the ARC
    puttext Session.FoundTextRow, Session.FoundTextColumn - 5, "01", "ENTER"
    'exit the function if the selection screen is not displayed
    If Not Check4Text(1, 72, "TDX24") Then
        LATD22FirstLoan = False
        Exit Function
    End If
    'select first loan
    puttext 11, 3, "X"
    'enter short comments
    If Len(comment) < 132 Then
        puttext 21, 2, comment & "  {" & Script & "} /" & UserID
        If PauPls Then
            If vbYes = MsgBox("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", 36, "Add Additional Comments") Then
                Session.WaitForTerminalKey rcIBMInsertKey, "1:00:00"
                Hit "INS"
            End If
        End If
        Hit "ENTER"
        TaskDoesNotExist = (Not Check4Text(23, 2, "01764 TASK RECORD ALREADY EXISTS"))
        If Not Check4Text(23, 2, "02860") Then LATD22FirstLoan = False
    'enter long comments
    Else
        'fill the first screen
        puttext 21, 2, MID(comment, 1, 154), "ENTER"
        If Not Check4Text(23, 2, "02860") Then
            LATD22FirstLoan = False
            Exit Function
        End If
        Hit "F4"
        'enter the rest on the expanded comments screen
        For k = 155 To Len(comment)
            Session.TransmitANSI MID(comment, k, 260)
            k = k + 260
        Next k
        Session.TransmitANSI "  {" & Script & "} /" & UserID
        If PauPls Then
            If vbYes = MsgBox("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", 36, "Add Additional Comments") Then
                Session.WaitForTerminalKey rcIBMInsertKey, "1:00:00"
                Hit "INS"
            End If
        End If
        Hit "ENTER"
        TaskDoesNotExist = (Not Check4Text(23, 2, "01764 TASK RECORD ALREADY EXISTS"))
        If Not Check4Text(23, 2, "02114") Then LATD22FirstLoan = False
    End If
End Function

'new, SR1310, aa
'<1>, sr1537, tp, 05/01/06

