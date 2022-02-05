CREATE PROCEDURE [peps].[UpdateOtherAddressProcessed]
		@RecordId bigint
AS
	UPDATE
		[peps].OTHERADD
	SET
		ProcessedAt = GETDATE()
	WHERE
		OTHERADD_ID = @RecordId
RETURN 0
