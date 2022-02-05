CREATE PROCEDURE [monitor].[GetBorrowerRecentMonitorSkippedTask]
	@AccountNumber CHAR(10)
AS
	SELECT TOP 1
		CreatedAt,
		ARC,
		Comment
	FROM 
		ArcAddProcessing
	WHERE
		ScriptId = 'Monitor' 
		AND
		ARC IN ('OVRPS', 'OVRLR') 
		AND
		CreatedAt >= CAST(DATEADD(DD, -10, GETDATE()) AS DATE) 
		AND
		AccountNumber = @AccountNumber
RETURN 0
