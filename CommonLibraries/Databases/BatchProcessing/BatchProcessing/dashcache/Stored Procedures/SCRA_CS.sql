CREATE PROCEDURE [dashcache].[SCRA_CS]
AS

	SELECT
		CASE WHEN COUNT(*) > 1 THEN 0 ELSE 1 END -- if no records have been updated in the last 40 days return "1" to indicat a problem
	FROM
		CLS.scra.ActiveDuty AD
	WHERE
		AD.NotificationDate > DATEADD(DAY, -40, GETDATE())

RETURN 0