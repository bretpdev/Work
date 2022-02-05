Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Imports SP.Q
Imports Q
Imports System.Data.SqlClient

Public Class BorrowerBS
    Inherits SP.Borrower

#Region "Class variables and properties"
    Private _isVIP As Boolean = False
    Public ReadOnly Property IsVIP() As Boolean
        Get
            Return _isVIP
        End Get
    End Property

    Private _specialHandlingIsRequired As Boolean = False
    Public ReadOnly Property SpecialHandlingIsRequired() As Boolean
        Get
            Return _specialHandlingIsRequired
        End Get
    End Property

    Private _isRehabilitated As Boolean = False
    Public ReadOnly Property IsRehabilitated() As Boolean
        Get
            Return _isRehabilitated
        End Get
    End Property

    Private _isRepurchased As Boolean = False
    Public ReadOnly Property IsRepurchased() As Boolean
        Get
            Return _isRepurchased
        End Get
    End Property

    Private _loans As New List(Of Loan)
    Public ReadOnly Property Loans() As List(Of Loan)
        Get
            Return _loans
        End Get
    End Property

    Private _installments As New List(Of Installment)()
    Public ReadOnly Property Installments() As List(Of Installment) 'Installments(0,x) =Amount, 1 = 1st due date
        Get
            Return _installments
        End Get
    End Property

    Private _lastContactDateText As String = ""
    Public ReadOnly Property LastContactDateText() As String
        Get
            Return _lastContactDateText
        End Get
    End Property

    Private _lastAttemptedContactDateText As String = ""
    Public ReadOnly Property LastAttemptedContactDateText() As String
        Get
            Return _lastAttemptedContactDateText
        End Get
    End Property

    Private _twentyDateLetter As New TwentyDayLetter()
    Public ReadOnly Property TwentyDayLetter() As TwentyDayLetter
        Get
            Return _twentyDateLetter
        End Get
    End Property

    Private _totalPayoffAmount As Double = 0
    Public ReadOnly Property TotalPayoffAmount() As Double
        Get
            Return _totalPayoffAmount
        End Get
    End Property

    Private _totalPrincipalDue As Double = 0
    Public ReadOnly Property TotalPrincipalDue() As Double
        Get
            Return _totalPrincipalDue
        End Get
    End Property

    Private _totalInterestDue As Double = 0
    Public ReadOnly Property TotalInterestDue() As Double
        Get
            Return _totalInterestDue
        End Get
    End Property

    Private _totalDailyInterest As Double = 0
    Public ReadOnly Property TotalDailyInterest() As Double
        Get
            Return _totalDailyInterest
        End Get
    End Property

    Private _totalAmountPastDue As Double = 0
    Public ReadOnly Property TotalAmountPastDue() As Double
        Get
            Return _totalAmountPastDue
        End Get
    End Property

    Private _lastPayment As New Payment()
    Public ReadOnly Property LastPayment() As Payment
        Get
            Return _lastPayment
        End Get
    End Property

    'ACP
    Private _loanStatus As String = ""
    Public ReadOnly Property LoanStatus() As String
        Get
            Return _loanStatus
        End Get
    End Property

    Private _currentPrincipalBalance As Double = 0

    Private _interestRateText As String = ""
    Public ReadOnly Property InterestRateText() As String
        Get
            Return _interestRateText
        End Get
    End Property

    Private _disbursementDateText As String = ""
    Public ReadOnly Property DisbursementDateText() As String
        Get
            Return _disbursementDateText
        End Get
    End Property

    Private _loanProgram As String = ""
    Public ReadOnly Property LoanProgram() As String
        Get
            Return _loanProgram
        End Get
    End Property

    Private _nextDueDateText As String = ""
    Public ReadOnly Property NextDueDateText() As String
        Get
            Return _nextDueDateText
        End Get
    End Property

    Private _billingDueDayText As String = ""
    Public ReadOnly Property BillingDueDayText() As String
        Get
            Return _billingDueDayText
        End Get
    End Property

    Private _statutoryInterestRates As New List(Of LoanInterestRate)()
    Public ReadOnly Property StatutoryInterestRates() As List(Of LoanInterestRate)
        Get
            Return _statutoryInterestRates
        End Get
    End Property

    Private _amountDueText As String = ""

    Private _firstScreen As String

    Private _existsInLco As Boolean = False
    Public ReadOnly Property ExistsInLco() As Boolean
        Get
            Return _existsInLco
        End Get
    End Property

    Private _hasSuspension As Boolean = False
    Public ReadOnly Property HasSuspension() As Boolean
        Get
            Return _hasSuspension
        End Get
    End Property

    Private _noteDudeNotesForOneLink As String = "" 'needed for OneLINK note dude functionality (BS Only)
    Public Property NoteDudeNotesForOneLink() As String
        Get
            Return _noteDudeNotesForOneLink
        End Get
        Set(ByVal value As String)
            _noteDudeNotesForOneLink = value
        End Set
    End Property

    Private _fullStatusDescriptions As New List(Of String)()
    Public ReadOnly Property FullStatusDescriptions() As List(Of String)
        Get
            Return _fullStatusDescriptions
        End Get
    End Property

    Private _privateLoansDataTable As New DataTable()
    Public ReadOnly Property PrivateLoansDataTable() As DataTable
        Get
            Return _privateLoansDataTable
        End Get
    End Property

    Private _hasPrivateLoan As Boolean = False
    Public ReadOnly Property HasPrivateLoan() As Boolean
        Get
            Return _hasPrivateLoan
        End Get
    End Property

    Private _thirdPartyListViewItems As New List(Of ThirdPartyListViewItem)()
    Public ReadOnly Property ThirdPartyListViewItems() As List(Of ThirdPartyListViewItem)
        Get
            Return _thirdPartyListViewItems
        End Get
    End Property

    ''' <summary>
    ''' Only access through BorrowerBS properties.  This is an object that can be used for values outside the normal borrower object that is specific to homepage.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ScriptSpecificInfo() As MDScriptInfoSpecificToCustomerService
        Get
            Return CType(ScriptInfoToGenericBusinessUnit, MDScriptInfoSpecificToCustomerService)
        End Get
        Set(ByVal value As MDScriptInfoSpecificToCustomerService)
            ScriptInfoToGenericBusinessUnit = CType(value, MDScriptInfoSpecificToCustomerService)
        End Set
    End Property

    ''' <summary>
    ''' Has repayment schedule indicator.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HasRepaymentSchedule() As String
        Get
            Return ScriptSpecificInfo.HasRepaymentSchedule
        End Get
        Set(ByVal value As String)
            ScriptSpecificInfo.HasRepaymentSchedule = value
        End Set
    End Property

    ''' <summary>
    ''' Current amount due.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CurrentAmountDue() As Double
        Get
            Return ScriptSpecificInfo.CurrentAmountDue
        End Get
        Set(ByVal value As Double)
            ScriptSpecificInfo.CurrentAmountDue = value
        End Set
    End Property

    ''' <summary>
    ''' Outstanding late fees.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OutstandingLateFees() As Double
        Get
            Return ScriptSpecificInfo.OutstandingLateFees
        End Get
        Set(ByVal value As Double)
            ScriptSpecificInfo.OutstandingLateFees = value
        End Set
    End Property

    ''' <summary>
    ''' Monthly Payment Amount.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MonthlyPaymentAmount() As String
        Get
            Return ScriptSpecificInfo.MonthlyPaymentAmount
        End Get
        Set(ByVal value As String)
            ScriptSpecificInfo.MonthlyPaymentAmount = value
        End Set
    End Property

