
CREATE PROCEDURE [mab].[InsertQueue]
	@QueueName varchar(8),
	@FutureDated bit
AS
	INSERT INTO CollQueue(QueueName, FutureDated)
	VALUES(@QueueName, @FutureDated)

GRANT EXECUTE ON [mab].[InsertQueue] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[mab].[InsertQueue] TO [db_executor]
    AS [dbo];

