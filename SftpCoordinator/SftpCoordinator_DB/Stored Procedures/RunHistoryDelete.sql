CREATE PROCEDURE [dbo].[RunHistoryDelete]
	@RunHistoryID int
AS

DELETE FROM 
	RunHistory
WHERE 
	RunHistoryID = @RunHistoryID