#End Region 'Class variables and properties

    Public Sub New(ByVal liteBorrower As SP.BorrowerLite)
        MyBase.New(liteBorrower)
        ScriptInfoToGenericBusinessUnit = New MDScriptInfoSpecificToCustomerService 'create one specific to Customer Service
    End Sub

    Public Overridable Sub SetPayoffAmountForGivenPayoffDate(ByVal payoffDate As DateTime)
        FastPath("TX3Z/ITS2O" + SSN)
        PutText(7, 26, payoffDate.ToString("MM"))
        PutText(7, 29, payoffDate.ToString("dd"))
        PutText(7, 32, payoffDate.ToString("yy"))
        PutText(9, 16, "X")
        PutText(9, 54, "Y", True)
        Hit("ENTER")
        _totalPayoffAmount = Double.Parse(GetText(12, 29, 10))
    End Sub

    Public Overridable Sub EnterAcpAndCheckThirdParties(ByVal homePageForm As Form)
        If Check4Text(1, 72, "TCX06") Then
            MyBase.ShowTCX06Values = True
            If Not Check4Text(24, 26, "F4=PYOF") Then SP.Q.Hit("F2")
            Hit("F4")
            PutText(7, 26, DateTime.Now.ToString("MMddyy"))
            PutText(9, 16, "X")
            PutText(9, 54, "Y", True)
            Hit("ENTER")
            _totalPayoffAmount = Double.Parse(GetText(12, 29, 10))
            _totalPrincipalDue = Double.Parse(GetText(14, 29, 10))
            _totalInterestDue = Double.Parse(GetText(15, 29, 10))
            _totalDailyInterest = Double.Parse(GetText(17, 29, 10))
            _totalAmountPastDue = Double.Parse(GetText(18, 29, 10))
            MyBase.TotalLateFeesDue = Double.Parse(GetText(19, 29, 10))
            Hit("F12")
            Hit("F12")
            _lastPayment.ReceivedDate = GetText(11, 69, 8)
            _lastPayment.Amount = Double.Parse(GetText(10, 67, 10))
        Else
            MyBase.ShowTCX06Values = False
        End If
        CType(homePageForm, frmBSHomePage).SetDisplayForNoAcpBsvCall(MyBase.BorLite.NoACPBSVCall)
        CType(homePageForm, frmBSHomePage).SetThirdParty(True)
        If Check4Text(1, 72, "TCX0I") AndAlso (ActivityCode <> "AM" AndAlso ContactCode <> "10") Then
            If Not Check4Text(24, 26, "F4=REL") Then Hit("F2")
            Hit("F4")
            If Check4Text(1, 74, "TXX1Y") Then
                'Prompt the user to verify ‘Yes’ or ‘No’ whether the caller is an authorized 3rd party
                SP.Processing.Visible = False
                If SP.frmYesNo.YesNo("Is the caller an authorized 3rd party?") Then
                    'Caller is authorized 3rd party
                    MyBase.Found3rd = False
                    Dim alreadyLoopedOnce As Boolean = False
                    Do While MyBase.Found3rd = False
                        Do While Check4Text(1, 74, "TXX1Y") = False
                            SP.frmWhoaDUDE.WhoaDUDE("No way DUDE! You gota be on the Borrower Relationships Screen(TXX1Y), if you wana keep surfing. Press 'OK' when you are there.", "Bad Screen", True)
                        Loop
                        If alreadyLoopedOnce Then
                            If SP.frmYesNo.YesNo("Is the caller an authorized 3rd party?") = False Then
                                'Not Authorized
                                CType(homePageForm, frmBSHomePage).SetThirdParty(False)
                                Exit Do
                            End If
                        End If
                        While Check4Text(23, 2, "90007") = False
                            For row As Integer = 10 To 21
                                If Check4Text(row, 49, "Y") Then
                                    MyBase.Found3rd = True
                                    'collect authorized third party information
                                    ThirdPartyListViewItems.Add(New ThirdPartyListViewItem(GetText(row, 5, 1), GetText(row, 23, 23), GetText(row, 67, 10)))
                                End If
                            Next row
                            SP.Q.Hit("F8")
                        End While
                        If MyBase.Found3rd = False Then
                            'warn the user that there is no authorized 3rd party and re-pause the script for the user to make the appropriate updates
                            SP.frmWhoaDUDE.WhoaDUDE("Are you lolo? There is no authorized 3rd party. How about you make it mo betta and press 'OK' when you are ready.", "No Third Party Found.", True)
                        End If
                        alreadyLoopedOnce = True
                    Loop
                Else
                    'Not Authorized
                    CType(homePageForm, frmBSHomePage).SetThirdParty(False)
                End If
                SP.Processing.Visible = True
            End If
        End If
        If MyBase.BorLite.NoACPBSVCall = False Then
            While Check4Text(1, 74, "TCX0D") = False AndAlso Check4Text(1, 72, "TCX06") = False AndAlso Check4Text(1, 72, "TCX0I") = False AndAlso Check4Text(1, 72, "TCX0C") = False
                Hit("F12")
            End While
        End If

        If Check4Text(1, 74, "TCX0D") Then
            PutText(15, 40, "3", True)
            If Not Check4Text(24, 2, "F4=PYOFF") Then Hit("F2")
            Hit("F4")
            PutText(7, 26, DateTime.Now.ToString("MMddyy"))
            PutText(9, 16, "X")
            PutText(9, 54, "Y", True)
            Hit("ENTER")
            _totalPayoffAmount = Double.Parse(GetText(12, 29, 10))
            _totalPrincipalDue = Double.Parse(GetText(14, 29, 10))
            _totalInterestDue = Double.Parse(GetText(15, 29, 10))
            _totalDailyInterest = Double.Parse(GetText(17, 29, 10))
            _totalAmountPastDue = Double.Parse(GetText(18, 29, 10))
            MyBase.TotalLateFeesDue = Double.Parse(GetText(19, 29, 10))

            Hit("F12")
            Hit("F12")
            Hit("F12")
        End If
        SetSuspension()
    End Sub

    Public Overridable Sub SetWhetherBorrowerExistsInLco()
        FastPath("TPDD" & SSN)
        _existsInLco = Check4Text(2, 19, "PERSONAL DEMOGRAPHIC")
    End Sub

    Public Overridable Function FoundAcpLoanData(ByVal homePageForm As Form, Optional ByVal contactPhone As String = "") As Boolean
        'This function will gather Loan information return true if it Finds the TCX0D, TCX06, or TCX0I screens, or return False if it finds a screen it doesnt recognize.
        If MyBase.BorLite.NoACPBSVCall Then Return False
        While Check4Text(1, 74, "TCX0D") = False AndAlso Check4Text(1, 72, "TCX06") = False AndAlso Check4Text(1, 72, "TCX0I") = False AndAlso Check4Text(1, 72, "TCX0C") = False
            If (MyBase.ContactCode = "03" OrElse MyBase.ContactCode = "04") AndAlso (MyBase.ActivityCode = "TC" OrElse MyBase.ActivityCode = "EM") Then 'BORROWER CONTACT
                MyBase.TCX04SelectionValue = "6"
            ElseIf MyBase.AttemptType = "No Answer" Then
                MyBase.TCX04SelectionValue = "1"
            ElseIf MyBase.AttemptType = "Answering Machine/Service" Then
                MyBase.TCX04SelectionValue = "2"
            ElseIf MyBase.AttemptType = "Wrong Number" Then
                MyBase.TCX04SelectionValue = "3"
            ElseIf MyBase.AttemptType = "Phone Busy" Then
                MyBase.TCX04SelectionValue = "4"
            ElseIf MyBase.AttemptType = "Disconnected Phone/Out of Service" Then
                MyBase.TCX04SelectionValue = "5"
            Else '3RD PARTY CONTACT
                MyBase.TCX04SelectionValue = "9"
            End If
            'depending on which screen is encountered gather info and hit F12 for the next screen
            If Check4Text(1, 74, "TCX02") Then
                'Enter gathered info
                If Check4Text(8, 2, "_") Then
                    PutText(8, 2, "X")
                    _loanStatus = GetText(14, 4, 7)
                    _currentPrincipalBalance = Double.Parse(GetText(14, 11, 9))
                    _interestRateText = GetText(14, 23, 6)
                    _disbursementDateText = GetText(14, 30, 8)
                    _loanProgram = GetText(14, 50, 6)
                    _amountDueText = GetText(14, 67, 8)
                Else
                    _loanStatus = GetText(13, 4, 7)
                    _currentPrincipalBalance = Double.Parse(GetText(13, 11, 9))
                    _interestRateText = GetText(13, 23, 6)
                    _disbursementDateText = GetText(13, 30, 8)
                    _loanProgram = GetText(13, 50, 6)
                    _amountDueText = GetText(13, 67, 8)
                End If
            ElseIf Check4Text(1, 74, "TCX04") Then
                'Enter gathered info
                If Not contactPhone = "" And BorLite.ACPSelection = "2" Then
                    PutText(7, 17, contactPhone)
                End If
                PutText(22, 35, MyBase.TCX04SelectionValue)
                Hit("Enter")
                If Check4Text(23, 2, "02110") = False Then
                    Dim commentType As String = ""
                    Select Case MyBase.TCX04SelectionValue
                        Case "1"
                            commentType = "No Answer"
                        Case "2"
                            commentType = "Answering Machine/Service"
                        Case "3"
                            commentType = "Wrong Number"
                        Case "4"
                            commentType = "Phone Busy"
                        Case "5"
                            commentType = "Phone Out of Service/Disconnected"
                    End Select
                    If commentType.Length > 0 Then
                        SP.Processing.Visible = False
                        Dim commentForm As New frmGetComment()
                        commentForm.ShowDialog(commentType)
                        SP.Processing.Visible = True
                        CType(homePageForm, frmBSHomePage).CloseAllSubForms()
                        CType(homePageForm, frmBSHomePage).Close()
                        CType(homePageForm, frmBSHomePage).DemographicsForm.UpdateSys()
                        SP.Processing.Visible = False
                        SP.UsrInf.ReturnToFavoriteScreen()
                        Return False
                    End If
                End If
            ElseIf Check4Text(1, 72, "TCX14") Then
                'Enter gathered info
                If GetText(10, 6, 2) = "BK" Then
                    MyBase.TCX14SelectionValue = "1"
                ElseIf GetText(16, 6, 2) = "BK" Then
                    MyBase.TCX14SelectionValue = "2"
                Else
                    MyBase.TCX14SelectionValue = ""
                End If
                PutText(22, 13, MyBase.TCX14SelectionValue)
            ElseIf Check4Text(1, 72, "TCX0A") Then
                Dim paymentAmount As String = GetText(13, 49, 12)
                Dim message As String = "Borrower must: Return a signed repayment obligation in order to be eligible for deferment or forbearance, and/or make full monthly payments of $" + paymentAmount
                MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Hit("Enter")
            Else
                SP.frmWhoaDUDE.WhoaDUDE("Whoa! That's like a totally new screen form me. I'm gonna need some help with this.  Please contact Systems Support.", "Knarly DUDE", True)
                Return False
            End If
            Hit("Enter") 'move to the next screen
            If Check4Text(23, 2, "02110") Then '02110 SELECTION INVALID FOR INCOMING CALL
                MyBase.BorLite.ACPSelection = "2"
                MyBase.ReturnToACP()
            End If
        End While
        If Check4Text(1, 74, "TCX0D") Then
            _firstScreen = "TCX0D"
        ElseIf Check4Text(1, 72, "TCX06") Then
            _firstScreen = "TCX06"
        ElseIf Check4Text(1, 72, "TCX0I") Then
            _firstScreen = "TCX0I"
        ElseIf Check4Text(1, 72, "TCX0C") Then
            _firstScreen = "TCX0C"
        End If
        Return True
    End Function

