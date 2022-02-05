
CREATE PROCEDURE [dbo].[SetLenderUpdateProcessed]
	@LenderUpdateId int,
	@ProcessedBy varchar(100)
AS
	UPDATE 
		LenderUpdates 
	SET
		ProcessedAt = GETDATE(),
		ProcessedBy = @ProcessedBy
	WHERE
		LenderUpdateId = @LenderUpdateId
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetLenderUpdateProcessed] TO [db_executor]
    AS [dbo];

