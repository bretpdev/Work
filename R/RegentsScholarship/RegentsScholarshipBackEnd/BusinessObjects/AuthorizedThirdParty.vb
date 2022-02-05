Public Class AuthorizedThirdParty
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _address1 As String
    Public Property Address1() As String
        Get
            Return _address1
        End Get
        Set(ByVal value As String)
            _address1 = value
        End Set
    End Property

    Private _address2 As String
    Public Property Address2() As String
        Get
            Return _address2
        End Get
        Set(ByVal value As String)
            _address2 = value
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

    Private _country As String
    Public Property Country() As String
        Get
            Return _country
        End Get
        Set(ByVal value As String)
            _country = value
        End Set
    End Property

    Private _id As Long
    Public Property Id() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
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

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _parentStudent As Student
    Public Function ParentStudent() As Student
        Return _parentStudent
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

    Private _zip As String
    Public Property Zip() As String
        Get
            Return _zip
        End Get
        Set(ByVal value As String)
            _zip = value
        End Set
    End Property
#End Region

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _address1Original As String
    Private _address2Original As String
    Private _cityOriginal As String
    Private _countryOriginal As String
    Private _isValidOriginal As Boolean
    Private _nameOriginal As String
    Private _stateOriginal As String
    Private _zipOriginal As String
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal parentStudent As Student)
        _objectIsNew = True
        _address1 = ""
        _address2 = ""
        _city = ""
        _country = ""
        _id = 0
        _isValid = True
        _name = ""
        _parentStudent = parentStudent
        _state = "UT"
        _zip = ""
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String, ByVal studentId As String)
        If (_objectIsNew) Then
            _id = DataAccess.SetAuthorizedThirdParty(Me)
            'Third parties are added by users rather than the downloader, so record a transaction for new instances.
            RecordTransactions(userId, studentId)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            _id = DataAccess.SetAuthorizedThirdParty(Me)
            RecordTransactions(userId, studentId)
        End If
    End Sub

    Public Shared Function Load(ByVal parentStudent As Student) As List(Of AuthorizedThirdParty)
        Dim storedParties As List(Of AuthorizedThirdParty) = DataAccess.GetAuthorizedThirdParties(parentStudent)
        For Each thirdParty As AuthorizedThirdParty In storedParties
            thirdParty._objectIsNew = False
            thirdParty._parentStudent = parentStudent
            thirdParty.SetChangeTrackingVariables()
        Next
        Return storedParties
    End Function

    Public Sub Validate()
        If (_address1.Length > 0 OrElse _city.Length > 0 OrElse _zip.Length > 0 OrElse _country.Length > 0) Then
            'if one is not empty then the name should also be provided
            If (String.IsNullOrEmpty(_name)) Then
                Dim message As String = "One or more of the Authorized Third Parties have incomplete data.  A name must be provided for each Authorized Third Party."
                Throw New RegentsInvalidDataException(message)
            End If
        End If
        'check zip validity
        If (_zip.Length > 0 AndAlso _zip.Length < 5) Then
            Dim message As String = "One or more of the Authorized Third Parties have an invalid Zip Code.  The provided Zip Code must be between five and nine characters."
            Throw New RegentsInvalidDataException(message)
        End If
    End Sub

    Private Function HasChanges() As Boolean
        If _address1 <> _address1Original Then Return True
        If _address2 <> _address2Original Then Return True
        If _city <> _cityOriginal Then Return True
        If _country <> _countryOriginal Then Return True
        If _isValid <> _isValidOriginal Then Return True
        If _name <> _nameOriginal Then Return True
        If _state.Trim() <> _stateOriginal.Trim() Then Return True
        If _zip <> _zipOriginal Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String, ByVal studentId As String)
        Dim thirdPartyId As String = String.Format("Third party (ID: {0})", _id)
        If _address1 <> _address1Original Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} address 1", thirdPartyId), _address1Original, _address1)
        End If
        If _address2 <> _address2Original Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} address 2", thirdPartyId), _address2Original, _address2)
        End If
        If _city <> _cityOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} city", thirdPartyId), _cityOriginal, _city)
        End If
        If _country <> _countryOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} country", thirdPartyId), _countryOriginal, _country)
        End If
        If _isValid <> _isValidOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} is valid", thirdPartyId), _isValidOriginal.ToString(), _isValid.ToString())
        End If
        If _name <> _nameOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} name", thirdPartyId), _nameOriginal, _name)
        End If
        If _state.Trim() <> _stateOriginal.Trim() Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} state", thirdPartyId), _stateOriginal.Trim(), _state.Trim())
        End If
        If _zip <> _zipOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} zip code", thirdPartyId), _zipOriginal, _zip)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _address1Original = _address1
        _address2Original = _address2
        _cityOriginal = _city
        _countryOriginal = _country
        _isValidOriginal = _isValid
        _nameOriginal = _name
        _stateOriginal = _state
        _zipOriginal = _zip
    End Sub
End Class
