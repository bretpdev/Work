CREATE PROCEDURE [dbo].[GetProjectFilesDetailed]
AS

SELECT 
	p.[Name] AS ProjectName, 
	p.Notes AS ProjectNotes, 
	pf.SourceRoot, 
	SP.[Description] AS SourceType, 
	pf.DestinationRoot, 
	DP.[Description] AS DestinationType, 
	SearchPattern, 
	AntiSearchPattern, 
	dbo.YNI(DecryptFile) AS [Decrypt], 
	dbo.YNI(CompressFile) AS [Compress], 
	AggregationFormatString, 
	RenameFormatString, 
	dbo.YNI(EncryptFile) AS Encrypt, 
	dbo.YNI(FixLineEndings) AS FixLineEndings,
	dbo.YNI(IsArchiveJob) AS IsArchiveJob
FROM 
	Projects p 
	LEFT JOIN ProjectFiles pf 
		ON pf.ProjectId = p.ProjectId
	LEFT JOIN PathTypes SP 
		ON pf.SourcePathTypeId = SP.PathTypeId
	LEFT JOIN PathTypes DP 
		ON pf.DestinationPathTypeId = DP.PathTypeId
WHERE 
	p.Retired = 0
	AND pf.Retired = 0
ORDER BY 
	ProjectName
