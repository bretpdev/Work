CREATE PROCEDURE projectrequest.GetAllActiveProjects
AS
SELECT
	P.ProjectName,
	BU.BusinessUnit,
	P.SubmittedBy,
	P.ProjectStatus,
	P.SubmittedAt,
	P.ProjectSummary,
	P.BusinessNeed,
	P.Benefits,
	P.ImplementationApproach
FROM
	[CentralData].[projectrequest].[Projects] P
	INNER JOIN [CentralData].[projectrequest].[BusinessUnits] BU
		ON P.BusinessUnitId = BU.BusinessUnitId
WHERE
	P.ArchivedAt IS NULL
GO