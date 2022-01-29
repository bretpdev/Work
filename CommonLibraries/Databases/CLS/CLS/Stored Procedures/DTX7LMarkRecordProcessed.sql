CREATE PROCEDURE [dbo].[DTX7LMarkRecordProcessed]
	@DTX7LDeletedRecordId int
	
AS
	UPDATE
		DTX7LDeletedRecords
	SET
		ProcessedAt = GETDATE()
	WHERE
		DTX7LDeletedRecordId = @DTX7LDeletedRecordId
RETURN 0
