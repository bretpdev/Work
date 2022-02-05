Public Class Loan
    Private _docId As String
    Private _loanStatus As String
    Private _reason As String
    Private _servicer As String
    Private _useForResultsDisplay As Boolean = True

    Public Property DocId() As String
        Get
            Return _docId
        End Get
        Set(ByVal value As String)
            _docId = value
        End Set
    End Property

    Public Property LoanStatus() As String
        Get
            Return _loanStatus
        End Get
        Set(ByVal value As String)
            _loanStatus = value
        End Set
    End Property

    Public Property Reason() As String
        Get
            Return _reason
        End Get
        Set(ByVal value As String)
            _reason = value
        End Set
    End Property

    Public Property Servicer() As String
        Get
            Return _servicer
        End Get
        Set(ByVal value As String)
            _servicer = value
        End Set
    End Property

    Public Property UseForResultsDisplay() As Boolean
        Get
            Return _useForResultsDisplay
        End Get
        Set(ByVal value As Boolean)
            _useForResultsDisplay = value
        End Set
    End Property

    Public Sub New(Optional ByVal loanStatus As String = "", Optional ByVal servicer As String = "", Optional ByVal docId As String = "", Optional ByVal reason As String = "")
        _loanStatus = loanStatus
        _servicer = servicer
        _docId = docId
        _reason = reason
    End Sub
End Class
