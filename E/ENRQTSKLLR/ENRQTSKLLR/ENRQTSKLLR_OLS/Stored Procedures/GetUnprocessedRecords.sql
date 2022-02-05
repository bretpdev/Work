CREATE PROCEDURE [enrqtskllr].[GetUnprocessedRecords]

AS

	SELECT 
		PQ.ProcessingQueueId,
		PQ.AccountNumber,
		PD01.DF_PRS_ID AS Ssn,
		Q.QueueName,
		PQ.ArcAddedAt
	FROM
		enrqtskllr.ProcessingQueue PQ
		INNER JOIN enrqtskllr.Queues Q
			ON Q.QueueId = PQ.QueueId
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON PD01.DF_SPE_ACC_ID = PQ.AccountNumber
	WHERE
		PQ.DeletedAt IS NULL
		AND 
		(
			PQ.ProcessedAt IS NULL
			OR PQ.ArcAddedAt IS NULL
		)

RETURN 0
