CREATE PROCEDURE [tcpapns].[OneLinkMarkProcessed]
	@FileProcessingId INT
AS
	UPDATE [tcpapns].[OneLinkFileProcessing]
	SET
		ProcessedOn = GETDATE()
	WHERE
		FileProcessingId = @FileProcessingId
RETURN 0