CREATE PROCEDURE [dbo].[InsertInvalidFile]
	@FilePath NVARCHAR(256),
	@FileTimestamp DATETIME,
	@ErrorMessage NVARCHAR(MAX)
AS

INSERT INTO [dbo].[InvalidFiles] (FilePath, FileTimestamp, ErrorMessage)
VALUES (@FilePath, @FileTimestamp, @ErrorMessage)

SELECT CAST(SCOPE_IDENTITY() AS INT)
