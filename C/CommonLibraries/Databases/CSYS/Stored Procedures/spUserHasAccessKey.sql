-- =============================================
-- Author:		Bret Pehrson
-- Create date: 02/08/2013
-- Description:	Returns a list of keys assigned to a user
-- =============================================
CREATE PROCEDURE [dbo].[spUserHasAccessKey]
	@WindowsUserName VARCHAR(50),
	@UserKey VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Result bit = 0
	SELECT
		@Result = 1
	FROM SYSA_DAT_RoleKeyAssignment RoleKeys
		JOIN SYSA_DAT_Users Users
			ON RoleKeys.RoleID = Users.[Role]
		JOIN SYSA_LST_UserKeys Keys
			ON RoleKeys.UserKeyID = Keys.ID
	WHERE Users.WindowsUserName = @WindowsUserName
		AND RoleKeys.EndDate IS NULL
		AND @UserKey = Keys.UserKey
	ORDER BY RoleKeys.UserKeyID
	
	SELECT @Result
END
