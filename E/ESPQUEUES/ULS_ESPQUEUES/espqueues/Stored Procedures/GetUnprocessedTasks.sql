CREATE PROCEDURE [espqueues].[GetUnprocessedTasks]
	
AS
	
	SELECT 
		ProcessingQueueId,
		BorrowerSsn,
		PD10.DF_SPE_ACC_ID [AccountNumber],
		[Queue],
		SubQueue,
		TaskControlNumber,
		RequestArc,
		RequestArcCreatedAt,
		HasOtherGuarantor,
		ArcAddProcessingId,
		ProcessedAt,
		ProcessingStepId
	FROM
		espqueues.ProcessingQueue PQ
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = PQ.BorrowerSsn
	WHERE
		ProcessedAt IS NULL
		AND ReassignedAt IS NULL
		AND DeletedAt IS NULL

RETURN 0
