CREATE PROCEDURE [quecomplet].[CleanUpRecords]
	
AS
	DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE)
	DECLARE @WeekAgo DATE = CAST(DATEADD(WEEK, -1, @CurrentDate) AS DATE)
	/*
		This update takes care of tasks that were picked up by the script
		but were not fully processed.  This can occur due to, say, a 
		COMException when a Session becomes inoperable.
	*/
	UPDATE 
		quecomplet.Queues
	SET
		PickedUpForProcessing = NULL
	WHERE
		PickedUpForProcessing IS NOT NULL
		AND ProcessedAt IS NULL
		AND DeletedAt IS NULL
		AND CAST(PickedUpForProcessing AS DATE) < @CurrentDate --Pull back tasks that were picked up before today

	/*
		This update takes care of tasks that the script attempted to close
		but that encountered an error and are still unclosed per WQ20.
	*/
	UPDATE
		Q
	SET
		ProcessedAt = NULL,
		PickedUpForProcessing = NULL,
		HadError = 0
	FROM
		quecomplet.Queues Q
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON Q.TaskControlNumber = WQ20.WN_CTL_TSK
			AND Q.Queue = WQ20.WF_QUE
			AND Q.SubQueue = WQ20.WF_SUB_QUE
			AND WQ20.WC_STA_WQUE20 IN ('A','H','P','U','W')
	WHERE
		(
			Q.ProcessedAt IS NOT NULL 
			AND Q.DeletedAt IS NULL 
			AND CAST(Q.ProcessedAt AS DATE) < @CurrentDate --Quecomplet thinks it closed it before today.  
		)  
		AND WQ20.WN_CTL_TSK IS NOT NULL --Task still out in WQ20 in a non-closed status
		AND WQ20.WD_ACT_REQ < CAST(Q.AddedAt AS DATE) --Filters out future tasks with matching task control number, queue, subqueue 
		AND Q.HadError = 1
		AND CAST(Q.AddedAt AS DATE) BETWEEN @WeekAgo AND @CurrentDate --Task closure requested within last week

RETURN 0