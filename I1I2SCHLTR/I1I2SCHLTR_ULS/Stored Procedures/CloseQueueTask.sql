CREATE PROCEDURE [i1i2schltr].[CloseQueueTask]
	@Queue VARCHAR(2),
	@Subqueue VARCHAR(2),
	@Ssn VARCHAR(9)

AS

DECLARE @CurrentDateTime DATETIME = GETDATE();
DECLARE	@ScriptId VARCHAR(20) = 'I1I2SCHLTR';

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
		DeletedBy,
		WasFound
	)
	SELECT DISTINCT
		@Queue,
		@Subqueue,
		WQ20.WN_CTL_TSK,
		PD10.DF_SPE_ACC_ID,
		2, --'C' (Complete)
		3, --COMPL
		NULL,
		NULL,
		0,
		@CurrentDateTime,
		@ScriptId,
		NULL,
		NULL,
		NULL
	FROM
		UDW..PD10_PRS_NME PD10
		INNER JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = PD10.DF_PRS_ID
		LEFT JOIN ULS.quecomplet.Queues Q
			ON Q.[Queue] = @Queue
			AND Q.SubQueue = @Subqueue
			AND Q.TaskControlNumber = WQ20.WN_CTL_TSK 
			AND Q.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND DeletedAt IS NULL
			AND DeletedBy IS NULL
	WHERE
		WQ20.WF_QUE = @Queue
		AND WQ20.WF_SUB_QUE = @SubQueue
		AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')
		AND WQ20.PF_REQ_ACT IN ('SCLHS','S4SCL')
		AND PD10.DF_PRS_ID = @Ssn
		AND Q.AccountIdentifier IS NULL
RETURN 0
