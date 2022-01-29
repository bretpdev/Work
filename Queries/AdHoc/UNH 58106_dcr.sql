--run on UHEAASQLDB
USE ProcessLogs
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 1414
--1414
UPDATE
	PN
SET
	PN.ResolvedAt = GETDATE(),
	PN.ResolvedBy = 'UNH 58106'
FROM
	ProcessLogs..ProcessLogs PL 
	INNER JOIN ProcessLogs..ProcessNotifications PN 
		ON PN.ProcessLogId = PL.ProcessLogId 
	INNER JOIN ULS.log.ProcessLogMessages PLM 
		ON PLM.ProcessNotificationId = PN.ProcessNotificationId 
WHERE
	PL.ScriptId = 'ARCADDPROC' 
	AND PN.ResolvedAt IS NULL 
	AND PLM.LogMessage LIKE '%ACDV5%' 
	AND PL.StartedOn > '2018-08-01'

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END