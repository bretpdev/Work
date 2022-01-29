Attribute VB_Name = "BatchReQueue"
Private CommentArray() As String
'Indices for CommentArray: 0 = Comment, 1 = AddTaskIndicator
Private NumInAC As Integer
Private ARCsNeeded() As String
Private NumInAN As Integer
Private SSNsNoCancel() As String
Private SNCNum As Integer

Public Sub Main()
    Dim UserID As String
    If Not SP.Common.CalledByMBS Then
        If vbOK <> MsgBox("This script re-queues tasks.  Please click 'OK' continue or 'Cancel' to stop the script.", vbOKCancel, "Re-Queue Tasks") Then End
    End If
    'init vars
    NumInAC = 0
    NumInAN = 0
    ReDim ARCsNeeded(0)
    ReDim CommentArray(0, 0)
    ReDim SSNsNoCancel(0)
    'Create Needed ARC Report
    LP9AGetTasks
    DecideIfValidTaskToAdd
    UserID = SP.Common.GetUserID()
    TD22AddValidTasks UserID
    LP8YCancelTasks UserID
    
    If SP.Common.TestMode Then
        Open "X:\PADD\Logs\Test\ReQueueLog.txt" For Output As #1
    Else
        Open "X:\PADD\Logs\ReQueueLog.txt" For Output As #1
    End If
    
    Write #1, "Done", "Done"
    Close #1
    
    'Create and e-mail ARCs needed report
    If NumInAN > 0 Then
        CreateAndEmailReport UserID
    End If
    
    ProcComp "MBSREQUEBAT.TXT"
End Sub

'This function sends an email error report to System support Personnel that can assign the user to the ARC
Private Sub CreateAndEmailReport(UserID As String)
    Dim counter As Integer
    Dim eSubject As String
    Dim eBody As String
    
    'set subject and body text
    eSubject = "ARCs needed for " & UserID
    eBody = "UserID " & UserID & " needs access to the following ARCs in COMPASS:" & vbLf & vbLf
    
    While counter < NumInAN
        eBody = eBody & ARCsNeeded(counter) & vbLf
        counter = counter + 1
    Wend
    eBody = eBody & vbLf & "This was sent by the Batch Re-queue." & vbLf & vbLf & "Thanks"
    
    'send message
    If SP.Common.TestMode Then
        SP.Common.SendMail SP.Common.BSYSRecips("BatchReQueue"), , eSubject, eBody, , , , , True
    Else
        SP.Common.SendMail SP.Common.BSYSRecips("BatchReQueue"), , eSubject, eBody
    End If
End Sub

'This function cancels all tasks in the ZZZZCOMP queue that are assigned to the user
'and have a date last worked <= current day.
Private Sub LP8YCancelTasks(UserID As String)
    Dim row As Integer
    Dim counter As Integer
    Dim CancelTask As Boolean
    CancelTask = True
    row = 7
    FastPath "LP8YCALL;ZZZZCOMP"
    Session.TransmitANSI "1"
    Hit "Enter"
    'change first task to "A" status
    puttext 7, 33, "A"
    'Cancel the first task because it no longer fits the criteria off the others
    For counter = 0 To SNCNum - 1
        If Check4Text(row, 2, SSNsNoCancel(counter)) Then
            CancelTask = False
            Exit For
        End If
    Next counter
    If CancelTask Then puttext 7, 33, "X" & UserID
    Hit "Enter"
    Hit "F6"
    While Check4Text(row, 38, UserID) _
    And Check4Text(row, 33, "A") _
    And CDate(GetText(row, 26, 2) & "/" & GetText(row, 28, 2) & "/" & GetText(row, 22, 4)) <= Date _
    And Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        CancelTask = True
        For counter = 0 To SNCNum - 1
            If Check4Text(row, 2, SSNsNoCancel(counter)) Then
                CancelTask = False
                Exit For
            End If
        Next counter
        If CancelTask Then puttext row, 33, "X"
        row = row + 1
        If row = 21 Then
            row = 7
            Hit "F8"
        End If
    Wend
    'post any missed tasks
    Hit "Enter"
    Hit "F6"
End Sub

