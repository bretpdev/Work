'These lookup singletons are an attempt at improving performance by keeping any
'lookup values we use in memory rather than going to the database for them each time.
Public Class Lookups
    'Declare the constructor as protected so the class can't be instantiated.
    Protected Sub New()
    End Sub

    'For each lookup we want to implement, initialize a shared variable to null,
    'and provide a shared read-only property that returns the variable
    '(giving a value the first time it's used).
    Private Shared _accessLevels As List(Of String) = Nothing
    Public Shared ReadOnly Property AccessLevels() As List(Of String)
        Get
            If (_accessLevels Is Nothing) Then
                _accessLevels = DataAccess.GetAccessLevelLookups()
            End If
            Return _accessLevels
        End Get
    End Property

    Private Shared _actTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property ActTypes() As List(Of String)
        Get
            If (_actTypes Is Nothing) Then
                _actTypes = DataAccess.GetActTypeLookups()
            End If
            Return _actTypes
        End Get
    End Property

    Private Shared _addressTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property AddressTypes() As List(Of String)
        Get
            If (_addressTypes Is Nothing) Then
                _addressTypes = DataAccess.GetAddressTypeLookups()
            End If
            Return _addressTypes
        End Get
    End Property

    Private Shared _awardAmounts As List(Of AwardAmount) = Nothing
    Public Shared ReadOnly Property AwardAmounts() As List(Of AwardAmount)
        Get
            If (_awardAmounts Is Nothing) Then
                _awardAmounts = DataAccess.GetAwardAmountLookups()
            End If
            Return _awardAmounts
        End Get
    End Property

    Private Shared _awardStatuses As List(Of String) = Nothing
    Public Shared ReadOnly Property AwardStatuses() As List(Of String)
        Get
            If (_awardStatuses Is Nothing) Then
                _awardStatuses = DataAccess.GetAwardStatusLookups()
            End If
            Return _awardStatuses
        End Get
    End Property

    Private Shared _classStatuses As List(Of String) = Nothing
    Public Shared ReadOnly Property ClassStatuses() As List(Of String)
        Get
            If (_classStatuses Is Nothing) Then
                _classStatuses = DataAccess.GetClassStatusLookups()
            End If
            Return _classStatuses
        End Get
    End Property

    'Class titles can be added on the fly, so we can't have a set list. Provide a pass-through property.
    Public Shared ReadOnly Property ClassTitles() As List(Of ClassTitle)
        Get
            Return DataAccess.GetClassTitleLookups()
        End Get
    End Property

    Private Shared _classTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property ClassTypes() As List(Of String)
        Get
            If (_classTypes Is Nothing) Then
                _classTypes = DataAccess.GetClassTypeLookups()
            End If
            Return _classTypes
        End Get
    End Property

    Private Shared _classWeights As List(Of String) = Nothing
    Public Shared ReadOnly Property ClassWeights() As List(Of String)
        Get
            If (_classWeights Is Nothing) Then
                _classWeights = DataAccess.GetClassWeightLookups()
            End If
            Return _classWeights
        End Get
    End Property

    'Colleges can be added on the fly, so we can't have a set list. Provide a pass-through property.
    Public Shared ReadOnly Property Colleges() As List(Of College)
        Get
            Return DataAccess.GetCollegeLookups()
        End Get
    End Property

    Private Shared _collegeTerms As List(Of String) = Nothing
    Public Shared ReadOnly Property CollegeTerms() As List(Of String)
        Get
            If (_collegeTerms Is Nothing) Then
                _collegeTerms = DataAccess.GetCollegeTermLookups()
            End If
            Return _collegeTerms
        End Get
    End Property

    Private Shared _communicationSources As List(Of String) = Nothing
    Public Shared ReadOnly Property CommunicationSources() As List(Of String)
        Get
            If (_communicationSources Is Nothing) Then
                _communicationSources = DataAccess.GetCommunicationSourceLookups()
            End If
            Return _communicationSources
        End Get
    End Property

    Private Shared _communicationTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property CommunicationTypes() As List(Of String)
        Get
            If (_communicationTypes Is Nothing) Then
                _communicationTypes = DataAccess.GetCommunicationTypeLookups()
            End If
            Return _communicationTypes
        End Get
    End Property

    Private Shared _denialReasons As List(Of String) = Nothing
    Public Shared ReadOnly Property DenialReasons() As List(Of String)
        Get
            If (_denialReasons Is Nothing) Then
                _denialReasons = DataAccess.GetDenialReasonLookups()
            End If
            Return _denialReasons
        End Get
    End Property

    Private Shared _districts As List(Of District) = Nothing
    Public Shared ReadOnly Property Districts() As List(Of District)
        Get
            If (_districts Is Nothing) Then
                _districts = DataAccess.GetDistrictLookups()
            End If
            Return _districts
        End Get
    End Property

    Private Shared _documentTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property DocumentTypes() As List(Of String)
        Get
            If (_documentTypes Is Nothing) Then
                _documentTypes = DataAccess.GetDocumentTypeLookups()
            End If
            Return _documentTypes
        End Get
    End Property

    Private Shared _emailTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property EmailTypes() As List(Of String)
        Get
            If (_emailTypes Is Nothing) Then
                _emailTypes = DataAccess.GetEmailTypeLookups()
            End If
            Return _emailTypes
        End Get
    End Property

    'Ethnicities can be added on the fly, so we can't have a set list. Provide a pass-through property.
    Public Shared ReadOnly Property Ethnicities() As List(Of Ethnicity)
        Get
            Return DataAccess.GetEthnicityLookups()
        End Get
    End Property

    Private Shared _genders As List(Of String) = Nothing
    Public Shared ReadOnly Property Genders() As List(Of String)
        Get
            If (_genders Is Nothing) Then
                _genders = DataAccess.GetGenderLookups()
            End If
            Return _genders
        End Get
    End Property

    Private Shared _grades As List(Of Grade) = Nothing
    Public Shared ReadOnly Property Grades() As List(Of Grade)
        Get
            If (_grades Is Nothing) Then
                _grades = DataAccess.GetGradeLookups()
            End If
            Return _grades
        End Get
    End Property

    Private Shared _hearAboutRegentsMethods As List(Of String) = Nothing
    Public Shared ReadOnly Property HearAboutRegentsMethods() As List(Of String)
        Get
            If (_hearAboutRegentsMethods Is Nothing) Then
                _hearAboutRegentsMethods = DataAccess.GetHearAboutSourceLookups()
            End If
            Return _hearAboutRegentsMethods
        End Get
    End Property

    Private Shared _leaveDeferralReasons As List(Of String) = Nothing
    Public Shared ReadOnly Property LeaveDeferralReasons() As List(Of String)
        Get
            If (_leaveDeferralReasons Is Nothing) Then
                _leaveDeferralReasons = DataAccess.GetLeaveDeferralReasonLookups()
            End If
            Return _leaveDeferralReasons
        End Get
    End Property

    Private Shared _leaveDeferralTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property LeaveDeferralTypes() As List(Of String)
        Get
            If (_leaveDeferralTypes Is Nothing) Then
                _leaveDeferralTypes = DataAccess.GetLeaveDeferralTypeLookups()
            End If
            Return _leaveDeferralTypes
        End Get
    End Property

    Private Shared _paymentTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property PaymentTypes() As List(Of String)
        Get
            If (_paymentTypes Is Nothing) Then
                _paymentTypes = DataAccess.GetPaymentTypeLookups()
            End If
            Return _paymentTypes
        End Get
    End Property

    Private Shared _phoneTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property PhoneTypes() As List(Of String)
        Get
            If (_phoneTypes Is Nothing) Then
                _phoneTypes = DataAccess.GetPhoneTypeLookups()
            End If
            Return _phoneTypes
        End Get
    End Property

    Private Shared _programYears As List(Of ProgramOperationYear) = Nothing
    Public Shared ReadOnly Property ProgramYears() As List(Of ProgramOperationYear)
        Get
            If (_programYears Is Nothing) Then
                _programYears = DataAccess.GetProgramOperationYearLookups()
            End If
            Return _programYears
        End Get
    End Property

    Private Shared _reviewTypes As List(Of String) = Nothing
    Public Shared ReadOnly Property ReviewTypes() As List(Of String)
        Get
            If (_reviewTypes Is Nothing) Then
                _reviewTypes = DataAccess.GetReviewTypeLookups()
            End If
            Return _reviewTypes
        End Get
    End Property

    Private Shared _schools As List(Of School) = Nothing
    Public Shared ReadOnly Property Schools() As List(Of School)
        Get
            If (_schools Is Nothing) Then
                _schools = DataAccess.GetSchoolLookups()
            End If
            Return _schools
        End Get
    End Property

    Private Shared _states As List(Of State) = Nothing
    Public Shared ReadOnly Property States() As List(Of State)
        Get
            If (_states Is Nothing) Then
                _states = DataAccess.GetStateLookups()
            End If
            Return _states
        End Get
    End Property

    Private Shared _usbctStatuses As List(Of String) = Nothing
    Public Shared ReadOnly Property UsbctStatuses() As List(Of String)
        Get
            If (_usbctStatuses Is Nothing) Then
                _usbctStatuses = DataAccess.GetUsbctStatusLookups()
            End If
            Return _usbctStatuses
        End Get
    End Property

