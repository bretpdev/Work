CREATE PROCEDURE [qstatsextr].[GetQueuesWithBlankManager]
AS
	
	SELECT 
		QueueName 
	FROM 
		QSTA_LST_QueueDetail
	WHERE
		BusinessUnit = '' OR BusinessUnit IS NULL


RETURN 0
