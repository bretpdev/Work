CREATE PROCEDURE [dashcache].[ARCADD_CS]
AS

	SELECT
		COUNT(AAP_C.ArcAddProcessingId)
	FROM
		CLS.dbo.ArcAddProcessing AAP_C
	WHERE
		AAP_C.ProcessedAt IS NULL
		AND
		AAP_C.ProcessOn < DATEADD(HOUR, -3, GETDATE())
		AND
		AAP_C.CreatedAt < DATEADD(HOUR, -3, GETDATE())

RETURN 0
