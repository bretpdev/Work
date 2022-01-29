Public Class Student

    Private _accntId As String
    Public Property AccntId() As String
        Get
            Return _accntId
        End Get
        Set(ByVal value As String)
            _accntId = value
        End Set
    End Property

    Private _firstName As String
    Public Property firstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Private _lastName As String
    Public Property lastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _address1 As String
    Public Property address1() As String
        Get
            Return _address1
        End Get
        Set(ByVal value As String)
            _address1 = value
        End Set
    End Property

    Private _address2 As String
    Public Property address2() As String
        Get
            Return _address2
        End Get
        Set(ByVal value As String)
            _address2 = value
        End Set
    End Property

    Private _city As String
    Public Property city() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property

    Private _state As String
    Public Property state() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property

    Private _zip As String
    Public Property zip() As String
        Get
            Return _zip
        End Get
        Set(ByVal value As String)
            _zip = value
        End Set
    End Property
End Class
