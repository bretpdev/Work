CREATE PROCEDURE [qstatsextr].[GetQueuesWithLateDays]
AS
	
	SELECT 
		QueueName, 
		NumOfDaysLateTask 
	FROM 
		QSTA_LST_QueueDetail 
	WHERE 
		NumOfDaysLateTask IS NOT NULL

RETURN 0
