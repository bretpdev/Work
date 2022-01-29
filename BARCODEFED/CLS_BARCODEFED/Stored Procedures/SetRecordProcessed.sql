CREATE PROCEDURE [barcodefed].[SetRecordProcessed]
	@ReturnMailId int
AS
	UPDATE
		[barcodefed].ReturnMail
	SET
		ProcessedAt = GETDATE()
	WHERE
		ReturnMailId = @ReturnMailId
RETURN 0