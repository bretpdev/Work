CREATE PROCEDURE [peps].[UpdateProgramProcessed]
		@RecordId bigint
AS
	UPDATE
		[peps].PROGRAM
	SET
		ProcessedAt = GETDATE()
	WHERE
		PROGRAM_ID = @RecordId
RETURN 0
