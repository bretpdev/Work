CREATE PROCEDURE [peps].[UpdateDetailProcessed]
	@RecordId bigint
AS
	UPDATE
		[peps].DETAIL
	SET
		ProcessedAt = GETDATE()
	WHERE
		DETAIL_ID = @RecordId
RETURN 0
