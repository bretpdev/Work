Public Class DSFinalWeeklyOutcomes
    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _incomplete As Integer
    Public Property Incomplete() As Integer
        Get
            Return _incomplete
        End Get
        Set(ByVal value As Integer)
            _incomplete = value
        End Set
    End Property

    Private _pendingDenial As Integer
    Public Property PendingDenial() As Integer
        Get
            Return _pendingDenial
        End Get
        Set(ByVal value As Integer)
            _pendingDenial = value
        End Set
    End Property

    Private _denied As Integer
    Public Property Denied() As Integer
        Get
            Return _denied
        End Get
        Set(ByVal value As Integer)
            _denied = value
        End Set
    End Property

    Private _pendingBaseOnly As Integer
    Public Property PendingBaseOnly() As Integer
        Get
            Return _pendingBaseOnly
        End Get
        Set(ByVal value As Integer)
            _pendingBaseOnly = value
        End Set
    End Property

    Private _baseOnly As Integer
    Public Property BaseOnly() As Integer
        Get
            Return _baseOnly
        End Get
        Set(ByVal value As Integer)
            _baseOnly = value
        End Set
    End Property

    Private _pendingBaseAndExemplary As Integer
    Public Property PendingBaseAndExemplary() As Integer
        Get
            Return _pendingBaseAndExemplary
        End Get
        Set(ByVal value As Integer)
            _pendingBaseAndExemplary = value
        End Set
    End Property

    Private _baseAndExemplary As Integer
    Public Property BaseAndExemplary() As Integer
        Get
            Return _baseAndExemplary
        End Get
        Set(ByVal value As Integer)
            _baseAndExemplary = value
        End Set
    End Property

    Private _uesp As Integer
    Public Property UESP() As Integer
        Get
            Return _uesp
        End Get
        Set(ByVal value As Integer)
            _uesp = value
        End Set
    End Property
End Class
