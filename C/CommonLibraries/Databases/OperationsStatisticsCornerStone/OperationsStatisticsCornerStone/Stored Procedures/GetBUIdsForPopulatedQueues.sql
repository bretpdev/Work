CREATE PROCEDURE [dbo].[GetBUIdsForPopulatedQueues]
	@RunDateTime DateTime
AS
	SELECT DISTINCT
		QD.BusinessUnit
	FROM	
		QSTA_LST_QueueDetail QD
	INNER JOIN QSTA_DAT_QueueData Q
		ON Q.[Queue] = QD.QueueName
		AND Q.RunDateTime = @RunDateTime
RETURN 0
