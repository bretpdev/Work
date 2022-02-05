Attribute VB_Name = "DPA"
Private counter As Integer
Private amtno As Double
Private batch_total As Double
Private junk As String
Private ID As String
Private Amt As String
Private PMTSFile As String
Private batchID As String
Private Data() As String
Private ttlpostamt As String
Private ttlpmts As Double
    
Public Sub DPAInput()
    Dim X As Integer
    Dim row As Integer
    
    ttlpostamt = InputBox("This is the DPA Posting Script.  Enter the total posting amount below and press OK to continue.", "DPA Posting")
    If ttlpostamt = "" Then End
    ReDim Data(1, 0) As String
    If Dir("T:\dpaerror.csv") > "" Then
        Kill "T:\dpaerror.csv"
    End If
    If Sp.Common.TestMode Then
        PMTSFile = "X:\PADD\FTP\TEST\PMTS.CSV"
    Else
        PMTSFile = "Q:\ACH\pmts.csv"
    End If
    'check if file exists
    If Dir(PMTSFile) <> "" Then
        Open PMTSFile For Input As #1
    Else
        MsgBox "The DPA file is missing.  Please contact Systems Support for assistance."
        End
    End If
    'check if file empty
    If FileLen(PMTSFile) < 1 Then
        MsgBox "The DPA file is empty.  Please contact Systems Support for assistance."
        End
    End If
    'check if file is old
    If FileDateTime(PMTSFile) < Date - 4 Then
        MsgBox "This file is too old.  Please contact Systems Support for assistance."
        End
    End If
    
    With Application
        batch_total = 0
        counter = 0
        Do While Not EOF(1)
            Input #1, junk, ID, junk, Amt, junk, d5, junk, junk, junk
            amtno = val(Amt)
            If ID <> "Participant ID" Then
                Do While Len(ID) < 10
                    ID = "0" & ID
                Loop
                ReDim Preserve Data(1, UBound(Data, 2) + 1)
                Data(0, UBound(Data, 2)) = ID
                Data(1, UBound(Data, 2)) = Amt
                batch_total = batch_total + amtno
                counter = counter + 1
            End If
        Loop
        
        While Format(batch_total, "########0.00") <> Format(ttlpostamt, "########0.00")
            ttlpostamt = InputBox("The total amount entered does not match the totals in the posting file.  Reenter the amount below and press OK to continue.", "DPA Posting")
            If ttlpostamt = "" Then End
        Wend
        
        Sp.Q.FastPath "LC38A" & Format(Date, "MMDDYYYY")
        Sp.Q.puttext 9, 32, Format(counter, "0000000")
        Sp.Q.puttext 9, 42, Format(batch_total, "00000000000.00")
        .TransmitTerminalKey rcIBMTabKey
        .TransmitANSI "BR"
        batchID = Sp.Q.GetText(9, 6, 12)
        Sp.Q.hit "ENTER"
        
        X = 0
        Do While Sp.Q.check4text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            Sp.Q.hit "F8"
        Loop
        For X = 0 To 12
            If Sp.Q.check4text(9 + X, 2, "_") = False Then
                batchID = Sp.Q.GetText(8 + X, 6, 13)
                Sp.Q.puttext 8 + X, 2, "X", "ENTER" 'select the new Batch ID
                Exit For
            End If
        Next X
               
        row = 9
        For X = 1 To UBound(Data, 2)
         
            Sp.Q.puttext row, 2, Data(0, X)
            Sp.Q.puttext row, 18, Data(1, X)
            Sp.Q.puttext row, 34, Format(Date, "MMDDYYYY"), "ENTER"
            If Sp.Q.check4text(22, 3, "44068") Then
                Sp.Q.hit "ENTER"
            ElseIf Sp.Q.check4text(22, 3, "40021") Then
                Open "T:\dpaerror.csv" For Append As #2
                If FileLen("T:\dpaerror.csv") = 0 Then
                    Write #2, "DPA Posting Error Report", ""
                    Write #2, "", ""
                    Write #2, "Account ID", "Amount"
                End If
                Write #2, Data(0, X) & "^", Data(1, X)
                Close #2
                Sp.Q.puttext row, 2, "          "
                row = row - 1
            End If
            
            row = row + 1
            If row = 21 Then row = 9
        Next X
        
        Sp.Q.hit "F12"
        Do While Sp.Q.check4text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            Sp.Q.hit "F8"
        Loop
        Session.FindText batchID, 1, 1
        Sp.Q.puttext Session.FoundTextRow, Session.FoundTextColumn - 4, "X"
        Sp.Q.hit "F2"
        If Sp.Q.check4text(22, 3, "44034") Then
            'totals verified
            MsgBox "Batch entry is complete and batch totals have been verified."
        Else
            'totals not verified
            MsgBox "Batch entry is complete, however batch totals do NOT match. Please review the error report and correct manually."
        End If
        
    'display the error file if it exists
    If Dir("T:\dpaerror.csv") <> "" Then ShowErrors
    
    End With
    Close #1
End Sub

Private Sub ShowErrors()
    Dim ExcelApp As excel.Application
    Set ExcelApp = CreateObject("Excel.Application")
    ExcelApp.Visible = True
    ExcelApp.Workbooks.Open ("T:\dpaerror.csv")
End Sub
