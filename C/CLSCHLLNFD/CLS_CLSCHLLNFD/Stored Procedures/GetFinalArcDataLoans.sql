CREATE PROCEDURE [clschllnfd].[GetFinalArcDataLoans]
	@BorrowerSsn VARCHAR(10)
AS
	SELECT DISTINCT
		LoanSeq
	FROM
		[clschllnfd].[SchoolClosureData]
	WHERE
		BorrowerSsn = @BorrowerSsn
RETURN 0
