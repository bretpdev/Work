CREATE PROCEDURE [monitor].[GetMonitorSettings]
AS
	
SELECT
	MonitorSettingsId, MaxIncrease, MaxForce, MaxPreNote, LastRecoveryPage
FROM
	monitor.MonitorSettings

RETURN 0
