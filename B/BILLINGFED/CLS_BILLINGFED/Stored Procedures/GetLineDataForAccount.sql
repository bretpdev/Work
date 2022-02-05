CREATE PROCEDURE [billing].[GetLineDataForAccount]
(
	@PrintProcessingId int
)
AS
BEGIN
	SELECT 
		LD.LineData
	FROM
		billing.LineData LD 
		INNER JOIN billing.PrintProcessing PP on LD.PrintProcessingId = PP.PrintProcessingId
	WHERE
		PP.PrintProcessingId = @PrintProcessingId
END;