#Region "Projection classes"
    Public Class AwardAmount
        Private _description As String
        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Private _amount As Double
        Public Property Amount() As Double
            Get
                Return _amount
            End Get
            Set(ByVal value As Double)
                _amount = value
            End Set
        End Property

        Private _awardYear As String
        Public Property AwardYear() As String
            Get
                Return _awardYear
            End Get
            Set(ByVal value As String)
                _awardYear = value
            End Set
        End Property
    End Class

    Public Class ClassTitle
        Private _conditionalSchoolApprovalYears As String
        Public Property ConditionalSchoolApprovalYears() As String
            Get
                Return _conditionalSchoolApprovalYears
            End Get
            Set(ByVal value As String)
                _conditionalSchoolApprovalYears = value
            End Set
        End Property

        Private _conditionalSchoolCode As String
        Public Property ConditionalSchoolCode() As String
            Get
                Return _conditionalSchoolCode
            End Get
            Set(ByVal value As String)
                _conditionalSchoolCode = value
            End Set
        End Property

        Private _isInApprovedList As Boolean
        Public Property IsInApprovedList() As Boolean
            Get
                Return _isInApprovedList
            End Get
            Set(ByVal value As Boolean)
                _isInApprovedList = value
            End Set
        End Property

        Private _title As String
        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Private _type As String
        Public Property Type() As String
            Get
                Return _type
            End Get
            Set(ByVal value As String)
                _type = value
            End Set
        End Property

        Private _weight As String
        Public Property Weight() As String
            Get
                Return _weight
            End Get
            Set(ByVal value As String)
                _weight = value
            End Set
        End Property
    End Class

    Public Class College
        Private _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _isInDefaultList As Boolean
        Public Property IsInDefaultList() As Boolean
            Get
                Return _isInDefaultList
            End Get
            Set(ByVal value As Boolean)
                _isInDefaultList = value
            End Set
        End Property
    End Class

    Public Class District
        Private _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _communicationDirectoryId As Integer
        Public Property CommunicationDirectoryID() As Integer
            Get
                Return _communicationDirectoryId
            End Get
            Set(ByVal value As Integer)
                _communicationDirectoryId = value
            End Set
        End Property
    End Class

    Public Class Ethnicity
        Private _description As String
        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Private _isInDefaultList As Boolean
        Public Property IsInDefaultList() As Boolean
            Get
                Return _isInDefaultList
            End Get
            Set(ByVal value As Boolean)
                _isInDefaultList = value
            End Set
        End Property
    End Class

    Public Class Grade
        Private _letter As String
        Public Property Letter() As String
            Get
                Return _letter
            End Get
            Set(ByVal value As String)
                _letter = value
            End Set
        End Property

        Private _gpaValue As Nullable(Of Double)
        Public Property GpaValue() As Nullable(Of Double)
            Get
                Return _gpaValue
            End Get
            Set(ByVal value As Nullable(Of Double))
                _gpaValue = value
            End Set
        End Property
    End Class

    Public Class ProgramOperationYear
        Private _year As String
        Public Property Year() As String
            Get
                Return _year
            End Get
            Set(ByVal value As String)
                _year = value
            End Set
        End Property

        Private _applicationDeadline As DateTime
        Public Property ApplicationDeadline() As DateTime
            Get
                Return _applicationDeadline
            End Get
            Set(ByVal value As DateTime)
                _applicationDeadline = value
            End Set
        End Property

        Private _priorityDeadline As DateTime
        Public Property PriorityDeadline() As DateTime
            Get
                Return _priorityDeadline
            End Get
            Set(ByVal value As DateTime)
                _priorityDeadline = value
            End Set
        End Property

        Private _finalDeadline As DateTime
        Public Property FinalDeadline() As DateTime
            Get
                Return _finalDeadline
            End Get
            Set(ByVal value As DateTime)
                _finalDeadline = value
            End Set
        End Property
    End Class

    Public Class School
        Private _ceebCode As String
        Public Property CeebCode() As String
            Get
                Return _ceebCode
            End Get
            Set(ByVal value As String)
                _ceebCode = value
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

        Private _district As String
        Public Property District() As String
            Get
                Return _district
            End Get
            Set(ByVal value As String)
                _district = value
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

        Private _stateAbbreviation As String
        Public Property StateAbbreviation() As String
            Get
                Return _stateAbbreviation
            End Get
            Set(ByVal value As String)
                _stateAbbreviation = value
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

        Private _type As String
        Public Property Type() As String
            Get
                Return _type
            End Get
            Set(ByVal value As String)
                _type = value
            End Set
        End Property
    End Class

    Public Class State
        Private _abbreviation As String
        Public Property Abbreviation() As String
            Get
                Return _abbreviation
            End Get
            Set(ByVal value As String)
                _abbreviation = value
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
    End Class
#End Region 'Projection classes
End Class
