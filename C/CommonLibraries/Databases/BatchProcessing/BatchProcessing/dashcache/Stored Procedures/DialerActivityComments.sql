CREATE PROCEDURE [dashcache].[DialerActivityComments]
AS
SELECT
	CASE WHEN COUNT(*) > 1 THEN 0 ELSE 1 END [UHEAACount]
FROM	
	[NobleCalls].[dbo].[NobleCallHistory] NCH
WHERE
	NCH.CreatedAt > DATEADD(HOUR, -3, GETDATE())