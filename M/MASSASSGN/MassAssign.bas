Attribute VB_Name = "MassAssign"
Private Dept As String
Private Queue As String
Private UserIDWrk As String     'working variable of string of user IDs
Private pUserIDWrk As String    'working variable of string of user IDs entered previously
Private UserId() As String
Private Seld As Integer         'indicator of currently assigned/future-dated tasks selections
Private DateLstWrk As Double

Private CommaX As Integer       'position of comma in Users string
Private i As Integer            'for loop counter
Private j As Integer            'tasks assigned counter
Private x As Integer            'number of user IDs
Private Y As Integer            'number of tasks to be assigned
Private z As Integer            'number of tasks to assign to each user
Private row As Integer          'LP8Y row being processed
Private OpFrm As MssAssgnOptions
Private BySSNFrm As MssAssgnBySSN
Private Reassign As MssReAssgn

'assign tasks according to user specifications
Sub MassAssignMain()
    'check for permission to run based off BSYS values
    If UBound(SP.SQLEx("SELECT WinUName, TypeKey FROM dbo.GENR_REF_AuthAccess WHERE WinUName = '" & Environ("USERNAME") & "' and TypeKey = 'Mass Assign'"), 2) = 0 Then
        MsgBox "You are not authorized to run this script.", vbCritical, "Unauthorized"
        End
    End If
    With Session
        'warn the user of the purpose of the script and end if the dialog box is canceled
        warn = MsgBox("This script assigns groups of tasks to specified users IDs.  You must access LP8Y in change mode for the queue which contains the tasks you want to assign before running the script.  Click OK to continue or click Cancel to quit.", vbOKCancel, "Mass Assign Tasks")
        If warn <> 1 Then End
'<1->   ***Shared is added below for testing purposes***
        If InStr(1, Session.CommandLineSwitches, "LoanManagement") Or InStr(1, Session.CommandLineSwitches, "Shared") Then
            Set OpFrm = New MssAssgnOptions
            OpFrm.Show
            If OpFrm.UserOption() = MassAssignBySSN Then
                'do by ssn functionality
                Set BySSNFrm = New MssAssgnBySSN
                BySSNFrm.Show
                End
            ElseIf OpFrm.UserOption() = MassAssignReassign Then
                'reassign
                'do reassign functionality
                Set Reassign = New MssReAssgn
                Reassign.Show
                End
            End If
        End If
'</1>
        'warn the user and end the script if a queue is not displayed on LP8Y in change mode
        If .GetDisplayText(1, 2, 6) <> "LP8Y C" Or .GetDisplayText(1, 75, 3) <> "DET" Then
            MsgBox "You must access the LP8Y QUEUE TASK DETAIL screen in change mode for the queue which contains the tasks you want to assign before running this script.  Select the queue and run the script again.", , "Queue not Selected in Change Mode"
            End
        End If
        UserIDWrk = ""
        AssignProc
    End With
End Sub

