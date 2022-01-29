FILENAME EA27_IN "X:\Archive\BANA\TEST\EA27\SC*";

DATA _01BorrowerRecord(keep=BorrowerSSN loan_number PACKETNumber OriginalSSN StudentSSN SeparationDate NumberMonthsGrace GuarantorCode LoanStatusCode DriverLicenseNumber 
		DriverLicenseState DefermentFlag BorrowerBirthDate  ActualGradDate FeeGroupCode BorrowerCitizenshipCode BorrowerLastName BorrowerLastNameSuffix 
		BorrowerFirstName BorrowerMiddleName SuspenseCode SensitivityCode LiborDate DisputeCode)
	_02DefermentDataRecord(keep=BorrowerSSN DefermentBeginDate DefermentEndDate 
		DiscretionaryForbearanceMonths StudentDefermentMonths FellowshipDefermentMonths PublicHealthDefermentMonths 
		ArmedForcesDefermentMonths PeaceCorpsDefermentMonths VistaDefermentMonths InternshipDefermentMonths 
		TaxExemptDefermentMonths UnemploymentDefermentMonths DisabilityDefermentMonths RehabilitationDefermentMonths 
		DefermentType InterestCapitalizationDate CommitAmount InterestAmount78 EconomicHardshipDefermentMonths 
		MotherhoodDefermentMonths NOAADefermentMonths ParentalLeaveDefermentMonths TeacherDefermentMonths 
		FirstIBRDefermentBeginDate FirstIBRDefermentEndDate SecondIBRDefermentBeginDate SecondIBRDefermentEndDate 
		ThirdIBRDefermentBeginDate ThirdIBRDefermentEndDate FourthIBRDefermentBeginDate FourthIBRDefermentEndDate 
		FifthIBRDefermentBeginDate FifthIBRDefermentEndDate MilitaryGraceBeginDate MilitaryGraceEndDate PostDeferementGraceEnd
		DependentInSchoolDefermentMonths ExtendedForbearanceMonths MilitaryDefermentMonths ExtendedStudentDefermentMonths
		AdministrativeForbearanceMonths MandatoryForbearanceMonths MandatoryAdminForbearanceMonths 
		PLUSInSchoolDefermentIndicator InternForbearanceMonths PLUSInSchoolDefermentMonths TLFForbearanceMonths 
		PostActiveStudentDefermentMonths ExcessiveDebtForbearanceMonths CNCSForbearanceMonths MilitaryForbearanceMonths 
		LowPayForbearancePaymentAmount GradPlusPostEnrollDeferMonths)
	_03PaymentDataRecord(keep=BorrowerSSN loan_number PriorServicerFirstUnpaidInstall LastBorrowerPaymentDate 
		MonthlyPaymentAmount RepaymentPlanCode DefFbrCertificationDate SeparationCertificationDate RemainingLoanTerm 
		NextPaymentDueDate FirstDueDateCurrentRPS Term1CurrentRPS Amount1CurrentRPS Term2CurrentRPS Amount2CurrentRPS 
		Term3CurrentRPS Amount3CurrentRPS Term4CurrentRPS Amount4CurrentRPS Term5CurrentRPS Amount5CurrentRPS 
		Term6CurrentRPS Amount6CurrentRPS EnrollmentEffectiveDate SCRAServiceBeginDate SCRAServiceEndDate Term7CurrentRPS Amount7CurrentRPS 
		Term8CurrentRPS Amount8CurrentRPS Term9CurrentRPS Amount9CurrentRPS Term10CurrentRPS Amount10CurrentRPS TermMonthUsed
		ACSDaysDelinquent PrincipalBalanceRPSBegin RepaymentPlanStartDate ICRInterestOnly PreviousRepaymentPlan IBRInterestSubsidyBeginDate 
		IBRInterestSubsidyEndDate IBRInterestSubsidyAmount IBRInterestSubsidyMonths IBRForgivenessMonthCounter 
		ICRForgivenessMonthCounter InterestRateRepaymentBegin)
	_04InterestLateChargeRecord(keep=BorrowerSSN loan_number CurrentDue PartialDueAmount MaximumInterestRule78 
		PostDefermentGraceMonths NumberForbearances NumberPaymentsMade LateChargesAccrued TotalLateChargesPaid DateLastLateFeeAssessed SpecialtyClaimType 
		BankruptcyType SpecialtyStatusNotificationDate SpecialtyClaimProcessingCode VADeterminationEffectiveDate 
		DischargeDocsReceived DatePhysicianRequestExtension PhysicianCertificationDate BankruptcyCourtDate 
		PublicServiceForgivenessDate PublicServiceForgivePayCounter TeacherLoanForgivenessAmount 
		TeacherLoanForgivenessDate ForgivenessPrograms TeacherLoanForgivenessType StateTeacherApplication 
		PreviouslyAppliedForgiveness)
	_05SupplementalBorrowerRecord(KEEP=BorrowerSSN loan_number HomePhoneNumber AlternatePhoneNumber	HomePhoneCode 
		AlternatePhoneCode AddressEffectiveDate AddressStatusDate AddressConditionCode LoanTypeCode BillingTypeCode ClaimCode EnrollmentCode
		SchoolCode SchoolCampusCode IBREligibilityIndicator PartialFinancialHardshipAmount PermanentStandardPayAmount 
		IBRForgiveStartDate StandardPaymentAmount NumQualifyingPayments IBRCreateDate OriginalSchoolCode RehabilitationIndicator 
		RehabilitationDate RehabilitationAmount AddressType CorrespondenceType AcademicYearBeginDate AcademicYearEndDate 
		FinancialAwardYear CRC HPPAIndicator DependencyStatusCode AdditionalUnsubEligIndicator DebtID OriginalRepaymentDate 
		CellPhoneNumber CellPhoneCode WorkPhoneNumber WorkPhoneCode RecallIndicator IBRSubsidyBeginDate INRSubsidyEndDate
		PFHBeginDate PFHEndDate IBRHardshipDefermentMonths FuturePFHBeginDate FuturePFHEndDate FuturePFHAmount OldestDisbursementDate)
	_06BorrowerAddressRecord(keep=BorrowerSSN BorrowerStreetAddress BorrowerCareOfAddress 
		BorrowerCity BorrowerState BorrowerZip BorrowerZip4 ForeignAddressCode BorrowerEmailAddress BorrowerCountryCode)
	_07_08DisbClaimEnrollRecord(keep=BorrowerSSN DisbursementNumber DEAL_ID loan_number ClaimFiledDate ClaimpaidDate ClaimRejectDate NoteDate DisbursementDate NoteAmount
		DisbursementAmount BorrowerPaidPrincipalAmount BorrowerPaidInterestAmount PrincipalBalanceOutstanding BorrowerAccruedInterest 
		InterestReceivableGovernment BenefitCode BondIdentifierCode PromotionalCode ApplicationNumber DisbursementSegment CheckNumber DisbursementType 
		GradeLevel LoanGuaranteeDate LenderLastResort ClientProductId MPNConfirmationCode FederalApplicationCode SerialLoanCode SchoolLoanCertificationDate 
		CommonLineUniqueID LoanOriginationIndicatorCode FullyDisbursedIndicator BorrIncentivePlanCode ApplicationReceivedDate LoanOriginationCode ESignatureAuthenticationTypeCode BlanketApprovalDate 
		BorrowerESignatureIndicatorCode PLUSAltStudentEsignatureIndCode BorowerESignatureSource GuaranteeType OriginalLenderID 
		PreDisbCancellationAmount PreDisbursementCancellationDate ApprovedLateIndicator LastAnticipatedDate PLUSDisbursementRemaining RefundDate RefundAmount 
		CapitalizedInterestAmount PrincipalPaidByClaim InterestPaidByClaim LoanIdNumber InterestRate InterestCode LoanPeriodStartDate LoanPeriodEndDate LoanOriginationFeeBorPaid 
		InsurancePremiumBorPaid CreditScoreCode CertificateId LoanIdentification UnderlyingLoantype ConsolidationIncrease PayeeCode PayeeName LoanOriginationFeeClientPaid UnreportedCapitalizedInterestOID 
		UnreportedLoanOriginationFeeOID GuarantyFeeGuarantorPaid GuarantorFeeLenderPaid CancellationDate CancellationAmount VariableRateType)
	_09SummarizedDisbursementRecord(keep=BorrowerSSN SumPrincipalBalance SumCapitalizedInterest SumPrincipalPaid 
		SumInterestPaid SumPrincipalPaidbyClaim SumInterestReceivableBorr SumInterestPaidByClaim SumLoanOriginationFeesBorrPaid 
		SumInsurancePremiums SumLoanOriginationFeesClientPaid SumUnreportedCapInterestOID SumUnreportedOriginationFeeOID 
		SumGuarantyFeeGuarantorPaid SumGuarantyFeeLenderPaid CurrentPenalties CurrentTreasuryOffsetFee CurrentAdministrationFee 
		CurrentMiscellaneousFee CurrentCollectionFee CollectionFeeCap DOJFee)
	_10_11ReferenceData(keep=BorrowerSSN RefNumber RefType RefCOLine RefSSN RefDOB RefLastName RefLastNameSuffix RefFirstName 
		RefMiddleName CoBorrowerSuspenseCode RefCODSSN CODDOB RefForeignAddressCode RefStreetAddress RefCity RefState RefZip RefZip4 RefResidencePhoneNum 
		RefAlternatePhoneNUm RefAddressType RefStreetAddress2 RefCountryCode RefEmailAddress RefCitizenshipCode)
	_12DefermentHistory(KEEP=BorrowerSSN loan_number DefermentBeginDate DefermentEndDate DefermentType)
	_13SupplementalPaymentData(keep=BorrowerSSN Term7CurrentRPS Amount7CurrentRPS Term8CurrentRPS 
		Amount8CurrentRPS Term9CurrentRPS Amount9CurrentRPS Term10CurrentRPS Amount10CurrentRPS Term11CurrentRPS 
		Amount11CurrentRPS Term12CurrentRPS Amount12CurrentRPS Term13CurrentRPS Amount13CurrentRPS Term14CurrentRPS 
		Amount14CurrentRPS Term15CurrentRPS Amount15CurrentRPS Term16CurrentRPS Amount16CurrentRPS ClaimingLenderID 
		DefaultDate ClaimPaidToLenderDate FirstTreasuryOffsetCertDate JudgmentDate JudgmentExpirationDate 
		VariableInterestAddOnRate InterestRateCap AmountCollectedbyGA InterestPaidtoLenderbyGA AccountOwner)
	_14SupplementalRepaymentData(keep=BorrowerSSN JointRepaymentIndicator MaritalStatus FilingStatus 
		FamilySize IRSConsentDate IRSConsentExpirationDate IRSConsentRevokeDate BorrowerAGIIncome LastAGIIncomeDate 
		SpouseSSN SpouseLastName SpouseLastNameSuffix SpouseFirstName SpouseMiddleName SpouseDOB SpouseIRSConsentDate
		SpouseIRSConsentExpirationDate SpouseIRSConsentRevokeDate SpouseAGIIncome SpouseLastAGIIncomeDate 
		SpousePayAmtatRepayment SpousePayAmtatIBRBeginDate ForcedICRIndicator ICROnTimePaymentsMade NegAmortizationBeginDate 
		NegAmortizationPayAmount ICRNegAmortizationLength CapInterestDateinICRPlan TenPercentThresholdIndicator
		CumNegAmortizationIntCapitalized NegAmortizationIRB InterestAmtfromICR06_30CAPLetter BorrowerIncomeSource 
		SpouseIncomeSource )
	_15BenefitRecord(KEEP=BorrowerSSN BenefitType BenefitPercentage RebateAmount BenefitStatus 
		PaymentsToward12PayCommitment LatePayToward12PayCommit MilitaryServiceStartDate MilitaryServiceEndDate 
		MonUseTowardNoInt60MonthAccrual RebateRevokeDate BenefitStatusEffDate)
	_16MPNRecord(KEEP=BorrowerSSN MPNStatusCode MPNExpirationDate MPNID)
	_17ConsolidationRecord(KEEP=BorrowerSSN ConsolidatedApplicationID PlusLoanIndicator 
		LoanPriorto2008Oct01Indicator)
	_18DirectDebit(keep=BorrowerSSN ACHAwardID ACHAccountFirstName ACHAccountLastName
		ABARoutingNum BankAccountNum BankAccountType LastExtractionAmount LastExtractionDate InstallmentAmount 
		AdditionalAmountExtracted NextScheduleExtractionDate ACHApplicationSource ACHBeginDate ACHEndDate 
		ACHAccountStreet1 ACHAccountStreet2 ACHAccountCity ACHAccountState ACHAccountZip ACHAccountZip4 )
	_19TeachGrant(keep=BorrowerSSN TeachAwardID AgreementtoServeID AgreementtoServeDate
		AgreementtoServeStatusCode InstitutionCampusReportingGrant GrantCPSOriginalSSN GrantCPSOriginalNameCode 
		GrantCurrentAwardType GrantGradeLevel GrantMaxDisbursementNum GrantCurAwardSeqNum GrantCPSTransactionNum 
		GrantTotAcceptedAwardAmountPaid GrantAwardAmount GrantEnrollmentDate GrantCreateDate GrantDateLastUpdate 
		GrantDisbursementPostedCODDate GrantAwardYear GrantExpertTeacherFlag GrantStatus GrantStatusDate 
		GrantLifeCircumstanceType GrantLifeCircumstanceBeginDate GrantLifeCircumstanceEndDate GrantYearsCompleted 
		GrantTeachSatRequirementsDate )
	_20TeachGrant(keep=BorrowerSSN AcademicYear SchoolName SchoolCounty SchoolState SubjectTaught
		CurrentCertDueDate LastCertDueDate TypeNoticeSent DateNoticeSent TeachObligationBeginDate TeachObligationEndDate 
		LifeCircumstanceMonths TeachObligationYear1Date TeachObligationYear2Date TeachObligationYear3Date 
		TeachObligationYear4Date LoanConvertedtoGrantDate GrantConvertedtoLoanDate )
	_22Perkins(keep=BorrowerSSN LastAdvanceDate LastGracePeriodDate LoanAcceleratedIndicator 
		LoanLitigatedIndicator AssignmentReason PrincipalAmountAdjusted PrincipalAmountCancelled InterestAmountCancelled 
		CancellationType CancellationServiceStartDate CancellationServiceEndDate CollectionCostRepaid )
