CREATE PROCEDURE [finalrev].[InsertSchool]
	@SchoolCode VARCHAR(10)
AS
	IF NOT EXISTS (SELECT SchoolCode FROM finalrev.Schools WHERE SchoolCode = @SchoolCode)
		BEGIN
			INSERT INTO finalrev.Schools(SchoolCode)
			VALUES(@SchoolCode)
		END

	SELECT
		SchoolsId
	FROM
		finalrev.Schools
	WHERE
		SchoolCode = @SchoolCode