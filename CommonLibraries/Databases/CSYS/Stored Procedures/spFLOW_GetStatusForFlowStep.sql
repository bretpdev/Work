
/********************************************************
*Routine Name	: [dbo].[spFLOW_GetStatusForFlowStep]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/31/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spFLOW_GetStatusForFlowStep]
	-- Add the parameters for the stored procedure here
	  @FlowID varchar(50) = ''
	, @FlowStepSequenceNumber int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Status] FROM FLOW_DAT_FlowStep
	WHERE FlowID = @FlowID
	AND FlowStepSequenceNumber = @FlowStepSequenceNumber

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetStatusForFlowStep] TO [db_executor]
    AS [dbo];

