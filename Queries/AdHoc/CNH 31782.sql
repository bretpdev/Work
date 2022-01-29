USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[spMD_GetCompassLoanData]    Script Date: X/XX/XXXX XX:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spMD_GetCompassLoanData]
	@AccountNumber				VARCHAR(XX)
AS
BEGIN

    SELECT 
		LNXX.LN_SEQ as LoanSeqNum,
		LNXX.IC_LON_PGM as LoanType,
		LNXX.LD_LON_X_DSB as FirstDisbursementDate,
		LNXX.LA_LON_AMT_GTR as OriginalBalance,
		LNXX.LA_CUR_PRI as CurrentPrincipal,
		coalesce(LNXX.LR_ITR,X.XXX) as InterestRate,
		coalesce(LNXX.TYP_SCH_DIS,'') as RepaymentType,
		coalesce(DWXX.DW_LON_STA,'') as [Status],
		coalesce(DWXX.WC_DW_LON_STA,'') as StatusCode,
		coalesce(SDXX.LD_SCL_SPR,'') as SeparationDate,
		LNXX.LD_END_GRC_PRD as GraceEndDate,
		coalesce(DWXX.WD_LON_RPD_SR,'') as RepaymentStartDate,
		coalesce(LNXX.LD_SNT_RPD_DIS,'') as Disclosure,
		coalesce(SDXX.DOE_SCL_ENR_CUR,'') as SchoolName,
		coalesce(BILL.BIL_MTD,'') as BillMethod, 
		coalesce(LNXX.LN_RPS_TRM,X) as RepaymentTerm,
		coalesce(LNXX.DUE_DAY,'') as DueDate,
		coalesce(LNXX.LR_INT_RDC_PGM_ORG,X.XXX) as StatutoryInterestRate, --
		coalesce(LNXX.LN_BBS_STS_PCV_PAY,X) as OnTimeMontlyPayments,
		coalesce(LNXX.PN_BBT_DLQ_MOT,X) as RequiredOnTimePayments,
		coalesce(LNXX.LD_CRT_LONXX,'') as RPSDate,
		coalesce(BILL.PAID_AHEAD,'N') as PaidAhead,
		coalesce(LNXX.LR_INT_RDC_PGM_ORG,X.XXX) as RegInt, --
		coalesce(LNXX.LC_STA_LNXX,'') as OnACH, --A = Y, anything else = N
		CASE
			WHEN coalesce(BRXX.BN_EFT_NSF_CTR,X) >= X THEN 'N' 
			ELSE 'Y'
		END as ACHEligible,
			
		coalesce(LNXX.PR_EFT_RIR,X.XXX) AS ACHRate,
		coalesce(LNXX.RIR_CT,'') as RIRCount,
		RIR.RIR_INT as RIRInt,
		RIR.RIR_TYP as RIRType,
		'NA' AS HEP,
		coalesce(LNXX.LC_BBS_ELG,'') as RIREligibility,
		coalesce(LNXX.LD_BBS_DSQ,'') as RIREligibilityDate
	FROM
		dbo.PDXX_PRS_NME PDXX 
		INNER JOIN dbo.LNXX_Lon LNXX ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		LEFT OUTER JOIN dbo.LNXX_InterestRate LNXX
			ON PDXX.DF_SPE_ACC_ID = LNXX.DF_SPE_ACC_ID
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT OUTER JOIN dbo.LNXX_RepaymentSched LNXX
			ON PDXX.DF_SPE_ACC_ID = LNXX.DF_SPE_ACC_ID
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT OUTER JOIN dbo.DWXX_Loan DWXX
			ON PDXX.DF_SPE_ACC_ID = DWXX.DF_SPE_ACC_ID
			AND LNXX.LN_SEQ = DWXX.LN_SEQ
		LEFT OUTER JOIN dbo.SDXX_School SDXX
			ON PDXX.DF_SPE_ACC_ID = SDXX.DF_SPE_ACC_ID
			AND LNXX.LN_SEQ = SDXX.LN_SEQ		
		LEFT OUTER JOIN dbo.LOAN_Bill BILL
			ON PDXX.DF_SPE_ACC_ID = BILL.DF_SPE_ACC_ID
			AND LNXX.LN_SEQ = BILL.LN_SEQ
		LEFT OUTER JOIN dbo.LNXX_Benefit LNXX
			ON PDXX.DF_SPE_ACC_ID = LNXX.DF_SPE_ACC_ID
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT OUTER JOIN dbo.LNXX_EFTStatus LNXX
			ON PDXX.DF_SPE_ACC_ID = LNXX.DF_SPE_ACC_ID
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT OUTER JOIN dbo.View_RIR RIR
			ON PDXX.DF_SPE_ACC_ID = RIR.DF_SPE_ACC_ID
			AND LNXX.LN_SEQ = RIR.LN_SEQ	
		LEFT OUTER JOIN dbo.BRXX_Autopay BRXX
			ON PDXX.DF_SPE_ACC_ID = BRXX.DF_SPE_ACC_ID

	WHERE PDXX.DF_SPE_ACC_ID = @AccountNumber
END
