CREATE PROCEDURE [lnderlettr].[SetQueueClosedAt]
	@LettersId INT
AS
UPDATE
	[lnderlettr].Letters
SET
	QueueClosedAt = GETDATE()
WHERE
	LettersId = @LettersId