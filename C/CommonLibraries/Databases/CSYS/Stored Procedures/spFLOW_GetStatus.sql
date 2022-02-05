
/********************************************************
*Routine Name	: [dbo].[spFLOW_GetStatus]
*Purpose		: Returns a distinct list of Statuses
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/06/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spFLOW_GetStatus]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT [Status] FROM FLOW_DAT_FlowStep

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetStatus] TO [db_executor]
    AS [dbo];

