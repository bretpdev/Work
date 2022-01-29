CREATE PROCEDURE [dbo].[GetAllPathTypes]
AS
SELECT 
	p.PathTypeId, 
	p.[Description], 
	p.RootPath,
	COUNT(pf.ProjectFileId) AS AffectedFiles
FROM 
	dbo.PathTypes p
	LEFT JOIN dbo.ProjectFiles pf 
		ON p.PathTypeId IN (pf.SourcePathTypeId, pf.DestinationPathTypeId)
WHERE 
	COALESCE(pf.Retired,0) = 0 
GROUP BY 
	p.PathTypeID, 
	p.[Description], 
	p.RootPath
