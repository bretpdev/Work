CREATE PROCEDURE [dbo].[LT_TS04BDLDSQ_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		FMT.Label AS [Loan Program],
		LN10.LD_LON_1_DSB AS [First Disbursement Date],
		LN10.LA_CUR_PRI AS [Current Loan Balance],
		RP02.PA_BBT_REB AS [Rebate Amount Lost]
	FROM 
		LN10_Loan LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
		JOIN LN54_BenefitProgram LN54
			ON LN10.DF_SPE_ACC_ID = LN54.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN54.LN_SEQ
		JOIN RP02_BenefitProgramTiers RP02
			ON LN54.PM_BBS_PGM = RP02.PM_BBS_PGM
			AND LN54.PN_BBS_PGM_SEQ = RP02.PN_BBS_PGM_SEQ
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([LT_TS04BDLDSQ_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS04BDLDSQ_Loans] TO [db_executor]
    AS [dbo];
