Attribute VB_Name = "CancelQTksANDErrActvCom"
Function FormStart()
    'Make Sure that the users should be able to access the scripts functionality
'<1->
'   If (Not ErrorActivityComment.LP40("UT00044") And Not ErrorActivityComment.LP40("UT00144") And Not ErrorActivityComment.LP40("UT00180")) Then
    If Not IsAuthUser Then
'</1>
        MsgBox "Your user ID is not allowed to access this script.", vbInformation, "ACCESS DENIED"
        End
    End If
    
    If (vbCancel = MsgBox("This script is designed to cancel queue tasks and, if desired, error out activity comments.  If you wish to proceed click OK else click cancel.", vbOKCancel, "Cancel Queue Tasks/Error Activity Comment Script")) Then End
    'Show the form to get user input
    frmCancelQueueTasks.Show 1
    
    If Dir("T:\SSN Comments to Error out.txt") <> "" Then Kill "T:\SSN Comments to Error out.txt"
    MsgBox "Processing Complete"
End Function

Function Main(Queue As String, SubQueue As String, Msg As String, _
            Optional ARC As String = "", Optional DFrom As String = "", _
            Optional DTo As String = "")
    frmCancelQueueTasks.Hide
    If Msg = "" Then
        'If the error message is left blank then cancel all the queue tasks
        TX6XCancelAllTasks Queue, SubQueue
        If ARC <> "" Then
            'If a ARC is specified then delete the activity comments
            ErrorOutActivityComments ARC, DFrom, DTo
        End If
    Else
        'If the Error message isn't blank then search for and cancel the queue tasks with that message.
        TX6XCancelErrorMsgTasks Queue, SubQueue, Msg
    End If
End Function
         
Function ErrorOutActivityComments(ARC As String, DFrom As String, DTo As String)
    Dim SSN As String
    If ("" <> Dir("T:\SSN Comments to Error out.txt")) Then
        Open "T:\SSN Comments to Error out.txt" For Input As #1
        While (Not EOF(1))
           Input #1, SSN
           'Delete actixity comment
           ErrorActivityComment.DeleteActivityComment SSN, ARC, DFrom, DTo
        Wend
        Close #1
    End If
End Function

         
'This function cancels all tasks in a given queue
Function TX6XCancelAllTasks(Queue As String, SubQueue As String)
    Dim SSN As String
    Open "T:\SSN Comments to Error out.txt" For Output As #2
    FastPathInput "TX3Z/ITX6X" & Queue & ";" & SubQueue
    While (Not Textcheck(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"))
        'Select the first task in the queue
        XYInput 21, 18, "01", True
        'Create SSN String
        SSN = Session.GetDisplayText(3, 8, 3) & Session.GetDisplayText(3, 12, 2) & Session.GetDisplayText(3, 15, 4)
        'Put SSN in file "SSN Comments to Error out.txt" for erroring out activity comments if the user made that choice
        Write #2, SSN
        FastPathInput "TX3Z/ITX6X" & Queue & ";" & SubQueue
        XYInput 21, 18, "01"
        Press "F2"
        XYInput 8, 19, "X"
        XYInput 9, 19, "CANCL", True
        'If error is found when trying to enter an action response then blank it out and then hit enter again
        If (Textcheck(23, 2, "01644 ACTION RESPONSE CODE IS INVALID - TASK HAS NO ACTIVITY")) Then
            XYInput 9, 19, "     ", True
        End If
        FastPathInput "TX3Z/ITX6X" & Queue & ";" & SubQueue
    Wend
    Close #2
End Function

Function TX6XCancelErrorMsgTasks(Queue As String, SubQueue As String, Msg As String)
    Dim flag As Boolean
    flag = True
    FastPathInput "TX3Z/ITX6X" & Queue & ";" & SubQueue
    While (flag)
        'Select tasks that have the specified error code
        If (LocatedValidTask(Msg)) Then
            FastPathInput "TX3Z/ITX6X" & Queue & ";" & SubQueue
            XYInput 21, 18, "01"
            Press "F2"
            XYInput 8, 19, "X"
            XYInput 9, 19, "CANCL", True
            'If error is found when trying to enter an action response then blank it out and then hit enter again
            If (Textcheck(23, 2, "01644 ACTION RESPONSE CODE IS INVALID - TASK HAS NO ACTIVITY")) Then
                XYInput 9, 19, "     ", True
            End If
            FastPathInput "TX3Z/ITX6X" & Queue & ";" & SubQueue
        Else
            flag = False
        End If
    Wend
End Function
         
Function LocatedValidTask(Msg As String) As Boolean
    Dim Row As Integer, col As Integer
    LocatedValidTask = True
    'Infinite loop, the exit function statements are what stops the loop
    While (1)
        'If the error message is found on the screen then select and place that task in a work status
        If (FindErrorMsgOnTX6X(Row, col, Msg)) Then
            XYInput 21, 18, Session.GetDisplayText((Row - 1), 4, 1), True
            Exit Function
        'If the message isn't found on the screen then press F8 and check to see if you're at the end of the queue
        Else
            Press "F8"
            If (Textcheck(23, 2, "90007 NO MORE DATA TO DISPLAY")) Then
                LocatedValidTask = False
                Exit Function
            'If I've gone through 20 pages I need to press enter to access the next 20
            ElseIf (Textcheck(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA")) Then
                Press "Enter"
            End If
        End If
    Wend
End Function
         
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

'This function adds spaces to the end of the error message being sure
'that FindErrorMsgOnTX6X() finds the exact error message.
Function AddSpacesToErrorMsg(Msg As String)
    While (Len(Msg) < 80)
        Msg = Msg & " "
    Wend
End Function

'This function checks the error messages on TX6X and returns true if it finds Msg and
'changes row and col to reflect Msg screen placement.
Function FindErrorMsgOnTX6X(Row As Integer, col As Integer, Msg As String) As Boolean
    AddSpacesToErrorMsg Msg
    If (Textcheck(9, 2, Msg)) Then
        Row = 9
        col = 2
        FindErrorMsgOnTX6X = True
    ElseIf (Textcheck(12, 2, Msg)) Then
        Row = 12
        col = 2
        FindErrorMsgOnTX6X = True
    ElseIf (Textcheck(15, 2, Msg)) Then
        Row = 15
        col = 2
        FindErrorMsgOnTX6X = True
    ElseIf (Textcheck(18, 2, Msg)) Then
        Row = 18
        col = 2
        FindErrorMsgOnTX6X = True
    Else
        FindErrorMsgOnTX6X = False
    End If
End Function

'determine if the user is an authorized user
Function IsAuthUser() As Boolean
    Dim AuthUserID As String
    Dim UserID As String
    
    'get userid and set initial value
    UserID = sp.Common.GetUserID
    IsAuthUser = True
    
    'open the authorized user ID list
    If sp.Common.TestMode Then
        Open "X:\Sessions\Lists\Test\Cancel Queue Tasks Auth Users.txt" For Input As #1
    Else
        Open "X:\Sessions\Lists\Cancel Queue Tasks Auth Users.txt" For Input As #1
    End If
    
    'exit the function if the ID is in the list
    While Not EOF(1)
        Input #1, AuthUserID
        If AuthUserID = UserID Then
            Close #1
            Exit Function
        End If
    Wend
    
    'set value to false if ID is not on the list
    IsAuthUser = False
End Function

'new, sr 477, ??/??/??, 06/16/04
'<1>, sr1728, jd


