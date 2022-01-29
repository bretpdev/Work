USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	INSERT INTO CLS.quecomplet.Queues(Queue,SubQueue,TaskControlNumber,AccountIdentifier,TaskStatusId,ActionResponseId,PickedUpForProcessing,ProcessedAt,HadError,AddedAt,AddedBy,DeletedAt,DeletedBy)
	SELECT
		WQXX.WF_QUE,
		WQXX.WF_SUB_QUE,
		WQXX.WN_CTL_TSK,
		PDXX.DF_SPE_ACC_ID,
		X, --Cancel
		X, --Cancel
		NULL,
		NULL,
		X,
		GETDATE(),
		'CNH XXXXX',
		NULL,
		NULL
	FROM
		CDW..FBXX_BR_FOR_REQ FBXX
		INNER JOIN CDW..PDXX_PRS_NME PDXX
			ON PDXX.DF_PRS_ID = FBXX.BF_SSN
		INNER JOIN CDW..WQXX_TSK_QUE WQXX
			ON WQXX.BF_SSN = FBXX.BF_SSN
			AND WQXX.WF_QUE = 'VB'
			AND WQXX.WF_SUB_QUE = 'FB'
			AND WQXX.WC_STA_WQUEXX NOT IN('X','C')
	WHERE
		PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')
		AND FBXX.LC_FOR_TYP = 'XX'
		AND 
		(	
			FBXX.LD_FOR_REQ_BEG = 'XXXX-XX-XX'
			OR FBXX.LD_FOR_BR_REQ_END = 'XXXX-XX-XX'
		)


	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

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
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END