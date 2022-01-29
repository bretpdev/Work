CREATE PROCEDURE [dbo].[GetArcAddCount]
	
AS
	SELECT 
		COUNT(*) AS ArcCount
	FROM 
		ArcAddProcessing AAP
	WHERE
		AAP.ProcessOn <= GETDATE()
		AND AAP.ProcessedAt IS NULL
		AND RTRIM(ISNULL(ActivityType,'')) = '' --Added to make sure this does not interfere with Onelink AAP
RETURN 0
