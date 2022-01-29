CREATE PROCEDURE [dbo].[GetAllRegions]
	
AS
	SELECT 
		RegionId,
		RegionName
	FROM
		Regions
RETURN 0
