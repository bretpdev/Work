CREATE PROCEDURE [lnderlettr].[SetLetterCreatedAt]
	@LettersId INT
AS
	UPDATE
		[lnderlettr].Letters
	SET
		LetterCreatedAt = GETDATE()
	WHERE
		LettersId = @LettersId
GO
GRANT EXECUTE
    ON OBJECT::[lnderlettr].[SetLetterCreatedAt] TO [db_executor]
    AS [dbo];

