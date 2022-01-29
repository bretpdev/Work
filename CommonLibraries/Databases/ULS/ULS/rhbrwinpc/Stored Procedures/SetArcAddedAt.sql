CREATE PROCEDURE [rhbrwinpc].[SetArcAddedAt]
	@LettersId INT
AS
	UPDATE
		[rhbrwinpc].Letters
	SET
		ArcAddedAt = GETDATE()
	WHERE
		LettersId = @LettersId
GO
GRANT EXECUTE
    ON OBJECT::[rhbrwinpc].[SetArcAddedAt] TO [db_executor]
    AS [dbo];

