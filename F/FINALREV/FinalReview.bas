Attribute VB_Name = "FinalReview"
Private ssn As String           'target SSN
Private AN As String
Private BSSN As String          'borrower SSN
Private firstName As String
Private lastName As String
Private address1 As String
Private address2 As String
Private city As String
Private state As String
Private zip As String
Private country As String
Private Phone As String
Private AltPhone As String
Private email As String
Private EndCo As Boolean

Private UserID As String
Private SkipStart As String
Private SkipType As String
Private TaskAdded As Boolean
Private TaskNeededOL As Boolean
Private SkipSystem As String
Private RefIDs() As String

Private TenthSSN As Integer
Private DocFolder As String
Private LogFolder As String
Private rSSN As String
Private rSta As Integer

Private CCC As String
Private ACSPartCd As String
Private ToPrinter As Boolean

Private School As String
Private SchName As String
Private SchAdd As String
Private SchAdd2 As String
Private SchAdd3 As String
Private SchCity As String
Private SchST As String
Private SchCountry As String
Private SchZip As String
Private OPEStatus As String
Private SSNList As String

'recovery statuses
Enum Status
    All = 0
    KORGLNDR = 1    'original lender
    KSCHLLTR = 2    'school contact
    KENDORSR = 3    'endorser/comaker call
    BRWRCALs = 4    'borrower call
    ACURINT2 = 5    'Accurint
    KRDTRQST = 6    'credit report
    KREALEDA = 7    'Real EDA
    KINTRNET = 8    'Accurint
    ref_rev = 9
    LP50rec = 10
    KRREFrec = 11
    letters = 12
End Enum

Sub Main()
    Common.ResetPublicVars
    Dim Que As String
    Dim Q As Integer

    'warn the user of the purpose of the script and end if the dialog box is canceled
    If Not SP.Common.CalledByMBS Then If MsgBox("This script reviews and completes FINALRVW queue tasks.  Click OK to continue or Cancel to quit.", 33, "OneLINK Final Review") = 2 Then End
    
    'determine folder for REFRCAL letter
    DocFolder = "X:\PADD\Skip\"
    If SP.Common.TestMode(, DocFolder, LogFolder) Then ToPrinter = False Else ToPrinter = True
    
    'determine recovery state
    If Dir(LogFolder & "finrevlog.txt") <> "" Then
        Open LogFolder & "finrevlog.txt" For Input As #2
        Input #2, rSSN, rSta, TenthSSN
        Close #2
        If Dir("T:\finrev.txt") = "" Then
            Open "T:\finrev.txt" For Output As #1
            Close #1
        End If
        If Dir("T:\frschltrs.txt") = "" Then
            Open "T:\frschltrs.txt" For Output As #3
            Close #3
        End If
    Else
        'set recovery values
        rSSN = ""
        rSta = Status.All
        TenthSSN = 0
        
        'create new merge text file
        Open "T:\finrev.txt" For Output As #1
        Close #1
        Open "T:\frschltrs.txt" For Output As #3
        Close #3
    End If

    'get userID
    UserID = SP.Common.GetUserID

    'process tasks in FINALRVW and KFINALRV queues
    For Q = 1 To 2
        'if in recovery, check if the queue task has already been completed
        If Dir(LogFolder & "finrevcomplete.txt") <> "" Then
            Exit For
        End If
        
        If Q = 1 Then FastPath "LP9AC" & "FINALRVW" Else FastPath "LP9AC" & "KFINALRV"

        'warn the user and end the script if the user is assigned to a task in another queue
        If Not Check4Text(1, 9, "FINALRVW") And Not Check4Text(1, 9, "KFINALRV") Then
            MsgBox "You are currently assigned to a task in another queue.  That task must be completed before running this script.", 48, "Incomplete Task"
            End
        End If
        
        'process tasks if there are any
        If Check4Text(1, 67, "WORKGROUP TASK") Or Check4Text(1, 71, "QUEUE TASK") Then
            'process FINALRVW tasks
            Do
                'initialze variables
                TaskAdded = False
                TaskNeededOL = False
                ReDim RefIDs(4, 0) As String
                EndCo = False
                
                'get task information
                If Check4Text(1, 9, "FINALRVW") Then
                    Que = "FINALRVW"
                    BSSN = GetText(9, 61, 9)
                    ssn = GetText(5, 70, 9)
                    If BSSN <> GetText(5, 70, 9) Then EndCo = True Else EndCo = False
                Else
                    Que = "KFINALRV"
                    ssn = GetText(17, 70, 9)
                    BSSN = ssn
                    EndCo = False
                End If
                'set counter for follow up tasks
                If ssn <> rSSN Then
                    rSta = Status.All
                    TenthSSN = TenthSSN + 1
                End If
                
                'get demographic info
                AN = ""
                GetLP22 ssn, AN, lastName, , firstName, address1, address2, city, state, zip, country, , Phone, , , AltPhone, email
            
                'determine which path to take
                SelectSkipSystem
    
                'someday they may fix this but OneLINK won't see the activity record on LP9A unless you pause before adding it
                Wait "2"
                'add activity record
                If rSta < Status.LP50rec Then
                    If TaskAdded And EndCo Then
                        AddLP50 BSSN, "KUEFR", "FINALREV", "AM", "36", "additional skip events requested", , , ssn
                    ElseIf TaskNeededOL And EndCo Then
                        AddLP50 BSSN, "KUEFR", "FINALREV", "AM", "36", , , , ssn
                    ElseIf Not TaskAdded And Not TaskNeededOL And EndCo Then
                        AddLP50 BSSN, "KSEFR", "FINALREV", "AM", "36", , , , ssn
                    ElseIf TaskAdded And Not EndCo Then
                        AddLP50 ssn, "KUBFR", "FINALREV", "AM", "36", "additional skip events requested"
                    ElseIf TaskNeededOL And Not EndCo Then
                        AddLP50 ssn, "KUBFR", "FINALREV", "AM", "36"
                    ElseIf Not TaskAdded And Not TaskNeededOL And Not EndCo Then
                        AddLP50 ssn, "KSBFR", "FINALREV", "AM", "36"
                    End If
                    UpSta 10
                End If
                
                're-select and complete the task
                FastPath "LP9AC" & Que
                Hit "F6"
                
                'cancel the task if the skip table row has been deleted for the task
                If Check4Text(22, 3, "47461") Or Check4Text(22, 3, "49230") Or Check4Text(22, 3, "47460") Then
                    'access LP8Y
                    FastPath "LP8YCSKP" & ";" & Que & ";;" & ssn
                    'cancel the task if it is found
                    If Check4Text(1, 64, "QUEUE TASK DETAIL") Then
                        'make the task available
                        puttext 7, 33, "A", "F6"
                        'cancel the task
                        puttext 7, 33, "X", "F6"
                    End If
                 End If
                
                'add a KSKPREVW task for every twentieth (used to be tenth) FINALRVW task completed
                If TenthSSN = 20 Then
                    AddLP9O ssn, "KSKPREVW"
                    TenthSSN = 0
                End If
                
                'go to the next task if LP9A is displayed
                If Check4Text(1, 67, "WORKGROUP TASK") Or Check4Text(1, 71, "QUEUE TASK") Then
                    Hit "F8"
                    'exit if there are no more tasks
                    If Check4Text(22, 3, "46004") Or Check4Text(22, 3, "47450") Then Exit Do
                'access LP9A if it is not displayed
                Else
                    FastPath "LP9AC" & Que
                    'exit if there are no more tasks
                    If Not Check4Text(1, 67, "WORKGROUP TASK") And Not Check4Text(1, 71, "QUEUE TASK") Then Exit Do
                End If
            Loop
            
            'update recovery status
            ssn = ""
            UpSta 11
        End If
    Next Q
    
    Open LogFolder & "finrevcomplete.txt" For Output As #4
    Write #4, "Task Complete"
    Close #4

    ssn = ""
    
    Dim aData(1 To 20) As String
    'add an activity record for the references to which letters were sent
    If rSta < Status.letters Then
        If FileLen("T:\finrev.txt") > 0 Then
            Open "T:\finrev.txt" For Input As #1
            ssn = ""
            While rSSN <> "" And ssn <> rSSN
                Input #1, aData(1), aData(2), aData(3), aData(4), aData(5), aData(6), aData(7), aData(8), aData(9), aData(10), aData(11), aData(12), aData(13), aData(14), aData(15), ssn, aData(16), aData(17), aData(18), aData(19), aData(20)
            Wend
            While Not EOF(1)
                Input #1, aData(1), aData(2), aData(3), aData(4), aData(5), aData(6), aData(7), aData(8), aData(9), aData(10), aData(11), aData(12), aData(13), aData(14), aData(15), ssn, aData(16), aData(17), aData(18), aData(19), aData(20)
                SP.Common.AddLP50 ssn, "KRREF", "FINALREV", "LT", "27", , , True
                UpSta 11
            Wend
            Close #1
        End If
        UpSta 12
    End If
    
    'print letters if there are any, create merge text file backup, and delete merge text file
    If rSta < 13 Then   'TODO: Define this status number in the Status enum.
        If FileLen("T:\finrev.txt") > 0 Then
        
            'read records from data file, get cost center info, and write to merge text file
            Open "T:\finrev.txt" For Input As #1
            Open "T:\finrevprn.txt" For Output As #3
            Write #3, "RefName", "RefAddress1", "RefAddress2", "RefCity", "RefState", "RefZip", "RefCountry", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Country", "RefID", "ACSKeyline", "ACSPartCd", "AccountNumber", "State_Ind", "COST_CENTER_CODE"
            While Not EOF(1)
                Input #1, aData(1), aData(2), aData(3), aData(4), aData(5), aData(6), aData(7), aData(8), aData(9), aData(10), aData(11), aData(12), aData(13), aData(14), aData(15), aData(16), aData(17), aData(18), aData(19), aData(20), ssn
                GetCCC
                AN = ""
                SP.Common.GetLP22 ssn, AN
                Write #3, aData(1), aData(2), aData(3), aData(4), aData(5), aData(6), aData(7), aData(8), aData(9), aData(10), aData(11), aData(12), aData(13), aData(14), aData(15), aData(16), aData(20), ACSPartCd, AN, aData(18), CCC
            Wend
            Close
            'sort merge text file
            SortFileCCP "T:\finrevprn.txt", 19, 20
            'print letters
            Barcode2D.AddBarcodeAndStaticCurrentDate "T:\finrevprn.txt", "RefID", "REFRCAL"
            SP.CostCenterPrinting.Main DocFolder, "REFRCAL", "Skip Borrower Reference Letter", -1, "REFLT", "T:\finrevprn.txt", Time, "REFCALL", , ToPrinter
            'archive and delete files
            If Dir("T:\finrev.bak") <> "" Then Kill "T:\finrev.bak"
            'Name "T:\finrev.txt" As "T:\finrev.bak"
            Kill "T:\finrev.txt"
            Kill "T:\finrevprn.txt"
        End If
        UpSta 13
    End If
    
    'add activity records for letters if there are any
    If rSta <= 14 Then  'TODO: Define this status number in the Status enum.
        Dim SSN4 As String
        Dim Schls As String

        If FileLen("T:\frschltrs.txt") > 0 Then
            'create a comment file which lists each SSN in the schl ltr file only once
            Open "T:\frschltrs.txt" For Input As #3
            While Not EOF(3)
                Input #3, ssn, Queue, firstName, MI, lastName, address1, address2, city, state, zip, Phone, AltPhone, email, School
                'review the comment file to see if the SSN is already in it
                If Dir("T:\frschltrscom.txt") <> "" Then
                    Open "T:\frschltrscom.txt" For Input As #4
                    Do While Not EOF(4)
                        Input #4, SSN4
                        'go to the next SSN if the SSN is found
                        If SSN4 = ssn Then
                            Close #4
                            Exit Do
                        End If
                        'add the SSN to the comment file if it wasn't found in the comment file
                        If EOF(4) Then
                            Close #4
                            Open "T:\frschltrscom.txt" For Append As #4
                            Write #4, ssn
                            Close #4
                            Exit Do
                        End If
                    Loop
                'create a new file and add the SSN if there isn't a file already
                Else
                    Open "T:\frschltrscom.txt" For Output As #4
                    Write #4, ssn
                    Close #4
                End If
            Wend
            Close #3
            
            'add one activity record for each SSN in the comment file listing all of the schools for which the SSN appears in the schl ltr file
            ssn = ""
            Open "T:\frschltrscom.txt" For Input As #4
            While Not EOF(4) And ssn <> rSSN
                Input #4, ssn
            Wend
            SP.Common.Login
            While Not EOF(4)
                Schls = ""
                Input #4, ssn
                'find all of the schools in the print data file for the SSN and add them to the comment
                Open "T:\frschltrs.txt" For Input As #3
                Do Until EOF(3)
                    Input #3, SSN4, Queue, firstName, MI, lastName, address1, address2, city, state, zip, Phone, AltPhone, email, School
                    If ssn = SSN4 Then Schls = Schls & School & " "
                Loop
                Close #3
                
                'add activity record and update recovery status
                SP.Common.ATD22AllLoans ssn, "KLSLT", "letter(s) mailed to school(s): " & Schls & "to request verification of borrower's demographic information", "FINALREV", UserID
                UpSta 14
            Wend
            Close #4
            
            'delete comment file
            Kill "T:\frschltrscom.txt"
        End If
    End If

    'print school letters if there are any
    If rSta < 15 Then   'TODO: Define this status number in the Status enum.
        If FileLen("T:\frschltrs.txt") > 0 Then
            SchLettersProc
            If Dir("T:\frschltrs.bak") <> "" Then Kill "T:\frschltrs.bak"
            'Name "T:\frschltrs.txt" As "T:\frschltrs.bak"
            Kill "T:\frschltrs.txt"
        End If
        UpSta 15
    End If

    'delete temporary files and warn the user that processing is complete
    If Dir("T:\frschltrdatCover.txt") <> "" Then Kill "T:\frschltrdatCover.txt"
    If Dir("T:\frschltrdat.txt") <> "" Then Kill "T:\frschltrdat.txt"
    Kill LogFolder & "finrevlog.txt"
    Kill LogFolder & "finrevcomplete.txt"
    ProcComp "MBSFINALREV.TXT"
