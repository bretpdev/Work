CREATE TABLE [dbo].[Settings]
(
	[SettingId] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(64) NOT NULL, 
    [Value] SQL_VARIANT NOT NULL, 
    [DisplayOrdinal] INT NOT NULL default(0)
)
