
CREATE PROCEDURE spQSTA_GetErrorBuMissing
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT	QueueName 
	FROM	QSTA_LST_QueueDetail 
	WHERE	BusinessUnit IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spQSTA_GetErrorBuMissing] TO [db_executor]
    AS [dbo];