End Sub

'select the skip system
Sub SelectSkipSystem()
    Dim tSkipSystem As String
    Dim LoanStatus As String
    Dim ClaimDate As String
    Dim ClaimStatus As Boolean
    Dim row As Integer
    Dim CheckLC05 As Boolean
      
    CheckLC05 = False

    'get skip info
    FastPath "LP8QI" & ssn
    SkipStart = Format(GetText(5, 29, 8), "##/##/####")
    SkipType = GetText(5, 17, 1)
    
    'go to LG10
    FastPath "LG10I" & BSSN
    If Check4Text(1, 52, "LOAN BWR STATUS RECAP DISPLAY") Then
        If Not Check4Text(5, 27, "700126") Then
            SkipSystem = "OneLink"
        Else
            SkipSystem = GetSkipSystem
        End If
    Else
        row = 7
        While Not (Check4Text(20, 3, "46004") Or Check4Text(20, 3, "47004"))
            If Not Check4Text(row, 46, "700126") Then
                tSkipSystem = "OneLink"
            Else
                puttext 19, 15, GetText(row, 5, 2), "ENTER"
                tSkipSystem = GetSkipSystem
                Hit "F12"
            End If
            
            If tSkipSystem = "CheckLC05" Then
                CheckLC05 = True
            ElseIf tSkipSystem <> SkipSystem And SkipSystem <> "" Then
                SkipSystem = "Both"
            Else
                SkipSystem = tSkipSystem
            End If
            
            row = row + 1
            If GetText(row, 5, 2) = "" Then
                Hit "F8"
                row = 7
            End If
        Wend
    End If
    
    'check LC05
    If CheckLC05 Or SkipSystem = "CheckLC05" Then
        FastPath "LC05I" & ssn
        If Check4Text(1, 62, "DEFAULT/CLAIM RECAP") Then puttext 21, 13, "01", "ENTER"
        ClaimDate = Format(GetText(5, 13, 8), "##/##/####")
        Hit "F8"
        While Not Check4Text(22, 3, "46004")
            If DateValue(Format(GetText(5, 13, 8), "##/##/####")) > DateValue(ClaimDate) Then
                ClaimDate = Format(GetText(5, 13, 8), "##/##/####")
                If Check4Text(4, 10, "04") Or (Check4Text(4, 10, "03") And GetText(19, 73, 8) <> "") Then
                    ClaimStatus = False
                Else
                    ClaimStatus = True
                End If
            End If
            Hit "F8"
        Wend
        
        If ClaimStatus Then tSkipSystem = "Compass" Else tSkipSystem = "OneLink"
        
        If tSkipSystem <> SkipSystem And SkipSystem <> "CheckLC05" Then
            SkipSystem = "Both"
        Else
            SkipSystem = tSkipSystem
        End If
    End If
    
    'route to appropriate subroutine
    If SkipSystem = "OneLink" Then
        OneLink
    ElseIf SkipSystem = "Compass" Then
        Compass
    ElseIf SkipSystem = "Both" Then
        Compass
    End If
End Sub

'get skip system based on LG10 status
Function GetSkipSystem()
    Select Case GetText(11, 59, 2)
    Case "CP"
        GetSkipSystem = "CheckLC05"
    Case "AE", "AL", "CA", "DN", "PC", "PN", "PM", "PF", "UA", "UB", "UC", "UD", "UI"
        GetSkipSystem = "OneLink"
    Case Else
        GetSkipSystem = "Compass"
    End Select
End Function

Sub CloseKLSLTActivityComment(ssn As String)
    SP.Q.FastPath "TX3Z/CTD2A" & ssn
    SP.Q.puttext 11, 65, "KLSLT"
    SP.Q.puttext 21, 16, Format(Now, "MMDDYY")
    SP.Q.puttext 21, 30, Format(Now, "MMDDYY")
    SP.Q.puttext 7, 60, "X", "ENTER"
    If SP.Q.Check4Text(23, 2, "01019") Then Exit Sub 'there was a problem adding the activity record skip processing.
    If SP.Q.Check4Text(1, 72, "TDX2C") Then 'activity selection screen
        SP.Q.puttext 7, 2, "X", "ENTER"
    End If
    Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        If Replace(SP.Q.GetText(15, 2, 5), "_", "") = "" Then
            If OPEStatus = "C" Then
                SP.Q.puttext 15, 2, "INVAD", "ENTER"
            Else
                SP.Q.puttext 15, 2, "PRNTD", "ENTER"
            End If
        End If
        SP.Q.Hit "F8"
    Loop
End Sub

