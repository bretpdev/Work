<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegents
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    	Me.components = New System.ComponentModel.Container()
    	Dim dataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    	Dim dataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    	Dim dataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    	Dim dataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    	Dim dataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    	Dim dataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    	Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRegents))
    	Me.tabControlMaster = New System.Windows.Forms.TabControl()
    	Me.tabMainMenu = New System.Windows.Forms.TabPage()
    	Me.grpMainMenuStudentAccountSearch = New System.Windows.Forms.GroupBox()
    	Me.txtMainMenuSearchDateOfBirth = New System.Windows.Forms.MaskedTextBox()
    	Me.lblMainMenuSelectMatch = New System.Windows.Forms.Label()
    	Me.btnMainMenuSearch = New System.Windows.Forms.Button()
    	Me.lblMainMenuNoSearchResults = New System.Windows.Forms.Label()
    	Me.txtMainMenuSearchLastName = New System.Windows.Forms.TextBox()
    	Me.txtMainMenuSearchFirstName = New System.Windows.Forms.TextBox()
    	Me.txtMainMenuSearchSsn = New System.Windows.Forms.TextBox()
    	Me.lblMainMenuSearchDateOfBirth = New System.Windows.Forms.Label()
    	Me.lblMainMenuSearchLastName = New System.Windows.Forms.Label()
    	Me.lblMainMenuSearchFirstName = New System.Windows.Forms.Label()
    	Me.lblMainMenuSearchSsn = New System.Windows.Forms.Label()
    	Me.txtMainMenuSearchStateStudentId = New System.Windows.Forms.TextBox()
    	Me.lblMainMenuSearchStateStudentId = New System.Windows.Forms.Label()
    	Me.dgvMainMenuSearchMatches = New System.Windows.Forms.DataGridView()
    	Me.FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.StateStudentID = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.SocialSecurityNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.StreetAddressLine1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.City = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.AwardStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.MainMenuSearchResultBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.tabDemographics = New System.Windows.Forms.TabPage()
    	Me.grpDemographics411 = New System.Windows.Forms.GroupBox()
    	Me.pnlDemographics411 = New System.Windows.Forms.FlowLayoutPanel()
    	Me.grpHearAboutUs = New System.Windows.Forms.GroupBox()
    	Me.txtHowDidTheyHearAboutRegents = New System.Windows.Forms.TextBox()
    	Me.ScholarshipApplicationBindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
    	Me.Label2 = New System.Windows.Forms.Label()
    	Me.grpAuthThirdParties = New System.Windows.Forms.GroupBox()
    	Me.Panel1 = New System.Windows.Forms.Panel()
    	Me.AuthorizedThirdParty2 = New RegentsScholarshipFrontEnd.AuthorizedThirdPartyControl()
    	Me.AuthorizedThirdParty1 = New RegentsScholarshipFrontEnd.AuthorizedThirdPartyControl()
    	Me.txtDemographicsSsn = New System.Windows.Forms.TextBox()
    	Me.StudentBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.lblDemographicsSsn = New System.Windows.Forms.Label()
    	Me.txtDemographicsStateStudentId = New System.Windows.Forms.TextBox()
    	Me.lblDemographicsStateStudentId = New System.Windows.Forms.Label()
    	Me.btnDemographicsSaveChanges = New System.Windows.Forms.Button()
    	Me.grpDemographicsEligibility = New System.Windows.Forms.GroupBox()
    	Me.chkDemographicsIntendsToApplyForFederalAid = New System.Windows.Forms.CheckBox()
    	Me.chkDemographicsCriminalRecord = New System.Windows.Forms.CheckBox()
    	Me.chkDemographicsEligibleForFederalAid = New System.Windows.Forms.CheckBox()
    	Me.chkDemographicsUsCitizen = New System.Windows.Forms.CheckBox()
    	Me.grpDemographicsContact = New System.Windows.Forms.GroupBox()
    	Me.chkDemographicsValidSchoolEmail = New System.Windows.Forms.CheckBox()
    	Me.SchoolEmailBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.chkDemographicsValidPersonalEmail = New System.Windows.Forms.CheckBox()
    	Me.PersonalEmailBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.chkDemographicsValidCellPhone = New System.Windows.Forms.CheckBox()
    	Me.CellPhoneBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.chkDemographicsValidPrimaryPhone = New System.Windows.Forms.CheckBox()
    	Me.PrimaryPhoneBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.txtDemographicsSchoolEmail = New System.Windows.Forms.TextBox()
    	Me.txtDemographicsPersonalEmail = New System.Windows.Forms.TextBox()
    	Me.txtDemographicsCellPhone = New System.Windows.Forms.TextBox()
    	Me.txtDemographicsPrimaryPhone = New System.Windows.Forms.TextBox()
    	Me.lblDemographicsSchoolEmail = New System.Windows.Forms.Label()
    	Me.lblDemographicsPersonalEmail = New System.Windows.Forms.Label()
    	Me.lblDemographicsCellPhone = New System.Windows.Forms.Label()
    	Me.lblDemographicsPrimaryPhone = New System.Windows.Forms.Label()
    	Me.grpDemographicsAddress = New System.Windows.Forms.GroupBox()
    	Me.Label12 = New System.Windows.Forms.Label()
    	Me.txtDemographicsAddressLastUpdated = New System.Windows.Forms.TextBox()
    	Me.lblDemographicsCountry = New System.Windows.Forms.Label()
    	Me.txtDemographicsCountry = New System.Windows.Forms.TextBox()
    	Me.MailingAddressBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.chkDemographicsValidAddress = New System.Windows.Forms.CheckBox()
    	Me.txtDemographicsZip = New System.Windows.Forms.TextBox()
    	Me.cmbDemographicsState = New System.Windows.Forms.ComboBox()
    	Me.txtDemographicsCity = New System.Windows.Forms.TextBox()
    	Me.txtDemographicsStreetAddress2 = New System.Windows.Forms.TextBox()
    	Me.txtDemographicsStreetAddress1 = New System.Windows.Forms.TextBox()
    	Me.lblDemographicsCityStateZip = New System.Windows.Forms.Label()
    	Me.lblDemographicsStreetAddress2 = New System.Windows.Forms.Label()
    	Me.lblDemographicsStreetAddress1 = New System.Windows.Forms.Label()
    	Me.grpDemographicsPersonal = New System.Windows.Forms.GroupBox()
    	Me.txtDemographicsDateOfBirth = New System.Windows.Forms.MaskedTextBox()
    	Me.cmbDemographicsEthnicity = New System.Windows.Forms.ComboBox()
    	Me.cmbDemographicsGender = New System.Windows.Forms.ComboBox()
    	Me.txtDemographicsAlternateLastName = New System.Windows.Forms.TextBox()
    	Me.txtDemographicsLastName = New System.Windows.Forms.TextBox()
    	Me.txtDemographicsMiddleName = New System.Windows.Forms.TextBox()
    	Me.txtDemographicsFirstName = New System.Windows.Forms.TextBox()
    	Me.lblDemographicsEthnicity = New System.Windows.Forms.Label()
    	Me.lblDemographicsName = New System.Windows.Forms.Label()
    	Me.lblDemographicsGender = New System.Windows.Forms.Label()
    	Me.lblDemographicsAlternateLastName = New System.Windows.Forms.Label()
    	Me.lblDemographicsDateOfBirth = New System.Windows.Forms.Label()
    	Me.tabApplication = New System.Windows.Forms.TabPage()
    	Me.GroupBox3 = New System.Windows.Forms.GroupBox()
    	Me.txtApplicationPlannedCollegeToAttend = New System.Windows.Forms.TextBox()
    	Me.Label4 = New System.Windows.Forms.Label()
    	Me.chkApplicationOtherScholarshipAwards = New System.Windows.Forms.CheckBox()
    	Me.CollegeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.txtApplicationOtherScholarshipAwardsAmount = New System.Windows.Forms.TextBox()
    	Me.lblApplicationOtherScholarshipAwardsAmount = New System.Windows.Forms.Label()
    	Me.GroupBox2 = New System.Windows.Forms.GroupBox()
    	Me.chkApplicationAttendedAnotherSchool = New System.Windows.Forms.CheckBox()
    	Me.cmbApplication9thGradeSchoolName = New System.Windows.Forms.ComboBox()
    	Me.Label3 = New System.Windows.Forms.Label()
    	Me.grpApplicationCollegeEnrollment = New System.Windows.Forms.GroupBox()
    	Me.dtpApplicationTermBeginDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.cmbApplicationTerm = New System.Windows.Forms.ComboBox()
    	Me.lblApplicationTerm = New System.Windows.Forms.Label()
    	Me.txtApplicationEnrolledCredits = New System.Windows.Forms.TextBox()
    	Me.lblApplicationTermBeginDate = New System.Windows.Forms.Label()
    	Me.cmbApplicationCollegeName = New System.Windows.Forms.ComboBox()
    	Me.lblApplicationEnrolledCredits = New System.Windows.Forms.Label()
    	Me.lblApplicationCollegeName = New System.Windows.Forms.Label()
    	Me.btnApplicationViewDocuments = New System.Windows.Forms.Button()
    	Me.grpApplicationDocumentStatus = New System.Windows.Forms.GroupBox()
    	Me.chkApplicationAppealDenied = New System.Windows.Forms.CheckBox()
    	Me.chkApplicationAppealApproved = New System.Windows.Forms.CheckBox()
    	Me.lblApplicationAppealDecision = New System.Windows.Forms.Label()
    	Me.dtpApplicationProofOfCitizenshipReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.lblApplicationProofOfCitizenshipReceivedDate = New System.Windows.Forms.Label()
    	Me.dtpHighSchoolScheduleReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.Label10 = New System.Windows.Forms.Label()
    	Me.dtpApplicationDefermentDecisionDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationDefermentRequestReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationLoaDecisionDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationLoaRequestReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationAppealDecisionDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationAppealReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationProofOfEnrollmentReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationSignaturePageReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.lblApplicationFinalHighSchoolTranscriptReceivedDate = New System.Windows.Forms.Label()
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationReceivedDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.lblApplicationDefermentDecisionDate = New System.Windows.Forms.Label()
    	Me.lblApplicationDefermentRequestReceivedDate = New System.Windows.Forms.Label()
    	Me.lblApplicationLoaDecisionDate = New System.Windows.Forms.Label()
    	Me.lblApplicationLoaRequestReceivedDate = New System.Windows.Forms.Label()
    	Me.lblApplicationAppealDecisionDate = New System.Windows.Forms.Label()
    	Me.lblApplicationAppealReceivedDate = New System.Windows.Forms.Label()
    	Me.lblApplicationFinalCollegeTranscriptReceivedDate = New System.Windows.Forms.Label()
    	Me.lblApplicationProofOfEnrollmentReceivedDate = New System.Windows.Forms.Label()
    	Me.lblApplicationConditionalAcceptanceFormReceivedDate = New System.Windows.Forms.Label()
    	Me.lblApplicationSignaturePageReceivedDate = New System.Windows.Forms.Label()
    	Me.lblApplicationInitialCollegeTranscriptReceivedDate = New System.Windows.Forms.Label()
    	Me.lblApplicationInitialHighSchoolTranscriptReceivedDate = New System.Windows.Forms.Label()
    	Me.lblApplicationReceivedDate = New System.Windows.Forms.Label()
    	Me.grpApplicationReviewStatus = New System.Windows.Forms.GroupBox()
    	Me.chkApplicationFinalTranscriptReviewStartStop = New System.Windows.Forms.CheckBox()
    	Me.chkApplicationSecondTranscriptReviewStartStop = New System.Windows.Forms.CheckBox()
    	Me.chkApplicationInitialTranscriptReviewStartStop = New System.Windows.Forms.CheckBox()
    	Me.lblApplicationSecondQuickReviewInProgress = New System.Windows.Forms.Label()
    	Me.lblApplicationFirstQuickReviewInProgress = New System.Windows.Forms.Label()
    	Me.lblApplicationUespAwardReviewInProgress = New System.Windows.Forms.Label()
    	Me.txtApplicationSecondQuickReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationSecondQuickReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationSecondQuickReview = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationFirstQuickReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationFirstQuickReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationFirstQuickReview = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationSecondTranscriptReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationSecondTranscriptReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationSecondTranscriptReview = New System.Windows.Forms.CheckBox()
    	Me.lblReviewStatusUserId = New System.Windows.Forms.Label()
    	Me.lblReviewStatusDate = New System.Windows.Forms.Label()
    	Me.txtApplicationUespAwardReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationExemplaryAwardReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationBaseAwardReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationInitialAwardReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationCategoryReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationClassReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationFinalTranscriptReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationInitialTranscriptReviewDate = New System.Windows.Forms.TextBox()
    	Me.txtApplicationUespAwardReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationUespAwardReview = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationExemplaryAwardReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationExemplaryAwardReview = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationBaseAwardReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationBaseAwardReview = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationInitialAwardReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationInitialAwardReview = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationCategoryReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationCategoryReview = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationClassReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationClassReview = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationFinalTranscriptReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationFinalTranscriptReview = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationInitialTranscriptReviewUserId = New System.Windows.Forms.TextBox()
    	Me.chkApplicationInitialTranscriptReview = New System.Windows.Forms.CheckBox()
    	Me.grpApplicationClasses = New System.Windows.Forms.GroupBox()
    	Me.tabControlClasses = New System.Windows.Forms.TabControl()
    	Me.tabEnglish = New System.Windows.Forms.TabPage()
    	Me.Label5 = New System.Windows.Forms.Label()
    	Me.englishClass6 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.englishClass5 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.englishClass4 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.englishClass3 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.englishClass2 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.englishClass1 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.lblEnglishGrades = New System.Windows.Forms.Label()
    	Me.txtEnglishVerifiedDate = New System.Windows.Forms.TextBox()
    	Me.lblEnglishVerifiedDate = New System.Windows.Forms.Label()
    	Me.txtEnglishVerifiedBy = New System.Windows.Forms.TextBox()
    	Me.lblEnglishVerifiedBy = New System.Windows.Forms.Label()
    	Me.radEnglishRequirementMetInProgress = New System.Windows.Forms.RadioButton()
    	Me.lblEnglishWeightDesignation = New System.Windows.Forms.Label()
    	Me.radEnglishRequirementMetNo = New System.Windows.Forms.RadioButton()
    	Me.radEnglishRequirementMetYes = New System.Windows.Forms.RadioButton()
    	Me.lblEnglishRequirementMet = New System.Windows.Forms.Label()
    	Me.lblEnglishTitle = New System.Windows.Forms.Label()
    	Me.lblEnglishGradeLevel = New System.Windows.Forms.Label()
    	Me.lblEnglishCredits = New System.Windows.Forms.Label()
    	Me.lblEnglishWeightedAverageGrade = New System.Windows.Forms.Label()
    	Me.tabMathematics = New System.Windows.Forms.TabPage()
    	Me.Label6 = New System.Windows.Forms.Label()
    	Me.lblMathematicsGrades = New System.Windows.Forms.Label()
    	Me.txtMathematicsVerifiedDate = New System.Windows.Forms.TextBox()
    	Me.lblMathematicsVerifiedDate = New System.Windows.Forms.Label()
    	Me.txtMathematicsVerifiedBy = New System.Windows.Forms.TextBox()
    	Me.lblMathematicsVerifiedBy = New System.Windows.Forms.Label()
    	Me.radMathematicsRequirementMetInProgress = New System.Windows.Forms.RadioButton()
    	Me.lblMathematicsWeightDesignation = New System.Windows.Forms.Label()
    	Me.radMathematicsRequirementMetNo = New System.Windows.Forms.RadioButton()
    	Me.radMathematicsRequirementMetYes = New System.Windows.Forms.RadioButton()
    	Me.lblMathematicsRequirementMet = New System.Windows.Forms.Label()
    	Me.lblMathematicsTitle = New System.Windows.Forms.Label()
    	Me.lblMathematicsGradeLevel = New System.Windows.Forms.Label()
    	Me.lblMathematicsCredits = New System.Windows.Forms.Label()
    	Me.lblMathematicsWeightedAverageGrade = New System.Windows.Forms.Label()
    	Me.mathematicsClass6 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.mathematicsClass5 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.mathematicsClass4 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.mathematicsClass3 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.mathematicsClass2 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.mathematicsClass1 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.tabScienceWithLab = New System.Windows.Forms.TabPage()
    	Me.Label7 = New System.Windows.Forms.Label()
    	Me.lblScienceGrades = New System.Windows.Forms.Label()
    	Me.txtScienceVerifiedDate = New System.Windows.Forms.TextBox()
    	Me.lblScienceVerifiedDate = New System.Windows.Forms.Label()
    	Me.txtScienceVerifiedBy = New System.Windows.Forms.TextBox()
    	Me.lblScienceVerifiedBy = New System.Windows.Forms.Label()
    	Me.radScienceRequirementMetInProgress = New System.Windows.Forms.RadioButton()
    	Me.lblScienceWeightDesignation = New System.Windows.Forms.Label()
    	Me.radScienceRequirementMetNo = New System.Windows.Forms.RadioButton()
    	Me.radScienceRequirementMetYes = New System.Windows.Forms.RadioButton()
    	Me.lblScienceRequirementMet = New System.Windows.Forms.Label()
    	Me.lblScienceTitle = New System.Windows.Forms.Label()
    	Me.lblScienceGradeLevel = New System.Windows.Forms.Label()
    	Me.lblScienceCredits = New System.Windows.Forms.Label()
    	Me.lblScienceWeightedAverageGrade = New System.Windows.Forms.Label()
    	Me.scienceClass8 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.scienceClass7 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.scienceClass6 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.scienceClass5 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.scienceClass4 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.scienceClass3 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.scienceClass2 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.scienceClass1 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.tabSocialScience = New System.Windows.Forms.TabPage()
    	Me.Label8 = New System.Windows.Forms.Label()
    	Me.lblSocialScienceGrades = New System.Windows.Forms.Label()
    	Me.txtSocialScienceVerifiedDate = New System.Windows.Forms.TextBox()
    	Me.lblSocialScienceVerifiedDate = New System.Windows.Forms.Label()
    	Me.txtSocialScienceVerifiedBy = New System.Windows.Forms.TextBox()
    	Me.lblSocialScienceVerifiedBy = New System.Windows.Forms.Label()
    	Me.radSocialScienceRequirementMetInProgress = New System.Windows.Forms.RadioButton()
    	Me.lblSocialScienceWeightDesignation = New System.Windows.Forms.Label()
    	Me.radSocialScienceRequirementMetNo = New System.Windows.Forms.RadioButton()
    	Me.radSocialScienceRequirementMetYes = New System.Windows.Forms.RadioButton()
    	Me.lblSocialScienceRequirementMet = New System.Windows.Forms.Label()
    	Me.lblSocialScienceTitle = New System.Windows.Forms.Label()
    	Me.lblSocialScienceGradeLevel = New System.Windows.Forms.Label()
    	Me.lblSocialScienceCredits = New System.Windows.Forms.Label()
    	Me.lblSocialScienceWeightedAverageGrade = New System.Windows.Forms.Label()
    	Me.socialScienceClass8 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.socialScienceClass7 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.socialScienceClass6 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.socialScienceClass5 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.socialScienceClass4 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.socialScienceClass3 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.socialScienceClass2 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.socialScienceClass1 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.tabForeignLanguage = New System.Windows.Forms.TabPage()
    	Me.Label9 = New System.Windows.Forms.Label()
    	Me.lblForeignLanguageGrades = New System.Windows.Forms.Label()
    	Me.txtForeignLanguageVerifiedDate = New System.Windows.Forms.TextBox()
    	Me.lblForeignLanguageVerifiedDate = New System.Windows.Forms.Label()
    	Me.txtForeignLanguageVerifiedBy = New System.Windows.Forms.TextBox()
    	Me.lblForeignLanguageVerifiedBy = New System.Windows.Forms.Label()
    	Me.radForeignLanguageRequirementMetInProgress = New System.Windows.Forms.RadioButton()
    	Me.lblForeignLanguageWeightDesignation = New System.Windows.Forms.Label()
    	Me.radForeignLanguageRequirementMetNo = New System.Windows.Forms.RadioButton()
    	Me.radForeignLanguageRequirementMetYes = New System.Windows.Forms.RadioButton()
    	Me.lblForeignLanguageRequirementMet = New System.Windows.Forms.Label()
    	Me.lblForeignLanguageTitle = New System.Windows.Forms.Label()
    	Me.lblForeignLanguageGradeLevel = New System.Windows.Forms.Label()
    	Me.lblForeignLanguageCredits = New System.Windows.Forms.Label()
    	Me.lblForeignLanguageWeightedAverageGrade = New System.Windows.Forms.Label()
    	Me.foreignLanguageClass5 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.foreignLanguageClass4 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.foreignLanguageClass3 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.foreignLanguageClass2 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.foreignLanguageClass1 = New RegentsScholarshipFrontEnd.ClassDataControl()
    	Me.btnApplicationLinkDocument = New System.Windows.Forms.Button()
    	Me.grpApplicationAwardStatus = New System.Windows.Forms.GroupBox()
    	Me.txtAppYear = New System.Windows.Forms.TextBox()
    	Me.Label1 = New System.Windows.Forms.Label()
    	Me.dtpApplicationLeaveOfAbsenceEndDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationLeaveOfAbsenceBeginDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationDefermentEndDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationDefermentBeginDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.dtpApplicationAwardStatusLetterSent = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.PrimaryAwardBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.grpApplicationDenialReasons = New System.Windows.Forms.GroupBox()
    	Me.cmbApplicationDenialReason1 = New System.Windows.Forms.ComboBox()
    	Me.cmbApplicationDenialReason2 = New System.Windows.Forms.ComboBox()
    	Me.cmbApplicationDenialReason6 = New System.Windows.Forms.ComboBox()
    	Me.cmbApplicationDenialReason3 = New System.Windows.Forms.ComboBox()
    	Me.cmbApplicationDenialReason5 = New System.Windows.Forms.ComboBox()
    	Me.cmbApplicationDenialReason4 = New System.Windows.Forms.ComboBox()
    	Me.lblApplicationAwardStatusLetterSent = New System.Windows.Forms.Label()
    	Me.txtApplicationAwardStatusDate = New System.Windows.Forms.TextBox()
    	Me.lblApplicationExemplaryAwardAmount = New System.Windows.Forms.Label()
    	Me.btnApplicationSaveChanges = New System.Windows.Forms.Button()
    	Me.txtApplicationExemplaryAwardAmount = New System.Windows.Forms.TextBox()
    	Me.ExemplaryAwardBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.cmbApplicationLeaveOfAbsenceReason = New System.Windows.Forms.ComboBox()
    	Me.lblApplicationLeaveOfAbsenceReason = New System.Windows.Forms.Label()
    	Me.lblApplicationLeaveOfAbsenceEndDate = New System.Windows.Forms.Label()
    	Me.lblApplicationLeaveOfAbsenceBeginDate = New System.Windows.Forms.Label()
    	Me.cmbApplicationDefermentReason = New System.Windows.Forms.ComboBox()
    	Me.lblApplicationDefermentReason = New System.Windows.Forms.Label()
    	Me.lblApplicationDefermentEndDate = New System.Windows.Forms.Label()
    	Me.lblApplicationDefermentBeginDate = New System.Windows.Forms.Label()
    	Me.lblApplicationSupplementalAwardAmount = New System.Windows.Forms.Label()
    	Me.txtApplicationSupplementalAwardAmount = New System.Windows.Forms.TextBox()
    	Me.UespSupplementalAwardBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.chkApplicationSupplementalAwardApproved = New System.Windows.Forms.CheckBox()
    	Me.chkApplicationExemplaryAwardEarned = New System.Windows.Forms.CheckBox()
    	Me.lblApplicationBaseAwardAmount = New System.Windows.Forms.Label()
    	Me.txtApplicationBaseAwardAmount = New System.Windows.Forms.TextBox()
    	Me.txtApplicationAwardStatusUserId = New System.Windows.Forms.TextBox()
    	Me.lblApplicationAwardStatusUserId = New System.Windows.Forms.Label()
    	Me.lblApplicationAwardStatusDate = New System.Windows.Forms.Label()
    	Me.cmbApplicationAwardStatus = New System.Windows.Forms.ComboBox()
    	Me.lblApplicationAwardStatus = New System.Windows.Forms.Label()
    	Me.grpApplicationHighSchool = New System.Windows.Forms.GroupBox()
    	Me.txtApplicationHighSchoolCity = New System.Windows.Forms.TextBox()
    	Me.HighSchoolBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.txtApplicationHighSchoolDistrict = New System.Windows.Forms.TextBox()
    	Me.dtpApplicationGraduationDate = New RegentsScholarshipFrontEnd.NullableDateTimePicker()
    	Me.cmbApplicationHighSchoolName = New System.Windows.Forms.ComboBox()
    	Me.radApplicationUbsctFail = New System.Windows.Forms.RadioButton()
    	Me.radApplicationUbsctPass = New System.Windows.Forms.RadioButton()
    	Me.lblApplicationUbsct = New System.Windows.Forms.Label()
    	Me.txtApplicationActReadingScore = New System.Windows.Forms.TextBox()
    	Me.lblApplicationActReadingScore = New System.Windows.Forms.Label()
    	Me.txtApplicationActScienceScore = New System.Windows.Forms.TextBox()
    	Me.lblApplicationActScienceScore = New System.Windows.Forms.Label()
    	Me.txtApplicationActMathScore = New System.Windows.Forms.TextBox()
    	Me.lblApplicationActMathScore = New System.Windows.Forms.Label()
    	Me.txtApplicationActEnglishScore = New System.Windows.Forms.TextBox()
    	Me.lblApplicationActEnglishScore = New System.Windows.Forms.Label()
    	Me.chkApplicationIbDiploma = New System.Windows.Forms.CheckBox()
    	Me.txtApplicationActCompositeScore = New System.Windows.Forms.TextBox()
    	Me.lblApplicationActCompositeScore = New System.Windows.Forms.Label()
    	Me.txtApplicationCumulativeGpa = New System.Windows.Forms.TextBox()
    	Me.lblApplicationCumulativeGpa = New System.Windows.Forms.Label()
    	Me.txtApplicationCeebCode = New System.Windows.Forms.TextBox()
    	Me.lblApplicationGraduationDate = New System.Windows.Forms.Label()
    	Me.lblApplicationCeebCode = New System.Windows.Forms.Label()
    	Me.lblApplicationSchoolDistrict = New System.Windows.Forms.Label()
    	Me.lblApplicationHighSchoolCity = New System.Windows.Forms.Label()
    	Me.lblApplicationHighSchoolName = New System.Windows.Forms.Label()
    	Me.chkApplicationUtahHighSchool = New System.Windows.Forms.CheckBox()
    	Me.tabCommunications = New System.Windows.Forms.TabPage()
    	Me.CommunicationPrinter = New RegentsScholarshipFrontEnd.CommunicationRecordPrintingControl()
    	Me.CommunicationDataGridView = New System.Windows.Forms.DataGridView()
    	Me.UserID = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.clmTimeStamp = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.clmType = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.clmSource = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.clmSubject = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.clmText = New System.Windows.Forms.DataGridViewTextBoxColumn()
    	Me.Is411 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
    	Me.CommunicationBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    	Me.btnCommunicationViewDocuments = New System.Windows.Forms.Button()
    	Me.btnCommunicationLinkDocument = New System.Windows.Forms.Button()
    	Me.btnCommunicationClearFields = New System.Windows.Forms.Button()
    	Me.btnCommunicationSave = New System.Windows.Forms.Button()
    	Me.grpCommunicationRecord = New System.Windows.Forms.GroupBox()
    	Me.chkCommunications411 = New System.Windows.Forms.CheckBox()
    	Me.cmbCommunicationSource = New System.Windows.Forms.ComboBox()
    	Me.lblCommunicationSource = New System.Windows.Forms.Label()
    	Me.txtCommunicationComments = New System.Windows.Forms.TextBox()
    	Me.txtCommunicationSubject = New System.Windows.Forms.TextBox()
    	Me.cmbCommunicationType = New System.Windows.Forms.ComboBox()
    	Me.txtCommunicationUserId = New System.Windows.Forms.TextBox()
    	Me.txtCommunicationDateTime = New System.Windows.Forms.TextBox()
    	Me.lblCommunicationSubject = New System.Windows.Forms.Label()
    	Me.lblCommunicationType = New System.Windows.Forms.Label()
    	Me.lblCommunicationUserId = New System.Windows.Forms.Label()
    	Me.lblCommunicationDateTime = New System.Windows.Forms.Label()
    	Me.tabPayments = New System.Windows.Forms.TabPage()
    	Me.txtPaymentsCumulativeCreditHoursPaid = New System.Windows.Forms.TextBox()
    	Me.Label11 = New System.Windows.Forms.Label()
    	Me.btnPaymentsViewDocuments = New System.Windows.Forms.Button()
    	Me.btnPaymentsLinkDocument = New System.Windows.Forms.Button()
    	Me.btnPaymentsSaveChanges = New System.Windows.Forms.Button()
    	Me.pnlPayments = New System.Windows.Forms.FlowLayoutPanel()
    	Me.btnPaymentsNewPayment = New System.Windows.Forms.Button()
    	Me.tabPaymentBatch = New System.Windows.Forms.TabPage()
    	Me.splPaymentBatchSplitter = New System.Windows.Forms.SplitContainer()
    	Me.btnPaymentBatchPreliminary = New System.Windows.Forms.Button()
    	Me.lblPaymentBatchNumber = New System.Windows.Forms.Label()
    	Me.btnPaymentBatchFinal = New System.Windows.Forms.Button()
    	Me.txtPaymentBatchLog = New System.Windows.Forms.TextBox()
    	Me.lblHeaderRecordLocked = New System.Windows.Forms.Label()
    	Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
    	Me.btnDistrictCommunication = New System.Windows.Forms.ToolStripButton()
    	Me.btnHighSchoolCommunications = New System.Windows.Forms.ToolStripButton()
    	Me.btnJuniorHighSchoolCommuncation = New System.Windows.Forms.ToolStripButton()
    	Me.btnMiscCommunication = New System.Windows.Forms.ToolStripButton()
    	Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
    	Me.btnReports = New System.Windows.Forms.ToolStripButton()
    	Me.btnTransactionAuditHistory = New System.Windows.Forms.ToolStripButton()
    	Me.btnWebPasswordReset = New System.Windows.Forms.ToolStripButton()
    	Me.btnQuickBatchReview = New System.Windows.Forms.ToolStripButton()
    	Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
    	Me.btnExit = New System.Windows.Forms.ToolStripButton()
    	Me.lblHeaderStudentName = New System.Windows.Forms.Label()
    	Me.txtHeaderStudentName = New System.Windows.Forms.TextBox()
    	Me.pnlStudentInfo = New System.Windows.Forms.Panel()
    	Me.lblHeaderStudentId = New System.Windows.Forms.Label()
    	Me.txtHeaderStudentId = New System.Windows.Forms.TextBox()
    	Me.lblHeaderAwardStatus = New System.Windows.Forms.Label()
    	Me.txtHeaderAwardStatus = New System.Windows.Forms.TextBox()
    	Me.tabControlMaster.SuspendLayout
    	Me.tabMainMenu.SuspendLayout
    	Me.grpMainMenuStudentAccountSearch.SuspendLayout
    	CType(Me.dgvMainMenuSearchMatches,System.ComponentModel.ISupportInitialize).BeginInit
    	CType(Me.MainMenuSearchResultBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.tabDemographics.SuspendLayout
    	Me.grpDemographics411.SuspendLayout
    	Me.grpHearAboutUs.SuspendLayout
    	CType(Me.ScholarshipApplicationBindingSource1,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.grpAuthThirdParties.SuspendLayout
    	Me.Panel1.SuspendLayout
    	CType(Me.StudentBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.grpDemographicsEligibility.SuspendLayout
    	Me.grpDemographicsContact.SuspendLayout
    	CType(Me.SchoolEmailBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	CType(Me.PersonalEmailBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	CType(Me.CellPhoneBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	CType(Me.PrimaryPhoneBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.grpDemographicsAddress.SuspendLayout
    	CType(Me.MailingAddressBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.grpDemographicsPersonal.SuspendLayout
    	Me.tabApplication.SuspendLayout
    	Me.GroupBox3.SuspendLayout
    	CType(Me.CollegeBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.GroupBox2.SuspendLayout
    	Me.grpApplicationCollegeEnrollment.SuspendLayout
    	Me.grpApplicationDocumentStatus.SuspendLayout
    	Me.grpApplicationReviewStatus.SuspendLayout
    	Me.grpApplicationClasses.SuspendLayout
    	Me.tabControlClasses.SuspendLayout
    	Me.tabEnglish.SuspendLayout
    	Me.tabMathematics.SuspendLayout
    	Me.tabScienceWithLab.SuspendLayout
    	Me.tabSocialScience.SuspendLayout
    	Me.tabForeignLanguage.SuspendLayout
    	Me.grpApplicationAwardStatus.SuspendLayout
    	CType(Me.PrimaryAwardBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.grpApplicationDenialReasons.SuspendLayout
    	CType(Me.ExemplaryAwardBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	CType(Me.UespSupplementalAwardBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.grpApplicationHighSchool.SuspendLayout
    	CType(Me.HighSchoolBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.tabCommunications.SuspendLayout
    	CType(Me.CommunicationDataGridView,System.ComponentModel.ISupportInitialize).BeginInit
    	CType(Me.CommunicationBindingSource,System.ComponentModel.ISupportInitialize).BeginInit
    	Me.grpCommunicationRecord.SuspendLayout
    	Me.tabPayments.SuspendLayout
    	Me.pnlPayments.SuspendLayout
    	Me.tabPaymentBatch.SuspendLayout
        Me.splPaymentBatchSplitter.Panel1.SuspendLayout()
    	Me.splPaymentBatchSplitter.Panel2.SuspendLayout
    	Me.splPaymentBatchSplitter.SuspendLayout
    	Me.ToolStrip1.SuspendLayout
    	Me.pnlStudentInfo.SuspendLayout
    	Me.SuspendLayout
    	'
    	'tabControlMaster
    	'
    	Me.tabControlMaster.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
    	    	    	Or System.Windows.Forms.AnchorStyles.Left)  _
    	    	    	Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.tabControlMaster.Controls.Add(Me.tabMainMenu)
    	Me.tabControlMaster.Controls.Add(Me.tabDemographics)
    	Me.tabControlMaster.Controls.Add(Me.tabApplication)
    	Me.tabControlMaster.Controls.Add(Me.tabCommunications)
    	Me.tabControlMaster.Controls.Add(Me.tabPayments)
    	Me.tabControlMaster.Controls.Add(Me.tabPaymentBatch)
    	Me.tabControlMaster.Location = New System.Drawing.Point(0, 63)
    	Me.tabControlMaster.Name = "tabControlMaster"
    	Me.tabControlMaster.SelectedIndex = 0
    	Me.tabControlMaster.Size = New System.Drawing.Size(941, 749)
    	Me.tabControlMaster.TabIndex = 0
    	'
    	'tabMainMenu
    	'
    	Me.tabMainMenu.Controls.Add(Me.grpMainMenuStudentAccountSearch)
    	Me.tabMainMenu.Controls.Add(Me.dgvMainMenuSearchMatches)
    	Me.tabMainMenu.Location = New System.Drawing.Point(4, 22)
    	Me.tabMainMenu.Name = "tabMainMenu"
    	Me.tabMainMenu.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabMainMenu.Size = New System.Drawing.Size(933, 723)
    	Me.tabMainMenu.TabIndex = 0
    	Me.tabMainMenu.Text = "Main Menu"
    	Me.tabMainMenu.UseVisualStyleBackColor = true
    	'
    	'grpMainMenuStudentAccountSearch
    	'
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.txtMainMenuSearchDateOfBirth)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.lblMainMenuSelectMatch)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.btnMainMenuSearch)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.lblMainMenuNoSearchResults)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.txtMainMenuSearchLastName)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.txtMainMenuSearchFirstName)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.txtMainMenuSearchSsn)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.lblMainMenuSearchDateOfBirth)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.lblMainMenuSearchLastName)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.lblMainMenuSearchFirstName)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.lblMainMenuSearchSsn)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.txtMainMenuSearchStateStudentId)
    	Me.grpMainMenuStudentAccountSearch.Controls.Add(Me.lblMainMenuSearchStateStudentId)
    	Me.grpMainMenuStudentAccountSearch.Location = New System.Drawing.Point(6, 6)
    	Me.grpMainMenuStudentAccountSearch.Name = "grpMainMenuStudentAccountSearch"
    	Me.grpMainMenuStudentAccountSearch.Size = New System.Drawing.Size(913, 142)
    	Me.grpMainMenuStudentAccountSearch.TabIndex = 0
    	Me.grpMainMenuStudentAccountSearch.TabStop = false
    	Me.grpMainMenuStudentAccountSearch.Text = "Student Account Search"
    	'
    	'txtMainMenuSearchDateOfBirth
    	'
    	Me.txtMainMenuSearchDateOfBirth.Location = New System.Drawing.Point(79, 69)
    	Me.txtMainMenuSearchDateOfBirth.Mask = "00/00/0000"
    	Me.txtMainMenuSearchDateOfBirth.Name = "txtMainMenuSearchDateOfBirth"
    	Me.txtMainMenuSearchDateOfBirth.Size = New System.Drawing.Size(100, 20)
    	Me.txtMainMenuSearchDateOfBirth.TabIndex = 5
    	Me.txtMainMenuSearchDateOfBirth.ValidatingType = GetType(Date)
    	'
    	'lblMainMenuSelectMatch
    	'
    	Me.lblMainMenuSelectMatch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.lblMainMenuSelectMatch.BackColor = System.Drawing.Color.Yellow
    	Me.lblMainMenuSelectMatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.lblMainMenuSelectMatch.ForeColor = System.Drawing.Color.Brown
    	Me.lblMainMenuSelectMatch.Location = New System.Drawing.Point(300, 102)
    	Me.lblMainMenuSelectMatch.Name = "lblMainMenuSelectMatch"
    	Me.lblMainMenuSelectMatch.Size = New System.Drawing.Size(286, 33)
    	Me.lblMainMenuSelectMatch.TabIndex = 12
    	Me.lblMainMenuSelectMatch.Text = "More than one student matches the search criteria. Double-click the correct stude"& _ 
    	"nt from the list below."
    	Me.lblMainMenuSelectMatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    	Me.lblMainMenuSelectMatch.Visible = false
    	'
    	'btnMainMenuSearch
    	'
    	Me.btnMainMenuSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnMainMenuSearch.Location = New System.Drawing.Point(10, 104)
    	Me.btnMainMenuSearch.Name = "btnMainMenuSearch"
    	Me.btnMainMenuSearch.Size = New System.Drawing.Size(277, 28)
    	Me.btnMainMenuSearch.TabIndex = 6
    	Me.btnMainMenuSearch.Text = "Search"
    	Me.btnMainMenuSearch.UseVisualStyleBackColor = true
    	'
    	'lblMainMenuNoSearchResults
    	'
    	Me.lblMainMenuNoSearchResults.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.lblMainMenuNoSearchResults.BackColor = System.Drawing.Color.Yellow
    	Me.lblMainMenuNoSearchResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.lblMainMenuNoSearchResults.ForeColor = System.Drawing.Color.Red
    	Me.lblMainMenuNoSearchResults.Location = New System.Drawing.Point(455, 107)
    	Me.lblMainMenuNoSearchResults.Name = "lblMainMenuNoSearchResults"
    	Me.lblMainMenuNoSearchResults.Size = New System.Drawing.Size(135, 23)
    	Me.lblMainMenuNoSearchResults.TabIndex = 13
    	Me.lblMainMenuNoSearchResults.Text = "No students found."
    	Me.lblMainMenuNoSearchResults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    	Me.lblMainMenuNoSearchResults.Visible = false
    	'
    	'txtMainMenuSearchLastName
    	'
    	Me.txtMainMenuSearchLastName.Location = New System.Drawing.Point(376, 43)
    	Me.txtMainMenuSearchLastName.MaxLength = 25
    	Me.txtMainMenuSearchLastName.Name = "txtMainMenuSearchLastName"
    	Me.txtMainMenuSearchLastName.Size = New System.Drawing.Size(157, 20)
    	Me.txtMainMenuSearchLastName.TabIndex = 4
    	'
    	'txtMainMenuSearchFirstName
    	'
    	Me.txtMainMenuSearchFirstName.Location = New System.Drawing.Point(376, 17)
    	Me.txtMainMenuSearchFirstName.MaxLength = 20
    	Me.txtMainMenuSearchFirstName.Name = "txtMainMenuSearchFirstName"
    	Me.txtMainMenuSearchFirstName.Size = New System.Drawing.Size(157, 20)
    	Me.txtMainMenuSearchFirstName.TabIndex = 3
    	'
    	'txtMainMenuSearchSsn
    	'
    	Me.txtMainMenuSearchSsn.Location = New System.Drawing.Point(130, 43)
    	Me.txtMainMenuSearchSsn.MaxLength = 11
    	Me.txtMainMenuSearchSsn.Name = "txtMainMenuSearchSsn"
    	Me.txtMainMenuSearchSsn.Size = New System.Drawing.Size(157, 20)
    	Me.txtMainMenuSearchSsn.TabIndex = 2
    	'
    	'lblMainMenuSearchDateOfBirth
    	'
    	Me.lblMainMenuSearchDateOfBirth.AutoSize = true
    	Me.lblMainMenuSearchDateOfBirth.Location = New System.Drawing.Point(7, 72)
    	Me.lblMainMenuSearchDateOfBirth.Name = "lblMainMenuSearchDateOfBirth"
    	Me.lblMainMenuSearchDateOfBirth.Size = New System.Drawing.Size(66, 13)
    	Me.lblMainMenuSearchDateOfBirth.TabIndex = 5
    	Me.lblMainMenuSearchDateOfBirth.Text = "Date of Birth"
    	'
    	'lblMainMenuSearchLastName
    	'
    	Me.lblMainMenuSearchLastName.AutoSize = true
    	Me.lblMainMenuSearchLastName.Location = New System.Drawing.Point(300, 46)
    	Me.lblMainMenuSearchLastName.Name = "lblMainMenuSearchLastName"
    	Me.lblMainMenuSearchLastName.Size = New System.Drawing.Size(58, 13)
    	Me.lblMainMenuSearchLastName.TabIndex = 4
    	Me.lblMainMenuSearchLastName.Text = "Last Name"
    	'
    	'lblMainMenuSearchFirstName
    	'
    	Me.lblMainMenuSearchFirstName.AutoSize = true
    	Me.lblMainMenuSearchFirstName.Location = New System.Drawing.Point(300, 20)
    	Me.lblMainMenuSearchFirstName.Name = "lblMainMenuSearchFirstName"
    	Me.lblMainMenuSearchFirstName.Size = New System.Drawing.Size(57, 13)
    	Me.lblMainMenuSearchFirstName.TabIndex = 3
    	Me.lblMainMenuSearchFirstName.Text = "First Name"
    	'
    	'lblMainMenuSearchSsn
    	'
    	Me.lblMainMenuSearchSsn.AutoSize = true
    	Me.lblMainMenuSearchSsn.Location = New System.Drawing.Point(7, 46)
    	Me.lblMainMenuSearchSsn.Name = "lblMainMenuSearchSsn"
    	Me.lblMainMenuSearchSsn.Size = New System.Drawing.Size(117, 13)
    	Me.lblMainMenuSearchSsn.TabIndex = 2
    	Me.lblMainMenuSearchSsn.Text = "Social Security Number"
    	'
    	'txtMainMenuSearchStateStudentId
    	'
    	Me.txtMainMenuSearchStateStudentId.Location = New System.Drawing.Point(130, 17)
    	Me.txtMainMenuSearchStateStudentId.MaxLength = 10
    	Me.txtMainMenuSearchStateStudentId.Name = "txtMainMenuSearchStateStudentId"
    	Me.txtMainMenuSearchStateStudentId.Size = New System.Drawing.Size(157, 20)
    	Me.txtMainMenuSearchStateStudentId.TabIndex = 1
    	'
    	'lblMainMenuSearchStateStudentId
    	'
    	Me.lblMainMenuSearchStateStudentId.AutoSize = true
    	Me.lblMainMenuSearchStateStudentId.Location = New System.Drawing.Point(7, 20)
    	Me.lblMainMenuSearchStateStudentId.Name = "lblMainMenuSearchStateStudentId"
    	Me.lblMainMenuSearchStateStudentId.Size = New System.Drawing.Size(86, 13)
    	Me.lblMainMenuSearchStateStudentId.TabIndex = 0
    	Me.lblMainMenuSearchStateStudentId.Text = "State Student ID"
    	'
    	'dgvMainMenuSearchMatches
    	'
    	Me.dgvMainMenuSearchMatches.AllowUserToAddRows = false
    	Me.dgvMainMenuSearchMatches.AllowUserToDeleteRows = false
    	Me.dgvMainMenuSearchMatches.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
    	    	    	Or System.Windows.Forms.AnchorStyles.Left)  _
    	    	    	Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.dgvMainMenuSearchMatches.AutoGenerateColumns = false
    	Me.dgvMainMenuSearchMatches.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
    	dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    	dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
    	dataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
    	dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
    	dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    	dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
    	Me.dgvMainMenuSearchMatches.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1
    	Me.dgvMainMenuSearchMatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
    	Me.dgvMainMenuSearchMatches.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FirstName, Me.LastName, Me.StateStudentID, Me.SocialSecurityNumber, Me.StreetAddressLine1, Me.City, Me.AwardStatus})
    	Me.dgvMainMenuSearchMatches.DataSource = Me.MainMenuSearchResultBindingSource
    	dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    	dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
    	dataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
    	dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
    	dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    	dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
    	Me.dgvMainMenuSearchMatches.DefaultCellStyle = dataGridViewCellStyle2
    	Me.dgvMainMenuSearchMatches.Location = New System.Drawing.Point(6, 154)
    	Me.dgvMainMenuSearchMatches.MultiSelect = false
    	Me.dgvMainMenuSearchMatches.Name = "dgvMainMenuSearchMatches"
    	Me.dgvMainMenuSearchMatches.ReadOnly = true
    	dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    	dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
    	dataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
    	dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
    	dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    	dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
    	Me.dgvMainMenuSearchMatches.RowHeadersDefaultCellStyle = dataGridViewCellStyle3
    	Me.dgvMainMenuSearchMatches.RowHeadersVisible = false
    	Me.dgvMainMenuSearchMatches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
    	Me.dgvMainMenuSearchMatches.Size = New System.Drawing.Size(919, 563)
    	Me.dgvMainMenuSearchMatches.TabIndex = 7
    	'
    	'FirstName
    	'
    	Me.FirstName.DataPropertyName = "FirstName"
    	Me.FirstName.HeaderText = "First Name"
    	Me.FirstName.Name = "FirstName"
    	Me.FirstName.ReadOnly = true
    	Me.FirstName.Width = 76
    	'
    	'LastName
    	'
    	Me.LastName.DataPropertyName = "LastName"
    	Me.LastName.HeaderText = "Last Name"
    	Me.LastName.Name = "LastName"
    	Me.LastName.ReadOnly = true
    	Me.LastName.Width = 77
    	'
    	'StateStudentID
    	'
    	Me.StateStudentID.DataPropertyName = "StateStudentID"
    	Me.StateStudentID.HeaderText = "State Student ID"
    	Me.StateStudentID.Name = "StateStudentID"
    	Me.StateStudentID.ReadOnly = true
    	Me.StateStudentID.Width = 92
    	'
    	'SocialSecurityNumber
    	'
    	Me.SocialSecurityNumber.DataPropertyName = "SocialSecurityNumber"
    	Me.SocialSecurityNumber.HeaderText = "Social Security Number"
    	Me.SocialSecurityNumber.Name = "SocialSecurityNumber"
    	Me.SocialSecurityNumber.ReadOnly = true
    	Me.SocialSecurityNumber.Width = 130
    	'
    	'StreetAddressLine1
    	'
    	Me.StreetAddressLine1.DataPropertyName = "StreetAddressLine1"
    	Me.StreetAddressLine1.HeaderText = "Street Address Line 1"
    	Me.StreetAddressLine1.Name = "StreetAddressLine1"
    	Me.StreetAddressLine1.ReadOnly = true
    	Me.StreetAddressLine1.Width = 122
    	'
    	'City
    	'
    	Me.City.DataPropertyName = "City"
    	Me.City.HeaderText = "City"
    	Me.City.Name = "City"
    	Me.City.ReadOnly = true
    	Me.City.Width = 49
    	'
    	'AwardStatus
    	'
    	Me.AwardStatus.DataPropertyName = "AwardStatus"
    	Me.AwardStatus.HeaderText = "Award Status"
    	Me.AwardStatus.Name = "AwardStatus"
    	Me.AwardStatus.ReadOnly = true
    	Me.AwardStatus.Width = 88
    	'
    	'MainMenuSearchResultBindingSource
    	'
    	Me.MainMenuSearchResultBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.MainMenuSearchResult)
    	'
    	'tabDemographics
    	'
    	Me.tabDemographics.Controls.Add(Me.grpDemographics411)
    	Me.tabDemographics.Controls.Add(Me.grpHearAboutUs)
    	Me.tabDemographics.Controls.Add(Me.grpAuthThirdParties)
    	Me.tabDemographics.Controls.Add(Me.txtDemographicsSsn)
    	Me.tabDemographics.Controls.Add(Me.lblDemographicsSsn)
    	Me.tabDemographics.Controls.Add(Me.txtDemographicsStateStudentId)
    	Me.tabDemographics.Controls.Add(Me.lblDemographicsStateStudentId)
    	Me.tabDemographics.Controls.Add(Me.btnDemographicsSaveChanges)
    	Me.tabDemographics.Controls.Add(Me.grpDemographicsEligibility)
    	Me.tabDemographics.Controls.Add(Me.grpDemographicsContact)
    	Me.tabDemographics.Controls.Add(Me.grpDemographicsAddress)
    	Me.tabDemographics.Controls.Add(Me.grpDemographicsPersonal)
    	Me.tabDemographics.Location = New System.Drawing.Point(4, 22)
    	Me.tabDemographics.Name = "tabDemographics"
    	Me.tabDemographics.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabDemographics.Size = New System.Drawing.Size(933, 723)
    	Me.tabDemographics.TabIndex = 1
    	Me.tabDemographics.Text = "Demographics"
    	Me.tabDemographics.UseVisualStyleBackColor = true
    	'
    	'grpDemographics411
    	'
    	Me.grpDemographics411.Controls.Add(Me.pnlDemographics411)
    	Me.grpDemographics411.Dock = System.Windows.Forms.DockStyle.Right
    	Me.grpDemographics411.Location = New System.Drawing.Point(631, 3)
    	Me.grpDemographics411.Name = "grpDemographics411"
    	Me.grpDemographics411.Size = New System.Drawing.Size(299, 717)
    	Me.grpDemographics411.TabIndex = 26
    	Me.grpDemographics411.TabStop = false
    	Me.grpDemographics411.Text = "The 411"
    	'
    	'pnlDemographics411
    	'
    	Me.pnlDemographics411.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.pnlDemographics411.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
    	Me.pnlDemographics411.Location = New System.Drawing.Point(3, 16)
    	Me.pnlDemographics411.Name = "pnlDemographics411"
    	Me.pnlDemographics411.Size = New System.Drawing.Size(293, 698)
    	Me.pnlDemographics411.TabIndex = 25
    	'
    	'grpHearAboutUs
    	'
    	Me.grpHearAboutUs.Controls.Add(Me.txtHowDidTheyHearAboutRegents)
    	Me.grpHearAboutUs.Controls.Add(Me.Label2)
    	Me.grpHearAboutUs.Location = New System.Drawing.Point(12, 467)
    	Me.grpHearAboutUs.Name = "grpHearAboutUs"
    	Me.grpHearAboutUs.Size = New System.Drawing.Size(613, 79)
    	Me.grpHearAboutUs.TabIndex = 23
    	Me.grpHearAboutUs.TabStop = false
    	Me.grpHearAboutUs.Text = "How Did They Hear About Us"
    	'
    	'txtHowDidTheyHearAboutRegents
    	'
    	Me.txtHowDidTheyHearAboutRegents.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ScholarshipApplicationBindingSource1, "HowTheyHeardAboutRegents", true))
    	Me.txtHowDidTheyHearAboutRegents.Location = New System.Drawing.Point(9, 19)
    	Me.txtHowDidTheyHearAboutRegents.Multiline = true
    	Me.txtHowDidTheyHearAboutRegents.Name = "txtHowDidTheyHearAboutRegents"
    	Me.txtHowDidTheyHearAboutRegents.ReadOnly = true
    	Me.txtHowDidTheyHearAboutRegents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    	Me.txtHowDidTheyHearAboutRegents.Size = New System.Drawing.Size(598, 54)
    	Me.txtHowDidTheyHearAboutRegents.TabIndex = 7
    	'
    	'ScholarshipApplicationBindingSource1
    	'
    	Me.ScholarshipApplicationBindingSource1.DataSource = GetType(RegentsScholarshipBackEnd.ScholarshipApplication)
    	'
    	'Label2
    	'
    	Me.Label2.AutoSize = true
    	Me.Label2.Location = New System.Drawing.Point(6, 63)
    	Me.Label2.Name = "Label2"
    	Me.Label2.Size = New System.Drawing.Size(0, 13)
    	Me.Label2.TabIndex = 6
    	'
    	'grpAuthThirdParties
    	'
    	Me.grpAuthThirdParties.Controls.Add(Me.Panel1)
    	Me.grpAuthThirdParties.Location = New System.Drawing.Point(12, 552)
    	Me.grpAuthThirdParties.Name = "grpAuthThirdParties"
    	Me.grpAuthThirdParties.Size = New System.Drawing.Size(613, 165)
    	Me.grpAuthThirdParties.TabIndex = 22
    	Me.grpAuthThirdParties.TabStop = false
    	Me.grpAuthThirdParties.Text = "Authorized Third Parties"
    	'
    	'Panel1
    	'
    	Me.Panel1.AutoScroll = true
    	Me.Panel1.Controls.Add(Me.AuthorizedThirdParty2)
    	Me.Panel1.Controls.Add(Me.AuthorizedThirdParty1)
    	Me.Panel1.Location = New System.Drawing.Point(6, 12)
    	Me.Panel1.Name = "Panel1"
    	Me.Panel1.Size = New System.Drawing.Size(601, 142)
    	Me.Panel1.TabIndex = 21
    	'
    	'AuthorizedThirdParty2
    	'
    	Me.AuthorizedThirdParty2.AuthorizedThirdPartyData = Nothing
    	Me.AuthorizedThirdParty2.Location = New System.Drawing.Point(3, 160)
    	Me.AuthorizedThirdParty2.Name = "AuthorizedThirdParty2"
    	Me.AuthorizedThirdParty2.Size = New System.Drawing.Size(435, 147)
    	Me.AuthorizedThirdParty2.TabIndex = 1
    	'
    	'AuthorizedThirdParty1
    	'
    	Me.AuthorizedThirdParty1.AuthorizedThirdPartyData = Nothing
    	Me.AuthorizedThirdParty1.Location = New System.Drawing.Point(3, 7)
    	Me.AuthorizedThirdParty1.Name = "AuthorizedThirdParty1"
    	Me.AuthorizedThirdParty1.Size = New System.Drawing.Size(435, 147)
    	Me.AuthorizedThirdParty1.TabIndex = 0
    	'
    	'txtDemographicsSsn
    	'
    	Me.txtDemographicsSsn.BackColor = System.Drawing.SystemColors.Window
    	Me.txtDemographicsSsn.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.StudentBindingSource, "SocialSecurityNumber", true))
    	Me.txtDemographicsSsn.Location = New System.Drawing.Point(297, 9)
    	Me.txtDemographicsSsn.MaxLength = 11
    	Me.txtDemographicsSsn.Name = "txtDemographicsSsn"
    	Me.txtDemographicsSsn.Size = New System.Drawing.Size(121, 20)
    	Me.txtDemographicsSsn.TabIndex = 2
    	'
    	'StudentBindingSource
    	'
    	Me.StudentBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.Student)
    	'
    	'lblDemographicsSsn
    	'
    	Me.lblDemographicsSsn.AutoSize = true
    	Me.lblDemographicsSsn.Location = New System.Drawing.Point(262, 12)
    	Me.lblDemographicsSsn.Name = "lblDemographicsSsn"
    	Me.lblDemographicsSsn.Size = New System.Drawing.Size(29, 13)
    	Me.lblDemographicsSsn.TabIndex = 20
    	Me.lblDemographicsSsn.Text = "SSN"
    	'
    	'txtDemographicsStateStudentId
    	'
    	Me.txtDemographicsStateStudentId.BackColor = System.Drawing.Color.WhiteSmoke
    	Me.txtDemographicsStateStudentId.Location = New System.Drawing.Point(110, 9)
    	Me.txtDemographicsStateStudentId.MaxLength = 10
    	Me.txtDemographicsStateStudentId.Name = "txtDemographicsStateStudentId"
    	Me.txtDemographicsStateStudentId.ReadOnly = true
    	Me.txtDemographicsStateStudentId.Size = New System.Drawing.Size(121, 20)
    	Me.txtDemographicsStateStudentId.TabIndex = 1
    	Me.txtDemographicsStateStudentId.TabStop = false
    	'
    	'lblDemographicsStateStudentId
    	'
    	Me.lblDemographicsStateStudentId.AutoSize = true
    	Me.lblDemographicsStateStudentId.Location = New System.Drawing.Point(18, 12)
    	Me.lblDemographicsStateStudentId.Name = "lblDemographicsStateStudentId"
    	Me.lblDemographicsStateStudentId.Size = New System.Drawing.Size(86, 13)
    	Me.lblDemographicsStateStudentId.TabIndex = 0
    	Me.lblDemographicsStateStudentId.Text = "State Student ID"
    	'
    	'btnDemographicsSaveChanges
    	'
    	Me.btnDemographicsSaveChanges.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnDemographicsSaveChanges.Location = New System.Drawing.Point(485, 9)
    	Me.btnDemographicsSaveChanges.Name = "btnDemographicsSaveChanges"
    	Me.btnDemographicsSaveChanges.Size = New System.Drawing.Size(140, 26)
    	Me.btnDemographicsSaveChanges.TabIndex = 7
    	Me.btnDemographicsSaveChanges.Text = "Save Changes"
    	Me.btnDemographicsSaveChanges.UseVisualStyleBackColor = true
    	'
    	'grpDemographicsEligibility
    	'
    	Me.grpDemographicsEligibility.Controls.Add(Me.chkDemographicsIntendsToApplyForFederalAid)
    	Me.grpDemographicsEligibility.Controls.Add(Me.chkDemographicsCriminalRecord)
    	Me.grpDemographicsEligibility.Controls.Add(Me.chkDemographicsEligibleForFederalAid)
    	Me.grpDemographicsEligibility.Controls.Add(Me.chkDemographicsUsCitizen)
    	Me.grpDemographicsEligibility.Location = New System.Drawing.Point(12, 412)
    	Me.grpDemographicsEligibility.Name = "grpDemographicsEligibility"
    	Me.grpDemographicsEligibility.Size = New System.Drawing.Size(613, 48)
    	Me.grpDemographicsEligibility.TabIndex = 6
    	Me.grpDemographicsEligibility.TabStop = false
    	Me.grpDemographicsEligibility.Text = "Eligibility Information"
    	'
    	'chkDemographicsIntendsToApplyForFederalAid
    	'
    	Me.chkDemographicsIntendsToApplyForFederalAid.AutoSize = true
    	Me.chkDemographicsIntendsToApplyForFederalAid.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.StudentBindingSource, "IntendsToApplyForFederalAid", true))
    	Me.chkDemographicsIntendsToApplyForFederalAid.Location = New System.Drawing.Point(389, 24)
    	Me.chkDemographicsIntendsToApplyForFederalAid.Name = "chkDemographicsIntendsToApplyForFederalAid"
    	Me.chkDemographicsIntendsToApplyForFederalAid.Size = New System.Drawing.Size(173, 17)
    	Me.chkDemographicsIntendsToApplyForFederalAid.TabIndex = 28
    	Me.chkDemographicsIntendsToApplyForFederalAid.Text = "Intends to Apply for Federal Aid"
    	Me.chkDemographicsIntendsToApplyForFederalAid.UseVisualStyleBackColor = true
    	'
    	'chkDemographicsCriminalRecord
    	'
    	Me.chkDemographicsCriminalRecord.AutoSize = true
    	Me.chkDemographicsCriminalRecord.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.StudentBindingSource, "HasCriminalRecord", true))
    	Me.chkDemographicsCriminalRecord.Location = New System.Drawing.Point(9, 24)
    	Me.chkDemographicsCriminalRecord.Name = "chkDemographicsCriminalRecord"
    	Me.chkDemographicsCriminalRecord.Size = New System.Drawing.Size(100, 17)
    	Me.chkDemographicsCriminalRecord.TabIndex = 25
    	Me.chkDemographicsCriminalRecord.Text = "Criminal Record"
    	Me.chkDemographicsCriminalRecord.UseVisualStyleBackColor = true
    	'
    	'chkDemographicsEligibleForFederalAid
    	'
    	Me.chkDemographicsEligibleForFederalAid.AutoSize = true
    	Me.chkDemographicsEligibleForFederalAid.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.StudentBindingSource, "IsEligibleForFederalAid", true))
    	Me.chkDemographicsEligibleForFederalAid.Location = New System.Drawing.Point(236, 24)
    	Me.chkDemographicsEligibleForFederalAid.Name = "chkDemographicsEligibleForFederalAid"
    	Me.chkDemographicsEligibleForFederalAid.Size = New System.Drawing.Size(127, 17)
    	Me.chkDemographicsEligibleForFederalAid.TabIndex = 27
    	Me.chkDemographicsEligibleForFederalAid.Text = "Eligible for FederalAid"
    	Me.chkDemographicsEligibleForFederalAid.UseVisualStyleBackColor = true
    	'
    	'chkDemographicsUsCitizen
    	'
    	Me.chkDemographicsUsCitizen.AutoSize = true
    	Me.chkDemographicsUsCitizen.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.StudentBindingSource, "IsUsCitizen", true))
    	Me.chkDemographicsUsCitizen.Location = New System.Drawing.Point(135, 24)
    	Me.chkDemographicsUsCitizen.Name = "chkDemographicsUsCitizen"
    	Me.chkDemographicsUsCitizen.Size = New System.Drawing.Size(75, 17)
    	Me.chkDemographicsUsCitizen.TabIndex = 26
    	Me.chkDemographicsUsCitizen.Text = "US Citizen"
    	Me.chkDemographicsUsCitizen.UseVisualStyleBackColor = true
    	'
    	'grpDemographicsContact
    	'
    	Me.grpDemographicsContact.Controls.Add(Me.chkDemographicsValidSchoolEmail)
    	Me.grpDemographicsContact.Controls.Add(Me.chkDemographicsValidPersonalEmail)
    	Me.grpDemographicsContact.Controls.Add(Me.chkDemographicsValidCellPhone)
    	Me.grpDemographicsContact.Controls.Add(Me.chkDemographicsValidPrimaryPhone)
    	Me.grpDemographicsContact.Controls.Add(Me.txtDemographicsSchoolEmail)
    	Me.grpDemographicsContact.Controls.Add(Me.txtDemographicsPersonalEmail)
    	Me.grpDemographicsContact.Controls.Add(Me.txtDemographicsCellPhone)
    	Me.grpDemographicsContact.Controls.Add(Me.txtDemographicsPrimaryPhone)
    	Me.grpDemographicsContact.Controls.Add(Me.lblDemographicsSchoolEmail)
    	Me.grpDemographicsContact.Controls.Add(Me.lblDemographicsPersonalEmail)
    	Me.grpDemographicsContact.Controls.Add(Me.lblDemographicsCellPhone)
    	Me.grpDemographicsContact.Controls.Add(Me.lblDemographicsPrimaryPhone)
    	Me.grpDemographicsContact.Location = New System.Drawing.Point(12, 283)
    	Me.grpDemographicsContact.Name = "grpDemographicsContact"
    	Me.grpDemographicsContact.Size = New System.Drawing.Size(613, 123)
    	Me.grpDemographicsContact.TabIndex = 5
    	Me.grpDemographicsContact.TabStop = false
    	Me.grpDemographicsContact.Text = "Contact Information"
    	'
    	'chkDemographicsValidSchoolEmail
    	'
    	Me.chkDemographicsValidSchoolEmail.AutoSize = true
    	Me.chkDemographicsValidSchoolEmail.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.SchoolEmailBindingSource, "IsValid", true))
    	Me.chkDemographicsValidSchoolEmail.Location = New System.Drawing.Point(473, 97)
    	Me.chkDemographicsValidSchoolEmail.Name = "chkDemographicsValidSchoolEmail"
    	Me.chkDemographicsValidSchoolEmail.Size = New System.Drawing.Size(116, 17)
    	Me.chkDemographicsValidSchoolEmail.TabIndex = 24
    	Me.chkDemographicsValidSchoolEmail.Text = "Valid School E-mail"
    	Me.chkDemographicsValidSchoolEmail.UseVisualStyleBackColor = true
    	'
    	'SchoolEmailBindingSource
    	'
    	Me.SchoolEmailBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.Email)
    	'
    	'chkDemographicsValidPersonalEmail
    	'
    	Me.chkDemographicsValidPersonalEmail.AutoSize = true
    	Me.chkDemographicsValidPersonalEmail.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.PersonalEmailBindingSource, "IsValid", true))
    	Me.chkDemographicsValidPersonalEmail.Location = New System.Drawing.Point(473, 71)
    	Me.chkDemographicsValidPersonalEmail.Name = "chkDemographicsValidPersonalEmail"
    	Me.chkDemographicsValidPersonalEmail.Size = New System.Drawing.Size(124, 17)
    	Me.chkDemographicsValidPersonalEmail.TabIndex = 22
    	Me.chkDemographicsValidPersonalEmail.Text = "Valid Personal E-mail"
    	Me.chkDemographicsValidPersonalEmail.UseVisualStyleBackColor = true
    	'
    	'PersonalEmailBindingSource
    	'
    	Me.PersonalEmailBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.Email)
    	'
    	'chkDemographicsValidCellPhone
    	'
    	Me.chkDemographicsValidCellPhone.AutoSize = true
    	Me.chkDemographicsValidCellPhone.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.CellPhoneBindingSource, "IsValid", true))
    	Me.chkDemographicsValidCellPhone.Location = New System.Drawing.Point(285, 45)
    	Me.chkDemographicsValidCellPhone.Name = "chkDemographicsValidCellPhone"
    	Me.chkDemographicsValidCellPhone.Size = New System.Drawing.Size(103, 17)
    	Me.chkDemographicsValidCellPhone.TabIndex = 20
    	Me.chkDemographicsValidCellPhone.Text = "Valid Cell Phone"
    	Me.chkDemographicsValidCellPhone.UseVisualStyleBackColor = true
    	'
    	'CellPhoneBindingSource
    	'
    	Me.CellPhoneBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.Phone)
    	'
    	'chkDemographicsValidPrimaryPhone
    	'
    	Me.chkDemographicsValidPrimaryPhone.AutoSize = true
    	Me.chkDemographicsValidPrimaryPhone.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.PrimaryPhoneBindingSource, "IsValid", true))
    	Me.chkDemographicsValidPrimaryPhone.Location = New System.Drawing.Point(285, 19)
    	Me.chkDemographicsValidPrimaryPhone.Name = "chkDemographicsValidPrimaryPhone"
    	Me.chkDemographicsValidPrimaryPhone.Size = New System.Drawing.Size(120, 17)
    	Me.chkDemographicsValidPrimaryPhone.TabIndex = 18
    	Me.chkDemographicsValidPrimaryPhone.Text = "Valid Primary Phone"
    	Me.chkDemographicsValidPrimaryPhone.UseVisualStyleBackColor = true
    	'
    	'PrimaryPhoneBindingSource
    	'
    	Me.PrimaryPhoneBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.Phone)
    	'
    	'txtDemographicsSchoolEmail
    	'
    	Me.txtDemographicsSchoolEmail.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.SchoolEmailBindingSource, "Address", true))
    	Me.txtDemographicsSchoolEmail.Location = New System.Drawing.Point(91, 95)
    	Me.txtDemographicsSchoolEmail.MaxLength = 56
    	Me.txtDemographicsSchoolEmail.Name = "txtDemographicsSchoolEmail"
    	Me.txtDemographicsSchoolEmail.Size = New System.Drawing.Size(376, 20)
    	Me.txtDemographicsSchoolEmail.TabIndex = 23
    	'
    	'txtDemographicsPersonalEmail
    	'
    	Me.txtDemographicsPersonalEmail.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PersonalEmailBindingSource, "Address", true))
    	Me.txtDemographicsPersonalEmail.Location = New System.Drawing.Point(91, 69)
    	Me.txtDemographicsPersonalEmail.MaxLength = 56
    	Me.txtDemographicsPersonalEmail.Name = "txtDemographicsPersonalEmail"
    	Me.txtDemographicsPersonalEmail.Size = New System.Drawing.Size(376, 20)
    	Me.txtDemographicsPersonalEmail.TabIndex = 21
    	'
    	'txtDemographicsCellPhone
    	'
    	Me.txtDemographicsCellPhone.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CellPhoneBindingSource, "Number", true))
    	Me.txtDemographicsCellPhone.Location = New System.Drawing.Point(91, 43)
    	Me.txtDemographicsCellPhone.MaxLength = 17
    	Me.txtDemographicsCellPhone.Name = "txtDemographicsCellPhone"
    	Me.txtDemographicsCellPhone.Size = New System.Drawing.Size(170, 20)
    	Me.txtDemographicsCellPhone.TabIndex = 19
    	'
    	'txtDemographicsPrimaryPhone
    	'
    	Me.txtDemographicsPrimaryPhone.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PrimaryPhoneBindingSource, "Number", true))
    	Me.txtDemographicsPrimaryPhone.Location = New System.Drawing.Point(91, 17)
    	Me.txtDemographicsPrimaryPhone.MaxLength = 17
    	Me.txtDemographicsPrimaryPhone.Name = "txtDemographicsPrimaryPhone"
    	Me.txtDemographicsPrimaryPhone.Size = New System.Drawing.Size(170, 20)
    	Me.txtDemographicsPrimaryPhone.TabIndex = 17
    	'
    	'lblDemographicsSchoolEmail
    	'
    	Me.lblDemographicsSchoolEmail.AutoSize = true
    	Me.lblDemographicsSchoolEmail.Location = New System.Drawing.Point(6, 98)
    	Me.lblDemographicsSchoolEmail.Name = "lblDemographicsSchoolEmail"
    	Me.lblDemographicsSchoolEmail.Size = New System.Drawing.Size(71, 13)
    	Me.lblDemographicsSchoolEmail.TabIndex = 3
    	Me.lblDemographicsSchoolEmail.Text = "School E-mail"
    	'
    	'lblDemographicsPersonalEmail
    	'
    	Me.lblDemographicsPersonalEmail.AutoSize = true
    	Me.lblDemographicsPersonalEmail.Location = New System.Drawing.Point(6, 72)
    	Me.lblDemographicsPersonalEmail.Name = "lblDemographicsPersonalEmail"
    	Me.lblDemographicsPersonalEmail.Size = New System.Drawing.Size(79, 13)
    	Me.lblDemographicsPersonalEmail.TabIndex = 2
    	Me.lblDemographicsPersonalEmail.Text = "Personal E-mail"
    	'
    	'lblDemographicsCellPhone
    	'
    	Me.lblDemographicsCellPhone.AutoSize = true
    	Me.lblDemographicsCellPhone.Location = New System.Drawing.Point(6, 46)
    	Me.lblDemographicsCellPhone.Name = "lblDemographicsCellPhone"
    	Me.lblDemographicsCellPhone.Size = New System.Drawing.Size(58, 13)
    	Me.lblDemographicsCellPhone.TabIndex = 1
    	Me.lblDemographicsCellPhone.Text = "Cell Phone"
    	'
    	'lblDemographicsPrimaryPhone
    	'
    	Me.lblDemographicsPrimaryPhone.AutoSize = true
    	Me.lblDemographicsPrimaryPhone.Location = New System.Drawing.Point(6, 20)
    	Me.lblDemographicsPrimaryPhone.Name = "lblDemographicsPrimaryPhone"
    	Me.lblDemographicsPrimaryPhone.Size = New System.Drawing.Size(75, 13)
    	Me.lblDemographicsPrimaryPhone.TabIndex = 0
    	Me.lblDemographicsPrimaryPhone.Text = "Primary Phone"
    	'
    	'grpDemographicsAddress
    	'
    	Me.grpDemographicsAddress.Controls.Add(Me.Label12)
    	Me.grpDemographicsAddress.Controls.Add(Me.txtDemographicsAddressLastUpdated)
    	Me.grpDemographicsAddress.Controls.Add(Me.lblDemographicsCountry)
    	Me.grpDemographicsAddress.Controls.Add(Me.txtDemographicsCountry)
    	Me.grpDemographicsAddress.Controls.Add(Me.chkDemographicsValidAddress)
    	Me.grpDemographicsAddress.Controls.Add(Me.txtDemographicsZip)
    	Me.grpDemographicsAddress.Controls.Add(Me.cmbDemographicsState)
    	Me.grpDemographicsAddress.Controls.Add(Me.txtDemographicsCity)
    	Me.grpDemographicsAddress.Controls.Add(Me.txtDemographicsStreetAddress2)
    	Me.grpDemographicsAddress.Controls.Add(Me.txtDemographicsStreetAddress1)
    	Me.grpDemographicsAddress.Controls.Add(Me.lblDemographicsCityStateZip)
    	Me.grpDemographicsAddress.Controls.Add(Me.lblDemographicsStreetAddress2)
    	Me.grpDemographicsAddress.Controls.Add(Me.lblDemographicsStreetAddress1)
    	Me.grpDemographicsAddress.Location = New System.Drawing.Point(12, 153)
    	Me.grpDemographicsAddress.Name = "grpDemographicsAddress"
    	Me.grpDemographicsAddress.Size = New System.Drawing.Size(433, 124)
    	Me.grpDemographicsAddress.TabIndex = 4
    	Me.grpDemographicsAddress.TabStop = false
    	Me.grpDemographicsAddress.Text = "Address Information"
    	'
    	'Label12
    	'
    	Me.Label12.AutoSize = true
    	Me.Label12.Location = New System.Drawing.Point(176, 99)
    	Me.Label12.Name = "Label12"
    	Me.Label12.Size = New System.Drawing.Size(71, 13)
    	Me.Label12.TabIndex = 26
    	Me.Label12.Text = "Last Updated"
    	'
    	'txtDemographicsAddressLastUpdated
    	'
    	Me.txtDemographicsAddressLastUpdated.Location = New System.Drawing.Point(253, 96)
    	Me.txtDemographicsAddressLastUpdated.Name = "txtDemographicsAddressLastUpdated"
    	Me.txtDemographicsAddressLastUpdated.ReadOnly = true
    	Me.txtDemographicsAddressLastUpdated.Size = New System.Drawing.Size(74, 20)
    	Me.txtDemographicsAddressLastUpdated.TabIndex = 25
    	'
    	'lblDemographicsCountry
    	'
    	Me.lblDemographicsCountry.AutoSize = true
    	Me.lblDemographicsCountry.Location = New System.Drawing.Point(6, 99)
    	Me.lblDemographicsCountry.Name = "lblDemographicsCountry"
    	Me.lblDemographicsCountry.Size = New System.Drawing.Size(43, 13)
    	Me.lblDemographicsCountry.TabIndex = 24
    	Me.lblDemographicsCountry.Text = "Country"
    	'
    	'txtDemographicsCountry
    	'
    	Me.txtDemographicsCountry.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.MailingAddressBindingSource, "Country", true))
    	Me.txtDemographicsCountry.Location = New System.Drawing.Point(55, 96)
    	Me.txtDemographicsCountry.MaxLength = 25
    	Me.txtDemographicsCountry.Name = "txtDemographicsCountry"
    	Me.txtDemographicsCountry.Size = New System.Drawing.Size(115, 20)
    	Me.txtDemographicsCountry.TabIndex = 16
    	'
    	'MailingAddressBindingSource
    	'
    	Me.MailingAddressBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.MailingAddress)
    	'
    	'chkDemographicsValidAddress
    	'
    	Me.chkDemographicsValidAddress.AutoSize = true
    	Me.chkDemographicsValidAddress.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.MailingAddressBindingSource, "IsValid", true))
    	Me.chkDemographicsValidAddress.Location = New System.Drawing.Point(333, 99)
    	Me.chkDemographicsValidAddress.Name = "chkDemographicsValidAddress"
    	Me.chkDemographicsValidAddress.Size = New System.Drawing.Size(90, 17)
    	Me.chkDemographicsValidAddress.TabIndex = 15
    	Me.chkDemographicsValidAddress.Text = "Valid Address"
    	Me.chkDemographicsValidAddress.UseVisualStyleBackColor = true
    	'
    	'txtDemographicsZip
    	'
    	Me.txtDemographicsZip.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.MailingAddressBindingSource, "ZipCode", true))
    	Me.txtDemographicsZip.Location = New System.Drawing.Point(341, 69)
    	Me.txtDemographicsZip.MaxLength = 9
    	Me.txtDemographicsZip.Name = "txtDemographicsZip"
    	Me.txtDemographicsZip.Size = New System.Drawing.Size(82, 20)
    	Me.txtDemographicsZip.TabIndex = 14
    	'
    	'cmbDemographicsState
    	'
    	Me.cmbDemographicsState.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.MailingAddressBindingSource, "State", true))
    	Me.cmbDemographicsState.FormattingEnabled = true
    	Me.cmbDemographicsState.Location = New System.Drawing.Point(293, 68)
    	Me.cmbDemographicsState.Name = "cmbDemographicsState"
    	Me.cmbDemographicsState.Size = New System.Drawing.Size(42, 21)
    	Me.cmbDemographicsState.Sorted = true
    	Me.cmbDemographicsState.TabIndex = 13
    	'
    	'txtDemographicsCity
    	'
    	Me.txtDemographicsCity.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.MailingAddressBindingSource, "City", true))
    	Me.txtDemographicsCity.Location = New System.Drawing.Point(97, 69)
    	Me.txtDemographicsCity.MaxLength = 20
    	Me.txtDemographicsCity.Name = "txtDemographicsCity"
    	Me.txtDemographicsCity.Size = New System.Drawing.Size(190, 20)
    	Me.txtDemographicsCity.TabIndex = 12
    	'
    	'txtDemographicsStreetAddress2
    	'
    	Me.txtDemographicsStreetAddress2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.MailingAddressBindingSource, "Line2", true))
    	Me.txtDemographicsStreetAddress2.Location = New System.Drawing.Point(97, 43)
    	Me.txtDemographicsStreetAddress2.MaxLength = 35
    	Me.txtDemographicsStreetAddress2.Name = "txtDemographicsStreetAddress2"
    	Me.txtDemographicsStreetAddress2.Size = New System.Drawing.Size(326, 20)
    	Me.txtDemographicsStreetAddress2.TabIndex = 11
    	'
    	'txtDemographicsStreetAddress1
    	'
    	Me.txtDemographicsStreetAddress1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.MailingAddressBindingSource, "Line1", true))
    	Me.txtDemographicsStreetAddress1.Location = New System.Drawing.Point(97, 17)
    	Me.txtDemographicsStreetAddress1.MaxLength = 35
    	Me.txtDemographicsStreetAddress1.Name = "txtDemographicsStreetAddress1"
    	Me.txtDemographicsStreetAddress1.Size = New System.Drawing.Size(326, 20)
    	Me.txtDemographicsStreetAddress1.TabIndex = 10
    	'
    	'lblDemographicsCityStateZip
    	'
    	Me.lblDemographicsCityStateZip.AutoSize = true
    	Me.lblDemographicsCityStateZip.Location = New System.Drawing.Point(6, 72)
    	Me.lblDemographicsCityStateZip.Name = "lblDemographicsCityStateZip"
    	Me.lblDemographicsCityStateZip.Size = New System.Drawing.Size(76, 13)
    	Me.lblDemographicsCityStateZip.TabIndex = 2
    	Me.lblDemographicsCityStateZip.Text = "City, State, Zip"
    	'
    	'lblDemographicsStreetAddress2
    	'
    	Me.lblDemographicsStreetAddress2.AutoSize = true
    	Me.lblDemographicsStreetAddress2.Location = New System.Drawing.Point(6, 46)
    	Me.lblDemographicsStreetAddress2.Name = "lblDemographicsStreetAddress2"
    	Me.lblDemographicsStreetAddress2.Size = New System.Drawing.Size(85, 13)
    	Me.lblDemographicsStreetAddress2.TabIndex = 1
    	Me.lblDemographicsStreetAddress2.Text = "Street Address 2"
    	'
    	'lblDemographicsStreetAddress1
    	'
    	Me.lblDemographicsStreetAddress1.AutoSize = true
    	Me.lblDemographicsStreetAddress1.Location = New System.Drawing.Point(6, 20)
    	Me.lblDemographicsStreetAddress1.Name = "lblDemographicsStreetAddress1"
    	Me.lblDemographicsStreetAddress1.Size = New System.Drawing.Size(85, 13)
    	Me.lblDemographicsStreetAddress1.TabIndex = 0
    	Me.lblDemographicsStreetAddress1.Text = "Street Address 1"
    	'
    	'grpDemographicsPersonal
    	'
    	Me.grpDemographicsPersonal.Controls.Add(Me.txtDemographicsDateOfBirth)
    	Me.grpDemographicsPersonal.Controls.Add(Me.cmbDemographicsEthnicity)
    	Me.grpDemographicsPersonal.Controls.Add(Me.cmbDemographicsGender)
    	Me.grpDemographicsPersonal.Controls.Add(Me.txtDemographicsAlternateLastName)
    	Me.grpDemographicsPersonal.Controls.Add(Me.txtDemographicsLastName)
    	Me.grpDemographicsPersonal.Controls.Add(Me.txtDemographicsMiddleName)
    	Me.grpDemographicsPersonal.Controls.Add(Me.txtDemographicsFirstName)
    	Me.grpDemographicsPersonal.Controls.Add(Me.lblDemographicsEthnicity)
    	Me.grpDemographicsPersonal.Controls.Add(Me.lblDemographicsName)
    	Me.grpDemographicsPersonal.Controls.Add(Me.lblDemographicsGender)
    	Me.grpDemographicsPersonal.Controls.Add(Me.lblDemographicsAlternateLastName)
    	Me.grpDemographicsPersonal.Controls.Add(Me.lblDemographicsDateOfBirth)
    	Me.grpDemographicsPersonal.Location = New System.Drawing.Point(12, 42)
    	Me.grpDemographicsPersonal.Name = "grpDemographicsPersonal"
    	Me.grpDemographicsPersonal.Size = New System.Drawing.Size(613, 105)
    	Me.grpDemographicsPersonal.TabIndex = 3
    	Me.grpDemographicsPersonal.TabStop = false
    	Me.grpDemographicsPersonal.Text = "Personal Information"
    	'
    	'txtDemographicsDateOfBirth
    	'
    	Me.txtDemographicsDateOfBirth.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.StudentBindingSource, "DateOfBirth", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, Nothing, "MM/dd/yyyy"))
    	Me.txtDemographicsDateOfBirth.Location = New System.Drawing.Point(78, 78)
    	Me.txtDemographicsDateOfBirth.Mask = "00/00/0000"
    	Me.txtDemographicsDateOfBirth.Name = "txtDemographicsDateOfBirth"
    	Me.txtDemographicsDateOfBirth.Size = New System.Drawing.Size(100, 20)
    	Me.txtDemographicsDateOfBirth.TabIndex = 10
    	Me.txtDemographicsDateOfBirth.ValidatingType = GetType(Date)
    	'
    	'cmbDemographicsEthnicity
    	'
    	Me.cmbDemographicsEthnicity.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.StudentBindingSource, "Ethnicity", true))
    	Me.cmbDemographicsEthnicity.FormattingEnabled = true
    	Me.cmbDemographicsEthnicity.Location = New System.Drawing.Point(352, 78)
    	Me.cmbDemographicsEthnicity.MaxLength = 20
    	Me.cmbDemographicsEthnicity.Name = "cmbDemographicsEthnicity"
    	Me.cmbDemographicsEthnicity.Size = New System.Drawing.Size(255, 21)
    	Me.cmbDemographicsEthnicity.Sorted = true
    	Me.cmbDemographicsEthnicity.TabIndex = 9
    	'
    	'cmbDemographicsGender
    	'
    	Me.cmbDemographicsGender.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.StudentBindingSource, "Gender", true))
    	Me.cmbDemographicsGender.FormattingEnabled = true
    	Me.cmbDemographicsGender.Location = New System.Drawing.Point(236, 78)
    	Me.cmbDemographicsGender.Name = "cmbDemographicsGender"
    	Me.cmbDemographicsGender.Size = New System.Drawing.Size(43, 21)
    	Me.cmbDemographicsGender.Sorted = true
    	Me.cmbDemographicsGender.TabIndex = 8
    	'
    	'txtDemographicsAlternateLastName
    	'
    	Me.txtDemographicsAlternateLastName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.StudentBindingSource, "AlternateLastName", true))
    	Me.txtDemographicsAlternateLastName.Location = New System.Drawing.Point(118, 50)
    	Me.txtDemographicsAlternateLastName.MaxLength = 25
    	Me.txtDemographicsAlternateLastName.Name = "txtDemographicsAlternateLastName"
    	Me.txtDemographicsAlternateLastName.Size = New System.Drawing.Size(234, 20)
    	Me.txtDemographicsAlternateLastName.TabIndex = 6
    	'
    	'txtDemographicsLastName
    	'
    	Me.txtDemographicsLastName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.StudentBindingSource, "LastName", true))
    	Me.txtDemographicsLastName.Location = New System.Drawing.Point(373, 25)
    	Me.txtDemographicsLastName.MaxLength = 25
    	Me.txtDemographicsLastName.Name = "txtDemographicsLastName"
    	Me.txtDemographicsLastName.Size = New System.Drawing.Size(234, 20)
    	Me.txtDemographicsLastName.TabIndex = 5
    	'
    	'txtDemographicsMiddleName
    	'
    	Me.txtDemographicsMiddleName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.StudentBindingSource, "MiddleName", true))
    	Me.txtDemographicsMiddleName.Location = New System.Drawing.Point(285, 25)
    	Me.txtDemographicsMiddleName.MaxLength = 20
    	Me.txtDemographicsMiddleName.Name = "txtDemographicsMiddleName"
    	Me.txtDemographicsMiddleName.Size = New System.Drawing.Size(82, 20)
    	Me.txtDemographicsMiddleName.TabIndex = 4
    	'
    	'txtDemographicsFirstName
    	'
    	Me.txtDemographicsFirstName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.StudentBindingSource, "FirstName", true))
    	Me.txtDemographicsFirstName.Location = New System.Drawing.Point(89, 25)
    	Me.txtDemographicsFirstName.MaxLength = 20
    	Me.txtDemographicsFirstName.Name = "txtDemographicsFirstName"
    	Me.txtDemographicsFirstName.Size = New System.Drawing.Size(190, 20)
    	Me.txtDemographicsFirstName.TabIndex = 3
    	'
    	'lblDemographicsEthnicity
    	'
    	Me.lblDemographicsEthnicity.AutoSize = true
    	Me.lblDemographicsEthnicity.Location = New System.Drawing.Point(299, 81)
    	Me.lblDemographicsEthnicity.Name = "lblDemographicsEthnicity"
    	Me.lblDemographicsEthnicity.Size = New System.Drawing.Size(47, 13)
    	Me.lblDemographicsEthnicity.TabIndex = 6
    	Me.lblDemographicsEthnicity.Text = "Ethnicity"
    	'
    	'lblDemographicsName
    	'
    	Me.lblDemographicsName.AutoSize = true
    	Me.lblDemographicsName.Location = New System.Drawing.Point(6, 28)
    	Me.lblDemographicsName.Name = "lblDemographicsName"
    	Me.lblDemographicsName.Size = New System.Drawing.Size(77, 13)
    	Me.lblDemographicsName.TabIndex = 2
    	Me.lblDemographicsName.Text = "Name (F, M, L)"
    	'
    	'lblDemographicsGender
    	'
    	Me.lblDemographicsGender.AutoSize = true
    	Me.lblDemographicsGender.Location = New System.Drawing.Point(188, 81)
    	Me.lblDemographicsGender.Name = "lblDemographicsGender"
    	Me.lblDemographicsGender.Size = New System.Drawing.Size(42, 13)
    	Me.lblDemographicsGender.TabIndex = 5
    	Me.lblDemographicsGender.Text = "Gender"
    	'
    	'lblDemographicsAlternateLastName
    	'
    	Me.lblDemographicsAlternateLastName.AutoSize = true
    	Me.lblDemographicsAlternateLastName.Location = New System.Drawing.Point(6, 54)
    	Me.lblDemographicsAlternateLastName.Name = "lblDemographicsAlternateLastName"
    	Me.lblDemographicsAlternateLastName.Size = New System.Drawing.Size(103, 13)
    	Me.lblDemographicsAlternateLastName.TabIndex = 3
    	Me.lblDemographicsAlternateLastName.Text = "Alternate Last Name"
    	'
    	'lblDemographicsDateOfBirth
    	'
    	Me.lblDemographicsDateOfBirth.AutoSize = true
    	Me.lblDemographicsDateOfBirth.Location = New System.Drawing.Point(6, 81)
    	Me.lblDemographicsDateOfBirth.Name = "lblDemographicsDateOfBirth"
    	Me.lblDemographicsDateOfBirth.Size = New System.Drawing.Size(66, 13)
    	Me.lblDemographicsDateOfBirth.TabIndex = 4
    	Me.lblDemographicsDateOfBirth.Text = "Date of Birth"
    	'
    	'tabApplication
    	'
    	Me.tabApplication.AutoScroll = true
    	Me.tabApplication.AutoScrollMargin = New System.Drawing.Size(0, 6)
    	Me.tabApplication.Controls.Add(Me.GroupBox3)
    	Me.tabApplication.Controls.Add(Me.GroupBox2)
    	Me.tabApplication.Controls.Add(Me.grpApplicationCollegeEnrollment)
    	Me.tabApplication.Controls.Add(Me.btnApplicationViewDocuments)
    	Me.tabApplication.Controls.Add(Me.grpApplicationDocumentStatus)
    	Me.tabApplication.Controls.Add(Me.grpApplicationReviewStatus)
    	Me.tabApplication.Controls.Add(Me.grpApplicationClasses)
    	Me.tabApplication.Controls.Add(Me.btnApplicationLinkDocument)
    	Me.tabApplication.Controls.Add(Me.grpApplicationAwardStatus)
    	Me.tabApplication.Controls.Add(Me.grpApplicationHighSchool)
    	Me.tabApplication.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.tabApplication.Location = New System.Drawing.Point(4, 22)
    	Me.tabApplication.Name = "tabApplication"
    	Me.tabApplication.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabApplication.Size = New System.Drawing.Size(933, 723)
    	Me.tabApplication.TabIndex = 2
    	Me.tabApplication.Text = "Application"
    	Me.tabApplication.UseVisualStyleBackColor = true
    	'
    	'GroupBox3
    	'
    	Me.GroupBox3.Controls.Add(Me.txtApplicationPlannedCollegeToAttend)
    	Me.GroupBox3.Controls.Add(Me.Label4)
    	Me.GroupBox3.Controls.Add(Me.chkApplicationOtherScholarshipAwards)
    	Me.GroupBox3.Controls.Add(Me.txtApplicationOtherScholarshipAwardsAmount)
    	Me.GroupBox3.Controls.Add(Me.lblApplicationOtherScholarshipAwardsAmount)
    	Me.GroupBox3.Location = New System.Drawing.Point(8, 1030)
    	Me.GroupBox3.Name = "GroupBox3"
    	Me.GroupBox3.Size = New System.Drawing.Size(739, 58)
    	Me.GroupBox3.TabIndex = 55
    	Me.GroupBox3.TabStop = false
    	Me.GroupBox3.Text = "Other Scholarship Awards"
    	'
    	'txtApplicationPlannedCollegeToAttend
    	'
    	Me.txtApplicationPlannedCollegeToAttend.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ScholarshipApplicationBindingSource1, "PlannedCollegeToAttend", true))
    	Me.txtApplicationPlannedCollegeToAttend.Location = New System.Drawing.Point(462, 23)
    	Me.txtApplicationPlannedCollegeToAttend.Name = "txtApplicationPlannedCollegeToAttend"
    	Me.txtApplicationPlannedCollegeToAttend.ReadOnly = true
    	Me.txtApplicationPlannedCollegeToAttend.Size = New System.Drawing.Size(263, 20)
    	Me.txtApplicationPlannedCollegeToAttend.TabIndex = 76
    	'
    	'Label4
    	'
    	Me.Label4.AutoSize = true
    	Me.Label4.Location = New System.Drawing.Point(313, 26)
    	Me.Label4.Name = "Label4"
    	Me.Label4.Size = New System.Drawing.Size(142, 13)
    	Me.Label4.TabIndex = 75
    	Me.Label4.Text = "Planned College Attendance"
    	'
    	'chkApplicationOtherScholarshipAwards
    	'
    	Me.chkApplicationOtherScholarshipAwards.AutoSize = true
    	Me.chkApplicationOtherScholarshipAwards.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.CollegeBindingSource, "HasOtherScholarships", true))
    	Me.chkApplicationOtherScholarshipAwards.Location = New System.Drawing.Point(9, 25)
    	Me.chkApplicationOtherScholarshipAwards.Name = "chkApplicationOtherScholarshipAwards"
    	Me.chkApplicationOtherScholarshipAwards.Size = New System.Drawing.Size(151, 17)
    	Me.chkApplicationOtherScholarshipAwards.TabIndex = 66
    	Me.chkApplicationOtherScholarshipAwards.Text = "Other scholarship awards?"
    	Me.chkApplicationOtherScholarshipAwards.UseVisualStyleBackColor = true
    	'
    	'CollegeBindingSource
    	'
    	Me.CollegeBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.College)
    	'
    	'txtApplicationOtherScholarshipAwardsAmount
    	'
    	Me.txtApplicationOtherScholarshipAwardsAmount.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CollegeBindingSource, "OtherScholarshipsAmount", true))
    	Me.txtApplicationOtherScholarshipAwardsAmount.Location = New System.Drawing.Point(222, 23)
    	Me.txtApplicationOtherScholarshipAwardsAmount.Name = "txtApplicationOtherScholarshipAwardsAmount"
    	Me.txtApplicationOtherScholarshipAwardsAmount.Size = New System.Drawing.Size(72, 20)
    	Me.txtApplicationOtherScholarshipAwardsAmount.TabIndex = 67
    	'
    	'lblApplicationOtherScholarshipAwardsAmount
    	'
    	Me.lblApplicationOtherScholarshipAwardsAmount.AutoSize = true
    	Me.lblApplicationOtherScholarshipAwardsAmount.Location = New System.Drawing.Point(173, 27)
    	Me.lblApplicationOtherScholarshipAwardsAmount.Name = "lblApplicationOtherScholarshipAwardsAmount"
    	Me.lblApplicationOtherScholarshipAwardsAmount.Size = New System.Drawing.Size(43, 13)
    	Me.lblApplicationOtherScholarshipAwardsAmount.TabIndex = 74
    	Me.lblApplicationOtherScholarshipAwardsAmount.Text = "Amount"
    	'
    	'GroupBox2
    	'
    	Me.GroupBox2.Controls.Add(Me.chkApplicationAttendedAnotherSchool)
    	Me.GroupBox2.Controls.Add(Me.cmbApplication9thGradeSchoolName)
    	Me.GroupBox2.Controls.Add(Me.Label3)
    	Me.GroupBox2.Location = New System.Drawing.Point(8, 847)
    	Me.GroupBox2.Name = "GroupBox2"
    	Me.GroupBox2.Size = New System.Drawing.Size(740, 99)
    	Me.GroupBox2.TabIndex = 54
    	Me.GroupBox2.TabStop = false
    	Me.GroupBox2.Text = "Other School Information"
    	'
    	'chkApplicationAttendedAnotherSchool
    	'
    	Me.chkApplicationAttendedAnotherSchool.AutoSize = true
    	Me.chkApplicationAttendedAnotherSchool.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ScholarshipApplicationBindingSource1, "AttendedAnotherSchool", true))
    	Me.chkApplicationAttendedAnotherSchool.Location = New System.Drawing.Point(9, 59)
    	Me.chkApplicationAttendedAnotherSchool.Name = "chkApplicationAttendedAnotherSchool"
    	Me.chkApplicationAttendedAnotherSchool.Size = New System.Drawing.Size(260, 17)
    	Me.chkApplicationAttendedAnotherSchool.TabIndex = 55
    	Me.chkApplicationAttendedAnotherSchool.Text = "Did you attend another school during 9-12 grade?"
    	Me.chkApplicationAttendedAnotherSchool.UseVisualStyleBackColor = true
    	'
    	'cmbApplication9thGradeSchoolName
    	'
    	Me.cmbApplication9thGradeSchoolName.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.ScholarshipApplicationBindingSource1, "NinthGradeSchool", true))
    	Me.cmbApplication9thGradeSchoolName.FormattingEnabled = true
    	Me.cmbApplication9thGradeSchoolName.Location = New System.Drawing.Point(127, 25)
    	Me.cmbApplication9thGradeSchoolName.MaxLength = 30
    	Me.cmbApplication9thGradeSchoolName.Name = "cmbApplication9thGradeSchoolName"
    	Me.cmbApplication9thGradeSchoolName.Size = New System.Drawing.Size(382, 21)
    	Me.cmbApplication9thGradeSchoolName.Sorted = true
    	Me.cmbApplication9thGradeSchoolName.TabIndex = 50
    	'
    	'Label3
    	'
    	Me.Label3.AutoSize = true
    	Me.Label3.Location = New System.Drawing.Point(4, 28)
    	Me.Label3.Name = "Label3"
    	Me.Label3.Size = New System.Drawing.Size(121, 13)
    	Me.Label3.TabIndex = 49
    	Me.Label3.Text = "9th Grade School Name"
    	'
    	'grpApplicationCollegeEnrollment
    	'
    	Me.grpApplicationCollegeEnrollment.Controls.Add(Me.dtpApplicationTermBeginDate)
    	Me.grpApplicationCollegeEnrollment.Controls.Add(Me.cmbApplicationTerm)
    	Me.grpApplicationCollegeEnrollment.Controls.Add(Me.lblApplicationTerm)
    	Me.grpApplicationCollegeEnrollment.Controls.Add(Me.txtApplicationEnrolledCredits)
    	Me.grpApplicationCollegeEnrollment.Controls.Add(Me.lblApplicationTermBeginDate)
    	Me.grpApplicationCollegeEnrollment.Controls.Add(Me.cmbApplicationCollegeName)
    	Me.grpApplicationCollegeEnrollment.Controls.Add(Me.lblApplicationEnrolledCredits)
    	Me.grpApplicationCollegeEnrollment.Controls.Add(Me.lblApplicationCollegeName)
    	Me.grpApplicationCollegeEnrollment.Location = New System.Drawing.Point(8, 952)
    	Me.grpApplicationCollegeEnrollment.Name = "grpApplicationCollegeEnrollment"
    	Me.grpApplicationCollegeEnrollment.Size = New System.Drawing.Size(740, 72)
    	Me.grpApplicationCollegeEnrollment.TabIndex = 52
    	Me.grpApplicationCollegeEnrollment.TabStop = false
    	Me.grpApplicationCollegeEnrollment.Text = "College Enrollment Information"
    	'
    	'dtpApplicationTermBeginDate
    	'
    	Me.dtpApplicationTermBeginDate.CustomFormat = ""
    	Me.dtpApplicationTermBeginDate.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.CollegeBindingSource, "TermBeginDate", true))
    	Me.dtpApplicationTermBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationTermBeginDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationTermBeginDate.Location = New System.Drawing.Point(495, 18)
    	Me.dtpApplicationTermBeginDate.Name = "dtpApplicationTermBeginDate"
    	Me.dtpApplicationTermBeginDate.NullValue = " "
    	Me.dtpApplicationTermBeginDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationTermBeginDate.TabIndex = 63
    	Me.dtpApplicationTermBeginDate.Value = Nothing
    	'
    	'cmbApplicationTerm
    	'
    	Me.cmbApplicationTerm.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CollegeBindingSource, "Term", true))
    	Me.cmbApplicationTerm.FormattingEnabled = true
    	Me.cmbApplicationTerm.Location = New System.Drawing.Point(637, 17)
    	Me.cmbApplicationTerm.Name = "cmbApplicationTerm"
    	Me.cmbApplicationTerm.Size = New System.Drawing.Size(97, 21)
    	Me.cmbApplicationTerm.TabIndex = 64
    	'
    	'lblApplicationTerm
    	'
    	Me.lblApplicationTerm.AutoSize = true
    	Me.lblApplicationTerm.Location = New System.Drawing.Point(600, 22)
    	Me.lblApplicationTerm.Name = "lblApplicationTerm"
    	Me.lblApplicationTerm.Size = New System.Drawing.Size(31, 13)
    	Me.lblApplicationTerm.TabIndex = 74
    	Me.lblApplicationTerm.Text = "Term"
    	'
    	'txtApplicationEnrolledCredits
    	'
    	Me.txtApplicationEnrolledCredits.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CollegeBindingSource, "NumberOfEnrolledCredits", true))
    	Me.txtApplicationEnrolledCredits.Location = New System.Drawing.Point(145, 45)
    	Me.txtApplicationEnrolledCredits.MaxLength = 4
    	Me.txtApplicationEnrolledCredits.Name = "txtApplicationEnrolledCredits"
    	Me.txtApplicationEnrolledCredits.Size = New System.Drawing.Size(63, 20)
    	Me.txtApplicationEnrolledCredits.TabIndex = 65
    	'
    	'lblApplicationTermBeginDate
    	'
    	Me.lblApplicationTermBeginDate.AutoSize = true
    	Me.lblApplicationTermBeginDate.Location = New System.Drawing.Point(402, 22)
    	Me.lblApplicationTermBeginDate.Name = "lblApplicationTermBeginDate"
    	Me.lblApplicationTermBeginDate.Size = New System.Drawing.Size(87, 13)
    	Me.lblApplicationTermBeginDate.TabIndex = 72
    	Me.lblApplicationTermBeginDate.Text = "Term Begin Date"
    	'
    	'cmbApplicationCollegeName
    	'
    	Me.cmbApplicationCollegeName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CollegeBindingSource, "Name", true))
    	Me.cmbApplicationCollegeName.FormattingEnabled = true
    	Me.cmbApplicationCollegeName.Location = New System.Drawing.Point(85, 19)
    	Me.cmbApplicationCollegeName.MaxLength = 30
    	Me.cmbApplicationCollegeName.Name = "cmbApplicationCollegeName"
    	Me.cmbApplicationCollegeName.Size = New System.Drawing.Size(307, 21)
    	Me.cmbApplicationCollegeName.TabIndex = 62
    	'
    	'lblApplicationEnrolledCredits
    	'
    	Me.lblApplicationEnrolledCredits.AutoSize = true
    	Me.lblApplicationEnrolledCredits.Location = New System.Drawing.Point(7, 48)
    	Me.lblApplicationEnrolledCredits.Name = "lblApplicationEnrolledCredits"
    	Me.lblApplicationEnrolledCredits.Size = New System.Drawing.Size(132, 13)
    	Me.lblApplicationEnrolledCredits.TabIndex = 72
    	Me.lblApplicationEnrolledCredits.Text = "Number of Enrolled Credits"
    	'
    	'lblApplicationCollegeName
    	'
    	Me.lblApplicationCollegeName.AutoSize = true
    	Me.lblApplicationCollegeName.Location = New System.Drawing.Point(6, 22)
    	Me.lblApplicationCollegeName.Name = "lblApplicationCollegeName"
    	Me.lblApplicationCollegeName.Size = New System.Drawing.Size(73, 13)
    	Me.lblApplicationCollegeName.TabIndex = 0
    	Me.lblApplicationCollegeName.Text = "College Name"
    	'
    	'btnApplicationViewDocuments
    	'
    	Me.btnApplicationViewDocuments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnApplicationViewDocuments.Location = New System.Drawing.Point(334, 602)
    	Me.btnApplicationViewDocuments.Name = "btnApplicationViewDocuments"
    	Me.btnApplicationViewDocuments.Size = New System.Drawing.Size(86, 42)
    	Me.btnApplicationViewDocuments.TabIndex = 46
    	Me.btnApplicationViewDocuments.Text = "View Documents"
    	Me.btnApplicationViewDocuments.UseVisualStyleBackColor = true
    	'
    	'grpApplicationDocumentStatus
    	'
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.chkApplicationAppealDenied)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.chkApplicationAppealApproved)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationAppealDecision)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationProofOfCitizenshipReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationProofOfCitizenshipReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpHighSchoolScheduleReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.Label10)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationDefermentDecisionDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationDefermentRequestReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationLoaDecisionDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationLoaRequestReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationAppealDecisionDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationAppealReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationProofOfEnrollmentReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationConditionalAcceptanceFormReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationSignaturePageReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationFinalCollegeTranscriptReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationFinalHighSchoolTranscriptReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationInitialCollegeTranscriptReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.dtpApplicationReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationDefermentDecisionDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationDefermentRequestReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationLoaDecisionDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationLoaRequestReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationAppealDecisionDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationAppealReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationFinalCollegeTranscriptReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationProofOfEnrollmentReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationConditionalAcceptanceFormReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationSignaturePageReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationInitialCollegeTranscriptReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationInitialHighSchoolTranscriptReceivedDate)
    	Me.grpApplicationDocumentStatus.Controls.Add(Me.lblApplicationReceivedDate)
    	Me.grpApplicationDocumentStatus.Location = New System.Drawing.Point(434, 240)
    	Me.grpApplicationDocumentStatus.Name = "grpApplicationDocumentStatus"
    	Me.grpApplicationDocumentStatus.Size = New System.Drawing.Size(314, 459)
    	Me.grpApplicationDocumentStatus.TabIndex = 3
    	Me.grpApplicationDocumentStatus.TabStop = false
    	Me.grpApplicationDocumentStatus.Text = "Document Status"
    	'
    	'chkApplicationAppealDenied
    	'
    	Me.chkApplicationAppealDenied.AutoSize = true
    	Me.chkApplicationAppealDenied.Location = New System.Drawing.Point(246, 331)
    	Me.chkApplicationAppealDenied.Name = "chkApplicationAppealDenied"
    	Me.chkApplicationAppealDenied.Size = New System.Drawing.Size(60, 17)
    	Me.chkApplicationAppealDenied.TabIndex = 113
    	Me.chkApplicationAppealDenied.Text = "Denied"
    	Me.chkApplicationAppealDenied.UseVisualStyleBackColor = true
    	'
    	'chkApplicationAppealApproved
    	'
    	Me.chkApplicationAppealApproved.AutoSize = true
    	Me.chkApplicationAppealApproved.Location = New System.Drawing.Point(158, 331)
    	Me.chkApplicationAppealApproved.Name = "chkApplicationAppealApproved"
    	Me.chkApplicationAppealApproved.Size = New System.Drawing.Size(72, 17)
    	Me.chkApplicationAppealApproved.TabIndex = 112
    	Me.chkApplicationAppealApproved.Text = "Approved"
    	Me.chkApplicationAppealApproved.UseVisualStyleBackColor = true
    	'
    	'lblApplicationAppealDecision
    	'
    	Me.lblApplicationAppealDecision.AutoSize = true
    	Me.lblApplicationAppealDecision.Location = New System.Drawing.Point(68, 332)
    	Me.lblApplicationAppealDecision.Name = "lblApplicationAppealDecision"
    	Me.lblApplicationAppealDecision.Size = New System.Drawing.Size(48, 13)
    	Me.lblApplicationAppealDecision.TabIndex = 111
    	Me.lblApplicationAppealDecision.Text = "Decision"
    	'
    	'dtpApplicationProofOfCitizenshipReceivedDate
    	'
    	Me.dtpApplicationProofOfCitizenshipReceivedDate.CustomFormat = ""
    	Me.dtpApplicationProofOfCitizenshipReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationProofOfCitizenshipReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationProofOfCitizenshipReceivedDate.Location = New System.Drawing.Point(211, 253)
    	Me.dtpApplicationProofOfCitizenshipReceivedDate.Name = "dtpApplicationProofOfCitizenshipReceivedDate"
    	Me.dtpApplicationProofOfCitizenshipReceivedDate.NullValue = " "
    	Me.dtpApplicationProofOfCitizenshipReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationProofOfCitizenshipReceivedDate.TabIndex = 39
    	Me.dtpApplicationProofOfCitizenshipReceivedDate.Value = Nothing
    	'
    	'lblApplicationProofOfCitizenshipReceivedDate
    	'
    	Me.lblApplicationProofOfCitizenshipReceivedDate.AutoSize = true
    	Me.lblApplicationProofOfCitizenshipReceivedDate.Location = New System.Drawing.Point(6, 257)
    	Me.lblApplicationProofOfCitizenshipReceivedDate.Name = "lblApplicationProofOfCitizenshipReceivedDate"
    	Me.lblApplicationProofOfCitizenshipReceivedDate.Size = New System.Drawing.Size(146, 13)
    	Me.lblApplicationProofOfCitizenshipReceivedDate.TabIndex = 110
    	Me.lblApplicationProofOfCitizenshipReceivedDate.Text = "Proof of Citizenship Received"
    	'
    	'dtpHighSchoolScheduleReceivedDate
    	'
    	Me.dtpHighSchoolScheduleReceivedDate.CustomFormat = ""
    	Me.dtpHighSchoolScheduleReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpHighSchoolScheduleReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpHighSchoolScheduleReceivedDate.Location = New System.Drawing.Point(211, 97)
    	Me.dtpHighSchoolScheduleReceivedDate.Name = "dtpHighSchoolScheduleReceivedDate"
    	Me.dtpHighSchoolScheduleReceivedDate.NullValue = " "
    	Me.dtpHighSchoolScheduleReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpHighSchoolScheduleReceivedDate.TabIndex = 109
    	Me.dtpHighSchoolScheduleReceivedDate.Value = Nothing
    	'
    	'Label10
    	'
    	Me.Label10.AutoSize = true
    	Me.Label10.Location = New System.Drawing.Point(6, 101)
    	Me.Label10.Name = "Label10"
    	Me.Label10.Size = New System.Drawing.Size(162, 13)
    	Me.Label10.TabIndex = 108
    	Me.Label10.Text = "High School Schedule Received"
    	'
    	'dtpApplicationDefermentDecisionDate
    	'
    	Me.dtpApplicationDefermentDecisionDate.CustomFormat = ""
    	Me.dtpApplicationDefermentDecisionDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationDefermentDecisionDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationDefermentDecisionDate.Location = New System.Drawing.Point(213, 432)
    	Me.dtpApplicationDefermentDecisionDate.Name = "dtpApplicationDefermentDecisionDate"
    	Me.dtpApplicationDefermentDecisionDate.NullValue = " "
    	Me.dtpApplicationDefermentDecisionDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationDefermentDecisionDate.TabIndex = 45
    	Me.dtpApplicationDefermentDecisionDate.Value = Nothing
    	'
    	'dtpApplicationDefermentRequestReceivedDate
    	'
    	Me.dtpApplicationDefermentRequestReceivedDate.CustomFormat = ""
    	Me.dtpApplicationDefermentRequestReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationDefermentRequestReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationDefermentRequestReceivedDate.Location = New System.Drawing.Point(213, 406)
    	Me.dtpApplicationDefermentRequestReceivedDate.Name = "dtpApplicationDefermentRequestReceivedDate"
    	Me.dtpApplicationDefermentRequestReceivedDate.NullValue = " "
    	Me.dtpApplicationDefermentRequestReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationDefermentRequestReceivedDate.TabIndex = 44
    	Me.dtpApplicationDefermentRequestReceivedDate.Value = Nothing
    	'
    	'dtpApplicationLoaDecisionDate
    	'
    	Me.dtpApplicationLoaDecisionDate.CustomFormat = ""
    	Me.dtpApplicationLoaDecisionDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationLoaDecisionDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationLoaDecisionDate.Location = New System.Drawing.Point(213, 380)
    	Me.dtpApplicationLoaDecisionDate.Name = "dtpApplicationLoaDecisionDate"
    	Me.dtpApplicationLoaDecisionDate.NullValue = " "
    	Me.dtpApplicationLoaDecisionDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationLoaDecisionDate.TabIndex = 43
    	Me.dtpApplicationLoaDecisionDate.Value = Nothing
    	'
    	'dtpApplicationLoaRequestReceivedDate
    	'
    	Me.dtpApplicationLoaRequestReceivedDate.CustomFormat = ""
    	Me.dtpApplicationLoaRequestReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationLoaRequestReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationLoaRequestReceivedDate.Location = New System.Drawing.Point(213, 354)
    	Me.dtpApplicationLoaRequestReceivedDate.Name = "dtpApplicationLoaRequestReceivedDate"
    	Me.dtpApplicationLoaRequestReceivedDate.NullValue = " "
    	Me.dtpApplicationLoaRequestReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationLoaRequestReceivedDate.TabIndex = 42
    	Me.dtpApplicationLoaRequestReceivedDate.Value = Nothing
    	'
    	'dtpApplicationAppealDecisionDate
    	'
    	Me.dtpApplicationAppealDecisionDate.CustomFormat = ""
    	Me.dtpApplicationAppealDecisionDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationAppealDecisionDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationAppealDecisionDate.Location = New System.Drawing.Point(211, 305)
    	Me.dtpApplicationAppealDecisionDate.Name = "dtpApplicationAppealDecisionDate"
    	Me.dtpApplicationAppealDecisionDate.NullValue = " "
    	Me.dtpApplicationAppealDecisionDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationAppealDecisionDate.TabIndex = 41
    	Me.dtpApplicationAppealDecisionDate.Value = Nothing
    	'
    	'dtpApplicationAppealReceivedDate
    	'
    	Me.dtpApplicationAppealReceivedDate.CustomFormat = ""
    	Me.dtpApplicationAppealReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationAppealReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationAppealReceivedDate.Location = New System.Drawing.Point(211, 279)
    	Me.dtpApplicationAppealReceivedDate.Name = "dtpApplicationAppealReceivedDate"
    	Me.dtpApplicationAppealReceivedDate.NullValue = " "
    	Me.dtpApplicationAppealReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationAppealReceivedDate.TabIndex = 40
    	Me.dtpApplicationAppealReceivedDate.Value = Nothing
    	'
    	'dtpApplicationProofOfEnrollmentReceivedDate
    	'
    	Me.dtpApplicationProofOfEnrollmentReceivedDate.CustomFormat = ""
    	Me.dtpApplicationProofOfEnrollmentReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationProofOfEnrollmentReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationProofOfEnrollmentReceivedDate.Location = New System.Drawing.Point(211, 227)
    	Me.dtpApplicationProofOfEnrollmentReceivedDate.Name = "dtpApplicationProofOfEnrollmentReceivedDate"
    	Me.dtpApplicationProofOfEnrollmentReceivedDate.NullValue = " "
    	Me.dtpApplicationProofOfEnrollmentReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationProofOfEnrollmentReceivedDate.TabIndex = 38
    	Me.dtpApplicationProofOfEnrollmentReceivedDate.Value = Nothing
    	'
    	'dtpApplicationConditionalAcceptanceFormReceivedDate
    	'
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate.CustomFormat = ""
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate.Location = New System.Drawing.Point(211, 201)
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate.Name = "dtpApplicationConditionalAcceptanceFormReceivedDate"
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate.NullValue = " "
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate.TabIndex = 37
    	Me.dtpApplicationConditionalAcceptanceFormReceivedDate.Value = Nothing
    	'
    	'dtpApplicationSignaturePageReceivedDate
    	'
    	Me.dtpApplicationSignaturePageReceivedDate.CustomFormat = ""
    	Me.dtpApplicationSignaturePageReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationSignaturePageReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationSignaturePageReceivedDate.Location = New System.Drawing.Point(211, 123)
    	Me.dtpApplicationSignaturePageReceivedDate.Name = "dtpApplicationSignaturePageReceivedDate"
    	Me.dtpApplicationSignaturePageReceivedDate.NullValue = " "
    	Me.dtpApplicationSignaturePageReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationSignaturePageReceivedDate.TabIndex = 36
    	Me.dtpApplicationSignaturePageReceivedDate.Value = Nothing
    	'
    	'dtpApplicationFinalCollegeTranscriptReceivedDate
    	'
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate.CustomFormat = ""
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate.Location = New System.Drawing.Point(211, 174)
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate.Name = "dtpApplicationFinalCollegeTranscriptReceivedDate"
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate.NullValue = " "
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate.TabIndex = 35
    	Me.dtpApplicationFinalCollegeTranscriptReceivedDate.Value = Nothing
    	'
    	'lblApplicationFinalHighSchoolTranscriptReceivedDate
    	'
    	Me.lblApplicationFinalHighSchoolTranscriptReceivedDate.AutoSize = true
    	Me.lblApplicationFinalHighSchoolTranscriptReceivedDate.Location = New System.Drawing.Point(6, 152)
    	Me.lblApplicationFinalHighSchoolTranscriptReceivedDate.Name = "lblApplicationFinalHighSchoolTranscriptReceivedDate"
    	Me.lblApplicationFinalHighSchoolTranscriptReceivedDate.Size = New System.Drawing.Size(189, 13)
    	Me.lblApplicationFinalHighSchoolTranscriptReceivedDate.TabIndex = 2
    	Me.lblApplicationFinalHighSchoolTranscriptReceivedDate.Text = "Final High School Transcript Received"
    	'
    	'dtpApplicationFinalHighSchoolTranscriptReceivedDate
    	'
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate.CustomFormat = ""
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate.Location = New System.Drawing.Point(211, 148)
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate.Name = "dtpApplicationFinalHighSchoolTranscriptReceivedDate"
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate.NullValue = " "
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate.TabIndex = 33
    	Me.dtpApplicationFinalHighSchoolTranscriptReceivedDate.Value = Nothing
    	'
    	'dtpApplicationInitialCollegeTranscriptReceivedDate
    	'
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate.CustomFormat = ""
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate.Location = New System.Drawing.Point(211, 71)
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate.Name = "dtpApplicationInitialCollegeTranscriptReceivedDate"
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate.NullValue = " "
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate.TabIndex = 34
    	Me.dtpApplicationInitialCollegeTranscriptReceivedDate.Value = Nothing
    	'
    	'dtpApplicationInitialHighSchoolTranscriptReceivedDate
    	'
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate.CustomFormat = ""
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate.Location = New System.Drawing.Point(211, 45)
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate.Name = "dtpApplicationInitialHighSchoolTranscriptReceivedDate"
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate.NullValue = " "
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate.TabIndex = 32
    	Me.dtpApplicationInitialHighSchoolTranscriptReceivedDate.Value = Nothing
    	'
    	'dtpApplicationReceivedDate
    	'
    	Me.dtpApplicationReceivedDate.CustomFormat = ""
    	Me.dtpApplicationReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationReceivedDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationReceivedDate.Location = New System.Drawing.Point(211, 19)
    	Me.dtpApplicationReceivedDate.Name = "dtpApplicationReceivedDate"
    	Me.dtpApplicationReceivedDate.NullValue = " "
    	Me.dtpApplicationReceivedDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationReceivedDate.TabIndex = 31
    	Me.dtpApplicationReceivedDate.Value = Nothing
    	'
    	'lblApplicationDefermentDecisionDate
    	'
    	Me.lblApplicationDefermentDecisionDate.AutoSize = true
    	Me.lblApplicationDefermentDecisionDate.Location = New System.Drawing.Point(8, 436)
    	Me.lblApplicationDefermentDecisionDate.Name = "lblApplicationDefermentDecisionDate"
    	Me.lblApplicationDefermentDecisionDate.Size = New System.Drawing.Size(126, 13)
    	Me.lblApplicationDefermentDecisionDate.TabIndex = 107
    	Me.lblApplicationDefermentDecisionDate.Text = "Deferment Decision Date"
    	'
    	'lblApplicationDefermentRequestReceivedDate
    	'
    	Me.lblApplicationDefermentRequestReceivedDate.AutoSize = true
    	Me.lblApplicationDefermentRequestReceivedDate.Location = New System.Drawing.Point(8, 410)
    	Me.lblApplicationDefermentRequestReceivedDate.Name = "lblApplicationDefermentRequestReceivedDate"
    	Me.lblApplicationDefermentRequestReceivedDate.Size = New System.Drawing.Size(148, 13)
    	Me.lblApplicationDefermentRequestReceivedDate.TabIndex = 105
    	Me.lblApplicationDefermentRequestReceivedDate.Text = "Deferment Request Received"
    	'
    	'lblApplicationLoaDecisionDate
    	'
    	Me.lblApplicationLoaDecisionDate.AutoSize = true
    	Me.lblApplicationLoaDecisionDate.Location = New System.Drawing.Point(8, 384)
    	Me.lblApplicationLoaDecisionDate.Name = "lblApplicationLoaDecisionDate"
    	Me.lblApplicationLoaDecisionDate.Size = New System.Drawing.Size(98, 13)
    	Me.lblApplicationLoaDecisionDate.TabIndex = 103
    	Me.lblApplicationLoaDecisionDate.Text = "LOA Decision Date"
    	'
    	'lblApplicationLoaRequestReceivedDate
    	'
    	Me.lblApplicationLoaRequestReceivedDate.AutoSize = true
    	Me.lblApplicationLoaRequestReceivedDate.Location = New System.Drawing.Point(8, 358)
    	Me.lblApplicationLoaRequestReceivedDate.Name = "lblApplicationLoaRequestReceivedDate"
    	Me.lblApplicationLoaRequestReceivedDate.Size = New System.Drawing.Size(120, 13)
    	Me.lblApplicationLoaRequestReceivedDate.TabIndex = 101
    	Me.lblApplicationLoaRequestReceivedDate.Text = "LOA Request Received"
    	'
    	'lblApplicationAppealDecisionDate
    	'
    	Me.lblApplicationAppealDecisionDate.AutoSize = true
    	Me.lblApplicationAppealDecisionDate.Location = New System.Drawing.Point(6, 309)
    	Me.lblApplicationAppealDecisionDate.Name = "lblApplicationAppealDecisionDate"
    	Me.lblApplicationAppealDecisionDate.Size = New System.Drawing.Size(110, 13)
    	Me.lblApplicationAppealDecisionDate.TabIndex = 99
    	Me.lblApplicationAppealDecisionDate.Text = "Appeal Decision Date"
    	'
    	'lblApplicationAppealReceivedDate
    	'
    	Me.lblApplicationAppealReceivedDate.AutoSize = true
    	Me.lblApplicationAppealReceivedDate.Location = New System.Drawing.Point(6, 283)
    	Me.lblApplicationAppealReceivedDate.Name = "lblApplicationAppealReceivedDate"
    	Me.lblApplicationAppealReceivedDate.Size = New System.Drawing.Size(89, 13)
    	Me.lblApplicationAppealReceivedDate.TabIndex = 97
    	Me.lblApplicationAppealReceivedDate.Text = "Appeal Received"
    	'
    	'lblApplicationFinalCollegeTranscriptReceivedDate
    	'
    	Me.lblApplicationFinalCollegeTranscriptReceivedDate.AutoSize = true
    	Me.lblApplicationFinalCollegeTranscriptReceivedDate.Location = New System.Drawing.Point(6, 178)
    	Me.lblApplicationFinalCollegeTranscriptReceivedDate.Name = "lblApplicationFinalCollegeTranscriptReceivedDate"
    	Me.lblApplicationFinalCollegeTranscriptReceivedDate.Size = New System.Drawing.Size(166, 13)
    	Me.lblApplicationFinalCollegeTranscriptReceivedDate.TabIndex = 7
    	Me.lblApplicationFinalCollegeTranscriptReceivedDate.Text = "Final College Transcript Received"
    	'
    	'lblApplicationProofOfEnrollmentReceivedDate
    	'
    	Me.lblApplicationProofOfEnrollmentReceivedDate.AutoSize = true
    	Me.lblApplicationProofOfEnrollmentReceivedDate.Location = New System.Drawing.Point(6, 231)
    	Me.lblApplicationProofOfEnrollmentReceivedDate.Name = "lblApplicationProofOfEnrollmentReceivedDate"
    	Me.lblApplicationProofOfEnrollmentReceivedDate.Size = New System.Drawing.Size(145, 13)
    	Me.lblApplicationProofOfEnrollmentReceivedDate.TabIndex = 6
    	Me.lblApplicationProofOfEnrollmentReceivedDate.Text = "Proof of Enrollment Received"
    	'
    	'lblApplicationConditionalAcceptanceFormReceivedDate
    	'
    	Me.lblApplicationConditionalAcceptanceFormReceivedDate.AutoSize = true
    	Me.lblApplicationConditionalAcceptanceFormReceivedDate.Location = New System.Drawing.Point(6, 205)
    	Me.lblApplicationConditionalAcceptanceFormReceivedDate.Name = "lblApplicationConditionalAcceptanceFormReceivedDate"
    	Me.lblApplicationConditionalAcceptanceFormReceivedDate.Size = New System.Drawing.Size(195, 13)
    	Me.lblApplicationConditionalAcceptanceFormReceivedDate.TabIndex = 5
    	Me.lblApplicationConditionalAcceptanceFormReceivedDate.Text = "Conditional Acceptance Form Received"
    	'
    	'lblApplicationSignaturePageReceivedDate
    	'
    	Me.lblApplicationSignaturePageReceivedDate.AutoSize = true
    	Me.lblApplicationSignaturePageReceivedDate.Location = New System.Drawing.Point(6, 127)
    	Me.lblApplicationSignaturePageReceivedDate.Name = "lblApplicationSignaturePageReceivedDate"
    	Me.lblApplicationSignaturePageReceivedDate.Size = New System.Drawing.Size(129, 13)
    	Me.lblApplicationSignaturePageReceivedDate.TabIndex = 4
    	Me.lblApplicationSignaturePageReceivedDate.Text = "Signature Page Received"
    	'
    	'lblApplicationInitialCollegeTranscriptReceivedDate
    	'
    	Me.lblApplicationInitialCollegeTranscriptReceivedDate.AutoSize = true
    	Me.lblApplicationInitialCollegeTranscriptReceivedDate.Location = New System.Drawing.Point(6, 75)
    	Me.lblApplicationInitialCollegeTranscriptReceivedDate.Name = "lblApplicationInitialCollegeTranscriptReceivedDate"
    	Me.lblApplicationInitialCollegeTranscriptReceivedDate.Size = New System.Drawing.Size(168, 13)
    	Me.lblApplicationInitialCollegeTranscriptReceivedDate.TabIndex = 3
    	Me.lblApplicationInitialCollegeTranscriptReceivedDate.Text = "Initial College Transcript Received"
    	'
    	'lblApplicationInitialHighSchoolTranscriptReceivedDate
    	'
    	Me.lblApplicationInitialHighSchoolTranscriptReceivedDate.AutoSize = true
    	Me.lblApplicationInitialHighSchoolTranscriptReceivedDate.Location = New System.Drawing.Point(6, 49)
    	Me.lblApplicationInitialHighSchoolTranscriptReceivedDate.Name = "lblApplicationInitialHighSchoolTranscriptReceivedDate"
    	Me.lblApplicationInitialHighSchoolTranscriptReceivedDate.Size = New System.Drawing.Size(191, 13)
    	Me.lblApplicationInitialHighSchoolTranscriptReceivedDate.TabIndex = 1
    	Me.lblApplicationInitialHighSchoolTranscriptReceivedDate.Text = "Initial High School Transcript Received"
    	'
    	'lblApplicationReceivedDate
    	'
    	Me.lblApplicationReceivedDate.AutoSize = true
    	Me.lblApplicationReceivedDate.Location = New System.Drawing.Point(6, 23)
    	Me.lblApplicationReceivedDate.Name = "lblApplicationReceivedDate"
    	Me.lblApplicationReceivedDate.Size = New System.Drawing.Size(108, 13)
    	Me.lblApplicationReceivedDate.TabIndex = 0
    	Me.lblApplicationReceivedDate.Text = "Application Received"
    	'
    	'grpApplicationReviewStatus
    	'
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationFinalTranscriptReviewStartStop)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationSecondTranscriptReviewStartStop)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationInitialTranscriptReviewStartStop)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.lblApplicationSecondQuickReviewInProgress)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.lblApplicationFirstQuickReviewInProgress)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.lblApplicationUespAwardReviewInProgress)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationSecondQuickReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationSecondQuickReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationSecondQuickReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationFirstQuickReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationFirstQuickReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationFirstQuickReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationSecondTranscriptReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationSecondTranscriptReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationSecondTranscriptReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.lblReviewStatusUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.lblReviewStatusDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationUespAwardReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationExemplaryAwardReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationBaseAwardReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationInitialAwardReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationCategoryReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationClassReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationFinalTranscriptReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationInitialTranscriptReviewDate)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationUespAwardReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationUespAwardReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationExemplaryAwardReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationExemplaryAwardReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationBaseAwardReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationBaseAwardReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationInitialAwardReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationInitialAwardReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationCategoryReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationCategoryReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationClassReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationClassReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationFinalTranscriptReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationFinalTranscriptReview)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.txtApplicationInitialTranscriptReviewUserId)
    	Me.grpApplicationReviewStatus.Controls.Add(Me.chkApplicationInitialTranscriptReview)
    	Me.grpApplicationReviewStatus.Location = New System.Drawing.Point(8, 240)
    	Me.grpApplicationReviewStatus.Name = "grpApplicationReviewStatus"
    	Me.grpApplicationReviewStatus.Size = New System.Drawing.Size(420, 322)
    	Me.grpApplicationReviewStatus.TabIndex = 2
    	Me.grpApplicationReviewStatus.TabStop = false
    	Me.grpApplicationReviewStatus.Text = "Review Status"
    	'
    	'chkApplicationFinalTranscriptReviewStartStop
    	'
    	Me.chkApplicationFinalTranscriptReviewStartStop.Appearance = System.Windows.Forms.Appearance.Button
    	Me.chkApplicationFinalTranscriptReviewStartStop.AutoSize = true
    	Me.chkApplicationFinalTranscriptReviewStartStop.BackColor = System.Drawing.Color.LightGreen
    	Me.chkApplicationFinalTranscriptReviewStartStop.Location = New System.Drawing.Point(177, 84)
    	Me.chkApplicationFinalTranscriptReviewStartStop.Name = "chkApplicationFinalTranscriptReviewStartStop"
    	Me.chkApplicationFinalTranscriptReviewStartStop.Size = New System.Drawing.Size(39, 23)
    	Me.chkApplicationFinalTranscriptReviewStartStop.TabIndex = 112
    	Me.chkApplicationFinalTranscriptReviewStartStop.Text = "Start"
    	Me.chkApplicationFinalTranscriptReviewStartStop.UseVisualStyleBackColor = false
    	'
    	'chkApplicationSecondTranscriptReviewStartStop
    	'
    	Me.chkApplicationSecondTranscriptReviewStartStop.Appearance = System.Windows.Forms.Appearance.Button
    	Me.chkApplicationSecondTranscriptReviewStartStop.AutoSize = true
    	Me.chkApplicationSecondTranscriptReviewStartStop.BackColor = System.Drawing.Color.LightGreen
    	Me.chkApplicationSecondTranscriptReviewStartStop.Location = New System.Drawing.Point(177, 56)
    	Me.chkApplicationSecondTranscriptReviewStartStop.Name = "chkApplicationSecondTranscriptReviewStartStop"
    	Me.chkApplicationSecondTranscriptReviewStartStop.Size = New System.Drawing.Size(39, 23)
    	Me.chkApplicationSecondTranscriptReviewStartStop.TabIndex = 111
    	Me.chkApplicationSecondTranscriptReviewStartStop.Text = "Start"
    	Me.chkApplicationSecondTranscriptReviewStartStop.UseVisualStyleBackColor = false
    	'
    	'chkApplicationInitialTranscriptReviewStartStop
    	'
    	Me.chkApplicationInitialTranscriptReviewStartStop.Appearance = System.Windows.Forms.Appearance.Button
    	Me.chkApplicationInitialTranscriptReviewStartStop.AutoSize = true
    	Me.chkApplicationInitialTranscriptReviewStartStop.BackColor = System.Drawing.Color.LightGreen
    	Me.chkApplicationInitialTranscriptReviewStartStop.Location = New System.Drawing.Point(177, 30)
    	Me.chkApplicationInitialTranscriptReviewStartStop.Name = "chkApplicationInitialTranscriptReviewStartStop"
    	Me.chkApplicationInitialTranscriptReviewStartStop.Size = New System.Drawing.Size(39, 23)
    	Me.chkApplicationInitialTranscriptReviewStartStop.TabIndex = 56
    	Me.chkApplicationInitialTranscriptReviewStartStop.Text = "Start"
    	Me.chkApplicationInitialTranscriptReviewStartStop.UseVisualStyleBackColor = false
    	'
    	'lblApplicationSecondQuickReviewInProgress
    	'
    	Me.lblApplicationSecondQuickReviewInProgress.AutoSize = true
    	Me.lblApplicationSecondQuickReviewInProgress.Location = New System.Drawing.Point(169, 295)
    	Me.lblApplicationSecondQuickReviewInProgress.Name = "lblApplicationSecondQuickReviewInProgress"
    	Me.lblApplicationSecondQuickReviewInProgress.Size = New System.Drawing.Size(60, 13)
    	Me.lblApplicationSecondQuickReviewInProgress.TabIndex = 110
    	Me.lblApplicationSecondQuickReviewInProgress.Text = "In Progress"
    	'
    	'lblApplicationFirstQuickReviewInProgress
    	'
    	Me.lblApplicationFirstQuickReviewInProgress.AutoSize = true
    	Me.lblApplicationFirstQuickReviewInProgress.Location = New System.Drawing.Point(169, 191)
    	Me.lblApplicationFirstQuickReviewInProgress.Name = "lblApplicationFirstQuickReviewInProgress"
    	Me.lblApplicationFirstQuickReviewInProgress.Size = New System.Drawing.Size(60, 13)
    	Me.lblApplicationFirstQuickReviewInProgress.TabIndex = 109
    	Me.lblApplicationFirstQuickReviewInProgress.Text = "In Progress"
    	'
    	'lblApplicationUespAwardReviewInProgress
    	'
    	Me.lblApplicationUespAwardReviewInProgress.AutoSize = true
    	Me.lblApplicationUespAwardReviewInProgress.Location = New System.Drawing.Point(169, 269)
    	Me.lblApplicationUespAwardReviewInProgress.Name = "lblApplicationUespAwardReviewInProgress"
    	Me.lblApplicationUespAwardReviewInProgress.Size = New System.Drawing.Size(60, 13)
    	Me.lblApplicationUespAwardReviewInProgress.TabIndex = 108
    	Me.lblApplicationUespAwardReviewInProgress.Text = "In Progress"
    	'
    	'txtApplicationSecondQuickReviewDate
    	'
    	Me.txtApplicationSecondQuickReviewDate.Location = New System.Drawing.Point(166, 292)
    	Me.txtApplicationSecondQuickReviewDate.Name = "txtApplicationSecondQuickReviewDate"
    	Me.txtApplicationSecondQuickReviewDate.ReadOnly = true
    	Me.txtApplicationSecondQuickReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationSecondQuickReviewDate.TabIndex = 0
    	Me.txtApplicationSecondQuickReviewDate.TabStop = false
    	'
    	'txtApplicationSecondQuickReviewUserId
    	'
    	Me.txtApplicationSecondQuickReviewUserId.Location = New System.Drawing.Point(248, 292)
    	Me.txtApplicationSecondQuickReviewUserId.Name = "txtApplicationSecondQuickReviewUserId"
    	Me.txtApplicationSecondQuickReviewUserId.ReadOnly = true
    	Me.txtApplicationSecondQuickReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationSecondQuickReviewUserId.TabIndex = 0
    	Me.txtApplicationSecondQuickReviewUserId.TabStop = false
    	'
    	'chkApplicationSecondQuickReview
    	'
    	Me.chkApplicationSecondQuickReview.AutoSize = true
    	Me.chkApplicationSecondQuickReview.Location = New System.Drawing.Point(7, 294)
    	Me.chkApplicationSecondQuickReview.Name = "chkApplicationSecondQuickReview"
    	Me.chkApplicationSecondQuickReview.Size = New System.Drawing.Size(133, 17)
    	Me.chkApplicationSecondQuickReview.TabIndex = 30
    	Me.chkApplicationSecondQuickReview.Text = "Second Quick Review"
    	Me.chkApplicationSecondQuickReview.UseVisualStyleBackColor = true
    	'
    	'txtApplicationFirstQuickReviewDate
    	'
    	Me.txtApplicationFirstQuickReviewDate.Location = New System.Drawing.Point(166, 188)
    	Me.txtApplicationFirstQuickReviewDate.Name = "txtApplicationFirstQuickReviewDate"
    	Me.txtApplicationFirstQuickReviewDate.ReadOnly = true
    	Me.txtApplicationFirstQuickReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationFirstQuickReviewDate.TabIndex = 0
    	Me.txtApplicationFirstQuickReviewDate.TabStop = false
    	'
    	'txtApplicationFirstQuickReviewUserId
    	'
    	Me.txtApplicationFirstQuickReviewUserId.Location = New System.Drawing.Point(248, 188)
    	Me.txtApplicationFirstQuickReviewUserId.Name = "txtApplicationFirstQuickReviewUserId"
    	Me.txtApplicationFirstQuickReviewUserId.ReadOnly = true
    	Me.txtApplicationFirstQuickReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationFirstQuickReviewUserId.TabIndex = 0
    	Me.txtApplicationFirstQuickReviewUserId.TabStop = false
    	'
    	'chkApplicationFirstQuickReview
    	'
    	Me.chkApplicationFirstQuickReview.AutoSize = true
    	Me.chkApplicationFirstQuickReview.Location = New System.Drawing.Point(7, 190)
    	Me.chkApplicationFirstQuickReview.Name = "chkApplicationFirstQuickReview"
    	Me.chkApplicationFirstQuickReview.Size = New System.Drawing.Size(115, 17)
    	Me.chkApplicationFirstQuickReview.TabIndex = 26
    	Me.chkApplicationFirstQuickReview.Text = "First Quick Review"
    	Me.chkApplicationFirstQuickReview.UseVisualStyleBackColor = true
    	'
    	'txtApplicationSecondTranscriptReviewDate
    	'
    	Me.txtApplicationSecondTranscriptReviewDate.Location = New System.Drawing.Point(166, 58)
    	Me.txtApplicationSecondTranscriptReviewDate.Name = "txtApplicationSecondTranscriptReviewDate"
    	Me.txtApplicationSecondTranscriptReviewDate.ReadOnly = true
    	Me.txtApplicationSecondTranscriptReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationSecondTranscriptReviewDate.TabIndex = 0
    	Me.txtApplicationSecondTranscriptReviewDate.TabStop = false
    	'
    	'txtApplicationSecondTranscriptReviewUserId
    	'
    	Me.txtApplicationSecondTranscriptReviewUserId.Location = New System.Drawing.Point(248, 58)
    	Me.txtApplicationSecondTranscriptReviewUserId.Name = "txtApplicationSecondTranscriptReviewUserId"
    	Me.txtApplicationSecondTranscriptReviewUserId.ReadOnly = true
    	Me.txtApplicationSecondTranscriptReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationSecondTranscriptReviewUserId.TabIndex = 0
    	Me.txtApplicationSecondTranscriptReviewUserId.TabStop = false
    	'
    	'chkApplicationSecondTranscriptReview
    	'
    	Me.chkApplicationSecondTranscriptReview.AutoSize = true
    	Me.chkApplicationSecondTranscriptReview.Location = New System.Drawing.Point(7, 60)
    	Me.chkApplicationSecondTranscriptReview.Name = "chkApplicationSecondTranscriptReview"
    	Me.chkApplicationSecondTranscriptReview.Size = New System.Drawing.Size(152, 17)
    	Me.chkApplicationSecondTranscriptReview.TabIndex = 21
    	Me.chkApplicationSecondTranscriptReview.Text = "Second Transcript Review"
    	Me.chkApplicationSecondTranscriptReview.UseVisualStyleBackColor = true
    	'
    	'lblReviewStatusUserId
    	'
    	Me.lblReviewStatusUserId.AutoSize = true
    	Me.lblReviewStatusUserId.Location = New System.Drawing.Point(261, 16)
    	Me.lblReviewStatusUserId.Name = "lblReviewStatusUserId"
    	Me.lblReviewStatusUserId.Size = New System.Drawing.Size(43, 13)
    	Me.lblReviewStatusUserId.TabIndex = 98
    	Me.lblReviewStatusUserId.Text = "User ID"
    	'
    	'lblReviewStatusDate
    	'
    	Me.lblReviewStatusDate.AutoSize = true
    	Me.lblReviewStatusDate.Location = New System.Drawing.Point(181, 16)
    	Me.lblReviewStatusDate.Name = "lblReviewStatusDate"
    	Me.lblReviewStatusDate.Size = New System.Drawing.Size(30, 13)
    	Me.lblReviewStatusDate.TabIndex = 97
    	Me.lblReviewStatusDate.Text = "Date"
    	'
    	'txtApplicationUespAwardReviewDate
    	'
    	Me.txtApplicationUespAwardReviewDate.Location = New System.Drawing.Point(166, 266)
    	Me.txtApplicationUespAwardReviewDate.Name = "txtApplicationUespAwardReviewDate"
    	Me.txtApplicationUespAwardReviewDate.ReadOnly = true
    	Me.txtApplicationUespAwardReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationUespAwardReviewDate.TabIndex = 0
    	Me.txtApplicationUespAwardReviewDate.TabStop = false
    	'
    	'txtApplicationExemplaryAwardReviewDate
    	'
    	Me.txtApplicationExemplaryAwardReviewDate.Location = New System.Drawing.Point(166, 240)
    	Me.txtApplicationExemplaryAwardReviewDate.Name = "txtApplicationExemplaryAwardReviewDate"
    	Me.txtApplicationExemplaryAwardReviewDate.ReadOnly = true
    	Me.txtApplicationExemplaryAwardReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationExemplaryAwardReviewDate.TabIndex = 0
    	Me.txtApplicationExemplaryAwardReviewDate.TabStop = false
    	'
    	'txtApplicationBaseAwardReviewDate
    	'
    	Me.txtApplicationBaseAwardReviewDate.Location = New System.Drawing.Point(166, 214)
    	Me.txtApplicationBaseAwardReviewDate.Name = "txtApplicationBaseAwardReviewDate"
    	Me.txtApplicationBaseAwardReviewDate.ReadOnly = true
    	Me.txtApplicationBaseAwardReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationBaseAwardReviewDate.TabIndex = 0
    	Me.txtApplicationBaseAwardReviewDate.TabStop = false
    	'
    	'txtApplicationInitialAwardReviewDate
    	'
    	Me.txtApplicationInitialAwardReviewDate.Location = New System.Drawing.Point(166, 162)
    	Me.txtApplicationInitialAwardReviewDate.Name = "txtApplicationInitialAwardReviewDate"
    	Me.txtApplicationInitialAwardReviewDate.ReadOnly = true
    	Me.txtApplicationInitialAwardReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationInitialAwardReviewDate.TabIndex = 0
    	Me.txtApplicationInitialAwardReviewDate.TabStop = false
    	'
    	'txtApplicationCategoryReviewDate
    	'
    	Me.txtApplicationCategoryReviewDate.Location = New System.Drawing.Point(166, 136)
    	Me.txtApplicationCategoryReviewDate.Name = "txtApplicationCategoryReviewDate"
    	Me.txtApplicationCategoryReviewDate.ReadOnly = true
    	Me.txtApplicationCategoryReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationCategoryReviewDate.TabIndex = 0
    	Me.txtApplicationCategoryReviewDate.TabStop = false
    	'
    	'txtApplicationClassReviewDate
    	'
    	Me.txtApplicationClassReviewDate.Location = New System.Drawing.Point(166, 110)
    	Me.txtApplicationClassReviewDate.Name = "txtApplicationClassReviewDate"
    	Me.txtApplicationClassReviewDate.ReadOnly = true
    	Me.txtApplicationClassReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationClassReviewDate.TabIndex = 0
    	Me.txtApplicationClassReviewDate.TabStop = false
    	'
    	'txtApplicationFinalTranscriptReviewDate
    	'
    	Me.txtApplicationFinalTranscriptReviewDate.Location = New System.Drawing.Point(166, 84)
    	Me.txtApplicationFinalTranscriptReviewDate.Name = "txtApplicationFinalTranscriptReviewDate"
    	Me.txtApplicationFinalTranscriptReviewDate.ReadOnly = true
    	Me.txtApplicationFinalTranscriptReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationFinalTranscriptReviewDate.TabIndex = 0
    	Me.txtApplicationFinalTranscriptReviewDate.TabStop = false
    	'
    	'txtApplicationInitialTranscriptReviewDate
    	'
    	Me.txtApplicationInitialTranscriptReviewDate.Location = New System.Drawing.Point(166, 32)
    	Me.txtApplicationInitialTranscriptReviewDate.Name = "txtApplicationInitialTranscriptReviewDate"
    	Me.txtApplicationInitialTranscriptReviewDate.ReadOnly = true
    	Me.txtApplicationInitialTranscriptReviewDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationInitialTranscriptReviewDate.TabIndex = 0
    	Me.txtApplicationInitialTranscriptReviewDate.TabStop = false
    	'
    	'txtApplicationUespAwardReviewUserId
    	'
    	Me.txtApplicationUespAwardReviewUserId.Location = New System.Drawing.Point(248, 265)
    	Me.txtApplicationUespAwardReviewUserId.Name = "txtApplicationUespAwardReviewUserId"
    	Me.txtApplicationUespAwardReviewUserId.ReadOnly = true
    	Me.txtApplicationUespAwardReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationUespAwardReviewUserId.TabIndex = 0
    	Me.txtApplicationUespAwardReviewUserId.TabStop = false
    	'
    	'chkApplicationUespAwardReview
    	'
    	Me.chkApplicationUespAwardReview.AutoSize = true
    	Me.chkApplicationUespAwardReview.Location = New System.Drawing.Point(7, 268)
    	Me.chkApplicationUespAwardReview.Name = "chkApplicationUespAwardReview"
    	Me.chkApplicationUespAwardReview.Size = New System.Drawing.Size(127, 17)
    	Me.chkApplicationUespAwardReview.TabIndex = 29
    	Me.chkApplicationUespAwardReview.Text = "UESP Award Review"
    	Me.chkApplicationUespAwardReview.UseVisualStyleBackColor = true
    	'
    	'txtApplicationExemplaryAwardReviewUserId
    	'
    	Me.txtApplicationExemplaryAwardReviewUserId.Location = New System.Drawing.Point(248, 239)
    	Me.txtApplicationExemplaryAwardReviewUserId.Name = "txtApplicationExemplaryAwardReviewUserId"
    	Me.txtApplicationExemplaryAwardReviewUserId.ReadOnly = true
    	Me.txtApplicationExemplaryAwardReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationExemplaryAwardReviewUserId.TabIndex = 0
    	Me.txtApplicationExemplaryAwardReviewUserId.TabStop = false
    	'
    	'chkApplicationExemplaryAwardReview
    	'
    	Me.chkApplicationExemplaryAwardReview.AutoSize = true
    	Me.chkApplicationExemplaryAwardReview.Location = New System.Drawing.Point(7, 242)
    	Me.chkApplicationExemplaryAwardReview.Name = "chkApplicationExemplaryAwardReview"
    	Me.chkApplicationExemplaryAwardReview.Size = New System.Drawing.Size(146, 17)
    	Me.chkApplicationExemplaryAwardReview.TabIndex = 28
    	Me.chkApplicationExemplaryAwardReview.Text = "Exemplary Award Review"
    	Me.chkApplicationExemplaryAwardReview.UseVisualStyleBackColor = true
    	'
    	'txtApplicationBaseAwardReviewUserId
    	'
    	Me.txtApplicationBaseAwardReviewUserId.Location = New System.Drawing.Point(248, 214)
    	Me.txtApplicationBaseAwardReviewUserId.Name = "txtApplicationBaseAwardReviewUserId"
    	Me.txtApplicationBaseAwardReviewUserId.ReadOnly = true
    	Me.txtApplicationBaseAwardReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationBaseAwardReviewUserId.TabIndex = 0
    	Me.txtApplicationBaseAwardReviewUserId.TabStop = false
    	'
    	'chkApplicationBaseAwardReview
    	'
    	Me.chkApplicationBaseAwardReview.AutoSize = true
    	Me.chkApplicationBaseAwardReview.Location = New System.Drawing.Point(7, 216)
    	Me.chkApplicationBaseAwardReview.Name = "chkApplicationBaseAwardReview"
    	Me.chkApplicationBaseAwardReview.Size = New System.Drawing.Size(122, 17)
    	Me.chkApplicationBaseAwardReview.TabIndex = 27
    	Me.chkApplicationBaseAwardReview.Text = "Base Award Review"
    	Me.chkApplicationBaseAwardReview.UseVisualStyleBackColor = true
    	'
    	'txtApplicationInitialAwardReviewUserId
    	'
    	Me.txtApplicationInitialAwardReviewUserId.Location = New System.Drawing.Point(248, 162)
    	Me.txtApplicationInitialAwardReviewUserId.Name = "txtApplicationInitialAwardReviewUserId"
    	Me.txtApplicationInitialAwardReviewUserId.ReadOnly = true
    	Me.txtApplicationInitialAwardReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationInitialAwardReviewUserId.TabIndex = 0
    	Me.txtApplicationInitialAwardReviewUserId.TabStop = false
    	'
    	'chkApplicationInitialAwardReview
    	'
    	Me.chkApplicationInitialAwardReview.AutoSize = true
    	Me.chkApplicationInitialAwardReview.Location = New System.Drawing.Point(7, 164)
    	Me.chkApplicationInitialAwardReview.Name = "chkApplicationInitialAwardReview"
    	Me.chkApplicationInitialAwardReview.Size = New System.Drawing.Size(122, 17)
    	Me.chkApplicationInitialAwardReview.TabIndex = 25
    	Me.chkApplicationInitialAwardReview.Text = "Initial Award Review"
    	Me.chkApplicationInitialAwardReview.UseVisualStyleBackColor = true
    	'
    	'txtApplicationCategoryReviewUserId
    	'
    	Me.txtApplicationCategoryReviewUserId.Location = New System.Drawing.Point(248, 135)
    	Me.txtApplicationCategoryReviewUserId.Name = "txtApplicationCategoryReviewUserId"
    	Me.txtApplicationCategoryReviewUserId.ReadOnly = true
    	Me.txtApplicationCategoryReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationCategoryReviewUserId.TabIndex = 0
    	Me.txtApplicationCategoryReviewUserId.TabStop = false
    	'
    	'chkApplicationCategoryReview
    	'
    	Me.chkApplicationCategoryReview.AutoSize = true
    	Me.chkApplicationCategoryReview.Location = New System.Drawing.Point(7, 138)
    	Me.chkApplicationCategoryReview.Name = "chkApplicationCategoryReview"
    	Me.chkApplicationCategoryReview.Size = New System.Drawing.Size(107, 17)
    	Me.chkApplicationCategoryReview.TabIndex = 24
    	Me.chkApplicationCategoryReview.Text = "Category Review"
    	Me.chkApplicationCategoryReview.UseVisualStyleBackColor = true
    	'
    	'txtApplicationClassReviewUserId
    	'
    	Me.txtApplicationClassReviewUserId.Location = New System.Drawing.Point(248, 110)
    	Me.txtApplicationClassReviewUserId.Name = "txtApplicationClassReviewUserId"
    	Me.txtApplicationClassReviewUserId.ReadOnly = true
    	Me.txtApplicationClassReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationClassReviewUserId.TabIndex = 0
    	Me.txtApplicationClassReviewUserId.TabStop = false
    	'
    	'chkApplicationClassReview
    	'
    	Me.chkApplicationClassReview.AutoSize = true
    	Me.chkApplicationClassReview.Location = New System.Drawing.Point(7, 112)
    	Me.chkApplicationClassReview.Name = "chkApplicationClassReview"
    	Me.chkApplicationClassReview.Size = New System.Drawing.Size(90, 17)
    	Me.chkApplicationClassReview.TabIndex = 23
    	Me.chkApplicationClassReview.Text = "Class Review"
    	Me.chkApplicationClassReview.UseVisualStyleBackColor = true
    	'
    	'txtApplicationFinalTranscriptReviewUserId
    	'
    	Me.txtApplicationFinalTranscriptReviewUserId.Location = New System.Drawing.Point(248, 83)
    	Me.txtApplicationFinalTranscriptReviewUserId.Name = "txtApplicationFinalTranscriptReviewUserId"
    	Me.txtApplicationFinalTranscriptReviewUserId.ReadOnly = true
    	Me.txtApplicationFinalTranscriptReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationFinalTranscriptReviewUserId.TabIndex = 0
    	Me.txtApplicationFinalTranscriptReviewUserId.TabStop = false
    	'
    	'chkApplicationFinalTranscriptReview
    	'
    	Me.chkApplicationFinalTranscriptReview.AutoSize = true
    	Me.chkApplicationFinalTranscriptReview.Location = New System.Drawing.Point(7, 86)
    	Me.chkApplicationFinalTranscriptReview.Name = "chkApplicationFinalTranscriptReview"
    	Me.chkApplicationFinalTranscriptReview.Size = New System.Drawing.Size(137, 17)
    	Me.chkApplicationFinalTranscriptReview.TabIndex = 22
    	Me.chkApplicationFinalTranscriptReview.Text = "Final Transcript Review"
    	Me.chkApplicationFinalTranscriptReview.UseVisualStyleBackColor = true
    	'
    	'txtApplicationInitialTranscriptReviewUserId
    	'
    	Me.txtApplicationInitialTranscriptReviewUserId.Location = New System.Drawing.Point(248, 32)
    	Me.txtApplicationInitialTranscriptReviewUserId.Name = "txtApplicationInitialTranscriptReviewUserId"
    	Me.txtApplicationInitialTranscriptReviewUserId.ReadOnly = true
    	Me.txtApplicationInitialTranscriptReviewUserId.Size = New System.Drawing.Size(161, 20)
    	Me.txtApplicationInitialTranscriptReviewUserId.TabIndex = 0
    	Me.txtApplicationInitialTranscriptReviewUserId.TabStop = false
    	'
    	'chkApplicationInitialTranscriptReview
    	'
    	Me.chkApplicationInitialTranscriptReview.AutoSize = true
    	Me.chkApplicationInitialTranscriptReview.Location = New System.Drawing.Point(7, 34)
    	Me.chkApplicationInitialTranscriptReview.Name = "chkApplicationInitialTranscriptReview"
    	Me.chkApplicationInitialTranscriptReview.Size = New System.Drawing.Size(139, 17)
    	Me.chkApplicationInitialTranscriptReview.TabIndex = 20
    	Me.chkApplicationInitialTranscriptReview.Text = "Initial Transcript Review"
    	Me.chkApplicationInitialTranscriptReview.UseVisualStyleBackColor = true
    	'
    	'grpApplicationClasses
    	'
    	Me.grpApplicationClasses.Controls.Add(Me.tabControlClasses)
    	Me.grpApplicationClasses.Location = New System.Drawing.Point(8, 1094)
    	Me.grpApplicationClasses.Name = "grpApplicationClasses"
    	Me.grpApplicationClasses.Size = New System.Drawing.Size(906, 617)
    	Me.grpApplicationClasses.TabIndex = 53
    	Me.grpApplicationClasses.TabStop = false
    	Me.grpApplicationClasses.Text = "Classes"
    	'
    	'tabControlClasses
    	'
    	Me.tabControlClasses.Controls.Add(Me.tabEnglish)
    	Me.tabControlClasses.Controls.Add(Me.tabMathematics)
    	Me.tabControlClasses.Controls.Add(Me.tabScienceWithLab)
    	Me.tabControlClasses.Controls.Add(Me.tabSocialScience)
    	Me.tabControlClasses.Controls.Add(Me.tabForeignLanguage)
    	Me.tabControlClasses.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.tabControlClasses.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
    	Me.tabControlClasses.Location = New System.Drawing.Point(3, 16)
    	Me.tabControlClasses.Name = "tabControlClasses"
    	Me.tabControlClasses.Padding = New System.Drawing.Point(15, 5)
    	Me.tabControlClasses.SelectedIndex = 0
    	Me.tabControlClasses.Size = New System.Drawing.Size(900, 598)
    	Me.tabControlClasses.TabIndex = 2
    	'
    	'tabEnglish
    	'
    	Me.tabEnglish.BackColor = System.Drawing.SystemColors.Control
    	Me.tabEnglish.Controls.Add(Me.Label5)
    	Me.tabEnglish.Controls.Add(Me.englishClass6)
    	Me.tabEnglish.Controls.Add(Me.englishClass5)
    	Me.tabEnglish.Controls.Add(Me.englishClass4)
    	Me.tabEnglish.Controls.Add(Me.englishClass3)
    	Me.tabEnglish.Controls.Add(Me.englishClass2)
    	Me.tabEnglish.Controls.Add(Me.englishClass1)
    	Me.tabEnglish.Controls.Add(Me.lblEnglishGrades)
    	Me.tabEnglish.Controls.Add(Me.txtEnglishVerifiedDate)
    	Me.tabEnglish.Controls.Add(Me.lblEnglishVerifiedDate)
    	Me.tabEnglish.Controls.Add(Me.txtEnglishVerifiedBy)
    	Me.tabEnglish.Controls.Add(Me.lblEnglishVerifiedBy)
    	Me.tabEnglish.Controls.Add(Me.radEnglishRequirementMetInProgress)
    	Me.tabEnglish.Controls.Add(Me.lblEnglishWeightDesignation)
    	Me.tabEnglish.Controls.Add(Me.radEnglishRequirementMetNo)
    	Me.tabEnglish.Controls.Add(Me.radEnglishRequirementMetYes)
    	Me.tabEnglish.Controls.Add(Me.lblEnglishRequirementMet)
    	Me.tabEnglish.Controls.Add(Me.lblEnglishTitle)
    	Me.tabEnglish.Controls.Add(Me.lblEnglishGradeLevel)
    	Me.tabEnglish.Controls.Add(Me.lblEnglishCredits)
    	Me.tabEnglish.Controls.Add(Me.lblEnglishWeightedAverageGrade)
    	Me.tabEnglish.ForeColor = System.Drawing.SystemColors.ControlText
    	Me.tabEnglish.Location = New System.Drawing.Point(4, 26)
    	Me.tabEnglish.Name = "tabEnglish"
    	Me.tabEnglish.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabEnglish.Size = New System.Drawing.Size(892, 568)
    	Me.tabEnglish.TabIndex = 0
    	Me.tabEnglish.Text = "English"
    	'
    	'Label5
    	'
    	Me.Label5.Location = New System.Drawing.Point(441, 64)
    	Me.Label5.Name = "Label5"
    	Me.Label5.Size = New System.Drawing.Size(51, 40)
    	Me.Label5.TabIndex = 83
    	Me.Label5.Text = "Year Taken (YY/YY)"
    	Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
    	'
    	'englishClass6
    	'
    	Me.englishClass6.Location = New System.Drawing.Point(6, 397)
    	Me.englishClass6.Name = "englishClass6"
    	Me.englishClass6.Size = New System.Drawing.Size(880, 52)
    	Me.englishClass6.TabIndex = 9
    	'
    	'englishClass5
    	'
    	Me.englishClass5.Location = New System.Drawing.Point(6, 339)
    	Me.englishClass5.Name = "englishClass5"
    	Me.englishClass5.Size = New System.Drawing.Size(880, 52)
    	Me.englishClass5.TabIndex = 8
    	'
    	'englishClass4
    	'
    	Me.englishClass4.Location = New System.Drawing.Point(6, 281)
    	Me.englishClass4.Name = "englishClass4"
    	Me.englishClass4.Size = New System.Drawing.Size(880, 52)
    	Me.englishClass4.TabIndex = 7
    	'
    	'englishClass3
    	'
    	Me.englishClass3.Location = New System.Drawing.Point(6, 223)
    	Me.englishClass3.Name = "englishClass3"
    	Me.englishClass3.Size = New System.Drawing.Size(880, 52)
    	Me.englishClass3.TabIndex = 6
    	'
    	'englishClass2
    	'
    	Me.englishClass2.Location = New System.Drawing.Point(6, 165)
    	Me.englishClass2.Name = "englishClass2"
    	Me.englishClass2.Size = New System.Drawing.Size(880, 52)
    	Me.englishClass2.TabIndex = 5
    	'
    	'englishClass1
    	'
    	Me.englishClass1.Location = New System.Drawing.Point(6, 107)
    	Me.englishClass1.Name = "englishClass1"
    	Me.englishClass1.Size = New System.Drawing.Size(880, 52)
    	Me.englishClass1.TabIndex = 4
    	'
    	'lblEnglishGrades
    	'
    	Me.lblEnglishGrades.AutoSize = true
    	Me.lblEnglishGrades.Location = New System.Drawing.Point(528, 91)
    	Me.lblEnglishGrades.Name = "lblEnglishGrades"
    	Me.lblEnglishGrades.Size = New System.Drawing.Size(279, 13)
    	Me.lblEnglishGrades.TabIndex = 82
    	Me.lblEnglishGrades.Text = "|---------------------------------------Grades-----------------------------------"& _ 
    	"----|"
    	'
    	'txtEnglishVerifiedDate
    	'
    	Me.txtEnglishVerifiedDate.Location = New System.Drawing.Point(282, 33)
    	Me.txtEnglishVerifiedDate.Name = "txtEnglishVerifiedDate"
    	Me.txtEnglishVerifiedDate.ReadOnly = true
    	Me.txtEnglishVerifiedDate.Size = New System.Drawing.Size(66, 20)
    	Me.txtEnglishVerifiedDate.TabIndex = 0
    	Me.txtEnglishVerifiedDate.TabStop = false
    	'
    	'lblEnglishVerifiedDate
    	'
    	Me.lblEnglishVerifiedDate.AutoSize = true
    	Me.lblEnglishVerifiedDate.Location = New System.Drawing.Point(246, 36)
    	Me.lblEnglishVerifiedDate.Name = "lblEnglishVerifiedDate"
    	Me.lblEnglishVerifiedDate.Size = New System.Drawing.Size(30, 13)
    	Me.lblEnglishVerifiedDate.TabIndex = 31
    	Me.lblEnglishVerifiedDate.Text = "Date"
    	'
    	'txtEnglishVerifiedBy
    	'
    	Me.txtEnglishVerifiedBy.Location = New System.Drawing.Point(70, 33)
    	Me.txtEnglishVerifiedBy.Name = "txtEnglishVerifiedBy"
    	Me.txtEnglishVerifiedBy.ReadOnly = true
    	Me.txtEnglishVerifiedBy.Size = New System.Drawing.Size(161, 20)
    	Me.txtEnglishVerifiedBy.TabIndex = 0
    	Me.txtEnglishVerifiedBy.TabStop = false
    	'
    	'lblEnglishVerifiedBy
    	'
    	Me.lblEnglishVerifiedBy.AutoSize = true
    	Me.lblEnglishVerifiedBy.Location = New System.Drawing.Point(7, 36)
    	Me.lblEnglishVerifiedBy.Name = "lblEnglishVerifiedBy"
    	Me.lblEnglishVerifiedBy.Size = New System.Drawing.Size(57, 13)
    	Me.lblEnglishVerifiedBy.TabIndex = 7
    	Me.lblEnglishVerifiedBy.Text = "Verified By"
    	'
    	'radEnglishRequirementMetInProgress
    	'
    	Me.radEnglishRequirementMetInProgress.AutoSize = true
    	Me.radEnglishRequirementMetInProgress.Location = New System.Drawing.Point(222, 10)
    	Me.radEnglishRequirementMetInProgress.Name = "radEnglishRequirementMetInProgress"
    	Me.radEnglishRequirementMetInProgress.Size = New System.Drawing.Size(78, 17)
    	Me.radEnglishRequirementMetInProgress.TabIndex = 3
    	Me.radEnglishRequirementMetInProgress.TabStop = true
    	Me.radEnglishRequirementMetInProgress.Text = "In Progress"
    	Me.radEnglishRequirementMetInProgress.UseVisualStyleBackColor = true
    	'
    	'lblEnglishWeightDesignation
    	'
    	Me.lblEnglishWeightDesignation.Location = New System.Drawing.Point(348, 78)
    	Me.lblEnglishWeightDesignation.Name = "lblEnglishWeightDesignation"
    	Me.lblEnglishWeightDesignation.Size = New System.Drawing.Size(63, 26)
    	Me.lblEnglishWeightDesignation.TabIndex = 78
    	Me.lblEnglishWeightDesignation.Text = "Weight Designation"
    	'
    	'radEnglishRequirementMetNo
    	'
    	Me.radEnglishRequirementMetNo.AutoSize = true
    	Me.radEnglishRequirementMetNo.Location = New System.Drawing.Point(165, 10)
    	Me.radEnglishRequirementMetNo.Name = "radEnglishRequirementMetNo"
    	Me.radEnglishRequirementMetNo.Size = New System.Drawing.Size(39, 17)
    	Me.radEnglishRequirementMetNo.TabIndex = 2
    	Me.radEnglishRequirementMetNo.TabStop = true
    	Me.radEnglishRequirementMetNo.Text = "No"
    	Me.radEnglishRequirementMetNo.UseVisualStyleBackColor = true
    	'
    	'radEnglishRequirementMetYes
    	'
    	Me.radEnglishRequirementMetYes.AutoSize = true
    	Me.radEnglishRequirementMetYes.Location = New System.Drawing.Point(111, 10)
    	Me.radEnglishRequirementMetYes.Name = "radEnglishRequirementMetYes"
    	Me.radEnglishRequirementMetYes.Size = New System.Drawing.Size(43, 17)
    	Me.radEnglishRequirementMetYes.TabIndex = 1
    	Me.radEnglishRequirementMetYes.TabStop = true
    	Me.radEnglishRequirementMetYes.Text = "Yes"
    	Me.radEnglishRequirementMetYes.UseVisualStyleBackColor = true
    	'
    	'lblEnglishRequirementMet
    	'
    	Me.lblEnglishRequirementMet.AutoSize = true
    	Me.lblEnglishRequirementMet.Location = New System.Drawing.Point(7, 12)
    	Me.lblEnglishRequirementMet.Name = "lblEnglishRequirementMet"
    	Me.lblEnglishRequirementMet.Size = New System.Drawing.Size(88, 13)
    	Me.lblEnglishRequirementMet.TabIndex = 3
    	Me.lblEnglishRequirementMet.Text = "Requirement Met"
    	'
    	'lblEnglishTitle
    	'
    	Me.lblEnglishTitle.AutoSize = true
    	Me.lblEnglishTitle.Location = New System.Drawing.Point(7, 91)
    	Me.lblEnglishTitle.Name = "lblEnglishTitle"
    	Me.lblEnglishTitle.Size = New System.Drawing.Size(117, 13)
    	Me.lblEnglishTitle.TabIndex = 0
    	Me.lblEnglishTitle.Text = "Title / School Attended"
    	'
    	'lblEnglishGradeLevel
    	'
    	Me.lblEnglishGradeLevel.Location = New System.Drawing.Point(409, 78)
    	Me.lblEnglishGradeLevel.Name = "lblEnglishGradeLevel"
    	Me.lblEnglishGradeLevel.Size = New System.Drawing.Size(36, 26)
    	Me.lblEnglishGradeLevel.TabIndex = 2
    	Me.lblEnglishGradeLevel.Text = "Grade Level"
    	'
    	'lblEnglishCredits
    	'
    	Me.lblEnglishCredits.AutoSize = true
    	Me.lblEnglishCredits.Location = New System.Drawing.Point(492, 91)
    	Me.lblEnglishCredits.Name = "lblEnglishCredits"
    	Me.lblEnglishCredits.Size = New System.Drawing.Size(39, 13)
    	Me.lblEnglishCredits.TabIndex = 4
    	Me.lblEnglishCredits.Text = "Credits"
    	'
    	'lblEnglishWeightedAverageGrade
    	'
    	Me.lblEnglishWeightedAverageGrade.Location = New System.Drawing.Point(810, 64)
    	Me.lblEnglishWeightedAverageGrade.Name = "lblEnglishWeightedAverageGrade"
    	Me.lblEnglishWeightedAverageGrade.Size = New System.Drawing.Size(67, 40)
    	Me.lblEnglishWeightedAverageGrade.TabIndex = 76
    	Me.lblEnglishWeightedAverageGrade.Text = "Weighted Average Grade"
    	'
    	'tabMathematics
    	'
    	Me.tabMathematics.Controls.Add(Me.Label6)
    	Me.tabMathematics.Controls.Add(Me.lblMathematicsGrades)
    	Me.tabMathematics.Controls.Add(Me.txtMathematicsVerifiedDate)
    	Me.tabMathematics.Controls.Add(Me.lblMathematicsVerifiedDate)
    	Me.tabMathematics.Controls.Add(Me.txtMathematicsVerifiedBy)
    	Me.tabMathematics.Controls.Add(Me.lblMathematicsVerifiedBy)
    	Me.tabMathematics.Controls.Add(Me.radMathematicsRequirementMetInProgress)
    	Me.tabMathematics.Controls.Add(Me.lblMathematicsWeightDesignation)
    	Me.tabMathematics.Controls.Add(Me.radMathematicsRequirementMetNo)
    	Me.tabMathematics.Controls.Add(Me.radMathematicsRequirementMetYes)
    	Me.tabMathematics.Controls.Add(Me.lblMathematicsRequirementMet)
    	Me.tabMathematics.Controls.Add(Me.lblMathematicsTitle)
    	Me.tabMathematics.Controls.Add(Me.lblMathematicsGradeLevel)
    	Me.tabMathematics.Controls.Add(Me.lblMathematicsCredits)
    	Me.tabMathematics.Controls.Add(Me.lblMathematicsWeightedAverageGrade)
    	Me.tabMathematics.Controls.Add(Me.mathematicsClass6)
    	Me.tabMathematics.Controls.Add(Me.mathematicsClass5)
    	Me.tabMathematics.Controls.Add(Me.mathematicsClass4)
    	Me.tabMathematics.Controls.Add(Me.mathematicsClass3)
    	Me.tabMathematics.Controls.Add(Me.mathematicsClass2)
    	Me.tabMathematics.Controls.Add(Me.mathematicsClass1)
    	Me.tabMathematics.Location = New System.Drawing.Point(4, 26)
    	Me.tabMathematics.Name = "tabMathematics"
    	Me.tabMathematics.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabMathematics.Size = New System.Drawing.Size(892, 568)
    	Me.tabMathematics.TabIndex = 1
    	Me.tabMathematics.Text = "Mathematics"
    	Me.tabMathematics.UseVisualStyleBackColor = true
    	'
    	'Label6
    	'
    	Me.Label6.Location = New System.Drawing.Point(440, 64)
    	Me.Label6.Name = "Label6"
    	Me.Label6.Size = New System.Drawing.Size(51, 40)
    	Me.Label6.TabIndex = 216
    	Me.Label6.Text = "Year Taken (YY/YY)"
    	Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopCenter
    	'
    	'lblMathematicsGrades
    	'
    	Me.lblMathematicsGrades.AutoSize = true
    	Me.lblMathematicsGrades.Location = New System.Drawing.Point(527, 91)
    	Me.lblMathematicsGrades.Name = "lblMathematicsGrades"
    	Me.lblMathematicsGrades.Size = New System.Drawing.Size(279, 13)
    	Me.lblMathematicsGrades.TabIndex = 215
    	Me.lblMathematicsGrades.Text = "|---------------------------------------Grades-----------------------------------"& _ 
    	"----|"
    	'
    	'txtMathematicsVerifiedDate
    	'
    	Me.txtMathematicsVerifiedDate.Location = New System.Drawing.Point(282, 33)
    	Me.txtMathematicsVerifiedDate.Name = "txtMathematicsVerifiedDate"
    	Me.txtMathematicsVerifiedDate.ReadOnly = true
    	Me.txtMathematicsVerifiedDate.Size = New System.Drawing.Size(66, 20)
    	Me.txtMathematicsVerifiedDate.TabIndex = 0
    	Me.txtMathematicsVerifiedDate.TabStop = false
    	'
    	'lblMathematicsVerifiedDate
    	'
    	Me.lblMathematicsVerifiedDate.AutoSize = true
    	Me.lblMathematicsVerifiedDate.Location = New System.Drawing.Point(246, 36)
    	Me.lblMathematicsVerifiedDate.Name = "lblMathematicsVerifiedDate"
    	Me.lblMathematicsVerifiedDate.Size = New System.Drawing.Size(30, 13)
    	Me.lblMathematicsVerifiedDate.TabIndex = 202
    	Me.lblMathematicsVerifiedDate.Text = "Date"
    	'
    	'txtMathematicsVerifiedBy
    	'
    	Me.txtMathematicsVerifiedBy.Location = New System.Drawing.Point(70, 33)
    	Me.txtMathematicsVerifiedBy.Name = "txtMathematicsVerifiedBy"
    	Me.txtMathematicsVerifiedBy.ReadOnly = true
    	Me.txtMathematicsVerifiedBy.Size = New System.Drawing.Size(161, 20)
    	Me.txtMathematicsVerifiedBy.TabIndex = 0
    	Me.txtMathematicsVerifiedBy.TabStop = false
    	'
    	'lblMathematicsVerifiedBy
    	'
    	Me.lblMathematicsVerifiedBy.AutoSize = true
    	Me.lblMathematicsVerifiedBy.Location = New System.Drawing.Point(7, 36)
    	Me.lblMathematicsVerifiedBy.Name = "lblMathematicsVerifiedBy"
    	Me.lblMathematicsVerifiedBy.Size = New System.Drawing.Size(57, 13)
    	Me.lblMathematicsVerifiedBy.TabIndex = 198
    	Me.lblMathematicsVerifiedBy.Text = "Verified By"
    	'
    	'radMathematicsRequirementMetInProgress
    	'
    	Me.radMathematicsRequirementMetInProgress.AutoSize = true
    	Me.radMathematicsRequirementMetInProgress.Location = New System.Drawing.Point(222, 10)
    	Me.radMathematicsRequirementMetInProgress.Name = "radMathematicsRequirementMetInProgress"
    	Me.radMathematicsRequirementMetInProgress.Size = New System.Drawing.Size(78, 17)
    	Me.radMathematicsRequirementMetInProgress.TabIndex = 3
    	Me.radMathematicsRequirementMetInProgress.TabStop = true
    	Me.radMathematicsRequirementMetInProgress.Text = "In Progress"
    	Me.radMathematicsRequirementMetInProgress.UseVisualStyleBackColor = true
    	'
    	'lblMathematicsWeightDesignation
    	'
    	Me.lblMathematicsWeightDesignation.Location = New System.Drawing.Point(347, 78)
    	Me.lblMathematicsWeightDesignation.Name = "lblMathematicsWeightDesignation"
    	Me.lblMathematicsWeightDesignation.Size = New System.Drawing.Size(63, 26)
    	Me.lblMathematicsWeightDesignation.TabIndex = 211
    	Me.lblMathematicsWeightDesignation.Text = "Weight Designation"
    	'
    	'radMathematicsRequirementMetNo
    	'
    	Me.radMathematicsRequirementMetNo.AutoSize = true
    	Me.radMathematicsRequirementMetNo.Location = New System.Drawing.Point(165, 10)
    	Me.radMathematicsRequirementMetNo.Name = "radMathematicsRequirementMetNo"
    	Me.radMathematicsRequirementMetNo.Size = New System.Drawing.Size(39, 17)
    	Me.radMathematicsRequirementMetNo.TabIndex = 2
    	Me.radMathematicsRequirementMetNo.TabStop = true
    	Me.radMathematicsRequirementMetNo.Text = "No"
    	Me.radMathematicsRequirementMetNo.UseVisualStyleBackColor = true
    	'
    	'radMathematicsRequirementMetYes
    	'
    	Me.radMathematicsRequirementMetYes.AutoSize = true
    	Me.radMathematicsRequirementMetYes.Location = New System.Drawing.Point(111, 10)
    	Me.radMathematicsRequirementMetYes.Name = "radMathematicsRequirementMetYes"
    	Me.radMathematicsRequirementMetYes.Size = New System.Drawing.Size(43, 17)
    	Me.radMathematicsRequirementMetYes.TabIndex = 1
    	Me.radMathematicsRequirementMetYes.TabStop = true
    	Me.radMathematicsRequirementMetYes.Text = "Yes"
    	Me.radMathematicsRequirementMetYes.UseVisualStyleBackColor = true
    	'
    	'lblMathematicsRequirementMet
    	'
    	Me.lblMathematicsRequirementMet.AutoSize = true
    	Me.lblMathematicsRequirementMet.Location = New System.Drawing.Point(7, 12)
    	Me.lblMathematicsRequirementMet.Name = "lblMathematicsRequirementMet"
    	Me.lblMathematicsRequirementMet.Size = New System.Drawing.Size(88, 13)
    	Me.lblMathematicsRequirementMet.TabIndex = 189
    	Me.lblMathematicsRequirementMet.Text = "Requirement Met"
    	'
    	'lblMathematicsTitle
    	'
    	Me.lblMathematicsTitle.AutoSize = true
    	Me.lblMathematicsTitle.Location = New System.Drawing.Point(7, 91)
    	Me.lblMathematicsTitle.Name = "lblMathematicsTitle"
    	Me.lblMathematicsTitle.Size = New System.Drawing.Size(117, 13)
    	Me.lblMathematicsTitle.TabIndex = 185
    	Me.lblMathematicsTitle.Text = "Title / School Attended"
    	'
    	'lblMathematicsGradeLevel
    	'
    	Me.lblMathematicsGradeLevel.Location = New System.Drawing.Point(408, 78)
    	Me.lblMathematicsGradeLevel.Name = "lblMathematicsGradeLevel"
    	Me.lblMathematicsGradeLevel.Size = New System.Drawing.Size(36, 26)
    	Me.lblMathematicsGradeLevel.TabIndex = 187
    	Me.lblMathematicsGradeLevel.Text = "Grade Level"
    	'
    	'lblMathematicsCredits
    	'
    	Me.lblMathematicsCredits.AutoSize = true
    	Me.lblMathematicsCredits.Location = New System.Drawing.Point(491, 91)
    	Me.lblMathematicsCredits.Name = "lblMathematicsCredits"
    	Me.lblMathematicsCredits.Size = New System.Drawing.Size(39, 13)
    	Me.lblMathematicsCredits.TabIndex = 192
    	Me.lblMathematicsCredits.Text = "Credits"
    	'
    	'lblMathematicsWeightedAverageGrade
    	'
    	Me.lblMathematicsWeightedAverageGrade.Location = New System.Drawing.Point(809, 64)
    	Me.lblMathematicsWeightedAverageGrade.Name = "lblMathematicsWeightedAverageGrade"
    	Me.lblMathematicsWeightedAverageGrade.Size = New System.Drawing.Size(69, 40)
    	Me.lblMathematicsWeightedAverageGrade.TabIndex = 209
    	Me.lblMathematicsWeightedAverageGrade.Text = "Weighted Average Grade"
    	'
    	'mathematicsClass6
    	'
    	Me.mathematicsClass6.Location = New System.Drawing.Point(6, 397)
    	Me.mathematicsClass6.Name = "mathematicsClass6"
    	Me.mathematicsClass6.Size = New System.Drawing.Size(880, 52)
    	Me.mathematicsClass6.TabIndex = 9
    	'
    	'mathematicsClass5
    	'
    	Me.mathematicsClass5.Location = New System.Drawing.Point(6, 339)
    	Me.mathematicsClass5.Name = "mathematicsClass5"
    	Me.mathematicsClass5.Size = New System.Drawing.Size(880, 52)
    	Me.mathematicsClass5.TabIndex = 8
    	'
    	'mathematicsClass4
    	'
    	Me.mathematicsClass4.Location = New System.Drawing.Point(6, 281)
    	Me.mathematicsClass4.Name = "mathematicsClass4"
    	Me.mathematicsClass4.Size = New System.Drawing.Size(880, 52)
    	Me.mathematicsClass4.TabIndex = 7
    	'
    	'mathematicsClass3
    	'
    	Me.mathematicsClass3.Location = New System.Drawing.Point(6, 223)
    	Me.mathematicsClass3.Name = "mathematicsClass3"
    	Me.mathematicsClass3.Size = New System.Drawing.Size(880, 52)
    	Me.mathematicsClass3.TabIndex = 6
    	'
    	'mathematicsClass2
    	'
    	Me.mathematicsClass2.Location = New System.Drawing.Point(6, 165)
    	Me.mathematicsClass2.Name = "mathematicsClass2"
    	Me.mathematicsClass2.Size = New System.Drawing.Size(880, 52)
    	Me.mathematicsClass2.TabIndex = 5
    	'
    	'mathematicsClass1
    	'
    	Me.mathematicsClass1.Location = New System.Drawing.Point(6, 107)
    	Me.mathematicsClass1.Name = "mathematicsClass1"
    	Me.mathematicsClass1.Size = New System.Drawing.Size(880, 52)
    	Me.mathematicsClass1.TabIndex = 4
    	'
    	'tabScienceWithLab
    	'
    	Me.tabScienceWithLab.Controls.Add(Me.Label7)
    	Me.tabScienceWithLab.Controls.Add(Me.lblScienceGrades)
    	Me.tabScienceWithLab.Controls.Add(Me.txtScienceVerifiedDate)
    	Me.tabScienceWithLab.Controls.Add(Me.lblScienceVerifiedDate)
    	Me.tabScienceWithLab.Controls.Add(Me.txtScienceVerifiedBy)
    	Me.tabScienceWithLab.Controls.Add(Me.lblScienceVerifiedBy)
    	Me.tabScienceWithLab.Controls.Add(Me.radScienceRequirementMetInProgress)
    	Me.tabScienceWithLab.Controls.Add(Me.lblScienceWeightDesignation)
    	Me.tabScienceWithLab.Controls.Add(Me.radScienceRequirementMetNo)
    	Me.tabScienceWithLab.Controls.Add(Me.radScienceRequirementMetYes)
    	Me.tabScienceWithLab.Controls.Add(Me.lblScienceRequirementMet)
    	Me.tabScienceWithLab.Controls.Add(Me.lblScienceTitle)
    	Me.tabScienceWithLab.Controls.Add(Me.lblScienceGradeLevel)
    	Me.tabScienceWithLab.Controls.Add(Me.lblScienceCredits)
    	Me.tabScienceWithLab.Controls.Add(Me.lblScienceWeightedAverageGrade)
    	Me.tabScienceWithLab.Controls.Add(Me.scienceClass8)
    	Me.tabScienceWithLab.Controls.Add(Me.scienceClass7)
    	Me.tabScienceWithLab.Controls.Add(Me.scienceClass6)
    	Me.tabScienceWithLab.Controls.Add(Me.scienceClass5)
    	Me.tabScienceWithLab.Controls.Add(Me.scienceClass4)
    	Me.tabScienceWithLab.Controls.Add(Me.scienceClass3)
    	Me.tabScienceWithLab.Controls.Add(Me.scienceClass2)
    	Me.tabScienceWithLab.Controls.Add(Me.scienceClass1)
    	Me.tabScienceWithLab.Location = New System.Drawing.Point(4, 26)
    	Me.tabScienceWithLab.Name = "tabScienceWithLab"
    	Me.tabScienceWithLab.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabScienceWithLab.Size = New System.Drawing.Size(892, 568)
    	Me.tabScienceWithLab.TabIndex = 2
    	Me.tabScienceWithLab.Text = "Science w/Lab"
    	Me.tabScienceWithLab.UseVisualStyleBackColor = true
    	'
    	'Label7
    	'
    	Me.Label7.Location = New System.Drawing.Point(440, 64)
    	Me.Label7.Name = "Label7"
    	Me.Label7.Size = New System.Drawing.Size(51, 40)
    	Me.Label7.TabIndex = 216
    	Me.Label7.Text = "Year Taken (YY/YY)"
    	Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopCenter
    	'
    	'lblScienceGrades
    	'
    	Me.lblScienceGrades.AutoSize = true
    	Me.lblScienceGrades.Location = New System.Drawing.Point(527, 91)
    	Me.lblScienceGrades.Name = "lblScienceGrades"
    	Me.lblScienceGrades.Size = New System.Drawing.Size(279, 13)
    	Me.lblScienceGrades.TabIndex = 215
    	Me.lblScienceGrades.Text = "|---------------------------------------Grades-----------------------------------"& _ 
    	"----|"
    	'
    	'txtScienceVerifiedDate
    	'
    	Me.txtScienceVerifiedDate.Location = New System.Drawing.Point(282, 33)
    	Me.txtScienceVerifiedDate.Name = "txtScienceVerifiedDate"
    	Me.txtScienceVerifiedDate.ReadOnly = true
    	Me.txtScienceVerifiedDate.Size = New System.Drawing.Size(66, 20)
    	Me.txtScienceVerifiedDate.TabIndex = 0
    	Me.txtScienceVerifiedDate.TabStop = false
    	'
    	'lblScienceVerifiedDate
    	'
    	Me.lblScienceVerifiedDate.AutoSize = true
    	Me.lblScienceVerifiedDate.Location = New System.Drawing.Point(246, 36)
    	Me.lblScienceVerifiedDate.Name = "lblScienceVerifiedDate"
    	Me.lblScienceVerifiedDate.Size = New System.Drawing.Size(30, 13)
    	Me.lblScienceVerifiedDate.TabIndex = 202
    	Me.lblScienceVerifiedDate.Text = "Date"
    	'
    	'txtScienceVerifiedBy
    	'
    	Me.txtScienceVerifiedBy.Location = New System.Drawing.Point(70, 33)
    	Me.txtScienceVerifiedBy.Name = "txtScienceVerifiedBy"
    	Me.txtScienceVerifiedBy.ReadOnly = true
    	Me.txtScienceVerifiedBy.Size = New System.Drawing.Size(161, 20)
    	Me.txtScienceVerifiedBy.TabIndex = 0
    	Me.txtScienceVerifiedBy.TabStop = false
    	'
    	'lblScienceVerifiedBy
    	'
    	Me.lblScienceVerifiedBy.AutoSize = true
    	Me.lblScienceVerifiedBy.Location = New System.Drawing.Point(7, 36)
    	Me.lblScienceVerifiedBy.Name = "lblScienceVerifiedBy"
    	Me.lblScienceVerifiedBy.Size = New System.Drawing.Size(57, 13)
    	Me.lblScienceVerifiedBy.TabIndex = 198
    	Me.lblScienceVerifiedBy.Text = "Verified By"
    	'
    	'radScienceRequirementMetInProgress
    	'
    	Me.radScienceRequirementMetInProgress.AutoSize = true
    	Me.radScienceRequirementMetInProgress.Location = New System.Drawing.Point(222, 10)
    	Me.radScienceRequirementMetInProgress.Name = "radScienceRequirementMetInProgress"
    	Me.radScienceRequirementMetInProgress.Size = New System.Drawing.Size(78, 17)
    	Me.radScienceRequirementMetInProgress.TabIndex = 3
    	Me.radScienceRequirementMetInProgress.Text = "In Progress"
    	Me.radScienceRequirementMetInProgress.UseVisualStyleBackColor = true
    	'
    	'lblScienceWeightDesignation
    	'
    	Me.lblScienceWeightDesignation.Location = New System.Drawing.Point(347, 78)
    	Me.lblScienceWeightDesignation.Name = "lblScienceWeightDesignation"
    	Me.lblScienceWeightDesignation.Size = New System.Drawing.Size(63, 26)
    	Me.lblScienceWeightDesignation.TabIndex = 211
    	Me.lblScienceWeightDesignation.Text = "Weight Designation"
    	'
    	'radScienceRequirementMetNo
    	'
    	Me.radScienceRequirementMetNo.AutoSize = true
    	Me.radScienceRequirementMetNo.Location = New System.Drawing.Point(165, 10)
    	Me.radScienceRequirementMetNo.Name = "radScienceRequirementMetNo"
    	Me.radScienceRequirementMetNo.Size = New System.Drawing.Size(39, 17)
    	Me.radScienceRequirementMetNo.TabIndex = 2
    	Me.radScienceRequirementMetNo.Text = "No"
    	Me.radScienceRequirementMetNo.UseVisualStyleBackColor = true
    	'
    	'radScienceRequirementMetYes
    	'
    	Me.radScienceRequirementMetYes.AutoSize = true
    	Me.radScienceRequirementMetYes.Location = New System.Drawing.Point(111, 10)
    	Me.radScienceRequirementMetYes.Name = "radScienceRequirementMetYes"
    	Me.radScienceRequirementMetYes.Size = New System.Drawing.Size(43, 17)
    	Me.radScienceRequirementMetYes.TabIndex = 1
    	Me.radScienceRequirementMetYes.Text = "Yes"
    	Me.radScienceRequirementMetYes.UseVisualStyleBackColor = true
    	'
    	'lblScienceRequirementMet
    	'
    	Me.lblScienceRequirementMet.AutoSize = true
    	Me.lblScienceRequirementMet.Location = New System.Drawing.Point(7, 12)
    	Me.lblScienceRequirementMet.Name = "lblScienceRequirementMet"
    	Me.lblScienceRequirementMet.Size = New System.Drawing.Size(88, 13)
    	Me.lblScienceRequirementMet.TabIndex = 189
    	Me.lblScienceRequirementMet.Text = "Requirement Met"
    	'
    	'lblScienceTitle
    	'
    	Me.lblScienceTitle.AutoSize = true
    	Me.lblScienceTitle.Location = New System.Drawing.Point(7, 91)
    	Me.lblScienceTitle.Name = "lblScienceTitle"
    	Me.lblScienceTitle.Size = New System.Drawing.Size(117, 13)
    	Me.lblScienceTitle.TabIndex = 185
    	Me.lblScienceTitle.Text = "Title / School Attended"
    	'
    	'lblScienceGradeLevel
    	'
    	Me.lblScienceGradeLevel.Location = New System.Drawing.Point(408, 78)
    	Me.lblScienceGradeLevel.Name = "lblScienceGradeLevel"
    	Me.lblScienceGradeLevel.Size = New System.Drawing.Size(36, 26)
    	Me.lblScienceGradeLevel.TabIndex = 187
    	Me.lblScienceGradeLevel.Text = "Grade Level"
    	'
    	'lblScienceCredits
    	'
    	Me.lblScienceCredits.AutoSize = true
    	Me.lblScienceCredits.Location = New System.Drawing.Point(491, 91)
    	Me.lblScienceCredits.Name = "lblScienceCredits"
    	Me.lblScienceCredits.Size = New System.Drawing.Size(39, 13)
    	Me.lblScienceCredits.TabIndex = 192
    	Me.lblScienceCredits.Text = "Credits"
    	'
    	'lblScienceWeightedAverageGrade
    	'
    	Me.lblScienceWeightedAverageGrade.Location = New System.Drawing.Point(809, 64)
    	Me.lblScienceWeightedAverageGrade.Name = "lblScienceWeightedAverageGrade"
    	Me.lblScienceWeightedAverageGrade.Size = New System.Drawing.Size(67, 40)
    	Me.lblScienceWeightedAverageGrade.TabIndex = 209
    	Me.lblScienceWeightedAverageGrade.Text = "Weighted Average Grade"
    	'
    	'scienceClass8
    	'
    	Me.scienceClass8.Location = New System.Drawing.Point(6, 513)
    	Me.scienceClass8.Name = "scienceClass8"
    	Me.scienceClass8.Size = New System.Drawing.Size(880, 52)
    	Me.scienceClass8.TabIndex = 11
    	'
    	'scienceClass7
    	'
    	Me.scienceClass7.Location = New System.Drawing.Point(6, 455)
    	Me.scienceClass7.Name = "scienceClass7"
    	Me.scienceClass7.Size = New System.Drawing.Size(880, 52)
    	Me.scienceClass7.TabIndex = 10
    	'
    	'scienceClass6
    	'
    	Me.scienceClass6.Location = New System.Drawing.Point(6, 397)
    	Me.scienceClass6.Name = "scienceClass6"
    	Me.scienceClass6.Size = New System.Drawing.Size(880, 52)
    	Me.scienceClass6.TabIndex = 9
    	'
    	'scienceClass5
    	'
    	Me.scienceClass5.Location = New System.Drawing.Point(6, 339)
    	Me.scienceClass5.Name = "scienceClass5"
    	Me.scienceClass5.Size = New System.Drawing.Size(880, 52)
    	Me.scienceClass5.TabIndex = 8
    	'
    	'scienceClass4
    	'
    	Me.scienceClass4.Location = New System.Drawing.Point(6, 281)
    	Me.scienceClass4.Name = "scienceClass4"
    	Me.scienceClass4.Size = New System.Drawing.Size(880, 52)
    	Me.scienceClass4.TabIndex = 7
    	'
    	'scienceClass3
    	'
    	Me.scienceClass3.Location = New System.Drawing.Point(6, 223)
    	Me.scienceClass3.Name = "scienceClass3"
    	Me.scienceClass3.Size = New System.Drawing.Size(880, 52)
    	Me.scienceClass3.TabIndex = 6
    	'
    	'scienceClass2
    	'
    	Me.scienceClass2.Location = New System.Drawing.Point(6, 165)
    	Me.scienceClass2.Name = "scienceClass2"
    	Me.scienceClass2.Size = New System.Drawing.Size(880, 52)
    	Me.scienceClass2.TabIndex = 5
    	'
    	'scienceClass1
    	'
    	Me.scienceClass1.Location = New System.Drawing.Point(6, 107)
    	Me.scienceClass1.Name = "scienceClass1"
    	Me.scienceClass1.Size = New System.Drawing.Size(880, 52)
    	Me.scienceClass1.TabIndex = 4
    	'
    	'tabSocialScience
    	'
    	Me.tabSocialScience.Controls.Add(Me.Label8)
    	Me.tabSocialScience.Controls.Add(Me.lblSocialScienceGrades)
    	Me.tabSocialScience.Controls.Add(Me.txtSocialScienceVerifiedDate)
    	Me.tabSocialScience.Controls.Add(Me.lblSocialScienceVerifiedDate)
    	Me.tabSocialScience.Controls.Add(Me.txtSocialScienceVerifiedBy)
    	Me.tabSocialScience.Controls.Add(Me.lblSocialScienceVerifiedBy)
    	Me.tabSocialScience.Controls.Add(Me.radSocialScienceRequirementMetInProgress)
    	Me.tabSocialScience.Controls.Add(Me.lblSocialScienceWeightDesignation)
    	Me.tabSocialScience.Controls.Add(Me.radSocialScienceRequirementMetNo)
    	Me.tabSocialScience.Controls.Add(Me.radSocialScienceRequirementMetYes)
    	Me.tabSocialScience.Controls.Add(Me.lblSocialScienceRequirementMet)
    	Me.tabSocialScience.Controls.Add(Me.lblSocialScienceTitle)
    	Me.tabSocialScience.Controls.Add(Me.lblSocialScienceGradeLevel)
    	Me.tabSocialScience.Controls.Add(Me.lblSocialScienceCredits)
    	Me.tabSocialScience.Controls.Add(Me.lblSocialScienceWeightedAverageGrade)
    	Me.tabSocialScience.Controls.Add(Me.socialScienceClass8)
    	Me.tabSocialScience.Controls.Add(Me.socialScienceClass7)
    	Me.tabSocialScience.Controls.Add(Me.socialScienceClass6)
    	Me.tabSocialScience.Controls.Add(Me.socialScienceClass5)
    	Me.tabSocialScience.Controls.Add(Me.socialScienceClass4)
    	Me.tabSocialScience.Controls.Add(Me.socialScienceClass3)
    	Me.tabSocialScience.Controls.Add(Me.socialScienceClass2)
    	Me.tabSocialScience.Controls.Add(Me.socialScienceClass1)
    	Me.tabSocialScience.Location = New System.Drawing.Point(4, 26)
    	Me.tabSocialScience.Name = "tabSocialScience"
    	Me.tabSocialScience.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabSocialScience.Size = New System.Drawing.Size(892, 568)
    	Me.tabSocialScience.TabIndex = 3
    	Me.tabSocialScience.Text = "Social Science"
    	Me.tabSocialScience.UseVisualStyleBackColor = true
    	'
    	'Label8
    	'
    	Me.Label8.Location = New System.Drawing.Point(441, 64)
    	Me.Label8.Name = "Label8"
    	Me.Label8.Size = New System.Drawing.Size(51, 40)
    	Me.Label8.TabIndex = 216
    	Me.Label8.Text = "Year Taken (YY/YY)"
    	Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopCenter
    	'
    	'lblSocialScienceGrades
    	'
    	Me.lblSocialScienceGrades.AutoSize = true
    	Me.lblSocialScienceGrades.Location = New System.Drawing.Point(528, 91)
    	Me.lblSocialScienceGrades.Name = "lblSocialScienceGrades"
    	Me.lblSocialScienceGrades.Size = New System.Drawing.Size(279, 13)
    	Me.lblSocialScienceGrades.TabIndex = 215
    	Me.lblSocialScienceGrades.Text = "|---------------------------------------Grades-----------------------------------"& _ 
    	"----|"
    	'
    	'txtSocialScienceVerifiedDate
    	'
    	Me.txtSocialScienceVerifiedDate.Location = New System.Drawing.Point(282, 33)
    	Me.txtSocialScienceVerifiedDate.Name = "txtSocialScienceVerifiedDate"
    	Me.txtSocialScienceVerifiedDate.ReadOnly = true
    	Me.txtSocialScienceVerifiedDate.Size = New System.Drawing.Size(66, 20)
    	Me.txtSocialScienceVerifiedDate.TabIndex = 0
    	Me.txtSocialScienceVerifiedDate.TabStop = false
    	'
    	'lblSocialScienceVerifiedDate
    	'
    	Me.lblSocialScienceVerifiedDate.AutoSize = true
    	Me.lblSocialScienceVerifiedDate.Location = New System.Drawing.Point(246, 36)
    	Me.lblSocialScienceVerifiedDate.Name = "lblSocialScienceVerifiedDate"
    	Me.lblSocialScienceVerifiedDate.Size = New System.Drawing.Size(30, 13)
    	Me.lblSocialScienceVerifiedDate.TabIndex = 202
    	Me.lblSocialScienceVerifiedDate.Text = "Date"
    	'
    	'txtSocialScienceVerifiedBy
    	'
    	Me.txtSocialScienceVerifiedBy.Location = New System.Drawing.Point(70, 33)
    	Me.txtSocialScienceVerifiedBy.Name = "txtSocialScienceVerifiedBy"
    	Me.txtSocialScienceVerifiedBy.ReadOnly = true
    	Me.txtSocialScienceVerifiedBy.Size = New System.Drawing.Size(161, 20)
    	Me.txtSocialScienceVerifiedBy.TabIndex = 0
    	Me.txtSocialScienceVerifiedBy.TabStop = false
    	'
    	'lblSocialScienceVerifiedBy
    	'
    	Me.lblSocialScienceVerifiedBy.AutoSize = true
    	Me.lblSocialScienceVerifiedBy.Location = New System.Drawing.Point(7, 36)
    	Me.lblSocialScienceVerifiedBy.Name = "lblSocialScienceVerifiedBy"
    	Me.lblSocialScienceVerifiedBy.Size = New System.Drawing.Size(57, 13)
    	Me.lblSocialScienceVerifiedBy.TabIndex = 198
    	Me.lblSocialScienceVerifiedBy.Text = "Verified By"
    	'
    	'radSocialScienceRequirementMetInProgress
    	'
    	Me.radSocialScienceRequirementMetInProgress.AutoSize = true
    	Me.radSocialScienceRequirementMetInProgress.Location = New System.Drawing.Point(222, 10)
    	Me.radSocialScienceRequirementMetInProgress.Name = "radSocialScienceRequirementMetInProgress"
    	Me.radSocialScienceRequirementMetInProgress.Size = New System.Drawing.Size(78, 17)
    	Me.radSocialScienceRequirementMetInProgress.TabIndex = 3
    	Me.radSocialScienceRequirementMetInProgress.TabStop = true
    	Me.radSocialScienceRequirementMetInProgress.Text = "In Progress"
    	Me.radSocialScienceRequirementMetInProgress.UseVisualStyleBackColor = true
    	'
    	'lblSocialScienceWeightDesignation
    	'
    	Me.lblSocialScienceWeightDesignation.Location = New System.Drawing.Point(348, 78)
    	Me.lblSocialScienceWeightDesignation.Name = "lblSocialScienceWeightDesignation"
    	Me.lblSocialScienceWeightDesignation.Size = New System.Drawing.Size(63, 26)
    	Me.lblSocialScienceWeightDesignation.TabIndex = 211
    	Me.lblSocialScienceWeightDesignation.Text = "Weight Designation"
    	'
    	'radSocialScienceRequirementMetNo
    	'
    	Me.radSocialScienceRequirementMetNo.AutoSize = true
    	Me.radSocialScienceRequirementMetNo.Location = New System.Drawing.Point(165, 10)
    	Me.radSocialScienceRequirementMetNo.Name = "radSocialScienceRequirementMetNo"
    	Me.radSocialScienceRequirementMetNo.Size = New System.Drawing.Size(39, 17)
    	Me.radSocialScienceRequirementMetNo.TabIndex = 2
    	Me.radSocialScienceRequirementMetNo.TabStop = true
    	Me.radSocialScienceRequirementMetNo.Text = "No"
    	Me.radSocialScienceRequirementMetNo.UseVisualStyleBackColor = true
    	'
    	'radSocialScienceRequirementMetYes
    	'
    	Me.radSocialScienceRequirementMetYes.AutoSize = true
    	Me.radSocialScienceRequirementMetYes.Location = New System.Drawing.Point(111, 10)
    	Me.radSocialScienceRequirementMetYes.Name = "radSocialScienceRequirementMetYes"
    	Me.radSocialScienceRequirementMetYes.Size = New System.Drawing.Size(43, 17)
    	Me.radSocialScienceRequirementMetYes.TabIndex = 1
    	Me.radSocialScienceRequirementMetYes.TabStop = true
    	Me.radSocialScienceRequirementMetYes.Text = "Yes"
    	Me.radSocialScienceRequirementMetYes.UseVisualStyleBackColor = true
    	'
    	'lblSocialScienceRequirementMet
    	'
    	Me.lblSocialScienceRequirementMet.AutoSize = true
    	Me.lblSocialScienceRequirementMet.Location = New System.Drawing.Point(7, 12)
    	Me.lblSocialScienceRequirementMet.Name = "lblSocialScienceRequirementMet"
    	Me.lblSocialScienceRequirementMet.Size = New System.Drawing.Size(88, 13)
    	Me.lblSocialScienceRequirementMet.TabIndex = 189
    	Me.lblSocialScienceRequirementMet.Text = "Requirement Met"
    	'
    	'lblSocialScienceTitle
    	'
    	Me.lblSocialScienceTitle.AutoSize = true
    	Me.lblSocialScienceTitle.Location = New System.Drawing.Point(7, 91)
    	Me.lblSocialScienceTitle.Name = "lblSocialScienceTitle"
    	Me.lblSocialScienceTitle.Size = New System.Drawing.Size(117, 13)
    	Me.lblSocialScienceTitle.TabIndex = 185
    	Me.lblSocialScienceTitle.Text = "Title / School Attended"
    	'
    	'lblSocialScienceGradeLevel
    	'
    	Me.lblSocialScienceGradeLevel.Location = New System.Drawing.Point(409, 78)
    	Me.lblSocialScienceGradeLevel.Name = "lblSocialScienceGradeLevel"
    	Me.lblSocialScienceGradeLevel.Size = New System.Drawing.Size(36, 26)
    	Me.lblSocialScienceGradeLevel.TabIndex = 187
    	Me.lblSocialScienceGradeLevel.Text = "Grade Level"
    	'
    	'lblSocialScienceCredits
    	'
    	Me.lblSocialScienceCredits.AutoSize = true
    	Me.lblSocialScienceCredits.Location = New System.Drawing.Point(492, 91)
    	Me.lblSocialScienceCredits.Name = "lblSocialScienceCredits"
    	Me.lblSocialScienceCredits.Size = New System.Drawing.Size(39, 13)
    	Me.lblSocialScienceCredits.TabIndex = 192
    	Me.lblSocialScienceCredits.Text = "Credits"
    	'
    	'lblSocialScienceWeightedAverageGrade
    	'
    	Me.lblSocialScienceWeightedAverageGrade.Location = New System.Drawing.Point(810, 64)
    	Me.lblSocialScienceWeightedAverageGrade.Name = "lblSocialScienceWeightedAverageGrade"
    	Me.lblSocialScienceWeightedAverageGrade.Size = New System.Drawing.Size(67, 40)
    	Me.lblSocialScienceWeightedAverageGrade.TabIndex = 209
    	Me.lblSocialScienceWeightedAverageGrade.Text = "Weighted Average Grade"
    	'
    	'socialScienceClass8
    	'
    	Me.socialScienceClass8.Location = New System.Drawing.Point(6, 513)
    	Me.socialScienceClass8.Name = "socialScienceClass8"
    	Me.socialScienceClass8.Size = New System.Drawing.Size(880, 52)
    	Me.socialScienceClass8.TabIndex = 11
    	'
    	'socialScienceClass7
    	'
    	Me.socialScienceClass7.Location = New System.Drawing.Point(6, 455)
    	Me.socialScienceClass7.Name = "socialScienceClass7"
    	Me.socialScienceClass7.Size = New System.Drawing.Size(880, 52)
    	Me.socialScienceClass7.TabIndex = 10
    	'
    	'socialScienceClass6
    	'
    	Me.socialScienceClass6.Location = New System.Drawing.Point(6, 397)
    	Me.socialScienceClass6.Name = "socialScienceClass6"
    	Me.socialScienceClass6.Size = New System.Drawing.Size(880, 52)
    	Me.socialScienceClass6.TabIndex = 9
    	'
    	'socialScienceClass5
    	'
    	Me.socialScienceClass5.Location = New System.Drawing.Point(6, 339)
    	Me.socialScienceClass5.Name = "socialScienceClass5"
    	Me.socialScienceClass5.Size = New System.Drawing.Size(880, 52)
    	Me.socialScienceClass5.TabIndex = 8
    	'
    	'socialScienceClass4
    	'
    	Me.socialScienceClass4.Location = New System.Drawing.Point(6, 281)
    	Me.socialScienceClass4.Name = "socialScienceClass4"
    	Me.socialScienceClass4.Size = New System.Drawing.Size(880, 52)
    	Me.socialScienceClass4.TabIndex = 7
    	'
    	'socialScienceClass3
    	'
    	Me.socialScienceClass3.Location = New System.Drawing.Point(6, 223)
    	Me.socialScienceClass3.Name = "socialScienceClass3"
    	Me.socialScienceClass3.Size = New System.Drawing.Size(880, 52)
    	Me.socialScienceClass3.TabIndex = 6
    	'
    	'socialScienceClass2
    	'
    	Me.socialScienceClass2.Location = New System.Drawing.Point(6, 165)
    	Me.socialScienceClass2.Name = "socialScienceClass2"
    	Me.socialScienceClass2.Size = New System.Drawing.Size(880, 52)
    	Me.socialScienceClass2.TabIndex = 5
    	'
    	'socialScienceClass1
    	'
    	Me.socialScienceClass1.Location = New System.Drawing.Point(6, 107)
    	Me.socialScienceClass1.Name = "socialScienceClass1"
    	Me.socialScienceClass1.Size = New System.Drawing.Size(880, 52)
    	Me.socialScienceClass1.TabIndex = 4
    	'
    	'tabForeignLanguage
    	'
    	Me.tabForeignLanguage.Controls.Add(Me.Label9)
    	Me.tabForeignLanguage.Controls.Add(Me.lblForeignLanguageGrades)
    	Me.tabForeignLanguage.Controls.Add(Me.txtForeignLanguageVerifiedDate)
    	Me.tabForeignLanguage.Controls.Add(Me.lblForeignLanguageVerifiedDate)
    	Me.tabForeignLanguage.Controls.Add(Me.txtForeignLanguageVerifiedBy)
    	Me.tabForeignLanguage.Controls.Add(Me.lblForeignLanguageVerifiedBy)
    	Me.tabForeignLanguage.Controls.Add(Me.radForeignLanguageRequirementMetInProgress)
    	Me.tabForeignLanguage.Controls.Add(Me.lblForeignLanguageWeightDesignation)
    	Me.tabForeignLanguage.Controls.Add(Me.radForeignLanguageRequirementMetNo)
    	Me.tabForeignLanguage.Controls.Add(Me.radForeignLanguageRequirementMetYes)
    	Me.tabForeignLanguage.Controls.Add(Me.lblForeignLanguageRequirementMet)
    	Me.tabForeignLanguage.Controls.Add(Me.lblForeignLanguageTitle)
    	Me.tabForeignLanguage.Controls.Add(Me.lblForeignLanguageGradeLevel)
    	Me.tabForeignLanguage.Controls.Add(Me.lblForeignLanguageCredits)
    	Me.tabForeignLanguage.Controls.Add(Me.lblForeignLanguageWeightedAverageGrade)
    	Me.tabForeignLanguage.Controls.Add(Me.foreignLanguageClass5)
    	Me.tabForeignLanguage.Controls.Add(Me.foreignLanguageClass4)
    	Me.tabForeignLanguage.Controls.Add(Me.foreignLanguageClass3)
    	Me.tabForeignLanguage.Controls.Add(Me.foreignLanguageClass2)
    	Me.tabForeignLanguage.Controls.Add(Me.foreignLanguageClass1)
    	Me.tabForeignLanguage.Location = New System.Drawing.Point(4, 26)
    	Me.tabForeignLanguage.Name = "tabForeignLanguage"
    	Me.tabForeignLanguage.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabForeignLanguage.Size = New System.Drawing.Size(892, 568)
    	Me.tabForeignLanguage.TabIndex = 4
    	Me.tabForeignLanguage.Text = "Foreign Language"
    	Me.tabForeignLanguage.UseVisualStyleBackColor = true
    	'
    	'Label9
    	'
    	Me.Label9.Location = New System.Drawing.Point(440, 64)
    	Me.Label9.Name = "Label9"
    	Me.Label9.Size = New System.Drawing.Size(51, 40)
    	Me.Label9.TabIndex = 216
    	Me.Label9.Text = "Year Taken (YY/YY)"
    	Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopCenter
    	'
    	'lblForeignLanguageGrades
    	'
    	Me.lblForeignLanguageGrades.AutoSize = true
    	Me.lblForeignLanguageGrades.Location = New System.Drawing.Point(527, 91)
    	Me.lblForeignLanguageGrades.Name = "lblForeignLanguageGrades"
    	Me.lblForeignLanguageGrades.Size = New System.Drawing.Size(279, 13)
    	Me.lblForeignLanguageGrades.TabIndex = 215
    	Me.lblForeignLanguageGrades.Text = "|---------------------------------------Grades-----------------------------------"& _ 
    	"----|"
    	'
    	'txtForeignLanguageVerifiedDate
    	'
    	Me.txtForeignLanguageVerifiedDate.Location = New System.Drawing.Point(282, 33)
    	Me.txtForeignLanguageVerifiedDate.Name = "txtForeignLanguageVerifiedDate"
    	Me.txtForeignLanguageVerifiedDate.ReadOnly = true
    	Me.txtForeignLanguageVerifiedDate.Size = New System.Drawing.Size(66, 20)
    	Me.txtForeignLanguageVerifiedDate.TabIndex = 0
    	Me.txtForeignLanguageVerifiedDate.TabStop = false
    	'
    	'lblForeignLanguageVerifiedDate
    	'
    	Me.lblForeignLanguageVerifiedDate.AutoSize = true
    	Me.lblForeignLanguageVerifiedDate.Location = New System.Drawing.Point(246, 36)
    	Me.lblForeignLanguageVerifiedDate.Name = "lblForeignLanguageVerifiedDate"
    	Me.lblForeignLanguageVerifiedDate.Size = New System.Drawing.Size(30, 13)
    	Me.lblForeignLanguageVerifiedDate.TabIndex = 202
    	Me.lblForeignLanguageVerifiedDate.Text = "Date"
    	'
    	'txtForeignLanguageVerifiedBy
    	'
    	Me.txtForeignLanguageVerifiedBy.Location = New System.Drawing.Point(70, 33)
    	Me.txtForeignLanguageVerifiedBy.Name = "txtForeignLanguageVerifiedBy"
    	Me.txtForeignLanguageVerifiedBy.ReadOnly = true
    	Me.txtForeignLanguageVerifiedBy.Size = New System.Drawing.Size(161, 20)
    	Me.txtForeignLanguageVerifiedBy.TabIndex = 0
    	Me.txtForeignLanguageVerifiedBy.TabStop = false
    	'
    	'lblForeignLanguageVerifiedBy
    	'
    	Me.lblForeignLanguageVerifiedBy.AutoSize = true
    	Me.lblForeignLanguageVerifiedBy.Location = New System.Drawing.Point(7, 36)
    	Me.lblForeignLanguageVerifiedBy.Name = "lblForeignLanguageVerifiedBy"
    	Me.lblForeignLanguageVerifiedBy.Size = New System.Drawing.Size(57, 13)
    	Me.lblForeignLanguageVerifiedBy.TabIndex = 198
    	Me.lblForeignLanguageVerifiedBy.Text = "Verified By"
    	'
    	'radForeignLanguageRequirementMetInProgress
    	'
    	Me.radForeignLanguageRequirementMetInProgress.AutoSize = true
    	Me.radForeignLanguageRequirementMetInProgress.Location = New System.Drawing.Point(222, 10)
    	Me.radForeignLanguageRequirementMetInProgress.Name = "radForeignLanguageRequirementMetInProgress"
    	Me.radForeignLanguageRequirementMetInProgress.Size = New System.Drawing.Size(78, 17)
    	Me.radForeignLanguageRequirementMetInProgress.TabIndex = 3
    	Me.radForeignLanguageRequirementMetInProgress.TabStop = true
    	Me.radForeignLanguageRequirementMetInProgress.Text = "In Progress"
    	Me.radForeignLanguageRequirementMetInProgress.UseVisualStyleBackColor = true
    	'
    	'lblForeignLanguageWeightDesignation
    	'
    	Me.lblForeignLanguageWeightDesignation.Location = New System.Drawing.Point(347, 78)
    	Me.lblForeignLanguageWeightDesignation.Name = "lblForeignLanguageWeightDesignation"
    	Me.lblForeignLanguageWeightDesignation.Size = New System.Drawing.Size(63, 26)
    	Me.lblForeignLanguageWeightDesignation.TabIndex = 211
    	Me.lblForeignLanguageWeightDesignation.Text = "Weight Designation"
    	'
    	'radForeignLanguageRequirementMetNo
    	'
    	Me.radForeignLanguageRequirementMetNo.AutoSize = true
    	Me.radForeignLanguageRequirementMetNo.Location = New System.Drawing.Point(165, 10)
    	Me.radForeignLanguageRequirementMetNo.Name = "radForeignLanguageRequirementMetNo"
    	Me.radForeignLanguageRequirementMetNo.Size = New System.Drawing.Size(39, 17)
    	Me.radForeignLanguageRequirementMetNo.TabIndex = 2
    	Me.radForeignLanguageRequirementMetNo.TabStop = true
    	Me.radForeignLanguageRequirementMetNo.Text = "No"
    	Me.radForeignLanguageRequirementMetNo.UseVisualStyleBackColor = true
    	'
    	'radForeignLanguageRequirementMetYes
    	'
    	Me.radForeignLanguageRequirementMetYes.AutoSize = true
    	Me.radForeignLanguageRequirementMetYes.Location = New System.Drawing.Point(111, 10)
    	Me.radForeignLanguageRequirementMetYes.Name = "radForeignLanguageRequirementMetYes"
    	Me.radForeignLanguageRequirementMetYes.Size = New System.Drawing.Size(43, 17)
    	Me.radForeignLanguageRequirementMetYes.TabIndex = 1
    	Me.radForeignLanguageRequirementMetYes.TabStop = true
    	Me.radForeignLanguageRequirementMetYes.Text = "Yes"
    	Me.radForeignLanguageRequirementMetYes.UseVisualStyleBackColor = true
    	'
    	'lblForeignLanguageRequirementMet
    	'
    	Me.lblForeignLanguageRequirementMet.AutoSize = true
    	Me.lblForeignLanguageRequirementMet.Location = New System.Drawing.Point(7, 12)
    	Me.lblForeignLanguageRequirementMet.Name = "lblForeignLanguageRequirementMet"
    	Me.lblForeignLanguageRequirementMet.Size = New System.Drawing.Size(88, 13)
    	Me.lblForeignLanguageRequirementMet.TabIndex = 189
    	Me.lblForeignLanguageRequirementMet.Text = "Requirement Met"
    	'
    	'lblForeignLanguageTitle
    	'
    	Me.lblForeignLanguageTitle.AutoSize = true
    	Me.lblForeignLanguageTitle.Location = New System.Drawing.Point(7, 91)
    	Me.lblForeignLanguageTitle.Name = "lblForeignLanguageTitle"
    	Me.lblForeignLanguageTitle.Size = New System.Drawing.Size(117, 13)
    	Me.lblForeignLanguageTitle.TabIndex = 185
    	Me.lblForeignLanguageTitle.Text = "Title / School Attended"
    	'
    	'lblForeignLanguageGradeLevel
    	'
    	Me.lblForeignLanguageGradeLevel.Location = New System.Drawing.Point(408, 78)
    	Me.lblForeignLanguageGradeLevel.Name = "lblForeignLanguageGradeLevel"
    	Me.lblForeignLanguageGradeLevel.Size = New System.Drawing.Size(36, 26)
    	Me.lblForeignLanguageGradeLevel.TabIndex = 187
    	Me.lblForeignLanguageGradeLevel.Text = "Grade Level"
    	'
    	'lblForeignLanguageCredits
    	'
    	Me.lblForeignLanguageCredits.AutoSize = true
    	Me.lblForeignLanguageCredits.Location = New System.Drawing.Point(491, 91)
    	Me.lblForeignLanguageCredits.Name = "lblForeignLanguageCredits"
    	Me.lblForeignLanguageCredits.Size = New System.Drawing.Size(39, 13)
    	Me.lblForeignLanguageCredits.TabIndex = 192
    	Me.lblForeignLanguageCredits.Text = "Credits"
    	'
    	'lblForeignLanguageWeightedAverageGrade
    	'
    	Me.lblForeignLanguageWeightedAverageGrade.Location = New System.Drawing.Point(809, 64)
    	Me.lblForeignLanguageWeightedAverageGrade.Name = "lblForeignLanguageWeightedAverageGrade"
    	Me.lblForeignLanguageWeightedAverageGrade.Size = New System.Drawing.Size(67, 40)
    	Me.lblForeignLanguageWeightedAverageGrade.TabIndex = 209
    	Me.lblForeignLanguageWeightedAverageGrade.Text = "Weighted Average Grade"
    	'
    	'foreignLanguageClass5
    	'
    	Me.foreignLanguageClass5.Location = New System.Drawing.Point(6, 339)
    	Me.foreignLanguageClass5.Name = "foreignLanguageClass5"
    	Me.foreignLanguageClass5.Size = New System.Drawing.Size(880, 52)
    	Me.foreignLanguageClass5.TabIndex = 8
    	'
    	'foreignLanguageClass4
    	'
    	Me.foreignLanguageClass4.Location = New System.Drawing.Point(6, 281)
    	Me.foreignLanguageClass4.Name = "foreignLanguageClass4"
    	Me.foreignLanguageClass4.Size = New System.Drawing.Size(880, 52)
    	Me.foreignLanguageClass4.TabIndex = 7
    	'
    	'foreignLanguageClass3
    	'
    	Me.foreignLanguageClass3.Location = New System.Drawing.Point(6, 223)
    	Me.foreignLanguageClass3.Name = "foreignLanguageClass3"
    	Me.foreignLanguageClass3.Size = New System.Drawing.Size(880, 52)
    	Me.foreignLanguageClass3.TabIndex = 6
    	'
    	'foreignLanguageClass2
    	'
    	Me.foreignLanguageClass2.Location = New System.Drawing.Point(6, 165)
    	Me.foreignLanguageClass2.Name = "foreignLanguageClass2"
    	Me.foreignLanguageClass2.Size = New System.Drawing.Size(880, 52)
    	Me.foreignLanguageClass2.TabIndex = 5
    	'
    	'foreignLanguageClass1
    	'
    	Me.foreignLanguageClass1.Location = New System.Drawing.Point(6, 107)
    	Me.foreignLanguageClass1.Name = "foreignLanguageClass1"
    	Me.foreignLanguageClass1.Size = New System.Drawing.Size(880, 52)
    	Me.foreignLanguageClass1.TabIndex = 4
    	'
    	'btnApplicationLinkDocument
    	'
    	Me.btnApplicationLinkDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnApplicationLinkDocument.Location = New System.Drawing.Point(230, 602)
    	Me.btnApplicationLinkDocument.Name = "btnApplicationLinkDocument"
    	Me.btnApplicationLinkDocument.Size = New System.Drawing.Size(83, 42)
    	Me.btnApplicationLinkDocument.TabIndex = 46
    	Me.btnApplicationLinkDocument.Text = "Link Document"
    	Me.btnApplicationLinkDocument.UseVisualStyleBackColor = true
    	'
    	'grpApplicationAwardStatus
    	'
    	Me.grpApplicationAwardStatus.Controls.Add(Me.txtAppYear)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.Label1)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.dtpApplicationLeaveOfAbsenceEndDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.dtpApplicationLeaveOfAbsenceBeginDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.dtpApplicationDefermentEndDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.dtpApplicationDefermentBeginDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.dtpApplicationAwardStatusLetterSent)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.grpApplicationDenialReasons)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationAwardStatusLetterSent)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.txtApplicationAwardStatusDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationExemplaryAwardAmount)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.btnApplicationSaveChanges)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.txtApplicationExemplaryAwardAmount)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.cmbApplicationLeaveOfAbsenceReason)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationLeaveOfAbsenceReason)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationLeaveOfAbsenceEndDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationLeaveOfAbsenceBeginDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.cmbApplicationDefermentReason)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationDefermentReason)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationDefermentEndDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationDefermentBeginDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationSupplementalAwardAmount)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.txtApplicationSupplementalAwardAmount)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.chkApplicationSupplementalAwardApproved)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.chkApplicationExemplaryAwardEarned)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationBaseAwardAmount)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.txtApplicationBaseAwardAmount)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.txtApplicationAwardStatusUserId)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationAwardStatusUserId)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationAwardStatusDate)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.cmbApplicationAwardStatus)
    	Me.grpApplicationAwardStatus.Controls.Add(Me.lblApplicationAwardStatus)
    	Me.grpApplicationAwardStatus.Location = New System.Drawing.Point(8, 6)
    	Me.grpApplicationAwardStatus.Name = "grpApplicationAwardStatus"
    	Me.grpApplicationAwardStatus.Size = New System.Drawing.Size(740, 228)
    	Me.grpApplicationAwardStatus.TabIndex = 1
    	Me.grpApplicationAwardStatus.TabStop = false
    	Me.grpApplicationAwardStatus.Text = "Award Status"
    	'
    	'txtAppYear
    	'
    	Me.txtAppYear.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ScholarshipApplicationBindingSource1, "ApplicationYear", true))
    	Me.txtAppYear.Location = New System.Drawing.Point(275, 20)
    	Me.txtAppYear.Name = "txtAppYear"
    	Me.txtAppYear.ReadOnly = true
    	Me.txtAppYear.Size = New System.Drawing.Size(44, 20)
    	Me.txtAppYear.TabIndex = 61
    	Me.txtAppYear.TabStop = false
    	'
    	'Label1
    	'
    	Me.Label1.AutoSize = true
    	Me.Label1.Location = New System.Drawing.Point(219, 21)
    	Me.Label1.Name = "Label1"
    	Me.Label1.Size = New System.Drawing.Size(51, 13)
    	Me.Label1.TabIndex = 62
    	Me.Label1.Text = "App Year"
    	'
    	'dtpApplicationLeaveOfAbsenceEndDate
    	'
    	Me.dtpApplicationLeaveOfAbsenceEndDate.CustomFormat = ""
    	Me.dtpApplicationLeaveOfAbsenceEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationLeaveOfAbsenceEndDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationLeaveOfAbsenceEndDate.Location = New System.Drawing.Point(386, 175)
    	Me.dtpApplicationLeaveOfAbsenceEndDate.Name = "dtpApplicationLeaveOfAbsenceEndDate"
    	Me.dtpApplicationLeaveOfAbsenceEndDate.NullValue = " "
    	Me.dtpApplicationLeaveOfAbsenceEndDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationLeaveOfAbsenceEndDate.TabIndex = 12
    	Me.dtpApplicationLeaveOfAbsenceEndDate.Value = Nothing
    	'
    	'dtpApplicationLeaveOfAbsenceBeginDate
    	'
    	Me.dtpApplicationLeaveOfAbsenceBeginDate.CustomFormat = ""
    	Me.dtpApplicationLeaveOfAbsenceBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationLeaveOfAbsenceBeginDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationLeaveOfAbsenceBeginDate.Location = New System.Drawing.Point(144, 175)
    	Me.dtpApplicationLeaveOfAbsenceBeginDate.Name = "dtpApplicationLeaveOfAbsenceBeginDate"
    	Me.dtpApplicationLeaveOfAbsenceBeginDate.NullValue = " "
    	Me.dtpApplicationLeaveOfAbsenceBeginDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationLeaveOfAbsenceBeginDate.TabIndex = 11
    	Me.dtpApplicationLeaveOfAbsenceBeginDate.Value = Nothing
    	'
    	'dtpApplicationDefermentEndDate
    	'
    	Me.dtpApplicationDefermentEndDate.CustomFormat = ""
    	Me.dtpApplicationDefermentEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationDefermentEndDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationDefermentEndDate.Location = New System.Drawing.Point(386, 122)
    	Me.dtpApplicationDefermentEndDate.Name = "dtpApplicationDefermentEndDate"
    	Me.dtpApplicationDefermentEndDate.NullValue = " "
    	Me.dtpApplicationDefermentEndDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationDefermentEndDate.TabIndex = 9
    	Me.dtpApplicationDefermentEndDate.Value = Nothing
    	'
    	'dtpApplicationDefermentBeginDate
    	'
    	Me.dtpApplicationDefermentBeginDate.CustomFormat = ""
    	Me.dtpApplicationDefermentBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationDefermentBeginDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationDefermentBeginDate.Location = New System.Drawing.Point(144, 122)
    	Me.dtpApplicationDefermentBeginDate.Name = "dtpApplicationDefermentBeginDate"
    	Me.dtpApplicationDefermentBeginDate.NullValue = " "
    	Me.dtpApplicationDefermentBeginDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationDefermentBeginDate.TabIndex = 8
    	Me.dtpApplicationDefermentBeginDate.Value = Nothing
    	'
    	'dtpApplicationAwardStatusLetterSent
    	'
    	Me.dtpApplicationAwardStatusLetterSent.CustomFormat = ""
    	Me.dtpApplicationAwardStatusLetterSent.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.PrimaryAwardBindingSource, "StatusLetterSentDate", true))
    	Me.dtpApplicationAwardStatusLetterSent.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationAwardStatusLetterSent.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationAwardStatusLetterSent.Location = New System.Drawing.Point(140, 46)
    	Me.dtpApplicationAwardStatusLetterSent.Name = "dtpApplicationAwardStatusLetterSent"
    	Me.dtpApplicationAwardStatusLetterSent.NullValue = " "
    	Me.dtpApplicationAwardStatusLetterSent.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationAwardStatusLetterSent.TabIndex = 2
    	Me.dtpApplicationAwardStatusLetterSent.Value = Nothing
    	'
    	'PrimaryAwardBindingSource
    	'
    	Me.PrimaryAwardBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.PrimaryAward)
    	'
    	'grpApplicationDenialReasons
    	'
    	Me.grpApplicationDenialReasons.Controls.Add(Me.cmbApplicationDenialReason1)
    	Me.grpApplicationDenialReasons.Controls.Add(Me.cmbApplicationDenialReason2)
    	Me.grpApplicationDenialReasons.Controls.Add(Me.cmbApplicationDenialReason6)
    	Me.grpApplicationDenialReasons.Controls.Add(Me.cmbApplicationDenialReason3)
    	Me.grpApplicationDenialReasons.Controls.Add(Me.cmbApplicationDenialReason5)
    	Me.grpApplicationDenialReasons.Controls.Add(Me.cmbApplicationDenialReason4)
    	Me.grpApplicationDenialReasons.Location = New System.Drawing.Point(487, 42)
    	Me.grpApplicationDenialReasons.Name = "grpApplicationDenialReasons"
    	Me.grpApplicationDenialReasons.Size = New System.Drawing.Size(251, 184)
    	Me.grpApplicationDenialReasons.TabIndex = 14
    	Me.grpApplicationDenialReasons.TabStop = false
    	Me.grpApplicationDenialReasons.Text = "Denial Reasons"
    	'
    	'cmbApplicationDenialReason1
    	'
    	Me.cmbApplicationDenialReason1.FormattingEnabled = true
    	Me.cmbApplicationDenialReason1.Location = New System.Drawing.Point(6, 19)
    	Me.cmbApplicationDenialReason1.Name = "cmbApplicationDenialReason1"
    	Me.cmbApplicationDenialReason1.Size = New System.Drawing.Size(239, 21)
    	Me.cmbApplicationDenialReason1.Sorted = true
    	Me.cmbApplicationDenialReason1.TabIndex = 1
    	'
    	'cmbApplicationDenialReason2
    	'
    	Me.cmbApplicationDenialReason2.FormattingEnabled = true
    	Me.cmbApplicationDenialReason2.Location = New System.Drawing.Point(6, 46)
    	Me.cmbApplicationDenialReason2.Name = "cmbApplicationDenialReason2"
    	Me.cmbApplicationDenialReason2.Size = New System.Drawing.Size(239, 21)
    	Me.cmbApplicationDenialReason2.Sorted = true
    	Me.cmbApplicationDenialReason2.TabIndex = 2
    	'
    	'cmbApplicationDenialReason6
    	'
    	Me.cmbApplicationDenialReason6.FormattingEnabled = true
    	Me.cmbApplicationDenialReason6.Location = New System.Drawing.Point(6, 154)
    	Me.cmbApplicationDenialReason6.Name = "cmbApplicationDenialReason6"
    	Me.cmbApplicationDenialReason6.Size = New System.Drawing.Size(239, 21)
    	Me.cmbApplicationDenialReason6.Sorted = true
    	Me.cmbApplicationDenialReason6.TabIndex = 6
    	'
    	'cmbApplicationDenialReason3
    	'
    	Me.cmbApplicationDenialReason3.FormattingEnabled = true
    	Me.cmbApplicationDenialReason3.Location = New System.Drawing.Point(6, 73)
    	Me.cmbApplicationDenialReason3.Name = "cmbApplicationDenialReason3"
    	Me.cmbApplicationDenialReason3.Size = New System.Drawing.Size(239, 21)
    	Me.cmbApplicationDenialReason3.Sorted = true
    	Me.cmbApplicationDenialReason3.TabIndex = 3
    	'
    	'cmbApplicationDenialReason5
    	'
    	Me.cmbApplicationDenialReason5.FormattingEnabled = true
    	Me.cmbApplicationDenialReason5.Location = New System.Drawing.Point(6, 127)
    	Me.cmbApplicationDenialReason5.Name = "cmbApplicationDenialReason5"
    	Me.cmbApplicationDenialReason5.Size = New System.Drawing.Size(239, 21)
    	Me.cmbApplicationDenialReason5.Sorted = true
    	Me.cmbApplicationDenialReason5.TabIndex = 5
    	'
    	'cmbApplicationDenialReason4
    	'
    	Me.cmbApplicationDenialReason4.FormattingEnabled = true
    	Me.cmbApplicationDenialReason4.Location = New System.Drawing.Point(6, 100)
    	Me.cmbApplicationDenialReason4.Name = "cmbApplicationDenialReason4"
    	Me.cmbApplicationDenialReason4.Size = New System.Drawing.Size(239, 21)
    	Me.cmbApplicationDenialReason4.Sorted = true
    	Me.cmbApplicationDenialReason4.TabIndex = 4
    	'
    	'lblApplicationAwardStatusLetterSent
    	'
    	Me.lblApplicationAwardStatusLetterSent.AutoSize = true
    	Me.lblApplicationAwardStatusLetterSent.Location = New System.Drawing.Point(4, 50)
    	Me.lblApplicationAwardStatusLetterSent.Name = "lblApplicationAwardStatusLetterSent"
    	Me.lblApplicationAwardStatusLetterSent.Size = New System.Drawing.Size(125, 13)
    	Me.lblApplicationAwardStatusLetterSent.TabIndex = 60
    	Me.lblApplicationAwardStatusLetterSent.Text = "Award Status Letter Sent"
    	'
    	'txtApplicationAwardStatusDate
    	'
    	Me.txtApplicationAwardStatusDate.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PrimaryAwardBindingSource, "StatusDate", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, Nothing, "d"))
    	Me.txtApplicationAwardStatusDate.Location = New System.Drawing.Point(393, 19)
    	Me.txtApplicationAwardStatusDate.Name = "txtApplicationAwardStatusDate"
    	Me.txtApplicationAwardStatusDate.ReadOnly = true
    	Me.txtApplicationAwardStatusDate.Size = New System.Drawing.Size(70, 20)
    	Me.txtApplicationAwardStatusDate.TabIndex = 0
    	Me.txtApplicationAwardStatusDate.TabStop = false
    	'
    	'lblApplicationExemplaryAwardAmount
    	'
    	Me.lblApplicationExemplaryAwardAmount.AutoSize = true
    	Me.lblApplicationExemplaryAwardAmount.Location = New System.Drawing.Point(253, 73)
    	Me.lblApplicationExemplaryAwardAmount.Name = "lblApplicationExemplaryAwardAmount"
    	Me.lblApplicationExemplaryAwardAmount.Size = New System.Drawing.Size(127, 13)
    	Me.lblApplicationExemplaryAwardAmount.TabIndex = 51
    	Me.lblApplicationExemplaryAwardAmount.Text = "Exemplary Award Amount"
    	Me.lblApplicationExemplaryAwardAmount.Visible = false
    	'
    	'btnApplicationSaveChanges
    	'
    	Me.btnApplicationSaveChanges.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnApplicationSaveChanges.Location = New System.Drawing.Point(640, 6)
    	Me.btnApplicationSaveChanges.Name = "btnApplicationSaveChanges"
    	Me.btnApplicationSaveChanges.Size = New System.Drawing.Size(99, 32)
    	Me.btnApplicationSaveChanges.TabIndex = 60
    	Me.btnApplicationSaveChanges.Text = "Save Changes"
    	Me.btnApplicationSaveChanges.UseVisualStyleBackColor = true
    	'
    	'txtApplicationExemplaryAwardAmount
    	'
    	Me.txtApplicationExemplaryAwardAmount.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ExemplaryAwardBindingSource, "Amount", true))
    	Me.txtApplicationExemplaryAwardAmount.Location = New System.Drawing.Point(386, 70)
    	Me.txtApplicationExemplaryAwardAmount.Name = "txtApplicationExemplaryAwardAmount"
    	Me.txtApplicationExemplaryAwardAmount.Size = New System.Drawing.Size(71, 20)
    	Me.txtApplicationExemplaryAwardAmount.TabIndex = 5
    	Me.txtApplicationExemplaryAwardAmount.Visible = false
    	'
    	'ExemplaryAwardBindingSource
    	'
    	Me.ExemplaryAwardBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.AdditionalAward)
    	'
    	'cmbApplicationLeaveOfAbsenceReason
    	'
    	Me.cmbApplicationLeaveOfAbsenceReason.FormattingEnabled = true
    	Me.cmbApplicationLeaveOfAbsenceReason.Location = New System.Drawing.Point(144, 201)
    	Me.cmbApplicationLeaveOfAbsenceReason.Name = "cmbApplicationLeaveOfAbsenceReason"
    	Me.cmbApplicationLeaveOfAbsenceReason.Size = New System.Drawing.Size(236, 21)
    	Me.cmbApplicationLeaveOfAbsenceReason.Sorted = true
    	Me.cmbApplicationLeaveOfAbsenceReason.TabIndex = 13
    	'
    	'lblApplicationLeaveOfAbsenceReason
    	'
    	Me.lblApplicationLeaveOfAbsenceReason.AutoSize = true
    	Me.lblApplicationLeaveOfAbsenceReason.Location = New System.Drawing.Point(4, 204)
    	Me.lblApplicationLeaveOfAbsenceReason.Name = "lblApplicationLeaveOfAbsenceReason"
    	Me.lblApplicationLeaveOfAbsenceReason.Size = New System.Drawing.Size(134, 13)
    	Me.lblApplicationLeaveOfAbsenceReason.TabIndex = 48
    	Me.lblApplicationLeaveOfAbsenceReason.Text = "Leave of Absence Reason"
    	'
    	'lblApplicationLeaveOfAbsenceEndDate
    	'
    	Me.lblApplicationLeaveOfAbsenceEndDate.AutoSize = true
    	Me.lblApplicationLeaveOfAbsenceEndDate.Location = New System.Drawing.Point(264, 179)
    	Me.lblApplicationLeaveOfAbsenceEndDate.Name = "lblApplicationLeaveOfAbsenceEndDate"
    	Me.lblApplicationLeaveOfAbsenceEndDate.Size = New System.Drawing.Size(116, 13)
    	Me.lblApplicationLeaveOfAbsenceEndDate.TabIndex = 46
    	Me.lblApplicationLeaveOfAbsenceEndDate.Text = "Leave of Absence End"
    	'
    	'lblApplicationLeaveOfAbsenceBeginDate
    	'
    	Me.lblApplicationLeaveOfAbsenceBeginDate.AutoSize = true
    	Me.lblApplicationLeaveOfAbsenceBeginDate.Location = New System.Drawing.Point(4, 179)
    	Me.lblApplicationLeaveOfAbsenceBeginDate.Name = "lblApplicationLeaveOfAbsenceBeginDate"
    	Me.lblApplicationLeaveOfAbsenceBeginDate.Size = New System.Drawing.Size(124, 13)
    	Me.lblApplicationLeaveOfAbsenceBeginDate.TabIndex = 44
    	Me.lblApplicationLeaveOfAbsenceBeginDate.Text = "Leave of Absence Begin"
    	'
    	'cmbApplicationDefermentReason
    	'
    	Me.cmbApplicationDefermentReason.FormattingEnabled = true
    	Me.cmbApplicationDefermentReason.Location = New System.Drawing.Point(144, 148)
    	Me.cmbApplicationDefermentReason.Name = "cmbApplicationDefermentReason"
    	Me.cmbApplicationDefermentReason.Size = New System.Drawing.Size(236, 21)
    	Me.cmbApplicationDefermentReason.Sorted = true
    	Me.cmbApplicationDefermentReason.TabIndex = 10
    	'
    	'lblApplicationDefermentReason
    	'
    	Me.lblApplicationDefermentReason.AutoSize = true
    	Me.lblApplicationDefermentReason.Location = New System.Drawing.Point(4, 151)
    	Me.lblApplicationDefermentReason.Name = "lblApplicationDefermentReason"
    	Me.lblApplicationDefermentReason.Size = New System.Drawing.Size(96, 13)
    	Me.lblApplicationDefermentReason.TabIndex = 40
    	Me.lblApplicationDefermentReason.Text = "Deferment Reason"
    	'
    	'lblApplicationDefermentEndDate
    	'
    	Me.lblApplicationDefermentEndDate.AutoSize = true
    	Me.lblApplicationDefermentEndDate.Location = New System.Drawing.Point(276, 126)
    	Me.lblApplicationDefermentEndDate.Name = "lblApplicationDefermentEndDate"
    	Me.lblApplicationDefermentEndDate.Size = New System.Drawing.Size(104, 13)
    	Me.lblApplicationDefermentEndDate.TabIndex = 38
    	Me.lblApplicationDefermentEndDate.Text = "Deferment End Date"
    	'
    	'lblApplicationDefermentBeginDate
    	'
    	Me.lblApplicationDefermentBeginDate.AutoSize = true
    	Me.lblApplicationDefermentBeginDate.Location = New System.Drawing.Point(4, 126)
    	Me.lblApplicationDefermentBeginDate.Name = "lblApplicationDefermentBeginDate"
    	Me.lblApplicationDefermentBeginDate.Size = New System.Drawing.Size(112, 13)
    	Me.lblApplicationDefermentBeginDate.TabIndex = 36
    	Me.lblApplicationDefermentBeginDate.Text = "Deferment Begin Date"
    	'
    	'lblApplicationSupplementalAwardAmount
    	'
    	Me.lblApplicationSupplementalAwardAmount.AutoSize = true
    	Me.lblApplicationSupplementalAwardAmount.Location = New System.Drawing.Point(272, 99)
    	Me.lblApplicationSupplementalAwardAmount.Name = "lblApplicationSupplementalAwardAmount"
    	Me.lblApplicationSupplementalAwardAmount.Size = New System.Drawing.Size(108, 13)
    	Me.lblApplicationSupplementalAwardAmount.TabIndex = 35
    	Me.lblApplicationSupplementalAwardAmount.Text = "UESP Award Amount"
    	'
    	'txtApplicationSupplementalAwardAmount
    	'
    	Me.txtApplicationSupplementalAwardAmount.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.UespSupplementalAwardBindingSource, "Amount", true))
    	Me.txtApplicationSupplementalAwardAmount.Location = New System.Drawing.Point(386, 95)
    	Me.txtApplicationSupplementalAwardAmount.Name = "txtApplicationSupplementalAwardAmount"
    	Me.txtApplicationSupplementalAwardAmount.Size = New System.Drawing.Size(71, 20)
    	Me.txtApplicationSupplementalAwardAmount.TabIndex = 7
    	'
    	'UespSupplementalAwardBindingSource
    	'
    	Me.UespSupplementalAwardBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.AdditionalAward)
    	'
    	'chkApplicationSupplementalAwardApproved
    	'
    	Me.chkApplicationSupplementalAwardApproved.AutoSize = true
    	Me.chkApplicationSupplementalAwardApproved.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.UespSupplementalAwardBindingSource, "IsApproved", true))
    	Me.chkApplicationSupplementalAwardApproved.Location = New System.Drawing.Point(7, 98)
    	Me.chkApplicationSupplementalAwardApproved.Name = "chkApplicationSupplementalAwardApproved"
    	Me.chkApplicationSupplementalAwardApproved.Size = New System.Drawing.Size(204, 17)
    	Me.chkApplicationSupplementalAwardApproved.TabIndex = 6
    	Me.chkApplicationSupplementalAwardApproved.Text = "UESP Supplemental Award Approved"
    	Me.chkApplicationSupplementalAwardApproved.UseVisualStyleBackColor = true
    	'
    	'chkApplicationExemplaryAwardEarned
    	'
    	Me.chkApplicationExemplaryAwardEarned.AutoSize = true
    	Me.chkApplicationExemplaryAwardEarned.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ExemplaryAwardBindingSource, "IsApproved", true))
    	Me.chkApplicationExemplaryAwardEarned.Location = New System.Drawing.Point(7, 72)
    	Me.chkApplicationExemplaryAwardEarned.Name = "chkApplicationExemplaryAwardEarned"
    	Me.chkApplicationExemplaryAwardEarned.Size = New System.Drawing.Size(144, 17)
    	Me.chkApplicationExemplaryAwardEarned.TabIndex = 4
    	Me.chkApplicationExemplaryAwardEarned.Text = "Exemplary Award Earned"
    	Me.chkApplicationExemplaryAwardEarned.UseVisualStyleBackColor = true
    	'
    	'lblApplicationBaseAwardAmount
    	'
    	Me.lblApplicationBaseAwardAmount.AutoSize = true
    	Me.lblApplicationBaseAwardAmount.Location = New System.Drawing.Point(277, 47)
    	Me.lblApplicationBaseAwardAmount.Name = "lblApplicationBaseAwardAmount"
    	Me.lblApplicationBaseAwardAmount.Size = New System.Drawing.Size(103, 13)
    	Me.lblApplicationBaseAwardAmount.TabIndex = 31
    	Me.lblApplicationBaseAwardAmount.Text = "Base Award Amount"
    	'
    	'txtApplicationBaseAwardAmount
    	'
    	Me.txtApplicationBaseAwardAmount.Location = New System.Drawing.Point(386, 44)
    	Me.txtApplicationBaseAwardAmount.Name = "txtApplicationBaseAwardAmount"
    	Me.txtApplicationBaseAwardAmount.ReadOnly = true
    	Me.txtApplicationBaseAwardAmount.Size = New System.Drawing.Size(71, 20)
    	Me.txtApplicationBaseAwardAmount.TabIndex = 3
    	'
    	'txtApplicationAwardStatusUserId
    	'
    	Me.txtApplicationAwardStatusUserId.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PrimaryAwardBindingSource, "StatusUserId", true))
    	Me.txtApplicationAwardStatusUserId.Location = New System.Drawing.Point(518, 18)
    	Me.txtApplicationAwardStatusUserId.Name = "txtApplicationAwardStatusUserId"
    	Me.txtApplicationAwardStatusUserId.ReadOnly = true
    	Me.txtApplicationAwardStatusUserId.Size = New System.Drawing.Size(116, 20)
    	Me.txtApplicationAwardStatusUserId.TabIndex = 0
    	Me.txtApplicationAwardStatusUserId.TabStop = false
    	'
    	'lblApplicationAwardStatusUserId
    	'
    	Me.lblApplicationAwardStatusUserId.AutoSize = true
    	Me.lblApplicationAwardStatusUserId.Location = New System.Drawing.Point(469, 22)
    	Me.lblApplicationAwardStatusUserId.Name = "lblApplicationAwardStatusUserId"
    	Me.lblApplicationAwardStatusUserId.Size = New System.Drawing.Size(43, 13)
    	Me.lblApplicationAwardStatusUserId.TabIndex = 28
    	Me.lblApplicationAwardStatusUserId.Text = "User ID"
    	'
    	'lblApplicationAwardStatusDate
    	'
    	Me.lblApplicationAwardStatusDate.AutoSize = true
    	Me.lblApplicationAwardStatusDate.Location = New System.Drawing.Point(324, 23)
    	Me.lblApplicationAwardStatusDate.Name = "lblApplicationAwardStatusDate"
    	Me.lblApplicationAwardStatusDate.Size = New System.Drawing.Size(63, 13)
    	Me.lblApplicationAwardStatusDate.TabIndex = 26
    	Me.lblApplicationAwardStatusDate.Text = "Status Date"
    	'
    	'cmbApplicationAwardStatus
    	'
    	Me.cmbApplicationAwardStatus.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PrimaryAwardBindingSource, "Status", true))
    	Me.cmbApplicationAwardStatus.FormattingEnabled = true
    	Me.cmbApplicationAwardStatus.Location = New System.Drawing.Point(80, 19)
    	Me.cmbApplicationAwardStatus.Name = "cmbApplicationAwardStatus"
    	Me.cmbApplicationAwardStatus.Size = New System.Drawing.Size(134, 21)
    	Me.cmbApplicationAwardStatus.Sorted = true
    	Me.cmbApplicationAwardStatus.TabIndex = 1
    	'
    	'lblApplicationAwardStatus
    	'
    	Me.lblApplicationAwardStatus.AutoSize = true
    	Me.lblApplicationAwardStatus.Location = New System.Drawing.Point(4, 22)
    	Me.lblApplicationAwardStatus.Name = "lblApplicationAwardStatus"
    	Me.lblApplicationAwardStatus.Size = New System.Drawing.Size(70, 13)
    	Me.lblApplicationAwardStatus.TabIndex = 24
    	Me.lblApplicationAwardStatus.Text = "Award Status"
    	'
    	'grpApplicationHighSchool
    	'
    	Me.grpApplicationHighSchool.Controls.Add(Me.txtApplicationHighSchoolCity)
    	Me.grpApplicationHighSchool.Controls.Add(Me.txtApplicationHighSchoolDistrict)
    	Me.grpApplicationHighSchool.Controls.Add(Me.dtpApplicationGraduationDate)
    	Me.grpApplicationHighSchool.Controls.Add(Me.cmbApplicationHighSchoolName)
    	Me.grpApplicationHighSchool.Controls.Add(Me.radApplicationUbsctFail)
    	Me.grpApplicationHighSchool.Controls.Add(Me.radApplicationUbsctPass)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationUbsct)
    	Me.grpApplicationHighSchool.Controls.Add(Me.txtApplicationActReadingScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationActReadingScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.txtApplicationActScienceScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationActScienceScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.txtApplicationActMathScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationActMathScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.txtApplicationActEnglishScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationActEnglishScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.chkApplicationIbDiploma)
    	Me.grpApplicationHighSchool.Controls.Add(Me.txtApplicationActCompositeScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationActCompositeScore)
    	Me.grpApplicationHighSchool.Controls.Add(Me.txtApplicationCumulativeGpa)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationCumulativeGpa)
    	Me.grpApplicationHighSchool.Controls.Add(Me.txtApplicationCeebCode)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationGraduationDate)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationCeebCode)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationSchoolDistrict)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationHighSchoolCity)
    	Me.grpApplicationHighSchool.Controls.Add(Me.lblApplicationHighSchoolName)
    	Me.grpApplicationHighSchool.Controls.Add(Me.chkApplicationUtahHighSchool)
    	Me.grpApplicationHighSchool.Location = New System.Drawing.Point(8, 705)
    	Me.grpApplicationHighSchool.Name = "grpApplicationHighSchool"
    	Me.grpApplicationHighSchool.Size = New System.Drawing.Size(740, 136)
    	Me.grpApplicationHighSchool.TabIndex = 51
    	Me.grpApplicationHighSchool.TabStop = false
    	Me.grpApplicationHighSchool.Text = "High School Information"
    	'
    	'txtApplicationHighSchoolCity
    	'
    	Me.txtApplicationHighSchoolCity.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.HighSchoolBindingSource, "City", true))
    	Me.txtApplicationHighSchoolCity.Location = New System.Drawing.Point(98, 43)
    	Me.txtApplicationHighSchoolCity.Name = "txtApplicationHighSchoolCity"
    	Me.txtApplicationHighSchoolCity.ReadOnly = true
    	Me.txtApplicationHighSchoolCity.Size = New System.Drawing.Size(225, 20)
    	Me.txtApplicationHighSchoolCity.TabIndex = 71
    	'
    	'HighSchoolBindingSource
    	'
    	Me.HighSchoolBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.HighSchool)
    	'
    	'txtApplicationHighSchoolDistrict
    	'
    	Me.txtApplicationHighSchoolDistrict.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.HighSchoolBindingSource, "District", true))
    	Me.txtApplicationHighSchoolDistrict.Location = New System.Drawing.Point(417, 43)
    	Me.txtApplicationHighSchoolDistrict.Name = "txtApplicationHighSchoolDistrict"
    	Me.txtApplicationHighSchoolDistrict.ReadOnly = true
    	Me.txtApplicationHighSchoolDistrict.Size = New System.Drawing.Size(192, 20)
    	Me.txtApplicationHighSchoolDistrict.TabIndex = 70
    	'
    	'dtpApplicationGraduationDate
    	'
    	Me.dtpApplicationGraduationDate.CustomFormat = ""
    	Me.dtpApplicationGraduationDate.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.HighSchoolBindingSource, "GraduationDate", true))
    	Me.dtpApplicationGraduationDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
    	Me.dtpApplicationGraduationDate.FormatAsString = "M/d/yyyy"
    	Me.dtpApplicationGraduationDate.Location = New System.Drawing.Point(98, 74)
    	Me.dtpApplicationGraduationDate.Name = "dtpApplicationGraduationDate"
    	Me.dtpApplicationGraduationDate.NullValue = " "
    	Me.dtpApplicationGraduationDate.Size = New System.Drawing.Size(95, 20)
    	Me.dtpApplicationGraduationDate.TabIndex = 52
    	Me.dtpApplicationGraduationDate.Value = Nothing
    	'
    	'cmbApplicationHighSchoolName
    	'
    	Me.cmbApplicationHighSchoolName.FormattingEnabled = true
    	Me.cmbApplicationHighSchoolName.Location = New System.Drawing.Point(352, 16)
    	Me.cmbApplicationHighSchoolName.MaxLength = 30
    	Me.cmbApplicationHighSchoolName.Name = "cmbApplicationHighSchoolName"
    	Me.cmbApplicationHighSchoolName.Size = New System.Drawing.Size(382, 21)
    	Me.cmbApplicationHighSchoolName.Sorted = true
    	Me.cmbApplicationHighSchoolName.TabIndex = 48
    	'
    	'radApplicationUbsctFail
    	'
    	Me.radApplicationUbsctFail.AutoSize = true
    	Me.radApplicationUbsctFail.Location = New System.Drawing.Point(554, 76)
    	Me.radApplicationUbsctFail.Name = "radApplicationUbsctFail"
    	Me.radApplicationUbsctFail.Size = New System.Drawing.Size(41, 17)
    	Me.radApplicationUbsctFail.TabIndex = 56
    	Me.radApplicationUbsctFail.TabStop = true
    	Me.radApplicationUbsctFail.Text = "Fail"
    	Me.radApplicationUbsctFail.UseVisualStyleBackColor = true
    	'
    	'radApplicationUbsctPass
    	'
    	Me.radApplicationUbsctPass.AutoSize = true
    	Me.radApplicationUbsctPass.Checked = true
    	Me.radApplicationUbsctPass.Location = New System.Drawing.Point(495, 76)
    	Me.radApplicationUbsctPass.Name = "radApplicationUbsctPass"
    	Me.radApplicationUbsctPass.Size = New System.Drawing.Size(48, 17)
    	Me.radApplicationUbsctPass.TabIndex = 55
    	Me.radApplicationUbsctPass.TabStop = true
    	Me.radApplicationUbsctPass.Text = "Pass"
    	Me.radApplicationUbsctPass.UseVisualStyleBackColor = true
    	'
    	'lblApplicationUbsct
    	'
    	Me.lblApplicationUbsct.AutoSize = true
    	Me.lblApplicationUbsct.Location = New System.Drawing.Point(441, 78)
    	Me.lblApplicationUbsct.Name = "lblApplicationUbsct"
    	Me.lblApplicationUbsct.Size = New System.Drawing.Size(43, 13)
    	Me.lblApplicationUbsct.TabIndex = 69
    	Me.lblApplicationUbsct.Text = "UBSCT"
    	'
    	'txtApplicationActReadingScore
    	'
    	Me.txtApplicationActReadingScore.Location = New System.Drawing.Point(701, 105)
    	Me.txtApplicationActReadingScore.Name = "txtApplicationActReadingScore"
    	Me.txtApplicationActReadingScore.Size = New System.Drawing.Size(22, 20)
    	Me.txtApplicationActReadingScore.TabIndex = 61
    	'
    	'lblApplicationActReadingScore
    	'
    	Me.lblApplicationActReadingScore.AutoSize = true
    	Me.lblApplicationActReadingScore.Location = New System.Drawing.Point(593, 108)
    	Me.lblApplicationActReadingScore.Name = "lblApplicationActReadingScore"
    	Me.lblApplicationActReadingScore.Size = New System.Drawing.Size(102, 13)
    	Me.lblApplicationActReadingScore.TabIndex = 67
    	Me.lblApplicationActReadingScore.Text = "ACT Reading Score"
    	'
    	'txtApplicationActScienceScore
    	'
    	Me.txtApplicationActScienceScore.Location = New System.Drawing.Point(548, 105)
    	Me.txtApplicationActScienceScore.Name = "txtApplicationActScienceScore"
    	Me.txtApplicationActScienceScore.Size = New System.Drawing.Size(22, 20)
    	Me.txtApplicationActScienceScore.TabIndex = 60
    	'
    	'lblApplicationActScienceScore
    	'
    	Me.lblApplicationActScienceScore.AutoSize = true
    	Me.lblApplicationActScienceScore.Location = New System.Drawing.Point(441, 108)
    	Me.lblApplicationActScienceScore.Name = "lblApplicationActScienceScore"
    	Me.lblApplicationActScienceScore.Size = New System.Drawing.Size(101, 13)
    	Me.lblApplicationActScienceScore.TabIndex = 65
    	Me.lblApplicationActScienceScore.Text = "ACT Science Score"
    	'
    	'txtApplicationActMathScore
    	'
    	Me.txtApplicationActMathScore.Location = New System.Drawing.Point(398, 105)
    	Me.txtApplicationActMathScore.Name = "txtApplicationActMathScore"
    	Me.txtApplicationActMathScore.Size = New System.Drawing.Size(22, 20)
    	Me.txtApplicationActMathScore.TabIndex = 59
    	'
    	'lblApplicationActMathScore
    	'
    	Me.lblApplicationActMathScore.AutoSize = true
    	Me.lblApplicationActMathScore.Location = New System.Drawing.Point(306, 108)
    	Me.lblApplicationActMathScore.Name = "lblApplicationActMathScore"
    	Me.lblApplicationActMathScore.Size = New System.Drawing.Size(86, 13)
    	Me.lblApplicationActMathScore.TabIndex = 63
    	Me.lblApplicationActMathScore.Text = "ACT Math Score"
    	'
    	'txtApplicationActEnglishScore
    	'
    	Me.txtApplicationActEnglishScore.Location = New System.Drawing.Point(263, 105)
    	Me.txtApplicationActEnglishScore.Name = "txtApplicationActEnglishScore"
    	Me.txtApplicationActEnglishScore.Size = New System.Drawing.Size(22, 20)
    	Me.txtApplicationActEnglishScore.TabIndex = 58
    	'
    	'lblApplicationActEnglishScore
    	'
    	Me.lblApplicationActEnglishScore.AutoSize = true
    	Me.lblApplicationActEnglishScore.Location = New System.Drawing.Point(161, 108)
    	Me.lblApplicationActEnglishScore.Name = "lblApplicationActEnglishScore"
    	Me.lblApplicationActEnglishScore.Size = New System.Drawing.Size(96, 13)
    	Me.lblApplicationActEnglishScore.TabIndex = 61
    	Me.lblApplicationActEnglishScore.Text = "ACT English Score"
    	'
    	'chkApplicationIbDiploma
    	'
    	Me.chkApplicationIbDiploma.AutoSize = true
    	Me.chkApplicationIbDiploma.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.HighSchoolBindingSource, "DiplomaIsInternationalBaccalaureate", true))
    	Me.chkApplicationIbDiploma.Location = New System.Drawing.Point(349, 77)
    	Me.chkApplicationIbDiploma.Name = "chkApplicationIbDiploma"
    	Me.chkApplicationIbDiploma.Size = New System.Drawing.Size(77, 17)
    	Me.chkApplicationIbDiploma.TabIndex = 54
    	Me.chkApplicationIbDiploma.Text = "IB Diploma"
    	Me.chkApplicationIbDiploma.UseVisualStyleBackColor = true
    	'
    	'txtApplicationActCompositeScore
    	'
    	Me.txtApplicationActCompositeScore.Location = New System.Drawing.Point(118, 105)
    	Me.txtApplicationActCompositeScore.Name = "txtApplicationActCompositeScore"
    	Me.txtApplicationActCompositeScore.Size = New System.Drawing.Size(22, 20)
    	Me.txtApplicationActCompositeScore.TabIndex = 57
    	'
    	'lblApplicationActCompositeScore
    	'
    	Me.lblApplicationActCompositeScore.AutoSize = true
    	Me.lblApplicationActCompositeScore.Location = New System.Drawing.Point(4, 108)
    	Me.lblApplicationActCompositeScore.Name = "lblApplicationActCompositeScore"
    	Me.lblApplicationActCompositeScore.Size = New System.Drawing.Size(111, 13)
    	Me.lblApplicationActCompositeScore.TabIndex = 58
    	Me.lblApplicationActCompositeScore.Text = "ACT Composite Score"
    	'
    	'txtApplicationCumulativeGpa
    	'
    	Me.txtApplicationCumulativeGpa.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.HighSchoolBindingSource, "CumulativeGpa", true))
    	Me.txtApplicationCumulativeGpa.Location = New System.Drawing.Point(289, 75)
    	Me.txtApplicationCumulativeGpa.Name = "txtApplicationCumulativeGpa"
    	Me.txtApplicationCumulativeGpa.Size = New System.Drawing.Size(34, 20)
    	Me.txtApplicationCumulativeGpa.TabIndex = 53
    	'
    	'lblApplicationCumulativeGpa
    	'
    	Me.lblApplicationCumulativeGpa.AutoSize = true
    	Me.lblApplicationCumulativeGpa.Location = New System.Drawing.Point(199, 78)
    	Me.lblApplicationCumulativeGpa.Name = "lblApplicationCumulativeGpa"
    	Me.lblApplicationCumulativeGpa.Size = New System.Drawing.Size(84, 13)
    	Me.lblApplicationCumulativeGpa.TabIndex = 54
    	Me.lblApplicationCumulativeGpa.Text = "Cumulative GPA"
    	'
    	'txtApplicationCeebCode
    	'
    	Me.txtApplicationCeebCode.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.HighSchoolBindingSource, "CeebCode", true))
    	Me.txtApplicationCeebCode.Location = New System.Drawing.Point(684, 43)
    	Me.txtApplicationCeebCode.Name = "txtApplicationCeebCode"
    	Me.txtApplicationCeebCode.Size = New System.Drawing.Size(48, 20)
    	Me.txtApplicationCeebCode.TabIndex = 51
    	'
    	'lblApplicationGraduationDate
    	'
    	Me.lblApplicationGraduationDate.AutoSize = true
    	Me.lblApplicationGraduationDate.Location = New System.Drawing.Point(4, 78)
    	Me.lblApplicationGraduationDate.Name = "lblApplicationGraduationDate"
    	Me.lblApplicationGraduationDate.Size = New System.Drawing.Size(85, 13)
    	Me.lblApplicationGraduationDate.TabIndex = 52
    	Me.lblApplicationGraduationDate.Text = "Graduation Date"
    	'
    	'lblApplicationCeebCode
    	'
    	Me.lblApplicationCeebCode.AutoSize = true
    	Me.lblApplicationCeebCode.Location = New System.Drawing.Point(615, 46)
    	Me.lblApplicationCeebCode.Name = "lblApplicationCeebCode"
    	Me.lblApplicationCeebCode.Size = New System.Drawing.Size(63, 13)
    	Me.lblApplicationCeebCode.TabIndex = 7
    	Me.lblApplicationCeebCode.Text = "CEEB Code"
    	'
    	'lblApplicationSchoolDistrict
    	'
    	Me.lblApplicationSchoolDistrict.AutoSize = true
    	Me.lblApplicationSchoolDistrict.Location = New System.Drawing.Point(336, 46)
    	Me.lblApplicationSchoolDistrict.Name = "lblApplicationSchoolDistrict"
    	Me.lblApplicationSchoolDistrict.Size = New System.Drawing.Size(75, 13)
    	Me.lblApplicationSchoolDistrict.TabIndex = 5
    	Me.lblApplicationSchoolDistrict.Text = "School District"
    	'
    	'lblApplicationHighSchoolCity
    	'
    	Me.lblApplicationHighSchoolCity.AutoSize = true
    	Me.lblApplicationHighSchoolCity.Location = New System.Drawing.Point(6, 46)
    	Me.lblApplicationHighSchoolCity.Name = "lblApplicationHighSchoolCity"
    	Me.lblApplicationHighSchoolCity.Size = New System.Drawing.Size(85, 13)
    	Me.lblApplicationHighSchoolCity.TabIndex = 3
    	Me.lblApplicationHighSchoolCity.Text = "High School City"
    	'
    	'lblApplicationHighSchoolName
    	'
    	Me.lblApplicationHighSchoolName.AutoSize = true
    	Me.lblApplicationHighSchoolName.Location = New System.Drawing.Point(250, 20)
    	Me.lblApplicationHighSchoolName.Name = "lblApplicationHighSchoolName"
    	Me.lblApplicationHighSchoolName.Size = New System.Drawing.Size(96, 13)
    	Me.lblApplicationHighSchoolName.TabIndex = 1
    	Me.lblApplicationHighSchoolName.Text = "High School Name"
    	'
    	'chkApplicationUtahHighSchool
    	'
    	Me.chkApplicationUtahHighSchool.AutoSize = true
    	Me.chkApplicationUtahHighSchool.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.HighSchoolBindingSource, "IsInUtah", true))
    	Me.chkApplicationUtahHighSchool.Location = New System.Drawing.Point(10, 20)
    	Me.chkApplicationUtahHighSchool.Name = "chkApplicationUtahHighSchool"
    	Me.chkApplicationUtahHighSchool.Size = New System.Drawing.Size(229, 17)
    	Me.chkApplicationUtahHighSchool.TabIndex = 47
    	Me.chkApplicationUtahHighSchool.Text = "Will you graduate from a Utah high school?"
    	Me.chkApplicationUtahHighSchool.UseVisualStyleBackColor = true
    	'
    	'tabCommunications
    	'
    	Me.tabCommunications.AutoScroll = true
    	Me.tabCommunications.Controls.Add(Me.CommunicationPrinter)
    	Me.tabCommunications.Controls.Add(Me.CommunicationDataGridView)
    	Me.tabCommunications.Controls.Add(Me.btnCommunicationViewDocuments)
    	Me.tabCommunications.Controls.Add(Me.btnCommunicationLinkDocument)
    	Me.tabCommunications.Controls.Add(Me.btnCommunicationClearFields)
    	Me.tabCommunications.Controls.Add(Me.btnCommunicationSave)
    	Me.tabCommunications.Controls.Add(Me.grpCommunicationRecord)
    	Me.tabCommunications.Location = New System.Drawing.Point(4, 22)
    	Me.tabCommunications.Name = "tabCommunications"
    	Me.tabCommunications.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabCommunications.Size = New System.Drawing.Size(933, 723)
    	Me.tabCommunications.TabIndex = 3
    	Me.tabCommunications.Text = "Communications"
    	Me.tabCommunications.UseVisualStyleBackColor = true
    	'
    	'CommunicationPrinter
    	'
    	Me.CommunicationPrinter.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    	Me.CommunicationPrinter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    	Me.CommunicationPrinter.Entity = Nothing
    	Me.CommunicationPrinter.EntityName = Nothing
    	Me.CommunicationPrinter.EntityType = Nothing
    	Me.CommunicationPrinter.Location = New System.Drawing.Point(323, 539)
    	Me.CommunicationPrinter.Name = "CommunicationPrinter"
    	Me.CommunicationPrinter.Size = New System.Drawing.Size(288, 95)
    	Me.CommunicationPrinter.SortAscending = false
    	Me.CommunicationPrinter.SortColumnName = Nothing
    	Me.CommunicationPrinter.TabIndex = 16
    	'
    	'CommunicationDataGridView
    	'
    	Me.CommunicationDataGridView.AllowUserToAddRows = false
    	Me.CommunicationDataGridView.AllowUserToDeleteRows = false
    	Me.CommunicationDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
    	    	    	Or System.Windows.Forms.AnchorStyles.Left)  _
    	    	    	Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.CommunicationDataGridView.AutoGenerateColumns = false
    	Me.CommunicationDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
    	dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    	dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
    	dataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
    	dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
    	dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    	dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
    	Me.CommunicationDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4
    	Me.CommunicationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
    	Me.CommunicationDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.UserID, Me.clmTimeStamp, Me.clmType, Me.clmSource, Me.clmSubject, Me.clmText, Me.Is411})
    	Me.CommunicationDataGridView.DataSource = Me.CommunicationBindingSource
    	dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    	dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
    	dataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
    	dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
    	dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    	dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
    	Me.CommunicationDataGridView.DefaultCellStyle = dataGridViewCellStyle5
    	Me.CommunicationDataGridView.Location = New System.Drawing.Point(10, 185)
    	Me.CommunicationDataGridView.Name = "CommunicationDataGridView"
    	Me.CommunicationDataGridView.ReadOnly = true
    	dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    	dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
    	dataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
    	dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
    	dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    	dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
    	Me.CommunicationDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6
    	Me.CommunicationDataGridView.RowHeadersVisible = false
    	Me.CommunicationDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
    	Me.CommunicationDataGridView.Size = New System.Drawing.Size(915, 348)
    	Me.CommunicationDataGridView.TabIndex = 5
    	'
    	'UserID
    	'
    	Me.UserID.DataPropertyName = "UserId"
    	Me.UserID.HeaderText = "User Id"
    	Me.UserID.Name = "UserID"
    	Me.UserID.ReadOnly = true
    	Me.UserID.Width = 66
    	'
    	'clmTimeStamp
    	'
    	Me.clmTimeStamp.DataPropertyName = "TimeStamp"
    	Me.clmTimeStamp.HeaderText = "Time Stamp"
    	Me.clmTimeStamp.Name = "clmTimeStamp"
    	Me.clmTimeStamp.ReadOnly = true
    	Me.clmTimeStamp.Width = 88
    	'
    	'clmType
    	'
    	Me.clmType.DataPropertyName = "Type"
    	Me.clmType.HeaderText = "Type"
    	Me.clmType.Name = "clmType"
    	Me.clmType.ReadOnly = true
    	Me.clmType.Width = 56
    	'
    	'clmSource
    	'
    	Me.clmSource.DataPropertyName = "Source"
    	Me.clmSource.HeaderText = "Source"
    	Me.clmSource.Name = "clmSource"
    	Me.clmSource.ReadOnly = true
    	Me.clmSource.Width = 66
    	'
    	'clmSubject
    	'
    	Me.clmSubject.DataPropertyName = "Subject"
    	Me.clmSubject.HeaderText = "Subject"
    	Me.clmSubject.Name = "clmSubject"
    	Me.clmSubject.ReadOnly = true
    	Me.clmSubject.Width = 68
    	'
    	'clmText
    	'
    	Me.clmText.DataPropertyName = "Text"
    	Me.clmText.HeaderText = "Comments"
    	Me.clmText.Name = "clmText"
    	Me.clmText.ReadOnly = true
    	Me.clmText.Width = 81
    	'
    	'Is411
    	'
    	Me.Is411.DataPropertyName = "Is411"
    	Me.Is411.HeaderText = "411"
    	Me.Is411.Name = "Is411"
    	Me.Is411.ReadOnly = true
    	Me.Is411.Width = 31
    	'
    	'CommunicationBindingSource
    	'
    	Me.CommunicationBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.Communication)
    	Me.CommunicationBindingSource.Sort = ""
    	'
    	'btnCommunicationViewDocuments
    	'
    	Me.btnCommunicationViewDocuments.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    	Me.btnCommunicationViewDocuments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnCommunicationViewDocuments.Location = New System.Drawing.Point(665, 649)
    	Me.btnCommunicationViewDocuments.Name = "btnCommunicationViewDocuments"
    	Me.btnCommunicationViewDocuments.Size = New System.Drawing.Size(127, 32)
    	Me.btnCommunicationViewDocuments.TabIndex = 15
    	Me.btnCommunicationViewDocuments.Text = "View Documents"
    	Me.btnCommunicationViewDocuments.UseVisualStyleBackColor = true
    	'
    	'btnCommunicationLinkDocument
    	'
    	Me.btnCommunicationLinkDocument.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    	Me.btnCommunicationLinkDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnCommunicationLinkDocument.Location = New System.Drawing.Point(490, 649)
    	Me.btnCommunicationLinkDocument.Name = "btnCommunicationLinkDocument"
    	Me.btnCommunicationLinkDocument.Size = New System.Drawing.Size(127, 32)
    	Me.btnCommunicationLinkDocument.TabIndex = 14
    	Me.btnCommunicationLinkDocument.Text = "Link Document"
    	Me.btnCommunicationLinkDocument.UseVisualStyleBackColor = true
    	'
    	'btnCommunicationClearFields
    	'
    	Me.btnCommunicationClearFields.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    	Me.btnCommunicationClearFields.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnCommunicationClearFields.Location = New System.Drawing.Point(315, 649)
    	Me.btnCommunicationClearFields.Name = "btnCommunicationClearFields"
    	Me.btnCommunicationClearFields.Size = New System.Drawing.Size(127, 32)
    	Me.btnCommunicationClearFields.TabIndex = 2
    	Me.btnCommunicationClearFields.Text = "Clear Fields"
    	Me.btnCommunicationClearFields.UseVisualStyleBackColor = true
    	'
    	'btnCommunicationSave
    	'
    	Me.btnCommunicationSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    	Me.btnCommunicationSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnCommunicationSave.Location = New System.Drawing.Point(140, 649)
    	Me.btnCommunicationSave.Name = "btnCommunicationSave"
    	Me.btnCommunicationSave.Size = New System.Drawing.Size(127, 32)
    	Me.btnCommunicationSave.TabIndex = 1
    	Me.btnCommunicationSave.Text = "Save"
    	Me.btnCommunicationSave.UseVisualStyleBackColor = true
    	'
    	'grpCommunicationRecord
    	'
    	Me.grpCommunicationRecord.Controls.Add(Me.chkCommunications411)
    	Me.grpCommunicationRecord.Controls.Add(Me.cmbCommunicationSource)
    	Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationSource)
    	Me.grpCommunicationRecord.Controls.Add(Me.txtCommunicationComments)
    	Me.grpCommunicationRecord.Controls.Add(Me.txtCommunicationSubject)
    	Me.grpCommunicationRecord.Controls.Add(Me.cmbCommunicationType)
    	Me.grpCommunicationRecord.Controls.Add(Me.txtCommunicationUserId)
    	Me.grpCommunicationRecord.Controls.Add(Me.txtCommunicationDateTime)
    	Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationSubject)
    	Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationType)
    	Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationUserId)
    	Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationDateTime)
    	Me.grpCommunicationRecord.Dock = System.Windows.Forms.DockStyle.Top
    	Me.grpCommunicationRecord.Location = New System.Drawing.Point(3, 3)
    	Me.grpCommunicationRecord.Name = "grpCommunicationRecord"
    	Me.grpCommunicationRecord.Size = New System.Drawing.Size(927, 172)
    	Me.grpCommunicationRecord.TabIndex = 0
    	Me.grpCommunicationRecord.TabStop = false
    	Me.grpCommunicationRecord.Text = "Communication Record"
    	'
    	'chkCommunications411
    	'
    	Me.chkCommunications411.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.chkCommunications411.AutoSize = true
    	Me.chkCommunications411.Location = New System.Drawing.Point(745, 71)
    	Me.chkCommunications411.Name = "chkCommunications411"
    	Me.chkCommunications411.Size = New System.Drawing.Size(44, 17)
    	Me.chkCommunications411.TabIndex = 11
    	Me.chkCommunications411.Text = "411"
    	Me.chkCommunications411.UseVisualStyleBackColor = true
    	'
    	'cmbCommunicationSource
    	'
    	Me.cmbCommunicationSource.FormattingEnabled = true
    	Me.cmbCommunicationSource.Location = New System.Drawing.Point(449, 44)
    	Me.cmbCommunicationSource.Name = "cmbCommunicationSource"
    	Me.cmbCommunicationSource.Size = New System.Drawing.Size(173, 21)
    	Me.cmbCommunicationSource.TabIndex = 2
    	'
    	'lblCommunicationSource
    	'
    	Me.lblCommunicationSource.AutoSize = true
    	Me.lblCommunicationSource.Location = New System.Drawing.Point(345, 47)
    	Me.lblCommunicationSource.Name = "lblCommunicationSource"
    	Me.lblCommunicationSource.Size = New System.Drawing.Size(41, 13)
    	Me.lblCommunicationSource.TabIndex = 10
    	Me.lblCommunicationSource.Text = "Source"
    	'
    	'txtCommunicationComments
    	'
    	Me.txtCommunicationComments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
    	    	    	Or System.Windows.Forms.AnchorStyles.Left)  _
    	    	    	Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.txtCommunicationComments.Location = New System.Drawing.Point(66, 95)
    	Me.txtCommunicationComments.MaxLength = 500
    	Me.txtCommunicationComments.Multiline = true
    	Me.txtCommunicationComments.Name = "txtCommunicationComments"
    	Me.txtCommunicationComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    	Me.txtCommunicationComments.Size = New System.Drawing.Size(739, 66)
    	Me.txtCommunicationComments.TabIndex = 4
    	'
    	'txtCommunicationSubject
    	'
    	Me.txtCommunicationSubject.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
    	    	    	Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.txtCommunicationSubject.Location = New System.Drawing.Point(225, 69)
    	Me.txtCommunicationSubject.MaxLength = 50
    	Me.txtCommunicationSubject.Name = "txtCommunicationSubject"
    	Me.txtCommunicationSubject.Size = New System.Drawing.Size(465, 20)
    	Me.txtCommunicationSubject.TabIndex = 3
    	'
    	'cmbCommunicationType
    	'
    	Me.cmbCommunicationType.FormattingEnabled = true
    	Me.cmbCommunicationType.Location = New System.Drawing.Point(449, 17)
    	Me.cmbCommunicationType.Name = "cmbCommunicationType"
    	Me.cmbCommunicationType.Size = New System.Drawing.Size(173, 21)
    	Me.cmbCommunicationType.TabIndex = 1
    	'
    	'txtCommunicationUserId
    	'
    	Me.txtCommunicationUserId.Location = New System.Drawing.Point(111, 43)
    	Me.txtCommunicationUserId.Name = "txtCommunicationUserId"
    	Me.txtCommunicationUserId.ReadOnly = true
    	Me.txtCommunicationUserId.Size = New System.Drawing.Size(173, 20)
    	Me.txtCommunicationUserId.TabIndex = 0
    	Me.txtCommunicationUserId.TabStop = false
    	'
    	'txtCommunicationDateTime
    	'
    	Me.txtCommunicationDateTime.Location = New System.Drawing.Point(111, 17)
    	Me.txtCommunicationDateTime.Name = "txtCommunicationDateTime"
    	Me.txtCommunicationDateTime.ReadOnly = true
    	Me.txtCommunicationDateTime.Size = New System.Drawing.Size(173, 20)
    	Me.txtCommunicationDateTime.TabIndex = 0
    	Me.txtCommunicationDateTime.TabStop = false
    	'
    	'lblCommunicationSubject
    	'
    	Me.lblCommunicationSubject.AutoSize = true
    	Me.lblCommunicationSubject.Location = New System.Drawing.Point(152, 72)
    	Me.lblCommunicationSubject.Name = "lblCommunicationSubject"
    	Me.lblCommunicationSubject.Size = New System.Drawing.Size(43, 13)
    	Me.lblCommunicationSubject.TabIndex = 3
    	Me.lblCommunicationSubject.Text = "Subject"
    	'
    	'lblCommunicationType
    	'
    	Me.lblCommunicationType.AutoSize = true
    	Me.lblCommunicationType.Location = New System.Drawing.Point(345, 20)
    	Me.lblCommunicationType.Name = "lblCommunicationType"
    	Me.lblCommunicationType.Size = New System.Drawing.Size(31, 13)
    	Me.lblCommunicationType.TabIndex = 2
    	Me.lblCommunicationType.Text = "Type"
    	'
    	'lblCommunicationUserId
    	'
    	Me.lblCommunicationUserId.AutoSize = true
    	Me.lblCommunicationUserId.Location = New System.Drawing.Point(7, 46)
    	Me.lblCommunicationUserId.Name = "lblCommunicationUserId"
    	Me.lblCommunicationUserId.Size = New System.Drawing.Size(43, 13)
    	Me.lblCommunicationUserId.TabIndex = 1
    	Me.lblCommunicationUserId.Text = "User ID"
    	'
    	'lblCommunicationDateTime
    	'
    	Me.lblCommunicationDateTime.AutoSize = true
    	Me.lblCommunicationDateTime.Location = New System.Drawing.Point(7, 20)
    	Me.lblCommunicationDateTime.Name = "lblCommunicationDateTime"
    	Me.lblCommunicationDateTime.Size = New System.Drawing.Size(98, 13)
    	Me.lblCommunicationDateTime.TabIndex = 0
    	Me.lblCommunicationDateTime.Text = "Date/Time Created"
    	'
    	'tabPayments
    	'
    	Me.tabPayments.Controls.Add(Me.txtPaymentsCumulativeCreditHoursPaid)
    	Me.tabPayments.Controls.Add(Me.Label11)
    	Me.tabPayments.Controls.Add(Me.btnPaymentsViewDocuments)
    	Me.tabPayments.Controls.Add(Me.btnPaymentsLinkDocument)
    	Me.tabPayments.Controls.Add(Me.btnPaymentsSaveChanges)
    	Me.tabPayments.Controls.Add(Me.pnlPayments)
    	Me.tabPayments.Location = New System.Drawing.Point(4, 22)
    	Me.tabPayments.Name = "tabPayments"
    	Me.tabPayments.Padding = New System.Windows.Forms.Padding(3)
    	Me.tabPayments.Size = New System.Drawing.Size(933, 723)
    	Me.tabPayments.TabIndex = 4
    	Me.tabPayments.Text = "Payments"
    	Me.tabPayments.UseVisualStyleBackColor = true
    	'
    	'txtPaymentsCumulativeCreditHoursPaid
    	'
    	Me.txtPaymentsCumulativeCreditHoursPaid.Location = New System.Drawing.Point(769, 252)
    	Me.txtPaymentsCumulativeCreditHoursPaid.Name = "txtPaymentsCumulativeCreditHoursPaid"
    	Me.txtPaymentsCumulativeCreditHoursPaid.ReadOnly = true
    	Me.txtPaymentsCumulativeCreditHoursPaid.Size = New System.Drawing.Size(33, 20)
    	Me.txtPaymentsCumulativeCreditHoursPaid.TabIndex = 65
    	Me.txtPaymentsCumulativeCreditHoursPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    	'
    	'Label11
    	'
    	Me.Label11.AutoSize = true
    	Me.Label11.Location = New System.Drawing.Point(741, 223)
    	Me.Label11.Name = "Label11"
    	Me.Label11.Size = New System.Drawing.Size(89, 26)
    	Me.Label11.TabIndex = 64
    	Me.Label11.Text = "Cumulative Credit"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Hours Paid"
    	Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    	'
    	'btnPaymentsViewDocuments
    	'
    	Me.btnPaymentsViewDocuments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnPaymentsViewDocuments.Location = New System.Drawing.Point(734, 145)
    	Me.btnPaymentsViewDocuments.Name = "btnPaymentsViewDocuments"
    	Me.btnPaymentsViewDocuments.Size = New System.Drawing.Size(99, 42)
    	Me.btnPaymentsViewDocuments.TabIndex = 63
    	Me.btnPaymentsViewDocuments.Text = "View Documents"
    	Me.btnPaymentsViewDocuments.UseVisualStyleBackColor = true
    	'
    	'btnPaymentsLinkDocument
    	'
    	Me.btnPaymentsLinkDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnPaymentsLinkDocument.Location = New System.Drawing.Point(734, 88)
    	Me.btnPaymentsLinkDocument.Name = "btnPaymentsLinkDocument"
    	Me.btnPaymentsLinkDocument.Size = New System.Drawing.Size(99, 42)
    	Me.btnPaymentsLinkDocument.TabIndex = 62
    	Me.btnPaymentsLinkDocument.Text = "Link Document"
    	Me.btnPaymentsLinkDocument.UseVisualStyleBackColor = true
    	'
    	'btnPaymentsSaveChanges
    	'
    	Me.btnPaymentsSaveChanges.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnPaymentsSaveChanges.Location = New System.Drawing.Point(734, 6)
    	Me.btnPaymentsSaveChanges.Name = "btnPaymentsSaveChanges"
    	Me.btnPaymentsSaveChanges.Size = New System.Drawing.Size(99, 32)
    	Me.btnPaymentsSaveChanges.TabIndex = 61
    	Me.btnPaymentsSaveChanges.Text = "Save Changes"
    	Me.btnPaymentsSaveChanges.UseVisualStyleBackColor = true
    	'
    	'pnlPayments
    	'
    	Me.pnlPayments.AutoScroll = true
    	Me.pnlPayments.Controls.Add(Me.btnPaymentsNewPayment)
    	Me.pnlPayments.Dock = System.Windows.Forms.DockStyle.Left
    	Me.pnlPayments.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
    	Me.pnlPayments.Location = New System.Drawing.Point(3, 3)
    	Me.pnlPayments.Name = "pnlPayments"
    	Me.pnlPayments.Size = New System.Drawing.Size(654, 68)
    	Me.pnlPayments.TabIndex = 0
    	Me.pnlPayments.WrapContents = false
    	'
    	'btnPaymentsNewPayment
    	'
    	Me.btnPaymentsNewPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnPaymentsNewPayment.Location = New System.Drawing.Point(3, 3)
    	Me.btnPaymentsNewPayment.Name = "btnPaymentsNewPayment"
    	Me.btnPaymentsNewPayment.Size = New System.Drawing.Size(99, 32)
    	Me.btnPaymentsNewPayment.TabIndex = 62
    	Me.btnPaymentsNewPayment.Text = "New Payment"
    	Me.btnPaymentsNewPayment.UseVisualStyleBackColor = true
    	'
    	'tabPaymentBatch
    	'
    	Me.tabPaymentBatch.Controls.Add(Me.splPaymentBatchSplitter)
    	Me.tabPaymentBatch.Location = New System.Drawing.Point(4, 22)
    	Me.tabPaymentBatch.Name = "tabPaymentBatch"
    	Me.tabPaymentBatch.Size = New System.Drawing.Size(933, 723)
    	Me.tabPaymentBatch.TabIndex = 5
    	Me.tabPaymentBatch.Text = "Payment Batch"
    	Me.tabPaymentBatch.UseVisualStyleBackColor = true
    	'
    	'splPaymentBatchSplitter
    	'
    	Me.splPaymentBatchSplitter.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.splPaymentBatchSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
    	Me.splPaymentBatchSplitter.Location = New System.Drawing.Point(0, 0)
    	Me.splPaymentBatchSplitter.Name = "splPaymentBatchSplitter"
    	Me.splPaymentBatchSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal
    	'
    	'splPaymentBatchSplitter.Panel1
    	'
    	Me.splPaymentBatchSplitter.Panel1.Controls.Add(Me.btnPaymentBatchPreliminary)
    	Me.splPaymentBatchSplitter.Panel1.Controls.Add(Me.lblPaymentBatchNumber)
    	Me.splPaymentBatchSplitter.Panel1.Controls.Add(Me.btnPaymentBatchFinal)
    	'
    	'splPaymentBatchSplitter.Panel2
    	'
    	Me.splPaymentBatchSplitter.Panel2.Controls.Add(Me.txtPaymentBatchLog)
    	Me.splPaymentBatchSplitter.Size = New System.Drawing.Size(192, 74)
    	Me.splPaymentBatchSplitter.SplitterDistance = 45
    	Me.splPaymentBatchSplitter.TabIndex = 67
    	'
    	'btnPaymentBatchPreliminary
    	'
    	Me.btnPaymentBatchPreliminary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnPaymentBatchPreliminary.Location = New System.Drawing.Point(8, 15)
    	Me.btnPaymentBatchPreliminary.Name = "btnPaymentBatchPreliminary"
    	Me.btnPaymentBatchPreliminary.Size = New System.Drawing.Size(99, 42)
    	Me.btnPaymentBatchPreliminary.TabIndex = 63
    	Me.btnPaymentBatchPreliminary.Text = "Preliminary"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Run"
    	Me.btnPaymentBatchPreliminary.UseVisualStyleBackColor = true
    	'
    	'lblPaymentBatchNumber
    	'
    	Me.lblPaymentBatchNumber.AutoSize = true
    	Me.lblPaymentBatchNumber.Location = New System.Drawing.Point(304, 30)
    	Me.lblPaymentBatchNumber.Name = "lblPaymentBatchNumber"
    	Me.lblPaymentBatchNumber.Size = New System.Drawing.Size(75, 13)
    	Me.lblPaymentBatchNumber.TabIndex = 65
    	Me.lblPaymentBatchNumber.Text = "Batch Number"
    	Me.lblPaymentBatchNumber.Visible = false
    	'
    	'btnPaymentBatchFinal
    	'
    	Me.btnPaymentBatchFinal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.btnPaymentBatchFinal.Location = New System.Drawing.Point(172, 15)
    	Me.btnPaymentBatchFinal.Name = "btnPaymentBatchFinal"
    	Me.btnPaymentBatchFinal.Size = New System.Drawing.Size(99, 42)
    	Me.btnPaymentBatchFinal.TabIndex = 64
    	Me.btnPaymentBatchFinal.Text = "Final"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Run"
    	Me.btnPaymentBatchFinal.UseVisualStyleBackColor = true
    	'
    	'txtPaymentBatchLog
    	'
    	Me.txtPaymentBatchLog.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.txtPaymentBatchLog.Location = New System.Drawing.Point(0, 0)
    	Me.txtPaymentBatchLog.Multiline = true
    	Me.txtPaymentBatchLog.Name = "txtPaymentBatchLog"
    	Me.txtPaymentBatchLog.ReadOnly = true
    	Me.txtPaymentBatchLog.Size = New System.Drawing.Size(192, 25)
    	Me.txtPaymentBatchLog.TabIndex = 66
    	'
    	'lblHeaderRecordLocked
    	'
    	Me.lblHeaderRecordLocked.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.lblHeaderRecordLocked.AutoSize = true
    	Me.lblHeaderRecordLocked.BackColor = System.Drawing.Color.LemonChiffon
    	Me.lblHeaderRecordLocked.Font = New System.Drawing.Font("Microsoft Sans Serif", 10!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
    	Me.lblHeaderRecordLocked.ForeColor = System.Drawing.Color.Red
    	Me.lblHeaderRecordLocked.Location = New System.Drawing.Point(667, 7)
    	Me.lblHeaderRecordLocked.Name = "lblHeaderRecordLocked"
    	Me.lblHeaderRecordLocked.Size = New System.Drawing.Size(261, 17)
    	Me.lblHeaderRecordLocked.TabIndex = 6
    	Me.lblHeaderRecordLocked.Text = "Record locked by MrSnuffleupagus"
    	Me.lblHeaderRecordLocked.Visible = false
    	'
    	'ToolStrip1
    	'
    	Me.ToolStrip1.CanOverflow = false
    	Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnDistrictCommunication, Me.btnHighSchoolCommunications, Me.btnJuniorHighSchoolCommuncation, Me.btnMiscCommunication, Me.ToolStripSeparator2, Me.btnReports, Me.btnTransactionAuditHistory, Me.btnWebPasswordReset, Me.btnQuickBatchReview, Me.ToolStripSeparator1, Me.btnExit})
    	Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
    	Me.ToolStrip1.Name = "ToolStrip1"
    	Me.ToolStrip1.Size = New System.Drawing.Size(941, 25)
    	Me.ToolStrip1.TabIndex = 1
    	Me.ToolStrip1.Text = "ToolStrip1"
    	'
    	'btnDistrictCommunication
    	'
    	Me.btnDistrictCommunication.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.school
    	Me.btnDistrictCommunication.ImageTransparentColor = System.Drawing.Color.Magenta
    	Me.btnDistrictCommunication.Name = "btnDistrictCommunication"
    	Me.btnDistrictCommunication.Size = New System.Drawing.Size(96, 22)
    	Me.btnDistrictCommunication.Text = "District Comm."
    	'
    	'btnHighSchoolCommunications
    	'
    	Me.btnHighSchoolCommunications.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.school
    	Me.btnHighSchoolCommunications.ImageTransparentColor = System.Drawing.Color.Magenta
    	Me.btnHighSchoolCommunications.Name = "btnHighSchoolCommunications"
    	Me.btnHighSchoolCommunications.Size = New System.Drawing.Size(76, 22)
    	Me.btnHighSchoolCommunications.Text = "HS Comm."
    	'
    	'btnJuniorHighSchoolCommuncation
    	'
    	Me.btnJuniorHighSchoolCommuncation.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.school
    	Me.btnJuniorHighSchoolCommuncation.ImageTransparentColor = System.Drawing.Color.Magenta
    	Me.btnJuniorHighSchoolCommuncation.Name = "btnJuniorHighSchoolCommuncation"
    	Me.btnJuniorHighSchoolCommuncation.Size = New System.Drawing.Size(93, 22)
    	Me.btnJuniorHighSchoolCommuncation.Text = "JH/MS Comm."
    	'
    	'btnMiscCommunication
    	'
    	Me.btnMiscCommunication.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.cellphone
    	Me.btnMiscCommunication.ImageTransparentColor = System.Drawing.Color.Magenta
    	Me.btnMiscCommunication.Name = "btnMiscCommunication"
    	Me.btnMiscCommunication.Size = New System.Drawing.Size(87, 22)
    	Me.btnMiscCommunication.Text = "Misc. Comm."
    	'
    	'ToolStripSeparator2
    	'
    	Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
    	Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
    	'
    	'btnReports
    	'
    	Me.btnReports.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.reports
    	Me.btnReports.ImageTransparentColor = System.Drawing.Color.Magenta
    	Me.btnReports.Name = "btnReports"
    	Me.btnReports.Size = New System.Drawing.Size(65, 22)
    	Me.btnReports.Text = "Reports"
    	'
    	'btnTransactionAuditHistory
    	'
    	Me.btnTransactionAuditHistory.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.audit
    	Me.btnTransactionAuditHistory.ImageTransparentColor = System.Drawing.Color.Magenta
    	Me.btnTransactionAuditHistory.Name = "btnTransactionAuditHistory"
    	Me.btnTransactionAuditHistory.Size = New System.Drawing.Size(111, 22)
    	Me.btnTransactionAuditHistory.Text = "Transaction Audit"
    	'
    	'btnWebPasswordReset
    	'
    	Me.btnWebPasswordReset.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.Keys
    	Me.btnWebPasswordReset.ImageTransparentColor = System.Drawing.Color.Magenta
    	Me.btnWebPasswordReset.Name = "btnWebPasswordReset"
    	Me.btnWebPasswordReset.Size = New System.Drawing.Size(129, 22)
    	Me.btnWebPasswordReset.Text = "Web Password Reset"
    	'
    	'btnQuickBatchReview
    	'
    	Me.btnQuickBatchReview.Enabled = false
    	Me.btnQuickBatchReview.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.preferences
    	Me.btnQuickBatchReview.ImageTransparentColor = System.Drawing.Color.Magenta
    	Me.btnQuickBatchReview.Name = "btnQuickBatchReview"
    	Me.btnQuickBatchReview.Size = New System.Drawing.Size(121, 22)
    	Me.btnQuickBatchReview.Text = "Batch Quick Review"
    	'
    	'ToolStripSeparator1
    	'
    	Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
    	Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
    	'
    	'btnExit
    	'
    	Me.btnExit.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.quit
    	Me.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta
    	Me.btnExit.Name = "btnExit"
    	Me.btnExit.Size = New System.Drawing.Size(45, 22)
    	Me.btnExit.Text = "Exit"
    	'
    	'lblHeaderStudentName
    	'
    	Me.lblHeaderStudentName.AutoSize = true
    	Me.lblHeaderStudentName.BackColor = System.Drawing.Color.Transparent
    	Me.lblHeaderStudentName.Location = New System.Drawing.Point(172, 9)
    	Me.lblHeaderStudentName.Name = "lblHeaderStudentName"
    	Me.lblHeaderStudentName.Size = New System.Drawing.Size(35, 13)
    	Me.lblHeaderStudentName.TabIndex = 2
    	Me.lblHeaderStudentName.Text = "Name"
    	Me.lblHeaderStudentName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    	'
    	'txtHeaderStudentName
    	'
    	Me.txtHeaderStudentName.BackColor = System.Drawing.Color.WhiteSmoke
    	Me.txtHeaderStudentName.Enabled = false
    	Me.txtHeaderStudentName.Location = New System.Drawing.Point(213, 6)
    	Me.txtHeaderStudentName.Name = "txtHeaderStudentName"
    	Me.txtHeaderStudentName.ReadOnly = true
    	Me.txtHeaderStudentName.Size = New System.Drawing.Size(205, 20)
    	Me.txtHeaderStudentName.TabIndex = 3
    	'
    	'pnlStudentInfo
    	'
    	Me.pnlStudentInfo.BackColor = System.Drawing.Color.Gainsboro
    	Me.pnlStudentInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    	Me.pnlStudentInfo.Controls.Add(Me.lblHeaderRecordLocked)
    	Me.pnlStudentInfo.Controls.Add(Me.lblHeaderStudentId)
    	Me.pnlStudentInfo.Controls.Add(Me.txtHeaderStudentId)
    	Me.pnlStudentInfo.Controls.Add(Me.lblHeaderAwardStatus)
    	Me.pnlStudentInfo.Controls.Add(Me.txtHeaderAwardStatus)
    	Me.pnlStudentInfo.Controls.Add(Me.lblHeaderStudentName)
    	Me.pnlStudentInfo.Controls.Add(Me.txtHeaderStudentName)
    	Me.pnlStudentInfo.Dock = System.Windows.Forms.DockStyle.Top
    	Me.pnlStudentInfo.Location = New System.Drawing.Point(0, 25)
    	Me.pnlStudentInfo.Name = "pnlStudentInfo"
    	Me.pnlStudentInfo.Size = New System.Drawing.Size(941, 32)
    	Me.pnlStudentInfo.TabIndex = 74
    	'
    	'lblHeaderStudentId
    	'
    	Me.lblHeaderStudentId.AutoSize = true
    	Me.lblHeaderStudentId.BackColor = System.Drawing.Color.Transparent
    	Me.lblHeaderStudentId.Location = New System.Drawing.Point(3, 9)
    	Me.lblHeaderStudentId.Name = "lblHeaderStudentId"
    	Me.lblHeaderStudentId.Size = New System.Drawing.Size(58, 13)
    	Me.lblHeaderStudentId.TabIndex = 6
    	Me.lblHeaderStudentId.Text = "Student ID"
    	Me.lblHeaderStudentId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    	'
    	'txtHeaderStudentId
    	'
    	Me.txtHeaderStudentId.BackColor = System.Drawing.Color.WhiteSmoke
    	Me.txtHeaderStudentId.Enabled = false
    	Me.txtHeaderStudentId.Location = New System.Drawing.Point(67, 6)
    	Me.txtHeaderStudentId.Name = "txtHeaderStudentId"
    	Me.txtHeaderStudentId.ReadOnly = true
    	Me.txtHeaderStudentId.Size = New System.Drawing.Size(89, 20)
    	Me.txtHeaderStudentId.TabIndex = 7
    	'
    	'lblHeaderAwardStatus
    	'
    	Me.lblHeaderAwardStatus.AutoSize = true
    	Me.lblHeaderAwardStatus.BackColor = System.Drawing.Color.Transparent
    	Me.lblHeaderAwardStatus.Location = New System.Drawing.Point(434, 9)
    	Me.lblHeaderAwardStatus.Name = "lblHeaderAwardStatus"
    	Me.lblHeaderAwardStatus.Size = New System.Drawing.Size(70, 13)
    	Me.lblHeaderAwardStatus.TabIndex = 4
    	Me.lblHeaderAwardStatus.Text = "Award Status"
    	Me.lblHeaderAwardStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    	'
    	'txtHeaderAwardStatus
    	'
    	Me.txtHeaderAwardStatus.BackColor = System.Drawing.Color.WhiteSmoke
    	Me.txtHeaderAwardStatus.Enabled = false
    	Me.txtHeaderAwardStatus.Location = New System.Drawing.Point(510, 6)
    	Me.txtHeaderAwardStatus.Name = "txtHeaderAwardStatus"
    	Me.txtHeaderAwardStatus.ReadOnly = true
    	Me.txtHeaderAwardStatus.Size = New System.Drawing.Size(127, 20)
    	Me.txtHeaderAwardStatus.TabIndex = 5
    	'
    	'frmRegents
    	'
    	Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
    	Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    	Me.ClientSize = New System.Drawing.Size(941, 815)
    	Me.Controls.Add(Me.pnlStudentInfo)
    	Me.Controls.Add(Me.ToolStrip1)
    	Me.Controls.Add(Me.tabControlMaster)
    	Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
    	Me.Name = "frmRegents"
    	Me.Text = "Regents Scholarship Application Review"
    	Me.tabControlMaster.ResumeLayout(false)
    	Me.tabMainMenu.ResumeLayout(false)
    	Me.grpMainMenuStudentAccountSearch.ResumeLayout(false)
    	Me.grpMainMenuStudentAccountSearch.PerformLayout
    	CType(Me.dgvMainMenuSearchMatches,System.ComponentModel.ISupportInitialize).EndInit
    	CType(Me.MainMenuSearchResultBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	Me.tabDemographics.ResumeLayout(false)
    	Me.tabDemographics.PerformLayout
    	Me.grpDemographics411.ResumeLayout(false)
    	Me.grpHearAboutUs.ResumeLayout(false)
    	Me.grpHearAboutUs.PerformLayout
    	CType(Me.ScholarshipApplicationBindingSource1,System.ComponentModel.ISupportInitialize).EndInit
    	Me.grpAuthThirdParties.ResumeLayout(false)
    	Me.Panel1.ResumeLayout(false)
    	CType(Me.StudentBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	Me.grpDemographicsEligibility.ResumeLayout(false)
    	Me.grpDemographicsEligibility.PerformLayout
    	Me.grpDemographicsContact.ResumeLayout(false)
    	Me.grpDemographicsContact.PerformLayout
    	CType(Me.SchoolEmailBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	CType(Me.PersonalEmailBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	CType(Me.CellPhoneBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	CType(Me.PrimaryPhoneBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	Me.grpDemographicsAddress.ResumeLayout(false)
    	Me.grpDemographicsAddress.PerformLayout
    	CType(Me.MailingAddressBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	Me.grpDemographicsPersonal.ResumeLayout(false)
    	Me.grpDemographicsPersonal.PerformLayout
    	Me.tabApplication.ResumeLayout(false)
    	Me.GroupBox3.ResumeLayout(false)
    	Me.GroupBox3.PerformLayout
    	CType(Me.CollegeBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	Me.GroupBox2.ResumeLayout(false)
    	Me.GroupBox2.PerformLayout
    	Me.grpApplicationCollegeEnrollment.ResumeLayout(false)
    	Me.grpApplicationCollegeEnrollment.PerformLayout
    	Me.grpApplicationDocumentStatus.ResumeLayout(false)
    	Me.grpApplicationDocumentStatus.PerformLayout
    	Me.grpApplicationReviewStatus.ResumeLayout(false)
    	Me.grpApplicationReviewStatus.PerformLayout
    	Me.grpApplicationClasses.ResumeLayout(false)
    	Me.tabControlClasses.ResumeLayout(false)
    	Me.tabEnglish.ResumeLayout(false)
    	Me.tabEnglish.PerformLayout
    	Me.tabMathematics.ResumeLayout(false)
    	Me.tabMathematics.PerformLayout
    	Me.tabScienceWithLab.ResumeLayout(false)
    	Me.tabScienceWithLab.PerformLayout
    	Me.tabSocialScience.ResumeLayout(false)
    	Me.tabSocialScience.PerformLayout
    	Me.tabForeignLanguage.ResumeLayout(false)
    	Me.tabForeignLanguage.PerformLayout
    	Me.grpApplicationAwardStatus.ResumeLayout(false)
    	Me.grpApplicationAwardStatus.PerformLayout
    	CType(Me.PrimaryAwardBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	Me.grpApplicationDenialReasons.ResumeLayout(false)
    	CType(Me.ExemplaryAwardBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	CType(Me.UespSupplementalAwardBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	Me.grpApplicationHighSchool.ResumeLayout(false)
    	Me.grpApplicationHighSchool.PerformLayout
    	CType(Me.HighSchoolBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	Me.tabCommunications.ResumeLayout(false)
    	CType(Me.CommunicationDataGridView,System.ComponentModel.ISupportInitialize).EndInit
    	CType(Me.CommunicationBindingSource,System.ComponentModel.ISupportInitialize).EndInit
    	Me.grpCommunicationRecord.ResumeLayout(false)
    	Me.grpCommunicationRecord.PerformLayout
    	Me.tabPayments.ResumeLayout(false)
    	Me.tabPayments.PerformLayout
    	Me.pnlPayments.ResumeLayout(false)
    	Me.tabPaymentBatch.ResumeLayout(false)
    	Me.splPaymentBatchSplitter.Panel1.ResumeLayout(false)
    	Me.splPaymentBatchSplitter.Panel1.PerformLayout
    	Me.splPaymentBatchSplitter.Panel2.ResumeLayout(false)
    	Me.splPaymentBatchSplitter.Panel2.PerformLayout
        Me.splPaymentBatchSplitter.ResumeLayout(False)
    	Me.ToolStrip1.ResumeLayout(false)
    	Me.ToolStrip1.PerformLayout
    	Me.pnlStudentInfo.ResumeLayout(false)
    	Me.pnlStudentInfo.PerformLayout
    	Me.ResumeLayout(false)
    	Me.PerformLayout
    End Sub
    Friend WithEvents tabControlMaster As System.Windows.Forms.TabControl
    Friend WithEvents tabMainMenu As System.Windows.Forms.TabPage
    Friend WithEvents tabDemographics As System.Windows.Forms.TabPage
    Friend WithEvents grpMainMenuStudentAccountSearch As System.Windows.Forms.GroupBox
    Friend WithEvents tabApplication As System.Windows.Forms.TabPage
    Friend WithEvents tabCommunications As System.Windows.Forms.TabPage
    Friend WithEvents lblMainMenuSearchDateOfBirth As System.Windows.Forms.Label
    Friend WithEvents lblMainMenuSearchLastName As System.Windows.Forms.Label
    Friend WithEvents lblMainMenuSearchFirstName As System.Windows.Forms.Label
    Friend WithEvents lblMainMenuSearchSsn As System.Windows.Forms.Label
    Friend WithEvents txtMainMenuSearchStateStudentId As System.Windows.Forms.TextBox
    Friend WithEvents lblMainMenuSearchStateStudentId As System.Windows.Forms.Label
    Friend WithEvents lblMainMenuSelectMatch As System.Windows.Forms.Label
    Friend WithEvents dgvMainMenuSearchMatches As System.Windows.Forms.DataGridView
    Friend WithEvents btnMainMenuSearch As System.Windows.Forms.Button
    Friend WithEvents txtMainMenuSearchLastName As System.Windows.Forms.TextBox
    Friend WithEvents txtMainMenuSearchFirstName As System.Windows.Forms.TextBox
    Friend WithEvents txtMainMenuSearchSsn As System.Windows.Forms.TextBox
    Friend WithEvents lblMainMenuNoSearchResults As System.Windows.Forms.Label
    Friend WithEvents grpCommunicationRecord As System.Windows.Forms.GroupBox
    Friend WithEvents lblCommunicationSubject As System.Windows.Forms.Label
    Friend WithEvents lblCommunicationType As System.Windows.Forms.Label
    Friend WithEvents lblCommunicationUserId As System.Windows.Forms.Label
    Friend WithEvents lblCommunicationDateTime As System.Windows.Forms.Label
    Friend WithEvents txtCommunicationComments As System.Windows.Forms.TextBox
    Friend WithEvents txtCommunicationSubject As System.Windows.Forms.TextBox
    Friend WithEvents cmbCommunicationType As System.Windows.Forms.ComboBox
    Friend WithEvents txtCommunicationUserId As System.Windows.Forms.TextBox
    Friend WithEvents txtCommunicationDateTime As System.Windows.Forms.TextBox
    Friend WithEvents btnCommunicationClearFields As System.Windows.Forms.Button
    Friend WithEvents btnCommunicationSave As System.Windows.Forms.Button
    Friend WithEvents cmbCommunicationSource As System.Windows.Forms.ComboBox
    Friend WithEvents lblCommunicationSource As System.Windows.Forms.Label
    Friend WithEvents btnCommunicationViewDocuments As System.Windows.Forms.Button
    Friend WithEvents btnCommunicationLinkDocument As System.Windows.Forms.Button
    Friend WithEvents grpDemographicsPersonal As System.Windows.Forms.GroupBox
    Friend WithEvents lblDemographicsStateStudentId As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsEthnicity As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsGender As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsName As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsAlternateLastName As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsDateOfBirth As System.Windows.Forms.Label
    Friend WithEvents txtDemographicsLastName As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsMiddleName As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsFirstName As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsAlternateLastName As System.Windows.Forms.TextBox
    Friend WithEvents cmbDemographicsEthnicity As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDemographicsGender As System.Windows.Forms.ComboBox
    Friend WithEvents grpDemographicsAddress As System.Windows.Forms.GroupBox
    Friend WithEvents lblDemographicsCityStateZip As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsStreetAddress2 As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsStreetAddress1 As System.Windows.Forms.Label
    Friend WithEvents cmbDemographicsState As System.Windows.Forms.ComboBox
    Friend WithEvents txtDemographicsCity As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsStreetAddress2 As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsStreetAddress1 As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsZip As System.Windows.Forms.TextBox
    Friend WithEvents chkDemographicsValidAddress As System.Windows.Forms.CheckBox
    Friend WithEvents txtDemographicsCountry As System.Windows.Forms.TextBox
    Friend WithEvents grpDemographicsContact As System.Windows.Forms.GroupBox
    Friend WithEvents lblDemographicsCountry As System.Windows.Forms.Label
    Friend WithEvents txtDemographicsPrimaryPhone As System.Windows.Forms.TextBox
    Friend WithEvents lblDemographicsSchoolEmail As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsPersonalEmail As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsCellPhone As System.Windows.Forms.Label
    Friend WithEvents lblDemographicsPrimaryPhone As System.Windows.Forms.Label
    Friend WithEvents txtDemographicsSchoolEmail As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsPersonalEmail As System.Windows.Forms.TextBox
    Friend WithEvents txtDemographicsCellPhone As System.Windows.Forms.TextBox
    Friend WithEvents grpDemographicsEligibility As System.Windows.Forms.GroupBox
    Friend WithEvents chkDemographicsCriminalRecord As System.Windows.Forms.CheckBox
    Friend WithEvents chkDemographicsIntendsToApplyForFederalAid As System.Windows.Forms.CheckBox
    Friend WithEvents chkDemographicsEligibleForFederalAid As System.Windows.Forms.CheckBox
    Friend WithEvents chkDemographicsUsCitizen As System.Windows.Forms.CheckBox
    Friend WithEvents btnDemographicsSaveChanges As System.Windows.Forms.Button
    Friend WithEvents txtDemographicsStateStudentId As System.Windows.Forms.TextBox
    Friend WithEvents grpApplicationClasses As System.Windows.Forms.GroupBox
    Friend WithEvents grpApplicationHighSchool As System.Windows.Forms.GroupBox
    Friend WithEvents grpApplicationAwardStatus As System.Windows.Forms.GroupBox
    Friend WithEvents lblApplicationAwardStatus As System.Windows.Forms.Label
    Friend WithEvents cmbApplicationAwardStatus As System.Windows.Forms.ComboBox
    Friend WithEvents lblApplicationAwardStatusDate As System.Windows.Forms.Label
    Friend WithEvents txtApplicationAwardStatusUserId As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationAwardStatusUserId As System.Windows.Forms.Label
    Friend WithEvents lblApplicationSupplementalAwardAmount As System.Windows.Forms.Label
    Friend WithEvents txtApplicationSupplementalAwardAmount As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationSupplementalAwardApproved As System.Windows.Forms.CheckBox
    Friend WithEvents chkApplicationExemplaryAwardEarned As System.Windows.Forms.CheckBox
    Friend WithEvents lblApplicationBaseAwardAmount As System.Windows.Forms.Label
    Friend WithEvents txtApplicationBaseAwardAmount As System.Windows.Forms.TextBox
    Friend WithEvents btnApplicationSaveChanges As System.Windows.Forms.Button
    Friend WithEvents cmbApplicationDefermentReason As System.Windows.Forms.ComboBox
    Friend WithEvents lblApplicationDefermentReason As System.Windows.Forms.Label
    Friend WithEvents lblApplicationDefermentEndDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationDefermentBeginDate As System.Windows.Forms.Label
    Friend WithEvents cmbApplicationLeaveOfAbsenceReason As System.Windows.Forms.ComboBox
    Friend WithEvents lblApplicationLeaveOfAbsenceReason As System.Windows.Forms.Label
    Friend WithEvents lblApplicationLeaveOfAbsenceEndDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationLeaveOfAbsenceBeginDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationHighSchoolName As System.Windows.Forms.Label
    Friend WithEvents chkApplicationUtahHighSchool As System.Windows.Forms.CheckBox
    Friend WithEvents lblApplicationSchoolDistrict As System.Windows.Forms.Label
    Friend WithEvents lblApplicationHighSchoolCity As System.Windows.Forms.Label
    Friend WithEvents txtApplicationCeebCode As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationGraduationDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationCeebCode As System.Windows.Forms.Label
    Friend WithEvents txtApplicationActCompositeScore As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationActCompositeScore As System.Windows.Forms.Label
    Friend WithEvents txtApplicationCumulativeGpa As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationCumulativeGpa As System.Windows.Forms.Label
    Friend WithEvents chkApplicationIbDiploma As System.Windows.Forms.CheckBox
    Friend WithEvents chkDemographicsValidCellPhone As System.Windows.Forms.CheckBox
    Friend WithEvents chkDemographicsValidPrimaryPhone As System.Windows.Forms.CheckBox
    Friend WithEvents chkDemographicsValidSchoolEmail As System.Windows.Forms.CheckBox
    Friend WithEvents chkDemographicsValidPersonalEmail As System.Windows.Forms.CheckBox
    Friend WithEvents grpApplicationReviewStatus As System.Windows.Forms.GroupBox
    Friend WithEvents chkApplicationInitialTranscriptReview As System.Windows.Forms.CheckBox
    Friend WithEvents txtApplicationBaseAwardReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationBaseAwardReview As System.Windows.Forms.CheckBox
    Friend WithEvents txtApplicationInitialAwardReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationInitialAwardReview As System.Windows.Forms.CheckBox
    Friend WithEvents txtApplicationCategoryReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationCategoryReview As System.Windows.Forms.CheckBox
    Friend WithEvents txtApplicationClassReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationClassReview As System.Windows.Forms.CheckBox
    Friend WithEvents txtApplicationFinalTranscriptReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationFinalTranscriptReview As System.Windows.Forms.CheckBox
    Friend WithEvents txtApplicationInitialTranscriptReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationUespAwardReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationUespAwardReview As System.Windows.Forms.CheckBox
    Friend WithEvents txtApplicationExemplaryAwardReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationExemplaryAwardReview As System.Windows.Forms.CheckBox
    Friend WithEvents grpApplicationDocumentStatus As System.Windows.Forms.GroupBox
    Friend WithEvents lblApplicationFinalCollegeTranscriptReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationProofOfEnrollmentReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationConditionalAcceptanceFormReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationSignaturePageReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationInitialCollegeTranscriptReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationFinalHighSchoolTranscriptReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationInitialHighSchoolTranscriptReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationReceivedDate As System.Windows.Forms.Label
    Friend WithEvents txtApplicationActReadingScore As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationActReadingScore As System.Windows.Forms.Label
    Friend WithEvents txtApplicationActScienceScore As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationActScienceScore As System.Windows.Forms.Label
    Friend WithEvents txtApplicationActMathScore As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationActMathScore As System.Windows.Forms.Label
    Friend WithEvents txtApplicationActEnglishScore As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationActEnglishScore As System.Windows.Forms.Label
    Friend WithEvents radApplicationUbsctFail As System.Windows.Forms.RadioButton
    Friend WithEvents radApplicationUbsctPass As System.Windows.Forms.RadioButton
    Friend WithEvents lblApplicationUbsct As System.Windows.Forms.Label
    Friend WithEvents grpApplicationCollegeEnrollment As System.Windows.Forms.GroupBox
    Friend WithEvents lblApplicationCollegeName As System.Windows.Forms.Label
    Friend WithEvents txtApplicationEnrolledCredits As System.Windows.Forms.TextBox
    Friend WithEvents cmbApplicationCollegeName As System.Windows.Forms.ComboBox
    Friend WithEvents lblApplicationEnrolledCredits As System.Windows.Forms.Label
    Friend WithEvents lblApplicationTermBeginDate As System.Windows.Forms.Label
    Friend WithEvents tabControlClasses As System.Windows.Forms.TabControl
    Friend WithEvents tabEnglish As System.Windows.Forms.TabPage
    Friend WithEvents radEnglishRequirementMetInProgress As System.Windows.Forms.RadioButton
    Friend WithEvents radEnglishRequirementMetNo As System.Windows.Forms.RadioButton
    Friend WithEvents radEnglishRequirementMetYes As System.Windows.Forms.RadioButton
    Friend WithEvents lblEnglishRequirementMet As System.Windows.Forms.Label
    Friend WithEvents lblEnglishCredits As System.Windows.Forms.Label
    Friend WithEvents lblEnglishGradeLevel As System.Windows.Forms.Label
    Friend WithEvents lblEnglishTitle As System.Windows.Forms.Label
    Friend WithEvents tabMathematics As System.Windows.Forms.TabPage
    Friend WithEvents txtEnglishVerifiedBy As System.Windows.Forms.TextBox
    Friend WithEvents lblEnglishVerifiedBy As System.Windows.Forms.Label
    Friend WithEvents txtEnglishVerifiedDate As System.Windows.Forms.TextBox
    Friend WithEvents lblEnglishVerifiedDate As System.Windows.Forms.Label
    Friend WithEvents lblEnglishWeightedAverageGrade As System.Windows.Forms.Label
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnReports As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnTransactionAuditHistory As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnExit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents lblHeaderStudentName As System.Windows.Forms.Label
    Friend WithEvents txtHeaderStudentName As System.Windows.Forms.TextBox
    Friend WithEvents pnlStudentInfo As System.Windows.Forms.Panel
    Friend WithEvents lblHeaderAwardStatus As System.Windows.Forms.Label
    Friend WithEvents txtHeaderAwardStatus As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationExemplaryAwardAmount As System.Windows.Forms.Label
    Friend WithEvents txtApplicationExemplaryAwardAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationAwardStatusDate As System.Windows.Forms.TextBox
    Friend WithEvents cmbApplicationDenialReason6 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbApplicationDenialReason5 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbApplicationDenialReason4 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbApplicationDenialReason3 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbApplicationDenialReason2 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbApplicationDenialReason1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblApplicationAwardStatusLetterSent As System.Windows.Forms.Label
    Friend WithEvents txtApplicationUespAwardReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationExemplaryAwardReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationBaseAwardReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationInitialAwardReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationCategoryReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationClassReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationFinalTranscriptReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationInitialTranscriptReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationSecondTranscriptReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationSecondTranscriptReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationSecondTranscriptReview As System.Windows.Forms.CheckBox
    Friend WithEvents lblReviewStatusUserId As System.Windows.Forms.Label
    Friend WithEvents lblReviewStatusDate As System.Windows.Forms.Label
    Friend WithEvents txtApplicationSecondQuickReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationSecondQuickReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationSecondQuickReview As System.Windows.Forms.CheckBox
    Friend WithEvents txtApplicationFirstQuickReviewDate As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationFirstQuickReviewUserId As System.Windows.Forms.TextBox
    Friend WithEvents chkApplicationFirstQuickReview As System.Windows.Forms.CheckBox
    Friend WithEvents lblApplicationDefermentDecisionDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationDefermentRequestReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationLoaDecisionDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationLoaRequestReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationAppealDecisionDate As System.Windows.Forms.Label
    Friend WithEvents lblApplicationAppealReceivedDate As System.Windows.Forms.Label
    Friend WithEvents cmbApplicationTerm As System.Windows.Forms.ComboBox
    Friend WithEvents lblApplicationTerm As System.Windows.Forms.Label
    Friend WithEvents txtApplicationOtherScholarshipAwardsAmount As System.Windows.Forms.TextBox
    Friend WithEvents lblApplicationOtherScholarshipAwardsAmount As System.Windows.Forms.Label
    Friend WithEvents chkApplicationOtherScholarshipAwards As System.Windows.Forms.CheckBox
    Friend WithEvents lblEnglishWeightDesignation As System.Windows.Forms.Label
    Friend WithEvents lblEnglishGrades As System.Windows.Forms.Label
    Friend WithEvents lblMathematicsGrades As System.Windows.Forms.Label
    Friend WithEvents txtMathematicsVerifiedDate As System.Windows.Forms.TextBox
    Friend WithEvents lblMathematicsVerifiedDate As System.Windows.Forms.Label
    Friend WithEvents txtMathematicsVerifiedBy As System.Windows.Forms.TextBox
    Friend WithEvents lblMathematicsVerifiedBy As System.Windows.Forms.Label
    Friend WithEvents radMathematicsRequirementMetInProgress As System.Windows.Forms.RadioButton
    Friend WithEvents lblMathematicsWeightDesignation As System.Windows.Forms.Label
    Friend WithEvents radMathematicsRequirementMetNo As System.Windows.Forms.RadioButton
    Friend WithEvents radMathematicsRequirementMetYes As System.Windows.Forms.RadioButton
    Friend WithEvents lblMathematicsRequirementMet As System.Windows.Forms.Label
    Friend WithEvents lblMathematicsTitle As System.Windows.Forms.Label
    Friend WithEvents lblMathematicsGradeLevel As System.Windows.Forms.Label
    Friend WithEvents lblMathematicsCredits As System.Windows.Forms.Label
    Friend WithEvents lblMathematicsWeightedAverageGrade As System.Windows.Forms.Label
    Friend WithEvents tabScienceWithLab As System.Windows.Forms.TabPage
    Friend WithEvents lblScienceGrades As System.Windows.Forms.Label
    Friend WithEvents txtScienceVerifiedDate As System.Windows.Forms.TextBox
    Friend WithEvents lblScienceVerifiedDate As System.Windows.Forms.Label
    Friend WithEvents txtScienceVerifiedBy As System.Windows.Forms.TextBox
    Friend WithEvents lblScienceVerifiedBy As System.Windows.Forms.Label
    Friend WithEvents radScienceRequirementMetInProgress As System.Windows.Forms.RadioButton
    Friend WithEvents lblScienceWeightDesignation As System.Windows.Forms.Label
    Friend WithEvents radScienceRequirementMetNo As System.Windows.Forms.RadioButton
    Friend WithEvents radScienceRequirementMetYes As System.Windows.Forms.RadioButton
    Friend WithEvents lblScienceRequirementMet As System.Windows.Forms.Label
    Friend WithEvents lblScienceTitle As System.Windows.Forms.Label
    Friend WithEvents lblScienceGradeLevel As System.Windows.Forms.Label
    Friend WithEvents lblScienceCredits As System.Windows.Forms.Label
    Friend WithEvents lblScienceWeightedAverageGrade As System.Windows.Forms.Label
    Friend WithEvents tabSocialScience As System.Windows.Forms.TabPage
    Friend WithEvents lblSocialScienceGrades As System.Windows.Forms.Label
    Friend WithEvents txtSocialScienceVerifiedDate As System.Windows.Forms.TextBox
    Friend WithEvents lblSocialScienceVerifiedDate As System.Windows.Forms.Label
    Friend WithEvents txtSocialScienceVerifiedBy As System.Windows.Forms.TextBox
    Friend WithEvents lblSocialScienceVerifiedBy As System.Windows.Forms.Label
    Friend WithEvents radSocialScienceRequirementMetInProgress As System.Windows.Forms.RadioButton
    Friend WithEvents lblSocialScienceWeightDesignation As System.Windows.Forms.Label
    Friend WithEvents radSocialScienceRequirementMetNo As System.Windows.Forms.RadioButton
    Friend WithEvents radSocialScienceRequirementMetYes As System.Windows.Forms.RadioButton
    Friend WithEvents lblSocialScienceRequirementMet As System.Windows.Forms.Label
    Friend WithEvents lblSocialScienceTitle As System.Windows.Forms.Label
    Friend WithEvents lblSocialScienceGradeLevel As System.Windows.Forms.Label
    Friend WithEvents lblSocialScienceCredits As System.Windows.Forms.Label
    Friend WithEvents lblSocialScienceWeightedAverageGrade As System.Windows.Forms.Label
    Friend WithEvents tabForeignLanguage As System.Windows.Forms.TabPage
    Friend WithEvents lblForeignLanguageGrades As System.Windows.Forms.Label
    Friend WithEvents txtForeignLanguageVerifiedDate As System.Windows.Forms.TextBox
    Friend WithEvents lblForeignLanguageVerifiedDate As System.Windows.Forms.Label
    Friend WithEvents txtForeignLanguageVerifiedBy As System.Windows.Forms.TextBox
    Friend WithEvents lblForeignLanguageVerifiedBy As System.Windows.Forms.Label
    Friend WithEvents radForeignLanguageRequirementMetInProgress As System.Windows.Forms.RadioButton
    Friend WithEvents lblForeignLanguageWeightDesignation As System.Windows.Forms.Label
    Friend WithEvents radForeignLanguageRequirementMetNo As System.Windows.Forms.RadioButton
    Friend WithEvents radForeignLanguageRequirementMetYes As System.Windows.Forms.RadioButton
    Friend WithEvents lblForeignLanguageRequirementMet As System.Windows.Forms.Label
    Friend WithEvents lblForeignLanguageTitle As System.Windows.Forms.Label
    Friend WithEvents lblForeignLanguageGradeLevel As System.Windows.Forms.Label
    Friend WithEvents lblForeignLanguageCredits As System.Windows.Forms.Label
    Friend WithEvents lblForeignLanguageWeightedAverageGrade As System.Windows.Forms.Label
    Friend WithEvents foreignLanguageClass5 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents foreignLanguageClass4 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents foreignLanguageClass3 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents foreignLanguageClass2 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents foreignLanguageClass1 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents englishClass6 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents englishClass5 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents englishClass4 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents englishClass3 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents englishClass2 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents englishClass1 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents mathematicsClass6 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents mathematicsClass5 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents mathematicsClass4 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents mathematicsClass3 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents mathematicsClass2 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents mathematicsClass1 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents scienceClass8 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents scienceClass7 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents scienceClass6 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents scienceClass5 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents scienceClass4 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents scienceClass3 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents scienceClass2 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents scienceClass1 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents socialScienceClass8 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents socialScienceClass7 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents socialScienceClass6 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents socialScienceClass5 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents socialScienceClass4 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents socialScienceClass3 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents socialScienceClass2 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents socialScienceClass1 As RegentsScholarshipFrontEnd.ClassDataControl
    Friend WithEvents btnApplicationViewDocuments As System.Windows.Forms.Button
    Friend WithEvents btnApplicationLinkDocument As System.Windows.Forms.Button
    Friend WithEvents grpApplicationDenialReasons As System.Windows.Forms.GroupBox
    Friend WithEvents StudentBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents MailingAddressBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents PrimaryPhoneBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents SchoolEmailBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents PersonalEmailBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CellPhoneBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents PrimaryAwardBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ExemplaryAwardBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents UespSupplementalAwardBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents HighSchoolBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents cmbApplicationHighSchoolName As System.Windows.Forms.ComboBox
    Friend WithEvents CollegeBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents lblApplicationUespAwardReviewInProgress As System.Windows.Forms.Label
    Friend WithEvents txtDemographicsSsn As System.Windows.Forms.TextBox
    Friend WithEvents lblDemographicsSsn As System.Windows.Forms.Label
    Friend WithEvents MainMenuSearchResultBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CommunicationBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents lblHeaderRecordLocked As System.Windows.Forms.Label
    Friend WithEvents CommunicationDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents clmStudentID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dtpApplicationAwardStatusLetterSent As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationDefermentBeginDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationLeaveOfAbsenceEndDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationLeaveOfAbsenceBeginDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationDefermentEndDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationDefermentDecisionDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationDefermentRequestReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationLoaDecisionDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationLoaRequestReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationAppealDecisionDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationAppealReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationProofOfEnrollmentReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationConditionalAcceptanceFormReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationSignaturePageReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationFinalCollegeTranscriptReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationInitialCollegeTranscriptReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationFinalHighSchoolTranscriptReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationInitialHighSchoolTranscriptReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationTermBeginDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpApplicationGraduationDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents lblHeaderStudentId As System.Windows.Forms.Label
    Friend WithEvents txtHeaderStudentId As System.Windows.Forms.TextBox
    Friend WithEvents grpAuthThirdParties As System.Windows.Forms.GroupBox
    Friend WithEvents btnWebPasswordReset As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnQuickBatchReview As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDistrictCommunication As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnHighSchoolCommunications As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnJuniorHighSchoolCommuncation As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnMiscCommunication As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents FirstName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LastName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StateStudentID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SocialSecurityNumber As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StreetAddressLine1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents City As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AwardStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents grpHearAboutUs As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtHowDidTheyHearAboutRegents As System.Windows.Forms.TextBox
    Friend WithEvents ScholarshipApplicationBindingSource1 As System.Windows.Forms.BindingSource
    Friend WithEvents txtAppYear As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblApplicationSecondQuickReviewInProgress As System.Windows.Forms.Label
    Friend WithEvents lblApplicationFirstQuickReviewInProgress As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkApplicationAttendedAnotherSchool As System.Windows.Forms.CheckBox
    Friend WithEvents cmbApplication9thGradeSchoolName As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtApplicationPlannedCollegeToAttend As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents AuthorizedThirdParty2 As RegentsScholarshipFrontEnd.AuthorizedThirdPartyControl
    Friend WithEvents AuthorizedThirdParty1 As RegentsScholarshipFrontEnd.AuthorizedThirdPartyControl
    Friend WithEvents CommunicationPrinter As RegentsScholarshipFrontEnd.CommunicationRecordPrintingControl
    Friend WithEvents dtpHighSchoolScheduleReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtApplicationHighSchoolDistrict As System.Windows.Forms.TextBox
    Friend WithEvents txtApplicationHighSchoolCity As System.Windows.Forms.TextBox
    Friend WithEvents txtMainMenuSearchDateOfBirth As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtDemographicsDateOfBirth As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtpApplicationProofOfCitizenshipReceivedDate As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents lblApplicationProofOfCitizenshipReceivedDate As System.Windows.Forms.Label
    Friend WithEvents tabPayments As System.Windows.Forms.TabPage
    Friend WithEvents pnlPayments As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnPaymentsSaveChanges As System.Windows.Forms.Button
    Friend WithEvents btnPaymentsNewPayment As System.Windows.Forms.Button
    Friend WithEvents btnPaymentsViewDocuments As System.Windows.Forms.Button
    Friend WithEvents btnPaymentsLinkDocument As System.Windows.Forms.Button
    Friend WithEvents txtPaymentsCumulativeCreditHoursPaid As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tabPaymentBatch As System.Windows.Forms.TabPage
    Friend WithEvents btnPaymentBatchFinal As System.Windows.Forms.Button
    Friend WithEvents btnPaymentBatchPreliminary As System.Windows.Forms.Button
    Friend WithEvents lblPaymentBatchNumber As System.Windows.Forms.Label
    Friend WithEvents txtPaymentBatchLog As System.Windows.Forms.TextBox
    Friend WithEvents splPaymentBatchSplitter As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkCommunications411 As System.Windows.Forms.CheckBox
    Friend WithEvents UserID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmTimeStamp As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmSource As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmSubject As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmText As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Is411 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents grpDemographics411 As System.Windows.Forms.GroupBox
    Friend WithEvents pnlDemographics411 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents chkApplicationInitialTranscriptReviewStartStop As System.Windows.Forms.CheckBox
    Friend WithEvents chkApplicationFinalTranscriptReviewStartStop As System.Windows.Forms.CheckBox
    Friend WithEvents chkApplicationSecondTranscriptReviewStartStop As System.Windows.Forms.CheckBox
    Friend WithEvents lblApplicationAppealDecision As System.Windows.Forms.Label
    Friend WithEvents chkApplicationAppealDenied As System.Windows.Forms.CheckBox
    Friend WithEvents chkApplicationAppealApproved As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtDemographicsAddressLastUpdated As System.Windows.Forms.TextBox

End Class
