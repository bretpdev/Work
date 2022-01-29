VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} MssAssgnBySSN 
   Caption         =   "Mass Assign By SSN Range"
   ClientHeight    =   5055
   ClientLeft      =   45
   ClientTop       =   345
   ClientWidth     =   5400
   OleObjectBlob   =   "MssAssgnBySSN.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "MssAssgnBySSN"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False



Option Explicit

Private Ranges As New Collection
Private DoItAgain As Boolean

Private Sub InitControls()
    'clear items
    cbQueues.Clear
    If DoItAgain = False Then lbRanges.Clear
    'add items to queue combo box
    cbQueues.AddItem "CCOHORTS"
    cbQueues.AddItem "CHRTSKIP"
    cbQueues.AddItem "FCOHORTS"
    If DoItAgain = False Then
        'put together range collection items and add keys to list box
        lbRanges.ColumnCount = 2
        lbRanges.AddItem "-------", 0
        lbRanges.Column(1, 0) = "Range 1 = 00-09, 63-65, 66-69"
        lbRanges.Column(2, 0) = "00,01,02,03,04,05,06,07,08,09,63,64,65,66,67,68,69"
        lbRanges.AddItem "-------", 1
        lbRanges.Column(1, 1) = "Range 2 = 10-19, 73-75, 76-79"
        lbRanges.Column(2, 1) = "10,11,12,13,14,15,16,17,18,19,73,74,75,76,77,78,79"
        lbRanges.AddItem "-------", 2
        lbRanges.Column(1, 2) = "Range 3 = 20-29, 83-85, 86-89"
        lbRanges.Column(2, 2) = "20,21,22,23,24,25,26,27,28,29,83,84,85,86,87,88,89"
        lbRanges.AddItem "-------", 3
        lbRanges.Column(1, 3) = "Range 4 = 30-39, 70-72, 80-82"
        lbRanges.Column(2, 3) = "30,31,32,33,34,35,36,37,38,39,70,71,72,80,81,82"
        lbRanges.AddItem "-------", 4
        lbRanges.Column(1, 4) = "Range 5 = 40-49, 93-95, 96-99"
        lbRanges.Column(2, 4) = "40,41,42,43,44,45,46,47,48,49,93,94,95,96,97,98,99"
        lbRanges.AddItem "-------", 5
        lbRanges.Column(1, 5) = "Range 6 = 50-59, 60-62, 90-92"
        lbRanges.Column(2, 5) = "50,51,52,53,54,55,56,57,58,59,60,61,62,90,91,92"
    End If
    'enable and disable appropriate controls
    tbUserID.Enabled = False
    lbRanges.Enabled = False
    FrAssign.Enabled = False
    Label1.Enabled = False
    Label3.Enabled = False
    btnAssign.Enabled = False
    cbQueues.Locked = False
    btnProcess.Enabled = False
End Sub

Private Sub btnAssign_Click()
    Dim i As Integer
    Dim Selections As Integer
    Dim UIDMissing As Boolean
    'do data check against user id
    FastPath "LP40I" & tbUserID.Text & ";"
    If Check4Text(1, 70, "USER SEARCH") Then
        MsgBox "The user id you provided can't be found on LP40.  Please check for data entry errors and try again.", vbCritical, "User ID Not Found"
        Exit Sub
    End If
    'make sure that the user hasn't selected more than 3 ranges and not less that 1
    While i < lbRanges.ListCount
        If lbRanges.Selected(i) Then
            Selections = Selections + 1
        End If
        i = i + 1
    Wend
    If Selections > 3 Then
        'if more than 3 were selected then give user error
        MsgBox "You can't select more than three ranges to assign to a specified user.", vbCritical, "Too Many Ranges!"
        Exit Sub
    ElseIf Selections = 0 Then
        'if one wasn't selected then give user error
        MsgBox "You must select at least one range to assign the user.", vbCritical, "Too Few Ranges!"
        Exit Sub
    End If
    i = 0
    'add user id for all selected items in list box
    While i < lbRanges.ListCount
        If lbRanges.Selected(i) Then
            lbRanges.Column(0, i) = UCase(tbUserID.Text)
            lbRanges.Selected(i) = False
        End If
        i = i + 1
    Wend
    btnProcess.Enabled = True
    cbQueues.Locked = True
    tbUserID.Text = ""
End Sub

Private Sub btnCancel_Click()
    End
End Sub

'checks if all ranges are assigned
Function AllRangesAreAssigned() As Boolean
    Dim i As Integer
    For i = 0 To lbRanges.ListCount - 1
        If lbRanges.Column(0, i) = "-------" Then
            AllRangesAreAssigned = False
            Exit Function
        End If
    Next
    AllRangesAreAssigned = True
End Function