'process Compass skips
Sub Compass()
    Dim KORGLNDR As Integer     'original lender
    Dim KSCHLLTR As Integer     'school contact
    Dim KLSLTs As String        'schools for KLSLT ARC
    Dim ENDORSEu As Integer     'endorser/comaker call KAEAB or KUEAB
    Dim ENDORSEs As Integer     'endorser/comaker call KSEAB
    Dim BRWRCALa As Integer     'borrower call KABAB or KUBAB
    Dim BRWRCALs As Integer     'borrower call KSBAB
    Dim ACURINT2 As Integer     'DMV
    Dim KRDTRQST As Integer     'credit report
    Dim KREALEDA As Integer     'Real EDA
    Dim KINTRNET As Integer     'Internet
    Dim row As Integer
    Dim i As Integer
    
    'reset indicators
    KORGLNDR = 0
    KSCHLLTR = 0
    ENDORSEu = 0
    ENDORSEs = 0
    BRWRCALa = 0
    BRWRCALs = 0
    ACURINT2 = 0
    KRDTRQST = 0
    KREALEDA = 0
    KINTRNET = 0
    
''    'determine skip start date (greatest verified date of invalid info)
''    If Check4Text(7, 36, "INVALID") Then SkipStart = GetText(7, 56, 8) Else SkipStart = "01/01/1900"
''    If Check4Text(13, 73, "INVALID") And DateValue(GetText(13, 62, 8)) > DateValue(SkipStart) Then SkipStart = GetText(13, 62, 8)
''
''    'determine skip type
''    If Check4Text(7, 36, "INVALID") And Check4Text(13, 73, "VALID") Then
''        SkipType = "A"
''    ElseIf Check4Text(7, 36, "VALID") And Check4Text(13, 73, "INVALID") Then
''        SkipType = "P"
''    ElseIf Check4Text(7, 36, "INVALID") And Check4Text(13, 73, "INVALID") Then
''        SkipType = "B"
''    End If

    'address
    If SkipType = "A" Then
        'search TD2A for original lender and school contact
        FastPath "TX3Z/ITD2A" & ssn
        puttext 4, 66, "X", "Enter"
        If Check4Text(1, 72, "TDX2C") Then puttext 5, 14, "X", "Enter"
        Do While Check4Text(1, 72, "TDX2D")
            If DateValue(GetText(13, 31, 8)) < DateValue(SkipStart) Then Exit Do
            Select Case GetText(13, 2, 5)
            Case "S7LA9"
                KORGLNDR = KORGLNDR + 1
            Case "KLSLT", "KLSCH"
                If Check4Text(13, 2, "KLSCH") Then
                    KSCHLLTR = KSCHLLTR + 1
                ElseIf Check4Text(13, 2, "KLSLT") And KLSLTs = "" Then
                    For i = 17 To 22
                        KLSLTs = KLSLTs & GetText(i, 2, 79)
                    Next i
                End If
            End Select
            nextpage
            If Check4Text(23, 2, "90007") Then Exit Do
        Loop
        
        'search LP50 for endorser call, borrower call, DMV, credit report, DA, and Internet
        FastPath "LP50I" & ssn & ";;X"
        Do While Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY")
            If DateValue(Format(GetText(7, 15, 8), "##/##/####")) < DateValue(SkipStart) Then Exit Do
            Select Case GetText(8, 2, 5)
                Case "KAEAB", "KUEAB"
                    If EndCo Then ENDORSEu = ENDORSEu + 1
                Case "KSEAB"
                    If EndCo Then ENDORSEs = ENDORSEs + 1
                Case "KABAB", "KUBAB"
                    If Not EndCo Then BRWRCALa = BRWRCALa + 1
                Case "KSBAB"
                    If Not EndCo Then BRWRCALs = BRWRCALs + 1
                Case "KUBSS"
                    ACURINT2 = ACURINT2 + 1
                'Case "KSECR", "KUECR"
                '    If EndCo Then KRDTRQST = KRDTRQST + 1
                'Case "KSBCR", "KUBCR"
                '    If Not EndCo Then KRDTRQST = KRDTRQST + 1
                'Case "KFAMR", "KUBDA", "KSBDA", "KUBNL", "KFNOL", "KFNOP", "KFNOS", "KFSAM", "KFUPD", "KSBUP", "KSEUP", "KSFUP", "KUBNP", "KUBNS", "KUBSM", "KUENL", "KUENP", "KUENS", "KUESM", "KUFNL", "KUFNP", "KUFNS", "KUFSM"
                '    KREALEDA = KREALEDA + 1
                'Case "KSEIT", "KUEIT"
                '    If EndCo Then KINTRNET = KINTRNET + 1
                'Case "KUBIT", "KSBIT"
                '    If Not EndCo Then KINTRNET = KINTRNET + 1
            End Select
            Hit "F8"
            If Check4Text(22, 3, "46004") Then Exit Do
        Loop
        
        'add queue tasks for missing activities and update recovery
        If KORGLNDR = 0 And rSta < Status.KORGLNDR Then
            AddLP9O ssn, "KORGLNDR"
            UpSta 1
        End If
        If KSCHLLTR = 0 And rSta < Status.KSCHLLTR Then
            If Not SchLtrCrtd(KLSLTs) Then KSCHLLTR = 1
            UpSta 2
        End If
        If EndCo And (ENDORSEu < 2 Or ENDORSEa = 0) And rSta < Status.KENDORSR Then
            AddLP9O ssn, "KENDORSR"
            UpSta 3
        End If
        If (BRWRCALa < 2 Or BRWRCALs = 0) And rSta < Status.BRWRCALs Then
            AddLP9O ssn, "BRWRCALS"
            UpSta 4
        End If

        If ACURINT2 = 0 And rSta < Status.ACURINT2 Then
            AddLP9O ssn, "ACURINT2"
            UpSta 5
        End If
        'If KRDTRQST = 0 And rSta < Status.KRDTRQST Then
        '    AddLP9O SSN, "KRDTRQST"
        '    UpSta 6
        'End If
        'If KREALEDA = 0 And rSta < Status.KREALEDA Then
        '    AddLP9O SSN, "KREALEDA"
        '    UpSta 7
        'End If
        'If KINTRNET = 0 And rSta < Status.KINTRNET Then
        '    AddLP9O SSN, "KINTRNET"
        '    UpSta 8
        'End If
        
        'determine if a task was added to determine correct action code for LP50 activity record
        'If KORGLNDR = 0 Or KSCHLLTR = 0 Or KRDTRQST = 0 Or (EndCo And (ENDORSEu < 2 Or ENDORSEa = 0)) Or _
        '   (BRWRCALa < 2 Or BRWRCALs = 0) Or ACURINT2 = 0 Or KRDTRQST = 0 Or KREALEDA = 0 Or _
        '   KINTRNET = 0 Then TaskAdded = True
           
        If KORGLNDR = 0 Or KSCHLLTR = 0 Or (EndCo And (ENDORSEu < 2 Or ENDORSEa = 0)) Or _
           (BRWRCALa < 2 Or BRWRCALs = 0) Or ACURINT2 = 0 _
           Then TaskAdded = True
                    
        'review references to verify they were contacted
        If rSta < Status.ref_rev Then RefReview SkipType
            
    'phone
    ElseIf SkipType = "P" Then
        'search TD2A for school contact
        FastPath "TX3Z/ITD2A" & ssn
        puttext 4, 66, "X", "Enter"
        If Check4Text(1, 72, "TDX2C") Then puttext 5, 14, "X", "Enter"
        Do While Check4Text(1, 72, "TDX2D")
            'If DateValue(GetText(13, 31, 8)) < DateValue(SkipStart) Then Exit Do
            If Check4Text(13, 2, "KLSCH") Then
                KSCHLLTR = 1
            ElseIf Check4Text(13, 2, "KLSLT") And KLSLTs = "" Then
                For i = 17 To 22
                    KLSLTs = KLSLTs & GetText(i, 2, 79)
                Next i
            End If

            nextpage
            If Check4Text(23, 2, "90007") Then Exit Do
        Loop
        
        'search LP50 for DA and Internet
        FastPath "LP50I" & ssn & ";;X"
        Do While Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY")
            If DateValue(Format(GetText(7, 15, 8), "##/##/####")) < DateValue(SkipStart) Then Exit Do
            Select Case GetText(8, 2, 5)
            'Case "KFAMR", "KUBDA", "KSBDA", "KUBNL", "KFNOL", "KFNOP", "KFNOS", "KFSAM", "KFUPD", "KSBUP", "KSEUP", "KSFUP", "KUBNP", "KUBNS", "KUBSM", "KUENL", "KUENP", "KUENS", "KUESM", "KUFNL", "KUFNP", "KUFNS", "KUFSM"
            '    KREALEDA = KREALEDA + 1
            Case "KUBSS"
                ACURINT2 = ACURINT2 + 1
            'Case "KUBIT", "KSBIT"
            '    If Not EndCo Then KINTRNET = KINTRNET + 1
            End Select
            Hit "F8"
            If Check4Text(22, 3, "46004") Then Exit Do
        Loop
        
        'add queue tasks for missing activities
        If KSCHLLTR = 0 And rSta < Status.KSCHLLTR Then
            If Not SchLtrCrtd(KLSLTs) Then KSCHLLTR = 1
            UpSta 2
        End If

        If ACURINT2 = 0 And rSta < Status.ACURINT2 Then
            AddLP9O ssn, "ACURINT2"
            UpSta 5
        End If
        'If KREALEDA = 0 And rSta < Status.KREALEDA Then
        '    AddLP9O SSN, "KREALEDA"
        '   UpSta 7
        'End If
        'If KINTRNET = 0 And rSta < Status.KINTRNET Then
        '    AddLP9O SSN, "KINTRNET"
        '    UpSta 8
        'End If
        
        'determine if a task was added to determine correct action code for LP50 activity record
        'If KSCHLLTR = 0 Or KREALEDA = 0 Or KINTRNET = 0 Then TaskAdded = True
        If KSCHLLTR = 0 Or ACURINT2 = 0 Then TaskAdded = True
                    
        'review references to verify they were contacted
        If rSta < Status.ref_rev Then
            RefReview SkipType
        End If
        
    'both
    ElseIf SkipType = "B" Then
        'search TD2A for original lender and school contact
        FastPath "TX3Z/ITD2A" & ssn
        puttext 4, 66, "X", "Enter"
        If Check4Text(1, 72, "TDX2C") Then puttext 5, 14, "X", "Enter"
        Do While Check4Text(1, 72, "TDX2D")
            If DateValue(GetText(13, 31, 8)) < DateValue(SkipStart) Then Exit Do
            Select Case GetText(13, 2, 5)
            Case "S7LA9"
                KORGLNDR = KORGLNDR + 1
            Case "KLSLT", "KLSCH"
                If Check4Text(13, 2, "KLSCH") Then
                    KSCHLLTR = KSCHLLTR + 1
                ElseIf Check4Text(13, 2, "KLSLT") And KLSLTs = "" Then
                    For i = 17 To 22
                        KLSLTs = KLSLTs & GetText(i, 2, 79)
                    Next i
                End If
            End Select
            nextpage
            If Check4Text(23, 2, "90007") Then Exit Do
        Loop
        
        'search LP50 for DMV, credit report, DA, and Internet
        FastPath "LP50I" & ssn & ";;X"
        Do While Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY")
            If DateValue(Format(GetText(7, 15, 8), "##/##/####")) < DateValue(SkipStart) Then Exit Do
            Select Case GetText(8, 2, 5)
            Case "KUBSS"
                ACURINT2 = ACURINT2 + 1
            'Case "KSECR", "KUECR"
            '    If EndCo Then KRDTRQST = KRDTRQST + 1
            'Case "KSBCR", "KUBCR"
            '    If Not EndCo Then KRDTRQST = KRDTRQST + 1
            'Case "KFAMR", "KUBDA", "KSBDA", "KUBNL", "KFNOL", "KFNOP", "KFNOS", "KFSAM", "KFUPD", "KSBUP", "KSEUP", "KSFUP", "KUBNP", "KUBNS", "KUBSM", "KUENL", "KUENP", "KUENS", "KUESM", "KUFNL", "KUFNP", "KUFNS", "KUFSM"
            '    KREALEDA = KREALEDA + 1
            'Case "KSEIT", "KUEIT"
            '    If EndCo Then KINTRNET = KINTRNET + 1
            'Case "KUBIT", "KSBIT"
            '    If Not EndCo Then KINTRNET = KINTRNET + 1
            End Select
            Hit "F8"
            If Check4Text(22, 3, "46004") Then Exit Do
        Loop
        
        'add queue tasks for missing activities
        If KORGLNDR = 0 And rSta < Status.KORGLNDR Then
            AddLP9O ssn, "KORGLNDR"
            UpSta 1
        End If
        If KSCHLLTR = 0 And rSta < Status.KSCHLLTR Then
            If Not SchLtrCrtd(KLSLTs) Then KSCHLLTR = 1
            UpSta 2
        End If

        ''If ACURINT2 = 0 And rSta < Status.ACURINT2 Then
        ''    If PartDMV Then AddLP9O SSN, "ACURINT2" Else ACURINT2 = 1
        ''    UpSta 5
        ''End If

        If ACURINT2 = 0 And rSta < Status.ACURINT2 Then 'Added this
            AddLP9O ssn, "ACURINT2"
            UpSta 5
        End If
        'If KRDTRQST = 0 And rSta < Status.KRDTRQST Then
         '   AddLP9O SSN, "KRDTRQST"
         '   UpSta 6
        'End If
        'If KREALEDA = 0 And rSta < Status.KREALEDA Then
        '    AddLP9O SSN, "KREALEDA"
        '    UpSta 7
        'End If
        'If KINTRNET = 0 And rSta < Status.KINTRNET Then
        '    AddLP9O SSN, "KINTRNET"
        '    UpSta 8
        'End If
        
        'determine if a task was added to determine correct action code for LP50 activity record
        'If KORGLNDR = 0 Or KSCHLLTR = 0 Or ACURINT2 = 0 Or _
        '   KRDTRQST = 0 Or KREALEDA = 0 Or KINTRNET = 0 Then TaskAdded = True
           
        If KORGLNDR = 0 Or KSCHLLTR = 0 Or ACURINT2 = 0 _
            Then TaskAdded = True
                    
        'review references to verify they were contacted
        If rSta < Status.ref_rev Then RefReview SkipType
    End If
