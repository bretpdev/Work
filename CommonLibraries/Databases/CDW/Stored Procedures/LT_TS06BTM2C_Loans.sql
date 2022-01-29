CREATE PROCEDURE [dbo].[LT_TS06BTM2C_Loans]
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
		LN10.LA_LON_AMT_GTR AS  [Original Principal Balance],
		LN10.LA_CUR_PRI AS [Current Principal Balance]
	FROM 
		LN10_Loan LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
		JOIN LN90_FinancialHistory LN90
			ON LN10.DF_SPE_ACC_ID = LN90.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN90.LN_SEQ
			AND LN90.PC_FAT_TYP = '10'
			AND LN90.PC_FAT_SUB_TYP = '81'
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BTM2C_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BTM2C_Loans] TO [db_executor]
    AS [dbo];
