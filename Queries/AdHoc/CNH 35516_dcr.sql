--run on UHEAASQLDB
USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXXX
--XXXX
UPDATE
	PN
SET
	PN.ResolvedAt = GETDATE(),
	PN.ResolvedBy = 'CNH XXXXX'
FROM
	ProcessLogs..ProcessLogs PL 
	INNER JOIN ProcessLogs..ProcessNotifications PN 
		ON PN.ProcessLogId = PL.ProcessLogId 
	INNER JOIN CLS.log.ProcessLogMessages PLM 
		ON PLM.ProcessNotificationId = PN.ProcessNotificationId 
WHERE
	PL.ScriptId = 'ARCADDPROC' 
	AND PN.ResolvedAt IS NULL 
	AND PLM.LogMessage LIKE '%ACDVX%' 
	AND PL.StartedOn > 'XXXX-XX-XX'


	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END