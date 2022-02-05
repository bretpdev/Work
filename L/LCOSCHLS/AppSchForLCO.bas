Attribute VB_Name = "AppSchForLCO"
Function Main()
    Dim SID As String
    Dim LoanTypes, LoanType
'<1>LoanTypes = Array("SUBCNS", "UNCNS")
    LoanTypes = Array("SUBSPC", "UNSPC")                '<1>
'<2>
    frmLCOSchLS.Show
'</2>If vbOK <> MsgBox("This script approves schools for consolidations.  To continue click OK, else click Cancel.", vbOKCancel, "Approve Schools for Consolidation") Then End
    'end the script if the file is empty
    If "" = Dir("C:\Windows\Temp\DSASQ04.R2") Then
        MsgBox """C:\Windows\Temp\DSASQ04.R2"" does not exist."
        End
    End If
    'open file
    Open "C:\Windows\Temp\DSASQ04.R2" For Input As #1
    'process the file
    While Not EOF(1)
        Input #1, SID
'<2>        For Each LoanType In LoanTypes
'           ProcessSchool CStr(LoanType), SID
            ProcTX10 frmLCOSchLS.txtLoanType.Text, SID, frmLCOSchLS.txtEffDate.Text
            ProcTX13 frmLCOSchLS.txtLoanType.Text, SID, frmLCOSchLS.txtEffDate.Text
'
'</2>       Next
    Wend
    Close #1
    MsgBox "Processing Complete"
End Function

Sub ProcTX10(LT As String, SID As String, EffDate As String)
    sp.Q.FastPath "TX3Z/ATX10" & SID & ";" & LT & ";"
    If sp.Q.Check4Text(23, 2, "01018 ENTERED KEY ALREADY EXISTS") Then
'        MsgBox "School and Loan Type already exists on TX10."
        Exit Sub
    End If
    'if record can't be added then add info to error report
    If sp.Q.Check4Text(1, 73, "TXX2X") Then
        Open "C:\Windows\Temp\Approved Schools For Consolidation Error Report.txt" For Append As #2
        Write #2, "TX10", SID, sp.Q.GetText(23, 2, 75)
        Close #2
        Exit Sub
    End If
    sp.Q.PutText 10, 17, "B"
    sp.Q.PutText 11, 17, EffDate, "ENTER"
    'if record can't be added then add info to error report
    If sp.Q.Check4Text(23, 2, "01004 RECORD SUCCESSFULLY ADDED") = False Then
        Open "C:\Windows\Temp\Approved Schools For Consolidation Error Report.txt" For Append As #2
        Write #2, "TX10", SID, sp.Q.GetText(23, 2, 75)
        Close #2
    End If
End Sub

Sub ProcTX13(LT As String, SID As String, EffDate As String)
    sp.Q.FastPath "TX3Z/ATX13" & SID & ";" & LT & ";000749"
    If sp.Q.Check4Text(23, 2, "01018 ENTERED KEY ALREADY EXISTS") Then
'        MsgBox "School and Loan Type already exists on TX13."
        Exit Sub
    End If
    'if record can't be added then add info to error report
    If sp.Q.Check4Text(1, 72, "TXX2Y") Then
        Open "C:\Windows\Temp\Approved Schools For Consolidation Error Report.txt" For Append As #2
        Write #2, "TX13", SID, Trim(Session.GetDisplayText(23, 2, 75)),
        Close #2
        Exit Sub
    End If
    sp.Q.PutText 8, 26, SID
    sp.Q.PutText 10, 12, "E"
    sp.Q.PutText 11, 15, EffDate, "ENTER"
    'if record can't be added then add info to error report
    If sp.Q.Check4Text(23, 2, "01004 RECORD SUCCESSFULLY ADDED") = False Then
        Open "C:\Windows\Temp\Approved Schools For Consolidation Error Report.txt" For Append As #2
        Write #2, "TX13", SID, Trim(Session.GetDisplayText(23, 2, 75)),
        Close #2
    End If
End Sub
'<2>
''this function process each school in the file
'Function ProcessSchool(LT As String, SID As String)
'    FastPathInput "TX3Z/ATX10" & SID & ";" & LT & ";"
'    If Textcheck(1, 73, "TXX2X") Then
'        Open "C:\Windows\Temp\Approved Schools For Consolidation Error Report.txt" For Append As #2
'        Write #2, "TX10 Pre", SID, Trim(Session.GetDisplayText(23, 2, 75)),
'        Close #2
'    Else
'        XYInput 10, 17, "B"
''<1>    XYInput 11, 17, "030104", True
'        XYInput 11, 17, "100104", True              '<1>
'        'if record can't be added then add info to error report
'        If Textcheck(23, 2, "01004 RECORD SUCCESSFULLY ADDED") = False Then
'            Open "C:\Windows\Temp\Approved Schools For Consolidation Error Report.txt" For Append As #2
'            Write #2, "TX10", SID, Trim(Session.GetDisplayText(23, 2, 75)),
'            Close #2
'        End If
'    End If
'    FastPathInput "TX3Z/ATX13" & SID & ";" & LT & ";000749"
'    If Textcheck(1, 72, "TXX2Y") Then
'        Open "C:\Windows\Temp\Approved Schools For Consolidation Error Report.txt" For Append As #2
'        Write #2, "TX13 Pre", SID, Trim(Session.GetDisplayText(23, 2, 75)),
'        Close #2
'    Else
'        XYInput 8, 26, SID
'        XYInput 10, 12, "E"
''<1>    XYInput 11, 15, "030104", True
'        XYInput 11, 15, "100104", True              '<1>
'        If Textcheck(23, 2, "01004 RECORD SUCCESSFULLY ADDED") = False Then
'            Open "C:\Windows\Temp\Approved Schools For Consolidation Error Report.txt" For Append As #2
'            Write #2, "TX13", SID, Trim(Session.GetDisplayText(23, 2, 75)),
'            Close #2
'        End If
'    End If
'End Function

''Checks for specific text at a certain location on the screen
'Function Textcheck(Y As Integer, x As Integer, I As String)
'    If (Session.GetDisplayText(Y, x, Len(I)) = I) Then
'    Textcheck = True
'    Else
'    Textcheck = False
'    End If
'End Function
'
''this function will transmit a key for you
'Function Press(key As String)
'    key = UCase(key)
'    Select Case key
'        Case "F1"
'            Session.TransmitTerminalKey rcIBMPf1Key
'        Case "F2"
'            Session.TransmitTerminalKey rcIBMPf2Key
'        Case "F3"
'            Session.TransmitTerminalKey rcIBMPf3Key
'        Case "F4"
'            Session.TransmitTerminalKey rcIBMPf4Key
'        Case "F5"
'            Session.TransmitTerminalKey rcIBMPf5Key
'        Case "F6"
'            Session.TransmitTerminalKey rcIBMPf6Key
'        Case "F7"
'            Session.TransmitTerminalKey rcIBMPf7Key
'        Case "F8"
'            Session.TransmitTerminalKey rcIBMPf8Key
'        Case "F9"
'            Session.TransmitTerminalKey rcIBMPf9Key
'        Case "F10"
'            Session.TransmitTerminalKey rcIBMPf10Key
'        Case "F11"
'            Session.TransmitTerminalKey rcIBMPf11Key
'        Case "F12"
'            Session.TransmitTerminalKey rcIBMPf12Key
'        Case "ENTER"
'            Session.TransmitTerminalKey rcIBMEnterKey
'        Case "CLEAR"
'            Session.TransmitTerminalKey rcIBMClearKey
'        Case "END"
'            Session.TransmitTerminalKey rcIBMEraseEOFKey
'        Case "DOWN"
'            Session.TransmitTerminalKey rcIBMPA2Key
'    Case "UP"
'            Session.TransmitTerminalKey rcIBMPA1Key
'        Case "TAB"
'            Session.TransmitTerminalKey rcIBMTabKey
'        Case Else
'            MsgBox "There has been a key code error.  Please contact a programmer."
'    End Select
'    Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'End Function
'
''Enters information into the Fast Path.
'Function FastPathInput(inp As String)
'    Session.TransmitTerminalKey rcIBMClearKey
'    Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'    Session.TransmitANSI inp
'    Session.TransmitTerminalKey rcIBMEnterKey
'    Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'End Function
'
''Enters inp into the given X,Y coordinates
'    Function XYInput(Y As Integer, x As Integer, inp As String, Optional Enter As Boolean = False)
'    Session.MoveCursor Y, x
'    Session.TransmitANSI inp
'    'if enter = true then hit enter.
'    If (Enter) Then
'        Session.TransmitTerminalKey rcIBMEnterKey
'        Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'    End If
'End Function
'</2>


'new sr717, aa, 08/12/04, 08/17/04
'<1> sr779, aa, 09/20/04, 09/21/04
'<2> sr1628, tp, 05/16/06
