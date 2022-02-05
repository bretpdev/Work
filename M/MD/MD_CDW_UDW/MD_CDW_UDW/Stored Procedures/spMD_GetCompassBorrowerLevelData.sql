CREATE PROCEDURE [dbo].[spMD_GetCompassBorrowerLevelData]
	@AccountIdentifier				VARCHAR(10),
	@IdentifierType					VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SSN					VARCHAR(9)

	--get SSN if account number is passed in
    IF @IdentifierType = 'SSN'
	BEGIN
		SET @SSN = @AccountIdentifier
		PRINT 'SSN'
	END 
	ELSE
	BEGIN
		SET @SSN = (SELECT BF_SSN FROM dbo.PD10_Borrower WHERE DF_SPE_ACC_ID = @AccountIdentifier)
		PRINT 'ACCT#'
	END

	--get all borrower level data with exception of ACH and Employer information
	SELECT	PD10.BF_SSN as SSN,
			PD10.DF_SPE_ACC_ID as AccountNumber,

			COALESCE(SUMM.WA_TOT_BRI_OTS, 0.00) as Interest,
			COALESCE(SUMM.COHORT, '') as CohortYear,
			COALESCE(SUMM.COHORT_IND, 'N') as CohortAlertIndicator,
			COALESCE(SUMM.LA_CUR_PRI, 0.00) as Principal,
			COALESCE(SUMM.LR_ITR_DLY, 0.00) as DailyInterest,
			COALESCE(SUMM.LR_ITR_MONTH, 0.00) as MonthlyInterestAccrual,
			COALESCE(SUMM.LR_ITR_MONTH_5, 0.00) as MonthlyInterestAccrualPlus5,
		
			COALESCE(REPAY.DUE_DAY, '') as NextDueDate,
			COALESCE(REPAY.LD_CRT_LON65, '') as LastRPSPrintDate,
			COALESCE(REPAY.MONTH_AMT, '') as MonthlyPaymentAmount,
			COALESCE(REPAY.MULT_DUE_DT, 'N') as MultipleDueDatesAlertIndicator,
			COALESCE(REPAY.DUE_DAY, '') as DueDay,

			COALESCE(DUE.CUR_DUE, 0.00) as CurrentAmountDue, 
			COALESCE(DUE.PAST_DUE, 0.00) as AmountPastDue,
			COALESCE(DUE.TOT_DUE, 0.00) as TotalAmountDue,
			COALESCE(DUE.TOT_DUE_FEE, 0.00) as TotalAmountPlusLateFees,

			COALESCE(BILL.MULT_BIL_MTD, 'N') as MultipleBillMethodAlertIndicator,
			COALESCE(BILL.BIL_MTD, '') as BillType,

			COALESCE(LPMT.LST_PMT_RCVD, '') as LastPaymentReceivedDate,
			COALESCE(ENDR.COBORROWER, 'N') as CoborrowerAlertIndicator,
			COALESCE(FORB.FORB_36, 'N') as Exceeded36MonthForbAlertIndicator,
			COALESCE(DELQ.LD_DLQ_OCC, '') as DateDelinquencyOccured,
			COALESCE(DELQ.CUR_DLQ, 0) as DaysDelinquent,
			COALESCE(FEE.FEE_WAV_DOL, 0) as TotalLateFeesWaived,
			COALESCE(FEE.FEE_WAV_CT, 0) as TotalNumberOfTimesLateFeesHaveBeenWaived,
			COALESCE(VIPSS,'N') AS VipAlertIndicator,
			COALESCE(SPHAN,'N') AS SpecialHandlingAlertIndicator,
			COALESCE(M1411.LX_ATY, '') as Info411,
			COALESCE(DL200.DL200, 0) as NumberOf20DayLettersSent,
			COALESCE(REHAB.LD_LON_RHB_PCV, '') as Rehabilitated,
			COALESCE(REHAB.REHAB_IND, 'N') AS RehabilitatedAlertIndicator,
			COALESCE(PDISB.PendDisb, 'N') AS HasPendingDisbursement,			
			COALESCE(RM31.LA_BR_RMT_PST, 0.00) AS PaymentsInSuspense

	FROM	dbo.PD10_Borrower PD10
			
			LEFT OUTER JOIN	dbo.BORR_Repayment REPAY ON
				PD10.DF_SPE_ACC_ID = REPAY.DF_SPE_ACC_ID
			LEFT OUTER JOIN dbo.BORR_Bill BILL ON
				PD10.DF_SPE_ACC_ID = BILL.DF_SPE_ACC_ID
			LEFT OUTER JOIN dbo.BORR_Summaries SUMM ON
				PD10.DF_SPE_ACC_ID = SUMM.DF_SPE_ACC_ID
			LEFT OUTER JOIN dbo.BORR_AmountDue DUE ON
				PD10.DF_SPE_ACC_ID = DUE.DF_SPE_ACC_ID

			LEFT OUTER JOIN dbo.BORR_LastPayment LPMT ON
				PD10.DF_SPE_ACC_ID = LPMT.DF_SPE_ACC_ID
			LEFT OUTER JOIN dbo.BORR_Delinquency DELQ ON
				PD10.DF_SPE_ACC_ID = DELQ.DF_SPE_ACC_ID			
			LEFT OUTER JOIN dbo.BORR_Hardship FORB ON
				PD10.DF_SPE_ACC_ID = FORB.DF_SPE_ACC_ID	
			LEFT OUTER JOIN dbo.BORR_Endorser ENDR ON
				PD10.DF_SPE_ACC_ID = ENDR.DF_SPE_ACC_ID	
			LEFT OUTER JOIN dbo.AY10_WaiveFee FEE ON
				PD10.DF_SPE_ACC_ID = FEE.DF_SPE_ACC_ID
			LEFT OUTER JOIN dbo.BORR_Rehabilitation REHAB ON
				PD10.DF_SPE_ACC_ID = REHAB.DF_SPE_ACC_ID
			LEFT OUTER JOIN dbo.AY10_20DayLetterSummary DL200 ON
				PD10.DF_SPE_ACC_ID = DL200.DF_SPE_ACC_ID
			LEFT OUTER JOIN  dbo.AY10_M1411 M1411 ON
				PD10.DF_SPE_ACC_ID = M1411.DF_SPE_ACC_ID
				AND M1411.LN_ATY_SEQ = (SELECT	MAX(LN_ATY_SEQ) AS SEQ
										FROM	dbo.AY10_M1411 
										WHERE	DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID)
			LEFT OUTER JOIN dbo.AY10_SpecialHandling VIPSS ON
				PD10.DF_SPE_ACC_ID = VIPSS.DF_SPE_ACC_ID
			LEFT OUTER JOIN dbo.RM31_Suspense RM31 ON
				PD10.DF_SPE_ACC_ID = RM31.DF_SPE_ACC_ID
			LEFT OUTER JOIN dbo.BORR_PendingDisb PDISB ON
				PD10.DF_SPE_ACC_ID = PDISB.DF_SPE_ACC_ID

	WHERE	PD10.BF_SSN = @SSN

	PRINT 'DONE'
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetCompassBorrowerLevelData] TO [Imaging Users]
    AS [dbo];

