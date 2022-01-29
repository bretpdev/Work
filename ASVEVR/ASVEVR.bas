Attribute VB_Name = "ASVEVR"
Sub Main()
    Dim LastSSNStillInQueue As String
    Dim SSNInProc As String
    Dim DtReq As String
    Dim sel As String
    Dim Status As String
    If Not SP.Common.CalledByMBS Then If vbOK <> MsgBox("This script does ASV EVR follow up processing.  Please click OK to continue.", vbOKCancel, "ASV EVR") Then End
    'make sure that the user is using the right user id
    If SP.Common.GetUserID() <> "UT00204" And Not SP.Common.CalledByMBS Then
        MsgBox "You must use ID ""UT00204"" to run this script.  Please log out and log back in using ""UT00204."""
        End
    End If
    'there can be one of two ending conditions a) queue is empty b) end of the queue is found
    While 1 'script will end when one of the ending conditions are met.
        SearchForSSN LastSSNStillInQueue, SSNInProc, DtReq
        'check if LP50
        If ActDtGorEDtReq(SSNInProc, DtReq) Then
            sel = SearchForSSNInProc(SSNInProc, Status)
            If Status = "U" Then
                puttext 21, 18, sel, "Enter"
                sel = SearchForSSNInProc(SSNInProc, Status)
            End If
            puttext 21, 18, sel, "F2"
            puttext 8, 19, "CCOMPL", "Enter" 'complete task
        Else
            LastSSNStillInQueue = SSNInProc 'the task is going to remain in the queue so it is the starting point of the next search through the queue.
            sel = SearchForSSNInProc(SSNInProc, Status)
            If Status = "U" Then
                puttext 21, 18, sel, "Enter"
                sel = SearchForSSNInProc(SSNInProc, Status)
                puttext 21, 18, sel, "F2"
                puttext 8, 19, "H", "Enter" 'put task on hold
            ElseIf Status = "W" Then
                puttext 21, 18, sel, "F2"
                puttext 8, 19, "H", "Enter" 'put task on hold
            End If
        End If
    Wend
End Sub

'This function searches for the SSN in Processing
Function SearchForSSNInProc(ssn As String, s As String) As String
    FastPath "TX3ZITX6XE2;AS"
    'search for SSN
    While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        If Check4Text(8, 6, ssn) Then
            SearchForSSNInProc = GetText(8, 4, 1)
            s = GetText(8, 75, 1)
            Exit Function
        ElseIf Check4Text(11, 6, ssn) Then
            SearchForSSNInProc = GetText(11, 4, 1)
            s = GetText(11, 75, 1)
            Exit Function
        ElseIf Check4Text(14, 6, ssn) Then
            SearchForSSNInProc = GetText(14, 4, 1)
            s = GetText(14, 75, 1)
            Exit Function
        ElseIf Check4Text(17, 6, ssn) Then
            SearchForSSNInProc = GetText(17, 4, 1)
            s = GetText(17, 75, 1)
            Exit Function
        End If
        If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
            Hit "Enter"
        Else
            Hit "F8"
        End If
    Wend
End Function

'this function does the LP50 check for GEVRR Action code Act Date
Function ActDtGorEDtReq(SSNInProc, DR As String) As Boolean
    FastPath "LP50I" & SSNInProc
    puttext 9, 20, "GEVRR", "Enter" 'search for action code
    If Check4Text(1, 58, "ACTIVITY SUMMARY SELECT") Then
        puttext 6, 2, "X", "Enter" 'select first one if selection screen is found
    End If
    'check if action code exists
    If Check4Text(1, 68, "ACTIVITY MENU") Then
        ActDtGorEDtReq = False
    Else 'act code was found
        'check if Act Dt >= task date requested
        If CDate(Format(GetText(7, 15, 8), "##/##/####")) >= CDate(DR) Then
            ActDtGorEDtReq = True
        Else
            ActDtGorEDtReq = False
        End If
    End If
End Function

'this function searches for the SSN fed to it.  If the ssn is blank it figures that the SSN is the first one in the queue.  It returns the selection number of the SSN queue task
Function SearchForSSN(ssn As String, SSN2 As String, DtReq As String)
    FastPath "TX3ZITX6XE2;AS"
    'if primary key screen is found then queue has no tasks
    If Check4Text(1, 72, "TXX6Y") Then
'<1->
'       MsgBox "Processing Complete"
'       End
        ProcComp "MBSASVEVR.txt"
