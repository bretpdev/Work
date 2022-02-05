Attribute VB_Name = "FORSCHADD"
'Foreign School Address Clean Up
Dim SCHOOL As String
Dim ADD1 As String
Dim ADD2 As String
Dim ADD3 As String
Dim City As String
Dim ZIP As String
Dim FSTATE As String
Dim FCOUNTRY As String
Dim FTPFolder As String

Sub ForScl()
    With Session
    
        warn = MsgBox("This script Updates Compass Foreign School Addresses with infomation retrieved from OneLINK.  Click OK if you wish to proceed.", vbOKCancel, "Foreign School Address Clean Up")
        If warn <> 1 Then
            End
        End If
                
        sp.Common.TestMode FTPFolder
                
        'delete old error file
        If Dir$("T:\ForeignSchoolAddressERROR.txt") > "" Then
            Kill "T:\ForeignSchoolAddressERROR.txt"
        End If
        
        If Dir$(FTPFolder & "DSASQ06.txt") > "" Then
            Open FTPFolder & "DSASQ06.txt" For Input As #1
            Do While Not EOF(1)
                Input #1, SCHOOL, ADD1, ADD2, ADD3, City, ZIP, FSTATE, FCOUNTRY
                .TransmitTerminalKey rcIBMClearKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitANSI "TX3ZCTX0Y" & SCHOOL & "000"
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                
                If .GetDisplayText(23, 2, 5) = "30021" Then
                    Open "T:\ForeignSchoolAddressERROR.txt" For Append As #2
                        Write #1, "The School ID " & SCHOOL & " is invalid. The script terminated after this ID did not bring up the TX0Y screen. "
                    Close #2
                    End
                Else 'if no error
                '<1->
'                    .MoveCursor 9, 23
'                    .TransmitTerminalKey rcIBMEraseEOFKey
'                    XYInput 9, 23, ADD1
'                    .MoveCursor 10, 23
'                    .TransmitTerminalKey rcIBMEraseEOFKey
'                    XYInput 10, 23, ADD2
'                    .MoveCursor 11, 23
'                    .TransmitTerminalKey rcIBMEraseEOFKey
'                    XYInput 11, 23, ADD3
'                    .MoveCursor 12, 23
'                    .TransmitTerminalKey rcIBMEraseEOFKey
'                    XYInput 12, 23, City
'                    .MoveCursor 13, 23
'                    .TransmitTerminalKey rcIBMEraseEOFKey 'clear State field
'                    .MoveCursor 13, 46
'                    .TransmitTerminalKey rcIBMEraseEOFKey
'                    XYInput 13, 46, ZIP
'                    .MoveCursor 14, 23
'                    .TransmitTerminalKey rcIBMEraseEOFKey
'                    XYInput 14, 23, FSTATE
'                    .MoveCursor 14, 51
'                    .TransmitTerminalKey rcIBMEraseEOFKey
'                    XYInput 14, 51, FCOUNTRY
                    .MoveCursor 11, 23
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    XYInput 11, 23, ADD1
                    .MoveCursor 12, 23
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    XYInput 12, 23, ADD2
                    .MoveCursor 13, 23
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    XYInput 13, 23, ADD3
                    .MoveCursor 14, 13
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    XYInput 14, 13, City
                    .MoveCursor 14, 53
                    .TransmitTerminalKey rcIBMEraseEOFKey 'clear State field
                    .MoveCursor 14, 69
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    XYInput 14, 69, ZIP
                    .MoveCursor 15, 21
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    XYInput 15, 21, FSTATE
                    .MoveCursor 15, 49
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    XYInput 15, 49, FCOUNTRY
'</1>
                    
                    .TransmitTerminalKey rcIBMEnterKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                
                    Do While .GetDisplayText(23, 2, 5) <> "01005" 'check for error
                        MsgBox "Take a moment to fix the error. Then press the Insert Key"
                        Session.WaitForTerminalKey rcIBMInsertKey, "01:00:00"
                        Session.TransmitTerminalKey rcIBMInsertKey
                        .TransmitTerminalKey rcIBMEnterKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    Loop
                
                End If
            Loop
            Close #1
            Kill FTPFolder & "DSASQ06.txt"
            MsgBox "Processing complete.", vbOKCancel + vbInformation, "Processing Complete"
        Else
            MsgBox "File " & FTPFolder & "DSASQ06.txt was not found."
        End If
    End With
End Sub
    
Function XYInput(Y As Integer, x As Integer, inp As String, Optional Enter As Boolean = False)
    Session.MoveCursor Y, x
    Session.TransmitANSI inp
    'if enter = true then hit enter.
    If (Enter) Then
        
        Session.TransmitTerminalKey rcIBMEnterKey
        Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
    End If
End Function

'<1>

