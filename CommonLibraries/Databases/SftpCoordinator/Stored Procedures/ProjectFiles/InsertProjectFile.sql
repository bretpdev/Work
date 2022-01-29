CREATE PROCEDURE [dbo].[InsertProjectFile]
	@ProjectId int,
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
	insert into ProjectFiles (ProjectId, SourceRoot, SourcePathTypeId, DestinationRoot, DestinationPathTypeId, SearchPattern, AntiSearchPattern,
	                          DecryptFile, CompressFile, EncryptFile, AggregationFormatString, RenameFormatString, FixLineEndings)
					  values (@ProjectId, @SourceRoot, @SourcePathTypeId, @DestinationRoot, @DestinationPathTypeId, @SearchPattern, @AntiSearchPattern,
					          @DecryptFile, @CompressFile, @EncryptFile, @AggregationFormatString, @RenameFormatString, @FixLineEndings)
RETURN 0
