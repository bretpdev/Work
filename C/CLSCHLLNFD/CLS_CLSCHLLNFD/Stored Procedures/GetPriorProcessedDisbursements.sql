CREATE PROCEDURE [clschllnfd].[GetPriorProcessedDisbursements]
	@BF_SSN char(9),
	@AddedAt DATETIME
AS
	SELECT DISTINCT
		LoanSeq
	FROM	
		CLS.clschllnfd.SchoolClosureData
	WHERE
		BorrowerSsn = @BF_SSN
		AND CAST(AddedAt AS DATE) = CAST(@AddedAt AS DATE)
		AND CAST(ProcessedAt AS DATE) = CAST(GETDATE() AS DATE) --Only want records that were processed on the run date
		AND DeletedAt IS NULL
		AND WasProcessedPrior = 1
		AND DeletedAt IS NULL
RETURN 0