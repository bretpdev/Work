CREATE PROCEDURE [compfafsa].[GetUserId]
	@Email VARCHAR(200)
AS

SELECT
	UserId
FROM
	compfafsa.Users
WHERE
	EmailAddress = @Email
	AND DeletedAt IS NULL
	AND DeletedBy IS NULL

RETURN 0
