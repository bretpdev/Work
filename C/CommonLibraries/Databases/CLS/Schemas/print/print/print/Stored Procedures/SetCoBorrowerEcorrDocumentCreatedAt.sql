CREATE PROCEDURE [print].[SetCoBorrowerEcorrDocumentCreatedAt]
	@PrintProcessingId int
AS
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		[print].PrintProcessingCoBorrower
	SET
		EcorrDocumentCreatedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId

	SELECT
		@Time
RETURN 0