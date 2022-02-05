CREATE PROCEDURE [achrirdf].[BorrowerHasUnprocessedRIRAJ]
	@AccountNumber char(10)
AS

SELECT  
	CAST(CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END AS BIT) as BorrowerHasUnprocessedRIRAJ
FROM 
	[ArcAddProcessing]
WHERE
	ProcessedAt IS NULL
AND
	AccountNumber = @AccountNumber
AND 
	ARC = 'RIRAJ'
AND
	ScriptId = 'ACHRIRDF'

RETURN 0
