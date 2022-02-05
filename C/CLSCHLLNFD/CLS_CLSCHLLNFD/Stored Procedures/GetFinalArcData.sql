CREATE PROCEDURE [clschllnfd].[GetFinalArcData]

AS
	SELECT DISTINCT
			CD.BorrowerSsn,
			PD10.DF_SPE_ACC_ID [AccountNumber],
			CD.LoanSeq,
			CAST(CD.AddedAt AS DATE) AddedAt
		FROM
			[clschllnfd].[SchoolClosureData] CD
			INNER JOIN CDW..PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = CD.BorrowerSsn
			LEFT JOIN
			(
				SELECT 
					BorrowerSsn,
					CAST(AddedAt AS DATE) AddedAt,
					COUNT(*) AS PROCESSED_COUNT
				FROM 
					[clschllnfd].[SchoolClosureData]
				WHERE
					PrintProcessingId IS NOT NULL 
					AND ProcessedAt IS NOT NULL
					AND DeletedAt IS NULL
					AND FinalArcAddProcessingId IS NULL
				GROUP BY
					BorrowerSsn,
					CAST(AddedAt AS DATE)
			) FINAL_COUNT
				ON FINAL_COUNT.BorrowerSsn = CD.BorrowerSsn
				AND CAST(FINAL_COUNT.AddedAt AS DATE) = CAST(CD.AddedAt AS DATE)
			LEFT JOIN
			(
				SELECT 
					BorrowerSsn,
					CAST(AddedAt AS DATE) AddedAt,
					COUNT(*) AS TOTAL_COUNT
				FROM 
					[clschllnfd].[SchoolClosureData]
				WHERE
					DeletedAt IS NULL
				GROUP BY
					BorrowerSsn,
					CAST(AddedAt AS DATE)
			) TOTAL
				ON TOTAL.BorrowerSsn = CD.BorrowerSsn
				AND CAST(TOTAL.AddedAt AS DATE) = CAST(CD.AddedAt AS DATE)
		WHERE
			PROCESSED_COUNT = TOTAL_COUNT
			AND CD.DeletedAt IS NULL
			AND CD.FinalArcAddProcessingId IS NULL
RETURN 0
