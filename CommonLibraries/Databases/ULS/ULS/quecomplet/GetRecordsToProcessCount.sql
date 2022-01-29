CREATE PROCEDURE [quecomplet].[GetRecordsToProcessCount]

AS
	SELECT 
		COUNT(*)
	FROM
		ULS.quecomplet.Queues Q
	WHERE
		Q.PickedUpForProcessing IS NULL
RETURN 0
