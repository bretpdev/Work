CREATE PROCEDURE [dbo].[SpecifySetting]
	@SettingId INT,
	@Name NVARCHAR(64),
	@Value SQL_VARIANT,
	@DisplayOrdinal INT
AS
	
IF NOT EXISTS(SELECT * FROM dbo.Settings WHERE SettingID = @SettingId)
	BEGIN
		INSERT INTO dbo.Settings (SettingId, [Name], [Value], DisplayOrdinal)
		VALUES(@SettingId, @Name, @Value, @DisplayOrdinal)
	END

UPDATE 
	dbo.Settings
SET
	[Name] = @Name,
	[Value] = @Value,
	DisplayOrdinal = @DisplayOrdinal
WHERE 
	SettingId = @SettingId
