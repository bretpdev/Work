-- =============================================
-- Author:		Bret Pehrson
-- Create date: 08/28/2012
-- Description:	Updates user roles to a new role
-- =============================================
CREATE PROCEDURE spSYSA_UpdateUserRole 
	-- Add the parameters for the stored procedure here
	@SqlUserID int = 0, 
	@RoleID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE SYSA_DAT_Users
	SET [Role] = @RoleID
	WHERE SqlUserId = @SqlUserID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_UpdateUserRole] TO [db_executor]
    AS [dbo];

