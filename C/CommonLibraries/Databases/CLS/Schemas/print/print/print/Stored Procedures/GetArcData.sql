
CREATE PROCEDURE [print].[GetArcData]
	@ScriptDataId INT
	
AS
	SELECT
		MAP.ArcScriptDataMappingId,
		C.Comment,
		A.Arc,
		MAP.ArcTypeId [Type]
	FROM
		ArcScriptDataMapping MAP
		INNER JOIN Arcs A 
			ON A.ArcId = MAP.ArcId
		LEFT JOIN Comments C
			ON C.CommentId = MAP.CommentId
	WHERE
		MAP.ScriptDataId = @ScriptDataId
RETURN 0