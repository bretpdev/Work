CREATE PROCEDURE [enrqtskllr].[InsertProcessingQueueRecords]

AS

BEGIN

	INSERT INTO OLS.enrqtskllr.ProcessingQueue (AccountNumber, QueueId, QueueTaskCreatedAt)
	SELECT
		PD01.DF_SPE_ACC_ID AS AccountNumber,
		Q.QueueId,
		CT30.IF_CRT_DTS_CT30 AS QueueTaskCreatedAt
	FROM
		ODW..CT30_CALL_QUE CT30
		INNER JOIN OLS.enrqtskllr.Queues Q
			ON Q.QueueName = CT30.IF_WRK_GRP
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON PD01.DF_PRS_ID = CT30.DF_PRS_ID_BR
		LEFT JOIN OLS.enrqtskllr.ProcessingQueue PQ
			ON PQ.QueueId = Q.QueueId
			AND PQ.AccountNumber = PD01.DF_SPE_ACC_ID
			AND PQ.QueueTaskCreatedAt = CT30.IF_CRT_DTS_CT30
			AND 
			(	
				CAST(PQ.CreatedAt AS DATE) = CAST(GETDATE() AS DATE) --Added today
				OR
				( 
					PQ.DeletedAt IS NULL
					AND PQ.ProcessedAt IS NULL
				) --Not worked
			)
	WHERE
		CT30.IC_TSK_STA = 'A' --Available
		AND PQ.AccountNumber IS NULL

	SELECT @@ROWCOUNT --Number of affected rows (a.k.a. insertion count)

END

RETURN 0
