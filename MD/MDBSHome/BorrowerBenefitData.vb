Imports System.Globalization

Public Class BorrowerBenefitData
    'Define some dates of interest:
    Dim _april06 As New DateTime(2006, 4, 30)
    Dim _january08 As New DateTime(2008, 1, 1)
    Dim _may08 As New DateTime(2008, 5, 1)
    Dim _july08 As New DateTime(2008, 7, 1)
    'And discriminating loan types:
    Dim _consolTypes() As String = {"UNCNS", "SUBCNS", "SUBSPC", "UNSPC"}

    'All properties are used for display in a DataGrid, so leave them as strings.
    Private _loanSequence As String
    Public Property LoanSequence() As String
        Get
            Return _loanSequence
        End Get
        Set(ByVal value As String)
            _loanSequence = value
        End Set
    End Property

    Private _firstDisbursementDate As DateTime
    Public Property FirstDisbursementDate() As String
        Get
            Return _firstDisbursementDate.ToString("MM/dd/yy")
        End Get
        Set(ByVal value As String)
            _firstDisbursementDate = DateTime.ParseExact(value, "MM/dd/yy", CultureInfo.InvariantCulture)
        End Set
    End Property

    Private _loanType As String
    Public Property LoanType() As String
        Get
            Return _loanType
        End Get
        Set(ByVal value As String)
            _loanType = value
        End Set
    End Property

    Private _currentBalance As String
    Public Property CurrentBalance() As String
        Get
            Return _currentBalance
        End Get
        Set(ByVal value As String)
            _currentBalance = value
        End Set
    End Property

    Private _beginningBalance As String
    Public Property BeginningBalance() As String
        Get
            Return _beginningBalance
        End Get
        Set(ByVal value As String)
            _beginningBalance = value
        End Set
    End Property

    Private _interestRate As String
    Public Property InterestRate() As String
        Get
            Return _interestRate
        End Get
        Set(ByVal value As String)
            _interestRate = value
        End Set
    End Property

    Private _onTimeMonthlyPayments As String
    Public Property OnTimeMontlyPayments() As String
        Get
            Return _onTimeMonthlyPayments
        End Get
        Set(ByVal value As String)
            _onTimeMonthlyPayments = value
        End Set
    End Property

    Private _requiredOnTimePayments As String
    Public Property RequiredOnTimePayments() As String
        Get
            Return _requiredOnTimePayments
        End Get
        Set(ByVal value As String)
            _requiredOnTimePayments = value
        End Set
    End Property

    Private _statutoryInterestRate As String
    Public Property StatutoryInterestRate() As String
        Get
            Return _statutoryInterestRate
        End Get
        Set(ByVal value As String)
            _statutoryInterestRate = value
        End Set
    End Property

    Private _hasAch As String
    Public Property HasAch() As String
        Get
            Return _hasAch
        End Get
        Set(ByVal value As String)
            _hasAch = value
        End Set
    End Property

    Public ReadOnly Property AchRate() As String
        Get
            If _consolTypes.Contains(_loanType) Then
                If _firstDisbursementDate <= _april06 Then
                    Return "1.25%"
                ElseIf _firstDisbursementDate < _january08 Then
                    Return "0.5%"
                ElseIf _firstDisbursementDate < _may08 Then
                    Return "0.25%"
                Else
                    Return "0.0%"
                End If
            ElseIf _loanType = "TILP" Then
                Return "1.0%"
            Else
                If _firstDisbursementDate <= _april06 Then
                    Return "1.25%"
                ElseIf _firstDisbursementDate < _january08 Then
                    Return "1.25%"
                ElseIf _firstDisbursementDate < _july08 Then
                    Return "0.5%"
                Else
                    Return "0.25%"
                End If
            End If
        End Get
    End Property

    Public ReadOnly Property RirPayments() As String
        Get
            Dim payments As String = _onTimeMonthlyPayments
            If Not String.IsNullOrEmpty(_requiredOnTimePayments) Then payments += "/" + _requiredOnTimePayments
            Return payments
        End Get
    End Property

    Public ReadOnly Property RirRate() As String
        Get
            If _consolTypes.Contains(_loanType) Then
                If _firstDisbursementDate <= _april06 Then
                    Return "1.00% RDC"
                ElseIf _firstDisbursementDate < _january08 Then
                    Return "1.00% RDC"
                Else
                    Return "N/A"
                End If
            Else
                If _firstDisbursementDate <= _april06 Then
                    Return "2.00% RDC"
                ElseIf _firstDisbursementDate < _january08 Then
                    Return "2.00% RDC"
                ElseIf _firstDisbursementDate < _may08 Then
                    Return "2.00% RBT"
                Else
                    Return "No RIR Program"
                End If
            End If
        End Get
    End Property

    Public ReadOnly Property Hep() As String
        Get
            If _firstDisbursementDate <= _april06 Then
                Return "Y"
            Else
                Return "N"
            End If
        End Get
    End Property
End Class
