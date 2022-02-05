--Creates a query that identifies accounts with ICRAL, IBALN, and RPYNL ARCs from the 2A/01 queue daily. Adds the G7IBR ARC to these accounts. Leaves a comment saying an IDR application is being sent. Writes the data to ARC Add Processing and Queue Completer to be worked by those processes.
DECLARE @CurrentDateTime DATETIME = GETDATE();
DECLARE	@ScriptId VARCHAR(20) = 'IDREXITSK';
DECLARE @ARC VARCHAR(5) = 'G7IBR';
DECLARE @Comment VARCHAR(50) = 'an IDR application is being sent';

DROP TABLE IF EXISTS #2A01_QUEUE;

--insert task data for desired population into a temp table which will be used twice to insert records into arcaddproesssing and queuecompleter
SELECT DISTINCT
	PD10.DF_SPE_ACC_ID,
	WQ20.WN_CTL_TSK
INTO 
	#2A01_QUEUE
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..WQ20_TSK_QUE WQ20
		ON PD10.DF_PRS_ID = WQ20.BF_SSN
WHERE
	WQ20.WF_QUE = '2A'
	AND WQ20.WF_SUB_QUE = '01'
	AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')
	AND WQ20.PF_REQ_ACT IN ('ICRAL','IBALN','RPYNL')
	AND WQ20.LN_ATY_SEQ IS NULL


BEGIN TRY
	BEGIN TRANSACTION
	
	--ARC ADD PROCESSING - insert records to add ARCs to accounts
	INSERT INTO CLS..ArcAddProcessing
	(
		ArcTypeId,
		ArcResponseCodeId,
		AccountNumber,
		RecipientId,
		ARC,
		ScriptId,
		ProcessOn,
		Comment,
		IsReference,
		IsEndorser,
		ProcessFrom,
		ProcessTo,
		NeededBy,
		RegardsTo,
		RegardsCode,
		CreatedAt,
		CreatedBy,
		ProcessedAt
	)
	SELECT
		1 AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		NewData.DF_SPE_ACC_ID,
		NULL AS RecipientId,
		@ARC AS ARC,
		@ScriptId AS ScriptId,
		@CurrentDateTime AS ProcessOn,
		@Comment AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		NULL AS ProcessFrom,
		NULL AS ProcessTo,
		NULL AS NeededBy,
		NULL AS RegardsTo,
		NULL AS RegardsCode,
		@CurrentDateTime AS CreatedAt,
		@ScriptId AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		#2A01_QUEUE NewData
	--exclude borrowers who already have an arcadd record
		LEFT JOIN CLS..ArcAddProcessing ExistingData
			ON NewData.DF_SPE_ACC_ID = ExistingData.AccountNumber
			AND ExistingData.ARC = @ARC
			AND (
					(
						CAST(ExistingData.CreatedAt AS DATE) = CAST(@CurrentDateTime AS DATE) --to remove anyone added today to prevent duplicates in recovery
						AND ExistingData.Comment = @Comment
					)
					OR ExistingData.ProcessedAt IS NULL
				)
	WHERE
		ExistingData.AccountNumber IS NULL --exclude borrowers who already have an arcadd record
	;

	--QUEUE COMPLETER - add record to queue completer
	INSERT INTO CLS.quecomplet.Queues
	(
		[Queue],
		SubQueue,
		TaskControlNumber,
		AccountIdentifier,
		TaskStatusId,
		ActionResponseId,
		PickedUpForProcessing,
		ProcessedAt,
		HadError,
		AddedAt,
		AddedBy,
		DeletedAt,
		DeletedBy,
		WasFound
	)
	SELECT
		'2A' AS [Queue],
		'01' AS SubQueue,
		WN_CTL_TSK AS TaskControlNumber,
		DF_SPE_ACC_ID AS AccountIdentifier,
		2 AS TaskStatusId, --'C' (complete)
		9 ActionResponseId, -- blank
		NULL AS PickedUpForProcessing,
		NULL AS ProcessedAt,
		0 AS HadError,
		@CurrentDateTime AS AddedAt,
		@ScriptId AS AddedBy,
		NULL AS DeletedAt,
		NULL AS DeletedBy,
		NULL AS WasFound
	FROM
		#2A01_QUEUE NewData
		--exclude tasks already in the queue completer table
		LEFT JOIN CLS.quecomplet.Queues ExistingData
			ON NewData.WN_CTL_TSK = ExistingData.TaskControlNumber
			AND NewData.DF_SPE_ACC_ID = ExistingData.AccountIdentifier
			AND ExistingData.[Queue] = '2A'
			AND ExistingData.SubQueue = '01'
			AND ExistingData.DeletedAt IS NULL
			AND (	
					CAST(ExistingData.AddedAt AS DATE) = CAST(@CurrentDateTime AS DATE) --to remove any tasks added today to prevent duplicates in recovery
					OR ExistingData.ProcessedAt IS NULL --remove tasks which already have and unprocessed record since they don't need another one
				)
	WHERE
			ExistingData.AccountIdentifier IS NULL

	COMMIT TRANSACTION;

END TRY

--write message to process logger if an error occurs
BEGIN CATCH
	DECLARE @EM_ VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId_ INT;
	DECLARE @ProcessNotificationId_ INT;
	DECLARE @NotificationTypeId_ INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId_ INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;

