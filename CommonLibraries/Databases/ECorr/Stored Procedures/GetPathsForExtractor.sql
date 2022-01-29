CREATE PROCEDURE [dbo].[GetPathsForExtractor]
	@SkipDays int
AS
	SELECT 
		DocumentDetailsId, 
		[Path] 
	FROM 
		ECorrFed..DocumentDetails DD 
	WHERE 
		DD.Printed BETWEEN '2016-1-1' 
		AND DATEADD(DAY, -@SkipDays, GETDATE()) AND DD.ZipFileName IS NULL AND [Active] = 1
RETURN 0