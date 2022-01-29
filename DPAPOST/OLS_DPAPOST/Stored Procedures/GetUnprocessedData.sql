CREATE PROCEDURE [dpapost].[GetUnprocessedData]
AS
	SELECT
		PostingDataId,
		AccountNumber,
		Amount,
		AddedAt
	FROM
		dpapost.PostingData
	WHERE
		DeletedAt IS NULL
		AND ProcessedAt IS NULL
		AND ErrorPosting = 0