'</1>
    End If
    If ssn = "" Then
        'search for the first task with DATE REQUESTED <= (current date - 10)
        SearchForNext SSN2, DtReq
    Else
        'search for SSN
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            If Check4Text(8, 6, ssn) Then
                'search remaining tasks on the screen
                If Check4Text(11, 4, "  ") = False Then
                    If CDate(GetText(11, 47, 10)) <= (Date - 10) Then
                        DtReq = GetText(11, 47, 10)
                        SSN2 = GetText(11, 6, 9)
                        Exit Function
                    End If
                End If
                If Check4Text(14, 4, "  ") = False Then
                    If CDate(GetText(14, 47, 10)) <= (Date - 10) Then
                        DtReq = GetText(14, 47, 10)
                        SSN2 = GetText(14, 6, 9)
                        Exit Function
                    End If
                End If
                If Check4Text(17, 4, "  ") = False Then
                    If CDate(GetText(17, 47, 10)) <= (Date - 10) Then
                        DtReq = GetText(17, 47, 10)
                        SSN2 = GetText(17, 6, 9)
                        Exit Function
                    End If
                End If
                If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                    Hit "Enter"
                Else
                    Hit "F8"
                End If
                'if next possible task isn't on the screen then search subsequent screens for the first task with DATE REQUESTED <= (current date - 10)
                SearchForNext SSN2, DtReq
                Exit Function
            ElseIf Check4Text(11, 6, ssn) Then
                'search remaining tasks on the screen
                If Check4Text(14, 4, "  ") = False Then
                    If CDate(GetText(14, 47, 10)) <= (Date - 10) Then
                        DtReq = GetText(14, 47, 10)
                        SSN2 = GetText(14, 6, 9)
                        Exit Function
                    End If
                End If
                If Check4Text(17, 4, "  ") = False Then
                    If CDate(GetText(17, 47, 10)) <= (Date - 10) Then
                        DtReq = GetText(17, 47, 10)
                        SSN2 = GetText(17, 6, 9)
                        Exit Function
                    End If
                End If
                If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                    Hit "Enter"
                Else
                    Hit "F8"
                End If
                'if next possible task isn't on the screen then search subsequent screens for the first task with DATE REQUESTED <= (current date - 10)
                SearchForNext SSN2, DtReq
                Exit Function
            ElseIf Check4Text(14, 6, ssn) Then
                'search remaining tasks on the screen
                If Check4Text(17, 4, "  ") = False Then
                    If CDate(GetText(17, 47, 10)) <= (Date - 10) Then
                        DtReq = GetText(17, 47, 10)
                        SSN2 = GetText(17, 6, 9)
                        Exit Function
                    End If
                End If
                If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                    Hit "Enter"
                Else
                    Hit "F8"
                End If
                'if next possible task isn't on the screen then search subsequent screens for the first task with DATE REQUESTED <= (current date - 10)
                SearchForNext SSN2, DtReq
                Exit Function
            ElseIf Check4Text(17, 6, ssn) Then
                If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                    Hit "Enter"
                Else
                    Hit "F8"
                End If
                'if next possible task isn't on the screen then search subsequent screens for the first task with DATE REQUESTED <= (current date - 10)
                SearchForNext SSN2, DtReq
                Exit Function
            End If
            If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                Hit "Enter"
            Else
                Hit "F8"
            End If
        Wend
        'if the script exits the loop normally then all queue tasks have been processed
'<1->
'       MsgBox "Processing Complete"
'       End
        ProcComp "MBSASVEVR.txt"
'</1>
    End If
End Function

'this function searches for the next possible task for the right date range
Function SearchForNext(SSN2 As String, DtReq As String)
    'search for the first task with DATE REQUESTED <= (current date - 10)
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            If Check4Text(8, 4, "  ") = False Then
                If CDate(GetText(8, 47, 10)) <= (Date - 10) Then
                    SSN2 = GetText(8, 6, 9)
                    DtReq = GetText(8, 47, 10)
                    Exit Function
                End If
            End If
            If Check4Text(11, 4, "  ") = False Then
                If CDate(GetText(11, 47, 10)) <= (Date - 10) Then
                    SSN2 = GetText(11, 6, 9)
                    DtReq = GetText(11, 47, 10)
                    Exit Function
                End If
            End If
            If Check4Text(14, 4, "  ") = False Then
                If CDate(GetText(14, 47, 10)) <= (Date - 10) Then
                    SSN2 = GetText(14, 6, 9)
                    DtReq = GetText(14, 47, 10)
                    Exit Function
                End If
            End If
            If Check4Text(17, 4, "  ") = False Then
                If CDate(GetText(17, 47, 10)) <= (Date - 10) Then
                    SSN2 = GetText(17, 6, 9)
                    DtReq = GetText(17, 47, 10)
                    Exit Function
                End If
            End If
            If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                Hit "Enter"
            Else
                Hit "F8"
            End If
        Wend
        'if the script exits the loop normally then all queue tasks have been processed
'<1->
'       MsgBox "Processing Complete"
        ProcComp "MBSASVEVR.txt"
'</1>
        End
End Function

'new sr1014, aa, 06/13/05, 06/22/05
'<1> sr1539, jd, disabled prompts if called by MBS


