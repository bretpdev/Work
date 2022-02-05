CREATE PROCEDURE [dbo].[spFLOW_GetFlowStepsForUser] 
	@UserID					INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	fs.FlowID,
			fs.FlowStepSequenceNumber,
			fs.AccessAlsoBasedOffBusinessUnit,
			fs.AccessKey,
			fs.NotificationType,
			fs.StaffAssignment,
			fs.ControlDisplayText,
			fs.Description as StepDescription,
			f.Description as FlowDescription,
			f.System as TheSystem,
			fs.Status,
			Coalesce(u.FirstName+' '+u.LastName, '') as StaffAssignmentLegalName
	FROM	dbo.FLOW_DAT_FlowStep fs
			INNER JOIN dbo.FLOW_DAT_Flow f
				ON fs.FlowID = f.FlowID
			LEFT JOIN dbo.SYSA_DAT_Users u
				on fs.StaffAssignment = u.SqlUserId
	WHERE	fs.StaffAssignment = @UserId
    
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetFlowStepsForUser] TO [db_executor]
    AS [dbo];


