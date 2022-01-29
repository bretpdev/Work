--run on UHEAASQLDB
USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXXX

	INSERT INTO CLS.quecomplet.Queues(Queue, SubQueue, TaskControlNumber, AccountIdentifier, TaskStatusId, ActionResponseId, AddedAt, AddedBy)
	SELECT 
		WQXX.WF_QUE,
		WQXX.WF_SUB_QUE,
		WQXX.WN_CTL_TSK,
		PDXX.DF_SPE_ACC_ID,
		X,
		X,
		GETDATE(),
		'CNH XXXXX'
	FROM
		CDW..WQXX_TSK_QUE WQXX
		INNER JOIN CDW..PDXX_PRS_NME PDXX
			ON PDXX.DF_PRS_ID = WQXX.BF_SSN
	WHERE
		WF_QUE = 'XX'
		AND WX_MSG_X_TSK like '%Disaster Administrative forbearance,XXXXX begin date > end date%'
		AND WC_STA_WQUEXX = 'U'

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END