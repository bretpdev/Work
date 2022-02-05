CREATE PROCEDURE [print].[SetPrintedAt]
	@PrintProcessingId int
AS
BEGIN
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		[print].PrintProcessing
	SET
		PrintedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId
END;

	SELECT
		@Time
RETURN 0;