'process the assignments
Sub AssignProc()
    With Session
    'get the queue department and name
        Dept = .GetDisplayText(1, 9, 4)
        Queue = .GetDisplayText(1, 13, 8)
        'strip the ; off the end of queue names that are less than 8 characters
        Do
            If Mid(Queue, Len(Queue), 1) = ";" Then
                Queue = Mid(Queue, 1, Len(Queue) - 1)
            Else
                Exit Do
            End If
        Loop
    'get assignment information
        'load the user form and assign the queue name
        Load MssAssgn
        MssAssgn.Que = Queue
        Do
            'prompt the user for the information
            MssAssgn.Show
            'warn the user to reenter the number of tasks to assign if it is not "ALL" or numeric
            If IsNumeric(MssAssgn.No) = False And MssAssgn.No <> "ALL" Then
                MsgBox "You must enter 'ALL' or a numeric value for the number of tasks to assign.  Click OK to reenter the number of tasks to assign.", , "Invalid Data"
            ElseIf pUserIDWrk <> UCase(MssAssgn.Users) Then
                'get the list of user IDs entered by the user (IDs are separated by commas)
                UserIDWrk = UCase(MssAssgn.Users)
                pUserIDWrk = UCase(MssAssgn.Users)
                'assign each user ID to the UserID array while the working variable has commas (indicating more user IDs are contained therein)
                x = 0
                Do
                    'determine the position of the next comma
                    CommaX = InStr(1, UserIDWrk, ",")
                    If CommaX = 0 Then Exit Do
                    'parse out the user ID and assign it to the variable
                    x = x + 1
                    ReDim Preserve UserId(1 To x) As String
                    UserId(x) = Trim(Mid(UserIDWrk, 1, CommaX - 1))
                    UserIDWrk = Mid(UserIDWrk, CommaX + 1, Len(UserIDWrk))
                Loop
                'assign the last or only user ID to the array
                x = x + 1
                ReDim Preserve UserId(1 To x) As String
                UserId(x) = Trim(UserIDWrk)
                'verify that each user ID is valid (found on LP40)
                For i = 1 To x
                    .TransmitTerminalKey rcIBMClearKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    .TransmitANSI "LP40I" & UserId(i)
                    .TransmitTerminalKey rcIBMEnterKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    'warn the user if the user ID is not valid
                    If .GetDisplayText(3, 2, 7) <> "USER ID" Then
                        MsgBox "User ID " & UserId(i) & " was not found.  Click OK and correct the user ID information entered in the User IDs field.", , "User ID not Found"
                        Exit For
                    End If
                    'continue processing if all of the user IDs are valid
                    If i = x Then Exit Do
                Next i
            Else
                Exit Do
            End If
        Loop
        'set the selected indicator to determine the criteria to use to determine tasks to be assigned
        If MssAssgn.Current = True And MssAssgn.Future = True Then
            Seld = 1
        ElseIf MssAssgn.Current = True Then
            Seld = 2
        ElseIf MssAssgn.Future = True Then
            Seld = 3
        Else
            Seld = 0
        End If
        'access LP8Y for the queue and display only available tasks
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP8YC" & Dept & Queue & ";;;;A;"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'warn the user and end the script if there are no available tasks
        If .GetDisplayText(1, 75, 3) <> "DET" Then
            warn = MsgBox("There are no available tasks in the queue you selected.  Do you want to select the next queue?  Click Yes to select the next queue and assign the tasks in that queue to the users you've already entered or click No to end the script.", vbYesNo, "No Available Tasks in Queue")
            If warn = 6 Then
                GetNextQ
            Else
                Unload MssAssgn
                End
            End If
        End If
    'determine the number of tasks to assign to each userID
        'get the number of tasks to assign
        Y = 0
        row = 7
        Do
            'get the date last worked (set to today if blank)
            If .GetDisplayText(row, 22, 8) = "        " Then
                DateLstWrk = DateValue(Date)
            Else
                DateLstWrk = DateValue(Format(.GetDisplayText(row, 26, 4) & .GetDisplayText(row, 22, 4), "##/##/####"))
            End If
            'determine if task should be reassigned
            Select Case Seld
            Case 1  'select all (currently assigned and future dated)
                Y = Y + 1
            Case 2  'select currently assigned but not future dated
                If DateLstWrk <= Date Then Y = Y + 1
            Case 3  'select future dated but not currently assigned
                If .GetDisplayText(row, 38, 2) <> "UT" Then Y = Y + 1
            Case 0  'do not select currently assigned or future dated
                If DateLstWrk <= Date And .GetDisplayText(row, 38, 2) <> "UT" Then Y = Y + 1
            End Select
            row = row + 1
            'go to the next page if there are no more tasks
            If .GetDisplayText(row, 33, 1) = " " Then
                .TransmitTerminalKey rcIBMPf8Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                If .GetDisplayText(22, 3, 5) = "46004" Then Exit Do
                row = 7
            End If
        Loop
        'warn the user and end the script if there are no assignable tasks
        If Y = 0 Then
            warn = MsgBox("There are no available tasks that meet the currently assigned/future dated criteria you selected.  Do you want to select the next queue?  Click Yes to select the next queue and assign the tasks in that queue to the users you've already entered based on the criteria you selected.  Click No to modify the selection of tasks to be assigned.  Click Cancel to end the script.", vbYesNoCancel, "No Tasks to Assign")
            If warn = 6 Then
                GetNextQ
            ElseIf warn = 7 Then
                AssignProc
            Else
                Unload MssAssgn
                End
            End If
        'warn the user and end the script if more user IDs were entered than there are assignable tasks
        ElseIf Y < x Then
            MsgBox "There are only " & Y & " available tasks in the queue that meet the currently assigned/future dated criteria you selected but you have entered " & x & " user IDs.  Click OK to reenter the users to which tasks should be assigned or modify the currently assigned/future dated criteria you selected.", , "Too Many Users"
            AssignProc
        'divide the assignable tasks by the number of user IDs if all tasks should be assigned or if the number of tasks to assign to each user ID times the number of user IDs exceeds the number of assignable tasks
        ElseIf MssAssgn.No = "ALL" Or Y < x * Val(MssAssgn.No) Then
            z = Int(Y / x)
        'otherwise, the number of tasks to assign to each user ID (z) = the number entered by the user
        Else
            z = Val(MssAssgn.No)
        End If
        'return to the first page
        .MoveCursor 2, 71
        .TransmitANSI "001"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
    'assign the tasks
        row = 7
        'assign z number of tasks to user IDs 1 to x
        For i = 1 To x
            j = 0
            Do Until j = z
                AssignTasks i
                If .GetDisplayText(22, 3, 5) = "46004" Then Exit For
            Loop
        Next i
        'if there are any tasks left to be assigned, assign them to the last user ID
        If MssAssgn.No = "ALL" Or Y < x * Val(MssAssgn.No) And Y > x Then
            Do Until .GetDisplayText(22, 3, 5) = "46004"
                AssignTasks x
            Loop
        End If
    'post the changes
        'there is a bug in OneLINK which thinks there are no updates if no changes were made on the current page
        'this loop posts the changes and then goes back a page until the updates are made or the first page is reached
        Do
            .TransmitTerminalKey rcIBMPf6Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            If .GetDisplayText(22, 3, 5) = "49000" Then Exit Do
            .TransmitTerminalKey rcIBMPf7Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            If .GetDisplayText(22, 3, 5) = "46003" Then Exit Do
        Loop
    'prompt the user to go to the next queueu
        warn = MsgBox("Do you want to assign tasks in the next queue?  Click Yes to continue or No to quit.", vbYesNo, "Assign More Tasks")
        If warn <> 6 Then
            Unload MssAssgn
            End
        Else
            GetNextQ
        End If
    End With
