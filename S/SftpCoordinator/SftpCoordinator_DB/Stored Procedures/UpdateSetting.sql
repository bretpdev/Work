CREATE PROCEDURE [dbo].[UpdateSetting]
	@SettingId INT = 0,
	@Value SQL_VARIANT
AS

UPDATE 
	dbo.Settings
SET 
	[Value] = @Value
WHERE 
	SettingId = @SettingId
