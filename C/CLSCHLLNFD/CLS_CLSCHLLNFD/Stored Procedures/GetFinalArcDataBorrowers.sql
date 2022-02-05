CREATE PROCEDURE [clschllnfd].[GetFinalArcDataBorrowers]

AS
	SELECT DISTINCT
		CD.BorrowerSsn
	FROM
		[clschllnfd].[SchoolClosureData] CD
		LEFT JOIN
		(
			SELECT 
				BorrowerSsn,
				COUNT(*) AS PROCESSED_COUNT
			FROM 
				[clschllnfd].[SchoolClosureData]
			WHERE
				PrintProcessingId IS NOT NULL 
				AND ProcessedAt IS NOT NULL
				AND ArcAddProcessingId IS NULL
				AND DeletedAt IS NULL
				AND FinalArcAddProcessingId IS NULL
			GROUP BY
				BorrowerSsn
		) FINAL_COUNT
			ON FINAL_COUNT.BorrowerSsn = CD.BorrowerSsn
		LEFT JOIN
		(
			SELECT 
				BorrowerSsn,
				COUNT(*) AS TOTAL_COUNT
			FROM 
				[clschllnfd].[SchoolClosureData]
			WHERE
				DeletedAt IS NULL
			GROUP BY
				BorrowerSsn
		) TOTAL
			ON TOTAL.BorrowerSsn = CD.BorrowerSsn
	WHERE
		PROCESSED_COUNT = TOTAL_COUNT

RETURN 0
