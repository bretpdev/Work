CREATE PROCEDURE [acurintr].[GetCompassQueues]

AS
	
	SELECT
		*
	FROM
		acurintr.CompassQueue
	WHERE
		ProcessedAt IS NULL
		AND DeletedAt IS NULL

RETURN 0