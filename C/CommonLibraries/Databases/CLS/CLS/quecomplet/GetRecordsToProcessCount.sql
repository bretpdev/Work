CREATE PROCEDURE [quecomplet].[GetRecordsToProcessCount]

AS
	SELECT 
		COUNT(*)
	FROM
		CLS.quecomplet.Queues Q
	WHERE
		Q.PickedUpForProcessing IS NULL
RETURN 0