End Sub

'process OneLINK skips
Sub OneLink()
    Dim KSCHLLTR As Integer     'school contact
    Dim KLSLTs As String        'schools for KLSLT ARC
    Dim KRDTRQST As Integer     'credit report
    Dim BRWRCALa As Integer     'borrower call KABAB or KUBAB
    Dim BRWRCALs As Integer     'borrower call KSBAB
    Dim ACURINT2 As Integer     'DMV
    Dim KREALEDA As Integer     'Real EDA
    Dim KINTRNET As Integer     'Internet
    Dim row As Integer
    Dim i As Integer
    
    'reset indicators
    KSCHLLTR = 0
    KRDTRQST = 0
    BRWRCALa = 0
    BRWRCALs = 0
    ACURINT2 = 0
    KREALEDA = 0
    KINTRNET = 0

    'access LP8Q
    FastPath "LP8QI" & ssn
        
    'address
    If SkipType = "A" Then
        'search LP8Q for school contact and credit report
        row = 9
        Do While Not Check4Text(22, 3, "46004")
            Select Case GetText(row, 12, 8)
            Case "SCHLLTTR", "SCHLCALL"
                KSCHLLTR = KSCHLLTR + 1
            'Case "RCREDITB"
            '    KRDTRQST = KRDTRQST + 1
            End Select
            row = row + 2
            If row > 19 Or Check4Text(row, 7, " ") Then
                Hit "F8"
                row = 9
            End If
        Loop
        'search LP50 for borrower call, DMV, DA, and Internet
        FastPath "LP50I" & ssn & ";;X"
        Do While Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY")
            If DateValue(Format(GetText(7, 15, 8), "##/##/####")) < DateValue(SkipStart) Then Exit Do
            Select Case GetText(8, 2, 5)
            Case "KABAB", "KUBAB"
                BRWRCALa = BRWRCALa + 1
            Case "KSBAB"
                BRWRCALs = BRWRCALs + 1
            Case "KUBSS"
                ACURINT2 = ACURINT2 + 1
            'Case "KFAMR", "KUBDA", "KSBDA", "KUBNL", "KFNOL", "KFNOP", "KFNOS", "KFSAM", "KFUPD", "KSBUP", "KSEUP", "KSFUP", "KUBNP", "KUBNS", "KUBSM", "KUENL", "KUENP", "KUENS", "KUESM", "KUFNL", "KUFNP", "KUFNS", "KUFSM"
            '    KREALEDA = KREALEDA + 1
            'Case "KSEIT", "KUEIT"
            '    If EndCo Then KINTRNET = KINTRNET + 1
            'Case "KUBIT", "KSBIT"
            '    If Not EndCo Then KINTRNET = KINTRNET + 1
            End Select
            Hit "F8"
            If Check4Text(22, 3, "46004") Then Exit Do
        Loop
        
        'determine if a task was added to determine correct action code for LP50 activity record
        'If KSCHLLTR = 0 Or KRDTRQST = 0 Or (BRWRCALa < 2 Or BRWRCALs = 0) Or _
        '   ACURINT2 = 0 Or KREALEDA = 0 Or KINTRNET = 0 Then TaskNeededOL = True
           
        If KSCHLLTR = 0 Or (BRWRCALa < 2 Or BRWRCALs = 0) Or _
           ACURINT2 = 0 Then TaskNeededOL = True
                    
        'review references to verify they were contacted
        If rSta < Status.ref_rev Then RefReview SkipType
        
    'phone
    ElseIf SkipType = "P" Then
       'search TD2A for school contact
        FastPath "TX3Z/ITD2A" & ssn
        puttext 4, 66, "X", "Enter"
        Do While Check4Text(1, 72, "TDX2D")
            If DateValue(GetText(13, 31, 8)) < DateValue(SkipStart) Then Exit Do
            If Check4Text(13, 2, "KLSCH") Then
                KSCHLLTR = 1
            ElseIf Check4Text(13, 2, "KLSLT") And KLSLTs = "" Then
                For i = 17 To 22
                    KLSLTs = KLSLTs & GetText(i, 2, 79)
                Next i
            End If
            nextpage
            If Check4Text(23, 2, "90007") Then Exit Do
        Loop
        
        'search LP50 for DA and Internet
        FastPath "LP50I" & ssn & ";;X"
        Do While Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY")
            If DateValue(Format(GetText(7, 15, 8), "##/##/####")) < DateValue(SkipStart) Then Exit Do
            Select Case GetText(8, 2, 5)
            'Case "KFAMR", "KUBDA", "KSBDA", "KUBNL", "KFNOL", "KFNOP", "KFNOS", "KFSAM", "KFUPD", "KSBUP", "KSEUP", "KSFUP", "KUBNP", "KUBNS", "KUBSM", "KUENL", "KUENP", "KUENS", "KUESM", "KUFNL", "KUFNP", "KUFNS", "KUFSM"
            '    KREALEDA = KREALEDA + 1
            Case "KUBSS"
                ACURINT2 = ACURINT2 + 1
            'Case "KUBIT", "KSBIT"
            '    If Not EndCo Then KINTRNET = KINTRNET + 1
            End Select
            Hit "F8"
            If Check4Text(22, 3, "46004") Then Exit Do
        Loop
        
        'determine if a task was added to determine correct action code for LP50 activity record
        'If KSCHLLTR = 0 Or KREALEDA = 0 Or KINTRNET = 0 Then TaskNeededOL = True
        If KSCHLLTR = 0 Or ACURINT2 = 0 Then TaskNeededOL = True
                    
        'review references to verify they were contacted
        If rSta < Status.ref_rev Then RefReview SkipType
        
    'both
    ElseIf SkipType = "B" Then
        'search LP8Q for school contact and credit report
        row = 9
        Do While Not Check4Text(22, 3, "46004")
            Select Case GetText(row, 12, 8)
            Case "SCHLLTTR", "SCHLCALL"
                KSCHLLTR = KSCHLLTR + 1
            'Case "RCREDITB"
            '    KRDTRQST = KRDTRQST + 1
            End Select
            row = row + 2
            If row > 19 Or Check4Text(row, 7, " ") Then
                Hit "F8"
                row = 9
            End If
        Loop
    
        'search LP50 for DMV, DA, and Internet
        FastPath "LP50I" & ssn & ";;X"
        Do While Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY")
            If DateValue(Format(GetText(7, 15, 8), "##/##/####")) < DateValue(SkipStart) Then Exit Do
            Select Case GetText(8, 2, 5)
            Case "KUBSS"
                ACURINT2 = ACURINT2 + 1
            'Case "KFAMR", "KUBDA", "KSBDA", "KUBNL", "KFNOL", "KFNOP", "KFNOS", "KFSAM", "KFUPD", "KSBUP", "KSEUP", "KSFUP", "KUBNP", "KUBNS", "KUBSM", "KUENL", "KUENP", "KUENS", "KUESM", "KUFNL", "KUFNP", "KUFNS", "KUFSM"
            '    KREALEDA = KREALEDA + 1
            'Case "KSEIT", "KUEIT"
            '    If EndCo Then KINTRNET = KINTRNET + 1
            'Case "KUBIT", "KSBIT"
            '    If Not EndCo Then KINTRNET = KINTRNET + 1
            End Select
            Hit "F8"
            If Check4Text(22, 3, "46004") Then Exit Do
        Loop

        'determine if a task was added to determine correct action code for LP50 activity record
        'If KSCHLLTR = 0 Or KRDTRQST = 0 Or _
        '   ACURINT2 = 0 Or KREALEDA = 0 Or KINTRNET = 0 Then TaskNeededOL = True
           
        If KSCHLLTR = 0 Or ACURINT2 = 0 Then TaskNeededOL = True
                    
        'review references to verify they were contacted
        If rSta < Status.ref_rev Then RefReview SkipType
    End If
    
