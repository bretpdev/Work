
CREATE PROCEDURE [bbreininsc].[SetProcessedAt]
	@RecordId INT
AS
BEGIN
	UPDATE 
		[ULS].[bbreininsc].[ReinstatementProcessing]
	SET
		ProcessedAt = GETDATE()
	WHERE
		RecordId = @RecordId
END