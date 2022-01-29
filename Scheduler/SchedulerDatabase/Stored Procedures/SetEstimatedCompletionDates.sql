CREATE PROCEDURE SetEstimatedCompletionDates
(
	@DevStartDate DATETIME,
	@DevEndDate DATETIME,
	@TestStartDate DATETIME,
	@TestEndDate DATETIME,
	@RequestId INT,
	@RequestTypeId INT
)
AS
BEGIN
	UPDATE
		RequestPriorities
	SET
		EstimatedDevStartDate = @DevStartDate,
		EstimatedDevEndDate = @DevEndDate,
		EstimatedTestStartDate = @TestStartDate,
		EstimatedTestEndDate = @TestEndDate
	WHERE
		RequestId = @RequestId
		AND
		RequestTypeId = @RequestTypeId

END