#Region "Multi-threading"
    Public Sub Turbo()
        SetVipAndSpecialHandling()
        SetRehabilitatedAndRepurchased()
        MyBase.GetLG10()
        _statutoryInterestRates = GetStatutoryInterestRates()
        MyBase.GetACH()
        SetTwentyDayLetter()
        _loans = GetLoansFromTS26()
        SetPrivateLoans()
        _installments = GetInstallmentsFromTS2X()
        SetAmountsFromTD0L()
        Thread.CurrentThread.Abort()
    End Sub

    Private Sub SetVipAndSpecialHandling()
        FastPath(String.Format("LP50I{0};;;;;VIPSS", MyBase.SSN))
        _isVIP = Not Check4Text(1, 68, "ACTIVITY MENU")
        FastPath(String.Format("LP50I{0};;;;;SPHAN", MyBase.SSN))
        _specialHandlingIsRequired = Not Check4Text(1, 68, "ACTIVITY MENU")
    End Sub

    Private Sub SetRehabilitatedAndRepurchased()
        _isRehabilitated = False
        _isRepurchased = False
        Dim row As Integer = 7
        FastPath("LC05I" + SSN)
        If Not Check4Text(1, 62, "DEFAULT/CLAIM RECAP") Then Return

        While Not Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY")
            If Not Check4Text(row, 3, " ") Then
                If Check4Text(row + 1, 25, "REHABILITA") Then
                    _isRehabilitated = True
                ElseIf Check4Text(row + 1, 25, "REPURCHASD") Then
                    _isRepurchased = True
                End If
            End If
            If _isRehabilitated AndAlso _isRepurchased Then Return
            row += 3
            If row > 16 Then
                Hit("F8")
                row = 7
            End If
        End While
    End Sub

    Private Function GetStatutoryInterestRates() As List(Of LoanInterestRate)
        Dim statutoryRates As New List(Of LoanInterestRate)()
        FastPath("TX3ZITS06" & SSN)
        If Check4Text(1, 71, "TSX07") Then
            Dim newLoanRate As New LoanInterestRate()
            newLoanRate.LnSeqText = "0001"
            'get rate
            For row As Integer = 11 To 21
                If Check4Text(row, 46, "A") Then
                    Dim beginDate As DateTime = DateTime.ParseExact(GetText(row, 17, 10), "MM dd yyyy", CultureInfo.InvariantCulture)
                    Dim endDate As DateTime = DateTime.ParseExact(GetText(row, 29, 10), "MM dd yyyy", CultureInfo.InvariantCulture)
                    If beginDate.Date <= DateTime.Now.Date AndAlso endDate.Date >= DateTime.Now.Date Then
                        newLoanRate.InterestRateText = GetText(row, 74, 5)
                        Exit For
                    End If
                End If
                If Check4Text(row + 1, 3, " ") Then
                    row = 11
                    Hit("F8")
                End If
            Next row
            statutoryRates.Add(newLoanRate)
        ElseIf Check4Text(1, 72, "TSX05") Then
            Dim loanRow As Integer = 8
            While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                Dim newLoanRate As New LoanInterestRate()
                newLoanRate.LnSeqText = "0" + GetText(loanRow, 47, 3)
                PutText(21, 18, "")
                Hit("End") 'blank sel field
                PutText(21, 18, GetText(loanRow, 3, 3), True) 'select row
                'get rate
                Dim rateRow As Integer = 11
                While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    If Check4Text(rateRow, 46, "A") Then
                        Dim beginDate As DateTime = DateTime.ParseExact(GetText(rateRow, 17, 10), "MM dd yyyy", CultureInfo.InvariantCulture)
                        Dim endDate As DateTime = DateTime.ParseExact(GetText(rateRow, 29, 10), "MM dd yyyy", CultureInfo.InvariantCulture)
                        If beginDate.Date <= DateTime.Now.Date AndAlso endDate.Date >= DateTime.Now.Date Then
                            newLoanRate.InterestRateText = GetText(rateRow, 74, 5)
                            statutoryRates.Add(newLoanRate)
                            Exit While
                        End If
                    End If
                    rateRow += 1
                    If Check4Text(rateRow, 3, " ") Then
                        rateRow = 11
                        Hit("F8")
                    End If
                End While
                Hit("F12")
                loanRow += 1
                If Check4Text(loanRow, 4, " ") Then
                    loanRow = 8
                    Hit("F8")
                End If
            End While
        End If
        Hit("F12")
        Return statutoryRates
    End Function

    Private Sub SetTwentyDayLetter()
        FastPath("TX3Z/ITD2A" + SSN)
        'Obtain the Date Last Contact 
        PutText(10, 65, "CNTCT", True)
        If Not Check4Text(23, 2, "01019") Then
            If Check4Text(1, 72, "TDX2C") Then
                _lastContactDateText = GetText(7, 4, 8)
            ElseIf Check4Text(1, 72, "TDX2D") Then
                _lastContactDateText = GetText(15, 31, 8)
            End If
            Hit("F12")
        End If
        'Obtain the Date Last Attempt 
        FastPath("TX3Z/ITD2A" + SSN)
        PutText(10, 65, "NOCTC", True)
        If Not Check4Text(23, 2, "01019") Then
            If Check4Text(1, 72, "TDX2C") Then
                _lastAttemptedContactDateText = GetText(7, 4, 8)
            ElseIf Check4Text(1, 72, "TDX2D") Then
                _lastAttemptedContactDateText = GetText(15, 31, 8)
            End If
            Hit("F12")
        End If
        FastPath("TX3Z/ITD2A" + SSN)
        'obtain the # Times 20-Day Letter
        PutText(11, 65, "DL200", True)
        If Check4Text(23, 2, "01019") Then
            _twentyDateLetter.NumberSent = "0"
        Else
            If Check4Text(1, 72, "TDX2C") Then
                PutText(5, 14, "X", True)
            End If
            _twentyDateLetter.NumberSent = GetText(5, 67, 2)

            'Hit F8 to page through up to 4 records to store for occurrences of a 20-Day Letter being sent to the borrower
            'These aren't used anywhere, but it's in the spec to gather them.
            _twentyDateLetter.SentDates(0) = GetText(13, 31, 8)
            For x As Integer = 1 To 3
                Hit("F8")
                If Check4Text(23, 2, "90007") Then Exit For
                _twentyDateLetter.SentDates(x) = GetText(13, 31, 8)
            Next

            Hit("F12")
        End If
    End Sub

    Private Function GetLoansFromTS26() As List(Of Loan)
        Dim foundLoans As New List(Of Loan)()
        Hit("F8")
        FastPath("TX3Z/ITS26" + SSN)
        If Check4Text(1, 72, "TSX28") Then
            Dim allLoansAreZeroBalance As Boolean = True
            Dim loanRow As Integer = 8
            Do While Check4Text(23, 2, "90007") = False
                Do While GetText(loanRow, 59, 10).Length > 0
                    If Double.Parse(GetText(loanRow, 59, 10).Replace(",", "")) > 0 Then
                        allLoansAreZeroBalance = False
                        PutText(21, 12, "")
                        Hit("END")
                        PutText(21, 12, GetText(loanRow, 2, 2), True)
                        'Get Loan Types and Cohort status and delinquency dates
                        Dim thisLoan As New Loan()
                        thisLoan.LoanType = GetText(6, 66, 6)
                        thisLoan.RepaymentStartDate = GetText(17, 44, 8)
                        Dim installmentText As String = GetText(19, 43, 9)
                        If installmentText.Length > 0 Then thisLoan.Installment = Double.Parse(installmentText)
                        Dim termText As String = GetText(20, 43, 4)
                        If termText.Length > 0 Then thisLoan.Term = Integer.Parse(termText)
                        Hit("ENTER")
                        'While we're here, let's get the intrest rate reduction offered for EFT.
                        thisLoan.EftRate = Double.Parse(GetText(20, 50, 6))
                        Hit("ENTER")
                        Dim daysDelinquentText As String = GetText(9, 46, 3)
                        If daysDelinquentText.Length > 0 Then thisLoan.DaysDelinquent = Integer.Parse(daysDelinquentText)
                        thisLoan.DelinquencyDate = GetText(9, 69, 8)
                        Hit("F12")
                        Hit("F12")
                        thisLoan.Status = GetText(3, 10, 20)
                        Hit("F12")
                        foundLoans.Add(thisLoan)
                    End If
                    loanRow += 1
                Loop
                Hit("F8")
                loanRow = 8
            Loop
            If allLoansAreZeroBalance Then
                foundLoans.Add(New Loan() With {.Status = "PIF/Deconverted"})
            End If
        ElseIf Check4Text(1, 72, "TSX29") Then
            'Get Loan Types and Cohort status and delinquency dates
            Dim thisLoan As New Loan()
            thisLoan.LoanType = GetText(6, 66, 6)
            thisLoan.RepaymentStartDate = GetText(17, 44, 8)
            Hit("ENTER")
            Hit("ENTER")
            Dim daysDelinquentText As String = GetText(9, 46, 3)
            If daysDelinquentText.Length > 0 Then thisLoan.DaysDelinquent = Integer.Parse(daysDelinquentText)
            thisLoan.DelinquencyDate = GetText(9, 69, 8)
            Hit("F12")
            Hit("F12")
            thisLoan.Status = GetText(3, 10, 20)
            Hit("F12")
            foundLoans.Add(thisLoan)
        End If

        'Side-effecting operations:
        MyBase.OnTime48Eligible = False
        'find the most delinquent loan from ITS26
        MyBase.NumDaysDelinquent = "0"
        MyBase.DateDelinquencyOccurred = ""
        For Each foundLoan As Loan In foundLoans
            If Integer.Parse(MyBase.NumDaysDelinquent) < foundLoan.DaysDelinquent Then
                MyBase.NumDaysDelinquent = foundLoan.DaysDelinquent.ToString()
                MyBase.DateDelinquencyOccurred = foundLoan.DelinquencyDate
            End If
        Next foundLoan

        Return foundLoans
    End Function

    Private Sub SetPrivateLoans()
        _hasPrivateLoan = False

        _privateLoansDataTable.Columns.Add("C0")
        _privateLoansDataTable.Columns.Add("C1")
        _privateLoansDataTable.Columns.Add("C2")
        _privateLoansDataTable.Columns.Add("C3")
        _privateLoansDataTable.Columns.Add("C4")
        _privateLoansDataTable.Columns.Add("C5")
        _privateLoansDataTable.Columns.Add("C6")
        _privateLoansDataTable.Columns.Add("C7")

        SP.Q.FastPath("TX3Z/IPO1N" & SSN)
        If Check4Text(22, 2, "01527") Then 'No loan found for borrower.
            Return
        ElseIf Check4Text(1, 75, "POX1P") Then 'Multiple loans
            Dim row As Integer = 9
            Do While Check4Text(22, 2, "90007") = False
                Dim loanType As String = GetText(row, 5, 6)
                If (loanType.Length = 0) OrElse (loanType = "SELECT") Then ' SELECT is what is encountered at the bottom
                    Hit("F8")
                    row = 9
                Else
                    If DataAccess.GetPrivateLoanTypes(TestMode()).Contains(loanType) Then
                        _hasPrivateLoan = True
                        PutText(21, 16, GetText(row, 2, 2), True)
                        AddRowToPrivateLoanTable(loanType)
                        Hit("F12")
                        row += 1
                    Else 'keep looking
                        row += 1
                    End If
                End If
            Loop
        ElseIf Check4Text(1, 75, "POX1S") Then 'Only one loan and we're in it.
            Dim loanType As String = GetText(2, 40, 6)
            AddRowToPrivateLoanTable(loanType)
        End If
    End Sub

    Public Sub AddRowToPrivateLoanTable(ByVal loanType As String)
        Dim arr(7) As String

        arr(1) = GetText(4, 46, 11) 'Application ID
        arr(2) = GetText(4, 70, 10) 'Application create date
        arr(3) = loanType

        Dim suspendedStatuses() As String = {"SUSPENDED", "SYSTEM SUSP"}
        Dim nonSuspendedStatuses() As String = {"FULLY DISB", "APPROVED", "PARTIALLY DISB", "PENDING"}
        Dim privateStatus As String = GetText(6, 27, 13)
        If nonSuspendedStatuses.Contains(privateStatus) Then
            Hit("F2")
            Hit("F11", "2")
            arr(4) = GetText(3, 10, 25)
            Hit("F12", "2")
            Hit("F2", "2")
        Else
            arr(4) = privateStatus
        End If

        If suspendedStatuses.Contains(privateStatus) Then
            If _fullStatusDescriptions.Contains(privateStatus) = False Then
                _fullStatusDescriptions.Add("Suspended")
            End If
            arr(6) = GetRejectionDescriptions(GetText(21, 22, 58))
        ElseIf nonSuspendedStatuses.Contains(privateStatus) Then
            If _fullStatusDescriptions.Contains("PENDING") = False Then
                _fullStatusDescriptions.Add("Pending")
            End If
        ElseIf privateStatus = "REJECTED" Then
            If _fullStatusDescriptions.Contains(privateStatus) = False Then
                _fullStatusDescriptions.Add("Rejected")
            End If
            arr(7) = GetRejectionDescriptions(GetText(21, 22, 58))
        ElseIf privateStatus = "INCOMPLETE" Then
            If _fullStatusDescriptions.Contains(privateStatus) = False Then
                _fullStatusDescriptions.Add("Incomplete")
            End If
            arr(5) = GetText(20, 20, 60) 'Rejection reason
        End If

        _privateLoansDataTable.Rows.Add(arr)
    End Sub

    Public Function GetRejectionDescriptions(ByVal rejectCode As String) As String
        Dim loanRejectDescriptions As New List(Of String)()

        While rejectCode.Length >= 5 'Should be While code.Length <> 0 but gave it some room 
            Dim firstFiveCodeCharacters As String
            If rejectCode.Length() >= 5 Then
                firstFiveCodeCharacters = rejectCode.Substring(0, 5)
                rejectCode = rejectCode.Substring(5).Trim()
            Else
                firstFiveCodeCharacters = rejectCode
            End If

            Try
                loanRejectDescriptions.Add(DataAccess.GetLoanRejectionDescription(TestMode(), firstFiveCodeCharacters))
            Catch ex As Exception
                MessageBox.Show("Problem reading from GENR_REF_LoanRejectCodes. Contact Process Automation.")
                loanRejectDescriptions.Add("Empty")
            End Try
        End While

        Return String.Join(", ", loanRejectDescriptions.ToArray())
    End Function

    Private Function GetInstallmentsFromTS2X() As List(Of Installment)
        Dim installments As New List(Of Installment)()
        FastPath("TX3Z/ITS2X" + SSN)
        If Check4Text(1, 72, "TSX2Y") Then
            'Target screen. Go through each repayment schedule that's in an active status.
            Do While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For row As Integer = 8 To 20
                    If Check4Text(row, 7, "A") Then
                        'Clear the selection field.
                        PutText(21, 14, "")
                        Hit("End")
                        'Select this repayment schedule and add the current installment.
                        PutText(21, 14, GetText(row, 3, 2), True)
                        Dim newInstallment As Installment = GetCurrentInstallmentFromTSX3W()
                        If newInstallment IsNot Nothing Then installments.Add(newInstallment)
                        'Return to the selection screen.
                        Hit("F12")
                        Hit("F12")
                    End If
                Next row
                Hit("F8")
            Loop
        ElseIf Check4Text(1, 71, "TSX3W") AndAlso Check4Text(7, 44, "A") Then
            'Selection screen.
            Dim newInstallment As Installment = GetCurrentInstallmentFromTSX3W()
            If newInstallment IsNot Nothing Then installments.Add(newInstallment)
        End If
        Return installments
    End Function

    Private Function GetCurrentInstallmentFromTSX3W() As Installment
        'Go to the repayment schedule detail screen.
        Hit("F4")
        'Find the row with the latest due date prior to today.
        Dim currentInstallmentRow As Integer = 16
        Dim latestDueDate As DateTime = DateTime.ParseExact(GetText(16, 22, 8), "MM/dd/yy", CultureInfo.InvariantCulture)
        If latestDueDate.Date > DateTime.Now.Date Then Return Nothing
        For installmentRow As Integer = 17 To 22
            If Check4Text(installmentRow, 22, "  ") Then Exit For
            Dim dueDate As Date = DateTime.ParseExact(GetText(installmentRow, 22, 8), "MM/dd/yy", CultureInfo.InvariantCulture)
            If dueDate > latestDueDate AndAlso dueDate < DateTime.Now.Date Then currentInstallmentRow = installmentRow
        Next installmentRow
        Dim currentInstallment As New Installment()
        currentInstallment.FirstDueDate = latestDueDate
        'Combine the installment amounts for all loans that are part of this installment schedule.
        Do While Check4Text(23, 2, "90007") = False
            currentInstallment.Amount += Double.Parse(GetText(currentInstallmentRow, 61, 10).Replace("$", "").Replace(",", ""))
            Hit("F8")
        Loop
        Return currentInstallment
    End Function

    Sub SetAmountsFromTD0L()
        MyBase.AmountPastDue = 0
        CurrentAmountDue = 0
        OutstandingLateFees = 0
        MyBase.TotalAmountDue = 0
        FastPath("TX3Z/ITD0L" + SSN)
        If Check4Text(1, 72, "TDX0M") Then
            Hit("F6")
            Do While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                MyBase.AmountPastDue += Double.Parse(GetText(15, 33, 10))
                MyBase.TotalAmountDue += Double.Parse(GetText(15, 67, 10))
                OutstandingLateFees += Double.Parse(GetText(16, 67, 10))
                Hit("F8")
            Loop
        End If
        CurrentAmountDue = MyBase.TotalAmountDue - MyBase.AmountPastDue
    End Sub
