CREATE PROCEDURE [compfafsa].[GetUserLockedOut]
	@Email VARCHAR(200)
AS
	DECLARE
		@Locked BIT = 1,
		@Unlocked BIT = 0,
		@MaxFailedAttempts INT = (SELECT LoginAttemptsBeforeLockout FROM ConfigurationVariables)
	IF EXISTS (SELECT * FROM Users WHERE EmailAddress = @Email AND FailedLogonAttempts < @MaxFailedAttempts)
		BEGIN
			SELECT @Unlocked
		END
	ELSE
		BEGIN
			SELECT @Locked
		END
		
RETURN 0
