Attribute VB_Name = "SCSkip"
Function Main()
    LP22ValidAddrCheck
    AddSkipOnLP5C FindCodeOnLG02()
    'add LP50 comment
    AddLP50 "", "ARSKP", "ADDSCSKP", "AM", "10"
    MsgBox "Skip Successfully Added"
End Function

'This function checks the valid address and phone fields
Function LP22ValidAddrCheck()
    FastPathInput "LP22I"
    If (Not Textcheck(11, 57, "N") And Not Textcheck(14, 38, "N")) Then
        MsgBox "Either the borrower's address or phone number must be invalid to add an administrative skip."
        End
    End If
End Function

'This function adds the skip on LP5C
Function AddSkipOnLP5C(SchoolCode As String)
    FastPathInput "LP5CA"
    XYInput 3, 39, SchoolCode & "001"
    XYInput 5, 22, "20", True
    'If the skip record can't be added then give the user an error message and end the script
    If (Not Textcheck(22, 3, "48003 DATA SUCCESSFULLY ADDED")) Then
        MsgBox "Error received when attempting to add skip request."
        End
    End If
End Function

'This function finds and returns one of the school codes on LG02
Function FindCodeOnLG02() As String
    Dim SchoolCode As Variant, SchoolCodes As Variant
    Dim Row As Integer
    'setup SchoolCodes array
    SchoolCodes = Array("02178500", "02298500", "02360800", "00367300", "00367400", "00367401", _
                            "00367403", "00367404", "00367405", "03003000")
    FastPathInput "LG02I"
    If (Textcheck(1, 58, "LOAN APPLICATION SELECT")) Then 'if the script finds a loan selection screen (multiple loans)
        Row = 10
        While (Not Textcheck(22, 3, "46004 NO MORE DATA TO DISPLAY"))
            For Each SchoolCode In SchoolCodes
                'if the code is found then collect it, return it and exit the function
                If (Textcheck(Row, 8, SchoolCode)) Then
                    FindCodeOnLG02 = Session.GetDisplayText(Row, 8, 8)
                    Exit Function
                End If
            Next
            Row = Row + 1
            If (Row = 21) Then
                Row = 10
                Press "F8"
            End If
        Wend
    Else 'If the script finds only one loan (no selection screen)
        For Each SchoolCode In SchoolCodes
            'if the code is found then collect that school code, return it and exit the function
            If (Textcheck(10, 41, SchoolCode)) Then
                FindCodeOnLG02 = Session.GetDisplayText(10, 41, 8)
                Exit Function
            End If
        Next
    End If
    'If the script hasn't exited the function before it gets to this point that means that one of the
    'valid school codes wasn't found.  Thus give the user an error message and end the script.
    MsgBox "This borrower is not part of a Special Campaign and therefore you cannot add a skip request."
    End
End Function

'Checks for specific text at a certain location on the screen
Function Textcheck(y As Integer, x As Integer, ByVal i As String)
    If (Session.GetDisplayText(y, x, Len(i)) = i) Then
    Textcheck = True
    Else
    Textcheck = False
    End If
End Function

'this function will transmit a key for you
Function Press(Key As String)
    Key = UCase(Key)
    Select Case Key
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
Function XYInput(y As Integer, x As Integer, inp As String, Optional Enter As Boolean = False)
Session.MoveCursor y, x
Session.TransmitANSI inp
'if enter = true then hit enter.
If (Enter) Then
Session.TransmitTerminalKey rcIBMEnterKey
Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
End If
End Function

'new, sr518, aa, 1/30/04, 02/27/04
