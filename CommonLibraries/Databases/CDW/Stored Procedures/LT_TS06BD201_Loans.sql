CREATE PROCEDURE [dbo].[LT_TS06BD201_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		FMT.Label AS [Loan Program],
		'US Dept of Ed' AS [Current Owner],
		LN10.LD_LON_1_DSB AS [First Disbursement Date],
		LN10.LA_CUR_PRI AS [Current Principal Balance],
		DW01.WD_LON_RPD_SR AS [Repayment Start Date]
	FROM
		LN10_Loan LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
		JOIN DW01_Loan DW01
			ON LN10.DF_SPE_ACC_ID = DW01.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = DW01.LN_SEQ
		JOIN DF10_Deferment DF10
			ON LN10.DF_SPE_ACC_ID = DF10.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = DF10.LN_SEQ
			AND GETDATE() BETWEEN CAST(DF10.LD_DFR_BEG AS DATE) AND CAST(DF10.LD_DFR_END AS DATE)
	WHERE 
		DF10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BD201_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BD201_Loans] TO [db_executor]
    AS [dbo];
