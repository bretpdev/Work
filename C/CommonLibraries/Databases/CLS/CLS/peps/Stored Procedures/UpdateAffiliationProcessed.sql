CREATE PROCEDURE [peps].[UpdateAffiliationProcessed]
	@RecordId bigint
AS
	UPDATE
		[peps].COA
	SET
		ProcessedAt = GETDATE()
	WHERE
		COA_ID = @RecordId
RETURN 0
