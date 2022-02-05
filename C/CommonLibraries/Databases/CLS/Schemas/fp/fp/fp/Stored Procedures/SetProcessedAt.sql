CREATE PROCEDURE [fp].[SetProcessedAt]
	@FileProcessingId int
AS
	UPDATE
		[fp].FileProcessing
	SET
		ProcessedAt = GETDATE()
	WHERE
		FileProcessingId = @FileProcessingId
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[SetProcessedAt] TO [db_executor]
    AS [dbo];