'Trent and I looked at this and it didn't make any sense so we deleted it and replaced it with the code below which matches the spec, jd
''    If SSNList = "" Then
''        SSNList = ssn + " "
''    End If
''
''    If ACURINT2 = 0 Then
''    Dim num As Integer
''        If (InStr(SSNList, ssn) = False) Then
''            SSNList = SSNList + ssn + " "
''            AddLP9O ssn, "ACURINT2"
''            UpSta 5
''        Else
''            num = num + 1
''        End If
''
''    End If

    If ACURINT2 = 0 Then
        AddLP9O ssn, "ACURINT2"
        UpSta 5
    End If
        
''    End If
End Sub

'select eligible references for review
Function RefReview(s As String)
    Dim row As Integer

    FastPath "LP2CI" & ssn
    'select eligible references if more than 1
    If Check4Text(1, 65, "REFERENCE SELECT") Then
        row = 7
        Do While Not Check4Text(22, 3, "46004")
            'select the review the reference if it is eligible
            If Check4Text(row, 27, "A") And Not Check4Text(row, 37, "04") Then
                puttext 21, 13, GetText(row - 1, 2, 2), "Enter"
                RefCheck s
                Hit "F12"
            End If
            row = row + 3
            'go to the next page
            If row > 19 Or Check4Text(row - 1, 3, " ") Then
                Hit "F8"
                row = 7
            End If
        Loop
    'review the 1 reference if it is eligible
    ElseIf Check4Text(1, 59, "REFERENCE DEMOGRAPHICS") Then
        If Check4Text(6, 67, "A") And Not Check4Text(5, 24, "04") Then RefCheck s
    End If
    
    'add REFRCALS or REFRCALR tasks if there are any
    If SkipSystem = "Compass" Or SkipSystem = "Both" Then
        AddREFRCALS
    End If
    'update recovery status and log
    UpSta 9
End Function

'check to see if the reference call task is complete
Function RefCheck(s As String)
    Dim SSD As Double
    Dim DLA As Double
    Dim DLC As Double
    Dim PED As Double
    Dim LSD As Double
    Dim RefID As String
    Dim KSBUP As Boolean
    Dim REFRCAL As Boolean
    
    REFRCAL = False
    
    'get ref ID
    RefID = GetText(3, 14, 9)
    
    'if status is 'A' and result code is not '04' then set indicator to check for KSBUP queue task
    If Check4Text(6, 67, "A") And Not Check4Text(13, 77, "04") And Check4Text(13, 36, "Y") Then KSBUP = True Else KSBUP = False
    
    'get date value of skip start
    SSD = DateValue(SkipStart)
    'get date value of date last attempt
    If Not Check4Text(13, 67, "MMDDCCYY") Then
        DLA = DateValue(Format(GetText(13, 67, 8), "##/##/####"))
    Else
        DLA = DateValue("01/01/1900")
    End If
    'get date value of date last contact
    If Not Check4Text(13, 58, "MMDDCCYY") Then
        DLC = DateValue(Format(GetText(13, 58, 8), "##/##/####"))
    Else
        DLC = DateValue("01/01/1900")
    End If
    'get date value of phone effective date
    If Not Check4Text(15, 71, "MMDDCCYY") Then
        PED = DateValue(Format(GetText(15, 71, 8), "##/##/####"))
    Else
        PED = DateValue("01/01/1900")
    End If
    'get date value of letter sent date
    If Not Check4Text(8, 73, "MMDDCCYY") Then
        LSD = DateValue(Format(GetText(8, 73, 8), "##/##/####"))
    Else
        LSD = DateValue("01/01/1900")
    End If
    
    'task is complete if address skip type date criteria is met
    If (DLA >= SSD And Val(GetText(13, 51, 2)) > 2) And (((s = "A" Or s = "B") And DLA >= PED) Or s <> "A") Or _
       (DLC >= SSD And DLC >= PED) Or _
       LSD > SSD Then
        'do nothing
    Else
        'if the phone is valid, send a letter and add a call task
        If Check4Text(13, 36, "Y") Then
            'send a letter if the address is valid
            If Check4Text(8, 53, "Y") Then AddLetter
            'add a REFRCAL queue task
            AddRefID RefID, "", KSBUP, SkipStart, "REFRCALS"
            REFRCAL = True
        'if the phone is not valid
        Else
            'send a letter if the address is valid or add a DA ref call task if the address is not valid
            If Check4Text(8, 53, "Y") Then
                AddLetter
            Else
                AddRefID RefID, "DA call, reference phone invalid", KSBUP, SkipStart, "REFRCALR"
                REFRCAL = True
            End If
        End If
    End If
    'add a record to look for a KSBUP activity record if a REFRCAL queue task was not added
    If KSBUP And Not REFRCAL Then AddRefID RefID, "", KSBUP, SkipStart, "REFRCALS"
End Function

'add row to text file to create REFCAL letter
Function AddLetter()
    Dim First As String
    Dim Last As String
    Dim RefID As String
    Dim ev As String
    
    Dim RefName As String
    Dim RefState As String
    Dim RefZip As String

    'exit function without adding a letter record if there already is one for the borrower/reference
    Open "T:\finrev.txt" For Input As #1
    While Not EOF(1)
        Input #1, ev, ev, ev, ev, ev, ev, ev, First, Last, ev, ev, ev, ev, ev, ev, RefID, ev, ev, ev, ev, ev
        If First = firstName And Last = lastName And RefID = GetText(3, 14, 9) Then
            Close #1
            Exit Function
        End If
    Wend
    Close #1

    If Check4Text(4, 60, " ") Then RefName = GetText(4, 44, 11) & " " & GetText(4, 5, 34) Else RefName = GetText(4, 44, 11) & " " & GetText(4, 60, 1) & " " & GetText(4, 5, 34)
    RefZip = GetText(10, 60, 9)
    If Len(RefZip) > 5 Then RefZip = Format(RefZip, "#####-####")
    If Check4Text(9, 55, " ") Then RefState = GetText(10, 52, 2) Else RefState = ""

    'add a letter records to the merge text file
    Open "T:\finrev.txt" For Append As #1
    Write #1, RefName, GetText(8, 9, 34), GetText(9, 9, 34), _
              GetText(10, 9, 29), RefState, RefZip, GetText(9, 55, 25), _
              firstName, lastName, address1, address2, city, state, zip, country, _
              GetText(3, 14, 9), "ACSPartCd", GetText(10, 52, 2), "CCC", _
              SP.ACSKeyLine(GetText(3, 14, 9), "R", "L"), ssn
    Close #1
End Function

'add refID and comment to the array (the info is written to an array to be processed later so LP50 can be accessed to determine if a KSBUP task is needed and to check LP8Y to prevent dup tasks in case of recovery without having to return to LP2C to continue reviewing references)
Function AddRefID(RefID As String, comment As String, KSBUP As Boolean, StartDt As String, Queue As String)
    'check to see if a record has been added for KSBUP, replace non KSBUP with KSBUP records
    For i = 1 To UBound(RefIDs, 2)
        If RefIDs(0, i) = RefID Then
            If RefIDs(2, i) = False And KSBUP = True Then RefIDs(2, i) = True
            If RefIDs(1, i) = "" And comment <> "" Then RefIDs(1, i) = comment
            Exit Function
        End If
    Next i
    
    'add record if none exists for the reference
    ReDim Preserve RefIDs(4, UBound(RefIDs, 2) + 1) As String
    RefIDs(0, UBound(RefIDs, 2)) = RefID
    RefIDs(1, UBound(RefIDs, 2)) = comment
    RefIDs(2, UBound(RefIDs, 2)) = KSBUP
    RefIDs(3, UBound(RefIDs, 2)) = Format(StartDt, "MMDDYYYY")
    RefIDs(4, UBound(RefIDs, 2)) = Queue
