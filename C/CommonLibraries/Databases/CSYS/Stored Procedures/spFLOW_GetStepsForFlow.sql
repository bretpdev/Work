CREATE PROCEDURE [dbo].[spFLOW_GetStepsForFlow] 
	@FlowID		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		fs.FlowID,
		fs.FlowStepSequenceNumber,
		fs.AccessAlsoBasedOffBusinessUnit,
		fs.AccessKey,
		fs.NotificationType,
		COALESCE(fs.StaffAssignment, 0) AS StaffAssignment,
		fs.StaffAssignmentCalculationID,
		fs.ControlDisplayText,
		fs.[Description],
		fs.DataValidationID,
		fs.[Status],
		CASE WHEN fs.StaffAssignment IS NULL THEN NULL ELSE u.FirstName + ' ' + u.LastName END AS StaffAssignmentLegalName
	FROM FLOW_DAT_FlowStep fs
		LEFT OUTER JOIN dbo.SYSA_DAT_Users u
			ON fs.StaffAssignment = u.SqlUserId
	WHERE fs.FlowID = @FlowID
	ORDER BY fs.FlowStepSequenceNumber
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetStepsForFlow] TO [db_executor]
    AS [dbo];