'creates a list of distinct users in list box
Function CreateUserList() As String()
    Dim i As Integer
    Dim Temp() As String
    ReDim Temp(0)
    For i = 0 To lbRanges.ListCount - 1
        'check if user is provided and be sure that the user id isn't already in the list
        If lbRanges.Column(0, i) <> "-------" And InStr(1, Join(Temp), lbRanges.Column(0, i)) = False Then
            Temp(UBound(Temp)) = lbRanges.Column(0, i)
            ReDim Preserve Temp(UBound(Temp) + 1)
        End If
    Next
    CreateUserList = Temp
End Function

Private Sub btnProcess_Click()
    Dim row As Integer
    Dim LastPageChangesWereMadeOn As String
    Dim UserIDList() As String
    Dim UserIDLstCntr As Integer
    'create list of individual distinct user ids if one of the SSN ranges aren't assigned to a user
    If AllRangesAreAssigned() = False Then
        UserIDList = CreateUserList()
    End If
    'process assigments
    FastPath "LP8YCPRE;" & cbQueues.Value
    'check if there are any tasks in the queue
    If Check4Text(1, 60, "QUEUE STATS SELECTION") Then
        If MsgBox("There doesn't appear to be any tasks in that queue.  Would you like to process another queue.", vbInformation, "No Tasks In Selected Queue") = vbYes Then
            DoItAgain = True
            InitControls
            Exit Sub
        Else
            End
        End If
    End If
    PutText 21, 13, "01", "Enter"
    row = 7
    While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        While row < 21
            If Check4Text(row, 33, "A") Then
                AssignTsk row, UserIDList, UserIDLstCntr
                LastPageChangesWereMadeOn = GetText(2, 71, 3)
            End If
            row = row + 1
        Wend
        row = 7
        Hit "F8"
    Wend
    If LastPageChangesWereMadeOn = 0 Then
        If MsgBox("There were no tasks to assign.  Would you like to process another queue?", vbYesNo, "Another Queue?") = vbYes Then
            DoItAgain = True
            InitControls
            Exit Sub
        Else
            End
        End If
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
        If Check4Text(22, 3, "49000 DATA SUCCESSFULLY UPDATED") = False Then
            MsgBox "There was a system problem while trying to update the queue assignments.  Please correct the problem and post the changes manually.", vbInformation, "System Problem"
            End
        End If
        If MsgBox("Would you like to process another queue?", vbYesNo, "Another Queue?") = vbYes Then
            DoItAgain = True
            InitControls
            Exit Sub
        Else
            End
        End If
    End If
End Sub

'assign task
Private Sub AssignTsk(r As Integer, UserIDList() As String, UserIDLstCntr As Integer)
    Dim RangeCounter As Integer
    Dim ListBoxCounter As Integer
    Dim RangeItem() As String
    While ListBoxCounter < lbRanges.ListCount
        RangeItem = Split(lbRanges.Column(2, ListBoxCounter), ",")
        While RangeCounter < (UBound(RangeItem) + 1)
            If Check4Text(r, 9, RangeItem(RangeCounter)) Then
                If lbRanges.Column(0, ListBoxCounter) = "-------" Then
                    PutText r, 38, UserIDList(UserIDLstCntr) 'put in next user id from list of given user ids if range was left unassigned
                    UserIDLstCntr = UserIDLstCntr + 1 'increment up by one for next user in list
                    If UserIDLstCntr = UBound(UserIDList) Then UserIDLstCntr = 0 'if at the bottom of the list then move back to the top
                Else
                    PutText r, 38, lbRanges.Column(0, ListBoxCounter) 'put in user id if assigned
                End If
                Exit Sub
            End If
            RangeCounter = RangeCounter + 1
        Wend
        RangeCounter = 0
        ListBoxCounter = ListBoxCounter + 1
    Wend
    '**********IMPORTANT*******************
    'If the script ever exits through this code that means a range isn't coded for and there is an issue
    MsgBox "A possible number appears to be missing from a range.  The script can't recover from this.  Please contact Systems Support.", vbCritical, "Error"
    End
End Sub

Private Sub cbQueues_Change()
    If cbQueues.Value <> "" Then
        tbUserID.Enabled = True
        lbRanges.Enabled = True
        FrAssign.Enabled = True
        Label1.Enabled = True
        Label3.Enabled = True
        btnAssign.Enabled = True
        If DoItAgain Then btnProcess.Enabled = True 'enable process button if recycling through for another queue
    Else
        tbUserID.Enabled = False
        lbRanges.Enabled = False
        FrAssign.Enabled = False
        Label1.Enabled = False
        Label3.Enabled = False
        btnAssign.Enabled = False
        btnProcess.Enabled = False
    End If
End Sub

Private Sub UserForm_Initialize()
    DoItAgain = False
    InitControls
End Sub
