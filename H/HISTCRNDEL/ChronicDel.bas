Attribute VB_Name = "ChronicDel"
Dim SSN As String
Dim pSSN As String               'previous SSN
Dim Name As String
Dim SchCd As String
Dim SchName As String
Dim FirstS02Date As String
Dim Schools As String            'text string of all school codes from the SAS file
Dim SchListOL() As String        'borrower/school records from OneLINK SAS file
Dim SchListN() As String         'borrower/school records matching criteria on NelNet
Dim SchListMstr() As String      'list of all school codes appearing in SAS file


'create a report of the percentage of chronic delinquent borrowers
Sub ChronicDelMain()
    With Session
        Dim ol As Integer        'number of borrowers in SAS file
        Dim n As Integer         'number of borrowers with 3 or more S02 records in the same 12 months
        Dim x As Integer         'number of S02 records in the same 12 months
        Dim ols As Integer       'number of records in the OneLINK school list array
        Dim ns As Integer        'number of records in the Nelnet school list array
        Dim ms As Integer        'number of schools in master school list
        Dim olx As Integer       'number of borrowers in SAS file for school
        Dim nx As Integer        'number of borrowers in Nelnet file for school
        
        warn = MsgBox("This script creates a report of the percentage of chronically delinquent borrowers.  The script must be run from the 'Select Division to Work With' screen.  Click OK to continue or cancel to quit.", vbOKCancel, "Chronic Delinquents")
        If warn <> 1 Then End
        'warn if the correct screen is not displayed
        If .GetDisplayText(1, 27, Len("Select Division to Work With")) <> "Select Division to Work With" Then
            MsgBox "This script must be run from the 'Select Division to Work With' screen.  Access the screen and run the script again.", , "Invalid Screen"
            End
        End If
        
        'process SAS file
        Common.MandatorySASProcessing "X:\PADD\FTP\ULWM01.LWM01R2.*.*", "X:\PADD\FTP\ULWM01.LWM01R1.*.*"
        'open files
        Open "X:\PADD\FTP\" & Dir("X:\PADD\FTP\ULWM01.LWM01R2.*.*") For Input As #1
        Open "X:\PADD\FTP\ULWM01.LWM01R2out.txt" For Output As #2
        
        'select SLFS
        .MoveCursor 8, 2
        .TransmitANSI "1"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'select option 1 (remote inquiry) on main menu
        .TransmitANSI "1"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'select option 2 (borrower groups) on remote inquiry full service screen
        .TransmitANSI "2"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        
        SSN = ""
        pSSN = ""
        Schools = ""
        ol = 0
        n = 0
        ols = 0
        ns = 0
        ms = 0
        Do While Not EOF(1)
            'get an ssn from the file and check to see if it is on Nelnet's system
            Do While .GetDisplayText(1, 2, 7) = "INAEPVK"
                'read data for each line in the file but only process each SSN once
                Do While pSSN = SSN And EOF(1) = False
                    Input #1, SSN, Name, SchCd, SchName
                    ols = ols + 1
                    ReDim Preserve SchListOL(1 To 2, 1 To ols) As String
                    SchListOL(1, ols) = SSN
                    SchListOL(2, ols) = SchCd
                    'add the school to the master school list if it is not there already
                    If InStr(Schools, SchCd) = 0 Then
                        Schools = Schools & SchCd
                        ms = ms + 1
                        ReDim Preserve SchListMstr(1 To 2, 1 To ms) As String
                        SchListMstr(1, ms) = SchCd
                        SchListMstr(2, ms) = SchName
                    End If
                Loop
                pSSN = SSN
                ol = ol + 1
                'enter the SSN
                .TransmitANSI SSN
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            Loop
            'hit F16 to access borrower history
            .TransmitTerminalKey rcIBMF16Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'find all occurances of S02 in column 5
            row = 7
            x = 0
            Do
                'if code S02 appears an the record date is not equal to the previous record date, process the record
                If .GetDisplayText(row, 5, 3) = "S02" And PrevS02Date <> .GetDisplayText(row - 1, 2, 8) Then
                    'get the record date
                    PrevS02Date = .GetDisplayText(row - 1, 2, 8)
                    'set the begin date for the year range if this is the first record
                    If x = 0 Then FirstS02Date = PrevS02Date
                    'increment or restart the counter depending on whether the date is with 12 months of the begin date
                    If DateValue(PrevS02Date) >= DateValue(FirstS02Date) - 365 Then
                        x = x + 1
                    Else
                        FirstS02Date = PrevS02Date
                        x = 1
                    End If
                End If
                'count the borrower and add the record to the output file and add a record to the Nelnet school list array for each school listed for the borrower if the count reaches 3
                If x = 3 Then
                    n = n + 1
                    'do until the next SSN is input
                    Do Until pSSN <> SSN
                        'write the record to the output file
                        Write #2, SSN, Name, SchCd, SchName
                        ns = ns + 1
                        ReDim Preserve SchListN(1 To 2, 1 To ns) As String
                        SchListN(1, ns) = SSN
                        SchListN(2, ns) = SchCd
                        If EOF(1) = True Then Exit Do
                        'input the next record
                        Input #1, SSN, Name, SchCd, SchName
                        'increment the records from OneLINK counter
                        ols = ols + 1
                        'add the record to the OneLINK school list array
                        ReDim Preserve SchListOL(1 To 2, 1 To ols) As String
                        SchListOL(1, ols) = SSN
                        SchListOL(2, ols) = SchCd
                        'add the school to the master school list if it is not there already
                        If InStr(Schools, SchCd) = 0 Then
                            Schools = Schools & SchCd
                            ms = ms + 1
                            ReDim Preserve SchListMstr(1 To 2, 1 To ms) As String
                            SchListMstr(1, ms) = SchCd
                            SchListMstr(2, ms) = SchName
                        End If
                    Loop
                    Exit Do
                End If
                row = row + 1
                'go to the next page or exit if the end of the page is reached
                If row = 22 Then
                    If .GetDisplayText(21, 79, 1) = "+" Then
                        .TransmitTerminalKey rcIBMPageDownKey
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        row = 7
                    Else
                        Exit Do
                    End If
                End If
            Loop
            'return to enter the next SSN
            .TransmitTerminalKey rcIBMF3Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitTerminalKey rcIBMF3Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Loop
        Close #1
        Close #2
        'return to the select division screen
        For i = 1 To 3
            .TransmitTerminalKey rcIBMF3Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Next i
       
    'sort master school list array by school code
        Dim tSchListMstr(1 To 2) As String
        
        For m = 1 To ms - 1
            For j = m + 1 To ms
                If SchListMstr(1, m) > SchListMstr(1, j) Then
                    
                    tSchListMstr(1) = SchListMstr(1, j)
                    tSchListMstr(2) = SchListMstr(2, j)
                    
                    SchListMstr(1, j) = SchListMstr(1, m)
                    SchListMstr(2, j) = SchListMstr(2, m)
                    
                    SchListMstr(1, m) = tSchListMstr(1)
                    SchListMstr(2, m) = tSchListMstr(2)
                                        
                End If
            Next j
        Next m
        
    'print the report
        Dim Word As Word.Application
        Set Word = CreateObject("Word.Application")
        With Word
            'open a blank Word doc
            .Documents.Add DocumentType:=wdNewBlankDocument
            .Visible = True
            'enter header
            .Selection.TypeText Text:="Historical Chronic Delinquents -- " & Format(Date, "MM/DD/YYYY")
            .Selection.TypeParagraph
            .Selection.TypeParagraph
            'enter totals
            .Selection.TypeText Text:="Borrowers Meeting Special Campaign Characteristics:" & vbTab & vbTab & vbTab & Format(n, "###,##0")
            .Selection.TypeParagraph
            .Selection.TypeText Text:="Borrowers at Target Schools in Repayment:" & vbTab & vbTab & vbTab & vbTab & vbTab & Format(ol, "###,##0")
            .Selection.TypeParagraph
            .Selection.TypeText Text:="Percentage of Chronically Delinquent Borrowers at All Target Schools:" & vbTab & Format(n / ol * 100, "##0.00") & " %"
            'enter detail for each school
            For i = 1 To ms
                nx = 0
                olx = 0
                'calc number of borrowers in Nelnet file for the school
                For j = 1 To ns
                    If SchListN(2, j) = SchListMstr(1, i) Then nx = nx + 1
                Next j
                'calc number of borrowers in SAS file for the school
                For j = 1 To ols
                    If SchListOL(2, j) = SchListMstr(1, i) Then olx = olx + 1
                Next j
                'print data
                .Selection.TypeParagraph
                .Selection.TypeParagraph
                .Selection.TypeText Text:=SchListMstr(1, i) & " - " & SchListMstr(2, i)
                .Selection.TypeParagraph
                .Selection.TypeText Text:="Borrowers Meeting Special Campaign Characteristics:" & vbTab & vbTab & vbTab & Format(nx, "###,##0")
                .Selection.TypeParagraph
                .Selection.TypeText Text:="Borrowers at Target School in Repayment:" & vbTab & vbTab & vbTab & vbTab & vbTab & Format(olx, "###,##0")
                .Selection.TypeParagraph
                .Selection.TypeText Text:="Percentage of Chronically Delinquent Borrowers at Target School:" & vbTab & vbTab & Format(nx / olx * 100, "##0.00") & " %"
            Next i
                
        End With
        'delete SAS file
        Kill "X:\PADD\FTP\" & Dir("X:\PADD\FTP\ULWM01.LWM01R2.*.*")
    End With
End Sub


