Attribute VB_Name = "RepurchasePosting"
Option Explicit
Dim FTPDir As String
Dim LogDir As String
Dim docDir As String
Type RecoveryData
    BatchCreated As String
    TotalsConfirmed As String
    RLine As String
End Type
Dim R29BondID As String
Dim BondIDTableInfo() As String


Sub Main()
    Dim R27FileTotal As Double
    Dim R27FileCount As Long
    Dim R27FileBorrCount As Long
    Dim R27FileDate As String
    Dim NelnetCount As Long
    Dim NelnetBorrCount As Long
    Dim NelnetTotal As Double
    Dim uheaaCount As Long
    Dim UHEAABorrCount As Long
    Dim UHEAATotal As Double
    Dim UserInputR29 As String
    Dim FileInProc As String
    Dim TotalsMatch As Boolean
    Dim userName As String
    Dim RD As RecoveryData
    
    If MsgBox("This script will post RP - repurchase payment.  Please click cancel to end the script.", vbOKCancel + vbInformation) <> vbOK Then End
    docDir = "X:\PADD\Operational Accounting\"
    Sp.Common.TestMode FTPDir, docDir, LogDir
    'get user name
    FastPath "LP40I"
    hit "Enter"
    userName = GetText(5, 14, 1) & LCase(GetText(5, 15, 34)) & " " & GetText(4, 14, 1) & LCase(GetText(4, 15, 34))
    'check for file
    If Dir(FTPDir & "ULWRP1.LWRP1R27*") = "" Then
        MsgBox "The script was unable to locate a SAS data file.", vbCritical
        End
    End If
    FileInProc = Dir(FTPDir & "ULWRP1.LWRP1R27*")
    'check if there is any data in the file
    If FileLen(FTPDir & FileInProc) = 0 Then
        MsgBox "The data file appears to be empty today.  Processing Complete!", vbInformation
        End
    End If
    'get total amount from R27
    R27Totals FileInProc, R27FileTotal, R27FileCount, R27FileBorrCount, R27FileDate, NelnetCount, NelnetBorrCount, uheaaCount, UHEAABorrCount, NelnetTotal, UHEAATotal
    'check if the script is in recovery
    If Dir(LogDir & "Repurchase Posting Log.txt") <> "" Then
        Open LogDir & "Repurchase Posting Log.txt" For Input As #5
        Input #5, RD.TotalsConfirmed, RD.BatchCreated, RD.RLine
        Close #5
    Else
        RD.TotalsConfirmed = "False"
        RD.BatchCreated = "False"
        RD.RLine = ""
    End If
    
    'prompt user for bond id
    Do
        'prompt user for bond id
        R29BondID = InputBox("Enter the bond ID that appears at the top of the ULWRP1.LWRP1R29 report.", "Enter Bond ID")
        If R29BondID = "" Then End
        
        'get info from BSYS table
        BondIDTableInfo = Sp.SqlEx("SELECT Bond_ID, Loan_Account FROM GENR_LST_BondAccountConversion WHERE Other = '" & R29BondID & "'")
        
        'warn user if info not found
        If UBound(BondIDTableInfo) = 0 Then
            If MsgBox("The bond ID entered was not found in the bond ID/account conversion table in BSYS.  Do you want to reenter the bond ID?  Click Yes to reenter the bond ID or click No to end the script.", vbYesNo + vbQuestion, "Bond ID not Found") = vbNo Then End
        Else
            Exit Do
        End If
    Loop
    
    If CBool(RD.TotalsConfirmed) = False Then 'recovery check
        'get information from user
        While TotalsMatch = False
            'get R29 total from user
            While IsNumeric(UserInputR29) = False
                UserInputR29 = InputBox("Please enter the total amount from R29 file.", "Enter R29 Total", "0")
                If IsNumeric(UserInputR29) = False Then
                    If MsgBox("That was not a valid entry.  Would you like to end running the script at this time.", vbInformation + vbYesNo) = vbYes Then End
                End If
            Wend
            
            'warn user if amounts don't match
            If CStr(CDbl(UserInputR29)) <> CStr(R27FileTotal) Then
                MsgBox "The amoun4t you entered from the R29 file doesn't match the total amount from R27.  Please try again.", vbCritical
                UserInputR29 = ""
            Else
                'the loop can end because the totals equal each other
                TotalsMatch = True
            End If
        Wend
        'update recovery log
        Open LogDir & "Repurchase Posting Log.txt" For Output As #5
        Write #5, "True", "False", ""
        Close #5
    End If
    'do posting functionality
    DoThePostingThing R27FileTotal, R27FileCount, FileInProc, RD
    'create request for funds transfer documents
    FundForms "UHEAA Repurchases", Format(UHEAATotal, "$###,###,##0.00"), userName
    'delete SAS file
    Kill FTPDir & FileInProc
    'print error report
    If Dir("T:\Repurchase Posting Error Rpt.txt") <> "" Then
        'if the error file exists then print report
        Sp.Common.PrintDocs docDir, "Repurchase Posting Errors", "T:\Repurchase Posting Error Rpt.txt"
        Kill "T:\Repurchase Posting Error Rpt.txt"
    End If
    'delete log file
    Kill LogDir & "Repurchase Posting Log.txt"
    MsgBox "Processing Complete!  Please pick up Wire Transfer Memo, reports and/or screen prints from printer.", vbInformation, "Processing Complete"
    End
