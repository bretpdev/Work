CREATE PROCEDURE [compfafsa].[GetUserPasswordResetHash]
	@Email VARCHAR(256)
AS

	SELECT
		HashedResetPassword,
		CASE WHEN DATEDIFF(MINUTE, ResetPasswordCreated, GETDATE()) <= 15 THEN 0 ELSE 1 END AS Expired
	FROM
		Users
	WHERE
		EmailAddress = @Email
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL

RETURN 0