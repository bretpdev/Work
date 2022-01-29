CREATE PROCEDURE [compfafsa].[GetUserIsAdmin]
	@EmailAddress VARCHAR(250)
AS
	SELECT
		U.[Admin]
	FROM
		[compfafsa].Users U
	WHERE
		U.EmailAddress = @EmailAddress
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL
		
RETURN 0
