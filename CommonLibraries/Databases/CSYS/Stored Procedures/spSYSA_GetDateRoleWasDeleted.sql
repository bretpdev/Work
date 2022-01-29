-- =============================================
-- Author:		Bret Pehrson
-- Create date: 11/6/2012
-- Description:	Returns the date a role was deleted
-- =============================================
CREATE PROCEDURE [dbo].[spSYSA_GetDateRoleWasDeleted] 
	-- Add the parameters for the stored procedure here
	@RoleID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EndDate
	FROM SYSA_LST_Role
	WHERE RoleID = @RoleID
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetDateRoleWasDeleted] TO [db_executor]
    AS [dbo];

