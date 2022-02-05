CREATE PROCEDURE [duedtecng].[GetAppSettings]
AS

	SELECT
		MaxIncrease
	FROM
		DUEDTECNG.AppSettings

RETURN 0
