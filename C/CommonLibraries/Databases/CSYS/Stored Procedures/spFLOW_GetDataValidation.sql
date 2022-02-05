
/********************************************************
*Routine Name	: [dbo].[spFLOW_GetDataValidation]
*Purpose		: Returns list of Data Validtion ID's
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/06/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spFLOW_GetDataValidation]
	-- Add the parameters for the stored procedure here
	  @System varchar(30) = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Name] FROM FLOW_LST_DataValidator
	WHERE [System] = @System

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetDataValidation] TO [db_executor]
    AS [dbo];

