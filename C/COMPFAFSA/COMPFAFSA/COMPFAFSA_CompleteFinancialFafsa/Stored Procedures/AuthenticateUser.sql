CREATE PROCEDURE [compfafsa].[AuthenticateUser]
	@EmailAddress varchar(250),
	@HashedPassword varchar(500)
AS
	DECLARE @Result VARCHAR(30) = 'FAILURE'
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
				DECLARE @STORED_PW VARCHAR(500) = (SELECT HashedPassword FROM Users WHERE EmailAddress = @EmailAddress AND DeletedAt IS NULL AND DeletedBy IS NULL)

				IF(@STORED_PW != @HashedPassword)
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
				ELSE
					BEGIN
						UPDATE
							Users
						SET
							LastLogin = GETDATE(),
							FailedLogonAttempts = 0
						WHERE
							EmailAddress = @EmailAddress
							AND DeletedAt IS NULL
							AND DeletedBy IS NULL
						
						DECLARE @PasswordSet BIT = (SELECT PasswordSet FROM Users WHERE EmailAddress = @EmailAddress AND DeletedAt IS NULL AND DeletedBy IS NULL)
						SET @Result = CASE WHEN @PasswordSet = 1 THEN 'SUCCESS' ELSE 'SUCCESSNEEDSETPASSWORD' END
					END
			END
	END

	IF @FoundEmail = 0
	BEGIN
		SET @Result = 'FAILURE'
		--RAISERROR('INVALID USERNAME OR PASSWORD', 16,1)
	END

	SELECT @Result

RETURN 0