Imports System.IO
Imports System.Drawing
Imports Uheaa.Common

<CLSCompliant(True)> _
Public MustInherit Class MDBorrowerDemographics
    Inherits BorrowerDemographics

    Private _fPText As String
    Private _printFont As Font

    Public Property EcorrCorrespondence As Boolean
    Public Property EcorrBilling As Boolean
    Public Property EcorrTax As Boolean
    Public Property DefaultLetterFormat As Integer
    Public Property UpdatedAlternateFormat As String
    Public Property UpdatedAlternateFormatId As Integer
    Public Property IsForeignAddress As Boolean

    'Demographic Data

    ''' <summary>
    ''' Borrower's accoount number.
    ''' </summary>
    Public Property CLAccNum() As String
    ''' <summary>
    ''' The COMPASS demographics information for the borrower.
    ''' </summary>
    Public Property CompassDemos() As MDBorrowerDemographics
    ''' <summary>
    ''' Indicator as to whether the borrower was found on the system the instance is designated for.
    ''' </summary>
    Public Property FoundOnSystem() As Boolean
    ''' <summary>
    ''' (DO NOT USE ANY MORE, DEPRECATED) What system this instance is designated for. (DO NOT USE ANY MORE, DEPRECATED)
    ''' </summary>
    Public Property TheSystem() As WhatSystem
    ''' <summary>
    ''' Borrower's first name.
    ''' </summary>
    Public Property FirstName() As String

    ''' <summary>
    ''' Borrower's last name.
    ''' </summary>
    Public Property LastName() As String
    ''' <summary>
    ''' Borrower's full name (first and last names combined).
    ''' </summary>
    Public Property Name() As String
    ''' <summary>
    ''' Borrower's date of birth.
    ''' </summary>
    ''' <value></value>
    Public Property DOB() As String
#Region "Home Phone"
    Public Property HomePhoneNum() As String
    Public Property HomePhoneExt() As String
    Public Property HomePhoneForeignCountry() As String
    Public Property HomePhoneForeignCity() As String
    Public Property HomePhoneForeignLocalNumber() As String
    Public Property HomePhoneMBL() As String
    Public Property HomePhoneConsent() As String
    Public Property HomePhoneValidityIndicator() As String
    Public Property HomePhoneVerificationDate() As String
#End Region
#Region "Other Phone"
    Public Property OtherPhoneNum() As String
    Public Property OtherPhoneExt() As String
    Public Property OtherPhoneForeignCountry() As String
    Public Property OtherPhoneForeignCity() As String
    Public Property OtherPhoneForeignLocalNumber() As String
    Public Property OtherPhoneMBL() As String
    Public Property OtherPhoneConsent() As String
    Public Property OtherPhoneValidityIndicator() As String
    Public Property OtherPhoneVerificationDate() As String
#End Region
#Region "Other Phone 2"
    Public Property OtherPhone2Num() As String
    Public Property OtherPhone2Ext() As String
    Public Property OtherPhone2ForeignCountry() As String
    Public Property OtherPhone2ForeignCity() As String
    Public Property OtherPhone2ForeignLocalNumber() As String
    Public Property OtherPhone2MBL() As String
    Public Property OtherPhone2Consent() As String
    Public Property OtherPhone2ValidityIndicator() As String
    Public Property OtherPhone2VerificationDate() As String
#End Region
#Region "Other Phone 3"
    Public Property OtherPhone3Num() As String
    Public Property OtherPhone3Ext() As String
    Public Property OtherPhone3ForeignCountry() As String
    Public Property OtherPhone3ForeignCity() As String
    Public Property OtherPhone3ForeignLocalNumber() As String
    Public Property OtherPhone3MBL() As String
    Public Property OtherPhone3Consent() As String
    Public Property OtherPhone3ValidityIndicator() As String
    Public Property OtherPhone3VerificationDate() As String
#End Region
    ''' <summary>
    ''' Indicator as whether the demographics have been verified.
    ''' </summary>
    Public Property DemographicsVerified() As Boolean
    ''' <summary>
    ''' Indicator as to whether a PO box should be allowed for the borrower.
    ''' </summary>
    Public Property POBoxAllowed() As String

    'valid indicators and data collected from the system (SP stands for system provided)

    ''' <summary>
    ''' Address verified date gathered originally from the system (SP stands for system provided).
    ''' </summary>
    Public Property SPAddrVerDt() As String
    ''' <summary>
    ''' Address validity indicator gathered originally from the system (SP stands for system provided)
    ''' </summary>
    Public Property SPAddrInd() As String
    ''' <summary>
    ''' Email verified date gathered originally from the system (SP stands for system provided)
    ''' </summary>
    Public Property SPEmailVerDt() As String
    ''' <summary>
    ''' Email validity indicator gathered originally from the system (SP stands for system provided)
    ''' </summary>
    Public Property SPEmailInd() As String
    'valid/verified indicators collected from the User (UP stands for user provided)

    Private _uPAddrVal As Boolean
    ''' <summary>
    ''' Address validity indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPAddrVal() As Boolean
        Get
            Return _uPAddrVal
        End Get
        Set(ByVal value As Boolean)
            _uPAddrVal = value
        End Set
    End Property

    Private _uPAddrVer As Boolean
    ''' <summary>
    ''' Address verified indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPAddrVer() As Boolean
        Get
            Return _uPAddrVer
        End Get
        Set(ByVal value As Boolean)
            _uPAddrVer = value
        End Set
    End Property

    Private _uPPhoneVal As Boolean
    ''' <summary>
    ''' Phone validity indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPPhoneVal() As Boolean
        Get
            Return _uPPhoneVal
        End Get
        Set(ByVal value As Boolean)
            _uPPhoneVal = value
        End Set
    End Property

    Private _uPPhoneNumVer As Boolean
    ''' <summary>
    ''' Phone verified indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPPhoneNumVer() As Boolean
        Get
            Return _uPPhoneNumVer
        End Get
        Set(ByVal value As Boolean)
            _uPPhoneNumVer = value
        End Set
    End Property

    Private _uPPhoneMBL As String
    ''' <summary>
    ''' Phone MBL indicator provied by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPPhoneMBL() As String
        Get
            Return _uPPhoneMBL
        End Get
        Set(ByVal value As String)
            _uPPhoneMBL = value
        End Set
    End Property

    Private _uPPhoneConsent As String
    ''' <summary>
    ''' Phone consent indicator provided by the user (UP stands for user provided)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPPhoneConsent() As String
        Get
            Return _uPPhoneConsent
        End Get
        Set(ByVal value As String)
            _uPPhoneConsent = value
        End Set
    End Property


    Private _uPOtherVal As Boolean
    ''' <summary>
    ''' Other or alternate phone validity indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPOtherVal() As Boolean
        Get
            Return _uPOtherVal
        End Get
        Set(ByVal value As Boolean)
            _uPOtherVal = value
        End Set
    End Property

    Private _uPOtherVer As Boolean
    ''' <summary>
    ''' Other or alternate phone verified indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPOtherVer() As Boolean
        Get
            Return _uPOtherVer
        End Get
        Set(ByVal value As Boolean)
            _uPOtherVer = value
        End Set
    End Property

    Private _uPOtherMBL As String
    ''' <summary>
    ''' Phone MBL indicator provied by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPOtherMBL() As String
        Get
            Return _uPOtherMBL
        End Get
        Set(ByVal value As String)
            _uPOtherMBL = value
        End Set
    End Property

    Private _uPOtherConsent As String
    ''' <summary>
    ''' Phone consent indicator provided by the user (UP stands for user provided)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPOtherConsent() As String
        Get
            Return _uPOtherConsent
        End Get
        Set(ByVal value As String)
            _uPOtherConsent = value
        End Set
    End Property

    Private _uPOther2Val As Boolean
    ''' <summary>
    ''' Other or alternate phone #2 validity indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPOther2Val() As Boolean
        Get
            Return _uPOther2Val
        End Get
        Set(ByVal value As Boolean)
            _uPOther2Val = value
        End Set
    End Property

    Private _uPOther2Ver As Boolean
    ''' <summary>
    ''' Other or alternate phone #2 verified indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPOther2Ver() As Boolean
        Get
            Return _uPOther2Ver
        End Get
        Set(ByVal value As Boolean)
            _uPOther2Ver = value
        End Set
    End Property

    Private _uPOther2MBL As String
    ''' <summary>
    ''' Phone MBL indicator provied by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPOther2MBL() As String
        Get
            Return _uPOther2MBL
        End Get
        Set(ByVal value As String)
            _uPOther2MBL = value
        End Set
    End Property

    ''' <summary>
    ''' Phone consent indicator provided by the user (UP stands for user provided)
    ''' </summary>
    Public Property UPOther2Consent() As String

    Private _uPEmailVal As Boolean
    ''' <summary>
    ''' Email validity indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPEmailVal() As Boolean
        Get
            Return _uPEmailVal
        End Get
        Set(ByVal value As Boolean)
            _uPEmailVal = value
        End Set
    End Property

    Private _uPEmailVer As Boolean
    ''' <summary>
    ''' Email verified indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UPEmailVer() As Boolean
        Get
            Return _uPEmailVer
        End Get
        Set(ByVal value As Boolean)
            _uPEmailVer = value
        End Set
    End Property



    'Demographic Fields updated for COMPASS only

    Private Shared _addressSaved As Boolean
    ''' <summary>
    ''' Indicator to track whether the Address was saved to the system (FOR COMPASS SYSTEM ONLY).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property AddressSaved() As Boolean
        Get
            Return _addressSaved
        End Get
        Set(ByVal value As Boolean)
            _addressSaved = value
        End Set
    End Property

    Private Shared _phoneSaved As Boolean
    ''' <summary>
    ''' Indicator to track whether the Phone was saved to the system (FOR COMPASS SYSTEM ONLY).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property PhoneSaved() As Boolean
        Get
            Return _phoneSaved
        End Get
        Set(ByVal value As Boolean)
            _phoneSaved = value
        End Set
    End Property

    Private Shared _otherSaved As Boolean
    ''' <summary>
    ''' Indicator to track whether the Other Phone was saved to the system (FOR COMPASS SYSTEM ONLY).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property OtherSaved() As Boolean
        Get
            Return _otherSaved
        End Get
        Set(ByVal value As Boolean)
            _otherSaved = value
        End Set
    End Property

    Private Shared _other2Saved As Boolean
    ''' <summary>
    ''' Indicator to track whether the Other Phone #2 was saved to the system (FOR COMPASS SYSTEM ONLY).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property Other2Saved() As Boolean
        Get
            Return _other2Saved
        End Get
        Set(ByVal value As Boolean)
            _other2Saved = value
        End Set
    End Property

    Private Shared _emailSaved As Boolean
    ''' <summary>
    ''' Indicator to track whether the Email was saved to the system (FOR COMPASS SYSTEM ONLY).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property EmailSaved() As Boolean
        Get
            Return _emailSaved
        End Get
        Set(ByVal value As Boolean)
            _emailSaved = value
        End Set
    End Property

    ''' <summary>
    ''' Indicator to track whether the other Email was saved to the system (FOR COMPASS SYSTEM ONLY).
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared _otherEmailSaved As Boolean
    Public Shared Property OtherEmailSaved() As Boolean
        Get
            Return _otherEmailSaved
        End Get
        Set(ByVal value As Boolean)
            _otherEmailSaved = value
        End Set
    End Property

    ''' <summary>
    ''' Indicator to track whether the other 2 Email was saved to the system (FOR COMPASS SYSTEM ONLY).
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared _otherEmail2Saved As Boolean
    Public Shared Property OtherEmail2Saved() As Boolean
        Get
            Return _otherEmail2Saved
        End Get
        Set(ByVal value As Boolean)
            _otherEmail2Saved = value
        End Set
    End Property

    ''' <summary>
    ''' Alternate or other email address.
    ''' </summary>
    ''' <remarks></remarks>
    Private _otherEmail As String
    Public Property OtherEmail() As String
        Get
            Return _otherEmail
        End Get
        Set(ByVal value As String)
            _otherEmail = value
        End Set
    End Property

    ''' <summary>
    ''' Alternate or other e-mail verified date gathered originally from the system (SP stands for system provided).
    ''' </summary>
    ''' <remarks></remarks>
    Private _sPOtEmailVerDt As String
    Public Property SPOtEmailVerDt() As String
        Get
            Return _sPOtEmailVerDt
        End Get
        Set(ByVal value As String)
            _sPOtEmailVerDt = value
        End Set
    End Property

    ''' <summary>
    ''' Alternate or other email validity indicator gathered originally from the system (SP stands for system provided)
    ''' </summary>
    ''' <remarks></remarks>
    Private _sPOtEmailInd As String
    Public Property SPOtEmailInd() As String
        Get
            Return _sPOtEmailInd
        End Get
        Set(ByVal value As String)
            _sPOtEmailInd = value
        End Set
    End Property

    ''' <summary>
    ''' Work or other email 2 address.
    ''' </summary>
    ''' <remarks></remarks>
    Private _otherEmail2 As String
    Public Property OtherEmail2() As String
        Get
            Return _otherEmail2
        End Get
        Set(ByVal value As String)
            _otherEmail2 = value
        End Set
    End Property

    ''' <summary>
    ''' Work or other email 2 verified date gathered originally from the system (SP stands for system provided).
    ''' </summary>
    ''' <remarks></remarks>
    Private _sPOt2EmailVerDt As String
    Public Property SPOt2EmailVerDt() As String
        Get
            Return _sPOt2EmailVerDt
        End Get
        Set(ByVal value As String)
            _sPOt2EmailVerDt = value
        End Set
    End Property

    ''' <summary>
    ''' Work or other email 2 validity indicator gathered originally from the system (SP stands for system provided)
    ''' </summary>
    ''' <remarks></remarks>
    Private _sPOt2EmailInd As String
    Public Property SPOt2EmailInd() As String
        Get
            Return _sPOt2EmailInd
        End Get
        Set(ByVal value As String)
            _sPOt2EmailInd = value
        End Set
    End Property

    ''' <summary>
    ''' Work or other email 2 address validity indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <remarks></remarks>
    Private _uPEmailOther2Val As String
    Public Property UPEmailOther2Val() As String
        Get
            Return _uPEmailOther2Val
        End Get
        Set(ByVal value As String)
            _uPEmailOther2Val = value
        End Set
    End Property

    ''' <summary>
    ''' Alternate or other email address validity indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <remarks></remarks>
    Private _uPEmailOtherVal As String
    Public Property UPEmailOtherVal() As String
        Get
            Return _uPEmailOtherVal
        End Get
        Set(ByVal value As String)
            _uPEmailOtherVal = value
        End Set
    End Property

    ''' <summary>
    ''' Alternate or other email address verified indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <remarks></remarks>
    Private _uPEMailOtherVer As String
    Public Property UPEMailOtherVer() As String
        Get
            Return _uPEMailOtherVer
        End Get
        Set(ByVal value As String)
            _uPEMailOtherVer = value
        End Set
    End Property

    ''' <summary>
    ''' Work or other email 2 address verified indicator provided by the user (UP stands for user provided).
    ''' </summary>
    ''' <remarks></remarks>
    Private _uPEMailOther2Ver As String
    Public Property UPEMailOther2Ver() As String
        Get
            Return _uPEMailOther2Ver
        End Get
        Set(ByVal value As String)
            _uPEMailOther2Ver = value
        End Set
    End Property



    ''' <summary>
    ''' (DO NOT USE ANY MORE, DEPRECATED) Enum for what system the instance represents. (DO NOT USE ANY MORE, DEPRECATED)
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum WhatSystem
        None = 0
        Onelink = 1
        Compass = 2
        LCO = 3
        UserProvided = 4
    End Enum

    Public MustOverride Sub PopulateObjectFromSystem()

