CREATE PROCEDURE [dbo].[GetLocateQueues]

AS
	SELECT
		QueueName F
	FROM 
		dbo.GENR_LST_LocateQueue
RETURN 0
