
CREATE PROCEDURE [monitor].[GetMonitorSettings]
AS
	
select
	MonitorSettingsId, MaxIncrease, MaxForce, MaxPreNote
from
	monitor.MonitorSettings

RETURN 0