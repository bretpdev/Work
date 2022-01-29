CREATE PROCEDURE [clschllnfd].[GetAllUnprocessedLoanSequences]
	@BF_SSN char(9),
	@AddedAt DATETIME
AS
	SELECT DISTINCT
		Unworked.LoanSeq
	FROM	
		CLS.clschllnfd.SchoolClosureData Unworked
		LEFT JOIN CLS.clschllnfd.SchoolClosureData WorkedToday
			ON WorkedToday.BorrowerSsn = Unworked.BorrowerSsn
			AND WorkedToday.LoanSeq = Unworked.LoanSeq
			AND CAST(WorkedToday.AddedAt AS DATE) = CAST(@AddedAt AS DATE)
			AND CAST(WorkedToday.ProcessedAt AS DATE) = CAST(GETDATE() AS DATE)
	WHERE
		Unworked.BorrowerSsn = @BF_SSN
		AND CAST(Unworked.AddedAt AS DATE) = CAST(@AddedAt AS DATE)
		AND Unworked.ProcessedAt IS NULL
		AND WorkedToday.BorrowerSsn IS NULL
RETURN 0
