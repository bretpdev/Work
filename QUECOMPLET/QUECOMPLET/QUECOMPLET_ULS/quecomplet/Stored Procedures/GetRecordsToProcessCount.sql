CREATE PROCEDURE [quecomplet].[GetRecordsToProcessCount]

AS
	SELECT 
		COUNT(*)
	FROM
		quecomplet.Queues Q
	WHERE
		Q.PickedUpForProcessing IS NULL
		AND Q.DeletedAt IS NULL
		AND Q.ProcessedAt IS NULL
RETURN 0