#Region "Hidden Script Side Properties"

    'These properties are being hidden because they are only used on the script side of processing
    'for the DUDE side of processing the properties declared at the top of the class are used.  On the 
    'script side of processing it is expected that the programmer will use polymorphism and access these
    'properties by using the BorrowerDemographic object which is in this class' heritage.

    Private Shadows Property Addr3() As String
        Get
            Return MyBase.Addr3
        End Get
        Set(ByVal value As String)
            MyBase.Addr3 = value
        End Set
    End Property

    Private Shadows Property Country() As String
        Get
            Return MyBase.Country
        End Get
        Set(ByVal value As String)
            MyBase.Country = value
        End Set
    End Property

    Private Shadows Property Phone() As String
        Get
            Return MyBase.Phone
        End Get
        Set(ByVal value As String)
            MyBase.Phone = value
        End Set
    End Property

    Private Shadows Property AltPhone() As String
        Get
            Return MyBase.AltPhone
        End Get
        Set(ByVal value As String)
            MyBase.AltPhone = value
        End Set
    End Property

    Private Shadows Property FName() As String
        Get
            Return MyBase.FName
        End Get
        Set(ByVal value As String)
            MyBase.FName = value
        End Set
    End Property

    Private Shadows Property LName() As String
        Get
            Return MyBase.LName
        End Get
        Set(ByVal value As String)
            MyBase.LName = value
        End Set
    End Property

    Private Shadows Property AccountNumber() As String
        Get
            Return MyBase.AccountNumber
        End Get
        Set(ByVal value As String)
            MyBase.AccountNumber = value
        End Set
    End Property

#End Region

End Class
