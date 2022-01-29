CREATE PROCEDURE [projectrequest].[GetScoreType]
(
	@ScoreTypeId INT
)
AS

SELECT 
	ST.[ScoreType] AS [ScoreType]
FROM 
	[projectrequest].[ScoreTypes] ST
WHERE
	ST.ScoreTypeId = @ScoreTypeId