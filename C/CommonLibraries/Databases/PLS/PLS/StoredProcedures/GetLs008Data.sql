CREATE PROCEDURE [ls008].[GetLs008Data]
	
AS
	SELECT
		P.ProcessId,
		P.ProcessName,
		P.ARC,
		P.ArcMessageText,
		P.OriginalCommentText,
		P.[Description]
	FROM
		[ls008].Processes P
	order by 
		p.ARC
RETURN 0
