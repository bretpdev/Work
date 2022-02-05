
/********************************************************
*Routine Name	: [dbo].[spFLOW_GetCalculationID]
*Purpose		: Returns a list of CalculationID's for the given system
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/03/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spFLOW_GetCalculationID]
	-- Add the parameters for the stored procedure here
	  @System varchar(30) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CalculationID FROM Flow_LST_StaffCalculationID
		  WHERE [System] = @System 

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetCalculationID] TO [db_executor]
    AS [dbo];

