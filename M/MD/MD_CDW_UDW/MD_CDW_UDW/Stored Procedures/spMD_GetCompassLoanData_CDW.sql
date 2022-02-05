CREATE PROCEDURE [dbo].[spMD_GetCompassLoanData]
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT LN10.LN_SEQ as LoanSeqNum,
			LN10.IC_LON_PGM as LoanType,
			LN10.LD_LON_1_DSB as FirstDisbursementDate,
			LN10.LA_LON_AMT_GTR as OriginalBalance,
			LN10.LA_CUR_PRI as CurrentPrincipal,
			coalesce(LN72.LR_ITR,0.000) as InterestRate,
			coalesce(LN65.TYP_SCH_DIS,'') as RepaymentType,
			coalesce(DW01.DW_LON_STA,'') as [Status],
			coalesce(DW01.WC_DW_LON_STA,'') as StatusCode,
			coalesce(SD10.LD_SCL_SPR,'') as SeparationDate,
			LN10.LD_END_GRC_PRD as GraceEndDate,
			coalesce(DW01.WD_LON_RPD_SR,'') as RepaymentStartDate,
			coalesce(LN65.LD_SNT_RPD_DIS,'') as Disclosure,
			coalesce(SD10.DOE_SCL_ENR_CUR,'') as SchoolName,
			coalesce(BILL.BIL_MTD,'') as BillMethod, 
			coalesce(LN65.LN_RPS_TRM,0) as RepaymentTerm,
			coalesce(LN65.DUE_DAY,'') as DueDate,
			coalesce(LN72.LR_INT_RDC_PGM_ORG,0.000) as StatutoryInterestRate, --
			coalesce(LN55.LN_BBS_STS_PCV_PAY,0) as OnTimeMontlyPayments,
			coalesce(LN55.PN_BBT_DLQ_MOT,0) as RequiredOnTimePayments,
			coalesce(LN65.LD_CRT_LON65,'') as RPSDate,
			coalesce(BILL.PAID_AHEAD,'N') as PaidAhead,
			coalesce(LN72.LR_INT_RDC_PGM_ORG,0.000) as RegInt, --
			coalesce(LN83.LC_STA_LN83,'') as OnACH, --A = Y, anything else = N
			CASE
				WHEN coalesce(BR30.BN_EFT_NSF_CTR,0) >= 2 THEN 'N' 
				ELSE 'Y'
			END as ACHEligible,
			
			coalesce(LN55.PR_EFT_RIR,0.000) AS ACHRate,
			coalesce(LN55.RIR_CT,'') as RIRCount,
			RIR.RIR_INT as RIRInt,
			RIR.RIR_TYP as RIRType,
			'NA' AS HEP,
			coalesce(LN55.LC_BBS_ELG,'') as RIREligibility,
			coalesce(LN55.LD_BBS_DSQ,'') as RIREligibilityDate
	FROM dbo.LN10_Loan LN10
		LEFT OUTER JOIN dbo.LN72_InterestRate LN72
			ON LN10.DF_SPE_ACC_ID = LN72.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN72.LN_SEQ
		LEFT OUTER JOIN dbo.LN65_RepaymentSched LN65
			ON LN10.DF_SPE_ACC_ID = LN65.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN65.LN_SEQ
		LEFT OUTER JOIN dbo.DW01_Loan DW01
			ON LN10.DF_SPE_ACC_ID = DW01.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = DW01.LN_SEQ
		LEFT OUTER JOIN dbo.SD10_School SD10
			ON LN10.DF_SPE_ACC_ID = SD10.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = SD10.LN_SEQ		
		LEFT OUTER JOIN dbo.LOAN_Bill BILL
			ON LN10.DF_SPE_ACC_ID = BILL.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = BILL.LN_SEQ
		LEFT OUTER JOIN dbo.LN55_Benefit LN55
			ON LN10.DF_SPE_ACC_ID = LN55.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN55.LN_SEQ
		LEFT OUTER JOIN dbo.LN83_EFTStatus LN83
			ON LN10.DF_SPE_ACC_ID = LN83.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN83.LN_SEQ
		LEFT OUTER JOIN dbo.View_RIR RIR
			ON LN10.DF_SPE_ACC_ID = RIR.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = RIR.LN_SEQ	
		LEFT OUTER JOIN dbo.BR30_Autopay BR30
			ON LN10.DF_SPE_ACC_ID = BR30.DF_SPE_ACC_ID
	
			



	WHERE LN10.DF_SPE_ACC_ID = @AccountNumber
END
