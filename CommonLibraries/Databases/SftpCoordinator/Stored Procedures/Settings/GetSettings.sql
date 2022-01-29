CREATE PROCEDURE [dbo].[GetSettings]
AS
	select SettingId, Name, Value from Settings
	order by DisplayOrdinal
RETURN 0

grant execute on [dbo].[GetSettings] to [db_executor]