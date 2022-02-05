CREATE PROCEDURE [projectrequest].[GetProductPrioritization]
	@Archived BIT = 0
AS

SELECT 
	P.ProjectId AS [ProjectId],
	P.ProjectName AS [ProjectName],
    BU.BusinessUnit AS [BusinessUnit],
	RTRIM(LEFT(P.ProjectSummary, 200)) AS [Details],
	P.ProjectStatus AS [Status],
	COALESCE(FinanceScore.Score,0) AS [FinanceScore],
	COALESCE(RequestorScore.Score, 0) AS [RequestorScore],
	COALESCE(UrgencyScore.Score, 0) AS [UrgencyScore],
	COALESCE(ResourceScore.Score, 0) AS [ResourcesScore],
	COALESCE(RiskScore.Score, 0) AS [RiskScore],
	((CAST((COALESCE(FinanceScore.Score,0) * COALESCE(FinanceScore.[Weight],0)) AS DECIMAL(12,4))  / CAST(((COALESCE(FinanceScore.OutOf,1)) * 100) AS DECIMAL(12,4))) + 
	((CAST(COALESCE(RequestorScore.Score,0) * COALESCE(RequestorScore.[Weight],0) AS DECIMAL(12,4)))  / CAST(((COALESCE(RequestorScore.OutOf,1)) * 100) AS DECIMAL(12,4))) + 
	((CAST(COALESCE(UrgencyScore.Score,0) * COALESCE(UrgencyScore.[Weight],0) AS DECIMAL(12,4)))  / CAST(((COALESCE(UrgencyScore.OutOf,1)) * 100) AS DECIMAL(12,4))) + 
	((CAST(COALESCE(ResourceScore.Score,0) * COALESCE(ResourceScore.[Weight],0) AS DECIMAL(12,4)))  / CAST(((COALESCE(ResourceScore.OutOf,1)) * 100) AS DECIMAL(12,4))) +
	((CAST(COALESCE(RiskScore.Score,0) * COALESCE(RiskScore.[Weight],0) AS DECIMAL(12,4)))  / CAST(((COALESCE(RiskScore.OutOf,1)) * 100) AS DECIMAL(12,4)))
	) * 100 AS [TotalScore]

FROM 
	[projectrequest].[Projects] P
	INNER JOIN [projectrequest].[BusinessUnits] BU
		ON P.BusinessUnitId = BU.BusinessUnitId
	LEFT JOIN
	(
		SELECT
			S.[ScoreId],
			S.[Score],
			ST.[Weight],
			ST.[OutOf]
		FROM
			[projectrequest].[Scores] S
			INNER JOIN [projectrequest].[ScoreTypes] ST
				ON S.ScoreTypeId = ST.ScoreTypeId
		WHERE
			S.Active = 1
	) FinanceScore
		ON P.FinanceScoreId = FinanceScore.ScoreId
	LEFT JOIN
	(
		SELECT
			S.[ScoreId],
			S.[Score],
			ST.[Weight],
			ST.[OutOf]
		FROM
			[projectrequest].[Scores] S
			INNER JOIN [projectrequest].[ScoreTypes] ST
				ON S.ScoreTypeId = ST.ScoreTypeId
		WHERE
			S.Active = 1
	) RequestorScore
		ON P.RequestorScoreId = RequestorScore.ScoreId
	LEFT JOIN
	(
		SELECT
			S.[ScoreId],
			S.[Score],
			ST.[Weight],
			ST.[OutOf]
		FROM
			[projectrequest].[Scores] S
			INNER JOIN [projectrequest].[ScoreTypes] ST
				ON S.ScoreTypeId = ST.ScoreTypeId
		WHERE
			S.Active = 1
	) UrgencyScore
		ON P.UrgencyScoreId = UrgencyScore.ScoreId
	LEFT JOIN
	(
		SELECT
			S.[ScoreId],
			S.[Score],
			ST.[Weight],
			ST.[OutOf]
		FROM
			[projectrequest].[Scores] S
			INNER JOIN [projectrequest].[ScoreTypes] ST
				ON S.ScoreTypeId = ST.ScoreTypeId
		WHERE
			S.Active = 1
	) ResourceScore
		ON P.ResourcesScoreId = ResourceScore.ScoreId
	LEFT JOIN
	(
		SELECT
			S.[ScoreId],
			S.[Score],
			ST.[Weight],
			ST.[OutOf]
		FROM
			[projectrequest].[Scores] S
			INNER JOIN [projectrequest].[ScoreTypes] ST
				ON S.ScoreTypeId = ST.ScoreTypeId
		WHERE
			S.Active = 1
	) RiskScore
		ON P.RiskScoreId = RiskScore.ScoreId
WHERE
	(P.ArchivedAt IS NULL AND @Archived = 0)
	OR
	(P.ArchivedAt IS NOT NULL AND @Archived = 1)