CREATE PROCEDURE [lnderlettr].[SetQueueClosedAt]
	@LettersId INT
AS
	UPDATE
		[lnderlettr].Letters
	SET
		ArcAddedAt = GETDATE()
	WHERE
		LettersId = @LettersId
GO
GRANT EXECUTE
    ON OBJECT::[lnderlettr].[SetQueueClosedAt] TO [db_executor]
    AS [dbo];

