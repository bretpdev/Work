CREATE PROCEDURE [clschllnfd].[GetDischargeTotal]
	@BF_SSN CHAR(9),
	@SchoolCode VARCHAR(8),
	@Date DATETIME
AS
	SELECT
		SchoolClosureDataId,
		DischargeAmount,
		LoanSeq
	FROM
		clschllnfd.SchoolClosureData
	WHERE
		BorrowerSsn = @BF_SSN
		AND SchoolCode = @SchoolCode
		AND CAST(AddedAt AS DATE) = @Date
		AND ProcessedAt IS NOT NULL
		AND DeletedAt IS NULL
RETURN 0