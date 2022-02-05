CREATE PROCEDURE [dbo].[spMD_GetCompassLoanData_UDW]
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @Ssn char(9)
	select @Ssn = BF_SSN from PD10_Borrower where DF_SPE_ACC_ID = @AccountNumber

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
			coalesce(LN55.LN_LON_BBT_PAY,0) as OnTimeMontlyPayments,
			coalesce(LN55.PN_BBT_PAY_ICV,0) as RequiredOnTimePayments,
			coalesce(LN65.LD_CRT_LON65,'') as RPSDate,
			coalesce(BILL.PAID_AHEAD,'N') as PaidAhead,
			coalesce(LN72.LR_INT_RDC_PGM_ORG,0.000) as RegInt, --
			coalesce(LN83.LC_STA_LN83,'') as OnACH, --A = Y, anything else = N
			CASE
				WHEN coalesce(BR30.BN_EFT_NSF_CTR,0) >= 2 THEN 'N' 
				ELSE 'Y'
			END as ACHEligible,
			coalesce(RP30.PR_EFT_RIR, LN10.ACH_RATE, 0.000) AS ACHRate,
			cast(dbo.GetLoanRirCount(@AccountNumber, LN10.LN_SEQ) as nvarchar(max)) as RIRCount,
			cast(dbo.GetLoanRirInt(@AccountNumber, LN10.LN_SEQ, LN10.IC_LON_PGM) as nvarchar(max)) as RIRInt,
			dbo.GetLoanRirType(@AccountNumber, LN10.LN_SEQ) as RIRType,
			'NA' AS HEP,
			CASE WHEN LN54.LC_BBS_ELG = 'Y' THEN 'Y' ELSE 'N' END as RIREligibility,
			CONVERT(VARCHAR(10), LN54.LD_BBS_DSQ, 101) as RIREligibilityDate
	FROM dbo.LN10_Loan LN10
		JOIN PD10_Borrower BF 
			ON BF.DF_SPE_ACC_ID = LN10.DF_SPE_ACC_ID
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
		LEFT OUTER JOIN dbo.LN83_EFTStatus LN83
			ON LN10.DF_SPE_ACC_ID = LN83.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN83.LN_SEQ
		LEFT OUTER JOIN dbo.BR30_Autopay BR30
			ON LN10.DF_SPE_ACC_ID = BR30.DF_SPE_ACC_ID
		LEFT OUTER JOIN LN54_LON_BBS LN54
			ON LN10.LN_SEQ = LN54.LN_SEQ
			AND LN54.BF_SSN = BF.BF_SSN
			AND LN54.LC_STA_LN54 = 'A'
		LEFT OUTER JOIN dbo.LN55_Benefit LN55
			ON LN10.DF_SPE_ACC_ID = LN55.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN55.LN_SEQ
		LEFT OUTER JOIN LN10_LON LON
			ON LON.BF_SSN = @SSN 
			AND LON.LN_SEQ = LN10.LN_SEQ
		LEFT OUTER JOIN RP30_EFT_RIR_PAR RP30
			ON LON.LF_LON_CUR_OWN = RP30.IF_OWN
			AND RP30.PC_EFT_RIR_STA = 'A'
			AND (RP30.IF_OWN IN ('834396', '830132', '82976901', '82976902', '82976903', '82976904','82976905','82976906', '82976907', '82976908') 
			     OR (RP30.IF_OWN IN ('828476', '834529') AND LON.LD_LON_1_DSB >= RP30.PD_EFT_RIR_EFF_BEG AND LN10.IC_LON_PGM = RP30.IC_LON_PGM AND RP30.PD_EFT_RIR_EFF_BEG = (select top 1 PD_EFT_RIR_EFF_BEG from RP30_EFT_RIR_PAR eft where LN10.IC_LON_PGM = eft.IC_LON_PGM and LON.LD_LON_1_DSB >= eft.PD_EFT_RIR_EFF_BEG order by PD_EFT_RIR_EFF_BEG desc))
				 OR (RP30.IF_OWN IN ('828476', '834529') AND LN10.IC_LON_PGM = RP30.IC_LON_PGM AND RP30.PD_EFT_RIR_EFF_BEG = (select top 1 PD_EFT_RIR_EFF_BEG from RP30_EFT_RIR_PAR eft where LN10.IC_LON_PGM = eft.IC_LON_PGM and LON.LD_LON_1_DSB < eft.PD_EFT_RIR_EFF_BEG order by PD_EFT_RIR_EFF_BEG asc) AND NOT EXISTS(select * from RP30_EFT_RIR_PAR eft where LN10.IC_LON_PGM = eft.IC_LON_PGM and LON.LD_LON_1_DSB >= eft.PD_EFT_RIR_EFF_BEG))
				 OR (RP30.IF_OWN IN ('826717', '834437', '834493', '83449301', '82847601', '829769') AND LN10.IC_LON_PGM = RP30.IC_LON_PGM))

	WHERE LN10.DF_SPE_ACC_ID = @AccountNumber
END
