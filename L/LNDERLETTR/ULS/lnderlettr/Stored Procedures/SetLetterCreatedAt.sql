CREATE PROCEDURE [lnderlettr].[SetLetterCreatedAt]
	@LettersId INT
AS
UPDATE
	[lnderlettr].Letters
SET
	LetterCreatedAt = GETDATE()
WHERE
	LettersId = @LettersId