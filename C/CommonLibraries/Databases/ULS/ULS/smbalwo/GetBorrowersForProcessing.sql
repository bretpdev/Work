CREATE PROCEDURE [smbalwo].[GetBorrowersForProcessing]

AS
	SELECT 
		DF_SPE_ACC_ID
	FROM
		ULS.[smbalwo].LoanWriteOffS LWO
	WHERE
		LWO.ProcessedAt IS NULL
		AND LWO.DeletedAt IS NULL
RETURN 0