#End Region 'Multi-threading

    Private Sub SetSuspension()
        FastPath("TX3Z/ITS3I;;;S;;" + SSN)
        If Check4Text(23, 2, "01020") Then
            _hasSuspension = False
        ElseIf Check4Text(1, 72, "TSX3H") OrElse Check4Text(1, 72, "TSX3G") Then
            _hasSuspension = True
        End If
    End Sub

    Public Overloads Sub EnterCommentsIntoSystem(ByVal FromBSHomePage As Boolean, ByVal ActionCode As String)
        If _noteDudeNotesForOneLink.Length > 0 Then 'only add notes if there are some too add
            ActivityCmts.AddCommentsToLP50(_noteDudeNotesForOneLink, ActionCode, ContactCode, ActivityCode)
        End If
    End Sub

    'write possible script info to text file
    Public Sub WriteOut()
        'TODO: See what this file is used for (it's not read anywhere in DUDE, so it's probably for scripts)
        'and determine what it will take to change it to the T drive, or see if it's being phased out
        'with the new technique of passing objects to scripts that are coded in C#.
        Dim argumentList As New List(Of String)()
        argumentList.Add(MyBase.SSN)
        argumentList.Add(MyBase.FirstName)
        argumentList.Add(MyBase.MI)
        argumentList.Add(MyBase.LastName)
        argumentList.Add(MyBase.UserProvidedDemos.Addr1)
        argumentList.Add(MyBase.UserProvidedDemos.Addr2)
        argumentList.Add(MyBase.UserProvidedDemos.City)
        argumentList.Add(MyBase.UserProvidedDemos.State)
        argumentList.Add(MyBase.UserProvidedDemos.Zip)
        argumentList.Add(MyBase.UserProvidedDemos.HomePhoneNum)
        argumentList.Add(MyBase.UserProvidedDemos.OtherPhoneNum)
        argumentList.Add(String.Empty)
        argumentList.Add(MyBase.UserProvidedDemos.OtherPhoneNum)
        argumentList.Add(MyBase.UserProvidedDemos.HomePhoneExt)
        argumentList.Add(MyBase.UserProvidedDemos.OtherPhoneExt)
        argumentList.Add(MyBase.UserProvidedDemos.OtherPhone2Ext)
        argumentList.Add(MyBase.UserProvidedDemos.Email)
        argumentList.Add(MyBase.CLAccNum)
        argumentList.Add(MyBase.NumDaysDelinquent)
        argumentList.Add(MyBase.DateDelinquencyOccurred)
        argumentList.Add(_twentyDateLetter.NumberSent)
        argumentList.Add(MyBase.AmountPastDue.ToString())
        argumentList.Add(CurrentAmountDue.ToString())
        argumentList.Add(MyBase.TotalAmountDue.ToString())
        argumentList.Add(OutstandingLateFees.ToString())
        argumentList.Add((MyBase.TotalAmountDue + OutstandingLateFees).ToString())
        argumentList.Add(_totalPrincipalDue.ToString())
        argumentList.Add(_totalInterestDue.ToString())
        argumentList.Add(_billingDueDayText)
        argumentList.Add(_nextDueDateText)
        argumentList.Add(_lastPayment.ReceivedDate)
        argumentList.Add(MyBase.MonthlyPA.ToString())
        argumentList.Add(MyBase.ACHData.HasACH)
        argumentList.Add(HasRepaymentSchedule)
        argumentList.Add(MyBase.UserProvidedDemos.UPEmailVal.ToString())
        Using fileWriter As New StreamWriter("T:\TempDemoUpdate.txt")
            fileWriter.WriteCommaDelimitedLine(argumentList.ToArray())
            fileWriter.Close()
        End Using
    End Sub
End Class
