CREATE PROCEDURE [bbreininsc].[GetRecordsToProcess]
AS
BEGIN
	SELECT DISTINCT
		RecordId,
		BF_SSN,
		LN_SEQ,
		LD_BIL_DU,
		RecordType
	FROM
		[bbreininsc].ReinstatementProcessing
	WHERE
		DeletedAt IS NULL
		AND ProcessedAt IS NULL
END