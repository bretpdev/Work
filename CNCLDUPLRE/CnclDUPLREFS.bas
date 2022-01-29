Attribute VB_Name = "CnclDUPLREFS"
Private ftpFolder As String

Public Sub CnclDUPLREFS()
    Common.ResetPublicVars

    'warn the user of the purpose of the script and end the script if the dialog box is cancelled
    If Not SP.Common.CalledByMBS Then
        If MsgBox("This script cancels all tasks in the DUPLREFS queue.  Click OK to continue or Cancel to quit.", vbOKCancel, "Cancel Duplicate References") <> vbOK Then End
    End If
    
    SP.Common.TestMode ftpFolder
    'get user id
    Common.LP40
    'warn the user and end the script is the user is no UT00205
    If UserID <> "UT00205" And Not SP.Common.CalledByMBS Then
        MsgBox "This script must be run using user ID UT00205.  Please log in using UT00205 and run the script again.", , "Incorrect User ID"
        End
    End If
    'get the SAS file
    Common.SASProcessing ftpFolder & "ULWD09.LWD09R2.*.*", "MBSCNCLDUPLRE.txt", ftpFolder & "ULWD09.LWD09R1.*.*"
    SasFile = SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWD09.LWD09R2.*.*")
    'check for empty file
    If FileLen(ftpFolder & SasFile) = 0 Then
        MsgBox "There are no loans to process today.", vbInformation
        Kill ftpFolder & SasFile
        ProcComp "MBSCNCLDUPLRE.txt"
    End If
    'go to duplrefs control to process file
    DUPCntl
    Kill ftpFolder & SasFile
    ProcComp "MBSCNCLDUPLRE.txt"
End Sub

'process file
Private Sub DUPCntl()
    'read SSNs in file to an array
    Dim socials() As String
    ReDim socials(0)
    Open ftpFolder & SasFile For Input As #1
    Do While Not EOF(1)
        'input the record
        Input #1, ssn
        'expand the array
        ReDim Preserve socials(UBound(socials) + 1) As String
        'add the SSN
        socials(UBound(socials)) = ssn
    Loop
    Close #1
    
    AssignTasks socials
    CompleteTasks
End Sub

'assign the tasks to user
Private Sub AssignTasks(ByRef socials() As String)
    Dim Row As Integer
    
    'access LP8Y
    FastPath "LP8YCSKP;DUPLREFS"
    'warn the user and end the script if the queue is not found (no tasks)
    If Not Check4Text(8, 11, "DUPLREFS") Then
        MsgBox "No tasks were found in the DUPLREFS queue.  Contact Systems Support for assistance.", 48, "No Tasks"
        End
    End If
    'access the queue
    Session.TransmitANSI "01"
    Hit "ENTER"
    'review each task to determine if it should be processed
    Row = 7
    Do While Not Check4Text(22, 3, "46004")
        'if the SSN for the tasks is in the array, assign the task to the user
        For i = 1 To UBound(socials)
            If Check4Text(Row, 2, socials(i)) And (Check4Text(Row, 33, "A") Or Check4Text(Row, 33, "W")) Then
                puttext Row, 38, UserID
                Exit For
            End If
        Next i
        Row = Row + 1
        'go to the next page
        If Check4Text(Row, 2, " ") Then
            Hit "F8"
            Row = 7
        End If
    Loop
    'there is a bug in OneLINK which thinks there are no updates if no changes were made on the current page
    'this loop posts the changes and then goes back a page until the updates are made or the first page is reached
    Do
        Hit "F6"
        If Check4Text(22, 3, "49000") Then Exit Do
        Hit "F7"
        If Check4Text(22, 3, "46003") Then Exit Do
    Loop
End Sub

'complete the tasks
Private Sub CompleteTasks()
    'access LP9A for assigned DUPLREFS tasks
    FastPath "LP9ACDUPLREFS;;SKP;Y;;"
    'warn the user and end the script if the task displayed is not from the DUPLREFS queue
    If Not Check4Text(1, 9, "DUPLREFS") Then
        MsgBox "You have an unresolved task in the " & GetText(1, 9, 8) & " queue.  Complete the task and then run the script again.", , "Unresolved Task"
        End
    End If
    'complete each task
    Do While (Not Check4Text(22, 3, "47423")) And (Not Check4Text(22, 3, "47420")) And (Not Check4Text(22, 3, "47450"))
        'add an activity record
        Wait "2"
        Hit "F2"
        Hit "F10"
        Session.TransmitANSI "KGNRL"
        Hit "ENTER"
        Session.TransmitANSI "AM36"
        puttext 13, 2, "duplicate reference review completed"
        'post the comment
        Hit "F6"
        Hit "F12"
        Hit "F2"
        'complete the task
        Hit "F6"
        If (Not Check4Text(22, 3, "49000 DATA SUCCESSFULLY UPDATED")) Then
            MsgBox "The script was unable to complete the task. Please complete it manually and re-run the script.", vbOKOnly, "Cancel DUPLREFS"
            End
        End If
        'go to the next task
        Hit "F8"
    Loop
End Sub
