DECLARE @StartDate DATE = '2017-01-17'
DECLARE @ArcsToBeRerun TABLE(LogMessage VARCHAR(MAX), Arc CHAR(5), AccountIdentifier VARCHAR(10), ScriptId VARCHAR(10), ArcAddId VARCHAR(10), UserId VARCHAR(8), Region VARCHAR(15))
INSERT INTO @ArcsToBeRerun(LogMessage, Arc, AccountIdentifier, ScriptId, ArcAddId, UserId, Region)
SELECT
	COALESCE(PLMC.LogMessage,PLMU.LogMessage) AS LogMessage,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), 60, 5) AS Arc,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('borrower: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+10, 10),',','')AS Account,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ScriptId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+10, 10) AS ScriptId,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ArcAddId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+10, 8),',','') AS ArcAddId,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('UserId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+8, 7) AS UserId,
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
	AND COALESCE(PLMC.LogMessage,PLMU.LogMessage) LIKE '%User has no authorization%'

SELECT DISTINCT UserId, region, Arc FROM @ArcsToBeRerun

SELECT * FROM @ArcsToBeRerun

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

--0 rows
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

--1232 rows
UPDATE
	AAP
SET
	AAP.ProcessedAt = NULL
--SELECT *
FROM
	ULS..ArcAddProcessing AAP 
	INNER JOIN @ArcsToBeRerun T
	ON T.ArcAddId = AAP.ArcAddProcessingId
	AND T.Region = 'UHEAA'

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = 1232 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
