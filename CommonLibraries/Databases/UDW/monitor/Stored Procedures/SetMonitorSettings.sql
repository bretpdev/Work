CREATE PROCEDURE [monitor].[SetMonitorSettings]
	@MaxIncrease MONEY,
	@MaxForce INT,
	@MaxPreNote INT,
	@LastRecoveryPage INT = NULL
AS

	UPDATE
		monitor.MonitorSettings
	SET
		MaxIncrease = @MaxIncrease,
		MaxForce = @MaxForce,
		MaxPreNote = @MaxPreNote,
		LastRecoveryPage = @LastRecoveryPage

RETURN 0