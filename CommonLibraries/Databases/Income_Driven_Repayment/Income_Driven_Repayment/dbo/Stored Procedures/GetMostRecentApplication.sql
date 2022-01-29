CREATE PROCEDURE [dbo].[GetMostRecentApplication]
	@ApplicationId CHAR(10) = NULL
AS
	SELECT
		MAX(application_id) as application_id
	FROM
		dbo.Applications
	WHERE
		e_application_id = @ApplicationId
	
RETURN 0