End Sub

'assign tasks
Function AssignTasks(u As Integer)
    With Session
        'get the date last worked (set to today if blank)
        If .GetDisplayText(row, 22, 8) = "        " Then
            DateLstWrk = DateValue(Date)
        Else
            DateLstWrk = DateValue(Format(.GetDisplayText(row, 26, 4) & .GetDisplayText(row, 22, 4), "##/##/####"))
        End If
        'determine if task should be reassigned
        Select Case Seld
        Case 1  'select all (currently assigned and future dated)
            .MoveCursor row, 38
            .TransmitANSI UserId(u)
            j = j + 1
        Case 2  'select currently assigned but not future dated
            If DateLstWrk <= Date Then
                .MoveCursor row, 38
                .TransmitANSI UserId(u)
                j = j + 1
            End If
        Case 3  'select future dated but not currently assigned
            If .GetDisplayText(row, 38, 2) <> "UT" Then
                .MoveCursor row, 38
                .TransmitANSI UserId(u)
                j = j + 1
            End If
        Case 0  'do not select currently assigned or future dated
            If DateLstWrk <= Date And .GetDisplayText(row, 38, 2) <> "UT" Then
                .MoveCursor row, 38
                .TransmitANSI UserId(u)
                j = j + 1
            End If
        End Select
        'verify the user ID can be assigned to the queue
        If j = 1 Then
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'warn the user and end the script if the user ID cannot be assigned to the queue
            If .GetDisplayText(22, 3, 5) <> "46012" Then
                MsgBox "User ID " & UserId(u) & " cannot be assigned to tasks in this queue.  Click OK to reenter the users to which tasks should be assigned.  All current changes will be lost.", , "Invalid User ID"
                AssignProc
            End If
        End If
        row = row + 1
        If .GetDisplayText(row, 33, 1) = " " Then
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            If .GetDisplayText(22, 3, 5) = "46004" Then Exit Function
            row = 7
        End If
    End With
