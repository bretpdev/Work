CREATE PROCEDURE [clschllnfd].[UpdateErrorArcAddId]
	@BorrowerSsn CHAR(9),
	@LoanSeq INT,
	@AddedAt DATETIME,
	@ArcAddProcessingId BIGINT
AS
	UPDATE
		clschllnfd.SchoolClosureData
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		BorrowerSsn = @BorrowerSsn
		AND LoanSeq = @LoanSeq
		AND CAST(AddedAt AS DATE) = CAST(@AddedAt AS DATE)
		AND DeletedAt IS NULL
RETURN 0