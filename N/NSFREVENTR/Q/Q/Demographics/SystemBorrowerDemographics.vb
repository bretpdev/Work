Public Class SystemBorrowerDemographics
    Inherits BorrowerDemographics

    Private _addrValidityIndicator As String
    ''' <summary>
    ''' Borrower Address Validity Indicator (Y or N)
    ''' </summary>
    Public Property AddrValidityIndicator() As String
        Get
            Return _addrValidityIndicator
        End Get
        Set(ByVal value As String)
            _addrValidityIndicator = value
        End Set
    End Property

    Private _addressValidityDate As Date
    ''' <summary>
    ''' Effective date of address validity
    ''' </summary>
    Public Property AddrValidityDate() As Date
        Get
            Return _addressValidityDate
        End Get
        Set(ByVal value As Date)
            _addressValidityDate = value
        End Set
    End Property

    Private _altPhoneValidityIndicator As String
    ''' <summary>
    ''' Borrower Alternate Phone Validity Indicator (Y or N)
    ''' </summary>
    Public Property AltPhoneValidityIndicator() As String
        Get
            Return _altPhoneValidityIndicator
        End Get
        Set(ByVal value As String)
            _altPhoneValidityIndicator = value
        End Set
    End Property

    Private _phoneValidityIndicator As String
    ''' <summary>
    ''' Borrower Phone Validity Indicator (Y or N)
    ''' </summary>
    Public Property PhoneValidityIndicator() As String
        Get
            Return _phoneValidityIndicator
        End Get
        Set(ByVal value As String)
            _phoneValidityIndicator = value
        End Set
    End Property

    Private _phoneValidityDate As Date
    ''' <summary>
    ''' Effective date of phone validity
    ''' </summary>
    Public Property PhoneValidityDate() As Date
        Get
            Return _phoneValidityDate
        End Get
        Set(ByVal value As Date)
            _phoneValidityDate = value
        End Set
    End Property

    Private _emailValidityIndicator As String
    ''' <summary>
    ''' Borrower Email Validity Indicator (Y or N)
    ''' </summary>
    ''' <remarks>I know, this is kind of a weird place to have this defined.  This has been placed here because 
    ''' of how it just happens to be part of what MD uses from the text file it reads and it happens to be part 
    ''' of what the common functions pull from the system demographic screens.  The choice was to place it here or to 
    ''' define it in both the MD and system objects that inherit from this object.  I made my choice.  I'm sorry if you don't like it.
    ''' Deal with it!!!</remarks>
    Public Property EmailValidityIndicator() As String
        Get
            Return _emailValidityIndicator
        End Get
        Set(ByVal value As String)
            _emailValidityIndicator = value
        End Set
    End Property

    Private _emailValidityDate As Date
    ''' <summary>
    ''' Effective date of e-mail validity
    ''' </summary>
    Public Property EmailValidityDate() As Date
        Get
            Return _emailValidityDate
        End Get
        Set(ByVal value As Date)
            _emailValidityDate = value
        End Set
    End Property

    Private _dob As String
    ''' <summary>
    ''' Borrower Date Of Birth
    ''' </summary>
    Public Property DOB() As String
        Get
            Return _dob
        End Get
        Set(ByVal value As String)
            _dob = value
        End Set
	End Property

	''' <summary>
	''' Moble, Landline, Unknown Phone type
	''' </summary>
	''' <remarks></remarks>
	Private _mbl As String
	Public Property MBL() As String
		Get
			Return _mbl
		End Get
		Set(ByVal value As String)
			_mbl = value
		End Set
	End Property

	''' <summary>
	''' Consent to contact borrower with an autodialer
	''' </summary>
	''' <remarks></remarks>
	Private _consent As String
	Public Property Consent() As String
		Get
			Return _consent
		End Get
		Set(ByVal value As String)
			_consent = value
		End Set
	End Property


	Private _primExt As String
	Public Property PrimExt() As String
		Get
			Return _primExt
		End Get
		Set(ByVal value As String)
			_primExt = value
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