End Function

'add REFRCALR (originally REFRCALS) queue tasks
Function AddREFRCALS()
    Dim i As Integer
    Dim row As Integer
    
    'add tasks for each refID in the array
    For i = 1 To UBound(RefIDs, 2)
        'get the comment for the task from a KSBUP activity record if necessary
        If RefIDs(2, i) = True Then
            'look for KSBUP activity record
            FastPath "LP50I" & RefIDs(0, i)
            puttext 9, 20, "KSBUP"
            puttext 18, 29, RefIDs(3, i)
            puttext 18, 41, Format(Date, "MMDDYYYY"), "Enter"
            If Check4Text(1, 58, "ACTIVITY SUMMARY SELECT") Then puttext 3, 13, "X", "Enter"
            'get the comment from the first record if there is one
            If Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY") Then
                For row = 13 To 20
                    RefIDs(1, i) = RefIDs(1, i) & GetText(row, 2, 75)
                Next row
                'limit the comment to the space available on LP9O
                RefIDs(1, i) = Trim(MID(RefIDs(1, i), 1, 180))
            End If
        End If
        TaskAdded = True
        'add a task if one does not already exist for the borrower/referece
        FastPath "LP8YISKP;" & RefIDs(4, i) & ";;" & ssn & ";" & RefIDs(0, i)
        If Not Check4Text(1, 64, "QUEUE TASK DETAIL") Then AddLP9O RefIDs(0, i), RefIDs(4, i), , RefIDs(0, i) & " " & RefIDs(1, i)
    Next i
End Function

'update recovery status and log
Function UpSta(Sta As Integer)
    rSta = Sta
    Open LogFolder & "finrevlog.txt" For Output As #2
    Write #2, ssn, Sta, TenthSSN
    Close #2
End Function

'use this subroutine to find compass skip accounts in LP8Y for testing
Sub GetTD3G()
    Dim SSNs(1 To 200) As String
    Dim N As Integer
    Dim row As Integer
    FastPath "LP8YISKP;FINALRVW"
    puttext 21, 13, "01", "Enter"
    row = 7
    N = 0
    Do While Not Check4Text(22, 3, "46004")
        N = N + 1
        SSNs(N) = GetText(row, 2, 9)
        row = row + 1
        If row > 20 Then
            Hit "F8"
            row = 7
        End If
    Loop
    
    For i = 1 To N
        FastPath "TX3Z/ITD3G" & SSNs(i)
        If Not Check4Text(1, 72, "TDX3C") Then Exit For
    Next i
End Sub

'Determine Cost Center Code
Sub GetCCC()
    Dim Seq As Integer 'Sequence 1 = MA2324, 2 = MA2329, 3 = MA2328, 4 = "MA2327"
    Dim STemp As Integer
    Dim Cnt As Integer
    Cnt = 1
    SP.Q.FastPath "LG10I" & ssn
    If SP.Q.Check4Text(1, 53, "LOAN BWR STATUS RECAP SELECT") Then
        'LG10 Selection screen found.
        Seq = 4 'default = 4
        SP.Q.puttext 19, 15, CStr(Cnt), "ENTER"
        While SP.Q.Check4Text(20, 3, "47001") = False
            STemp = GetGetCCC
            If Seq > STemp Then Seq = STemp 'always get the lowest sequence
            SP.Q.Hit "F12"
            Cnt = Cnt + 1
            SP.Q.puttext 19, 15, CStr(Cnt), "ENTER"
        Wend
    Else
        'LG10 target screen found
        Seq = GetGetCCC
    End If
    'set cost center code and ACS key
    If Seq = 1 Then
        CCC = "MA2324"
        ACSPartCd = "BWNHDGF"
    ElseIf Seq = 2 Then
        CCC = "MA2329"
        ACSPartCd = "BWNHDFH"
    ElseIf Seq = 3 Then
        CCC = "MA2328"
        ACSPartCd = "BWNHDFH"
    Else
        CCC = "MA2327"
        ACSPartCd = "BWNHDGF"
    End If
End Sub

Function GetGetCCC() As Integer
'get CCC per loan
    Dim Seq As Integer
    Dim x As Integer
    Dim IsUHEAA As Boolean
    
    If SP.Q.Check4Text(1, 52, "LOAN BWR STATUS RECAP DISPLAY") Then
        Seq = 4 'default
        Do While SP.Q.Check4Text(22, 3, "46004") = False
            'determine if holder is UHEAA
            If CInt(SP.SQL("SELECT COUNT(*) FROM GENR_REF_LenderAffiliation WHERE LenderID = '" & GetText(5, 18, 6) & "' AND Affiliation = 'UHEAA'")(1)) > 0 Then IsUHEAA = True Else IsUHEAA = False
            For x = 0 To 9
                If SP.Q.Check4Text(11 + x, 2, "_") = False Then
                    Exit For
                End If
                'Seq 1
''                If SP.Q.Check4Text(11 + x, 59, "CR") = False And SP.Q.Check4Text(11 + x, 59, "CP") = False And CDbl(SP.Q.GetText(11 + x, 48, 8)) > 0 And SP.Q.Check4Text(5, 18, "828476") Then
                If SP.Q.Check4Text(11 + x, 59, "CR") = False And SP.Q.Check4Text(11 + x, 59, "CP") = False And CDbl(SP.Q.GetText(11 + x, 48, 8)) > 0 And IsUHEAA Then
                    Seq = 1
                    Exit For
                'Seq2
                ElseIf (SP.Q.Check4Text(11 + x, 59, "CP") And CDbl(SP.Q.GetText(11 + x, 48, 8)) > 0) Or (SP.Q.Check4Text(11 + x, 59, "CR") And (SP.Q.Check4Text(11 + x, 64, "DB") = False And SP.Q.Check4Text(11 + x, 64, "DF") = False And SP.Q.Check4Text(11 + x, 64, "DQ") = False) And CDbl(SP.Q.GetText(11 + x, 48, 8)) > 0) Then
                    If Seq > 2 Then Seq = 2
                'Seq3
                ElseIf SP.Q.Check4Text(11 + x, 59, "CR") And (SP.Q.Check4Text(11 + x, 64, "DB") Or SP.Q.Check4Text(11 + x, 64, "DF") Or SP.Q.Check4Text(11 + x, 64, "DQ")) And CDbl(SP.Q.GetText(11 + x, 48, 8)) > 0 And Not IsUHEAA Then
'                ElseIf SP.Q.Check4Text(11 + x, 59, "CR") And (SP.Q.Check4Text(11 + x, 64, "DB") Or SP.Q.Check4Text(11 + x, 64, "DF") Or SP.Q.Check4Text(11 + x, 64, "DQ")) And CDbl(SP.Q.GetText(11 + x, 48, 8)) > 0 And Not SP.Q.Check4Text(5, 18, "828476") Then
                    If Seq > 3 Then Seq = 3
                End If
            Next x
            If Seq = 1 Then Exit Do
            SP.Q.Hit "F8"
        Loop
    Else
        MsgBox "LG10 Target screen not found. Contact System Support.", 16, "LG10 Not Found"
        End
    End If
    GetGetCCC = Seq
End Function

'sorts file data
Sub SortFileCCP(FilePathAndName As String, ElementToSortBy2 As Integer, ElementToSortBy1 As Integer, Optional HeaderRow As String = "")
    'ElementToSortBy2 secondary sort index
    'ElementToSortBy1 primary sort index
    With Session
        Dim FileRecs() As Variant
        ReDim FileRecs(0)
        Dim RecArray As Variant
        Dim Record1() As String
        Dim Record() As String
        Dim Rec As Integer
        Dim FieldCounter As Integer
        
        Open FilePathAndName For Input As #1
        Do Until EOF(1)
            Line Input #1, FileRecs(Rec)
            Rec = Rec + 1
            ReDim Preserve FileRecs(Rec)
        Loop
        Close #1
        For m = 0 To Rec - 1
            For j = m + 1 To Rec - 1
                Record = Split(FileRecs(m), ",") 'create array pointer to array
                Record1 = Split(FileRecs(j), ",") 'create array pointer to array
                If Record(ElementToSortBy1) & Record(ElementToSortBy2) > Record1(ElementToSortBy1) & Record1(ElementToSortBy2) Then
                    RecArray = FileRecs(j)
                    FileRecs(j) = FileRecs(m)
                    FileRecs(m) = RecArray
                End If
            Next j
        Next m
        Open FilePathAndName For Output As #2
        'header row
        If HeaderRow <> "" Then Print #2, HeaderRow
        For i = 0 To Rec - 1
            Print #2, FileRecs(i)
        Next i
        Close #2
    End With
End Sub

'display more TD2A pages
Function nextpage()
    If Check4Text(23, 2, "01033") Then
        Hit "Enter"
        If Check4Text(1, 72, "TDX2C") Then puttext 5, 14, "X", "Enter"
    Else
        Hit "F8"
    End If
End Function

