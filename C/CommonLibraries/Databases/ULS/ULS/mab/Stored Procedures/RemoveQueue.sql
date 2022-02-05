
CREATE PROCEDURE [mab].[RemoveQueue]
	@QueueId varchar(8)
AS
	UPDATE
		CollQueue
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SYSTEM_USER
	WHERE
		QueueId = @QueueId

GRANT EXECUTE ON [mab].[RemoveQueue] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[mab].[RemoveQueue] TO [db_executor]
    AS [dbo];

