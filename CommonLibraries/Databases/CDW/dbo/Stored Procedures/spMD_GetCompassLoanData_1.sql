

CREATE  PROCEDURE [dbo].[spMD_GetCompassLoanData]
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @Ssn char(9)
	select @Ssn = BF_SSN from PD10_Borrower where DF_SPE_ACC_ID = @AccountNumber

	declare @Payments as table(
		LoanSequence int,
		OnTimeMonthlyPayments varchar(max),
		RequiredOnTimePayments varchar(max),
		RirEligibility varchar(max),
		RirEligibilityDate varchar(max)
	)
	declare @AchRates as table(
		LoanSequence int,
		AchRate decimal(5, 3)
	)

	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RP30_EFT_RIR_PAR'))
	BEGIN
		insert into @AchRates
		select 
			LN_SEQ, 
			PR_EFT_RIR
		from dbo.LN10_Lon LN10 
		join dbo.RP30_EFT_RIR_PAR RP30 on LN10.LF_LON_CUR_OWN = RP30.IF_OWN
		where BF_SSN = @Ssn and PC_EFT_RIR_STA = 'A'
	END


	IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = 'LN_BBS_STS_PCV_PAY' AND Object_ID = Object_ID('LN55_Benefit'))
	BEGIN
		
		insert into @Payments (LoanSequence, OnTimeMonthlyPayments, RequiredOnTimePayments, RirEligibility, RirEligibilityDate)
		select x.*
		FROM (select Null as LN_BBS_STS_PCV_PAY, NULL as PN_BBT_DLQ_MOT, NULL as LC_BBS_ELG, NULL as LD_BBS_DSQ) as dummy --fix to allow us to select columns that don't exist in both CDW and UDW
		CROSS APPLY(
		select
			LN10.LN_Seq, LN_BBS_STS_PCV_PAY, PN_BBT_DLQ_MOT, LC_BBS_ELG, LD_BBS_DSQ
		from 
			dbo.LN10_Loan LN10 
		LEFT OUTER JOIN dbo.LN55_Benefit LN55
			ON LN10.DF_SPE_ACC_ID = LN55.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN55.LN_SEQ
		where LN10.DF_SPE_ACC_ID = @AccountNumber
		) as x
	END

	IF EXISTS(SELECT * FROM sys.columns 
        WHERE Name = 'LN_LON_BBT_PAY' AND Object_ID = Object_ID('LN55_Benefit'))
	BEGIN
		insert into @Payments (LoanSequence, OnTimeMonthlyPayments, RequiredOnTimePayments, RirEligibility, RirEligibilityDate)
		select 
			LN10.LN_Seq, LN_LON_BBT_PAY, PN_BBT_PAY_ICV, LC_BBS_ELG, LD_BBS_DSQ
		from 
			dbo.LN10_Loan LN10 
		LEFT OUTER JOIN dbo.LN55_Benefit LN55
			ON LN10.DF_SPE_ACC_ID = LN55.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN55.LN_SEQ
		LEFT OUTER JOIN dbo.LN54_Eligibility LN54
			ON LN10.DF_SPE_ACC_ID = LN54.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN54.LN_SEQ
		where LN10.DF_SPE_ACC_ID = @AccountNumber
	END

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
			coalesce(LN55.OnTimeMonthlyPayments,0) as OnTimeMontlyPayments,
			coalesce(LN55.RequiredOnTimePayments,0) as RequiredOnTimePayments,
			coalesce(LN65.LD_CRT_LON65,'') as RPSDate,
			coalesce(BILL.PAID_AHEAD,'N') as PaidAhead,
			coalesce(LN72.LR_INT_RDC_PGM_ORG,0.000) as RegInt, --
			coalesce(LN83.LC_STA_LN83,'') as OnACH, --A = Y, anything else = N
			CASE
				WHEN coalesce(BR30.BN_EFT_NSF_CTR,0) >= 2 THEN 'N' 
				ELSE 'Y'
			END as ACHEligible,
			coalesce(ACH.AchRate,0.000) AS ACHRate,
			cast(dbo.GetLoanRirCount(@AccountNumber, LN10.LN_SEQ) as nvarchar(max)) as RIRCount,
			cast(dbo.GetLoanRirInt(@AccountNumber, LN10.LN_SEQ) as nvarchar(max)) as RIRInt,
			dbo.GetLoanRirType(@AccountNumber, LN10.LN_SEQ) as RIRType,
			'NA' AS HEP,
			coalesce(LN55.RirEligibility, '') as RIREligibility,
			coalesce(LN55.RirEligibilityDate,'') as RIREligibilityDate
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
		LEFT OUTER JOIN dbo.LN83_EFTStatus LN83
			ON LN10.DF_SPE_ACC_ID = LN83.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN83.LN_SEQ
		LEFT OUTER JOIN dbo.BR30_Autopay BR30
			ON LN10.DF_SPE_ACC_ID = BR30.DF_SPE_ACC_ID
		LEFT OUTER JOIN @Payments LN55
			ON LN10.LN_SEQ = LN55.LoanSequence
		LEFT OUTER JOIN @AchRates ACH
			ON ACH.LoanSequence = LN10.LN_SEQ

	WHERE LN10.DF_SPE_ACC_ID = @AccountNumber
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetCompassLoanData] TO [Imaging Users]
    AS [dbo];

