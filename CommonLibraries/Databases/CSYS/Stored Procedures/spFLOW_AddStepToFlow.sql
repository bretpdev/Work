CREATE PROCEDURE [dbo].[spFLOW_AddStepToFlow] 
	@FlowID									VARCHAR(50),
	@FlowStepSequenceNumber					INT,
	@AccessAlsoBasedOffBusinessUnit			BIT,
	@ControlDisplayText						VARCHAR(200),
	@Description							VARCHAR(8000),
	@Status									VARCHAR(50),
	@AccessKey								VARCHAR(100) = null,
	@NotificationType						VARCHAR(100) = null,
	@StaffAssignment						INT = null,
	@StaffAssignmentCalculationID			VARCHAR(100) = null,
	@DataValidationID						VARCHAR(50) = null
AS
BEGIN

	SET NOCOUNT ON;
	
	if @StaffAssignment = 0
	SET @StaffAssignment = null

	INSERT INTO dbo.FLOW_DAT_FlowStep
	(FlowID, FlowStepSequenceNumber, AccessAlsoBasedOffBusinessUnit, AccessKey, NotificationType, StaffAssignment, StaffAssignmentCalculationID, ControlDisplayText, [Description], DataValidationID, [Status])
	VALUES 
	(@FlowID, @FlowStepSequenceNumber, @AccessAlsoBasedOffBusinessUnit, @AccessKey, @NotificationType, @StaffAssignment, @StaffAssignmentCalculationID, @ControlDisplayText, @Description, @DataValidationID, @Status)

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_AddStepToFlow] TO [db_executor]
    AS [dbo];