;

	INFILE EA27_IN DSD DLM = '10'x  MISSOVER END = EOF LRECL=32767;
	RETAIN loan_number 0;
	INPUT RECORD_TYP $ 1-2 @;
	IF RECORD_TYP = 'D1' THEN do;
		INPUT DEAL_ID $ 3-4;
	END;
		RETAIN DEAL_ID;
	IF RECORD_TYP = '01' THEN do;
		loan_number = (loan_number + 1);
		INPUT BorrowerSSN $ 3-11 
			PACKETNumber $12
			OriginalSSN $13-21
			StudentSSN $ 38-46
			@48 SeparationDate mmddyy6.
			NumberMonthsGrace 60-61
			GuarantorCode $ 69-70
			LoanStatusCode $ 71-72
			DriverLicenseNumber $ 83-102
			DriverLicenseState $ 103-104
			DefermentFlag $ 105
			@112 BorrowerBirthDate mmddyy6.
			@118 ActualGradDate mmddyy6.
			FeeGroupCode $ 124-126
			BorrowerCitizenshipCode $ 127
			BorrowerLastName $ 128-162
			BorrowerLastNameSuffix $ 163-166
			BorrowerFirstName $ 167-178
			BorrowerMiddleName $ 179-190
			SuspenseCode $191
			Sensitivitycode $192
			@249 LiborDate mmddyy6.
			DisputeCode $255-256;
		format SeparationDate BorrowerBirthDate LiborDate ActualGradDate mmddyy10.;
		output _01BorrowerRecord;
	END;
	RETAIN BorrowerSSN ;
	IF RECORD_TYP = '02' THEN do;
		INPUT @3 DefermentBeginDate mmddyy6.
			@9 DefermentEndDate mmddyy6.
			DiscretionaryForbearanceMonths 15-16
			StudentDefermentMonths 17-18
			FellowshipDefermentMonths 19-20
			PublicHealthDefermentMonths 21-22
			ArmedForcesDefermentMonths 23-24
			PeaceCorpsDefermentMonths 25-26
			VistaDefermentMonths 27-28
			InternshipDefermentMonths 29-30
			TaxExemptDefermentMonths 31-32
			UnemploymentDefermentMonths 33-34
			DisabilityDefermentMonths 35-36
			RehabilitationDefermentMonths 37-38
			DefermentType $ 39
			@40 InterestCapitalizationDate mmddyy6.
			@58 CommitAmount 7.2
			@65 InterestAmount78 7.2
			EconomicHardshipDefermentMonths 72-73
			MotherhoodDefermentMonths 74-75
			NOAADefermentMonths 76-77
			ParentalLeaveDefermentMonths 78-79
			TeacherDefermentMonths 80-81
			@82 FirstIBRDefermentBeginDate mmddyy6.
			@88 FirstIBRDefermentEndDate mmddyy6.
			@94 SecondIBRDefermentBeginDate mmddyy6.
			@100 SecondIBRDefermentEndDate mmddyy6.
			@106 ThirdIBRDefermentBeginDate mmddyy6.
			@112 ThirdIBRDefermentEndDate mmddyy6.
			@118 FourthIBRDefermentBeginDate mmddyy6.
			@124 FourthIBRDefermentEndDate mmddyy6.
			@130 FifthIBRDefermentBeginDate mmddyy6.
			@136 FifthIBRDefermentEndDate mmddyy6.
			@142 MilitaryGraceBeginDate mmddyy6.
			@148 MilitaryGraceEndDate mmddyy6.
			AdministrativeForbearanceMonths 154-155
			MandatoryForbearanceMonths 156-157
			MandatoryAdminForbearanceMonths 158-159
			PLUSInSchoolDefermentIndicator $ 160
			InternForbearanceMonths 163-164
			PLUSInSchoolDefermentMonths 165-166
			TLFForbearanceMonths 167-168
			PostActiveStudentDefermentMonths 169-170
			ExcessiveDebtForbearanceMonths 173-174
			CNCSForbearanceMonths 175-176
			MilitaryForbearanceMonths 177-178
			@179 LowPayForbearancePaymentAmount 7.2
			GradPlusPostEnrollDeferMonths 186-187
			@239 PostDefermentGraceEndDate mmddyy6.
			DependentInSchoolDefermentMonths 245-247
			ExtendedFrobearanceMonths 248-250
			MilitaryDefermentMonths 251-253
			ExtendedStudentDefermentMonths 254-256;
		format DefermentBeginDate DefermentEndDate InterestCapitalizationDate FirstIBRDefermentBeginDate FirstIBRDefermentEndDate
			SecondIBRDefermentBeginDate SecondIBRDefermentEndDate ThirdIBRDefermentBeginDate ThirdIBRDefermentEndDate 
			FourthIBRDefermentBeginDate FourthIBRDefermentEndDate FifthIBRDefermentBeginDate FifthIBRDefermentEndDate 
			MilitaryGraceBeginDate MilitaryGraceEndDate PostDefermentGraceEndDate mmddyy10.;
		format CommitAmount InterestAmount78 7.2;
		output _02DefermentDataRecord;
	END;
	IF RECORD_TYP = '03' THEN do;
		INPUT @3 PriorServicerFirstUnpaidInstall mmddyy6.
			@9 LastBorrowerPaymentDate mmddyy6.
			@53 MonthlyPaymentAmount 7.2
			RepaymentPlanCode $ 60-61
			@74 DefFbrCertificationDate mmddyy6.
			@80 SeparationCertificationDate mmddyy6.
			RemainingLoanTerm 86-88
			@89 NextPaymentDueDate mmddyy6.
			@95 FirstDueDateCurrentRPS mmddyy6.
			Term1CurrentRPS 101-103
			@104 Amount1CurrentRPS 7.2
			Term2CurrentRPS 111-113
			@114 Amount2CurrentRPS 7.2
			Term3CurrentRPS 121-123
			@124 Amount3CurrentRPS 7.2
			Term4CurrentRPS 131-133
			@134 Amount4CurrentRPS 7.2
			Term5CurrentRPS 141-143
			@144 Amount5CurrentRPS 7.2
			Term6CurrentRPS 151-153
			@154 Amount6CurrentRPS 7.2
			@161 EnrollmentEffectiveDate mmddyy6.
			@167 SCRAServiceBeginDate mmddyy8.
			@175 SCRAServiceEndDate mmddyy8.
			Term7CurrentRPS 211-213
			@214 Amount7CurrentRPS 7.2
			Term8CurrentRPS 221-223
			@224 Amount8CurrentRPS 7.2
			Term9CurrentRPS 231-233
			@234 Amount9CurrentRPS 7.2
			Term10CurrentRPS 241-243
			@244 Amount10CurrentRPS 7.2
			TermMonthUsed 251-253
			ACSDaysDelinquent 254-256;
			
		format PriorServicerFirstUnpaidInstall LastBorrowerPaymentDate DefFbrCertificationDate 
			SeparationCertificationDate NextPaymentDueDate FirstDueDateCurrentRPS 
			EnrollmentEffectiveDate SCRAServiceBeginDate SCRAServiceEndDate RepaymentPlanStartDate  mmddyy10.;
		format MonthlyPaymentAmount Amount1CurrentRPS Amount2CurrentRPS Amount3CurrentRPS 
			Amount4CurrentRPS Amount5CurrentRPS Amount6CurrentRPS 7.2;
		output _03PaymentDataRecord;
	END;
	IF RECORD_TYP = '04' THEN do;
		INPUT @3 CurrentDue 7.2
			@10 PartialDueAmount 7.2
			@45 MaximumInterestRule78 7.2
			PostDefermentGraceMonths 52-53
			NumberForbearances 54-55
			NumberPaymentsMade 56-57
			@58 LateChargesAccrued 7.2
			@65 TotalLateChargesPaid 7.2
			SpecialtyClaimType $ 80-81
			BankruptcyType $ 82
			@83 SpecialtyStatusNotificationDate mmddyy8.
			SpecialtyClaimProcessingCode $ 91
			@92 VADeterminationEffectiveDate mmddyy8.
			@100 DischargeDocsReceived mmddyy8.
			@108 DatePhysicianRequestExtension mmddyy8.
			@116 PhysicianCertificationDate mmddyy8.
			@124 BankruptcyCourtDate mmddyy8.
			@132 PublicServiceForgivenessDate mmddyy8.
			PublicServiceForgivePayCounter 140-142
			@143 TeacherLoanForgivenessAmount 7.2
			@151 TeacherLoanForgivenessDate mmddyy8.
			ForgivenessPrograms $ 175-177
			TeacherLoanForgivenessType $ 178-179
			StateTeacherApplication $ 180-181
			PreviouslyAppliedForgiveness $ 182
			@251 DateLastLateFeeAssessed mmddyy6.
