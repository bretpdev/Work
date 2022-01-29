
/********************************************************
*Routine Name	: [dbo].[spFLOW_GetFinalStatusForFlowStep]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/31/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spFLOW_GetFinalStatusForFlowStep]
	-- Add the parameters for the stored procedure here
	  @FlowID Varchar(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Status] FROM FLOW_DAT_FlowStep
	WHERE FlowID = @FlowID
	AND FlowStepSequenceNumber = 
		(SELECT MAX(FlowStepSequenceNumber) FROM FLOW_DAT_FlowStep WHERE FlowID = @FlowID)

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetFinalStatusForFlowStep] TO [db_executor]
    AS [dbo];