'print letters
Sub SchLettersProc()
    Dim MA2324SchCnt As Integer
    Dim MA2327SchCnt As Integer
    Dim MA2324F() As String
    Dim MA2327F() As String
    Dim x As Integer
    Dim tSchool As String
    
    ReDim MA2324F(0)
    ReDim MA2327F(0)
    MA2324SchCnt = 0
    MA2327SchCnt = 0
    
    'sort file by school
    SP.SortFile "T:\frschltrs.txt", 13
    
    'set variables
    DocPath = "X:\PADD\Skip\"
    If SP.TestMode(, DocPath) Then PrnCls = "N" Else PrnCls = "Y"
    tSchool = ""
    'open data file
    Open "T:\frschltrs.txt" For Input As #3
    Do Until EOF(3)
        'input record
        Input #3, ssn, Queue, firstName, MI, lastName, address1, address2, city, state, zip, Phone, AltPhone, email, School
        GetSchInfo
        CloseKLSLTActivityComment ssn
        If OPEStatus <> "C" Then
            'print letter for school and start new merge data file if school changes
            If School <> tSchool Then
                'print the list for the previous school
                If tSchool <> "" Then
                    Close #2
                    If ConfigureCCCForEntireFile("T:\frschltrdat.txt") = "MA2324" Then
                        MA2324SchCnt = MA2324SchCnt + 1
                        ReDim Preserve MA2324F(UBound(MA2324F) + 2)
                        MA2324F(UBound(MA2324F) - 1) = "T:\frschltrdatCover" & tSchool & ".txt"
                        MA2324F(UBound(MA2324F)) = "T:\frschltrdat" & tSchool & ".txt"
                        Set fso = CreateObject("Scripting.FileSystemObject")
                        fso.CopyFile "T:\frschltrdat.txt", MA2324F(UBound(MA2324F)), True
                        Set fso = CreateObject("Scripting.FileSystemObject")
                        fso.CopyFile "T:\frschltrdatCover.txt", MA2324F(UBound(MA2324F) - 1), True
                    Else
                        MA2327SchCnt = MA2327SchCnt + 1
                        ReDim Preserve MA2327F(UBound(MA2327F) + 2)
                        MA2327F(UBound(MA2327F) - 1) = "T:\frschltrdatCover" & tSchool & ".txt"
                        MA2327F(UBound(MA2327F)) = "T:\frschltrdat" & tSchool & ".txt"
                        Set fso = CreateObject("Scripting.FileSystemObject")
                        fso.CopyFile "T:\frschltrdat.txt", MA2327F(UBound(MA2327F)), True
                        Set fso = CreateObject("Scripting.FileSystemObject")
                        fso.CopyFile "T:\frschltrdatCover.txt", MA2327F(UBound(MA2327F) - 1), True
                    End If
                End If
                
                'set temp school = new school
                tSchool = School
                'write new school info to merge data file
                Open "T:\frschltrdatCover.txt" For Output As #2
                Write #2, "SSN", "Queue", "FirstName", "MI", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Country", "Phone", "AltPhone", "Email", "School", "SchName", "SchAdd", "SchAdd2", "SchAdd3", "SchCity", "SchST", "SchCountry", "SchZip"
                Write #2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", School, SchName, SchAdd, SchAdd2, SchAdd3, SchCity, SchST, SchCountry, SchZip
                Close #2
    
                'set up merge data file for new list
                Open "T:\frschltrdat.txt" For Output As #2
                Write #2, "SSN", "Queue", "FirstName", "MI", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Country", "Phone", "AltPhone", "Email", "School", "SchName", "SchAdd", "SchAdd2", "SchAdd3", "SchCity", "SchST", "SchCountry", "SchZip", "COST_CENTER_CODE"
            End If
            'write record to merge data file
            If Len(zip) > 5 Then zip = Format(zip, "@@@@@-@@@@")
            'Get cost center code
            GetCCCSclLtrs
            Write #2, Format(ssn, "@@@-@@-@@@@"), Queue, firstName, MI & " ", lastName, address1, address2, city, state, zip, "", Format(Phone, "(###) ###-####"), Format(AltPhone, "(###) ###-####"), email, School, SchName, SchAdd, SchAdd2, SchAdd3, SchCity, SchST, SchCountry, SchZip, CCC
        End If
    Loop
    'close files and print list for last school
    Close #3
    Close #2
    If OPEStatus <> "C" Then
        If ConfigureCCCForEntireFile("T:\frschltrdat.txt") = "MA2324" Then
            MA2324SchCnt = MA2324SchCnt + 1
            ReDim Preserve MA2324F(UBound(MA2324F) + 2)
            MA2324F(UBound(MA2324F) - 1) = "T:\frschltrdatCover" & School & ".txt"
            MA2324F(UBound(MA2324F)) = "T:\frschltrdat" & School & ".txt"
            Set fso = CreateObject("Scripting.FileSystemObject")
            fso.CopyFile "T:\frschltrdat.txt", MA2324F(UBound(MA2324F)), True
            Set fso = CreateObject("Scripting.FileSystemObject")
            fso.CopyFile "T:\frschltrdatCover.txt", MA2324F(UBound(MA2324F) - 1), True
        Else
            MA2327SchCnt = MA2327SchCnt + 1
            ReDim Preserve MA2327F(UBound(MA2327F) + 2)
            MA2327F(UBound(MA2327F) - 1) = "T:\frschltrdatCover" & School & ".txt"
            MA2327F(UBound(MA2327F)) = "T:\frschltrdat" & School & ".txt"
            Set fso = CreateObject("Scripting.FileSystemObject")
            fso.CopyFile "T:\frschltrdat.txt", MA2327F(UBound(MA2327F)), True
            Set fso = CreateObject("Scripting.FileSystemObject")
            fso.CopyFile "T:\frschltrdatCover.txt", MA2327F(UBound(MA2327F) - 1), True
        End If
    End If

    'print in order of number of pages
    Dim x2 As Integer
    Dim MA2324FCnt As Integer
    Dim MA2327FCnt As Integer
    MA2324FCnt = 0
    MA2327FCnt = 0
    Dim country As String
    Dim StateSide As Integer
    Dim Foriegn As Integer
    StateSide = 0
    Foriegn = 0
    'count foreign or state side for the cover letter.
    For x2 = 2 To UBound(MA2324F) Step 2
        Open MA2324F(x2) For Input As #1
            Input #1, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, country, v
            Input #1, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, country, v
        Close #1
        If country = "" Then
            StateSide = StateSide + 1
        Else
            Foriegn = Foriegn + 1
        End If
    Next x2
    'If UBound(MA2324F) > 0 Then LocalCCCPrinter "MA2324", "School Letter - Skip Trace Assistance Request", UBound(MA2324F) / 2
    If UBound(MA2324F) > 0 Then LocalCCCPrinter "MA2324", "School Letter - Skip Trace Assistance Request", StateSide, Foriegn
    x2 = 1
    Do
        If UBound(MA2324F) > 0 Then
            For x = 1 To UBound(MA2324F) Step 2
                If GetRows(MA2324F(x + 1)) = x2 Then
                    'print cover letter
                    Doc = "SCHLTRCVR"
                    Common.PrintLetters MA2324F(x)
                    Kill MA2324F(x)
                    MA2324FCnt = MA2324FCnt + 2
                    'print borrower data
                    Doc = "SCHLTRLST"
                    Common.PrintLetters MA2324F(x + 1)
                    Kill MA2324F(x + 1)
                End If
            Next x
        End If
        x2 = x2 + 1
        If MA2324FCnt = UBound(MA2324F) Then Exit Do
    Loop
    
    'count foreign or state side for the cover letter.
    StateSide = 0
    Foriegn = 0
    For x2 = 2 To UBound(MA2327F) Step 2
        Open MA2327F(x2) For Input As #1
            Input #1, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, country, v, v
            Input #1, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, country, v, v
                If country = "" Then
                    StateSide = StateSide + 1
                Else
                    Foriegn = Foriegn + 1
                End If
''            Do While Not EOF(1)
''                Input #1, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, Country, v, v
''                If Country = "" Then
''                    StateSide = StateSide + 1
''                Else
''                    Foriegn = Foriegn + 1
''                End If
''            Loop
        Close #1
    Next x2
    'If UBound(MA2327F) > 0 Then LocalCCCPrinter "MA2327", "School Letter - Skip Trace Assistance Request", UBound(MA2327F) / 2
    If UBound(MA2327F) > 0 Then LocalCCCPrinter "MA2327", "School Letter - Skip Trace Assistance Request", StateSide, Foriegn
    x2 = 1
    Do
        If UBound(MA2327F) > 0 Then
            For x = 1 To UBound(MA2327F) Step 2
                If GetRows(MA2327F(x + 1)) = x2 Then
                    'print cover letter
                    Doc = "SCHLTRCVR"
                    Common.PrintLetters MA2327F(x)
                    Kill MA2327F(x)
                    MA2327FCnt = MA2327FCnt + 2
                    'print borrower data
                    Doc = "SCHLTRLST"
                    Common.PrintLetters MA2327F(x + 1)
                    Kill MA2327F(x + 1)
                End If
            Next x
        End If
        x2 = x2 + 1
        If MA2327FCnt = UBound(MA2327F) Then Exit Do
    Loop
End Sub

Function GetRows(F As String) As Integer
    'There's a chance the file (F) may not exist, which causes problems. So first check that F is valid.
    If Dir(F) = "" Then
        GetRows = 0
        Exit Function
    End If
    Dim x As Integer
    Dim v As String
    x = -1
    Open F For Input As #99
    Do While Not EOF(99)
        Line Input #99, v
        x = x + 1
    Loop
    Close #99
    GetRows = x
End Function

