CREATE PROCEDURE [rhbrwinpc].[SetPrintedAt]
	@LettersId INT
AS
	UPDATE
		[rhbrwinpc].Letters
	SET
		PrintedAt = GETDATE()
	WHERE
		LettersId = @LettersId
GO
GRANT EXECUTE
    ON OBJECT::[rhbrwinpc].[SetPrintedAt] TO [db_executor]
    AS [dbo];

