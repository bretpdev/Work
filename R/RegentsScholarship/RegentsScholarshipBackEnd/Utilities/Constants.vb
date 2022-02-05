Public Class Constants
    ''' <summary>
    ''' Integer representing the award year, calculated from the current date.
    ''' </summary>
    ''' <remarks>The award year switches to the next calendar year on October 1.</remarks>
    Public Shared ReadOnly Property CURRENT_AWARD_YEAR() As Integer
        Get
            Dim dateInAwardYear As DateTime = DateTime.Now.AddMonths(3)
            Return dateInAwardYear.Year
        End Get
    End Property

    ''' <summary>
    ''' The password that is created for new users and for resetting a user's password.
    ''' </summary>
    Public Const DEFAULT_PASSWORD As String = "Welcome1"

    ''' <summary>
    ''' The maximum number of credit hours for which we'll pay the exemplary award.
    ''' </summary>
    Public Const MAX_CREDIT_HOURS_PAYABLE As Double = 65.0

    ''' <summary>
    ''' The maximum number of semesters for which we'll make scholarship payments.
    ''' </summary>
    Public Const MAX_SEMESTERS_PAYABLE As Integer = 4

    ''' <summary>
    ''' The maximum amount allowed for UESP payments.
    ''' </summary>
    Public Const MAX_UESP_AWARD_AMOUNT As Double = 400.0

    ''' <summary>
    ''' The earliest date that SQL Server will recognize. This differs from .NET's default date value.
    ''' </summary>
    Public Shared ReadOnly Property MINIMUM_SQL_SERVER_DATE() As DateTime
        Get
            Return New DateTime(1800, 1, 1)
        End Get
    End Property

    ''' <summary>
    ''' Path to the network folder where non-student (e.g., school, district) documents are saved.
    ''' </summary>
    Public Shared ReadOnly Property NON_STUDENT_DOCUMENT_ROOT() As String
        Get
            Dim path As String = "\\AD4\Restricted\Regents Scholarships\Non Student Documents\"
            If Constants.TEST_MODE Then
                path += "Test\"
            End If
            Return path
        End Get
    End Property

    ''' <summary>
    ''' Path to the network folder where student documents are saved.
    ''' </summary>
    Public Shared ReadOnly Property STUDENT_DOCUMENT_ROOT() As String
        Get
            Dim path As String = "\\AD4\Restricted\Regents Scholarships\Student Accounts\"
            If Constants.TEST_MODE Then
                path += "Test\"
            End If
            Return path
        End Get
    End Property

    ''' <summary>
    ''' User ID for actions performed by the system, such as reviews and letters.
    ''' </summary>
    Public Const SYSTEM_USER_ID As String = "RSS"

    ''' <summary>
    ''' Indicator of whether the system is running in test mode.
    ''' </summary>
    ''' <remarks>
    ''' A -mode command-line argument can be passed, followed by live or test, to specify the mode to run in.
    ''' If no mode is specified, the system runs in live by default.
    ''' </remarks>
    Public Shared ReadOnly Property TEST_MODE() As Boolean
        Get
            Return Environment.GetCommandLineArgs().Contains("test")
        End Get
    End Property

#Region "Nested classes"
    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the AccessLevelLookup table.
    ''' </summary>
    Public Class AccessLevel
        Public Const READ_ONLY As String = "Read only"
        Public Const APPLICATION_REVIEW As String = "Application review"
        Public Const PA As String = "PA"
        Public Const DCR As String = "DCR"
        Public Const BATCH_PROCESSING As String = "Batch processing"
        Public Const INACTIVE As String = "Inactive"
        Public Const PAYMENT_PROCESSING As String = "Payment processing"
        Public Const PAYMENT_PROCESSING_OVERRIDE As String = "Payment processing override"
    End Class

    ''' <summary>
    ''' String constants that exectly match the values in the Description column of the ActTypeLookup table.
    ''' </summary>
    Public Class ActCategory
        Public Const COMPOSITE As String = "Composite"
        Public Const ENGLISH As String = "English"
        Public Const MATH As String = "Math"
        Public Const READING As String = "Reading"
        Public Const SCIENCE As String = "Science"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the AddressTypeLookup table.
    ''' </summary>
    Public Class AddressType
        Public Const HOME As String = "Home"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the AwardStatusLookup table.
    ''' </summary>
    Public Class AwardStatus
        Public Const APPEAL_IN_PROCESS As String = "Appeal in Process"
        Public Const APPLICATION_DOWNLOADED As String = "Application Downloaded"
        Public Const APPROVED As String = "Approved"
        Public Const CONDITIONAL_APPROVAL As String = "Conditional Approval"
        Public Const DEFERRED As String = "Deferred"
        Public Const DENIED As String = "Denied"
        Public Const ELIGIBILITY_COMPLETED As String = "Eligibility Completed"
        Public Const ELIGIBILITY_EXPIRED As String = "Eligibility Expired"
        Public Const ELIGIBILITY_FORFEITED As String = "Eligibility Forfeited"
        Public Const INCOMPLETE As String = "Incomplete"
        Public Const LEAVE_OF_ABSENCE As String = "Leave of Absence"
        Public Const PENDING_APPROVAL As String = "Pending Approval"
        Public Const PENDING_DENIAL As String = "Pending Denial"
        Public Const PROBATION As String = "Probation"
        Public Const REVIEW_IN_PROCESS As String = "Review in Process"
    End Class

    ''' <summary>
    ''' String constants that exactly match the distinct values in the Description column of the AwardAmountLookup table.
    ''' </summary>
    Public Class AwardType
        Public Const BASE_AWARD_PRIORITY_DEADLINE_MET As String = "Base Award - Priority Deadline Met"
        Public Const BASE_AWARD_PRIORITY_DEADLINE_NOT_MET As String = "Base Award - Priority Deadline Not Met"
        Public Const EXEMPLARY_AWARD As String = "Exemplary Award"
        Public Const UESP_SUPPLEMENTAL_AWARD As String = "UESP Supplemental Award"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the ClassWeightLookup table.
    ''' </summary>
    Public Class ClassWeight
        Public Const AP As String = "AP"
        Public Const CE As String = "CE"
        Public Const IB As String = "IB"
        Public Const NONE As String = ""
        Public Const PRE_IB As String = "Pre-IB"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values that we care about in the Description column of the CollegeLookup table.
    ''' </summary>
    Public Class CollegeName
        Public Const BRIGHAM_YOUNG_UNIVERSITY As String = "Brigham Young University"
        Public Const LDS_BUSINESS_COLLEGE As String = "LDS Business College"
        Public Const WESTMINSTER_COLLEGE As String = "Westminster College"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the CollegeTermLookup table.
    ''' </summary>
    Public Class CollegeTerm
        Public Const FALL As String = "Fall"
        Public Const SPRING As String = "Spring"
        Public Const SUMMER As String = "Summer"
        Public Const WINTER As String = "Winter"
    End Class

    ''' <summary>
    ''' String constants that exactly match the contents of the CommunicationEntityTypeLookup table.
    ''' </summary>
    Public Class CommunicationEntityType
        Public Const DISTRICT As String = "District"
        Public Const MISC As String = "Misc"
        Public Const SCHOOL As String = "School"
        Public Const STUDENT As String = "Student"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the CommunicationSourceLookup table.
    ''' </summary>
    Public Class CommunicationSource
        Public Const COUNSELOR As String = "Counselor"
        Public Const DISTRICT_ADMINISTRATOR As String = "District Administrator"
        Public Const PARENT As String = "Parent"
        Public Const PRINCIPAL As String = "Principal"
        Public Const SCHOOL_ADMINISTRATOR As String = "School Administrator"
        Public Const STUDENT As String = "Student"
        Public Const TEACHER As String = "Teacher"
        Public Const UHEAA_STAFF As String = "UHEAA Staff"
        Public Const USHE_STAFF As String = "USHE Staff"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the CommunicationTypeLookup table.
    ''' </summary>
    Public Class CommunicationType
        Public Const PHONE As String = "Phone"
        Public Const LETTER As String = "Letter"
        Public Const FAX As String = "Fax"
        Public Const EMAIL As String = "Email"
        Public Const OFFICE_VISIT As String = "Office Visit"
        Public Const ADMIN_NOTES As String = "Admin Notes"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the ClassTypeLookup table.
    ''' </summary>
    Public Class CourseCategory
        Public Const ENGLISH As String = "English"
        Public Const FOREIGN_LANGUAGE As String = "Foreign Language"
        Public Const MATHEMATICS As String = "Mathematics"
        Public Const SCIENCE As String = "Science"
        Public Const SOCIAL_SCIENCE As String = "Social Science"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the DenialReasonLookup table that the system deals with.
    ''' </summary>
    Public Class DenialReason
        Public Const ACT_SCORE_NOT_REPORTED As String = "ACT Score not reported"
        Public Const APPLICATION_NOT_RECEIVED As String = "Application not received"
        Public Const APPLICATION_RECEIVED_LATE As String = "Application received late"
        Public Const COLLEGE_TRANSCRIPT_NOT_SUBMITTED As String = "College transcript not submitted"
        Public Const COLLEGE_TRANSCRIPT_RECEIVED_LATE As String = "College Transcript received late"
        Public Const CONDITIONAL_ACCEPTANCE_FORM_NOT_RECEIVED As String = "Conditional Acceptance form not received"
        Public Const CONDITIONAL_ACCEPTANCE_FORM_RECEIVED_LATE As String = "Conditional Acceptance form received late"
        Public Const COURSE_FORM_NOT_RECEIVED As String = "Course form not received"
        Public Const COURSE_FORM_RECEIVED_LATE As String = "Course form received late"
        Public Const CRIMINAL_RECORD_EXISTS As String = "Criminal record exists"
        Public Const CUMULATIVE_GPA_REQUIREMENT_NOT_MET As String = "Cumulative GPA requirement not met"
        Public Const ENGLISH_REQUIREMENT_NOT_MET As String = "English requirement not met"
        Public Const FOREIGN_LANGUAGE_REQUIREMENT_NOT_MET As String = "Foreign Language requirement not met"
        Public Const HIGH_SCHOOL_TRANSCRIPT_NOT_PROVIDED As String = "High School Transcript not provided"
        Public Const HIGH_SCHOOL_TRANSCRIPT_RECEIVED_LATE As String = "High School Transcript received late"
        Public Const MINIMUM_GRADE_REQUIREMENT_NOT_MET As String = "Minimum grade requirement not met"
        Public Const MINIMUM_GRADE_REQUIREMENT_NOT_MET_DUE_TO_P_GRADE As String = "Minimum grade requirement not met due to P grade"
        Public Const MINIMUM_NUMBER_OF_ENROLLED_COLLEGE_CREDITS_NOT_MET As String = "Minimum number of enrolled college credits not met"
        Public Const MATH_REQUIREMENT_NOT_MET As String = "Math requirement not met"
        Public Const NOT_A_US_CITIZEN As String = "Not a US Citizen"
        Public Const NOT_ENROLLED_AT_AN_ELIGIBLE_UTAH_INSTITUTION_OF_HIGHER_EDUCATION As String = "Not enrolled at an eligible Utah institution of higher education"
        Public Const NOT_ENROLLED_FALL_SEMESTER_IMMEDIATELY_AFTER_GRADUATION As String = "Not enrolled Fall semester immediately after graduation"
        Public Const PROOF_OF_CITIZENSHIP_NOT_RECEIVED As String = "Proof of citizenship not received"
        Public Const PROOF_OF_CITIZENSHIP_RECEIVED_LATE As String = "Proof of citizenship received late"
        Public Const PROOF_OF_REGISTRATION_NOT_RECEIVED As String = "Proof of registration not received"
        Public Const PROOF_OF_REGISTRATION_RECEIVED_LATE As String = "Proof of registration received late"
        Public Const SCIENCE_BIOLOGY_REQUIREMENT_NOT_MET As String = "Science Biology requirement not met"
        Public Const SCIENCE_CHEMISTRY_REQUIREMENT_NOT_MET As String = "Science Chemistry requirement not met"
        Public Const SCIENCE_PHYSICS_REQUIREMENT_NOT_MET As String = "Science Physics requirement not met"
        Public Const SCIENCE_REQUIREMENT_NOT_MET As String = "Science requirement not met"
        Public Const SOCIAL_SCIENCE_REQUIREMENT_NOT_MET As String = "Social Science requirement not met"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the DocumentTypeLookup table.
    ''' </summary>
    Public Class DocumentType
        Public Const APPEAL As String = "Appeal"
        Public Const APPEAL_DECISION As String = "Appeal Decision"
        Public Const APPLICATION As String = "Application"
        Public Const CONDITIONAL_ACCEPTANCE_FORM As String = "Conditional Acceptance Form"
        Public Const DEFERMENT_DECISION As String = "Deferment Decision"
        Public Const DEFERMENT_REQUEST As String = "Deferment Request"
        Public Const FINAL_COLLEGE_TRANSCRIPT As String = "Final College Transcript"
        Public Const FINAL_HIGH_SCHOOL_TRANSCRIPT As String = "Final High School Transcript"
        Public Const HIGH_SCHOOL_SCHEDULE As String = "High School Schedule"
        Public Const INITIAL_COLLEGE_TRANSCRIPT As String = "Initial College Transcript"
        Public Const INITIAL_HIGH_SCHOOL_TRANSCRIPT As String = "Initial High School Transcript"
        Public Const LEAVE_OF_ABSENCE_DECISION As String = "Leave of Absence Decision"
        Public Const LEAVE_OF_ABSENCE_REQUEST As String = "Leave of Absence Request"
        Public Const PROOF_OF_CITIZENSHIP As String = "Proof of Citizenship"
        Public Const PROOF_OF_ENROLLMENT As String = "Proof of Enrollment"
        Public Const SIGNATURE_PAGE As String = "Signature Page"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the EmailTypeLookup table.
    ''' </summary>
    Public Class EmailType
        Public Const PERSONAL As String = "Personal"
        Public Const SCHOOL As String = "School"
    End Class

    ''' <summary>
    ''' String constants that exectly match the values in the Description column of the LeaveDeferralTypeLookup table.
    ''' </summary>
    Public Class LeaveDeferralType
        Public Const DEFERRAL As String = "Deferral"
        Public Const LEAVE_OF_ABSENCE As String = "Leave of Absence"
    End Class

    ''' <summary>
    ''' String constants that exectly match the contents of the PaymentStatusLookup table.
    ''' </summary>
    Public Class PaymentStatus
        Public Const APPROVED As String = "Approved"
        Public Const DENIED As String = "Denied"
        Public Const PENDING As String = "Pending"
        Public Const PRELIM_APPROVAL As String = "Prelim Approval"
        Public Const PRELIM_DENIED As String = "Prelim Denied"
    End Class

    ''' <summary>
    ''' String constants that exactly match the contents of the PaymentTypeLookup table.
    ''' </summary>
    Public Class PaymentType
        Public Const BASE As String = "Base"
        Public Const EXEMPLARY As String = "Exemplary"
        Public Const UESP As String = "UESP"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the PhoneTypeLookup table.
    ''' </summary>
    Public Class PhoneType
        Public Const CELL As String = "Cell"
        Public Const PRIMARY As String = "Primary"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the ReviewTypeLookup table.
    ''' </summary>
    Public Class ReviewType
        Public Const BASE_AWARD As String = "Base Award"
        Public Const CATEGORY As String = "Category"
        Public Const CLASS_ As String = "Class"
        Public Const EXEMPLARY_AWARD As String = "Exemplary Award"
        Public Const FINAL_TRANSCRIPT As String = "Final Transcript"
        Public Const FIRST_QUICK As String = "First Quick"
        Public Const INITIAL_AWARD As String = "Initial Award"
        Public Const INITIAL_TRANSCRIPT As String = "Initial Transcript"
        Public Const SECOND_QUICK As String = "Second Quick"
        Public Const SECOND_TRANSCRIPT As String = "Second Transcript"
        Public Const UESP_AWARD As String = "UESP Award"
    End Class

    ''' <summary>
    ''' String constants that exactly match the values in the Description column of the UsbctStatusLookup table.
    ''' </summary>
    Public Class UsbctStatus
        Public Const FAIL As String = "Fail"
        Public Const PASS As String = "Pass"
    End Class
#End Region 'Nested classes
End Class
