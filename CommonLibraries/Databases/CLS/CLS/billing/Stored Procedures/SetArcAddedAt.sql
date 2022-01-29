CREATE PROCEDURE billing.[SetArcAddedAt]
	@PrintProcessingId int
AS
BEGIN
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		billing.PrintProcessing
	SET
		ArcAddedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId
END;
	
	SELECT
		@Time
RETURN 0;