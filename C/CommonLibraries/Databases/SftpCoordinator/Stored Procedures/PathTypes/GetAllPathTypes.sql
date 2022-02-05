CREATE PROCEDURE [dbo].[GetAllPathTypes]
AS
	SELECT p.PathTypeId, p.[Description], p.RootPath, count(pf.ProjectFileId) as AffectedFiles
	  FROM dbo.PathTypes p
 LEFT JOIN dbo.ProjectFiles pf on p.PathTypeId in (pf.SourcePathTypeId , pf.DestinationPathTypeId)
     where pf.Retired = 0 or pf.Retired is null
  GROUP BY p.PathTypeID, p.[Description], p.RootPath
RETURN 0
