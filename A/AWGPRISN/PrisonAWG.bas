Attribute VB_Name = "PrisonAWG"
Private LP9AComment1 As String
Private LP9AComment2 As String
Private LP9AComment3 As String
Private LP9AComment4 As String
Private contact As String
Private holddate As String
Private notes As String

'prompt the user to contact the employer and update OneLINK to complete tasks in the LEMPRISN queu
Public Sub PrisonAWG()
    Script = "  {AWGPRISN}"
    'warn the user that the script will complete tasks in the LEMPRISN queue
    If MsgBox("This script will prompt you to contact employers to complete tasks in the LEMPRISN queue.  Click OK to proceed or Cancel to quit.", vbOKCancel, "AWG Prison") <> vbOK Then End
    'access LEMPRISN queue
    FastPath "LP9ACLEMPRISN"
    'warn the user and end the script if no more tasks are found
    If Check4Text(22, 3, "47423") Or Check4Text(22, 3, "47420") Then
        MsgBox "There are no tasks in the LEMPRISN queue.  Processing is complete."
        End
    End If
    'warn the user and end the script if a task from a queue other than DPRISON is displayed
    If Not Check4Text(1, 9, "LEMPRISN") Then
        MsgBox "You have an unresolved task in the " & GetText(1, 9, 8) & " queue.  You must complete the task before working the LEMPRISN queue."
        End
    End If
    NextRec
End Sub

'get information about the task
Private Sub NextRec()
    SSN = GetText(17, 70, 9)
    'get the task comment
    LP9AComment1 = GetText(12, 11, 50)
    LP9AComment2 = GetText(12, 61, 8) & GetText(13, 11, 42)
    LP9AComment3 = GetText(13, 53, 16) & GetText(14, 11, 34)
    LP9AComment4 = GetText(14, 45, 24) & GetText(15, 11, 26)
    'pause for the user to read the comment
    PauseForInsert
    'go to the LC67Ver subroutine to verify that an active GG record exists
    LC67Ver
    'go to LC20 to get the employer ID
    FastPath "LC20I" & SSN
    EmployerID = GetText(10, 12, 7)
    'go to LPEM to get the employer information
    FastPath "LPEMI" & EmployerID
    employer = GetText(6, 21, 20)
    'pause for the user to contact the employer
    PauseForInsert
    Do
        'display dialog box for user input
        ResultInd = ""
        Load AWGPrsn
        AWGPrsn.Show
        'quit if the OK button was not clicked
        If Okay <> 1 Then
            Unload AWGPrsn
            End
        End If
        'gather values from dialog box variables
        If InStr(1, AWGPrsn.holddate, "/", 1) = 3 Then
            holddate = AWGPrsn.holddate
        Else
            holddate = Format(AWGPrsn.holddate, "##/##/####")
        End If
        contact = AWGPrsn.contact
        notes = AWGPrsn.notes
        Unload AWGPrsn
        'borrower no longer employed
        If ResultInd = 1 Then
            'go to the LC67 subroutine to withdraw the AWG action
            LC67
            'create a task in DPRISON
            Queue = "DPRISON"
            DateDue = Date
            LP9OComment1 = LP9AComment1
            LP9OComment2 = LP9AComment2
            LP9OComment3 = LP9AComment3
            LP9OComment4 = LP9AComment4
            Common.LP9O
            'add activity record to LP50
            ActCd = "L030N"
            ActTyp = "TC"
            ConTyp = "81"
            comment = "contacted " & contact & " at " & employer & " (" & EmployerID & "), borr no longer employed, task created in DPRISON, AWG action withdrawn (05, nle), " & notes
            Common.LP50
            Exit Do
        'borrower employed on leave
        ElseIf ResultInd = 2 Then
            'go to the LC67 subroutine to put a hold on the AWG action
            LC67
            'add activity record to LP50
            ActCd = "LSBEI"
            ActTyp = "TC"
            ConTyp = "81"
            comment = "contacted " & contact & " at " & employer & " (" & EmployerID & "), borr on leave, hold reason 23 put on AWG action to expire " & Format(holddate, "MM/DD/YYYY") & ", " & notes
            Common.LP50
            Exit Do
        'borrower not incarcerated
        ElseIf ResultInd = 3 Then
            'create a task in DJAILBRD
            Queue = "DJAILBRD"
            DateDue = Date
            LP9OComment1 = LP9AComment1
            LP9OComment2 = LP9AComment2
            LP9OComment3 = LP9AComment3
            LP9OComment4 = "empl says borr not incarc, please verify"
            Common.LP9O
            'add activity record to LP50
            ActCd = "LSBEI"
            ActTyp = "TC"
            ConTyp = "81"
            comment = "contacted " & contact & " at " & employer & " (" & EmployerID & "), borr not incarc, task created in DJAILBRD to verify borr status, " & notes
            Common.LP50
            Exit Do
        'unable to contact employer
        ElseIf ResultInd = 4 Then
            'create a task in LEMPRISN
            Queue = "LEMPRISN"
            DateDue = Date + 1
            LP9OComment1 = LP9AComment1
            LP9OComment2 = LP9AComment2
            LP9OComment3 = LP9AComment3
            LP9OComment4 = LP9AComment4
            Common.LP9O
            'add activity record to LP50
            ActCd = "LUBEI"
            ActTyp = "TT"
            ConTyp = "81"
            comment = "unable to contact " & employer & " (" & EmployerID & ") to verify borr incar status, task created in LEMPRISN to follow up tomorrow " & notes
            Common.LP50
            Exit Do
        'no result selected
        Else
            MsgBox "No result selected.  Click OK to enter information again."
        End If
    Loop
    'go to the Finish subroutine to complete the task and select the next task
    Finish
