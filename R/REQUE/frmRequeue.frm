VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmRequeue 
   Caption         =   "Re-Queue"
   ClientHeight    =   3795
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5760
   OleObjectBlob   =   "frmRequeue.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmRequeue"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False





Private Queue As String
Private SubQueue As String

Private Sub btnReQueue_Click()
    'check if the date is valid
    If DateFormatter("You must provide a valid date.") = False Then
        Exit Sub
    End If
    'check if the date is the current date or future date
    If CDate(tbDate.Text) < Date Then
        MsgBox "The re-queue date must be today or some future date."
        Exit Sub
    End If
    LP9OProcessing
    'if queue = "" then original screen was TC00 else it was TX6Q
    If Queue = "" Then 'TC00
        FastPathInput "TX3Z"
        Session.TransmitANSI "ITX6X"
        Press "End"
        Press "Enter"
        MsgBox "The task has been successfully re-queued."
        End
    Else 'TX6Q
        FastPathInput "TX3Z/ITX6X" & Queue & ";" & SubQueue
        If Textcheck(1, 74, "TXX71") Then
            MsgBox "The task has been successfully re-queued."
            End
        Else
            MsgBox "The task has been successfully re-queued.  There doesn't appear to be any more tasks in the " & Queue & " queue."
            End
        End If
    End If
End Sub

Private Sub UserForm_Initialize()
    'Create and set up Group Combo box
    cbGroup.AddItem "D - Borrower Services", 0
    cbGroup.AddItem "S - Auxiliary Services", 1
    cbGroup.AddItem "O - Loan Origination", 2
    cbGroup.AddItem "R - Account Services", 3
    cbGroup.AddItem "X - Administrative", 4
    cbGroup.ListIndex = 0
    'check if user is on TC00 or TX6Q
    If Textcheck(1, 72, "TXX6S") Then 'if the user is on TX6Q
        Label5.Visible = False
        cbGroup.Visible = False
        Label6.Left = 6
        Label6.Top = 78
        tbDate.Left = 66
        tbDate.Top = 72
        TX6QGatherInfo Queue, SubQueue
    ElseIf Textcheck(1, 74, "TCX13") Then 'if the user is on TC00
        TC00GatherInfo
    Else 'if the user is on neither TC00 or TX6Q then return an error and end processing
        MsgBox "You must be on TC00 or TX6Q to continue processing."
        End
    End If
End Sub

'this function gathers the needed information from TC00 for the requeue action
Function TC00GatherInfo()
    'be sure that the current queue task has been completed
    If Textcheck(23, 2, "01004 RECORD SUCCESSFULLY ADDED") Then
        lbSSN.Caption = Session.GetDisplayText(1, 9, 9)
        tbText.Text = Replace(Session.GetDisplayText(12, 10, 60) & " " & Session.GetDisplayText(13, 10, 60) & " " & Session.GetDisplayText(14, 10, 60) & " " & _
                      Session.GetDisplayText(15, 10, 60) & " " & Session.GetDisplayText(16, 10, 60) & " " & Session.GetDisplayText(17, 10, 60), "_", "")
    Else 'if the task hasn't been completed
        MsgBox "The queue task must be completed."
        End
    End If
End Function

'This function gathers the info for requeue action
Function TX6QGatherInfo(Queue As String, SubQueue As String)
    Queue = Session.GetDisplayText(4, 19, 2)
    SubQueue = Session.GetDisplayText(5, 19, 2)
    lbSSN.Caption = Session.GetDisplayText(1, 15, 9)
    lbARC.Caption = Session.GetDisplayText(6, 19, 5)
    lbARCDes.Caption = Trim(Session.GetDisplayText(6, 27, 50))
    tbText.Text = Trim(Session.GetDisplayText(16, 2, 78))
    tbText.Text = tbText.Text & " " & Trim(Session.GetDisplayText(20, 2, 78))
End Function

'This function re-queues the task on LP9O
Function LP9OProcessing()
'<1->
    'FastPathInput "LP9OA" & lbSSN.Caption & ";;ZZZZCOMP;ALL;;"
    FastPathInput "LP9OA" & lbSSN.Caption & ";;ZZZZCOMP"
'</1>
    XYInput 11, 25, Format(CDate(tbDate.Text), "MMDDYYYY")
    XYInput 16, 12, lbSSN.Caption & ", " & lbARC.Caption & ", " & Mid(cbGroup.Value, 1, 1) & ", " & tbText.Text
    Press "Enter"
    Press "F6"
    'check if the task was created
    If Textcheck(22, 3, "48003 DATA SUCCESSFULLY ADDED") = False Then
        If vbOK <> MsgBox("The queue task wasn't able to be added.  Please take a moment and fix the problem.", vbOKCancel, "Re-Queue Problems") Then End
        Session.WaitForTerminalKey rcIBMInsertKey, "01:00:00"
        Session.TransmitTerminalKey rcIBMInsertKey
        Press "Enter"
        Press "F6"
    End If
