CREATE PROCEDURE [dbo].[InsertProjectFile]
	@ProjectId INT,
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

INSERT INTO ProjectFiles(ProjectId, SourceRoot, SourcePathTypeId, DestinationRoot, DestinationPathTypeId, SearchPattern, AntiSearchPattern, DecryptFile, CompressFile, EncryptFile, AggregationFormatString, RenameFormatString, FixLineEndings, IsArchiveJob)
VALUES(@ProjectId, @SourceRoot, @SourcePathTypeId, @DestinationRoot, @DestinationPathTypeId, @SearchPattern, @AntiSearchPattern,@DecryptFile, @CompressFile, @EncryptFile, @AggregationFormatString, @RenameFormatString, @FixLineEndings, @IsArchiveJob)
