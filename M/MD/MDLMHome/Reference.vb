Public Class Reference
    Inherits System.Windows.Forms.ListViewItem

    Private _theName As String
    Public Property TheName() As String
        Get
            Return _theName
        End Get
        Set(ByVal value As String)
            _theName = value
        End Set
    End Property

    Private _addrValInd As String
    Public Property AddrValInd() As String
        Get
            Return _addrValInd
        End Get
        Set(ByVal value As String)
            _addrValInd = value
        End Set
    End Property

    Private _phoneValInd As String
    Public Property PhoneValInd() As String
        Get
            Return _phoneValInd
        End Get
        Set(ByVal value As String)
            _phoneValInd = value
        End Set
    End Property

    Private _iD As String
    Public Property ID() As String
        Get
            Return _iD
        End Get
        Set(ByVal value As String)
            _iD = value
        End Set
    End Property

    Private _relationship As String
    Public Property Relationship() As String
        Get
            Return _relationship
        End Get
        Set(ByVal value As String)
            _relationship = value
        End Set
    End Property

    Private _thirdPartyAuth As String
    Public Property ThirdPartyAuth() As String
        Get
            Return _thirdPartyAuth
        End Get
        Set(ByVal value As String)
            _thirdPartyAuth = value
        End Set
    End Property

    Private _status As String
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _addr1 As String
    Public Property Addr1() As String
        Get
            Return _addr1
        End Get
        Set(ByVal value As String)
            _addr1 = value
        End Set
    End Property

    Private _addr2 As String
    Public Property Addr2() As String
        Get
            Return _addr2
        End Get
        Set(ByVal value As String)
            _addr2 = value
        End Set
    End Property

    Private _city As String
    Public Property City() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property

    Private _state As String
    Public Property State() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property

    Private _zip As String
    Public Property Zip() As String
        Get
            Return _zip
        End Get
        Set(ByVal value As String)
            _zip = value
        End Set
    End Property

    Private _phone As String
    Public Property Phone() As String
        Get
            Return _phone
        End Get
        Set(ByVal value As String)
            _phone = value
        End Set
    End Property

    Private _oPhone As String
    Public Property OPhone() As String
        Get
            Return _oPhone
        End Get
        Set(ByVal value As String)
            _oPhone = value
        End Set
    End Property

    Private _oPhoneValInd As String
    Public Property OPhoneValInd() As String
        Get
            Return _oPhoneValInd
        End Get
        Set(ByVal value As String)
            _oPhoneValInd = value
        End Set
    End Property

    Private _email As String
    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal tName As String, ByVal tAddrValInd As String, ByVal tPhoneValInd As String, ByVal tID As String, ByVal ThrdPrtyAuth As String)
        _theName = tName
        _addrValInd = tAddrValInd
        _phoneValInd = tPhoneValInd
        _iD = tID
        _thirdPartyAuth = ThrdPrtyAuth
        Me.Text = _theName
        Me.SubItems.Add(_thirdPartyAuth)
        Me.SubItems.Add(_iD)
    End Sub

    Public Sub GetInfoFromLP2C()
        SP.FastPath("LP2CI;" + _iD)
        _relationship = SP.GetText(6, 20, 14)
        'already have third party auth
        _status = SP.GetText(6, 67, 1)
        _addr1 = SP.GetText(8, 9, 35)
        _addr2 = SP.GetText(9, 9, 35)
        'already have the address valid ind
        _city = SP.GetText(10, 9, 30)
        _state = SP.GetText(10, 52, 2)
        _zip = SP.GetText(10, 60, 9)
        _phone = SP.GetText(13, 16, 10)
        'already have the phone valid ind
        _oPhone = SP.GetText(14, 16, 10)
        _oPhoneValInd = SP.GetText(14, 36, 1)
        _email = SP.GetText(17, 7, 59)
    End Sub

End Class
