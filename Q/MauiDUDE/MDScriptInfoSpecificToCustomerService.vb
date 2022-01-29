<CLSCompliant(True)> _
Public Class MDScriptInfoSpecificToCustomerService
    Inherits MDScriptInfoSpecificToBusinessUnitBase

    ' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***

    'Has repayment schedule indicator.
    Private _hasRepaymentSchedule As String = String.Empty
    Public Property HasRepaymentSchedule() As String
        Get
            Return _hasRepaymentSchedule
        End Get
        Set(ByVal value As String)
            _hasRepaymentSchedule = value
        End Set
    End Property

    'CurrentAmountDue
    Private _currentAmountDue As Double = 0
    Public Property CurrentAmountDue() As Double
        Get
            Return _currentAmountDue
        End Get
        Set(ByVal value As Double)
            _currentAmountDue = value
        End Set
    End Property

    'OutstandingLateFees
    Private _outstandingLateFees As Double
    Public Property OutstandingLateFees() As Double
        Get
            Return _outstandingLateFees
        End Get
        Set(ByVal value As Double)
            _outstandingLateFees = value
        End Set
    End Property

    'sum of all Installments
    Private _monthlyPaymentAmount As Double
    Public Property MonthlyPaymentAmount() As Double
        Get
            Return _monthlyPaymentAmount
        End Get
        Set(ByVal value As Double)
            _monthlyPaymentAmount = value
        End Set
    End Property



End Class
