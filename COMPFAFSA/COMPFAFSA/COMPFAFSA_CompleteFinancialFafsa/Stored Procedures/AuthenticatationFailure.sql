CREATE PROCEDURE [compfafsa].[AuthenticationFailure]
	@EmailAddress varchar(250)
AS
	DECLARE @Result VARCHAR(20) = 'FAILURE'
	DECLARE @FoundEmail BIT = 0
	DECLARE @MaxFailedAttempts INT = (SELECT LoginAttemptsBeforeLockout FROM ConfigurationVariables)
	IF EXISTS (SELECT * FROM Users WHERE EmailAddress = @EmailAddress AND DeletedAt IS NULL AND DeletedBy IS NULL)
	BEGIN
		SET @FoundEmail = 1
		DECLARE @FailedAttempts int = (SELECT FailedLogonAttempts FROM Users WHERE EmailAddress = @EmailAddress AND DeletedAt IS NULL AND DeletedBy IS NULL)
		IF @FailedAttempts >= @MaxFailedAttempts
			BEGIN
				SET @Result = 'LOCKOUT'
				--RAISERROR('ACCOUNT IS LOCKED OUT', 16,1)
			END
		ELSE
			BEGIN

				UPDATE
					Users
				SET
					FailedLogonAttempts = FailedLogonAttempts + 1
				WHERE
					EmailAddress = @EmailAddress
					AND DeletedAt IS NULL
					AND DeletedBy IS NULL

				SET @Result = 'FAILURE'
				--RAISERROR('INVALID USERNAME OR PASSWORD', 16,1)
			END
	END

	IF @FoundEmail = 0
	BEGIN
		SET @Result = 'FAILURE'
		--RAISERROR('INVALID USERNAME OR PASSWORD', 16,1)
	END
	
	SELECT @Result

RETURN 0