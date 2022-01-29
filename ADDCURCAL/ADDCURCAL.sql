USE UDW
GO

DECLARE @Now DATETIME = GETDATE()
DECLARE @User VARCHAR(100) = SUSER_NAME()
DECLARE @ARC VARCHAR(5) = 'CUINC'
DECLARE @ScriptId VARCHAR(7) = 'ADDCURCAL'
DECLARE @Comment VARCHAR(100) = ''
DECLARE @ActionResponseId INT = (SELECT ActionResponseId FROM ULS.quecomplet.ActionResponses WHERE ActionResponse = 'COMPL')
DECLARE @TaskStatusId INT = (SELECT TaskStatusId FROM ULS.quecomplet.TaskStatuses WHERE TaskStatus = 'C')
DECLARE @Queues TABLE
(
	[Queue] VARCHAR(2),
	SubQueue VARCHAR(2),
	TaskControlNumber VARCHAR(20),
	AccountIdentifier VARCHAR(10),
	TaskStatusId INT,
	ActionResponseId INT,
	AddedBy VARCHAR(100),
	WD_ACT_REQ DATE,
	LN_SEQ SMALLINT
)

DECLARE @ArcAccount TABLE
(
	ArcAddProcessingId BIGINT,
	AccountNumber VARCHAR(10)
)

BEGIN TRY
BEGIN TRANSACTION

INSERT INTO @Queues([Queue], SubQueue, TaskControlNumber, AccountIdentifier, TaskStatusId, ActionResponseId, AddedBy, WD_ACT_REQ, LN_SEQ)
SELECT
	WQ20.WF_QUE,
	WQ20.WF_SUB_QUE,
	WQ20.WN_CTL_TSK,
	PD10.DF_SPE_ACC_ID,
	@TaskStatusId,
	@ActionResponseId,
	@User,
	WQ20.WD_ACT_REQ,
	LN10.LN_SEQ
FROM
	UDW..WQ20_TSK_QUE WQ20
	INNER JOIN PD10_PRS_NME PD10
		ON WQ20.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN
	(
		SELECT
			LN10.BF_SSN,
			MIN(LN10.LN_SEQ) AS LN_SEQ
		FROM
			LN10_LON LN10
		WHERE
			LN10.LC_STA_LON10 = 'R'
		GROUP BY
			LN10.BF_SSN
	) LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	LEFT JOIN UDW..WQ20_TSK_QUE WQ20_Arc_Added
		ON PD10.DF_PRS_ID = WQ20_Arc_Added.BF_SSN
		AND WQ20_Arc_Added.WF_QUE = 'CU'
		AND WQ20_Arc_Added.WC_STA_WQUE20 != 'C'
		AND WQ20_Arc_Added.WC_STA_WQUE20 != 'X'
WHERE
	WQ20.WF_QUE = 'GV'
	AND WQ20.WF_SUB_QUE = 'IV'
	--We use U because we don't want to deal with assigned or worked queues since the queue will need to be re-assigned to be closed
	AND WQ20.WC_STA_WQUE20 IN ('U')
	AND WQ20_Arc_Added.BF_SSN IS NULL

--Insert the queues into quecomplet
--INSERT INTO ULS.quecomplet.Queues([Queue], Subqueue, TaskControlNumber, AccountIdentifier, TaskStatusId, ActionResponseId, AddedBy)
--SELECT
--	NewQ.[Queue],
--	NewQ.SubQueue,
--	NewQ.TaskControlNumber,
--	NewQ.AccountIdentifier,
--	NewQ.TaskStatusId,
--	NewQ.ActionResponseId,
--	NewQ.AddedBy
--FROM
--	@Queues NewQ
--	LEFT JOIN ULS.quecomplet.Queues ExistQ
--		ON NewQ.AccountIdentifier = ExistQ.AccountIdentifier
--		AND NewQ.[Queue] = ExistQ.[Queue]
--		AND NewQ.SubQueue = ExistQ.SubQueue
--		AND NewQ.TaskControlNumber = ExistQ.TaskControlNumber
--WHERE
--	ExistQ.AccountIdentifier IS NULL

--Inserts queues that are active into arc add with the CUINC arc
INSERT INTO 
	ULS..ArcAddProcessing(ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessingAttempts, CreatedAt, CreatedBy, ProcessedAt)
OUTPUT
	INSERTED.ArcAddProcessingId,
	INSERTED.AccountNumber
INTO
	@ArcAccount(ArcAddProcessingId, AccountNumber)
SELECT DISTINCT
	0, --ArcTypeId, By Loan
	Q.AccountIdentifier, --AccountNumber
	@ARC, --ARC
	@ScriptId, --ScriptId
	@Now, --ProcessOn
	@Comment, --Comment
	0, --IsReference
	0, --IsEndorser
	0, --ProcessingAttempts
	@Now, --CreatedAt
	@User, --CreatedBy
	NULL --ProcessedAt
FROM
	@Queues Q
	--Duplication exclusion
	--Only adds new arcs when and arc after the WQ20 action request date does not exist
	LEFT JOIN ULS..ArcAddProcessing AAP
		ON Q.AccountIdentifier = AAP.AccountNumber
		AND AAP.ARC = @ARC
		AND AAP.ScriptId = @ScriptId
		AND AAP.ProcessingAttempts <= 1
		AND CAST(AAP.CreatedAt AS DATE) >= Q.WD_ACT_REQ
WHERE
	AAP.AccountNumber IS NULL

--Because we use the output, the duplication exclusion on AAP is not bypassed
INSERT INTO	
	ULS..ArcLoanSequenceSelection(ArcAddProcessingId,LoanSequence)
SELECT
	Acc.ArcAddProcessingId,
	Q.LN_SEQ
FROM
	@Queues Q
	INNER JOIN @ArcAccount Acc
		ON Q.AccountIdentifier = Acc.AccountNumber

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@Now,@Now,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;