End Sub

'this function does the posting thing
Function DoThePostingThing(amount As Double, count As Long, FIP As String, RD As RecoveryData)
    Dim row As Integer
    Dim RowEntry As Integer
    Dim Rec As String
    Dim HeaderRec As String
    Dim batchID As String
    Dim BatchIDSubPart As String
    If CBool(RD.BatchCreated) = False Then 'recovery check
        FastPath "LC38A"
        hit "Enter"
        'enter batch count
        puttext 9, 32, Format(count, "000000#")
        'enter batch total
        puttext 9, 42, Format(amount, "000,000,000.00")
        'enter Batch Type
        puttext 9, 72, "RP", "Enter"
        'update recovery log
        Open LogDir & "Repurchase Posting Log.txt" For Output As #5
        Write #5, "True", "True", ""
        Close #5
    End If
    'search for the batch to load
    FastPath "LC38C"
    hit "Enter"
    row = 9
    While check4text(row, 72, "RP") = False Or CInt(GetText(row, 32, 7)) <> count Or Round(CDbl(GetText(row, 42, 15)), 2) <> Round(amount, 2)
        If check4text(row, 2, "_") = False Then
            row = 9
            hit "F8"
        Else
            row = row + 1
        End If
    Wend
    puttext row, 2, "X", "Enter" 'select batch that was just added
    'get batch id parts
    batchID = GetText(3, 13, 8)
    BatchIDSubPart = GetText(3, 21, 4)
    'open file and enter all data into the batch
    Open FTPDir & FIP For Input As #1
    Line Input #1, HeaderRec 'get header row
    RowEntry = 9
    If RD.RLine <> "" Then 'recovery check
        'search for the last record successfully processed
        While Rec <> RD.RLine
            Line Input #1, Rec 'get data row
        Wend
        'search for the last system line not used to enter a record
        While check4text(RowEntry, 2, "_") = False
            RowEntry = RowEntry + 1
            If RowEntry = 21 Then
                RowEntry = 9
                hit "F8"
            End If
        Wend
    End If
    While Not EOF(1)
        Line Input #1, Rec 'get data row
        If RowEntry = 21 Then
            RowEntry = 9
        End If
        puttext RowEntry, 2, CStr(Split(Rec, ",")(0)) 'acct num
        puttext RowEntry, 18, Format(Split(Rec, ",")(1), "0,000,000.00") 'amount
        puttext RowEntry, 34, Format(CDate(Split(Rec, ",")(2)), "MMDDYYYY") 'effective date
        puttext RowEntry, 51, CStr(Split(Rec, ",")(3)) 'claim ID
        puttext RowEntry, 58, CStr(Split(Rec, ",")(4)) 'lender ID
        hit "Enter"
        'hit enter again if needed
        If check4text(22, 3, "40601") Then hit "Enter"
        'if the record wasn't accepted then write out to an error report
        If check4text(22, 3, "49000 DATA SUCCESSFULLY UPDATED") = False Then
            'add record to error report
            Open "T:\Repurchase Posting Error Rpt.txt" For Append As #2
            'if the file is empty then write out a header row
            If FileLen("T:\Repurchase Posting Error Rpt.txt") = 0 Then
                Write #2, "BF_SSN", "REP_AMT", "DT_REPUR", "LF_CLM_ID", "REP_LDR"
            End If
            Print #2, Rec 'write out data rec
            Close #2
            'blank last row's data because it didn't take (must do in case the last row entered was the last in the file)
            puttext RowEntry, 2, "", "End"
            puttext RowEntry, 5, "", "End"
            puttext RowEntry, 10, "", "End"
            puttext RowEntry, 18, "", "End"
            puttext RowEntry, 34, "", "End"
            puttext RowEntry, 51, "", "End"
            puttext RowEntry, 58, "", "End"
            'Hit "F5" 'refresh screen
            RowEntry = RowEntry - 1 'to counter act the incrementation just after the if then statement
        End If
        'update recovery log
        Open LogDir & "Repurchase Posting Log.txt" For Output As #5
        Write #5, "True", "True", Rec
        Close #5
        RowEntry = RowEntry + 1
    Wend
    Close #1
    'check batch balances
    FastPath "LC38C" & batchID & ";" & BatchIDSubPart
    puttext 9, 2, "X", "F2"
    'if error message other than all is well appears then do screen print
    If check4text(22, 3, "44034 BATCH TOTALS ARE VERIFIED") = False Then
        'print screen of issue if the batch didn't balance
        Session.TransmitTerminalKey rcIBMPrintKey
    Else
        'print batch totals if everything balanced
        FastPath "LC90I" & batchID & ";;" & BatchIDSubPart
        Session.TransmitTerminalKey rcIBMPrintKey
    End If