End Function

Private Sub cbGroup_Change()
    If Queue = "" Then
        If Mid(cbGroup.Value, 1, 1) = "D" Then
            lbARC.Caption = "DCALL"
        ElseIf Mid(cbGroup.Value, 1, 1) = "S" Then
            lbARC.Caption = "SCALL"
        ElseIf Mid(cbGroup.Value, 1, 1) = "O" Then
            lbARC.Caption = "OCALL"
        ElseIf Mid(cbGroup.Value, 1, 1) = "R" Then
            lbARC.Caption = "BCALL"
        ElseIf Mid(cbGroup.Value, 1, 1) = "X" Then
            lbARC.Caption = "XCALL"
        End If
    End If
End Sub

Private Sub UserForm_Terminate()
    End
End Sub

Private Sub btnCancel_Click()
    End
End Sub

'Checks for specific text at a certain location on the screen
Function Textcheck(Y As Integer, x As Integer, i As String)
    If (Session.GetDisplayText(Y, x, Len(i)) = i) Then
    Textcheck = True
    Else
    Textcheck = False
    End If
End Function

'this function will transmit a key for you
Function Press(key As String)
    key = UCase(key)
    Select Case key
        Case "F1"
            Session.TransmitTerminalKey rcIBMPf1Key
        Case "F2"
            Session.TransmitTerminalKey rcIBMPf2Key
        Case "F3"
            Session.TransmitTerminalKey rcIBMPf3Key
        Case "F4"
            Session.TransmitTerminalKey rcIBMPf4Key
        Case "F5"
            Session.TransmitTerminalKey rcIBMPf5Key
        Case "F6"
            Session.TransmitTerminalKey rcIBMPf6Key
        Case "F7"
            Session.TransmitTerminalKey rcIBMPf7Key
        Case "F8"
            Session.TransmitTerminalKey rcIBMPf8Key
        Case "F9"
            Session.TransmitTerminalKey rcIBMPf9Key
        Case "F10"
            Session.TransmitTerminalKey rcIBMPf10Key
        Case "F11"
            Session.TransmitTerminalKey rcIBMPf11Key
        Case "F12"
            Session.TransmitTerminalKey rcIBMPf12Key
        Case "ENTER"
            Session.TransmitTerminalKey rcIBMEnterKey
        Case "CLEAR"
            Session.TransmitTerminalKey rcIBMClearKey
        Case "END"
            Session.TransmitTerminalKey rcIBMEraseEOFKey
        Case "UP"
            Session.TransmitTerminalKey rcIBMPA1Key
        Case Else
            MsgBox "There has been a key code error.  Please contact a programmer."
    End Select
    Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
End Function

'Enters information into the Fast Path.
Function FastPathInput(inp As String)
Session.TransmitTerminalKey rcIBMClearKey
Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
Session.TransmitANSI inp
Session.TransmitTerminalKey rcIBMEnterKey
Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
End Function

'Enters inp into the given X,Y coordinates
Function XYInput(Y As Integer, x As Integer, inp As String, Optional Enter As Boolean = False)
Session.MoveCursor Y, x
Session.TransmitANSI inp
'if enter = true then hit enter.
If (Enter) Then
Session.TransmitTerminalKey rcIBMEnterKey
Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
End If
End Function

'format strings in MM/DD/YYYY format
Function DateFormatter(Optional ErrorMessage As String = "That wasn't a valid date.  Please try again.") As Boolean
        'return an invalid date if the length is not 6, 8, or 10 (lengths of valid date strings)
        If Len(tbDate.Text) <> 6 And Len(tbDate.Text) <> 8 And Len(tbDate.Text) <> 10 Then
            DateFormatter = False
        'format 6 digit date with no slashes
        ElseIf Len(tbDate.Text) = 6 And IsDate(Format(tbDate.Text, "##/##/##")) = True Then
            DateFormatter = True
            tbDate.Text = Format(DateValue(Format(tbDate.Text, "##/##/##")), "MM/DD/YYYY")
        'format 6 digit date with slashes
        ElseIf Len(tbDate.Text) = 8 And IsDate(tbDate.Text) = True Then
            DateFormatter = True
            tbDate.Text = Format(DateValue(tbDate.Text), "MM/DD/YYYY")
        'format 8 digit date with no slashes
        ElseIf Len(tbDate.Text) = 8 And IsDate(Format(tbDate.Text, "##/##/####")) Then
            DateFormatter = True
            tbDate.Text = Format(tbDate.Text, "##/##/####")
        'Check if 10 digit string is a date
        ElseIf Len(tbDate.Text) = 10 And IsDate(tbDate.Text) Then
            DateFormatter = True
        Else
            'an thing that didn't fit into one of the above catergories is not a date
            DateFormatter = False
        End If
        If DateFormatter = False Then MsgBox ErrorMessage
End Function


'new, sr600, aa, 04/13/04, 07/19/04
'<1>, sr747, aa, 08/13/04, 08/16/04
