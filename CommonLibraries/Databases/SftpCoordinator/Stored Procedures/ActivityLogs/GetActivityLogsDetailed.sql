CREATE PROCEDURE [dbo].[GetActivityLogsDetailed]
	@RunHistoryId int
AS
	select CreatedOn, SourcePath, DestinationPath, dbo.YNI(DecryptionSuccessful) as DecryptionOK, dbo.YNI(CopySuccessful) as CopyOK, 
		   dbo.YNI(FixLineEndingsSuccessful) as FixLineEndingsSuccessful, dbo.YNI(EncryptionSuccessful) as EncryptionOK, dbo.YNI(DeleteSuccessful) as DeleteOK, 
		   dbo.YNI(CompressionSuccessful) as CompressionOK, dbo.YNI(Successful) as Success, PreDecryptionArchiveLocation as PreDecryptionArchive, 
		   PreEncryptionArchiveLocation as PreEncryptionArchive, i.FilePath as InvalidFilePath, i.ErrorMessage as ErrorMessage, i.FileTimestamp as InvalidFileTimestamp, 
		   i.ResolvedBy as InvalidFileResolvedBy
	  from ActivityLog al
	  left join InvalidFiles i on i.InvalidFileId = al.InvalidFileId
	 where al.RunHistoryId = @RunHistoryId
RETURN 0

grant execute on [dbo].[GetActivityLogsDetailed] to [db_executor]