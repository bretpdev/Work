VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmCancelQueueTasks 
   Caption         =   "Cancel Queue Tasks/Error Out Activity Comments"
   ClientHeight    =   4590
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6810
   OleObjectBlob   =   "frmCancelQueueTasks.frx":0000
   StartUpPosition =   2  'CenterScreen
End
Attribute VB_Name = "frmCancelQueueTasks"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Private Sub Cancel_Click()
    End
End Sub


Private Sub OK_Click()
    If rbBoth.Value Then
        'If the data is valid then send it to the main subroutine
        If Not BothValidated() Then
            Exit Sub
        Else
            CancelQTksANDErrActvCom.Main tbQueue.Text, tbSubQueue.Text, tbQueueErrormsg.Text, _
            tbARC.Text, tbFrom.Text, tbTo.Text
        End If
    Else
        'If the data is valid then send it to the main subroutine
        If Not CancelValidated() Then
            Exit Sub
        Else
            CancelQTksANDErrActvCom.Main tbQueue.Text, tbSubQueue.Text, tbQueueErrormsg.Text
        End If
    End If
End Sub

'validates user input for all fields by calling cancelvalidated() and doing it's own validation.
'This function is called if the user wants to error out activity comments too
Private Function BothValidated() As Boolean
    BothValidated = True
    If (Not CancelValidated()) Then
        BothValidated = False
        Exit Function
    End If
    If (Not DataValidated() Or tbARC.TextLength < 1) Then
        MsgBox "A ARC and a valid From and To date are required", vbInformation, "ARC OR DATE ERROR"
        BothValidated = False
        Exit Function
    End If
    If (tbQueueErrormsg.TextLength > 0) Then
        BothValidated = False
        MsgBox "You can't select 'Cancel Queue Tasks and Error Activity Comments' and specify an 'Error Message' to look for at the same time.  When you select 'Cancel Queue Tasks and Error Activity Comments' you must intend to cancel all queue tasks and error out all activity comments.", vbInformation, "ERROR"
        Exit Function
    End If
End Function

'Validates all the data given by the user for canceling queue tasks
Private Function CancelValidated() As Boolean
    CancelValidated = True
    If (tbQueue.TextLength <> 2 Or tbSubQueue.TextLength <> 2) Then
        CancelValidated = False
        MsgBox "A two character Queue, a two character SubQueue and an Error message is required.", vbInformation, "VALIDATION ERROR"
    End If
    If (tbQueueErrormsg.TextLength < 1 And tbARC.Text = "") Then
        If (vbNo = MsgBox("Are you sure you want to leave the Error Message blank.  When the Error Message is left blank the script will cancel all tasks in that queue.  Click 'Yes' to continue or 'No' to enter a Error Message.", vbYesNo, "WARNING: ALL QUEUE TASKS WILL BE DELETED")) Then CancelValidated = False
    Else
        tbQueueErrormsg.Text = UCase(tbQueueErrormsg.Text)
    End If
End Function

Private Sub rbBoth_Click()
    EnableANDDisable
End Sub

Private Sub rbCancelOnly_Click()
    EnableANDDisable
End Sub

'Enable or disable the needed textboxes and labels depending on the radio button choice
Private Function EnableANDDisable()
    If (rbBoth.Value) Then
        Label4.Enabled = True
        Label7.Enabled = True
        Label5.Enabled = True
        Label6.Enabled = True
        frameErroring.Enabled = True
        tbARC.Enabled = True
        tbFrom.Enabled = True
        tbTo.Enabled = True
    Else
        Label7.Enabled = False
        Label4.Enabled = False
        Label5.Enabled = False
        Label6.Enabled = False
        frameErroring.Enabled = False
        tbARC.Enabled = False
        tbFrom.Enabled = False
        tbTo.Enabled = False
    End If
End Function

'Validate the dates given by the user
Function DataValidated() As Boolean
    DataValidated = True
    If (tbFrom.TextLength >= 6 And tbTo.TextLength >= 6) Then
        If IsNumeric(tbFrom.Text) Then
            If (tbFrom.TextLength = 6) Then tbFrom.Text = Format(tbFrom.Text, "MM/DD/YY")
            If (tbFrom.TextLength = 8) Then tbFrom.Text = Format(tbFrom.Text, "MM/DD/YYYY")
        End If
        If IsNumeric(tbTo.Text) Then
            If (tbTo.TextLength = 6) Then tbTo.Text = Format(tbFrom.Text, "MM/DD/YY")
            If (tbTo.TextLength = 8) Then tbTo.Text = Format(tbFrom.Text, "MM/DD/YYYY")
        End If
        If (Not IsDate(tbFrom.Text) Or Not IsDate(tbTo.Text)) Then
            DataValidated = False
        End If
    Else
        DataValidated = False
    End If
End Function


