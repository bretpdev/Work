CREATE PROCEDURE [webapi].[GetWebApps]
AS
	
	SELECT
		WA.WebAppId,
		WA.AppName,
		WA.[Url]
	FROM
		webapps.WebApps WA
	WHERE
		WA.InactivatedAt IS NULL

RETURN 0
