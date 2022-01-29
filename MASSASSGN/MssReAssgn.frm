VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} MssReAssgn 
   Caption         =   "Mass Re-Assign By SSN Range"
   ClientHeight    =   4845
   ClientLeft      =   45
   ClientTop       =   345
   ClientWidth     =   5400
   OleObjectBlob   =   "MssReAssgn.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "MssReAssgn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Option Explicit

Private Sub btnAdd_Click()
    Dim Selections As Integer
    Dim UIDMissing As Boolean
    'do data check against user id
    FastPath "LP40I" & tbUserID.Text & ";"
    If Check4Text(1, 70, "USER SEARCH") Then
        MsgBox "The user id you provided can't be found on LP40.  Please check for data entry errors and try again.", vbCritical, "User ID Not Found"
        Exit Sub
    End If
    lbNewUserIDs.AddItem UCase(tbUserID.Text), lbNewUserIDs.ListCount
    tbUserID.Text = ""
End Sub

Private Sub btnCancel_Click()
    End
End Sub

Private Function ContinueBasedOnCountChk(NR As Integer) As Boolean
    If cbNumOfTsks.Value = "ALL" Then
        ContinueBasedOnCountChk = True
    Else
        If (CInt(cbNumOfTsks.Value) > NR) Then
            ContinueBasedOnCountChk = True
        Else
            ContinueBasedOnCountChk = False
        End If
    End If
    
    
End Function

