
-- =============================================
-- Author:		Bret Pehrson
-- Create date: 10/04/2012
-- Description:	Returns the Role for the given user
-- =============================================
CREATE PROCEDURE [dbo].[spACDC_GetUserAssignedRole] 
	-- Add the parameters for the stored procedure here
	@WindowsUserName varchar(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT b.RoleName FROM SYSA_DAT_Users a
	JOIN SYSA_LST_Role b
	ON a.[Role] = b.RoleID
	WHERE a.WindowsUserName = @WindowsUserName
	AND a.Status = 'Active'
END