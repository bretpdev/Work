-- =============================================
-- Author:		Bret Pehrson
-- Create date: 9/18/2012
-- Description:	Returns the highest sequence number for given flow
-- =============================================
CREATE PROCEDURE [dbo].[spFLOW_GetMaxSequenceNumber] 
	-- Add the parameters for the stored procedure here
	@FlowID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MAX(FlowStepSequenceNumber) FROM FLOW_DAT_FlowStep
	WHERE FlowID = @FlowID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetMaxSequenceNumber] TO [db_executor]
    AS [dbo];

