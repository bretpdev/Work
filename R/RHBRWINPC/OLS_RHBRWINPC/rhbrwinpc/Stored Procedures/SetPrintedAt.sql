CREATE PROCEDURE [rhbrwinpc].[SetPrintedAt]
	@LettersId INT
AS
	UPDATE
		[rhbrwinpc].Letters
	SET
		PrintedAt = GETDATE()
	WHERE
		LettersId = @LettersId