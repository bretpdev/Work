
/********************************************************
*Routine Name	: [dbo].[spGENR_GetSystemList]
*Purpose		: Returns a list of systems
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		06/27/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spGENR_GetSystemList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [System] FROM GENR_LST_System

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetSystemList] TO [db_executor]
    AS [dbo];

