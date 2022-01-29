--run on UHEAASQLDB
USE CLS
GO


BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXXX

	INSERT INTO CLS.quecomplet.Queues(Queue, SubQueue, TaskControlNumber, AccountIdentifier, TaskStatusId, ActionResponseId, HadError, AddedAt, AddedBy)
	SELECT
		WF_QUE,
		WF_SUB_QUE,
		WN_CTL_TSK,
		BF_SSN,
		X, --Complete
		X, --COMPL
		X, --HadError
		GETDATE(),
		SUSER_SNAME()
	FROM
		CDW..WQXX_TSK_QUE
	WHERE
		WF_QUE ='XX'
		AND WF_SUB_QUE = 'XX'
		AND CAST(WD_ACT_REQ AS DATE) = 'XXXX-XX-XX'

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