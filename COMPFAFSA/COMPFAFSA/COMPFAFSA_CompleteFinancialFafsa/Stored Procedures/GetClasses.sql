CREATE PROCEDURE [compfafsa].[GetClasses]
AS
	SELECT
		Class
	FROM
		compfafsa.Classes
	WHERE
		DeletedAt IS NULL
		AND DeletedBy IS NULL
RETURN 0
