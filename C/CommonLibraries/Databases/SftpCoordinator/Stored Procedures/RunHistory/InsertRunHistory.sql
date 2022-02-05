CREATE PROCEDURE [dbo].[InsertRunHistory]
    @RunBy nvarchar(50)	
AS
	insert into [dbo].[RunHistory] (RunBy)
	values (@RunBy)
	declare @id int
	set @id = SCOPE_IDENTITY()
	select RunHistoryId, StartedOn from [dbo].[RunHistory] where RunHistoryId = @id
RETURN 0

grant execute on [dbo].[InsertRunHistory] to [db_executor]