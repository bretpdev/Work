Attribute VB_Name = "TILPSpecHistCmts"
Option Explicit

Dim FTPDir As String
Dim UID As String

Sub Main()
    If MsgBox("This is the TILP Special History Comment Add To COMPASS script.  To continue click OK.", vbInformation + vbOKCancel) <> vbOK Then End
    SP.Common.TestMode FTPDir
    UID = SP.Common.GetUserID()
    'check for both files and let user know if either or both are missing
    If Dir(FTPDir & "TilpAAC.R3.txt") = "" And Dir(FTPDir & "Total Teaching Credit*") = "" Then
        'no files to process
        MsgBox "Neither expected file could be found.  Please fix the problem and try again.", vbInformation
        End
    ElseIf Dir(FTPDir & "TilpAAC.R3.txt") = "" Then
        'let the user know that the "TilpAAC.R3.txt" file couldn't be found
        MsgBox "The ""TilpAAC.R3.txt"" file couldn't be found to be processed.  The script will process all files it can find to process.", vbInformation
    ElseIf Dir(FTPDir & "Total Teaching Credit*") = "" Then
        'let the user know that the "Total Teaching Credit*" file can't be found to be processed
        MsgBox "The ""Total Teaching Credit*"" file couldn't be found to be processed.  The script will process all files it can find to process.", vbInformation
    End If
    If Dir(FTPDir & "TilpAAC.R3.txt") <> "" Then ProcessFile "CACTS", Dir(FTPDir & "TilpAAC.R3.txt")
    If Dir(FTPDir & "Total Teaching Credit*") <> "" Then ProcessFile "OTLTC", Dir(FTPDir & "Total Teaching Credit*")
    MsgBox "Processing Complete!", vbExclamation
    End
End Sub

'do processing
Sub ProcessFile(ARC As String, FileToProc As String)
    Dim ssn As String
    Dim Data As String
    'open file
    Open FTPDir & FileToProc For Input As #1
    If ARC = "CACTS" Then
        'header row
        Input #1, ssn, Data
    End If
    'process file
    While Not EOF(1)
        'get data
        Input #1, ssn, Data
        If ARC = "OTLTC" Then Data = Format(Data, "000000.00")
        TILPTD22ByBal ssn, ARC, Data, "TILPSPCHIS", UID
    Wend
    Close #1
End Sub

Function TILPTD22ByBal(ssn As String, ARC As String, comment As String, Script As String, UserID As String, Optional PauPls As Boolean = False) As Boolean
    Dim row As Integer
    Dim found As Boolean
    Dim k As Integer
    TILPTD22ByBal = True
    
    FastPath "TX3Z/ATD22" & ssn
    If Not Check4Text(1, 72, "TDX23") Then
        TILPTD22ByBal = False
        Exit Function
    End If
    
    'add activity record if borrower found
    If Check4Text(1, 72, "TDX23") Then
       
        'find the ARC
        Do
            found = Session.FindText(ARC, 8, 8)
            If found Then Exit Do
            Hit "F8"
            If Check4Text(23, 2, "90007") Then
                TILPTD22ByBal = False
                Exit Function
            End If
        Loop
        'select the ARC
        puttext Session.FoundTextRow, Session.FoundTextColumn - 5, "01", "ENTER"
        'exit the function if the selection screen is not displayed
        If Not Check4Text(1, 72, "TDX24") Then Exit Function
        
        'review each loan for selection
        row = 11
        Do
            If CDbl(GetText(row, 68, 10)) > 0 And Not Check4Text(row, 78, "-") And Check4Text(row, 61, "TILP") Then puttext row, 3, "X"
            row = row + 1
    
            If Not Check4Text(row, 3, "_") Then
                If Check4Text(8, 75, "+") Then
                    Hit "F8"
                    Do While Check4Text(23, 2, "03483")
                        'bypass loan because 03483 SELECTED LOAN(S) IN DDB STATUS; CAN NOT BE ASSOCIATED TO ACTY REQUEST
                        Hit "END"
                        Hit "F8"
                    Loop
                    row = 11
                Else
                    Exit Do
                End If
            End If
            
        Loop
        
        'enter short comments
        If Len(comment) < 132 Then
            puttext 21, 2, comment & "  {" & Script & "} /" & UserID
            
            'commit changes and handle errors
            Hit "ENTER"
            While Not Check4Text(23, 2, "02860")
                MsgBox "An error was recieved while adding a " & ARC & " activity record in Compass.  Please correct the error and then hit <Insert> to continue.", 48, "Activity Record Error"
                SP.PauseForInsert
                Hit "ENTER"
            Wend
        'enter long comments
        Else
            'fill the first screen, commit changes, and handle errors
            puttext 21, 2, MID(comment, 1, 154), "ENTER"
            While Not Check4Text(23, 2, "02860")
                MsgBox "An error was recieved while adding a " & ARC & " activity record in Compass.  Please correct the error and then hit <Insert> to continue.", 48, "Activity Record Error"
                SP.PauseForInsert
                Hit "ENTER"
            Wend
            Hit "F4"
            'enter the rest on the expanded comments screen
            For k = 155 To Len(comment) Step 260
                Session.TransmitANSI MID(comment, k, 260)
            Next
            Session.TransmitANSI "  {" & Script & "} /" & UserID
            
            'commit changes and handle errors
            Hit "ENTER"
            While Not Check4Text(23, 2, "02114")
                MsgBox "An error was recieved while adding a " & ARC & " activity record in Compass.  Please correct the error and then hit <Insert> to continue.", 48, "Activity Record Error"
                SP.PauseForInsert
                Hit "ENTER"
            Wend
        End If
    End If
End Function
