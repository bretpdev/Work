Public Class IndicatorsForDemographicPart

    Public Sub New()
        _notValid = False
        _verified = False
        _invalidateFirst = False
    End Sub

    Public Sub New(ByVal tNotValid As Boolean)
        _notValid = tnotValid
        _verified = False
        _invalidateFirst = False
    End Sub

    Private _notValid As Boolean
    Public Property NotValid() As Boolean
        Get
            Return _notValid
        End Get
        Set(ByVal value As Boolean)
            _notValid = value
        End Set
    End Property

    Private _verified As Boolean
    Public Property Verified() As Boolean
        Get
            Return _verified
        End Get
        Set(ByVal value As Boolean)
            _verified = value
        End Set
    End Property

    Private _invalidateFirst As Boolean
    Public Property InvalidateFirst() As Boolean
        Get
            Return _invalidateFirst
        End Get
        Set(ByVal value As Boolean)
            _invalidateFirst = value
        End Set
    End Property

    Private _consent As Boolean
    Public Property Consent() As Boolean
        Get
            Return _consent
        End Get
        Set(ByVal value As Boolean)
            _consent = value
        End Set
    End Property


End Class
