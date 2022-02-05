CREATE PROCEDURE [compfafsa].[DeleteUserSchool]
	@UserId INT,
	@SchoolId INT
AS

DECLARE @Success INT = 1,
@Failure INT = 0

BEGIN TRANSACTION
BEGIN TRY
	DELETE FROM compfafsa.UserSchools
	WHERE
		UserId = @UserId
		AND SchoolId = @SchoolId

	COMMIT TRANSACTION

	SELECT @Success
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION

	SELECT @Failure
END CATCH
RETURN 0