;
		format SpecialtyStatusNotificationDate VADeterminationEffectiveDate DischargeDocsReceived 
			DatePhysicianRequestExtension PhysicianCertificationDate BankruptcyCourtDate 
			PublicServiceForgivenessDate TeacherLoanForgivenessDate DateLastLateFeeAssessed	mmddyy10.;
		output _04InterestLateChargeRecord;
	END;	
	IF RECORD_TYP = '05' THEN do;
		INPUT HomePhoneNumber $ 13-22
			AlternatePhoneNumber $ 23-32
			HomePhoneCode $ 33
			AlternatePhoneCode $ 34
			@35 AddressEffectiveDate mmddyy6.
			@41 AddressStatusDate mmddyy6.
			AddressConditionCode $ 47
			LoanTypeCode $ 49
			BillingTypeCode $50
			EnrollmentCode $ 52
			ClaimCode $54
			SchoolCode $ 56-61
			SchoolCampusCode $ 83-84
			IBREligibilityIndicator $ 85
			@86 PartialFinancialHardshipAmount 12.2
			@98 PermanentStandardPayAmount 12.2
			@110 IBRForgiveStartDate mmddyy6.
			@116 StandardPaymentAmount 12.2
			NumQualifyingPayments 128-130
			@131 IBRCreateDate mmddyy6.
			OriginalSchoolCode $ 137-144
			RehabilitationIndicator $ 145
			@146 RehabilitationDate mmddyy8.
			@154 RehabilitationAmount 13.2
			AddressType $ 165
			CorrespondenceType $ 166
			@167 AcademicYearBeginDate mmddyy8.
			@175 AcademicYearEndDate mmddyy8.
			FinancialAwardYear 183-186
			CRC $ 187-192
			HPPAIndicator $ 193
			DependencyStatusCode $ 194
			AdditionalUnsubEligIndicator $ 195
			DebtID $ 196-211
			@200 IBRSubsidyBeginDate mmddyy6.
			@206 IBRSubsidyEndDate mmddyy6.
			@212 PFHBeginDate mmddyy6.
			@218 PFHEndDate mmddyy6.
			IBRHardshipDefermentMonths 224-226
			@227 FuturePFHBeginDate mmddyy6.
			@223 FuturePFHEndDate mmddyy6.
			@239 FuturePFHAmount 7.2
			@251 BorrowersOldestDisbursement mmddyy6.;
		format AddressEffectiveDate AddressStatusDate IBRForgiveStartDate IBRCreateDate RehabilitationDate 
			AcademicYearBeginDate AcademicYearEndDate OriginalRepaymentDate IBRSubsidyBeginDate IBRSubsidyEndDate
			PFHBeginDate PFHEndDate FuturePFHBeginDate FuturePFHEndDate BorrowersOldestDisbursement mmddyy10.;
		output _05SupplementalBorrowerRecord;
	END;
	IF RECORD_TYP = '06' THEN do;
		INPUT BorrowerStreetAddress $ 3-27
			BorrowerCareOfAddress $ 28-52
			BorrowerCity $ 53-68
			BorrowerState $ 69-70
			BorrowerZip $ 71-75
			BorrowerZip4 $ 76-79
			ForeignAddressCode $ 80
			BorrowerEmailAddress $ 81-130
			BorrowerCountryCode $ 131-132;
		output _06BorrowerAddressRecord;
	END;
	IF RECORD_TYP = '07' THEN do;
		INPUT DisbursementNumber 3-4
			@5 NoteDate mmddyy6.
			@11 DisbursementDate mmddyy6.
			@17 NoteAmount 8.2
			@25 DisbursementAmount 8.2
			@33 BorrowerPaidPrincipalAmount 8.2
			@41 BorrowerPaidInterestAmount 8.2
			@48 PrincipalBalanceOutstanding 8.2
			@56 BorrowerAccruedInterest 8.2
			@63 InterestReceivableGovernment 8.2
			BenefitCode $ 70
			BondIdentifierCode $ 71-72
			PromotionalCode $82-85
			ApplicationNumber $ 86-96
			DisbursementSegment $ 97-98
			CheckNumber $ 99-108
			DisbursementType $ 109
			GradeLevel $ 116-117
			@119 LoanGuaranteeDate mmddyy6.
			LenderLastResort $ 125ClientProductId$126-129
			MPNConfirmationCode $ 130
			FederalApplicationCode $ 131
			SerialLoanCode $ 132
			@133 SchoolLoanCertificationDate mmddyy6.
			CommonLineUniqueID $ 139-157
			LoanOriginationIndicatorCode 158-161
			FullyDisbursedIndicator $ 162
			BorrIncentivePlanCode 163-166
			@168 ApplicationReceivedDate mmddyy6.
			LoanOriginationCode 182-185
			ESignatureAuthenticationTypeCode $ 186-187
			@188 BlanketApprovalDate mmddyy6.
			BorrowerESignatureIndicatorCode $ 194
			PLUSAltStudentEsignatureIndCode $ 195
			BorowerESignatureSource $ 196-204
			GuaranteeType $ 205
			OriginalLenderID $ 206-211
			@212 PreDisbCancellationAmount 8.2
			@220 PreDisbursementCancellationDate mmddyy8.
			ApprovedLateIndicator $ 228
			@229 LastAnticipatedDate mmddyy8.
			PLUSDisbursementRemaining 237-239;
		INPUT
			@05 ClaimFiledDate mmddyy6.
			@11 ClaimPaidDate mmddyy6.
			@17 ClaimRejectDate mmddyy6.
			@23 RefundDate mmddyy6.
			@29 RefundAmount 8.2
			@37 CapitalizedInterestAmount 8.2
			@44 PrincipalPaidByClaim 7.2
			@52 InterestPaidByClaim 6.2
			LoanIdNumber 59-66
			@67 InterestRate 5.3
			InterestCode $ 72-74
			@75 LoanPeriodStartDate mmddyy6.
			@81 LoanPeriodEndDate mmddyy6.
			@87 LoanOriginationFeeBorPaid 7.2
			@94 InsurancePremiumBorPaid 7.2
			CreditScoreCode $ 101
			CertificateId $105-118
			LoanIdentification $ 119-139
			UnderlyingLoanType $141-142
			ConsolidationIncrease $143
			PayeeCode 144-149
			PayeeName $150-199
			@200 LoanOriginationFeeClientPaid 7.2
			@207 UnreportedCapitalizedInterestOID 7.2
			@214 UnreportedLoanOriginationFeeOID 7.2
			@221 GuarantyFeeGuarantorPaid 7.2
			@228 GuarantorFeeLenderPaid 7.2
			@235 CancellationDate mmddyy6.
			@241 CancellationAmount 8.2
			VariableRateType $ 249;
		format NoteDate DisbursementDate LoanGuaranteeDate SchoolLoanCertificationDate ApplicationReceivedDate BlanketApprovalDate 
			PreDisbursementCancellationDate LastAnticipatedDate RefundDate LoanPeriodStartDate LoanPeriodEndDate CancellationDate
			ClaimFiledDate ClaimFiledDate ClaimPaidDate RefundDate	mmddyy10.;
		output _07_08DisbClaimEnrollRecord;
	END;
	IF RECORD_TYP = '09' THEN do;
		INPUT @3 SumPrincipalBalance 7.2
			@11 SumCapitalizedInterest 7.2
			@18 SumPrincipalPaid 8.2
			@26 SumInterestPaid 7.2
			@33 SumPrincipalPaidbyClaim 8.2
			@41 SumInterestReceivableBorr 7.2
			@48 SumInterestPaidByClaim 7.2
			@55 SumLoanOriginationFeesBorrPaid 7.2
			@62 SumInsurancePremiums 7.2
			@69 SumLoanOriginationFeesClientPaid 7.2
			@76 SumUnreportedCapInterestOID 7.2
			@83 SumUnreportedOriginationFeeOID 7.2
			@90 SumGuarantyFeeGuarantorPaid 7.2
			@97 SumGuarantyFeeLenderPaid 7.2
			CurrentPenalties 104-117
			CurrentTreasuryOffsetFee 118-131
			CurrentAdministrationFee 132-145
			CurrentMiscellaneousFee 146-159
			CurrentCollectionFee 160-173
			@174 CollectionFeeCap 8.6
			DOJFee 182-195;
		output _09SummarizedDisbursementRecord;
	END;
	IF RECORD_TYP = '10' THEN do;
		INPUT RefNumber 3-4
			RefType $ 5
			RefCOLine $ 34-61
			RefSSN $ 62-70
			@71 RefDOB mmddyy6.
			RefLastName $ 77-111
			RefLastNameSuffix $ 112-115
			RefFirstName $ 116-127
			RefMiddleName $ 128-139
			RefCODSSN $ 140-148
			@149 CODDOB mmddyy8.;
		INPUT RefForeignAddressCode $ 5
			RefStreetAddress $ 6-33
			RefCity $ 34-49
			RefState $ 50-51
			RefZip $ 52-56
			RefZip4 $ 57-60
			RefResidencePhoneNum $ 61-70
			RefAlternatePhoneNUm $ 71-80
			RefAddressType $ 81
			RefStreetAddress2 $ 82-109
			RefCountryCode $ 110-111
			RefEmailAddress $ 112-171
			RefCitizenshipCode $ 172;
		FORMAT RefDOB CODDOB mmddyy10.; 
		output _10_11ReferenceData;
	END;
	IF RECORD_TYP = '12' THEN do;
		INPUT @3 DefermentBeginDate mmddyy6.
			@9 DefermentEndDate mmddyy6.
			DefermentType $ 15;
		FORMAT DefermentBeginDate DefermentEndDate mmddyy10.; 
		output _12DefermentHistory;
	END;
	IF RECORD_TYP = '13' THEN do;
		INPUT Term7CurrentRPS 13-15
			@16 Amount7CurrentRPS 7.2
			Term8CurrentRPS 23-25
			@26 Amount8CurrentRPS 7.2
			Term9CurrentRPS 33-35
			@36 Amount9CurrentRPS 7.2
			Term10CurrentRPS 43-45
			@46 Amount10CurrentRPS 7.2
			Term11CurrentRPS 53-55
			@56 Amount11CurrentRPS 7.2
			Term12CurrentRPS 63-65
			@66 Amount12CurrentRPS 7.2
			Term13CurrentRPS 73-75
			@76 Amount13CurrentRPS 7.2
			Term14CurrentRPS 83-85
			@86 Amount14CurrentRPS 7.2
			Term15CurrentRPS 93-95
			@96 Amount15CurrentRPS 7.2
			Term16CurrentRPS 103-105
			@106 Amount16CurrentRPS 7.2
			ClaimingLenderID $ 115-120
			@121 DefaultDate yymmdd8.
			@129 ClaimPaidToLenderDate yymmdd8.
			@137 FirstTreasuryOffsetCertDate yymmdd8.
			@145 JudgmentDate yymmdd8.
			@153 JudgmentExpirationDate yymmdd8.
			VariableInterestAddOnRate 161-168
			@169 InterestRateCap 8.4
			@177 AmountCollectedbyGA 8.2
			@185 InterestPaidtoLenderbyGA 8.2
			AccountOwner $ 193-200;
		FORMAT DefaultDate ClaimPaidToLenderDate FirstTreasuryOffsetCertDate JudgmentDate JudgmentExpirationDate mmddyy10.; 
		output _13SupplementalPaymentData;
	END;
	IF RECORD_TYP = '14' THEN do;
		INPUT JointRepaymentIndicator $ 3
			MaritalStatus $ 4
			FilingStatus $ 5
			FamilySize 6-7
			@8 IRSConsentDate mmddyy8.
			@16 IRSConsentExpirationDate mmddyy8.
			@24 IRSConsentRevokeDate mmddyy8.
			@32 BorrowerAGIIncome 9.2
			@41 LastAGIIncomeDate mmddyy8.
			SpouseSSN $ 49-57
			SpouseLastName $ 58-92
			SpouseLastNameSuffix $ 93-96
			SpouseFirstName $ 97-108
			SpouseMiddleName $ 109-120
			@121 SpouseDOB mmddyy8.
			@129 SpouseIRSConsentDate mmddyy8.
			@137 SpouseIRSConsentExpirationDate mmddyy8.
			@145 SpouseIRSConsentRevokeDate mmddyy8.
			@153 SpouseAGIIncome 9.2
			@162 SpouseLastAGIIncomeDate mmddyy8.
			@170 SpousePayAmtatRepayment 12.2
			@182 SpousePayAmtatIBRBeginDate 12.2
			ForcedICRIndicator $ 194
			ICROnTimePaymentsMade 195-197
			@198 NegAmortizationBeginDate mmddyy8.
			@206 NegAmortizationPayAmount 8.2
			ICRNegAmortizationLength 214-215
			@216 CapInterestDateinICRPlan mmddyy8.
			TenPercentThresholdIndicator $ 224
			@225 CumNegAmortizationIntCapitalized 9.2
			@234 NegAmortizationIRB 8.2
			@242 InterestAmtfromICR06_30CAPLetter 9.2
			BorrowerIncomeSource $ 251-253
			SpouseIncomeSource $ 254-256;
		FORMAT IRSConsentDate IRSConsentExpirationDate IRSConsentRevokeDate LastAGIIncomeDate SpouseDOB
			SpouseIRSConsentDate SpouseIRSConsentExpirationDate SpouseIRSConsentRevokeDate 
			SpouseLastAGIIncomeDate NegAmortizationBeginDate CapInterestDateinICRPlan mmddyy10.; 
		output _14SupplementalRepaymentData;
	END;
	IF RECORD_TYP = '15' THEN do;
		INPUT BenefitType $ 3-5
			@6 BenefitPercentage 6.4
			@12 RebateAmount 9.2
			BenefitStatus $21
			PaymentsToward12PayCommitment 22-23
			LatePayToward12PayCommit 24-25
			@26 MilitaryServiceStartDate mmddyy8.
			@34 MilitaryServiceEndDate mmddyy8.
			MonUseTowardNoInt60MonthAccrual 42-43
			@44 RebateRevokeDate mmddyy8.
			@52 BenefitStatusEffDate mmddyy8.;
		FORMAT  MilitaryServiceStartDate MilitaryServiceEndDate RebateRevokeDate BenefitStatusEffDate mmddyy10.; 
		output _15BenefitRecord;
	END;
	IF RECORD_TYP = '16' THEN do;
		INPUT MPNStatusCode $ 3
			@4 MPNExpirationDate mmddyy8.
			MPNID $ 12-32;
		FORMAT  MPNExpirationDate mmddyy10.; 
		output _16MPNRecord;
	END;
	IF RECORD_TYP = '17' THEN do;
		INPUT ConsolidatedApplicationID $ 3-23
			PlusLoanIndicator $ 24
			LoanPriorto2008Oct01Indicator $ 25;
		output _17ConsolidationRecord;
	END;
	IF RECORD_TYP = '18' THEN do;
		INPUT ACHAwardID $ 3-23
			ACHAccountFirstName $ 24-38
			ACHAccountLastName $ 39-73
			ABARoutingNum $ 74-82
			BankAccountNum $ 83-99
			BankAccountType $ 100
			@101 LastExtractionAmount 7.2
			@108 LastExtractionDate mmddyy8.
			@116 InstallmentAmount 7.2
			@123 AdditionalAmountExtracted 7.2
			@130 NextScheduleExtractionDate mmddyy8.
			ACHApplicationSource $ 138
			@139 ACHBeginDate mmddyy8.
			@147 ACHEndDate mmddyy8.
			ACHAccountStreet1 $ 155-179
			ACHAccountStreet2 $ 180-204
			ACHAccountCity $ 205-220
			ACHAccountState $ 221-222
			ACHAccountZip $ 223-227
			ACHAccountZip4 $ 228-231;
		FORMAT LastExtractionDate NextScheduleExtractionDate ACHBeginDate ACHEndDate mmddyy10.; 
		output _18DirectDebit;
	END;
	IF RECORD_TYP = '19' THEN do;
		INPUT TeachAwardID $ 3-23
			AgreementtoServeID $ 24-44
			@45 AgreementtoServeDate mmddyy8.
			AgreementtoServeStatusCode $ 53
			InstitutionCampusReportingGrant $ 54-61
			GrantCPSOriginalSSN $ 62-70
			GrantCPSOriginalNameCode $ 71-72
			GrantCurrentAwardType $ 73-74
			GrantGradeLevel $ 75
			GrantMaxDisbursementNum $ 76-77
			GrantCurAwardSeqNum $ 78-80
			GrantCPSTransactionNum $ 81-82
			@83 GrantTotAcceptedAwardAmountPaid 7.2
			@90 GrantAwardAmount 7.2
			@97 GrantEnrollmentDate mmddyy8.
			@105 GrantCreateDate mmddyy8.
			@113 GrantDateLastUpdate mmddyy8.
			@121 GrantDisbursementPostedCODDate mmddyy8.
			GrantAwardYear $ 129-132
			GrantExpertTeacherFlag $ 133
			GrantStatus $ 134-135
			@136 GrantStatusDate mmddyy8.
			GrantLifeCircumstanceType $ 144-145
			@146 GrantLifeCircumstanceBeginDate mmddyy8.
			@154 GrantLifeCircumstanceEndDate mmddyy8.
			GrantYearsCompleted 162-163
			@164 GrantTeachSatRequirementsDate mmddyy8.;
		FORMAT AgreementtoServeDate GrantEnrollmentDate GrantCreateDate GrantDateLastUpdate GrantDisbursementPostedCODDate 
			GrantStatusDate GrantLifeCircumstanceBeginDate GrantLifeCircumstanceEndDate GrantTeachSatRequirementsDate mmddyy10.; 
		output _19TeachGrant;
	END;	
	IF RECORD_TYP = '20' THEN do;
		INPUT AcademicYear $ 3-10
			SchoolName $ 11-40  
			SchoolCounty $ 41-55
			SchoolState $ 56-57
			SubjectTaught $ 58-107
			@108 CurrentCertDueDate mmddyy8.
			@116 LastCertDueDate mmddyy8.
			TypeNoticeSent $ 124-127
			@128 DateNoticeSent mmddyy8.
			@136 TeachObligationBeginDate mmddyy8.
			@144 TeachObligationEndDate mmddyy8.
			LifeCircumstanceMonths 152-154
			@155 TeachObligationYear1Date mmddyy8.
			@163 TeachObligationYear2Date mmddyy8.
			@171 TeachObligationYear3Date mmddyy8.
			@179 TeachObligationYear4Date mmddyy8.
			@187 LoanConvertedtoGrantDate mmddyy8.
			@195 GrantConvertedtoLoanDate mmddyy8.;
		FORMAT CurrentCertDueDate LastCertDueDate DateNoticeSent TeachObligationBeginDate TeachObligationEndDate TeachObligationYear1Date 
			TeachObligationYear2Date TeachObligationYear3Date TeachObligationYear4Date LoanConvertedtoGrantDate GrantConvertedtoLoanDate
			mmddyy10.; 
		output _20TeachGrant;
	END;	
	IF RECORD_TYP = '22' THEN do;
		INPUT @3 LastAdvanceDate mmddyy8.
			@11 LastGracePeriodDate mmddyy8.
			LoanAcceleratedIndicator $ 19
			LoanLitigatedIndicator $ 20
			AssignmentReason $ 21-22
			@23 PrincipalAmountAdjusted 8.2
			@31 PrincipalAmountCancelled 8.2
			@39 InterestAmountCancelled 8.2
			CancellationType $ 47-48
			@49 CancellationServiceStartDate mmddyy8.
			@57 CancellationServiceEndDate mmddyy8.
			@65 CollectionCostRepaid 8.2;
		FORMAT LastAdvanceDate LastGracePeriodDate CancellationServiceStartDate CancellationServiceEndDate mmddyy10.; 
		output _22Perkins;
	END;
