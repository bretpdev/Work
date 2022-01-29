Public Class DSEndOfCycleOutcomes
    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _outcome As String
    Public Property Outcome() As String
        Get
            Return _outcome
        End Get
        Set(ByVal value As String)
            _outcome = value
        End Set
    End Property
End Class
