CREATE TABLE [monitor].[MonitorSettings] (
    [MonitorSettingsId] INT   IDENTITY (1, 1) NOT NULL,
    [MaxIncrease]       MONEY NOT NULL,
    [MaxForce]          INT   NOT NULL,
    [MaxPreNote]        INT   NOT NULL,
    PRIMARY KEY CLUSTERED ([MonitorSettingsId] ASC)
);

