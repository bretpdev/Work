CREATE PROCEDURE [compfafsa].[GetUserPasswordHash]
	@Email VARCHAR(256)
AS

	SELECT
		HashedPassword
	FROM
		Users
	WHERE
		EmailAddress = @Email
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL
RETURN 0