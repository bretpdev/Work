CREATE PROCEDURE [compfafsa].[ChangePassword]
	@EMAILADDRESS VARCHAR(250),
	@HASHEDPW VARCHAR(500)
AS

	DECLARE @Success INT = 1
	DECLARE @EmailNotFound INT = 2

	IF NOT EXISTS(SELECT * FROM Users WHERE EmailAddress = @EMAILADDRESS)
	BEGIN
		SELECT @EmailNotFound

	END
	ELSE
	BEGIN

		UPDATE Users
		SET 
			HashedPassword = @HASHEDPW,
			PasswordLastUpdated = GETDATE(),
			PasswordSet = 1
		WHERE
			EmailAddress = @EMAILADDRESS

		SELECT @Success

	END