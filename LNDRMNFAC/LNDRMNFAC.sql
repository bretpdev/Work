--LNDRMNFAC
USE UDW;

DECLARE @ScriptId VARCHAR(100) = 'LNDRMNFAC'
DECLARE @TaskStatusId INT = (SELECT TaskStatusId FROM ULS.quecomplet.TaskStatuses WHERE TaskStatus = 'X');
DECLARE @ActionResponseId INT = (SELECT ActionResponseId FROM ULS.quecomplet.ActionResponses WHERE ActionResponse = 'CANCL');

BEGIN TRY
	BEGIN TRANSACTION
	
	INSERT INTO ULS.quecomplet.Queues
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
			DeletedBy
		)
	SELECT DISTINCT
		WQ20.WF_QUE AS [Queue],
		WQ20.WF_SUB_QUE AS SubQueue,
		WQ20.WN_CTL_TSK AS TaskControlNumber,
		WQ20.BF_SSN AS AccountIdentifier,
		@TaskStatusId AS TaskStatusId,
		@ActionResponseId AS ActionResponseId,
		NULL AS PickedUpForProcessing,
		NULL AS ProcessedAt,
		0 AS HadError,
		GETDATE() AS AddedAt,
		@ScriptId AS AddedBy,
		NULL AS DeletedAt,
		NULL AS DeletedBy
	FROM
		UDW..WQ20_TSK_QUE WQ20
	--check for existing records
		LEFT JOIN ULS.quecomplet.Queues QUES
			ON WQ20.WF_QUE = QUES.[Queue]
			AND WQ20.WF_SUB_QUE = QUES.SubQueue
			AND WQ20.WN_CTL_TSK  = QUES.TaskControlNumber
			AND WQ20.BF_SSN = QUES.AccountIdentifier
			AND QUES.DeletedAt IS NULL
			AND QUES.ProcessedAt IS NULL
	WHERE
		WQ20.WF_QUE = '4X'
		AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')
		AND
			(
				WQ20.WX_MSG_1_TSK LIKE('749 0801%')-- Invld Dsbmt Dte
				OR WQ20.WX_MSG_1_TSK LIKE('749 0988%')-- More Records
				OR WQ20.WX_MSG_1_TSK LIKE('749 3601%')-- Invalid Date Entered Repay
				OR WQ20.WX_MSG_1_TSK LIKE('749 2401%')-- Invalid Refund Amt
				OR WQ20.WX_MSG_1_TSK LIKE('749 3001%') -- Invalid Date of Last Disb
				OR WQ20.WX_MSG_1_TSK LIKE('749 3101%')-- invalid Disb Amt
				OR WQ20.WX_MSG_1_TSK LIKE('749 3201%') -- Invalid Cancellation Date
				OR WQ20.WX_MSG_1_TSK LIKE('749 3301%')-- Invalid Cancellation Amt
				OR WQ20.WX_MSG_1_TSK LIKE('749 3302%')-- Cancellation Amt Differs From
			)
		AND QUES.AccountIdentifier IS NULL --don't add duplicates

	COMMIT TRANSACTION;

END TRY
	--write message to process logger if an error occurs
	BEGIN CATCH
		DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

		ROLLBACK TRANSACTION;

		DECLARE @ProcessLogId INT;
		DECLARE @ProcessNotificationId INT;
		DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
		DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
		INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
		SET @ProcessLogId = SCOPE_IDENTITY()

		INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
		SET @ProcessNotificationId = SCOPE_IDENTITY()

		INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;
