CREATE PROCEDURE [dbo].[GetCurrentWindowsUserToken]
AS
	
	SELECT
		GeneratedToken
	FROM
		webapi.UserTokens
	WHERE
		AssociatedWindowsUsername = SYSTEM_USER
		OR
		AssociatedWindowsUsername = REPLACE(SYSTEM_USER, 'UHEAA\', '')

RETURN 0
