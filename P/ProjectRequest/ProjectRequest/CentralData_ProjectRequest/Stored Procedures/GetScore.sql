CREATE PROCEDURE [projectrequest].[GetScore]
(
	@ScoreId INT
)
AS

SELECT 
	S.[ProjectId],
	S.[ScoreTypeId],
	S.[ScoreId],
    ST.[ScoreType] AS [ScoringDepartment],
	S.[Score]
FROM 
	[projectrequest].[Scores] S 
	INNER JOIN [projectrequest].[ScoreTypes] ST
		ON S.ScoreTypeId = ST.ScoreTypeId
WHERE
	S.Active = 1
	AND S.ScoreId = @ScoreId