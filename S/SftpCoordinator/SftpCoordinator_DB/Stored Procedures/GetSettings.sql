CREATE PROCEDURE [dbo].[GetSettings]
AS

SELECT 
	SettingId, 
	[Name], 
	[Value]
FROM
	Settings
ORDER BY 
	DisplayOrdinal
