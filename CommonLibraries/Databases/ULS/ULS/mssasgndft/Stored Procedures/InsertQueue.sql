CREATE PROCEDURE [mssasgndft].[InsertQueue]
	@QueueName varchar(8),
	@FutureDated bit
AS
	INSERT INTO mssasgndft.Queues(QueueName, FutureDated)
	VALUES(@QueueName, @FutureDated)

GRANT EXECUTE
    ON OBJECT::[mssasgndft].[InsertQueue] TO [db_executor]
    AS [dbo];
GO
GRANT EXECUTE
    ON OBJECT::[mssasgndft].[InsertQueue] TO [db_executor]
    AS [dbo];

