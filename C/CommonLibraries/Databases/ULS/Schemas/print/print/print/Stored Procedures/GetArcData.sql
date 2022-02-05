CREATE PROCEDURE [print].[GetArcData]
	@ScriptDataId INT
	
AS
	SELECT
		MAP.ArcScriptDataMappingId,
		C.Comment,
		A.Arc,
		MAP.ArcTypeId as [Type],
		ARC.ResponseCode,
		A.ActivityType,
		A.ActivityContact
	FROM
		ArcScriptDataMapping MAP
		INNER JOIN Arcs A 
			ON A.ArcId = MAP.ArcId
		LEFT JOIN Comments C
			ON C.CommentId = MAP.CommentId
		LEFT JOIN DBO.ArcResponseCodes ARC
			ON ARC.ArcResponseCodeId = MAP.ArcResponseCodeId
	WHERE
		MAP.ScriptDataId = @ScriptDataId
RETURN 0
