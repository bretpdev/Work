CREATE PROCEDURE [peps].[UpdateContactProcessed]
	@RecordId bigint
AS
	UPDATE
		[peps].CONTACT
	SET
		ProcessedAt = GETDATE()
	WHERE
		CONTACT_ID = @RecordId
RETURN 0
