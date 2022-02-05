CREATE PROCEDURE [dbo].[LT_TS06BTM2LM_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		FMT.Label AS [Loan Program],
		LN10.LD_LON_1_DSB AS [Disbursement Date],
		LN10.LA_LON_AMT_GTR AS  [Original Principal Balance]
	FROM 
		LN10_Loan LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber AND LN10.LA_CUR_PRI <= 0.00
		AND LN10.LD_PIF_RPT = (SELECT MAX(cast(LN10date.LD_PIF_RPT as datetime)) 
							FROM LN10_Loan LN10date 
							WHERE LN10date.DF_SPE_ACC_ID = LN10.DF_SPE_ACC_ID)
		
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned.  ([dbo].[LT_TS06BTM2LM_Loans])',11,2)
	END