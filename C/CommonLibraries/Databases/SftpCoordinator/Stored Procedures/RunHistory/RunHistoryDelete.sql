CREATE PROCEDURE [dbo].[RunHistoryDelete]
	@RunHistoryID int
AS
	delete 
	  from RunHistory
	 where RunHistoryID = @RunHistoryID
RETURN 0
