CREATE PROCEDURE [dbo].[UpdateProcessedAt]
	@ArcAddProcessingId int,
	@NobleCallHistoryId int
AS
	UPDATE
		NobleCallHistory
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		NobleCallHistoryId = @NobleCallHistoryId
RETURN 0
