CREATE PROCEDURE [dbo].[InsertInvalidFile]
	@FilePath nvarchar(256),
	@FileTimestamp datetime,
	@ErrorMessage nvarchar(MAX)
AS
	insert into [dbo].[InvalidFiles] (FilePath, FileTimestamp, ErrorMessage)
	values (@FilePath, @FileTimestamp, @ErrorMessage)

	select cast(SCOPE_IDENTITY() as int)
return 0

grant execute on [dbo].[InsertInvalidFile] to [db_executor]