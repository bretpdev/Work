CREATE PROCEDURE [dpapost].[SetProcessed]
	@PostingDataId INT
AS
	UPDATE
		dpapost.PostingData
	SET
		ProcessedAt = GETDATE()
	WHERE
		PostingDataId = @PostingDataId