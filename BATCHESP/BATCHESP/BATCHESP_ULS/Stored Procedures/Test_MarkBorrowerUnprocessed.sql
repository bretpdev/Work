CREATE PROCEDURE [batchesp].[Test_MarkBorrowerUnprocessed]
	@BorrowerIdentifier VARCHAR(10)
AS
	DECLARE @EspEnrollmentId INT
	DECLARE @Ssn CHAR(9)
	DECLARE @Date DATETIME

	SELECT
		@Date = MAX(CreatedAt),
		@Ssn = BorrowerSsn,
		@EspEnrollmentId = MAX(EspEnrollmentId)
	FROM
		[batchesp].[EspEnrollments]
	WHERE
		BorrowerSsn = @BorrowerIdentifier
		OR AccountNumber = @BorrowerIdentifier
	GROUP BY
		BorrowerSsn

	UPDATE
		[batchesp].[EspEnrollments]
	SET
		ProcessedAt = NULL, ProcessedBy = NULL, RequiresReview = 0
	WHERE
		EspEnrollmentId = @EspEnrollmentId

--SET @DATE = (SELECT MAX(CAST(CreatedAt AS DATE)) FROM [batchesp].[Ts01Enrollments] WHERE BorrowerSsn = @Ssn)

	UPDATE
		[batchesp].[Ts01Enrollments]
	SET
		ProcessedAt = NULL, ProcessedBy = NULL
	WHERE
		BorrowerSsn = @Ssn
		AND CreatedAt >= @Date

--SET @DATE = (SELECT MAX(CAST(CreatedAt AS DATE)) FROM [batchesp].Ts26LoanInformation WHERE BorrowerSsn = @Ssn)

	UPDATE
		[batchesp].[Ts26LoanInformation]
	SET
		ProcessedAt = NULL, ProcessedBy = NULL
	WHERE
		BorrowerSsn = @Ssn
		AND CreatedAt >= @Date

--SET @DATE = (SELECT MAX(CAST(CreatedAt AS DATE)) FROM [batchesp].Ts2hPendingDisbursements WHERE BorrowerSsn = @Ssn)

	UPDATE
		[batchesp].[Ts2hPendingDisbursements]
	SET
		ProcessedAt = NULL, ProcessedBy = NULL
	WHERE
		BorrowerSsn = @Ssn
		AND CreatedAt >= @Date
	
--SET @DATE = (SELECT MAX(CAST(CreatedAt AS DATE)) FROM [batchesp].TsayDefermentForbearances WHERE BorrowerSsn = @Ssn)

	UPDATE
		[batchesp].[TsayDefermentForbearances]
	SET
		ProcessedAt = NULL, ProcessedBy = NULL
	WHERE
		BorrowerSsn = @Ssn
		AND CreatedAt >= @Date

--SET @DATE = (SELECT MAX(CAST(CreatedAt AS DATE)) FROM [batchesp].ParentPlusLoanDetails WHERE BorrowerSsn = @Ssn)

	UPDATE
		[batchesp].[ParentPlusLoanDetails]
	SET
		ProcessedAt = NULL, ProcessedBy = NULL
	WHERE
		BorrowerSsn = @Ssn
		AND CreatedAt >= @Date

RETURN 0
