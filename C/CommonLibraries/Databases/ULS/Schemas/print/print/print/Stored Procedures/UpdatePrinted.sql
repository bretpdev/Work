CREATE PROCEDURE [print].[UpdatePrinted]
	@PrintProcessingIds		[print].[PrintProcessIdsTable] READONLY
AS
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		[print].PrintProcessing
	SET
		PrintedAt = @Time
	WHERE
		PrintProcessingId IN (SELECT PrintProcessingId FROM @PrintProcessingIds)
RETURN 0
