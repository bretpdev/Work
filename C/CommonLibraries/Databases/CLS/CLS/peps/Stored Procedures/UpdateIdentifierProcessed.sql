CREATE PROCEDURE [peps].[UpdateIdentifierProcessed]
		@RecordId bigint
AS
	UPDATE
		[peps].SCHIDS
	SET
		ProcessedAt = GETDATE()
	WHERE
		SCHIDS_ID = @RecordId
RETURN 0
