CREATE PROCEDURE [compfafsa].[CreateResetPassword]
	@Email VARCHAR(250),
	@HashedResetPassword VARCHAR(500)
AS

DECLARE 
		@Success INT = 1, 
		@Failure INT = 0

BEGIN TRANSACTION
BEGIN TRY
	UPDATE compfafsa.Users
	SET
		HashedResetPassword = @HashedResetPassword,
		ResetPasswordCreated = GETDATE()
	WHERE
		EmailAddress = @Email
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL

	COMMIT TRANSACTION
	SELECT @Success
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	SELECT @Failure
END CATCH