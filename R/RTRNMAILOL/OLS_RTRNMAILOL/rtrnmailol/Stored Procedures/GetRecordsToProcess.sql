CREATE PROCEDURE [rtrnmailol].[GetRecordsToProcess]
AS
	SELECT
		BarcodeDataId,
		AccountIdentifier,
		LetterId,
		CreateDate,
		Address1,
		Address2,
		City,
		[State],
		Zip,
		Comment
	FROM
		rtrnmailol.BarcodeData
	WHERE
		DeletedAt IS NULL
		AND ProcessedAt IS NULL
RETURN 0