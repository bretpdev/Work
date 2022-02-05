CREATE PROCEDURE [rtrnmailuh].[GetRecordsToProcess]
AS
	SELECT
		BarcodeDataId,
		AccountIdentifier,
		CreateDate,
		Address1,
		Address2,
		City,
		[State],
		Zip,
		Comment,
		ArcAddProcessingId
	FROM
		rtrnmailuh.BarcodeData
	WHERE
		DeletedAt IS NULL
		AND ProcessedAt IS NULL
RETURN 0