CREATE PROCEDURE [lnderlettr].[SetErroredAt]
	@LetterId INT
AS
UPDATE
	[lnderlettr].Letters
SET
	ErroredAt = GETDATE()
WHERE
	LettersId = @LetterId
