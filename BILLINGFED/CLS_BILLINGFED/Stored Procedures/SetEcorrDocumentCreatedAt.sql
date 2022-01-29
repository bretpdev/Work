CREATE PROCEDURE billing.[SetEcorrDocumentCreatedAt]
	@PrintProcessingId int
AS
BEGIN
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		billing.PrintProcessing
	SET
		EcorrDocumentCreatedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId
END;

	SELECT
		@Time
RETURN 0