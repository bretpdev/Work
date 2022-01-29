CREATE PROCEDURE [dbo].[UpdateRunHistoryMarkAsEnded]
	@RunHistoryId INT
AS

UPDATE
	RunHistory
SET
	EndedOn = GETDATE()
WHERE 
	RunHistoryId = @RunHistoryId
