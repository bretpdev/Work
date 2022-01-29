CREATE PROCEDURE [dbo].[LT_TS06BD6015_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		ln10.IC_LON_PGM AS [Loan Program],
		LN10.LD_LON_1_DSB AS [First Disbursement Date],
		LN10.LA_LON_AMT_GTR AS [Original Principal],
		LN10.LA_CUR_PRI AS [Current Principal Balance],
		DF10.LD_DFR_END AS [Deferment End Date]
	FROM
		LN10_Loan LN10
		--JOIN FormatTranslation FMT
		--	ON LN10.IC_LON_PGM = FMT.Start
		JOIN DF10_Deferment DF10
			ON LN10.DF_SPE_ACC_ID = DF10.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = DF10.LN_SEQ
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
		AND GETDATE() BETWEEN CAST(DF10.LD_DFR_BEG AS DATE) AND CAST(DF10.LD_DFR_END AS DATE)
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BD6015_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BD6015_Loans] TO [db_executor]
    AS [dbo];