End Sub

'complete the task and select the next task
Private Sub Finish()
    'access LP9A
    FastPath "LP9ACLEMPRISN"
    'complete the task
    Hit "F6"
    'select the next task
    Hit "F8"
    'warn the user and end the script if there are no more tasks to complete
    If Check4Text(22, 3, "46004") Then
        MsgBox "Processing Complete"
        End
    End If
    'return to the NextRec subroutine to process the next record
    NextRec
End Sub

Private Sub LC67Ver()
    'access LC67
    FastPath "LC67I" & SSN & "GG"
    'enter the first record is the selection screen is displayed
    If Check4Text(21, 3, "SEL") Then
        Session.TransmitANSI "01"
        Hit "ENTER"
    End If
    'stop looking if an open record is found
    Do While (Not Check4Text(16, 63, "  ")) And (Not Check4Text(8, 19, "  "))
        'go to the next record
        Hit "F8"
        'return the task to DPRISON and enter and activity record if no open records are found
        If Check4Text(22, 3, "46004") Then
            'create a task in DPRISON
            Queue = "DPRISON"
            DateDue = Date
            LP9OComment1 = LP9AComment1
            LP9OComment2 = LP9AComment2
            LP9OComment3 = LP9AComment3
            LP9OComment4 = LP9AComment4
            Common.LP9O
            'add activity record to LP50
            ActCd = "LPAWG"
            ActTyp = "AM"
            ConTyp = "10"
            comment = "no active employer for borrower in LEMPRISN, task created in DPRISON to re-review borrower"
            Common.LP50
            'go to the Finish subroutine to complete the task and select the next task
            Finish
        End If
    Loop
End Sub

'withdraw or hold the AWG action
Private Sub LC67()
    PADF = ""
    Do
        'access LC67
        FastPath "LC67C" & SSN & "GG" & PADF
        'continue processing if the target is displayed
        If Check4Text(1, 70, "AWG") Then
            PADF = GetText(4, 35, 8)
            Exit Do
        'warn the user an prompt for the correct primary action date filed if the record was not found
        ElseIf Check4Text(22, 3, "47004") Then
            MsgBox "The record was not found.  Click OK to reenter the Primary Action Date Filed."
            PADF = ""
        'prompt the user to enter the primary action date filed if the selection screen is displayed
        Else
            MsgBox "Review the AWG records to determine which Primary Action Date Filed to select and then hit Insert to continue."
            PauseForInsert
            PADF = InputBox("Enter the Primary Action Date Filed and click OK to release the AWG action or click Cancel to quit.", "Release AWG", "Enter PADF")
            If PADF = "" Then End
        End If
    Loop
    'withdraw the action if the borrower is no longer employed
    If ResultInd = 1 Then
        'if the inactive reason is not blank, warn the user and end the script  <1>
        If (Not Check4Text(16, 63, "__")) Or (Not Check4Text(8, 19, "__")) Then
            warn = MsgBox("The selected record cannot be withdrawn because it is already withdrawn or inactive.  Click OK to select another record.", vbOKOnly, "Inactive Record Selected")
            LC67
        Else
             'enter the withdrawn date
            PutText 7, 35, Format(Date, "MMDDYYYY")
            'enter the withdrawn reason 05 for no longer employed
            PutText 8, 19, "05"
            Hit "ENTER"
        End If
    End If
    'put a hold on the AWG action if the borrower is on leave
    If ResultInd = 2 Then
        If (Not Check4Text(16, 63, "__")) Or (Not Check4Text(8, 19, "__")) Then
            warn = MsgBox("A hold cannot be placed on the selected record because it is already withdrawn or inactive.  Click OK to select another record.", vbOKOnly, "Inactive Record Selected")
            LC67
        Else
            'enter the hold date
            PutText 17, 71, Format(holddate, "MMDDYYYY")
            'enter the hold reason 05 for incarcerated
            PutText 18, 57, "23"
            Hit "ENTER"
        End If
    End If
End Sub
