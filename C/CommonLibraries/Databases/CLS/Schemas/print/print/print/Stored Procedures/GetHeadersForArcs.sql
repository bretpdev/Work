CREATE PROCEDURE [print].[GetHeadersForArcs]
	@ArcScriptDataMappingId int
AS
	SELECT
		HN.HeaderName
	FROM
		ArcScriptDataMapping MAP
		INNER JOIN ArcLoanHeaderMapping LMAP
			ON LMAP.ArcScriptDataMappingId = MAP.ArcScriptDataMappingId
		INNER JOIN HeaderNames HN
			ON HN.HeaderNameId = LMAP.HeaderNameId
	WHERE
		MAP.ArcScriptDataMappingId = @ArcScriptDataMappingId
RETURN 0