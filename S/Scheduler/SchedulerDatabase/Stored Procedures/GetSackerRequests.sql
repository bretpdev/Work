CREATE PROCEDURE GetSackerRequests
AS
BEGIN

	SELECT
		RT.RequestType,
		RT.RequestTypeId,
		SC.Id [RequestId],
		SC.DevEstimate,
		SC.TestEstimate,
		SC.AssignedProgrammer [AssignedDeveloper],
		SC.AssignedTester,
		RP.EstimatedDevStartDate [DevStartDate],
		RP.EstimatedDevEndDate [DevEndDate],
		RP.EstimatedTestStartDate [TesterStartDate],
		RP.EstimatedTestEndDate [TesterEndDate],
		SC.Status [CurrentStatus]
	FROM
		SackerCache SC
		JOIN
		RequestTypes RT on RT.RequestTypeId = SC.RequestTypeId
		LEFT JOIN
		RequestPriorities RP ON RP.RequestId = SC.Id AND RP.RequestTypeId = SC.RequestTypeId
	WHERE 
		SC.Status NOT IN ('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')

END