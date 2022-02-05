CREATE PROCEDURE [dbo].[GetActivityLogsDetailed]
	@RunHistoryId INT
AS

SELECT 
	CreatedOn, 
	SourcePath, 
	DestinationPath, 
	dbo.YNI(DecryptionSuccessful) AS DecryptionOK,
	dbo.YNI(CopySuccessful) AS CopyOK, 
	dbo.YNI(FixLineEndingsSuccessful) AS FixLineEndingsSuccessful, 
	dbo.YNI(EncryptionSuccessful) AS EncryptionOK, 
	dbo.YNI(DeleteSuccessful) AS DeleteOK, 
	dbo.YNI(CompressionSuccessful) AS CompressionOK, 
	dbo.YNI(Successful) AS Success, 
	PreDecryptionArchiveLocation AS PreDecryptionArchive, 
	PreEncryptionArchiveLocation AS PreEncryptionArchive, 
	i.FilePath AS InvalidFilePath, 
	i.ErrorMessage AS ErrorMessage, 
	i.FileTimestamp AS InvalidFileTimestamp, 
	i.ResolvedBy AS InvalidFileResolvedBy
FROM 
	ActivityLog al
	left join InvalidFiles i 
		ON i.InvalidFileId = al.InvalidFileId
WHERE 
	al.RunHistoryId = @RunHistoryId
