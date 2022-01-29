<CLSCompliant(True)> _
Public Class MDBorrower

    Public Sub New()
        ScriptInfoToGenericBusinessUnit = Nothing 'Null it out.  It will be populated in the business unit specific borrower object.
    End Sub

    Private _borLite As MDBorrowerLite
    ''' <summary>
    ''' This object acts kind of like a boot strapper for the actual borrower object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BorLite() As MDBorrowerLite
        Get
            Return _borLite
        End Get
        Set(ByVal value As MDBorrowerLite)
            _borLite = value
        End Set
    End Property


    ''''''''''''''''''''''''''''''''''''''
    ''''''Borrower Demographics'''''''''''
    ''''''''''''''''''''''''''''''''''''''

    Private _sSN As String
    ''' <summary>
    ''' Borrower's SSN
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SSN() As String
        Get
            Return _sSN
        End Get
        Set(ByVal value As String)
            _sSN = value
        End Set
    End Property

    Private _cLAccNum As String
    ''' <summary>
    ''' Borrower's account number.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CLAccNum() As String
        Get
            Return _cLAccNum
        End Get
        Set(ByVal value As String)
            _cLAccNum = value
        End Set
    End Property

    Private _name As String
    ''' <summary>
    ''' Borrower's first and last names combined.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _firstName As String
    ''' <summary>
    ''' Borrower's first name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Private _mI As String
    ''' <summary>
    ''' Borrower's middle initial.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MI() As String
        Get
            Return _mI
        End Get
        Set(ByVal value As String)
            _mI = value
        End Set
    End Property

    Private _lastName As String
    ''' <summary>
    ''' Borrower's last name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _dOB As String
    ''' <summary>
    ''' Borrower's date of birth
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DOB() As String
        Get
            Return _dOB
        End Get
        Set(ByVal value As String)
            _dOB = value
        End Set
    End Property

    Private _pOBoxAllowed As Boolean
    ''' <summary>
    ''' Indicator for whether a PO box should be allowed or not for the borrower.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property POBoxAllowed() As Boolean
        Get
            Return _pOBoxAllowed
        End Get
        Set(ByVal value As Boolean)
            _pOBoxAllowed = value
        End Set
    End Property

    Private _demographicsVerified As Boolean
    ''' <summary>
    ''' Indicator for whether the user has verified demographics yet or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DemographicsVerified() As Boolean
        Get
            Return _demographicsVerified
        End Get
        Set(ByVal value As Boolean)
            _demographicsVerified = value
        End Set
    End Property

    Private _oneLINKDemos As MDBorrowerDemographics
    ''' <summary>
    ''' The OneLINK demographics information for the borrower.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OneLINKDemos() As MDBorrowerDemographics
        Get
            Return _oneLINKDemos
        End Get
        Set(ByVal value As MDBorrowerDemographics)
            _oneLINKDemos = value
        End Set
    End Property

    Private _compassDemos As MDBorrowerDemographics
    ''' <summary>
    ''' The COMPASS demographics information for the borrower.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CompassDemos() As MDBorrowerDemographics
        Get
            Return _compassDemos
        End Get
        Set(ByVal value As MDBorrowerDemographics)
            _compassDemos = value
        End Set
    End Property

    Private _userProvidedDemos As MDBorrowerDemographics
    ''' <summary>
    ''' The user provided demographics information for the borrower.  This is what the systems will be updated with.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UserProvidedDemos() As MDBorrowerDemographics
        Get
            Return _userProvidedDemos
        End Get
        Set(ByVal value As MDBorrowerDemographics)
            _userProvidedDemos = value
        End Set
    End Property


    Private _altAddress As MDBorrowerDemographics
    ''' <summary>
    ''' The borrower's alternate address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AltAddress() As MDBorrowerDemographics
        Get
            Return _altAddress
        End Get
        Set(ByVal value As MDBorrowerDemographics)
            _altAddress = value
        End Set
    End Property


    ''''''''''''''''''''''''''''''''''''''
    'Data from the Main Menu''''''''''''''
    ''''''''''''''''''''''''''''''''''''''
    Private _lG10Data(6, 0) As String
    ''' <summary>
    ''' Collected LG10 data for borrower.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LG10Data() As String(,)
        Get
            Return _lG10Data
        End Get
        Set(ByVal value As String(,))
            _lG10Data = value
        End Set
    End Property

    Private _aCHData As MDClassACHInfo
    ''' <summary>
    ''' ACH information for the borrower.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ACHData() As MDClassACHInfo
        Get
            Return _aCHData
        End Get
        Set(ByVal value As MDClassACHInfo)
            _aCHData = value
        End Set
    End Property

    Private _onTime48Monthly As Integer
    Public Property OnTime48Monthly() As Integer
        Get
            Return _onTime48Monthly
        End Get
        Set(ByVal value As Integer)
            _onTime48Monthly = value
        End Set
    End Property

    Private _onTime48Eligible As Boolean
    Public Property OnTime48Eligible() As Boolean
        Get
            Return _onTime48Eligible
        End Get
        Set(ByVal value As Boolean)
            _onTime48Eligible = value
        End Set
    End Property

    Private _monthlyPA As Double
    Public Property MonthlyPA() As Double
        Get
            Return _monthlyPA
        End Get
        Set(ByVal value As Double)
            _monthlyPA = value
        End Set
    End Property

    Private _showTCX06Values As Boolean
    Public Property ShowTCX06Values() As Boolean
        Get
            Return _showTCX06Values
        End Get
        Set(ByVal value As Boolean)
            _showTCX06Values = value
        End Set
    End Property

    Private _found3rd As Boolean
    Public Property Found3rd() As Boolean
        Get
            Return _found3rd
        End Get
        Set(ByVal value As Boolean)
            _found3rd = value
        End Set
    End Property

    Private _borrowerIsLCOOnly As Boolean
    Public Property BorrowerIsLCOOnly() As Boolean
        Get
            Return _borrowerIsLCOOnly
        End Get
        Set(ByVal value As Boolean)
            _borrowerIsLCOOnly = value
        End Set
    End Property

    'ITS0N

    Private _dateDelinquencyOccurred As String = ""
    Public Property DateDelinquencyOccurred() As String
        Get
            Return _dateDelinquencyOccurred
        End Get
        Set(ByVal value As String)
            _dateDelinquencyOccurred = value
        End Set
    End Property

    Private _numDaysDelinquent As String = ""
    Public Property NumDaysDelinquent() As String
        Get
            Return _numDaysDelinquent
        End Get
        Set(ByVal value As String)
            _numDaysDelinquent = value
        End Set
    End Property

    Private _amountPastDue As Double
    Public Property AmountPastDue() As Double
        Get
            Return _amountPastDue
        End Get
        Set(ByVal value As Double)
            _amountPastDue = value
        End Set
    End Property

    Private _totalAmountDue As Double
    Public Property TotalAmountDue() As Double
        Get
            Return _totalAmountDue
        End Get
        Set(ByVal value As Double)
            _totalAmountDue = value
        End Set
    End Property

    Private _totalLateFeesDue As Double
    Public Property TotalLateFeesDue() As Double
        Get
            Return _totalLateFeesDue
        End Get
        Set(ByVal value As Double)
            _totalLateFeesDue = value
        End Set
    End Property

    Private _principal As Decimal
    Public Property Principal() As Decimal
        Get
            Return _principal
        End Get
        Set(ByVal value As Decimal)
            _principal = value
        End Set
    End Property

    Private _interest As Decimal
    Public Property Interest() As Decimal
        Get
            Return _interest
        End Get
        Set(ByVal value As Decimal)
            _interest = value
        End Set
    End Property

    Private _dailyInterest As Decimal
    Public Property DailyInterest() As Decimal
        Get
            Return _dailyInterest
        End Get
        Set(ByVal value As Decimal)
            _dailyInterest = value
        End Set
    End Property
    ''''''''''''''''''''''''''''''''''''''

    Private _scripts As String
    Public Property Scripts() As String
        Get
            Return _scripts
        End Get
        Set(ByVal value As String)
            _scripts = value
        End Set
    End Property

    ''''''''''''''''''''''''''''''''''''''
    Private _activityCode As String
    ''' <summary>
    ''' Activity code selected by the user that will be entered for activity comments.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActivityCode() As String
        Get
            Return _activityCode
        End Get
        Set(ByVal value As String)
            _activityCode = value
        End Set
    End Property

    Private _contactCode As String
    ''' <summary>
    ''' Contact code selected by the user that will be entered for activity comments.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ContactCode() As String
        Get
            Return _contactCode
        End Get
        Set(ByVal value As String)
            _contactCode = value
        End Set
    End Property

    Private _attemptType As String
    Public Property AttemptType() As String
        Get
            Return _attemptType
        End Get
        Set(ByVal value As String)
            _attemptType = value
        End Set
    End Property

    ''''''''''''''''''''''''''''''''''''''

    Private _tCX04SelectionValue As String
    Public Property TCX04SelectionValue() As String
        Get
            Return _tCX04SelectionValue
        End Get
        Set(ByVal value As String)
            _tCX04SelectionValue = value
        End Set
    End Property

    Private _tCX14SelectionValue As String
    Public Property TCX14SelectionValue() As String
        Get
            Return _tCX14SelectionValue
        End Get
        Set(ByVal value As String)
            _tCX14SelectionValue = value
        End Set
    End Property

    Private _notes As String
    Public Property Notes() As String
        Get
            Return _notes
        End Get
        Set(ByVal value As String)
            _notes = value
        End Set
    End Property

    Private _activityCmts As MDActivityComments
    ''' <summary>
    ''' Object that is used to add LP50, TD22 and TD37 activity comments.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActivityCmts() As MDActivityComments
        Get
            Return _activityCmts
        End Get
        Set(ByVal value As MDActivityComments)
            _activityCmts = value
        End Set
    End Property

    Private _specialCommentArr(2, 0) As String
    Public Property SpecialCommentArr() As String(,)
        Get
            Return _specialCommentArr
        End Get
        Set(ByVal value As String(,))
            _specialCommentArr = value
        End Set
    End Property

    Private _runSpecialComments As Boolean
    Public Property RunSpecialComments() As Boolean
        Get
            Return _runSpecialComments
        End Get
        Set(ByVal value As Boolean)
            _runSpecialComments = value
        End Set
    End Property

    Private _loanProgramsDistinctList As List(Of String)
    Public Property LoanProgramsDistinctList() As List(Of String)
        Get
            Return _loanProgramsDistinctList
        End Get
        Set(ByVal value As List(Of String))
            _loanProgramsDistinctList = value
        End Set
    End Property


    ''' <summary>
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <remarks></remarks>
    Private _scriptInfoToGenericBusinessUnit As MDScriptInfoSpecificToBusinessUnitBase
    ''' <summary>
    ''' This object has data specific to the BU that the borrower was created for.  This should never be used as is, it should always be up cast the object type it really is.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ScriptInfoToGenericBusinessUnit() As MDScriptInfoSpecificToBusinessUnitBase
        Get
            Return _scriptInfoToGenericBusinessUnit
        End Get
        Set(ByVal value As MDScriptInfoSpecificToBusinessUnitBase)
            _scriptInfoToGenericBusinessUnit = value
        End Set
    End Property


    Private _calculatedBorrowerStatusListFromLG10 As List(Of String)
    Public Property CalculatedBorrowerStatusListFromLG10() As List(Of String)
        Get
            Return _calculatedBorrowerStatusListFromLG10
        End Get
        Set(ByVal value As List(Of String))
            _calculatedBorrowerStatusListFromLG10 = value
        End Set
    End Property




End Class




