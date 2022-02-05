CREATE TABLE [monitor].[MonitorSettings]
(
	[MonitorSettingsId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MaxIncrease] MONEY NOT NULL, 
    [MaxForce] INT NOT NULL, 
    [MaxPreNote] INT NOT NULL, 
    [LastRecoveryPage] INT NULL
)
