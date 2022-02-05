CREATE PROCEDURE [compfafsa].[AddUserSchool]
	@UserId INT,
	@School VARCHAR(200)
AS
BEGIN TRANSACTION
BEGIN TRY
	INSERT INTO compfafsa.UserSchools(UserId, SchoolId)
	SELECT
		@UserId,
		MasterSchoolListId
	FROM
		compfafsa.Schools S
		LEFT JOIN compfafsa.UserSchools US
			ON US.UserId = @UserId
			AND US.SchoolId = S.MasterSchoolListId
	WHERE
		SchoolName = @School
		AND US.SchoolId IS NULL
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL

	COMMIT TRANSACTION

	SELECT 1
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION

	SELECT 0
END CATCH
RETURN 0
