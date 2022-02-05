CREATE PROCEDURE [print].[SetArcAddedAt]
	@PrintProcessingId int
AS
BEGIN
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		[print].PrintProcessing
	SET
		ArcAddedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId
END;
	
	SELECT
		@Time
RETURN 0;