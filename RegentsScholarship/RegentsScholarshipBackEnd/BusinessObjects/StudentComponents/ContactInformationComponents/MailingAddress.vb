Public Class MailingAddress
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _addressType As String
    Public Function AddressType() As String
        Return _addressType
    End Function

    Private _city As String
    Public Property City() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property

    Private _country As String
    Public Property Country() As String
        Get
            Return _country
        End Get
        Set(ByVal value As String)
            _country = value
        End Set
    End Property

    Private _isValid As Boolean
    Public Property IsValid() As Boolean
        Get
            Return _isValid
        End Get
        Set(ByVal value As Boolean)
            _isValid = value
        End Set
    End Property

    Private _lastUpdated As DateTime
    Public Property LastUpdated() As DateTime
        Get
            Return _lastUpdated
        End Get
        Set(ByVal value As DateTime)
            _lastUpdated = value
        End Set
    End Property

    Private _line1 As String
    Public Property Line1() As String
        Get
            Return _line1
        End Get
        Set(ByVal value As String)
            _line1 = value
        End Set
    End Property

    Private _line2 As String
    Public Property Line2() As String
        Get
            Return _line2
        End Get
        Set(ByVal value As String)
            _line2 = value
        End Set
    End Property

    Private _parentContactInfo As ContactInformation
    Public Function ParentContactInfo() As ContactInformation
        Return _parentContactInfo
    End Function

    Private _state As String
    Public Property State() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property

    Private _zipCode As String
    Public Property ZipCode() As String
        Get
            Return _zipCode
        End Get
        Set(ByVal value As String)
            _zipCode = value
        End Set
    End Property
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _cityOriginal As String
    Private _countryOriginal As String
    Private _isValidOriginal As Boolean
    Private _line1Original As String
    Private _line2Original As String
    Private _stateOriginal As String
    Private _zipCodeOriginal As String
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal addressType As String, ByVal parentContactInfo As ContactInformation)
        _objectIsNew = True
        _addressType = addressType
        _city = ""
        _country = ""
        _isValid = True
        _lastUpdated = DateTime.Now
        _line1 = ""
        _line2 = ""
        _parentContactInfo = parentContactInfo
        _state = ""
        _zipCode = ""
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetMailingAddress(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            _lastUpdated = DateTime.Now
            DataAccess.SetMailingAddress(Me)
            RecordTransactions(userId)
        End If
    End Sub

    Public Shared Function Load(ByVal addressType As String, ByVal parentContactInfo As ContactInformation) As MailingAddress
        Dim storedAddress As MailingAddress = DataAccess.GetMailingAddress(addressType, parentContactInfo)
        'DataAccess may return a null object, which we can't have. Create a new one if needed.
        If (storedAddress Is Nothing) Then storedAddress = New MailingAddress(addressType, parentContactInfo) With {.City = "", .Country = "", .IsValid = True, .LastUpdated = DateTime.Now, .Line1 = "", .Line2 = "", .State = "", .ZipCode = ""}
        storedAddress._objectIsNew = False
        storedAddress._addressType = addressType
        storedAddress._parentContactInfo = parentContactInfo
        storedAddress.SetChangeTrackingVariables()
        Return storedAddress
    End Function

    Public Sub Validate()
        'Nothing to check for this class.
    End Sub

    Private Function HasChanges() As Boolean
        If (_city <> _cityOriginal) Then Return True
        If (_country <> _countryOriginal) Then Return True
        If (_isValid <> _isValidOriginal) Then Return True
        If (_line1 <> _line1Original) Then Return True
        If (_line2 <> _line2Original) Then Return True
        If (_state <> _stateOriginal) Then Return True
        If (_zipCode <> _zipCodeOriginal) Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentContactInfo.ParentStudent.StateStudentId
        If (_city <> _cityOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} address city", _addressType), _cityOriginal, _city)
        End If
        If (_country <> _countryOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} address country", _addressType), _countryOriginal, _country)
        End If
        If (_isValid <> _isValidOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} address is valid", _addressType), _isValidOriginal.ToString(), _isValid.ToString())
        End If
        If (_line1 <> _line1Original) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} address 1", _addressType), _line1Original, _line1)
        End If
        If (_line2 <> _line2Original) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} address 2", _addressType), _line2Original, _line2)
        End If
        If (_state <> _stateOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} address state", _addressType), _stateOriginal, _state)
        End If
        If (_zipCode <> _zipCodeOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} address zip code", _addressType), _zipCodeOriginal, _zipCode)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _cityOriginal = _city
        _countryOriginal = _country
        _isValidOriginal = _isValid
        _line1Original = _line1
        _line2Original = _line2
        _stateOriginal = _state
        _zipCodeOriginal = _zipCode
    End Sub
End Class
