CREATE PROCEDURE [print].LoadBillingData
(
	@ScriptId VARCHAR(10)
)
AS
BEGIN
INSERT INTO [print].BillingData
(
	LineDataId,
	DaysDelinquent
)
SELECT DISTINCT
	LD.LineDataId,
	dbo.SplitAndRemoveQuotes(LD.LineData,',',18,1) as DaysDeliquent
FROM
	[print].LineData LD 
	INNER JOIN [print].PrintProcessing PP on PP.PrintProcessingId = LD.PrintProcessingId
	INNER JOIN [print].ScriptFiles SF on SF.ScriptFileId = PP.ScriptFileId and SF.ScriptID = 'BILLINGFED'
	LEFT JOIN [print].BillingData BD ON BD.LineDataId = LD.LineDataId
WHERE
	BD.LineDataId is NULL
END;