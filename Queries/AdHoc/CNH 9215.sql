USE CLS 
GO 
 
BEGIN TRANSACTION 
	DECLARE @ERROR INT = X 
	DECLARE @ROWCOUNT INT = X 
 
INSERT INTO CLS.dbo.ArcAddProcessing(ArcTypeId, ArcResponseCodeId, AccountNumber, RecipientId, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeededBy, RegardsTo, RegardsCode, CreatedAt, CreatedBy, ProcessedAt)
SELECT 
	X, 
	null,
	SUBSTRING(PLM.LogMessage, XX, XX), 
	null, 
	SUBSTRING(PLM.LogMessage, XX, X), 
	'EMAILBTCF', 
	GETDATE(), 
	SUBSTRING(PLM.LogMessage, XX, XXX), 
	X, 
	X, 
	null, 
	null, 
	null, 
	'', 
	null, 
	GETDATE(), 
	'DCR', 
	null
FROM ProcessLogs.dbo.ProcessLogs PL
INNER JOIN ProcessLogs.dbo.ProcessNotifications PN 
ON PN.ProcessLogId = PL.ProcessLogId 
INNER JOIN CLS.log.ProcessLogMessages PLM
ON PLM.ProcessNotificationId = PN.ProcessNotificationId
WHERE PL.ProcessLogId ='XXXXXX' AND PN.NotificationSeverityTypeId = X
 

 	-- Save/Set the row count and error number (if any) from the previously executed statement 
 	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR 
 
 IF @ROWCOUNT = XXXX AND @ERROR = X 
 	BEGIN 
 		PRINT 'Transaction committed' 
 		COMMIT TRANSACTION 
 		--ROLLBACK TRANSACTION 
 	END 
 ELSE 
 	BEGIN 
 		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX)) 
 		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX)) 
 		PRINT 'Transaction NOT committed' 
 		ROLLBACK TRANSACTION 
 	END 
