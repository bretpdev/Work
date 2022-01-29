CREATE PROCEDURE [duedtecng].[MarkDueDateAsProcessed]
	@DueDateChangeId INT,
	@ManualReviewNeeded BIT
AS

	UPDATE
		duedtecng.DueDateChange
	SET
		ProcessedAt = GETDATE(), ProcessedBy = SYSTEM_USER, ManualReviewNeeded = @ManualReviewNeeded
	WHERE
		DueDateChangeId = @DueDateChangeId

RETURN 0
