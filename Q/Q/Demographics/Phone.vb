Public Class Phone

    Private _phoneType As String
    Public Property PhoneType() As String
        Get
            Return _phoneType
        End Get
        Set(ByVal value As String)
            _phoneType = value
        End Set
    End Property

    Private _mblIndicator As String
    Public Property MBLIndicator() As String
        Get
            Return _mblIndicator
        End Get
        Set(ByVal value As String)
            _mblIndicator = value
        End Set
    End Property


    Private _consentIndicator As String
    Public Property ConsentIndicator() As String
        Get
            Return _consentIndicator
        End Get
        Set(ByVal value As String)
            _consentIndicator = value
        End Set
    End Property


    Private _verifiedDate As Date
    Public Property VerifiedDate() As Date
        Get
            Return _verifiedDate
        End Get
        Set(ByVal value As Date)
            _verifiedDate = value
        End Set
    End Property


    Private _validityIndicator As String
    Public Property ValidityIndicator() As String
        Get
            Return _validityIndicator
        End Get
        Set(ByVal value As String)
            _validityIndicator = value
        End Set
    End Property


    Private _domesticAreaCode As String
    Public Property DomesticAreaCode() As String
        Get
            Return _domesticAreaCode
        End Get
        Set(ByVal value As String)
            _domesticAreaCode = value
        End Set
    End Property


    Private _domesticPrefix As String
    Public Property DomesticPrefix() As String
        Get
            Return _domesticPrefix
        End Get
        Set(ByVal value As String)
            _domesticPrefix = value
        End Set
    End Property


    Private _domesticLineNumber As String
    Public Property DomesticLineNumber() As String
        Get
            Return _domesticLineNumber
        End Get
        Set(ByVal value As String)
            _domesticLineNumber = value
        End Set
    End Property


    Private _extension As String
    Public Property Extension() As String
        Get
            Return _extension
        End Get
        Set(ByVal value As String)
            _extension = value
        End Set
    End Property


    Private _foreignCountryCode As String
    Public Property ForeignCountryCode() As String
        Get
            Return _foreignCountryCode
        End Get
        Set(ByVal value As String)
            _foreignCountryCode = value
        End Set
    End Property


    Private _foreignCityCode As String
    Public Property ForeignCityCode() As String
        Get
            Return _foreignCityCode
        End Get
        Set(ByVal value As String)
            _foreignCityCode = value
        End Set
    End Property


    Private _foreignLocalNumber As String
    Public Property ForeignLocalNumber() As String
        Get
            Return _foreignLocalNumber
        End Get
        Set(ByVal value As String)
            _foreignLocalNumber = value
        End Set
    End Property

End Class
