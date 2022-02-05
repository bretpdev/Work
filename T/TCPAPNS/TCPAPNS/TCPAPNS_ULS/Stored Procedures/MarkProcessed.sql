CREATE PROCEDURE [tcpapns].[MarkProcessed]
	@FileProcessingId INT,
	@ArcAddProcessingId BIGINT NULL
AS
	UPDATE [tcpapns].[FileProcessing]
	SET
		ProcessedOn = GETDATE(),
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		FileProcessingId = @FileProcessingId
RETURN 0
