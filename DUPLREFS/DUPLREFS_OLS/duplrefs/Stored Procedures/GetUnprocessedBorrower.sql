CREATE PROCEDURE [duplrefs].[GetUnprocessedBorrower]
	@BorrowerQueueId INT
	
AS
	
	SELECT 
		BQ.BorrowerQueueId,
		BQ.AccountNumber,
		BQ.UserId
	FROM
		duplrefs.BorrowerQueue BQ
	WHERE
		BQ.BorrowerQueueId = @BorrowerQueueId
		AND BQ.ProcessedAt IS NULL
		AND BQ.DeletedAt IS NULL

RETURN 0
