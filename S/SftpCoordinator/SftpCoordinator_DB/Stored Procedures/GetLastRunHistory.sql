CREATE PROCEDURE [dbo].[GetLastRunHistory]
AS

SELECT TOP 1 
	StartedOn
FROM 
	RunHistory
ORDER BY 
	StartedOn 
DESC
