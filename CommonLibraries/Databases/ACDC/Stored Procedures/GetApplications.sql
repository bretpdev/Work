CREATE PROCEDURE [dbo].[GetApplications]
AS
	SELECT
		app.ApplicationId,
		app.ApplicationName,
		app.AccessKey,
		app.SourcePath,
		app.StartingDll,
		app.StartingClass
	FROM
		APPLICATIONS app
	WHERE
		RemovedOn IS NULL
RETURN 0

GRANT EXECUTE ON [GetApplications] TO [db_exector]