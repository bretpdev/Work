Attribute VB_Name = "LoanSaleFix"
Function Main()
'<1>
'    Dim SaleID(16) As String
'    Dim OwnerID(16) As String
    Dim SaleID As String
    Load frmLoanSaleFix
    frmLoanSaleFix.Show
    SaleID = UCase(frmLoanSaleFix.txtSaleID.Value)
'</1>
    Dim row As Integer
'<1>
'    Dim Counter As Integer
'</1>
    Dim FileInput As String
'<2>
    Dim FTPFolder As String
    Sp.Common.TestMode FTPFolder
'</2>

    With Session
'<1>
'        'get old Sale IDs and Owner IDs
'        FastPathInput "TX3Z/ITS4P"
'        XYInput 5, 26, "04*"
'        XYInput 11, 26, "061704" 'sale date
'        XYInput 17, 26, "R", True
'        row = 8
'        While Textcheck(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
'            SaleID(Counter) = .GetDisplayText(row, 5, 7)
'            OwnerID(Counter) = .GetDisplayText(row, 13, 6)
'            row = row + 1
'            Counter = Counter + 1
'            If Textcheck(row, 3, " ") Then
'                row = 8
'                Press "F8"
'            End If
'        Wend
'        'enter in SSNs for each sale ID
'        Counter = 0
'        While Counter < 17
'</1>
            FastPathInput "TX3Z/CTS4P"
'<1>
'            XYInput 5, 26, "04*"
'            XYInput 7, 26, OwnerID(Counter)
'            XYInput 11, 26, "061804", True 'sale date
'<2>
'            XYInput 5, 26, SaleID, True
            XYInput 5, 26, Format(Date, "yy") & "*", True
            
            'Loan Sale Data Selection
            SelectLoanSaleData (SaleID)
'</2>
            'XYInput 10, 26, Format(Date, "mmddyyyy"), True   'sale date
'</1>
            Press "F4"
            Press "F4"
            Press "F2"
'<1>
'            Open "C:\Windows\Temp\Loan Sale 6-17-04.txt" For Input As #1
            'Find Old Sale ID
'            Input #1, FileInput
'            While FileInput <> SaleID '<1>(Counter)
'                Input #1, FileInput
'            Wend
'<2>
'            Open "C:\Windows\Temp\Loan Sale " & SaleID & ".txt" For Input As #1
            Open FTPFolder & SaleID & ".txt" For Input As #1
            'Clear the error log.
'            Open "X:\PADD\FTP\" & SaleID & "_errors.txt" For Output As #2
            Open FTPFolder & SaleID & "_errors.txt" For Output As #2
            Close #2
'</2>
'</1>
            'Process all SSNs and loan IDs  for that sale ID
            row = 9
            While Not EOF(1)
                Input #1, FileInput
                XYInput 10, 39, Mid(FileInput, 1, 9), True
'<2>
                If check4text(22, 2, "03914") Then  'No loans for this borrower.
                    Open FTPFolder & SaleID & "_errors.txt" For Append As #2
                    Write #2, FileInput     'Add the record to the error log.
                    Close #2
                Else
'</2>
                    While Trim(.GetDisplayText(row, 17, 3)) <> val(Mid(FileInput, 10, 4))
                        row = row + 1
                        If row = 23 Then
                            row = 9
                            Press "F8"
                        End If
                    Wend
                    XYInput row, 3, "S", True
'<2>
                    If Not check4text(22, 2, "02114") Then  '02114 indicates success.
                        Open FTPFolder & SaleID & "_errors.txt" For Append As #2
                        Write #2, FileInput     'Add the record to the error log.
                        Close #2
                    End If
'</2>
                    Press "F12"
                    Press "F4"
                    Press "F2"
                End If      '<2>
            Wend
            Close #1
'<1>
'            Counter = Counter + 1
'        Wend
        MsgBox "Loan Sale processing is complete.", vbOKOnly, "Processing Complete"
'</1>
        Kill FTPFolder & SaleID & ".txt"    '<2>
    End With
End Function

Sub SelectLoanSaleData(SaleID As String)
    Dim row As Integer
    Do
        'Check each record (lines 8-19) for a match on the Loan Sale ID.
        row = 8
        Do While Not check4text(row, 5, " ")
            If GetText(row, 5, 7) = SaleID Then
                'Select this record and return.
                puttext 21, 18, GetText(row, 2, 2), "ENTER"
                Exit Sub
            End If
            row = row + 1
        Loop
        'If the Loan Sale ID wasn't found on this page, go to the next page.
        hit "F8"
        'If we were already on page 20, hit ENTER to continue to a new set of pages.
        If check4text(22, 2, "90007") Then hit "ENTER"
        'Getting a "90007" message, hitting ENTER, and then getting a "01027" message instead of a new page means we're out of records; stop the script.
        If check4text(22, 2, "01027") Then
            MsgBox "The Loan Sale ID was not found in the system. The script will now end.", vbOKOnly + vbInformation, "Sale ID not found"
            End
        End If
    Loop
End Sub

'Checks for specific text at a certain location on the screen
Function Textcheck(Y As Integer, X As Integer, i As String)
    If (Session.GetDisplayText(Y, X, Len(i)) = i) Then
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
Function XYInput(Y As Integer, X As Integer, inp As String, Optional Enter As Boolean = False)
Session.MoveCursor Y, X
Session.TransmitANSI inp
'if enter = true then hit enter.
If (Enter) Then
Session.TransmitTerminalKey rcIBMEnterKey
Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
End If
End Function

'<1> sr2323, db
'<2> sr2359, db
