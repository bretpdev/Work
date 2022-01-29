
/********************************************************
*Routine Name	: [dbo].[spFLOW_GetInterfaces]
*Purpose		: Returns a list of interfaces available to ACDC
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		06/28/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spFLOW_GetInterfaces]
	@System varchar(30) = ''
AS
	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Interface, [System] FROM FLOW_LST_Interface WHERE [System] = @System

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetInterfaces] TO [db_executor]
    AS [dbo];

