
CREATE PROCEDURE spQSTA_GetErrorNoPopulate
	@RunDateTime			datetime	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT	QueueName 
	FROM	QSTA_LST_QueueDetail 
			JOIN QSTA_DAT_QueueData ON 
				QSTA_LST_QueueDetail.QueueName = QSTA_DAT_QueueData.Queue 
	WHERE	QSTA_LST_QueueDetail.BusinessUnit IS NULL
			AND QSTA_DAT_QueueData.RunDateTime = @RunDateTime 
			AND QSTA_LST_QueueDetail.SystemQInd = 'N'
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spQSTA_GetErrorNoPopulate] TO [db_executor]
    AS [dbo];

