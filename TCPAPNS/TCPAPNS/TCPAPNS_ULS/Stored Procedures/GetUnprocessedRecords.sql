CREATE PROCEDURE [tcpapns].[GetUnprocessedRecords]
AS
DECLARE 
	@AccountNumberIndex INT = 0,
	@PhoneIndex INT = 1,
	@DateIndex INT = 2,
	@WirIndex INT = 3

	SELECT 
		FP.FileProcessingId,
		FP.AccountNumber AS AccountNumber,
		FP.Phone AS PhoneNumber,
		FP.RecordDate AS [Date],
		FP.MobileIndicator AS MobileIndicator,
		FP.LineData,
		FP.HasConsentArc,
		0 AS HasConsentArcOneLink,
		FP.SourceFile
	FROM
		tcpapns.FileProcessing FP
	WHERE
		FP.DeletedAt IS NULL
		AND FP.DeletedBy IS NULL
		AND FP.ProcessedOn IS NULL