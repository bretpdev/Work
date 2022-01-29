CREATE PROCEDURE [qstatsextr].[GetQueuesNeedingDaysLatePopulated]
AS

	SELECT 
		QueueName, NumOfDaysLateTask, SystemIndicator 
	FROM 
		QSTA_LST_QueueDetail 
	WHERE 
		SystemIndicator = 'OneLINK' 
		AND 
		NumOfDaysLateTask IS NULL

RETURN 0
