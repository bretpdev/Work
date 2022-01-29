CREATE PROCEDURE [rcdialer].[GetUnprocessedRecords]
AS
	SELECT
		OutboundCallsId,
		RCID,
		ServicerId,
		FirstName,
		LastName,
		Address1,
		Address2,
		City,
		[State],
		Zip,
		Email,
		HomePhone,
		WorkPhone,
		CellPhone,
		MonthlyRepaymentAmount,
		SchoolCode,
		SchoolName,
		DaysDelinquent,
		DelinquentBucket
	FROM
		rcdialer.OutboundCalls
	WHERE
		DeletedAt IS NULL
		AND ProcessedAt IS NULL