End Function

'the function sums the dollar amounts from the R27 repurchase file
Function R27Totals(FIP As String, R27FileTotal As Double, R27FileCount As Long, R27FileBorrCount As Long, R27FileDate As String, NC As Long, NBC As Long, UC As Long, UBC As Long, NT As Double, UT As Double)
    Dim Rec As String
    Dim LastSSN As String
    Open FTPDir & FIP For Input As #1
    Line Input #1, Rec 'get header row
    While Not EOF(1)
        Line Input #1, Rec 'get data row
        'amount total
        R27FileTotal = R27FileTotal + CDbl(Split(Rec, ",")(1))
        'record count
        R27FileCount = R27FileCount + 1
        'get UHEAA and Nelnet loan counts
        If CStr(Split(Rec, ",")(4)) = "828476" Then
            UC = UC + 1
            UT = UT + CDbl(Split(Rec, ",")(1))
        Else
            NC = NC + 1
            NT = NT + CDbl(Split(Rec, ",")(1))
        End If
        'borrower count
        If LastSSN <> CStr(Split(Rec, ",")(0)) Then
            R27FileBorrCount = R27FileBorrCount + 1
            'get UHEAA and Nelnet borrower counts
            If CStr(Split(Rec, ",")(4)) = "828476" Then
                UBC = UBC + 1
            Else
                NBC = NBC + 1
            End If
            LastSSN = CStr(Split(Rec, ",")(0))
        End If
        'repurchase file date
        If R27FileDate = "" Then
            R27FileDate = Format(CDate(Split(Rec, ",")(2)), "MM/DD/YYYY")
        End If
    Wend
    Close #1
    R27FileTotal = Round(R27FileTotal, 2)
End Function

'create data file and print docs
Sub FundForms(TitleVar As String, DocAmt As Double, RequestedBy As String)
    Dim i As Integer
    'create wire transfer memo
    Open "T:\Daily Disb Funds Transfer dat.txt" For Output As #1
        Write #1, "DeadLineDate", "DeadLineTime", "Program", "From", "To", "Purpose", "Desc", "Amt", "ReqBy", "CurrDatAndTime"
        Write #1, Format(Date, "MM/DD/YYYY"), "12:00 PM", "Loan Purchase Program (LPP)", "LPP Trustee-" & BondIDTableInfo(0, 0) & " Loan Account " & BondIDTableInfo(1, 0), "PTIF 4261 - #2 UHEAA Guarantee Fund", "Repurchases", "Transfer for " & Format(Date, "MM YYYY") & " " & TitleVar, Format(DocAmt, "$#,###,##0.00"), RequestedBy, CStr(Now)
    Close #1
    'create 2 copies of each report
    For i = 1 To 2
        Sp.Common.PrintDocs docDir, "FUNDSTRAN", "T:\Daily Disb Funds Transfer dat.txt"
    Next
    Kill "T:\Daily Disb Funds Transfer dat.txt"
End Sub
