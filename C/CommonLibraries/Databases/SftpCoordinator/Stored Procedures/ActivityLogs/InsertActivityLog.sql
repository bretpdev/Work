CREATE PROCEDURE [dbo].[InsertActivityLog]
	@RunHistoryId int,
	@ProjectFileId int,
	@SourcePath nvarchar(512),
	@DestinationPath nvarchar(512),
	@DecryptionSuccessful bit = null,
	@EncryptionSuccessful bit = null,
	@CopySuccessful bit,
	@FixLineEndingsSuccessful bit = null,
	@DeleteSuccessful bit = null,
	@CompressionSuccessful bit = null,
	@Successful bit,
	@InvalidFileId int = null,
	@PreDecryptionArchiveLocation nvarchar(512) = null,
	@PreEncryptionArchiveLocation nvarchar(512) = null
AS
	insert into dbo.ActivityLog(RunHistoryId, ProjectFileId, SourcePath, DestinationPath, DecryptionSuccessful, EncryptionSuccessful, CopySuccessful, FixLineEndingsSuccessful, DeleteSuccessful, CompressionSuccessful, Successful, InvalidFileId, PreDecryptionArchiveLocation, PreEncryptionArchiveLocation)
	values (@RunHistoryId, @ProjectFileId, @SourcePath, @DestinationPath, @DecryptionSuccessful, @EncryptionSuccessful, @CopySuccessful, @FixLineEndingsSuccessful, @DeleteSuccessful, @CompressionSuccessful, @Successful, @InvalidFileId, @PreDecryptionArchiveLocation, @PreEncryptionArchiveLocation)
RETURN 0

grant execute on [dbo].[InsertActivityLog] to [db_executor]