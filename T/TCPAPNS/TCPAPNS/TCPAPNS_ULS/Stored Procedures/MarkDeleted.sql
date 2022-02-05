CREATE PROCEDURE [tcpapns].[MarkDeleted]
	@FileProcessingId INT
AS
	UPDATE [tcpapns].[FileProcessing]
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_NAME()
	WHERE
		FileProcessingId = @FileProcessingId
RETURN 0