'get school information
Sub GetSchInfo()
    With Session
        'access LPSC
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        .TransmitANSI "LPSCI" & School
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        'select dept 112 if it is displayed
        If .GetDisplayText(21, 3, 3) = "SEL" Then
            'find the row for dept 112
            row = 7
            Do Until .GetDisplayText(row, 7, 3) = "112"
                row = row + 1
                'go to the next page if the bottom of the page is reached
                If row = 19 Then
                    .TransmitTerminalKey rcIBMPf8Key
                    .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
                    row = 7
                    'use the general address if dept 112 is not found
                    If .GetDisplayText(22, 3, 5) = "46004" Then Exit Do
                End If
            Loop
            'select the row
            .TransmitANSI .GetDisplayText(row, 2, 2)
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        End If
        'get the information
        SchName = Trim(.GetDisplayText(5, 21, 40))
        SchAdd = Trim(.GetDisplayText(8, 21, 30))
        If .GetDisplayText(9, 21, 1) <> " " Then SchAdd2 = Trim(.GetDisplayText(9, 21, 30)) Else SchAdd2 = ""
        If .GetDisplayText(10, 21, 1) <> " " Then SchAdd3 = Trim(.GetDisplayText(10, 21, 30)) Else SchAdd3 = ""
        SchCity = Trim(.GetDisplayText(11, 21, 30))
        SchST = .GetDisplayText(11, 59, 2)
        If SchST = "FC" Then
            SchST = .GetDisplayText(12, 21, 2)
            SchCountry = .GetDisplayText(12, 55, 2)
        End If
        SchZip = .GetDisplayText(11, 66, 5)
        If .GetDisplayText(11, 71, 1) <> " " Then SchZip = SchZip & "-" & .GetDisplayText(11, 71, 4)
        SP.Q.Hit "F10"
        OPEStatus = SP.Q.GetText(4, 20, 1)
        SP.Q.Hit "F12"
    End With
End Sub

Sub LocalCCCPrinter(CCC As String, DocDescription As String, NumStates As Integer, NumForeign As Integer)
    Dim Cover As String
    
    Cover = "T:\DMVCover.txt"
    Open Cover For Output As #1
        Write #1, "BU", "Description", "NumPages", "Cost", "Standard", "Foreign", "CoverComment"
        Write #1, SP.GetBU(CCC), DocDescription, "", CCC, NumStates, NumForeign, "Deliver mail to business unit for processing"
    Close #1
    
    If SP.Common.TestMode Then
        SP.Common.PrintDocs "X:\PADD\General\Test\", "Scripted State Mail Cover Sheet", Cover, ToPrinter
    Else
        SP.Common.PrintDocs "X:\PADD\General\", "Scripted State Mail Cover Sheet", Cover, ToPrinter
    End If
    If Dir(Cover) <> "" Then Kill Cover 'delete cover sheet data file
End Sub

Function ConfigureCCCForEntireFile(StateFile As String) As String
    Dim StTemp As String
    Dim v As String
    Dim va(23)
    Dim MA2324 As Boolean
    Dim FileCCC As String
    
    StTemp = "T:\StTemp.txt"
    Set fso = CreateObject("Scripting.FileSystemObject")
    fso.CopyFile StateFile, StTemp, True
    Open StTemp For Input As #9
        FileCCC = "MA2327"
        Do While Not EOF(9)
            Input #9, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, CCC
            If CCC = "MA2324" Then
                Close #9
                FileCCC = "MA2324"
                Exit Do
            End If
        Loop
    Close #9
    
    Open StTemp For Input As #9
    Open StateFile For Output As #2
        Do While Not EOF(9)
            Input #9, va(0), va(1), va(2), va(3), va(4), va(5), va(6), va(7), va(8), va(9), va(10), va(11), va(12), va(13), va(14), va(15), va(16), va(17), va(18), va(19), va(20), va(21), va(22), va(23)
            If va(0) = "SSN" Then
                'header
                Write #2, va(0), va(1), va(2), va(3), va(4), va(5), va(6), va(7), va(8), va(9), va(10), va(11), va(12), va(13), va(14), va(15), va(16), va(17), va(18), va(19), va(20), va(21), va(22), va(23)
            Else
                Write #2, va(0), va(1), va(2), va(3), va(4), va(5), va(6), va(7), va(8), va(9), va(10), va(11), va(12), va(13), va(14), va(15), va(16), va(17), va(18), va(19), va(20), va(21), va(22), FileCCC
            End If
        Loop
    Close #2
    Close #9
    ConfigureCCCForEntireFile = FileCCC
    If Dir(StTemp) <> "" Then Kill StTemp
End Function

'Determine Cost Center Code
Sub GetCCCSclLtrs()
    Dim Seq As Integer 'Sequence 1 = MA2324, 2 = MA2329, 3 = MA2328, 4 = "MA2327"
    Dim STemp As Integer
    Dim Cnt As Integer
    Cnt = 1
    SP.Q.FastPath "LG10I" & ssn
    If SP.Q.Check4Text(1, 53, "LOAN BWR STATUS RECAP SELECT") Then
        'LG10 Selection screen found.
        Seq = 4 'default = 4
        SP.Q.puttext 19, 15, CStr(Cnt), "ENTER"
        While SP.Q.Check4Text(20, 3, "47001") = False
            STemp = GetGetCCCSclLtrs
            If Seq > STemp Then Seq = STemp 'always get the lowest sequence
            SP.Q.Hit "F12"
            Cnt = Cnt + 1
            SP.Q.puttext 19, 15, CStr(Cnt), "ENTER"
        Wend
    Else
        'LG10 target screen found
        Seq = GetGetCCCSclLtrs
    End If
    'set cost center code and ACS key
    If Seq = 1 Then
        CCC = "MA2324"
    Else
        CCC = "MA2327"
    End If
End Sub

Function GetGetCCCSclLtrs() As Integer
'get CCC per loan
    Dim Seq As Integer
    Dim x As Integer
    Dim IsUHEAA As Boolean
    
    If SP.Q.Check4Text(1, 52, "LOAN BWR STATUS RECAP DISPLAY") Then
        Seq = 4 'default
        Do While SP.Q.Check4Text(22, 3, "46004") = False
            'determine if holder is UHEAA
            If CInt(SP.SQL("SELECT COUNT(*) FROM GENR_REF_LenderAffiliation WHERE LenderID = '" & GetText(5, 18, 6) & "' AND Affiliation = 'UHEAA'")(1)) > 0 Then IsUHEAA = True Else IsUHEAA = False
            For x = 0 To 9
                If SP.Q.Check4Text(11 + x, 2, "_") = False Then
                    Exit For
                End If
                'Seq 1
                If SP.Q.Check4Text(11 + x, 59, "CR") = False And SP.Q.Check4Text(11 + x, 59, "CP") = False And CDbl(SP.Q.GetText(11 + x, 48, 8)) > 0 And IsUHEAA Then
                    Seq = 1
                    Exit For
                End If
            Next x
            If Seq = 1 Then Exit Do
            SP.Q.Hit "F8"
        Loop
    Else
        MsgBox "LG10 Taget screen not found. Contact System Support."
        End
    End If
    GetGetCCCSclLtrs = Seq
End Function

'determine if the borrower's state has a participating DMV
Function PartDMV() As Boolean
    Select Case state
    Case "UT", "NV", "TX", "FL", "IL", "MI", "TN", "KY", "SD", "MD", "MA", "NE", "WV"
        PartDMV = True
    Case Else
        PartDMV = False
    End Select
End Function

Function SchLtrCrtd(KLSLTs As String) As Boolean
    Dim SchFnd As Boolean
    Dim Schools() As String
    Dim School As String
    Dim i As Integer
    Dim row As Integer
    
    SchLtrCrtd = False
    
    'get schools from activity record
    If KLSLTs <> "" Then
        If InStr(32, KLSLTs, "TO") - 33 < 8 Then
            ReDim Schools(0) As String
        Else
            Schools = Split(MID(KLSLTs, 32, InStr(32, KLSLTs, "TO") - 33))
        End If
    Else
        ReDim Schools(0) As String
    End If
    
    FastPath "LG29I" & ssn
    'review schools on selection screen
    If Check4Text(1, 49, "STUDENT ENROLLMENT STATUS SELECT") Then
        row = 9
        While Not Check4Text(22, 3, "46004")
            'set school letter created indicator and add school to array of schools having been sent letters if the school was not in the activity record or has not already been identified to be send a letter
            If Not Check4School(Schools, GetText(row, 14, 8)) Then
                SchLtrCrtd = True
                ReDim Preserve Schools(UBound(Schools) + 1)
                Schools(UBound(Schools)) = GetText(row, 14, 8)
            End If
            row = row + 1
            
            'check for another page
            If Check4Text(row, 6, " ") Then
                Hit "F8"
                row = 9
            End If
        Wend
    'review school on detail screen
    ElseIf Check4Text(1, 48, "STUDENT ENROLLMENT STATUS DISPLAY") Then
        If Not Check4School(Schools, GetText(10, 71, 8)) Then SchLtrCrtd = True
    'add queue task if no LG29 record found (PLUS only borrower)
    Else
        SchLtrCrtd = True
        SP.Common.ATD22AllLoans ssn, "S4SCL", "", "FINALREV", UserID
    End If
 End Function
 
 'check to see if school has been sent a letter
 Function Check4School(Schools() As String, School As String) As Boolean
    Check4School = True
    
    SchFnd = False
    'compare the school on LG29 to schools having been sent letters
    For i = 0 To UBound(Schools)
        'set the indicator and stop looking if the school has been sent a letter
        If School = Schools(i) Then
            SchFnd = True
            Exit For
        End If
    Next i
    
    'add a record to the file if the school has not been sent a letter for the borrower
    If Not SchFnd Then
        Check4School = False
        Open "T:\frschltrs.txt" For Append As #3
        Write #3, ssn, "", firstName, "", lastName, address1, address2, city, state, zip, Phone, AltPhone, email, School
        Close #3
    End If
End Function







