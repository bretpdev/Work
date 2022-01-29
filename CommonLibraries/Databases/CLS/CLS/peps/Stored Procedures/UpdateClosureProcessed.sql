CREATE PROCEDURE [peps].[UpdateClosureProcessed]
	@RecordId bigint
AS
	UPDATE
		[peps].CLOSURE
	SET
		ProcessedAt = GETDATE()
	WHERE
		CLOSURE_ID = @RecordId
RETURN 0
