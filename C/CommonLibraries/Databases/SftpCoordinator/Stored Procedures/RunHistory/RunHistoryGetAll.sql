CREATE PROCEDURE [dbo].[RunHistoryGetAll]
AS
	select RunHistoryId, StartedOn, EndedOn, RunBy
	  from RunHistory
RETURN 0
