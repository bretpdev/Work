CREATE PROCEDURE [projectrequest].[GetProject]
(
	@ProjectId INT
)
AS

SELECT 
	P.[ProjectId] AS [ProjectRequestId],
	P.[ProjectName] AS [ProjectName],
    P.[SubmittedBy] AS [SubmittedBy],
	BU.[BusinessUnit] AS [Department],
	P.[SubmittedAt] AS [Date],
	P.[ProjectSummary] AS [ProjectSummary],
	P.[BusinessNeed] AS [BusinessNeed],
	P.[Benefits] AS [Benefits],
	P.[ImplementationApproach] AS [ImplementationApproach],
	P.[ProjectStatus] AS [Status],
	RS.Score AS [RequestorScore],
	US.Score AS [UrgencyScore],
	RISK.Score AS [RiskScore]
FROM 
	[projectrequest].[Projects] P
	INNER JOIN [projectrequest].[BusinessUnits] BU
		ON P.BusinessUnitId = BU.BusinessUnitId
	LEFT JOIN [projectrequest].[Scores] RS
		ON P.RequestorScoreId = RS.ScoreId
	LEFT JOIN [projectrequest].[Scores] US
		ON P.UrgencyScoreId = US.ScoreId
	LEFT JOIN [projectrequest].[Scores] RISK
		ON P.RiskScoreId = RISK.ScoreId
WHERE
	P.ProjectId = @ProjectId
