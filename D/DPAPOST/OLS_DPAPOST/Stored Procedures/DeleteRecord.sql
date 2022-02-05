CREATE PROCEDURE [dpapost].[DeleteRecord]
	@PostingDataId INT
AS
	UPDATE
		dpapost.PostingData
	SET
		DeletedAt = GETDATE(),
		DeletedBy = USER_NAME()
	WHERE
		PostingDataId = @PostingDataId