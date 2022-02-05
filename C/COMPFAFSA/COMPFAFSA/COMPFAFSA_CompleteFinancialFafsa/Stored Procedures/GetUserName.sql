CREATE PROCEDURE [compfafsa].[GetUserName]
	@UserId INT

AS
	
	SELECT 
		FullName
	FROM
		compfafsa.Users
	WHERE
		UserId = @UserId
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL


RETURN 0
