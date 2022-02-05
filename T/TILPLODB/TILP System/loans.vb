Imports System.Data.SqlClient

Public Class loans
    Private ALLoans As ArrayList
    Public SSN As String
    Private TestMode As Boolean
    Private Conn As SqlConnection
    Private Comm As SqlCommand
    Private Reader As SqlDataReader
    Public MaxSeqNum As Integer

    Public Sub New(ByVal tSSN As String, ByVal tTestmode As Boolean)
        SSN = tSSN
        TestMode = tTestmode
        MaxSeqNum = 0
        ALLoans = New ArrayList
        'set up DB connection
        If TestMode Then
            'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            'if in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        Comm = New SqlCommand
        Comm.Connection = Conn
    End Sub

    Public Function SlotProc(ByVal ID As Long, ByVal DisbAmt As Decimal, ByVal UserAccessLvl As Integer, ByVal LnStaChg2RejOrBorDecl As Boolean) As Boolean
        'Dim SlotSetUp As Boolean
        Dim OverrideSlotLimitation As Boolean
        If ID > 0 Then
            '**************************(Add Slot Only) or (Add Slot and Remove Slot)***************
            Comm.Connection.Open() 'open connection
            'check if disbursement amount will push school disbursed funds over it's allotment
            Comm.CommandText = "SELECT COALESCE(SUM(LoanDat.DisbAmount),0) AS TotalDisb, SchInfo.AwardAmtAvilblForYear FROM SchoolInformation SchInfo LEFT JOIN LoanDat ON (LoanDat.TermBeginDate BETWEEN SchInfo.YearBeginDt AND SchInfo.YearEndDt) AND SchInfo.SchoolCode = LoanDat.SchoolCode AND LoanDat.LoanStatus IN ('Disbursed','Pending Disbursement') WHERE SchInfo.[ID] = " + ID.ToString + " GROUP BY SchInfo.AwardAmtAvilblForYear"
            Reader = Comm.ExecuteReader()
            Reader.Read()
            If CDec(Reader("TotalDisb")) + DisbAmt > CDec(Reader("AwardAmtAvilblForYear")) Then
                MsgBox("The disbursement amount entered for the loan will push the school over it's allotted funds.", MsgBoxStyle.Critical, "Too Much Money!")
                Comm.Connection.Close() 'close connection
                Return False
            End If
            Reader.Close() 'close reader
            'slot processing can be done so true will be returned
            'check if borrower is already taking a slot for the school and year combo
            Comm.CommandText = "SELECT COALESCE(COUNT(*),0) AS TheResult FROM SchoolSlot WHERE SSN = '" + SSN + "' AND SchoolInfoID = " + ID.ToString + " AND SlotStatus = 'A'"
            If CInt(Comm.ExecuteScalar()) > 0 Then
                Comm.Connection.Close()
                Return True 'everything is set
            Else
                'check if adding another slot will push school slot number over it's allotment
                Comm.CommandText = "SELECT COALESCE(COUNT(SchoolSlot.RecNum),0) AS TotalSlots, SchInfo.SlotsAvilblForYear FROM SchoolInformation SchInfo LEFT JOIN SchoolSlot ON SchInfo.[ID] = SchoolSlot.SchoolInfoID AND SchoolSlot.SlotStatus = 'A' WHERE(SchInfo.[ID] = " + ID.ToString + ") GROUP BY SchInfo.SlotsAvilblForYear"
                Reader = Comm.ExecuteReader()
                Reader.Read()
                If CLng(Reader("TotalSlots")) + 1 > CLng(Reader("SlotsAvilblForYear")) Then
                    'adding another slot will be too many
                    OverrideSlotLimitation = False
                    If UserAccessLvl = 1 Then
                        'if the user has access level one then allow them to override the limitation
                        If MsgBox("The adding of this loan will require the borrower to use one of the school's slots, but the school does not have any more slots availible.  Do you want to override this limitation", MsgBoxStyle.YesNo + MsgBoxStyle.Critical, "Too Many People!") = MsgBoxResult.Yes Then
                            'user chose to override limitation
                            OverrideSlotLimitation = True
                        End If
                    Else
                        MsgBox("The adding of this loan will require the borrower to use one of the school's slots, but the school does not have any more slots availible.  The loan can't be added.  Please contact the manager of Account Services.", MsgBoxStyle.Critical, "Too Many People!")
                    End If
                    'if slot limitation was never overridden then exit sub with out further processing
                    If OverrideSlotLimitation = False Then
                        Comm.Connection.Close() 'close connection
                        Return False
                    End If
                End If
                Reader.Close() 'close reader
                'update all other slots held by the borrower to inactive and then create one for new school and year combo
                Comm.CommandText = "UPDATE SchoolSlot SET SlotStatus = 'I' WHERE SSN = '" + SSN + "'"
                Comm.ExecuteNonQuery()
                Comm.CommandText = "INSERT INTO SchoolSlot (SchoolInfoID, SSN, SlotStartDt) VALUES (" + ID.ToString + ",'" + SSN + "', GETDATE())"
                Comm.ExecuteNonQuery()
                Comm.Connection.Close()
                Return True
            End If
        Else
            '*************************************Remove slot only**********************************
            Comm.Connection.Open() 'open connection
            'update all slots held by the borrower to inactive
            Comm.CommandText = "UPDATE SchoolSlot SET SlotStatus = 'I' WHERE SSN = '" + SSN + "'"
            Comm.ExecuteNonQuery()
            Comm.Connection.Close()
            Return True
        End If
    End Function

    'this function sums all disbursed loans for a specific year
    Public Function GetTotalSumOfLoansForYear(ByVal YearBegin As String, ByVal YearEnd As String) As Decimal
        Comm.Connection.Open() 'open connection
        Comm.CommandText = "SELECT COALESCE(SUM(DisbAmount),0) as SumOfLoans FROM LoanDat WHERE TermBeginDate BETWEEN CAST('" + Format(CDate(YearBegin), "MM/dd/yyyy") + "' as DateTime) AND CAST('" + Format(CDate(YearEnd), "MM/dd/yyyy") + "' as DateTime) AND SSN = '" + SSN + "' AND LoanStatus = 'Disbursed'"
        GetTotalSumOfLoansForYear = CDec(Comm.ExecuteScalar())
        Comm.Connection.Close()
    End Function

    'this function gets the slot information
    Public Function RetSchoolSlotInfo(ByVal SchoolCode As String, ByVal TermBegin As String, ByRef YearBegin As String, ByRef YearEnd As String, ByRef AmtPerSlot As Decimal, ByRef NewBorrowerEligible As Boolean, ByRef ID As Long, ByRef AwardAmtAvilblForYear As Decimal) As Boolean
        Comm.Connection.Open() 'open connection
        Comm.CommandText = "SELECT [ID], YearBeginDt, YearEndDt, DollarAmtPerSlot, EligibleForNewLoans, AwardAmtAvilblForYear FROM SchoolInformation WHERE CAST('" + Format(CDate(TermBegin), "MM/dd/yyyy") + "' as DateTime) BETWEEN YearBeginDt AND YearEndDt AND SchoolCode = '" + SchoolCode + "'"
        Reader = Comm.ExecuteReader()
        If Reader.Read = False Then
            'school information not found
            Comm.Connection.Close()
            Return False
        Else
            'school information found
            ID = Reader("ID")
            YearBegin = Reader("YearBeginDt")
            YearEnd = Reader("YearEndDt")
            AmtPerSlot = Reader("DollarAmtPerSlot")
            NewBorrowerEligible = (Reader("EligibleForNewLoans") = "Y")
            AwardAmtAvilblForYear = Reader("AwardAmtAvilblForYear")
            Comm.Connection.Close()
            Return True
        End If
    End Function

    Public Function AddLoanEligible() As Boolean
        Comm.Connection.Open() 'open connection
        Comm.CommandText = "SELECT COUNT(*) FROM LoanDat WHERE SSN = '" & SSN & "' AND LOANSTATUS IN ('Pending Disbursement','Disbursed')"
        If CLng(Comm.ExecuteScalar()) >= 8 Then
            'not eligible
            Comm.Connection.Close() 'close connection
            Return False
        Else
            'eligible
            Comm.Connection.Close() 'close connection
            Return True
        End If
    End Function

    'loads data into array list for form
    Public Sub GetData(ByRef TheLoans As ArrayList)
        Dim I As Integer
        Dim L As loan
        TheLoans = New ArrayList
        While ALLoans.Count > I
            L = New loan
            L.DisbAmt = CType(ALLoans(I), loan).DisbAmt
            L.DisbDt = CType(ALLoans(I), loan).DisbDt
            L.EnrolSta = CType(ALLoans(I), loan).EnrolSta
            L.IntRate = CType(ALLoans(I), loan).IntRate
            L.LnSta = CType(ALLoans(I), loan).LnSta
            L.SchCode = CType(ALLoans(I), loan).SchCode
            L.SchName = CType(ALLoans(I), loan).SchName
            L.SeqNum = CType(ALLoans(I), loan).SeqNum
            L.Term = CType(ALLoans(I), loan).Term
            L.TermBeginDt = CType(ALLoans(I), loan).TermBeginDt
            L.TermEndDt = CType(ALLoans(I), loan).TermEndDt
            L.AlreadyExisting = CType(ALLoans(I), loan).AlreadyExisting
            TheLoans.Add(L)
            I = I + 1
        End While
    End Sub

    'loads data from DB
    Public Sub DoDBLoad()
        Dim L As loan
        Comm.Connection.Open()
        Comm.CommandText = "SELECT * FROM LoanDat A JOIN ParticipatingSchoolsList B ON A.SchoolCode = B.SchoolCode WHERE A.SSN = '" + SSN + "' ORDER BY A.LoanSeq"
        Reader = Comm.ExecuteReader
        'cycle through all records and load into objects
        While Reader.Read
            L = New loan
            L.SeqNum = CInt(Reader("LoanSeq"))
            L.LnSta = Reader("LoanStatus").ToString()
            L.SchName = Reader("SchoolName").ToString()
            L.SchCode = Reader("SchoolCode").ToString()
            L.IntRate = CDbl(Reader("IntRate"))
            L.DisbDt = Reader("DisbDate")
            L.DisbAmt = Format(CDbl(Reader("DisbAmount").ToString().Replace(",", "").Replace("$", "")), "######0.00")
            L.TermBeginDt = Reader("TermBeginDate").ToString()
            L.TermEndDt = Reader("TermEndDate").ToString()
            L.Term = Reader("Term").ToString()
            L.EnrolSta = Reader("EnrStatus").ToString()
            L.AlreadyExisting = True 'only update the DB if record gets modified
            'figure out max seq num for borrower
            If MaxSeqNum < CInt(Reader("LoanSeq")) Then
                MaxSeqNum = CInt(Reader("LoanSeq"))
            End If
            ALLoans.Add(L) 'add object to array list
        End While
        Reader.Close()
        Comm.Connection.Close()
    End Sub

    Public Sub SaveLoanData(ByRef TheLoans As ArrayList, ByVal UID As String)
        UpdateObjectData(TheLoans)
        UpdateDB(UID)
    End Sub

    'this sub updates the DB from the updated object information
    Private Sub UpdateDB(ByVal UID As String)
        Dim I As Integer
        Conn.Open()
        While I < ALLoans.Count
            'handle updates
            If CType(ALLoans(I), loan).Modified Then
                Comm.CommandText = "UPDATE LoanDat SET LoanStatus = '" + CType(ALLoans(I), loan).LnSta + "', SchoolCode = '" + CType(ALLoans(I), loan).SchCode + "', IntRate = " + CType(ALLoans(I), loan).IntRate.ToString + ", DisbDate = '" + CType(ALLoans(I), loan).DisbDt + "', DisbAmount = " + CType(ALLoans(I), loan).DisbAmt + ", TermBeginDate = '" + CType(ALLoans(I), loan).TermBeginDt + "', TermEndDate = '" + CType(ALLoans(I), loan).TermEndDt + "', Term = '" + CType(ALLoans(I), loan).Term + "', EnrStatus = '" + CType(ALLoans(I), loan).EnrolSta + "' WHERE SSN = '" + SSN + "' AND LoanSeq = " + CType(ALLoans(I), loan).SeqNum.ToString
                Comm.ExecuteNonQuery()
            ElseIf CType(ALLoans(I), loan).AlreadyExisting = False Then
                Comm.CommandText = "INSERT INTO LoanDat (SSN, LoanSeq, LoanStatus, SchoolCode, IntRate, DisbDate, DisbAmount, TermBeginDate, TermEndDate, Term, EnrStatus) VALUES ('" + SSN + "', " + CType(ALLoans(I), loan).SeqNum.ToString() + ", '" + CType(ALLoans(I), loan).LnSta + "', '" + CType(ALLoans(I), loan).SchCode + "', " + CType(ALLoans(I), loan).IntRate.ToString + ", '" + CType(ALLoans(I), loan).DisbDt + "', " + CType(ALLoans(I), loan).DisbAmt + ", '" + CType(ALLoans(I), loan).TermBeginDt + "', '" + CType(ALLoans(I), loan).TermEndDt + "', '" + CType(ALLoans(I), loan).Term + "', '" + CType(ALLoans(I), loan).EnrolSta + "')"
                Comm.ExecuteNonQuery()
            End If
            'figure out max seq num for borrower
            If MaxSeqNum < CType(ALLoans(I), loan).SeqNum Then
                MaxSeqNum = CType(ALLoans(I), loan).SeqNum
            End If
            'add activity comment if loan status changed
            If CType(ALLoans(I), loan).StatusChanged <> "" Then
                Comm.CommandText = "INSERT INTO ActivityDat (SSN, ActivitySeq, UserID, ActivityText) VALUES ('" + SSN + "', " + borrower.NextActivitySeqNum(TestMode, SSN).ToString + ", '" + UID + "', '" + "LN SEQ:" + CType(ALLoans(I), loan).SeqNum.ToString + "; NEW LN STATUS:" + CType(ALLoans(I), loan).LnSta + "; OLD LN STATUS:" + CType(ALLoans(I), loan).StatusChanged + "; DISB AMT:" + CType(ALLoans(I), loan).DisbAmt + "; ENRL STATUS:" + CType(ALLoans(I), loan).EnrolSta + "; TERM BEGIN:" + CType(ALLoans(I), loan).TermBeginDt + "; TERM END:" + CType(ALLoans(I), loan).TermEndDt + "; SCHOOL CD:" + CType(ALLoans(I), loan).SchCode + "; INT RATE:" + CType(ALLoans(I), loan).IntRate.ToString + "%')"
                Comm.ExecuteNonQuery()
            End If
            'reset flags
            CType(ALLoans(I), loan).AlreadyExisting = True
            CType(ALLoans(I), loan).Modified = False
            CType(ALLoans(I), loan).StatusChanged = ""
            I = I + 1
        End While
        Conn.Close()
    End Sub

    'this function takes information that has been updated or added by the user and updates/inserts them into the object array list of loans
    Private Sub UpdateObjectData(ByRef TheLoans As ArrayList)
        Dim I As Integer
        Dim II As Integer
        While I < TheLoans.Count
            If CType(TheLoans(I), loan).Modified Then
                II = 0
                'find loan to update
                While CType(TheLoans(I), loan).SeqNum <> CType(ALLoans(II), loan).SeqNum
                    II = II + 1
                End While
                'check if status changed
                If CType(TheLoans(I), loan).LnSta <> CType(ALLoans(II), loan).LnSta Then
                    'if it has changed then hold previous status for activity comments
                    CType(TheLoans(I), loan).StatusChanged = CType(ALLoans(II), loan).LnSta
                End If
                ALLoans(II) = TheLoans(I) 'replace old loan with new loan
            ElseIf CType(TheLoans(I), loan).AlreadyExisting = False Then
                ALLoans.Add(TheLoans(I)) 'add loan to array list
            End If
            I = I + 1
        End While
    End Sub

End Class

Public Class loan
    Public SeqNum As Integer
    Public SchName As String
    Public SchCode As String
    Public IntRate As Double
    Public DisbDt As String
    Public DisbAmt As String
    Public LnSta As String
    Public EnrolSta As String
    Public Term As String
    Public TermYear As String
    Public TermBeginDt As String
    Public TermEndDt As String
    'flags 
    Public AlreadyExisting As Boolean
    Public Modified As Boolean
    Public StatusChanged As String

    Public Sub New()
        AlreadyExisting = False
        Modified = False
        StatusChanged = ""
        SeqNum = 0
        SchName = ""
        SchCode = ""
        IntRate = 0
        DisbDt = ""
        DisbAmt = ""
        LnSta = ""
        EnrolSta = ""
        Term = ""
        TermBeginDt = ""
        TermEndDt = ""
        TermYear = ""
    End Sub

End Class
