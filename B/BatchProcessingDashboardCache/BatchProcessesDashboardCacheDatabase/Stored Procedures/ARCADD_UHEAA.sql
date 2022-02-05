CREATE PROCEDURE [dashcache].[ARCADD_UHEAA]
AS

	SELECT
		COUNT(AAP_U.ArcAddProcessingId)
	FROM
		ULS.dbo.ArcAddProcessing AAP_U
	WHERE
		AAP_U.ProcessedAt IS NULL
		AND
		AAP_U.ProcessOn < DATEADD(HOUR, -3, GETDATE())
		AND
		AAP_U.CreatedAt < DATEADD(HOUR, -3, GETDATE())

RETURN 0
