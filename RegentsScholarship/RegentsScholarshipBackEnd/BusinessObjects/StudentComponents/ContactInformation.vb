Public Class ContactInformation
#Region "Properties"
    Private _cellPhone As Phone
    Public Property CellPhone() As Phone
        Get
            Return _cellPhone
        End Get
        Set(ByVal value As Phone)
            _cellPhone = value
        End Set
    End Property

    Private _homeAddress As MailingAddress
    Public Property HomeAddress() As MailingAddress
        Get
            Return _homeAddress
        End Get
        Set(ByVal value As MailingAddress)
            _homeAddress = value
        End Set
    End Property

    Private _parentStudent As Student
    Public Function ParentStudent() As Student
        Return _parentStudent
    End Function

    Private _personalEmail As Email
    Public Property PersonalEmail() As Email
        Get
            Return _personalEmail
        End Get
        Set(ByVal value As Email)
            _personalEmail = value
        End Set
    End Property

    Private _primaryPhone As Phone
    Public Property PrimaryPhone() As Phone
        Get
            Return _primaryPhone
        End Get
        Set(ByVal value As Phone)
            _primaryPhone = value
        End Set
    End Property

    Private _schoolEmail As Email
    Public Property SchoolEmail() As Email
        Get
            Return _schoolEmail
        End Get
        Set(ByVal value As Email)
            _schoolEmail = value
        End Set
    End Property
#End Region 'Properties

    Public Sub New(ByVal parentStudent As Student)
        _cellPhone = New Phone(Constants.PhoneType.CELL, Me)
        _homeAddress = New MailingAddress(Constants.AddressType.HOME, Me)
        _parentStudent = parentStudent
        _personalEmail = New Email(Constants.EmailType.PERSONAL, Me)
        _primaryPhone = New Phone(Constants.PhoneType.PRIMARY, Me)
        _schoolEmail = New Email(Constants.EmailType.SCHOOL, Me)
    End Sub

    Public Sub Commit(ByVal userId As String)
        'As with Validate(), we just need to call Commit() on our member objects.
        _cellPhone.Commit(userId)
        _homeAddress.Commit(userId)
        _personalEmail.Commit(userId)
        _primaryPhone.Commit(userId)
        _schoolEmail.Commit(userId)
    End Sub

    Public Shared Function Load(ByVal parentStudent As Student) As ContactInformation
        Dim storedInfo As New ContactInformation(parentStudent)
        storedInfo.CellPhone = Phone.Load(Constants.PhoneType.CELL, storedInfo)
        storedInfo.HomeAddress = MailingAddress.Load(Constants.AddressType.HOME, storedInfo)
        storedInfo.PersonalEmail = Email.Load(Constants.EmailType.PERSONAL, storedInfo)
        storedInfo.PrimaryPhone = Phone.Load(Constants.PhoneType.PRIMARY, storedInfo)
        storedInfo.SchoolEmail = Email.Load(Constants.EmailType.SCHOOL, storedInfo)
        Return storedInfo
    End Function

    Public Sub Validate()
        'Since this class is just a composition of other objects, we just need to call Validate() on them.
        _cellPhone.Validate()
        _homeAddress.Validate()
        _personalEmail.Validate()
        _primaryPhone.Validate()
        _schoolEmail.Validate()
    End Sub
End Class
