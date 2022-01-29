CREATE PROCEDURE [dbo].[SpecifySetting]
	@SettingId int,
	@Name nvarchar(64),
	@Value sql_variant,
	@DisplayOrdinal int
AS
	if not exists(select * from dbo.Settings where SettingID = @SettingId)
	 begin
		   insert into dbo.Settings (SettingId, Name, Value, DisplayOrdinal)
		   values (@SettingId, @Name, @Value, @DisplayOrdinal)
	   end
	update dbo.Settings
	   set Name = @Name,
	       Value = @Value,
		   DisplayOrdinal = @DisplayOrdinal
	 where SettingId = @SettingId
RETURN 0
