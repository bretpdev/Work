CREATE PROCEDURE [dbo].[UpdateProcessedAt]
(
	@ArcAddProcessingId INT,
	@NobleCallHistoryId INT
)
AS
	UPDATE
		NobleCallHistory
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		NobleCallHistoryId = @NobleCallHistoryId
