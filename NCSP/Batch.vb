Imports System.IO

Public Class Batch

    Sub GenerateBatch()
        Dim OldAcct As String = String.Empty
        Dim AmtTotal As Double = 0
        Dim BatchNum As String = String.Empty
        BatchNum = GetNextBatchNumber()
        Dim DS As New DataSet
        'Dim DSSave As New DataSet
        Dim DSSum As New DataSet
        Dim BatchTotal As Double = 0

        'Same SQL statment as the Batch Recap Report
        Dim SC As New SqlClient.SqlCommand("select A.AcctID, C.FName + ' ' + C.LName as FName, C.SSN, A.TranID, B.InstID as SchedInst, E.Semester, E.SchedYr, A.TranTyp, A.TranAmt, A.TranStat, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact, A.TranHrPd From [Transaction] A inner join (select A1.AcctID, B1.InstID, Sum(A1.TranAmt) as SumTranAmt , A1.SchedID from [Transaction] A1 inner join (select R1.AcctID, Y2.InstID, R1.SchedID from [Transaction] R1 inner join (select distinct Z.AcctID, Y2.InstID, Z.SchedID from [Transaction] Z inner join Schedule Y on Z.AcctID = Y.AcctID and Y.SchedStat = 'Payment Pending' and Y.RowStatus = 'A' and Z.SchedID = Y.SchedID inner join Inst Y2 on Y.InstAtt = Y2.InstLong where Z.NCSPBatchNum = '' ) R6 on R6.AcctID = R1.AcctID inner join Schedule Y on R1.AcctID = Y.AcctID and Y.RowStatus = 'A' and R1.SchedID = Y.SchedID inner join Inst Y2 on Y.InstAtt = Y2.InstLong where R1.NCSPBatchNum = '' UNION select R1.AcctID, Y2.InstID, R1.SchedID	from [Transaction] R1 inner join (select distinct Y.AcctID, Y.SchedInst as InstID, Y.SchedID from [Transaction] Y where (Y.TranTyp = 'Supplemental Payment' or Y.TranTyp = 'Student Repayment') and Y.TranStat = 'Entered' and Y.NCSPBatchNum = '' ) R6 on R6.AcctID = R1.AcctID inner join Schedule Y on R1.AcctID = Y.AcctID and Y.RowStatus = 'A' and R1.SchedID = Y.SchedID inner join Inst Y2 on Y.InstAtt = Y2.InstLong where R1.NCSPBatchNum = '' ) B1 on A1.AcctID = B1.AcctID and A1.SchedID = B1.SchedID group by A1.AcctID, B1.InstID, A1.SchedID having Sum(A1.TranAmt) > 0 ) B on A.AcctID = B.AcctID and A.SchedID = B.SchedID inner join Account C on A.AcctID = C.AcctID and C.Balance > 0 and C.RowStatus = 'A' inner join Inst D on B.InstID = D.InstID left outer join Schedule E on A.AcctID = E.AcctID and A.SchedID = E.SchedID and A.SchedSemEnr = E.Semester and A.SchedYrEnr = E.SchedYr and E.RowStatus = 'A' Where A.NCSPBatchNum = '' order by C.LName, C.FName", dbConnection)
        Dim DA As New SqlClient.SqlDataAdapter(SC)
        Dim x As Integer

        dbConnection.Open()
        DA.Fill(DS)
        dbConnection.Close()
        RemoveZeroBalance(DS)
        RemoveAdjustmentOnlys(DS)
        Dim R As DataRow
        Dim R2 As DataRow


        AmtTotal = 0
        If DS.Tables(0).Rows.Count = 0 Then
            MsgBox("There are no transactions for the batch to process.", MsgBoxStyle.Information, "New Century Scholarship Program")
            Exit Sub
        End If
        For Each R In DS.Tables(0).Rows
            BatchTotal = BatchTotal + CDbl(R.Item("TranAmt"))
            If OldAcct <> R.Item("AcctID") And OldAcct <> "" Then
                'Add communications record for account
                AddCommunications(OldAcct, Now, False, False, "Account Maintenance", "Payment request for " & AmtTotal & " submitted to accounting.", "Check Request Sent to Accounting")
                AmtTotal = 0
            End If
            updateTransactionByAttribute(R.Item("TranID"), "NCSPBatchNum", BatchNum)
            updateTransactionByAttribute(R.Item("TranID"), "NCSPBatchDt", Now)
            updateTransactionByAttribute(R.Item("TranID"), "TranInst", R.Item("SchedInst"))

            For Each R2 In DS.Tables(0).Rows
                If R.Item("AcctID") = R2.Item("AcctID") And (R2.Item("TranTyp") = "Payment Pending" Or R2.Item("TranTyp") = "Entered") Then

                    'update TransInst for account
                    updateTransactionByAttribute(R.Item("TranID"), "TranInst", R2.Item("SchedInst"))
                    Exit For
                End If
            Next
            updateTransactionByAttribute(R.Item("TranID"), "TranStat", "Pending")
            AmtTotal = AmtTotal + CDbl(R.Item("TranAmt"))
            OldAcct = (R.Item("AcctID"))
        Next
        'Add communications record for last account in data set
        AddCommunications(OldAcct, Now, False, False, "Account Maintenance", "Payment request for " & AmtTotal & " submitted to accounting.", "Check Request Sent to Accounting")

        'PRINT BATCH REPORT
        'print report
        Dim rpt As New rptBatchDetail
        DS = New DataSet
        GetdbConnection()
        SC = New SqlClient.SqlCommand("select A.AcctID, C.FName + ' ' + C.LName as FName, C.SSN, A.SchedInst, A.tranInst, A.SchedSemEnr, A.SchedYrEnr, A.TranHrPd, A.TranTyp, A.TranAmt From [Transaction] A inner join Account C on A.AcctID = C.AcctID and C.RowStatus = 'A' Where A.NCSPBatchNum = '" & BatchNum & "' Order By C.LName, C.FName", dbConnection)
        DA = New SqlClient.SqlDataAdapter(SC)
        dbConnection.Open()
        DA.Fill(DS)
        dbConnection.Close()

        'set institute for records with out 
        For Each R In DS.Tables(0).Rows
            If R.Item("SchedInst") = "" Then
                For Each R2 In DS.Tables(0).Rows
                    If R.Item("AcctID") = R2.Item("AcctID") And (R2.Item("TranTyp") = "Payment" Or R2.Item("TranTyp") = "Supplemental Payment") Then
                        R.Item("SchedInst") = R2.Item("SchedInst")
                        Exit For
                    End If
                Next
            End If
        Next

        rpt.SetDataSource(DS.Tables(0))
        rpt.PrintToPrinter(1, False, 0, 0)
        If Directory.Exists(BatchDocPath & Format(Now, "MM_dd_yy")) = False Then Directory.CreateDirectory(BatchDocPath & Format(Now, "MM_dd_yy"))
        rpt.ExportToDisk(CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat, BatchDocPath & Format(Now, "MM_dd_yy") & "\BatchDetail_" & BatchNum & ".pdf")
        'End If

        'PRINT CHECK REQUEST
        '..................................
        Dim rpt2 As New rptACHRequest

        SC = New SqlClient.SqlCommand("Select '" & BatchNum & "' AS AchAdviceNumber, SchedInst, Sum(TranAmt) as TranAmt, InstLong, InstContact, InstAdd1, InstAdd2, CSZ" & _
                                      " From (select D2.InstAtt as SchedInst, SUM(A.TranAmt) AS TranAmt, D.InstLong, D.InstAdd1, D.InstAdd2, RTrim(D.InstCity) + ', ' + D.InstST + ' ' + D.InstZip as CSZ, D.InstContact" & _
                                             " From [Transaction] A" & _
                                                 " inner join Account C" & _
                                                        " on A.AcctID = C.AcctID and C.RowStatus = 'A'" & _
                                                 " inner join (select Q2.AcctID, Q2.InstAtt, Q2.SchedID" & _
                                                              " from Schedule Q2 inner join (select MAX(Qa.SchedID) as SchedID, Qa.AcctID" & _
                                                                                           " from Schedule Qa where Qa.RowStatus = 'A'" & _
                                                                                           " group by Qa.AcctID) Q1" & _
                                                                    " on Q2.AcctID = Q1.AcctID and Q2.SchedID = Q1.SchedID" & _
                                                              " where Q2.RowStatus = 'A') D2" & _
                                                        " on D2.AcctID = C.AcctID" & _
                                                  " inner join Inst D" & _
                                                        " on D2.InstAtt = D.InstLong" & _
                                                  " left outer join Schedule E" & _
                                                        " on A.AcctID = E.AcctID and A.SchedID = E.SchedID and E.RowStatus = 'A'" & _
                                             " Where A.NCSPBatchNum = '" & BatchNum & "' and A.TranTyp <> 'Payment' and A.TranTyp <> 'Supplemental Payment'" & _
                                             " Group By D2.InstAtt, D.InstLong, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact" & _
                                             " UNION" & _
                                             " select E.InstAtt as SchedInst, SUM(A.TranAmt) AS TranAmt,D.InstLong, D.InstAdd1, D.InstAdd2, RTrim(D.InstCity) + ', ' + D.InstST + ' ' + D.InstZip as CSZ, D.InstContact" & _
                                             " From [Transaction] A" & _
                                                   " inner join Account C" & _
                                                        " on A.AcctID = C.AcctID and C.RowStatus = 'A'" & _
                                                   " inner join (select z.AcctID, max(SchedID) as SchedID" & _
                                                               " from [Transaction] z" & _
                                                               " where z.NCSPBatchNum = '" & BatchNum & "' and (z.TranTyp = 'Payment' or z.TranTyp = 'Supplemental Payment')" & _
                                                               " group by z.AcctID) A2" & _
                                                         " on A.AcctID = A2.AcctID" & _
                                                   " left outer join Schedule E" & _
                                                         " on A2.AcctID = E.AcctID and A2.SchedID = E.SchedID and E.RowStatus = 'A'" & _
                                                   " inner join Inst D" & _
                                                         " on E.InstAtt = D.InstLong" & _
                                             " Where A.NCSPBatchNum = '" & BatchNum & "' and (A.TranTyp = 'Payment' or A.TranTyp = 'Supplemental Payment') and A.SchedInst = ''" & _
                                             " group by E.InstAtt, D.InstLong, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact" & _
                                             " UNION" & _
                                             " select E.InstAtt as SchedInst, SUM(A.TranAmt) AS TranAmt,D.InstLong, D.InstAdd1, D.InstAdd2, RTrim(D.InstCity) + ', ' + D.InstST + ' ' + D.InstZip as CSZ, D.InstContact" & _
                                             " From [Transaction] A" & _
                                                    " inner join Account C" & _
                                                        " on A.AcctID = C.AcctID and C.RowStatus = 'A'" & _
                                                    " left outer join Schedule E" & _
                                                        " on A.AcctID = E.AcctID and A.SchedID = E.SchedID and E.RowStatus = 'A'" & _
                                                    " inner join Inst D" & _
                                                        " on E.InstAtt = D.InstLong" & _
                                             " Where A.NCSPBatchNum = '" & BatchNum & "' and (A.TranTyp = 'Payment' or A.TranTyp = 'Supplemental Payment') and A.SchedInst <> ''" & _
                                             " Group By E.InstAtt, D.InstLong, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact) A" & _
                                        " Group by SchedInst, InstLong, InstAdd1, InstAdd2, CSZ, InstContact" & _
                                        " Order by SchedInst", dbConnection)

        Dim CRds As New DataSet
        DA = New SqlClient.SqlDataAdapter(SC)
        dbConnection.Open()
        DA.Fill(CRds)
        dbConnection.Close()

        rpt2.SetDataSource(CRds.Tables(0))
        rpt2.PrintToPrinter(1, False, 0, 0)
        If Directory.Exists(BatchDocPath & Format(Now, "MM_dd_yy")) = False Then Directory.CreateDirectory(BatchDocPath & Format(Now, "MM_dd_yy"))
        rpt2.ExportToDisk(CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat, BatchDocPath & Format(Now, "MM_dd_yy") & "\CheckRequest_" & BatchNum & ".pdf")

        'show report
        SC = New SqlClient.SqlCommand("Select AcctID, FName, LName, MI, SSN, InstID, Semester, SchedYr, CredHrEnr, TranTyp, SUM(TranAmt) AS TranAmt, TranStat, InstLong, InstAdd1, InstAdd2, InstCity, InstST, InstZip, InstContact From (select A.AcctID, C.FName, C.LName, C.MI, C.SSN, D.InstID, E.Semester, E.SchedYr, E.CredHrEnr, A.TranTyp, SUM(A.TranAmt) AS TranAmt, A.TranStat, D.InstLong, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact From [Transaction] A inner join Account C on A.AcctID = C.AcctID and C.RowStatus = 'A' inner join (select Q2.AcctID, Q2.InstAtt, Q2.SchedID, Q2.Semester, Q2.SchedYr from Schedule Q2 inner join (select MAX(Qa.SchedID) as SchedID, Qa.AcctID from Schedule Qa where Qa.RowStatus = 'A' group by Qa.AcctID) Q1 on Q2.AcctID = Q1.AcctID and Q2.SchedID = Q1.SchedID where Q2.RowStatus = 'A') D2 on D2.AcctID = C.AcctID inner join Inst D on D2.InstAtt = D.InstLong left outer join Schedule E on A.AcctID = E.AcctID and A.SchedID = E.SchedID and E.RowStatus = 'A' Where A.NCSPBatchNum = '" & BatchNum & "' and A.TranTyp <> 'Payment' and A.TranTyp <> 'Supplemental Payment' group by A.AcctID, C.FName, C.LName, C.MI, C.SSN, D.InstID, E.Semester, E.SchedYr, E.CredHrEnr, A.TranTyp, A.TranStat, D.InstLong, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact UNION select A.AcctID, C.FName, C.LName, C.MI, C.SSN, D.InstID, E.Semester, E.SchedYr, E.CredHrEnr, A.TranTyp, SUM(A.TranAmt) AS TranAmt, A.TranStat, D.InstLong, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact From [Transaction] A inner join Account C on A.AcctID = C.AcctID and C.RowStatus = 'A' left outer join Schedule E on A.AcctID = E.AcctID and A.SchedID = E.SchedID and E.RowStatus = 'A' inner join Inst D on E.InstAtt = D.InstLong Where A.NCSPBatchNum = '" & BatchNum & "' and (A.TranTyp = 'Payment' or A.TranTyp = 'Supplemental Payment') group by A.AcctID, C.FName, C.LName, C.MI, C.SSN, D.InstID, E.Semester, E.SchedYr, E.CredHrEnr, A.TranTyp, A.TranStat, D.InstLong, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact ) A group by AcctID, FName, LName, MI, SSN, InstID, Semester, SchedYr, CredHrEnr, TranTyp, TranStat, InstLong, InstAdd1, InstAdd2, InstCity, InstST, InstZip, InstContact order by A.AcctID", dbConnection)

        DA = New SqlClient.SqlDataAdapter(SC)
        dbConnection.Open()
        DA.Fill(DSSum)
        dbConnection.Close()

        'get unique schools
        Dim Schools As New ArrayList
        For Each R In DSSum.Tables(0).Rows
            If Schools.IndexOf(R.Item("InstID")) < 0 Then
                Schools.Add(R.Item("InstID"))
            End If
        Next

        'Dim Student As ArrayList
        Dim StudentTotal As String = String.Empty
        Dim StudentAmount As New ArrayList  'Total Amount
        Dim StudentAccount As New ArrayList 'AccountID
        Dim StudentFName As New ArrayList
        Dim StudentLName As New ArrayList
        Dim StudentSemester As New ArrayList
        Dim StudentYear As New ArrayList
        Dim SchoolTotal As Double = 0
        Dim FName As String = String.Empty
        Dim LName As String = String.Empty
        Dim SSN As String = String.Empty
        Dim Semester As String = String.Empty
        Dim SchedYr As String = String.Empty
        Dim CredHr As String = String.Empty
        Dim InstName As String = String.Empty
        Dim Add1 As String = String.Empty
        Dim Add2 As String = String.Empty
        Dim City As String = String.Empty
        Dim St As String = String.Empty
        Dim Zip As String = String.Empty
        Dim Contact As String = String.Empty

        'Set the Semester and Year of the Student Repayments to the Adjustment's Semester and Year
        For Each R In DSSum.Tables(0).Select("TranTyp = 'Adjustment'")
            For Each R2 In DSSum.Tables(0).Select("TranTyp = 'Student Repayment' and AcctID = '" & R.Item("AcctID") & "'")
                R2.Item("Semester") = R.Item("Semester")
                R2.Item("SchedYr") = R.Item("SchedYr")
                'R2.Item("InstID") = R.Item("InstID")
            Next
        Next

        For x = 0 To Schools.Count - 1
            FileOpen(1, "T:\NCSP_LetterToSchool.txt", OpenMode.Output)
            'WriteLine(1, "InstName", "Contact", "Add1", "Add2", "City", "St", "Zip", "SchoolTotal", "StudentFName", "StudentLName", "SSN", "Semester", "Year", "CredHr", "Amt")
            WriteLine(1, "InstName", "Contact", "Add1", "Add2", "City", "St", "Zip", "SchoolTotal", "StudentFName", "StudentLName", "SSN", "Semester", "Year", "Amt")
            FileClose(1)
            StudentAmount.Clear()
            StudentAccount.Clear()
            StudentFName.Clear()
            StudentLName.Clear()
            StudentSemester.Clear()
            StudentYear.Clear()

            'TODO:
            'I had a problem come up on the batch I just completed.  I have a student  Ericka Smith (0900322), she had an adjustment that she owed back of 139.61.  She had originally planned to go to Weber in the Spring 2010, then at the last minute changed to SUU, the tuition difference was the adjustment.  When I processed her Fall 2010 funding today, the 139.61 came off of the Batch Recap, Batch Detail, Check Request, and total on the school letter correctly.  However, on the school detail the adjustment did not come off and listed that we were sending 1,468.90 instead of the 1328.99 that we were really sending.  Somehow, the logic did not make it to that final point of the detail information.  
            'The system is trying to allocate the adjustment to the payment for the next semester when it should really just find any payment for the student and allocate it to that payment.  If there is no payment for the next semester the adjustment wouldn’t be allocated to any payment.

            For Each R In DSSum.Tables(0).Select("InstID = '" & Schools(x) & "'")
                InstName = R.Item("InstLong")
                Add1 = R.Item("InstAdd1")
                Add2 = R.Item("InstAdd2")
                City = R.Item("InstCity")
                St = R.Item("InstST")
                If Len(CStr(R.Item("InstZIP"))) > 5 Then Zip = CLng(CStr(R.Item("InstZIP"))).ToString("00000-0000") Else Zip = CStr(CStr(R.Item("InstZIP")))
                Contact = R.Item("InstContact")
                'Add the Adjustment to the next semester
                If R.Item("TranTyp") <> "Payment" And R.Item("TranTyp") <> "Supplemental Payment" Then
                    Dim nextYear As Integer
                    Dim nextSemester As Integer
                    nextYear = 0
                    nextSemester = 0
                    'Find the next semester
                    For Each R2 In DSSum.Tables(0).Select("InstID = '" & Schools(x) & "' and SSN = '" & R.Item("SSN") & "' and (TranTyp = 'Payment' or TranTyp = 'Supplemental Payment')")
                        If nextYear = 0 Then
                            nextYear = R2.Item("SchedYr")
                            nextSemester = SemesterInteger(R2.Item("Semester"))
                        ElseIf (CInt(R2.Item("SchedYr")) * 4) + SemesterInteger(R2.Item("Semester")) < (CInt(nextYear) * 4) + nextSemester And ((CInt(R2.Item("SchedYr")) * 4) + SemesterInteger(R2.Item("Semester")) >= (CInt(R.Item("SchedYr")) * 4) + SemesterInteger(R.Item("Semester"))) Then
                            nextYear = R2.Item("SchedYr")
                            nextSemester = SemesterInteger(R2.Item("Semester"))
                        End If
                        'End If
                    Next
                    'Add the Transaction amount to the next transaction
                    For Each R2 In DSSum.Tables(0).Select("InstID = '" & Schools(x) & "'")
                        If R.Item("SSN") = R2.Item("SSN") And (R2.Item("TranTyp") = "Payment" Or R2.Item("TranTyp") = "Supplemental Payment") Then
                            If (CInt(R2.Item("SchedYr")) * 4) + SemesterInteger(R2.Item("Semester")) = (CInt(nextYear) * 4) + nextSemester Then
                                R2.Item("TranAmt") = CDbl(R2.Item("TranAmt")) + CDbl(R.Item("TranAmt"))
                                Exit For
                            End If
                        End If
                    Next
                End If
            Next

            SchoolTotal = 0
            For Each R In DSSum.Tables(0).Select("InstID = '" & Schools(x) & "'")
                If R.Item("TranTyp") = "Payment" Or R.Item("TranTyp") = "Supplemental Payment" Then
                    SchoolTotal = SchoolTotal + CDbl(R.Item("TranAmt"))
                End If
            Next


            'NCSP School Transmittal Letter NCSSCHTRAN
            For Each R In DSSum.Tables(0).Select("InstID = '" & Schools(x) & "'")
                If R.Item("TranTyp") = "Payment" Or R.Item("TranTyp") = "Supplemental Payment" Then
                    StudentTotal = CDbl(R.Item("TranAmt"))
                    FName = R.Item("FName")
                    LName = R.Item("LName")
                    SSN = R.Item("SSN")
                    Semester = R.Item("Semester")
                    SchedYr = R.Item("SchedYr")
                    CredHr = R.Item("CredHrEnr")
                    If CDbl(StudentTotal) > 0 Then
                        FileOpen(1, "T:\NCSP_LetterToSchool.txt", OpenMode.Append)
                        WriteLine(1, InstName, Contact, Add1, Add2, City, St, Zip, FormatCurrency(CDbl(SchoolTotal), 2), FName, LName, Format(CInt(SSN), "000-00-0000"), Semester, SchedYr, FormatCurrency(CDbl(StudentTotal), 2))
                        FileClose(1)
                    End If
                End If
            Next

            'PRINT NCSSCHTRAN
            '..............................
            If Directory.Exists(BatchDocPath & Format(Now, "MM_dd_yy")) = False Then Directory.CreateDirectory(BatchDocPath & Format(Now, "MM_dd_yy"))
            If GetNumLines("T:\NCSP_LetterToSchool.txt") > 1 Then PrintBatchDoc("NCSSCHTRAN", , "T:\NCSP_LetterToSchool.txt", BatchDocPath & Format(Now, "MM_dd_yy") & "\NCSSCHTRAN_" & BatchNum & "_" & Schools(x))
        Next x

        MsgBox("Batch number " & BatchNum & " has been created.  Please retrieve the check request(s) from the printer.", MsgBoxStyle.Information, "New Century Scholarship Program")
    End Sub

    Function GetNumLines(ByVal Pfile As String) As Integer
        Dim x As Integer
        x = 0
        FileOpen(1, Pfile, OpenMode.Input)
        Do While Not EOF(1)
            LineInput(1)
            x += 1
        Loop
        FileClose(1)
        Return x
    End Function

    Function RemoveAdjustmentOnlys(ByRef DS As DataSet) As DataSet
        Dim tempr As DataRow
        Dim hasPayment As Boolean
        'get list of unique accounts
        Dim AccountList As New ArrayList
        For Each tempr In DS.Tables(0).Rows
            If AccountList.IndexOf(tempr.Item("AcctID")) < 0 Then
                AccountList.Add(tempr.Item("AcctID"))
            End If
        Next
        Dim tempx As Integer
        'Loop for each account
        For tempx = 0 To AccountList.Count - 1
            Dim Total As Double
            Total = 0
            hasPayment = False
            'Loop for each row
            For Each tempr In DS.Tables(0).Rows
                If AccountList(tempx) = tempr.Item("AcctID") Then
                    If tempr.Item("TranTyp") = "Payment" Or tempr.Item("TranTyp") = "Supplemental Payment" Then
                        hasPayment = True
                    End If
                End If
            Next
            If hasPayment = False Then
                'if student does not have a payment then remove all records for account
                Dim Removed As Boolean
                Removed = True
                Do While Removed = True
                    Removed = False
                    For Each tempr In DS.Tables(0).Rows
                        If AccountList(tempx) = tempr.Item("AcctID") Then
                            DS.Tables(0).Rows.Remove(tempr)
                            Removed = True
                            Exit For
                        End If
                    Next
                Loop
            End If
        Next
    End Function

    Function RemoveZeroBalance(ByRef DS As DataSet) As DataSet
        Dim tempr As DataRow
        'get list of unique accounts
        Dim AccountList As New ArrayList
        For Each tempr In DS.Tables(0).Rows
            If AccountList.IndexOf(tempr.Item("AcctID")) < 0 Then
                AccountList.Add(tempr.Item("AcctID"))
            End If
        Next
        Dim tempx As Integer
        'Loop for each account
        For tempx = 0 To AccountList.Count - 1
            Dim Total As Double
            Total = 0
            'Loop for each row
            For Each tempr In DS.Tables(0).Rows
                If AccountList(tempx) = tempr.Item("AcctID") Then
                    Total = Total + CDbl(tempr.Item("TranAmt"))
                End If
            Next
            If Total <= 0 Then
                'if total = 0 then remove all records for account
                Dim Removed As Boolean
                Removed = True
                Do While Removed = True
                    Removed = False
                    For Each tempr In DS.Tables(0).Rows
                        If AccountList(tempx) = tempr.Item("AcctID") Then
                            DS.Tables(0).Rows.Remove(tempr)
                            Removed = True
                            Exit For
                        End If
                    Next
                Loop
            End If
        Next
    End Function


    Sub PrintBatchDoc(ByVal Doc As String, Optional ByVal PrintedPrompt As Boolean = True, Optional ByVal Dat As String = "T:\NCSPdat.txt", Optional ByVal SaveAs As String = "")
        Dim newDoc As String = String.Empty

        'set path and file name of merged document to save and of the merge document to use
        Doc = DocFolder & Doc & ".doc"

        'create the document
        Dim msWord As New Microsoft.Office.Interop.Word.Application
        msWord.Visible = False
        With msWord
            'open merge document
            msWord.Documents.Open(FileName:=Doc, ConfirmConversions:=False, _
                ReadOnly:=True, AddToRecentFiles:=False, PasswordDocument:="", _
                PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
                WritePasswordTemplate:="")
            'set data file
            msWord.ActiveDocument.MailMerge.OpenDataSource(Name:=Dat, _
                ConfirmConversions:=False, ReadOnly:= _
                False, LinkToSource:=True, AddToRecentFiles:=False, PasswordDocument:="", _
                PasswordTemplate:="", WritePasswordDocument:="", WritePasswordTemplate:= _
                "", Revert:=False, Connection:="", SQLStatement _
                :="", SQLStatement1:="")
            'perform merge
            With .ActiveDocument.MailMerge
                .Destination = .Destination.wdSendToNewDocument
                .SuppressBlankLines = True
                .Execute(Pause:=False)
            End With
            'close form file
            msWord.Documents(Doc).Close(False)
            'password protect and save the document
            msWord.ActiveDocument.Protect(2, , "DX854av")
            msWord.ActiveDocument.SaveAs(FileName:=SaveAs)
            'display document if in test mode
            If IsTestMode Then
                msWord.Visible = True
                'print document, close Word, and prompt user 
            Else
                msWord.ActiveDocument.PrintOut(Background:=False)
                msWord.Application.Quit(False)
                If PrintedPrompt Then MsgBox("Please retrieve your document from the printer.", MsgBoxStyle.Information, "New Century Scholarship Program")
            End If
        End With
    End Sub

    Function GetNextBatchNumber() As String
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String
        SC = New SqlClient.SqlCommand("Select max(NCSPBatchNum) as NCSPBatchNum from [Transaction] where NCSPBatchNum like '" & Format(Today, "MMddyy") & "%'", dbConnection)
        dbConnection.Open()
        sqlRdr = SC.ExecuteReader
        sqlRdr.Read()
        If sqlRdr("NCSPBatchNum").GetType.ToString = "System.DBNull" Then
            val = ""
        Else
            val = sqlRdr("NCSPBatchNum")
        End If

        sqlRdr.Close()
        dbConnection.Close()
        If val = "" Then
            val = Format(Today, "MMddyy") & "01"
        Else
            val = Format(Today, "MMddyy") & Format(CInt(Mid(val, 7)) + 1, "00")
        End If

        Return val
    End Function

End Class
