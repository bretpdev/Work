CREATE PROCEDURE [accurint].[OL_GetUnprocessedRecords]
	@RunId INT
AS
	SELECT 
		DPQ.DemosId,
		DPQ.AccountNumber,
		PD01.DF_PRS_ID AS Ssn,
		DPQ.WorkGroup,
		DPQ.Department,
		DPQ.TaskCreatedAt,
		DPQ.SendToAccurint,
		DPQ.AddedToRequestFileAt,
		DPQ.TaskCompletedAt,
		DPQ.RequestCommentAdded, --Comment placed on request task
		DPQ.AddressTaskQueueId, --Task created when processing response file
		DPQ.PhoneTaskQueueId
	FROM
		accurint.DemosProcessingQueue_OL DPQ
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON PD01.DF_SPE_ACC_ID = DPQ.AccountNumber
	WHERE
		DPQ.DeletedAt IS NULL
		AND DPQ.RunId = @RunId
		AND
		(
			(
				DPQ.RequestCommentAdded IS NULL --No comment on account. All records should have a comment.
				OR
				(
					DPQ.TaskCompletedAt IS NULL --Task not completed. All non-special request records should have their request task completed.
					AND DPQ.WorkGroup IS NULL --If WorkGroup is null, it means this was submitted by a special request file, so there is no corresponding task to close
				)
			)
			OR
			(
				(
					DPQ.AddedToRequestFileAt IS NULL --Haven't put record in request file.
					OR 
					(
						DPQ.AddressTaskQueueId IS NULL --Haven't created task off of response file record.
						AND DPQ.PhoneTaskQueueId IS NULL
					)
				)
				AND
				(
					DPQ.SendToAccurint IS NULL --Haven't determined whether we need to send the record.
					OR DPQ.SendToAccurint = 1 --Determined we need to send the record in the request file.
				)
			)
		)
		


RETURN 0
