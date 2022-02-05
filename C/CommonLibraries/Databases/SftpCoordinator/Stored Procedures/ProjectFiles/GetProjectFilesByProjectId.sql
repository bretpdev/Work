CREATE PROCEDURE [dbo].[GetProjectFilesByProjectId]
	@ProjectId int
AS
	select ProjectFileId, ProjectId, SourceRoot, SourcePathTypeId, DestinationRoot, DestinationPathTypeId, SearchPattern, AntiSearchPattern, DecryptFile, CompressFile, EncryptFile, AggregationFormatString, RenameFormatString, FixLineEndings
	  from [dbo].[ProjectFiles] pf
	 where ProjectId = @ProjectId
	   and [Retired] = 0
RETURN 0

grant execute on [dbo].[GetProjectFilesByProjectId] to [db_executor]