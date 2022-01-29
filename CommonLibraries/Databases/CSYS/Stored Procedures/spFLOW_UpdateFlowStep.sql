CREATE PROCEDURE [dbo].[spFLOW_UpdateFlowStep] 
	@FlowID									VARCHAR(50),
	@FlowStepSequenceNumber					INT,
	@AccessAlsoBasedOffBusinessUnit			BIT,
	@ControlDisplayText						VARCHAR(200),
	@Description							VARCHAR(8000),
	@Status									VARCHAR(50),
	@AccessKey								VARCHAR(100) = null,
	@NotificationType						VARCHAR(100) = null,
	@StaffAssignment						INT = null,
	@StaffAssignmentCalculationID			VARCHAR(50) = null,
	@DataValidationID						VARCHAR(50) = null
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE dbo.FLOW_DAT_FlowStep
	SET AccessAlsoBasedOffBusinessUnit = @AccessAlsoBasedOffBusinessUnit, 
		AccessKey = @AccessKey, 
		NotificationType = @NotificationType, 
		StaffAssignment = @StaffAssignment, 
		StaffAssignmentCalculationID = @StaffAssignmentCalculationID,
		ControlDisplayText = @ControlDisplayText, 
		[Description] = @Description,
		DataValidationID = @DataValidationID,
		[Status] = @Status
	WHERE FlowID = @FlowID AND FlowStepSequenceNumber = @FlowStepSequenceNumber

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_UpdateFlowStep] TO [db_executor]
    AS [dbo];


