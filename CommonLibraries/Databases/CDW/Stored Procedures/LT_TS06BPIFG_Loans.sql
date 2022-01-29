CREATE PROCEDURE [dbo].[LT_TS06BPIFG_Loans]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		FMT.Label AS [LoanProgram],
		LN10.LD_LON_1_DSB AS [DisbDate],
		LN10.LA_CUR_PRI AS [CurrentPrin],
		LN72.LR_ITR AS [InterestRate]
	FROM
		LN10_Loan LN10
		JOIN FormatTranslation FMT
			ON LN10.IC_LON_PGM = FMT.Start
		JOIN LN72_InterestRate LN72
			ON LN10.DF_SPE_ACC_ID = LN72.DF_SPE_ACC_ID
			AND LN10.LN_SEQ = LN72.LN_SEQ
	WHERE 
		LN10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([dbo].[LT_TS06BPIFG_Loans])',11,2)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_TS06BPIFG_Loans] TO [db_executor]
    AS [dbo];
