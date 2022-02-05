CREATE PROCEDURE [hrbridge].[GetCompensation]
AS
	--Update the older records so that they don't get consolidated with Bridge
	SELECT
		EmployeeId,
		UpdatedAt,
		StartDate,
		TRY_CONVERT(VARCHAR(500), Rate),
		[Type],
		Exempt,
		Reason,
		Comment,
		PaidPer,
		PaySchedule
	FROM
		hrbridge.Compensation_BambooHR
	WHERE
		NeedsUpdated = 1