Private Sub btnContinue_Click()
    Dim row As Integer
    Dim DeptRow As Integer
    Dim LastPageChangesWereMadeOn As String
    Dim UserIDLstCntr As Integer
    Dim NumReassgnd As Integer
    Dim QueuesToProc() As String
    Dim QueueCntr As Integer
    'data checks
    If tbOldUserID.TextLength <> 7 Then
        MsgBox "You must provide a 7 character ""UT"" user id to unassign from tasks.", vbCritical, "Invalid User ID"
        Exit Sub
    End If
    If lbNewUserIDs.ListCount = 0 Then
        MsgBox "You must provide at least one user id to reassign the tasks to.", vbCritical, "New User ID Needed"
        Exit Sub
    End If
    If cbNumOfTsks.Value = "" Then
        MsgBox "Please provide the number of tasks you want to reassign.", vbCritical, "Number of Tasks to Reassign Needed"
        Exit Sub
    End If
    'process assigments
    'check if queue was provided
    If tbQueues.TextLength <> 0 Then
        'queue was provided
        FastPath "LP8YCPRE;" & tbQueues.Text & ";" & tbOldUserID.Text
        'check if there are any tasks in the queue
        If Check4Text(1, 60, "QUEUE STATS SELECTION") Then
            MsgBox "There doesn't appear to be any tasks in that queue for that user id or it doesn't exist.", vbInformation, "No Tasks In Selected Queue"
            End
        End If
        PutText 21, 13, "01", "Enter"
        row = 7
        While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False And ContinueBasedOnCountChk(NumReassgnd)
            If Check4Text(row, 33, "A") Then
                If CDate(GetText(row, 26, 2) & "/" & GetText(row, 28, 2) & "/" & GetText(row, 22, 4)) < Date Then
                    PutText row, 38, lbNewUserIDs.Column(0, UserIDLstCntr) 'put in next user id
                    UserIDLstCntr = UserIDLstCntr + 1 'increment up by one for next user in list
                    If UserIDLstCntr = lbNewUserIDs.ListCount Then UserIDLstCntr = 0 'if at the bottom of the list then move back to the top
                    LastPageChangesWereMadeOn = GetText(2, 71, 3)
                    NumReassgnd = NumReassgnd + 1
                End If
            End If
            row = row + 1
            If row = 21 Then
                row = 7
                Hit "F8"
            End If
        Wend
        If LastPageChangesWereMadeOn = 0 Then
            MsgBox "There were no tasks to assign.", vbInformation, "No Tasks"
            End
        Else
            'return to last page that changes were made on
            PutText 2, 71, "", "End"
            If Len(LastPageChangesWereMadeOn) = 1 Then
                PutText 2, 73, LastPageChangesWereMadeOn, "Enter"
            ElseIf Len(LastPageChangesWereMadeOn) = 2 Then
                PutText 2, 72, LastPageChangesWereMadeOn, "Enter"
            ElseIf Len(LastPageChangesWereMadeOn) = 3 Then
                PutText 2, 71, LastPageChangesWereMadeOn, "Enter"
            End If
            Hit "F6" 'post changes
            If Check4Text(22, 3, "49000 DATA SUCCESSFULLY UPDATED") = False And Check4Text(22, 3, "47450 NO CURRENT TASKS FOUND") = False Then
                MsgBox "There was a system problem while trying to update the queue assignments.  Please correct the problem and post the changes manually.", vbInformation, "System Problem"
                End
            Else
                MsgBox "Processing Complete!", vbInformation, "Processing Complete!"
                End
            End If
        End If
    Else
        ReDim QueuesToProc(0)
        'no queue was provided so all tasks for the user in the PRE department will be reassigned
        FastPath "LP8YCPRE;;" & tbOldUserID.Text
        'check if there are any tasks in the queues
        If Check4Text(1, 60, "QUEUE STATS SELECTION") Then
            MsgBox "There doesn't appear to be any tasks in the ""PRE"" department queues for that user ID.", vbInformation, "No Tasks In ""PRE"" Department Queues"
            End
        End If
        DeptRow = 8
        'get all queues listed
        While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            QueuesToProc(UBound(QueuesToProc)) = GetText(DeptRow, 11, 10)
            ReDim Preserve QueuesToProc(UBound(QueuesToProc) + 1)
            DeptRow = DeptRow + 1
            'check if the next row has a number to select
            If IsNumeric(GetText(DeptRow, 6, 3)) = False Then
                'if the next row is not numeric then page forward to see if there are more queues to process
                DeptRow = 8
                Hit "F8"
            End If
        Wend
        'process while there are queues to process and the task reassignment count has been met
        While QueueCntr < UBound(QueuesToProc) And ContinueBasedOnCountChk(NumReassgnd)
            FastPath "LP8YCPRE;" & QueuesToProc(QueueCntr) & ";" & tbOldUserID.Text
            PutText 21, 13, "01", "Enter" 'select queue
            row = 7
            While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False And ContinueBasedOnCountChk(NumReassgnd)
                If Check4Text(row, 33, "A") Then
                    If CDate(GetText(row, 26, 2) & "/" & GetText(row, 28, 2) & "/" & GetText(row, 22, 4)) < Date Then
                        PutText row, 38, lbNewUserIDs.Column(0, UserIDLstCntr) 'put in next user id
                        UserIDLstCntr = UserIDLstCntr + 1 'increment up by one for next user in list
                        If UserIDLstCntr = lbNewUserIDs.ListCount Then UserIDLstCntr = 0 'if at the bottom of the list then move back to the top
                        LastPageChangesWereMadeOn = GetText(2, 71, 3)
                        NumReassgnd = NumReassgnd + 1
                    End If
                End If
                row = row + 1
                If row = 21 Then
                    row = 7
                    Hit "F8"
                End If
            Wend
            If LastPageChangesWereMadeOn = 0 Then
                MsgBox "There were no tasks to assign.", vbInformation, "No Tasks"
                End
            Else
                'return to last page that changes were made on
                PutText 2, 71, "", "End"
                If Len(LastPageChangesWereMadeOn) = 1 Then
                    PutText 2, 73, LastPageChangesWereMadeOn, "Enter"
                ElseIf Len(LastPageChangesWereMadeOn) = 2 Then
                    PutText 2, 72, LastPageChangesWereMadeOn, "Enter"
                ElseIf Len(LastPageChangesWereMadeOn) = 3 Then
                    PutText 2, 71, LastPageChangesWereMadeOn, "Enter"
                End If
                Hit "F6" 'post changes
                If Check4Text(22, 3, "49000 DATA SUCCESSFULLY UPDATED") = False And Check4Text(22, 3, "47450 NO CURRENT TASKS FOUND") = False Then
                    MsgBox "There was a system problem while trying to update the queue assignments.  Please correct the problem and post the changes manually.", vbInformation, "System Problem"
                    End
                End If
            End If
            QueueCntr = QueueCntr + 1
        Wend
        MsgBox "Processing Complete!", vbInformation, "Processing Complete!"
        End
    End If
End Sub

Private Sub UserForm_Initialize()
    cbNumOfTsks.AddItem "10", 0
    cbNumOfTsks.AddItem "20", 1
    cbNumOfTsks.AddItem "30", 2
    cbNumOfTsks.AddItem "ALL", 3
End Sub
