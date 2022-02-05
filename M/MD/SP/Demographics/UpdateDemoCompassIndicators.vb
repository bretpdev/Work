Public Class UpdateDemoCompassIndicators
    Inherits UpdateDemoSystemIndicators

    Private _other2Phone As Boolean = False
    Public Property Other2Phone() As Boolean
        Get
            Return _other2Phone
        End Get
        Set(ByVal value As Boolean)
            _other2Phone = value
        End Set
    End Property

    Private _other2PhoneIndicator As Boolean = False
    Public Property Other2PhoneIndicator() As Boolean
        Get
            Return _other2PhoneIndicator
        End Get
        Set(ByVal value As Boolean)
            _other2PhoneIndicator = value
        End Set
    End Property

    Private _other2Consent As Boolean = False
    Public Property Other2Consent() As Boolean
        Get
            Return _other2Consent
        End Get
        Set(ByVal value As Boolean)
            _other2Consent = value
        End Set
    End Property

    Private _other2MBL As Boolean = False
    Public Property Other2MBL() As Boolean
        Get
            Return _other2MBL
        End Get
        Set(ByVal value As Boolean)
            _other2MBL = value
        End Set
    End Property

End Class
