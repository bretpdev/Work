CREATE PROCEDURE [dbo].[UpdateProjectFile]
	@ProjectFileId INT,
	@SourceRoot NVARCHAR(256),
	@SourcePathTypeId INT,
	@DestinationRoot NVARCHAR(256),
	@DestinationPathTypeId INT,
	@SearchPattern NVARCHAR(128) = NULL,
	@AntiSearchPattern NVARCHAR(128) = NULL,
	@DecryptFile BIT,
	@CompressFile BIT,
	@EncryptFile BIT,
	@AggregationFormatString NVARCHAR(64) = NULL,
	@RenameFormatString NVARCHAR(64) = NULL,
	@FixLineEndings BIT,
	@IsArchiveJob BIT
AS
	EXEC dbo.DeleteProjectFile @ProjectFileId

	DECLARE @ProjectId INT = (SELECT ProjectId FROM dbo.ProjectFiles WHERE ProjectFileId = @ProjectFileId)

	EXEC dbo.InsertProjectFile @ProjectId, @SourceRoot, @SourcePathTypeId, @DestinationRoot, @DestinationPathTypeId, @SearchPattern, @AntiSearchPattern, @DecryptFile, @CompressFile, @EncryptFile, @AggregationFormatString, @RenameFormatString, @FixLineEndings, @IsArchiveJob
	
	DECLARE @NewProjectFileId INT = SCOPE_IDENTITY()

	SELECT @NewProjectFileId AS ProjectFileId
