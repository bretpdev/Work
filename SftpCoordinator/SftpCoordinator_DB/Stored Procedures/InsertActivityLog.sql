CREATE PROCEDURE [dbo].[InsertActivityLog]
	@RunHistoryId INT,
	@ProjectFileId INT,
	@SourcePath NVARCHAR(512),
	@DestinationPath NVARCHAR(512),
	@DecryptionSuccessful BIT = NULL,
	@EncryptionSuccessful BIT = NULL,
	@CopySuccessful BIT,
	@FixLineEndingsSuccessful BIT = NULL,
	@DeleteSuccessful BIT = NULL,
	@CompressionSuccessful BIT = NULL,
	@Successful BIT,
	@InvalidFileId INT = NULL,
	@PreDecryptionArchiveLocation nvarchar(512) = NULL,
	@PreEncryptionArchiveLocation nvarchar(512) = NULL
AS

INSERT INTO dbo.ActivityLog(RunHistoryId, ProjectFileId, SourcePath, DestinationPath, DecryptionSuccessful, EncryptionSuccessful, CopySuccessful, FixLineEndingsSuccessful, DeleteSuccessful, CompressionSuccessful, Successful, InvalidFileId, PreDecryptionArchiveLocation, PreEncryptionArchiveLocation)
VALUES (@RunHistoryId, @ProjectFileId, @SourcePath, @DestinationPath, @DecryptionSuccessful, @EncryptionSuccessful, @CopySuccessful, @FixLineEndingsSuccessful, @DeleteSuccessful, @CompressionSuccessful, @Successful, @InvalidFileId, @PreDecryptionArchiveLocation, @PreEncryptionArchiveLocation)