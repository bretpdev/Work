CREATE PROCEDURE [smbalwo].[GetLoansForBorrower]
	@DF_SPE_ACC_ID VARCHAR(10)
AS
	SELECT
		LWO.LoanWriteOffId,
		LWO.LN_SEQ,
		lwo.IsDelinquent
	FROM
		CLS.smbalwo.LoanWriteOffs LWO
	WHERE
		LWO.DF_SPE_ACC_ID = @DF_SPE_ACC_ID
		AND LWO.ProcessedAt IS NULL
		AND LWO.DeletedAt IS NULL
RETURN 0
