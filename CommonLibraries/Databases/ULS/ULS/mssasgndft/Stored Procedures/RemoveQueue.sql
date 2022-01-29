CREATE PROCEDURE [mssasgndft].[RemoveQueue]
	@QueueId varchar(8)
AS
	UPDATE
		mssasgndft.Queues
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SYSTEM_USER
	WHERE
		QueueId = @QueueId

GRANT EXECUTE ON [mssasgndft].[RemoveQueue] TO db_executor