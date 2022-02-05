Module ProcessRules
    'do the processing for the rules
    Private Function ProcessRules(ByVal DS As DataSet, ByVal eligEndedReason As String, ByVal progressBarText As String)
        Dim DR As DataRow
        Dim letterText As String

        'get text for letter
        letterText = GetLetterText(eligEndedReason)

        'update progress bar
        ResetProgressBar(DS.Tables(0).Rows.Count, progressBarText)

        'update the account, add a communications record, and write the data to the letter data file
        For Each DR In DS.Tables(0).Rows
            UpdateAddWriteSave(DR, eligEndedReason, letterText)
            SplashForm.ProgressBar.PerformStep()
        Next
    End Function

    Public Function ProcessFallSemesterRule()
        'SELECT	DISTINCT A.AcctID, A.LName, A.FName, A.Add1, A.Add2, A.City, A.ST, A.Zip, A.Balance
        'FROM	dbo.Account A 
        'WHERE	Year(A.HSGradDt) = Year(GETDATE()) 
        '  AND A.Status <> 'Closed'
        '  AND A.RowStatus = 'A'
        '  AND (A.LOAStart IS NULL OR A.LOAApproved = 0 OR A.LOAStart > GETDATE() OR DATEDIFF(day, A.LOAEnd, GETDATE()) > 60)
        '  AND NOT EXISTS (SELECT	B.*
        '      FROM	dbo.Schedule B
        '      WHERE	B.Semester = 'Fall'
        '        AND B.SchedYr = Year(GETDATE())
        '        AND (B.CredHrEnr >= 12 OR LTH <> 0)
        '        AND A.AcctID = B.AcctID
        '        AND B.RowStatus = 'A')

        'TODO:  The query above needs to be changed as follows:
        'if the account status is 'Pending' then SchedDt <= Oct 15, send NSCINCFILE letter
        'if the acccout status is 'Approved' then SchedDt <= Sep 30, send Account Closed letter with ineligible reason of 'Not Enrolled by Fall' (current process)
        'review other rules to verify the edits account for the deadline

        'run the process if it is due to run
        If Month(Today) = 10 Then
            Dim DS As New DataSet

            DS = GetDataSet("SELECT DISTINCT A.AcctID, A.LName, A.FName, A.Add1, A.Add2, A.City, A.ST, A.Zip, A.Balance FROM dbo.Account A WHERE(Year(A.HSGradDt) = Year(GETDATE())) AND A.RowStatus = 'A' AND (A.LOAStart IS NULL OR A.LOAApproved = 0 OR A.LOAStart > GETDATE() OR DATEDIFF(day, A.LOAEnd, GETDATE()) > 60) AND NOT EXISTS (SELECT	B.* FROM dbo.Schedule B	WHERE B.Semester = 'Fall' AND B.SchedYr = Year(GETDATE()) AND (B.CredHrEnr >= 12 OR LTH <> 0) AND A.AcctID = B.AcctID AND B.RowStatus = 'A')")
            ProcessRules(DS, "Not Enrolled by Fall", "Processing enrolled by fall semester rule.")
        End If
    End Function

    'process the proof of enrollment rule
    Public Function ProcessProofofEnrollment()
        'SELECT	DISTINCT A.AcctID, A.LName, A.FName, A.Add1, A.Add2, A.City, A.ST, A.Zip, A.Balance, C.ScheduleCount
        'FROM	Account A
        '		LEFT OUTER JOIN (
        '					SELECT	DISTINCT B.AcctID, COUNT(B.Semester) AS ScheduleCount
        '					FROM	Schedule B
        '					WHERE	B.SchedStat IN ('Paid','Payment Pending')
        '							AND (B.CredHrEnr >= 12 OR B.LTH <> 0)
        '							AND B.RowStatus = 'A'
        '							AND

        '-- semesterEdit - Feb 15 deadline
        '--	(
        '--		(B.InstAtt = 'Brigham Young University'
        '--		AND (
        '--			(B.Semester IN ('Spring','Summer','Fall') AND B.SchedYr = YEAR(GETDATE()))
        '--			OR
        '--			(B.Semester IN ('Winter') AND B.SchedYr = YEAR(GETDATE()) + 1)
        '--			)
        '--		)
        '--	OR
        '--		(B.InstAtt <> 'Brigham Young University'
        '--		AND (
        '--			(B.Semester IN ('Summer','Fall') AND B.SchedYr = YEAR(GETDATE()))
        '--			OR
        '--			(B.Semester IN ('Spring') AND B.SchedYr = YEAR(GETDATE()) + 1)
        '--			)
        '--		)
        '--	)

        '-- semesterEdit - Sep 30 deadline
        '--	(
        '--		(B.InstAtt = 'Brigham Young University' AND B.Semester IN ('Spring','Summer','Fall') AND B.SchedYr = YEAR(GETDATE()))
        '--	OR
        '--		(B.InstAtt <> 'Brigham Young University' AND B.Semester IN ('Summer','Fall') AND B.SchedYr = YEAR(GETDATE()) + 1)
        '--	)
        '					GROUP BY B.AcctID
        '					) C
        '			ON A.AcctID = C.AcctID
        'WHERE	A.RowStatus = 'A'
        '		AND A.Status = 'Approved'
        '		AND (C.ScheduleCount < 2 OR C.ScheduleCount IS NULL)
        '		AND (A.LOAApproved = 0 OR A.LOASemReturn = '' OR

        '-- loaEdit - Feb 15 deadline 
        '--			(
        '--				(A.LOASemReturn IN ('Fall') AND A.LOAYearReturn < YEAR(GETDATE()))
        '--			OR
        '--				(A.LOASemReturn IN ('Spring', 'Winter') AND A.LOAYearReturn <= YEAR(GETDATE()))
        '--			)

        '-- loaEdit - Sep 30 deadline 
        '--			A.LOAYearReturn <= YEAR(GETDATE())

        '			)

        Dim semesterEdit As String = ""
        Dim loaEdit As String = ""
        Dim semesterCount As Integer = 0

        'determine edits based on month run
        Select Case Month(Today)
            Case 2 'Feb 15 deadline
                semesterEdit = "((B.InstAtt = 'Brigham Young University' AND ((B.Semester IN ('Spring','Summer','Fall') AND B.SchedYr = " & Year(Today) - 1 & ") OR (B.Semester IN ('Winter') AND B.SchedYr = " & Year(Today) & "))) OR (B.InstAtt <> 'Brigham Young University' AND ((B.Semester IN ('Summer','Fall') AND B.SchedYr = " & Year(Today) - 1 & ") OR (B.Semester IN ('Spring') AND B.SchedYr = " & Year(Today) & "))))"
                loaEdit = "((A.LOASemReturn IN ('Fall') AND A.LOAYearReturn < YEAR(GETDATE())) OR (A.LOASemReturn IN ('Spring', 'Winter') AND A.LOAYearReturn <= YEAR(GETDATE())))"
                semesterCount = 2
            Case 10 'Sep 30 deadline
                semesterEdit = "((B.InstAtt = 'Brigham Young University' AND B.Semester IN ('Spring','Summer','Fall') AND B.SchedYr = " & Year(Today) & ") OR (B.InstAtt <> 'Brigham Young University' AND B.Semester IN ('Summer','Fall') AND B.SchedYr = " & Year(Today) & "))"
                loaEdit = "A.LOAYearReturn <= YEAR(GETDATE())"
                semesterCount = 1
            Case Else 'no processing needed
                Exit Function
        End Select

        Dim DS As New DataSet
        DS = GetDataSet("SELECT DISTINCT A.AcctID, A.LName, A.FName, A.Add1, A.Add2, A.City, A.ST, A.Zip, A.Balance, C.ScheduleCount FROM Account A LEFT OUTER JOIN (SELECT DISTINCT B.AcctID, COUNT(B.Semester) AS ScheduleCount FROM Schedule B WHERE B.SchedStat IN ('Paid','Payment Pending') AND (B.CredHrEnr >= 12 OR B.LTH <> 0) AND B.RowStatus = 'A' AND " & semesterEdit & " GROUP BY B.AcctID) C ON A.AcctID = C.AcctID WHERE A.RowStatus = 'A' AND A.Status = 'Approved' AND (C.ScheduleCount < " & semesterCount & " OR C.ScheduleCount IS NULL) AND (A.LOAApproved = 0 OR A.LOASemReturn = '' OR " & loaEdit & ")")
        ProcessRules(DS, "Schedule Received Late", "Processing proof of enrollment rule.")
    End Function

    'process proof of completion rule
    Public Function ProcessProofofCompletion()
        'SELECT	DISTINCT A.AcctID, A.LName, A.FName, A.Add1, A.Add2, A.City, A.ST, A.Zip, A.Balance 
        'FROM	Account A
        '		INNER JOIN Schedule B
        '			ON A.AcctID = B.AcctID
        'WHERE	A.RowStatus = 'A'
        '		AND A.Status = 'Approved'
        '		AND B.SchedStat IN ('Paid','Payment Pending')
        '		AND (B.CredHrEnr >= 12 OR LTH <> 0)
        '		AND (CredHrComp = 0 OR CredHrComp IS NULL OR SemesterGPA = 0 OR SemesterGPA IS NULL)
        '		AND B.RowStatus = 'A'
        '       AND

        '--Feb 15 deadline
        '--B.Semester = 'Fall' AND B.SchedYr = YEAR(GETDATE()) - 1

        '--May 30 deadline
        '--B.Semester = 'Winter' AND B.InstAtt = 'Brigham Young University' AND B.SchedYr = YEAR(GETDATE())

        '--Jun 30 deadline
        '--B.Semester = 'Spring' AND B.InstAtt <> 'Brigham Young University' AND B.SchedYr = YEAR(GETDATE())

        '--Jul 30 deadline
        '--B.Semester = 'Summer' AND B.InstAtt = 'Brigham Young University' AND B.SchedYr = YEAR(GETDATE())

        '--Sep 30 deadline
        '--B.Semester = 'Summer' AND B.InstAtt <> 'Brigham Young University' AND B.SchedYr = YEAR(GETDATE())

        Dim semesterEdit As String = ""

        'determine edits based on month run
        Select Case Month(Today)
            Case 2 'Feb 15 deadline
                semesterEdit = "B.Semester = 'Fall' AND B.SchedYr = YEAR(GETDATE()) - 1"
            Case 6 'May 30 deadline
                semesterEdit = "B.Semester = 'Winter' AND B.InstAtt = 'Brigham Young University' AND B.SchedYr = YEAR(GETDATE())"
            Case 7 'Jun 30 deadline
                semesterEdit = "B.Semester = 'Spring' AND B.InstAtt <> 'Brigham Young University' AND B.SchedYr = YEAR(GETDATE())"
            Case 8 'Jul 30 deadline
                semesterEdit = "B.Semester = 'Summer' AND B.InstAtt = 'Brigham Young University' AND B.SchedYr = YEAR(GETDATE())"
            Case 10 'Sep 30 deadline
                semesterEdit = "B.Semester = 'Summer' AND B.InstAtt <> 'Brigham Young University' AND B.SchedYr = YEAR(GETDATE())"
            Case Else 'no processing needed
                Exit Function
        End Select

        Dim DS As New DataSet
        DS = GetDataSet("SELECT DISTINCT A.AcctID, A.LName, A.FName, A.Add1, A.Add2, A.City, A.ST, A.Zip, A.Balance FROM Account A	INNER JOIN Schedule B ON A.AcctID = B.AcctID WHERE A.RowStatus = 'A' AND A.Status = 'Approved' AND B.SchedStat IN ('Paid','Payment Pending') AND " & semesterEdit & " AND (B.CredHrEnr >= 12 OR LTH <> 0) AND (CredHrComp = 0 OR CredHrComp IS NULL OR SemesterGPA = 0 OR SemesterGPA IS NULL) AND B.RowStatus = 'A'")
        ProcessRules(DS, "Grades Received Late", "Processing proof of completion rule.")
    End Function
End Module
