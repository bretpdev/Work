CREATE PROCEDURE [lnderlettr].[SetArcAddedAt]
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
    ON OBJECT::[lnderlettr].[SetArcAddedAt] TO [db_executor]
    AS [dbo];

