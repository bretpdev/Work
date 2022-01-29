CREATE PROCEDURE [clschllnfd].[GetAccountsToPrint]
AS
SELECT DISTINCT
	Prints.SchoolClosureDataId,
	Prints.BorrowerSsn,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	Prints.LoanSeq,
	Prints.SchoolCode,
	CAST(Prints.AddedAt AS DATE) AS AddedAt
FROM 
	clschllnfd.SchoolClosureData Prints
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = Prints.BorrowerSsn
	LEFT JOIN clschllnfd.SchoolClosureData Pending --Has no pending records for processing and/or not processed today
		ON Pending.BorrowerSsn = Prints.BorrowerSsn
		AND Pending.DeletedAt IS NULL
		AND 
			( 
				Pending.ProcessedAt IS NULL --pending processing
				OR CAST(Pending.ProcessedAt AS DATE) = CAST(GETDATE() AS DATE) --disb was written off today
			)
WHERE 
	Prints.PrintProcessingId IS NULL 
	AND Prints.ProcessedAt IS NOT NULL --worked
	AND Prints.DeletedAt IS NULL
	AND Pending.BorrowerSsn IS NULL
ORDER BY
	Prints.BorrowerSsn

