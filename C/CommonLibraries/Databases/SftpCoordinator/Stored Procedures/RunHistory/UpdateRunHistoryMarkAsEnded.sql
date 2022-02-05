CREATE PROCEDURE [dbo].[UpdateRunHistoryMarkAsEnded]
	@RunHistoryId int
AS
	update [dbo].[RunHistory]
	   set EndedOn = getdate()
     where RunHistoryId = @RunHistoryId
RETURN 0

grant execute on [dbo].[UpdateRunHistoryMarkAsEnded] to [db_executor]