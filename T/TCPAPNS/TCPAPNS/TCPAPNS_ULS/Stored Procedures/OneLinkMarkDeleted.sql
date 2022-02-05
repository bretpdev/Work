CREATE PROCEDURE [tcpapns].[OneLinkMarkDeleted]
	@FileProcessingId INT
AS
	UPDATE [tcpapns].[OneLinkFileProcessing]
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_NAME()
	WHERE
		FileProcessingId = @FileProcessingId
RETURN 0