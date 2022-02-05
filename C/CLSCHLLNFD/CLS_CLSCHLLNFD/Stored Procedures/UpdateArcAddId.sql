CREATE PROCEDURE [clschllnfd].[UpdateArcAddId]
	@BorrowerSsn CHAR(9),
	@AddedAt DATETIME,
	@LoanSeq INT,
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
