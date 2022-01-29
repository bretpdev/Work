CREATE PROCEDURE [dbo].[UpdateSetting]
	@SettingId int = 0,
	@Value sql_variant
AS
	update dbo.Settings
	   set Value = @Value
	 where SettingId = @SettingId
RETURN 0

grant execute on [dbo].[UpdateSetting] to [db_executor]