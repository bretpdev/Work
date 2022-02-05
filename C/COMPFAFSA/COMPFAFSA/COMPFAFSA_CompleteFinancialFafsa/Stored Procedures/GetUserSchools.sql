CREATE PROCEDURE [compfafsa].[GetUserSchools]
	@UserId INT
AS
	SELECT
		US.UserId,
		US.SchoolId,
		S.SchoolName AS School
	FROM
		compfafsa.UserSchools US
		INNER JOIN compfafsa.Schools S
			ON S.MasterSchoolListId = US.SchoolId
			AND S.DeletedAt IS NULL
			AND S.DeletedBy IS NULL
	WHERE
		US.UserId = @UserId
RETURN 0