'This function adds all the valid tasks in the comment array on TD22
Private Sub TD22AddValidTasks(UserID As String)
    Dim counter As Integer
    Dim ErrorCounter As Integer
    Dim WriteError As Boolean
    WriteError = True 'if the no ARC error occures then the default is to write out the error until the ARC is found in the ARCsNeeded array
    'decide if the script is in recovery mode and if it is then it moves the array counter
    'to that point in the array
    Recovery counter
    While counter < NumInAC
        'If the task entry in the CommentArray has been marked as valid then add it on TD22
        If CommentArray(1, counter) = "Y" Then
            FastPath "TX3Z/ATD22" & MID(CommentArray(0, counter), 1, 9)
            If FindingAQueueOnTD22(MID(CommentArray(0, counter), 12, 5)) Then 'if the ARC is found then
                If Check4Text(23, 2, "02845") = False Then
                    'Create ARC entry
                    'mark all loans
                    puttext 11, 3, "X"
                    While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                        If Session.CursorRow <> 21 Then
                            Session.TransmitANSI "X"
                        Else
                            Hit "F8"
                        End If
                    Wend
                    'Enter the Comment and press enter
                    Dim td22Comment As String
                    td22Comment = MID(CommentArray(0, counter), 22)
                    If Len(td22Comment) <= 133 Then
                        puttext 21, 2, td22Comment & " {REQUEBAT}/(" & UserID & ")", "Enter"
                    Else
                        puttext 21, 2, MID(td22Comment, 1, 154), "Enter"
                        Hit "F4"
                        If Len(td22Comment) > 154 Then
                            puttext 8, 5, MID(td22Comment, 155) & " {REQUEBAT}/(" & UserID & ")", "Enter"
                        Else
                            puttext 8, 5, "{REQUEBAT}/(" & UserID & ")", "Enter"
                        End If
                    End If
                End If
            Else
                'track the SSN because its applicable queue task shouldn't be canceled
                ReDim Preserve SSNsNoCancel(SNCNum) 'increase array size by one
                SSNsNoCancel(SNCNum) = MID(CommentArray(0, counter), 1, 9)
                SNCNum = SNCNum + 1
                'make sure there aren't duplicates of the ARC in the ARCsNeeded array
                While ErrorCounter < NumInAN
                    'mid(commentarray(0,counter),12,5) = ARC in the comment array
                    If ARCsNeeded(ErrorCounter) = MID(CommentArray(0, counter), 12, 5) Then
                        WriteError = False 'mark not to write error if the ARC exists in the ARCsneeded array
                    End If
                    ErrorCounter = ErrorCounter + 1
                Wend
                'if the ARC wasn't found in the array then write the ARC to the Array
                If WriteError Then
                    ReDim Preserve ARCsNeeded(NumInAN)
                    ARCsNeeded(NumInAN) = MID(CommentArray(0, counter), 12, 5)
                    NumInAN = NumInAN + 1
                End If
                'reinit variables for next time through the loop
                ErrorCounter = 0
                WriteError = True
            End If
        End If
        'update recovery log
        If SP.Common.TestMode Then
            Open "X:\PADD\Logs\Test\ReQueueLog.txt" For Output As #1
        Else
            Open "X:\PADD\Logs\ReQueueLog.txt" For Output As #1
        End If
        Write #1, "TD22", MID(CommentArray(0, counter), 1, 9)
        Close #1
        counter = counter + 1
    Wend
End Sub

'This function decides whether the script is in recovery mode or not,
'it also moves the array counter to the appropriate place in the array.
Private Sub Recovery(counter As Integer)
    Dim Phase As String
    Dim ssn As String
    Dim logFile As String
    
    'Assign file path accordingly
    If SP.Common.TestMode Then
        logFile = "X:\PADD\Logs\Test\ReQueueLog.txt"
    Else
        logFile = "X:\PADD\Logs\ReQueueLog.txt"
    End If
    'if the recovery log doesn't exist create it
    If Dir(logFile) = "" Then
        Open logFile For Output As #1
        Write #1, "Done", "Done"
        Close #1
    Else
        Open logFile For Input As #1
        Input #1, Phase, ssn
        Close #1
        'decide if the script needs to recover
        If Phase <> "Done" Then
            'Recover if the script needs to
            While MID(CommentArray(0, counter), 1, 9) <> ssn
                counter = counter + 1
            Wend
            'Add one more to the counter so it's pointing to the next array entry to
            'be processed
            counter = counter + 1
        End If
    End If
End Sub

