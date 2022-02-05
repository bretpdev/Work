CREATE PROCEDURE [dbo].[MarkDueDateAsProcessed]
	@DueDateChangeId int

AS
	UPDATE
		DueDateChange
	SET
		ProcessedAt = GetDate()
	WHERE
		DueDateChangeId = @DueDateChangeId
RETURN 0
