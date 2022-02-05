Public Class UpdateDemoSystemIndicators

    Private _address As Boolean = False
    Public Property Address() As Boolean
        Get
            Return _address
        End Get
        Set(ByVal value As Boolean)
            _address = value
        End Set
    End Property

    Private _addressIndicator As Boolean = False
    Public Property AddressIndicator() As Boolean
        Get
            Return _addressIndicator
        End Get
        Set(ByVal value As Boolean)
            _addressIndicator = value
        End Set
    End Property

    Private _phone As Boolean = False
    Public Property Phone() As Boolean
        Get
            Return _phone
        End Get
        Set(ByVal value As Boolean)
            _phone = value
        End Set
    End Property

    Private _phoneIndicator As Boolean = False
    Public Property PhoneIndicator() As Boolean
        Get
            Return _phoneIndicator
        End Get
        Set(ByVal value As Boolean)
            _phoneIndicator = value
        End Set
    End Property

    Private _otherPhone As Boolean = False
    Public Property OtherPhone() As Boolean
        Get
            Return _otherPhone
        End Get
        Set(ByVal value As Boolean)
            _otherPhone = value
        End Set
    End Property

    Private _otherPhoneIndicator As Boolean = False
    Public Property OtherPhoneIndicator() As Boolean
        Get
            Return _otherPhoneIndicator
        End Get
        Set(ByVal value As Boolean)
            _otherPhoneIndicator = value
        End Set
    End Property

    Private _email As Boolean = False
    Public Property Email() As Boolean
        Get
            Return _email
        End Get
        Set(ByVal value As Boolean)
            _email = value
        End Set
    End Property

    Private _emailIndicator As Boolean = False
    Public Property EmailIndicator() As Boolean
        Get
            Return _emailIndicator
        End Get
        Set(ByVal value As Boolean)
            _emailIndicator = value
        End Set
    End Property


    Private _phoneConsent As Boolean
    Public Property PhoneConsent() As Boolean
        Get
            Return _phoneConsent
        End Get
        Set(ByVal value As Boolean)
            _phoneConsent = value
        End Set
    End Property


    Private _otherPhoneConsent As Boolean
    Public Property OtherPhoneConsent() As Boolean
        Get
            Return _otherPhoneConsent
        End Get
        Set(ByVal value As Boolean)
            _otherPhoneConsent = value
        End Set
    End Property


    Private _phoneMBL As Boolean = False
    Public Property PhoneMBL() As Boolean
        Get
            Return _phoneMBL
        End Get
        Set(ByVal value As Boolean)
            _phoneMBL = value
        End Set
    End Property

    Private _otherMBL As Boolean = False
    Public Property OtherMBL() As Boolean
        Get
            Return _otherMBL
        End Get
        Set(ByVal value As Boolean)
            _otherMBL = value
        End Set
    End Property


    Private _phoneNoPhoneIndicator As Boolean = False
    Public Property PhoneNoPhoneIndicator() As Boolean
        Get
            Return _phoneNoPhoneIndicator
        End Get
        Set(ByVal value As Boolean)
            _phoneNoPhoneIndicator = value
        End Set
    End Property


End Class
