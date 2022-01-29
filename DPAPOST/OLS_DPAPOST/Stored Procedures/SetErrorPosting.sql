CREATE PROCEDURE [dpapost].[SetErrorPosting]
	@PostingDataId INT
AS
	UPDATE
		dpapost.PostingData
	SET
		ErrorPosting = 1
	WHERE
		PostingDataId = @PostingDataId