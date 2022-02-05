Imports System.Windows.Forms
Imports System.Threading
Imports Q


Public Class BorrowerLM
    Inherits SP.Borrower

#Region "Class Vars"
    Private _hP As frmLMHomePage
    Private _status As Collection
    'New Thread For Multi-Threading
    Private _turboSpeed As New Thread(AddressOf MultiThreadTurbo)

    Private _borrStat(0) As String
    Public Property BorrStat() As String()
        Get
            Return _borrStat
        End Get
        Set(ByVal value As String())
            _borrStat = value
        End Set
    End Property

    Private _dateLastCntct As String
    Public Property DateLastCntct() As String
        Get
            Return _dateLastCntct
        End Get
        Set(ByVal value As String)
            _dateLastCntct = value
        End Set
    End Property

    Private _dateLastAtempt As String
    Public Property DateLastAtempt() As String
        Get
            Return _dateLastAtempt
        End Get
        Set(ByVal value As String)
            _dateLastAtempt = value
        End Set
    End Property

    Private _lastPaymentAmount As Double = 0
    Public Property LastPaymentAmount() As Double
        Get
            Return _lastPaymentAmount
        End Get
        Set(ByVal value As Double)
            _lastPaymentAmount = value
        End Set
    End Property

    Private _dueDay As String = ""
    Public Property DueDay() As String
        Get
            Return _dueDay
        End Get
        Set(ByVal value As String)
            _dueDay = value
        End Set
    End Property

    Private _hasTPDD As Boolean
    Public Property HasTPDD() As Boolean
        Get
            Return _hasTPDD
        End Get
        Set(ByVal value As Boolean)
            _hasTPDD = value
        End Set
    End Property

    Private _actionCode As String
    Public Property ActionCode() As String
        Get
            Return _actionCode
        End Get
        Set(ByVal value As String)
            _actionCode = value
        End Set
    End Property



    '''''''''''''''''''''''''''''
    ' ''Loan Management Home Page
    ' ''LC05

    Private _avrIntRate As Double = 0

    Private _acctBal As Double
    Public Property AcctBal() As Double
        Get
            Return _acctBal
        End Get
        Set(ByVal value As Double)
            _acctBal = value
        End Set
    End Property

    Private _dueDate As String
    Public Property DueDate() As String
        Get
            Return _dueDate
        End Get
        Set(ByVal value As String)
            _dueDate = value
        End Set
    End Property

    Private _paymentPendingInfo As String
    Public Property PaymentPendingInfo() As String
        Get
            Return _paymentPendingInfo
        End Get
        Set(ByVal value As String)
            _paymentPendingInfo = value
        End Set
    End Property

    Private _agencyMonthlyInterest As Double = 0
    Public Property AgencyMonthlyInterest() As Double
        Get
            Return _agencyMonthlyInterest
        End Get
        Set(ByVal value As Double)
            _agencyMonthlyInterest = value
        End Set
    End Property

    Private _accountOpenDate As String
    Public Property AccountOpenDate() As String
        Get
            Return _accountOpenDate
        End Get
        Set(ByVal value As String)
            _accountOpenDate = value
        End Set
    End Property

    Private _defaultStatus As String
    Public Property DefaultStatus() As String
        Get
            Return _defaultStatus
        End Get
        Set(ByVal value As String)
            _defaultStatus = value
        End Set
    End Property

    Private _errorCode As String
    Public Property ErrorCode() As String
        Get
            Return _errorCode
        End Get
        Set(ByVal value As String)
            _errorCode = value
        End Set
    End Property

    Private _defermentStatus As String
    Public Property DefermentStatus() As String
        Get
            Return _defermentStatus
        End Get
        Set(ByVal value As String)
            _defermentStatus = value
        End Set
    End Property

    Private _coMakerSSN As String
    Public Property CoMakerSSN() As String
        Get
            Return _coMakerSSN
        End Get
        Set(ByVal value As String)
            _coMakerSSN = value
        End Set
    End Property

    Private _lC05Detail As DataTable
    Public Property LC05Detail() As DataTable
        Get
            Return _lC05Detail
        End Get
        Set(ByVal value As DataTable)
            _lC05Detail = value
        End Set
    End Property

    Private _taxOffsetCode As String
    Public Property TaxOffsetCode() As String
        Get
            Return _taxOffsetCode
        End Get
        Set(ByVal value As String)
            _taxOffsetCode = value
        End Set
    End Property

    Private _tempPaymentAmt As Double
    Public Property TempPaymentAmt() As Double
        Get
            Return _tempPaymentAmt
        End Get
        Set(ByVal value As Double)
            _tempPaymentAmt = value
        End Set
    End Property

    Private _minPayBeginDt As String
    Public Property MinPayBeginDt() As String
        Get
            Return _minPayBeginDt
        End Get
        Set(ByVal value As String)
            _minPayBeginDt = value
        End Set
    End Property

    Private _minPayEndDt As String
    Public Property MinPayEndDt() As String
        Get
            Return _minPayEndDt
        End Get
        Set(ByVal value As String)
            _minPayEndDt = value
        End Set
    End Property

    Private _newLoanInd As String = "N"
    Public Property NewLoanInd() As String
        Get
            Return _newLoanInd
        End Get
        Set(ByVal value As String)
            _newLoanInd = value
        End Set
    End Property

    Private _collectionInd As String
    Public Property CollectionInd() As String
        Get
            Return _collectionInd
        End Get
        Set(ByVal value As String)
            _collectionInd = value
        End Set
    End Property

    Private _directDebit As String
    Public Property DirectDebit() As String
        Get
            Return _directDebit
        End Get
        Set(ByVal value As String)
            _directDebit = value
        End Set
    End Property

    Private _dateLastCntctCalculated As String
    Public Property DateLastCntctCalculated() As String
        Get
            Return _dateLastCntctCalculated
        End Get
        Set(ByVal value As String)
            _dateLastCntctCalculated = value
        End Set
    End Property

    Private _dateLastAtemptCalculated As String
    Public Property DateLastAtemptCalculated() As String
        Get
            Return _dateLastAtemptCalculated
        End Get
        Set(ByVal value As String)
            _dateLastAtemptCalculated = value
        End Set
    End Property

    Private _certified As String
    Public Property Certified() As String
        Get
            Return _certified
        End Get
        Set(ByVal value As String)
            _certified = value
        End Set
    End Property


    ''LC10
    Private _infoFoundOnLC10 As Boolean
    Public Property InfoFoundOnLC10() As Boolean
        Get
            Return _infoFoundOnLC10
        End Get
        Set(ByVal value As Boolean)
            _infoFoundOnLC10 = value
        End Set
    End Property

    Private _principalDefaulted As Double = 0
    Public Property PrincipalDefaulted() As Double
        Get
            Return _principalDefaulted
        End Get
        Set(ByVal value As Double)
            _principalDefaulted = value
        End Set
    End Property

    Private _interestDefaulted As Double = 0
    Public Property InterestDefaulted() As Double
        Get
            Return _interestDefaulted
        End Get
        Set(ByVal value As Double)
            _interestDefaulted = value
        End Set
    End Property

    Private _totalDefaulted As Double = 0
    Public Property TotalDefaulted() As Double
        Get
            Return _totalDefaulted
        End Get
        Set(ByVal value As Double)
            _totalDefaulted = value
        End Set
    End Property

    Private _principalCollected As Double = 0
    Public Property PrincipalCollected() As Double
        Get
            Return _principalCollected
        End Get
        Set(ByVal value As Double)
            _principalCollected = value
        End Set
    End Property

    Private _interestAccrued As Double = 0
    Public Property InterestAccrued() As Double
        Get
            Return _interestAccrued
        End Get
        Set(ByVal value As Double)
            _interestAccrued = value
        End Set
    End Property

    Private _interestCollected As Double = 0
    Public Property InterestCollected() As Double
        Get
            Return _interestCollected
        End Get
        Set(ByVal value As Double)
            _interestCollected = value
        End Set
    End Property

    Private _legalCostAccrued As Double = 0
    Public Property LegalCostAccrued() As Double
        Get
            Return _legalCostAccrued
        End Get
        Set(ByVal value As Double)
            _legalCostAccrued = value
        End Set
    End Property

    Private _legalCostCollected As Double = 0
    Public Property LegalCostCollected() As Double
        Get
            Return _legalCostCollected
        End Get
        Set(ByVal value As Double)
            _legalCostCollected = value
        End Set
    End Property

    Private _otherChargesAccrued As Double = 0
    Public Property OtherChargesAccrued() As Double
        Get
            Return _otherChargesAccrued
        End Get
        Set(ByVal value As Double)
            _otherChargesAccrued = value
        End Set
    End Property

    Private _otherChargesCollected As Double = 0
    Public Property OtherChargesCollected() As Double
        Get
            Return _otherChargesCollected
        End Get
        Set(ByVal value As Double)
            _otherChargesCollected = value
        End Set
    End Property

    Private _collectionCostAccrued As Double = 0
    Public Property CollectionCostAccrued() As Double
        Get
            Return _collectionCostAccrued
        End Get
        Set(ByVal value As Double)
            _collectionCostAccrued = value
        End Set
    End Property

    Private _collectionCostCollected As Double = 0
    Public Property CollectionCostCollected() As Double
        Get
            Return _collectionCostCollected
        End Get
        Set(ByVal value As Double)
            _collectionCostCollected = value
        End Set
    End Property

    Private _collectionFeesPojected As Double = 0
    Public Property CollectionFeesPojected() As Double
        Get
            Return _collectionFeesPojected
        End Get
        Set(ByVal value As Double)
            _collectionFeesPojected = value
        End Set
    End Property

    Private _outstandingBalanceDue As Double = 0
    Public Property OutstandingBalanceDue() As Double
        Get
            Return _outstandingBalanceDue
        End Get
        Set(ByVal value As Double)
            _outstandingBalanceDue = value
        End Set
    End Property

    Private _totalCollected As Double = 0
    Public Property TotalCollected() As Double
        Get
            Return _totalCollected
        End Get
        Set(ByVal value As Double)
            _totalCollected = value
        End Set
    End Property

    Private _expectedPytAmt As Double = 0
    Public Property ExpectedPytAmt() As Double
        Get
            Return _expectedPytAmt
        End Get
        Set(ByVal value As Double)
            _expectedPytAmt = value
        End Set
    End Property

    Private _payoff30Days As Double = 0
    Public Property Payoff30Days() As Double
        Get
            Return _payoff30Days
        End Get
        Set(ByVal value As Double)
            _payoff30Days = value
        End Set
    End Property


    ' ''LC18

    Private _rehabCode As String = 0
    Public Property RehabCode() As String
        Get
            Return _rehabCode
        End Get
        Set(ByVal value As String)
            _rehabCode = value
        End Set
    End Property


    Private _rehabCounter As Integer = 0
    Public Property RehabCounter() As Integer
        Get
            Return _rehabCounter
        End Get
        Set(ByVal value As Integer)
            _rehabCounter = value
        End Set
    End Property

    Private _ineligibleForRehabCode As String
    Public Property IneligibleForRehabCode() As String
        Get
            Return _ineligibleForRehabCode
        End Get
        Set(ByVal value As String)
            _ineligibleForRehabCode = value
        End Set
    End Property

    Private _reinstatementEligibilityCode As String
    Public Property ReinstatementEligibilityCode() As String
        Get
            Return _reinstatementEligibilityCode
        End Get
        Set(ByVal value As String)
            _reinstatementEligibilityCode = value
        End Set
    End Property

    Private _reinstatementDate As String
    Public Property ReinstatementDate() As String
        Get
            Return _reinstatementDate
        End Get
        Set(ByVal value As String)
            _reinstatementDate = value
        End Set
    End Property

    Private _reinstatementCounter As Integer = 0
    Public Property ReinstatementCounter() As Integer
        Get
            Return _reinstatementCounter
        End Get
        Set(ByVal value As Integer)
            _reinstatementCounter = value
        End Set
    End Property

    Private _day30Notice As String
    Public Property Day30Notice() As String
        Get
            Return _day30Notice
        End Get
        Set(ByVal value As String)
            _day30Notice = value
        End Set
    End Property

    Private _offsetAmt As String
    Public Property OffsetAmt() As String
        Get
            Return _offsetAmt
        End Get
        Set(ByVal value As String)
            _offsetAmt = value
        End Set
    End Property

    Private _yearsCertified As Integer = 0
    Public Property YearsCertified() As Integer
        Get
            Return _yearsCertified
        End Get
        Set(ByVal value As Integer)
            _yearsCertified = value
        End Set
    End Property

    Private _date1098ESent As String
    Public Property Date1098ESent() As String
        Get
            Return _date1098ESent
        End Get
        Set(ByVal value As String)
            _date1098ESent = value
        End Set
    End Property

    Private _interestReported As String
    Public Property InterestReported() As String
        Get
            Return _interestReported
        End Get
        Set(ByVal value As String)
            _interestReported = value
        End Set
    End Property


    ''LC20
    Private _employerName As String
    Public Property EmployerName() As String
        Get
            Return _employerName
        End Get
        Set(ByVal value As String)
            _employerName = value
        End Set
    End Property

    Private _overRideIndicator As String
    Public Property OverRideIndicator() As String
        Get
            Return _overRideIndicator
        End Get
        Set(ByVal value As String)
            _overRideIndicator = value
        End Set
    End Property


    ''LC34

    Private _perPaymentAmt As Double = 0
    Public Property PerPaymentAmt() As Double
        Get
            Return _perPaymentAmt
        End Get
        Set(ByVal value As Double)
            _perPaymentAmt = value
        End Set
    End Property


    ''LC41

    Private _lastPaymentRecieved As String
    Public Property LastPaymentRecieved() As String
        Get
            Return _lastPaymentRecieved
        End Get
        Set(ByVal value As String)
            _lastPaymentRecieved = value
        End Set
    End Property

    Private _paymentsFromLC41 As New ArrayList()
    Public Property PaymentsFromLC41() As ArrayList
        Get
            Return _paymentsFromLC41
        End Get
        Set(ByVal value As ArrayList)
            _paymentsFromLC41 = value
        End Set
    End Property


    ''LC67

    Private _primaryActionDate As String
    Public Property PrimaryActionDate() As String
        Get
            Return _primaryActionDate
        End Get
        Set(ByVal value As String)
            _primaryActionDate = value
        End Set
    End Property

    Private _dateWithdrawn As String
    Public Property DateWithdrawn() As String
        Get
            Return _dateWithdrawn
        End Get
        Set(ByVal value As String)
            _dateWithdrawn = value
        End Set
    End Property

    Private _reasonWithdrawn As String
    Public Property ReasonWithdrawn() As String
        Get
            Return _reasonWithdrawn
        End Get
        Set(ByVal value As String)
            _reasonWithdrawn = value
        End Set
    End Property

    Private _activeJudgment As String
    Public Property ActiveJudgment() As String
        Get
            Return _activeJudgment
        End Get
        Set(ByVal value As String)
            _activeJudgment = value
        End Set
    End Property

    Private _outstandingAWG As String
    Public Property OutstandingAWG() As String
        Get
            Return _outstandingAWG
        End Get
        Set(ByVal value As String)
            _outstandingAWG = value
        End Set
    End Property

    Private _garnishmentType As String
    Public Property GarnishmentType() As String
        Get
            Return _garnishmentType
        End Get
        Set(ByVal value As String)
            _garnishmentType = value
        End Set
    End Property

    ''comment status

    Private _specialHandling As Boolean
    Public Property SpecialHandling() As Boolean
        Get
            Return _specialHandling
        End Get
        Set(ByVal value As Boolean)
            _specialHandling = value
        End Set
    End Property

    Private _vIP As Boolean
    Public Property VIP() As Boolean
        Get
            Return _vIP
        End Get
        Set(ByVal value As Boolean)
            _vIP = value
        End Set
    End Property

    Private _dD136 As Boolean
    Public Property DD136() As Boolean
        Get
            Return _dD136
        End Get
        Set(ByVal value As Boolean)
            _dD136 = value
        End Set
    End Property


    'References

    Private _references As New Generic.List(Of Reference)
    Public Property References() As Generic.List(Of Reference)
        Get
            Return _references
        End Get
        Set(ByVal value As Generic.List(Of Reference))
            _references = value
        End Set
    End Property


    ''LG0H

    Private _infoFoundOnLG0H As Boolean
    Public Property InfoFoundOnLG0H() As Boolean
        Get
            Return _infoFoundOnLG0H
        End Get
        Set(ByVal value As Boolean)
            _infoFoundOnLG0H = value
        End Set
    End Property

    Private _eligibleForExtRepay As Boolean = True
    Public Property EligibleForExtRepay() As Boolean
        Get
            Return _eligibleForExtRepay
        End Get
        Set(ByVal value As Boolean)
            _eligibleForExtRepay = value
        End Set
    End Property


    ''for computing status of borrower

    Private _statusArr(0) As String
    Public Property StatusArr() As String()
        Get
            Return _statusArr
        End Get
        Set(ByVal value As String())
            _statusArr = value
        End Set
    End Property

    'comaker name from lp22

    Private _coMakerName As String
    Public Property CoMakerName() As String
        Get
            Return _coMakerName
        End Get
        Set(ByVal value As String)
            _coMakerName = value
        End Set
    End Property

    'contact script data
    Private _lMBorCnt As LMBorContactScript
    Public Property LMBorCnt() As LMBorContactScript
        Get
            Return _lMBorCnt
        End Get
        Set(ByVal value As LMBorContactScript)
            _lMBorCnt = value
        End Set
    End Property

    ''' <summary>
    ''' Only access through BorrowerLM properties.  This is an object that can be used for values outside the normal borrower object that is specific to homepage.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ScriptSpecificInfo() As MDScriptInfoSpecificToAcctResolution
        Get
            Return CType(ScriptInfoToGenericBusinessUnit, MDScriptInfoSpecificToAcctResolution)
        End Get
        Set(ByVal value As MDScriptInfoSpecificToAcctResolution)
            ScriptInfoToGenericBusinessUnit = CType(value, MDScriptInfoSpecificToAcctResolution)
        End Set
    End Property

    ''' <summary>
    ''' Employer ID.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EmployerID() As String
        Get
            Return ScriptSpecificInfo.EmployerID
        End Get
        Set(ByVal value As String)
            ScriptSpecificInfo.EmployerID = value
        End Set
    End Property


#End Region

    Public Sub New(ByVal BL As SP.BorrowerLite, ByVal HPTemp As frmLMHomePage)
        MyBase.New(BL)
        _hP = HPTemp
        'create and fill status collection
        CreateStatusCollection()
        _lMBorCnt = New LMBorContactScript()
        ScriptInfoToGenericBusinessUnit = New Q.MDScriptInfoSpecificToAcctResolution 'create one specific to Account Resolution
    End Sub

    Public Sub TPDDCheck()
        SP.Q.LoginToLCO()
        SP.Q.FastPath("TPDD" & SSN)
        If SP.Q.Check4Text(2, 19, "PERSONAL DEMOGRAPHIC") Then
            _hasTPDD = True
        Else
            _hasTPDD = False
        End If
        SP.Q.LoginToCompass()
    End Sub

    Public Sub DropOffOnLG02()
        SP.Q.FastPath("LG02I" & SSN)
    End Sub

    Public Sub DropOffOnLC02()
        SP.Q.FastPath("LC02I" & SSN)
    End Sub

    Public Function String8ToDate(ByVal str As String) As Date
        str = str.Insert(2, "/")
        str = str.Insert(5, "/")
        Return CDate(str)
    End Function

    'write possible script info to text file
    Public Sub WriteOut()
        FileOpen(1, "T:\TempDemoUpdate.txt", OpenMode.Output) ' Open file for output.
        If UserProvidedDemos.Addr1 <> String.Empty Then
            Write(1, SSN, FirstName, MI, LastName, UserProvidedDemos.Addr1, UserProvidedDemos.Addr2, _
                  UserProvidedDemos.City, UserProvidedDemos.State, UserProvidedDemos.Zip, _
                  UserProvidedDemos.HomePhoneNum, UserProvidedDemos.OtherPhoneNum, "", _
                  UserProvidedDemos.OtherPhoneNum, UserProvidedDemos.HomePhoneExt, UserProvidedDemos.OtherPhoneExt, _
                  UserProvidedDemos.OtherPhone2Ext, UserProvidedDemos.Email, CLAccNum, NumDaysDelinquent, _
                  DateDelinquencyOccurred, "", AmountPastDue, "", TotalAmountDue, "", "", "", "", DueDay, "", "", _
                  MonthlyPA, ACHData.HasACH, "", UserProvidedDemos.UPEmailVal.ToString)
        Else
            Write(1, SSN, FirstName, MI, LastName, OneLINKDemos.Addr1, OneLINKDemos.Addr2, OneLINKDemos.City, _
                  OneLINKDemos.State, OneLINKDemos.Zip, OneLINKDemos.HomePhoneNum, OneLINKDemos.OtherPhoneNum, "", _
                  OneLINKDemos.OtherPhoneNum, OneLINKDemos.HomePhoneExt, OneLINKDemos.OtherPhoneExt, _
                  OneLINKDemos.OtherPhoneExt, OneLINKDemos.Email, CLAccNum, NumDaysDelinquent, _
                  DateDelinquencyOccurred, "", AmountPastDue, "", TotalAmountDue, "", "", "", "", DueDay, "", "", _
                  MonthlyPA, ACHData.HasACH, "", "")
        End If
        FileClose(1)
    End Sub

    Public Function GetOpenQueueTasksFrmSys() As ArrayList
        Dim arr As New ArrayList
        Dim x As Integer
        SP.Q.FastPath("LP8YIDFT;;;" & SSN)
        If SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then
            SP.frmKnarlyDUDE.KnarlyDude("Life is good on the islands. This borrower has no open Default Queue Tasks.", "MauiDUDE")
            Return Nothing
        End If
        Do While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            For x = 7 To 21
                If SP.Q.GetText(x, 65, 8) <> "" Then
                    If arr.IndexOf(SP.Q.GetText(x, 65, 8)) = -1 Then
                        arr.Add(SP.Q.GetText(x, 65, 8))
                    End If
                End If
            Next
            SP.Q.Hit("F8")
        Loop
        Return arr
    End Function

    'does repayment calculations
    Public Function RepaymentCalculator(ByVal months As Integer) As String
        Dim TempPayment As Double
        TempPayment = System.Math.Round(Pmt(_avrIntRate / 100 / 12, months, -_acctBal), 0)
        If TempPayment > 50 Then
            Return Format(System.Math.Round(Pmt(_avrIntRate / 100 / 12, months, -_acctBal), 0), "$###,##0")
        Else
            Return "Below Write Off Limit"
        End If
    End Function

#Region "Gathering Info From System (multi-threading)"

    Public Sub MultiThreadTurbo()
        Turbo()
        Thread.CurrentThread.Abort()
    End Sub

    Public Sub Turbo()
        GetLG10() 'included from Main Menu
        GetACH() 'included from Main Menu
        GetLastMostRecentActivityCmts()
        DD136_DetermineVIPAndSpecialHndlAccts()
        GetLC10()
        GetLC18()
        GetLC2L()
        GetLC20()
        GetLC67()
        GetLP2C()
        GetLC05()
        GetCoMakerName()
        GetLC41()
        GetLC34()
        GetLG0H()
    End Sub

    Private Sub GetLastMostRecentActivityCmts()
        'switch current control with new one
        Dim ActCmt As New ActivityCmts(MDLMHome.ActivityCmts.DaysOrNumberOf.NumberOf, 5, False, Me)
        ActCmt.Location = New System.Drawing.Point(_hP.ActCmt.Left, _hP.ActCmt.Top)
        ActCmt.Width = _hP.ActCmt.Width
        ActCmt.Height = _hP.ActCmt.Height
        _hP.tabMain.Controls.Remove(_hP.ActCmt)
        _hP.tabMain.Controls.Add(ActCmt)
    End Sub

    Private Sub GetLG0H()
        SP.FastPath("LG0HI;" + SSN)
        If SP.Check4Text(1, 55, "DISBURSEMENT ACTIVITY MENU") Then
            _infoFoundOnLG0H = False
            Exit Sub
        Else
            _infoFoundOnLG0H = True
        End If
        If SP.Check4Text(1, 53, "DISBURSEMENT ACTIVITY SELECT") Then
            SP.PutText(21, 11, "01", True)
        End If
        'target screen
        While SP.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            If SP.Check4Text(12, 20, "M") = False Then 'check if date is populated
                If CDate(SP.GetText(12, 20, 8).Insert(2, "/").Insert(5, "/")) < CDate("10/07/1998") Then
                    _eligibleForExtRepay = False
                    Exit Sub
                End If
            End If
            If SP.Check4Text(12, 36, "M") = False Then 'check if date is populated
                If CDate(SP.GetText(12, 36, 8).Insert(2, "/").Insert(5, "/")) < CDate("10/07/1998") Then
                    _eligibleForExtRepay = False
                    Exit Sub
                End If
            End If
            If SP.Check4Text(12, 52, "M") = False Then 'check if date is populated
                If CDate(SP.GetText(12, 52, 8).Insert(2, "/").Insert(5, "/")) < CDate("10/07/1998") Then
                    _eligibleForExtRepay = False
                    Exit Sub
                End If
            End If
            If SP.Check4Text(12, 68, "M") = False Then 'check if date is populated
                If CDate(SP.GetText(12, 68, 8).Insert(2, "/").Insert(5, "/")) < CDate("10/07/1998") Then
                    _eligibleForExtRepay = False
                    Exit Sub
                End If
            End If
            While SP.Check4Text(12, 68, "M") = False 'exit loop when last disbursement on screen doesn't have populated date, this means there were no more disb for that loan
                If SP.Check4Text(12, 20, "M") = False Then 'check if date is populated
                    If CDate(SP.GetText(12, 20, 8).Insert(2, "/").Insert(5, "/")) < CDate("10/07/1998") Then
                        _eligibleForExtRepay = False
                        Exit Sub
                    End If
                End If
                If SP.Check4Text(12, 36, "M") = False Then 'check if date is populated
                    If CDate(SP.GetText(12, 36, 8).Insert(2, "/").Insert(5, "/")) < CDate("10/07/1998") Then
                        _eligibleForExtRepay = False
                        Exit Sub
                    End If
                End If
                If SP.Check4Text(12, 52, "M") = False Then 'check if date is populated
                    If CDate(SP.GetText(12, 52, 8).Insert(2, "/").Insert(5, "/")) < CDate("10/07/1998") Then
                        _eligibleForExtRepay = False
                        Exit Sub
                    End If
                End If
                If SP.Check4Text(12, 68, "M") = False Then 'check if date is populated
                    If CDate(SP.GetText(12, 68, 8).Insert(2, "/").Insert(5, "/")) < CDate("10/07/1998") Then
                        _eligibleForExtRepay = False
                        Exit Sub
                    End If
                End If
                SP.Hit("F10") 'page to next set of 
            End While
            SP.Hit("F8") 'page to the next loan
        End While
    End Sub

    Private Sub GetLC05()
        Dim x As Integer
        Dim dt As Date
        dt = Today
        Dim dateStr As String
        Dim arr(34) As String
        Dim TB As New DataTable()
        Dim AccountOpenDt As String
        Dim TempDueDate As String = ""
        Dim VariousDueDates As Boolean = False

        TB.Columns.Add("LenderPayoffDate")
        TB.Columns.Add("PrincipalDefaulted")
        TB.Columns.Add("InterestRate")
        TB.Columns.Add("InterestDefaulted")
        TB.Columns.Add("TotalAmountDefaulted")
        TB.Columns.Add("PrincipalCollected")
        TB.Columns.Add("InterestAccrudedThru")
        TB.Columns.Add("InterestCollected")
        TB.Columns.Add("LegalCostsAccrued")
        TB.Columns.Add("LegalCostsCollected")
        TB.Columns.Add("OtherChargesAccrued")
        TB.Columns.Add("OtherChargesCollected")
        TB.Columns.Add("CollectionCostsAccrued")
        TB.Columns.Add("CollectionCostsCollected")
        TB.Columns.Add("CollectionCostsProjected")
        TB.Columns.Add("OutstandingBalance")
        TB.Columns.Add("DirectPaymentIndicator")
        TB.Columns.Add("SubrogationCode")
        TB.Columns.Add("SubrogationDate")
        TB.Columns.Add("ClaimId")
        TB.Columns.Add("NextPaymentDue")
        TB.Columns.Add("FederalTaxOffsetId")
        TB.Columns.Add("MinimumPaymentAmount")
        TB.Columns.Add("MinPayBegin")
        TB.Columns.Add("MinPayEnd")
        TB.Columns.Add("GarnishmentCode")
        TB.Columns.Add("DateLastContact")
        TB.Columns.Add("DateLastAttempt")
        TB.Columns.Add("Endorser")
        TB.Columns.Add("CollectionCostIndicator")
        TB.Columns.Add("DefType")
        TB.Columns.Add("CurrentOutstandingPrincipal")
        TB.Columns.Add("CurrentInterest")
        TB.Columns.Add("OutstandingCollectionCosts")
        TB.Columns.Add("ExpectedPayAmt")


        SP.Q.FastPath("LC05I" & SSN)
        If SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then Exit Sub
        _paymentPendingInfo = SP.Q.GetText(20, 18, 61)
        _agencyMonthlyInterest = SP.Q.GetText(4, 44, 10)
        _acctBal = CDbl(SP.GetText(4, 69, 12))
        While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            For x = 7 To 16 Step 3
                dateStr = SP.Q.GetText(x, 30, 8)
                If dateStr <> "" Then
                    If SP.Q.GetText(x, 71, 10) <> "0.00" Then
                        'borrower level
                        If dt > String8ToDate(dateStr) And SP.Q.Check4Text(7, 25, "03") Then
                            dt = String8ToDate(dateStr)
                            _defaultStatus = SP.Q.GetText(x, 25, 2)
                            _defermentStatus = SP.Q.GetText(x + 1, 25, 10)
                            _errorCode = SP.Q.GetText(x + 2, 2, 5)
                            'select a loan
                            SP.Q.PutText(21, 13, Format(CInt(SP.Q.GetText(x, 2, 2)), "00"), True)
                            'go to page 2
                            SP.Q.Hit("F10")
                            _coMakerSSN = SP.Q.GetText(14, 54, 9)
                            SP.Q.Hit("F12")
                        End If
                        'loan level
                        'create a new listviewitem for the array
                        SP.Q.PutText(21, 13, Format(CInt(SP.Q.GetText(x, 2, 2)), "00"), True)
                        arr(0) = SP.Q.GetText(4, 73, 8)       'Lender payoff date A
                        arr(1) = (SP.Q.GetText(7, 34, 10))       'principal defaulted B
                        arr(2) = (SP.Q.GetText(6, 8, 6))       'interest rate
                        arr(3) = (SP.Q.GetText(8, 34, 10))       'interest defaulted C
                        arr(4) = (SP.Q.GetText(9, 34, 10))       'total amount defaulted D
                        arr(5) = (SP.Q.GetText(10, 34, 10))       'principal collected E 
                        arr(6) = (SP.Q.GetText(11, 34, 10))       'interest accruded thru F
                        arr(7) = (SP.Q.GetText(12, 34, 10))       'interest collected G
                        arr(8) = (SP.Q.GetText(13, 34, 10))       'legal costs accrued H
                        arr(9) = (SP.Q.GetText(14, 34, 10))       'legal costs collected I 
                        arr(10) = (SP.Q.GetText(15, 34, 10))       'other charges accrued J
                        arr(11) = (SP.Q.GetText(16, 34, 10))       'other charges collected K
                        arr(12) = (SP.Q.GetText(17, 34, 10))       'collection costs accrued L
                        arr(13) = (SP.Q.GetText(18, 34, 10))       'collection costs collected M
                        arr(14) = (SP.Q.GetText(19, 34, 10))       'collection costs projected N
                        arr(15) = (SP.Q.GetText(20, 34, 10))       'outstanding balance O
                        arr(16) = (SP.Q.GetText(3, 59, 1))       'direct payment indicator P
                        arr(17) = (SP.Q.GetText(19, 70, 2))       'subrogation code Q
                        arr(18) = (SP.Q.GetText(19, 73, 8))       'subrogation date R
                        arr(19) = (SP.Q.GetText(21, 11, 4))       'claim id S
                        arr(20) = (SP.Q.GetText(10, 73, 8))       'next payment due T
                        arr(34) = (SP.Q.GetText(11, 71, 10))       'expected pay due
                        'calculate weighted average interest rate
                        _avrIntRate = _avrIntRate + (Val(SP.GetText(6, 8, 6)) * (SP.GetText(20, 32, 12) / _acctBal))
                        'calculate due date if more than one then set to "various"
                        If TempDueDate = "" Then
                            TempDueDate = SP.Q.GetText(10, 73, 8)
                        ElseIf TempDueDate <> SP.Q.GetText(10, 73, 8) Then
                            VariousDueDates = True
                        End If
                        arr(21) = (SP.Q.GetText(6, 47, 2))       'federal tax offset id U
                        arr(31) = CStr(CDbl(SP.Q.GetText(7, 33, 11)) - CDbl(SP.Q.GetText(10, 33, 11)))       'current outstanding principal
                        arr(32) = CStr(CDbl(SP.Q.GetText(8, 33, 11)) - CDbl(SP.Q.GetText(12, 33, 11)))       'current interest
                        arr(33) = CStr(CDbl(SP.Q.GetText(17, 33, 11)) - CDbl(SP.Q.GetText(18, 33, 11)))       'Outstanding collection costs
                        AccountOpenDt = SP.GetText(4, 73, 2) + "/" + SP.GetText(4, 75, 2) + "/" + SP.GetText(4, 77, 4)
                        SP.Q.Hit("F10")
                        If CDate(AccountOpenDt) > Now.AddDays(-60) And SP.Check4Text(3, 73, "WG000002") Then
                            _newLoanInd = "Y"
                        End If
                        arr(22) = (SP.Q.GetText(20, 23, 6))       'minimum payment amount V 
                        arr(23) = (SP.Q.GetText(21, 2, 8)).Insert(2, "/").Insert(5, "/")       'min pay begin W
                        arr(24) = (SP.Q.GetText(21, 11, 8)).Insert(2, "/").Insert(5, "/")       'min pay end X
                        arr(25) = (SP.Q.GetText(9, 71, 2))       'garnishment code Y
                        arr(26) = (SP.Q.GetText(5, 73, 8)).Insert(2, "/").Insert(5, "/")       'date last contact Z
                        arr(27) = (SP.Q.GetText(6, 73, 8)).Insert(2, "/").Insert(5, "/")       'date last attempt A2

                        arr(28) = (SP.Q.GetText(14, 54, 9))       'endorser B2
                        arr(30) = SP.Q.GetText(17, 5, 20)       'def type
                        _rehabCode = SP.Q.GetText(7, 68, 2)     'Rehab Code
                        SP.Q.Hit("F10")
                        arr(29) = (SP.Q.GetText(8, 29, 1))       'collection cost indicator C2
                        'Add the array to the DataTable.

                        TB.Rows.Add(arr)
                        SP.Q.Hit("F12")
                    End If

                End If
            Next x
            SP.Q.Hit("F8")
        End While
        If VariousDueDates Then
            _dueDate = "Various"
        Else
            If TempDueDate IsNot Nothing And TempDueDate <> "" Then
                _dueDate = TempDueDate.Insert(2, "/").Insert(5, "/")
            Else
                _dueDate = ""
            End If
        End If
        _lC05Detail = TB
        _accountOpenDate = Format(dt, "MM/dd/yy")
        'loan level detail
        Dim HasYcc As Boolean
        Dim HasNcc As Boolean
        Dim HasYdd As Boolean
        Dim HasNdd As Boolean
        Dim TempTaxOffset As String = ""
        Dim HasYC As Boolean
        Dim HasNC As Boolean
        Dim TempDueDay As String = ""
        Dim TempMinPayBegin As String = ""
        Dim TempMinPayEnd As String = ""

        HasYcc = False
        HasNcc = False
        HasYdd = False
        HasNdd = False
        HasYC = False
        HasNC = False

        For Each R As DataRow In _lC05Detail.Rows
            'collection cost indicator
            If R.Item("CollectionCostIndicator") = "Y" Then
                HasYcc = True
            ElseIf R.Item("CollectionCostIndicator") = "N" Then
                HasNcc = True
            End If
            'direct debit ind.
            If R.Item("DirectPaymentIndicator") = "Y" Then
                HasYdd = True
            Else
                HasNdd = True
            End If
            'date last contact
            If IsDate(R.Item("DateLastContact")) Then
                If _dateLastCntct = "" Then
                    _dateLastCntct = Format(CDate(R.Item("DateLastContact")), "MM/dd/yyyy")
                Else
                    If CDate(R.Item("DateLastContact")) > CDate(_dateLastCntct) Then
                        _dateLastCntct = Format(CDate(R.Item("DateLastContact")), "MM/dd/yyyy")
                    End If
                End If
            End If
            'date last attempt
            If IsDate(R.Item("DateLastAttempt")) Then
                If _dateLastAtempt = "" Then
                    _dateLastAtempt = Format(CDate(R.Item("DateLastAttempt")), "MM/dd/yyyy")
                Else
                    If CDate(R.Item("DateLastAttempt")) > CDate(_dateLastAtempt) Then
                        _dateLastAtempt = Format(CDate(R.Item("DateLastAttempt")), "MM/dd/yyyy")
                    End If
                End If
            End If
            'federal tax offset
            If R.Item("FederalTaxOffsetId") <> "" And _taxOffsetCode <> "Various" Then
                _taxOffsetCode = R.Item("FederalTaxOffsetId")
                If TempTaxOffset <> _taxOffsetCode And TempTaxOffset <> "" Then
                    _taxOffsetCode = "Various"
                End If
                TempTaxOffset = _taxOffsetCode
            End If
            'certified
            If R.Item("FederalTaxOffsetId") = "" Or R.Item("FederalTaxOffsetId") = "04" Or R.Item("FederalTaxOffsetId") = "09" Or R.Item("FederalTaxOffsetId") = "11" Or R.Item("FederalTaxOffsetId") = "12" Then
                HasNC = True
            Else
                HasYC = True
            End If
            'Temp Payment Amount
            If IsNumeric(R.Item("MinimumPaymentAmount")) Then
                If IsDate(R.Item("MinPayEnd")) Then
                    If CDate(R.Item("MinPayEnd")) > Today Then
                        TempPaymentAmt = TempPaymentAmt + CDbl(R.Item("MinimumPaymentAmount"))
                    End If
                End If
            End If

            If _dueDay <> "Various" Then
                If R.Item("NextPaymentDue") <> "" Then
                    _dueDay = CStr(R.Item("NextPaymentDue")).Substring(2, 2)
                    If _dueDay <> TempDueDay And TempDueDay <> "" Then
                        _dueDay = "Various"
                    End If
                    TempDueDay = DueDay
                End If
            End If
            'Min Payment Begin
            If _minPayBeginDt <> "Various" Then
                If R.Item("MinPayBegin") <> "" Then
                    _minPayBeginDt = R.Item("MinPayBegin")
                    If _minPayBeginDt <> TempMinPayBegin And TempMinPayBegin <> "" Then
                        _minPayBeginDt = "Various"
                    End If
                    TempMinPayBegin = _minPayBeginDt
                End If
            End If
            'Min Payment End
            If _minPayEndDt <> "Various" Then
                If R.Item("MinPayEnd") <> "" Then
                    _minPayEndDt = R.Item("MinPayEnd")
                    If _minPayEndDt <> TempMinPayEnd And TempMinPayEnd <> "" Then
                        MinPayEndDt = "Various"
                    End If
                    TempMinPayEnd = _minPayEndDt
                End If
            End If
            'sum monthly payment amount
            If IsNumeric(R.Item("ExpectedPayAmt")) Then
                MonthlyPA = MonthlyPA + CDbl(R.Item("ExpectedPayAmt"))
            End If
        Next R

        If HasYcc And HasNcc Then
            _collectionInd = "Various"
        ElseIf HasYcc And Not HasNcc Then
            _collectionInd = "Yes"
        ElseIf Not HasYcc And HasNcc Then
            _collectionInd = "No"
        Else    'all blank
            _collectionInd = ""
        End If
        If HasYdd And HasNdd Then
            _directDebit = "Various"
        ElseIf HasYdd And Not HasNdd Then
            _directDebit = "Yes"
        ElseIf Not HasYdd And HasNdd Then
            _directDebit = "No"
        End If
        If _dateLastCntct <> "" Then
            _dateLastCntctCalculated = _dateLastCntct
        Else
            _dateLastCntctCalculated = ""
        End If
        If _dateLastAtempt <> "" Then
            _dateLastAtemptCalculated = _dateLastAtempt
        Else
            _dateLastAtemptCalculated = ""
        End If
        If HasYC And HasNC Then
            _certified = "Various"
        ElseIf HasYC And Not HasNC Then
            _certified = "Yes"
        ElseIf Not HasYC And HasNC Then
            _certified = "No"
        End If
    End Sub

    Private Sub GetCoMakerName()
        If _coMakerSSN <> "" Then
            SP.FastPath("LP22I" + _coMakerSSN)
            _coMakerName = SP.GetText(4, 44, 12) + " " + SP.GetText(4, 5, 35)
        End If
    End Sub

    Private Sub GetLC10()
        SP.Q.FastPath("LC10I" & SSN & ";N")
        If SP.Q.Check4Text(22, 3, "48012 ENTERED KEY NOT FOUND") Or SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Or SP.Q.Check4Text(22, 3, "45003") Then
            _infoFoundOnLC10 = False 'note that no info was collected from LC10
            Exit Sub
        End If
        _infoFoundOnLC10 = True
        _principalDefaulted = SP.Q.GetText(5, 36, 10)
        _interestDefaulted = SP.Q.GetText(6, 36, 10)
        _totalDefaulted = SP.Q.GetText(7, 36, 10)
        _principalCollected = SP.Q.GetText(8, 36, 10)
        _interestAccrued = SP.Q.GetText(9, 36, 10)
        _interestCollected = SP.Q.GetText(10, 36, 10)
        _legalCostAccrued = SP.Q.GetText(11, 36, 10)
        _legalCostCollected = SP.Q.GetText(12, 36, 10)
        _otherChargesAccrued = SP.Q.GetText(13, 36, 10)
        _otherChargesCollected = SP.Q.GetText(14, 36, 10)
        _collectionCostAccrued = SP.Q.GetText(15, 36, 10)
        _collectionCostCollected = SP.Q.GetText(16, 36, 10)
        _collectionFeesPojected = SP.Q.GetText(17, 36, 10)
        _outstandingBalanceDue = SP.Q.GetText(18, 36, 10)
        _totalCollected = SP.Q.GetText(5, 71, 10)
        _expectedPytAmt = SP.Q.GetText(6, 71, 10)
        SP.PutText(9, 20, Format(Today.AddDays(30), "MMddyyyy"), True)
        _payoff30Days = SP.Q.GetText(18, 36, 10)
    End Sub

    Private Sub GetLC18()
        SP.Q.FastPath("LC18I" & SSN)
        If SP.Q.Check4Text(22, 3, "48012 ENTERED KEY NOT FOUND") Then Exit Sub
        _rehabCounter = SP.Q.GetText(18, 51, 3)
        _ineligibleForRehabCode = SP.Q.GetText(18, 67, 2)
        _reinstatementEligibilityCode = SP.Q.GetText(17, 67, 1)
        _reinstatementDate = SP.Q.GetText(17, 69, 8).Insert(2, "/").Insert(5, "/")
        _day30Notice = SP.Q.GetText(14, 9, 2) & "/" & SP.Q.GetText(14, 11, 2) & "/" & SP.Q.GetText(14, 13, 4)
        If SP.Q.Check4Text(9, 29, "$$,$$$,$$$.CC") = False Then _offsetAmt = SP.Q.GetText(9, 29, 13)
        _yearsCertified = SP.Q.GetText(11, 17, 3)
        GetLC2L()
    End Sub

    Private Sub GetLC20()
        SP.Q.FastPath("LC20I" & SSN)
        If SP.Q.Check4Text(22, 3, "48012 ENTERED KEY NOT FOUND") Then Exit Sub
        If SP.Q.Check4Text(10, 2, "01 2000") = False And SP.Q.GetText(10, 2, 7) <> "01" Then
            _employerName = SP.Q.GetText(10, 21, 19)
            EmployerID = SP.Q.GetText(10, 12, 8)
            _overRideIndicator = SP.Q.GetText(10, 78, 1)
        End If
    End Sub

    Private Sub GetLC2L()
        SP.Q.FastPath("LC2L")
        Dim prevYear As String = DateTime.Now.Year - 1
        If SP.Q.Check4Text(8, 5, prevYear) Then
            SP.Q.PutText(20, 13, "1", True)
            Dim reprintDate As String = SP.Q.GetText(6, 66, 8)
            Dim reportDate As String = SP.Q.GetText(6, 55, 8)
            If (reprintDate.StartsWith("M")) Then
                reprintDate = reportDate
            End If
            _date1098ESent = reprintDate.ToDateFormat()
            _interestReported = SP.Q.GetText(6, 13, 11)
        End If
    End Sub

    Private Sub GetLC34()
        Dim x As Integer
        SP.Q.FastPath("LC34I" & SSN & ";01")
        If SP.Check4Text(1, 63, "GLOBAL UPDATE MENU") Then Exit Sub
        If SP.Q.Check4Text(1, 60, "DEFAULT GLOBAL UPDATE") Then
            Do While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                For x = 9 To 21
                    If SP.Q.GetText(x, 5, 4) <> "" Then
                        _perPaymentAmt = _perPaymentAmt + CDbl(SP.Q.GetText(x, 45, 9))
                    End If
                Next
                SP.Q.Hit("F8")
            Loop
        End If
    End Sub

    Private Sub GetLC41()
        Dim x As Integer
        Dim NineFound As Boolean = False
        Dim LastPaymentFound As Boolean = False
        SP.Q.FastPath("LC41I" & SSN & ";X")
        If SP.Check4Text(1, 63, "PAYMENT RECORD MENU") Then Exit Sub
        If SP.Q.Check4Text(1, 60, "PAYMENT RECORD SELECT") Then
            Do While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                For x = 7 To 18
                    If _paymentsFromLC41.Count < 9 And SP.Q.GetText(x, 15, 9) <> "" Then
                        _paymentsFromLC41.Add(New Payment(SP.Q.GetText(x, 15, 9), SP.Q.GetText(x, 5, 8).Insert(2, "/").Insert(5, "/"), SP.Q.GetText(x, 34, 2), SP.Q.GetText(x, 39, 2)))
                        If _paymentsFromLC41.Count = 9 Then
                            NineFound = True
                        End If
                    End If
                    If SP.Q.GetText(x, 34, 2) = "GP" And _lastPaymentRecieved = "" Then
                        _lastPaymentRecieved = SP.Q.GetText(x, 5, 8).Insert(2, "/").Insert(5, "/")
                        _lastPaymentAmount = SP.Q.GetText(x, 15, 9)
                        LastPaymentFound = True
                    End If
                    If NineFound And LastPaymentFound Then
                        Exit Sub
                    End If
                Next
                SP.Q.Hit("F8")
            Loop
        ElseIf SP.Q.Check4Text(1, 59, "PAYMENT RECORD DISPLAY") Then
            _paymentsFromLC41.Add(New Payment(SP.Q.GetText(3, 28, 10), SP.Q.GetText(4, 72, 8), SP.Q.GetText(4, 28, 10), SP.Q.GetText(6, 28, 10)))
            If SP.Q.GetText(4, 28, 10) = "GARNISHMENT" Then
                _lastPaymentRecieved = SP.Q.GetText(4, 72, 8).Insert(2, "/").Insert(5, "/")
                _lastPaymentAmount = SP.Q.GetText(3, 28, 10)
            End If
        End If
    End Sub

    Private Sub GetLC67()
        Dim Type As String
        _outstandingAWG = "No"
        _activeJudgment = "No"
        _garnishmentType = ""
        SP.Q.FastPath("LC67I" & SSN)
        If SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then Exit Sub
        If SP.Q.Check4Text(1, 59, "LEGAL ACTION SELECTION") Then
            Type = SP.Q.GetText(7, 10, 3)
            SP.Q.PutText(21, 13, "01", True)
            _primaryActionDate = SP.Q.GetText(4, 35, 2) & "/" & SP.Q.GetText(4, 37, 2) & "/" & SP.Q.GetText(4, 39, 4)
            _dateWithdrawn = SP.Q.GetText(7, 35, 2) & "/" & SP.Q.GetText(7, 37, 2) & "/" & SP.Q.GetText(7, 39, 4)
            _reasonWithdrawn = SP.Q.GetText(8, 19, 2)
            If Type = "JUD" Then
                If _dateWithdrawn = "MM/DD/CCYY" Then
                    _activeJudgment = "Yes"
                    If SP.Q.RIBM.FindText("EXECUTION", 8, 10) Then
                        _garnishmentType = "Judicial Garnishment"
                    End If
                End If
            ElseIf Type = "AWG" Then
                If _dateWithdrawn = "MM/DD/CCYY" Then
                    _outstandingAWG = "Yes"
                    _garnishmentType = "Administrative Wage Garnishment"
                End If
            End If
        ElseIf SP.Q.Check4Text(1, 57, "LEGAL ACTION AWG DISPLAY") Then
            _primaryActionDate = SP.Q.GetText(4, 35, 2) & "/" & SP.Q.GetText(4, 37, 2) & "/" & SP.Q.GetText(4, 39, 4)
            _dateWithdrawn = SP.Q.GetText(7, 35, 2) & "/" & SP.Q.GetText(7, 37, 2) & "/" & SP.Q.GetText(7, 39, 4)
            _reasonWithdrawn = SP.Q.GetText(8, 19, 2)
            If _dateWithdrawn = "MM/DD/CCYY" Then
                _activeJudgment = "Yes"
                If SP.Q.RIBM.FindText("EXECUTION", 8, 10) Then
                    _garnishmentType = "Judicial Garnishment"
                End If
            End If
        End If
    End Sub

    Private Sub DD136_DetermineVIPAndSpecialHndlAccts()
        SP.Q.FastPath("LP50I" + SSN + ";;;;;VIPSS")
        If SP.Q.Check4Text(1, 68, "ACTIVITY MENU") Then
            _vIP = False
        Else
            _vIP = True
        End If
        SP.Q.FastPath("LP50I" + SSN + ";;;;;SPHAN")
        If SP.Q.Check4Text(1, 68, "ACTIVITY MENU") Then
            _specialHandling = False
        Else
            _specialHandling = True
        End If
        SP.Q.FastPath("LP50I" + SSN + ";;;;;;")
        SP.PutText(9, 20, "DD136")
        SP.PutText(18, 29, Format(Today.AddMonths(-6), "MMddyyyy") + Format(Today, "MMddyyyy"), True)
        If SP.Q.Check4Text(1, 68, "ACTIVITY MENU") Then
            _dD136 = False
        Else
            _dD136 = True
        End If
    End Sub

    Private Sub GetLP2C()
        Dim Row As Integer = 6
        SP.FastPath("LP2CI" + SSN + ";;")
        If SP.Check4Text(1, 67, "REFERENCE MENU") Then Exit Sub 'exit sub if there is no data to be collected
        If SP.Check4Text(1, 65, "REFERENCE SELECT") Then
            While SP.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                'get reference info
                _references.Add(New Reference(SP.GetText(Row, 7, 40), SP.GetText(Row + 1, 42, 1), SP.GetText(Row + 1, 44, 1), SP.GetText(Row, 68, 10), SP.GetText(Row + 1, 33, 1)))
                Row = Row + 3
                If SP.Check4Text(Row, 3, " ") Or Row > 18 Then
                    Row = 6
                    SP.Hit("F8")
                End If
            End While
        ElseIf SP.Check4Text(1, 59, "REFERENCE DEMOGRAPHICS") Then
            'get reference info
            _references.Add(New Reference(SP.GetText(4, 44, 12) + " " + SP.GetText(4, 59, 3) + " " + SP.GetText(4, 5, 35), SP.GetText(8, 53, 1), SP.GetText(13, 42, 1), SP.GetText(3, 14, 10), SP.GetText(6, 51, 1)))
        End If
    End Sub

#End Region

#Region "Status"

    Public Sub FigureBorrowerStatus()
        Dim x As Integer
        'combine statuses from LG10 and TS26
        For x = LG10Data.GetLowerBound(1) To LG10Data.GetUpperBound(1)
            If GetStatus(LG10Data(5, x), LG10Data(6, x)) <> "" And Array.IndexOf(_statusArr, GetStatus(LG10Data(5, x), LG10Data(6, x))) < 0 Then
                _statusArr(_statusArr.GetUpperBound(0)) = GetStatus(LG10Data(5, x), LG10Data(6, x))
                ReDim Preserve _statusArr(_statusArr.GetUpperBound(0) + 1)
            End If
        Next
        For x = _borrStat.GetLowerBound(0) + 1 To _borrStat.GetUpperBound(0)
            If GetStatus(_borrStat(x), "") <> "" And Array.IndexOf(_statusArr, GetStatus(_borrStat(x), "")) < 0 Then
                _statusArr(_statusArr.GetUpperBound(0)) = (GetStatus(_borrStat(x), ""))
                ReDim Preserve _statusArr(_statusArr.GetUpperBound(0) + 1)
            End If
        Next
        'sort by rank
        Dim rank(0) As String
        For x = _statusArr.GetLowerBound(0) To _statusArr.GetUpperBound(0)
            If CStr(_statusArr.GetValue(x)) <> "" Then
                rank(x) = CStr(_status.Item(_statusArr.GetValue(x)))
                ReDim Preserve rank(rank.GetUpperBound(0) + 1)
            End If
        Next
        'display to label
        Array.Sort(rank, _statusArr)
        If _statusArr.GetUpperBound(0) < 4 Then
            For x = _statusArr.GetLowerBound(0) To _statusArr.GetUpperBound(0)
                If Not (_statusArr(x) Is Nothing) Then _hP.lbStatuses.Items.Add(_statusArr(x))
            Next
        Else
            _hP.btnMoStatus.Enabled = True
            For x = 1 To 3
                If Not (_statusArr(x) Is Nothing) Then _hP.lbStatuses.Items.Add(_statusArr(x))
            Next
        End If
    End Sub

    Private Function GetStatus(ByVal Stat As String, ByVal Rsn As String) As String
        GetStatus = ""
        If Rsn = "DE" Or Stat = "Verified Death".ToUpper Then Return "Death"
        If Stat = "Alleged Death".ToUpper Then Return "Alleged Death"
        If Rsn = "DI" Or Stat = "Verified Disability".ToUpper Then Return "Disability"
        If Stat = "Alleged Disability".ToUpper Then Return "Alleged Disability"
        If Rsn = "BC" Or Rsn = "BH" Or Rsn = "BO" Or Stat = "Verified Bankruptcy".ToUpper Then Return "Bankruptcy"
        If Stat = "Alleged Bankruptcy".ToUpper Then Return "Alleged Bankruptcy"
        If Stat = "CURE" Then Return "CURE"
        If Stat = "Repayment".ToUpper Or Stat = "RP" Then Return "In Repayment"
        If Stat = "IG" Or Stat = "In Grace".ToUpper Then Return "In Grace"
        If Stat = "IA" Or Stat = "In School".ToUpper Then Return "In School"
        If Stat = "ID" Then Return "In School/In Grace"
        If Stat = "DA" Or Stat = "Deferment".ToUpper Then Return "Deferment"
        If Stat = "FB" Or Stat = "Forbearance".ToUpper Then Return "Forbearance"
        If Stat = "CP" And (Rsn = "DF" Or Rsn = "DB" Or Rsn = "DQ" Or Rsn = "DU") Then Return "Default"
        If (Stat = "CR" And Rsn = "DF") Or Stat = "PRE-CLAIM SUBMITTED" Then Return "Preclaim"
        If Stat = "CP" And Rsn = "IN" Then Return "Ineligible"
        If ErrorCode = "40241" Then Return "Cond Disab"
        If ErrorCode = "44015" Then Return "Subrogation"
        If DefermentStatus = "DEFERRED" Then Return "DFT Hold"
    End Function

    Private Sub CreateStatusCollection()
        _status = New Collection
        _status.Add("A1", "Death")
        _status.Add("A2", "Alleged Death")
        _status.Add("A3", "Disability")
        _status.Add("A4", "Alleged Disability")
        _status.Add("A5", "Bankruptcy")
        _status.Add("A6", "Alleged Bankruptcy")
        _status.Add("A7", "CURE")
        _status.Add("C1", "In Repayment")
        _status.Add("C2", "In Grace")
        _status.Add("C3", "In School")
        _status.Add("C4", "In School/In Grace")
        _status.Add("C7", "Delinquent")
        _status.Add("C6", "Deferment")
        _status.Add("C5", "Forbearance")
        _status.Add("B1", "PIF/Deconverted")
        _status.Add("B2", "Default")
        _status.Add("B3", "Preclaim")
        _status.Add("B4", "Ineligible")
        _status.Add("A8", "Cond Disab")
        _status.Add("B6", "Subrogation")
        _status.Add("B7", "DFT Hold")
    End Sub

    Public Function AllStatuses() As ArrayList
        Dim sta As ArrayList
        sta = New ArrayList
        Dim x As Integer
        For x = _statusArr.GetLowerBound(0) To _statusArr.GetUpperBound(0)
            If _statusArr(x) <> "" Then
                sta.Add(_statusArr(x))
            End If
        Next
        Return sta
    End Function

#End Region


End Class
