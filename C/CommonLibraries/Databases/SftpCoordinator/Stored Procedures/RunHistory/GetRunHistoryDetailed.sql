CREATE PROCEDURE [dbo].[GetRunHistoryDetailed]
	@StartDate datetime = null,
	@EndDate datetime = null,
	@IncludeEmpty bit = 0
AS
	select RunHistoryId, StartedOn, EndedOn, SuccessfulFiles, InvalidFiles, RunBy
	  from RunHistoryDetailed
	 where (StartedOn >= @StartDate or @StartDate is null) 
	   and (StartedOn <= @EndDate or @EndDate is null)
	   and (InvalidFiles + SuccessfulFiles + @IncludeEmpty > 0)
	 order by StartedOn desc
RETURN 0

grant execute on [dbo].[GetRunHistoryDetailed] to [db_executor]