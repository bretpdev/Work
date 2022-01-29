
CREATE PROCEDURE [mab].[GetQueueList]
AS
	SELECT
		QueueId,
		QueueName,
		FutureDated
	FROM
		CollQueue
	WHERE
		DeletedAt IS NULL

GRANT EXECUTE ON [mab].[GetQueueList] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[mab].[GetQueueList] TO [db_executor]
    AS [dbo];