RUN;

%LET EA27_STR = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA_2.dsn; update_lock_typ=nolock; bl_keepnulls=no");

LIBNAME EA27 ODBC &EA27_STR;

proc sort data=_01BorrowerRecord ; by BorrowerSSN; run;
proc sort data=_02DefermentDataRecord ; by BorrowerSSN ; run;
proc sort data=_03PaymentDataRecord; by BorrowerSSN  ; run;
proc sort data=_04InterestLateChargeRecord ; by BorrowerSSN  ; run;
proc sort data=_05SupplementalBorrowerRecord ; by BorrowerSSN  ; run;
proc sort data=_06BorrowerAddressRecord ; by BorrowerSSN  ; run;
proc sort data=_07_08DisbClaimEnrollRecord ; by BorrowerSSN   DisbursementNumber; run;
proc sort data=_09SummarizedDisbursementRecord ; by BorrowerSSN  ; run;
proc sort data=_10_11ReferenceData ; by BorrowerSSN RefNumber; run;
proc sort data=_12DefermentHistory ; by BorrowerSSN   DefermentBeginDate; run;
proc sort data=_13SupplementalPaymentData ; by BorrowerSSN   ; run;
proc sort data=_14SupplementalRepaymentData ; by BorrowerSSN   ; run;
proc sort data=_15BenefitRecord ; by BorrowerSSN   BenefitType; run;
proc sort data=_16MPNRecord ; by BorrowerSSN   MPNID; run;
proc sort data=_17ConsolidationRecord ; by BorrowerSSN   ; run;
proc sort data=_18DirectDebit ; by BorrowerSSN   ; run;
proc sort data=_19TeachGrant ; by BorrowerSSN   ; run;
proc sort data=_20TeachGrant ; by BorrowerSSN   ; run;
proc sort data=_22Perkins ; by BorrowerSSN   ; run;

