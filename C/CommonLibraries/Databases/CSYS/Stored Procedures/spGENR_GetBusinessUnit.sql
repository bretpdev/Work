
/********************************************************
*Routine Name	: [dbo].[spGENR_GetBusinessUnit]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		08/13/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spGENR_GetBusinessUnit]
	-- Add the parameters for the stored procedure here
	  @SqlUserID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT a.[ID], a.[Name]
	FROM GENR_LST_BusinessUnits a
	JOIN SYSA_DAT_Users b
	ON a.ID = b.BusinessUnit
	WHERE SqlUserId = @SqlUserID

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetBusinessUnit] TO [db_executor]
    AS [dbo];

