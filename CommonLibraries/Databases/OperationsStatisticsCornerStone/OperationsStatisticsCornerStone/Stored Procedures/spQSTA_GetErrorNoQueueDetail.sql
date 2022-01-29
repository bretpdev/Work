
CREATE PROCEDURE spQSTA_GetErrorNoQueueDetail
	@RunDateTime		datetime
AS
BEGIN
	SET NOCOUNT ON;
	SELECT	[Queue] 
	FROM	QSTA_DAT_QueueData 
	WHERE	RunDateTime = @RunDateTime 
			AND [Queue] NOT IN (SELECT QueueName FROM QSTA_LST_QueueDetail)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spQSTA_GetErrorNoQueueDetail] TO [db_executor]
    AS [dbo];

