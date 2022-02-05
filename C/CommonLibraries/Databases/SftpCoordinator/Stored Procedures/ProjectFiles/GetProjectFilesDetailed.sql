CREATE PROCEDURE [dbo].[GetProjectFilesDetailed]
AS
	select case when IsFederal = 1 then 'Federal' else 'UHEAA' end as ProjectType,
	       p.Name as ProjectName, p.Notes as ProjectNotes, p.IsFederal as IsFederal, pf.SourceRoot, source.[Description] as SourceType, pf.DestinationRoot, 
	       destination.[Description] as DestinationType, SearchPattern, AntiSearchPattern, dbo.YNI(DecryptFile) as [Decrypt], dbo.YNI(CompressFile) as Compress, 
		   AggregationFormatString, RenameFormatString, dbo.YNI(EncryptFile) as Encrypt, dbo.YNI(FixLineEndings) as FixLineEndings
	  from Projects p 
 left join ProjectFiles pf on pf.ProjectId = p.ProjectId
 left join PathTypes source on pf.SourcePathTypeId = source.PathTypeId
 left join PathTypes destination on pf.DestinationPathTypeId = destination.PathTypeId
     where p.[Retired] = 0
	   and pf.[Retired] = 0
  order by IsFederal, ProjectName
RETURN 0

grant execute on [dbo].[GetProjectFilesDetailed] to [db_executor]