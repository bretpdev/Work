CREATE PROCEDURE [qworkerlgp].[CleanUpRecords]
AS
	DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE)
	DECLARE @WeekAgo DATE = CAST(DATEADD(WEEK, -1, @CurrentDate) AS DATE)
	/*
		This update takes care of tasks that were picked up by the script
		but were not fully processed.  This can occur due to, say, a 
		COMException when a Session becomes inoperable.
	*/
	UPDATE 
		qworkerlgp.Queues
	SET
		PickedUpForProcessing = NULL
	WHERE
		PickedUpForProcessing IS NOT NULL
		AND ProcessedAt IS NULL
		AND DeletedAt IS NULL
		AND CAST(PickedUpForProcessing AS DATE) < @CurrentDate --Pull back tasks that were picked up before today
RETURN 0
