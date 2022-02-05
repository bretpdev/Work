CREATE PROCEDURE [compfafsa].[CreateAccount]
	@FullName VARCHAR(300),
	@EMAILADDRESS VARCHAR(250),
	@HASHEDPW VARCHAR(500),
	@Admin BIT
AS

	DECLARE @Success INT = 1
	DECLARE @EmailAlreadyFound INT = 2

	IF EXISTS(SELECT * FROM Users WHERE EmailAddress = @EMAILADDRESS AND DeletedAt IS NULL AND DeletedBy IS NULL)
	BEGIN
		SELECT @EmailAlreadyFound

	END
	ELSE
	BEGIN

		INSERT INTO Users(FullName, EmailAddress, HashedPassword, LastLogin, PasswordLastUpdated, FailedLogonAttempts, [Admin])
		VALUES(@FullName, @EMAILADDRESS, @HASHEDPW, GETDATE(), GETDATE(), 0, @Admin)

		SELECT @Success

	END