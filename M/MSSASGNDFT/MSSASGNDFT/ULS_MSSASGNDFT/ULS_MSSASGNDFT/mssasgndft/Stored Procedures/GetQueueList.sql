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