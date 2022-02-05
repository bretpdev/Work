CREATE PROCEDURE [qstatsextr].[GetQueuesWithNoDescription]
AS
	
	SELECT 
		QueueName 
	FROM 
		QSTA_LST_QueueDetail
	WHERE
		QueueDesc = '' or QueueDesc IS NULL

RETURN 0
