﻿CREATE TYPE [fafsadblod].[FafsaData] AS TABLE(
	[YearIndicator] [char](1) NOT NULL,
	[OriginalSsn] [varchar](9) NOT NULL,
	[OriginalNameId] [varchar](2) NOT NULL,
	[TransactionNumber] [varchar](2) NOT NULL,
	[StudentLastName] [varchar](16) NOT NULL,
	[StudentFirstName] [varchar](12) NOT NULL,
	[MiddleInitial] [varchar](1) NOT NULL,
	[PermanentMailingAddress] [varchar](35) NOT NULL,
	[PermanentCity] [varchar](16) NOT NULL,
	[PermanentState] [varchar](2) NOT NULL,
	[PermanentZipCode] [varchar](5) NOT NULL,
	[StudentDateOfBirth] [varchar](8) NOT NULL,
	[StudentPermanentPhoneNumber] [varchar](10) NOT NULL,
	[StudentDriversLicenseNumber] [varchar](20) NOT NULL,
	[StudentDriversLicenseStateCode] [varchar](2) NOT NULL,
	[StudentEmailAddress] [varchar](50) NOT NULL,
	[StudentCitizenshipStatus] [varchar](1) NOT NULL,
	[StudentAlientRegistrationNumber] [varchar](9) NOT NULL,
	[StudentMaritalStatus] [varchar](1) NOT NULL,
	[StudentMaritalStatusDate] [varchar](6) NOT NULL,
	[StudentStateOfLegalResidence] [varchar](2) NOT NULL,
	[StudentLegalResidentBefore01012015] [varchar](1) NOT NULL,
	[StudentLegalResidenceDate] [varchar](6) NOT NULL,
	[Gender] [varchar](1) NOT NULL,
	[SelectiveServiceRegister] [varchar](1) NOT NULL,
	[DrugConvictionAffectingEligibility] [varchar](1) NOT NULL,
	[Parent1HighestGradeCompleted] [varchar](1) NOT NULL,
	[Parent2HighestGradeCompleted] [varchar](1) NOT NULL,
	[HighSchoolDiplomaOrEquivalent] [varchar](1) NOT NULL,
	[HighSchoolName] [varchar](50) NOT NULL,
	[HighSchoolCity] [varchar](28) NOT NULL,
	[HighSchoolState] [varchar](2) NOT NULL,
	[HighSchoolCode] [varchar](12) NOT NULL,
	[FirstBachelorsDegreeBy07012020] [varchar](1) NOT NULL,
	[GradeLevelInCollege] [varchar](1) NOT NULL,
	[DegreeCertificate] [varchar](1) NOT NULL,
	[InterestInWorkStudy] [varchar](1) NOT NULL,
	[Filler1] [varchar](50) NOT NULL,
	[StudentTaxReturnCompleted] [varchar](1) NOT NULL,
	[StudentTypeOf2018TaxForm] [varchar](1) NOT NULL,
	[StudentTaxReturnFilingStatus] [varchar](1) NOT NULL,
	[StudentFiledSchedule1] [varchar](1) NOT NULL,
	[StudentAdjustedGrossIncomeFromIrsForm] [varchar](7) NOT NULL,
	[StudentUSIncomeTaxPaid] [varchar](7) NOT NULL,
	[StudentIncomeEarnedFromWork] [varchar](7) NOT NULL,
	[SpouseIncomeEarnedFromWork] [varchar](7) NOT NULL,
	[StudentCashSavingsChecking] [varchar](7) NOT NULL,
	[StudentInvestmentNetWorth] [varchar](7) NOT NULL,
	[StudentBusinessInvestmentFarmNetWorth] [varchar](7) NOT NULL,
	[StudentEducationalCredits] [varchar](7) NOT NULL,
	[StudentChildSupportPaid] [varchar](7) NOT NULL,
	[StudentNeedBasedEmployment] [varchar](7) NOT NULL,
	[StudentGrantScholarshipAid] [varchar](7) NOT NULL,
	[StudentCombatPay] [varchar](7) NOT NULL,
	[StudentCoopEarnings] [varchar](7) NOT NULL,
	[StudentPensionPayments] [varchar](7) NOT NULL,
	[StudentIraPayments] [varchar](7) NOT NULL,
	[StudentChildSupportReceived] [varchar](7) NOT NULL,
	[StudentInterestIncome] [varchar](7) NOT NULL,
	[StudentUntaxedPortionOfIraDistributionAndPensions] [varchar](7) NOT NULL,
	[StudentMilitaryClergyAllowances] [varchar](7) NOT NULL,
	[StudentVeteransNoneducationBenefits] [varchar](7) NOT NULL,
	[StudentOtherUntaxedIncome] [varchar](7) NOT NULL,
	[StudentOtherNonReportedMoney] [varchar](7) NOT NULL,
	[Filler2] [varchar](59) NOT NULL,
	[BornBefore01011997] [varchar](1) NOT NULL,
	[StudentMarried] [varchar](1) NOT NULL,
	[WorkingOnMastersOrDoctorate] [varchar](1) NOT NULL,
	[ActiveDutyArmedForces] [varchar](1) NOT NULL,
	[VeteranArmedForces] [varchar](1) NOT NULL,
	[HasChildDependents] [varchar](1) NOT NULL,
	[HasDependentsOtherThanSpouseOrChild] [varchar](1) NOT NULL,
	[OrphanWardOfCourtFosterCare] [varchar](1) NOT NULL,
	[EmancipatedMinor] [varchar](1) NOT NULL,
	[LegalGuardianship] [varchar](1) NOT NULL,
	[UnaccompaniedYouthSchoolDistrictLiaison] [varchar](1) NOT NULL,
	[UnaccompaniedYouthHUD] [varchar](1) NOT NULL,
	[RiskOfHomelessness] [varchar](1) NOT NULL,
	[Filler3] [varchar](5) NOT NULL,
	[ParentMaritalStatus] [varchar](1) NOT NULL,
	[ParentMaritalStatusDate] [varchar](6) NOT NULL,
	[Parent1Ssn] [varchar](9) NOT NULL,
	[Parent1LastName] [varchar](16) NOT NULL,
	[Parent1FirstNameInitial] [varchar](1) NOT NULL,
	[Parent1DateOfBirth] [varchar](8) NOT NULL,
	[Parent2Ssn] [varchar](9) NOT NULL,
	[Parent2LastName] [varchar](16) NOT NULL,
	[Parent2FirstNameInitial] [varchar](1) NOT NULL,
	[Parent2DateOfBirth] [varchar](8) NOT NULL,
	[ParentEmailAddress] [varchar](50) NOT NULL,
	[ParentStateOfLegalResidence] [varchar](2) NOT NULL,
	[ParentLegalResidentBefore01012015] [varchar](1) NOT NULL,
	[ParentLegalResidenceDate] [varchar](6) NOT NULL,
	[ParentNumberOfFamilyMembers] [varchar](2) NOT NULL,
	[ParentNumberInCollege] [varchar](1) NOT NULL,
	[ParentMedicaidOrSupplementalSecurityIncomeBenefits] [varchar](1) NOT NULL,
	[ParentSupplementalNutritionAssistanceProgramBenefits] [varchar](1) NOT NULL,
	[ParentFreeOrReducedPriceSchoolLunchBenefits] [varchar](1) NOT NULL,
	[ParentTanfBenefits] [varchar](1) NOT NULL,
	[ParentWicBenefits] [varchar](1) NOT NULL,
	[ParentTaxReturnCompleted] [varchar](1) NOT NULL,
	[ParentTypeOf2018TaxForm] [varchar](1) NOT NULL,
	[ParentTaxReturnFilingStatus] [varchar](1) NOT NULL,
	[ParentFiledSchedule1] [varchar](1) NOT NULL,
	[ParentDislocatedWorker] [varchar](1) NOT NULL,
	[ParentAdjustedGrossIncomeFromIrsForm] [varchar](7) NOT NULL,
	[ParentUSIncomeTaxPaid] [varchar](7) NOT NULL,
	[Parent1IncomeEarnedFromWork] [varchar](7) NOT NULL,
	[Parent2IncomeEarnedFromWork] [varchar](7) NOT NULL,
	[ParentCashSavingsChecking] [varchar](7) NOT NULL,
	[ParentInvestmentNetWorth] [varchar](7) NOT NULL,
	[ParentBusinessInvestmentFarmNetWorth] [varchar](7) NOT NULL,
	[ParentEducationalCredits] [varchar](7) NOT NULL,
	[ParentChildSupportPaid] [varchar](7) NOT NULL,
	[ParentNeedBasedEmployment] [varchar](7) NOT NULL,
	[ParentGrantScholarshipAid] [varchar](7) NOT NULL,
	[ParentCombatPay] [varchar](7) NOT NULL,
	[ParentCoopEarnings] [varchar](7) NOT NULL,
	[ParentPensionPayments] [varchar](7) NOT NULL,
	[ParentIraPayments] [varchar](7) NOT NULL,
	[ParentChildSupportReceived] [varchar](7) NOT NULL,
	[ParentInterestIncome] [varchar](7) NOT NULL,
	[ParentUntaxedPortionsOfIraDistributionAndPensions] [varchar](7) NOT NULL,
	[ParentMilitaryClergyAllowances] [varchar](7) NOT NULL,
	[ParentVeteransNoneducationBenefits] [varchar](7) NOT NULL,
	[ParentOtherUntaxedIncome] [varchar](7) NOT NULL,
	[Filler4] [varchar](59) NOT NULL,
	[StudentNumberOfFamilyMembers] [varchar](2) NOT NULL,
	[StudentNumberInCollege] [varchar](1) NOT NULL,
	[StudentMedicaidOrSupplementalSecurityIncomeBenefits] [varchar](1) NOT NULL,
	[StudentSupplementalNutritionAssistanceProgramBenefits] [varchar](1) NOT NULL,
	[StudentFreeOrReducedPriceLunchBenefits] [varchar](1) NOT NULL,
	[StudentTanfBenefits] [varchar](1) NOT NULL,
	[StudentWicBenefits] [varchar](1) NOT NULL,
	[StudentSpouseDislocatedWorker] [varchar](1) NOT NULL,
	[Filler5] [varchar](5) NOT NULL,
	[FederalSchoolCode1] [varchar](6) NOT NULL,
	[FederalSchoolCode1HousingPlans] [varchar](1) NOT NULL,
	[FederalSchoolCode2] [varchar](6) NOT NULL,
	[FederalSchoolCode2HousingPlans] [varchar](1) NOT NULL,
	[FederalSchoolCode3] [varchar](6) NOT NULL,
	[FederalSchoolCode3HousingPlans] [varchar](1) NOT NULL,
	[FederalSchoolCode4] [varchar](6) NOT NULL,
	[FederalSchoolCode4HousingPlans] [varchar](1) NOT NULL,
	[FederalSchoolCode5] [varchar](6) NOT NULL,
	[FederalSchoolCode5HousingPlans] [varchar](1) NOT NULL,
	[FederalSchoolCode6] [varchar](6) NOT NULL,
	[FederalSchoolCode6HousingPlans] [varchar](1) NOT NULL,
	[FederalSchoolCode7] [varchar](6) NOT NULL,
	[FederalSchoolCode7HousingPlans] [varchar](1) NOT NULL,
	[FederalSchoolCode8] [varchar](6) NOT NULL,
	[FederalSchoolCode8HousingPlans] [varchar](1) NOT NULL,
	[FederalSchoolCode9] [varchar](6) NOT NULL,
	[FederalSchoolCode9HousingPlans] [varchar](1) NOT NULL,
	[FederalSchoolCode10] [varchar](6) NOT NULL,
	[FederalSchoolCode10HousingPlans] [varchar](1) NOT NULL,
	[Filler6] [varchar](35) NOT NULL,
	[DateApplicationCompleted] [varchar](8) NOT NULL,
	[SignedBy] [varchar](1) NOT NULL,
	[Filler7] [varchar](5) NOT NULL,
	[PreparerSsn] [varchar](9) NOT NULL,
	[PreparerEmployeeIdentificationNumber] [varchar](9) NOT NULL,
	[PreparerSignature] [varchar](1) NOT NULL,
	[Filler8] [varchar](10) NOT NULL,
	[DependencyOverrideIndicator] [varchar](1) NOT NULL,
	[FaaFederalSchoolCode] [varchar](6) NOT NULL,
	[Filler9] [varchar](11) NOT NULL,
	[DependencyStatus] [varchar](1) NOT NULL,
	[TransactionDataSourceTypeCode] [varchar](2) NOT NULL,
	[TransactionReceiptDate] [varchar](8) NOT NULL,
	[SpecialCircumstancesFlag] [varchar](1) NOT NULL,
	[StudentIrsRequestFlag] [varchar](2) NOT NULL,
	[ParentIrsRequestFlag] [varchar](2) NOT NULL,
	[ParentAssetThresholdExceeded] [varchar](1) NOT NULL,
	[StudentAssetThresholdExceeded] [varchar](1) NOT NULL,
	[StudentIrsAdjustedGrossIncomeDataFieldFlag] [varchar](1) NOT NULL,
	[StudentIrsFederalIncomeTaxDataFieldFlag] [varchar](1) NOT NULL,
	[ParentIrsAdjustedGrossIncomeDataFieldFlag] [varchar](1) NOT NULL,
	[ParentIrsFederalIncomeTaxDataFieldFlag] [varchar](1) NOT NULL,
	[StudentIrsDisplayFlag] [varchar](1) NOT NULL,
	[ParentIrsDisplayFlag] [varchar](1) NOT NULL,
	[StudentIrsTypeOfTaxReturnDataFieldFlag] [varchar](1) NOT NULL,
	[StudentIrsEducationCreditsDataFieldFlag] [varchar](1) NOT NULL,
	[StudentIrsIraPaymentsDataFieldFlag] [varchar](1) NOT NULL,
	[StudentIrsInterestIncomeDataFieldFlag] [varchar](1) NOT NULL,
	[StudentIrsUntaxedPortionsOfIraDistributionAndPensionsDataFieldFlag] [varchar](1) NOT NULL,
	[Filler10] [varchar](1) NOT NULL,
	[StudentIrsTaxReturnFilingStatusDataFieldFlag] [varchar](1) NOT NULL,
	[Filler11] [varchar](1) NOT NULL,
	[ParentIrsTypeOfTaxReturnDataFieldFlag] [varchar](1) NOT NULL,
	[ParentIrsEducationCreditsDataFieldFlag] [varchar](1) NOT NULL,
	[ParentIrsIraPaymentsDataFieldFlag] [varchar](1) NOT NULL,
	[ParentIrsInterestIncomeDataFieldFlag] [varchar](1) NOT NULL,
	[ParentIrsUntaxedPortionsOfIraDistributionAndPensionsDataFieldFlag] [varchar](1) NOT NULL,
	[Filler12] [varchar](5) NOT NULL,
	[ParentIrsTaxReturnFilingStatusDataFieldFlag] [varchar](1) NOT NULL,
	[Filler13] [varchar](1) NOT NULL,
	[ChildrenOfFallenHeroesIndicator] [varchar](1) NOT NULL,
	[Filler14] [varchar](62) NOT NULL,
	[AssumptionOverride1] [varchar](1) NOT NULL,
	[AssumptionOverride2] [varchar](1) NOT NULL,
	[AssumptionOverride3] [varchar](1) NOT NULL,
	[AssumptionOverride4] [varchar](1) NOT NULL,
	[AssumptionOverride5] [varchar](1) NOT NULL,
	[AssumptionOverride6] [varchar](1) NOT NULL,
	[ElectronicTransactionIndicatorDestinationNumber] [varchar](7) NOT NULL,
	[RejectOverride3] [varchar](1) NOT NULL,
	[RejectOverride12] [varchar](1) NOT NULL,
	[RejectOverride20] [varchar](1) NOT NULL,
	[RejectOverrideA] [varchar](1) NOT NULL,
	[RejectOverrideB] [varchar](1) NOT NULL,
	[RejectOverrideC] [varchar](1) NOT NULL,
	[RejectOverrideG] [varchar](1) NOT NULL,
	[RejectOverrideJ] [varchar](1) NOT NULL,
	[RejectOverrideK] [varchar](1) NOT NULL,
	[RejectOverrideN] [varchar](1) NOT NULL,
	[RejectOverrideW] [varchar](1) NOT NULL,
	[RejectOverride21] [varchar](1) NOT NULL,
	[Filler15] [varchar](79) NOT NULL,
	[StudentCurrentSsn] [varchar](9) NOT NULL,
	[CorrectionAppliedAgainstTransactionNumber] [varchar](2) NOT NULL,
	[Filler16] [varchar](4) NOT NULL,
	[ProfessionalJudgment] [varchar](1) NOT NULL,
	[Filler17] [varchar](10) NOT NULL,
	[ApplicationDataSourceTypCode] [varchar](2) NOT NULL,
	[ApplicationReceiptDate] [varchar](8) NOT NULL,
	[AddressOnlyChangeFlag] [varchar](1) NOT NULL,
	[CpsPushedIsirFlag] [varchar](1) NOT NULL,
	[EfcChangeFlag] [varchar](1) NOT NULL,
	[StudentLastNameSsnChangeFlag] [varchar](1) NOT NULL,
	[RejectStatusChangeFlag] [varchar](1) NOT NULL,
	[SarcChangeFlag] [varchar](1) NOT NULL,
	[VerificationSelectionChangeFlag] [varchar](1) NOT NULL,
	[ComputeNumber] [varchar](3) NOT NULL,
	[SourceOfCorrection] [varchar](1) NOT NULL,
	[DuplicateSsnIndicator] [varchar](1) NOT NULL,
	[GraduateFlag] [varchar](1) NOT NULL,
	[PellGrantEligibilityFlag] [varchar](1) NOT NULL,
	[TransactionProcessedDate] [varchar](8) NOT NULL,
	[ProcessedRecordType] [varchar](1) NOT NULL,
	[RejectReasonCodes] [varchar](14) NOT NULL,
	[ReprocessedReasonCode] [varchar](2) NOT NULL,
	[SarcFlag] [varchar](1) NOT NULL,
	[AutomaticZeroEfcIndicator] [varchar](1) NOT NULL,
	[SimplifiedNeedsTest] [varchar](1) NOT NULL,
	[ParentCalculated2018TaxStatus] [varchar](1) NOT NULL,
	[StudentCalculated2018TaxStatus] [varchar](1) NOT NULL,
	[StudentAdditionalFinancialInformationTotalCalculatedByCps] [varchar](8) NOT NULL,
	[StudentUntaxedIncomeTotalCalculatedByCps] [varchar](8) NOT NULL,
	[ParentAdditionalFinancialInformationTotalCalculatedByCps] [varchar](8) NOT NULL,
	[ParentUntaxedIncomeTotalCalculatedByCps] [varchar](8) NOT NULL,
	[HighSchoolFlag] [varchar](1) NOT NULL,
	[Filler18] [varchar](10) NOT NULL,
	[AssumedCitizenship] [varchar](1) NOT NULL,
	[AssumedStudentMaritalStatus] [varchar](1) NOT NULL,
	[AssumedStudentAgi] [varchar](7) NOT NULL,
	[AssumedStudentUSTaxPaid] [varchar](7) NOT NULL,
	[AssumedStudentIncomeFromWork] [varchar](7) NOT NULL,
	[AssumedSpouseIncomeFromWork] [varchar](7) NOT NULL,
	[AssumedStudentAdditionalFinancialInformationTotal] [varchar](8) NOT NULL,
	[AssumedDateOfBirthPrior] [varchar](1) NOT NULL,
	[AssumedStudentMarriedRemarried] [varchar](1) NOT NULL,
	[AssumedHasChildDependents] [varchar](1) NOT NULL,
	[AssumedHasDependentsOtherThanSpouseOrChild] [varchar](1) NOT NULL,
	[AssumedStudentNumberInFamily] [varchar](2) NOT NULL,
	[AssumedStudentNumberInCollege] [varchar](1) NOT NULL,
	[Filler19] [varchar](3) NOT NULL,
	[AssumedStudentAssetThresholdExceeded] [varchar](1) NOT NULL,
	[Filler20] [varchar](9) NOT NULL,
	[AssumedParentMaritalStatus] [varchar](1) NOT NULL,
	[AssumedParent1Ssn] [varchar](1) NOT NULL,
	[AssumedParent2Ssn] [varchar](1) NOT NULL,
	[AssumedParentNumberInFamily] [varchar](2) NOT NULL,
	[AssumedParentNumberInCollege] [varchar](1) NOT NULL,
	[AssumedParentAgi] [varchar](7) NOT NULL,
	[AssumedParentUSTaxPaid] [varchar](7) NOT NULL,
	[AssumedParent1IncomeFromWork] [varchar](7) NOT NULL,
	[AssumedParent2IncomeFromWork] [varchar](7) NOT NULL,
	[AssumedParentAdditionalFinancialInformationTotal] [varchar](8) NOT NULL,
	[AssumedParentAssetThresholdExceeded] [varchar](1) NOT NULL,
	[Filler21] [varchar](9) NOT NULL,
	[PrimaryEfc] [varchar](6) NOT NULL,
	[SecondaryEfc] [varchar](6) NOT NULL,
	[SignatureRejectEfc] [varchar](6) NOT NULL,
	[PrimaryEfcType] [varchar](1) NOT NULL,
	[SecondaryEfcType] [varchar](1) NOT NULL,
	[PrimaryAlternateMonth1] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth2] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth3] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth4] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth5] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth6] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth7] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth8] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth10] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth11] [varchar](6) NOT NULL,
	[PrimaryAlternateMonth12] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth1] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth2] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth3] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth4] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth5] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth6] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth7] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth8] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth10] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth11] [varchar](6) NOT NULL,
	[SecondaryAlternateMonth12] [varchar](6) NOT NULL,
	[TotalIncome] [varchar](8) NOT NULL,
	[AllowancesAgainstTotalIncome] [varchar](7) NOT NULL,
	[StateAndOtherTaxAllowance] [varchar](7) NOT NULL,
	[EmploymentAllowance] [varchar](7) NOT NULL,
	[IncomeProtectionAllowance] [varchar](7) NOT NULL,
	[AvailableIncome] [varchar](8) NOT NULL,
	[ContributionFromAvailableIncome] [varchar](7) NOT NULL,
	[DiscretionaryNetWorth] [varchar](9) NOT NULL,
	[NetWorth] [varchar](9) NOT NULL,
	[AssetProtectionAllowance] [varchar](9) NOT NULL,
	[ParentContributionFromAssets] [varchar](7) NOT NULL,
	[AdjustedAvailableIncome] [varchar](8) NOT NULL,
	[TotalStudentContribution] [varchar](7) NOT NULL,
	[TotalParentContribution] [varchar](7) NOT NULL,
	[ParentContribution] [varchar](7) NOT NULL,
	[StudentTotalIncome] [varchar](8) NOT NULL,
	[StudentAllowanceAgainstTotalIncome] [varchar](7) NOT NULL,
	[DependentStudentIncomeContribution] [varchar](7) NOT NULL,
	[StudentDiscretionaryNetWorth] [varchar](9) NOT NULL,
	[StudentContributionFromAssets] [varchar](7) NOT NULL,
	[FisapTotalIncome] [varchar](8) NOT NULL,
	[SecondaryTotalIncome] [varchar](8) NOT NULL,
	[SecondaryAllowancesAgainstTotalIncome] [varchar](7) NOT NULL,
	[SecondaryStateAndOtherTaxAllowance] [varchar](7) NOT NULL,
	[SecondaryEmploymentAllowance] [varchar](7) NOT NULL,
	[SecondaryIncomeProtectionAllowance] [varchar](7) NOT NULL,
	[SecondaryAvailableIncome] [varchar](8) NOT NULL,
	[SecondaryContributionFromAvailableIncome] [varchar](7) NOT NULL,
	[SecondaryDiscretionaryNetWorth] [varchar](9) NOT NULL,
	[SecondaryNetWorth] [varchar](9) NOT NULL,
	[SecondaryAssetProtectionAllowance] [varchar](9) NOT NULL,
	[SecondaryParentContributionFromAssets] [varchar](7) NOT NULL,
	[SecondaryAdjustedAvailableIncome] [varchar](8) NOT NULL,
	[SecondaryTotalStudentContribution] [varchar](7) NOT NULL,
	[SecondaryTotalParentContribution] [varchar](7) NOT NULL,
	[SecondaryParentContribution] [varchar](7) NOT NULL,
	[SecondaryStudentTotalIncome] [varchar](8) NOT NULL,
	[SecondaryStudentAllowanceAgainstTotalIncome] [varchar](7) NOT NULL,
	[SecondaryDependentStudentIncomeContribution] [varchar](7) NOT NULL,
	[SecondaryStudentDiscretionaryNetWorth] [varchar](9) NOT NULL,
	[SecondaryStudentContributionFromAssets] [varchar](7) NOT NULL,
	[SecondaryFisapTotalIncome] [varchar](8) NOT NULL,
	[Filler22] [varchar](50) NOT NULL,
	[CorrectionFlags] [varchar](195) NOT NULL,
	[Filler23] [varchar](15) NOT NULL,
	[HighlightFlags] [varchar](195) NOT NULL,
	[Filler24] [varchar](15) NOT NULL,
	[FafsaDataVerifyFlags] [varchar](195) NOT NULL,
	[Filler25] [varchar](15) NOT NULL,
	[DhsMatchFlag] [varchar](1) NOT NULL,
	[SecondaryDhsMatchFlag] [varchar](1) NOT NULL,
	[Filler26] [varchar](15) NOT NULL,
	[DhsVerificationNumber] [varchar](15) NOT NULL,
	[Filler27] [varchar](1) NOT NULL,
	[NsldsMatchFlag] [varchar](1) NOT NULL,
	[NsldsPostscreeningReasonCode] [varchar](6) NOT NULL,
	[Filler28] [varchar](9) NOT NULL,
	[Parent1SsnMatchFlag] [varchar](1) NOT NULL,
	[Filler29] [varchar](9) NOT NULL,
	[Parent2SsnMatchFlag] [varchar](1) NOT NULL,
	[SelectiveServiceMatchFlag] [varchar](1) NOT NULL,
	[SelectiveServiceRegistrationFlag] [varchar](1) NOT NULL,
	[SsaCitizenshipFlag] [varchar](1) NOT NULL,
	[Filler30] [varchar](8) NOT NULL,
	[SsnMatchFlag] [varchar](1) NOT NULL,
	[VaMatchFlag] [varchar](1) NOT NULL,
	[DepartmentOfDefenseMatchFlag] [varchar](1) NOT NULL,
	[DepartmentOfDefenseParentDateOfDeath] [varchar](8) NOT NULL,
	[Filler31] [varchar](50) NOT NULL,
	[CommentCodes] [varchar](60) NOT NULL,
	[Filler32] [varchar](15) NOT NULL,
	[ElectronicFederalSchoolCodeIndicator] [varchar](1) NOT NULL,
	[ElectronicTransactionIndicatorFlag] [varchar](1) NOT NULL,
	[Filler33] [varchar](10) NOT NULL,
	[Filler34] [varchar](5) NOT NULL,
	[VerificationTrackingFlag] [varchar](4) NOT NULL,
	[StudentSelectedForVerification] [varchar](1) NOT NULL,
	[Filler35] [varchar](199) NOT NULL,
	[NsldsTransactionNumber] [varchar](2) NOT NULL,
	[NsldsDatabaseResultsFlag] [varchar](1) NOT NULL,
	[Filler36] [varchar](1) NOT NULL,
	[NsldsPellOverpaymentFlag] [varchar](1) NOT NULL,
	[NsldsPellOverpaymentContact] [varchar](8) NOT NULL,
	[NsldsSeogOverpaymentFlag] [varchar](1) NOT NULL,
	[NsldsSeogOverpaymentContact] [varchar](8) NOT NULL,
	[NsldsPerkinsOverpaymentFlag] [varchar](1) NOT NULL,
	[NsldsPerkinsOverpaymentContact] [varchar](8) NOT NULL,
	[NsldsTeachOverpaymentFlag] [varchar](1) NOT NULL,
	[NsldsTeachOverpaymentContact] [varchar](8) NOT NULL,
	[NsldsIraqAfghanistanServiceGrantOverpaymentFlag] [varchar](1) NOT NULL,
	[NsldsIraqAfghanistanServiceGrantOverpaymentContact] [varchar](8) NOT NULL,
	[NsldsDefaultedLoanFlag] [varchar](1) NOT NULL,
	[NsldsDischargedLoanFlag] [varchar](1) NOT NULL,
	[NsldsFraudLoanFlag] [varchar](1) NOT NULL,
	[NsldsLoanSatisfactoryRepaymentFlag] [varchar](1) NOT NULL,
	[NsldsActiveBankruptcyFlag] [varchar](1) NOT NULL,
	[NsldsTeachGrantLoanConversionFlag] [varchar](1) NOT NULL,
	[NsldsAggregateSubsidizedOutstandingPrincipalBalance] [varchar](6) NOT NULL,
	[NsldsAggregateUnsubsidizedOutstandingPrincipalBalance] [varchar](6) NOT NULL,
	[NsldsAggregateCombinedOutstandingPrincipalBalance] [varchar](6) NOT NULL,
	[NsldsAggregateUnallocatedConsolidatedOutstandingPrincipalBalance] [varchar](6) NOT NULL,
	[NsldsAggregateTeachLoanPrincipalBalance] [varchar](6) NOT NULL,
	[NsldsAggregateSubsidizedPendingDisbursement] [varchar](6) NOT NULL,
	[NsldsAggregateUnsubsidizedPendingDisbursement] [varchar](6) NOT NULL,
	[NsldsAggregateCombinedPendingDisbursement] [varchar](6) NOT NULL,
	[NsldsAggregateSubsidizedTotal] [varchar](6) NOT NULL,
	[NsldsAggregateUnsubsidizedTotal] [varchar](6) NOT NULL,
	[NsldsAggregateCombinedTotal] [varchar](6) NOT NULL,
	[NsldsUnallocatedConsolidatedTotal] [varchar](6) NOT NULL,
	[NsldsTeachLoanTotal] [varchar](6) NOT NULL,
	[NsldsPerkinsCumulativeDisbursementAmount] [varchar](6) NOT NULL,
	[NsldsPerkinsCurrentYearDisbursementAmount] [varchar](6) NOT NULL,
	[NsldsAggregateTeachUndergraduatePostBaccalaureateDisbursementAmount] [varchar](6) NOT NULL,
	[NsldsAggregateTeachGraduateDisbursementAmount] [varchar](6) NOT NULL,
	[NsldsDefaultedLoanChangeFlag] [varchar](1) NOT NULL,
	[NsldsFraudLoanChangeFlag] [varchar](1) NOT NULL,
	[NsldsDischargedLoanChangeFlag] [varchar](1) NOT NULL,
	[NsldsLoanSatisfactoryRepaymentChangeFlag] [varchar](1) NOT NULL,
	[NsldsActiveBankruptcyChangeFlag] [varchar](1) NOT NULL,
	[NsldsTeachGrantLoanConversionChangeFlag] [varchar](1) NOT NULL,
	[NsldsOverpaymentsChangeFlag] [varchar](1) NOT NULL,
	[NsldsAggregateLoanChangeFlag] [varchar](1) NOT NULL,
	[NsldsPerkinsLoanChangeFlag] [varchar](1) NOT NULL,
	[NsldsPellPaymentChangeFlag] [varchar](1) NOT NULL,
	[NsldsTeachGrantChangeFlag] [varchar](1) NOT NULL,
	[NsldsAdditionalPellFlag] [varchar](1) NOT NULL,
	[NsldsAdditionalLoansFlag] [varchar](1) NOT NULL,
	[NsldsAdditionalTeachGrantFlag] [varchar](1) NOT NULL,
	[NsldsDirectLoanMasterPromNoteFlag] [varchar](1) NOT NULL,
	[NsldsDirectLoanPlusMasterPromNoteFlag] [varchar](1) NOT NULL,
	[NsldsDirectLoanGraduatePlusMasterPromNoteFlag] [varchar](1) NOT NULL,
	[NsldsUndergraduateSubsidizedLoanLimitFlag] [varchar](1) NOT NULL,
	[NsldsUndergraduateCombinedLoanLimitFlag] [varchar](1) NOT NULL,
	[NsldsGraduateSubsidizedLoanLimitFlag] [varchar](1) NOT NULL,
	[NsldsGraduateCombinedLoanLimitFlag] [varchar](1) NOT NULL,
	[NsldsPellLifetimeLimitFlag] [varchar](1) NOT NULL,
	[NsldsPellLifetimeEligibilityUsed] [varchar](7) NOT NULL,
	[NsldsSubsidizedUsageLimitAppliesFlag] [varchar](1) NOT NULL,
	[NsldsSubsidizedUsagePeriod] [varchar](6) NOT NULL,
	[NsldsUnusualEnrollmentHistoryFlag] [varchar](1) NOT NULL,
	[Filler37] [varchar](6) NOT NULL,
	[NsldsPellSequenceNumber1] [varchar](2) NOT NULL,
	[NsldsPellVerificationFlag1] [varchar](3) NOT NULL,
	[NsldsEfc1] [varchar](6) NOT NULL,
	[NsldsPellSchoolCode1] [varchar](8) NOT NULL,
	[NsldsPellTransactionNumber1] [varchar](2) NOT NULL,
	[NsldsPellLastUpdateDate1] [varchar](8) NOT NULL,
	[NsldsPellScheduledAmount1] [varchar](6) NOT NULL,
	[NsldsPellAmountPaidToDate1] [varchar](6) NOT NULL,
	[NsldsPellPercentScheduledAwardUsedByAwardYear1] [varchar](7) NOT NULL,
	[NsldsPellAwardAmount1] [varchar](6) NOT NULL,
	[NsldsAdditionalEligibilityIndicator1] [varchar](1) NOT NULL,
	[Filler38] [varchar](3) NOT NULL,
	[Filler39] [varchar](4) NOT NULL,
	[NsldsPellSequenceNumber2] [varchar](2) NOT NULL,
	[NsldsPellVerificationFlag2] [varchar](3) NOT NULL,
	[NsldsEfc2] [varchar](6) NOT NULL,
	[NsldsPellSchoolCode2] [varchar](8) NOT NULL,
	[NsldsPellTransactionNumber2] [varchar](2) NOT NULL,
	[NsldsPellLastUpdateDate2] [varchar](8) NOT NULL,
	[NsldsPellScheduledAmount2] [varchar](6) NOT NULL,
	[NsldsPellAmountPaidToDate2] [varchar](6) NOT NULL,
	[NsldsPellPercentScheduledAwardUsedByAwardYear2] [varchar](7) NOT NULL,
	[NsldsPellAwardAmount2] [varchar](6) NOT NULL,
	[NsldsAdditionalEligibilityIndicator2] [varchar](1) NOT NULL,
	[Filler40] [varchar](3) NOT NULL,
	[Filler41] [varchar](4) NOT NULL,
	[NsldsPellSequenceNumber3] [varchar](2) NOT NULL,
	[NsldsPellVerificationFlag3] [varchar](3) NOT NULL,
	[NsldsEfc3] [varchar](6) NOT NULL,
	[NsldsPellSchoolCode3] [varchar](8) NOT NULL,
	[NsldsPellTransactionNumber3] [varchar](2) NOT NULL,
	[NsldsPellLastUpdateDate3] [varchar](8) NOT NULL,
	[NsldsPellScheduledAmount3] [varchar](6) NOT NULL,
	[NsldsPellAmountPaidToDate3] [varchar](6) NOT NULL,
	[NsldsPellPercentScheduledAwardUsedByAwardYear3] [varchar](7) NOT NULL,
	[NsldsPellAwardAmount3] [varchar](6) NOT NULL,
	[NsldsAdditionalEligibilityIndicator3] [varchar](1) NOT NULL,
	[Filler42] [varchar](3) NOT NULL,
	[Filler43] [varchar](4) NOT NULL,
	[NsldsTeachGrantSequenceNumber1] [varchar](2) NOT NULL,
	[NsldsTeachGrantSchoolCode1] [varchar](8) NOT NULL,
	[NsldsTeachGrantTransactionNumber1] [varchar](2) NOT NULL,
	[NsldsTeachGrantLastUpdateDate1] [varchar](8) NOT NULL,
	[NsldsTeachGrantScheduledAwardAmount1] [varchar](6) NOT NULL,
	[NsldsTeachGrantAmountPaidToDate1] [varchar](6) NOT NULL,
	[NsldsTeachGrantAwardAmount1] [varchar](6) NOT NULL,
	[NsldsTeachGrantAcademicYearLevel1] [varchar](1) NOT NULL,
	[NsldsTeachGrantCodSequenceCode1] [varchar](3) NOT NULL,
	[NsldsTeachGrantAwardYear1] [varchar](4) NOT NULL,
	[NsldsTeachGrantLoanConversionFlag1] [varchar](1) NOT NULL,
	[NsldsTeachGrantSequenceNumber2] [varchar](2) NOT NULL,
	[NsldsTeachGrantSchoolCode2] [varchar](8) NOT NULL,
	[NsldsTeachGrantTransactionNumber2] [varchar](2) NOT NULL,
	[NsldsTeachGrantLastUpdateDate2] [varchar](8) NOT NULL,
	[NsldsTeachGrantScheduledAwardAmount2] [varchar](6) NOT NULL,
	[NsldsTeachGrantAmountPaidToDate2] [varchar](6) NOT NULL,
	[NsldsTeachGrantAwardAmount2] [varchar](6) NOT NULL,
	[NsldsTeachGrantAcademicYearLevel2] [varchar](1) NOT NULL,
	[NsldsTeachGrantCodSequenceCode2] [varchar](3) NOT NULL,
	[NsldsTeachGrantAwardYear2] [varchar](4) NOT NULL,
	[NsldsTeachGrantLoanConversionFlag2] [varchar](1) NOT NULL,
	[NsldsTeachGrantSequenceNumber3] [varchar](2) NOT NULL,
	[NsldsTeachGrantSchoolCode3] [varchar](8) NOT NULL,
	[NsldsTeachGrantTransactionNumber3] [varchar](2) NOT NULL,
	[NsldsTeachGrantLastUpdateDate3] [varchar](8) NOT NULL,
	[NsldsTeachGrantScheduledAwardAmount3] [varchar](6) NOT NULL,
	[NsldsTeachGrantAmountPaidToDate3] [varchar](6) NOT NULL,
	[NsldsTeachGrantAwardAmount3] [varchar](6) NOT NULL,
	[NsldsTeachGrantAcademicYearLevel3] [varchar](1) NOT NULL,
	[NsldsTeachGrantCodSequenceCode3] [varchar](3) NOT NULL,
	[NsldsTeachGrantAwardYear3] [varchar](4) NOT NULL,
	[NsldsTeachGrantLoanConversionFlag3] [varchar](1) NOT NULL,
	[NsldsLoanSequenceNumber1] [varchar](2) NOT NULL,
	[NsldsLoanTypeCode1] [varchar](1) NOT NULL,
	[NsldsLoanChangeFlag1] [varchar](1) NOT NULL,
	[NsldsLoanProgramCode1] [varchar](2) NOT NULL,
	[NsldsLoanNetAmount1] [varchar](6) NOT NULL,
	[NsldsLoanCurrentStatusCode1] [varchar](2) NOT NULL,
	[NsldsLoanCurrentStatusDate1] [varchar](8) NOT NULL,
	[NsldsLoanAggregatePrincipalBalance1] [varchar](6) NOT NULL,
	[NsldsLoanPrincipalBalanceDate1] [varchar](8) NOT NULL,
	[NsldsLoanBeginDate1] [varchar](8) NOT NULL,
	[NsldsLoanEndDate1] [varchar](8) NOT NULL,
	[NsldsLoanGuarantyAgencyCode1] [varchar](3) NOT NULL,
	[NsldsLoanContactType1] [varchar](3) NOT NULL,
	[NsldsLoanSchoolCode1] [varchar](8) NOT NULL,
	[NsldsLoanContactCode1] [varchar](8) NOT NULL,
	[NsldsLoanGradeLevel1] [varchar](3) NOT NULL,
	[NsldsLoanAdditionalUnsubsidizedFlag1] [varchar](1) NOT NULL,
	[NsldsLoanCapitalizedInterestFlag1] [varchar](1) NOT NULL,
	[NsldsLoanDisbursementAmount1] [varchar](6) NOT NULL,
	[NsldsLoanDisbursementDate1] [varchar](8) NOT NULL,
	[NsldsLoanConfirmedLoanSubsidyStatus1] [varchar](1) NOT NULL,
	[NsldsLoanSubsidyStatusDate1] [varchar](8) NOT NULL,
	[NsldsLoanSequenceNumber2] [varchar](2) NOT NULL,
	[NsldsLoanTypeCode2] [varchar](1) NOT NULL,
	[NsldsLoanChangeFlag2] [varchar](1) NOT NULL,
	[NsldsLoanProgramCode2] [varchar](2) NOT NULL,
	[NsldsLoanNetAmount2] [varchar](6) NOT NULL,
	[NsldsLoanCurrentStatusCode2] [varchar](2) NOT NULL,
	[NsldsLoanCurrentStatusDate2] [varchar](8) NOT NULL,
	[NsldsLoanAggregatePrincipalBalance2] [varchar](6) NOT NULL,
	[NsldsLoanPrincipalBalanceDate2] [varchar](8) NOT NULL,
	[NsldsLoanBeginDate2] [varchar](8) NOT NULL,
	[NsldsLoanEndDate2] [varchar](8) NOT NULL,
	[NsldsLoanGuarantyAgencyCode2] [varchar](3) NOT NULL,
	[NsldsLoanContactType2] [varchar](3) NOT NULL,
	[NsldsLoanSchoolCode2] [varchar](8) NOT NULL,
	[NsldsLoanContactCode2] [varchar](8) NOT NULL,
	[NsldsLoanGradeLevel2] [varchar](3) NOT NULL,
	[NsldsLoanAdditionalUnsubsidizedFlag2] [varchar](1) NOT NULL,
	[NsldsLoanCapitalizedInterestFlag2] [varchar](1) NOT NULL,
	[NsldsLoanDisbursementAmount2] [varchar](6) NOT NULL,
	[NsldsLoanDisbursementDate2] [varchar](8) NOT NULL,
	[NsldsLoanConfirmedLoanSubsidyStatus2] [varchar](1) NOT NULL,
	[NsldsLoanSubsidyStatusDate2] [varchar](8) NOT NULL,
	[NsldsLoanSequenceNumber3] [varchar](2) NOT NULL,
	[NsldsLoanTypeCode3] [varchar](1) NOT NULL,
	[NsldsLoanChangeFlag3] [varchar](1) NOT NULL,
	[NsldsLoanProgramCode3] [varchar](2) NOT NULL,
	[NsldsLoanNetAmount3] [varchar](6) NOT NULL,
	[NsldsLoanCurrentStatusCode3] [varchar](2) NOT NULL,
	[NsldsLoanCurrentStatusDate3] [varchar](8) NOT NULL,
	[NsldsLoanAggregatePrincipalBalance3] [varchar](6) NOT NULL,
	[NsldsLoanPrincipalBalanceDate3] [varchar](8) NOT NULL,
	[NsldsLoanBeginDate3] [varchar](8) NOT NULL,
	[NsldsLoanEndDate3] [varchar](8) NOT NULL,
	[NsldsLoanGuarantyAgencyCode3] [varchar](3) NOT NULL,
	[NsldsLoanContactType3] [varchar](3) NOT NULL,
	[NsldsLoanSchoolCode3] [varchar](8) NOT NULL,
	[NsldsLoanContactCode3] [varchar](8) NOT NULL,
	[NsldsLoanGradeLevel3] [varchar](3) NOT NULL,
	[NsldsLoanAdditionalUnsubsidizedFlag3] [varchar](1) NOT NULL,
	[NsldsLoanCapitalizedInterestFlag3] [varchar](1) NOT NULL,
	[NsldsLoanDisbursementAmount3] [varchar](6) NOT NULL,
	[NsldsLoanDisbursementDate3] [varchar](8) NOT NULL,
	[NsldsLoanConfirmedLoanSubsidyStatus3] [varchar](1) NOT NULL,
	[NsldsLoanSubsidyStatusDate3] [varchar](8) NOT NULL,
	[NsldsLoanSequenceNumber4] [varchar](2) NOT NULL,
	[NsldsLoanTypeCode4] [varchar](1) NOT NULL,
	[NsldsLoanChangeFlag4] [varchar](1) NOT NULL,
	[NsldsLoanProgramCode4] [varchar](2) NOT NULL,
	[NsldsLoanNetAmount4] [varchar](6) NOT NULL,
	[NsldsLoanCurrentStatusCode4] [varchar](2) NOT NULL,
	[NsldsLoanCurrentStatusDate4] [varchar](8) NOT NULL,
	[NsldsLoanAggregatePrincipalBalance4] [varchar](6) NOT NULL,
	[NsldsLoanPrincipalBalanceDate4] [varchar](8) NOT NULL,
	[NsldsLoanBeginDate4] [varchar](8) NOT NULL,
	[NsldsLoanEndDate4] [varchar](8) NOT NULL,
	[NsldsLoanGuarantyAgencyCode4] [varchar](3) NOT NULL,
	[NsldsLoanContactType4] [varchar](3) NOT NULL,
	[NsldsLoanSchoolCode4] [varchar](8) NOT NULL,
	[NsldsLoanContactCode4] [varchar](8) NOT NULL,
	[NsldsLoanGradeLevel4] [varchar](3) NOT NULL,
	[NsldsLoanAdditionalUnsubsidizedFlag4] [varchar](1) NOT NULL,
	[NsldsLoanCapitalizedInterestFlag4] [varchar](1) NOT NULL,
	[NsldsLoanDisbursementAmount4] [varchar](6) NOT NULL,
	[NsldsLoanDisbursementDate4] [varchar](8) NOT NULL,
	[NsldsLoanConfirmedLoanSubsidyStatus4] [varchar](1) NOT NULL,
	[NsldsLoanSubsidyStatusDate4] [varchar](8) NOT NULL,
	[NsldsLoanSequenceNumber5] [varchar](2) NOT NULL,
	[NsldsLoanTypeCode5] [varchar](1) NOT NULL,
	[NsldsLoanChangeFlag5] [varchar](1) NOT NULL,
	[NsldsLoanProgramCode5] [varchar](2) NOT NULL,
	[NsldsLoanNetAmount5] [varchar](6) NOT NULL,
	[NsldsLoanCurrentStatusCode5] [varchar](2) NOT NULL,
	[NsldsLoanCurrentStatusDate5] [varchar](8) NOT NULL,
	[NsldsLoanAggregatePrincipalBalance5] [varchar](6) NOT NULL,
	[NsldsLoanPrincipalBalanceDate5] [varchar](8) NOT NULL,
	[NsldsLoanBeginDate5] [varchar](8) NOT NULL,
	[NsldsLoanEndDate5] [varchar](8) NOT NULL,
	[NsldsLoanGuarantyAgencyCode5] [varchar](3) NOT NULL,
	[NsldsLoanContactType5] [varchar](3) NOT NULL,
	[NsldsLoanSchoolCode5] [varchar](8) NOT NULL,
	[NsldsLoanContactCode5] [varchar](8) NOT NULL,
	[NsldsLoanGradeLevel5] [varchar](3) NOT NULL,
	[NsldsLoanAdditionalUnsubsidizedFlag5] [varchar](1) NOT NULL,
	[NsldsLoanCapitalizedInterestFlag5] [varchar](1) NOT NULL,
	[NsldsLoanDisbursementAmount5] [varchar](6) NOT NULL,
	[NsldsLoanDisbursementDate5] [varchar](8) NOT NULL,
	[NsldsLoanConfirmedLoanSubsidyStatus5] [varchar](1) NOT NULL,
	[NsldsLoanSubsidyStatusDate5] [varchar](8) NOT NULL,
	[NsldsLoanSequenceNumber6] [varchar](2) NOT NULL,
	[NsldsLoanTypeCode6] [varchar](1) NOT NULL,
	[NsldsLoanChangeFlag6] [varchar](1) NOT NULL,
	[NsldsLoanProgramCode6] [varchar](2) NOT NULL,
	[NsldsLoanNetAmount6] [varchar](6) NOT NULL,
	[NsldsLoanCurrentStatusCode6] [varchar](2) NOT NULL,
	[NsldsLoanCurrentStatusDate6] [varchar](8) NOT NULL,
	[NsldsLoanAggregatePrincipalBalance6] [varchar](6) NOT NULL,
	[NsldsLoanPrincipalBalanceDate6] [varchar](8) NOT NULL,
	[NsldsLoanBeginDate6] [varchar](8) NOT NULL,
	[NsldsLoanEndDate6] [varchar](8) NOT NULL,
	[NsldsLoanGuarantyAgencyCode6] [varchar](3) NOT NULL,
	[NsldsLoanContactType6] [varchar](3) NOT NULL,
	[NsldsLoanSchoolCode6] [varchar](8) NOT NULL,
	[NsldsLoanContactCode6] [varchar](8) NOT NULL,
	[NsldsLoanGradeLevel6] [varchar](3) NOT NULL,
	[NsldsLoanAdditionalUnsubsidizedFlag6] [varchar](1) NOT NULL,
	[NsldsLoanCapitalizedInterestFlag6] [varchar](1) NOT NULL,
	[NsldsLoanDisbursementAmount6] [varchar](6) NOT NULL,
	[NsldsLoanDisbursementDate6] [varchar](8) NOT NULL,
	[NsldsLoanConfirmedLoanSubsidyStatus6] [varchar](1) NOT NULL,
	[NsldsLoanSubsidyStatusDate6] [varchar](8) NOT NULL
);