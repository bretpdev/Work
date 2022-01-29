CREATE PROCEDURE [dbo].[GetProjectFilesByProjectId]
	@ProjectId INT
AS

SELECT 
	ProjectFileId, 
	ProjectId, 
	SourceRoot, 
	SourcePathTypeId, 
	DestinationRoot, 
	DestinationPathTypeId, 
	SearchPattern, 
	AntiSearchPattern, 
	DecryptFile, 
	CompressFile, 
	EncryptFile, 
	AggregationFormatString, 
	RenameFormatString, 
	FixLineEndings,
	IsArchiveJob
FROM 
	ProjectFiles
WHERE 
	ProjectId = @ProjectId
	AND Retired = 0