End Function

'get the next queue to process
Sub GetNextQ()
    With Session
        'access LP8Y
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP8YC" & Dept & ";"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'find the previous queue processed
        Do
            found = .FindText(Queue, 8, 11)
            If found = True Then Exit Do
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'prompt the user to select the queue manually or end the script if the previous queue is not found
            If .GetDisplayText(22, 3, 5) = "46004" Then
                warn = MsgBox("Click OK to pause the script, select the correct queue, and then hit <Insert> to continue or click Cancel to quit.", vbOKCancel, "Select Queue")
                If warn <> 1 Then
                    Unload MssAssgn
                    End
                Else
                    SelNextQ
                End If
            End If
        Loop
        'go to the next page if the previous queue is the last on the page
        If .FoundTextRow = 19 Then
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            row = 8
        Else
            row = .FoundTextRow + 1
        End If
        'find the next queue with more than 0 outstanding tasks
        Do Until CDbl(.GetDisplayText(row, 68, 6)) > 0
            row = row + 1
            If .FoundTextRow = 19 Then
                .TransmitTerminalKey rcIBMPf8Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                'prompt the user to select the queue manually or end the script if another queue is not found with more than 0 outstanding tasks
                If .GetDisplayText(22, 3, 5) = "46004" Then
                    warn = MsgBox("Click OK to pause the script, select the correct queue, and then hit <Insert> to continue or click Cancel to quit.", vbOKCancel, "Select Queue")
                    If warn <> 1 Then
                        Unload MssAssgn
                        End
                    Else
                        SelNextQ
                    End If
                End If
                row = 8
            End If
        Loop
        'select the queue
        .TransmitANSI .GetDisplayText(row, 6, 2)
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'prompt the user to verify that the correct queue is displayed
        warn = MsgBox("Is this the correct queue?  Click Yes to assign tasks in the queue.  Click No to pause the script, select the correct queue, and then hit <Insert> to continue.  Click Cancel to quit.", vbYesNoCancel, "Verify Queue Selection")
        'assign tasks if the user clicked yes
        If warn = 6 Then
            AssignProc
        'allow the user to select the queue manually if the user clicked no
        ElseIf warn = 7 Then
            SelNextQ
        'end the script if the user clicked cancel
        Else
            Unload MssAssgn
            End
        End If
    End With
End Sub

'manually select the next queue to process
Sub SelNextQ()
    With Session
        Do
            'pause for the user to select the queue
            .WaitForTerminalKey rcIBMInsertKey, "1:0:0"
            .TransmitTerminalKey rcIBMInsertKey
            'warn the user if a queue is not displayed on LP8Y in change mode
            If .GetDisplayText(1, 2, 6) <> "LP8Y C" Or .GetDisplayText(1, 75, 3) <> "DET" Then
                warn = MsgBox("You must access the LP8Y QUEUE TASK DETAIL screen in change mode for the queue which contains the tasks you want to assign.  Click Retry to pause the script, select the queue and hit <Insert> to continue or click cancel to quit.", vbRetryCancel, "Queue not Selected")
                'end the script if the dialog box was cancelled
                If warn = 2 Then
                    Unload MssAssgn
                    End
                End If
            'exit the loop and assign tasks if a queue was displayed
            Else
                Exit Do
            End If
        Loop
        AssignProc
    End With
End Sub

'new sr 556, jd, 03/04/04, 03/29/04
'<1> SR 2109, aa


