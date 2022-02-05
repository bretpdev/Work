CREATE PROCEDURE [compfafsa].[GetExistingUsers]
AS
	DECLARE @MaxFailedAttempts INT = (SELECT LoginAttemptsBeforeLockout FROM ConfigurationVariables)

	SELECT 
		UserId,
		EmailAddress,
		FullName,
		[Admin],
		CASE WHEN U.FailedLogonAttempts >= @MaxFailedAttempts THEN 1 ELSE 0 END AS LockedOut
	FROM
		compfafsa.Users U
	WHERE
		U.DeletedAt IS NULL
		AND U.DeletedBy IS NULL

RETURN 0
