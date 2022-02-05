
/********************************************************
*Routine Name	: [dbo].[spSYSA_CheckRoleExistance]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/25/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_CheckRoleExistence]
	-- Add the parameters for the stored procedure here
	  @RoleName nvarchar(64)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT RoleName FROM SYSA_LST_Role
	WHERE RoleName = @RoleName
	AND EndDate IS NULL

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_CheckRoleExistence] TO [db_executor]
    AS [dbo];

