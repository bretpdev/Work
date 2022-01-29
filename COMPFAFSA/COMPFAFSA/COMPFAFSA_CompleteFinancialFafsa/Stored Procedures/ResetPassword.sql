CREATE PROCEDURE [compfafsa].[ResetPassword]
	@UserId INT,
	@HASHEDPW VARCHAR(500)
AS

	DECLARE @Success INT = 1
	DECLARE @UserNotFound INT = 2

	IF NOT EXISTS(SELECT * FROM Users WHERE @UserId = UserId)
	BEGIN
		SELECT @UserNotFound

	END

	ELSE
	BEGIN

		UPDATE Users
		SET 
			HashedPassword = @HASHEDPW,
			PasswordLastUpdated = GETDATE(),
			PasswordSet = 1
		WHERE
			UserId = @UserId

		SELECT @Success

	END
