Attribute VB_Name = "CRSSetup"
Function Main()
    Dim SchoolCode As String
    Dim LoanTypes, LoanType, CRSCodes, CRSCode
    LoanTypes = Array("STFFRD", "UNSTFD", "PLUS")
    CRSCodes = Array("0060", "0061", "0062", "0063", "0064", "0065", "0066", _
                     "0067", "0068", "0069", "0070", "0071", "0072", "0073", _
                     "0074", "0079", "0088", "0091", "0092", "0096", "0109", _
                     "0113", "0114", "0135", "0136", "0141", "0143", "0144", _
                     "0152", "0153", "0154", "0155", "0156", "0157", "0158", _
                     "0159", "0160", "0161", "0162", "0163", "0164", "0165", _
                     "0169", "0170", "0194", "0195", "0205", "0231", "0247", _
                     "0307")
    If vbOK <> MsgBox("This script does C/R/S Setup for approved schools on COMPASS LO.  Click OK to continue or Cancel to end the script.", vbOKCancel, "C/R/S Setup") Then End
    If "" = Dir("T:\DSASG07.txt") Then
        MsgBox "The needed data file is not present.  Please resolve the issue and restart the script."
        End
    End If
    Open "T:\" & Dir("T:\DSASG07.txt") For Input As #1 'open data file
    Open "T:\CRS Setup Errors.txt" For Output As #2 'open error log
    'for each record in the file
    While Not EOF(1)
        Input #1, SchoolCode
        'for each loan type
        For Each LoanType In LoanTypes
            'for each CRS code
            For Each CRSCode In CRSCodes
                FastPathInput "TX3ZAPO7M" & SchoolCode & ";" & CStr(LoanType) & ";" & CStr(CRSCode)
                If Textcheck(22, 2, "04623 ENTERED COMBINATION ALREADY EXISTS") Then 'if it already exists
                    FastPathInput "TX3ZCPO7M" & SchoolCode & ";" & CStr(LoanType) & ";" & CStr(CRSCode)
                    XYInput 13, 26, "A"
                    Press "Enter"
                Else 'if it needs to be added
                    If Textcheck(12, 26, "C") Then
                        XYInput 18, 17, "YO1000"
                        XYInput 18, 63, "YO1001"
                        If CStr(CRSCode) = "0156" Then 'if bankruptcy code
                            XYInput 18, 33, "YO1002"
                        End If
                        Press "Enter"
                    ElseIf Textcheck(12, 26, "S") Then
                        Press "Enter"
                    End If
                End If
            Next
        Next
    Wend
    Close #2
    Close #1
    MsgBox "Processing Complete"
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
        Case "END"
            Session.TransmitTerminalKey rcIBMEraseEOFKey
        Case "DOWN"
            Session.TransmitTerminalKey rcIBMPA2Key
    Case "UP"
            Session.TransmitTerminalKey rcIBMPA1Key
        Case "TAB"
            Session.TransmitTerminalKey rcIBMTabKey
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

'new, sr765, aa, 08/25/04, 09/20/04
