Public Class Phone
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _isValid As Boolean
    Public Property IsValid() As Boolean
        Get
            Return _isValid
        End Get
        Set(ByVal value As Boolean)
            _isValid = value
        End Set
    End Property

    Private _number As String
    Public Property Number() As String
        Get
            Return _number
        End Get
        Set(ByVal value As String)
            _number = value
        End Set
    End Property

    Private _parentContactInfo As ContactInformation
    Public Function ParentContactInfo() As ContactInformation
        Return _parentContactInfo
    End Function

    Private _phoneType As String
    Public Function PhoneType() As String
        Return _phoneType
    End Function
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _isValidOriginal As Boolean
    Private _numberOriginal As String
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal phoneType As String, ByVal parentContactInfo As ContactInformation)
        _objectIsNew = True
        _isValid = False
        _number = ""
        _parentContactInfo = parentContactInfo
        _phoneType = phoneType
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetPhoneNumber(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            DataAccess.SetPhoneNumber(Me)
            RecordTransactions(userId)
        End If
    End Sub

    Public Shared Function Load(ByVal phoneType As String, ByVal parentContactInfo As ContactInformation) As Phone
        Dim storedPhone As Phone = DataAccess.GetPhoneNumber(phoneType, parentContactInfo)
        'DataAccess may return a null object, which we can't have. Create a new one if needed.
        If (storedPhone Is Nothing) Then storedPhone = New Phone(phoneType, parentContactInfo) With {.Number = "", .IsValid = False}
        storedPhone._objectIsNew = False
        storedPhone._phoneType = phoneType
        storedPhone._parentContactInfo = parentContactInfo
        storedPhone.SetChangeTrackingVariables()
        Return storedPhone
    End Function

    Public Sub Validate()
        'Nothing to check for this class.
    End Sub

    Private Function HasChanges() As Boolean
        If (_isValid <> _isValidOriginal) Then Return True
        If (_number <> _numberOriginal) Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentContactInfo.ParentStudent.StateStudentId
        If (_isValid <> _isValidOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} phone is valid", _phoneType), _isValidOriginal.ToString(), _isValid.ToString())
        End If
        If (_number <> _numberOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} phone number", _phoneType), _numberOriginal, _number)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _isValidOriginal = _isValid
        _numberOriginal = _number
    End Sub
End Class
