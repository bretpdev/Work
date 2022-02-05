CREATE PROCEDURE [emailbtcf].[GetLineData]
	@LineDataId INT
AS
	
SELECT
	LD.LineDataId,
	LD.LineData
FROM
	emailbtcf.LineData LD
WHERE
	LD.LineDataId = @LineDataId