CREATE PROCEDURE [qstatsextr].[GetQueuesWithNoManager]
	@RunTimeDate DATETIME
AS
	
	SELECT 
		[Queue]
	FROM 
		QSTA_DAT_QueueData 
	WHERE 
		RunTimeDate = @RunTimeDate 
		AND 
		[Queue] NOT IN (SELECT QueueName FROM QSTA_LST_QueueDetail)


RETURN 0
