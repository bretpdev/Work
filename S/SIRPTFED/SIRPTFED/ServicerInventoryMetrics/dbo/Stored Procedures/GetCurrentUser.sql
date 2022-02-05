CREATE PROCEDURE [dbo].[GetCurrentUser]
	@WindowsuserId VARCHAR(100)

AS
	SELECT
		AllowedUserId,
		AllowedUser, 
		IsAdmin
	FROM	
		AllowedUsers
	WHERE
		AllowedUser = @WindowsuserId
RETURN 0