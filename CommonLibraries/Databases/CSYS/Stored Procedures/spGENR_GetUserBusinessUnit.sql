
/********************************************************
*Routine Name	: [dbo].[spGENR_GetUserBusinessUnit]
*Purpose		:  
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/31/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spGENR_GetUserBusinessUnit]
	-- Add the parameters for the stored procedure here
	  @SqlUserID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT BusinessUnit FROM SYSA_DAT_Users
	WHERE SqlUserId = @SqlUserID

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetUserBusinessUnit] TO [db_executor]
    AS [dbo];

