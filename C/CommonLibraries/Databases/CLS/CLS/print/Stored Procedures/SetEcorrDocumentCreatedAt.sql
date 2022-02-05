CREATE PROCEDURE [print].[SetEcorrDocumentCreatedAt]
	@PrintProcessingId int
AS
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		[print].PrintProcessing
	SET
		EcorrDocumentCreatedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId

	SELECT
		@Time
RETURN 0
GO


