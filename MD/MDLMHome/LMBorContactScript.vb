Public Class LMBorContactScript

    Private _SSN As String
    Public Property SSN() As String
        Get
            Return _SSN
        End Get
        Set(ByVal value As String)
            _SSN = value
        End Set
    End Property

    Private _ScriptName As String
    Public Property ScriptName() As String
        Get
            Return _ScriptName
        End Get
        Set(ByVal value As String)
            _ScriptName = value
        End Set
    End Property

    Private _Amt As String
    Public Property Amt() As String
        Get
            Return _Amt
        End Get
        Set(ByVal value As String)
            _Amt = value
        End Set
    End Property

    Private _IneligibleReason As String
    Public Property IneligibleReason() As String
        Get
            Return _IneligibleReason
        End Get
        Set(ByVal value As String)
            _IneligibleReason = value
        End Set
    End Property

    Private _drvLic As String
    Public Property DrvLic() As String
        Get
            Return _drvLic
        End Get
        Set(ByVal value As String)
            _drvLic = value
        End Set
    End Property

    Private _lendPayOffDate As String
    Public Property LendPayOffDate() As String
        Get
            Return _lendPayOffDate
        End Get
        Set(ByVal value As String)
            _lendPayOffDate = value
        End Set
    End Property

    Private _taxOffsetID As String
    Public Property TaxOffsetID() As String
        Get
            Return _taxOffsetID
        End Get
        Set(ByVal value As String)
            _taxOffsetID = value
        End Set
    End Property

    Private _nextPayDueDate As String
    Public Property NextDueDate() As String
        Get
            Return _nextPayDueDate
        End Get
        Set(ByVal value As String)
            _nextPayDueDate = value
        End Set
    End Property

    Private _expectPayAmt As String
    Public Property ExpectPayAmt() As String
        Get
            Return _expectPayAmt
        End Get
        Set(ByVal value As String)
            _expectPayAmt = value
        End Set
    End Property

    Private _payDueDate As String
    Public Property PayDueDate() As String
        Get
            Return _payDueDate
        End Get
        Set(ByVal value As String)
            _payDueDate = value
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

    Private _firstName As String
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
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

    Private _street As String
    Public Property Street() As String
        Get
            Return _street
        End Get
        Set(ByVal value As String)
            _street = value
        End Set
    End Property

    Private _street2 As String
    Public Property Street2() As String
        Get
            Return _street2
        End Get
        Set(ByVal value As String)
            _street2 = value
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

    Private _claimReason As String
    Public Property ClaimReason() As String
        Get
            Return _claimReason
        End Get
        Set(ByVal value As String)
            _claimReason = value
        End Set
    End Property

    Private _rehabCounter As String
    Public Property RehabCounter() As String
        Get
            Return _rehabCounter
        End Get
        Set(ByVal value As String)
            _rehabCounter = value
        End Set
    End Property

    Private _rehabActionCode As String
    Public Property RehabActionCode() As String
        Get
            Return _rehabActionCode
        End Get
        Set(ByVal value As String)
            _rehabActionCode = value
        End Set
    End Property

    Private _LC67 As String
    Public Property LC67() As String
        Get
            Return _LC67
        End Get
        Set(ByVal value As String)
            _LC67 = value
        End Set
    End Property

    Private _outstandingBalance As String
    Public Property OutstandingBalance() As String
        Get
            Return _outstandingBalance
        End Get
        Set(ByVal value As String)
            _outstandingBalance = value
        End Set
    End Property

    Private _ptpAmt As String
    Public Property PtpAmt() As String
        Get
            Return _ptpAmt
        End Get
        Set(ByVal value As String)
            _ptpAmt = value
        End Set
    End Property

    Private _ptpDate As String
    Public Property PtpDate() As String
        Get
            Return _ptpDate
        End Get
        Set(ByVal value As String)
            _ptpDate = value
        End Set
    End Property

    Private _madeChanges As Boolean
    Public Property MadeChanges() As Boolean
        Get
            Return _madeChanges
        End Get
        Set(ByVal value As Boolean)
            _madeChanges = value
        End Set
    End Property

    Private _refChanged As String
    Public Property RefChanged() As String
        Get
            Return _refChanged
        End Get
        Set(ByVal value As String)
            _refChanged = value
        End Set
    End Property

    Private _ref1addy1 As String
    Public Property Ref1Addy1() As String
        Get
            Return _ref1addy1
        End Get
        Set(ByVal value As String)
            _ref1addy1 = value
        End Set
    End Property

    Private _ref1addy2 As String
    Public Property Ref1Addy2() As String
        Get
            Return _ref1addy2
        End Get
        Set(ByVal value As String)
            _ref1addy2 = value
        End Set
    End Property

    Private _ref1city As String
    Public Property Ref1city() As String
        Get
            Return _ref1city
        End Get
        Set(ByVal value As String)
            _ref1city = value
        End Set
    End Property

    Private _ref1State As String
    Public Property Ref1state() As String
        Get
            Return _ref1State
        End Get
        Set(ByVal value As String)
            _ref1State = value
        End Set
    End Property

    Private _ref1Zip As String
    Public Property Ref1zip() As String
        Get
            Return _ref1Zip
        End Get
        Set(ByVal value As String)
            _ref1Zip = value
        End Set
    End Property

    Private _ref1phone As String
    Public Property Ref1phone() As String
        Get
            Return _ref1phone
        End Get
        Set(ByVal value As String)
            _ref1phone = value
        End Set
    End Property

    Private _ref1fname As String
    Public Property Ref1fname() As String
        Get
            Return _ref1fname
        End Get
        Set(ByVal value As String)
            _ref1fname = value
        End Set
    End Property

    Private _ref1lname As String
    Public Property Ref1lname() As String
        Get
            Return _ref1lname
        End Get
        Set(ByVal value As String)
            _ref1lname = value
        End Set
    End Property

    Private _ref1rel As String
    Public Property Ref1rel() As String
        Get
            Return _ref1rel
        End Get
        Set(ByVal value As String)
            _ref1rel = value
        End Set
    End Property

    Private _ref2addy1 As String
    Public Property Ref2addy1() As String
        Get
            Return _ref2addy1
        End Get
        Set(ByVal value As String)
            _ref2addy1 = value
        End Set
    End Property

    Private _ref2addy2 As String
    Public Property Ref2Addy2() As String
        Get
            Return _ref2addy2
        End Get
        Set(ByVal value As String)
            _ref2addy2 = value
        End Set
    End Property

    Private _ref2city As String
    Public Property Ref2city() As String
        Get
            Return _ref2city
        End Get
        Set(ByVal value As String)
            _ref2city = value
        End Set
    End Property

    Private _ref2State As String
    Public Property Ref2state() As String
        Get
            Return _ref2State
        End Get
        Set(ByVal value As String)
            _ref2State = value
        End Set
    End Property

    Private _ref2Zip As String
    Public Property Ref2zip() As String
        Get
            Return _ref2Zip
        End Get
        Set(ByVal value As String)
            _ref2Zip = value
        End Set
    End Property

    Private _ref2phone As String
    Public Property Ref2phone() As String
        Get
            Return _ref2phone
        End Get
        Set(ByVal value As String)
            _ref2phone = value
        End Set
    End Property

    Private _ref2fname As String
    Public Property Ref2fname() As String
        Get
            Return _ref2fname
        End Get
        Set(ByVal value As String)
            _ref2fname = value
        End Set
    End Property

    Private _ref2lname As String
    Public Property Ref2lname() As String
        Get
            Return _ref2lname
        End Get
        Set(ByVal value As String)
            _ref2lname = value
        End Set
    End Property

    Private _ref2rel As String
    Public Property Ref2rel() As String
        Get
            Return _ref2rel
        End Get
        Set(ByVal value As String)
            _ref2rel = value
        End Set
    End Property

    Private _collCostProj As String
    Public Property CollCostProj() As String
        Get
            Return _collCostProj
        End Get
        Set(ByVal value As String)
            _collCostProj = value
        End Set
    End Property

    Public Sub ChangeRef(ByVal ref As String)
        Select Case ref
            Case "Employer"
                Relationship = "EM"
            Case "Friend"
                Relationship = "FR"
            Case "Guardian"
                Relationship = "GU"
            Case "Spouse"
                Relationship = "SP"
            Case "Not Available"
                Relationship = "N"
            Case "Neighbor"
                Relationship = "NE"
            Case "Other"
                Relationship = "OT"
            Case "Parent"
                Relationship = "PA"
            Case "Relative"
                Relationship = "RE"
            Case "Roommate"
                Relationship = "RM"
            Case "Sibling"
                Relationship = "SI"
            Case "E"
                Relationship = "Employer"
            Case "EM"
                Relationship = "Employer"
            Case "F"
                Relationship = "Friend"
            Case "FR"
                Relationship = "Friend"
            Case "G"
                Relationship = "Guardian"
            Case "M"
                Relationship = "Spouse"
            Case "N"
                Relationship = "Not Available"
            Case "NE"
                Relationship = "Neighbor"
            Case "O"
                Relationship = "Other"
            Case "OT"
                Relationship = "Other"
            Case "P"
                Relationship = "Parent"
            Case "PA"
                Relationship = "Parent"
            Case "R"
                Relationship = "Relative"
            Case "RE"
                Relationship = "Relative"
            Case "RM"
                Relationship = "Roommate"
            Case "S"
                Relationship = "Sibling"
            Case "SI"
                Relationship = "Sibling"
            Case "SP"
                Relationship = "Spouse"
        End Select
    End Sub

End Class
