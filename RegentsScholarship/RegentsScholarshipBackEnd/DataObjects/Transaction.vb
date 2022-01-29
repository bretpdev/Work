Public Class Transaction
    Private _userId As String
    Public Property UserId() As String
        Get
            Return _userId
        End Get
        Set(ByVal value As String)
            _userId = value
        End Set
    End Property

    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _timeStamp As DateTime
    Public Property TimeStamp() As DateTime
        Get
            Return _timeStamp
        End Get
        Set(ByVal value As DateTime)
            _timeStamp = value
        End Set
    End Property

    Private _changedProperty As String
    Public Property ChangedProperty() As String
        Get
            Return _changedProperty
        End Get
        Set(ByVal value As String)
            _changedProperty = value
        End Set
    End Property

    Private _oldValue As String
    Public Property OldValue() As String
        Get
            Return _oldValue
        End Get
        Set(ByVal value As String)
            _oldValue = value
        End Set
    End Property

    Private _newValue As String
    Public Property NewValue() As String
        Get
            Return _newValue
        End Get
        Set(ByVal value As String)
            _newValue = value
        End Set
    End Property
End Class
