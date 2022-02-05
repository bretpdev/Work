<CLSCompliant(True)> _
Public Class BorrowerDemographics
    Inherits PersonDemographics

    Private _ssn As String
    ''' <summary>
    ''' Borrower Social Security Number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SSN() As String
        Get
            Return _ssn
        End Get
        Set(ByVal value As String)
            _ssn = value
        End Set
    End Property

    Private _accountNumber As String
    ''' <summary>
    ''' Borrower Account Number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountNumber() As String
        Get
            Return _accountNumber
        End Get
        Set(ByVal value As String)
            _accountNumber = value
        End Set
    End Property


End Class
