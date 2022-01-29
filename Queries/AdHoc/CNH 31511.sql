DECLARE @StartDate DATE = 'XXXX-XX-XX'
DECLARE @ArcsToBeRerun TABLE(LogMessage VARCHAR(MAX), Arc CHAR(X), AccountIdentifier VARCHAR(XX), ScriptId VARCHAR(XX), ArcAddId VARCHAR(XX), UserId VARCHAR(X), Region VARCHAR(XX))
INSERT INTO @ArcsToBeRerun(LogMessage, Arc, AccountIdentifier, ScriptId, ArcAddId, UserId, Region)
SELECT
	COALESCE(PLMC.LogMessage,PLMU.LogMessage) AS LogMessage,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), XX, X) AS Arc,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('borrower: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+XX, XX),',','')AS Account,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ScriptId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+XX, XX) AS ScriptId,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ArcAddId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+XX, X),',','') AS ArcAddId,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('UserId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+X, X) AS UserId,
	CASE WHEN PLMC.ProcessLogMessageId IS NULL THEN 'UHEAA' ELSE 'CORNERSTONE' END AS Region
FROM 
	ProcessLogs..ProcessLogs PL
	LEFT JOIN ProcessLogs..ProcessNotifications PN
		ON PN.ProcessLogId = PL.ProcessLogId
	LEFT OUTER JOIN CLS.[log].ProcessLogMessages PLMC
		ON PLMC.ProcessNotificationId = PN.ProcessNotificationId
	LEFT OUTER JOIN ULS.[log].ProcessLogMessages PLMU
		ON PLMU.ProcessNotificationId = PN.ProcessNotificationId
WHERE
	PL.StartedOn >= @StartDate
	AND PL.ScriptId = 'ARCADDPROC'
	AND COALESCE(PLMC.LogMessage,PLMU.LogMessage) LIKE '%User has no authorization%'
--SELECT * FROM @ArcsToBeRerun
--SELECT DISTINCT UserId, region, AR.Arc FROM @ArcsToBeRerun AR JOIN NobleCalls..Arcs A ON AR.Arc = A.Arc




DECLARE @Accounts TABLE(NobleCallHistoryId INT, AccountIdentifier VARCHAR(XX), ArcAddProcessingId INT)
DECLARE @region varchar(XX) = 'CompassCornerStone'
INSERT INTO @Accounts(NobleCallHistoryId, AccountIdentifier, ArcAddProcessingId)
SELECT
	NCH.NobleCallHistoryId,
	NCH.AccountIdentifier,
	NCH.ArcAddProcessingId
FROM
	NobleCalls.dbo.NobleCallHistory NCH
	LEFT JOIN NobleCalls.dbo.CallCampaigns CC ON CC.CallCampaign = NCH.CallCampaign
	LEFT JOIN NobleCalls.dbo.DispositionCodeMapping DCM on DCM.DispositionCode = NCH.DispositionCode
	LEFT JOIN NobleCalls.dbo.Comments C on C.CommentId = DCM.CommentId
	LEFT JOIN NobleCalls.dbo.Arcs A on A.ArcId = DCM.ArcId
	LEFT JOIN NobleCalls.dbo.ResponseCodes RC on RC.ResponseCodeId = DCM.ResponseCodeId
	LEFT JOIN NobleCalls.dbo.Regions R ON R.RegionId = CC.RegionId
WHERE
	ISNULL(R.region, 'UnknownRegion') = ISNULL(@region, 'UnknownRegion')
	AND
	CAST(NCH.ActivityDate as DATE) > 'X/X/XX' AND CAST(NCH.ActivityDate AS DATE) < 'X/XX/XX'
	AND
	NCH.Deleted = X
	AND
	NCH.CallType != X -- manual dial; no connect
	AND
	NCH.AccountIdentifier != 'null'
	AND
	CC.CallCampaign NOT IN ('VABC', 'VABU')
	AND
	NCH.IsInbound = X
	AND
	NCH.ReconciledAt IS NULL
ORDER BY
	NCH.ReconciledAt
--SELECT * FROM @Accounts

DECLARE @ArcToUpdate TABLE(ArcAddProcessingId INT)
INSERT INTO @ArcToUpdate(ArcAddProcessingId)
SELECT ArcAddProcessingId FROM @Accounts A JOIN @ArcsToBeRerun AR ON AR.ArcAddId = A.ArcAddProcessingId

--SELECT * FROM @ArcToUpdate

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

--X rows
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
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END