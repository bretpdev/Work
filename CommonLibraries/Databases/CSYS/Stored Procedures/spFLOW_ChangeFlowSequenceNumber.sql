CREATE PROCEDURE [dbo].[spFLOW_ChangeFlowSequenceNumber] 
	@FlowID					VARCHAR(50),
	@CurrentSequenceNumber	INT,
	@NewSequenceNumber		INT
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE dbo.FLOW_DAT_FlowStep
	SET FlowStepSequenceNumber = @NewSequenceNumber
	WHERE FlowID = @FlowID AND FlowStepSequenceNumber = @CurrentSequenceNumber

END

GO
