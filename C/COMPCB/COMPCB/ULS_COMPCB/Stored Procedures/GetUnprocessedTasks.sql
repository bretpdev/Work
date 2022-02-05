CREATE PROCEDURE [compcb].[GetUnprocessedTasks]
AS
	SELECT 
		ProcessingQueueId,
		BorrowerSsn,
		BorrowerAccountNumber,
		EndorserSsn,
		EndorserAccountNumber,
		TaskControlNumber,
		RequestArc,
		TaskRequestedDate,
		IsEndorserTask,
		IsForeignAddress,
		ProcessedAt,
		ArcAddProcessingId,
		ProcessingAttempts
	FROM
		compcb.ProcessingQueue
	WHERE
		DeletedAt IS NULL
		AND ProcessingAttempts < 3 --Field starts at zero, so script will only try to work it 3 times
		AND
			(
				ProcessedAt IS NULL --Task not processed in session 
				OR (ArcAddProcessingId IS NULL AND IsForeignAddress = 1) -- Foreign address review task not created
			)
RETURN 0
