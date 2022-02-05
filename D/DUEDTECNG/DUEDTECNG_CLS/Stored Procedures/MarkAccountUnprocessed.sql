CREATE PROCEDURE [duedtecng].[MarkAccountUnprocessed]
	@AccountIdentifier VARCHAR(10)
AS

	UPDATE
		duedtecng.DueDateChange
	SET
		ProcessedAt = NULL, ProcessedBy = NULL, ManualReviewNeeded = NULL
	WHERE
		Ssn = @AccountIdentifier
		OR
		AccountNumber = @AccountIdentifier

RETURN 0
