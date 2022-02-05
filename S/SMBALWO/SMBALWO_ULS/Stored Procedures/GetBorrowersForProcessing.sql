CREATE PROCEDURE [smbalwo].[GetBorrowersForProcessing]
	@TILPOnly BIT
AS
	SELECT
		LWO.LoanWriteOffId,
		LWO.DF_SPE_ACC_ID [AccountNumber],
		LWO.LN_SEQ [LoanSequence],
		LWO.IsTilp
	FROM
		ULS.[smbalwo].LoanWriteOffS LWO
	WHERE
		LWO.ProcessedAt IS NULL
		AND LWO.DeletedAt IS NULL
		AND LWO.IsTILP = @TILPOnly
RETURN 0