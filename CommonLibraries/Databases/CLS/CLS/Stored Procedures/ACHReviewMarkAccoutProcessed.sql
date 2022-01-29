CREATE PROCEDURE [dbo].[ACHReviewMarkAccoutProcessed]
	@AccountNumber char(10)
AS
	UPDATE
		ACHReview
	SET
		ProcessedOn = GETDATE()
	WHERE
		AccountNumber = @AccountNumber
		AND ProcessedOn IS NULL
RETURN 0
