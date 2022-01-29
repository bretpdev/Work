CREATE PROCEDURE [accurint].[UH_GetUnprocessedRecords]
	@RunId INT
AS
	SELECT 
		DPQ.DemosId,
		DPQ.AccountNumber,
		PD10.DF_PRS_ID AS Ssn,
		DPQ.EndorserSsn,
		DPQ.[Queue],
		DPQ.SubQueue,
		DPQ.TaskControlNumber,
		DPQ.TaskRequestArc,
		DPQ.TaskCreatedAt,
		DPQ.AddedToRequestFileAt,
		DPQ.TaskCompletedAt,
		DPQ.RequestArcId,
		DPQ.ResponseAddressArcId,
		DPQ.ResponsePhoneArcId
	FROM
		accurint.DemosProcessingQueue_UH DPQ
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_SPE_ACC_ID = DPQ.AccountNumber
	WHERE
		DPQ.DeletedAt IS NULL
		AND DPQ.RunId = @RunId
		AND
		(
			DPQ.AddedToRequestFileAt IS NULL --Not sent. All records should be sent in the request file, unlike OL.
			OR DPQ.TaskCompletedAt IS NULL
			OR DPQ.RequestArcId IS NULL
			OR 
			(
				DPQ.ResponseAddressArcId IS NULL
				AND DPQ.ResponsePhoneArcId IS NULL
			)
		)

RETURN 0
