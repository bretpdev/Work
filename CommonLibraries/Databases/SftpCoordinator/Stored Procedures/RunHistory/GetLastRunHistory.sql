CREATE PROCEDURE [dbo].[GetLastRunHistory]
AS
	select top 1 StartedOn
	  from RunHistory
	 order by StartedOn desc
RETURN 0

grant execute on [dbo].[GetLastRunHistory] to [db_executor]