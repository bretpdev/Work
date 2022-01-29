CREATE PROCEDURE [dbo].[spFLOW_DeleteFlowStep] 
	@FlowID					VARCHAR(50),
	@FlowStepSequenceNumber	INT
AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM dbo.FLOW_DAT_FlowStep WHERE FlowID = @FlowID AND FlowStepSequenceNumber = @FlowStepSequenceNumber

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_DeleteFlowStep] TO [db_executor]
    AS [dbo];


GO
