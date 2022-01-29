Public Class Student
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _alternateLastName As String
    Public Property AlternateLastName() As String
        Get
            Return _alternateLastName
        End Get
        Set(ByVal value As String)
            _alternateLastName = If(value IsNot Nothing, value, "")
        End Set
    End Property

    Private _authorizedThirdParties As List(Of AuthorizedThirdParty)
    Public Property AuthorizedThirdParties() As List(Of AuthorizedThirdParty)
        Get
            Return _authorizedThirdParties
        End Get
        Set(ByVal value As List(Of AuthorizedThirdParty))
            _authorizedThirdParties = value
        End Set
    End Property

    Private _college As College
    Public Property College() As College
        Get
            Return _college
        End Get
        Set(ByVal value As College)
            _college = value
        End Set
    End Property

    Private _contactInfo As ContactInformation
    Public Property ContactInfo() As ContactInformation
        Get
            Return _contactInfo
        End Get
        Set(ByVal value As ContactInformation)
            _contactInfo = value
        End Set
    End Property

    Public Function CumulativeCreditHoursPaid() As Double
        Dim approvedPayments As IEnumerable(Of Payment) = _payments.Where(Function(p) p.Type = Constants.PaymentType.EXEMPLARY AndAlso p.Status = Constants.PaymentStatus.APPROVED)
        If (approvedPayments.Count = 0) Then
            Return 0
        Else
            Return approvedPayments.Select(Function(p) p.Credits).Sum()
        End If
    End Function

    Private _dateOfBirth As DateTime
    Public Property DateOfBirth() As DateTime
        Get
            Return _dateOfBirth
        End Get
        Set(ByVal value As DateTime)
            _dateOfBirth = value
        End Set
    End Property

    Private _ethnicity As String
    Public Property Ethnicity() As String
        Get
            Return _ethnicity
        End Get
        Set(ByVal value As String)
            _ethnicity = value
        End Set
    End Property

    Private _firstName As String
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Private _gender As String
    Public Property Gender() As String
        Get
            Return _gender
        End Get
        Set(ByVal value As String)
            _gender = value
        End Set
    End Property

    Private _hasCriminalRecord As Boolean
    Public Property HasCriminalRecord() As Boolean
        Get
            Return _hasCriminalRecord
        End Get
        Set(ByVal value As Boolean)
            _hasCriminalRecord = value
        End Set
    End Property

    Private _hasUespAccount As Boolean
    Public Property HasUespAccount() As Boolean
        Get
            Return _hasUespAccount
        End Get
        Set(ByVal value As Boolean)
            _hasUespAccount = value
        End Set
    End Property

    Private _highSchool As HighSchool
    Public Property HighSchool() As HighSchool
        Get
            Return _highSchool
        End Get
        Set(ByVal value As HighSchool)
            _highSchool = value
        End Set
    End Property

    Private _intendsToApplyForFederalAid As Boolean
    Public Property IntendsToApplyForFederalAid() As Boolean
        Get
            Return _intendsToApplyForFederalAid
        End Get
        Set(ByVal value As Boolean)
            _intendsToApplyForFederalAid = value
        End Set
    End Property

    Private _isEligibleForFederalAid As Boolean
    Public Property IsEligibleForFederalAid() As Boolean
        Get
            Return _isEligibleForFederalAid
        End Get
        Set(ByVal value As Boolean)
            _isEligibleForFederalAid = value
        End Set
    End Property

    Private _isUsCitizen As Boolean
    Public Property IsUsCitizen() As Boolean
        Get
            Return _isUsCitizen
        End Get
        Set(ByVal value As Boolean)
            _isUsCitizen = value
        End Set
    End Property

    Private _lastName As String
    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _middleName As String
    Public Property MiddleName() As String
        Get
            Return _middleName
        End Get
        Set(ByVal value As String)
            _middleName = If(value IsNot Nothing, value, "")
        End Set
    End Property

    Private _payments As List(Of Payment)
    Public Property Payments() As List(Of Payment)
        Get
            Return _payments
        End Get
        Set(ByVal value As List(Of Payment))
            _payments = value
        End Set
    End Property

    Private _scholarshipApplication As ScholarshipApplication
    Public Property ScholarshipApplication() As ScholarshipApplication
        Get
            Return _scholarshipApplication
        End Get
        Set(ByVal value As ScholarshipApplication)
            _scholarshipApplication = value
        End Set
    End Property

    Private _socialSecurityNumber As String
    Public Property SocialSecurityNumber() As String
        Get
            Return _socialSecurityNumber
        End Get
        Set(ByVal value As String)
            'Strip out dashes if there are any.
            _socialSecurityNumber = If(value IsNot Nothing, value.Replace("-", ""), "")
        End Set
    End Property

    Private _stateStudentId As String
    Public Function StateStudentId() As String
        Return _stateStudentId
    End Function
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _alternateLastNameOriginal As String
    Private _authorizedThirdPartiesOriginal As List(Of AuthorizedThirdParty)
    Private _dateOfBirthOriginal As DateTime
    Private _ethnicityOriginal As String
    Private _firstNameOriginal As String
    Private _genderOriginal As String
    Private _hasCriminalRecordOriginal As Boolean
    Private _hasUespAccountOriginal As Boolean
    Private _intendsToApplyForFederalAidOriginal As Boolean
    Private _isEligibleForFederalAidOriginal As Boolean
    Private _isUsCitizenOriginal As Boolean
    Private _lastNameOriginal As String
    Private _middleNameOriginal As String
    Private _paymentsOriginal As List(Of Payment)
    Private _socialSecurityNumberOriginal As String
