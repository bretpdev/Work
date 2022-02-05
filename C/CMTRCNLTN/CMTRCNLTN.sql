
BEGIN TRY

SET CONCAT_NULL_YIELDS_NULL OFF
DECLARE @ScriptId VARCHAR(10) = 'CMTRCNLTN';
DECLARE @StartTime DATETIME = GETDATE();

-- UHEAA
PRINT 'Mark UHEAA call histories as deleted where the borrower does not have a released loan'
UPDATE
	NCH
SET
	NCH.Deleted = 1,
	NCH.DeletedAt = GETDATE(),
	NCH.DeletedBy = 'Comment Reconciliation (no released loans)'
--SELECT DISTINCT *
FROM
	NobleCalls..NobleCallHistory NCH
	INNER JOIN 
	(
		SELECT DISTINCT
			NCH.NobleCallHistoryId,
			COALESCE(BSSN.DF_PRS_ID, BACT.DF_PRS_ID) [SSN]
		FROM
			NobleCalls..NobleCallHistory NCH
			LEFT JOIN UDW..PD10_PRS_NME BSSN 
				ON BSSN.DF_PRS_ID = NCH.AccountIdentifier
			LEFT JOIN UDW..PD10_PRS_NME BACT 
				ON BACT.DF_SPE_ACC_ID = NCH.AccountIdentifier
		WHERE
			NCH.IsReconciled = 0
			AND NCH.IsInbound = 0
			AND NCH.Deleted = 0
			AND NCH.CreatedAt > '2016-11-1'
			AND NCH.RegionId = 2 -- CompassUheaa (aka campaigns that start with LPP)
	) NCHB 
		ON NCHB.NobleCallHistoryId = NCH.NobleCallHistoryId
	LEFT JOIN UDW..LN10_LON LN10 
		ON LN10.BF_SSN = NCHB.SSN 
		AND LN10.LC_STA_LON10 = 'R'
WHERE
	LN10.BF_SSN IS NULL --Borrower doesn't have a released loan

PRINT 'UHEAA - Attempt to reconcile locally'
UPDATE
	NCH
SET
	IsReconciled = 1,
	ReconciledAt = GETDATE()
FROM
	NobleCalls..NobleCallHistory NCH
	INNER JOIN
	(
		SELECT
			AY20.LX_ATY,
			SUBSTRING(AY20.LX_ATY, CHARINDEX('NOBLECALLHISTORYID:', AY20.LX_ATY) + 19, (CHARINDEX('; AGENT:', AY20.LX_ATY) - (CHARINDEX('NOBLECALLHISTORYID:', AY20.LX_ATY) + 19))) [NobleCallHistoryId]
		FROM
			UDW..AY10_BR_LON_ATY AY10
			INNER JOIN UDW..AY15_ATY_CMT AY15
				ON AY15.BF_SSN = AY10.BF_SSN
				AND AY15.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				AND AY15.LC_STA_AY15 = 'A'
			INNER JOIN UDW..AY20_ATY_TXT AY20
				ON AY20.BF_SSN = AY15.BF_SSN
				AND AY20.LN_ATY_SEQ = AY15.LN_ATY_SEQ
				AND AY20.LN_ATY_CMT_SEQ = AY15.LN_ATY_CMT_SEQ
		WHERE
			AY10.LD_ATY_REQ_RCV BETWEEN CAST(DATEADD(DAY, -4, @StartTime) AS DATE) AND CAST(@StartTime AS DATE)
			AND AY20.LX_ATY LIKE 'NOBLECALLHISTORYID:%; AGENT:%'
			AND AY10.LC_STA_ACTY10 = 'A'
	) AY10 
		ON AY10.NobleCallHistoryId = NCH.NobleCallHistoryId
WHERE
	NCH.RegionId = 2 --CompassUheaa (aka campaigns that start with LPP)
	AND NCH.CreatedAt BETWEEN DATEADD(DAY, -4, @StartTime) AND DATEADD(HOUR, -1, @StartTime)
	AND	NCH.DeletedAt IS NULL
	AND	NCH.Deleted = 0
	AND
	(
		NCH.ReconciledAt IS NULL
		OR NCH.IsReconciled = 0
	)

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage VARCHAR(4000) = 'Transaction not committed. The ' + @ScriptId + ' encountered an error. Error: ' + (SELECT ERROR_MESSAGE());
	PRINT @ErrorMessage
	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@StartTime,GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId, NotificationSeverityTypeId, ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId, @NotificationSeverityTypeId, @ProcessLogId, NULL, NULL);
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId, @ErrorMessage);

	THROW;
END CATCH;

