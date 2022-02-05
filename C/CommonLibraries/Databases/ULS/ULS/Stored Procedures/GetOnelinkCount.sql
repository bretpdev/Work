CREATE PROCEDURE [dbo].[GetOnelinkCount]
	
AS
	SELECT 
		COUNT(*)
	FROM
		ArcAddProcessingOnelink
	WHERE
		ProcessedAt IS NULL
		AND ProcessOn <= GETDATE()
RETURN 0
