DECLARE @StartDate DATE = 'XXXX-XX-XX'
DECLARE @ArcsToBeRerun TABLE(LogMessage VARCHAR(MAX), Arc CHAR(X), AccountIdentifier VARCHAR(XX), ScriptId VARCHAR(XX), ArcAddId VARCHAR(XX), UserId VARCHAR(X), Region VARCHAR(XX))
INSERT INTO @ArcsToBeRerun(LogMessage, Arc, AccountIdentifier, ScriptId, ArcAddId, UserId, Region)
SELECT
	COALESCE(PLMC.LogMessage,PLMU.LogMessage) AS LogMessage,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('Error adding ', COALESCE(PLMC.LogMessage,PLMU.LogMessage))+XX, X),',','') AS Arc,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('borrower: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+XX, XX),',','')AS Account,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ScriptId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+XX, XX) AS ScriptId,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ArcAddId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+XX, X),',','') AS ArcAddId,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('UserId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+X, X) AS UserId,
	CASE WHEN PLMC.ProcessLogMessageId IS NULL THEN 'UHEAA' ELSE 'CORNERSTONE' END AS Region
FROM 
	ProcessLogs..ProcessLogs PL
	INNER JOIN ProcessLogs..ProcessNotifications PN
		ON PN.ProcessLogId = PL.ProcessLogId
	LEFT OUTER JOIN CLS.[log].ProcessLogMessages PLMC
		ON PLMC.ProcessNotificationId = PN.ProcessNotificationId
	LEFT OUTER JOIN ULS.[log].ProcessLogMessages PLMU
		ON PLMU.ProcessNotificationId = PN.ProcessNotificationId
WHERE
	PL.StartedOn >= @StartDate
	AND PL.ScriptId = 'ARCADDPROC'
	AND COALESCE(PLMC.LogMessage,PLMU.LogMessage) LIKE '%TIMEOUT OR%'
	AND PL.Region = 'CORNERSTONE'

--SELECT DISTINCT UserId, region, Arc FROM @ArcsToBeRerun

SELECT * FROM @ArcsToBeRerun

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

--XXX rows
UPDATE
	AAP
SET
	AAP.ProcessedAt = NULL
--SELECT * 
FROM
	CLS..ArcAddProcessing AAP 
	INNER JOIN @ArcsToBeRerun T
	ON T.ArcAddId = AAP.ArcAddProcessingId
	AND T.Region = 'CORNERSTONE'

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = XXX AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
