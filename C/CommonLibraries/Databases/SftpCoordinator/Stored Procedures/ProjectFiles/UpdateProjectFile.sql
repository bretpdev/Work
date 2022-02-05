CREATE PROCEDURE [dbo].[UpdateProjectFile]
	@ProjectFileId int,
	@SourceRoot nvarchar(256),
	@SourcePathTypeId int,
	@DestinationRoot nvarchar(256),
	@DestinationPathTypeId int,
	@SearchPattern nvarchar(128) = null,
	@AntiSearchPattern nvarchar(128) = null,
	@DecryptFile bit,
	@CompressFile bit,
	@EncryptFile bit,
	@AggregationFormatString nvarchar(64) = null,
	@RenameFormatString nvarchar(64) = null,
	@FixLineEndings bit
AS
	exec dbo.DeleteProjectFile @ProjectFileId

	declare @ProjectId int
	select @ProjectId = ProjectId from dbo.ProjectFiles where ProjectFileId = @ProjectFileId

	exec dbo.InsertProjectFile @ProjectId, @SourceRoot, @SourcePathTypeId, @DestinationRoot, @DestinationPathTypeId, @SearchPattern,
	                           @AntiSearchPattern, @DecryptFile, @CompressFile, @EncryptFile, @AggregationFormatString, @RenameFormatString,
							   @FixLineEndings
	
	declare @NewProjectFileId int
	select @NewProjectFileId = SCOPE_IDENTITY()

	select @NewProjectFileId as ProjectFileId
RETURN 0
