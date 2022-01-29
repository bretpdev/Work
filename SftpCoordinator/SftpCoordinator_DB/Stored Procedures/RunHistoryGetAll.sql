CREATE PROCEDURE [dbo].[RunHistoryGetAll]
AS

SELECT 
	RunHistoryId, 
	StartedOn, 
	EndedOn, 
	RunBy
FROM 
	RunHistory
