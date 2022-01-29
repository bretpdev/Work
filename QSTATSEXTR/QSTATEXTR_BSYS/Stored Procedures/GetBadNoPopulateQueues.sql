CREATE PROCEDURE [qstatsextr].[GetBadNoPopulateQueues]
	@RunTimeDate DATETIME
AS

	SELECT 
		DET.QueueName 
	FROM 
		QSTA_LST_QueueDetail DET
		INNER JOIN QSTA_DAT_QueueData DAT
			ON DET.QueueName = DAT.[Queue]
	WHERE 
		NULLIF(DET.BusinessUnit, '') IS NULL
		AND DAT.RunTimeDate = @RunTimeDate
		AND DET.SystemQInd = 'N'

RETURN 0
