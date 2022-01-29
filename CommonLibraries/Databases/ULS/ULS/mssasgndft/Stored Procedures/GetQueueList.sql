CREATE PROCEDURE [mssasgndft].[GetQueueList]
AS
	SELECT
		QueueId,
		QueueName,
		FutureDated
	FROM
		mssasgndft.Queues
	WHERE
		DeletedAt IS NULL

GRANT EXECUTE
    ON OBJECT::[mssasgndft].[GetQueueList] TO [db_executor]
    AS [dbo];
GO
GRANT EXECUTE
    ON OBJECT::[mssasgndft].[GetQueueList] TO [db_executor]
    AS [dbo];

