CREATE PROCEDURE [curracclat].[SetProcessedAt]
	@ProcessDataId INT
AS
	UPDATE
		[curracclat].ProcessData
	SET
		ProcessedAt = GETDATE()
	WHERE
		ProcessDataId = @ProcessDataId