'This function marks all the AddTaskIndicators (see line 2 for CommentArray indices)
'with a "Y" or "N"
Private Sub DecideIfValidTaskToAdd()
    Dim counter As Integer
    While counter < NumInAC
        'split out task by group code in the comment
        'mid(CommentArray(0, counter), 19, 1) = Group Code in the comment string
        Select Case MID(CommentArray(0, counter), 19, 1)
            Case "O" 'if SSN has a loan with bal > 0 then mark as valid task to add
                'mid(CommentArray(0, counter), 1, 9) = SSN in the comment string
                If ITS26Check(MID(CommentArray(0, counter), 1, 9)) Then
                    CommentArray(1, counter) = "Y"
                Else
                    CommentArray(1, counter) = "N"
                End If
            Case "D"
                'mid(CommentArray(0, counter), 1, 9) = SSN in the comment string
                If ITS26Check(MID(CommentArray(0, counter), 1, 9)) Then
                    CommentArray(1, counter) = "Y"
                Else
                    CommentArray(1, counter) = "N"
                End If
            Case "S"
                'mid(CommentArray(0, counter), 1, 9) = SSN in the comment string
                If ITS26Check(MID(CommentArray(0, counter), 1, 9)) Then
                    CommentArray(1, counter) = "Y"
                Else
                    CommentArray(1, counter) = "N"
                End If
            Case "R" 'if SSN has a loan with bal > 0 then mark as valid task to add
                'mid(CommentArray(0, counter), 1, 9) = SSN in the comment string
                If ITS26Check(MID(CommentArray(0, counter), 1, 9)) Then
                    CommentArray(1, counter) = "Y"
                Else
                    CommentArray(1, counter) = "N"
                End If
            Case "X" 'if SSN has a loan with bal > 0 then mark as valid task to add
                'mid(CommentArray(0, counter), 1, 9) = SSN in the comment string
                If ITS26Check(MID(CommentArray(0, counter), 1, 9)) Then
                    CommentArray(1, counter) = "Y"
                Else
                    CommentArray(1, counter) = "N"
                End If
        End Select
        counter = counter + 1
    Wend
End Sub

'This function gets the comment for each task in the ZZZZCOMP queue
Private Sub LP9AGetTasks()
    ReDim CommentArray(1, 0)
    FastPath "LP9ACZZZZCOMP;;ALL;;"
    'Check that there are tasks to process.
    If Check4Text(22, 3, "47450") Then
        ProcComp "MBSREQUEBAT.TXT"
        End
    End If
    'be sure the user is in the correct queue
    If Check4Text(3, 24, "ZZZZCOMP") = False Then
        MsgBox "You have unresolved tasks in another queue.  Please complete them and then run the script."
        End
    End If
    'get comment info for each task
    While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        'create an opening in the comment array
        ReDim Preserve CommentArray(1, NumInAC)
        'record comment in CommentArray
        CommentArray(0, NumInAC) = Replace(GetText(12, 11, 58) & " " & GetText(13, 11, 58) & " " & GetText(14, 11, 58) & " " & GetText(15, 11, 26), "_", "")
        NumInAC = NumInAC + 1
        Hit "F8"
    Wend
End Sub

'This function checks to be sure that the borrower has at least one loan with a principle
'balance > 0.  If it finds one it returns true, false otherwise.
Private Function ITS26Check(ssn As String) As Boolean
    FastPath "TX3Z/ITS26" & ssn
    'check what screen results from the screen change
    If Check4Text(1, 72, "TSX28") Then 'if selection screen
        Dim row As Integer
        row = 8
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            'if a loan is found with a bal > 0 then return true
            If Check4Text(row, 3, " ") = False And Val(GetText(row, 61, 9)) <> 0 Then
                ITS26Check = True
                Exit Function
            End If
            row = row + 1
            If row = 21 Then
                Hit "F8"
                row = 8
            End If
        Wend
        'if a loan with a bal > 0 isn't found then return false
        ITS26Check = False
    ElseIf Check4Text(1, 72, "TSX29") Then 'if individual loan screen
        ITS26Check = (Val(GetText(11, 13, 9)) <> 0)
    End If
End Function

'This Function Searches for a Queue on TD22 if it finds it, the function selects it and returns true, else it returns false.
Private Function FindingAQueueOnTD22(Queue As String) As Boolean
    Dim row As Integer
    row = 8
    FindingAQueueOnTD22 = False
    While (Not Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
        If (Check4Text(row, 7, " " & Queue & " ")) Then
            puttext row, 3, "01", "Enter"
            FindingAQueueOnTD22 = True
            Exit Function
        Else
            If (Check4Text(row, 47, " " & Queue & " ")) Then
                puttext row, 43, "01", "Enter"
                FindingAQueueOnTD22 = True
                Exit Function
            End If
        End If
        row = row + 1
        If (Not row < 23) Then
            Hit "F8"
            row = 8
        End If
    Wend
End Function
