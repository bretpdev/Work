
/********************************************************
*Routine Name	: [dbo].[spSYSA_GetInactiveKeysForRole]
*Purpose		: Returns a list of user keys assigned to an inactive role
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		08/06/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_GetInactiveKeysForRole]
	-- Add the parameters for the stored procedure here
	  @RoleID int = 0
	, @DeleteDate datetime
	, @DeleteDatePlusMinute datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT RoleID, UserKeyID FROM SYSA_DAT_RoleKeyAssignment
	WHERE RoleID = @RoleID
	AND EndDate between @DeleteDate AND @DeleteDatePlusMinute

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetInactiveKeysForRole] TO [db_executor]
    AS [dbo];

