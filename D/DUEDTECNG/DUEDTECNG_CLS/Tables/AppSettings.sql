CREATE TABLE [duedtecng].[AppSettings]
(
	[AppSettingsId] INT PRIMARY KEY,
	[MaxIncrease] DECIMAL(9, 2) NOT NULL, 
    CONSTRAINT [CK_AppSettings_OneRow] CHECK (AppSettingsId = 1) --only allow one row for this table
)