#End Region 'Change tracking variables

    ''' <summary>
    ''' Power-of-two enum that delineates major components of a student's record,
    ''' which may be treated separately for certain operations (such as saving a student's data).
    ''' </summary>
    Public Enum Component
        Demographics = 1
        Application = 2
        Payments = 4
    End Enum

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal stateStudentId As String)
        _objectIsNew = True
        _alternateLastName = ""
        _authorizedThirdParties = New List(Of AuthorizedThirdParty)()
        _authorizedThirdParties.Add(New AuthorizedThirdParty(Me))
        _authorizedThirdParties.Add(New AuthorizedThirdParty(Me))
        _college = New College(Me)
        _contactInfo = New ContactInformation(Me)
        _dateOfBirth = Nothing
        _ethnicity = ""
        _firstName = ""
        _gender = ""
        _hasCriminalRecord = False
        _highSchool = New HighSchool(Me)
        _intendsToApplyForFederalAid = False
        _isEligibleForFederalAid = False
        _isUsCitizen = False
        _lastName = ""
        _middleName = ""
        _payments = New List(Of Payment)()
        _scholarshipApplication = New ScholarshipApplication(Me)
        _socialSecurityNumber = ""
        _stateStudentId = stateStudentId
        SetChangeTrackingVariables()
    End Sub

    Public Sub ChangeStateStudentId(ByVal newStudentId As String, ByVal userId As String)
        Try
            DataAccess.ChangeStudentId(userId, _stateStudentId, newStudentId)
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "State student ID", _stateStudentId, newStudentId)
            _stateStudentId = newStudentId
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Commit(ByVal userId As String, ByVal components As Component)
        Validate(components)

        Try
            DataAccess.BeginTransaction()

            If (components And Component.Demographics) Then
                If (_objectIsNew) Then
                    DataAccess.SetStudent(Me)
                    SetChangeTrackingVariables()
                    _objectIsNew = False
                ElseIf (HasChanges()) Then
                    DataAccess.SetStudent(Me)
                    RecordTransactions(userId)
                End If
                _contactInfo.Commit(userId)
                For Each ATP As AuthorizedThirdParty In _authorizedThirdParties
                    ATP.Commit(userId, _stateStudentId)
                Next ATP
            End If

            If (components And Component.Application) Then
                _scholarshipApplication.Commit(userId)
                _college.Commit(userId)
                _highSchool.Commit(userId)
            End If

            If (components And Component.Payments) Then
                DataAccess.DeletePayments(Me)
                For Each pt As Payment In _payments
                    pt.Commit(userId)
                Next pt
            End If

            DataAccess.CommitTransaction()
        Catch ex As Exception
            DataAccess.RollbackTransaction()
            Throw
        End Try
    End Sub

    Public Shared Function Load(ByVal stateStudentId As String) As Student
        Dim storedStudent As Student = DataAccess.GetStudent(stateStudentId)
        storedStudent._objectIsNew = False
        storedStudent._stateStudentId = stateStudentId
        storedStudent.College = College.Load(storedStudent)
        storedStudent.ContactInfo = ContactInformation.Load(storedStudent)
        storedStudent.HighSchool = HighSchool.Load(storedStudent)
        storedStudent.ScholarshipApplication = ScholarshipApplication.Load(storedStudent)
        storedStudent.Payments = Payment.Load(storedStudent)
        storedStudent.AuthorizedThirdParties = AuthorizedThirdParty.Load(storedStudent)
        Do While (storedStudent.AuthorizedThirdParties.Count < 2)
            storedStudent.AuthorizedThirdParties.Add(New AuthorizedThirdParty(storedStudent))
        Loop
        storedStudent.SetChangeTrackingVariables()
        Return storedStudent
    End Function

    Private Function HasChanges() As Boolean
        If _alternateLastName <> _alternateLastNameOriginal Then Return True
        If _authorizedThirdParties.Count <> _authorizedThirdPartiesOriginal.Count Then Return True
        If (_dateOfBirth.Date <> _dateOfBirthOriginal.Date) Then Return True
        If _ethnicity <> _ethnicityOriginal Then Return True
        If _firstName <> _firstNameOriginal Then Return True
        If _gender <> _genderOriginal Then Return True
        If _hasCriminalRecord <> _hasCriminalRecordOriginal Then Return True
        If _hasUespAccount <> _hasUespAccountOriginal Then Return True
        If _intendsToApplyForFederalAid <> _intendsToApplyForFederalAidOriginal Then Return True
        If _isEligibleForFederalAid <> _isEligibleForFederalAidOriginal Then Return True
        If _isUsCitizen <> _isUsCitizenOriginal Then Return True
        If _lastName <> _lastNameOriginal Then Return True
        If _middleName <> _middleNameOriginal Then Return True
        For Each newPayment As Payment In _payments
            Dim fooPayment As Payment = newPayment
            If (_paymentsOriginal.Where(Function(p) p.College = fooPayment.College AndAlso p.Year = fooPayment.Year AndAlso p.Semester = fooPayment.Semester).Count() = 0) Then Return True
        Next newPayment
        For Each oldPayment As Payment In _paymentsOriginal
            Dim fooPayment As Payment = oldPayment
            If (_payments.Where(Function(p) p.College = fooPayment.College AndAlso p.Year = fooPayment.Year AndAlso p.Semester = fooPayment.Semester).Count() = 0) Then Return True
        Next oldPayment
        If _socialSecurityNumber <> _socialSecurityNumberOriginal Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        If _alternateLastName <> _alternateLastNameOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Alternate last name", _alternateLastNameOriginal, _alternateLastName)
        End If
        Dim numberOfParties As Integer = _authorizedThirdPartiesOriginal.Count
        For Each newParty As AuthorizedThirdParty In _authorizedThirdParties
            If (Not _authorizedThirdPartiesOriginal.Contains(newParty)) Then
                numberOfParties += 1
                DataAccess.AddTransactionRecord(userId, _stateStudentId, "Authorized third party " + numberOfParties.ToString(), "", newParty.Name)
            End If
        Next newParty
        For Each oldParty As AuthorizedThirdParty In _authorizedThirdPartiesOriginal
            If (Not _authorizedThirdParties.Contains(oldParty)) Then
                DataAccess.AddTransactionRecord(userId, _stateStudentId, "Authorized third party " + _authorizedThirdPartiesOriginal.IndexOf(oldParty).ToString(), oldParty.Name, "")
            End If
        Next oldParty
        If (_dateOfBirth.Date <> _dateOfBirthOriginal.Date) Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Date of birth", _dateOfBirthOriginal.ToShortDateString(), _dateOfBirth.ToShortDateString())
        End If
        If _ethnicity <> _ethnicityOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Ethnicity", _ethnicityOriginal, _ethnicity)
        End If
        If _firstName <> _firstNameOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "First name", _firstNameOriginal, _firstName)
        End If
        If _gender <> _genderOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Gender", _genderOriginal, _gender)
        End If
        If _hasCriminalRecord <> _hasCriminalRecordOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Has criminal record", _hasCriminalRecordOriginal, _hasCriminalRecord)
        End If
        If _hasUespAccount <> _hasUespAccountOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Has UESP account", _hasUespAccountOriginal, _hasUespAccount)
        End If
        If _intendsToApplyForFederalAid <> _intendsToApplyForFederalAidOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Intends to apply for federal aid", _intendsToApplyForFederalAidOriginal, _intendsToApplyForFederalAid)
        End If
        If _isEligibleForFederalAid <> _isEligibleForFederalAidOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Is eligible for federal aid", _isEligibleForFederalAidOriginal, _isEligibleForFederalAid)
        End If
        If _isUsCitizen <> _isUsCitizenOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Is US citizen", _isUsCitizenOriginal, _isUsCitizen)
        End If
        If _lastName <> _lastNameOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Last name", _lastNameOriginal, _lastName)
        End If
        If _middleName <> _middleNameOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Middle name", _middleNameOriginal, _middleName)
        End If
        For Each newPayment As Payment In _payments
            Dim fooPayment As Payment = newPayment
            If (_paymentsOriginal.Where(Function(p) p.College = fooPayment.College AndAlso p.Year = fooPayment.Year AndAlso p.Semester = fooPayment.Semester).Count() = 0) Then
                Dim propertyText As String = String.Format("Added payment to {0} for {1} {2}", fooPayment.College, fooPayment.Semester, fooPayment.Year)
                DataAccess.AddTransactionRecord(userId, _stateStudentId, propertyText, "0", fooPayment.Amount.ToString("0.00"))
            End If
        Next newPayment
        For Each oldPayment As Payment In _paymentsOriginal
            Dim fooPayment As Payment = oldPayment
            If (_payments.Where(Function(p) p.College = fooPayment.College AndAlso p.Year = fooPayment.Year AndAlso p.Semester = fooPayment.Semester).Count() = 0) Then
                Dim propertyText As String = String.Format("Deleted payment to {0} for {1} {2}", fooPayment.College, fooPayment.Semester, fooPayment.Year)
                DataAccess.AddTransactionRecord(userId, _stateStudentId, propertyText, fooPayment.Amount.ToString("0.00"), "0")
            End If
        Next oldPayment
        If _socialSecurityNumber <> _socialSecurityNumberOriginal Then
            DataAccess.AddTransactionRecord(userId, _stateStudentId, "Social security number", _socialSecurityNumberOriginal, _socialSecurityNumber)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _alternateLastNameOriginal = _alternateLastName
        _authorizedThirdPartiesOriginal = New List(Of AuthorizedThirdParty)(_authorizedThirdParties)
        _dateOfBirthOriginal = _dateOfBirth
        _ethnicityOriginal = _ethnicity
        _firstNameOriginal = _firstName
        _genderOriginal = _gender
        _hasCriminalRecordOriginal = _hasCriminalRecord
        _hasUespAccountOriginal = _hasUespAccount
        _intendsToApplyForFederalAidOriginal = _intendsToApplyForFederalAid
        _isEligibleForFederalAidOriginal = _isEligibleForFederalAid
        _isUsCitizenOriginal = _isUsCitizen
        _lastNameOriginal = _lastName
        _middleNameOriginal = _middleName
        _paymentsOriginal = New List(Of Payment)(_payments)
        _socialSecurityNumberOriginal = _socialSecurityNumber
    End Sub

    Private Sub Validate(ByVal components As Component)
        If (components And Component.Demographics) Then
            'Check that gender is a legitimate value.
            If (Not String.IsNullOrEmpty(_gender)) Then
                If Not Lookups.Genders.Contains(_gender) Then
                    Dim message As String = String.Format("{0} is not a valid gender.", _gender)
                    Throw New RegentsInvalidDataException(message)
                End If
            End If

            'Check that DOB is at least 10 years in the past.
            If _dateOfBirth > Now.AddYears(-10) Then
                Dim message As String = String.Format("Date of birth ({0}) is not 10 years in the past or more.", _dateOfBirth.ToString("MM/dd/yyyy"))
                Throw New RegentsInvalidDataException(message)
            End If

            For Each thirdParty As AuthorizedThirdParty In _authorizedThirdParties
                thirdParty.Validate()
            Next thirdParty
        End If

        If (components And Component.Application) Then
            'Check if graduation date can be future dated or must be current or in the past.
            If ScholarshipApplication.Reviews.ContainsKey(Constants.ReviewType.FINAL_TRANSCRIPT) Then
                If Validator.DateIsCurrentOrInPast(HighSchool.GraduationDate) = False Then
                    Dim message As String = "Graduation Date can't be in the future if the Final Transcript Review has taken place.  Please resolve the issue and try again."
                    Throw New RegentsInvalidDataException(message)
                End If
            End If

            'Check that the application year exists in the ProgramOperationYears table.
            If Not Lookups.ProgramYears.Select(Function(p) p.Year).Contains(_scholarshipApplication.ApplicationYear) Then
                Dim message As String = String.Format("{0} is not a recognized application year.", _scholarshipApplication.ApplicationYear)
                Throw New RegentsInvalidDataException(message)
            End If

            _college.Validate()
            _contactInfo.Validate()
            _highSchool.Validate()
            _scholarshipApplication.Validate()
        End If

        If (components And Component.Payments) Then
            For Each currentPayment As Payment In _payments
                currentPayment.Validate()
            Next currentPayment
        End If
    End Sub
End Class
