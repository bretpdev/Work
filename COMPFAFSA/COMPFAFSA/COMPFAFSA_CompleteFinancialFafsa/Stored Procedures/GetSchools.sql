CREATE PROCEDURE [compfafsa].[GetSchools]
AS
	SELECT 
		SchoolName AS School
	FROM
		compfafsa.Schools
	WHERE
		DeletedAt IS NULL
		AND DeletedBy IS NULL
RETURN 0
