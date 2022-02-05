CREATE PROCEDURE [accurint].[GetRecordsSentCount]
	@RunId INT
AS
	
	SELECT
		COUNT(1) AS [RecordsSent]
	FROM
		(
			SELECT 
				DemosId
			FROM
				accurint.DemosProcessingQueue_OL
			WHERE
				RunId = @RunId
				AND DeletedAt IS NULL
				AND AddedToRequestFileAt IS NOT NULL

			UNION ALL

			SELECT
				DemosId
			FROM
				accurint.DemosProcessingQueue_UH
			WHERE
				RunId = @RunId
				AND DeletedAt IS NULL
				AND AddedToRequestFileAt IS NOT NULL
		) SentRecords

RETURN 0
