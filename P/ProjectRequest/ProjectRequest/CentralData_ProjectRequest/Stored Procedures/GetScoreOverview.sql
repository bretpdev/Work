CREATE PROCEDURE [projectrequest].[GetScoreOverview]
(
	@ProjectId INT
)
AS

SELECT 
	@ProjectId AS [ProjectId],
	ST.[ScoreTypeId] AS [ScoreTypeId],
    ST.[ScoreType] AS [ScoreDescription],
	CurrentScores.[Score] AS [Score],
	CurrentScores.[ScoreId] AS [ScoreId],
    ST.[Weight] AS [ScoreWeight],
	ST.[OutOf] AS [ScoreOutOf]
FROM 
	[projectrequest].[ScoreTypes] ST
	LEFT JOIN
	(
		SELECT
			S.Score,
			S.ScoreId,
			S.ScoreTypeId
		FROM
			[projectrequest].[Projects] P
			INNER JOIN [projectrequest].[Scores] S
				ON P.[FinanceScoreId] = S.[ScoreId]
				OR P.[RequestorScoreId] = S.[ScoreId]
				OR P.[UrgencyScoreId] = S.[ScoreId]
				OR P.[ResourcesScoreId] = S.[ScoreId]
				OR P.[RiskScoreId] = S.[ScoreId]
		WHERE
			P.ProjectId = @ProjectId
			AND S.Active = 1
	) CurrentScores
		ON ST.ScoreTypeId = CurrentScores.ScoreTypeId