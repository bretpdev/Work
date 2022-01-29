Public Class Email
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _address As String
    Public Property Address() As String
        Get
            Return _address
        End Get
        Set(ByVal value As String)
            _address = value
        End Set
    End Property

    Private _emailType As String
    Public Function EmailType() As String
        Return _emailType
    End Function

    Private _isValid As Boolean
    Public Property IsValid() As Boolean
        Get
            Return _isValid
        End Get
        Set(ByVal value As Boolean)
            _isValid = value
        End Set
    End Property

    Private _parentContactInfo As ContactInformation
    Public Function ParentContactInfo() As ContactInformation
        Return _parentContactInfo
    End Function
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _addressOriginal As String
    Private _isValidOriginal As Boolean
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal emailType As String, ByVal parentContactInfo As ContactInformation)
        _objectIsNew = True
        _address = ""
        _emailType = emailType
        _isValid = False
        _parentContactInfo = parentContactInfo
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetEmailAddress(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            DataAccess.SetEmailAddress(Me)
            RecordTransactions(userId)
        End If
    End Sub

    Public Shared Function Load(ByVal emailType As String, ByVal parentContactInfo As ContactInformation) As Email
        Dim storedEmail As Email = DataAccess.GetEmailAddress(emailType, parentContactInfo)
        'DataAccess may return a null object, which we can't have. Create a new one if needed.
        If (storedEmail Is Nothing) Then storedEmail = New Email(emailType, parentContactInfo) With {.Address = "", .IsValid = False}
        storedEmail._objectIsNew = False
        storedEmail._emailType = emailType
        storedEmail._parentContactInfo = parentContactInfo
        storedEmail.SetChangeTrackingVariables()
        Return storedEmail
    End Function

    Public Sub Validate()
        'Nothing to check for this class.
    End Sub

    Private Function HasChanges() As Boolean
        If (_isValid <> _isValidOriginal) Then Return True
        If (_address <> _addressOriginal) Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentContactInfo.ParentStudent.StateStudentId
        If (_isValid <> _isValidOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} e-mail is valid", _emailType), _isValidOriginal.ToString(), _isValid.ToString())
        End If
        If (_address <> _addressOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} e-mail address", _emailType), _addressOriginal, _address)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _isValidOriginal = _isValid
        _addressOriginal = _address
    End Sub
End Class
