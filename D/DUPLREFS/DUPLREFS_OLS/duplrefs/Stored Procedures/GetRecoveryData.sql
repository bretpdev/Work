CREATE PROCEDURE [duplrefs].[GetRecoveryData]
	@AccountNumber VARCHAR(10)

AS

	--This script indicates if the user did not finish processing this borrower 
	SELECT 
		BQ.BorrowerQueueId,
		BQ.CreatedAt,
		BQ.UserId
	FROM
		duplrefs.BorrowerQueue BQ
	WHERE
		BQ.AccountNumber = @AccountNumber
		AND BQ.ProcessedAt IS NULL
		AND BQ.DeletedAt IS NULL

RETURN 0
