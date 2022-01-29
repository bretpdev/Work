CREATE PROCEDURE [quecomplet].[CleanUpRecords]
	
AS
	UPDATE
		ULS.quecomplet.Queues
	SET
		PickedUpForProcessing = NULL
	WHERE
		PickedUpForProcessing IS NOT NULL
		AND ProcessedAt IS NULL
		AND DATEDIFF(DAY, PickedUpForProcessing, GETDATE()) > 1
RETURN 0
