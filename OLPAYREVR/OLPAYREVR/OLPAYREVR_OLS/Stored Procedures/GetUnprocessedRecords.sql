CREATE PROCEDURE [olpayrevr].[GetUnprocessedRecords]

AS

	SELECT 
		PQ.ProcessingQueueId,
		PQ.Ssn,
		PD01.DF_SPE_ACC_ID AS AccountNumber,
		PQ.PaymentAmount,
		PQ.PaymentEffectiveDate,
		PQ.PaymentType,
		PQ.PaymentPostDate
	FROM
		olpayrevr.ReversalsProcessingQueue PQ
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON PD01.DF_PRS_ID = PQ.Ssn
	WHERE
		PQ.DeletedAt IS NULL
		AND PQ.ProcessedAt IS NULL
		AND 
		(
			PQ.PaymentAlreadyReversed = 0
			OR PQ.PaymentAlreadyReversed IS NULL
		)

RETURN 0
