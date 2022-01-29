CREATE PROCEDURE [compfafsa].[DeleteUser]
	@UserId INT,
	@DeletingUser VARCHAR(200)
AS

DECLARE @Success INT = 1,
@Failure INT = 0

BEGIN TRANSACTION
BEGIN TRY
	UPDATE compfafsa.Users
	SET
		DeletedAt = GETDATE(),
		DeletedBy = @DeletingUser
	WHERE
		UserId = @UserId 

	COMMIT TRANSACTION

	SELECT @Success
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION

	SELECT @Failure
END CATCH
RETURN 0