%macro dedup(dataset);
proc sql;
	create table &dataset as 
		select distinct 
			*
		from
			&dataset
;
quit;
%mend;

%dedup(_01BorrowerRecord);
%dedup(_02DefermentDataRecord);
%dedup(_03PaymentDataRecord);
%dedup(_04InterestLateChargeRecord);
%dedup(_05SupplementalBorrowerRecord);
%dedup(_06BorrowerAddressRecord);
%dedup(_07_08DisbClaimEnrollRecord);
%dedup(_09SummarizedDisbursementRecord);
%dedup(_10_11ReferenceData);
%dedup(_12DefermentHistory);
%dedup(_13SupplementalPaymentData);
%dedup(_14SupplementalRepaymentData);
%dedup(_15BenefitRecord);
%dedup(_16MPNRecord);
%dedup(_17ConsolidationRecord);
%dedup(_18DirectDebit);
%dedup(_19TeachGrant);
%dedup(_20TeachGrant);
%dedup(_22Perkins);

%macro insert2table(tbl);
DATA EA27.&TBL;
SET &TBL;
RUN;
%mend;

/**To erase all data;*/
PROC SQL noprint;
CONNECT TO ODBC AS EA27 (&EA27_STR);
SELECT *
FROM CONNECTION TO EA27 (
drop table _01BorrowerRecord
drop table _02DefermentDataRecord
drop table _03PaymentDataRecord
drop table _04InterestLateChargeRecord
drop table _05SupplementalBorrowerRecord
drop table _06BorrowerAddressRecord
drop table _07_08DisbClaimEnrollRecord
drop table _09SummarizedDisbursementRecord
drop table _10_11ReferenceData
drop table _12DefermentHistory
drop table _13SupplementalPaymentData
drop table _14SupplementalRepaymentData
drop table _15BenefitRecord
drop table _16MPNRecord
drop table _17ConsolidationRecord
drop table _18DirectDebit
drop table _19TeachGrant
drop table _20TeachGrant
drop table _22Perkins

);
DISCONNECT FROM EA27;
QUIT;

%insert2table(_01BorrowerRecord);
%insert2table(_02DefermentDataRecord);
%insert2table(_03PaymentDataRecord);
%insert2table(_04InterestLateChargeRecord);
%insert2table(_05SupplementalBorrowerRecord);
%insert2table(_06BorrowerAddressRecord);
%insert2table(_07_08DisbClaimEnrollRecord);
%insert2table(_09SummarizedDisbursementRecord);
%insert2table(_10_11ReferenceData);
%insert2table(_12DefermentHistory);
%insert2table(_13SupplementalPaymentData);
%insert2table(_14SupplementalRepaymentData);
%insert2table(_15BenefitRecord);
%insert2table(_16MPNRecord);
%insert2table(_17ConsolidationRecord);
%insert2table(_18DirectDebit);
%insert2table(_19TeachGrant);
%insert2table(_20TeachGrant);
%insert2table(_22Perkins);
