CREATE PROCEDURE [dbo].[GetRunHistoryDetailed]
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@IncludeEmpty BIT = 0
AS

SELECT 
	RunHistoryId, 
	StartedOn, 
	EndedOn, 
	SuccessfulFiles, 
	InvalidFiles, 
	RunBy
FROM 
	RunHistoryDetailed
WHERE
	StartedOn BETWEEN COALESCE(@StartDate,'1900-01-01') AND COALESCE(@EndDate,'2099-01-01')
	AND InvalidFiles + SuccessfulFiles + @IncludeEmpty > 0
ORDER BY 
	StartedOn 
DESC
