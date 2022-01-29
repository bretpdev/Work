CREATE PROCEDURE [dbo].[GetArcAddCount]
	
AS
	SELECT 
		COUNT(*) AS ArcCount
	FROM 
		ArcAddProcessing AAP
	WHERE
		AAP.ProcessOn <= GETDATE()
		AND AAP.ProcessedAt IS NULL
RETURN 0
